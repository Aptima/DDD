using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using AME.MeasureModels;
using System.Xml.XPath;
using log4net;
using AME.Views.View_Components.Chart;

namespace AME.Views.View_Components {
	
	public partial class MeasureToGraphSelector : Form {

		private static readonly ILog logger = LogManager.GetLogger(typeof(MeasureToGraphSelector));

		private MeasuresController measuresController = null;
		private MeasureInfo selectedMeasure = null;
		private List<MeasureInfo> measures = null;
		private IChartData chartData = null;
		private int selectedItem = -1;
		
		private int processId = -1;
		private int runId = -1;
        private String measureConfig = null;
	
		public MeasureToGraphSelector(string measureConfig, int processId, int runId) {

            this.measureConfig = measureConfig;
			this.processId = processId;
			this.runId = runId;

            measuresController = (MeasuresController)AMEManager.Instance.Get(this.measureConfig);

			InitializeComponent();

			this.AcceptButton = this.okButton; ;
			this.CancelButton = this.cancelButton; ;

			this.loadData();
		}

		public MeasureInfo Measure {
			get {
				return this.selectedMeasure;
			}
		}

		public IChartData ChartData {
			get {
				return this.chartData;
			}
		}

		private void loadData() {

			if (this.processId == -1 || this.runId == -1)
				return;

			this.measures = measuresController.getAllGraphableMeasures(runId);
			if (this.measures.Count > 0) {
				foreach (MeasureInfo measure in this.measures)
					this.measureListBox.Items.Add(measure.DisplayName);

			}
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
			if (measureSet == null)
				return null;
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

		private void buildMultiBarChart(int runId, MeasureInfo measure) {

			DataTable chartTable = this.getMeasuredGraphDataTable(runId, measure);
			if (chartTable == null) {
				String message = "There is no data for the measure: " + measure.DisplayName;
				logger.Debug(message + " for Run " + runId);
				MessageBox.Show(message, "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			String[] labels = new String[chartTable.Rows.Count];
			BarLayer[] barLayers = new BarLayer[chartTable.Columns.Count - 1];
			
			DataRow row = null;
			double[] data = null;
			for (int j = 0; j < chartTable.Columns.Count; j++) {
				
				data = new double[labels.Length];

				for (int i = 0; i < chartTable.Rows.Count; i++) {

					row = chartTable.Rows[i];

					if (j == 0) {
						labels[i] = row[j].ToString();
					}
					else {
						data[i] = Double.Parse(row[j].ToString());
					}
				}

				if (j != 0) {

					barLayers[j - 1] = new BarLayer(data, chartTable.Columns[j].ColumnName);
				}
			}

			this.chartData = new MultiBarChartData(measure.DisplayName, barLayers, labels);
		}

		private void buildBarChart(int runId, MeasureInfo measure) {
			
			DataTable chartTable = this.getMeasuredGraphDataTable(runId, measure);
			if (chartTable == null) {
				String message = "There is no data for the measure: " + measure.DisplayName;
				logger.Debug(message + " for Run " + runId);
				MessageBox.Show(message, "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			String[] labels = new String[chartTable.Rows.Count];
			double[] data = new double[labels.Length];

			DataRow row = null;
			for (int j = 0; j < chartTable.Columns.Count; j++) {

				for (int i = 0; i < chartTable.Rows.Count; i++) {

					row = chartTable.Rows[i];

					if (j == 0) {
						labels[i] = row[j].ToString();
					}
					else {
						data[i] = Double.Parse(row[j].ToString());
					}
				}
			}

			this.chartData = new BarChartData(measure.DisplayName, data, labels, null);
		}

		private void okButton_Click(object sender, EventArgs e) {

			if (this.measureListBox.CheckedIndices.Count > 0) {
				int checkedItem = this.measureListBox.CheckedIndices[0];
				this.selectedMeasure = this.measures[checkedItem];

				if (("ChartType.MULTIBAR").Equals(this.selectedMeasure.GraphType)) {
					this.buildMultiBarChart(runId, this.selectedMeasure);
				}
				else if (("ChartType.BAR").Equals(this.selectedMeasure.GraphType)) {
					this.buildBarChart(runId, this.selectedMeasure);
				}

				this.DialogResult = DialogResult.OK;
			}
			else {
				this.DialogResult = DialogResult.Cancel;
			}
			this.Close();
		}

		private void measureListBox_SelectedIndexChanged(object sender, EventArgs e) {
			int newSelection = this.measureListBox.SelectedIndex;
			if (this.selectedItem != -1 && this.selectedItem != newSelection) {
				this.measureListBox.SetItemChecked(this.selectedItem, false);
			}
			this.selectedItem = newSelection;
		}
	}
}
