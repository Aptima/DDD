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
    public partial class Engram : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 engramId = -1;
        private VSGController controller;
        private string engramName;




        public Engram()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

        }

        /*       public Int32 EngramId
               {
                   get
                   {
                       return engramId;
                   }
                   set
                   {
                       engramId = value;
                       if (engramId >= 0)
                       {
                           engramId.ComponentId = engramId;
                           txtInitialValue.ComponentId = engramId;

                           // Reset can happen here. Only one playfield is allowed.
                           reset();
                           UpdateViewComponent();
                       }
                   }
               }*/

        public Int32 EngramId
        {
            get
            {
                return engramId;
            }
            set
            {
                engramId = value;
                if (engramId >= 0)
                {
                    txtInitialValue.ComponentId = engramId;
                    customLinkBox1.DisplayRootId = controller.ScenarioId;
                    customLinkBox1.ConnectRootId = engramId;
                    customParameterEnumBoxDataType.ComponentId = engramId;

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }
        public String EngramName
        {
            get
            {
                return engramName;
            }
            set
            {
                engramName = value;
                customTabPage1.Description = engramName;
            }
        }

        private void reset()
        {

        }

        private void PopulateDataTypes()
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
                txtInitialValue.Controller = controller;
                customParameterEnumBoxDataType.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            txtInitialValue.UpdateViewComponent();
            customParameterEnumBoxDataType.UpdateViewComponent();
        }

        #endregion
    }


}
