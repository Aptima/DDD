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
    public partial class EvtPnl_Create : UserControl, AME.Views.View_Components.IViewComponent
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
                //subplatformsListBox.DisplayID = value;
                
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


        public EvtPnl_Create()
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
                ownerLinkBox.Controller = _controller;
                kindLinkBox.Controller = _controller;
                subplatformLinkBox.Controller = _controller;
                //subplatformsListBox.Controller = _controller;
                
            }
        }

        public void UpdateViewComponent()
        {
            unitIDBox.Text = ((VSGController)Controller).GetComponentName(DisplayID);
            
            ownerLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            ownerLinkBox.DisplayComponentType = "DecisionMaker";
            ownerLinkBox.DisplayLinkType = "Scenario";
            ownerLinkBox.ConnectRootId = ((VSGController)Controller).ScenarioId;
            ownerLinkBox.ConnectFromId = DisplayID;
            ownerLinkBox.ConnectLinkType = "Scenario";
            ownerLinkBox.OneToMany = false;
     
            kindLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            kindLinkBox.DisplayComponentType = "Species";
            kindLinkBox.DisplayLinkType = "Scenario";
            kindLinkBox.ConnectRootId = this.DisplayID;
            kindLinkBox.ConnectFromId = this.DisplayID;
            kindLinkBox.ConnectLinkType = "CreateEventKind";
            kindLinkBox.OneToMany = false;

            subplatformLinkBox.DisplayRootId = ((VSGController)Controller).ScenarioId;
            subplatformLinkBox.DisplayComponentType = "CreateEvent";
            subplatformLinkBox.DisplayLinkType = "Scenario";
            subplatformLinkBox.ConnectRootId = this.DisplayID;
            subplatformLinkBox.ConnectFromId = DisplayID;
            subplatformLinkBox.ConnectLinkType = "CreateEventSubplatform";
            subplatformLinkBox.OneToMany = true;

            kindLinkBox.UpdateViewComponent();
            ownerLinkBox.UpdateViewComponent();
            subplatformLinkBox.UpdateViewComponent();

            //subplatformsListBox.UpdateViewComponent();
            
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ownerLinkBox.DeleteAllLinks();
        }



    }
}
