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
    public partial class CustomDateTimePicker : DateTimePicker, IViewComponent
    {
        private ViewComponentHelper myHelper;
        private IController controller;
        private Int32 componentId = -1;
        private String parameterCategory;
        private String parameterName;
        private eParamParentType parameterType;
        private Boolean reading = false;
        private String previous = String.Empty;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
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

        public CustomDateTimePicker()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomDateTimePicker(IContainer container)
        {
            container.Add(this);

            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
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
            reading = true;

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
                            //if (formatType == null)
                            //{
                            //    String paramType = node.GetAttribute("type", String.Empty);
                            //    formatType = Type.GetType(paramType);
                            //}
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
                            //if (formatType == null)
                            //{
                            //    String paramType = node.GetAttribute("type", String.Empty);
                            //    formatType = Type.GetType(paramType);
                            //}
                        }
                    }
                }
                //if (displayFormatter != null)
                //{
                //    if (formatType != null)
                //    {
                //        text = String.Format(displayFormatter, formatString, Convert.ChangeType(text, formatType));
                //    }
                //}
                this.Text = text;
                previous = this.Text;
            }
            else
            {
                this.Text = String.Empty;
            }
            reading = false;
        }

        #endregion

        public void SetParameterValue()
        {
            if (reading || componentId <= 0)
                return;

            if (controller != null)
            {
                try
                {
                    //String text = this.Text;
                    //if (this.Multiline & useDelimiter)
                    //    text = text.Replace(System.Environment.NewLine, delimiter);
                    String value = this.Text;
                    if (!previous.Equals(value))
                    {
                        //if (numberStyle != NumberStyles.None)
                        //{
                        //    if (formatType != null)
                        //    {
                        //        MethodInfo mi = formatType.GetMethod("Parse",     // e.g. "Int32.Parse, NumberStyles enum provides formating info
                        //            new Type[] { stringType, numberStylesType });

                        //        Object invokeReturn = mi.Invoke(formatType, new object[] { value, numberStyle });
                        //        value = invokeReturn.ToString();
                        //    }
                        //}

                        controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, value, parameterType);
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

        private void CustomDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            SetParameterValue();
        }

        private void CustomDateTimePicker_Leave(object sender, EventArgs e)
        {
            SetParameterValue();
        }

        private void CustomDateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetParameterValue();
            }
        }
    }
}
