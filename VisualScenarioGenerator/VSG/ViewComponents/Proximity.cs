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
    public partial class Proximity : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController controller;
        private String proximityName;
        private Int32 componentId;
        private Int32 proximityId;

        public Int32 ProximityId
        {
            get { return proximityId; }
            set
            {
                proximityId = value;
                if (proximityId > 0)
                {
                    textBoxRange.ComponentId = proximityId;
                    UpdateViewComponent();
                }
            }
        }

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

        public String ProximityName
        {
            get
            {
                return proximityName;
            }
            set
            {
                proximityName = value;
                customTabPage1.Description = proximityName;
            }
        }

        public Proximity()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
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
                textBoxRange.Controller = controller;

            }
        }

        public void UpdateViewComponent()
        {
            textBoxRange.UpdateViewComponent();
        }

        #endregion



  
  
    

 

    }
}
