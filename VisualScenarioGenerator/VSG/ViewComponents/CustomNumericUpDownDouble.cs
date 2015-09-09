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
    public partial class CustomNumericUpDownDouble : NumericUpDown, IViewComponent
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
        private double valueMultiplier;

        public double ValueMultiplier
        {
            get
            {
                return valueMultiplier;
            }

            set
            {
                valueMultiplier = value;
            }
        }

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

        private bool updating = false;
        
        public void UpdateViewComponent()
        {
            updating = true;
            String text = String.Empty;
            if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
            {
                IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                if (node != null)
                {
                    text = node.GetAttribute("value", node.NamespaceURI);
                    try
                    {
                        int suppliedValue = (int)(double.Parse(text) * valueMultiplier);
                        
                        if ((suppliedValue >= this.Minimum) && (suppliedValue <= Maximum))
                        {
                            this.Value = suppliedValue;
                            this.Text = String.Format("{0}", suppliedValue);
                        }
                    }
                    catch // if text cannot be parsed, e.g. empty string
                    {
                        this.Text = String.Empty;
                    }
                }
                else // if node does not exist
                {
                    this.Text = String.Empty;
                }
            }
            updating = false;
        }

        #endregion

        public CustomNumericUpDownDouble()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
            this.TextAlign = HorizontalAlignment.Right;
            valueMultiplier = 1;
        }

        public CustomNumericUpDownDouble(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void SetParameterValue()
        {
            if (controller != null)
            {
                try
                {
                    if (this.Text != null) // push down text, not decimal value
                    {
                        string text = String.Format("{0}",(double)this.Value /valueMultiplier);
                        controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, text, parameterType);
                    }
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
        private void CustomNumericUpDownDouble_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetParameterValue();
            }
        }

        private void CustomNumericUpDownDouble_Leave(object sender, EventArgs e)
        {
            SetParameterValue();
        }

        private void CustomNumericUpDownDouble_ValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                // set text to value
                if (this.Value >= this.Minimum && this.Value <= this.Maximum)
                {
                    this.Text = this.Value.ToString();
                    SetParameterValue();
                }
            }
        }
    }
}