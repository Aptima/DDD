using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VSG.Libraries;
using VSG.Dialogs;

using AME.Views.View_Components;
using AME.Controllers;
using VSG.Controllers;

namespace VSG.ViewComponents
{
    public partial class ScorePanel : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        //private ScoreDataStruct _datastore = ScoreDataStruct.Empty;
        private Int32 scoreID = -1;
        private IController controller;
        public ScorePanel()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
           
            //bbSelectedRules.SetLabels("Available Rules:", "Selected Rules:");
        }

        public int ScoreID
        {
            get
            {
                return scoreID;
            }
            set
            {
                scoreID = value;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {


        }
        public void UpdateViewComponent()
        {
            if (scoreID >= 0)
            {
                String name = ((VSGController)controller).GetComponentName(scoreID);
                //label4.Text = String.Format("Score: {0}",name);
                tabPage.Description = name;
                txtInitialValue.ComponentId = scoreID;
                txtInitialValue.UpdateViewComponent();

                dmCalculateBox.DisplayRootId = ((VSGController)controller).ScenarioId;
                dmCalculateBox.DisplayComponentType = "DecisionMaker";
                dmCalculateBox.DisplayLinkType = "Scenario";
                dmCalculateBox.ConnectRootId = scoreID;
                dmCalculateBox.ConnectFromId = scoreID;
                dmCalculateBox.ConnectLinkType = "ScoreApplies";
                dmCalculateBox.OneToMany = true;
                dmCalculateBox.UpdateViewComponent();


                dmViewBox.DisplayRootId = ((VSGController)controller).ScenarioId;
                dmViewBox.DisplayComponentType = "DecisionMaker";
                dmViewBox.DisplayLinkType = "Scenario";
                dmViewBox.ConnectRootId = scoreID;
                dmViewBox.ConnectFromId = scoreID;
                dmViewBox.ConnectLinkType = "ScoreViewers";
                dmViewBox.OneToMany = true;
                dmViewBox.UpdateViewComponent();

                rulesLinkBox.DisplayRootId = ((VSGController)controller).ScenarioId;
                rulesLinkBox.DisplayComponentType = "Rule";
                rulesLinkBox.DisplayLinkType = "Scenario";
                rulesLinkBox.ConnectLinkType = "ScoreRules";
                rulesLinkBox.ConnectRootId = scoreID;
                rulesLinkBox.ConnectFromId = scoreID;
                rulesLinkBox.OneToMany = true;
                rulesLinkBox.UpdateViewComponent();
            }
        }
        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
                txtInitialValue.Controller = controller;
                dmCalculateBox.Controller = controller;
                dmViewBox.Controller = controller;
                rulesLinkBox.Controller = controller;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
