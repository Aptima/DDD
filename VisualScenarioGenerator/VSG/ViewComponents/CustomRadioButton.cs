using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml;
using System.Xml.XPath;

namespace AME.Views.View_Components
{
    public partial class CustomRadioButton : RadioButton, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController controller;
        private Int32 componentId;
        private String parameterCategory;
        private String parameterName;
        private eParamParentType parameterType;
        private bool updating = false;

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

        public eParamParentType SelectedIDType
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
            updating = true;
            String text = String.Empty;
            if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
            {
                IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                if (node!= null) 
                {
                    text = node.GetAttribute("value", String.Empty);
                }
            }
            this.Checked = false;
            if ("true" == text.ToLower())
                this.Checked = true;

            updating = false;
        }

        #endregion

        public CustomRadioButton()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomRadioButton(IContainer container)
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            container.Add(this);

            InitializeComponent();
        }
        public void SetParameterValue()
        {
            if (controller != null)
            {
                try
                {
                    controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.Checked.ToString().ToLower(), parameterType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                    this.UpdateViewComponent();
                }
            }
        }

        private void CustomRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                SetParameterValue();
            }
        }
        
 
    }
}