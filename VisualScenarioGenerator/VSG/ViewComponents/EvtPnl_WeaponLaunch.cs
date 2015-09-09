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
    public partial class EvtPnl_WeaponLaunch : UserControl, AME.Views.View_Components.IViewComponent
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
                timeBox.ComponentId = displayID;

                engramRange1.DisplayID = displayID;
                eventID1.DisplayID = displayID;

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

        public EvtPnl_WeaponLaunch()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
            //eventID1.unitLinkBox.SelectedIndexChanged += new System.EventHandler(this.unitLinkBox_SelectedIndexChanged);
        }


        private void unitLinkBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateViewComponent();
            
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
                timeBox.Controller = _controller;
                //initialStateLinkBox.Controller = _controller;
                initialStateComboBox.Controller = _controller;
                weaponLinkBox.Controller = _controller;
                targetLinkBox.Controller = _controller;
                engramRange1.Controller = _controller;
                eventID1.Controller = _controller;

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
                //fuelDepletion.Controller = _controller;
                fuelDepletionStateComboBox1.Controller = _controller;
                iconListView1.Controller = _controller;
                initialFuel.Controller = _controller;
                launchDuration.Controller = _controller;
                maxSpeed.Controller = _controller;
                stealable.Controller = _controller;
                timeToAttack.Controller = _controller;
            }
        }

        public void UpdateViewComponent()
        {
            VSGController myController = (VSGController)Controller;
            //timeBox.UpdateViewComponent();
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
            eventID1.UpdateViewComponent();
            //unitIDBox.Text = myController.GetComponentName(parentCompID);
            List<int> ids = myController.GetChildIDs(displayID, "CreateEvent", "EventID");
            int createID = -1;
            if (ids.Count > 0)
            {
                createID = ids[0];
            }

            if (DisplayID == weaponLinkBox.ConnectFromId)
            {
                if (createID != weaponLinkBox.DisplayRootId)
                {
                    weaponLinkBox.DeleteAllLinks();
                }
            }

            weaponLinkBox.DisplayRootId = createID;
            weaponLinkBox.DisplayComponentType = "CreateEvent";
            weaponLinkBox.DisplayLinkType = "CreateEventSubplatform";
            weaponLinkBox.ConnectRootId = DisplayID;
            weaponLinkBox.ConnectFromId = DisplayID;
            weaponLinkBox.ConnectLinkType = "WeaponLaunchEventSubplatform";
            weaponLinkBox.OneToMany = false;
            weaponLinkBox.UpdateViewComponent();

            //if (DisplayID == targetLinkBox.ConnectFromId)
            //{
            //    if (createID != targetLinkBox.DisplayRootId)
            //    {
            //        targetLinkBox.DeleteAllLinks();
            //    }
            //}

            targetLinkBox.DisplayRootId = _controller.ScenarioId;
            targetLinkBox.DisplayComponentType = "CreateEvent";
            targetLinkBox.DisplayLinkType = "Scenario";
            targetLinkBox.ConnectRootId = DisplayID;
            targetLinkBox.ConnectFromId = DisplayID;
            targetLinkBox.ConnectLinkType = "WeaponLaunchEventTarget";
            targetLinkBox.OneToMany = false;
            targetLinkBox.UpdateViewComponent();

            ids = myController.GetChildIDs(DisplayID, "CreateEvent", "WeaponLaunchEventSubplatform");


            if (eventID1.Unit)
            {
                initialStateComboBox.ShowAllStates = true;
                fuelDepletionStateComboBox1.ShowAllStates = true;
            }
            else
            {
                initialStateComboBox.ShowAllStates = false;
                fuelDepletionStateComboBox1.ShowAllStates = false;
            }

            int subpID = -1;
            if (ids.Count > 0)
            {
                subpID = ids[0];
            }
            if (subpID < 0)
            {
                //initialStateLinkBox.DeleteAllLinks();
                // no subplatform specified: remove the states listed
                initialStateComboBox.SpeciesId = -1;
                //initialStateLinkBox.DisplayRootId = -1;
            }
            else
            {
                ids = myController.GetChildIDs(subpID, "Species", "CreateEventKind");
                if (ids.Count >= 1)
                {
                    //if (initialStateLinkBox.DisplayRootId != ids[0])
                    //{
                    //    initialStateLinkBox.DeleteAllLinks();
                    //}

                    initialStateComboBox.SpeciesId = ids[0];
                    initialStateComboBox.ComponentId = DisplayID;
                    initialStateComboBox.ParameterCategory = "WeaponLaunchEvent";
                    initialStateComboBox.ParameterName = "InitialState";

                    //initialStateLinkBox.DisplayRootId = ids[0];
                    //initialStateLinkBox.DisplayComponentType = "State";
                    //initialStateLinkBox.DisplayLinkType = "Scenario";
                    //initialStateLinkBox.ConnectRootId = DisplayID;
                    //initialStateLinkBox.ConnectFromId = DisplayID;
                    //initialStateLinkBox.ConnectLinkType = "LaunchEventInitialState";
                    //initialStateLinkBox.OneToMany = false;

                }
            }


            initialStateComboBox.UpdateViewComponent();
            engramRange1.UpdateViewComponent();


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
                
                fuelDepletionStateComboBox1.Enabled = true;
                
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
                    fuelDepletionStateComboBox1.SpeciesId = ids[0];
                    fuelDepletionStateComboBox1.ComponentId = DisplayID;
                    fuelDepletionStateComboBox1.ParameterCategory = "LaunchEvent";
                    fuelDepletionStateComboBox1.ParameterName = "FuelDepletionState";

                }
                else
                {
                    fuelDepletionStateComboBox1.SpeciesId = -1;
                }
                fuelDepletionStateComboBox1.UpdateViewComponent();
            }
            else
            {
                fuelDepletionStateComboBox1.Enabled = false;
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

        private void EvtPnl_Launch_Load(object sender, EventArgs e)
        {

        }

        //private void kindRadio_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (kindRadio.Checked == true)
        //    {
        //        kindLinkBox.Enabled = true;
        //        unitLinkBox.Enabled = false;
        //    }
        //}

        //private void unitRadio_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (unitRadio.Checked == true)
        //    {
        //        kindLinkBox.Enabled = false;
        //        unitLinkBox.Enabled = true;
        //    }
        //}
    }
}
