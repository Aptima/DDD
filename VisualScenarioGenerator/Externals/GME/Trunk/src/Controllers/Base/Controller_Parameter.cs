using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Data;
using AME.Model;
using Collections;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using AME.Controllers.Base.DataStructures;

namespace AME.Controllers
{
    public partial class Controller
    {
        #region Helper

        // Given a Link ID, looks up the linkType for that ID
        // Also follows the fromID and toID for the link and retrieves their component types
        private bool GetLinkIDInfo(int linkID, out String linkType, out String fromType, out String toType)
        {
            linkType = "";
            fromType = "";
            toType = "";

            DataRow link = null;

            if (useLoadingCaches)
            {
                link = loadingLinkCache[m_model]["" + linkID];
            }
            else
            {

                DataTable links = this.m_model.GetLink(linkID);
                if (links.Rows.Count == 1)
                {
                    link = links.Rows[0];
                }
                else
                {
                    return false;
                }
            }

            linkType = link[SchemaConstants.Type].ToString();
            int fromID = Int32.Parse(link[SchemaConstants.From].ToString());
            int toID = Int32.Parse(link[SchemaConstants.To].ToString());

            return GetLinkIDInfo(fromID, toID, out fromType, out toType);
        }

        // Given two component IDs from a link (e.g. fromID, toID above) retrieves their component types
        private bool GetLinkIDInfo(int fromID, int toID, out String fromType, out String toType)
        {
            fromType = "";
            toType = "";

            DataRow fromRow, toRow;

            if (useLoadingCaches)
            {
                fromRow = loadingComponentCache[m_model]["" + fromID];
                toRow = loadingComponentCache[m_model]["" + toID];
            }
            else
            {
                fromRow = this._GetComponent(fromID);
                toRow = this._GetComponent(toID);
            }

            if (fromRow != null && toRow != null)
            {
                fromType = fromRow[SchemaConstants.Type].ToString();
                toType = toRow[SchemaConstants.Type].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        // Given a component ID, returns its componentType
        private String GetComponentType(int componentID)
        {
            DataRow component = this._GetComponent(componentID);
            if (component != null)
            {
                String componentType = component[SchemaConstants.Type].ToString();
                return componentType;
            }
            else
            {
                return "";
            }
        }

        public String GetComponentParameterValueFromXml(XPathNavigator parametersXML, String fullParameterName)
        {
            String parameterCategory, parameterName, childField;
            this.GetCategoryNameAndField(fullParameterName, out parameterCategory, out parameterName, out childField);
            String xpath = String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName);
            XPathNavigator parameterNode = parametersXML.SelectSingleNode(xpath);
            String value = parameterNode.GetAttribute(ConfigFileConstants.Value, "");
            return value;
        }

        // Given a combined parameter name (e.g. Location.X) pulls out the name and category
        private bool GetCategoryNameAndField(String combined, out String category, out String name, out String childField)
        {
            category = "";
            name = "";
            childField = "";

            int delimiterIndex = combined.IndexOf(SchemaConstants.ParameterDelimiter); // "."
            if (delimiterIndex > 0)
            {
                category = combined.Substring(0, delimiterIndex);  
                
                int startOfNameIndex = delimiterIndex + 1;
                name = combined.Substring(startOfNameIndex, combined.Length - startOfNameIndex);
                
                int fieldLeftIndex = name.IndexOf(SchemaConstants.FieldLeftDelimeter);
                if (fieldLeftIndex > 0)
                {
                    int startField = fieldLeftIndex + 1;
                    childField = name.Substring(startField, name.Length - startField - 1);
                    name = name.Substring(0, fieldLeftIndex);
                }
                return true;
            }
            else 
            {
                return false;
            }
        }

        #endregion //Helper

        #region Config Read

        private List<NameValuePair> GetNameValuesFromBinary(byte[] paramValue)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(paramValue);
            try
            {
                object output = formatter.Deserialize(memoryStream);
                Array arrayCast = (Array)output;
                List<NameValuePair> nameValues = new List<NameValuePair>();
                for (int i = 0; i < arrayCast.Length; i++)
                {
                    nameValues.Add(new NameValuePair("", arrayCast.GetValue(i).ToString()));
                }
                return nameValues;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private void ProcessParameterDataRow(DataRow parameterRow, ref Dictionary<String, String> parameterNameValues, ref Dictionary<String, List<NameValuePair>> parameterArrayNameValues)
        {
            String name = parameterRow[SchemaConstants.Name].ToString();
            String value = parameterRow[SchemaConstants.Value].ToString();

            String test = parameterRow[SchemaConstants.BinaryValue].ToString();
            if (test.Length > 0) // collection
            {
                byte[] paramValue = (byte[])parameterRow[SchemaConstants.BinaryValue];

                if (paramValue != null && paramValue.Length > 0)
                {
                    List<NameValuePair> nameValues = GetNameValuesFromBinary(paramValue);

                    if (!parameterArrayNameValues.ContainsKey(name))
                    {
                        parameterArrayNameValues.Add(name, nameValues);
                    }
                }
            }
            else
            {
                parameterNameValues.Add(name, value); // non collection
            }
        }

        // Given a component id, pulls out the component type and redirects to subroutine.
        // If a caller already knows the type, they can save a little work by calling it directly
        private IXPathNavigable _GetParametersForComponent(int componentID, 
            Dictionary<String, XmlNode> typeToParameterXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            String key = "" + componentID;

            if (componentParametersXMLCache[m_model].ContainsKey(key))
            {
                return componentParametersXMLCache[m_model][key];
            }

            XmlDocument toView = new XmlDocument();

            DataRow component = this._GetComponent(componentID);
            if (component != null)
            {
                return _GetParametersForComponent(component, typeToParameterXMLCache, parameterTableCache);
            }
            else
            {
                return toView;
            }
        }
      
        // Given a link id, pulls out the linktype, from, and to types and redirects to subroutine.
        // If a caller already knows these values, they can save a little work by calling calling it directly
        private IXPathNavigable _GetParametersForLink(int linkID, 
            Dictionary<String, XmlNode> typeToParameterXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            String xmlCacheKey = "" + linkID;
            if (linkParametersXMLCache[m_model].ContainsKey(xmlCacheKey))
            {
                return linkParametersXMLCache[m_model][xmlCacheKey];
            }

            XmlDocument toView = new XmlDocument();
           
            String linkType, fromType, toType;
            bool success = this.GetLinkIDInfo(linkID, out linkType, out fromType, out toType);

            if (success)
            {
                return _GetParametersForLink(linkID, linkType, fromType, toType, typeToParameterXMLCache, parameterTableCache);
            }
            else
            {
                return toView;
            }
        }
            
        // Read link parameter xml from the config file
        private IXPathNavigable _GetParametersForLink(int linkID, String linkType, String fromType, String toType,
            Dictionary<String, XmlNode> typeToParameterXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            String xmlCacheKey = "" + linkID;
            if (linkParametersXMLCache[m_model].ContainsKey(xmlCacheKey))
            {
                return linkParametersXMLCache[m_model][xmlCacheKey];
            }

            linkType = this.GetBaseLinkType(linkType);

            XmlNode complex = null;

            StringBuilder forKey = new StringBuilder();
            forKey.Append(this.Configuration);
            forKey.Append(linkType);
            forKey.Append(fromType);
            forKey.Append(toType);
            String key = forKey.ToString();

            if (typeToParameterXMLCache != null && typeToParameterXMLCache.ContainsKey(key))
            {
                complex = typeToParameterXMLCache[key];
            }
            else
            {
                complex = this.m_model.GetParametersXML(linkType, fromType, toType);
                if (typeToParameterXMLCache != null)
                {
                    typeToParameterXMLCache.Add(key, complex);
                }
            }

            XmlDocument returnDocument = ProcessParameters(complex, linkID, Component.eComponentType.None, eParamParentType.Link, parameterTableCache);

            XmlDocumentFragment fragment = returnDocument.CreateDocumentFragment();
            fragment.InnerXml = returnDocument.InnerXml;
            linkParametersXMLCache[m_model].Add("" + linkID, fragment);

            return returnDocument;
        }

        private IXPathNavigable _GetParametersForComponent(DataRow component,
            Dictionary<String, XmlNode> typeToParameterXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            int id = (int)component[SchemaConstants.Id];
            String type = (string)component[SchemaConstants.Type];
            Component.eComponentType eType = this._GetComponentType(component);

            return this._GetParametersForComponent(id, type, eType, typeToParameterXMLCache, parameterTableCache);
        }

        private IXPathNavigable _GetParametersForComponent(int componentID, String componentType, Component.eComponentType eType,
            Dictionary<String, XmlNode> typeToParameterXMLCache, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            String key = "" + componentID;

            if (componentParametersXMLCache[m_model].ContainsKey(key))
            {
                return componentParametersXMLCache[m_model][key];
            }

            XmlNode complex = null;

            if (typeToParameterXMLCache != null && typeToParameterXMLCache.ContainsKey(componentType))
            {
                complex = typeToParameterXMLCache[componentType];
            }
            else
            {
                complex = this.m_model.GetParametersXML(componentType);
                if (typeToParameterXMLCache != null)
                {
                    typeToParameterXMLCache.Add(componentType, complex);
                }
            }

            XmlDocument returnDocument = ProcessParameters(complex, componentID, eType, eParamParentType.Component, parameterTableCache);

            XmlDocumentFragment fragment = returnDocument.CreateDocumentFragment();
            fragment.InnerXml = returnDocument.InnerXml;
            componentParametersXMLCache[m_model].Add(key, fragment);

            return returnDocument;
        }

        public List<XmlElement> GetNameValuesParameterXML(XmlDocument sourceDoc, List<NameValuePair> nameValues)
        {
            List<XmlElement> nameValuesXML = new List<XmlElement>();

            foreach (NameValuePair nameValue in nameValues)
            {
                XmlElement collectionParameter = sourceDoc.CreateElement(XmlSchemaConstants.Display.sParameter);

                if (nameValue.Name != null && nameValue.Name.Length > 0)
                {
                    XmlAttribute nameAttribute = sourceDoc.CreateAttribute(ConfigFileConstants.Name);
                    nameAttribute.Value = nameValue.Name;
                    collectionParameter.Attributes.Append(nameAttribute);
                }

                XmlAttribute valueAttribute = sourceDoc.CreateAttribute(ConfigFileConstants.Value);
                valueAttribute.Value = nameValue.Value;
                collectionParameter.Attributes.Append(valueAttribute);

                nameValuesXML.Add(collectionParameter);
            }
            return nameValuesXML;
        }


        private XmlNode ProcessChildParameterValue(XmlNode self, String name, Dictionary<String, String> parameterNameValues,
                                            Dictionary<String, List<NameValuePair>> parameterArrayNameValues)
        {
            String value;
            if (!parameterArrayNameValues.ContainsKey(name)) // parameter is not a collection
            {
                if (parameterNameValues.ContainsKey(name))
                {
                    value = parameterNameValues[name]; // default xml values will go into the DB
                }
                else
                {
                    value = String.Empty; // otherwise, use empty string
                }

                XmlAttribute valueAttr = self.Attributes[ConfigFileConstants.Value];

                if (valueAttr == null) // optional in configuration.xml, so may not be present, add it.
                {
                    valueAttr = self.OwnerDocument.CreateAttribute(ConfigFileConstants.Value);
                    self.Attributes.Append(valueAttr);
                }

                self.Attributes[ConfigFileConstants.Value].Value = value; // set value from DB
            }
            else // parameter is a collection type
            {
                List<NameValuePair> arrayValues = parameterArrayNameValues[name];
                List<XmlElement> parameterNameValuesXML = GetNameValuesParameterXML(self.OwnerDocument, arrayValues);
                foreach (XmlElement xmlNameValue in parameterNameValuesXML)
                {
                    self.AppendChild(xmlNameValue);
                }
            }
            return self;
        }
        
        
        // Process parameters the same way for link and component
        // Node looks like '/Parameters'
        private XmlDocument ProcessParameters(XmlNode parameters, int componentOrLinkID, Component.eComponentType eType, 
            eParamParentType paramParType, Dictionary<String, List<DataRow>> parameterTableCache)
        {
            bool IDisClass = eType == Component.eComponentType.Class;
            bool IDisSubclass = eType == Component.eComponentType.Subclass;

            // prepare values from DB
            Dictionary<String, String> parameterNameValues = new Dictionary<String, String>();
            Dictionary<String, List<NameValuePair>> parameterArrayNameValues = new Dictionary<String, List<NameValuePair>>();
            if (parameterTableCache != null)
            {
                String key = componentOrLinkID+paramParType.ToString();
                if (parameterTableCache.ContainsKey(key))
                {
                    List<DataRow> parameterRows = parameterTableCache[key];

                    parameterNameValues = new Dictionary<String, String>();

                    foreach (DataRow parameterRow in parameterRows)
                    {
                        ProcessParameterDataRow(parameterRow, ref parameterNameValues, ref parameterArrayNameValues);
                    }
                }
                else
                {
                    parameterNameValues = new Dictionary<String, String>();
                }
            }
            else
            {
                DataTable parameterTable = this.m_model.GetParameterTable(componentOrLinkID, paramParType.ToString());

                foreach (DataRow parameterRow in parameterTable.Rows)
                {
                    ProcessParameterDataRow(parameterRow, ref parameterNameValues, ref parameterArrayNameValues);
                }
            }

            // prepare XML document for view
            XmlDocument toView = new XmlDocument();
            XmlElement parametersNodeForView = null;

            if (paramParType == eParamParentType.Component)
            {
                parametersNodeForView = toView.CreateElement(XmlSchemaConstants.Display.sComponentParameters); // ComponentParameters
            }
            else if (paramParType == eParamParentType.Link)
            {
                parametersNodeForView = toView.CreateElement(XmlSchemaConstants.Display.sLinkParameters);  // LinkParameters
            }

            XmlNode parametersRoot = toView.AppendChild(parametersNodeForView);

            if (parameters != null) 
            {
                // create Parameters with type 'Complex', these will hold all the parameters for a category.
                List<String> categories = new List<String>();

                XmlNodeList allCategories = parameters.SelectNodes(XmlSchemaConstants.Display.sParameter);
                foreach (XmlNode categoryNode in allCategories) // collect all categories
                {
                    String category = categoryNode.Attributes[ConfigFileConstants.category].Value;
                    if (!categories.Contains(category))
                    {
                        categories.Add(category);
                    }
                }

                foreach (String category in categories) // go through categories, build parent node, collect children
                {
                    XmlElement newComplex = toView.CreateElement("Parameter");

                    XmlAttribute categoryAttributeForNewComplex = toView.CreateAttribute(ConfigFileConstants.category);
                    categoryAttributeForNewComplex.Value = category; 

                    XmlAttribute typeAttributeForNewComplex = toView.CreateAttribute(ConfigFileConstants.Type);
                    typeAttributeForNewComplex.Value = ConfigFileConstants.complexType; // type="Complex"

                    newComplex.SetAttributeNode(categoryAttributeForNewComplex);
                    newComplex.SetAttributeNode(typeAttributeForNewComplex);

                    XmlNodeList complexChildren = parameters.SelectNodes("Parameter[@category='" + category + "']"); // go through children...

                    foreach (XmlNode complexChild in complexChildren)
                    {
                        String name = complexChild.Attributes[ConfigFileConstants.displayedName].Value;
                        String combined = category + SchemaConstants.ParameterDelimiter + name;  // category.name

                        XmlNode copy = complexChild.CloneNode(true);

                        // check browsable attribute
                        XmlAttribute browseAttr = copy.Attributes[ConfigFileConstants.browsable];
                        if (browseAttr == null)
                        {
                            browseAttr = copy.OwnerDocument.CreateAttribute(ConfigFileConstants.browsable);
                            browseAttr.Value = "true";
                            copy.Attributes.Append(browseAttr);
                        }

                        // check classOnly - 
                        // if parameter is specified as class only: 
                        // For a class, set browsable to true, otherwise false.
                        // otherwise, use browse attribute value
                        XmlAttribute classAttr = copy.Attributes[ConfigFileConstants.classOnly];
                        if (classAttr != null)
                        {
                            String classAttrValue = classAttr.Value;
                            if (classAttrValue.Equals("true"))
                            {
                                if (IDisClass)
                                {
                                    browseAttr.Value = "true";
                                }
                                else if (IDisSubclass && combined != Component.Class.InstancesUseClassName.Name) // don't show InstancesUseClassName name for subclasses
                                {
                                    browseAttr.Value = "true";
                                }
                                else
                                {
                                    browseAttr.Value = "false";
                                }
                            }
                        }

                        copy = ProcessChildParameterValue(copy, combined, parameterNameValues, parameterArrayNameValues);

                        // check for struct children
                        XmlNodeList structChildren = copy.SelectNodes("Parameters/Parameter");
                        for (int i = 0; i < structChildren.Count; i++)
                        {
                            XmlNode structChild = structChildren[i];
                            String fieldName = structChild.Attributes[ConfigFileConstants.Name].Value;
                            String structName = combined + SchemaConstants.FieldLeftDelimeter + fieldName + SchemaConstants.FieldRightDelimeter;

                            structChild = ProcessChildParameterValue(structChild, structName, parameterNameValues, parameterArrayNameValues);
                        }

                        newComplex.AppendChild(toView.ImportNode(copy, true));
                    } // parameter loop

                    // sanity check: if complex has no children, don't add it (they might have all been empty IgnoreEmptyString is true)
                    if (newComplex.HasChildNodes)
                    {
                        parametersRoot.AppendChild(newComplex); // append complex node to root
                    }
                } // complex loop
            }
            return toView;
        }

        #endregion // Config Read

        #region Create/Update Parameters

        // validates type, checks constraints, and updates parameter values for both components and links
        private bool Check_Type_Constraints_And_Update(int parentID, string paramName, string paramValue, eParamParentType parentType)
        {
            string paramMin = null;
            string paramMax = null;
            Type cSharpType;
            String linkType = "";
            String fromType = "";
            String toType = ""; 
            String componentType = "";

            // intialize anything we need...
            if (parentType == eParamParentType.Link)
            {
                GetLinkIDInfo(parentID, out linkType, out fromType, out toType);
            }
            else if (parentType == eParamParentType.Component)
            {
                componentType = GetComponentType(parentID);
            }

            // category.name -> category, name
            String name, category, childField;
            String parameterTypeConverter = "";
            if (GetCategoryNameAndField(paramName, out category, out name, out childField))
            {
                // convert string type to C# Type
                String parameterType = "";
           
                if (parentType == eParamParentType.Link) // (XML is in different places in config file)
                {
                    linkType = GetBaseLinkType(linkType);
                    parameterType = this.m_model.GetParameterType(linkType, fromType, toType, category, name, childField);
                    parameterTypeConverter = this.m_model.GetParameterTypeConverter(linkType, fromType, toType, category, name, childField);

                }
                else if (parentType == eParamParentType.Component)
                {
                    parameterType = this.m_model.GetParameterType(componentType, category, name, childField);
                    parameterTypeConverter = this.m_model.GetParameterTypeConverter(componentType, category, name, childField);
                }

                cSharpType = AMEManager.GetType(parameterType, componentType); 

                // Look up constraint 
                ParamConstraint paramConstraint = GetConstraint(category, name, childField, parentType, componentType, linkType, fromType, toType);

                if (paramConstraint != null)
                {
                    paramMin = paramConstraint.Range.Min;
                    paramMax = paramConstraint.Range.Max;
                }
            }
            else
            {
                throw new Exception("Could not find parameter category and name from: " + paramName);
            }

            //For valid type conversion will go fine, else will throw exception.
            object oValidatedValue = this._ValidateValue(cSharpType, paramValue, paramMin, paramMax, parameterTypeConverter);
            string sParamValueToStore = (oValidatedValue == null) ? paramValue : oValidatedValue.ToString();

            // now update parameter's value
            updateParameter(parentID, parentType, paramName, sParamValueToStore, category, name, childField);
         
            return true;
        }

        private XmlNode FindStructureInCache(String key, String category, String name, String childField, eParamParentType parentType)
        {
            Dictionary<AME.Model.Model, Dictionary<String, XmlDocumentFragment>> cache;

            if (parentType == eParamParentType.Component)
            {
                cache = componentParametersXMLCache;
            }
            else
            {
                cache = linkParametersXMLCache;
            }

            if (cache[m_model].ContainsKey(key))
            {
                XmlNodeList categoryChildren = cache[m_model][key].FirstChild.ChildNodes;
                XmlAttribute categoryAttr;

                foreach (XmlNode categoryChild in categoryChildren)
                {
                    categoryAttr = categoryChild.Attributes["category"];
                    if (categoryAttr.Value.Equals(category))
                    {
                        XmlNodeList children = categoryChild.ChildNodes;
                        foreach (XmlNode child in children)
                        {
                            if (child.Attributes["displayedName"].Value.Equals(name))
                            {
                                if (childField.Length == 0)
                                {
                                    XmlAttribute value = child.Attributes["value"];
                                    if (value != null)
                                    {
                                        return child;
                                    }
                                }
                                else
                                {
                                    XmlNodeList structChildren = child.FirstChild.ChildNodes;
                                    foreach (XmlNode structChild in structChildren)
                                    {
                                        if (structChild.Attributes["name"].Value.Equals(childField))
                                        {
                                            XmlAttribute value = structChild.Attributes["value"];
                                            if (value != null)
                                            {
                                                return structChild;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void UpdateCacheWithValue(String key, String category, String name, String childField, String value, eParamParentType parentType)
        {
            XmlNode find = FindStructureInCache(key, category, name, childField, parentType);
            if (find != null)
            {
                find.Attributes["value"].Value = value;
            }
        }

        private string GetCacheValue(String key, String category, String name, String childField, eParamParentType parentType)
        {
            XmlNode find = FindStructureInCache(key, category, name, childField, parentType);
            if (find != null)
            {
                return find.Attributes["value"].Value;
            }
            else
            {
                return null;
            }
        }

        // propogate a single source to a single target
        public void _PropagateParameters(int sourceID, int target)
        {
            PropagateParameters(sourceID, "", target);
        }

        // propogate parameters down to subclasses, instances, etc.
        // use class id and subclass link type
        // or (class id or subclass id) and instance linktype
        public void _PropagateParameters(int sourceID, String linkTypeForTargets)
        {
            PropagateParameters(sourceID, linkTypeForTargets, -1);
        }

        private void PropagateParameters(int sourceID, String linkTypeForTargets, int target)
        {
            DataRow source = this._GetComponent(sourceID);

            if (source != null)
            {
                String sourceName = source[SchemaConstants.Type].ToString();

                int childID = -1;
                DataRowCollection childRows = null;

                if (linkTypeForTargets != null && linkTypeForTargets.Length > 0)
                {
                    //get all of the children from source following linktype, and update their parameters
                    DataTable children = this._GetChildComponents(sourceID, linkTypeForTargets);
                    childRows = children.Rows;

                    if (childRows.Count > 0)
                    {
                        childID = Int32.Parse(childRows[0][SchemaConstants.Id].ToString());
                    }
                }
                else if (target != -1)
                {
                    childID = target;
                }

                String childKey = "" + childID;

                // Get child XML (once)
                XPathNavigator nav = GetParametersForComponent(childID).CreateNavigator();

                // get source parameters
                DataTable sourceComponentParameters = this.m_model.GetParameterTable(sourceID, eParamParentType.Component.ToString());
                foreach (DataRow sourceParam in sourceComponentParameters.Rows)
                {
                    string paramName = (string)sourceParam[SchemaConstants.Name];
                    string paramValue = (string)sourceParam[SchemaConstants.Value];

                    // category.name -> category, name
                    String name, category, childField;
                    if (GetCategoryNameAndField(paramName, out category, out name, out childField))
                    {
                        XPathNodeIterator findInChildren = nav.Select(String.Format("ComponentParameters/Parameter[@type='{0}']/Parameter[@displayedName='{1}'][@category='{2}']", ConfigFileConstants.complexType, name, category));
                        if (findInChildren.Count > 0)
                        {
                            findInChildren.MoveNext();

                            String browsable = findInChildren.Current.GetAttribute(ConfigFileConstants.browsable, findInChildren.Current.NamespaceURI);
                            if (browsable != null && browsable.Length > 0)
                            {
                                bool stringToBool = Boolean.Parse(browsable);

                                if (stringToBool)
                                {
                                    if (childRows != null)
                                    {
                                        foreach (DataRow child in childRows)
                                        {
                                            int fromDR = Int32.Parse(child[SchemaConstants.Id].ToString());
                                            this.m_model.CreateParameter(fromDR, eParamParentType.Component.ToString(), paramName, paramValue, "");

                                            String key = "" + fromDR;

                                            // update cache from propogation
                                            UpdateCacheWithValue(key, category, name, childField, paramValue, eParamParentType.Component);
                                            //cache.UpdateParameters(m_model, fromDR, name, category, paramValue, eParamParentType.Component);
                                        }
                                    }
                                    else if (target != -1)
                                    {
                                        this.m_model.CreateParameter(childID, eParamParentType.Component.ToString(), paramName, paramValue, "");

                                        // update cache from propogation
                                        UpdateCacheWithValue(childKey, category, name, childField, paramValue, eParamParentType.Component);
                                        //cache.UpdateParameters(m_model, childID, name, category, paramValue, eParamParentType.Component);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Get a constraint for a component or link
        public ParamConstraint GetConstraint(String category, String name, String childField, eParamParentType componentOrLink, String componentType,
            String linkType, String fromType, String toType)
        {
            XPathNavigator targetParameter = null; // get the parameter node

            if (componentOrLink == eParamParentType.Component) // (XML is in different places in config file)
            {
                targetParameter = this.m_model.GetParameter(componentType, category, name, childField);
            }
            else if (componentOrLink == eParamParentType.Link)
            {
                targetParameter = this.m_model.GetParameter(linkType, fromType, toType, category, name, childField);
            }

            if (targetParameter != null) // populate constraints object if any are provided for parameter
            {
                XPathNodeIterator constraints = targetParameter.Select("Constraints/Constraint");

                if (constraints.Count > 0)
                {
                    ParamConstraint paramConstraint = new ParamConstraint();

                    foreach (XPathNavigator constraint in constraints)
                    {
                        string sConstraintName = constraint.GetAttribute(ConfigFileConstants.constraintName, constraint.NamespaceURI);
                        string sConstraintValue = constraint.GetAttribute(ConfigFileConstants.constraintValue, constraint.NamespaceURI);

                            switch (sConstraintName.ToLower())
                            {
                                case ConfigFileConstants.minConstraint: // "min"
                                    paramConstraint.Range.Min = sConstraintValue;
                                    break;
                                case ConfigFileConstants.maxConstraint: // "max"
                                    paramConstraint.Range.Max = sConstraintValue;
                                    break;
                            }
                        }
                        return paramConstraint;
                    }
                }
            return null;
        }

        // Update parameters for a link or component
        private bool _UpdateParameters(int parentID, string paramName, string paramValue, eParamParentType paramParType)
        {
            //Component.eComponentType eComponentType = _GetComponentType(parentID);

            // check type, constraints, and update
            return Check_Type_Constraints_And_Update(parentID, paramName, paramValue, paramParType);
        }

        private bool _UpdateParameters(int parentID, string paramName, byte[] paramValue, eParamParentType paramParType)
        {
            this.m_model.CreateParameter(parentID,
                paramParType.ToString(),
                paramName, paramValue, "");

            // update fragment
            String key = "" + parentID;
            String name, category, childField;
            GetCategoryNameAndField(paramName, out category, out name, out childField);
            XmlNode find = FindStructureInCache(key, category, name, childField, paramParType);

            if (find == null)
            {
                Console.WriteLine("Couldn't find XML cache node: " + paramName);
                return false;
            }

            List<NameValuePair> nameValues = GetNameValuesFromBinary(paramValue);
            List<XmlElement> newChildren = GetNameValuesParameterXML(find.OwnerDocument, nameValues);

            XmlNodeList oldChildren = find.ChildNodes;
            for (int i = (oldChildren.Count - 1); i >= 0; i--)
            {
                find.RemoveChild(oldChildren[i]);
            }

            foreach (XmlNode newChild in newChildren)
            {
                find.AppendChild(newChild);
            }

            // category.name -> category, name
            cache.UpdateParameters(m_model, parentID, category, name, childField, newChildren, paramParType);

            return true;
        }

        // creating defaults for a component
        private bool Create_Component_DefaultParameters(int compID, string componentType)
        {
            // different xml location, same processing
            XmlNode complex = this.m_model.GetParametersXML(componentType);
            return CreateDefaults(complex, compID, eParamParentType.Component, componentType, "", "", "");
        }

        // creating defaults for a link
        private bool Create_Link_DefaultParameters(int linkID, String linkType, int fromID, int toID)
        {
            String fromType, toType;

            bool getFromTo = this.GetLinkIDInfo(fromID, toID, out fromType, out toType);
            if (getFromTo)
            {
                // different xml location, same processing
                linkType = this.GetBaseLinkType(linkType);
                XmlNode complex = this.m_model.GetParametersXML(linkType, fromType, toType);
                if (complex != null)
                {
                    return CreateDefaults(complex, linkID, eParamParentType.Link, linkType + fromType + toType, linkType, fromType, toType);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Create defaults for a link or a component
        private bool CreateDefaults(XmlNode parameters, int componentOrLinkID, eParamParentType componentOrLink, String componentType,
            String linkType, String fromType, String toType)
        {
            if (parameters != null)
            {
                List<ParameterInfo> infoList = GetParameterInfoList(parameters, componentType, linkType, fromType, toType, componentOrLink);
                foreach (ParameterInfo info in infoList)
                {
                    if (info.BinaryData != null)
                    {
                        this.UpdateParameters(componentOrLinkID, info.Combined, info.BinaryData, componentOrLink);
                    }
                    else
                    {
                        updateParameter(componentOrLinkID, componentOrLink, info.Combined, info.Value, info.Category, info.Name, info.Field);
                    }
                }
            }
            return true;
        }

        private void updateParameter(int parentID, eParamParentType parentType, String paramName, String value,
            String category, String name, String childField)
        {
            // now update parameter's value
            this.m_model.CreateParameter(parentID,
                parentType.ToString(),
                paramName, value, "");

            String key = "" + parentID;

            UpdateCacheWithValue(key, category, name, childField, value, parentType);
            cache.UpdateParameters(m_model, parentID, category, name, childField, value, parentType);
        }

        #endregion // Create/Update Parameters
    }
}
