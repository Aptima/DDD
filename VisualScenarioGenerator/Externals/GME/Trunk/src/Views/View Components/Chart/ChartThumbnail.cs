using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;
using ChartDirector;

namespace AME.Views.View_Components.Chart {

    public partial class ChartThumbnail : UserControl {

        /// <summary>
        /// Public delegate for rasing ChartThumbnailDeleted event
        /// </summary>
        public delegate void ChartThumbnailDeletedHandler(object sender, ChartThumbnailDeletedArgs ev);
        /// <summary>
        /// This event is rased when a user selects to delete this thumbnail
        /// </summary>
        public event ChartThumbnailDeletedHandler ChartThumbnailDeleted;

        private static readonly ILog logger = LogManager.GetLogger(typeof(ChartThumbnail));
        private String id = null; 
        private IChartData chartData = null;
        
        public ChartThumbnail() {
			ChartGlobal.register(); //required to set the ChartDirector license
            InitializeComponent();
        }

        public ChartThumbnail(IChartData chartData) : this() {
            this.chartData = chartData;
            this.createChart();
        }

        public ChartThumbnail(IChartData chartData, String id)
            : this(chartData) {
            this.id = id;
        }

        public IChartData ChartData {
            get {
                return this.chartData;
            }
            set {
                this.chartData = value;
            }
        }

        public String Label {
            get {
                return this.label.Text;
            }
            set {
                if (value == null)
                    this.label.Text = String.Empty;
                else
                    this.label.Text = value;
            }
        }

        public String ID {
            get {
                return this.id;
            }
        }

        public ChartType ChartType {
            get {
                return this.chartData.ChartType;
            }
        }

        public void createChart() {
            if (this.chartData != null) {
				switch (ChartType) {
					case ChartType.LINEAR:
						this.createLinearChart();
						break;
					case ChartType.PIE:
						this.createPieChart();
						break;
					case ChartType.BAR:
						this.createBarChart();
						break;
					case ChartType.MULTIBAR:
						this.createMultiBarChart();
						break;

				}
            }
        }

		private void createPieChart() {

			int offset = 10;
			PieChartData data = (PieChartData)this.chartData;
			this.Label = data.Title;

			// Create a PieChart object
            ChartDirector.PieChart c = new ChartDirector.PieChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height); 
			int radius = (Math.Min(this.chartPanel.Size.Width, this.chartPanel.Size.Height) - (2 * offset) ) / 2;

			// Set the center of the pie
			c.setPieSize(this.chartPanel.Size.Width/2, this.chartPanel.Size.Height/2, radius);

			// Set the pie data and the pie labels
			c.setData(data.Data);
			c.setLabelFormat("{label}");

			// output the chart
			this.chartPanel.Image = c.makeImage();  
		}

		private void createBarChart() {

			int offset = 10;
			BarChartData data = (BarChartData)this.chartData;
			this.Label = data.Title;

			// Create a XYChart object.  Use a 2 pixel 3D border.
			XYChart c = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, -1, 2);

			// Set the plotarea 
			c.setPlotArea(offset, offset, this.chartPanel.Size.Width - (2 * offset),
				this.chartPanel.Size.Height - (2 * offset), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

			// Add a multi-color bar chart layer using the given data and colors. Use
			// a 1 pixel 3D border for the bars.
			ChartDirector.BarLayer layer = c.addBarLayer3(data.Data, data.Colors);
			layer.setBorderColor(-1, 1);
			layer.set3D();
			

			// Set the labels on the x axis to null => no labels
			c.yAxis().setLabelFormat(null);

			// output the chart
			this.chartPanel.Image = c.makeImage();
		}


		private void createMultiBarChart() {
			
			int offset = 10;
			MultiBarChartData data = (MultiBarChartData)this.chartData;
			this.Label = data.Title;

			// Create a XYChart object.  Use a 2 pixel 3D border.
			XYChart c = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, -1, 2);

			// Set the plotarea 
			c.setPlotArea(offset, offset, this.chartPanel.Size.Width - (2 * offset),
				this.chartPanel.Size.Height - (2 * offset), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

			// Add a multi-bar chart layer using the given data
			ChartDirector.BarLayer layer = c.addBarLayer2(ChartDirector.Chart.Side, data.BarLayers.Length);
			foreach (BarLayer barLayer in data.BarLayers) {
				layer.addDataSet(barLayer.Data, -1, barLayer.Label);
			}

			// Set the labels on the x axis to null => no labels
			c.yAxis().setLabelFormat(null);

			// output the chart
			this.chartPanel.Image = c.makeImage();
		}

        private void createLinearChart() {

            int offset = 10;
            LinearChartData data = (LinearChartData)this.chartData;
            this.Label = data.Title;

            XYChart c = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, 0x000000, 1);

            c.setPlotArea(2 * offset, offset, this.chartPanel.Size.Width - (3 * offset), 
                this.chartPanel.Size.Height - (3 * offset), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            c.yAxis().setLabelFormat(null);
            c.yAxis().setTitle(data.YAxisLabel, "Arial", 7);
            c.xAxis().setTitle(data.XAxisLabel, "Arial", 7);


            LineLayer layer = null;
            foreach (LinearLayer linearLayer in data.Layers) {
                layer = c.addLineLayer2();
                layer.setLineWidth(2);
                //layer.setXData(linearLayer.XValues);
                layer.addDataSet(linearLayer.YValues);
            }

            this.chartPanel.Image = c.makeImage();
        }

        private void raise_MouseClick(object sender, MouseEventArgs e) {
            this.OnMouseClick(e);
        }

        private void raise_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.OnMouseDoubleClick(e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OnChartThumbnailDeleted(new ChartThumbnailDeletedArgs(this.ChartData.Title));
        }

        protected virtual void OnChartThumbnailDeleted(ChartThumbnailDeletedArgs e) {
            ChartThumbnailDeleted(this, e);
        }
    }

    public class ChartThumbnailDeletedArgs : EventArgs {
        private String chartName;

        public ChartThumbnailDeletedArgs(String chartName) {
            this.chartName = chartName;
        }

        public String ChartName {
            get {
                return this.chartName;
            }
        }
    }
}
