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
    public partial class Network : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 teamId = -1;
        private VSGController controller;
        private String teamName;

        public Network()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 NetworkId
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

                    // Reset can happen here. Only one playfield is allowed.
                    reset();
                    UpdateViewComponent();
                }
            }
        }

        public String NetworkName
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

        private void customLinkBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

  

      
    }
}
