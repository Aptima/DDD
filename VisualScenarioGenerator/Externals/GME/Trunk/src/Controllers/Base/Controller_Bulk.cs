using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers.Base.DataStructures;
using System.IO;
using System.Data;
using AME.Model;
using System.Xml;
using AME.Controllers.Base.Data_Structures;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace AME.Controllers
{
    /* This class handles logic for bulk inserts into the database.  This class also handles some of the delayed
     * validation functionality.  The general use for this class should look something like this:
     * 
     * UseDelayedValidation = true;
     * 
     * BulkCreate
     * BulkLinks
     * BulkParameters
     * 
     * UseDelayedValidation = false;
     * 
     * The topIDs and linktypes inserted during the BulkLink creation will be validated after setting UseDelayedValidation to false.  
     * Regular Connects will also work, but BulkLinks is recommended for large inserts.  
     * 
     * The Bulk inserts will check UNIQUE, NOT NULL, and PRIMARY KEY constraints.  If any of these fail, the bulk transaction will fail.
     * A common example of this would be a component creating default parameters (in BulkCreate) which are then updated by BulkParameters.
     * To get around this, BulkCreate includes the option to specify the 'Holder-Names' (from IImportTool) of components that are going
     * to have parameters created for them later.  Default values for these parameters will be skipped.
     * 
     * Author:  Mark Weston
     */
    public partial class Controller
    {
        // stores top IDs and link types from links formed (to be checked by delayed validation)
        private Dictionary<String, TopIDAndLinkType> storedValidationTopIDsAndLinkTypes = new Dictionary<String, TopIDAndLinkType>();

        // a component table cache used during delayed validation (indexed by ID)
        private Dictionary<String, DataRow> delayedValidationComponentTableCache;

        // a copy of the component table used during delayed validation
        private DataTable delayedValidationComponentTable;

        // dictionary of a dictionary that maps an ID to a list of DataRows (read this as mapping fromID -> toID rows for quick child lookups)
        // These are specific caches of linktypes.  The first dictionary is indexed by linktype.
        private Dictionary<String, Dictionary<String, List<DataRow>>> delayedValidationLinkTableCache;

        // caches ParameterValidationInfo
        // String is a 'key', for components this is the type + combined name of the parameter
        // for links it is the configuration + linkType + fromType + toType + combined name 
        // the keys are constructed this way to uniquely identify the parameter
        // validation info includes the Type and constraint Min and Max
        // This is constructed during BulkCreate and BulkLinks
        // When BulkParameters is called later on, we've already indexed everything we need to quickly validate any parameter values
        // we're going to insert
        private Dictionary<String, ParameterValidationInfo> parameterValidationInfo = new Dictionary<String, ParameterValidationInfo>();

        private Boolean m_useDelayedValidation = false;

        // for bulk File IO
        private String bulkFile;
        private FileInfo bulkInfo;
        private FileStream write;
        private StreamWriter streamWrite;

        // setup writing stream
        private void SetupBulkFileIO(String filename)
        {
            bulkFile = Path.Combine(m_model.ImportPath, filename);
            bulkInfo = new FileInfo(bulkFile);
            if (bulkInfo.Exists)
            {
                bulkInfo.Delete();
            }
            write = bulkInfo.OpenWrite();
            streamWrite = new StreamWriter(write);
            streamWrite.AutoFlush = true;
        }

        // called when setting UseDelayedValidation from true to false
        private void DelayedValidate(Dictionary<String, TopIDAndLinkType> storedTopIDsAndLinkTypes)
        {
            delayedValidationComponentTable = null;
            delayedValidationComponentTableCache = null;
            delayedValidationLinkTableCache = null;

            this.EnableLoadingCache(); // all reading - cache tables

            // validate all top ids and link types
            ComponentOptions options = new ComponentOptions();
            foreach (TopIDAndLinkType tidlt in storedValidationTopIDsAndLinkTypes.Values)
            {
                XmlDocument doc = (XmlDocument)this.GetComponentAndChildren(tidlt.TopID, tidlt.TopID, tidlt.LinkType, options);
                
                // we're done, save some work by updating the cache here with the validated doc
                String key1 = cache.CreateCacheKey(tidlt.TopID, tidlt.TopID, tidlt.LinkType);
                String key2 = cache.CreateCacheKey(tidlt.TopID, tidlt.LinkType);
                if (m_addComponentLinkType != null)
                {
                    if (m_addComponentLinkType != tidlt.LinkType)
                    {
                        cache.AddCacheDocument(m_model, key1, doc);
                        cache.AddCacheDocument(m_model, key2, (XmlDocument)doc.Clone());
                    }
                }
                else
                {
                    cache.AddCacheDocument(m_model, key1, doc);
                    cache.AddCacheDocument(m_model, key2, (XmlDocument)doc.Clone());
                }
            }

            this.DisableLoadingCache();

            this.storedValidationTopIDsAndLinkTypes.Clear();
        }

        /// <summary>
        /// Bulk inserts the specified ComponentInfos.  The Dictionary parameter is optional, but if used should contain
        /// the 'HolderName+Combined ParameterName' of the ComponentInfos that will have parameters created for them later (e.g. in BulkParameters).  
        /// BulkCreate will skip creating default parameters for those parameters.
        /// </summary>
        /// <param name="bulkComponentInfo"></param>
        /// <param name="namesWithParameters"></param>
        /// <returns></returns>
        public List<Int32> BulkCreateComponents(List<ComponentInfo> bulkComponentInfo, Dictionary<String, String> namesWithParameters)
        {
            Int32 i = 0;

            CheckUseLoadingCaches();

            parameterValidationInfo.Clear();

            if (bulkComponentInfo.Count == 0)
            {
                DataTable retcomponents = this.GetComponentTable();
                Dictionary<int, int> retskipIDs = GetSkipIds(retcomponents);
                BulkCreateDefaultParameters(retcomponents, eParamParentType.Component, retskipIDs, namesWithParameters, bulkComponentInfo, new List<LinkInfo>());
                return new List<Int32>();
            }

            SetupBulkFileIO("BulkComponents.txt");

            // skip components already in the component table (so we can track the IDs created)
            DataTable components = this.GetComponentTable();
            Dictionary<int, int> skipIDs = GetSkipIds(components);

            // write out the bulk file, tab delimited
            int rowCount = 0;
    
            foreach (ComponentInfo cInfo in bulkComponentInfo)
            {
                if (cInfo.Description.Equals(""))
                {
                    cInfo.Description = "\0"; // use null -> sql interprets as empty string
                }
                streamWrite.WriteLine(Convert.ToString(i++) + "\t" + cInfo.Type + "\t" + cInfo.Name + "\t" + cInfo.Description + "\t" + cInfo.EType);
                rowCount++;
            }

            streamWrite.Close();
            write.Close();

            String formatFile = "ComponentTableFormat.fmt";
            this.m_model.BulkCreate("ComponentTable", bulkInfo, formatFile, rowCount);

            SetupBulkFileIO("BulkDefaultParameters.txt");

            // store bulk default parameters by type
            // combine with the IDs created by the bulk component create
            components = this.GetComponentTable();
            return BulkCreateDefaultParameters(components, eParamParentType.Component, skipIDs, namesWithParameters, bulkComponentInfo, new List<LinkInfo>());
        }

        private Dictionary<int, int> GetSkipIds(DataTable parentTable)
        {
            Dictionary<int, int> skipIDs = new Dictionary<int, int>();
            int id;
            foreach (DataRow c in parentTable.Rows)
            {
                id = Int32.Parse(c[SchemaConstants.Id].ToString());
                skipIDs.Add(id, id);
            }
            return skipIDs;
        }

        private List<Int32> BulkCreateDefaultParameters(
            DataTable parentTable, eParamParentType parent, Dictionary<int, int> skipIDs, Dictionary<String, String> namesWithParameters,
            List<ComponentInfo> bulkComponentInfo, List<LinkInfo> bulkLinkInfo)
        {
            Int32 i = 0;
            Dictionary<String, List<ParameterInfo>> typeToInfoCache = BulkDefaultParameterInfo(parent, parentTable);

            List<Int32> componentIds = new List<Int32>();

            List<ParameterInfo> infoList;
            int incrementer = 0;
            int id, fromID, toID;
            String componentName, componentType, key = null, fromType, toType, linkType;
            int rowCount = 0;
            String parentString = parent.ToString();
            foreach (DataRow row in parentTable.Rows)
            {
                id = Int32.Parse(row[SchemaConstants.Id].ToString());

                if (!skipIDs.ContainsKey(id)) // is this a new component?
                {
                    if (parent == eParamParentType.Component)
                    {
                        componentName = row[SchemaConstants.Name].ToString();
                        componentType = row[SchemaConstants.Type].ToString();
                        key = componentType;
                    }
                    else if (parent == eParamParentType.Link)
                    {
                        fromID = Int32.Parse(row[SchemaConstants.From].ToString());
                        toID = Int32.Parse(row[SchemaConstants.To].ToString());
                        linkType = row[SchemaConstants.Type].ToString();
                        bool getFromTo = this.GetLinkIDInfo(fromID, toID, out fromType, out toType);
                        if (getFromTo)
                        {
                            // different xml location, same processing
                            linkType = this.GetBaseLinkType(linkType);
                            key = this.Configuration + linkType + fromType + toType;
                        }
                    }

                    componentIds.Add(id); // store and return the ID to match to the holder name

                    if (typeToInfoCache.ContainsKey(key)) // look up stored ParameterInfos for this type
                    {
                        infoList = typeToInfoCache[key];

                        foreach (ParameterInfo info in infoList)
                        {
                            // skip if this holder name matches a name with parameters to be created later on
                            if ((parent == eParamParentType.Component && !namesWithParameters.ContainsKey(bulkComponentInfo[incrementer].HolderName + info.Combined))
                                 ||
                                 parent == eParamParentType.Link && !namesWithParameters.ContainsKey(bulkLinkInfo[incrementer].HolderName + info.Combined))
                            {
                                if (info.Description.Equals(""))
                                {
                                    info.Description = "\0"; // use null -> sql interprets as empty string
                                }
                                if (info.Value.Equals(""))
                                {
                                    info.Value = "\0"; // use null -> sql interprets as empty string
                                }

                                if (info.BinaryData != null)
                                {
                                    try
                                    {
                                        this.UpdateParameters(id, info.Combined, info.BinaryData, parent);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "Error setting default array value");
                                    }
                                }
                                else
                                {
                                    streamWrite.WriteLine(Convert.ToString(i++) + "\t" + id + "\t" + parentString + "\t" + info.Combined + "\t" + info.Value + "\t" + info.Description + "\t");
                                    rowCount++;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Could not find type: " + key);
                    }
                    incrementer++;
                }
            }

            if (streamWrite != null)
            {
                streamWrite.Close();
            }

            if (rowCount > 0)
            {
                String formatFile = "ParameterTableFormatDefault.fmt";
                this.m_model.BulkCreate("ParameterTable", bulkInfo, formatFile, rowCount);
            }
            else
            {
                if (bulkInfo != null && bulkInfo.Exists)
                {
                    bulkInfo.Delete();
                }
            }
            return componentIds;
        }

        private Dictionary<String, List<ParameterInfo>> BulkDefaultParameterInfo(eParamParentType parent, DataTable parentTable)
        {
            Dictionary<String, List<ParameterInfo>> typeToInfoCache = new Dictionary<string, List<ParameterInfo>>();
            XmlNode complex = null;
            List<ParameterInfo> infoForType;
            String componentType = "", fromType = "", toType = "", linkType = "", key = "";
            int fromID, toID;
            bool getFromTo;
            foreach (DataRow row in parentTable.Rows)
            {
                if (parent == eParamParentType.Component)
                {
                    componentType = row[SchemaConstants.Type].ToString();
                    key = componentType;
                }
                else if (parent == eParamParentType.Link)
                {
                    fromID = Int32.Parse(row[SchemaConstants.From].ToString());
                    toID = Int32.Parse(row[SchemaConstants.To].ToString());
                    linkType = row[SchemaConstants.Type].ToString();
                    getFromTo = this.GetLinkIDInfo(fromID, toID, out fromType, out toType);
                    if (getFromTo)
                    {
                        linkType = this.GetBaseLinkType(linkType);
                        key = this.Configuration + linkType + fromType + toType;
                        componentType = linkType + fromType + toType;
                    }
                }

                if (!typeToInfoCache.ContainsKey(key))
                {
                    infoForType = new List<ParameterInfo>();

                    if (parent == eParamParentType.Component)
                    {
                        complex = this.m_model.GetParametersXML(componentType);
                    }
                    else if (parent == eParamParentType.Link)
                    {
                        complex = this.m_model.GetParametersXML(linkType, fromType, toType);
                    }

                    if (complex != null)
                    {
                        infoForType = GetParameterInfoList(complex, componentType, linkType, fromType, toType, parent, true, key);
                        typeToInfoCache.Add(key, infoForType);
                    }
                    else
                    {
                        typeToInfoCache.Add(key, new List<ParameterInfo>());
                    }
                }

            }
            return typeToInfoCache;
        }

        private List<ParameterInfo> GetParameterInfoList(XmlNode parametersNode, String componentType,
            String linkType, String fromType, String toType, eParamParentType parent)
        {
            return GetParameterInfoList(parametersNode, componentType, linkType, fromType, toType, parent, false, "");
        }

        private List<ParameterInfo> GetParameterInfoList(XmlNode parametersNode, String componentType, 
            String linkType, String fromType, String toType, eParamParentType parent, bool addToValidationInfo, String key)
        {
            XmlAttribute typeAttr, nameAttr, categoryAttr;
            Type cSharpType;
            ParamConstraint paramConstraint;
            List<ParameterInfo> infoList = new List<ParameterInfo>();
            String category, name, field;
            ParameterInfo info;

            XmlNodeList allParameters = parametersNode.SelectNodes("Parameter");
     
            foreach (XmlNode parameter in allParameters) // process each parameter...
            {
                nameAttr = parameter.Attributes[ConfigFileConstants.displayedName];
                categoryAttr = parameter.Attributes[ConfigFileConstants.category];
                typeAttr = parameter.Attributes[ConfigFileConstants.Type];
                if (typeAttr != null &&
                    typeAttr.Value != ConfigFileConstants.complexType &&
                    nameAttr != null &&
                    categoryAttr != null)
                {
                    category = categoryAttr.Value;
                    name = nameAttr.Value;
                    field = "";
                    cSharpType = AMEManager.GetType(typeAttr.Value, componentType);
                    paramConstraint = GetConstraint(category, name, field, parent, componentType, linkType, fromType, toType);
                    info = GetParameterInfo(parameter, category, name, field, cSharpType, paramConstraint, parent);
                    infoList.Add(info);

                    if (addToValidationInfo)
                    {
                        parameterValidationInfo.Add(key + info.Combined, new ParameterValidationInfo(cSharpType, info.ParamMin, info.ParamMax));
                    }
                 
                    XmlNodeList structChildren = parameter.SelectNodes("Parameters/Parameter");

                    foreach (XmlNode structChild in structChildren) // process each parameter...
                    {
                        typeAttr = structChild.Attributes[ConfigFileConstants.Type];
                        cSharpType = AMEManager.GetType(typeAttr.Value, componentType);
                        field = structChild.Attributes[ConfigFileConstants.Name].Value;
                        paramConstraint = GetConstraint(category, name, field, parent, componentType, linkType, fromType, toType);
                        info = GetParameterInfo(structChild, category, name, field, cSharpType, paramConstraint, parent);
                        infoList.Add(info);
                        
                        if (addToValidationInfo)
                        {
                            parameterValidationInfo.Add(key + info.Combined, new ParameterValidationInfo(cSharpType, info.ParamMin, info.ParamMax));
                        }
                    }
                }
            }
            return infoList;
        }

        private ParameterInfo GetParameterInfo(XmlNode parameter,
        String category, String name, String field, Type cSharpType, ParamConstraint paramConstraint, eParamParentType parent)
        {
            XmlAttribute valueAttr, descriptionAttr;
            String valueAttrValue, descriptionAttrValue = "", combined;
            String paramMin = null;
            String paramMax = null;

            descriptionAttr = parameter.Attributes[ConfigFileConstants.description];

            if (descriptionAttr != null) // optional
            {
                descriptionAttrValue = descriptionAttr.Value;
            }

            combined = category + SchemaConstants.ParameterDelimiter + name;
            if (field.Length > 0)
            {
                combined = combined + SchemaConstants.FieldLeftDelimeter + field + SchemaConstants.FieldRightDelimeter;
            }

            paramMin = null;
            paramMax = null;

            if (paramConstraint != null)
            {
                paramMin = paramConstraint.Range.Min;
                paramMax = paramConstraint.Range.Max;
            }

            valueAttr = parameter.Attributes[ConfigFileConstants.Value];
            if (valueAttr != null)
            {
                valueAttrValue = valueAttr.Value;
                object oValidatedValue = this._ValidateValue(cSharpType, valueAttrValue, paramMin, paramMax);

                if (oValidatedValue is Array)
                {
                    // array - can't bulk insert, serialize
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream memoryStream = new MemoryStream();
                    formatter.Serialize(memoryStream, oValidatedValue);
                    byte[] arrayData = memoryStream.ToArray();
                    return new ParameterInfo(category, name, field, combined, parent, "", descriptionAttrValue, paramMin, paramMax, arrayData);
                }
                else
                {
                    string sParamValToStore = (oValidatedValue == null) ? valueAttrValue : oValidatedValue.ToString();
                    return new ParameterInfo(category, name, field, combined, parent, sParamValToStore, descriptionAttrValue, paramMin, paramMax);
                }
            }
            else
            {
                return new ParameterInfo(category, name, field, combined, parent, "", descriptionAttrValue, paramMin, paramMax);
            }
        }

        public List<Int32> BulkCreateLinks(List<LinkInfo> bulkLinkInfo, Dictionary<String, String> namesWithParameters)
        {
            Int32 i = 0;

            CheckUseLoadingCaches();

            if (bulkLinkInfo.Count == 0)
            {
                DataTable links = this.GetLinkTable();
                Dictionary<int, int> skipIDs = GetSkipIds(links);
                BulkCreateDefaultParameters(links, eParamParentType.Link, skipIDs, namesWithParameters, new List<ComponentInfo>(), bulkLinkInfo);
                return new List<Int32>();
            }
            try
            {
                // prebuild a component cache if we're delaying validation
                if (UseDelayedValidation && delayedValidationComponentTableCache == null)
                {
                    delayedValidationComponentTable = GetComponentTable();
                    delayedValidationComponentTableCache = GetComponentTableCache(delayedValidationComponentTable);
                }

                SetupBulkFileIO("BulkLinks.txt");

                DataTable links = this.GetLinkTable();
                Dictionary<int, int> skipIDs = GetSkipIds(links);
                int rowCount = 0;

                foreach (LinkInfo lInfo in bulkLinkInfo)
                {
                    if (lInfo.Description.Equals(""))
                    {
                        lInfo.Description = "\0"; // use null -> sql interprets as empty string
                    }

                    //Checking if top component exists
                    if (!this._ComponentExists(lInfo.RootID))
                    {
                        string sError = string.Format("Top {0} does not exist.", lInfo.RootID);
                        throw new System.Exception(sError);
                    }

                    DataRow fromComponent = this._GetComponent(lInfo.FromID);
                    DataRow toComponent = this._GetComponent(lInfo.ToID);

                    //Checking parent is in Database
                    if (fromComponent == null)
                    {
                        string sError = string.Format("Parent {0} does not exist.", lInfo.FromID);
                        throw new System.Exception(sError);
                    }
                    //Checking child is in Database
                    if (toComponent == null)
                    {
                        string sError = string.Format("Child {0} does not exist.", lInfo.ToID);
                        throw new System.Exception(sError);
                    }

                    //not allowing view to create a link of type Class-Instance
                    if (lInfo.Type == Component.Class.ClassInstanceLinkType || lInfo.Type == Component.Class.ClassSubclassLinkType)
                    {
                        string sError =
                        string.Format("Can not create a link of {0} type. It is reserved word.",
                            lInfo.Type);
                        throw new System.ArgumentException(sError);
                    }

                    // sanity check before validation
                    ValidateConnect(lInfo.Type, fromComponent[SchemaConstants.Type].ToString(), toComponent[SchemaConstants.Type].ToString());

                    if (UseDelayedValidation)
                    {
                        // for fast validation e.g. VSG import
                        // store the top ids and the linktypes, use getChildren with validation to 
                        // retrieve those and UseCachedValidation is turned back on
                        // remember to turn off UseCachedValidation when done!
                        string storedValidKey = "" + lInfo.RootID + lInfo.Type;
                        if (!storedValidationTopIDsAndLinkTypes.ContainsKey(storedValidKey))
                        {
                            storedValidationTopIDsAndLinkTypes.Add(storedValidKey, new TopIDAndLinkType(lInfo.RootID, lInfo.Type));
                        }
                    }

                    streamWrite.WriteLine(Convert.ToString(i++) + "\t" + lInfo.FromID + "\t" + lInfo.ToID + "\t" + lInfo.Type + "\t" + lInfo.Description);
                    rowCount++;
                }

                streamWrite.Close();
                
                String formatFile = "LinkTableFormat.fmt";
                this.m_model.BulkCreate("LinkTable", bulkInfo, formatFile, rowCount);

                SetupBulkFileIO("LinkParameters.txt");

                links = this.GetLinkTable();
                return BulkCreateDefaultParameters(links, eParamParentType.Link, skipIDs, namesWithParameters,
                                                   new List<ComponentInfo>(), bulkLinkInfo);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // discard component table cache - otherwise will negatively affect the next configuration
                // by preventing it from seeing the component table state!
                delayedValidationComponentTable = null;
                delayedValidationComponentTableCache = null;
            }
        }

        public void BulkCreateParameters(List<ParameterInfo> bulkParameterInfo)
        {
            Int32 i = 0;

            CheckUseLoadingCaches();

            if (bulkParameterInfo.Count == 0)
            {
                return;
            }

            // prebuild a component cache if we're delaying validation
            if (UseDelayedValidation && delayedValidationComponentTableCache == null)
            {
                delayedValidationComponentTable = GetComponentTable();
                delayedValidationComponentTableCache = GetComponentTableCache(delayedValidationComponentTable);
            }

            String linkType = "";
            String componentType = "";
            String fromType = "";
            String toType = "";
            String key = "";

            SetupBulkFileIO("BulkNewParameters.txt");

            int rowCount = 0;
            foreach (ParameterInfo info in bulkParameterInfo)
            {
                // category.name -> category, name
                String name, category, childField;

                if (GetCategoryNameAndField(info.Combined, out category, out name, out childField))
                {
                    // intialize anything we need...
                    if (info.ParentType == eParamParentType.Link)
                    {
                        GetLinkIDInfo(info.ParentID, out linkType, out fromType, out toType);
                        linkType = this.GetBaseLinkType(linkType);
                        key = this.Configuration + linkType + fromType + toType + info.Combined;
                    }
                    else if (info.ParentType == eParamParentType.Component)
                    {
                        componentType = GetComponentType(info.ParentID);
                        key = componentType + info.Combined;
                    }

                    string sParamValToStore = info.Value;

                    if (parameterValidationInfo.ContainsKey(key))
                    {
                        object oValidatedValue = this._ValidateValue(parameterValidationInfo[key].Type, info.Value, parameterValidationInfo[key].ParamMin, parameterValidationInfo[key].ParamMax);
                        sParamValToStore = (oValidatedValue == null) ? info.Value : oValidatedValue.ToString();

                        UpdateCacheWithValue("" + info.ParentID, category, name, childField, sParamValToStore, info.ParentType);
                    }
                    else
                    {
                        throw new Exception("Could not find validation type: " + key);
                    }

                    if (info.Description.Equals(""))
                    {
                        info.Description = "\0"; // use null -> sql interprets as empty string
                    }
                    if (info.Value.Equals(""))
                    {
                        info.Value = "\0"; // use null -> sql interprets as empty string
                    }
                    streamWrite.WriteLine(Convert.ToString(i++) + "\t" + info.ParentID + "\t" + info.ParentType.ToString() + "\t" + info.Combined + "\t" + info.Value + "\t" + info.Description + "\t");
                    rowCount++;
                }
            }

            streamWrite.Close();

            String formatFile = "ParameterTableFormat.fmt";
            this.m_model.BulkCreate("ParameterTable", bulkInfo, formatFile, rowCount);

            // discard component table cache - otherwise will negatively affect the next configuration
            // by preventing it from seeing the component table state!
            delayedValidationComponentTable = null;
            delayedValidationComponentTableCache = null;
        }
    }
}
