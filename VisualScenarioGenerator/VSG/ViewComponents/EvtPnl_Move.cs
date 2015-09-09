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
    public partial class EvtPnl_Move : UserControl, AME.Views.View_Components.IViewComponent
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
                timeBox.ComponentId = displayID;
                xBox.ComponentId = displayID;
                yBox.ComponentId = displayID;
                zBox.ComponentId = displayID;
                throttleBox.ComponentId = displayID;
                engramRange1.DisplayID = displayID;
                eventID1.DisplayID = displayID;
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
        public EvtPnl_Move()
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
                xBox.Controller = _controller;
                yBox.Controller = _controller;
                zBox.Controller = _controller;
                throttleBox.Controller = _controller;
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
            xBox.UpdateViewComponent();
            yBox.UpdateViewComponent();
            zBox.UpdateViewComponent();
            throttleBox.UpdateViewComponent();
            engramRange1.UpdateViewComponent();
            eventID1.UpdateViewComponent();
        }

        #endregion
    }
}
