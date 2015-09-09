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
    public partial class EvtPnl_Transfer : UserControl, AME.Views.View_Components.IViewComponent
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
                timeBox.ComponentId = value;
                engramRange1.DisplayID = value;
                eventID1.DisplayID = value;
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
                eventID1.ParentID = value;
            }
        }
        public EvtPnl_Transfer()
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
                timeBox.Controller = _controller;
                fromLinkBox.Controller = _controller;
                toLinkBox.Controller = _controller;
                engramRange1.Controller = _controller;
                eventID1.Controller = _controller;
            }
        }

        public void UpdateViewComponent()
        {
            String type = _controller.GetComponentType(ParentCompID);
            timeBox.UpdateViewComponent();
            if (type == "ReiterateEvent"
                || type == "CompletionEvent"
                || type == "SpeciesCompletionEvent")
            {
                timeBox.Enabled = false;
            }
            else
            {
                timeBox.Enabled = true;
            }

            //unitIDBox.Text = ((VSGController)Controller).GetComponentName(parentCompID);
            eventID1.UpdateViewComponent();
            fromLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            fromLinkBox.DisplayComponentType = "DecisionMaker";
            fromLinkBox.DisplayLinkType = "Scenario";
            fromLinkBox.OneToMany = false;
            //fromLinkBox.ConnectRootId = ((VSGController)Controller).ScenarioId;
            fromLinkBox.ConnectRootId = DisplayID;
            fromLinkBox.ConnectFromId = DisplayID;
            fromLinkBox.ConnectLinkType = "TransferEventFromDM";

            toLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            toLinkBox.DisplayComponentType = "DecisionMaker";
            toLinkBox.DisplayLinkType = "Scenario";
            toLinkBox.OneToMany = false;
            toLinkBox.ConnectRootId = DisplayID;
            toLinkBox.ConnectFromId = DisplayID;
            toLinkBox.ConnectLinkType = "TransferEventToDM";

            //timeBox.UpdateViewComponent();
            fromLinkBox.UpdateViewComponent();
            toLinkBox.UpdateViewComponent();
            engramRange1.UpdateViewComponent();
        }

        #endregion


    }
}
