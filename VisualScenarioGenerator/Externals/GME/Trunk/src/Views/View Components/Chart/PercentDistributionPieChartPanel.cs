using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ChartDirector;

namespace AME.Views.View_Components.Chart {

	public partial class PercentDistributionPieChartPanel : UserControl {

		private PieChartData chartData = null;
        private ChartDirector.PieChart pieChart = null;
		
		private enum DataValidation {
			VALID, INVALID, EXCEEDS100PERCENT
		}

		public PercentDistributionPieChartPanel() {
			ChartGlobal.register(); //required to set the ChartDirector license
			InitializeComponent();
		}

		public bool loadData(PieChartData pieChartData) {

            this.pieChart = new ChartDirector.PieChart(this.Size.Width, this.Size.Height); 

			switch (this.validateData(pieChartData)) {
				case DataValidation.INVALID:
					this.drawInvalidChart("Invalid Data");
					return false;
				case DataValidation.EXCEEDS100PERCENT:
					this.drawInvalidChart("Sum of all percent values exceeds 100%");
					return false;
				case DataValidation.VALID:
					//if (!this.compareTo(pieChartData)) {
						//this.drawInvalidChart("Valid DATA, GREAT!!!");
						this.chartData = pieChartData;
						this.drawChart();
					//}
					return true;
			}
			return false; //unknown case
		}

		public void clear() {
			// output the chart
            this.viewer.Image = new ChartDirector.PieChart(this.Size.Width, this.Size.Height).makeImage();  
		}

		private void drawChart() {

			int offset = 5;
			int radius = (Math.Min(this.viewer.Size.Width, this.viewer.Size.Height) - (2 * offset)) / 2;
			// Set the center of the pie
			this.pieChart.setPieSize(this.viewer.Size.Width/2, this.viewer.Size.Height/2, radius);
			this.pieChart.addTitle(this.chartData.Title, "", 10);

			double sum = 0;
			foreach (double d in this.chartData.Data)
				sum += d;

			if (sum != 100) {
				double[] newData = new double[this.chartData.Data.Length + 1];
				String[] newLabels = new String[this.chartData.Labels.Length + 1];
				Array.Copy(this.chartData.Data, newData, this.chartData.Data.Length);
				Array.Copy(this.chartData.Labels, newLabels, this.chartData.Labels.Length);

				newData[newData.Length - 1] = 100 - sum;
				newLabels[newLabels.Length - 1] = "Unassigned";

				// Set the pie data
				this.pieChart.setData(newData, newLabels);
			}
			else {
				this.pieChart.setData(this.chartData.Data, this.chartData.Labels);

			}

			this.pieChart.setLabelFormat("{label} {percent}%");
			this.pieChart.setLabelStyle("", 7);
			this.pieChart.set3D();
			this.pieChart.setLabelLayout(1);
			
			// output the chart
			this.viewer.Image = this.pieChart.makeImage();  
		}

		private void drawInvalidChart(String message) {

			int x = this.viewer.Size.Width / 2;
			int y = this.viewer.Size.Height / 2;

			this.pieChart.addText(x, y, message, "bold", 10, 0xFF0000, 5); 

			// output the chart
			this.viewer.Image = this.pieChart.makeImage();  
		}

		private DataValidation validateData(PieChartData pieChartData) {

			if (pieChartData == null || pieChartData.ChartType != ChartType.PIE || pieChartData.Data == null || 
				pieChartData.Labels == null || pieChartData.Data.Length != pieChartData.Labels.Length) {

				return DataValidation.INVALID;
			}
			else {
				double sum = 0;
				foreach (double num in pieChartData.Data)
					sum += num;

				if (sum > 100)
					return DataValidation.EXCEEDS100PERCENT;
				else
					return DataValidation.VALID;
			}
		}

		private bool compareTo(PieChartData pieChartData) {
			
			if (this.chartData == null || !this.chartData.Title.Equals(pieChartData.Title) || 
				this.chartData.Data.Length != pieChartData.Data.Length) {
			
				return false;
			}

			for (int i = 0; i < this.chartData.Data.Length; i++) {

				if (this.chartData.Data[i] != pieChartData.Data[i] || !this.chartData.Labels[i].Equals(pieChartData.Labels[i]))
					return false;
			}

			return true;
		}
	}
}
