using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;
using AME.Controllers;
using System.Xml.XPath;
using AME.Views.View_Components;
using AME.EngineModels;

namespace AME.Views.View_Components {
    public partial class NavigatorGrid : UserControl, IViewComponent {

        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(NavigatorGrid));

        private AssessmentController assessmentController = null;
        private ProcessType processType;
		private Dictionary<String, ProcessApplicationAdapter> processApplicationAdapters = new Dictionary<string, ProcessApplicationAdapter>();
		private String processName = null;
		private int processId = -1;
        private int selectedRow = -1;

        public delegate void NavigatorRowChangeHandler(object sender, NavigatorRowChangeArgs ev);
        public event NavigatorRowChangeHandler NavigatorRowChanged;
        
        private NavigatorGrid() 
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();

            AMEManager cm = AMEManager.Instance;
            Controller projectController = (Controller)cm.Get("ProjectEditor");
            this.assessmentController = (AssessmentController)cm.Get("AssessmentEditor");
        }

		public NavigatorGrid(ProcessType processType) : this() {
            this.processType = processType;
        }

        public int ProcessId {
            get {
                return this.processId;
            }
        }

        #region IViewComponent Members

        public Controllers.IController Controller {
            get {
                return this.assessmentController;
            }
            set {
                if (value is AssessmentController)
                    this.assessmentController = (AssessmentController)value;
            }
        }

        public void UpdateViewComponent() {
            this.loadData(this.processId); //reload the grid
        }

        #endregion

		public void addProcessApplicationAdapter(String name, ProcessApplicationAdapter adapter) {
			this.processApplicationAdapters.Add(name, adapter);
		}
		
		public int[] getSelectedRunIds() {
            int[] runId = new int[this.dataGridView.SelectedRows.Count];
            DataGridViewRow row = null;
            for (int i = 0; i < this.dataGridView.SelectedRows.Count; i++) {
                row = this.dataGridView.SelectedRows[i];
                try {
                    runId[i] = Int32.Parse(row.Cells["ID"].FormattedValue.ToString());
                }
                catch (FormatException) {
                    logger.Warn("Run ID in the Navigator row " + row.Index + " is not an integer");
                    return new int[] { }; //error -> return an empty array
                }
                catch (ArgumentException) {
                    logger.Warn("Navigator grid does not have an \"ID\" column");
                    return new int[] { }; //error -> return an empty array
                }
            }
            return runId;
        }

        public void loadData(int processId) {

			this.processId = processId;

			XPathNavigator navigator = assessmentController.GetComponent(this.processId).CreateNavigator();
			XPathNodeIterator iterator = navigator.Select("Components/Component");
			if (iterator.MoveNext())
				this.processName = iterator.Current.GetAttribute("Name", navigator.NamespaceURI);
			else
				this.processName = null;

			navigator = assessmentController.GetAllProcessRuns(this.processId, processType).CreateNavigator();

            DrawingUtility.SuspendDrawing(this);
            this.DataBindings.Clear();
            this.Invalidate();

            DataTable processData = new DataTable();
            processData.Columns.Add("ID", typeof(Int32));
            processData.Columns.Add("Run Name", typeof(String));
            // build other table columns
            XPathNodeIterator parameterNodes = navigator.Select("//Parameter");
            foreach (XPathNavigator paramNav in parameterNodes) {
                paramNav.MoveToFirstAttribute();

                if (!processData.Columns.Contains(paramNav.Value)) {
                    processData.Columns.Add(paramNav.Value);
                }
            }

            // populate master table rows
            DataRow dr;
            XPathNodeIterator runNodes;
            switch (this.processType) {
                case ProcessType.OPTIMIZATION:
                    runNodes = navigator.Select("//OptRun");
                    break;
                case ProcessType.SIMULATION:
                    runNodes = navigator.Select("//SimRun");
                    break;
                default:
                    throw new Exception("The Process Type is not defined in the ProcessControllType enum");
            }
            
            foreach (XPathNavigator runNav in runNodes) {
                dr = processData.NewRow();

                runNav.MoveToFirstAttribute();
                dr[0] = Int32.Parse(runNav.Value);


                runNav.MoveToNextAttribute();
                dr[1] = runNav.Value;

                runNav.MoveToParent();
                parameterNodes = runNav.SelectChildren("Parameter", runNav.NamespaceURI);
                foreach (XPathNavigator paramNav in parameterNodes) {
                    paramNav.MoveToFirstAttribute();
                    string paramName = paramNav.Value;

                    paramNav.MoveToNextAttribute();
                    string paramValue = paramNav.Value;

                    dr[paramName] = paramValue;
                }
                processData.Rows.Add(dr);
            }
            this.dataGridView.DataSource = processData;
            DrawingUtility.ResumeDrawing(this);
        }

        public void applyResults() {

			if (this.processApplicationAdapters.Count == 0)
				return; //no adapters

			if (this.processName != null) {
				ProcessApplicationAdapter pAdapter = this.processApplicationAdapters[this.processName];
				int[] runId = this.getSelectedRunIds();

				if (pAdapter != null && runId.Length > 0) {
					pAdapter.applyProcess(runId[0]);
				}
			}
        }
      
        private void rowSelectionChanged(object sender, EventArgs e) {

            if (this.dataGridView.SelectedRows.Count != 1 ||
                this.dataGridView.SelectedRows[0].Index == this.selectedRow) {
                return; //multiple selection -> don't fire the NavigatorRowChange Event
            }
            else {
                this.selectedRow = this.dataGridView.SelectedRows[0].Index;
            }
            int[] runId = this.getSelectedRunIds();
            if (runId != null && runId.Length > 0)
                this.OnNavigatorRowChanged(new NavigatorRowChangeArgs(runId[0]));
        }

        protected virtual void OnNavigatorRowChanged(NavigatorRowChangeArgs e) {
            try {
                NavigatorRowChanged(this, e);
            }
            catch (NullReferenceException) {
                //continue, the grid hasn't been created yet
            }
        }
    }

    public class NavigatorRowChangeArgs : EventArgs {
        private int runId;

        public NavigatorRowChangeArgs(int runId) {
            this.runId = runId;
        }

        public int SimRunId {
            get {
                return this.runId;
            }
        }
    }
}
