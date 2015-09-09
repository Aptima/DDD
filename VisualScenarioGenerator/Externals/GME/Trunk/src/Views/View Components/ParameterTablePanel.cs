using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Xml.XPath;
using AME.Model;
using AME;
using System.Xml;
using System.Windows.Forms.Design;
using AME.Views.View_Component_Packages;

namespace AME.Views.View_Components {

    public partial class ParameterTablePanel : ViewComponentPanel, IViewComponent {

        private ViewComponentHelper myHelper;
        private String description = String.Empty;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public ParameterTablePanel() {

            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the text for the label.  If the text is an empty string or
        /// null, the label becomes infisible.
        /// </summary>
        /// <value>The text for the label.</value>
        public String Label {
            get {
                return this.label.Text;
            }
            set {
                if (value == null || value.Length == 0) {
                    this.label.Text = String.Empty;
                    this.label.Visible = false;
                }
                else {
                    this.label.Text = value;
                    this.label.Visible = true;
                }
            }
        }

        #region ParameterTable Members

        public int SelectedID {
            get {
                return this.customPropertyGrid1.SelectedID;
            }
            set {
                this.customPropertyGrid1.SelectedID = value;
            }
        }

        public eParamParentType SelectedIDType {
            get {
                return this.customPropertyGrid1.SelectedIDType;
            }
            set {
                this.customPropertyGrid1.SelectedIDType = value;
            }
        }

        public IController Controller {
            get {
                return this.customPropertyGrid1.Controller;
            }
            set {
                this.customPropertyGrid1.Controller = value;
            }
        }

        public void UpdateViewComponent() 
        {
            //update child grid
            this.customPropertyGrid1.UpdateViewComponent();

            if (this.SelectedID < 1)
            {
                this.Label = null;
            }
            else
            {
                switch (this.customPropertyGrid1.SelectedIDType)
                {
                   case eParamParentType.Component:
                        if (this.customPropertyGrid1.Blank == false)
                        {
                            try
                            {
                                IXPathNavigable doc = this.Controller.GetComponent(this.SelectedID);
                                XPathNavigator navigator = doc.CreateNavigator().SelectSingleNode("/Components/Component");
                                String type = navigator.GetAttribute("Type", navigator.NamespaceURI);
                                String name = navigator.GetAttribute("Name", navigator.NamespaceURI);
                                if (String.IsNullOrEmpty(description))
                                {
                                    this.Label = String.Format("{0}: {1}", type, name);
                                }
                                else
                                {
                                    this.Label = String.Format("{0}: {1} {2}", type, name, description);
                                }
                            }
                            catch (NullReferenceException)
                            {
                                this.SelectedID = -1;
                                this.customPropertyGrid1.UpdateViewComponent();
                                this.Label = null;
                            }
                        }
                        else
                        {
                            this.Label = null;
                        }
                        break;
                    case eParamParentType.Link:
                        if (this.customPropertyGrid1.Blank == false)
                        {
                            IXPathNavigable linkDocument = this.Controller.GetLink(SelectedID, true);
                            XPathNavigator linkDocumentNav = linkDocument.CreateNavigator();

                            XPathNavigator link = linkDocumentNav.SelectSingleNode("/Links/Link");

                            if (link != null)
                            {
                                String fromName = link.GetAttribute(XmlSchemaConstants.Display.Link.FromName, link.NamespaceURI);
                                String fromType = link.GetAttribute(XmlSchemaConstants.Display.Link.FromType, link.NamespaceURI);

                                String toName = link.GetAttribute(XmlSchemaConstants.Display.Link.ToName, link.NamespaceURI);
                                String toType = link.GetAttribute(XmlSchemaConstants.Display.Link.ToType, link.NamespaceURI);

                                this.Label = "Link from "+fromType+" "+fromName+" to "+toType+" "+toName+" Properties:";
                            }
                        }
                        else
                        {
                            this.Label = null;
                        }
                        break;
                } // switch
            } // valid id
        } // UpdateViewComponent
        #endregion
    }
}

