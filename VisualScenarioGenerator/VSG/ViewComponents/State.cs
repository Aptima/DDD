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

    public partial class State : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 stateId = -1;
        private Int32 speciesId = -1;
        private VSGController controller;
        private String stateName;
        private bool updating = false;

        public State()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

        }

        public Boolean ChkbxStealable
        {
            get { return stealable.Checked; }
            set
            {
                stealable.Checked = value;
                UpdateViewComponent();
            }
        }

        public Int32 StateId
        {
            get
            {
                return stateId;
            }
            set
            {
                stateId = value;
                if (stateId >= 0)
                {
                    this.stealable.ComponentId = stateId;
                    this.dockingDuration.ComponentId = stateId;
                    this.fuelCapacity.ComponentId = stateId;
                    this.fuelConsumption.ComponentId = stateId;
                    this.initialFuel.ComponentId = stateId;
                    this.launchDuration.ComponentId = stateId;
                    this.maxSpeed.ComponentId = stateId;
                    this.timeToAttack.ComponentId = stateId;
                    this.engagementDuration.ComponentId = stateId;
                    this.stateComboBox1.ComponentId = stateId;

                    this.overrideIcon.ComponentId = stateId;
                    this.overrideDockingDuration.ComponentId = stateId;
                    this.overrideFuelCapacity.ComponentId = stateId;
                    this.overrideFuelConsumption.ComponentId = stateId;
                    this.overrideFuelDepletionState.ComponentId = stateId;
                    this.overrideInitialFuel.ComponentId = stateId;
                    this.overrideLaunchDuration.ComponentId = stateId;
                    this.overrideMaximumSpeed.ComponentId = stateId;
                    this.overrideStealable.ComponentId = stateId;
                    this.overrideTimeToAttack.ComponentId = stateId;
                    this.overrideEngagementDuration.ComponentId = stateId;

                    //lkbxFuelDepletion.DisplayRootId = speciesId;
                    //lkbxFuelDepletion.ConnectRootId = stateId;
                    //lkbxFuelDepletion.ConnectFromId = stateId;

                    // use dynamic
                    //if (controller != null && lkbxFuelDepletion.ConnectLinkType != null)
                    //{
                    //    String dynamic = controller.GetDynamicLinkType(lkbxFuelDepletion.ConnectLinkType, stateId.ToString());
                    //    lkbxFuelDepletion.ConnectLinkType = dynamic;
                    //}

                    lkbxSensor.DisplayRootId = controller.ScenarioId;
                    lkbxSensor.ConnectRootId = stateId;
                    lkbxSensor.ConnectFromId = stateId;
                    lkbxSensor.OneToMany = true;

                    //lkbxEmitter.DisplayRootId = controller.ScenarioId;
                    //lkbxEmitter.ConnectRootId = stateId;
                    //lkbxEmitter.ConnectFromId = stateId;
                    //lkbxEmitter.OneToMany = true;

                    //lkbxCapability.ConnectRootId = stateId;
                    //lkbxCapability.ConnectFromId = stateId;
                    //lkbxCapability.OneToMany = true;

                    //lkbxSingleton.DisplayRootId = speciesId;
                    //lkbxSingleton.ConnectRootId = stateId;
                    //lkbxSingleton.ConnectFromId = stateId;
                    //lkbxSingleton.OneToMany = true;

                    //lkbxCombo.DisplayRootId = speciesId;
                    //lkbxCombo.ConnectRootId = stateId;
                    //lkbxCombo.ConnectFromId = stateId;
                    //lkbxCombo.OneToMany = true;

                    //lkbxCombo.DisplayRootId = speciesId;
                    //lkbxCombo.ConnectFromId = stateId;
                    //lkbxCombo.ConnectFromId = stateId;
                    //lkbxCombo.OneToMany = true;

                    iconListView1.ComponentId = stateId;                

                    UpdateViewComponent();
                }
            }
        }

        public Int32 SpeciedId
        {
            get { return speciesId; }
            set
            {
                speciesId = value;
                if (speciesId > 0)
                {
                    this.stateComboBox1.SpeciesId = speciesId;

                    //lkbxCapability.DisplayRootId = speciesId;
                    UpdateViewComponent();
                }
            }
        }

        public String StateName
        {
            get
            {
                return stateName;
            }
            set
            {
                stateName = value;
                customTabPage1.Description = stateName;
            }
        }
        private Int32 componentId;
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
                //lkbxFuelDepletion.Controller = controller;
                //lkbxCombo.Controller = controller;
                lkbxSensor.Controller = controller;
                //lkbxEmitter.Controller = controller;
                //lkbxCapability.Controller = controller;
                //lkbxSingleton.Controller = controller;
                iconListView1.Controller = controller;

                this.stealable.Controller = controller;
                this.dockingDuration.Controller = controller;
                this.fuelCapacity.Controller = controller;
                this.fuelConsumption.Controller = controller;
                this.initialFuel.Controller = controller;
                this.launchDuration.Controller = controller;
                this.maxSpeed.Controller = controller;
                this.timeToAttack.Controller = controller;
                this.engagementDuration.Controller = controller;
                this.stateComboBox1.Controller = controller;


                this.overrideIcon.Controller = controller;
                this.overrideDockingDuration.Controller = controller;
                this.overrideFuelCapacity.Controller = controller;
                this.overrideFuelConsumption.Controller = controller;
                this.overrideFuelDepletionState.Controller = controller;
                this.overrideInitialFuel.Controller = controller;
                this.overrideLaunchDuration.Controller = controller;
                this.overrideMaximumSpeed.Controller = controller;
                this.overrideStealable.Controller = controller;
                this.overrideTimeToAttack.Controller = controller;
                this.overrideEngagementDuration.Controller = controller;

                // and combo
            }
        }

        public void UpdateViewComponent()
        {
            updating = true;
            if (StateId < 0)
            {
                return;
            }

            int speciesID = controller.GetSpeciesOfState(StateId);

            if (speciesID < 0)
            {
                return;
            }

            int speciesBase = controller.GetSpeciesBase(speciesId);


            string myStateName = controller.GetComponentName(StateId);


            if (speciesBase < 0 && myStateName == "FullyFunctional")
            {
                this.overrideIcon.Enabled = false;
                this.overrideDockingDuration.Enabled = false;
                this.overrideFuelCapacity.Enabled = false;
                this.overrideFuelConsumption.Enabled = false;
                this.overrideFuelDepletionState.Enabled = false;
                this.overrideInitialFuel.Enabled = false;
                this.overrideLaunchDuration.Enabled = false;
                this.overrideMaximumSpeed.Enabled = false;
                this.overrideStealable.Enabled = false;
                this.overrideTimeToAttack.Enabled = false;
                this.overrideEngagementDuration.Enabled = false;

                this.stealable.Enabled = true;
                this.dockingDuration.Enabled = true;
                this.fuelCapacity.Enabled = true;
                this.fuelConsumption.Enabled = true;
                this.initialFuel.Enabled = true;
                this.launchDuration.Enabled = true;
                this.maxSpeed.Enabled = true;
                this.timeToAttack.Enabled = true;
                this.engagementDuration.Enabled = true;
                //this.lkbxFuelDepletion.Enabled = true;
                this.iconListView1.Enabled = true;
                this.stateComboBox1.Enabled = true;

                this.dockingDurationLabel.Enabled = true;
                this.fuelCapacityLabel.Enabled = true;
                this.fuelConsumptionLabel.Enabled = true;
                this.initialFuelLabel.Enabled = true;
                this.launchDurationLabel.Enabled = true;
                this.maximumSpeedLabel.Enabled = true;
                this.timeToAttackLabel.Enabled = true;
                this.label1.Enabled = true;
                this.fuelDepletionStateLabel.Enabled = true;
                this.iconLabel.Enabled = true;

                this.stealable.UpdateViewComponent();
                this.dockingDuration.UpdateViewComponent();
                this.fuelCapacity.UpdateViewComponent();
                this.fuelConsumption.UpdateViewComponent();
                this.initialFuel.UpdateViewComponent();
                this.launchDuration.UpdateViewComponent();
                this.maxSpeed.UpdateViewComponent();
                this.timeToAttack.UpdateViewComponent();
                this.engagementDuration.UpdateViewComponent();
                this.iconListView1.UpdateViewComponent();
                //this.lkbxFuelDepletion.UpdateViewComponent();
                this.stateComboBox1.UpdateViewComponent();
            }
            else
            {
                this.overrideIcon.Enabled = true;
                this.overrideDockingDuration.Enabled = true;
                this.overrideFuelCapacity.Enabled = true;
                this.overrideFuelConsumption.Enabled = true;
                this.overrideFuelDepletionState.Enabled = true;
                this.overrideInitialFuel.Enabled = true;
                this.overrideLaunchDuration.Enabled = true;
                this.overrideMaximumSpeed.Enabled = true;
                this.overrideStealable.Enabled = true;
                this.overrideTimeToAttack.Enabled = true;
                this.overrideEngagementDuration.Enabled = true;


                this.overrideIcon.UpdateViewComponent();
                this.overrideDockingDuration.UpdateViewComponent();
                this.overrideFuelCapacity.UpdateViewComponent();
                this.overrideFuelConsumption.UpdateViewComponent();
                this.overrideFuelDepletionState.UpdateViewComponent();
                this.overrideInitialFuel.UpdateViewComponent();
                this.overrideLaunchDuration.UpdateViewComponent();
                this.overrideMaximumSpeed.UpdateViewComponent();
                this.overrideStealable.UpdateViewComponent();
                this.overrideTimeToAttack.UpdateViewComponent();
                this.overrideEngagementDuration.UpdateViewComponent();

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

                if (overrideFuelConsumption.Checked)
                {
                    fuelConsumption.Enabled = true;
                    fuelConsumption.UpdateViewComponent();
                    fuelConsumptionLabel.Enabled = true;
                }
                else
                {
                    fuelConsumption.Enabled = false;
                    fuelConsumptionLabel.Enabled = false;
                }

                if (overrideFuelDepletionState.Checked)
                {
                    //lkbxFuelDepletion.Enabled = true;

                    fuelDepletionStateLabel.Enabled = true;

                    stateComboBox1.Enabled = true;

                    //String dynamicType = controller.GetDynamicLinkType("StateState", stateId.ToString());
                    

                    //lkbxFuelDepletion.DisplayRootId = speciesId;
                    //lkbxFuelDepletion.DisplayComponentType = "State";
                    //lkbxFuelDepletion.DisplayLinkType = "Scenario";
                    //lkbxFuelDepletion.ConnectRootId = StateId;
                    //lkbxFuelDepletion.ConnectFromId = StateId;
                    //lkbxFuelDepletion.ConnectLinkType = dynamicType;
                    //lkbxFuelDepletion.OneToMany = false;

                    //lkbxFuelDepletion.UpdateViewComponent();
                    this.stateComboBox1.UpdateViewComponent();
                }
                else
                {
                    //lkbxFuelDepletion.Enabled = false;
                    fuelDepletionStateLabel.Enabled = false;
                    this.stateComboBox1.Enabled = false;
                    //lkbxFuelDepletion.DeleteAllLinks();
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

                if (overrideInitialFuel.Checked)
                {
                    initialFuel.Enabled = true;
                    initialFuel.UpdateViewComponent();
                    initialFuelLabel.Enabled = true;

                }
                else
                {
                    initialFuel.Enabled = false;
                    initialFuelLabel.Enabled = false;
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

                if (overrideEngagementDuration.Checked)
                {
                    engagementDuration.Enabled = true;
                    engagementDuration.UpdateViewComponent();
                    label1.Enabled = true;
                }
                else
                {
                    engagementDuration.Enabled = false;
                    label1.Enabled = false;
                }
            }

            //lkbxFuelDepletion.UpdateViewComponent();
            //lkbxCombo.UpdateViewComponent();
            lkbxSensor.UpdateViewComponent();
            //lkbxEmitter.UpdateViewComponent();
            //lkbxCapability.UpdateViewComponent();
            //lkbxSingleton.UpdateViewComponent();
            iconListView1.UpdateViewComponent();
            // and combo

            this.stateComboBox1.UpdateViewComponent();

          
            updating = false;
        }

        #endregion

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void overrideCheckedChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                this.UpdateViewComponent();
            }
        }

       
    }
}
