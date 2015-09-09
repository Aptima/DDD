/*
 * Class            : Controller
 * File             : Controller_Public.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      : 
 * Contains the public methods, properties, and events. It implements the members 
 * of interface - IController.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using AME.Model;
using System.Xml;
using AME.Controllers.Base.Data_Structures;
using AME.Views.View_Components;
using AME.Tools;
using AME.Controllers.Base.DataStructures;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

#endregion  //Namespaces

namespace AME.Controllers
{
    public partial class Controller
    {

        #region Members to be deleted later on

        public DataTable GetComponentTable()
        {
            if (delayedValidationComponentTable != null)
            {
                return delayedValidationComponentTable;
            }
            return m_model.GetComponentTable();
        }//GetConfigurationTable

        public DataTable GetLinkTable()
        {
            return m_model.GetLinkTable();
        }//GetLinkTable

        public DataTable GetParameterTable()
        {
            return m_model.GetParameterTable();
        }//GetParameterTable

        #endregion

        #region IController Members

        public string ModelPath { get { return m_model.ModelPath; } }
        public string OutputPath { get { return m_model.OutputPath; } }
        public string DataPath { get { return m_model.DataPath; } }
        public string XmlPath { get { return m_model.XmlPath; } }
        public string DocumentationPath { get { return m_model.DocumentationPath; } }
        public string LicensePath { get { return m_model.LicensePath; } }

        public Stream GetImage(String image)
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // get the namespace from the model paths object, combine with the  name
                String imagePath = this.m_model.ModelConfiguration.imgNamespace + "." + image;
                StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(imagePath));
                return streamReader.BaseStream;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read image: " + image, e.Message);
                return null;
            }
        }

        public XmlReader GetConfigurationReader()
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                String configPath = this.m_model.ModelConfiguration.configurationNamespace + ".configuration.xml";
                StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(configPath));
                XmlReader xmlReader = XmlReader.Create(streamReader);
                return xmlReader;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read configuration file", e.Message);
                return null;
            }
        }

        public XmlReader GetXSD(String xsd)
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // get the namespace from the model paths object, combine with the xsd name
                String xsdPath = this.m_model.ModelConfiguration.xsdNamespace + "." + xsd;
                StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(xsdPath));
                XmlReader xmlReader = XmlReader.Create(streamReader);
                return xmlReader;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Could not read XSD: " + xsd);
                return null;
            }
        }

        public Stream GetXSDStream(String xsd)
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // get the namespace from the model paths object, combine with the xsd name
                String xsdPath = this.m_model.ModelConfiguration.xsdNamespace + "." + xsd;
                StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(xsdPath));
                return streamReader.BaseStream;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read XSD: " + xsd, e.Message);
                return null;
            }
        }

        public XmlReader GetXSL(String xsl)
        {
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // get the namespace from the model paths object, combine with the xsl name
                String xslPath = this.m_model.ModelConfiguration.xslNamespace + "." + xsl;
                StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(xslPath));
                XmlReader xmlReader = XmlReader.Create(streamReader);
                return xmlReader;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not read XSL: " + xsl, e.Message);
                return null;
            }
        }

        public string Configuration
        {
            get { return this.m_sConfigType; }
        }//Configuration

        public string RootComponentType
        {
            get { return this.m_sRootComponentType; }
        }//RootComponentType

        public string ConfigurationLinkType
        {
            get { return this.m_sConfigurationLinkType; }
        }//ComponentLinkType

        public bool UseDelayedValidation
        {
            get { return m_useDelayedValidation; }
            set
            {
                if (m_useDelayedValidation && !value)
                {
                    // turned off from on, validate
                    DelayedValidate(storedValidationTopIDsAndLinkTypes);
                }

                m_useDelayedValidation = value;
            }
        }//UseDelayedValidation

        public bool AllowProgrammaticCreation
        {
            get { return m_allowProgrammaticCreation; }
            set { m_allowProgrammaticCreation = value; }
        }//AllowProgrammaticCreation

        public bool ProgressBarForDeletes
        {
            get { return progressBarForDeletes; }
            set
            {
                progressBarForDeletes = value;

                if (!progressBarForDeletes)
                {
                    BackgroundDone();
                }
            }
        }

        /// <summary>
        /// If true, parameters that have an empty string value will not be returned by the controller.
        /// All parameters can accept empty string as a legal value.
        /// The resulting behavior allows the user to clear out parameter text fields to 'delete' parameters.
        /// If false, empty string values will be returned, empty string will only be accepted on successful
        /// type validation (e.g. a string)
        /// By default, false
        /// </summary>
        public bool IgnoreEmptyString
        {
            get { return m_IgnoreEmptyString; }
            set { m_IgnoreEmptyString = value; }
        }

        //InitializeDB
        public void InitializeDB()
        {
            this.m_model.InitializeDB();
        }//InitializeDB

        public void ClearCache()
        {
            cache.ClearCache(m_model);
            componentParametersXMLCache[m_model].Clear();
            linkParametersXMLCache[m_model].Clear();
        }

        public void CacheIndexLinkTypes()
        {
            cache.IndexLinkTypes(m_model);
        }

        public void ClearCache(String linkType)
        {
            cache.ClearCache(m_model, linkType);
        }

        //DropDatabase
        public void DropDatabase()
        {
            this.m_model.DropDatabase();
        }//DropDatabase

        //ImportSql
        public void ImportSql(String filename)
        {
            this.m_model.ImportSql(filename);
        }//ImportSql

        public IXPathNavigable GetConfiguration()
        {
            IXPathNavigable component = this.m_model.GetConfiguration(this.Configuration);
            if (component != null)
            {
                return component;
            }
            else
            {
                return null;
            }
        }

        public IXPathNavigable GetComponent(String type)
        {
            IXPathNavigable component = this.m_model.GetComponent(type);
            if (component != null)
            {
                return component;
            }
            else
            {
                return null;
            }
        }

        public IXPathNavigable GetLinks()
        {
            IXPathNavigable component = this.m_model.GetLinks();
            if (component != null)
            {
                return component;
            }
            else
            {
                return null;
            }
        }

        #region Components

        public NameValueCollection GetRootLinkTypes()
        {
            return m_model.GetRootLinkTypes();
        }

        public int GetInstanceCount(int compID, int classID)
        {
            return this._GetInstanceIDs(compID, classID).Count;
        }//GetInstanceCount

        public IXPathNavigable GetOutputXml(string filename)
        {
            return this._GetOutputXml(filename);
        }//GetComponentAndChildren

        public void WriteOutputXml(String filename, XmlDocument toWrite)
        {
            this.m_model.WriteOutputXML(filename, toWrite);
        }

        public IXPathNavigable GetComponentAndChildren(int compID, string linkType, ComponentOptions compOptions)
        {
            if (compID != -1)
            {
                String key = cache.CreateCacheKey(compID, linkType);

                if (!cache.Contains(m_model, key) || (compOptions.Includes != null && compOptions.Includes.Count > 0) || processingIncludes)
                {
                    ComponentOptions full = new ComponentOptions();
                    full.CompParams = true;
                    full.ClassInstanceInfo = true;
                    full.SubclassInstanceInfo = true;
                    full.LinkParams = true;

                    if (processingIncludes) // use the level down and parameter settings for the includes
                    {
                        full.CompParams = compOptions.CompParams;
                        full.LinkParams = compOptions.LinkParams;
                        full.LevelDown = compOptions.LevelDown;
                    }

                    if (compOptions.Includes != null) // index includes for get call
                    {
                        includesMap.Clear();
                        foreach(Include include in compOptions.Includes)
                        {
                            if (!includesMap.ContainsKey(include.ComponentType))
                            {
                                includesMap[include.ComponentType] = new List<Include>();
                            }
                            includesMap[include.ComponentType].Add(include);
                        }
                    }

                    XmlDocument returnDocument = this._GetComponentsXmlDoc(null, compID, linkType, full);
                    if ( (compOptions.LevelDown.HasValue && compOptions.LevelDown.Value > 0) || !compOptions.LevelDown.HasValue)
                    {
                        if (!UseDelayedValidation && !processingIncludes) // skip when either is true
                        {
                            cache.AddCacheDocument(m_model, key, returnDocument);
                        }
                    }
                    if (compOptions.Includes != null) // clear when we're done if we used the map
                    {
                        includesMap.Clear();
                    }
                    return returnDocument;
                }
                else
                {
                    return cache.GetDocumentFromCache(m_model, key);
                }
            }
            else
            {
                return this._GetComponentsXmlDoc(null, compID, linkType, compOptions);
            }
        }//GetComponentAndChildren

        public IXPathNavigable GetComponentAndChildren(int topID, int compID, string linkType, ComponentOptions compOptions)
        {
            if (topID != -1 && compID != -1)
            {
                String key = cache.CreateCacheKey(topID, compID, linkType);
                if (!cache.Contains(m_model, key))
                {
                    // special case - display IDs with no level down, i.e. just the component
                    if ((compOptions.LevelDown.HasValue && compOptions.LevelDown.Value == 0))
                    {
                        String tryFullRootKey = cache.CreateCacheKey(topID, topID, linkType);
                        if (cache.Contains(m_model, tryFullRootKey))
                        {
                            XmlDocument baseDoc = cache.GetDocumentFromCache(m_model, tryFullRootKey);
                            XmlNode displayIDNode = baseDoc.SelectSingleNode("//Component[@ID='" + compID + "']");

                            XmlElement topXmlNode;
                            XmlDocument displayDoc = this._GetComponentsXmlDoc(out topXmlNode);
                            displayIDNode = displayDoc.ImportNode(displayIDNode, true);
                            topXmlNode.AppendChild(displayIDNode);
                            return displayDoc;
                        }
                        else
                        {
                            // error finding display ID
                            ComponentOptions full = new ComponentOptions();
                            full.CompParams = true;
                            full.ClassInstanceInfo = true;
                            full.SubclassInstanceInfo = true;
                            full.LinkParams = true;
                            XmlDocument returnDocument = this._GetComponentsXmlDoc(topID, compID, linkType, full);
                            if (!UseDelayedValidation) // skip when true
                            {
                                cache.AddCacheDocument(m_model, key, returnDocument);
                            }
                            return returnDocument;
                        }
                    }
                    else
                    {
                        // use full options when adding to cache
                        ComponentOptions full = new ComponentOptions();
                        full.CompParams = true;
                        full.ClassInstanceInfo = true;
                        full.SubclassInstanceInfo = true;
                        full.LinkParams = true;
                        XmlDocument returnDocument = this._GetComponentsXmlDoc(topID, compID, linkType, full);
                        if (!UseDelayedValidation) // skip when true
                        {
                            cache.AddCacheDocument(m_model, key, returnDocument);
                        }
                        return returnDocument;
                    }
                }
                else
                {
                    return cache.GetDocumentFromCache(m_model, key);
                }
            }
            else
            {                  
                return this._GetComponentsXmlDoc(topID, compID, linkType, compOptions);
            }
        }//GetComponentAndChildren

        //returns new Component's ID
        public ComponentAndLinkID AddComponent(int topID, int parentID, string type, string name, string linkType, string desc)
        {
            CheckUseLoadingCaches();

            m_addComponentLinkType = linkType;

            bool thisMethodTurnedOff = false;
            if (ViewUpdateStatus)
            {
                this.TurnViewUpdateOff();
                thisMethodTurnedOff = true;
            }

            ComponentAndLinkID cAndL = this._AddComponent(topID, parentID, type, name, linkType, desc);

            if (cAndL != null)
            {
                cache.AddComponentToCache(this, m_model, topID, parentID, cAndL.ComponentID, lastValidateAddComponentType, lastValidateAddBaseType, cAndL.LinkID, name, linkType, desc, Component.eComponentType.Component, lastValidateAddParentXPath, lastSchemaValuesValidateAdd);

                if (thisMethodTurnedOff)
                {
                    this.TurnViewUpdateOn(false, false);
                }
                SendUpdateOfType(UpdateType.Component);
            }

            if (!ViewUpdateStatus && thisMethodTurnedOff)
            {
                this.TurnViewUpdateOn(false, false);
            }

            m_addComponentLinkType = null;

            return cAndL;
        }//AddComponent

        /// <summary>
        /// Adds and links a component to the database after a given position.  TopID is used for validation, this should be an ID that matches the first component
        /// in the schema found by following the passed linkType.  ParentID is the component to add to.  Type, desc, and name are used
        /// to create the component.  Most importantly, linkID specifies the linkID after which this component should be created.  This allows components to be created 
        /// the positionally in between others.
        /// </summary>
        /// <param name="topID"></param>
        /// <param name="parentID"></param>
        /// <param name="type"></param>
        /// <param name="linkID"></param>
        /// <param name="name"></param>
        /// <param name="linkType"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public ComponentAndLinkID AddComponent(int topID, int parentID, string type, int linkID, string name, string linkType, string desc)
        {
            CheckUseLoadingCaches();

            m_addComponentLinkType = linkType;

            bool thisMethodTurnedOff = false;
            if (ViewUpdateStatus)
            {
                this.TurnViewUpdateOff();
                thisMethodTurnedOff = true;
            }

            ComponentAndLinkID cAndL = this._AddComponent(topID, parentID, type, linkID, name, linkType, desc);

            if (cAndL != null)
            {
                cache.AddComponentToCache(this, m_model, topID, parentID, cAndL.ComponentID, lastValidateAddComponentType, lastValidateAddBaseType, cAndL.LinkID, name, linkType, desc, Component.eComponentType.Component, lastValidateAddParentXPath, lastSchemaValuesValidateAdd);

                if (thisMethodTurnedOff)
                {
                    this.TurnViewUpdateOn(false, false);
                }
                SendUpdateOfType(UpdateType.Component);
            }

            if (!ViewUpdateStatus && thisMethodTurnedOff)
            {
                this.TurnViewUpdateOn(false, false);
            }

            m_addComponentLinkType = null;

            return cAndL;
        }

        public IXPathNavigable GetComponent(int compID) {

            XmlElement topNode;
            XmlDocument doc = this._GetComponentsXmlDoc(out topNode);
            ComponentOptions compOptions = new ComponentOptions();
            XmlElement element = this._GetXmlTree(doc, this._GetComponent(compID), compOptions, null);

            if (element != null)
                topNode.AppendChild(element);

            return doc;

        }//GetComponent

        public IXPathNavigable GetLink(int linkID, bool addFromToNamesAndTypes)
        {
            return this._GetLink(linkID, addFromToNamesAndTypes);
        }//GetLink

        //returns new Component's ID
        public int CreateComponent(string type, string name, string desc)
        {
            CheckUseLoadingCaches();

            int componentID = this._CreateComponent(type, name, desc);

            if (componentID != -1)
            {
                SendUpdateOfType(UpdateType.Component);
            }

            return componentID;
        }//CreateComponent        

        public int CreateClass(string type, string name, string desc)
        {
            CheckUseLoadingCaches();

            int classID = this._CreateComponentClass(type, name, desc);

            if (classID != -1)
            {
                SendUpdateOfType(UpdateType.Component);
            }

            return classID;
        }//CreateClass   

        public ComponentAndLinkID AddComponentClass(int topID, int parentID, string type, string name, string linkType, string desc)
        {
            CheckUseLoadingCaches();

            m_addComponentLinkType = linkType;

            bool thisMethodTurnedOff = false;
            if (ViewUpdateStatus)
            {
                this.TurnViewUpdateOff();
                thisMethodTurnedOff = true;
            }

            ComponentAndLinkID cAndL = this._AddComponentClass(topID, parentID, type, name, linkType, desc);

            if (cAndL != null)
            {
                cache.AddComponentToCache(this, m_model, topID, parentID, cAndL.ComponentID, lastValidateAddComponentType, lastValidateAddBaseType, cAndL.LinkID, name, linkType, desc, Component.eComponentType.Class, lastValidateAddParentXPath, lastSchemaValuesValidateAdd);

                if (thisMethodTurnedOff)
                {
                    this.TurnViewUpdateOn(false, false);
                }
                SendUpdateOfType(UpdateType.Component);
            }//valid class ID

            if (!ViewUpdateStatus && thisMethodTurnedOff)
            {
                this.TurnViewUpdateOn(false, false);
            }

            m_addComponentLinkType = null;

            return cAndL;
        }//AddComponentClass

        public ComponentAndLinkID AddComponentInstance(int topID, int parentID, int classID, string name, string desc, string linkType)
        {
            CheckUseLoadingCaches();

            // use call that takes count param, does suffix append
            List<ComponentAndLinkID> instances = this._AddComponentInstances(topID, parentID, classID, name, linkType, desc, 1);

            if (instances.Count > 0)
            {
                SendUpdateOfType(UpdateType.Component);

                return instances[0];
            }
            else
            {
                return null;
            }
        }//AddComponentInstance
        public List<ComponentAndLinkID> AddComponentInstances(
            int topID, int parentID, int classID,
            string name, string linkType, string desc,
            int count)
        {
            CheckUseLoadingCaches();

            List<ComponentAndLinkID> instances = new List<ComponentAndLinkID>();

            try
            {
                instances = this._AddComponentInstances(
                    topID, parentID, classID,
                    name, linkType, desc, count);
            }
            catch (System.Exception e)
            {
                //Throwing the same exception because want to notify user with the message.
                throw e;
            }
            finally
            {
                //Doing refresh here if in case some instances got created successfully 
                //but after that others throw exception, still we can show the created components.
                SendUpdateOfType(UpdateType.Component);
            }

            return instances;
        }//AddComponentInstances

        public ComponentAndLinkID AddSubClass(int topID, int parentID, int sourceID, String nameForSubclass, String descriptionForSubclass, String linkType)
        {
            CheckUseLoadingCaches();

            return this._AddSubClass(topID, parentID, sourceID, nameForSubclass, descriptionForSubclass, linkType);
        }

        public bool UpdateComponentDescription(int compID, string value)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._UpdateComponentDescription(compID, value);

            if (bSuccess)
            {
                cache.UpdateComponentDescription(m_model, compID, value);

                SendUpdateOfType(UpdateType.Component);
            }

            return bSuccess;
        }//UpdateComponentDescription

        public void UpdateComponentName(int topID, int compID, string linkType, string newName)
        {
            CheckUseLoadingCaches();

            this._UpdateComponentName(topID, compID, linkType, newName);

            cache.UpdateComponentName(m_model, compID, newName);

            SendUpdateOfType(UpdateType.Component);
        }//UpdateComponentName

        public void UpdateComponentName(int compID, string newName)
        {
            CheckUseLoadingCaches();

            this._UpdateComponentName(compID, newName);

            cache.UpdateComponentName(m_model, compID, newName);

            SendUpdateOfType(UpdateType.Component);
        }//UpdateComponentName

        public bool DeleteComponent(int compID)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteComponentAndDisconnect(compID);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

            return bSuccess;
        }//DeleteComponent

        public bool DeleteComponentAndChildren(int compID)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteComponentAndGrandChildren(compID, false);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

            return bSuccess;
        }//DeleteComponentAndChildren

        public bool DeleteComponentAndChildren(int compID, bool reportProgress)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteComponentAndGrandChildren(compID, reportProgress);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

            return bSuccess;
        }//DeleteComponentAndChildren

        public bool DeleteComponentAndChildren(int compID, string linktype, List<String> ignoreTheseTypes)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteComponentAndGrandChildren(compID, linktype, ignoreTheseTypes);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

            return bSuccess;
        }//DeleteComponentAndChildren

        public bool DeleteComponentAndChildren(int compID, string linktype)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteComponentAndGrandChildren(compID, linktype, new List<String>()); // no ignore list

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

            return bSuccess;
        }//DeleteComponentAndChildren

        public Dictionary<String, String> GetParameterTableIndex()
        {
            DataTable pTable = this.GetParameterTable();
            Dictionary<String, String> index = new Dictionary<String, String>();
            StringBuilder keyBuilder;
            String key;
            foreach (DataRow pRow in pTable.Rows)
            {
                keyBuilder = new StringBuilder(3);
                keyBuilder.Append(pRow[SchemaConstants.ParentId].ToString());
                keyBuilder.Append(pRow[SchemaConstants.ParentType].ToString());
                keyBuilder.Append(pRow[SchemaConstants.Name].ToString());
                key = keyBuilder.ToString();
                index.Add(key, key);
            }
            return index;
        }

        public void DeleteParameter(int parentID, String parentType, String name, bool update)
        {
            CheckUseLoadingCaches();

            this.m_model.DeleteParameter(parentID, parentType, name);

            if (update)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }

        }

        public bool IsInheritedComponent(string compType)
        {
            return this.IsInheritedComponent(compType);
        }//IsInheritedComponent

        public string GetBaseComponentType(string compType)
        {
            return this._GetBaseComponentType(compType);
        }//GetBaseComponentType

        #endregion  //Components

        #region Links

        public string GetDynamicLinkType(String linkType, String componentName)
        {
            return GetDynamicLinkType(linkType, componentName, false);
        }

        public string GetDynamicLinkType(String linkType, String componentName, Boolean forImporter)
        {
            string seperator;
            
            if (forImporter)
            {
                seperator = ImportTool.Delimitter; // "_@_"
            }
            else
            {
                 seperator = XmlSchemaConstants.Config.Config_LinkType_Seperator; // "_"
            }

            // is the linkType already dynamic?
            if (linkType.Contains(seperator))
            {
                int indexOfSeperator = linkType.IndexOf(seperator);
                string justAfterSeperator = linkType.Substring(0, indexOfSeperator + 1);

                // combine and form dynamic
                return justAfterSeperator + componentName;
            }
            else  // combine and form dynamic
            {
                return linkType + seperator + componentName;
            }
        }

        public bool IsLinkTypeDynamic(String linkType)
        {
            return linkType.Contains(XmlSchemaConstants.Config.Config_LinkType_Seperator); // _
        }

        public int GetDynamicPivotFromLinkType(String linkType)
        {
            if (!IsLinkTypeDynamic(linkType))
            {
                return -1;
            }
            else
            {
                try
                {
                    string seperator = XmlSchemaConstants.Config.Config_LinkType_Seperator; // "_"
                    int indexOfSeperator = linkType.IndexOf(seperator);
                    indexOfSeperator++;
                    string justAfterSeperator = linkType.Substring(indexOfSeperator, linkType.Length - indexOfSeperator);
                    return Int32.Parse(justAfterSeperator);
                }
                catch 
                {
                    return -1;
                }
            }

        }

        public string GetBaseLinkType(String dynamicLinkType)
        {
            string seperator = XmlSchemaConstants.Config.Config_LinkType_Seperator; // "_"

            // is the linkType already dynamic?
            if (dynamicLinkType.Contains(seperator))
            {
                int indexOfSeperator = dynamicLinkType.IndexOf(seperator);
                return dynamicLinkType.Substring(0, indexOfSeperator);
            }
            else  // just return
            {
                return dynamicLinkType;
            }
        }

        public Boolean LinkExists(int fromID, int toID, String linkType)
        {
            //Checking if a link of same type does not exist between components.
            return this._GetChildComponentIDs(fromID, linkType).Contains(toID);
        }

        public int GetLinkID(int fromID, int toID, String linkType)
        {
            int lID = -1;
            DataTable link = m_model.GetLink(fromID, toID, linkType);
            if (link.Rows.Count > 0)
            {
                lID = Int32.Parse(link.Rows[0][SchemaConstants.Id].ToString());
            }
            return lID;
        }

        public DataRow GetLink(int fromID, int toID, String linkType)
        {
            DataTable link = m_model.GetLink(fromID, toID, linkType);
            if (link.Rows.Count > 0)
            {
                return link.Rows[0];
            }
            return null;
        }

        public int Connect(int topID, int parentID, int childID, string linkType)
        {
            CheckUseLoadingCaches();

            try {
                int linkID = this._Connect(topID, parentID, childID, linkType);

                if (linkID != -1) {
                    SendUpdateOfType(UpdateType.Component);
                }

                return linkID;
            }
            catch (Exception) {
                throw;
            }
        }//Conect

        public bool DeleteLink(int linkID)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteLink(linkID);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.Component);
            }

            return bSuccess;
        }//DeleteLink

        public bool DeleteLinks(String linkType)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteLinks(linkType);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.Component);
            }

            return bSuccess;
        }

        public bool DeleteLink(DataRow link)
        {
            CheckUseLoadingCaches();

            bool bSuccess = this._DeleteLink(link);

            if (bSuccess)
            {
                SendUpdateOfType(UpdateType.Component);
            }

            return bSuccess;
        }//DeleteLink

        public void IncrementLink(int linkID)
        {
            CheckUseLoadingCaches();

            this._IncrementLink(linkID);
        }

        public void DecrementLink(int linkID)
        {
            CheckUseLoadingCaches();

            this._DecrementLink(linkID);
        }

        #endregion  //Links

        #region Parameters

        public IXPathNavigable GetParametersForLink(int linkID)
        {
            return this._GetParametersForLink(linkID, null, null); // bypass cache
        }

        public IXPathNavigable GetParametersForLink(int linkID, String linkType, String fromType, String toType)
        {
            return this._GetParametersForLink(linkID, linkType, fromType, toType, null, null); // bypass cache
        }

        public IXPathNavigable GetParametersForComponent(int id)
        {
            return this._GetParametersForComponent(id, null, null); // bypass cache
        }

        public void PropagateParameters(int sourceId, int targetID)
        {
            CheckUseLoadingCaches();

            this._PropagateParameters(sourceId, targetID);
        }

        public void PropagateParameters(int sourceId, String targetLinkType)
        {
            CheckUseLoadingCaches();

            this._PropagateParameters(sourceId, targetLinkType);
        }
        public void UpdateParameters(int parentID, string paramName, string paramValue, eParamParentType paramParType)
        { 
            UpdateParameters( parentID,  paramName,  paramValue,  paramParType, true);
        }
        public void UpdateParameters(int parentID, string paramName, string paramValue, eParamParentType paramParType, bool sendUpdate)
        {
            CheckUseLoadingCaches();

            if (AllowNewData)
            {
                bool bSuccess = this._UpdateParameters(parentID, paramName, paramValue, paramParType);

                if (bSuccess && sendUpdate)
                {
                    SendUpdateOfType(UpdateType.Parameter, true, paramName);
                }
            }
        }
        public void UpdateParameters(int parentID, string paramName, byte[] paramValue, eParamParentType paramParType)
        {
            UpdateParameters(parentID, paramName, paramValue, paramParType, true);
        }
        public void UpdateParameters(int parentID, string paramName, byte[] paramValue, eParamParentType paramParType, bool sendUpdate)
        {
            CheckUseLoadingCaches();

            if (AllowNewData)
            {
                bool bSuccess = this._UpdateParameters(parentID, paramName, paramValue, paramParType);

                if (bSuccess && sendUpdate)
                {
                    SendUpdateOfType(UpdateType.Parameter, true, paramName);
                }
            }
        }

        #endregion  //Parameters

        #region AllowNewData

        private bool m_allowNewData = true;

        public bool AllowNewData
        {
            get { return this.m_allowNewData; }
            set { m_allowNewData = value; }
        }

        #endregion

        #region UpdateView

        private bool m_bViewUpdateStatus = true;

        public bool ViewUpdateStatus
        {
            get { return this.m_bViewUpdateStatus; }
        }//ViewUpdateStatus

        public void TurnViewUpdateOn()
        {
            this.m_bViewUpdateStatus = true;

            SendUpdateOfType(UpdateType.ComponentAndParameter);
        }//TurnViewUpdateOn

        public void TurnViewUpdateOn(bool componentUpdate, bool parameterUpdate)
        {
            this.m_bViewUpdateStatus = true;

            if (componentUpdate && parameterUpdate)
            {
                SendUpdateOfType(UpdateType.ComponentAndParameter);
            }
            else if (componentUpdate)
            {
                SendUpdateOfType(UpdateType.Component);
            }
            else if (parameterUpdate)
            {
                SendUpdateOfType(UpdateType.Parameter);
            }
        }//TurnViewUpdateOn

        public void TurnViewUpdateOff()
        {
            this.m_bViewUpdateStatus = false;
        }//TurnViewUpdateOff

        public void UpdateView()
        {
            SendUpdateOfType(UpdateType.ComponentAndParameter, false);
        }//UpdateView

        public void RegisterForUpdate(IViewComponent component)
        {
            if (!registeredForUpdates[m_model].Contains(component.IViewHelper))
            {
                registeredForUpdates[m_model].Add(component.IViewHelper);
            }
        }

        public void RegisterForUpdate(IViewComponent component, List<String> parameterCategories)
        {
            component.IViewHelper.ParameterCategories = parameterCategories;
            RegisterForUpdate(component);
        }

        public void UnregisterForUpdate(IViewComponent component)
        {
            if (registeredForUpdates[m_model].Contains(component.IViewHelper))
            {
                registeredForUpdates[m_model].Remove(component.IViewHelper);
            }
        }

        public void RegisterForUpdate(IViewComponentPanel panel)
        {
            if (!registeredForUpdates[m_model].Contains(panel.IViewHelper))
            {
                registeredForUpdates[m_model].Add(panel.IViewHelper);
            }
        }

        public void UnregisterForUpdate(IViewComponentPanel panel)
        {
            if (registeredForUpdates[m_model].Contains(panel.IViewHelper))
            {
                registeredForUpdates[m_model].Remove(panel.IViewHelper);
            }
        }

        public void ResetUpdateState()
        {
            foreach (IViewComponentHelper h in registeredForUpdates[m_model])
            {
                h.NeedsUpdate = true;
            }
        }

        protected void SendUpdateOfType(UpdateType type)
        {
            SendUpdateOfType(type, true, null);
        }

        protected void SendUpdateOfType(UpdateType type, bool dataChanged)
        {
            SendUpdateOfType(type, dataChanged, null);
        }

        protected void SendUpdateOfType(UpdateType type, bool dataChanged, String paramName)
        {
            String category = null, name, childField;
            if (paramName != null)
            {
                GetCategoryNameAndField(paramName, out category, out name, out childField);
            }

            if (m_bViewUpdateStatus)
            {
                foreach (IViewComponentHelper h in registeredForUpdates[m_model])
                {
                    if (dataChanged)
                    {
                        h.NeedsUpdate = true;
                    }

                    if (h.IViewHelperVisible && h.NeedsUpdate)
                    {
                        UpdateType cUpdateType = h.IViewHelperUpdateType;
                        if (
                           (type == UpdateType.Component && (cUpdateType == UpdateType.Component || cUpdateType == UpdateType.ComponentAndParameter))
                           ||
                           (type == UpdateType.Parameter && (cUpdateType == UpdateType.Parameter || cUpdateType == UpdateType.ComponentAndParameter))
                           ||
                           (type == UpdateType.ComponentAndParameter))
                        {
                            if (category != null && h.ParameterCategories.Count > 0)
                            {
                                if (h.ParameterCategories.Contains(category))
                                {
                                    if (!h.InvokeRequired)
                                    {
                                        h.IViewHelperUpdateViewComponent(type);
                                        h.NeedsUpdate = false;
                                    }
                                    else
                                    {
                                        h.InvokeUpdate(type);
                                        h.NeedsUpdate = false;
                                    }
                                }
                            }
                            else
                            {
                                if (!h.InvokeRequired)
                                {
                                    h.IViewHelperUpdateViewComponent(type);
                                    h.NeedsUpdate = false;
                                }
                                else
                                {
                                    h.InvokeUpdate(type);
                                    h.NeedsUpdate = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion  //UpdateView

        #region Images

        public Dictionary<string, Bitmap> GetIcons()
        {
            return this.m_model.GetComponentBitmaps(this.Configuration);
        }//GetIcons

        public Dictionary<string, Image> GetImages()
        {
            Dictionary<string, Image> images = new Dictionary<string, Image>();

            return images;
        }//GetImages

        #endregion  //Images

        #endregion

    }//Controller class
}//Controllers namespace
