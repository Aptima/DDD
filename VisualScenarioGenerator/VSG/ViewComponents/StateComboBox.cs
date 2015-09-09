using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using VSG.Controllers;
using System.Xml;
using System.Xml.XPath;

namespace VSG.ViewComponents
{
    public partial class StateComboBox : ComboBox, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 componentId = -1;
        private String parameterCategory;
        private String parameterName;
        private Int32 speciesId = -1;
        private Boolean showAllStates = false;
        private Boolean populating = false;
        private Boolean updating = false;

        private VSGController controller;

        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }
            set
            {
                componentId = value;
                if (componentId > 0)
                {
                    UpdateViewComponent();
                }
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

        public Int32 SpeciesId
        {
            get 
            { 
                return speciesId;
            }
            set
            {
                speciesId = value;
                if (speciesId > 0)
                {
                    UpdateViewComponent();
                }
            }
        }

        public Boolean ShowAllStates
        {
            get
            { 
                return showAllStates;
            }
            set 
            { 
                showAllStates = value;
            }
        }

        public StateComboBox()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public StateComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void fill()
        {
            populating = true;

            this.Items.Clear();
            this.Text = "<Select State>";

            if (showAllStates)
            {
                ComponentOptions compOptions = new ComponentOptions();
                IXPathNavigable iNavScenario = controller.GetComponentAndChildren(controller.ScenarioId, "Scenario", compOptions);
                XPathNavigator navScenario = iNavScenario.CreateNavigator();
                XPathNodeIterator itStates = navScenario.Select("/Components/Component/Component/Component[@Type='State']");
                while (itStates.MoveNext())
                {     
                    String name = itStates.Current.GetAttribute("Name", itStates.Current.NamespaceURI);
                    if (!this.Items.Contains(name))
                        this.Items.Add(name);
                }
            }
            else
            {
                if (speciesId < 0)
                {
                    return;
                }

                ComponentOptions compOptions = new ComponentOptions();
                IXPathNavigable iNavSpeciesType = controller.GetComponentAndChildren(controller.ScenarioId, "SpeciesType", compOptions);
                XPathNavigator navSpeciesType = iNavSpeciesType.CreateNavigator();
                XPathNavigator navSpecies = navSpeciesType.SelectSingleNode(String.Format("/Components/Component/Component[@ID='{0}']", speciesId));
                XPathNodeIterator itSpecies = navSpecies.Select("self::*");
                while (itSpecies.MoveNext())
                {
                    String id = itSpecies.Current.GetAttribute("ID", itSpecies.Current.NamespaceURI);
                    IXPathNavigable iNavScenario = controller.GetComponentAndChildren(Int32.Parse(id), "Scenario", compOptions);
                    XPathNavigator navScenario = iNavScenario.CreateNavigator();
                    XPathNodeIterator itStates = navScenario.Select("/Components/Component[@Type='Species']/Component[@Type='State']");
                    while (itStates.MoveNext())
                    {
                        String name = itStates.Current.GetAttribute("Name", itStates.Current.NamespaceURI);
                        if (!this.Items.Contains(name))
                            this.Items.Add(name);
                    }
                }
            }

            selectItem();

            populating = false;
        }

        private void selectItem()
        {
            if (this.Items.Count > 0)
            {
                String text = String.Empty;

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
                            index = 0;
                            try
                            {
                                this.SelectedIndex = index;
                                controller.UpdateParameters(componentId, String.Format("{0}.{1}", parameterCategory, parameterName), this.SelectedItem.ToString(), eParamParentType.Component);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                            }                           
                        }
                        else
                            this.SelectedItem = this.Items[index];
                        
                    }
                }
            }
        }

        #region IViewComponent Members

        public IController Controller
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
            fill();
            updating = false;
        }

        #endregion

        private void StateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!populating & !updating)
            {
                try
                {
                    controller.UpdateParameters(componentId, String.Format("{0}.{1}", parameterCategory, parameterName), this.SelectedItem.ToString(), eParamParentType.Component);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
                }
            }
        }
    }
}
