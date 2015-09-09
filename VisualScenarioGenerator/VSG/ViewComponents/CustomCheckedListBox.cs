using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Data;
using AME.Controllers;
using AME.Model;
using System.Xml;
using System.Security.Cryptography;
using System.Xml.XPath;

using AME.Views.View_Components;
using VSG.Controllers;
namespace VSG.ViewComponents
{
    public partial class CustomCheckedListBox : CheckedListBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController m_controller;
        private Int32 m_sourceParentComponentId = -1;
        private Int32 m_destParentComponentId = -1;
        private String m_sourceLinkType = "";
        private String m_destLinkType = "";
        private String m_childComponentType = "";
        //private String m_childNamePropertyCat = "";
        //private String m_childNamePropertyName = "";

        //private String crc = "";
        private string IDAttribute, NameAttribute;

        public int SourceParentComponentId
        {
            get
            {
                return m_sourceParentComponentId;
            }
            set { m_sourceParentComponentId = value; }
        }
        public int DestParentComponentId
        {
            get
            {
                return m_destParentComponentId;
            }
            set { m_destParentComponentId = value; }
        }
        public String SourceLinkType
        {
            get
            {
                return m_sourceLinkType;
            }
            set { m_sourceLinkType = value; }
        }
        public String DestLinkType
        {
            get
            {
                return m_destLinkType;
            }
            set { m_destLinkType = value; }
        }
        public String ChildComponentType
        {
            get
            {
                return m_childComponentType;
            }
            set { m_childComponentType = value; }
        }
        /*
        public String ChildNamePropertyCat
        {
            get
            {
                return m_childNamePropertyCat;
            }
            set { m_childNamePropertyCat = value; }
        }

        public String ChildNamePropertyName
        {
            get
            {
                return m_childNamePropertyName;
            }
            set { m_childNamePropertyName = value; }
        }
        */
        public CustomCheckedListBox() : base()
        {
            myHelper = new ViewComponentHelper(this);

            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;
        }

        public AME.Controllers.IController Controller
        {
            get
            {
                return m_controller;
            }
            set
            {
                m_controller = value;
            }
        }
        public void UpdateViewComponent()
        {
            Dictionary<int, CheckboxItem> items = new Dictionary<int, CheckboxItem>();
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;

            IXPathNavigable document = m_controller.GetComponentAndChildren(m_sourceParentComponentId, m_sourceLinkType, compOptions);
            XPathNavigator navigator = document.CreateNavigator();


            DrawingUtility.SuspendDrawing(this);

            this.Items.Clear();

            XPathNodeIterator listofRootElements;


            //listofRootElements = navigator.Select(String.Format("/Components/Component/Component"));
            listofRootElements = navigator.Select(String.Format(
                        "/Components/Component/Component[@Type='{0}' or @BaseType='{0}']",
                        "DecisionMaker"
                        ));


            // Add the checkbox items
            if (listofRootElements != null)
            {
                //bool selectedFound = false;

                String nodeName;
                int nodeID;

                foreach (XPathNavigator paramNav in listofRootElements)
                {
                    //add the name/id paired item.
                    nodeName = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                    nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));

                    CheckboxItem item = new CheckboxItem(nodeName, nodeID);
                    //this.Items.Add(item);
                    this.Items.Add(item,((VSGController)m_controller).IsLinked(nodeID,m_destParentComponentId,m_destLinkType));

                    items[item.MyID] = item;

                }

            }

            // Select the items that are connected to the destination
            /*
            IXPathNavigable document2 = m_controller.GetComponentAndChildren(m_destParentComponentId, m_destLinkType, compOptions);
            XPathNavigator navigator2 = document2.CreateNavigator();
            XPathNodeIterator listofRootElements2;
            listofRootElements2 = navigator2.Select(String.Format("/Components/Component/Component"));
            foreach (XPathNavigator paramNav in listofRootElements2)
            {
                String nodeName2;
                int nodeID2;
                nodeName2 = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                nodeID2 = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));
                if (items.ContainsKey(nodeID2))
                {
                    int i = this.Items.IndexOf(items[nodeID2]);
                    this.SetItemChecked(i, true);
                }
            }
             */
            DrawingUtility.ResumeDrawing(this);

        }
    }
    public class CheckboxItem
    {
        private String myName;
        public String MyName
        {
            get { return myName; }
            set { myName = value; }
        }

        private int myID;
        public int MyID
        {
            get { return myID; }
            set { myID = value; }
        }

        public CheckboxItem(String comboName, int comboID)
        {
            myName = comboName;
            myID = comboID;
        }

        public override String ToString()
        {
            return myName;
        }
    }
}
