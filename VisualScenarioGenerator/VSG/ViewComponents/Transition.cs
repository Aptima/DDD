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
    public partial class Transition : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 speciesId = -1;
        private Int32 transitionId = -1;
        private String transitionName;

        private VSGController controller;

        public String TransitionName
        {
            get
            {
                return transitionName;
            }
            set
            {
                transitionName = value;
                customTabPage1.Description = transitionName;
            }
        }

        public Transition()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 TransitionId
        {
            get { return transitionId; }
            set
            {
                transitionId = value;
                if (transitionId > 0)
                {
                    nudIntensity.ComponentId = transitionId;
                    nudProbability.ComponentId = transitionId;

                    nndRange.ComponentId = transitionId;

                    stateComboBox1.ComponentId = transitionId;

                    UpdateViewComponent();
                }
            }
        }
        public Int32 SpeciesId
        {
            get { return speciesId; }
            set
            {
                speciesId = value;
                if (speciesId > 0)
                {
                    stateComboBox1.SpeciesId = speciesId;

                    UpdateViewComponent();
                }
            }
        }

        private void reset()
        {

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
                nndRange.Controller = controller;
                nudIntensity.Controller = controller;
                nudProbability.Controller = controller;
                stateComboBox1.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            nudIntensity.UpdateViewComponent();
            nudProbability.UpdateViewComponent();
            nndRange.UpdateViewComponent();
            stateComboBox1.UpdateViewComponent();
        }

        #endregion
    }
}
