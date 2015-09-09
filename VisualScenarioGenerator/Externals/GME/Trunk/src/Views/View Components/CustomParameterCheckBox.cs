using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;
using System.Reflection;

namespace AME.Views.View_Components
{
    public partial class CustomParameterCheckBox : CheckBox, IViewComponent
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
        private Boolean updating = false;

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
                            this.Checked = Boolean.Parse(text);
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
                            this.Checked = Boolean.Parse(text);
                        }
                    }
                }
            }
            else
            {
                this.Checked = false;
            }
            updating = false;
        }

        public CustomParameterCheckBox()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();

            this.CheckedChanged += new EventHandler(CustomParameterCheckBox_CheckedChanged);
        }

        private void CustomParameterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (controller != null)
            {
                if (!updating)
                {
                    try
                    {
                        String value = this.Checked.ToString();
                        controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, value, parameterType);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                        this.UpdateViewComponent();
                    }
                }
            }
        }

        public CustomParameterCheckBox(IContainer container)
            : this()
        {
            container.Add(this);
        }
    }
}