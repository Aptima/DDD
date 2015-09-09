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
    public partial class Combo : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 speciesId = -1;
        private Int32 comboId = -1;
        private String comboName;

        private VSGController controller;

        public String ComboName
        {
            get
            {
                return comboName;
            }
            set
            {
                comboName = value;
                customTabPage1.Description = comboName;
            }
        }

        public Combo()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 ComboId
        {
            get { return comboId; }
            set
            {
                comboId = value;
                if (comboId > 0)
                {


                    stateComboBox1.ComponentId = comboId;

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
                stateComboBox1.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {

            stateComboBox1.UpdateViewComponent();
        }

        #endregion
    }
}
