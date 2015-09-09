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
    public partial class CustomNonnegativeDouble : TextBox, IViewComponent
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

        public double Value
        {
            get { return Double.Parse(Text); }
            set { if (value >= 0) Text = value.ToString(); }
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

        public void UpdateViewComponent()
        {
            String text = String.Empty;
            if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
            {
                IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                if (node != null)
                {
                    //get value from controller via database out of xml
                    text = node.GetAttribute("value", node.NamespaceURI);
                    try
                    {
                        double suppliedValue = double.Parse(text);
                        if (suppliedValue >= 0)
                            this.Text = text;
                    }
                    catch // if parse fails, e.g. empty string
                    {
                        this.Text = String.Empty;
                    }
                }
                else // if node doesn't exist
                {
                    this.Text = String.Empty;
                }
            }
        }

        #endregion

        public CustomNonnegativeDouble()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomNonnegativeDouble(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.TextAlign = HorizontalAlignment.Right;
        }

        // Type checking for keydown and leave?
        // Escape key should revert back - call update


        public void SetParameterValue()
        {
            if (controller != null)
            {
                try
                {
                    controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.Text, parameterType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                    this.UpdateViewComponent();
                }
            }
        }
        private void CustomNonnegativeDouble_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    SetParameterValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                    this.UpdateViewComponent();
                }
            }
        }

        private void CustomNonnegativeDouble_Leave(object sender, EventArgs e)
        {
            try
            {
                SetParameterValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");

                this.UpdateViewComponent();
            }
        }
        private void CustomNonnegativeDouble_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            char c = e.KeyChar;
            if (
                (!(Char.IsDigit(c) || ('.' == c)))
              || (('.' == c) && (Text.IndexOf('.') > -1))
               )
            {
                Console.Beep();
                e.Handled = true;
            }
            else if (Text != ".")
            {
                try
                {
                    Double.Parse(Text);
                }
                catch
                {
                    Console.Beep(); // Doesn't beep. Fix available
                    e.Handled = true;
                }
            }
        }

    }
}