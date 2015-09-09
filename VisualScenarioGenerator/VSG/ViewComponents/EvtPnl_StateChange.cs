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
    public partial class EvtPnl_StateChange : UserControl, AME.Views.View_Components.IViewComponent
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

        public EvtPnl_StateChange()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
            //eventID1.unitLinkBox.SelectedIndexChanged += new System.EventHandler(this.unitLinkBox_SelectedIndexChanged);
        }
        //private void unitLinkBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
            //UpdateViewComponent();
        //}
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
                timeBox.Controller = value;
                stateComboBox.Controller = _controller;
                fromStateComboBox.Controller = _controller;
                exceptStateComboBox.Controller = _controller;
                engramRange1.Controller = _controller;
                eventID1.Controller = _controller;
            }
        }

        public void UpdateViewComponent()
        {
            VSGController myController = (VSGController)Controller;
            //unitIDBox.Text = myController.GetComponentName(parentCompID);
            //timeBox.UpdateViewComponent();
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

            eventID1.UpdateViewComponent();
            List<int> ids = myController.GetChildIDs(displayID, "CreateEvent", "EventID");
            int createID = -1;
            if (ids.Count > 0)
            {
                createID = ids[0];
            }
            else
            {
                createID = -1;
                //stateLinkBox.DeleteAllLinks();
                //fromStateLinkBox.DeleteAllLinks();
                //exceptStateLinkBox.DeleteAllLinks();
            }

            if (eventID1.Unit)
            {
                stateComboBox.ShowAllStates = true;
                fromStateComboBox.ShowAllStates = true;
                exceptStateComboBox.ShowAllStates = true;

            }
            else
            {
                stateComboBox.ShowAllStates = false;
                fromStateComboBox.ShowAllStates = false;
                exceptStateComboBox.ShowAllStates = false;
            }


            ids = myController.GetChildIDs(createID, "Species", "CreateEventKind");
            if (ids.Count >= 1)
            {
                stateComboBox.SpeciesId = ids[0];
                stateComboBox.ComponentId = DisplayID;
                stateComboBox.ParameterCategory = "StateChangeEvent";
                stateComboBox.ParameterName = "State";
                //stateComboBox.ShowAllStates = false;

                //stateLinkBox.DisplayRootId = ids[0];
                //stateLinkBox.DisplayComponentType = "State";
                //stateLinkBox.DisplayLinkType = "Scenario";
                //stateLinkBox.ConnectRootId = DisplayID;
                //stateLinkBox.ConnectFromId = DisplayID;
                //stateLinkBox.ConnectLinkType = "StateChangeEventNewState";
                //stateLinkBox.OneToMany = false;

                fromStateComboBox.SpeciesId = ids[0];
                fromStateComboBox.ComponentId = DisplayID;
                fromStateComboBox.ParameterCategory = "StateChangeEvent";
                fromStateComboBox.ParameterName = "FromState";
                //fromStateComboBox.ShowAllStates = false;

                //fromStateLinkBox.DisplayRootId = ids[0];
                //fromStateLinkBox.DisplayComponentType = "State";
                //fromStateLinkBox.DisplayLinkType = "Scenario";
                //fromStateLinkBox.ConnectRootId = DisplayID;
                //fromStateLinkBox.ConnectFromId = DisplayID;
                //fromStateLinkBox.ConnectLinkType = "StateChangeEventFromState";
                //fromStateLinkBox.OneToMany = true;

                exceptStateComboBox.SpeciesId = ids[0];
                exceptStateComboBox.ComponentId = DisplayID;
                exceptStateComboBox.ParameterCategory = "StateChangeEvent";
                exceptStateComboBox.ParameterName = "ExceptState";
                //exceptStateComboBox.ShowAllStates = false;

                //exceptStateLinkBox.DisplayRootId = ids[0];
                //exceptStateLinkBox.DisplayComponentType = "State";
                //exceptStateLinkBox.DisplayLinkType = "Scenario";
                //exceptStateLinkBox.ConnectRootId = DisplayID;
                //exceptStateLinkBox.ConnectFromId = DisplayID;
                //exceptStateLinkBox.ConnectLinkType = "StateChangeEventExceptState";
                //exceptStateLinkBox.OneToMany = true;
            }
            else
            {
                stateComboBox.SpeciesId = -1;
                fromStateComboBox.SpeciesId = -1;
                exceptStateComboBox.SpeciesId = -1;

                stateComboBox.ShowAllStates = true;
                stateComboBox.ComponentId = DisplayID;
                stateComboBox.ParameterCategory = "StateChangeEvent";
                stateComboBox.ParameterName = "State";

                fromStateComboBox.ShowAllStates = true;
                fromStateComboBox.ComponentId = DisplayID;
                fromStateComboBox.ParameterCategory = "StateChangeEvent";
                fromStateComboBox.ParameterName = "FromState";

                exceptStateComboBox.ShowAllStates = true;
                exceptStateComboBox.ComponentId = DisplayID;
                exceptStateComboBox.ParameterCategory = "StateChangeEvent";
                exceptStateComboBox.ParameterName = "ExceptState";

                //stateLinkBox.DisplayRootId = -1;
                //fromStateLinkBox.DisplayRootId = -1;
                //exceptStateLinkBox.DisplayRootId = -1;
            }

            stateComboBox.UpdateViewComponent();
            fromStateComboBox.UpdateViewComponent();
            exceptStateComboBox.UpdateViewComponent();
            engramRange1.UpdateViewComponent();
        }

        #endregion
    }
}
