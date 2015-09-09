using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using System.Drawing;
using AME.Controllers;
using System.Collections.Specialized;
using Mvp.Xml.Common.XPath;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;

namespace AME.Model
{
    public abstract class Model
    {
        protected String documentationPath = String.Empty;
        protected String licensePath = String.Empty;
        protected String modelPath = String.Empty;
        protected String outputPath = String.Empty;
        protected String xmlPath = String.Empty;
        protected String dataPath = String.Empty;
        protected String importPath = String.Empty;
        protected String formatPath = String.Empty;

        protected XmlDocument xmlDoc;
        protected IndexingXPathNavigator indexedXMLDoc;

        private AME.Model.Configuration.Configuration modelConfiguration;

        public String ModelConfigurationName
        {
            set 
            {
                if (!value.EndsWith(".xml"))
                    value = String.Concat(value, ".xml");
                String fileName = Path.Combine(Application.StartupPath, value);
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    XDocument xDocument = XDocument.Load(file.FullName);
                    XmlSerializer xmlSerializerInput = new XmlSerializer(typeof(AME.Model.Configuration.Configuration));
                    modelConfiguration = (AME.Model.Configuration.Configuration)xmlSerializerInput.Deserialize(xDocument.CreateReader());

                    String path;
                    
                    #if (DEBUG)
                        path = Directory.GetCurrentDirectory();
                    #else
                        path = Application.StartupPath;
                    #endif

                    if (modelConfiguration.Directories.Documentation.relative)
                        documentationPath = Path.Combine(path, modelConfiguration.Directories.Documentation.Path);
                    else
                        documentationPath = modelConfiguration.Directories.Documentation.Path;

                    if (modelConfiguration.Directories.License.relative)
                        licensePath = Path.Combine(path, modelConfiguration.Directories.License.Path);
                    else
                        licensePath = modelConfiguration.Directories.License.Path;
                   
                    if (modelConfiguration.Directories.Model.relative)
                        modelPath = Path.Combine(path, modelConfiguration.Directories.Model.Path);
                    else
                        modelPath = modelConfiguration.Directories.Model.Path;
                    
                    if (modelConfiguration.Directories.Output.relative)
                        outputPath = Path.Combine(path, modelConfiguration.Directories.Output.Path);
                    else
                        outputPath = modelConfiguration.Directories.Output.Path;
                    
                    if (modelConfiguration.Directories.Xml.relative)
                        xmlPath = Path.Combine(path, modelConfiguration.Directories.Xml.Path);
                    else
                        xmlPath = modelConfiguration.Directories.Xml.Path;
                    
                    if (modelConfiguration.Directories.Data.relative)
                        dataPath = Path.Combine(path, modelConfiguration.Directories.Data.Path);
                    else
                        dataPath = modelConfiguration.Directories.Data.Path;

                    if (modelConfiguration.Database.Import.relative)
                        importPath = Path.Combine(path, modelConfiguration.Database.Import.Path);
                    else
                        importPath = modelConfiguration.Database.Import.Path;

                    if (modelConfiguration.Database.Format.relative)
                        formatPath = Path.Combine(path, modelConfiguration.Database.Format.Path);
                    else
                        formatPath = modelConfiguration.Database.Format.Path;

                    LoadConfiguration();
                }
                else
                    throw new Exception("Model configuration file could not be found!");
            }
        }

        public AME.Model.Configuration.Configuration ModelConfiguration
        {
            get 
            {
                AME.Model.Configuration.Configuration c = new AME.Model.Configuration.Configuration();

                c.name = modelConfiguration.name;
                c.xsdNamespace = modelConfiguration.xsdNamespace;
                c.xslNamespace = modelConfiguration.xslNamespace;
                c.imgNamespace = modelConfiguration.imgNamespace;
                c.configurationNamespace = modelConfiguration.configurationNamespace;

                c.Directories.Documentation.relative = false;
                c.Directories.Documentation.Path = documentationPath;

                c.Directories.License.relative = false;
                c.Directories.License.Path = licensePath;

                c.Directories.Model.relative = false;
                c.Directories.Model.Path = modelPath;

                c.Directories.Output.relative = false;
                c.Directories.Output.Path = outputPath;

                c.Directories.Xml.relative = false;
                c.Directories.Xml.Path = xmlPath;

                c.Directories.Data.relative = false;
                c.Directories.Data.Path = dataPath;

                c.Database.Import.relative = false;
                c.Database.Import.Path = importPath;

                c.Database.Format.relative = false;
                c.Database.Format.Path = formatPath;

                return c;
            }
        }

