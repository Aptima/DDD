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
    public partial class EvtPnl_VoiceChannelOpen : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _controller;
        private int displayID = -1;
        private int parentCompID = -1;
        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                displayID = value;
                voiceChannelName.ComponentId = value;
                timeUpDown.ComponentId = value;
            }
        }
        public int ParentCompID
        {
            get
            {
                return parentCompID;
            }
            set
            {
                parentCompID = value;
            }
        }
        public EvtPnl_VoiceChannelOpen()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = (VSGController)value;
                voiceChannelName.Controller = value;
                timeUpDown.Controller = value;
                accessLinkBox.Controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            String type = _controller.GetComponentType(ParentCompID);
            timeUpDown.UpdateViewComponent();
            if (type == "ReiterateEvent"
                || type == "CompletionEvent"
                || type == "SpeciesCompletionEvent")
            {
                timeUpDown.Enabled = false;
            }
            else
            {
                timeUpDown.Enabled = true;
            }

            VSGController myController = (VSGController)Controller;
            voiceChannelName.UpdateViewComponent();
            //timeUpDown.UpdateViewComponent();

            accessLinkBox.DisplayRootId = myController.ScenarioId;
            accessLinkBox.DisplayComponentType = "DecisionMaker";
            accessLinkBox.DisplayLinkType = "Scenario";
            accessLinkBox.ConnectRootId = DisplayID;
            accessLinkBox.ConnectFromId = DisplayID;
            accessLinkBox.ConnectLinkType = "OpenVoiceChannelEventAccess";
            accessLinkBox.OneToMany = true;
            accessLinkBox.UpdateViewComponent();
        }

        #endregion

        private void voiceChannelName_TextChanged(object sender, EventArgs e)
        {

        }

        private void timeUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void accessLinkBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EvtPnl_VoiceChannelOpen_Load(object sender, EventArgs e)
        {

        }
    }
}
