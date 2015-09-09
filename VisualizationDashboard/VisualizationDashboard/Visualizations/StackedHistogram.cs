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
    public class StackedHistogram : DashboardVisualization
    {
        public StackedHistogram(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
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
                    chartLabels[i], chartMeasureIDs[i], true);
            }

            // Create array to store instances names in
            instIDs = new string[chartLabels[0].Count, chartLabels[1].Count];

            // Create the list of data arrays for the PM engine to fill in
            rtPMEData = new List<object>();

            // Loop over factors 1
            for (int y = 0; y < chartLabels[1].Count; y++)
            {
                // Create a new RTPMEData structure to hold the data for this pie chart
                instDataList = new InstCurValueList();
                instDataList.instanceIDs = new List<string>();
                instDataList.dataValues = new List<double>();

                for (int x = 0; x < chartLabels[0].Count; x++)
                {
                    List<string> factorLevels = new List<string>();

                    factorLevels.Add(chartLabels[0][x]);
                    factorLevels.Add(chartLabels[1][y]);

                    // Create the RT PME Instance Definition
                    instIDs[x, y] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);

                    // Add this instance to the data list for this stacked histogram
                    instDataList.instanceIDs.Add(instIDs[x, y]);
                    instDataList.dataValues.Add(100.0);
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

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }
            for (int i = 0; i < chartLabels[0].Count; i++)
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

            // Create the Stacked Histogram
            XYChart c = new XYChart(configDisplay.Width, configDisplay.Height);
            //c.setPlotArea(150, 50, configDisplay.Width - 200, configDisplay.Height - 100);
            c.setPlotArea(50, 50, configDisplay.Width - 100, configDisplay.Height - 175);
            //c.addLegend(20, 20);
            c.xAxis().setLabels(xLabels.ToArray());

            // Add a stacked bar layer and set the layer 3D depth to 8 pixels
            BarLayer layer = c.addBarLayer2(Chart.Stack, 8);

            // Enable bar label for the whole bar
            layer.setAggregateLabelStyle();

            // Enable bar label for each segment of the stacked bar
            layer.setDataLabelStyle();

            for (int i = 0; i < chartLabels[1].Count; i++)
            {
                if (GetConfigDisplayData(i, ref dataArray))
                {
                    layer.addDataSet(dataArray, (int) barcodeColors[i], chartLabels[1][i]);
                }
            }

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
            img.Stretch = Stretch.Uniform;

            Grid.SetColumn(img, 0);
            Grid.SetRow(img, 2);
            grid.Children.Add(img);
        }

    }
}
