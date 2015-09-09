using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class EvtPnl_Reveal : UserControl, AME.Views.View_Components.IViewComponent
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
                eventID1.DisplayID = value;
                timeBox.ComponentId = displayID;
                xBox.ComponentId = displayID;
                yBox.ComponentId = displayID;
                zBox.ComponentId = displayID;
                initialTagParameterTextBox.ComponentId = displayID;
                engramRange.DisplayID = displayID;
                //customLinkComboBoxLinkedRegion

                overrideDockingDuration.ComponentId = displayID;
                overrideFuelCapacity.ComponentId = displayID;
                overrideFuelConsumptionRate.ComponentId = displayID;
                overrideFuelDepletionState.ComponentId = displayID;
                overrideIcon.ComponentId = displayID;
                overrideInitialFuelLoad.ComponentId = displayID;
                overrideLaunchDuration.ComponentId = displayID;
                overrideMaximumSpeed.ComponentId = displayID;
                overrideStealable.ComponentId = displayID;
                overrideTimeToAttack.ComponentId = displayID;
                dockingDuration.ComponentId = displayID;
                fuelCapacity.ComponentId = displayID;
                fuelConsumption.ComponentId = displayID;
                //fuelDepletion.ComponentId = displayID;
                iconListView1.ComponentId = displayID;
                initialFuel.ComponentId = displayID;
                launchDuration.ComponentId = displayID;
                maxSpeed.ComponentId = displayID;
                stealable.ComponentId = displayID;
                timeToAttack.ComponentId = displayID;
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
                eventID1.ParentID = value;
            }
        }


        public EvtPnl_Reveal()
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
                eventID1.Controller = _controller;
                customLinkComboBoxLinkedRegion.Controller = _controller;
                timeBox.Controller = _controller;
                xBox.Controller = _controller;
                yBox.Controller = _controller;
                zBox.Controller = _controller;

                initialStateComboBox.Controller = _controller;
                initialTagParameterTextBox.Controller = _controller;
                engramRange.Controller = _controller;

                overrideDockingDuration.Controller = _controller;
                overrideFuelCapacity.Controller = _controller;
                overrideFuelConsumptionRate.Controller = _controller;
                overrideFuelDepletionState.Controller = _controller;
                overrideIcon.Controller = _controller;
                overrideInitialFuelLoad.Controller = _controller;
                overrideLaunchDuration.Controller = _controller;
                overrideMaximumSpeed.Controller = _controller;
                overrideStealable.Controller = _controller;
                overrideTimeToAttack.Controller = _controller;
                dockingDuration.Controller = _controller;
                fuelCapacity.Controller = _controller;
                fuelConsumption.Controller = _controller;
                fuelDepletionStateComboBox.Controller = _controller;
                iconListView1.Controller = _controller;
                initialFuel.Controller = _controller;
                launchDuration.Controller = _controller;
                maxSpeed.Controller = _controller;
                stealable.Controller = _controller;
                timeToAttack.Controller = _controller;

                customLinkBox1.Controller = _controller;
            }
        }

        public void UpdateViewComponent()
        {
            //label1.Text = String.Format("Reveal[{0}]", displayID);
            eventID1.UpdateViewComponent();
            VSGController myController = (VSGController)Controller;
            String type = _controller.GetComponentType(ParentCompID);
            timeBox.UpdateViewComponent();
            if (type == "ReiterateEvent"
                || type == "CompletionEvent"
                || type == "SpeciesCompletionEvent")
            {
                timeBox.Enabled = false;
            }
            else
            {
                timeBox.Enabled = true;
            }
            //timeBox.UpdateViewComponent();
            //unitIDBox.Text = ((VSGController)Controller).GetComponentName(parentCompID);


            if (eventID1.Unit)
            {
                initialStateComboBox.ShowAllStates = true;
                fuelDepletionStateComboBox.ShowAllStates = true;
                
            }
            else
            {
                initialStateComboBox.ShowAllStates = false;
                fuelDepletionStateComboBox.ShowAllStates = false;
            }

            List<int> ids = myController.GetChildIDs(displayID, "CreateEvent", "EventID");
            int createID = -1;
            if (ids.Count > 0)
            {
                createID = ids[0];
            }
            ids = myController.GetChildIDs(createID, "Species", "CreateEventKind");
            if (ids.Count >= 1)
            {
                //if (initialStateLinkBox.DisplayRootId != ids[0]) // logic for when root changes
                //{
                //    initialStateLinkBox.DeleteAllLinks();
                //}

                //initialStateLinkBox.DisplayRootId = ids[0];
                //initialStateLinkBox.DisplayComponentType = "State";
                //initialStateLinkBox.DisplayLinkType = "Scenario";
                //initialStateLinkBox.ConnectRootId = DisplayID;
                //initialStateLinkBox.ConnectFromId = DisplayID;
                //initialStateLinkBox.ConnectLinkType = "RevealEventInitialState";
                //initialStateLinkBox.OneToMany = false;

                initialStateComboBox.SpeciesId = ids[0];
                initialStateComboBox.ComponentId = DisplayID;
                initialStateComboBox.ParameterCategory = "RevealEvent";
                initialStateComboBox.ParameterName = "InitialState";
            }
            else
            {
                //initialStateLinkBox.DeleteAllLinks();
                // no species specified:  remove the states listed
                //initialStateLinkBox.DisplayRootId = -1;
                initialStateComboBox.SpeciesId = -1;
            }

            initialStateComboBox.UpdateViewComponent();
            xBox.UpdateViewComponent();
            yBox.UpdateViewComponent();
            zBox.UpdateViewComponent();
            engramRange.UpdateViewComponent();
            initialTagParameterTextBox.UpdateViewComponent();

            overrideDockingDuration.UpdateViewComponent();
            overrideFuelCapacity.UpdateViewComponent();
            overrideFuelConsumptionRate.UpdateViewComponent();
            overrideFuelDepletionState.UpdateViewComponent();
            overrideIcon.UpdateViewComponent();
            overrideInitialFuelLoad.UpdateViewComponent();
            overrideLaunchDuration.UpdateViewComponent();
            overrideMaximumSpeed.UpdateViewComponent();
            overrideStealable.UpdateViewComponent();
            overrideTimeToAttack.UpdateViewComponent();
            customLinkBox1.DisplayRootId = myController.ScenarioId;
            customLinkBox1.DisplayComponentType = "ActiveRegion";
            customLinkBox1.DisplayLinkType = "Scenario";
            customLinkBox1.ConnectRootId = DisplayID;
            customLinkBox1.ConnectFromId = DisplayID;
            customLinkBox1.ConnectLinkType = "LinkObjectToRegion";
            customLinkBox1.OneToMany = false;
            // <mw> filter to only show active regions
            customLinkBox1.ParameterFilterCategory = "Active Region";
            customLinkBox1.ParameterFilterName = "Is this a Dynamic Region";
            customLinkBox1.ParameterFilterValue = "true";
            customLinkBox1.FilterResult = true;
            // <mw>
            customLinkBox1.UpdateViewComponent();
            /*
            customLinkComboBoxLinkedRegion.DisplayRootId = ((VSGController)_controller).ScenarioId;
            customLinkComboBoxLinkedRegion.DisplayLinkType = "Scenario";
            customLinkComboBoxLinkedRegion.ConnectRootId = ((VSGController)_controller).ScenarioId;
            customLinkComboBoxLinkedRegion.ConnectFromId = DisplayID;
            customLinkComboBoxLinkedRegion.ConnectLinkType = "Scenario";*/
           // customLinkComboBoxLinkedRegion.

            //customLinkComboBoxLinkedRegion.UpdateViewComponent();

            if (overrideDockingDuration.Checked)
            {
                dockingDuration.Enabled = true;
                dockingDuration.UpdateViewComponent();
                dockingDurationLabel.Enabled = true;
            }
            else
            {
                dockingDuration.Enabled = false;
                dockingDurationLabel.Enabled = false;
            }

            if (overrideFuelCapacity.Checked)
            {
                fuelCapacity.Enabled = true;
                fuelCapacity.UpdateViewComponent();

                fuelCapacityLabel.Enabled = true;
            }
            else
            {
                fuelCapacity.Enabled = false;
                fuelCapacityLabel.Enabled = false;
            }

            if (overrideFuelConsumptionRate.Checked)
            {
                fuelConsumption.Enabled = true;
                fuelConsumption.UpdateViewComponent();
                fuelConsumptionRateLabel.Enabled = true;
            }
            else
            {
                fuelConsumption.Enabled = false;
                fuelConsumptionRateLabel.Enabled = false;
            }

            if (overrideFuelDepletionState.Checked)
            {

                fuelDepletionStateComboBox.Enabled = true;
                
                fuelDepletionStateLabel.Enabled = true;

                if (ids.Count >= 1)
                {
                    //if (fuelDepletion.DisplayRootId != ids[0])
                    //{
                    //    fuelDepletion.DeleteAllLinks();
                    //}
                    //fuelDepletion.DisplayRootId = ids[0];
                    //fuelDepletion.DisplayComponentType = "State";
                    //fuelDepletion.DisplayLinkType = "Scenario";
                    //fuelDepletion.ConnectRootId = DisplayID;
                    //fuelDepletion.ConnectFromId = DisplayID;
                    //fuelDepletion.ConnectLinkType = "FuelDepletionState";
                    //fuelDepletion.OneToMany = false;

                    fuelDepletionStateComboBox.SpeciesId = ids[0];
                    fuelDepletionStateComboBox.ComponentId = DisplayID;
                    fuelDepletionStateComboBox.ParameterCategory = "RevealEvent";
                    fuelDepletionStateComboBox.ParameterName = "FuelDepletionState";

                }
                fuelDepletionStateComboBox.UpdateViewComponent();
            }
            else
            {
                fuelDepletionStateComboBox.Enabled = false;
                fuelDepletionStateLabel.Enabled = false;
                //fuelDepletion.DeleteAllLinks();
            }

            if (overrideIcon.Checked)
            {
                iconListView1.Enabled = true;
                iconListView1.UpdateViewComponent();
                iconLabel.Enabled = true;
            }
            else
            {
                iconListView1.Enabled = false;
                iconLabel.Enabled = false;
            }

            if (overrideInitialFuelLoad.Checked)
            {
                initialFuel.Enabled = true;
                initialFuel.UpdateViewComponent();
                initialFuelLoadLabel.Enabled = true;
                
            }
            else
            {
                initialFuel.Enabled = false;
                initialFuelLoadLabel.Enabled = false;
            }

            if (overrideLaunchDuration.Checked)
            {
                launchDuration.Enabled = true;
                launchDuration.UpdateViewComponent();
                launchDurationLabel.Enabled = true;
            }
            else
            {
                launchDuration.Enabled = false;
                launchDurationLabel.Enabled = false;
            }

            if (overrideMaximumSpeed.Checked)
            {
                maxSpeed.Enabled = true;
                maxSpeed.UpdateViewComponent();
                maximumSpeedLabel.Enabled = true;
                
            }
            else
            {
                maxSpeed.Enabled = false;
                maximumSpeedLabel.Enabled = false;
            }

            if (overrideStealable.Checked)
            {
                stealable.Enabled = true;
                stealable.UpdateViewComponent();
                
                
            }
            else
            {
                stealable.Enabled = false;
                
            }

            if (overrideTimeToAttack.Checked)
            {
                timeToAttack.Enabled = true;
                timeToAttack.UpdateViewComponent();
                timeToAttackLabel.Enabled = true;
            }
            else
            {
                timeToAttack.Enabled = false;
                timeToAttackLabel.Enabled = false;
            }


        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
