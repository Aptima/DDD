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
    public partial class Singleton : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 singletonId = -1;
        private String singletonName;
        private Boolean populating = false;
        private Boolean updating = false;

        private VSGController controller;

        public String SingletonName
        {
            get
            {
                return singletonName;
            }
            set
            {
                singletonName = value;
                customTabPage1.Description = singletonName;
            }
        }

        public Singleton()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 SingletonId
        {
            get { return singletonId; }
            set
            {
                singletonId = value;
                if (singletonId > 0)
                {
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
                    controller.UpdateParameters(singletonId, "Singleton.Capability", this.comboBox1.SelectedItem.ToString(), eParamParentType.Component);
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

                IXPathNavigable inav = controller.GetParametersForComponent(singletonId);
                XPathNavigator nav = inav.CreateNavigator();
                XPathNavigator node = nav.SelectSingleNode("ComponentParameters/Parameter[@category='Singleton']/Parameter[@displayedName='Capability']");
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
            }
        }

        public void UpdateViewComponent()
        {
            updating = true;
            fillLevels();
            updating = false;
        }

        #endregion
    }
}
