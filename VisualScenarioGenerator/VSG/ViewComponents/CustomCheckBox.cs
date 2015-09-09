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
    public partial class CustomCheckBox : CheckBox, IViewComponent
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

        private Boolean updating = false;

        public void UpdateViewComponent()
        {
            updating = true;

            String text = String.Empty;
            if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
            {
                IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                XPathNavigator navigator = inav.CreateNavigator();

                XPathNavigator node = navigator.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                if (node!= null) 
                {
                    text = node.GetAttribute("value", String.Empty);
                }
            }
            
            this.Checked = false;
            if ("true" == text.ToLower())
            {
                this.Checked = true;
            }

            updating = false;

        }

        #endregion

        public CustomCheckBox()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomCheckBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void CustomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                if (controller != null)
                {
                    controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.Checked.ToString().ToLower(), parameterType);
                }
            }
        }

        
    }
}