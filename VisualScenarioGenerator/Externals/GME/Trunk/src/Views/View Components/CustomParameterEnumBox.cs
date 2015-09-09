using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;

using AME.Controllers;
using System.Drawing;

namespace AME.Views.View_Components
{
    public partial class CustomParameterEnumBox : ListBox, IViewComponent
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
        private String componentName;
        private String componentType;
        private String enumName;
        private Boolean updating = false;
        private Boolean populating = false;
        private Boolean isColorEnum = false;

        public Boolean IsColorEnum
        {
            get
            {
                return isColorEnum;
            }

            set
            {
                isColorEnum = value;
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
                if (controller != null && componentId >= 0)
                {
                    XPathNavigator componentNav = controller.GetComponent(componentId).CreateNavigator().SelectSingleNode(String.Format("Components/Component[@ID='{0}']", componentId));
                    if (componentNav != null)
                    {
                        componentName = componentNav.GetAttribute("Name", String.Empty);
                        componentType = componentNav.GetAttribute("Type", String.Empty);

                        populateEnumList();
                    }
                }
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

        public String EnumName
        {

            get
            {
                return enumName;
            }

            set
            {
                enumName = value;
            }
        }

        public CustomParameterEnumBox()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();
        }

        public CustomParameterEnumBox(IContainer container)
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            container.Add(this);

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
            updating = true;

            selectEnumItem();
            
            updating = false;
        }

        #endregion

        private void populateEnumList()
        {
            populating = true;
            DrawingUtility.SuspendDrawing(this);

            this.SelectedItem = -1;
            this.DataSource = null;
            //this.Items.Clear();

            Type enumType = AMEManager.GetType(enumName, componentType);

            Array values = Enum.GetValues(enumType);

            this.DataSource = values;

            DrawingUtility.ResumeDrawing(this);
            populating = false;
        }

        private void selectEnumItem()
        {
            if (Items.Count > 0)
            {
                String text = String.Empty;
                if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
                {
                    IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                    XPathNavigator nav = inav.CreateNavigator();
                    XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                    if (node != null)
                    {
                        text = node.GetAttribute("value", String.Empty);
                        if (!text.Equals(String.Empty))
                        {
                            Int32 index = this.FindStringExact(text);
                            if (index == -1) // No match so default to first item.
                            {
                                if (IsColorEnum) // try one last thing for colors
                                {
                                    Object convert = new ColorConverter().ConvertFromString(text);
                                    if (convert != null)
                                    {
                                        Color convertAsColor = (Color)convert;
                                        if (convertAsColor.IsNamedColor)
                                        {
                                            index = this.FindStringExact(convertAsColor.Name);
                                        }
                                    }
                                }
                            }
 
                            if (index != -1)
                            {
                                this.SelectedItem = Items[index];
                            }
                            else
                            {
                                index = 0;
                                this.SelectedItem = Items[index];
                                // Also set the db to this default value.
                                setSelected();
                            }
                        }
                    }
                }
            }
        }

        private void setSelected()
        {
            controller.TurnViewUpdateOff();
            try
            {
                controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.SelectedItem.ToString(), parameterType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
            }
            controller.TurnViewUpdateOn(false, false);
        }

        private void CustomParameterEnumBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!populating & !updating && componentId >= 0) // don't push on negative ID
            {
                try
                {
                    controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, this.SelectedItem.ToString(), parameterType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }
    }
}
