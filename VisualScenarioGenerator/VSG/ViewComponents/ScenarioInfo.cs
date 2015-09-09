using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Libraries;
using VSG.Dialogs;
using VSG.Controllers;

using AME.Views.View_Components;
using AME.Controllers;

namespace VSG.ViewComponents
{
    public partial class ScenarioInfo : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 scenarioId = -1;
        private VSGController controller;

        public ScenarioInfo()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 ScenarioId
        {
            get
            {
                return scenarioId;
            }
            set
            {
                scenarioId = value;
                if (scenarioId >= 0)
                {
                    scenarioId = value;
                    scenario_name.ComponentId = scenarioId;
                    scenario_description.ComponentId = scenarioId;
                    scenario_time_to_attack.ComponentId = scenarioId;
                    customParameterEnumBoxRangeRingDisplay.EnumName = "RangeRingDisplayEnum";
                    customParameterEnumBoxRangeRingDisplay.ComponentId = scenarioId;
                    customCheckBoxAssetTransfer.ComponentId = scenarioId;

                    UpdateViewComponent();
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
                controller = value as VSGController;
                scenario_name.Controller = controller;
                scenario_description.Controller = controller;
                scenario_time_to_attack.Controller = controller;
                customParameterEnumBoxRangeRingDisplay.Controller = controller;
                customCheckBoxAssetTransfer.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            scenario_name.UpdateViewComponent();
            scenario_description.UpdateViewComponent();
            scenario_time_to_attack.UpdateViewComponent();
            customParameterEnumBoxRangeRingDisplay.UpdateViewComponent();
            customCheckBoxAssetTransfer.UpdateViewComponent();
        }

        #endregion

        private void scenario_time_to_attack_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
