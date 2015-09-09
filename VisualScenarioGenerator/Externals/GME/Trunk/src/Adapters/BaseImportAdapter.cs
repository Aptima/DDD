using System.Xml;
using System.Collections.Generic;
using System;
using System.Xml.XPath;
using System.Xml.Schema;
using AME.Tools;
using System.IO;
using AME.Controllers;
using System.Windows.Forms;

namespace AME.Adapters
{
    /// <summary>
    /// A basic import adapter that has some shared helper methods.  Subclasses are left to implement Process.
    /// </summary>
    public abstract class BaseImportAdapter : IImportAdapter
    {
        protected XmlDocument importDoc; // the importer document we're creating
        protected XmlElement components, links, parameters; // elements for tables in the import document
        protected Dictionary<String, String> existingLinks = new Dictionary<String, String>();
        protected Dictionary<String, String> existingComponents = new Dictionary<String, String>();
        protected Dictionary<String, String> supplementalParameters = new Dictionary<String, String>();

        protected BaseImportAdapter()
        {
            importDoc = new XmlDocument();

            CreateConfiguration();

            existingComponents.Clear();
            existingLinks.Clear();
            supplementalParameters.Clear();
        }

        public abstract void CreateConfiguration();

        public abstract IXPathNavigable Process(string uriSource); // will be implemented by subclasses

        // helper methods

        // add attributes to an xpath, e.g. Element[@name='nameValue'][@type='typeValue']
        // the array at the end is evenly divided into names to match, each followed by their value
        protected XPathNodeIterator XPathWithAttributes(XPathNavigator nav, String baseElementPath, params String[] attributeNameAndValues)
        {
            String xpath = preparePath(baseElementPath, attributeNameAndValues);
            return nav.Select(xpath);
        }

        protected XPathNavigator XPathWithAttributesSingle(XPathNavigator nav, String baseElementPath, params String[] attributeNameAndValues)
        {
            String xpath = preparePath(baseElementPath, attributeNameAndValues);
            return nav.SelectSingleNode(xpath);
        }

        private String preparePath(String baseElementPath, params String[] attributeNameAndValues)
        {
            for (int i = 0; i < attributeNameAndValues.Length; i++)
            {
                String value = attributeNameAndValues[i + 1];
                String quoteString = "'";
                if (value.Contains("'")) // replace single quote with double quotes (apostrophe present inside value use double, otherwise use single)
                {
                    quoteString = "\"";
                }
                baseElementPath += "[@" + attributeNameAndValues[i] + "=" + quoteString + value + quoteString + "]";
                
                i++; // next pair
            }
            return baseElementPath;
        }
     
        // simplification - send empty string for us
        protected String GetAttribute(XPathNavigator nav, String attributeName)
        {
            return nav.GetAttribute(attributeName, String.Empty);
        }

        // common create database configuration element and table elements
        protected void CreateConfiguration(String configuration, String isRoot)
        {
            XmlElement dataBaseElement = importDoc.CreateElement("database");

            XmlElement cfgElement = importDoc.CreateElement("configuration");

            XmlAttribute nameAttr = importDoc.CreateAttribute("name");
            nameAttr.Value = configuration;
            cfgElement.Attributes.Append(nameAttr);

            XmlAttribute isRootAttr = importDoc.CreateAttribute("isRootController");
            isRootAttr.Value = isRoot;
            cfgElement.Attributes.Append(isRootAttr);

            components = importDoc.CreateElement("componentTable");
            cfgElement.AppendChild(components);

            links = importDoc.CreateElement("linkTable");
            cfgElement.AppendChild(links);

            parameters = importDoc.CreateElement("parameterTable");
            cfgElement.AppendChild(parameters);

            dataBaseElement.AppendChild(cfgElement);

            importDoc.AppendChild(dataBaseElement);
        }

        // shared XML creation methods
        protected void CreateComponent(XmlDocument document, string type, string name, string description, XmlNode componentElement)
        {
            CreateComponent("", document, type, name, description, componentElement);
        }

        protected void CreateComponent(string id, XmlDocument document, string type, string name, string description, XmlNode componentElement)
        {
            XmlElement component = document.CreateElement("component");

            XmlAttribute att;
            if (id != String.Empty)
            {
                att = document.CreateAttribute("id");
                att.Value = id;
                component.Attributes.Append(att);
            }

            att = document.CreateAttribute("type");
            att.Value = type;
            component.Attributes.Append(att);

            att = document.CreateAttribute("name");
            att.Value = name;
            component.Attributes.Append(att);

            att = document.CreateAttribute("description");
            att.Value = description;
            component.Attributes.Append(att);

            componentElement.AppendChild(component);
        }

        // shared XML creation methods
        protected void CreateLink(XmlDocument document, string rootComponentId, string fromComponentId, string toComponentId, string type, string description, XmlNode linkElement)
        {
            CreateLink("", document, rootComponentId, fromComponentId, toComponentId, type, description, linkElement);
        }

        protected void CreateLink(string id, XmlDocument document, string rootComponentId, string fromComponentId, string toComponentId, string type, string description, XmlNode linkElement)
        {
            XmlElement link = document.CreateElement("link");

            XmlAttribute att;
            if (id != String.Empty)
            {
                att = document.CreateAttribute("id");
                att.Value = id;
                link.Attributes.Append(att);
            }

            XmlAttribute aRootComponentId = document.CreateAttribute("rootComponentId");
            aRootComponentId.Value = rootComponentId;
            link.Attributes.Append(aRootComponentId);

            XmlAttribute aFromComponentId = document.CreateAttribute("fromComponentId");
            aFromComponentId.Value = fromComponentId;
            link.Attributes.Append(aFromComponentId);

            XmlAttribute aToComponentId = document.CreateAttribute("toComponentId");
            aToComponentId.Value = toComponentId;
            link.Attributes.Append(aToComponentId);

            XmlAttribute aType = document.CreateAttribute("type");
            aType.Value = type;
            link.Attributes.Append(aType);

            XmlAttribute aDescription = document.CreateAttribute("description");
            aDescription.Value = description;
            link.Attributes.Append(aDescription);

            linkElement.AppendChild(link);
        }

