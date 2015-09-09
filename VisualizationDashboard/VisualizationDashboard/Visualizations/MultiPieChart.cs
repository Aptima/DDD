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
    public class MultiPieChart : DashboardVisualization
    {
        private int[] pieChartColors = { 0x00ff00, 0x669933, 0xffcccc };
        const double MINSCALE = 0.4;

        public MultiPieChart(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
        }

        public override void GetDataLists()
        {
            int numOpenFactors = 3;
            chartLabels = new List<string>[numOpenFactors];
            chartMeasureIDs = new List<int>[numOpenFactors];
            rtPMEData = new List<object>();
            InstCurValueList instDataList;
            string[, ,] instIDs;

            // Get the Factor Labels
            for (int i = 0; i < numOpenFactors; i++)
            {
                chartLabels[i] = new List<string>();
                chartMeasureIDs[i] = new List<int>();

                GetFactorLabels(configDisplay.DisplayFactors[i].ConfigDisplay.MeasureName, configDisplay.DisplayFactors[i].FactorName,
                    chartLabels[i], chartMeasureIDs[i]);
            }

            // Create array to store instances names in
            instIDs = new string[chartLabels[0].Count, chartLabels[1].Count, chartLabels[2].Count];

            // Create the list of data arrays for the PM engine to fill in
            rtPMEData = new List<object>();

            // Loop over factors 1 + 2 (X and Y)
            for (int x = 0; x < chartLabels[0].Count; x++)
            {
                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    // Create a new RTPMEData structure to hold the data for this pie chart
                    instDataList = new InstCurValueList();
                    instDataList.instanceIDs = new List<string>();
                    instDataList.dataValues = new List<double>();


                    for (int z = 0; z < chartLabels[2].Count; z++)
                    {
                        List<string> factorLevels = new List<string>();

                        factorLevels.Add(chartLabels[0][x]);
                        factorLevels.Add(chartLabels[1][y]);
                        factorLevels.Add(chartLabels[2][z]);

                        if (y == chartLabels[1].Count - 1)
                        {
                            // Create a Y total instance
                            List<string> listToTotal = new List<string>();

                            for (int j = 0; j < chartLabels[1].Count - 1; j++)
                            {
                                listToTotal.Add(instIDs[x, j, z]);
                            }
                            // Create a total instance
                            instIDs[x, y, z] = CreateRTPMESumInstanceDef(configDisplay, factorLevels, listToTotal, null);
                        }
                        else if (x == chartLabels[0].Count - 1)
                        {
                            // Create a X total instance
                            List<string> listToTotal = new List<string>();

                            for (int j = 0; j < chartLabels[0].Count - 1; j++)
                            {
                                listToTotal.Add(instIDs[j, y, z]);
                            }
                            // Create a total instance
                            instIDs[x, y, z] = CreateRTPMESumInstanceDef(configDisplay, factorLevels, listToTotal, null);
                        }
                        else if (z == chartLabels[2].Count - 1)
                        {
                            // Create a Z total instance
                            List<string> listToTotal = new List<string>();

                            for (int j = 0; j < chartLabels[2].Count - 1; j++)
                            {
                                listToTotal.Add(instIDs[x, y, j]);
                            }
                            // Create a total instance
                            instIDs[x, y, z] = CreateRTPMESumInstanceDef(configDisplay, factorLevels, listToTotal, null);
                        }
                        else
                        {
                            // Create a Z total instance
                            instIDs[x, y, z] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);
                        }

                        // Add this instance to the data list for this pie chart
                        if ((instIDs[x, y, z] != null) && (instIDs[x, y, z].Length > 0) &&
                            (z != chartLabels[2].Count - 1))
                        {
                            instDataList.instanceIDs.Add(instIDs[x, y, z]);
                            instDataList.dataValues.Add(100.0);
                        }
                    }

                    // Add new data object
                    rtPMEData.Add(instDataList);
                }
            }

            // Get Max instance
            string maxInstanceID = instIDs[chartLabels[0].Count - 1,
                                            chartLabels[1].Count - 1,
                                            chartLabels[2].Count - 1];

            // Generate ratio (for pie chart size) dataset
            instDataList = new InstCurValueList();
            instDataList.instanceIDs = new List<string>();
            instDataList.dataValues = new List<double>();

            for (int x = 0; x < chartLabels[0].Count; x++)
            {
                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    string dividendID = instIDs[x, y, chartLabels[2].Count - 1];
                    string ratioID = CreateRTPMERatioInstanceDef(configDisplay, dividendID, maxInstanceID);
                    if ((ratioID != null) && (ratioID.Length > 0))
                    {
                        // Add this instance to the data list for this pie chart
                        instDataList.instanceIDs.Add(ratioID);
                        instDataList.dataValues.Add(0.0);
                    }
                }
            }

            // Add new data object
            rtPMEData.Add(instDataList);

        }

        public override void InitVisualization(StackPanel configDisplayPanel)
        {
            List<string>[] chartLabels = null;

            this.configDisplayPanel = configDisplayPanel;

            // Initialize chart
            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            // Create a grid of pie charts
            Grid pieGrid = new Grid();
            pieGrid.Width = configDisplay.Width;
            pieGrid.Height = configDisplay.Height;
            pieGrid.HorizontalAlignment = HorizontalAlignment.Left;
            pieGrid.VerticalAlignment = VerticalAlignment.Top;

            // Tan border
            SolidColorBrush titleBorderBrush = new SolidColorBrush();
            titleBorderBrush.Color = Color.FromRgb(248, 245, 232);

            // Define the Columns
            for (int i = 0; i < chartLabels[0].Count + 1; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                if (i == 0)
                {
                    colDef.Width = GridLength.Auto;
                }
                pieGrid.ColumnDefinitions.Add(colDef);
            }

            // Add Title and legend rows
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            pieGrid.RowDefinitions.Add(rowDef);

            rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            pieGrid.RowDefinitions.Add(rowDef);

            // Define the Rows
            for (int i = 0; i < chartLabels[1].Count + 1; i++)
            {
                rowDef = new RowDefinition();
                if (i == chartLabels[1].Count)
                {
                    rowDef.Height = GridLength.Auto;
                }
                pieGrid.RowDefinitions.Add(rowDef);
            }

            Border border = null;

            // Add Title
            border = new Border();
            border.BorderBrush = titleBorderBrush;
            border.BorderThickness = new Thickness(4);
            TextBlock txt = new TextBlock();
            txt.Text = configDisplay.Name;
            txt.FontSize = 14;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            border.Child = txt;
            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);
            Grid.SetColumnSpan(border, chartLabels[0].Count + 1);
            pieGrid.Children.Add(border);

            // Label highlight brush
            SolidColorBrush labelHighlightBrush = new SolidColorBrush();
            labelHighlightBrush.Color = Color.FromRgb(188, 201, 214);

            DockPanel dockPanel = null;

            // Add Labels
            for (int i = 0; i < chartLabels[0].Count; i++)
            {
                border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1);
                dockPanel = new DockPanel();
                dockPanel.Background = labelHighlightBrush;
                txt = new TextBlock();
                txt.Text = chartLabels[0][i];
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                dockPanel.Children.Add(txt);
                border.Child = dockPanel;
                Grid.SetColumn(border, i + 1);
                Grid.SetRow(border, chartLabels[1].Count + 2);
                pieGrid.Children.Add(border);
            }

            for (int i = 0; i < chartLabels[1].Count; i++)
            {
                border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1);
                dockPanel = new DockPanel();
                dockPanel.Background = labelHighlightBrush;
                txt = new TextBlock();
                txt.Text = chartLabels[1][i];
                txt.VerticalAlignment = VerticalAlignment.Center;
                dockPanel.Children.Add(txt);
                border.Child = dockPanel;
                Grid.SetColumn(border, 0);
                Grid.SetRow(border, i + 2);
                pieGrid.Children.Add(border);
            }

            // Create the legend for the multi-pie chart
            {
                XYChart legend = new XYChart(300, 50);
                Image img = new Image();
                LegendBox legendBox = legend.addLegend(0, 0, false);
                dockPanel = new DockPanel();

                legendBox.setBackground(0xffffff, 0xffffff);

                for (int z = 0; z < chartLabels[2].Count - 1; z++)
                {
                    legendBox.addKey(chartLabels[2][z], pieChartColors[z]);
                }

                System.Drawing.Image imgWinForms = legend.makeImage();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                imgWinForms.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                CroppedBitmap croppedBitmap = new CroppedBitmap(bi, new Int32Rect(0, 0, 300, 40));
                img.Source = croppedBitmap;
                img.Height = 40;
                img.Width = 300;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetColumn(dockPanel, 0);
                Grid.SetRow(dockPanel, 1);
                Grid.SetColumnSpan(dockPanel, chartLabels[0].Count + 1);
                dockPanel.Children.Add(img);
                pieGrid.Children.Add(dockPanel);
            }


            // Add Grid to config display panel
            configDisplayPanel.Children.Add(pieGrid);
        }

        public override void UpdateVisualization()
        {
            List<string>[] chartLabels = null;
            int dataPos = 0;
            string tooltipString;
            double[] scalingDataArray = null;

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            // Obtain grid of pie charts
            if ((configDisplayPanel.Children == null) || (configDisplayPanel.Children.Count != 1) ||
                (!(configDisplayPanel.Children[0] is Grid)))
            {
                return;
            }
            Grid pieGrid = configDisplayPanel.Children[0] as Grid;

            // Clear the pie grid image children
            List<UIElement> removalList = new List<UIElement>();
            foreach (UIElement child in pieGrid.Children)
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
                    pieGrid.Children.Remove(child);
                }
            }

            // Get the ratio data values for scaling the pie charts
            GetConfigDisplayData(chartLabels[0].Count * chartLabels[1].Count, ref scalingDataArray);

            // Add the individual charts to the Grid
            for (int i = 0; i < chartLabels[0].Count; i++)
            {
                for (int j = 0; j < chartLabels[1].Count; j++)
                {
                    double[] dataArray = null;

                    // Create a PieChart object of size 180 x 160 pixels
                    PieChart c = new PieChart(180, 160);
                    Image img = new Image();
                    //Border border = new Border();
                    //border.BorderBrush = Brushes.Black;
                    //border.BorderThickness = new Thickness(0, 1, 0, 0);

                    // Set the center of the pie at (90, 80) and the radius to 60 pixels
                    c.setPieSize(90, 80, 60);

                    // Set the border color of the sectors to black
                    c.setLineColor(0x000000);

                    // Set the background color of the sector label to pale yellow (ffffc0) with a
                    // black border (000000)
                    //c.setLabelStyle().setBackground(0xffffc0, 0x000000);


                    // Set the title, data and colors according to which pie to draw
                    tooltipString = "";
                    if (GetConfigDisplayData(dataPos, ref dataArray))
                    {
                        c.setData(dataArray);
                        for (int z = 0; z < chartLabels[2].Count - 1; z++)
                        {
                            tooltipString += chartLabels[2][z] + " = " + dataArray[z].ToString() +
                                "\n";
                        }
                    }

                    c.setColors2(Chart.DataColor, pieChartColors);
                    c.setLabelStyle().setFontColor(0xFFFFFF);

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
                    img.Margin = new Thickness(1);
                    //border.Child = img;
                    if ((tooltipString != null) && (tooltipString.Length > 0))
                    {
                        img.ToolTip = tooltipString;
                    }

                    Grid.SetColumn(img, i + 1);
                    Grid.SetRow(img, j + 2);

                    // Scale the pie chart image by the ratio
                    if ((scalingDataArray != null) && (scalingDataArray.Length > dataPos))
                    {
                        if (scalingDataArray[dataPos] == 0.0)
                        {
                            img.Width = 1;
                            img.Height = 1;
                        }
                        else if (double.IsNaN(scalingDataArray[dataPos]))
                        {
                            img.Width = pieGrid.ColumnDefinitions[i + 1].ActualWidth;
                            img.Height = pieGrid.RowDefinitions[j + 2].ActualHeight;
                        }
                        else
                        {
                            double scaleFactor = ((1.0 - MINSCALE) * scalingDataArray[dataPos]) + MINSCALE;
                            img.Width = pieGrid.ColumnDefinitions[i + 1].ActualWidth * scaleFactor;
                            img.Height = pieGrid.RowDefinitions[j + 2].ActualHeight * scaleFactor;
                        }
                    }
                    dataPos++;
                    pieGrid.Children.Add(img);

                }
            }

        }

    }
}
