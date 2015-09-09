using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ChartDirector;

using Controllers;

namespace View_Components
{
    public partial class ChartView : WinChartViewer
    {

        private IController myController;

        private string title = "";
        private string xAxisLabel = "";
        private string yAxisLabel = "";
        private double[] xAxisValues = { };
        private Dictionary<string, double[]> yDataDict = new Dictionary<string, double[]>();

        private const int THUMBNAIL = 0;
        private const int FULLSIZE = 1;
        private int graphType = FULLSIZE;

        public ChartView()
        {
            InitializeComponent();

            ChartDirector.Chart.setLicenseCode("DIST-0000-0536-4cc1-aec1");
        }

        #region Properties

        public string Title
        {
            set
            {
                this.title = value;
            }
        }

        public string XAxisLabel
        {
            set
            {
                this.xAxisLabel = value;
            }
        }

        public string YAxisLabel
        {
            set
            {
                this.yAxisLabel = value;
            }
        }

        public double[] XAxisValues
        {
            set
            {
                this.xAxisValues = value;
            }
        }

        public Dictionary<string, double[]> YAxisDataDict
        {
            set
            {
                this.yDataDict = value;
            }
        }
        #endregion

        #region ViewComponentUpdate Members
        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }
        #endregion

        public void drawGraph(bool fullSize) {
            if (fullSize)
                this.drawFullSize();
            else
                this.drawThumbnail();

        }

        private void drawThumbnail() {

            this.graphType = THUMBNAIL;

            int xOffset = 10;
            int yTopMargin = 28;
            int yLowMargin = 10;

            // Create an XYChart object of size 644 x 434 pixels, with a light blue
            // (EEEEFF) background, black border, 1 pxiel 3D border effect
            XYChart c = new XYChart(this.Size.Width, this.Size.Height, 0xeeeeff, 0x000000, 1);
            //XYChart c = new XYChart(644, 434, 0xeeeeff, 0x000000, 1);
            //c.setRoundedFrame();

            // Add a title box to the chart using 15 pts Times Bold Italic font, on a
            // light blue (CCCCFF) background with glass effect. white (0xffffff) on
            // a dark red (0x800000) background, with a 1 pixel 3D border.
            c.addTitle(this.title, "Times New Roman Bold Italic", 8).setBackground(0xccccff, 0x000000,
                ChartDirector.Chart.softLighting());
            
            // Set the plotarea at (55, 65) and of size 520 x 250 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light
            // grey color (0xcccccc)
            c.setPlotArea(xOffset, yTopMargin, this.Size.Width - (2*xOffset), this.Size.Height - (yTopMargin+yLowMargin), 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            // Add a legend box at (50, 30) (top of the chart) with horizontal
            // layout. Use 9 pts Arial Bold font. Set the background and border color
            // to Transparent.
            //c.addLegend(50, 30, false, "Arial Bold", 9).setBackground(
            //    ChartDirector.Chart.Transparent);


            //c.xAxis().setTitle(this.xAxisLabel);
            //c.yAxis().setTitle(this.yAxisLabel);

            //c.xAxis().setLabels(this.xAxisValues);
            //c.yAxis().setLabels(null, null);
            c.yAxis().setLabelFormat(null);
            // Add a line layer to the chart
            LineLayer layer = c.addLineLayer2();

            // Set the default line width to 2 pixels
            layer.setLineWidth(2);

            // Add the three data sets to the line layer. For demo purpose, we use a
            // dash line color for the last line
            foreach (KeyValuePair<string, double[]> kvp in this.yDataDict) {
                layer.addDataSet(kvp.Value, -1, kvp.Key);
            }

            //layer.addDataSet(data1, 0x008800, "Server #2");
            //layer.addDataSet(data2, c.dashLineColor(0x3333ff, ChartDirector.Chart.DashLine), "Server #3");

            this.Image = c.makeImage();

            //include tool tip for the chart
            this.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='[{dataSetName}] Hour {xLabel}: {value} MBytes'");
        }

        private void drawFullSize() {

            this.graphType = FULLSIZE;

            // Create an XYChart object of size 644 x 434 pixels, with a light blue
            // (EEEEFF) background, black border, 1 pxiel 3D border effect
            XYChart c = new XYChart(this.Size.Width, this.Size.Height, 0xeeeeff, 0x000000, 1);
            //XYChart c = new XYChart(644, 434, 0xeeeeff, 0x000000, 1);
            //c.setRoundedFrame();

            // Set the plotarea at (55, 65) and of size 520 x 250 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light
            // grey color (0xcccccc)
            c.setPlotArea(55, 65, 520, 250, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            // Add a legend box at (50, 30) (top of the chart) with horizontal
            // layout. Use 9 pts Arial Bold font. Set the background and border color
            // to Transparent.
            c.addLegend(50, 30, false, "Arial Bold", 9).setBackground(
                ChartDirector.Chart.Transparent);

            // Add a title box to the chart using 15 pts Times Bold Italic font, on a
            // light blue (CCCCFF) background with glass effect. white (0xffffff) on
            // a dark red (0x800000) background, with a 1 pixel 3D border.
            c.addTitle(this.title, "Times New Roman Bold Italic", 15).setBackground(0xccccff, 0x000000,
                ChartDirector.Chart.softLighting());

            c.xAxis().setTitle(this.xAxisLabel);
            c.yAxis().setTitle(this.yAxisLabel);

            c.xAxis().setLabels(this.xAxisValues);

            // Add a line layer to the chart
            LineLayer layer = c.addLineLayer2();

            // Set the default line width to 2 pixels
            layer.setLineWidth(2);

            // Add the three data sets to the line layer. For demo purpose, we use a
            // dash line color for the last line
            foreach (KeyValuePair<string, double[]> kvp in this.yDataDict)
            {
                layer.addDataSet(kvp.Value, -1, kvp.Key);
            }

            //layer.addDataSet(data1, 0x008800, "Server #2");
            //layer.addDataSet(data2, c.dashLineColor(0x3333ff, ChartDirector.Chart.DashLine), "Server #3");

            this.Image = c.makeImage();

            //include tool tip for the chart
            this.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='[{dataSetName}] Hour {xLabel}: {value} MBytes'");
        }

        public ChartView clone() {
            ChartView view = new ChartView();
            view.Title = this.title;
            view.XAxisLabel = this.xAxisLabel;
            view.YAxisLabel = this.yAxisLabel;
            view.XAxisValues = this.xAxisValues;
            view.YAxisDataDict = this.yDataDict;

            return view;
        }
    }
}