        protected void CreateParameter(XmlDocument document, string parentId, string parentType, string name, string value, string description, XmlNode paramElement)
        {
            XmlElement parameter = document.CreateElement("parameter");

            XmlAttribute aParentId = document.CreateAttribute("parentId");
            aParentId.Value = parentId;
            parameter.Attributes.Append(aParentId);

            XmlAttribute aParentType = document.CreateAttribute("parentType");
            aParentType.Value = parentType;
            parameter.Attributes.Append(aParentType);

            XmlAttribute aName = document.CreateAttribute("name");
            aName.Value = name;
            parameter.Attributes.Append(aName);

            XmlAttribute aValue = document.CreateAttribute("value");
            aValue.Value = value;
            parameter.Attributes.Append(aValue);

            XmlAttribute aDescription = document.CreateAttribute("description");
            aDescription.Value = description;
            parameter.Attributes.Append(aDescription);

            paramElement.AppendChild(parameter);
        }

        protected String GetDynamicLinkType(String linkType, String dynamicPivot)
        {
            string seperator = ImportTool.Delimitter; // "_@_"

            // is the linkType already dynamic?
            if (linkType.Contains(seperator))
            {
                int indexOfSeperator = linkType.IndexOf(seperator);
                string justAfterSeperator = linkType.Substring(0, indexOfSeperator + 1);

                // combine and form dynamic
                return justAfterSeperator + dynamicPivot;
            }
            else  // combine and form dynamic
            {
                return linkType + seperator + dynamicPivot;
            }
        }

        protected String GetLinkID(String from, String to, String type)
        {
            return from + to + type;
        }

        protected XPathNavigator validate(IController validatingController, XmlDocument document, String xsdPath)
        {
            XPathNavigator navValidatedXml = null;
            XmlReader schemaDatabase = validatingController.GetXSD(xsdPath);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaDatabase);

            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

            StringReader strReader = new StringReader(document.OuterXml);
            XmlReader xmlReader = XmlReader.Create(strReader, settings);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlReader);
                navValidatedXml = doc.CreateNavigator();
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Validation Error");
            }
            finally
            {
                schemaDatabase.Close();
                strReader.Close();
                xmlReader.Close();
            }
            return navValidatedXml;
        }

        protected void validationEventHandler(object sender, ValidationEventArgs args)
        {
            String message = String.Empty;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    message = "Import Error: " + args.Message;
                    break;
                case XmlSeverityType.Warning:
                    message = "Import Warning: " + args.Message;
                    break;
            }

            throw new System.Xml.Schema.XmlSchemaValidationException(args.Message);
        }

        protected void Link(String root, String parent, String linkType, params String[] children)
        {
            for (int i = 0; i < children.Length; i++)
            {
                String key = GetLinkID(parent, children[i], linkType);
                if (!existingLinks.ContainsKey(key))
                {
                    existingLinks.Add(key, key);
                    CreateLink(root, parent, children[i], linkType, "");
                }
            }
        }

        protected bool Create(String type, String name)
        {
            if (!existingComponents.ContainsKey(name))
            {
                existingComponents.Add(name, name);
                CreateComponent(type, name, "");
                return true;
            }
            return false;
        }

        // simplification - convert lists to arrays
        protected void Link(String root, String parent, String type, List<String> children)
        {
            Link(root, parent, type, children.ToArray());
        }

        // simplification - use the import document and component table for us
        protected void CreateComponent(string type, string name, string description)
        {
            CreateComponent(importDoc, type, name, description, components);
        }

        // simplification - use the import document and component table for us
        protected void CreateComponent(string id, string type, string name, string description)
        {
            CreateComponent(id, importDoc, type, name, description, components);
        }

        // simplification - use the import document and link table for us
        protected void CreateLink(string id, string rootComponentId, string fromComponentId, string toComponentId, string type, string description)
        {
            CreateLink(id, importDoc, rootComponentId, fromComponentId, toComponentId, type, description, links);
        }

        // simplification - use the import document and link table for us
        protected void CreateLink(string rootComponentId, string fromComponentId, string toComponentId, string type, string description)
        {
            CreateLink(importDoc, rootComponentId, fromComponentId, toComponentId, type, description, links);
        }

        // simplification - use the import document and parameter table for us
        protected void CreateComponentParameter(string parentId, string name, string value)
        {
            String key = parentId + " " + name;
            if (!supplementalParameters.ContainsKey(key))
            {
                supplementalParameters.Add(key, value);
                CreateParameter(importDoc, parentId, eParamParentType.Component.ToString(), name, value, "", parameters);
            }
            else
            {
                if (!supplementalParameters[key].Equals(value))
                {
                    String error = "Duplicate parameter for: " + key + ", multiple values provided: " + supplementalParameters[key] + ", " + value;
                    Console.WriteLine(error);
                    MessageBox.Show(error);
                }
            }
        }

        protected void CreateLinkParameter(string parentId, string name, string value)
        {
            CreateParameter(importDoc, parentId, eParamParentType.Link.ToString(), name, value, "", parameters);
        }
    }
}
                
       

