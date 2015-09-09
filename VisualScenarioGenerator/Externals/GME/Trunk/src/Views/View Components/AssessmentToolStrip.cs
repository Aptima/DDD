using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using log4net;
using AME.Controllers;
using User_Controls;
using AME.EngineModels;
using AME.Views.User_Controls;
using AME.Views.View_Component_Packages;

namespace AME.Views.View_Components {

    
    public partial class AssessmentToolStrip : ToolStrip {

        private static readonly ILog logger = LogManager.GetLogger(typeof(AssessmentToolStrip));
        private Controller assessmentController = null;

        private Panel contentPanel = null;
        private Control processControl = null;
        private NavigatorGrid navigatorGrid = null;
        private AssessemntResults assessmentResults = null;
        private String config = null;
        private String measureConfig = null;

        private AssessmentToolStrip() {
            InitializeComponent();
        }

        public AssessmentToolStrip(ProcessType type) : this() {

            AMEManager cm = AMEManager.Instance;
            Controller projectController = (Controller)cm.Get("ProjectEditor");

            String typeString = null;
            switch (type) {
                case (ProcessType.SIMULATION):
                    this.config = "AssessmentEditor";
                    this.measureConfig = "SimMeasuresEditor";
                    this.assessmentController = (AssessmentController)cm.Get(this.config);
                    typeString = " for Simulation";
                    this.processButton.ToolTipText = "Simulation";
                    this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                        this.processComboBox,
                        this.toolStripSeparator,
                        this.graphButton,
                        this.processButton});
                    break;

                case (ProcessType.OPTIMIZATION):
                    this.config = "OptAssessmentEditor";
                    this.measureConfig = "OptMeasuresEditor";
					this.assessmentController = (AssessmentController)cm.Get(this.config);
                    typeString = " for Optimization";
                    this.processButton.ToolTipText = "Optimization";
                    this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                        this.processComboBox,
                        this.toolStripSeparator,
                        this.graphButton,
                        this.applyResultsButton,
						this.processButton});
                    break;

                default:
                    throw new Exception("Attempt to create an unkown type of the AssessmentToolstrip");
            }

            this.assessmentController.RegisterForUpdate(processComboBox);

            // Init Simulation Selection CustomCombo
            this.processComboBox.Controller = projectController;
            this.processComboBox.Type = assessmentController.RootComponentType;
            this.processComboBox.LinkType = projectController.RootComponentType;

            logger.Debug("Created AssessmentToolStrip" + typeString);
        }

        /*
        public AssessmentToolStrip(NavigatorGrid navigatorGrid, AssessemntResults assessmentResults)
            : this() {
            this.navigatorGrid = navigatorGrid;
            this.assessmentResults = assessmentResults;
        }
        */

        public Panel ContentPanel
        {
            set
            {
                this.contentPanel = value;
            }
        }

        public Control ProcessControl
        {
            set {
                this.processControl = value;
            }
        }

        public NavigatorGrid NavigatorGrid {
            set {
                this.navigatorGrid = value;
            }
        }

        public AssessemntResults AssessmentResultsPanel {
            get
            {
                return this.assessmentResults;
            }
            set 
            {
                this.assessmentResults = value;
            }
        }

        public CustomCombo ProcessCustomCombo {
            get {
                return this.processComboBox.CustomComboControl;
            }
		}

		public void enableGraphing(bool enable) {
			if (enable)
				this.Items.Insert(2, this.graphButton);
			else
				this.Items.Remove(this.graphButton);
		}

		#region Private Methods

		/// <summary>
		/// Used to graph raw data.  Produces either a Costom Graph or a Run-to-Run Graph
		/// </summary>
        private void graphButton_Click(object sender, EventArgs e) {

            if (this.navigatorGrid == null || this.assessmentResults == null) {
                logger.Warn("Either NavigatorGrid or AssessmentResultsPanel is NULL");
            }

            int[] selectedRun = this.navigatorGrid.getSelectedRunIds();
            try {
				GraphSetupForm form = new GraphSetupForm(this.navigatorGrid.ProcessId, selectedRun);
				form.StartPosition = FormStartPosition.CenterParent;
				DialogResult res = form.ShowDialog(this);
				if (res == DialogResult.OK) {
					if (selectedRun.Length == 1)
						this.assessmentResults.addCustomGraph(form.ChartData);
					else
						this.assessmentResults.addRunToRunGraph(form.ChartData, selectedRun);
				}
			}
			catch (Exception ex) {
				logger.Debug(ex.Message);
			}
        }

		/// <summary>
		/// Used to graph measured data.  Produces a Measured Graph
		/// </summary>
		private void graphMeasuresButton_Click(object sender, EventArgs e) {

			if (this.navigatorGrid == null || this.assessmentResults == null) {
				logger.Warn("Either NavigatorGrid or AssessmentResultsPanel is NULL");
			}

			int[] selectedRun = this.navigatorGrid.getSelectedRunIds();

			if (selectedRun.Length > 1) {

				MessageBox.Show("Select a single Run in the Navigator", "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try {

				MeasureToGraphSelector measureSelector = new MeasureToGraphSelector(this.measureConfig, this.navigatorGrid.ProcessId, selectedRun[0]);
				DialogResult res = measureSelector.ShowDialog(this);
				if (res == DialogResult.OK) {
					if (("ChartType.LINEAR").Equals(measureSelector.Measure.GraphType)) {

						GraphSetupForm form = new GraphSetupForm(this.navigatorGrid.ProcessId, selectedRun[0], measureSelector.Measure);
						form.StartPosition = FormStartPosition.CenterParent;
						res = form.ShowDialog(this);
						if (res == DialogResult.OK) {
							this.assessmentResults.addMeasureGraph(form.ChartData);
						}
					}
					else {
						this.assessmentResults.addMeasureGraph(measureSelector.ChartData);
					}
				}
			}
			catch (Exception ex) {
				logger.Debug(ex.Message);
			}
		}

        private void processButton_Click(object sender, EventArgs e) {
            //UserControl uc = (UserControl)this.assessmentForm;
            if (this.contentPanel != null && this.processControl != null)
            {
                //uc.SuspendLayout();
                processControl.Hide();
                processControl.SuspendLayout();

                if (this.contentPanel.Controls.Contains(this.processControl))
                {
                    this.contentPanel.Controls.Remove(this.processControl);
                }
                else
                {
                    this.processControl.Dock = DockStyle.Left;
                    this.contentPanel.Controls.Add(this.processControl);
                    this.contentPanel.BringToFront();
                }
                //uc.ResumeLayout();
                processControl.ResumeLayout();
                processControl.Show();

                if (processControl is IViewComponentPanel)
                {
                    ((IViewComponentPanel)processControl).UpdateViewComponent();
                }
            }
        }

        private void applyResultsButton_Click(object sender, EventArgs e) {
            this.navigatorGrid.applyResults();
        }

        private void processComboBox_SelectedIDChangedEvent(CustomCombo source, int ID, string Name)
        {
            if (this.navigatorGrid == null || this.assessmentResults == null) 
            {
                logger.Warn("NavigatorGrid is NULL");
            }
            
            this.navigatorGrid.loadData(ID);
		}

		#endregion
	}
}
