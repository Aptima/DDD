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
using System.Drawing;

namespace AME.Views.View_Components
{
    public partial class CustomParameterColorBox : Panel, IViewComponent
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
        private ColorConverter conv; 

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
            if (!updating)
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
                                this.BackColor = (Color)conv.ConvertFromString(text);
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
                                this.BackColor = (Color)conv.ConvertFromString(text);
                            }
                        }
                    }
                }
                else
                {
                    this.BackColor = Color.Gray;
                }
            }
        }

        public CustomParameterColorBox()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();

            conv = new ColorConverter();

            this.Size = new Size(20, 20);
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public CustomParameterColorBox(IContainer container)
            : this()
        {
            container.Add(this);
        }
    }
}