        protected String safeSqlLiteral(string inputSQL)
        {
            String outputSQL = inputSQL;
            //outputSQL = outputSQL.Replace(@";", @"';'");
            outputSQL = outputSQL.Replace(@"'", @"''");
            outputSQL = outputSQL.Replace(@"\", @"\\");

            return outputSQL;
        }

        public String DocumentationPath
        {
            get
            {
                return documentationPath;
            }

            set
            {
                documentationPath = value;
            }
        }

        public String LicensePath
        {
            get
            {
                return licensePath;
            }

            set
            {
                licensePath = value;
            }
        }

        public String DataPath
        {
            get
            {
                return dataPath;
            }

            set
            {
                dataPath = value;
            }
        }

        public String XmlPath
        {
            get
            {
                return xmlPath;
            }

            set
            {
                xmlPath = value;
            }
        }

        public String OutputPath
        {
            get
            {
                return outputPath;
            }

            set
            {
                outputPath = value;
            }
        }

        public String ModelPath
        {
            get
            {
                return modelPath;
            }

            set
            {
                modelPath = value;
            }
        }

        public String ImportPath
        {
            get
            {
                return importPath;
            }

            set
            {
                importPath = value;
            }
        }


        public String FormatPath
        {
            get
            {
                return formatPath;
            }

            set
            {
                formatPath = value;
            }
        }

        public void LoadConfiguration()
        {
            try
            {
                String name = String.Concat(modelConfiguration.configurationNamespace, ".configuration.xml");
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                using (StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(name)))
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load(streamReader);
                }
                indexedXMLDoc = new IndexingXPathNavigator(xmlDoc.CreateNavigator());
                indexedXMLDoc.AddKey("ParameterNameKey", "GME/Global/Components/Component/ComplexParameters/Parameters/Parameter", "concat(@displayedName, '||', @category, '||', ../../../@name)");
                indexedXMLDoc.AddKey("ParameterBaseKey", "GME/Global/Components/Component/ComplexParameters/Parameters/Parameter", "concat(@displayedName, '||', @category, '||', ../../../@base)");
                indexedXMLDoc.AddKey("ParameterStructNameKey", "GME/Global/Components/Component/ComplexParameters/Parameters/Parameter/Parameters/Parameter", "concat(../../@displayedName, '||', ../../@category, '||', ../../../../../@name, '||', @name)");
                indexedXMLDoc.AddKey("ParameterStructBaseKey", "GME/Global/Components/Component/ComplexParameters/Parameters/Parameter/Parameters/Parameter", "concat(../../@displayedName, '||', ../../@category, '||', ../../../../../@base, '||', @name)");

                indexedXMLDoc.AddKey("ComponentNameKey", "GME/Global/Components/Component", "@name");
                indexedXMLDoc.AddKey("ComponentBaseKey", "GME/Global/Components/Component", "@base");
                
                indexedXMLDoc.AddKey("LinkParameterKey", "GME/Global/Links/Link/Connect/ComplexParameters/Parameters/Parameter", "concat(@displayedName, '||', @category, '||', ../../../@from, '||', ../../../@to, '||', ../../../../@type)");
                indexedXMLDoc.AddKey("LinkParameterStructKey", "GME/Global/Links/Link/Connect/ComplexParameters/Parameters/Parameter/Parameters/Parameter", "concat(../../@displayedName, '||', ../../@category, '||', ../../../../../@from, '||', ../../../../../@to, '||', ../../../../../../@type, '||', @name)");
                indexedXMLDoc.AddKey("LinkFromToKey", "GME/Global/Links/Link/Connect", "concat(@from, '||', @to, '||', ../@type)");
                indexedXMLDoc.AddKey("LinkKey", "GME/Global/Links/Link", "@type");
                
                indexedXMLDoc.AddKey("ConfigurationKey", "GME/Configurations/Configuration", "@name");

                indexedXMLDoc.BuildIndexes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to load for configuration.xml at path '{0}'!", modelConfiguration.configurationNamespace));
            }
        }

        abstract public void InitializeDB();
        abstract public void ImportSql(String filename);

        abstract public void DropDatabase();

        abstract public DataTable GetComponentTable();
        abstract public DataTable GetComponentTable(String columnType, String name);
        abstract public DataTable GetComponent(Int32 id);
        abstract public DataTable GetLink(Int32 id);
        public virtual DataTable GetLink(Int32 fromID, Int32 toID, String linkType) { throw new Exception("GetLink not implemented by this model!  Should override"); }
        abstract public DataTable GetLinkTable();
        abstract public List<String> GetDynamicLinkTypes(String linkTypes);
        abstract public DataTable GetLinkTable(String linkType);
        abstract public DataTable GetParameterTable();
        abstract public DataTable GetParameterTable(Int32 parentId, String parentType);
        abstract public DataTable GetChildComponents(Int32 id);
        abstract public DataTable GetChildComponents(Int32 id, String linkType);
        abstract public DataTable GetChildComponents(Int32 id, String columnName, String columnValue);
        abstract public DataTable GetChildComponents(Int32 id, String columnName, String columnValue, String linkType);
        abstract public DataTable GetParentComponents(Int32 id);
        abstract public DataTable GetParentComponents(Int32 id, String linkType);
        abstract public DataTable GetChildComponentLinks(Int32 id);
        abstract public DataTable GetChildComponentLinks(Int32 id, String linkType);
        abstract public NameValueCollection GetRootLinkTypes();

        abstract public IXPathNavigable GetOutputXml(String filename);
        abstract public String GetConfigurationControllerName(String configuration);
        abstract public List<String> GetConfigurationNames();
        abstract public String GetRootComponent(String configuration);
        abstract public String GetComponentLinkType(String configuration);

        abstract public Dictionary<String, Bitmap> GetComponentBitmaps(String configuration);

        abstract public Int32 CreateComponent(String component, String name, String eType, String description);
        abstract public Int32 GetComponentId(String component, String name);
        abstract public void DeleteComponent(Int32 id);
        abstract public Int32 UpdateComponent(Int32 id, String columnName, String columnValue);
        abstract public Int32 CreateLink(Int32? fromComponentId, Int32 toComponentId, String type, String description);
        abstract public void DeleteLink(Int32 id);
        abstract public List<Int32> DeleteLinks(String linkType);
        abstract public Int32 CreateParameter(Int32 parentId, String parentType, String name, String value, String description);
        abstract public void DeleteParameter(int parentId, String parentType, String name);// { throw new Exception("Single parameter delete not supported by this model!"); }
        abstract public XmlNodeList GetSubComponents(String configuration, String component);

        [Obsolete("Please use GetComponent(String component): Components are now defined in global space.")] 
        abstract public IXPathNavigable GetComponent(String configuration, String component);

        // sql model will override, no-op for others
        abstract public void BulkCreate(String table, FileInfo inputFile, String formatFile, int rowsPerBatch);// { throw new Exception("Bulk create not supported by this model!"); }
        abstract public Int32 CreateParameter(Int32 parentId, String parentType, String name, byte[] value, String description);// { throw new Exception("Binary data insert not supported by this model!"); }

        public void WriteOutputXML(String filename, XmlDocument toWrite)
        {
            //String outputFilepath = Path.GetFullPath(configurationPath + @"\output");
            if (!filename.Contains(".")) // assume xml if no extension
            {
                filename += ".xml";
            }
            String fullFilename = Path.Combine(outputPath, filename);
            if (fullFilename.Length > 0)
            {
                toWrite.Save(fullFilename);
            }
        }

        public IXPathNavigable GetConfiguration(String configuration)
        {
            try
            {
                XPathNodeIterator configurationIterator = indexedXMLDoc.Select("key('ConfigurationKey', '" + configuration + "')");

                if (configurationIterator.MoveNext())
                {
                    return configurationIterator.Current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get configuration '{0}'", configuration));
                return null;
            }
            return null;
        }

        public IXPathNavigable GetLink(String linkType)
        {
            try
            {
                XPathNodeIterator it = indexedXMLDoc.Select("key('LinkKey', '" + linkType + "')");

                if (it.MoveNext())
                {
                    return it.Current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get link node for '{0}'", linkType));
            }
            return null;
        }

        public IXPathNavigable GetLink(String linkType, String fromType, String toType)
        {
            try
            {
                XPathNodeIterator it = indexedXMLDoc.Select("key('LinkFromToKey', '" + fromType + "||" + toType + "||" + linkType + "')");

                if (it.MoveNext())
                {
                    return it.Current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get link node for '{0}', '{1}', '{2}'", linkType, fromType, toType));
            }
            return null;
        }

        public IXPathNavigable GetLinks()
        {
            try
            {
                XmlNode returnNode = xmlDoc.SelectSingleNode("GME/Global/Links");
                if (returnNode != null)
                {
                    return returnNode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get link types"));
                return null;
            }
            return null;
        }

        public XmlNode GetParametersXML(String componentType)
        {
            XmlNode parameters = null;
            try
            {
                parameters = xmlDoc.SelectSingleNode(String.Format("GME/Global/Components/Component[@name='{0}' or @base='{0}']/ComplexParameters/Parameters", componentType));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter node for '{0}'", componentType));
            }
            return parameters;
        }

        public XmlNode GetParametersXML(String linkType, String fromType, String toType)
        {
            XmlNode parameters = null;
            try
            {
                parameters = xmlDoc.SelectSingleNode(String.Format("GME/Global/Links/Link[@type='{0}']/Connect[@from='{1}'][@to='{2}']/ComplexParameters/Parameters", linkType, fromType, toType));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameters for '{0}', '{1}', '{2}'", linkType, fromType, toType));
            }
            return parameters;
        }

        public XPathNavigator GetParameter(String componentType, String category, String parameterName, String childField)
        {
            try
            {
                //parameterNode = xmlDoc.SelectSingleNode(String.Format("GME/Global/Components/Component[@name='{0}' or @base='{0}']/ComplexParameters/Parameters/Parameter[@displayedName='{1}'][@category='{2}']", componentType, parameterName, category));
                XPathNodeIterator it;
                if (childField.Length == 0)
                {
                    it = indexedXMLDoc.Select("key('ParameterNameKey', '" + parameterName + "||" + category + "||" + componentType + "')");
                }
                else
                {
                    it = indexedXMLDoc.Select("key('ParameterStructNameKey', '" + parameterName + "||" + category + "||" + componentType + "||" + childField + "')");
                }


                if (it.MoveNext())
                {
                    return it.Current;
                }

                if (childField.Length == 0)
                {
                    it = indexedXMLDoc.Select("key('ParameterBaseKey', '" + parameterName + "||" + category + "||" + componentType + "')");
                }
                else
                {
                    it = indexedXMLDoc.Select("key('ParameterStructBaseKey', '" + parameterName + "||" + category + "||" + componentType + "||" + childField + "')");
                }


                if (it.MoveNext())
                {
                    return it.Current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter node for '{0}', '{1}', '{2}'", componentType, parameterName, category));
            }
            return null;
        }

        public String GetParameterType(String componentType, String category, String parameterName, String childField)
        {
            String type = String.Empty;
            try
            {
                XPathNavigator parameterNode = GetParameter(componentType, category, parameterName, childField);
                if (parameterNode != null)
                {
                    type = parameterNode.GetAttribute("type", parameterNode.NamespaceURI);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter type for  '{0}', '{1}', '{2}'", parameterName, category, componentType));
            }
            return type;
        }

        public String GetParameterTypeConverter(String componentType, String category, String parameterName, String childField)
        {
            String typeconverter = String.Empty;
            try
            {
                XPathNavigator parameterNode = GetParameter(componentType, category, parameterName, childField);
                if (parameterNode != null)
                {
                    typeconverter = parameterNode.GetAttribute("typeconverter", parameterNode.NamespaceURI);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter type converter for  '{0}', '{1}', '{2}'", parameterName, category, componentType));
            }
            return typeconverter;
        }

        public XPathNavigator GetParameter(String linkType, String fromComponentType, String toComponentType, String category, String parameterName, String childField)
        {
            XPathNavigator parameterNode = null;
            try
            {
                //parameterNode = xmlDoc.SelectSingleNode(String.Format("/GME/Global/Configurations/Configuration[@name='{0}']/LinkTypes/Link[@type='{1}']/ComplexParameters[@from='{2}'][@to='{3}']/Parameters/Parameter[@displayedName='{4}'][@category='{5}']", configuration, linkType, fromComponentType, toComponentType, parameterName, category));
                XPathNodeIterator it;

                if (childField.Length == 0)
                {
                    it = indexedXMLDoc.Select("key('LinkParameterKey', '" + parameterName + "||" + category + "||" + fromComponentType + "||" + toComponentType + "||" + linkType + "')");
                }
                else
                {
                    it = indexedXMLDoc.Select("key('LinkParameterStructKey', '" + parameterName + "||" + category + "||" + fromComponentType + "||" + toComponentType + "||" + linkType + "||" + childField + "')");
                }
                
                if (it.MoveNext())
                {
                    return it.Current;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter node for '{0}', '{1}', '{2}', '{3}', '{4}'", linkType, fromComponentType, toComponentType, parameterName, category));
            }
            return parameterNode;
        }

        public String GetParameterType(String linkType, String fromComponentType, String toComponentType, String category, String parameterName, String childField)
        {
            String type = String.Empty;
            try
            {
                XPathNavigator parameterNode = this.GetParameter(linkType, fromComponentType, toComponentType, category, parameterName, childField);
                if (parameterNode != null)
                {
                    type = parameterNode.GetAttribute("type", parameterNode.NamespaceURI);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter type for '{0}', '{1}', '{2}', '{3}', '{4}'", linkType, fromComponentType, toComponentType, parameterName, category));
            }
            return type;
        }
        public String GetParameterTypeConverter(String linkType, String fromComponentType, String toComponentType, String category, String parameterName, String childField)
        {
            String typeconverter = String.Empty;
            try
            {
                XPathNavigator parameterNode = this.GetParameter(linkType, fromComponentType, toComponentType, category, parameterName, childField);
                if (parameterNode != null)
                {
                    typeconverter = parameterNode.GetAttribute("typeconverter", parameterNode.NamespaceURI);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parameter type converter for '{0}', '{1}', '{2}', '{3}', '{4}'", linkType, fromComponentType, toComponentType, parameterName, category));
            }
            return typeconverter;
        }

        public IXPathNavigable GetComponent(String component)
        {
            IXPathNavigable comp = null;
            try
            {
                //String sXPath = String.Format("GME/Global/Components/Component[@name='{0}']", compType);
                XPathNodeIterator it = indexedXMLDoc.Select("key('ComponentNameKey', '" + component + "')");
                if (it.MoveNext())
                {
                    return it.Current;
                }
            }
            catch { }

            return comp;
        }//GetComponentType

        public String GetBaseComponentType(String compType)
        {
            String sBaseCompType = null;

            try
            {
                //String sXPath = String.Format("GME/Global/Components/Component[@name='{0}']", compType);
                XPathNodeIterator it = indexedXMLDoc.Select("key('ComponentNameKey', '" + compType + "')");
                if (it.MoveNext())
                {
                    XPathNavigator baseComp = it.Current;
                    String baseAttr = baseComp.GetAttribute("base", baseComp.NamespaceURI);
                    if (baseAttr != null && baseAttr.Length > 0)
                    {
                        sBaseCompType = baseAttr;
                    }
                }
            }//try
            catch { }

            return sBaseCompType;
        }//GetBaseComponentType
        public List<String> GetDerivedComponentType(String baseCompType)
        {
            List<String> list = new List<string>();

            try
            {
                //String sXPath = String.Format("GME/Global/Components/Component[@base='{0}']", baseCompType);
                XPathNodeIterator derivedCompList = indexedXMLDoc.Select("key('ComponentBaseKey', '" + baseCompType + "')");

                if (derivedCompList != null)
                {
                    foreach (XPathNavigator compNode in derivedCompList)
                    {
                        list.Add(compNode.GetAttribute("name", compNode.NamespaceURI));
                    }//for each component
                }
            }//try
            catch
            {
                list.Clear();
            }//catch

            return list;
        }//GetDerivedComponentType
    }//Model class
}//MVC
