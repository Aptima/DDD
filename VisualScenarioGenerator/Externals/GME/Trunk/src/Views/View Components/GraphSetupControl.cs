using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using AME.Controllers;
using log4net;
using AME.Views.View_Components.Chart;
using AME.MeasureModels;

namespace AME.Views.View_Components {
    public partial class GraphSetupControl : UserControl {

        private static readonly ILog logger = LogManager.GetLogger(typeof(GraphSetupControl));
        
        private AssessmentController assessmentController = null;
		private MeasuresController measuresController;
        private Dictionary<int, DataTable> tData = new Dictionary<int, DataTable>();
        private int simulationId = -1;
        private String[] tableColumn = null;
        private IChartData chartData = null;

        private GraphSetupControl() {

            AMEManager cm = AMEManager.Instance;
            this.assessmentController = (AssessmentController)cm.Get("AssessmentEditor");
			this.measuresController = (MeasuresController)AMEManager.Instance.Get("OptMeasuresEditor");

            InitializeComponent();
        }

        //public GraphSetupControl(int index, Dictionary<int, DataTable> tData) : this() {

        //    this.tData = tData;
        //    this.tableColumn = this.getDataColumnNames();
        //    this.typeToGraphCombo.Items.AddRange(this.tableColumn);
        //}

        public GraphSetupControl(int simulationId, int[] simRunId) : this() {

            this.simulationId = simulationId;

			DataTable dataTable = null;
            foreach (int id in simRunId) {
				dataTable = this.getRawDataTable(id);
				if (dataTable != null)
					this.tData.Add(id, dataTable);
            }

			if (this.tData.Count > 0) {
				this.tableColumn = this.getDataColumnNames();
				this.typeToGraphCombo.Items.AddRange(this.tableColumn);
			}
			else
				throw new Exception("Data for graphing is anavaliable");
        }

		public GraphSetupControl(int processId, int runId, MeasureInfo measure)
			: this() {

			this.simulationId = processId;

			DataTable dataTable = this.getMeasuredGraphDataTable(runId, measure);
			if (dataTable != null)
				this.tData.Add(runId, dataTable);

			if (this.tData.Count > 0) {
				this.tableColumn = this.getDataColumnNames();
				this.typeToGraphCombo.Items.AddRange(this.tableColumn);
			}
			else
				throw new Exception("Data for graphing is anavaliable");
		}
        
		public IChartData ChartData {
            get {
                return this.chartData;
            }
        }

        public Button AcceptButton {
            get {
                return this.okButton;
            }
        }

        public Button CancelButton {
            get {
                return this.cancelButton;
            }
        }

        private IChartData getGraphingData() {

            if (this.simRunAxisCheckBox.Checked == true) 
                return this.getChartDataSimRunAsX();
            else
                return this.getChartData();
        }

        private void clear() {
            this.graphNameTextBox.Clear();
            this.xAxisCombo.SelectedIndex = -1;
            this.yAxisCombo.SelectedIndex = -1;
            this.typeToGraphCombo.SelectedIndex = -1;
            this.itemsToGraphListBox.Items.Clear();
            this.simRunAxisCheckBox.Checked = false;

            this.chartData = null;
        }

