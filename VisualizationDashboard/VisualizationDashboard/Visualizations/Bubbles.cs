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
    public class Bubbles : DashboardVisualization
    {
        private double[] xOffsets = null;
        private double[] yOffsets = null;

        public Bubbles(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
        }

        public override void GetDataLists()
        {
            chartLabels = new List<string>[3];
            chartMeasureIDs = new List<int>[3];
            rtPMEData = new List<object>();
            InstCurValueList instDataList;
            string[,,] instIDs;

            if (configDisplay.DisplayFactors.Count < 2)
            {
                return;
            }

            // Get the Factor Labels
            for (int i = 0; i < configDisplay.DisplayFactors.Count; i++)
            {
                chartLabels[i] = new List<string>();
                chartMeasureIDs[i] = new List<int>();

                GetFactorLabels(configDisplay.DisplayFactors[i].ConfigDisplay.MeasureName, configDisplay.DisplayFactors[i].FactorName,
                    chartLabels[i], chartMeasureIDs[i], true, i == 1);
            }

            // Create offset arrays
            if ((configDisplay.DisplayFactors.Count == 2) ||  (chartLabels[2].Count == 1))
            {
                xOffsets = new double[] {0};
                yOffsets = new double[] {0};
            }
            else if (chartLabels[2].Count == 2)
            {
                xOffsets = new double[] { -0.2, 0.2 };
                yOffsets = new double[] { -0.2, 0.2 };
            }
            else if (chartLabels[2].Count == 3)
            {
                xOffsets = new double[] { -0.2, 0, 0.2 };
                yOffsets = new double[] { -0.2, 0, 0.2 };
            }
            else if (chartLabels[2].Count == 4)
            {
                xOffsets = new double[] { -0.2, 0.2, -0.2, 0.2 };
                yOffsets = new double[] { -0.2, -0.2, 0.2, 0.2 };
            }
            else if (chartLabels[2].Count == 5)
            {
                xOffsets = new double[] { 0.0, -0.2, 0.2, -0.2, 0.2 };
                yOffsets = new double[] { 0.0, -0.2, -0.2, 0.2, 0.2 };
            }

            // Create array to store instances names in
            int zCount = 0;
            if (configDisplay.DisplayFactors.Count == 2)
            {
                zCount = 1;
            }
            else
            {
                zCount = chartLabels[2].Count;
            }

            instIDs = new string[chartLabels[0].Count, chartLabels[1].Count, zCount];

            // Create the list of data arrays for the PM engine to fill in
            rtPMEData = new List<object>();

            // Loop over factors 1
            for (int z = 0; z < zCount; z++)
            {
                // Create a new RTPMEData structure to hold the data for this pie chart
                instDataList = new InstCurValueList();
                instDataList.instanceIDs = new List<string>();
                instDataList.dataValues = new List<double>();

                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    for (int x = 0; x < chartLabels[0].Count; x++)
                    {
                        List<string> factorLevels = new List<string>();

                        factorLevels.Add(chartLabels[0][x]);
                        factorLevels.Add(chartLabels[1][y]);
                        if (configDisplay.DisplayFactors.Count > 2)
                        {
                            factorLevels.Add(chartLabels[2][z]);
                        }

                        // Create the RT PME Instance Definition
                        instIDs[x, y, z] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);

                        // Add this instance to the data list for this stacked histogram
                        instDataList.instanceIDs.Add(instIDs[x, y, z]);
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

            SingleChartInit(configDisplayPanel, false, true);
        }

        public override void UpdateVisualization()
        {
            List<string>[] chartLabels = null;
            double[] dataArray = null;
            Image img = new Image();
            int i = 0;
            List<string> xLabels = new List<string>();
            List<string> yLabels = new List<string>();

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            int zCount = 0;
            if (chartLabels[2] == null)
            {
                zCount = 1;
            }
            else
            {
                zCount = chartLabels[2].Count;
            }

            xLabels.Add("");
            for (i = 0; i < chartLabels[0].Count; i++)
            {
                xLabels.Add(chartLabels[0][i]);
            }
            xLabels.Add("");
            yLabels.Add("");
            for (i = 0; i < chartLabels[1].Count; i++)
            {
                yLabels.Add(chartLabels[1][i]);
            }
            yLabels.Add("");

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
            c.setPlotArea(100, 0, configDisplay.Width - 200, configDisplay.Height - 100);
            c.xAxis().setLabels(xLabels.ToArray());
            c.yAxis().setLabels(yLabels.ToArray());

            c.xAxis().setTickOffset(-0.5);
            c.yAxis().setTickOffset(-0.5);

            // Create x and y value list
            List<double>[] dataX = new List<double>[zCount];
            List<double>[] dataY = new List<double>[zCount];

            for (int z = 0; z < zCount; z++)
            {
                dataX[z] = new List<double>();
                dataY[z] = new List<double>();

                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    for (int x = 0; x < chartLabels[0].Count; x++)
                    {
                        dataX[z].Add(x + 1 + xOffsets[z]);
                        dataY[z].Add(y + 1 + yOffsets[z]);
                    }
                }
            }
            List<double> dataZ = new List<double>();
            double maxBubbleSize = (configDisplay.Width - 200) / xLabels.Count / 2.0;

            i = 0;
            for (int z = 0; z < zCount; z++)
            {
                if (GetConfigDisplayData(i, ref dataArray))
                {
                    ArrayMath a = new ArrayMath(dataArray.ToArray());
                    double maxValue = a.max();

                    if (maxValue < maxBubbleSize)
                    {
                        for (int j = 0; j < dataArray.Length; j++)
                        {
                            if (dataArray[j] == 0)
                            {
                                dataZ.Add(0.0);
                            }
                            else
                            {
                                dataZ.Add(dataArray[j] + 10);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < dataArray.Length; j++)
                        {
                            if (dataArray[j] == 0)
                            {
                                dataZ.Add(0.0);
                            }
                            else
                            {
                                dataZ.Add(dataArray[j] / maxValue * maxBubbleSize + 10);
                            }
                        }
                    }
                }
                i++;

                if (chartLabels[2] == null)
                {
                    c.addScatterLayer(dataX[z].ToArray(), dataY[z].ToArray(), configDisplay.MetricName, Chart.CircleSymbol, 9,
                        (int) barcodeColorsTrans[z], (int) barcodeColorsTrans[z]).setSymbolScale(dataZ.ToArray());
                }
                else
                {
                    c.addScatterLayer(dataX[z].ToArray(), dataY[z].ToArray(), chartLabels[2][z], Chart.CircleSymbol, 9,
                        (int) barcodeColorsTrans[z], (int) barcodeColorsTrans[z]).setSymbolScale(dataZ.ToArray());
                }
                dataZ.Clear();
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
