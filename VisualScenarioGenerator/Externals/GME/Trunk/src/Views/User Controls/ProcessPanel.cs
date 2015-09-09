using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using AME;
using AME.Views.View_Components;
using AME.EngineModels;
using AME.Views.View_Component_Packages;

namespace User_Controls {

    public abstract partial class ProcessPanel : UserControl, IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        protected ModelingController modelingController;
		protected MeasuresController measuresController;
        private RootController projectController = (RootController)AMEManager.Instance.Get("ProjectEditor");
        private Int32 m_CurrentProjectID;
        private String processName = String.Empty;
        private List<CustomCombo> customComboList;
        protected Boolean calculateMeasures = true; // Running measures should be optional

		public delegate void NewRunCreateHandler(object sender, EventArgs ev);
		public event NewRunCreateHandler NewRunCreated;

        private ProcessPanel() {

            myHelper = new ViewPanelHelper(this);

            InitializeComponent();
        }

        public ProcessPanel(ProcessType type) : this() {

            //projectController = (RootController)GMEManager.Instance.Get("ProjectEditor");


            String typeStr = String.Empty;
            switch (type) {

                case ProcessType.OPTIMIZATION:
					modelingController = (ModelingController)AMEManager.Instance.Get("OptimizationEditor");
					measuresController = (MeasuresController)AMEManager.Instance.Get("OptMeasuresEditor");
                    typeStr = "Optimization";
                    break;

                case ProcessType.SIMULATION:
                    modelingController = (ModelingController)AMEManager.Instance.Get("SimulationEditor");
                    measuresController = (MeasuresController)AMEManager.Instance.Get("SimMeasuresEditor");
                    typeStr = "Simulation";
                    break;

                default:
                    throw new Exception("The Process Type is not defined in the ProcessControllType enum");
            }
            if (modelingController == null)
                return;

            this.topTabPage.Description = "Select " + typeStr + " to run";
            this.topTabPage.Text = typeStr;
            this.bottomTabPage.Description = "Configure and Run the " + typeStr + "Run";
            this.bottomTabPage.Text = typeStr + "Run";

            processCustomCombo.Controller = projectController;
            processCustomCombo.Type = modelingController.RootComponentType;
            processCustomCombo.LinkType = projectController.RootComponentType;

            parameterTable.Controller = modelingController;

            modelingController.RegisterForUpdate(this);
            modelingController.RegisterForUpdate(processCustomCombo);
            modelingController.RegisterForUpdate(parameterTable);

            customComboList = new List<CustomCombo>();
        }

        public Boolean CalculateMeasures
        {
            get
            { 
                return calculateMeasures;
            }
            set
            { 
                calculateMeasures = value;
            }
        }

        public int CurrentProjectID {
            get {
                return m_CurrentProjectID;
            }
            set {
                m_CurrentProjectID = value;
            }
        }

        // Override
        public void UpdateViewComponent() 
        {
            processCustomCombo.DisplayID = CurrentProjectID;
            processCustomCombo.UpdateViewComponent();
        }
        // END

        #region Protected Members

		protected virtual void OnNewRunCreated(EventArgs e) {
			try {
				NewRunCreated(this, e);
			}
			catch (NullReferenceException) {
				//continue, the grid hasn't been created yet
			}
		}

        protected List<CustomCombo> InputsComboList {
            get {
                return this.customComboList;
            }
        }

        protected CustomCombo ProcessCustomCombo {
            get {
                return this.processCustomCombo;
            }
        }

        /// <summary>
        /// This is the name of the process that is selected in the processCustomCombo
        /// </summary>
        /// <value>The name of the process.</value>
        protected String ProcessName {
            get {
                return this.processName;
            }
        }

        protected virtual void createRunInputs() {
            if (processCustomCombo.SelectedItem != null) {
                inputsFlowLayoutPanel.Controls.Clear();
                customComboList.Clear();

                int selectedItemId = processCustomCombo.SelectedID;
                Dictionary<String, String> inputs = modelingController.GetInputNames(selectedItemId, true);

                inputsFlowLayoutPanel.SuspendLayout();
                foreach (String input in inputs.Keys) {
                    Label label = new Label();
                    //label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                    label.AutoSize = true;
                    label.Margin = new System.Windows.Forms.Padding(3);
                    label.Text = input + ":";
                    label.Size = new System.Drawing.Size(229, 21);
                    //label.Dock = DockStyle.Fill;
                    this.inputsFlowLayoutPanel.Controls.Add(label);

                    CustomCombo customCombo = new CustomCombo();
                    //customCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));                 
                    customCombo.Controller = projectController;
                    customCombo.LinkType = inputs[input];
                    customCombo.Type = input;
                    customCombo.DisplayID = CurrentProjectID;
                    customCombo.UpdateViewComponent();
                    customCombo.Size = new System.Drawing.Size(229, 21);
                    //customCombo.Dock = DockStyle.Fill;
                    this.inputsFlowLayoutPanel.Controls.Add(customCombo);

                    customComboList.Add(customCombo);
                }
                inputsFlowLayoutPanel.ResumeLayout();
            }
        }
       
        #endregion

        #region Abstract Methods
        
        protected abstract void runButtonClick();
        
        #endregion

        #region Private Methods

        private void CustomComboControl_SelectedIDChangedEvent(CustomCombo source, int ID, string Name)
        {
            
            parameterTable.SelectedID = ID;
            this.processName = Name;
            parameterTable.UpdateViewComponent();
            createRunInputs();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.runButtonClick();
        }


        private void resetSimRunInputs() {
            foreach (CustomCombo customCombo in customComboList) {
                if (customCombo.SelectedItem != null) {
                    customCombo.Text = String.Empty;
                }
            }
        }

        #endregion
    }
}
