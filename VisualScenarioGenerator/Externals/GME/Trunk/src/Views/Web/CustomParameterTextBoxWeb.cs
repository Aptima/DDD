using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml;
using System.Xml.XPath;

namespace AME.Views.View_Components.Web
{
    public partial class CustomParameterTextBoxWeb : System.Web.UI.WebControls.TextBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController controller;
        private Int32 componentId = -1;
        private String parameterCategory;
        private String parameterName;
        private Boolean useDelimiter = false;

        private eParamParentType parameterType;

        public String ParameterCategory
        {
            get
            {
                return parameterCategory;
            }

            set
            {
                parameterCategory = value;
            }
        }

        public String ParameterName
        {

            get
            {
                return parameterName;
            }

            set
            {
                parameterName = value;
            }
        }

        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }

            set
            {
                componentId = value;
            }
        }

        public eParamParentType ParameterType
        {
            get
            {
                return parameterType;
            }

            set
            {
                parameterType = value;
            }
        }

        public Boolean UseDelimiter
        {
            get
            {
                return useDelimiter;
            }

            set
            {
                useDelimiter = value;
            }
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            if (componentId >= 0)
            {
                String text = String.Empty;
                if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
                {
                    if (parameterType.Equals(eParamParentType.Component))
                    {
                        IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                        XPathNavigator nav = inav.CreateNavigator();
                        XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                        if (node != null)
                        {
                            text = node.GetAttribute("value", String.Empty);
                            //if (this.Multiline & useDelimiter)
                            //    text = text.Replace(delimiter, System.Environment.NewLine);
                        }
                    }
                    else if (parameterType.Equals(eParamParentType.Link))
                    {
                        IXPathNavigable inav = controller.GetParametersForLink(componentId);
                        XPathNavigator nav = inav.CreateNavigator();
                        XPathNavigator node = nav.SelectSingleNode(String.Format("LinkParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                        if (node != null)
                        {
                            text = node.GetAttribute("value", String.Empty);
                            //if (this.Multiline & useDelimiter)
                            //    text = text.Replace(delimiter, System.Environment.NewLine);
                        }
                    }
                }
                this.Text = text;
            }
            else
                this.Text = String.Empty;
        }

        #endregion

        private void InitializeComponent()
        {
            //this.SuspendLayout();
            // 
            // CustomTextBox
            // 
            this.TextChanged += new System.EventHandler(this.CustomTextBox_Leave);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomTextBox_KeyDown);
            //this.ResumeLayout(false);
        }

        public CustomParameterTextBoxWeb()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomParameterTextBoxWeb(IContainer container) : this()
        {
            container.Add(this);
        }

        public void SetParameterValue()
        {
            if (controller != null)
            {
                try
                {
                    //String text = this.Text;
                    //if (this.Multiline & useDelimiter)
                    //    text = text.Replace(System.Environment.NewLine, delimiter);
                    controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.Text, parameterType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                    this.UpdateViewComponent();
                }
            }
        }

        // Type checking for keydown and leave?
        // Escape key should revert back - call update
        protected virtual void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetParameterValue();
            }
            //else if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            //{
            //    if (this.Multiline && useDelimiter)
            //        SetParameterValue();
            //}
        }

        protected virtual void CustomTextBox_Leave(object sender, EventArgs e)
        {
            SetParameterValue();
        }
    }
}