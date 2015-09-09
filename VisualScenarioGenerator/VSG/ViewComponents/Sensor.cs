using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using VSG.Controllers;
using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class Sensor : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 sensorId = -1;
        private VSGController controller;
        private string sensorName;
        private static bool populating = false;
        private static bool updating = false;

        private AME.Views.View_Components.CustomRadioButton  previous = null;

        public Int32 SensorId
        {
            get { return sensorId; }
            set
            {
                sensorId = value;
                if (sensorId > 0)
                {
                    this.customRadioButton1.ComponentId = sensorId;
                    this.customRadioButton2.ComponentId = sensorId;
                    this.customRadioButton3.ComponentId = sensorId;
                    nndRange.ComponentId = sensorId;
                    customParameterEnumBox1.ComponentId = sensorId;
                    UpdateViewComponent();
                    PopulateCustomAttributes();
                }
            }
        }
        public Sensor()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
            previous = customRadioButton1;
        }

        private void PopulateCustomAttributes()
        {
            populating = true;

            listBoxCustomAttributes.Items.Clear();

            IXPathNavigable iNavSensor = controller.GetParametersForComponent(sensorId);
            XPathNavigator navSensor = iNavSensor.CreateNavigator();
            //XPathNavigator sensorAttributeParameter = navSensor.SelectSingleNode("ComponentParameters/Parameter[@category='Engram']/Parameter[@displayedName='Initial Value']");
            //if (sensorAttributeParameter != null)
            //{
            //String sensorAttribute = sensorAttributeParameter.GetAttribute("value", navSensor.NamespaceURI);

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 4;
            compOptions.CompParams = true;
            IXPathNavigable iNavScenario = controller.GetComponentAndChildren(controller.ScenarioId, controller.ConfigurationLinkType, compOptions);
            XPathNavigator navScenario = iNavScenario.CreateNavigator();
            XPathNodeIterator itNavEmitters = navScenario.Select("/Components/Component/Component[@Type='Engram']");///Component[@Type='State']/Component[@Type='Emitter']");
            while (itNavEmitters.MoveNext())
            {
                String engramName = itNavEmitters.Current.GetAttribute("Name", navScenario.NamespaceURI);
                if (!listBoxCustomAttributes.Items.Contains(engramName))
                {
                    listBoxCustomAttributes.Items.Add(engramName);
                }
                //XPathNavigator emitterAttributeParameter = itNavEmitters.Current.SelectSingleNode("ComponentParameters/Parameter[@category='Engram']");
                //if (emitterAttributeParameter != null)
                //{
                //    String emitterAttribute = emitterAttributeParameter.GetAttribute("value", navScenario.NamespaceURI);

                //    //if (sensorAttribute.Equals(emitterAttribute))
                //    //{
                //    //    XPathNodeIterator itNavLevels = itNavEmitters.Current.Select("Component[@Type='Level']");
                //    //    while (itNavLevels.MoveNext())
                //    //    {
                //    //        String levelName = itNavLevels.Current.GetAttribute("Name", navScenario.NamespaceURI);
                //    //        if (!comboBox1.Items.Contains(levelName))
                //    //            comboBox1.Items.Add(levelName);
                //    //    }
                //    //}
                //}
            }
            //}

            SelectItem();

            populating = false;
        }

        private void SelectItem()
        {
            if (listBoxCustomAttributes.Items.Count > 0)
            {
                String text = String.Empty;

                IXPathNavigable inav = controller.GetParametersForComponent(sensorId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode("ComponentParameters/Parameter[@category='Sensor']/Parameter[@displayedName='Custom_Attribute']");
                if (node != null)
                {
                    text = node.GetAttribute("value", String.Empty);
                    if (!text.Equals(String.Empty))
                    {
                        Int32 index = this.listBoxCustomAttributes.FindStringExact(text);
                        if (index == -1) // No match so default to first item.
                            index = 0;
                        this.listBoxCustomAttributes.SelectedItem = listBoxCustomAttributes.Items[index];
                    }
                }
            }
        }

        public String SensorName
        {
            get
            {
                return sensorName;
            }
            set
            {
                sensorName = value;
                customTabPage1.Description = sensorName;
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
                controller = (VSGController)value;  // Only give this a vsg controller.

                nndRange.Controller = controller;
                this.customRadioButton1.Controller = controller;
                this.customRadioButton2.Controller = controller;
                this.customRadioButton3.Controller = controller;
                customParameterEnumBox1.Controller = controller;


            }
        }

        public void UpdateViewComponent()
        {
            updating = true;
            PopulateCustomAttributes();
            nndRange.UpdateViewComponent();
            this.customRadioButton1.UpdateViewComponent();
            this.customRadioButton2.UpdateViewComponent();
            this.customRadioButton3.UpdateViewComponent();
            customParameterEnumBox1.UpdateViewComponent();
            updating = false;
        }

        #endregion


        private void customRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = customRadioButton1.Checked;
            nndRange.Enabled = customRadioButton1.Checked;
            customParameterEnumBox1.Enabled = customRadioButton2.Checked;
            listBoxCustomAttributes.Enabled = customRadioButton3.Checked;

            if (customRadioButton1.Checked)
            {
                    ComponentOptions c = new ComponentOptions();
                    c.CompParams = true;
                    IXPathNavigable iNavParameters = controller.GetComponentAndChildren(sensorId, "Scenario", c);


                    XPathNavigator navParameters = iNavParameters.CreateNavigator();
                    XPathNodeIterator itr = navParameters.Select(".//Component[@Type='SensorRange']");
                    if (itr.Count > 0)
                    {
                        if (MessageBox.Show("Selecting this option will delete this sensor's existing Sensor Ranges.\nDo you want to continue?", "Delete Sensor Ranges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                            DialogResult.Yes)
                        {
                            foreach (XPathNavigator node in itr)
                            {
                                String attributeValue = node.GetAttribute("ID", node.NamespaceURI);
                                if (attributeValue != string.Empty)
                                {
                                    controller.DeleteComponent(Convert.ToInt32(attributeValue));
                                }
                            }
                        }
                        else
                        {
                            previous.Checked = true;
                        }
                    }
   
            }
            previous = customRadioButton1;
        }

        private void customRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = customRadioButton1.Checked;
            nndRange.Enabled = customRadioButton1.Checked;
            customParameterEnumBox1.Enabled = customRadioButton2.Checked;
            listBoxCustomAttributes.Enabled = customRadioButton3.Checked;

            previous = customRadioButton2;
        }

        private void customRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = customRadioButton1.Checked;
            nndRange.Enabled = customRadioButton1.Checked;
            customParameterEnumBox1.Enabled = customRadioButton2.Checked;
            listBoxCustomAttributes.Enabled = customRadioButton3.Checked;

            previous = customRadioButton3;
        }

        private void listBoxCustomAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!populating & !updating)
            //{
            //    try
            //    {
            //        controller.UpdateParameters(sensorId, "Sensor.Custom_Attribute", this.listBoxCustomAttributes.SelectedItem.ToString(), eParamParentType.Component);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
            //    }
            //}
        }

        private void listBoxCustomAttributes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (/*!populating &*/ !updating)
            {
                try
                {
                    controller.UpdateParameters(sensorId, "Sensor.Custom_Attribute", this.listBoxCustomAttributes.SelectedItem.ToString(), eParamParentType.Component);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }







    }
}
