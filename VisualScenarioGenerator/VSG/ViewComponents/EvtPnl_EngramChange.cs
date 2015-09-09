using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class EvtPnl_EngramChange : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _controller;
        private int displayID = -1;
        private int parentCompID = -1;
        private bool populating = false;
        private bool updating = false;

        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                displayID = value;
                engramValue.ComponentId = value;
                timeUpDown.ComponentId = value;
                customCheckBoxSpecificUnit.ComponentId = value;
                engramUnitID1.DisplayID = value;

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
        public EvtPnl_EngramChange()
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

                engramValue.Controller = value;
                timeUpDown.Controller = value;
                engramNameBox.Controller = value;
                customCheckBoxSpecificUnit.Controller = value;
                engramUnitID1.Controller = value;
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

            engramNameBox.DisplayRootId = _controller.ScenarioId;
            engramNameBox.DisplayLinkType = "Scenario";
            engramNameBox.DisplayComponentType = "Engram";
            engramNameBox.ConnectRootId = DisplayID;
            engramNameBox.ConnectFromId = DisplayID;
            engramNameBox.ConnectLinkType = "EngramID";
            engramNameBox.UpdateViewComponent();
            engramValue.UpdateViewComponent();
            customCheckBoxSpecificUnit.UpdateViewComponent();
            engramUnitID1.UpdateViewComponent();

            engramUnitID1.Enabled = customCheckBoxSpecificUnit.Checked;
        }

        #endregion

    }
}
