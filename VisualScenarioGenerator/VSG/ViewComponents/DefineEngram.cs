using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using GME.Controllers;

namespace VSG.ViewComponents
{
    public partial class DefineEngram : UserControl, GME.Views.View_Components.IViewComponent
    {
        private Int32 defineEngramId = -1;
        private VSGController controller;
        private string defineEngramName;




        public DefineEngram()
        {
            InitializeComponent();

        }

 /*       public Int32 DefineEngramId
        {
            get
            {
                return defineEngramId;
            }
            set
            {
                defineEngramId = value;
                if (defineEngramId >= 0)
                {
                    defineEngramId.ComponentId = defineEngramId;
                    txtInitialValue.ComponentId = defineEngramId;

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }*/

        public Int32 DefineEngramId
        {
            get
            {
                return defineEngramId;
            }
            set
            {
                defineEngramId = value;
                if (defineEngramId >= 0)
                {
                    txtInitialValue.ComponentId = defineEngramId;
                    customLinkBox1.DisplayRootId = controller.ScenarioId;
                    customLinkBox1.ConnectRootId = defineEngramId;

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }
        public String DefineEngramName
        {
            get
            {
                return defineEngramName;
            }
            set
            {
                defineEngramName = value;
                customTabPage1.Description = "Name: " + defineEngramName;
            }
        }

        private void reset()
        {

        }

        #region IViewComponent Members

        public GME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = (VSGController)value;  // Only give this a vsg controller.
                       txtInitialValue.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
                txtInitialValue.UpdateViewComponent();
          }

        #endregion
    }


}
