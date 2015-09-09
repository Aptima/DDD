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
    public partial class EvtPnl_ChatroomOpen : UserControl, AME.Views.View_Components.IViewComponent
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
                chatRoomName.ComponentId = value;
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
        public EvtPnl_ChatroomOpen()
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
                chatRoomName.Controller = value;
                timeUpDown.Controller = value;
                membersLinkBox.Controller = value;
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
            chatRoomName.UpdateViewComponent();
            //timeUpDown.UpdateViewComponent();


            membersLinkBox.DisplayRootId = myController.ScenarioId;
            membersLinkBox.DisplayComponentType = "DecisionMaker";
            membersLinkBox.DisplayLinkType = "Scenario";
            membersLinkBox.ConnectRootId = DisplayID;
            membersLinkBox.ConnectFromId = DisplayID;
            membersLinkBox.ConnectLinkType = "OpenChatRoomEventMember";
            membersLinkBox.OneToMany = true;
            membersLinkBox.UpdateViewComponent();
        }

        #endregion
    }
}
