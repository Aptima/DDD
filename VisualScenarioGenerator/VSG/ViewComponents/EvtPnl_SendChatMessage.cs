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
    public partial class EvtPnl_SendChatMessage : UserControl, AME.Views.View_Components.IViewComponent
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
                timeUpDown.ComponentId = value;
                messageTextCustomParameterTextBox.ComponentId = value;
                chatRoomLinkBox.ConnectFromId = value;
                chatRoomLinkBox.ConnectRootId = value;
                senderDMLinkBox.ConnectFromId = value;
                senderDMLinkBox.ConnectRootId = value;
                if (_controller != null)
                {
                    chatRoomLinkBox.DisplayRootId = _controller.ScenarioId;
                    senderDMLinkBox.DisplayRootId = _controller.ScenarioId;
                }
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

        public EvtPnl_SendChatMessage()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = (VSGController)value;
                timeUpDown.Controller = value;
                chatRoomLinkBox.Controller = value;
                senderDMLinkBox.Controller = value;
                messageTextCustomParameterTextBox.Controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            //timeUpDown.UpdateViewComponent();
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
            chatRoomLinkBox.UpdateViewComponent();
            senderDMLinkBox.UpdateViewComponent();
            messageTextCustomParameterTextBox.UpdateViewComponent();
        }

        #endregion
    }
}
