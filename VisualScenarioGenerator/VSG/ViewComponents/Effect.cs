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
    public partial class Effect : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController controller;
        private String effectName;
        private Int32 componentId;
        private Int32 effectId;

        public Int32 EffectId
        {
            get { return effectId; }
            set
            {
                effectId = value;
                if (effectId > 0)
                {
                    upDownIntensity.ComponentId = effectId;
                    
                    probabilityCustomNumericUpDownDouble.ComponentId = effectId;
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

        public String EffectName
        {
            get
            {
                return effectName;
            }
            set
            {
                effectName = value;
                customTabPage1.Description = effectName;
            }
        }

        public Effect()
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
                upDownIntensity.Controller = controller;
                probabilityCustomNumericUpDownDouble.Controller = controller;
                

            }
        }

        public void UpdateViewComponent()
        {
            upDownIntensity.UpdateViewComponent();
            
            probabilityCustomNumericUpDownDouble.UpdateViewComponent();
        }

        #endregion



  
  
    

 

    }
}
