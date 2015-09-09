using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Model;
using Controllers;
using System.Xml.XPath;
using GME.Views.Forms;
using View_Components;
using log4net;

namespace GME.Views.View_Components {

    public partial class AssessmentResultsPanel : UserControl {

        private static readonly ILog logger = LogManager.GetLogger(typeof(AssessmentResultsPanel));

        public const int RAWDATAPANEL = 0;
        public const int SMALLGRAPHPANEL = 1;
        public const int BIGGRAPHPANEL = 2;
        private int lowestTabSelected = 0; //index of the left-most tab selectable tab
        private int highestTabSelected = 0; //index of the right-most tab ever selected

        private AssessmentController assessmentController = null;
        private Dictionary<int, DataTable> tData = new Dictionary<int, DataTable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentResultsPanel"/> class.
        /// By default the ToolBar is not displayed
        /// </summary>
        public AssessmentResultsPanel() : this(false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentResultsPanel"/> class.
        /// </summary>
        /// <param name="showToolbar">if set to <c>true</c> [show toolbar].</param>
        public AssessmentResultsPanel(bool showToolbar) {
            InitializeComponent();

            GMEManager cm = GMEManager.Instance;
            this.assessmentController = (AssessmentController)cm.Get("AssessmentEditor");

            if (showToolbar)
                this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            
            this.graphDataButton.Enabled = false;
            this.backButton.Enabled = false;
            this.forwardButton.Enabled = false;

            logger.Debug("AssessmentResultPanel created");
        }

        public TabControl ResultsTabControl {
            get {
                return this.resultsTabControl;
            }
        }

        public int LowestTabSelected {
            get {
                return this.lowestTabSelected;
            }
        }

        public int HighestTabSelected {
            get {
                return this.highestTabSelected;
            }
        }

        public void loadData(int[] runId, int displayFormat) {

            Dictionary<int, IXPathNavigable> data = new Dictionary<int, IXPathNavigable>();
            if (runId != null && runId.Length > 0) {

                for (int i = 0; i < runId.Length; i++) {
                    data.Add(runId[i], this.assessmentController.GetSimRunData(runId[i]));
                }
                this.loadTableData(data, displayFormat);
            }
        }

        public void clear() {
            this.tData.Clear();
            this.flowLayoutPanel.Controls.Clear();
            this.expandedGraphTab.Controls.Clear();
            this.resultsTabControl.SelectTab(RAWDATAPANEL);
            this.lowestTabSelected = 0;
            this.highestTabSelected = 0;
            this.graphDataButton.Enabled = false;
            this.backButton.Enabled = false;
            this.forwardButton.Enabled = false;
        }

        private void loadTableData(Dictionary<int, IXPathNavigable> xmlData, int displayFormat) {
            if (xmlData == null)
                return; //no data, dont try to populate the form

            //Clean up all panels
            this.flowLayoutPanel.Controls.Clear();
            this.expandedGraphTab.Controls.Clear();

            DataTable tempTable = null;

            foreach (int key in xmlData.Keys) {

                tempTable = new DataTable();

                //Get data in DataTable format
                bool isColumnBuilt = false;
                List<String> attrValues = null;

                XPathNavigator firstDataSet = xmlData[key].CreateNavigator();
                XPathNodeIterator dataNodes = firstDataSet.Select("//DataSet");
                foreach (XPathNavigator dataNav in dataNodes) {

                    attrValues = new List<String>();
                    XPathNodeIterator parameterNodes =
                        dataNav.SelectDescendants(XPathNodeType.Element, false);
                    foreach (XPathNavigator parameter in parameterNodes) {

                        if (parameter.Name.CompareTo("Data") != 0)
                            continue;

                        if (!isColumnBuilt) {
                            String name = parameter.GetAttribute("Name", parameter.NamespaceURI);

                            if (!tempTable.Columns.Contains(name))
                                tempTable.Columns.Add(name);
                        }

                        String value = parameter.GetAttribute("Value", parameter.NamespaceURI);
                        attrValues.Add(value);
                    }
                    isColumnBuilt = true;

                    tempTable.Rows.Add(attrValues.ToArray());
                }

                this.tData.Add(key, tempTable);
            }

            if (displayFormat == AssessmentToolbar.SHOWTABLE) {
                if (this.tData.Count == 1) {
                    foreach (DataTable t in this.tData.Values)
                        this.runRawData.DataSource = t; //there will be only one

                    this.graphDataButton.Enabled = true;
                    this.lowestTabSelected = RAWDATAPANEL;
                }
                else {
                    String message = "Attempt to display data in the table form for multiple SimRuns";
                    MessageBox.Show(message, "Aptima Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Error(message);
                }
            }
            else if (displayFormat == AssessmentToolbar.SHOWGRAPH) {
                AssessmentGraphSetupForm form = new AssessmentGraphSetupForm(this.tData);
                form.GraphViewDataCallback =
                    new GME.Views.Forms.AssessmentGraphSetupForm.GraphViewDataDelegate(this.GraphViewDataCallbackFn);
                form.Show();

                if (form.AcceptButton.DialogResult == DialogResult.OK) {
                    if (this.tData.Count == 1) {
                        foreach (DataTable t in this.tData.Values)
                            this.runRawData.DataSource = t; //there will be only one

                        this.backButton.Enabled = true;
                        this.lowestTabSelected = RAWDATAPANEL;
                    }
                    else {
                        this.runRawData.DataSource = null;
                        this.backButton.Enabled = false;
                        this.lowestTabSelected = SMALLGRAPHPANEL;
                    }

                    this.resultsTabControl.SelectTab(SMALLGRAPHPANEL);
                }
            }
            else {
                logger.Error("'displayFormat' parameter of AssessmentResultsPanel.loadTableData(...) " +
                    "method is neither AssessmentToolbar.SHOWTABLE nor AssessmentToolbar.SHOWTABLE");
            }
        }


        private void loadGraphData(ChartViewData[] data) {

            int width = (this.flowLayoutPanel.Size.Width / 2) - 20;
            int height = (3 * width) / 4;

            ChartView view = null;
            for (int i = 0; i < data.Length; i++) {
                view = new ChartView();
                try {
                    view.Title = data[i].Title;
                    view.XAxisLabel = data[i].XAxisLabel;
                    view.XAxisValues = data[i].XValues;
                    view.YAxisLabel = data[i].YAxisLabel;
                    view.YAxisDataDict = data[i].YValues;
                }
                catch (NullReferenceException) {
                    continue;
                }
                view.Size = new Size(width, height);
                view.MouseClick += new System.Windows.Forms.MouseEventHandler(this.smallGraph_MouseClick);
                view.MouseEnter += new System.EventHandler(this.view_MouseEnter);
                view.MouseLeave += new System.EventHandler(this.view_MouseLeave);

                view.drawGraph(false);
                this.flowLayoutPanel.Controls.Add(view);

            }
        }

        private void GraphViewDataCallbackFn(ChartViewData[] data) {
            this.flowLayoutPanel.Controls.Clear();
            this.expandedGraphTab.Controls.Clear();
            this.loadGraphData(data);
            this.resultsTabControl.SelectTab(SMALLGRAPHPANEL);
        }

        private void tabControl_Selected(object sender, EventArgs e) {
            int selectedTabIndex = ((TabControlEventArgs)e).TabPageIndex;
            if (this.highestTabSelected < selectedTabIndex) {
                this.highestTabSelected = selectedTabIndex;
                this.forwardButton.Enabled = false;
            }
            else if (this.highestTabSelected == selectedTabIndex) {
                this.forwardButton.Enabled = false;
            }
            else { //this.highestTabSelected > selectedTabIndex
                this.forwardButton.Enabled = true;
            }

            if (selectedTabIndex > this.lowestTabSelected)
                this.backButton.Enabled = true;
            else
                this.backButton.Enabled = false;
        }

        private void smallGraph_MouseClick(object sender, EventArgs e) {
            ChartView view = ((ChartView)sender).clone();
            this.expandedGraphTab.Controls.Clear();
            this.expandedGraphTab.Controls.Add(view);
            view.Dock = System.Windows.Forms.DockStyle.Fill;
            view.drawGraph(true);
            view.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bigGraph_MouseClick);
            view.MouseEnter += new EventHandler(this.view_MouseEnter);
            view.MouseLeave += new EventHandler(this.view_MouseLeave);

            this.resultsTabControl.SelectTab(BIGGRAPHPANEL);
        }

        private void bigGraph_MouseClick(object sender, EventArgs e) {
            this.resultsTabControl.SelectTab(SMALLGRAPHPANEL);
        }

        private void view_MouseEnter(object sender, EventArgs e) {
            ((ChartView)sender).Cursor = Cursors.Hand;
        }

        private void view_MouseLeave(object sender, EventArgs e) {
            ((ChartView)sender).Cursor = Cursors.Default;
        }

        private void graphDataButton_Click(object sender, EventArgs e) {
            
            AssessmentGraphSetupForm form = new AssessmentGraphSetupForm(this.tData);
            
            form.GraphViewDataCallback =
                new GME.Views.Forms.AssessmentGraphSetupForm.GraphViewDataDelegate(this.GraphViewDataCallbackFn);
            form.Show();
        }

        private void backButton_Click(object sender, EventArgs e) {
            this.resultsTabControl.SelectTab(this.resultsTabControl.SelectedIndex - 1);
        }

        private void forwardButton_Click(object sender, EventArgs e) {
            this.resultsTabControl.SelectTab(this.resultsTabControl.SelectedIndex + 1);
        }

        public static void Main() {
            GMEManager.Instance.CreateModel("localhost", 3306, "root", "devpass", "most");
            GMEManager.Instance.ConfigurationPath = @"C:\dev\A-Model\Projects\IO-AOC\Trunk\src\Config";
            GMEManager.Instance.CreateControllers();

            Form form = new Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.ClientSize = new System.Drawing.Size(668, 458);

            //AssessmentResultsPanel assessmentResultsPanel = new AssessmentResultsPanel(ass, 1059);
            AssessmentResultsPanel assessmentResultsPanel = new AssessmentResultsPanel();
            assessmentResultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(assessmentResultsPanel);
            Application.Run(form);
            //Application.Run(new ChartForm(ass, 1059));
        }
    }
}
