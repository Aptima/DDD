using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using System.Xml.XPath;
using log4net;
using AME.Views.View_Components.Chart;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace AME.Views.View_Components {
    public partial class AssessemntResults : UserControl {

        private static readonly ILog logger = LogManager.GetLogger(typeof(AssessemntResults));

        private int simulationId = -1;
        private int simRunId = -1;
        private AssessmentController assessmentController = null;
		private MeasuresController measuresController = null;

        private enum Tabs {
            RAWDATA = 0,
            CUSTOMGRAPHS = 1,
            RTORGRAPHS = 2,
            MEASUREDATA = 3,
            VISUALIZATION = 4
        }


        private enum GraphDataType {
            CUSTOMGRAPHS = 0,
            RUNTORUNGRAPHS = 1,
			MEASUREGRAPHS = 2
        }

        private bool rawdataLoaded = false;
        private bool customgraphsLoaded = false;
        private bool rtorgraphsLoaded = false;
        private bool measuredataLoaded = false;
		private bool measureGraphsLoaded = false;
        private bool visualizationLoaded = false;
        private bool reportcardLoaded = false;

        private IChartData[] customGraphs = new IChartData[0];
        private IChartData[] runtorunGraphs = new IChartData[0];
		private IChartData[] measureGraphs = new IChartData[0];

        private string measureOutputFileName = String.Empty;
        private string measureInputFileName = String.Empty;
        
        public AssessemntResults() {

            AMEManager cm = AMEManager.Instance;
            this.assessmentController = (AssessmentController)cm.Get("AssessmentEditor");
			this.measuresController = (MeasuresController)AMEManager.Instance.Get("OptMeasuresEditor");

            InitializeComponent();
            this.tabControl.TabPages.Clear();
        }

        public int SimulationId {
            get {
                return this.simulationId;
            }
            set {
                if (value != this.simulationId) {
                    this.simulationId = value; //careful!! dont forget to set a new SimRunId now!!!
                    this.simRunId = -1;
                    this.clearTabs();
                }
            }
        }

        public int SimRunId {
            get {
                return this.simRunId;
            }
            set {
                if (value != this.simRunId) {
                    try {
                        this.simRunId = value;
                        measureOutputFileName = assessmentController.OutputPath + MeasureOutputFileName();
                        measureInputFileName = assessmentController.OutputPath + MeasureInputFileName();
                        System.Diagnostics.Debug.WriteLine("Changed measure output to " + measureOutputFileName);
                        this.clearTabs();
                        this.loadData();
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
		}

		#region Add New Graph Methods

		public void addCustomGraph(IChartData chartData) {

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Graph");
            root.SetAttribute("name", chartData.Title);

            XmlElement element = null;
            XmlElement layer = null;
            switch(chartData.ChartType) {
                case (ChartType.LINEAR):
                    root.SetAttribute("type", ChartType.LINEAR.ToString());
                    LinearChartData data = (LinearChartData)chartData;
                    
                    element = doc.CreateElement("XLabel");
                    element.SetAttribute("name", data.XAxisLabel);
                    root.AppendChild(element);
                    
                    element = doc.CreateElement("YLabel");
                    element.SetAttribute("name", data.YAxisLabel);
                    root.AppendChild(element);

                    element = doc.CreateElement("Layers");
                    element.SetAttribute("name", data.LayerName);
                    foreach (LinearLayer l in data.Layers) {
                        layer = doc.CreateElement("Layer");
                        layer.SetAttribute("name", l.LayerLegend);
                        element.AppendChild(layer);
                    }
                    root.AppendChild(element);

                    if (this.assessmentController.AddCustomGraphToFile(this.SimRunId, root)) {
                        List<IChartData> list = new List<IChartData>(this.customGraphs);
                        list.Add(chartData);
                        this.customGraphs = list.ToArray();
                        break;
                    }
                    else {
                        logger.Debug(String.Format(
                            "Unable to save a new Custom graph {0} for the SimRun ID: {1}",
                            chartData.Title, this.SimRunId));
                        return; //do NOT add the graph!
                    }
                default:
                    logger.Warn("Undefined ChartType " + chartData.ChartType.ToString());
                    return; //do NOT add the graph!
            }

            this.displaySavedCustomGraph(chartData);
        }

        public void addRunToRunGraph(IChartData chartData, int[] selectedSimRunId) {

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Graph");
            root.SetAttribute("name", chartData.Title);

            XmlElement element = null;
            XmlElement runs = null;
            XmlElement layer = null;
            List<String> layerList = new List<String>();
            bool useSimRunAsX = false;
            switch (chartData.ChartType) {
                case (ChartType.LINEAR):
                    root.SetAttribute("type", ChartType.LINEAR.ToString());
                    LinearChartData data = (LinearChartData)chartData;

                    runs = doc.CreateElement("SimRuns");
                    foreach (int simRun in selectedSimRunId) {
                        element = doc.CreateElement("SimRun");
                        element.SetAttribute("id", simRun.ToString());
                        runs.AppendChild(element);
                    }
                    root.AppendChild(runs);

                    element = doc.CreateElement("XLabel");
                    element.SetAttribute("name", data.XAxisLabel);
                    root.AppendChild(element);

                    element = doc.CreateElement("YLabel");
                    element.SetAttribute("name", data.YAxisLabel);
                    root.AppendChild(element);

                    //Create Layers node and its Layer children
                    element = doc.CreateElement("Layers");
                    element.SetAttribute("name", data.LayerName);
                    foreach (LinearLayer l in data.Layers) {

                        String prefix = null;
                        String layerName = l.LayerLegend;
                        foreach (int simRun in selectedSimRunId) {
                            prefix = "SimRun " + simRun + ": ";

                            if (l.LayerLegend.StartsWith(prefix)) {
                                layerName = l.LayerLegend.Replace(prefix, "");
                                break;
                            }
                        }
                        if (!layerList.Contains(layerName))
                            layerList.Add(layerName);

                    }
                    foreach (String lName in layerList) {
                        layer = doc.CreateElement("Layer");
                        layer.SetAttribute("name", lName);
                        element.AppendChild(layer);

                    }
                    element.SetAttribute("useSimRunAsX", useSimRunAsX.ToString().ToLower());
                    root.AppendChild(element);

                    if (this.assessmentController.AddRunToRunGraphToFile(this.SimulationId, root)) {
                        List<IChartData> list = new List<IChartData>(this.runtorunGraphs);
                        list.Add(chartData);
                        this.runtorunGraphs = list.ToArray();
                        break;
                    }
                    else {
                        logger.Debug(String.Format(
                            "Unable to save a new Run-To-Run graph {0} for the Simulation ID: {1}",
                            chartData.Title, this.SimulationId));
                        return; //do NOT add the graph!
                    }
                default:
                    logger.Warn("Undefined ChartType " + chartData.ChartType.ToString());
                    return; //do NOT add the graph!
            }

            this.displaySavedRunToRunGraph(chartData);
		}

		public void addMeasureGraph(IChartData chartData) {

			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Graph");
			root.SetAttribute("name", chartData.Title);

			switch (chartData.ChartType) {
				case (ChartType.BAR):
					root.SetAttribute("type", ChartType.BAR.ToString());
					break;
				case (ChartType.MULTIBAR):
					root.SetAttribute("type", ChartType.MULTIBAR.ToString());
					break;

				default:
					logger.Warn("Undefined ChartType " + chartData.ChartType.ToString());
					return; //do NOT add the graph!
			}

			if (this.measuresController.addMeasureGraphToFile(this.SimRunId, root, MeasuresController.GraphCategory.SINGLEMEASURE)) {
				List<IChartData> list = new List<IChartData>(this.measureGraphs);
				list.Add(chartData);
				this.measureGraphs = list.ToArray();
			}
			else {
				logger.Debug(String.Format(
					"Unable to save a new Measure Custom graph {0} for the Run ID: {1}",
					chartData.Title, this.SimRunId));
				return; //do NOT add the graph!
			}

			this.displaySavedMeasureGraph(chartData);
		}

		#endregion

		#region TabbedPages Management Methods

        private void displayTab(TabPage page, bool display)
        {
            if (display)
                this.tabControl.TabPages.Add(page);
            else
                this.tabControl.TabPages.Remove(page);
        }

		/// <summary>
		/// Display or remove the Raw Data tabbed page from the assessment results
		/// </summary>
		/// <param name="display">true - display, false - remove</param>
        public void displayRawDataTab(bool display) {
            this.displayTab(this.rawDataPage, display);
        }

		/// <summary>
		/// Display or remove the Custom Graphs tabbed page from the assessment results
		/// </summary>
		/// <param name="display">true - display, false - remove</param>
		public void displayCustomGraphsTab(bool display) {
            this.displayTab(this.customGraphsPage, display);
		}

		/// <summary>
		/// Display or remove the Run To Run Graphs tabbed page from the assessment results
		/// </summary>
		/// <param name="display">true - display, false - remove</param>
		public void displayRunToRunGraphsTab(bool display) {
            this.displayTab(this.rToRGraphsPage, display);
		}

		/// <summary>
		/// Display or remove the Measures tabbed page from the assessment results
		/// </summary>
		/// <param name="display">true - display, false - remove</param>
		public void displayMeasuresTab(bool display) {
            this.displayTab(this.measureDataPage, display);
		}

		/// <summary>
		/// Display or remove the Measure Graph tabbed page from the assessment results
		/// <param name="display">true - display, false - remove</param>
		/// </summary>
		public void displayMeasureGraphTab(bool display) {
            this.displayTab(this.measureGraphPage, display);
		}

		/// <summary>
		/// Display or remove the Visualization tabbed page from the assessment results
		/// <param name="display">true - display, false - remove</param>
		/// </summary>
		public void displayVisualizationTab(bool display) {
            this.displayTab(this.visualizationPage, display);
		}


        /// <summary>
        /// Display or remove the Report Card tabbed page from the assessment results
        /// <param name="display">true - display, false - remove</param>
        /// </summary>
        public void displayReportCardTab(bool display) {
            this.displayTab(this.reportCardPage, display);
        }

		#endregion

		#region Private Methods

		/// <summary>
        /// Create a ChartThumbnail from the ChartData and display it on the 
        /// Custom Graphs Tab Panel.
        /// </summary>
        /// <param name="chartData">The chart data.</param>
        private void displaySavedCustomGraph(IChartData chartData) {
            this.customGraphsFlowPanel.Controls.Add(this.createThumbnail(chartData));
            this.tabControl.SelectTab(this.customGraphsPage);
        }

        /// <summary>
        /// Create a ChartThumbnail from the ChartData and display it on the 
        /// Run-To-Run Graphs Tab Panel.
        /// </summary>
        /// <param name="chartData">The chart data.</param>
        private void displaySavedRunToRunGraph(IChartData chartData) {
            this.rToRGraphsFlowPanel.Controls.Add(this.createThumbnail(chartData));
            this.tabControl.SelectTab(this.rToRGraphsPage);
        }

		private void displaySavedMeasureGraph(IChartData chartData) {
			this.measureGraphsFlowPanel.Controls.Add(this.createThumbnail(chartData));
			this.tabControl.SelectTab(this.measureGraphPage);
		}

        /// <summary>
        /// Creates the ChartThumbnail object from the ChartData and adds all
        /// Mouse Events to it.
        /// </summary>
        /// <param name="chartData">The chart data.</param>
        /// <returns></returns>
        private ChartThumbnail createThumbnail(IChartData chartData) {
            ChartThumbnail thumbnail = new ChartThumbnail(chartData);
            thumbnail.MouseClick += new System.Windows.Forms.MouseEventHandler(this.thumbnail_Click);
            thumbnail.ChartThumbnailDeleted += new ChartThumbnail.ChartThumbnailDeletedHandler(thumbnail_ChartThumbnailDeleted);

            return thumbnail;
        }
        
        private void thumbnail_Click(object sender, MouseEventArgs e) {
            IChartData data = ((ChartThumbnail)sender).ChartData;
            ChartForm form = new ChartForm(data);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }

        private void thumbnail_ChartThumbnailDeleted(object sender, ChartThumbnailDeletedArgs e) {

			TabPage selectedPage = this.tabControl.SelectedTab;
			if (selectedPage == this.customGraphsPage) {
				if (this.assessmentController.DeleteCustomGraphFromFile(this.SimRunId, e.ChartName)) {
					this.customGraphsFlowPanel.Controls.Remove((ChartThumbnail)sender);
				}
			}
			else if (selectedPage == this.rToRGraphsPage) {
				if (this.assessmentController.DeleteRunToRunGraphFromFile(this.SimulationId, e.ChartName)) {
					this.rToRGraphsFlowPanel.Controls.Remove((ChartThumbnail)sender);
				}
			}
			else if (selectedPage == this.measureGraphPage) {
				if (this.measuresController.deleteGraphFromFile(this.SimRunId, e.ChartName, AME.Controllers.MeasuresController.GraphCategory.SINGLEMEASURE)) {
					this.measureGraphsFlowPanel.Controls.Remove((ChartThumbnail)sender);
				}
			}
			else {
				logger.Warn("Selected Tab Index is undefined");
			}

			#region old
			//switch (this.tabControl.SelectedIndex) {
			//    case ((int)Tabs.CUSTOMGRAPHS):
			//        if (this.assessmentController.DeleteCustomGraphFromFile(this.SimRunId, e.ChartName)) {
			//            this.customGraphsFlowPanel.Controls.Remove((ChartThumbnail)sender);
			//        }
			//        break;
			//    case ((int)Tabs.RTORGRAPHS):
			//        if (this.assessmentController.DeleteRunToRunGraphFromFile(this.SimulationId, e.ChartName)) {
			//            this.rToRGraphsFlowPanel.Controls.Remove((ChartThumbnail)sender);
			//        }
			//        break;
			//    default:
			//        logger.Debug("Selected Tab Index is neither CUSTOMGRAPHS nor RTORGRAPHS");
			//        break;
			//}
			#endregion
		}

        private void tabControl_Selected(object sender, TabControlEventArgs e) {

			bool doLoad = false;
			TabPage selectedPage = this.tabControl.SelectedTab;
			if (selectedPage == this.rawDataPage) {
				if (!this.rawdataLoaded) {
					doLoad = true;
					this.rawdataLoaded = true;
				}
			}
			else if (selectedPage == this.customGraphsPage) {
				if (!this.customgraphsLoaded) {
					doLoad = true;
					this.customgraphsLoaded = true;
				}
			}
			else if (selectedPage == this.rToRGraphsPage) {
				if (!this.rtorgraphsLoaded) {
					doLoad = true;
					this.rtorgraphsLoaded = true;
				}
			}
			else if (selectedPage == this.measureDataPage) {
				if (!this.measuredataLoaded) {
					doLoad = true;
					this.measuredataLoaded = true;
				}
			}
			else if (selectedPage == this.measureGraphPage) {
				if (!this.measureGraphsLoaded) {
					doLoad = true;
					this.measureGraphsLoaded = true;
				}
			}
            else if (selectedPage == this.visualizationPage)
            {
                if (!this.visualizationLoaded)
                {
                    doLoad = true;
                    this.visualizationLoaded = true;
                }
            }
            else if (selectedPage == this.reportCardPage)
            {
                if (!this.reportcardLoaded)
                {
                    doLoad = true;
                    this.reportcardLoaded = true;
                }
            }
            else {
				logger.Warn("Selected Tab Index is undefined");
			}

            if (doLoad) {
                try {
                    this.loadData();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }

			#region old
			//switch (this.tabControl.SelectedIndex) {
			//    case ((int)Tabs.RAWDATA):
			//        if (!this.rawdataLoaded) {
			//            this.loadData();
			//            this.rawdataLoaded = true;
			//        }
			//        break;
			//    case ((int)Tabs.CUSTOMGRAPHS):
			//        if (!this.customgraphsLoaded) {
			//            this.loadData();
			//            this.customgraphsLoaded = true;
			//        }
			//        break;
			//    case ((int)Tabs.RTORGRAPHS):
			//        if (!this.rtorgraphsLoaded) {
			//            this.loadData();
			//            this.rtorgraphsLoaded = true;
			//        }
			//        break;
			//    case ((int)Tabs.MEASUREDATA):
			//        if (!this.measuredataLoaded) {
			//            this.loadData();
			//            this.measuredataLoaded = true;
			//        }
			//        break;
			//    case ((int)Tabs.VISUALIZATION):
			//        if (!this.visualizationLoaded)
			//        {
			//            this.loadData();
			//            this.visualizationLoaded = true;
			//        }
			//        break;
			//    default:
			//        logger.Warn("Selected Tab Index is undefined");
			//        break;
			//}
			#endregion
		}

        private void loadData() {
            if (tabControl.SelectedTab.Name.Equals("rawDataPage")) {
                this.dataGridView.DataSource = this.getRawDataTable();
                this.rawdataLoaded = true;
            }
            else if (tabControl.SelectedTab.Name.Equals("customGraphsPage")) {
                this.customGraphs = this.getGraphs(GraphDataType.CUSTOMGRAPHS);
                if (this.customGraphs != null && this.customGraphs.Length > 0) {
                    foreach (IChartData data in this.customGraphs) {
                        this.displaySavedCustomGraph(data);
                    }
                }
                else
                    logger.Debug("There are NO Custom Graphs for SimRun " + this.simRunId);

                this.customgraphsLoaded = true;
            }
            else if (tabControl.SelectedTab.Name.Equals("rToRGraphsPage")) {
                this.runtorunGraphs = this.getGraphs(GraphDataType.RUNTORUNGRAPHS);
                if (this.runtorunGraphs != null && this.runtorunGraphs.Length > 0) {
                    foreach (IChartData data in this.runtorunGraphs) {
                        this.displaySavedRunToRunGraph(data);
                    }
                }
                else
                    logger.Debug("There are NO Run-To-Run Graphs for Simulation " + this.simulationId);
                
                this.rtorgraphsLoaded = true;
            }
            else if (tabControl.SelectedTab.Name.Equals("measureDataPage")) {

                string resultDoc = this.getMeasureHtmlOutputPath();
                if (resultDoc != null) {
                    this.measureWebBrowser.Navigate(new Uri(resultDoc));
                }
                else
                    logger.Debug("There are NO Measure Output for SimRun " + this.simRunId);

                this.measuredataLoaded = true;
            }
			else if (tabControl.SelectedTab.Name.Equals("measureGraphPage")) {
				this.measureGraphs = this.getMeasureGraphs();
				if (this.measureGraphs != null && this.measureGraphs.Length > 0) {
					foreach (IChartData data in this.measureGraphs) {
						this.displaySavedMeasureGraph(data);
					}
				}
				else
					logger.Debug("There are NO Measure Graphs for Run ID" + this.simRunId);

				this.measureGraphsLoaded = true;
			}
            else if (tabControl.SelectedTab.Name.Equals("visualizationPage"))
            {
                Int32 visId = this.assessmentController.GetVisualizationId(this.SimRunId);

                this.ganttChart1.ComponentId = visId;
                this.ganttChart1.Controller = this.assessmentController;
                this.ganttChart1.Category = "Visualization Parameters";
                this.ganttChart1.Parameter = "Filename";
                this.ganttChart1.UpdateViewComponent(); 
            }
            else if (tabControl.SelectedTab.Name.Equals("reportCardPage"))
            {
                panelLeft.Controls.Clear();
                if (measureOutputFileName == String.Empty || measureInputFileName == String.Empty) {
                    try {
                        SimRunId = simRunId;
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                if (!File.Exists(measureOutputFileName) || !File.Exists(measureInputFileName)) {
                    throw new Exception("Required data files were not found.");
                }
                ReportCardModel model = null;
                try {
                    ReportCardHelper helper = new ReportCardHelper();
                    model = helper.GetReportCardData(measureOutputFileName, measureInputFileName);
                    if (model != null) {
                        Label label;
                        foreach (DataRow row in model.Tables[ReportCardModel.TABLE_REPORT_CARD].Rows) {
                            label = new Label();
                            label.AutoSize = false;
                            label.Text = row[ReportCardModel.MEASURE_NAME].ToString();
                            label.Dock = DockStyle.Top;
                            panelLeft.Controls.Add(label);
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void clearTabs() {
            this.dataGridView.DataSource = null;
            this.customGraphs = new IChartData[0];
            this.customGraphsFlowPanel.Controls.Clear();
            this.runtorunGraphs = new IChartData[0];
            this.rToRGraphsFlowPanel.Controls.Clear();
            this.measureWebBrowser.Navigate(new Uri("about:blank"));
			this.measureGraphsFlowPanel.Controls.Clear();

            this.rawdataLoaded = false;
            this.customgraphsLoaded = false;
            this.rtorgraphsLoaded = false;
            this.measuredataLoaded = false;
			this.measureGraphsLoaded = false;
            this.visualizationLoaded = false;
            this.reportcardLoaded = false;
        }

        private DataTable getRawDataTable() {

            //Get data in DataTable format
            DataTable tempTable = new DataTable();
            bool isColumnBuilt = false;
            List<String> attrValues = null;

            XPathNavigator firstDataSet = null;
            try {
                firstDataSet = this.assessmentController.GetSimRunData(this.SimRunId).CreateNavigator();
            }
            catch (NullReferenceException) {
                return tempTable;
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

		private DataTable getMeasuredGraphDataTable(int runId, String measureName) {

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

			XPathNavigator measureSet = firstDataSet.SelectSingleNode("/Measures/Measure[@name='" + measureName +"']");
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

		private IChartData[] getMeasureGraphs() {

			String measureName = null;
			String chartType = null;

			XPathNavigator firstDataSet = this.measuresController.getGraphs(this.SimRunId, MeasuresController.GraphCategory.SINGLEMEASURE).CreateNavigator();

			XPathNodeIterator graphNodes = firstDataSet.Select("//Graph");
			List<IChartData> chartDataList = new List<IChartData>();
			foreach (XPathNavigator graphNav in graphNodes) {
				
				measureName = graphNav.GetAttribute("name", graphNav.NamespaceURI);
				chartType = graphNav.GetAttribute("type", graphNav.NamespaceURI);
				DataTable chartTable = this.getMeasuredGraphDataTable(this.simRunId, measureName);
				IChartData chartData = null;

				switch (chartType) {
					case ("BAR"):
						chartData = this.createBarChart(measureName, chartTable);
						break;
					case ("MULTIBAR"):
						chartData = this.createMultiBarChart(measureName, chartTable);
						break;
				}
				chartDataList.Add(chartData);
			}

			return chartDataList.ToArray();
		}

		private MultiBarChartData createMultiBarChart(String title, DataTable chartTable) {

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

			return new MultiBarChartData(title, barLayers, labels);
		}

		private BarChartData createBarChart(String title, DataTable chartTable) {

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

			return new BarChartData(title, data, labels, null);
		}

        private IChartData[] getGraphs(GraphDataType gdt) {

            List<IChartData> chartData = new List<IChartData>();

            String graphName = null;
            String chartType = null;
            String xLabel = null;
            String yLabel = null;
            String layerName = null;
            List<int> simRuns = null;
            List<String> layers = null;

			XPathNavigator firstDataSet = null;;
			switch (gdt) {
				case GraphDataType.CUSTOMGRAPHS:
					this.assessmentController.GetCustomGraphs(this.SimRunId).CreateNavigator();
					break;
				case GraphDataType.RUNTORUNGRAPHS:
					this.assessmentController.GetCustomGraphs(this.SimulationId).CreateNavigator();
					break;
				default:
					return null;

			}

            XPathNodeIterator graphNodes = firstDataSet.Select("//Graph");
            foreach (XPathNavigator graphNav in graphNodes) {

                if (gdt == GraphDataType.RUNTORUNGRAPHS && 
                    graphNav.SelectSingleNode(String.Format(@"SimRuns/SimRun[@id='{0}']", this.SimRunId)) == null) {

                    continue; //doesn't contain the current SimRun data
                }

                simRuns = new List<int>();
                int sRun;
                layers = new List<String>();
                graphName = graphNav.GetAttribute("name", graphNav.NamespaceURI);
                chartType = graphNav.GetAttribute("type", graphNav.NamespaceURI);
                XPathNodeIterator pathParameters = null;
                XPathNodeIterator graphParameters = graphNav.Select("*");
                foreach (XPathNavigator parameter in graphParameters) {
                    switch(parameter.Name) {
                        case("SimRuns"):
                            pathParameters = parameter.Select("SimRun");
                            foreach (XPathNavigator run in pathParameters) {
                                try {
                                    sRun = Int32.Parse(run.GetAttribute("id", run.NamespaceURI));
                                    if (!simRuns.Contains(sRun))
                                        simRuns.Add(sRun);
                                }
                                catch (FormatException) {
                                    logger.Warn("SimRun id is not an integer: " + 
                                        run.GetAttribute("id", run.NamespaceURI));
                                }
                            }
                            break;
                        case("XLabel"):
                            xLabel = parameter.GetAttribute("name", parameter.NamespaceURI);
                            break;
                        case("YLabel"):
                            yLabel = parameter.GetAttribute("name", parameter.NamespaceURI);
                            break;
                        case("Layers"):
                            layerName = parameter.GetAttribute("name", parameter.NamespaceURI);
                            pathParameters = parameter.Select("Layer");
                            foreach (XPathNavigator layerNode in pathParameters)
                                layers.Add(layerNode.GetAttribute("name", layerNode.NamespaceURI));
                            break;
                        default:
                            logger.Debug("Undefined parameter " + parameter.Name + " in graph " + graphName);
                            break;
                    }
                }

                List<ILayer> ll = new List<ILayer>();
                IChartData data = null;
                switch (chartType) {
                    case ("LINEAR"):

                        if (gdt == GraphDataType.RUNTORUNGRAPHS) {
                            foreach (int run in simRuns) {
                                ll.AddRange(getLayersData(run, true, xLabel, yLabel, layerName, layers.ToArray()));
                            }
                        }
                        else
                            ll.AddRange(getLayersData(this.SimRunId, false, xLabel, yLabel, layerName, layers.ToArray()));

                        LinearLayer[] lll = new LinearLayer[ll.Count];
                        int count = 0;
                        foreach (ILayer il in ll)
                            lll[count++] = (LinearLayer)il;

                        data = new LinearChartData(graphName, xLabel, yLabel, layerName, lll);
                        break;
                    default:
                        logger.Debug("Graph " + graphName + " is of undefined ChartType." + chartType);
                        break;
                }
                chartData.Add(data);
            }
            return chartData.ToArray();
        }

        private ILayer[] getLayersData(int runId, bool isRunToRun, String xLabel, String yLabel, String layerName, String[] layerItem) {

            DataTable table = new DataTable();
            table.Columns.Add(layerName);
            table.Columns.Add(xLabel);
            table.Columns.Add(yLabel);

            XPathNavigator firstDataSet = this.assessmentController.GetSimRunData(runId).CreateNavigator();
            XPathNodeIterator dataNodes = firstDataSet.Select("//DataSet");
            
            String temp = null, name = null, xVal = null, yVal = null;
            //bool match = false;
            foreach (XPathNavigator dataNav in dataNodes) {

                XPathNodeIterator parameterNodes =
                    dataNav.SelectDescendants(XPathNodeType.Element, false);
                foreach (XPathNavigator parameter in parameterNodes) {

                    if (parameter.Name.CompareTo("Data") != 0)
                        continue;

                    temp = parameter.GetAttribute("Name", parameter.NamespaceURI);
                    if (temp.CompareTo(layerName) == 0) {
                        name = parameter.GetAttribute("Value", parameter.NamespaceURI);
                    }
                    else if (temp.CompareTo(xLabel) == 0){
                        xVal = parameter.GetAttribute("Value", parameter.NamespaceURI);
                    }
                    else if (temp.CompareTo(yLabel) == 0) {
                        yVal = parameter.GetAttribute("Value", parameter.NamespaceURI);
                    }
                }

                //if (match)
                    table.Rows.Add(new String[] { name, xVal, yVal });

                temp = null;
                name = null;
                xVal = null;
                yVal = null;
            }

            ILayer[] finalLayer = new LinearLayer[layerItem.Length];
            int counter = 0;
            List<double> x = null;
            List<double> y = null;
            String legendName = null;
            foreach (String item in layerItem) {

                x = new List<double>();
                y = new List<double>();
                
                foreach (DataRow row in table.Rows) {
                    if (row[layerName].ToString().CompareTo(item) == 0) {

                        try {
                            x.Add(Double.Parse(row[xLabel].ToString()));
                            y.Add(Double.Parse(row[yLabel].ToString()));
                        }
                        catch (FormatException) {
                            String message = "Data you chose to graph is not numeric";
                            logger.Debug(message);
                            throw new FormatException(message);
                        }
                    }
                }

                if (isRunToRun) {
                    legendName = "SimRun " + runId + ": " + item;
                }
                else {
                    legendName = item;
                }
                finalLayer[counter++] = new LinearLayer(x.ToArray(), y.ToArray(), legendName);
            }

            return finalLayer;
        }

        private string getMeasureHtmlOutputPath() {
            string sMeasureHtmlOutput = null;
            try {
                string sOutputDir = this.assessmentController.OutputPath;
                string sResultDoc = sOutputDir + "MeasuresOutput.html";
                string sSourceDoc = sOutputDir + MeasureOutputFileName();

                XmlReader xslReader = this.assessmentController.GetXSL("Measures.xsl");

                XslCompiledTransform xslTransform = new XslCompiledTransform();
                xslTransform.Load(xslReader);
                xslReader.Close();

                xslTransform.Transform(sSourceDoc, sResultDoc);

                sMeasureHtmlOutput = sResultDoc;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            return sMeasureHtmlOutput;
        }
        private string MeasureOutputFileName() {
            string outputFileName = String.Empty;
            XPathNavigator simRun = this.assessmentController.GetMeasureData(this.simRunId).CreateNavigator();
            string sXPath = string.Format("Components/Component[@ID='{0}']/Component[@BaseType='{1}']/ComponentParameters/Parameter/Parameter[@propertyName='{2}']", this.simRunId, "Measure", "OutputFilename");
            XPathNodeIterator measureParameterNodes = simRun.Select(sXPath);
            foreach (XPathNavigator measureParameter in measureParameterNodes) {
                outputFileName = measureParameter.GetAttribute("value", measureParameter.NamespaceURI);
                outputFileName += ".xml";
            }
            return outputFileName;
        }
        private string MeasureInputFileName() {
            string inputFileName = String.Empty;
            XPathNavigator simRun = this.assessmentController.GetMeasureData(this.simRunId).CreateNavigator();
            string sXPath = string.Format("Components/Component[@ID='{0}']/Component[@BaseType='{1}']/ComponentParameters/Parameter/Parameter[@propertyName='{2}']", this.simRunId, "Measure", "InputFilename");
            XPathNodeIterator measureParameterNodes = simRun.Select(sXPath);
            foreach (XPathNavigator measureParameter in measureParameterNodes) {
                inputFileName = measureParameter.GetAttribute("value", measureParameter.NamespaceURI);
                inputFileName += ".xml";
            }
            return inputFileName;
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }//getMeasureOutputPath

		#endregion
	}
}
