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

namespace AME.Views.View_Components
{
    public partial class CustomCombo : ComboBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController myController;

        private int m_DisplayID = -1;

        public int DisplayID
        {
            get {
                return m_DisplayID;
            }
            set { m_DisplayID = value; }
        }

        private String type = "";

        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        private int selectedID = -1;

        public int SelectedID
        {
            get { return selectedID; }
            set { selectedID = value; }
        }

        private Boolean useParameter = false;

        public Boolean UseParameter
        {
            set { useParameter = value; }
        }

        private String parameterName;

        public String ParameterName
        {
            set { parameterName = value; }
        }

        public delegate void SelectedIDChanged(CustomCombo source, int ID, String Name);

        // Define an event based on the above delegate
        public event SelectedIDChanged SelectedIDChangedEvent;

        private string IDAttribute, NameAttribute;

        public CustomCombo() : base()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Component);

            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;

            this.DropDownStyle = ComboBoxStyle.DropDownList; // non editable list
            this.SelectionChangeCommitted += new EventHandler(CustomCombo_SelectionChangeCommitted);
        }

        private void CustomCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // internally synchronize on selection event
            if (this.SelectedItem != null)
            {
                ComboItem cast = (ComboItem)this.SelectedItem;
                if (this.SelectedID != cast.MyID)
                {
                    this.selectedID = cast.MyID;
                    if (SelectedIDChangedEvent != null)
                    {
                        SelectedIDChangedEvent(this, cast.MyID, cast.MyName);
                    }
                }
            }
        }

        #region ViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        private String m_LinkType;

        public string LinkType
        {
            get { return m_LinkType; }
            set { m_LinkType = value; }
        }

        public void UpdateViewComponent()
        {
            if (myController == null)
                return;

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;
            compOptions.InstanceUseClassName = true;

            IXPathNavigable document = myController.GetComponentAndChildren(m_DisplayID, m_LinkType, compOptions);
            XPathNavigator navigator = document.CreateNavigator();

            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //Byte[] bs = System.Text.Encoding.UTF8.GetBytes(navigator.OuterXml);
            //bs = x.ComputeHash(bs);
            //System.Text.StringBuilder s = new System.Text.StringBuilder();
            //foreach (Byte b in bs)
            //{
            //    s.Append(b.ToString("x2").ToLower());
            //}
            //String newCrc = s.ToString();

            //if (!crc.Equals(newCrc)) // CRC check - see CustomTreeView and ParameterTable
            {
                DrawingUtility.SuspendDrawing(this);

                this.Items.Clear();

                XPathNodeIterator listofRootElements; 

                if (type != "")
                {
                    listofRootElements  = navigator.Select(String.Format(
                        "/Components/Component/Component[@Type='{0}' or @BaseType='{0}']",
                        type 
                        ));
                }
                else
                {
                    listofRootElements = navigator.Select(String.Format("/Components/Component/Component"));
                }

                if (listofRootElements != null)
                {
                    bool selectedFound = false;

                    String nodeName;
                    int nodeID;

                    foreach (XPathNavigator paramNav in listofRootElements)
                    {
                        //add the name/id paired item.

                        if (useParameter)
                        {
                            nodeName = paramNav.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='" + parameterName + "']/@value").Value;
                        }
                        else
                        {
                            nodeName = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                        }
                        nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));

                        ComboItem item = new ComboItem(nodeName, nodeID);
                        this.Items.Add(item);

                        if (item.MyID.Equals(selectedID))  // restore selection
                        {
                            selectedFound = true;
                            this.SelectedItem = item;
                        }
                    }
                    if (selectedFound == false)
                    {
                        this.SelectedValue = null;
                    }
                }

                DrawingUtility.ResumeDrawing(this);
            }
        }
        #endregion

		/// <summary>
		/// This is an "Unbounded" version of the UpdateViewComponent() method.  The difference is that here
		/// instead of fetching only children of specific type we are fetching all descendants of the root component.
		/// It became necessary when components of the same type are linked hierarchically, one under another and NOT
		/// as a list when all of them are linked to the same root.
		/// </summary>
		public void UnboundedUpdateViewComponent() {
			ComponentOptions compOptions = new ComponentOptions();
			compOptions.LevelDown = 3;
			compOptions.InstanceUseClassName = true;

			IXPathNavigable document = myController.GetComponentAndChildren(m_DisplayID, m_LinkType, compOptions);
			XPathNavigator navigator = document.CreateNavigator();

			//MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
			//Byte[] bs = System.Text.Encoding.UTF8.GetBytes(navigator.OuterXml);
			////bs = x.ComputeHash(bs);
			//System.Text.StringBuilder s = new System.Text.StringBuilder();
			//foreach (Byte b in bs) {
			//	s.Append(b.ToString("x2").ToLower());
			//}
			//String newCrc = s.ToString();

			//if (!crc.Equals(newCrc)) // CRC check - see CustomTreeView and ParameterTable
            {
				DrawingUtility.SuspendDrawing(this);

				this.Items.Clear();

				XPathNodeIterator listofRootElements;

				if (type != "") {
					listofRootElements  = navigator.Select(String.Format(
						"//Component[@Type='{0}' or @BaseType='{0}']",
						type
						));
				}
				else {
					listofRootElements = navigator.Select(String.Format("//Component"));
				}

				if (listofRootElements != null) {
					bool selectedFound = false;

					String nodeName;
					int nodeID;

					foreach (XPathNavigator paramNav in listofRootElements) {
						//add the name/id paired item.
						nodeName = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
						nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));

						ComboItem item = new ComboItem(nodeName, nodeID);
						this.Items.Add(item);

						if (item.MyID.Equals(selectedID))  // restore selection
                        {
							selectedFound = true;
							this.SelectedItem = item;
						}
					}
					if (selectedFound == false) {
						this.SelectedValue = null;
					}
				}

				DrawingUtility.ResumeDrawing(this);
			}
		}
	}

    // A custom combo item that we use to populate custom combo boxes.  This
    // object stores a string and an int which refer to a project/mission/org name
    // and the component id that name maps to.
    public class ComboItem
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

        public ComboItem(String comboName, int comboID)
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
