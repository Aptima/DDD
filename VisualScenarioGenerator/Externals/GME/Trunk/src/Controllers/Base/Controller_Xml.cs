/*
 * Class            : Controller
 * File             : Controller_Public.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      :
 * Contains the private and protected methods to validate adding new component,
 * connecting existing components by XML schema. It returns the component, its child
 * components, and parameters in XML format.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using AME.Model;
using System.Xml.XPath;
using AME.Controllers.Base.DataStructures;

#endregion  //Namespaces

namespace AME.Controllers
{
    public partial class Controller
    {

        #region Component Add Validation

        protected XmlElement _GetXmlTree_Validation(XmlDocument doc, int compID, string linkType, DataTable componentTable)
        {
            // check config file, see if we need to use deep copy
            // the default is to use the cache
            // if deep copy is enabled for a link type, be careful, the schema should prevent any cycles 
            // on that link type - the controller will follow them!

            /*
             * 	<Global>
		            <Configurations>
			            <Configuration name="VSG">
				            <LinkTypes>
					            <Link name="Event ID" type="EventID" description="">
             * 
             */
            //IXPathNavigable configurationXML = m_model.GetConfiguration(this.Configuration);

            bool useCache = true;

            IXPathNavigable linkNav = m_model.GetLinks();
            if (linkNav != null)
            {
                XPathNavigator forLinkType = linkNav.CreateNavigator().SelectSingleNode(String.Format(
                    "Link[@{0}='{1}']", ConfigFileConstants.Type, linkType));

                if (forLinkType != null)
                {
                    String deepCopy = forLinkType.GetAttribute(ConfigFileConstants.deepCopy, forLinkType.NamespaceURI);

                    if (deepCopy != null && deepCopy.Length > 0)
                    {
                        bool useDeepCopy = bool.Parse(deepCopy);

                        if (useDeepCopy)
                        {
                            useCache = false;
                        }
                    }
                }
            }

            Dictionary<String, DataRow> componentTableCache;
            Dictionary<String, List<DataRow>> linkTableCache;
            GetComponentAndLinkTableCache(out componentTableCache, out linkTableCache, componentTable, linkType);

            DataRow component = null;
            String key = ""+compID;

            if (componentTableCache != null)
            {
                if (componentTableCache.ContainsKey(key))
                {
                    component = componentTableCache[key];
                }
            }
            else
            {
                component = this._GetComponent(compID);
            }

            return this._GetXmlTree_Validation(
                doc,
                component,
                linkType,
                useCache,
                new Dictionary<int, XmlElement>(),
                new Dictionary<String, String>(),
                componentTable,
                componentTableCache,
                linkTableCache
                );
        }//_GetXmlTree_Validation

        protected XmlElement _GetXmlTree_Validation(XmlDocument doc, DataRow component, string linkType, bool useCache,
            Dictionary<int, XmlElement> componentCache, Dictionary<String, String> typeToBaseCache, 
                DataTable componentTable, Dictionary<String, DataRow> componentTableCache, 
                    Dictionary<String, List<DataRow>> linkTableCache)
        {
            if (component == null) { return null; }

            string sType = (string)component[SchemaConstants.Type];
            int iCompID = (int)component[SchemaConstants.Id];
            string sName = (string)component[SchemaConstants.Name];
            Component.eComponentType etype = this._GetComponentType(component);

            //if component is already in cached collection, clone existing one instead of 
            //going over DB interaction.
            if (useCache)
            {
                if (componentCache.ContainsKey(iCompID))
                {
                    return (XmlElement)componentCache[iCompID].Clone();
                }
            }

            //creating element
            string sBaseComponentType;
            if (typeToBaseCache != null && typeToBaseCache.ContainsKey(sType))
            {
                sBaseComponentType = typeToBaseCache[sType];
            }
            else
            {
                sBaseComponentType = this._GetUltimateBaseComponentType(sType);
                if (typeToBaseCache != null)
                {
                    typeToBaseCache.Add(sType, sBaseComponentType);
                }
            }

            XmlElement xmlElement = doc.CreateElement(sBaseComponentType);

            //creating attributes
            XmlAttribute attributeType = doc.CreateAttribute(XmlSchemaConstants.Config.eType);
            attributeType.Value = etype.ToString();

            XmlAttribute attributeID = doc.CreateAttribute(XmlSchemaConstants.Config.Id);
            attributeID.Value = iCompID.ToString();

            XmlAttribute attributeName = doc.CreateAttribute(XmlSchemaConstants.Config.Name);
            attributeName.Value = sName;

            //adding attributes
            xmlElement.SetAttributeNode(attributeType);
            xmlElement.SetAttributeNode(attributeID);
            xmlElement.SetAttributeNode(attributeName);

            //adding a copy of component in cached data
            if (useCache)
            {
                componentCache.Add(iCompID, (XmlElement)xmlElement.Clone());
            }

            //if linktype then work on children. No linktype means no children.
            if (linkType != null)
            {
                //getting children
                DataTable childComponents = null;
                if (componentTable != null && componentTableCache != null && linkTableCache != null)
                {
                    childComponents = GetChildComponentsByCache(iCompID, linkType, componentTable, componentTableCache, linkTableCache);
                }
                else
                {
                    childComponents = this.m_model.GetChildComponents(iCompID, linkType);  // PROBLEM?
                }

                foreach (DataRow childComponent in childComponents.Rows)
                {
                    //getting xml element for the child component
                    XmlElement childElement = this._GetXmlTree_Validation(doc, childComponent, linkType, useCache, componentCache,
                        typeToBaseCache, componentTable, componentTableCache, linkTableCache);

                    //adding child element to parent
                    xmlElement.AppendChild(childElement);
                }//foreach child component

            }//if linktype not null

            return xmlElement;
            
        }//_GetXmlTree_Validation

        private XmlDocument _GetXmlDoc_Validation(int compID, string linkType)
        {
            return _GetXmlDoc_Validation(compID, linkType, GetComponentTable());
        }

        private XmlDocument _GetXmlDoc_Validation(int compID, string linkType, DataTable componentTable)
        {
            String linkTypeForSchema = this._GetLinkTypeForSchema(linkType);
            string schemaPath = this._GetValidationSchemaPath(linkTypeForSchema);
            XmlElement configElement;
            XmlDocument doc = this._GetXmlDoc_Validation(out configElement, schemaPath);

            //creating attribute
            XmlAttribute attributeLinkType = doc.CreateAttribute(XmlSchemaConstants.Config.LinkType);
            attributeLinkType.Value = linkTypeForSchema;
            //adding attribute
            configElement.SetAttributeNode(attributeLinkType);

            XmlElement xmlElement = this._GetXmlTree_Validation(doc, compID, linkType, componentTable);
            if (xmlElement != null)
            {
                configElement.AppendChild(xmlElement);
            }

            return doc;
        }//_GetXmlDoc_Validation

        protected XmlDocument _GetXmlDoc_Validation(out XmlElement configElement, string schemaFilePath)
        {
            bool bValidSchemaPath = false;
            bValidSchemaPath = Path.HasExtension(schemaFilePath);
            
            if (bValidSchemaPath)
            {
                using (XmlReader schemaXSD = this.GetXSD(schemaFilePath))
                {
                    if (schemaXSD == null)
                    {
                        throw new System.IO.FileNotFoundException("File does not exist.", schemaFilePath);
                    }
                }
                
            }

            if (!bValidSchemaPath)
            {
                throw new System.ArgumentException("Invalid schema path.");
            }

            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create root element using 'configuration' variable.
            //configElement = doc.CreateElement(this.Configuration);
            configElement = doc.CreateElement(Root);

            // Add schema information to root.
            XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schema.Value = schemaFilePath;
            configElement.SetAttributeNode(schema);

            doc.AppendChild(configElement);

            return doc;
        }//_GetXmlDoc_Validation

        private XmlReaderSettings _GetXmlReaderSettings_Validation
        {
            get
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new ValidationEventHandler(_validationEventHandler);

                return settings;
            }//get
        }//_GetXmlReaderSettings_Validation

        private void _Validate(XmlDocument doc) {
            try {
                _Validate(doc, "");
            }
            catch (Exception) {
                throw;
            }
        }
        private void _Validate(XmlDocument validationDoc, String componentType)
        {
            try {
                string sSchemaPath = this._GetSchemaPath(validationDoc);
                if (sSchemaPath != null) {
                    //Create settings
                    XmlReaderSettings settings = new XmlReaderSettings();
                    XmlReader reader = GetXSD(sSchemaPath);
                    settings.Schemas.Add(null, reader);
                    settings.ValidationType = ValidationType.Schema;

                    //Create reader with settings
                    StringReader strReader = new StringReader(validationDoc.OuterXml);
                    XmlReader xmlReader = XmlReader.Create(strReader, settings);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlReader);

                    reader.Close();

                    if (componentType != "") {
                        string sXpathVisibleFuncs =
                    string.Format(lastValidateAddComponentXPath + "[@{0}]",
                        XmlSchemaConstants.Config.VisibleFunctions);
                        string sXpathInvisibleFuncs =
                    string.Format(lastValidateAddComponentXPath + "[@{0}]",
                        XmlSchemaConstants.Config.InvisibleFunctions);

                        lastSchemaValuesValidateAdd = new List<ComponentFunction>();

                        XmlNodeList nodeListVisibleFuncs = doc.SelectNodes(sXpathVisibleFuncs);
                        XmlNodeList nodeListInvisibleFuncs = doc.SelectNodes(sXpathInvisibleFuncs);

                        if (nodeListVisibleFuncs.Count >= 1) {
                            XmlAttribute visible = nodeListVisibleFuncs[0].Attributes[XmlSchemaConstants.Config.VisibleFunctions];

                            string sVisibleFuncName = visible.Name;
                            string sVisibleFuncValue = visible.Value;

                            List<ComponentFunction> compFuncs = lastValidateSchemaParser.ParseComponentFunction(sVisibleFuncName, sVisibleFuncValue);
                            if (compFuncs != null) {
                                foreach (ComponentFunction compFunc in compFuncs) {
                                    lastSchemaValuesValidateAdd.Add(new ComponentFunction(compFunc.Name, compFunc.Action, true));
                                }
                            }
                        }

                        if (nodeListInvisibleFuncs.Count >= 1) {
                            XmlAttribute invisible = nodeListInvisibleFuncs[0].Attributes[XmlSchemaConstants.Config.InvisibleFunctions];

                            string sInvisibleFuncName = invisible.Name;
                            string sInvisibleFuncValue = invisible.Value;

                            List<ComponentFunction> compFuncs = lastValidateSchemaParser.ParseComponentFunction(sInvisibleFuncName, sInvisibleFuncValue);
                            if (compFuncs != null) {
                                foreach (ComponentFunction compFunc in compFuncs) {
                                    lastSchemaValuesValidateAdd.Add(new ComponentFunction(compFunc.Name, compFunc.Action, false));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) {
                throw;
            }
        }        

        private string _VerifyBeforeValidateAdd_GetParentXPath(
            int topID,
            int parentID
            )
        {

            //Verifying if Top component exists
            if (!this._ComponentExists(topID))
            {
                string sError = string.Format("Root {0} does not exist.", topID);
                throw new System.Exception(sError);
            }

            //Verifying if Parent exists
            DataRow parentComponent = this._GetComponent(parentID);
            if (parentComponent == null)
            {
                string sError = string.Format("Parent {0} does not exist.", parentID);
                throw new System.Exception(sError);
            }

            //Validating parent type. Parent has to be a Component or Instance
            Component.eComponentType eParentType = this._GetComponentType(parentComponent);
            if ((eParentType != Component.eComponentType.Component)
                && (eParentType != Component.eComponentType.Instance)
                    && (eParentType != Component.eComponentType.Subclass)
                        && (eParentType != Component.eComponentType.Class))
            {
                string sError = string.Format("Invalid parent type {0}.", eParentType.ToString());
                throw new System.ArgumentException(sError);
            }

            //getting parent xml node
            string sParentType = (string)parentComponent[SchemaConstants.Type];

            lastValidateAddParentXPath = string.Format("//Component[@{0}='{1}'][@{2}='{3}']",
    XmlSchemaConstants.Display.Component.Type, sParentType, XmlSchemaConstants.Config.Id, parentID); // cache will use this

            sParentType = this._GetUltimateBaseComponentType(sParentType); // replaces type with base type, if it exists.

            lastValidateAddComponentXPath = "//" + sParentType + "[@ID='" + parentID + "']"; // cache will use this, use base types
            
            //from doc getting top's children & children's children matching the condition
            string sParentXPath = string.Format("//{0}[@{1}='{2}']",
                sParentType, XmlSchemaConstants.Config.Id, parentID);

            return sParentXPath;
        }//_VerifyBeforeValidateAdd_GetParentXPath

        private void _ValidateAdd(
            int topID,
            int parentID, 
            string componentType, 
            Component.eComponentType eComponentType, 
            string name, 
            string linkType)
        {
            string sParentXPath = this._VerifyBeforeValidateAdd_GetParentXPath(topID, parentID);

            XmlDocument doc = this._GetXmlDoc_Validation(topID, linkType);

            this._ValidateAdd(doc, sParentXPath, componentType, eComponentType, name);
        }//_ValidateAdd

        protected void _ValidateAdd(
            XmlDocument doc, 
            string sParentXPath, 
            string componentType,
            Component.eComponentType eComponentType,
            string name)
        {
            try {
                lastValidateAddComponentType = componentType;

                //creating child xml element
                String baseType = this._GetBaseComponentType(componentType);
                lastValidateAddBaseType = baseType;

                if (baseType != null) // replace with base for validation
            {
                    componentType = baseType;
                }

                XmlElement childXml = doc.CreateElement(componentType);
                //creating attributes
                XmlAttribute attributeType = doc.CreateAttribute(XmlSchemaConstants.Config.eType);
                attributeType.Value = eComponentType.ToString();
                XmlAttribute attributeName = doc.CreateAttribute(XmlSchemaConstants.Config.Name);
                attributeName.Value = name;
                //adding attributes
                childXml.SetAttributeNode(attributeType);
                childXml.SetAttributeNode(attributeName);

                // use 'base' of component type to find in validated document
                lastValidateAddComponentXPath = lastValidateAddComponentXPath + "/" + componentType; // cache will use this

                doc = AddChildAtXPath(doc, sParentXPath, childXml, true);

                this._Validate(doc, componentType); //Validating the XmlDocument
            }
            catch (Exception) {
                throw;
            }
        }//_ValidateAdd


        public XmlDocument AddChildAtXPath(XmlDocument doc, string sParentXPath, XmlElement childXML, bool addAtDeepestMatch)
        {
            XmlNode parentXml = null;
            int maxDepth = 0;
            int minDepth = 100000;

            XmlNodeList nodeList = doc.SelectNodes(sParentXPath);

            // out of the matching nodes to the xpath, pick the node
            // that has the greatest depth and use it as the parent.
            // this will ensure the XML that will be validated matches
            // XML that gets created after this data is entered into the database
            // Using the first matching xpath can allow some things to get by this
            // validation but will fail on future validation.
            if (nodeList.Count > 0)
            {
                foreach (XmlNode testNode in nodeList)
                {
                    int nodeDepth = 0;
                    XmlNode depthNullTest = testNode.ParentNode;
                    while (depthNullTest != null)
                    {
                        depthNullTest = depthNullTest.ParentNode;
                        nodeDepth++;
                    }

                    if (addAtDeepestMatch)
                    {
                        if (nodeDepth >= maxDepth)
                        {
                            parentXml = testNode;
                            maxDepth = nodeDepth;
                        }
                    }
                    else
                    {
                        if (nodeDepth <= minDepth)
                        {
                            parentXml = testNode;
                            minDepth = nodeDepth;
                        }
                    }
                }
            }
            else
            {
                throw new System.Exception("ParentID does not exist");
            }

            //adding child xml to parent xml node
            parentXml.AppendChild(childXML);

            return doc;
        }

        private void _ValidateRename(
            int topID,
            int compID,
            string linkType,
            string name)
        {
            //Verifying if Top component exists
            if (!this._ComponentExists(topID))
            {
                string sError = string.Format("Top {0} does not exist.", topID);
                throw new System.Exception(sError);
            }

            XmlDocument doc = this._GetXmlDoc_Validation(topID, linkType);

            this._ValidateRename(doc, compID, name);
        }//_ValidateRename

        protected void _ValidateRename(
            XmlDocument doc,
            int compID,
            string name)
        {

            //Verifying if Component exists
            DataRow component = this._GetComponent(compID);
            if (component == null)
            {
                string sError = string.Format("Parent {0} does not exist.", compID);
                throw new System.Exception(sError);
            }

            //getting component xml node
            string sComponentType = (string)component[SchemaConstants.Type];
            //from doc getting top's children & children's children matching the condition
            string sComponentXPath = string.Format("//{0}[@{1}='{2}']",
                this._GetUltimateBaseComponentType(sComponentType), 
                XmlSchemaConstants.Config.Id, compID);

            XmlNode componentXml = doc.SelectSingleNode(sComponentXPath);
            componentXml.Attributes[XmlSchemaConstants.Config.Name].Value = name;

            //Validating the XmlDocument
            this._Validate(doc);

        }//_ValidateRename

        private void _validationEventHandler(object sender, ValidationEventArgs args)
        {
            throw new System.Exception(args.Message);
        }//_validationEventHandler

        private void _ValidationCallbackOne(object sender, ValidationEventArgs args)
        {
            throw new System.Exception("Invalid schema.");
        }//_ValidationCallbackOne

        private string _GetLinkTypeForSchema(string linkType)
        {
            String linkTypeForSchema = linkType;
            // if dynamic, pull out linkType from "linkType_SomeComponent" for schema path, etc.
            if (linkType.Contains(ConfigFileConstants.dynamicLinkTypeDelimiter))
            {
                int firstInstance = linkType.IndexOf(ConfigFileConstants.dynamicLinkTypeDelimiter);
                linkTypeForSchema = linkType.Substring(0, firstInstance);
            }

            return linkTypeForSchema;
        }//_GetLinkTypeForSchema

        private string _GetValidationSchemaPath(string linkType)
        {
            IXPathNavigable link = this.m_model.GetLink(linkType);

            if (link != null)
            {
                XPathNavigator nav = link.CreateNavigator();
                String fileName = nav.GetAttribute(ConfigFileConstants.schemaFilename, string.Empty);

                String basePath = "";
                String extension = ".xsd";

                // if no filename is given, use the linktype
                if (fileName == null || fileName.Length == 0)
                {
                    return basePath + linkType + extension;
                }
                else
                {
                    if (fileName.Contains(extension))
                    {
                        return basePath + fileName;
                    }
                    else
                    {
                        return basePath + fileName + extension;
                    }
                }
            }
            else
            {
                throw new Exception("Could not find link in configuration file for: " + linkType);
            }

            //string sPath = string.Format(@"{0}\{1}{2}{3}.xsd", 
            //    this.m_model.ConfigurationPath,
            //    this.Configuration,
            //    XmlSchemaConstants.Config.Config_LinkType_Seperator,
            //    linkType);
            //return sPath;
        }//_GetValidationSchemaPath

        private string _GetSchemaPath(XmlDocument doc)
        {
            string sSchemaPath = null;

            try
            {
                if  (
                    (doc != null)
                    &&
                    (doc.DocumentElement != null)
                    )
                {
                    XmlAttribute schemaAttribute = doc.DocumentElement.Attributes["xsi:noNamespaceSchemaLocation"];
                    sSchemaPath = schemaAttribute.Value;
                }
            }//try
            catch {}
        
            return sSchemaPath;
        }//_GetSchemaPath


        #region Read Values From Schema

        protected SchemaDefaults _GetDefaultValuesFromSchema(XmlDocument validationDoc)
        {
            SchemaDefaults schemaValues = new SchemaDefaults();

            try
            {
                if (validationDoc != null)
                {
                    string sSchemaPath = this._GetSchemaPath(validationDoc);
                    if (sSchemaPath != null)
                    {
                        //Create settings
                        XmlReaderSettings settings = new XmlReaderSettings();
                        XmlReader xsdReader = this.GetXSD(sSchemaPath);
                        settings.Schemas.Add(null, xsdReader);
                        settings.ValidationType = ValidationType.Schema;

                        //Create reader with settings
                        StringReader strReader = new StringReader(validationDoc.OuterXml);
                        XmlReader xmlReader = XmlReader.Create(strReader, settings);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlReader);

                        string sXpathVisibleFuncs =
                            string.Format("//*[@{0}][@{1}]",
                            XmlSchemaConstants.Config.Id,
                            XmlSchemaConstants.Config.VisibleFunctions);
                        string sXpathInvisibleFuncs =
                            string.Format("//*[@{0}][@{1}]",
                            XmlSchemaConstants.Config.Id,
                            XmlSchemaConstants.Config.InvisibleFunctions);

                        XmlNodeList nodeListVisibleFuncs = doc.SelectNodes(sXpathVisibleFuncs);
                        XmlNodeList nodeListInvisibleFuncs = doc.SelectNodes(sXpathInvisibleFuncs);

                        foreach (XmlNode visibleFuncNode in nodeListVisibleFuncs)
                        {
                            XmlAttribute xmlAttrCompID = visibleFuncNode.Attributes[XmlSchemaConstants.Config.Id];
                            XmlAttribute xmlAttrVisibleFuncs = visibleFuncNode.Attributes[XmlSchemaConstants.Config.VisibleFunctions];
                            int iCompID = int.Parse(xmlAttrCompID.Value);
                            string sVisibleFuncName = xmlAttrVisibleFuncs.Name;
                            string sVisibleFuncValue = xmlAttrVisibleFuncs.Value;
                            schemaValues.AddComponentFunctions(iCompID, sVisibleFuncName, sVisibleFuncValue);
                        }//foreach visible function node

                        foreach (XmlNode invisibleFuncNode in nodeListInvisibleFuncs)
                        {
                            XmlAttribute xmlAttrCompID = invisibleFuncNode.Attributes[XmlSchemaConstants.Config.Id];
                            XmlAttribute xmlAttrInvisibleFuncs = invisibleFuncNode.Attributes[XmlSchemaConstants.Config.InvisibleFunctions];
                            int iCompID = int.Parse(xmlAttrCompID.Value);
                            string sInvisibleFuncName = xmlAttrInvisibleFuncs.Name;
                            string sInvisibleFuncValue = xmlAttrInvisibleFuncs.Value;
                            schemaValues.AddComponentFunctions(iCompID, sInvisibleFuncName, sInvisibleFuncValue);
                        }//foreach invisible function node

                        xsdReader.Close();
                    }//if schemaPath != null
                }//if validationDoc != null
            }//try
            catch(Exception e) 
            {
                throw e;
            }

            return schemaValues;
        }//_GetDefaultValuesFromSchema

        #endregion  //read values from schema

        #endregion  //component add validation

        #region Get Components To Display

        private string _DisplayComponentsSchemaPath
        {
            get
            {
                string sPath = "Components.xsd";
                return sPath;
            }//get
        }//_DisplayComponentsSchemaPath

        private string _DisplayLinkSchemaPath
        {
            get
            {
                string sPath = "Links.xsd";
                return sPath;
            }//get
        }//_DisplayLinkSchemaPath

        private XmlElement _GetXmlTree(
            XmlDocument doc, int compID, string linkType,
            ComponentOptions compOptions,
            SchemaDefaults schemaDefs,
            DataTable componentTable)
        {

            Dictionary<String, List<DataRow>> parameterTableCache = null;

            if (useLoadingCaches)
            {
                parameterTableCache = loadingParameterCache[m_model];
            }
            else
            {
                if (compOptions.CompParams)
                {
                    if (!Controller.componentParametersXMLCache[m_model].ContainsKey("" + compID) && compID >= 0)
                    {
                        parameterTableCache = GetParameterTableCache();
                    }
                }
            }

            // building the caches can be expensive, only do so if we have a link type and multi-level 
            // otherwise we're fetching for a single component
            if (linkType != null && !linkType.Equals("") && ( (compOptions.LevelDown.HasValue && compOptions.LevelDown.Value >= 2) || !compOptions.LevelDown.HasValue))
            {
                Dictionary<String, DataRow> componentTableCache;
                Dictionary<String, List<DataRow>> linkTableCache;
                GetComponentAndLinkTableCache(out componentTableCache, out linkTableCache, componentTable, linkType);

                DataRow component = null;
                String key = "" + compID;
                if (componentTableCache != null)
                {
                    if (componentTableCache.ContainsKey(key))
                    {
                        component = componentTableCache[key];
                    }
                }
                else
                {
                    component = this._GetComponent(compID);
                }

                return this._GetXmlTree(
                    doc,
                    component,
                    linkType,
                    compOptions,
                    schemaDefs,
                    new Dictionary<String, String>(),
                    new Dictionary<String, XmlNode>(),
                    componentTable,
                    componentTableCache,
                    linkTableCache,
                    parameterTableCache,
                    new List<Int32>());
            }
            else
            {
                return this._GetXmlTree(
                   doc,
                   this._GetComponent(compID),
                   linkType,
                   compOptions,
                   schemaDefs,
                   new Dictionary<String, String>(),
                   new Dictionary<String, XmlNode>(),
                   componentTable,
                   null,
                   null,
                   null,
                   new List<Int32>());
            }
        }//_GetXmlTree


        protected Dictionary<String, DataRow> GetLinkTableCache()
        {
            Dictionary<String, DataRow> linkTableCache = new Dictionary<String, DataRow>();

            DataTable linkTable = m_model.GetLinkTable();

            foreach (DataRow linkRow in linkTable.Rows)
            {
                String id = linkRow[SchemaConstants.Id].ToString();

                if (!linkTableCache.ContainsKey(id))
                {
                    linkTableCache.Add(id, linkRow);
                }
            }

            return linkTableCache;
        }

        /// <summary>
        /// Returns dictionary hashed to fromComponentId, sorts ascending by id, 
        /// </summary>
        /// <param name="linkType"></param>
        /// <returns></returns>
        protected Dictionary<String, List<DataRow>> GetLinkTableCache(String linkType)
        {
            if (delayedValidationLinkTableCache != null)
            {
                if (delayedValidationLinkTableCache.ContainsKey(linkType))
                {
                    return delayedValidationLinkTableCache[linkType];
                }
            }

            Dictionary<String, List<DataRow>> linkTableCache = new Dictionary<String, List<DataRow>>();

            DataTable linkTable = m_model.GetLinkTable(linkType);

            List<DataRow> forRows;
            foreach (DataRow linkRow in linkTable.Rows)
            {
                String fromID = linkRow[SchemaConstants.From].ToString();

                if (linkTableCache.ContainsKey(fromID)) 
                {
                    linkTableCache[fromID].Add(linkRow);
                }
                else
                {
                    forRows = new List<DataRow>();
                    forRows.Add(linkRow);
                    linkTableCache.Add(fromID, forRows);
                }
            }

            return linkTableCache;
        }

        protected void PopulateChildToClassCache(Dictionary<String, DataRow> componentTableCache)
        {
            if (useLoadingCaches)
            {
                return; // skip 
            }

            instanceToClassCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
            instanceToClassCache.Add(m_model, new Dictionary<string, DataRow>());
            loadingInstanceToClassCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
            loadingInstanceToClassCache.Add(m_model, new Dictionary<string, DataRow>());

            DataTable linkTable = m_model.GetLinkTable(Component.Class.ClassInstanceLinkType);

            foreach (DataRow linkRow in linkTable.Rows)
            {
                String fromID = linkRow[SchemaConstants.From].ToString();
                String toID = linkRow[SchemaConstants.To].ToString();

                instanceToClassCache[m_model].Add(toID, componentTableCache[fromID]);
                loadingInstanceToClassCache[m_model].Add(toID, componentTableCache[fromID]);
            }

            subclassToClassCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
            subclassToClassCache.Add(m_model, new Dictionary<string, DataRow>());
            loadingSubClassToClassCache = new Dictionary<AME.Model.Model, Dictionary<string, DataRow>>();
            loadingSubClassToClassCache.Add(m_model, new Dictionary<string, DataRow>());

            linkTable = m_model.GetLinkTable(Component.Class.ClassSubclassLinkType);

            foreach (DataRow linkRow in linkTable.Rows)
            {
                String fromID = linkRow[SchemaConstants.From].ToString();
                String toID = linkRow[SchemaConstants.To].ToString();

                subclassToClassCache[m_model].Add(toID, componentTableCache[fromID]);
                loadingSubClassToClassCache[m_model].Add(toID, componentTableCache[fromID]);
            }
        }

        public void CheckUseLoadingCaches()
        {
            if (useLoadingCaches)
            {
                throw new Exception("Loading caches enabled, data cannot be updated.  Please disable loading caches first");
            }
        }

        public void GetComponentAndLinkTableCache(out Dictionary<String, DataRow> componentTableCache, 
                                                            out Dictionary<String, List<DataRow>> linkTableCache, DataTable componentTable, String linkType)
        {
            if (useLoadingCaches)
            {
                componentTableCache = loadingComponentCache[m_model];
                if (loadingLTCache[m_model].ContainsKey(linkType))
                {
                    linkTableCache = loadingLTCache[m_model][linkType];
                }
                else
                {
                    linkTableCache = GetLinkTableCache(linkType);
                }
            }
            else
            {
                componentTableCache = GetComponentTableCache(componentTable);
                linkTableCache = GetLinkTableCache(linkType);
            }
        }

        public void EnableLoadingCache()
        {
            loadingComponentTable[m_model] = GetComponentTable();
            loadingComponentCache[m_model] = GetComponentTableCache(loadingComponentTable[m_model]);
            loadingLinkCache[m_model] = GetLinkTableCache();
            loadingParameterCache[m_model] = GetParameterTableCache();
            loadingLTCache[m_model] = new Dictionary<String, Dictionary<String, List<DataRow>>>();
             
            //  link types
            IXPathNavigable iNav = this.GetLinks();
            XPathNavigator nav = iNav.CreateNavigator();
            XPathNodeIterator links = nav.Select("Link");

            foreach (XPathNavigator link in links)
            {
                String lt = link.GetAttribute(SchemaConstants.Type, "");
                Dictionary<String, List<DataRow>> ltCache = GetLinkTableCache(lt);
                loadingLTCache[m_model].Add(lt, ltCache);
            }

            PopulateChildToClassCache(loadingComponentCache[m_model]);

            useLoadingCaches = true;
        }

        public void DisableLoadingCache()
        {
            loadingComponentTable[m_model] = null;
            loadingComponentCache[m_model] = new Dictionary<string, DataRow>();
            loadingLinkCache[m_model] = new Dictionary<string, DataRow>();
            loadingParameterCache[m_model] = new Dictionary<string, List<DataRow>>();
            loadingLTCache[m_model] = new Dictionary<String, Dictionary<String, List<DataRow>>>();

            useLoadingCaches = false;
        }
     
        protected Dictionary<String, DataRow> GetComponentTableCache(DataTable componentTable)
        {
            if (delayedValidationComponentTableCache != null)
            {
                return delayedValidationComponentTableCache;
            }

            Dictionary<String, DataRow> componentTableCache = new Dictionary<String, DataRow>();

            foreach (DataRow compRow in componentTable.Rows)
            {
                String id = compRow[SchemaConstants.Id].ToString();

                if (!componentTableCache.ContainsKey(id))
                {
                    componentTableCache[id] = compRow;
                }
            }

            return componentTableCache;
        }

        protected Dictionary<String, List<DataRow>> GetParameterTableCache()
        {
            Dictionary<String, List<DataRow>> parameterTableCache = new Dictionary<String, List<DataRow>>();

            DataTable parameterTable = m_model.GetParameterTable();

            List<DataRow> forRows;
            foreach (DataRow parameterRow in parameterTable.Rows)
            {
                String parentID = parameterRow[SchemaConstants.ParentId].ToString();
                String parentType = parameterRow[SchemaConstants.ParentType].ToString();

                String key = parentID + parentType;

                if (parameterTableCache.ContainsKey(key))
                {
                    parameterTableCache[key].Add(parameterRow);
                }
                else
                {
                    forRows = new List<DataRow>();
                    forRows.Add(parameterRow);
                    parameterTableCache.Add(key, forRows);
                }
            }

            return parameterTableCache;
        }

        protected IXPathNavigable _GetOutputXml(String filename)
        {
            return this.m_model.GetOutputXml(filename);
        }

        private DataTable GetChildComponentsByCache(int componentID, String linkType, DataTable componentTable, 
            Dictionary<String, DataRow> componentTableCache, Dictionary<String, List<DataRow>> linkTableCache)
        {
            DataTable childComponents = componentTable.Clone();

            int componentRowCount = 0;

            childComponents.Columns.Add(SchemaConstants.LinkID, typeof(int));  //add column "LinkID"

            String compIDStringCompare = componentID.ToString();

            if (linkTableCache.ContainsKey(compIDStringCompare))
            {
                List<DataRow> fromMatch = linkTableCache[compIDStringCompare];

                foreach (DataRow linkChild in fromMatch)
                {
                    String toID = linkChild[SchemaConstants.To].ToString();
                    if (componentTableCache.ContainsKey(toID))
                    {
                        DataRow componentChild = componentTableCache[toID];

                        childComponents.ImportRow(componentChild);
                        childComponents.Rows[componentRowCount][SchemaConstants.LinkID] = linkChild[SchemaConstants.Id]; // set Link ID column's value
                        componentRowCount++;
                    }
                }
            }

            return childComponents;
        }

        private XmlElement _GetXmlTree(XmlDocument doc, DataRow component, string linkType,
            ComponentOptions compOptions, SchemaDefaults schemaDefs, Dictionary<String, String> typeToBaseCache, 
                Dictionary<String, XmlNode> typeToParametersXMLCache, DataTable componentTable, Dictionary<String, DataRow> componentTableCache, 
                    Dictionary<String, List<DataRow>> linkTableCache, Dictionary<String, List<DataRow>> parameterTableCache, List<Int32> callerIDs)
        {
            if (componentTableCache != null && instanceToClassCache == null)
            {
                PopulateChildToClassCache(componentTableCache);
            }

            if (useLoadingCaches)
            {
                instanceToClassCache = loadingInstanceToClassCache;
                subclassToClassCache = loadingSubClassToClassCache;
            }

            if (component == null)
            {
                return null;
            }

            XmlElement xmlElement = this._GetXmlTree(doc, component, compOptions, schemaDefs, typeToBaseCache, 
                    typeToParametersXMLCache, parameterTableCache);

            //getting children
            int iCompID = (int)component[SchemaConstants.Id];

            if (
                !compOptions.LevelDown.HasValue ||
                (compOptions.LevelDown.HasValue && (compOptions.LevelDown.Value > 0))
                )
            {
                ComponentOptions newCompOptions = compOptions.Clone();
                if (compOptions.LevelDown.HasValue)
                {
                    newCompOptions.LevelDown = compOptions.LevelDown - 1;
                }

                if (linkType != null && !linkType.Equals(String.Empty))
                {

                    DataTable childComponents = null;

                    // new fetch - build from component and link table in memory
                    if (componentTable != null && componentTableCache != null && linkTableCache != null)
                    {
                        childComponents = GetChildComponentsByCache(iCompID, linkType, componentTable, componentTableCache, linkTableCache);
                    }
                    else
                    {
                        childComponents = this.m_model.GetChildComponents(iCompID, linkType);
                    }

                    // add self to caller ID list before recursing into children
                    callerIDs.Add(iCompID);
  
                    foreach (DataRow childComponent in childComponents.Rows)
                    {
                        XmlElement childElement = null;

                        // if child is parent, set level to 0 terminate infinite recursion 
                        // or if the child has already made this call, ie he's in the caller ID list
                        int childID = (int)childComponent[SchemaConstants.Id];
                        if (iCompID == childID || callerIDs.Contains(childID))
                        {
                            ComponentOptions cloneForSelfLink = newCompOptions.Clone();
                            cloneForSelfLink.LevelDown = 0;
                            //getting xml element for the child component
                            childElement = this._GetXmlTree(doc, childComponent, linkType, cloneForSelfLink, schemaDefs, typeToBaseCache,
                                typeToParametersXMLCache, componentTable, componentTableCache, linkTableCache, parameterTableCache, callerIDs);
                        }
                        else
                        {
                            childElement = this._GetXmlTree(doc, childComponent, linkType, newCompOptions, schemaDefs, typeToBaseCache,
                                typeToParametersXMLCache, componentTable, componentTableCache, linkTableCache, parameterTableCache, callerIDs);
                        }

                        //adding child element to parent
                        xmlElement.AppendChild(childElement);
                    }//foreach child component

                    // we're returning, remove ourself from the caller list
                    callerIDs.Remove(iCompID);

                    // before we return, check includes for other linktypes to include
                    if (includesMap.Count > 0 && !processingIncludes)
                    {
                        String compType = (String)component[SchemaConstants.Type];
                        if (includesMap.ContainsKey(compType))
                        {
                            List<Include> referencedIncludes = includesMap[compType];
                            foreach (Include referencedInclude in referencedIncludes)
                            {
                                String referenceIncludeLT = referencedInclude.LinkType;
                                if (referencedInclude.IsDynamic)
                                {
                                    referenceIncludeLT = this.GetDynamicLinkType(referenceIncludeLT, "" + iCompID);
                                }
                                ComponentOptions full = new ComponentOptions();
                                full.CompParams = referencedInclude.IncludeParameters;
                                full.ClassInstanceInfo = false;
                                full.SubclassInstanceInfo = false;
                                full.LinkParams = referencedInclude.IncludeParameters;
                                full.LevelDown = referencedInclude.LevelDown;

                                processingIncludes = true; // a guard so that this call doesn't invoke includes!
                                IXPathNavigable inav = this.GetComponentAndChildren(iCompID, referenceIncludeLT, full);
                                processingIncludes = false;

                                XmlElement includes = xmlElement.OwnerDocument.CreateElement("Include");
                                XmlAttribute isDynamicAttr = xmlElement.OwnerDocument.CreateAttribute("isDynamic");
                                XmlAttribute linkTypeAttr = xmlElement.OwnerDocument.CreateAttribute("linkType");
                                XmlAttribute componentTypeAttr = xmlElement.OwnerDocument.CreateAttribute("componentType");

                                isDynamicAttr.Value = referencedInclude.IsDynamic.ToString();
                                linkTypeAttr.Value = referencedInclude.LinkType;
                                componentTypeAttr.Value = referencedInclude.ComponentType;

                                includes.Attributes.Append(isDynamicAttr);
                                includes.Attributes.Append(linkTypeAttr);
                                includes.Attributes.Append(componentTypeAttr);

                                XmlNodeList includeAppends = ((XmlDocument)inav).SelectNodes("/Components/Component/Component");
                                foreach (XmlNode includeAppend in includeAppends)
                                {
                                    XmlNode imported = xmlElement.OwnerDocument.ImportNode(includeAppend, true);
                                    includes.AppendChild(imported);
                                }
                                xmlElement.AppendChild(includes);
                            }
                        }
                    }
                } // linkType check
            }//if depth is > 0

            return xmlElement;
        }//_GetXmlTree

        private XmlElement _GetXmlTree(XmlDocument doc, int compID, string childType, bool isChildBaseType, IComponentOnlyOptions comOptions)
        {
            DataRow component = this._GetComponent(compID);

            return this._GetXmlTree(doc, component, childType, isChildBaseType, comOptions);
        }


        private XmlElement _GetXmlTree(XmlDocument doc, DataRow component, string childType, bool isChildBaseType, IComponentOnlyOptions comOptions)
        {
            XmlElement xmlElement = null;
        //this._GetXmlTree(doc, compID, childType, isChildBaseType, compOptions)
         //   DataTable _GetChildComponents(int compID, string childType, bool isChildBaseType)

            xmlElement = this._GetXmlTree(
                doc, component, comOptions, null, null,
                    null, null);

            if (component != null)
            {
                //getting children
                int iCompID = (int)component[SchemaConstants.Id];
                DataTable childComponents = null;
                childComponents = this._GetChildComponents(iCompID, childType, isChildBaseType);
                foreach (DataRow childComponent in childComponents.Rows)
                {
                    XmlElement childElement = null;
                    childElement = this._GetXmlTree(
                doc, childComponent, comOptions, null, null,
                    null, null);

                    xmlElement.AppendChild(childElement);
                }//foreach child component
            }//if component is not null (or say it exists)

            return xmlElement;
        }

        //bypass cache, e.g. single component lookup
        protected XmlElement _GetXmlTree(XmlDocument doc, int compID, IComponentOnlyOptions compOptions, SchemaDefaults schemaDefs)
        {
            return this._GetXmlTree(doc, this._GetComponent(compID), compOptions, schemaDefs, null, null, null);
        }//_GetXmlTree

        //bypass cache, e.g. single component lookup
        protected XmlElement _GetXmlTree(XmlDocument doc, DataRow component, IComponentOnlyOptions compOptions, SchemaDefaults schemaDefs)
        {
            return this._GetXmlTree(doc, component, compOptions, schemaDefs, null, null, null);
        }//_GetXmlTree

        public XmlElement CommonElementCreate(XmlDocument doc, int id, int? linkID, String baseType, String type, String name, String desc, Component.eComponentType eType)
        {
            int? iClassID = null;
            int? iSubClassID = null;
            DataRow drClass = null;
            DataRow drSubClass = null;

            //creating element
            XmlElement xmlElement = doc.CreateElement(XmlSchemaConstants.Display.sComponent);

            //creating attributes
            XmlAttribute attrCompType = doc.CreateAttribute(XmlSchemaConstants.Display.Component.Type);
            attrCompType.Value = type;

            XmlAttribute attrCompID = doc.CreateAttribute(XmlSchemaConstants.Display.Component.Id);
            attrCompID.Value = id.ToString();

            XmlAttribute attrCompName = doc.CreateAttribute(XmlSchemaConstants.Display.Component.Name);
            attrCompName.Value = name;

            XmlAttribute attrCompDesc = doc.CreateAttribute(XmlSchemaConstants.Display.Component.Description);
            attrCompDesc.Value = desc;

            XmlAttribute attCompEtype = doc.CreateAttribute(XmlSchemaConstants.Display.Component.eType);
            attCompEtype.Value = eType.ToString();

            //adding attributes
            xmlElement.SetAttributeNode(attrCompType);
            xmlElement.SetAttributeNode(attrCompID);
            xmlElement.SetAttributeNode(attrCompName);
            xmlElement.SetAttributeNode(attrCompDesc);
            xmlElement.SetAttributeNode(attCompEtype);

            //Base Component Type attribute
            if (baseType != null)
            {
                XmlAttribute attrBaseCompType = doc.CreateAttribute(XmlSchemaConstants.Display.Component.BaseType);
                attrBaseCompType.Value = baseType;
                xmlElement.SetAttributeNode(attrBaseCompType);
            }//Base Component Type attribute

            //LinkID attribute

            if (linkID.HasValue)
            {
                XmlAttribute attrLinkID = doc.CreateAttribute(XmlSchemaConstants.Display.Component.LinkID);
                attrLinkID.Value = linkID.Value.ToString();

                xmlElement.SetAttributeNode(attrLinkID);
            }

            //class & subclass components and their ids
            this._GetClassSubclass(id, eType, out drClass, out drSubClass);
            if (drClass != null)
            {
                iClassID = (int)drClass[SchemaConstants.Id];
            }
            if (drSubClass != null)
            {
                iSubClassID = (int)drSubClass[SchemaConstants.Id];
            }
            
            //if (compOptions.InstanceUseClassName)
            //{
            //    // check use class name controller variable
            //    //ComponentIDName classIDName = this.CheckUseClassName(iCompID, eCompEtype);
            //    if (drClass != null)
            //    {
            //        string key = drClass[SchemaConstants.Id].ToString();

            //        string valueCheck = GetCacheValue(key, Component.Class.InstancesUseClassName.Name, eParamParentType.Component);
            //        if (valueCheck != null)
            //        {
            //            if (valueCheck.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                xmlElement.Attributes[XmlSchemaConstants.Display.Component.Name].Value = drClass[SchemaConstants.Name].ToString();
            //            }
            //        }
            //        else
            //        {
            //            ComponentIDName classIDName = this.CheckUseClassName(drClass);

            //            if (classIDName != null)
            //            {
            //                xmlElement.Attributes[XmlSchemaConstants.Display.Component.Name].Value = classIDName.Name;
            //            }
            //        }
            //    }
            //}//if user is asking to use class name for instances

            if (iClassID.HasValue)
            {
                XmlAttribute attrClassID = doc.CreateAttribute(XmlSchemaConstants.Display.Component.ClassID);
                attrClassID.Value = iClassID.Value.ToString();

                xmlElement.SetAttributeNode(attrClassID);
            }//if classInstanceInfo

            if (iSubClassID.HasValue)
            {
                XmlAttribute attrSubclassID = doc.CreateAttribute(XmlSchemaConstants.Display.Component.SubclassID);
                attrSubclassID.Value = iSubClassID.Value.ToString();

                xmlElement.SetAttributeNode(attrSubclassID);
            }//SubclassInstanceInfo

            return xmlElement;
        }

        protected XmlElement _GetXmlTree(XmlDocument doc, DataRow component, IComponentOnlyOptions compOptions, SchemaDefaults schemaDefs, Dictionary<String, String> typeToBaseCache,
            Dictionary<String, XmlNode> typeToParametersXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            if (component == null) { return null; }

            string sCompType;
            string sBaseCompType;
            int iCompID;
            string sCompName;
            string sCompDesc;
            int? iLinkID = null;

            Component.eComponentType eCompEtype = Component.eComponentType.None;

            sCompType = (string)component[SchemaConstants.Type];

            if (typeToBaseCache != null && typeToBaseCache.ContainsKey(sCompType))
            {
                sBaseCompType = typeToBaseCache[sCompType];
            }
            else
            {
                sBaseCompType = this._GetBaseComponentType(sCompType);
                if (typeToBaseCache != null)
                {
                    typeToBaseCache.Add(sCompType, sBaseCompType);
                }
            }

            iCompID = (int)component[SchemaConstants.Id];
            sCompName = (string)component[SchemaConstants.Name];
            sCompDesc = (string)component[SchemaConstants.Description];
            eCompEtype = this._GetComponentType(component);

            //LinkID
            if (component.Table.Columns.Contains(SchemaConstants.LinkID))
            {
                iLinkID = (int)component[SchemaConstants.LinkID];
            }

            XmlElement xmlElement = this.CommonElementCreate(doc, iCompID, iLinkID, sBaseCompType, sCompType, sCompName, sCompDesc, eCompEtype);

            // Component Parameters
            if (compOptions.CompParams)
            {
                IXPathNavigable clone = _GetParametersForComponent(component, typeToParametersXMLCache, parameterTableCache);
                if (clone != null)
                {
                    XmlNode insert = ((XmlNode)clone).SelectSingleNode(XmlSchemaConstants.Display.sComponentParameters);
                    insert = doc.ImportNode(insert, true);
                    xmlElement.AppendChild(insert);
                }
            }//if Parameters

            //Link Parameters
            if (compOptions.LinkParams && iLinkID.HasValue)
            {
                IXPathNavigable clone = _GetParametersForLink(iLinkID.Value, typeToParametersXMLCache, parameterTableCache);

                if (clone != null)
                {
                    XmlNode insert = ((XmlNode)clone).SelectSingleNode(XmlSchemaConstants.Display.sLinkParameters);
                    insert = doc.ImportNode(insert, true);
                    xmlElement.AppendChild(insert);
                }
            }//if Link Parameters

            //Functions
            if (schemaDefs != null)
            {
                XmlElement funcsElement = this._GetFunctionsForComponent(doc, iCompID, schemaDefs);

                if (funcsElement != null)
                {
                    xmlElement.AppendChild(funcsElement);
                }
            }//if functions

            return xmlElement;
        }//_GetXmlTree

        private XmlElement _GetFunctionsForComponent(XmlDocument doc, int compID, SchemaDefaults schemaDefs)
        {
            //creating "Functions" element
            XmlElement xmlElementFuncs = doc.CreateElement(XmlSchemaConstants.Display.sFunction + "s");    //Functions

            if (schemaDefs.ComponentFunctions.ContainsKey(compID))
            {
                Dictionary<string, ComponentFunction> compFuncs = schemaDefs.ComponentFunctions[compID];
                foreach (ComponentFunction compFunc in compFuncs.Values)
                {
                    //adding "Function" element to "Functions" element
                    XmlElement xmlElementFunc = CreateXmlFunction(doc, compFunc.Name, compFunc.Action, ""+compFunc.Visible);
                    xmlElementFuncs.AppendChild(xmlElementFunc);
                }//foreach component function
            }//if any functions for compID

            return xmlElementFuncs;
        }//_GetFunctionsForComponent

        public XmlElement CreateXmlFunction(XmlDocument doc, String name, String action, String visible)
        {
            //creating "Function" element
            XmlElement xmlElementFunc = doc.CreateElement(XmlSchemaConstants.Display.sFunction);

            //creating attributes
            XmlAttribute attrFuncName = doc.CreateAttribute(XmlSchemaConstants.Display.Function.Name);
            attrFuncName.Value = name;

            XmlAttribute attrFuncAction = doc.CreateAttribute(XmlSchemaConstants.Display.Function.Action);
            attrFuncAction.Value = action;

            XmlAttribute attrFuncVisible = doc.CreateAttribute(XmlSchemaConstants.Display.Function.Visible);
            attrFuncVisible.Value = visible.ToLower(); // boolean parsing

            //adding attributes
            xmlElementFunc.SetAttributeNode(attrFuncName);
            xmlElementFunc.SetAttributeNode(attrFuncAction);
            xmlElementFunc.SetAttributeNode(attrFuncVisible);

            return xmlElementFunc;
        }

        protected XmlDocument _GetComponentsXmlDoc(out XmlElement topNode)
        {
            string sSchemaFilePath = this._DisplayComponentsSchemaPath;
            bool bValidSchemaPath = false;
            bValidSchemaPath = Path.HasExtension(sSchemaFilePath);

            if (bValidSchemaPath)
            {
                using (XmlReader schemaXSD = this.GetXSD(sSchemaFilePath))
                {
                    if (schemaXSD == null)
                    {
                        throw new System.IO.FileNotFoundException("File does not exist.", sSchemaFilePath);
                    }
                }
            }

            if (!bValidSchemaPath)
            {
                throw new System.ArgumentException("Invalid schema path.");
            }

            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create top node element
            topNode = doc.CreateElement(XmlSchemaConstants.Display.sComponent + "s");//"Components"

            // Add schema information to top node.
            XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schema.Value = this._DisplayComponentsSchemaPath;
            topNode.SetAttributeNode(schema);

            doc.AppendChild(topNode);

            return doc;
        }//_GetComponentsXmlDoc

        public XmlDocument _GetComponentsXmlDoc(int? topID, int compID, string linkType, ComponentOptions compOptions)
        {
            XmlElement topXmlNode;
            XmlDocument doc = this._GetComponentsXmlDoc(out topXmlNode);

            if (topID.HasValue && topID.Value == -1 && compID == -1)
            {
                return doc;
            }

            if (compID == -1)
            {
                return doc;
            }

            SchemaDefaults schemaDefs = null;

            DataTable componentTable = null;

            if (useLoadingCaches)
            {
                componentTable = loadingComponentTable[m_model];
            }
            else
            {
                componentTable = GetComponentTable();
            }

            if (topID.HasValue)
            {
                //Verifying if Top Component Exists
                if ( this._ComponentExists(topID.Value) )
                {
                    //Getting validation document
                    XmlDocument validationDoc = this._GetXmlDoc_Validation(topID.Value, linkType, componentTable);
                    if (validationDoc != null)
                    {
                        //Reading default values from schema through validation document.
                        schemaDefs = this._GetDefaultValuesFromSchema(validationDoc);
                    }
                }//if Top component exists
            }//if topID has value

            XmlElement xmlElement =
                this._GetXmlTree(doc, compID, linkType,
                compOptions, schemaDefs, componentTable);
            if (xmlElement != null)
            {
                topXmlNode.AppendChild(xmlElement);
            }
            
            instanceToClassCache = null; // cleanup within the context of this call
            subclassToClassCache = null;

            return doc;
        }//_GetComponentsXmlDoc

        protected XmlDocument _GetComponentAndChildren(int compID, string childType, bool isChildBaseType, IComponentOnlyOptions compOptions)
        {
            XmlElement topXmlNode;
            XmlDocument doc = this._GetComponentsXmlDoc(out topXmlNode);

            XmlElement xmlElement = this._GetXmlTree(doc, compID, childType, isChildBaseType, compOptions);
            if (xmlElement != null)
            {
                topXmlNode.AppendChild(xmlElement);
            }

            return doc;
        }//_GetComponentAndChildren


        #endregion  //Get Components To Display

#if NotNeededAnymore_KeepForReference

        private int _XmlElementMaxDepth(XmlNode node, string searchName)
        {
            return this._XmlNodeMaxDepth(node, searchName, XmlNodeType.Element);
        }//_XmlElementMaxDepth

        private int _XmlNodeMaxDepth(XmlNode node, string searchName, XmlNodeType searchNodeType)
        {
            int maxDepth = -1;

            if (node != null)
            {
                using (XmlNodeReader nodeReader = new XmlNodeReader(node))
                {
                    while (nodeReader.Read())
                    {
                        if ((nodeReader.NodeType == searchNodeType)
                            && (nodeReader.Name == searchName))
                        {
                            int depth = nodeReader.Depth;

                            if (depth > maxDepth)
                            {
                                maxDepth = depth;
                            }
                        }//if element
                    }//while
                }
            }

            return maxDepth;
        }//_XmlNodeMaxDepth

        private XmlSchema ReadAndCompileSchema(string fileName)
        {
            XmlTextReader tr = new XmlTextReader(fileName, new NameTable());

            XmlSchemaSet schemaSet = new XmlSchemaSet();

            // The Read method will throw errors encountered
            // on parsing the schema
            XmlSchema schema = XmlSchema.Read(tr,
                new ValidationEventHandler(ValidationCallbackOne));
            tr.Close();

            schemaSet.Add(schema);
            schemaSet.Compile(); // The Compile method will throw errors 
                                 // encountered on compiling the schema

            return schema;
        }

        private List<string> GetAllComponentTypes()
        {
            List<string> componentTypes = new List<string>();

            XmlSchema schema = this.ReadAndCompileSchema(this.ConfigFilePath);

            foreach (XmlSchemaElement elem in schema.Elements.Values)
            {
                string sType = elem.Name;
                //not adding the ResourceEditor or etc.. to the component type list
                if ( (sType != this.ConfigType)
                    && !componentTypes.Contains(sType) )
                {
                    componentTypes.Add(sType);
                }
            }//foreach element

            return componentTypes;
        }//GetAllComponentTypes

        private string GetFixedAttributeValue(string compType, string attrName)
        {
            string sAttrValue = "";
            XmlSchema schema = this.ReadAndCompileSchema(this.ConfigFilePath);

            foreach (XmlSchemaElement elem in schema.Elements.Values)
            {
                if ( (elem.Name == compType) &&
                    (elem.ElementSchemaType is XmlSchemaComplexType) )
                {
                    XmlSchemaComplexType ct = (XmlSchemaComplexType)elem.ElementSchemaType;

                    if (ct.AttributeUses.Count > 0)
                    {
                        IDictionaryEnumerator ienum = ct.AttributeUses.GetEnumerator();

                        while (ienum.MoveNext())
                        {
                            XmlSchemaAttribute att = (XmlSchemaAttribute)ienum.Value;
                            if (att.Name == attrName)
                            {
                                sAttrValue = att.FixedValue;
                                return sAttrValue;
                            }//if attribute name matches
                        }//while each attribute
                    }//if has attribute
                }//if component type matches
            }//foreach element

            return sAttrValue;
        }//GetFixedAttributeValue

#endif

    }//Controller class
}//Controllers namespace
