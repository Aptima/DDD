using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;
using AME.Controllers;

namespace AME.Adapters
{
    abstract public class ControllerToXmlAdapter<T> : IExportDataAdapter<T>
    {
        protected XmlDocument doc;
        private ControllerToXmlSettings settings;

        protected ControllerToXmlAdapter(ControllerToXmlSettings settings)
        {
            this.settings = settings;
        }

        protected ControllerToXmlAdapter(Controller controller, RootController rootController, Boolean treeDataFormat)
        {
            this.settings = new ControllerToXmlSettings(controller, rootController, treeDataFormat);
        }

        protected void CreateDoc()
        {
            if (this.settings.treeDataFormat)
            {
                treeData();
            }
            else
            {
                flatData();
            }
        }

        private void treeData()
        {

            doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create root element
            XmlElement root = doc.CreateElement("LinkTypes");

            // Add schema information to root.
            // MW, remove for now?
            //XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            //schema.Value = Path.Combine(GMEManager.Instance.ConfigurationPath, @"database.xsd");
            //root.SetAttributeNode(schema);
            doc.AppendChild(root);

            //String linkType = controller.ConfigurationLinkType;
            NameValueCollection collection = settings.controller.GetRootLinkTypes();
            foreach (String fromComponentId in collection.Keys)
            {
                ComponentOptions compOptions = new ComponentOptions();
                compOptions.CompParams = true;
                compOptions.LinkParams = true;
                String[] types = collection[fromComponentId].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String type in types)
                {
                    XmlElement link = doc.CreateElement("Link");
                    XmlAttribute aId = doc.CreateAttribute("id");
                    aId.Value = fromComponentId;
                    link.Attributes.Append(aId);
                    XmlAttribute aType = doc.CreateAttribute("type");
                    aType.Value = type;
                    link.Attributes.Append(aType);

                    Int32 rootId = Int32.Parse(fromComponentId);
                    IXPathNavigable xmlTree = settings.controller.GetComponentAndChildren(rootId, type, compOptions);
                    XPathNavigator nav = xmlTree.CreateNavigator().SelectSingleNode("Components/Component");

                    link.InnerXml = nav.OuterXml;
                    root.AppendChild(link);

                    ///////

                    //Int32 rootId = Int32.Parse(fromComponentId);
                    //IXPathNavigable xmlTree = controller.GetComponentAndChildren(rootId, type, compOptions);
                    //XPathNavigator nav = xmlTree.CreateNavigator().SelectSingleNode("Components/Component");

                    //XmlNode link = ((IHasXmlNode)nav).GetNode();
                    //XmlNode newLink = doc.ImportNode(link, true);
                    //XmlAttribute aId = doc.CreateAttribute("id");
                    //aId.Value = fromComponentId;
                    //newLink.Attributes.Append(aId);
                    //XmlAttribute aType = doc.CreateAttribute("type");
                    //aType.Value = type;
                    //newLink.Attributes.Append(aType);

                    //root.AppendChild(newLink);
                }
            }
        }

        private void flatData()
        {
            // Get DB and create a DB xml file
            //dataController = (DataEntryController)GMEManager.Instance.Get("VSGEditor");
            DataTable componentTable = settings.controller.GetComponentTable();
            DataTable linkTable = settings.controller.GetLinkTable();
            DataTable parameterTable = settings.controller.GetParameterTable();

            doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create root element
            XmlElement root = doc.CreateElement("database");

            // Add schema information to root.
            // MW, remove for now?
            //XmlAttribute schema = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            //schema.Value = Path.Combine(GMEManager.Instance.ConfigurationPath, @"exportTree.xsd");
            //root.SetAttributeNode(schema);
            doc.AppendChild(root);

            XmlElement components = doc.CreateElement("componentTable");
            foreach (DataRow row in componentTable.Rows)
            {
                XmlElement component = doc.CreateElement("component");

                XmlAttribute id = doc.CreateAttribute("id");
                id.Value = Convert.ToString(row["id"]);
                component.SetAttributeNode(id);

                XmlAttribute type = doc.CreateAttribute("type");
                type.Value = Convert.ToString(row["type"]);
                component.SetAttributeNode(type);

                XmlAttribute name = doc.CreateAttribute("name");
                name.Value = Convert.ToString(row["name"]);
                component.SetAttributeNode(name);

                XmlAttribute description = doc.CreateAttribute("description");
                description.Value = Convert.ToString(row["description"]);
                component.SetAttributeNode(description);

                components.AppendChild(component);
            }
            root.AppendChild(components);

            XmlElement links = doc.CreateElement("linkTable");
            foreach (DataRow row in linkTable.Rows)
            {
                XmlElement link = doc.CreateElement("link");

                XmlAttribute id = doc.CreateAttribute("id");
                id.Value = Convert.ToString(row["id"]);
                link.SetAttributeNode(id);

                XmlAttribute fromComponentId = doc.CreateAttribute("fromComponentId");
                fromComponentId.Value = Convert.ToString(row["fromComponentId"]);
                link.SetAttributeNode(fromComponentId);

                XmlAttribute toComponentId = doc.CreateAttribute("toComponentId");
                toComponentId.Value = Convert.ToString(row["toComponentId"]);
                link.SetAttributeNode(toComponentId);

                XmlAttribute description = doc.CreateAttribute("description");
                description.Value = Convert.ToString(row["description"]);
                link.SetAttributeNode(description);

                links.AppendChild(link);
            }
            root.AppendChild(links);

            XmlElement parameters = doc.CreateElement("parameterTable");
            foreach (DataRow row in parameterTable.Rows)
            {
                XmlElement parameter = doc.CreateElement("parameter");

                XmlAttribute id = doc.CreateAttribute("id");
                id.Value = Convert.ToString(row["id"]);
                parameter.SetAttributeNode(id);

                XmlAttribute parentId = doc.CreateAttribute("parentId");
                parentId.Value = Convert.ToString(row["parentId"]);
                parameter.SetAttributeNode(parentId);

                XmlAttribute parentType = doc.CreateAttribute("parentType");
                parentType.Value = Convert.ToString(row["parentType"]);
                parameter.SetAttributeNode(parentType);

                XmlAttribute name = doc.CreateAttribute("name");
                name.Value = Convert.ToString(row["name"]);
                parameter.SetAttributeNode(name);

                XmlAttribute value = doc.CreateAttribute("value");
                value.Value = Convert.ToString(row["value"]);
                parameter.SetAttributeNode(value);

                XmlAttribute description = doc.CreateAttribute("description");
                description.Value = Convert.ToString(row["description"]);
                parameter.SetAttributeNode(description);

                parameters.AppendChild(parameter);
            }
            root.AppendChild(parameters);
            
        }

        #region IExportDataAdapter<T> Members

        abstract public T Process();

        #endregion
    }

    public class ControllerToXmlSettings : IExportDataSettings
    {
        public Controller controller;
        public RootController rootController;
        public Boolean treeDataFormat;

        public ControllerToXmlSettings(Controller controller, RootController rootController, Boolean treeDataFormat)
        {
            this.controller = controller;
            this.rootController = rootController;
            this.treeDataFormat = treeDataFormat;
        }

    }
}
