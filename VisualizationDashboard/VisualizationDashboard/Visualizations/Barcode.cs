using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;

using DashboardDataAccess;
using ChartDirector;

namespace VisualizationDashboard.Visualizations
{
    class Barcode : DashboardVisualization
    {
        private List<double> startList = null;
        private List<double> endList = null;
        private List<double> operatorNum = null;
        private List<int> colorList = null;
        static private int timeTick = 1;
        const int BARCODETIMESPAN = 30;

        public Barcode(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
            startList = new List<double>();
            endList = new List<double>();
            operatorNum = new List<double>();
            colorList = new List<int>();
        }

        public override void GetDataLists()
        {
            int numOpenFactors = 2;
            chartLabels = new List<string>[numOpenFactors];
            chartMeasureIDs = new List<int>[numOpenFactors];
            rtPMEData = new List<object>();
            InstCurValueList instDataList;
            string[,] instIDs;

            // Get the Factor Labels
            for (int i = 0; i < numOpenFactors; i++)
            {
                chartLabels[i] = new List<string>();
                chartMeasureIDs[i] = new List<int>();

                GetFactorLabels(configDisplay.DisplayFactors[i].ConfigDisplay.MeasureName, configDisplay.DisplayFactors[i].FactorName,
                    chartLabels[i], chartMeasureIDs[i]);
            }

            // Create array to store instances names in
            instIDs = new string[chartLabels[0].Count - 1, chartLabels[1].Count - 1];

            // Create the list of data arrays for the PM engine to fill in
            rtPMEData = new List<object>();

            // Loop over factors 1
            for (int x = 0; x < chartLabels[0].Count - 1; x++)
            {
                // Create a new rtPMEData structure to hold the data for this pie chart
                instDataList = new InstCurValueList();
                instDataList.instanceIDs = new List<string>();
                instDataList.dataValues = new List<double>();

                for (int y = 0; y < chartLabels[1].Count - 1; y++)
                {
                    List<string> factorLevels = new List<string>();

                    factorLevels.Add(chartLabels[0][x]);
                    factorLevels.Add(chartLabels[1][y]);

                    instIDs[x, y] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);

                    // Add this instance to the data list for this stacked histogram
                    if ((instIDs[x, y] != null) && (instIDs[x, y].Length > 0))
                    {
                        instDataList.instanceIDs.Add(instIDs[x, y]);
                        instDataList.dataValues.Add(100.0);
                    }
                }

                // Add new data object
                rtPMEData.Add(instDataList);
            }
        }

        public override void InitVisualization(StackPanel configDisplayPanel)
        {
            this.configDisplayPanel = configDisplayPanel;

            SingleChartInit(configDisplayPanel);
        }

        public override void UpdateVisualization()
        {
            List<string>[] chartLabels = null;
            double[] dataArray = null;
            Image img = new Image();
            List<string> xLabels = new List<string>();
            int minTime = 0;

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }
            for (int i = 0; i < chartLabels[0].Count - 1; i++)
            {
                xLabels.Add(chartLabels[0][i]);
            }

            // Obtain grid
            if ((configDisplayPanel.Children == null) || (configDisplayPanel.Children.Count != 1) ||
                (!(configDisplayPanel.Children[0] is Grid)))
            {
                return;
            }
            Grid grid = configDisplayPanel.Children[0] as Grid;
            double gridWidth = grid.ColumnDefinitions[0].ActualWidth;
            double gridHeight = configDisplay.Height - (grid.RowDefinitions[0].ActualHeight + grid.RowDefinitions[1].ActualHeight);

            // Clear the grid images
            List<UIElement> removalList = new List<UIElement>();
            foreach (UIElement child in grid.Children)
            {
                if (child is Image)
                {
                    removalList.Add(child);
                }
            }
            if (removalList.Count > 0)
            {
                foreach (UIElement child in removalList)
                {
                    grid.Children.Remove(child);
                }
            }

            // The task index, start date, end date and color for each bar
            double[] taskNo = { 0, 1 };
            double[] startValues = { .5, 2.5 };
            double[] endValues = { 1.5, 3.5 };

            // Create a XYChart object of size 620 x 325 pixels. Set background color
            // to light red (0xffcccc), with 1 pixel 3D border effect.
            XYChart c = new XYChart((int)gridWidth, (int)gridHeight);

