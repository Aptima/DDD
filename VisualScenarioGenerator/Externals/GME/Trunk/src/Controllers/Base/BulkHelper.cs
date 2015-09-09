using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using AME.Tools;
using System.Windows.Forms;
using AME.Views.View_Components;
using System.Threading;
using System.Xml.XPath;
using AME.Model;

namespace AME.Controllers.Base
{
    public class BulkHelper : IViewComponent
    {
        private XmlDocument iDoc;
        private XmlElement componentTable, linkTable, parameterTable;
        private IViewComponentHelper helper;

        private int idCounter;
        private string configurationName;
        private bool parametersPresent, linksPresent, waitingForUpdate;
        private Dictionary<int, int> existingIDs;
        private Dictionary<string, string> existingLinks;
        private Dictionary<string, string> nameToIDs;

        public BulkHelper()
        {
            helper = new NonControlViewComponentHelper(this);
        }

        public void UpdateViewComponent()
        {
            waitingForUpdate = false;
        }

        public IController Controller
        {
            get
            {
                return AMEManager.Instance.Get(configurationName);
            }
            set { } // ignore
        }

        public IViewComponentHelper IViewHelper
        {
            get { return helper; }
        }

        private string CreateKey(string parent, string child, string lt)
        {
            return parent + child + lt;
        }

        public void BeginBulkOperations(string p_configurationName)
        {
            existingIDs = new Dictionary<int, int>();
            existingLinks = new Dictionary<string, string>();
            nameToIDs = new Dictionary<string, string>();
            idCounter = 0;
            linksPresent = false;
            parametersPresent = false;

            configurationName = p_configurationName;

            iDoc = new XmlDocument();

            XmlElement database = iDoc.CreateElement("database");
            XmlElement configuration = iDoc.CreateElement("configuration");
            componentTable = iDoc.CreateElement("componentTable");
            linkTable = iDoc.CreateElement("linkTable");
            parameterTable = iDoc.CreateElement("parameterTable");

            XmlAttribute name = iDoc.CreateAttribute("name");
            name.Value = configurationName;
            XmlAttribute isRootController = iDoc.CreateAttribute("isRootController");
            isRootController.Value = "false";
            configuration.Attributes.Append(name);
            configuration.Attributes.Append(isRootController);

            database.AppendChild(configuration);
            configuration.AppendChild(componentTable);
            configuration.AppendChild(linkTable);
            configuration.AppendChild(parameterTable);

            iDoc.AppendChild(database);

            IController thisConfig = AMEManager.Instance.Get(configurationName);
            thisConfig.RegisterForUpdate(this);
        }

        public void EndBulkOperations()
        {
            EndBulkOperations(false, false, "", "");
        }

        private static volatile AutoResetEvent ev = new AutoResetEvent(false);
        private static volatile Boolean signaled = false;
        public static void Signal()
        {
            signaled = true;
            ev.Set();
        }

