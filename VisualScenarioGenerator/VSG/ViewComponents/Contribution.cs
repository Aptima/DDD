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
    public partial class Contribution : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 contributionId = -1;
        private String contributionName;
        private Boolean populating = false;
        private Boolean updating = false;

        private VSGController controller;

        public double NndRange
        {
            get { return nndRange.Value; }
            set { if (value >= 0)nndRange.Value = value; UpdateViewComponent(); }
        }
        public int NudIntensity
        {
            get { return (int)(nudIntensity.Value); }
            set { if (value <= 1000)nudIntensity.Value = value; UpdateViewComponent(); }
        }
        public int NudProbability
        {
            get { return (int)(nudProbability.Value); }
            set { if (value <= 100)nudProbability.Value = value; UpdateViewComponent(); }
        }

        public String ContributionName
        {
            get
            {
                return contributionName;
            }
            set
            {
                contributionName = value;
                customTabPage1.Description = contributionName;
            }
        }

        public Contribution()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 ContributionId
        {
            get { return contributionId; }
            set
            {
                contributionId = value;
                if (contributionId > 0)
                {
            
                    nndRange.ComponentId = contributionId;
                    nudIntensity.ComponentId = contributionId;
                    nudProbability.ComponentId = contributionId;
                    UpdateViewComponent();
                }
            }
        }

        private void fillLevels()
        {
            populating = true;

            comboBox1.Items.Clear();
            comboBox1.Text = "<Select Capability>";
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 5;
            IXPathNavigable iNavScenario = controller.GetComponentAndChildren(controller.ScenarioId, controller.ConfigurationLinkType, compOptions);
            XPathNavigator navScenario = iNavScenario.CreateNavigator();
            XPathNodeIterator itCapabilities = navScenario.Select("/Components/Component/Component[@Type='Species']/Component[@Type='State']/Component[@Type='Capability']");
            while (itCapabilities.MoveNext())
            {
                String name = itCapabilities.Current.GetAttribute("Name", navScenario.NamespaceURI);
                if (!comboBox1.Items.Contains(name))
                    comboBox1.Items.Add(name);
            }

            selectItem();

            populating = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!populating & !updating)
            {
                try
                {
                    controller.UpdateParameters(contributionId, "Contribution.Capability", this.comboBox1.SelectedItem.ToString(), eParamParentType.Component);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }

        private void selectItem()
        {
            if (comboBox1.Items.Count > 0)
            {
                String text = String.Empty;

                IXPathNavigable inav = controller.GetParametersForComponent(contributionId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode("ComponentParameters/Parameter[@category='Contribution']/Parameter[@displayedName='Capability']");
                if (node != null)
                {
                    text = node.GetAttribute("value", String.Empty);
                    if (!text.Equals(String.Empty))
                    {
                        Int32 index = this.comboBox1.FindStringExact(text);
                        if (index == -1) // No match so default to first item.
                            index = 0;
                        this.comboBox1.SelectedItem = comboBox1.Items[index];
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
                nudProbability.Controller = controller;
                nudIntensity.Controller = controller;
                nndRange.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            updating = true;
            nudProbability.UpdateViewComponent();
            nudIntensity.UpdateViewComponent();
            nndRange.UpdateViewComponent();
            fillLevels();
            updating = false;
        }

        #endregion

    }
}

