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

    public partial class Species : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 speciesId = -1;
        private VSGController controller;
        private String speciesName;

        public Species()
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Parameter);

            InitializeComponent();

            lkbxType.CheckLinkLevel = 2; // for scenario to species to species
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
                if (speciesId >= 0)
                {
                    ckbxIsWeapon.ComponentId = speciesId;
                    ckbxRemoveOnDestruction.ComponentId = speciesId;
                    nndCollisionRadius.ComponentId = speciesId;
                    rbAir.ComponentId = speciesId;
                    rbLand.ComponentId = speciesId;
                    rbSea.ComponentId = speciesId;
                    rbExistingSpecies.ComponentId = speciesId;

                    lkbxType.DisplayRootId = controller.ScenarioId;
                    lkbxType.DisplayComponentType = "Species";
                    lkbxType.DisplayLinkType = "Scenario";
                    lkbxType.ConnectRootId = controller.ScenarioId;
                    lkbxType.ConnectLinkType = "SpeciesType";
                    lkbxType.ConnectFromId = speciesId;
                    lkbxType.OneToMany = false;

                    customLinkBoxDecisionMakers.DisplayRootId = controller.ScenarioId;
                    customLinkBoxDecisionMakers.ConnectRootId = speciesId;// controller.ScenarioId;
                    customLinkBoxDecisionMakers.ConnectFromId = speciesId;


                    customCheckBoxLaunchedByOwner.ComponentId = speciesId;

                    this.simpleLinkGrid1.ComponentId = speciesId;
                    this.simpleLinkGrid1.RootId = controller.ScenarioId;

                    this.classificationRule1.ComponentId = speciesId;
                    this.classificationRule1.RootId = controller.ScenarioId;
                    this.classificationRule1.Link = "Scenario";//assumption
                    this.classificationRule1.ParameterCategory = "Species";
                    this.classificationRule1.ParameterName = "ClassificationDisplayRules";

                    
                    UpdateViewComponent2();
                }
            }
        }

        public String SpeciesName
        {
            get
            {
                return speciesName;
            }
            set
            {
                speciesName = value;
                customTabPage1.Description = speciesName;
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
                ckbxIsWeapon.Controller = controller;
                ckbxRemoveOnDestruction.Controller = controller;
                nndCollisionRadius.Controller = controller;
                rbLand.Controller = controller;
                rbAir.Controller = controller;
                rbSea.Controller = controller;
                rbExistingSpecies.Controller = controller;
                lkbxType.Controller = controller;
                customCheckBoxLaunchedByOwner.Controller = controller;
                customLinkBoxDecisionMakers.Controller = controller;
                this.simpleLinkGrid1.Controller = controller;
                this.classificationRule1.Controller = controller;
                controller.RegisterForUpdate(this);
            }
        }

        public void UpdateViewComponent()
        {
            if (this.Parent != null && !updating)
            {
                if (rbLand.Checked || rbAir.Checked || rbSea.Checked)
                {
                    lkbxType.DeleteAllLinks();
                    lkbxType.Enabled = false;
                }
                else if (rbExistingSpecies.Checked)
                {
                    lkbxType.Enabled = true;
                }
            }
        }

        private Boolean updating = false;

        public void UpdateViewComponent2()
        {
            updating = true;

            ckbxIsWeapon.UpdateViewComponent();
            ckbxRemoveOnDestruction.UpdateViewComponent();
            nndCollisionRadius.UpdateViewComponent();
            rbLand.UpdateViewComponent();
            rbAir.UpdateViewComponent();
            rbSea.UpdateViewComponent();
            rbExistingSpecies.UpdateViewComponent();
            lkbxType.UpdateViewComponent();
            customCheckBoxLaunchedByOwner.UpdateViewComponent();
            customLinkBoxDecisionMakers.UpdateViewComponent();
            this.simpleLinkGrid1.UpdateViewComponent();
            this.classificationRule1.UpdateViewComponent();

            if (rbLand.Checked || rbAir.Checked || rbSea.Checked)
            {
                lkbxType.DeleteAllLinks();
                lkbxType.Enabled = false;
            }
            else if (rbExistingSpecies.Checked)
            {
                lkbxType.Enabled = true;
            }

            updating = false;
        }

        #endregion

        private void customCheckBoxLaunchedByOwner_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
