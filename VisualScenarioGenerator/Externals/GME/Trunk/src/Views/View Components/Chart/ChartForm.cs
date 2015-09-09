using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using ChartDirector;

namespace AME.Views.View_Components.Chart {

    public partial class ChartForm : Form {

        private static readonly ILog logger = LogManager.GetLogger(typeof(ChartForm));

        private IChartData chartData = null;
        
        private ChartForm() {
			ChartGlobal.register(); //required to set the ChartDirector license
            InitializeComponent();
        }

        public ChartForm(IChartData chartData) : this() {
            this.chartData = chartData;
            this.createChart();
        }

        public IChartData ChartData {
            get {
                return this.chartData;
            }
        }

        private void createChart() {

            if (this.chartData == null) {
                logger.Debug("The ChartData for this graph is NULL");
                return;
            }

            switch (this.chartData.ChartType) {
                case ChartType.LINEAR:
                    this.buildLinearChart();
                    break;
				case ChartType.PIE:
					this.buildPieChart();
					break;
				case ChartType.BAR:
					this.buildBarChart();
					break;
				case ChartType.MULTIBAR:
					this.buildMultiBarChart();
					break;

                default:
                    logger.Warn("ChartDataType is not defined");
                    break;
            }
        }

        private void buildLinearChart() {

            int topMargin = 20;
            int bottomnMargin = 70;
            int leftMargin = 50;
            int rightMargin = 150;

            LinearChartData data = (LinearChartData)this.chartData;
            this.Text = data.Title;

            XYChart c = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, 0x000000, 1);

            //c.setPlotArea(2 * offset, offset, this.chartPanel.Size.Width - (3 * offset),
            //    this.chartPanel.Size.Height - (3 * offset), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
            c.setPlotArea(leftMargin, topMargin, this.chartPanel.Size.Width - (leftMargin + rightMargin),
                this.chartPanel.Size.Height - (topMargin + bottomnMargin), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);


            c.addLegend(Size.Width - (rightMargin + 5), topMargin, true, "Arial Bold", 9).setBackground(
                ChartDirector.Chart.Transparent);

            c.xAxis().setTitle(data.XAxisLabel);
            c.yAxis().setTitle(data.YAxisLabel);

            LineLayer layer = null;
            foreach (LinearLayer linearLayer in data.Layers) {
                layer = c.addLineLayer2();
                layer.setLineWidth(2);
                layer.setXData(linearLayer.XValues);
                layer.addDataSet(linearLayer.YValues, -1, linearLayer.LayerLegend);
            }

            this.chartPanel.Image = c.makeImage();
        }

		private void buildPieChart() {

			int offset = 20;
			PieChartData data = (PieChartData)this.chartData;
            ChartDirector.PieChart pieChart = new ChartDirector.PieChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height);

			int radius = (Math.Min(this.chartPanel.Size.Width, this.chartPanel.Size.Height) - (2 * offset)) / 2;
			// Set the center of the pie
			pieChart.setPieSize(this.chartPanel.Size.Width/2, this.chartPanel.Size.Height/2, radius);
			pieChart.addTitle(data.Title);

			pieChart.setData(data.Data, data.Labels);

			pieChart.setLabelStyle("", 8);
			pieChart.set3D();
			pieChart.setLabelLayout(1);

			// output the chart
			this.chartPanel.Image = pieChart.makeImage();  				
		}

		private void buildBarChart() {

			int topMargin = 30;
			int bottomnMargin = 70;
			int leftMargin = 50;
			int rightMargin = 30; 
			
			BarChartData data = (BarChartData)this.chartData;
			this.Text = data.Title;
			XYChart barChart = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, -1, 2);

            barChart.setPlotArea(leftMargin, topMargin, this.chartPanel.Size.Width - (leftMargin + rightMargin),
                this.chartPanel.Size.Height - (topMargin + bottomnMargin), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
			
			barChart.addTitle(data.Title);
			// Set the labels on the x axis.
			barChart.xAxis().setLabels(data.Labels);

			ChartDirector.BarLayer layer = barChart.addBarLayer3(data.Data, data.Colors, data.Labels);
			layer.setBorderColor(-1, 1);
			layer.set3D();

			// output the chart
			this.chartPanel.Image = barChart.makeImage();
		}

		private void buildMultiBarChart() {

			int topMargin = 30;
			int bottomnMargin = 60;
			int leftMargin = 50;
			int rightMargin = 150;

			MultiBarChartData data = (MultiBarChartData)this.chartData;
			this.Text = data.Title;
			XYChart barChart = new XYChart(this.chartPanel.Size.Width, this.chartPanel.Size.Height, 0xeeeeff, -1, 2);

			barChart.setPlotArea(leftMargin, topMargin, this.chartPanel.Size.Width - (leftMargin + rightMargin),
				this.chartPanel.Size.Height - (topMargin + bottomnMargin), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

			barChart.addTitle(data.Title);

			barChart.addLegend(Size.Width - (rightMargin - 5), topMargin, true, "Arial Bold", 9).setBackground(
				ChartDirector.Chart.Transparent);

			// Set the labels on the x axis.
			barChart.xAxis().setLabels(data.XLabels);

			// Add a multi-bar chart layer using the given data
			ChartDirector.BarLayer layer = barChart.addBarLayer2(ChartDirector.Chart.Side, data.BarLayers.Length);
			foreach (BarLayer barLayer in data.BarLayers) {
				layer.addDataSet(barLayer.Data, -1, barLayer.Label);
			}

			// output the chart
			this.chartPanel.Image = barChart.makeImage();
		}

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}