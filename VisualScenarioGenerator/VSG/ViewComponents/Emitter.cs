using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using AME.Controllers;

using System.Xml;
using System.Xml.XPath;
using AME.Views.View_Components;


namespace VSG.ViewComponents
{
    public partial class Emitter : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 emitterId = -1;
        private VSGController controller;
        private String emitterName;

        private int previous_index = 0;
        private bool updating = false;
        private bool populating = false;

        public Int32 EmitterId
        {
            get { return emitterId; }
            set
            {
                if (emitterId != value)
                {
                    emitterId = value;
                    if (emitterId > 0)
                    {
                        updating = true;

                        customParameterEnumBox1.ComponentId = emitterId;
                        this.customRadioButton1.ComponentId = emitterId;
                        this.customRadioButton2.ComponentId = emitterId;
                        updating = false;
                        UpdateViewComponent();
                        //PopulateCustomAttributes();
                    }
                }
            }
        }

        public String EmitterName
        {
            get
            {
                return emitterName;
            }
            set
            {
                emitterName = value;
                customTabPage1.Description = emitterName;
            }
        }


        public Emitter()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        private void PopulateCustomAttributes()
        {
            populating = true;

            listBoxCustomAttribute.Items.Clear();

            IXPathNavigable iNavEmitter = controller.GetParametersForComponent(emitterId);
            XPathNavigator navEmitter = iNavEmitter.CreateNavigator();

            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 4;
            compOptions.CompParams = true;
            IXPathNavigable iNavScenario = controller.GetComponentAndChildren(controller.ScenarioId, controller.ConfigurationLinkType, compOptions);
            XPathNavigator navScenario = iNavScenario.CreateNavigator();
 /*standard attribute*/
            //customRadioButton2.Checked = !Convert.ToBoolean(navEmitter.SelectSingleNode("ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Custom_Attribute_Emitter']").GetAttribute("value", navEmitter.NamespaceURI));
            XPathNodeIterator itNavEngrams = navScenario.Select("/Components/Component/Component[@Type='Engram']");
            while (itNavEngrams.MoveNext())
            {
                String engramName = itNavEngrams.Current.GetAttribute("Name", navScenario.NamespaceURI);
                if (!listBoxCustomAttribute.Items.Contains(engramName))
                {
                    listBoxCustomAttribute.Items.Add(engramName);
                }
            }
            if (this.customRadioButton1.Checked)
                SelectItem();

            populating = false;
        }

        private void SelectItem()
        {
            if (listBoxCustomAttribute.Items.Count > 0)
            {
                String text = String.Empty;

                IXPathNavigable inav = controller.GetParametersForComponent(emitterId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode("ComponentParameters/Parameter[@category='Emitter']/Parameter[@displayedName='Custom_Attribute']");
                if (node != null)
                {
                    text = node.GetAttribute("value", String.Empty);
                    if (!text.Equals(String.Empty))
                    {
                        Int32 index = this.listBoxCustomAttribute.FindStringExact(text);
                        if (index == -1) // No match so default to first item.
                            index = 0;
                        this.listBoxCustomAttribute.SelectedItem = listBoxCustomAttribute.Items[index];
                    }
                }
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
                customParameterEnumBox1.Controller = controller;
                this.customRadioButton1.Controller = controller;
                this.customRadioButton2.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            customParameterEnumBox1.UpdateViewComponent();
            this.customRadioButton1.UpdateViewComponent();
            this.customRadioButton2.UpdateViewComponent();
            PopulateCustomAttributes();
        }
        #endregion

        private void customParameterEnumBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (customParameterEnumBox1.SelectedItem != null)
            {
                string value = customParameterEnumBox1.SelectedItem.ToString();
                if (((value == "Default") || (value == "Invisible")) && (!updating))
                {
                    // get parameters using node id
                    ComponentOptions c = new ComponentOptions();
                    c.CompParams = true;
                    IXPathNavigable iNavParameters = controller.GetComponentAndChildren(emitterId, "Scenario", c);


                    XPathNavigator navParameters = iNavParameters.CreateNavigator();
                    XPathNodeIterator itr = navParameters.Select(".//Component[@Type='Level']");
                    if (itr.Count > 0)
                    {
                        if (MessageBox.Show("Selecting this option will delete this emitter's existing levels.\nDo you want to continue?", "Delete Levels", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
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
                            customParameterEnumBox1.SelectedIndex = previous_index;
                        }
                    }
                }

                previous_index = customParameterEnumBox1.SelectedIndex;
            }
        }

        private void customRadioButton2_CheckedChanged(object sender, EventArgs e)
        {//standard atts
            customParameterEnumBox1.Enabled = customRadioButton2.Checked;
            listBoxCustomAttribute.Enabled = customRadioButton1.Checked;
        }

        private void customRadioButton1_CheckedChanged(object sender, EventArgs e)
        {//custom atts
            customParameterEnumBox1.Enabled = customRadioButton2.Checked;
            listBoxCustomAttribute.Enabled = customRadioButton1.Checked;
            SelectItem();
        }

        private void listBoxCustomAttribute_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                try
                {
                    controller.UpdateParameters(emitterId, "Emitter.Custom_Attribute", this.listBoxCustomAttribute.SelectedItem.ToString(), eParamParentType.Component);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }

    }
}