            c.setPlotArea(150, 50, (int)gridWidth - 200, (int)gridHeight - 100, 0xffffff, 0xeeeeee, Chart.LineColor,
                0xc0c0c0, 0xc0c0c0).setGridWidth(2, 1, 1, 1);

            // swap the x and y axes to create a horziontal box-whisker chart
            c.swapXY();

            // Get RT PME Data
            for (int i = 0; i < xLabels.Count; i++)
            {
                if (GetConfigDisplayData(i, ref dataArray))
                {
                    for (int j = 0; j < dataArray.Length; j++)
                    {
                        if (dataArray[j] > 0)
                        {
                            startList.Add((double)timeTick - 0.4);
                            endList.Add((double)timeTick + 0.4);
                            operatorNum.Add(i);
                            colorList.Add((int)barcodeColors[j]);
                        }
                    }
                }
            }

            timeTick++;
            // Remove old information from data lists
            if (timeTick - BARCODETIMESPAN < 0)
            {
                minTime = 0;
            }
            else
            {
                minTime = timeTick - BARCODETIMESPAN;
            }
            int pos = 0;
            while ((pos < startList.Count) && (startList[pos] <= minTime))
            {
                pos++;
            };
            if (pos != 0)
            {
                startList.RemoveRange(0, pos);
                endList.RemoveRange(0, pos);
                operatorNum.RemoveRange(0, pos);
                colorList.RemoveRange(0, pos);
            }

            // Add a multi-color box-whisker layer to represent the bars
            BoxWhiskerLayer layer = c.addBoxWhiskerLayer2(startList.ToArray(), endList.ToArray(), null, null, null, colorList.ToArray());
            layer.setXData(operatorNum.ToArray());
            layer.setBorderColor(Chart.SameAsMainColor);

            c.yAxis().setLinearScale((double)minTime, (double)timeTick + 0.5, 1, 0);

            // Set the y-axis to shown on the top (right + swapXY = top)
            c.setYAxisOnRight();

            // Set the labels on the x axis
            c.xAxis().setLabels(xLabels.ToArray());
            //c.yAxis().setLabels(yLabelsVals);

            // Reverse the x-axis scale so that it points downwards.
            c.xAxis().setReverse();

            // Set the horizontal ticks and grid lines to be between the bars
            c.xAxis().setTickOffset(0.5);
            //c.yAxis().setTickOffset(0.5);

            // Divide the plot area height ( = 200 in this chart) by the number of
            // tasks to get the height of each slot. Use 80% of that as the bar
            // height.
            //layer.setDataWidth((int)(200 * 4 / 5 / (xLabels.Count)));

            // Add a legend box at (140, 265) - bottom of the plot area. Use 8 pts
            // Arial Bold as the font with auto-grid layout. Set the width to the
            // same width as the plot area. Set the backgorund to grey (dddddd).
            /*            LegendBox legendBox = c.addLegend2(140, 265, Chart.AutoGrid,
                            "Arial Bold", 8);
                        legendBox.setWidth(461);
                        legendBox.setBackground(0xdddddd);
                        */

            /*
                        XYChart c = new XYChart(configDisplay.Width, configDisplay.Height);
                        c.setPlotArea(150, 50, configDisplay.Width - 200, configDisplay.Height - 100);
                        c.addLegend(20, 20);
                        c.xAxis().setLabels(xLabels.ToArray());

                        // Add a stacked bar layer and set the layer 3D depth to 8 pixels
                        BarLayer layer = c.addBarLayer2(Chart.Stack, 8);

                        // Enable bar label for the whole bar
                        layer.setAggregateLabelStyle();

                        // Enable bar label for each segment of the stacked bar
                        layer.setDataLabelStyle();

                        for (int i = 0; i < xLabels.Count; i++)
                        {
                            if (rtpmeConnection.GetConfigDisplayData(configDisplay.ConfigDisplayID, i, ref dataArray))
                            {
                                layer.addDataSet(dataArray, colorValues[i], xLabels[i]);
                            }
                        }
                        */

            // Generate an image of the chart
            System.Drawing.Image imgWinForms = c.makeImage();
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...

            imgWinForms.Save(ms, ImageFormat.Bmp);

            // Rewind the stream...

            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;

            bi.EndInit();
            img.Source = bi;

            Grid.SetColumn(img, 0);
            Grid.SetRow(img, 2);
            grid.Children.Add(img);
        }            

    }
}
