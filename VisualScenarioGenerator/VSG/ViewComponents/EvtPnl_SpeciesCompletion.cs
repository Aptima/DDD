using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class EvtPnl_SpeciesCompletion : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _controller;
        private int displayID = -1;
        private int parentCompID = -1;

        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                displayID = value;
            }
        }
        public int ParentCompID
        {
            get
            {
                return parentCompID;
            }
            set
            {
                parentCompID = value;
            }
        }

        public EvtPnl_SpeciesCompletion()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = (VSGController)value;
                stateComboBox.Controller = _controller;
                
                speciesLinkBox.Controller = _controller;
            }
        }

        private Boolean updating = false;

        public void UpdateViewComponent()
        {
            updating = true;

            VSGController myController = (VSGController)Controller;
            //unitIDBox.Text = myController.GetComponentName(parentCompID);
            ComponentOptions op = new ComponentOptions();
            op.LevelDown = 1;

            String text = String.Empty;

            //if (eventID1.Unit)
            //{
            //    stateComboBox.ShowAllStates = true;
            //    //fuelDepletionStateComboBox.ShowAllStates = true;

            //}
            //else
            //{
            //    stateComboBox.ShowAllStates = false;
            //    //fuelDepletionStateComboBox.ShowAllStates = false;
            //}


            IXPathNavigable inav = myController.GetParametersForComponent(DisplayID);
            XPathNavigator nav = inav.CreateNavigator();
            XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", "SpeciesCompletionEvent", "Action"));
            if (node != null)
            {
                text = node.GetAttribute("value", String.Empty);
            }
            actionCombo.SelectedItem = text;
            if (text == "")
            {
                stateRadio.Checked = true;
                stateComboBox.Enabled = true;
                actionCombo.Enabled = false;
            }
            else
            {
                actionRadio.Checked = true;
                //stateRadio.Checked = false;
                stateComboBox.Enabled = false;
                actionCombo.Enabled = true;
            }

            speciesLinkBox.DisplayRootId = myController.ScenarioId;
            speciesLinkBox.DisplayComponentType = "Species";
            speciesLinkBox.DisplayLinkType = "Scenario";
            speciesLinkBox.ConnectRootId = DisplayID;
            speciesLinkBox.ConnectFromId = DisplayID;
            speciesLinkBox.ConnectLinkType = "SpeciesCompletionEventSpecies";
            speciesLinkBox.UpdateViewComponent();


            List<int> ids = myController.GetChildIDs(DisplayID, "Species", "SpeciesCompletionEventSpecies");
            if (ids.Count >= 1)
            {
                //stateLinkBox.
                stateComboBox.SpeciesId = ids[0];
                stateComboBox.ComponentId = DisplayID;
                stateComboBox.ParameterCategory = "SpeciesCompletionEvent";
                stateComboBox.ParameterName = "State";
                //stateLinkBox.DisplayRootId = ids[0];
                //stateLinkBox.DisplayComponentType = "State";
                //stateLinkBox.DisplayLinkType = "Scenario";
                //stateLinkBox.ConnectRootId = DisplayID;
                //stateLinkBox.ConnectFromId = DisplayID;
                //stateLinkBox.ConnectLinkType = "SpeciesCompletionEventNewState";

            }
            else
            {
                stateComboBox.SpeciesId = -1;
            }

            stateComboBox.UpdateViewComponent();

            updating = false;
        }

        #endregion

        private void actionRadioSelectChanged()
        {
            if (actionRadio.Checked && !updating)
            {
                actionCombo.Enabled = true;
                stateComboBox.Enabled = false;
                //stateLinkBox.DeleteAllLinks();
                actionCombo.SelectedItem = "MoveComplete_Event";
                ((VSGController)Controller).UpdateParameters(DisplayID, "SpeciesCompletionEvent" + "." + "Action", (String)actionCombo.SelectedItem, AME.Controllers.eParamParentType.Component);
            }
        }

        private void stateRadioSelectChanged()
        {
            if (stateRadio.Checked && !updating)
            {
                actionCombo.Enabled = false;
                stateComboBox.Enabled = true;
                ((VSGController)Controller).UpdateParameters(DisplayID, "SpeciesCompletionEvent" + "." + "Action", "", AME.Controllers.eParamParentType.Component);
            }
        }

        private void actionRadio_CheckedChanged(object sender, EventArgs e)
        {
            actionRadioSelectChanged();
        }

        private void stateRadio_CheckedChanged(object sender, EventArgs e)
        {
            stateRadioSelectChanged();
        }

        private void actionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                String item = (String)actionCombo.SelectedItem;
                ((VSGController)Controller).UpdateParameters(DisplayID, "SpeciesCompletionEvent" + "." + "Action", item, AME.Controllers.eParamParentType.Component);
            }
        }
    }
}
