using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;
using View_Components;
using Controllers;

namespace GME.Views.View_Components {

    public partial class AssessmentToolbar : UserControl {

        public const int SHOWTABLE = 0;
        public const int SHOWGRAPH = 1;

        private static readonly ILog logger = LogManager.GetLogger(typeof(AssessmentToolbar));

        AssessmentController assessmentController = null;
        private DataGridView navigator = null;
        private AssessmentResultsPanel resultsPanel = null;

        /// <summary>
        /// Deletage to load the Navigator in the Assessment Form
        /// </summary>
        /// <param name="simmulationId">Simmulation Component ID</param>
        public delegate void LoadSimmulationGridDelegate(int simmulationId);
        public LoadSimmulationGridDelegate loadSimmulationCallback;

        public delegate void LoadSimRunDataDelegate(int tableOrGraph);
        public LoadSimRunDataDelegate loadSimRunDataCallback; 


        public AssessmentToolbar() {

            InitializeComponent();
            
            this.clearAll();
            GMEManager cm = GMEManager.Instance;

            RootController projectController = (RootController)cm.Get("ProjectEditor");
            this.assessmentController = (AssessmentController)cm.Get("AssessmentEditor");
            this.assessmentController.ComponentUpdate += 
                new ComponentUpdateHandler(this.cEvent_ComponentUpdate);

            // Init Simulation Selection CustomCombo
            this.simmulationComboBox.Controller = projectController;
            this.simmulationComboBox.Type = assessmentController.RootComponentType;
            this.simmulationComboBox.LinkType = projectController.RootComponentType;
            this.simmulationComboBox.SelectionChangeCommitted +=
                new EventHandler(this.simmulationComboBox_SelectionChangeCommitted);

            logger.Debug("Created AssessmentToolbar");
        }

        public CustomCombo SimmulationComboBox {
            get {
                return this.simmulationComboBox.CustomComboControl;
            }
        }

        public void UpdateViewComponent() {
            this.simmulationComboBox.UpdateViewComponent();
        }

        public void setNavigator(DataGridView navigator) {
            this.navigator = navigator;
            if (this.navigator == null)
                logger.Warn("AssessmentToolbar parameter navigator is NULL");
            else {
                this.navigator.SelectionChanged +=
                    new System.EventHandler(this.navigator_SelectionChanged);
            }
        }

        public void setResultsPanel(AssessmentResultsPanel resultsPanel) {
            this.resultsPanel = resultsPanel;
            if (this.resultsPanel == null)
                logger.Warn("AssessmentToolbar parameter resultsPanel is NULL");
            else {
                this.resultsPanel.ResultsTabControl.Selected +=
                    new System.Windows.Forms.TabControlEventHandler(this.resultsPanel_TabIndexChanged);

                //this.resultsPanel.TabIndexChanged +=
                //    new System.EventHandler(this.resultsPanel_TabIndexChanged);
            }
        }

        /// <summary>
        /// Clears all buttons but setting them disabled.  Called on init when no rows in the
        /// navigator table have been selected.  Also should be called when a new SimRun is created
        /// by the Simmulation.
        /// </summary>
        private void clearAll() {
            this.displayTableDataButton.Enabled = false;
            this.graphDataButton.Enabled = false;
            this.tablePageButton.Enabled = false;
            this.thumbnailPageButton.Enabled = false;
            this.bigGraphPageButton.Enabled = false;
        }

        private void navigator_SelectionChanged(object sender, EventArgs e) {
            this.graphDataButton.Enabled = true;
            if (this.navigator.SelectedRows.Count == 1) {
                this.displayTableDataButton.Enabled = true;
            }
            else {
                this.displayTableDataButton.Enabled = false;
            }
        }

        private void resultsPanel_TabIndexChanged(object sender, EventArgs e) {
            int selectedTabIndex = ((TabControlEventArgs)e).TabPageIndex;
            int highIndex = this.resultsPanel.HighestTabSelected;
            int lowIndex = this.resultsPanel.LowestTabSelected;

            for (int i = lowIndex; i <= highIndex; i++) {
                switch (i) {
                    case AssessmentResultsPanel.RAWDATAPANEL:
                        this.tablePageButton.Enabled = (i != selectedTabIndex);
                        break;
                    case AssessmentResultsPanel.SMALLGRAPHPANEL:
                        this.thumbnailPageButton.Enabled = (i != selectedTabIndex);
                        break;
                    case AssessmentResultsPanel.BIGGRAPHPANEL:
                        this.bigGraphPageButton.Enabled = (i != selectedTabIndex);
                        break;
                }
            }
        }

        private void simmulationComboBox_SelectionChangeCommitted(object sender, EventArgs e) {
            CustomCombo senderCast = ((CustomComboToolStripItem)sender).CustomComboControl;
            ComboItem item = (ComboItem)senderCast.SelectedItem;

            if (item != null && senderCast.SelectedID != item.MyID) { // == means no change, don't update it
                senderCast.SelectedID = item.MyID;
                this.loadSimmulationCallback(item.MyID);
            }
        }

        private void cEvent_ComponentUpdate() {
            this.simmulationComboBox.UpdateViewComponent();
        }
        
        private void displayTableDataButton_Click(object sender, EventArgs e) {
            this.loadSimRunDataCallback(AssessmentToolbar.SHOWTABLE);
        }

        private void graphDataButton_Click(object sender, EventArgs e) {
            this.loadSimRunDataCallback(AssessmentToolbar.SHOWGRAPH);
        }

        private void tablePageButton_Click(object sender, EventArgs e) {
            this.resultsPanel.ResultsTabControl.SelectTab(AssessmentResultsPanel.RAWDATAPANEL);
        }

        private void thumbnailPageButton_Click(object sender, EventArgs e) {
            this.resultsPanel.ResultsTabControl.SelectTab(AssessmentResultsPanel.SMALLGRAPHPANEL);
        }

        private void bigGraphPageButton_Click(object sender, EventArgs e) {
            this.resultsPanel.ResultsTabControl.SelectTab(AssessmentResultsPanel.BIGGRAPHPANEL);
        }
    }
}