        public int EndBulkOperations(Boolean clearDatabase, Boolean showDialog, String title, String message)
        {
            int returnID = -1;

            if (idCounter > 0 || linksPresent || parametersPresent)
            {
                ImportTool importTool = new ImportTool();
                importTool.PutTypeInName = false;
                importTool.PutNameInDescription = false;

                Form importForm = Application.OpenForms[0]; // MW may need to fix this, this Form invokes the Dialog
                waitingForUpdate = true;
                signaled = false;

                bool importSuccess = importTool.Import(Controller, iDoc, this, importForm, clearDatabase, showDialog, title, message);

                while (waitingForUpdate) // pump the import and wait for controller to send UpdateViewComponent - will set updating to false
                {
                    Application.DoEvents();
                    if (showDialog)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
    
                        if (!signaled) // if already signaled, but waitingForUpdate is still true, don't wait again
                        {
                            ev.WaitOne();
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
                returnID = importTool.RootId;
                importTool = null;
            }
            IController thisConfig = AMEManager.Instance.Get(configurationName);
            thisConfig.UnregisterForUpdate(this);
            return returnID;
        }

        public bool ReferenceNameExists(string name)
        {
            return nameToIDs.ContainsKey(name);
        }

        public void AddReferenceID(string name, string id)
        {
            nameToIDs.Add(name, id);
        }

        public string GetReferenceID(string name)
        {
            if (nameToIDs.ContainsKey(name))
            {
                return nameToIDs[name];
            }
            else
            {
                MessageBox.Show("Reference name: " + name + " not found");
                return "-1";
            }
        }

        public bool IDExists(int id)
        {
            return existingIDs.ContainsKey(id);
        }

        public bool LinkExists(int parent, int child, string lt)
        {
            string key = CreateKey("" + parent, "" + child, lt);
            return existingLinks.ContainsKey(key);
        }


        public void AddExistingComponent(int ID, string type, string name, string desc)
        {
            CreateComponentXML("" + ID, type, name, desc, Component.eComponentType.Component.ToString(), "");
            existingIDs.Add(ID, ID);
        }

        public string CreateComponent(string type, string name, string desc)
        {
            return CreateComponent(type, name, desc, Component.eComponentType.Component.ToString(), "");
        }

        public string CreateRootComponent(string type, string name, string desc)
        {
            return CreateComponent(type, name, desc, Component.eComponentType.Component.ToString(), "true");
        }

        public string CreateClass(string type, string name, string desc)
        {
            return CreateComponent(type, name, desc, Component.eComponentType.Class.ToString(), "");
        }

        public string AddComponent(string type, string name, string desc, string topID, string parentID, string linktype)
        {
            return this.AddComponent(type, name, desc, Component.eComponentType.Component.ToString(), topID, parentID, linktype);
        }

        private string AddComponent(string type, string name, string desc, string etype, string topID, string parentID, string linktype)
        {
            string childID = this.CreateComponent(type, name, desc, etype, "");
            this.Connect(topID, parentID, childID, linktype);
            return childID;
        }

        private string CreateComponent(string type, string name, string desc, string etype, string root)
        {
            name = idCounter + ImportTool.Delimitter + name;
            idCounter++;
            CreateComponentXML("", type, name, desc, etype, root);
            return name;
        }

        public String Connect(string topID, string parentID, string childID, string linkType)
        {
            return CreateLink(topID, parentID, childID, linkType, "");
        }

        public String Connect(int topID, int parentID, int childID, string linkType)
        {
            return CreateLink("" + topID, "" + parentID, "" + childID, linkType, "");
        }

        public void CopyComponentParameters(XPathNavigator existingComponent, String newComponentID)
        {
            CopyComponentParameters(existingComponent, newComponentID, new List<String>());
        }

        public void CopyComponentParameters(XPathNavigator existingComponent, String newComponentID, List<String> ignore)
        {
            XPathNodeIterator parameterIterator = existingComponent.Select("ComponentParameters/Parameter/Parameter");
            foreach (XPathNavigator parameter in parameterIterator)
            {
                writeParameter(newComponentID, parameter, eParamParentType.Component, ignore);
            }
        }

        public void CopyLinkParameters(XPathNavigator existingComponent, String newComponentID)
        {
            CopyLinkParameters(existingComponent, newComponentID, new List<String>());
        }

        public void CopyLinkParameters(XPathNavigator existingComponent, String newLinkID, List<String> ignore)
        {
            XPathNodeIterator parameterIterator = existingComponent.Select("LinkParameters/Parameter/Parameter");
            foreach (XPathNavigator parameter in parameterIterator)
            {
                writeParameter(newLinkID, parameter, eParamParentType.Link, ignore);
            }
        }

        private void writeParameter(String newComponentID, XPathNavigator parameter, eParamParentType parent, List<String> ignore)
        {
            String category, pname, full, value;
            category = parameter.GetAttribute(ConfigFileConstants.category, String.Empty);
            pname = parameter.GetAttribute(ConfigFileConstants.displayedName, String.Empty);
            
            full = category + SchemaConstants.ParameterDelimiter + pname;
            value = parameter.GetAttribute(ConfigFileConstants.Value, String.Empty);

            if (ignore.Count == 0 || !ignore.Contains(full))
            {
                UpdateParameters(newComponentID, full, value, parent);
            }
        }

        public void UpdateParameters(string parentID, string paramName, string value, eParamParentType paramParType)
        {
            if (!parametersPresent)
            {
                parametersPresent = true;
            }
            CreateParameter(parentID, paramParType.ToString(), paramName, value, "");
        }

        private void CreateComponentXML(string id, string type, string name, string description, string eType, string root)
        {
            XmlElement component = iDoc.CreateElement("component");

            XmlAttribute att;
            if (id != String.Empty)
            {
                att = iDoc.CreateAttribute("id");
                att.Value = id;
                component.Attributes.Append(att);
            }

            if (root != String.Empty)
            {
                att = iDoc.CreateAttribute("root");
                att.Value = root;
                component.Attributes.Append(att);
            }

            att = iDoc.CreateAttribute("type");
            att.Value = type;
            component.Attributes.Append(att);

            att = iDoc.CreateAttribute("name");
            att.Value = name;
            component.Attributes.Append(att);

            att = iDoc.CreateAttribute("description");
            att.Value = description;
            component.Attributes.Append(att);

            att = iDoc.CreateAttribute("eType");
            att.Value = eType;
            component.Attributes.Append(att);

            componentTable.AppendChild(component);
        }

        private String CreateLink(string rootComponentId, string fromComponentId, string toComponentId, string type, string description)
        {
            string key = CreateKey(fromComponentId, toComponentId, type);
            if (!existingLinks.ContainsKey(key))
            {
                existingLinks.Add(key, key);
            }
            else
            {
                return "-1";
            }

            if (!linksPresent)
            {
                linksPresent = true;
            }

            XmlElement link = iDoc.CreateElement("link");

            XmlAttribute aRootComponentId = iDoc.CreateAttribute("rootComponentId");
            aRootComponentId.Value = rootComponentId;
            link.Attributes.Append(aRootComponentId);

            XmlAttribute aFromComponentId = iDoc.CreateAttribute("fromComponentId");
            aFromComponentId.Value = fromComponentId;
            link.Attributes.Append(aFromComponentId);

            XmlAttribute aToComponentId = iDoc.CreateAttribute("toComponentId");
            aToComponentId.Value = toComponentId;
            link.Attributes.Append(aToComponentId);

            XmlAttribute aType = iDoc.CreateAttribute("type");
            aType.Value = type;
            link.Attributes.Append(aType);

            XmlAttribute aDescription = iDoc.CreateAttribute("description");
            aDescription.Value = description;
            link.Attributes.Append(aDescription);

            linkTable.AppendChild(link);

            return key;
        }

        private void CreateParameter(string parentId, string parentType, string name, string value, string description)
        {
            if (!value.Equals(String.Empty))
            {
                XmlElement parameter = iDoc.CreateElement("parameter");

                XmlAttribute aParentId = iDoc.CreateAttribute("parentId");
                aParentId.Value = parentId;
                parameter.Attributes.Append(aParentId);

                XmlAttribute aParentType = iDoc.CreateAttribute("parentType");
                aParentType.Value = parentType;
                parameter.Attributes.Append(aParentType);

                XmlAttribute aName = iDoc.CreateAttribute("name");
                aName.Value = name;
                parameter.Attributes.Append(aName);

                XmlAttribute aValue = iDoc.CreateAttribute("value");
                aValue.Value = value;
                parameter.Attributes.Append(aValue);

                XmlAttribute aDescription = iDoc.CreateAttribute("description");
                aDescription.Value = description;
                parameter.Attributes.Append(aDescription);

                parameterTable.AppendChild(parameter);
            }
        }
    }
}