        private DataTable getRawDataTable(int simRunId) {

            //Get data in DataTable format
            DataTable tempTable = new DataTable();
            bool isColumnBuilt = false;
            List<String> attrValues = null;

			XPathNavigator firstDataSet = null;
			try {
				firstDataSet = this.assessmentController.GetSimRunData(simRunId).CreateNavigator();

			}
			catch (NullReferenceException e) {
				String message = "Unable to find the raw data file for this Run ID " + simRunId;
				logger.Debug(e.Message + ": " + message);
				MessageBox.Show(message, "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				return null;
			} 
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

            return tempTable;
        }

		private DataTable getMeasuredGraphDataTable(int runId, MeasureInfo measure) {

			//Get data in DataTable format
			DataTable tempTable = new DataTable();
			bool isColumnBuilt = false;
			List<String> attrValues = null;

			XPathNavigator firstDataSet = null;
			try {
				firstDataSet = this.measuresController.getMeasuredData(runId).CreateNavigator();

			}
			catch (NullReferenceException e) {
				String message = "Unable to find the raw data file for this Run ID " + runId;
				logger.Debug(e.Message + ": " + message);
				MessageBox.Show(message, "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				return null;
			}

			XPathNavigator measureSet = firstDataSet.SelectSingleNode("/Measures/Measure[@name='" + measure.DisplayName +"']");
			XPathNodeIterator dataNodes = measureSet.Select("DataSet");
			foreach (XPathNavigator dataNav in dataNodes) {

				attrValues = new List<String>();
				XPathNodeIterator parameterNodes =
                    dataNav.SelectDescendants(XPathNodeType.Element, false);
				foreach (XPathNavigator parameter in parameterNodes) {

					if (parameter.Name.CompareTo("Data") != 0)
						continue;

					if (!isColumnBuilt) {
						String name = parameter.GetAttribute("name", parameter.NamespaceURI);

						if (!tempTable.Columns.Contains(name))
							tempTable.Columns.Add(name);
					}

					String value = parameter.GetAttribute("value", parameter.NamespaceURI);
					attrValues.Add(value);
				}
				isColumnBuilt = true;

				tempTable.Rows.Add(attrValues.ToArray());
			}

			return tempTable;
		}

        private LinearChartData getChartData() {

            String title = "";
            String typeToGraph = "";
            String xLabel = "";
            String yLabel = "";
            try {
                title = this.graphNameTextBox.Text;
                typeToGraph = this.typeToGraphCombo.SelectedItem.ToString();
                xLabel = this.xAxisCombo.SelectedItem.ToString();
                yLabel = this.yAxisCombo.SelectedItem.ToString();
            }
            catch (NullReferenceException) {
                logger.Debug("Some of the graphing parameters are NULL");
                return null;
            }

            List<double> x = null;
            List<double> y = null;
            List<LinearLayer> layers = new List<LinearLayer>();
            DataTable table = null;

            if (this.itemsToGraphListBox.CheckedItems.Count < 1) {
                MessageBox.Show("Please selecte Items To Graph", "Graphing Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("No Items To Graph were selected");
            }

            foreach (String checkedItemName in this.itemsToGraphListBox.CheckedItems) {

                foreach (int key in this.tData.Keys) {
                    
                    table = this.tData[key];
                    x = new List<double>();
                    y = new List<double>();

                    foreach (DataRow row in table.Rows) {

                        if (row[typeToGraph].ToString().CompareTo(checkedItemName) == 0) {

                            try {
                                x.Add(Double.Parse(row[xLabel].ToString()));
                                y.Add(Double.Parse(row[yLabel].ToString()));
                            }
                            catch (FormatException) {
                                String message = "Data you chose to graph is not numeric";
                                MessageBox.Show(message, "Graphing Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
    
                                //logger.Debug(message);
                                throw new FormatException(message);
                            }
                        }
                    }
                    String prefix = String.Empty;
                    if (this.tData.Count > 1)
                        prefix = "SimRun " + key + ": ";

                    layers.Add(new LinearLayer(x.ToArray(), y.ToArray(), prefix + checkedItemName));
                }
            }

            return new LinearChartData(title, xLabel, yLabel, typeToGraph, layers.ToArray());
        }

        private LinearChartData getChartDataSimRunAsX() {

            String title = "";
            String typeToGraph = "";
            String xLabel = "";
            String yLabel = "";
            try {
                title = this.graphNameTextBox.Text;
                typeToGraph = this.typeToGraphCombo.SelectedItem.ToString();
                xLabel = this.xAxisCombo.SelectedItem.ToString();
                yLabel = this.yAxisCombo.SelectedItem.ToString();
            }
            catch (NullReferenceException) {
                return null;
            }

            DataTable table = null;
            SortedDictionary<int, String[][]> allData = new SortedDictionary<int,String[][]>();
            List<String[]> dataPoints = null;
            String[] dataPoint = null;

            foreach (int key in this.tData.Keys) {

                table = this.tData[key];
                dataPoints = new List<String[]>();
                foreach (String checkedItemName in this.itemsToGraphListBox.CheckedItems) {

                    foreach (DataRow row in table.Rows) {

                        if (row[typeToGraph].ToString().CompareTo(checkedItemName) == 0) {

                            dataPoint = new String[3];
                            dataPoint[0] = checkedItemName + ": " + xLabel + " " + row[xLabel]; //CTL: Cycle 1 
                            dataPoint[1] = row[xLabel].ToString(); //1 - for now unused field
                            dataPoint[2] = row[yLabel].ToString(); //82
                            dataPoints.Add(dataPoint);
                        }
                    }
                }
                allData.Add(key, dataPoints.ToArray());
            }

            double[] xValues = new double[allData.Count];
            Dictionary<String, List<double>> yTemp = new Dictionary<string, List<double>>();
            int i = 0;
            foreach (int key in allData.Keys) {
                xValues[i++] = key;

                foreach (String[] array in allData[key]) {
                    if (!yTemp.ContainsKey(array[0]))
                        yTemp.Add(array[0], new List<double>());

                    try {
                        yTemp[array[0]].Add(Double.Parse(array[2]));
                    }
                    catch (FormatException) {    
                        throw new FormatException("Data you chose to graph is not numeric");
                    }
                }
            }

            LinearLayer[] layer = new LinearLayer[yTemp.Count];
            int j = 0;
            foreach (String key in yTemp.Keys) {
                layer[j++] = new LinearLayer(xValues, yTemp[key].ToArray(), key);
            }

            return new LinearChartData(title, "SimRun", yLabel, typeToGraph, layer);
        }

        private String[] getDataColumnNames() {

            List<String[]> columns = new List<String[]>();
            String[] names = null;
            foreach (DataTable table in this.tData.Values) {

                int i = 0;
                names = new String[table.Columns.Count];
                foreach (DataColumn c in table.Columns) {
                    names[i++] = c.Caption;
                }

                columns.Add(names);
            }

            return this.getAllCommonItems(columns);
        }

        /// <summary>
        /// Removes the duplicate items and empty Strings in each individual 
        /// String[] and then returns only the items that are present in EACH 
        /// of the Sting[] of the List.  Returned items are sorted.
        /// </summary>
        private String[] getAllCommonItems(List<String[]> list) {

            SortedList<String, int> bigList = new SortedList<string, int>();
            List<String> smallList = null;

            foreach (String[] item in list) {

                smallList = new List<string>();

                //build list of all unique elements of each String array by discarding duplicates
                foreach (String element in item) {
                    if (element.Trim().CompareTo(String.Empty) != 0 && !smallList.Contains(element))
                        smallList.Add(element);
                }

                //add them to the bigList and keep count of the number
                //of String arrays they are present at
                foreach (String str in smallList) {

                    if (!bigList.ContainsKey(str))
                        bigList.Add(str, 1); //add new items with count = 1
                    else
                        ++bigList[str]; //item exists, increment the count by 1 
                }
            }

            //if this value is not present in ALL String arrays - discard it
            List<String> keys = new List<string>(bigList.Keys);
            foreach (String key in keys) {
                if (bigList[key] != this.tData.Count)
                    bigList.Remove(key);
            }

            String[] finalArray = new String[bigList.Keys.Count];
            bigList.Keys.CopyTo(finalArray, 0);
            Array.Sort(finalArray);
            return finalArray;
        }

        private bool isDuplicateChartName() {
            int[] runId = new int[this.tData.Keys.Count];
            int counter = 0;
            foreach (int key in this.tData.Keys)
                runId[counter++] = key;

            if (runId.Length == 1) {
                XPathNavigator navigator = this.assessmentController.GetCustomGraphs(runId[0]).CreateNavigator();
                if (navigator.SelectSingleNode(String.Format(@"CustomGraphs/Graph[@name='{0}']", this.chartData.Title)) == null)
                    return false;
                else
                    return true;
            }
            else {
                XPathNavigator navigator = this.assessmentController.GetRunToRunGraphs(this.simulationId).CreateNavigator();
                if (navigator.SelectSingleNode(String.Format(@"RunToRunGraphs/Graph[@name='{0}']", this.chartData.Title)) == null)
                    return false;
                else
                    return true;
            }
        }

        private void typeToGraphCombo_SelectedIndexChanged(object sender, EventArgs e) {

            this.itemsToGraphListBox.Items.Clear();
            this.xAxisCombo.Items.Clear();
            this.yAxisCombo.Items.Clear();
            this.simRunAxisCheckBox.Checked = false;

            try {

                String selectedType = this.typeToGraphCombo.SelectedItem.ToString();
                List<String[]> bigList = new List<string[]>(this.tData.Count);
                List<String> tempList = null;
                foreach (DataTable table in this.tData.Values) {
                    tempList = new List<string>();
                    foreach (DataRow row in table.Rows) {
                        tempList.Add(row[selectedType].ToString());
                    }
                    bigList.Add(tempList.ToArray());
                }

                String[] typeArray = this.getAllCommonItems(bigList);
                this.itemsToGraphListBox.Items.AddRange(typeArray);

                this.xAxisCombo.Items.AddRange(this.tableColumn);
                this.xAxisCombo.Items.Remove(selectedType);
                this.yAxisCombo.Items.AddRange(this.tableColumn);
                this.yAxisCombo.Items.Remove(selectedType);
            }
            catch (NullReferenceException) {
                //this.typeToGraphCombo.SelectedItem == null
            }
            finally {
                this.xAxisCombo.Enabled = true;
                this.yAxisCombo.Enabled = true;
                this.itemsToGraphListBox.Enabled = true;

                if (this.tData.Count > 1) {
                    this.simRunAxisCheckBox.Enabled = true;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e) {
            try {
                this.chartData = this.getGraphingData();

                if (this.chartData.Title == null || this.chartData.Title.CompareTo(String.Empty) == 0) {
                    MessageBox.Show("Graph Name can not be blank", "Graph Setup Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.chartData = null;
                }
                else if (isDuplicateChartName()) {
                    MessageBox.Show("Graph Name '" + this.chartData.Title + "' already exists", 
                        "Graph Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.chartData = null;
                }

            }
            catch (Exception ex) {
                this.chartData = null;
                logger.Debug(ex.Message);
            }
        }

        private void clearButton_Click(object sender, EventArgs e) {
            this.clear();
        }
    }

    public class GraphSetupForm : Form {

        private GraphSetupControl graphSetup = null;

        public GraphSetupForm(int simulationId, int[] simRunId)
            : base() {

			try {
				this.graphSetup = new GraphSetupControl(simulationId, simRunId);
			}
			catch (Exception e) {
				throw e;
			}

			this.Text = "Graph Setup";
			//this.Icon = Form.

            this.graphSetup.Dock = DockStyle.Fill;
            this.graphSetup.AcceptButton.Click += new EventHandler(AcceptButton_Click);

            int width = graphSetup.Size.Width + 10;
            int height = graphSetup.Size.Height + 30;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(width, height);
            this.MaximumSize = new Size(width, height);
            this.MinimumSize = new Size(width, height);

            this.AcceptButton = graphSetup.AcceptButton;
            this.CancelButton = graphSetup.CancelButton;
            this.Controls.Add(graphSetup);

        }

		public GraphSetupForm(int simulationId, int simRunId, MeasureInfo measure)
			: base() {

			try {
				this.graphSetup = new GraphSetupControl(simulationId, simRunId, measure);
			}
			catch (Exception e) {
				throw e;
			}

			this.Text = "Graph Setup";
			//this.Icon = Form.

			this.graphSetup.Dock = DockStyle.Fill;
			this.graphSetup.AcceptButton.Click += new EventHandler(AcceptButton_Click);

			int width = graphSetup.Size.Width + 10;
			int height = graphSetup.Size.Height + 30;

			this.StartPosition = FormStartPosition.CenterScreen;
			this.Size = new Size(width, height);
			this.MaximumSize = new Size(width, height);
			this.MinimumSize = new Size(width, height);

			this.AcceptButton = graphSetup.AcceptButton;
			this.CancelButton = graphSetup.CancelButton;
			this.Controls.Add(graphSetup);
		}

        private void AcceptButton_Click(object sender, EventArgs e) {
            if (this.graphSetup.ChartData != null) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public IChartData ChartData {
            get {
                return this.graphSetup.ChartData;
            }
        }
    }

    //public class ActivatePanelEvent : EventArgs {
    //    private bool activate = false;

    //    public ActivatePanelEvent(bool activate) {
    //        this.activate = activate;
    //    }

    //    public bool isActive() {
    //        return this.activate;
    //    }
    //}

    //public class ChartViewData {

    //    private string title = "";
    //    private string xAxisLabel = "";
    //    private string yAxisLabel = "";
    //    private double[] xValues = { };
    //    private Dictionary<string, double[]> yValues = new Dictionary<string, double[]>();

    //    public ChartViewData(String title, String xAxisLabel, double[] xValues,
    //        String yAxisLabel, Dictionary<string, double[]> yValues) {

    //        this.title = title;
    //        this.xAxisLabel = xAxisLabel;
    //        this.xValues = xValues;
    //        this.yAxisLabel = yAxisLabel;
    //        this.yValues = yValues;
    //    }

    //    public String Title {
    //        get {
    //            return this.title;
    //        }
    //    }

    //    public String XAxisLabel {
    //        get {
    //            return this.xAxisLabel;
    //        }
    //    }

    //    public String YAxisLabel {
    //        get {
    //            return this.yAxisLabel;
    //        }
    //    }

    //    public double[] XValues {
    //        get {
    //            return this.xValues;
    //        }
    //    }

    //    public Dictionary<string, double[]> YValues {
    //        get {
    //            return this.yValues;
    //        }
    //    }
    //}
}
