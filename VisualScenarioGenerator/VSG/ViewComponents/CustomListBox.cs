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
    public partial class CustomListBox : ListBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController _controller;
        private Int32 displayId = -1;
        private String displayLinkType = "";
        private String displayComponentType = "";
        private string IDAttribute, NameAttribute;
        public int DisplayID
        {
            get
            {
                return displayId;
            }
            set
            {
                displayId = value;
            }
        }

        public String DisplayLinkType
        {
            get
            {
                return displayLinkType;
            }
            set
            {
                displayLinkType = value;
            }
        }
        public String DisplayComponentType
        {
            get
            {
                return displayComponentType;
            }
            set
            {
                displayComponentType = value;
            }
        }
        public CustomListBox() : base()
        {
            myHelper = new ViewComponentHelper(this);

            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;
        }

        public AME.Controllers.IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        public void UpdateViewComponent()
        {
            Dictionary<int, ListBoxItem> items = new Dictionary<int, ListBoxItem>();
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;

            IXPathNavigable document = _controller.GetComponentAndChildren(DisplayID, DisplayLinkType, compOptions);
            XPathNavigator navigator = document.CreateNavigator();


            DrawingUtility.SuspendDrawing(this);

            this.Items.Clear();

            XPathNodeIterator listofRootElements;


            //listofRootElements = navigator.Select(String.Format("/Components/Component/Component"));
            listofRootElements = navigator.Select(String.Format(
                        "/Components/Component/Component[@Type='{0}' or @BaseType='{0}']",
                        DisplayComponentType
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

                    ListBoxItem item = new ListBoxItem(nodeName, nodeID);
                    //this.Items.Add(item);
                    this.Items.Add(item);

                    items[item.MyID] = item;

                }

            }

            
            DrawingUtility.ResumeDrawing(this);

        }
    }
    public class ListBoxItem
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

        public ListBoxItem(String compName, int compID)
        {
            myName = compName;
            myID = compID;
        }

        public override String ToString()
        {
            return myName;
        }
    }
}
