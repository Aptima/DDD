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
    public partial class Team : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 teamId = -1;
        private VSGController controller;
        private String teamName;

        public Team()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 TeamId
        {
            get
            {
                return teamId;
            }
            set
            {
                teamId = value;
                if (teamId >= 0)
                {
                    customLinkBox1.DisplayRootId = controller.ScenarioId;
                    customLinkBox1.ConnectRootId = teamId;
                    customLinkBox1.ConnectFromId = teamId;

                    // use dynamic link type
                    if (controller != null && customLinkBox1.ConnectLinkType != null)
                    {
                        String dynamic = controller.GetDynamicLinkType(customLinkBox1.ConnectLinkType, teamId.ToString());
                        customLinkBox1.ConnectLinkType = dynamic;
                    }

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }

        public String TeamName
        {
            get
            {
                return teamName;
            }
            set
            {
                teamName = value;
                customTabPage1.Description = teamName;
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
                customLinkBox1.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            customLinkBox1.UpdateViewComponent();
        }

        #endregion

  

      
    }
}
