using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using VSG.Controllers;

using AME.Controllers;
using AME.Views.View_Components;

namespace VSG.ViewComponents
{
    public partial class EvtPnl_Completion : UserControl, AME.Views.View_Components.IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController _controller;
        private int displayID = -1;
        private int parentCompID = -1;
        //private int createCompID = -1;
        public int DisplayID
        {
            get
            {
                return displayID;
            }
            set
            {
                displayID = value;
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

        public EvtPnl_Completion()
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
                //stateLinkBox.Controller = _controller;
                stateComboBox.Controller = _controller;
                engramRange1.Controller = _controller;
                eventID1.Controller = _controller;
            }
        }


        public void UpdateViewComponent()
        {
            VSGController myController = (VSGController)Controller;
            //unitIDBox.Text = myController.GetComponentName(parentCompID);
            eventID1.UpdateViewComponent();
            ComponentOptions op = new ComponentOptions();
            op.LevelDown = 1;

            String text = String.Empty;

            IXPathNavigable inav = myController.GetParametersForComponent(DisplayID);
            XPathNavigator nav = inav.CreateNavigator();
            XPathNavigator node = nav.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", "CompletionEvent", "Action"));
            if (node != null)
            {
                text = node.GetAttribute("value", String.Empty);
            }
            actionCombo.SelectedItem = text;
            if (text == "")
            {
                stateRadio.Checked = true;
                stateComboBox.Enabled = true;
                
                actionCombo.Enabled = false;
            }
            else
            {
                actionRadio.Checked = true;
                //stateRadio.Checked = false;
                stateComboBox.Enabled = false;
                actionCombo.Enabled = true;
            }

            List<int> ids = myController.GetChildIDs(displayID, "CreateEvent", "EventID");
            int createID = -1;
            if (ids.Count > 0)
            {
                createID = ids[0];
            }
            ids = myController.GetChildIDs(createID, "Species", "CreateEventKind");

            if (eventID1.Unit)
            {
                stateComboBox.ShowAllStates = true;
            }
            else
            {
                stateComboBox.ShowAllStates = false;
            }

            if (ids.Count >= 1)
            {
                //stateLinkBox.
                //stateLinkBox.DisplayRootId = ids[0];
                //stateLinkBox.DisplayComponentType = "State";
                //stateLinkBox.DisplayLinkType = "Scenario";
                //stateLinkBox.ConnectRootId = DisplayID;
                //stateLinkBox.ConnectFromId = DisplayID;
                //stateLinkBox.ConnectLinkType = "CompletionEventNewState";

                stateComboBox.SpeciesId = ids[0];
                stateComboBox.ComponentId = DisplayID;
                stateComboBox.ParameterCategory = "CompletionEvent";
                stateComboBox.ParameterName = "State";



            }
            else
            {
                //stateLinkBox.DisplayRootId = -1;
                stateComboBox.SpeciesId = -1;
            }

            stateComboBox.UpdateViewComponent();
            engramRange1.UpdateViewComponent();
        }

        #endregion

        void radioSelectChanged()
        {
            if (actionRadio.Checked)
            {
                actionCombo.Enabled = true;
                stateComboBox.Enabled = false;
                //stateLinkBox.DeleteAllLinks();
                actionCombo.SelectedItem = "MoveComplete_Event";
                ((VSGController)Controller).UpdateParameters(DisplayID, "CompletionEvent" + "." + "Action", (String)actionCombo.SelectedItem, AME.Controllers.eParamParentType.Component);
            }
            else if (stateRadio.Checked)
            {
                actionCombo.Enabled = false;
                stateComboBox.Enabled = true;
                ((VSGController)Controller).UpdateParameters(DisplayID, "CompletionEvent" + "." + "Action", "", AME.Controllers.eParamParentType.Component);
            }
        }

        private void actionRadio_CheckedChanged(object sender, EventArgs e)
        {
            radioSelectChanged();
        }

        private void stateRadio_CheckedChanged(object sender, EventArgs e)
        {
            radioSelectChanged();
        }

        private void actionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String item = (String)actionCombo.SelectedItem;
            ((VSGController)Controller).UpdateParameters(DisplayID, "CompletionEvent" + "." + "Action", item, AME.Controllers.eParamParentType.Component);
        }
    }
}
