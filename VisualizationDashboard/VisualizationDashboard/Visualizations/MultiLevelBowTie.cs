using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DashboardDataAccess;
using System.Xml;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Globalization;

using Aptima.Visualization.Data;

namespace VisualizationDashboard.Visualizations
{
    class MultiLevelBowTie : DashboardVisualization
    {
        static private double legendWidth = 80.0;
        static private double bowTieMargin = 20.0;
        static private double topMargin = 20.0;

        public MultiLevelBowTie(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
        }

        public override void GetDataLists()
        {
            chartLabels = new List<string>[3];
            chartMeasureIDs = new List<int>[3];
            rtPMEData = new List<object>();
            InstCurValueList instDataList;
            string[, ,] instIDs;

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
                    chartLabels[i], chartMeasureIDs[i], i == 0);
            }

            int xCount = 0;
            if (configDisplay.DisplayFactors.Count == 2)
            {
                xCount = 1;
            }
            else
            {
                xCount = chartLabels[2].Count;
            }

            // Create array to store instances names in
            instIDs = new string[chartLabels[0].Count, chartLabels[1].Count, xCount];

            // Create the list of data arrays for the PM engine to fill in
            rtPMEData = new List<object>();

            // Loop over factors 1
            for (int z = 0; z < chartLabels[0].Count; z++)
            {
                // Create a new RTPMEData structure to hold the data for this pie chart
                instDataList = new InstCurValueList();
                instDataList.instanceIDs = new List<string>();
                instDataList.dataValues = new List<double>();

                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    for (int x = 0; x < xCount; x++)
                    {
                        List<string> factorLevels = new List<string>();

                        factorLevels.Add(chartLabels[0][z]);
                        factorLevels.Add(chartLabels[1][y]);
                        if (configDisplay.DisplayFactors.Count > 2)
                        {
                            factorLevels.Add(chartLabels[2][x]);
                        }

                        // Create the RT PME Instance Definition
                        instIDs[z, y, x] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);

                        // Add this instance to the data list for this stacked histogram
                        instDataList.instanceIDs.Add(instIDs[z, y, x]);
                        instDataList.dataValues.Add(100.0);
                    }
                }

                // Add new data object
                rtPMEData.Add(instDataList);
            }
        }

        public override void InitVisualization(System.Windows.Controls.StackPanel configDisplayPanel)
        {
            this.configDisplayPanel = configDisplayPanel;

            SingleChartInit(configDisplayPanel, true);

        }

        public override void UpdateVisualization()
        {
            List<string>[] chartLabels = null;
            double[] dataArray = null;
            List<string> xLabels = new List<string>();

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            int xCount = 0;
            if (chartLabels[2] == null)
            {
                xCount = 1;
            }
            else
            {
                xCount = chartLabels[2].Count;
            }

            // Obtain grid
            if ((configDisplayPanel.Children == null) || (configDisplayPanel.Children.Count != 1) ||
                (!(configDisplayPanel.Children[0] is Grid)))
            {
                return;
            }
            Grid grid = configDisplayPanel.Children[0] as Grid;

            //Get any old multi level bow tie in the grid
            var visualizationsToBeRemoved = grid.Children.OfType<Aptima.Visualization.MultilevelBowTie>();
            //remove the old multi level bow tie from the grid
            visualizationsToBeRemoved.ToList().ForEach(x => grid.Children.Remove(x));

            // Remove old labels
            var visualizationsToBeRemoved2 = grid.Children.OfType<Label>();
            visualizationsToBeRemoved2.ToList().ForEach(x => grid.Children.Remove(x));

            double bowTieWidth = configDisplay.Width - legendWidth;
            double bowTieHeight = configDisplay.Height;

            Aptima.Visualization.MultilevelBowTie bowTie = new Aptima.Visualization.MultilevelBowTie();
            bowTie.center = new Point(bowTieWidth / 2, bowTieHeight / 2);
            bowTie.MaxRadius = Math.Min(bowTieWidth / 2, bowTieHeight / 2);
            bowTie.MaxRadius -= bowTieMargin;

            for (int z = 0; z < chartLabels[0].Count; z++)
            {
                HierarchicalValue rootHValue = null;
                HierarchicalValue outerRim1 = null;
                HierarchicalValue outerRim2 = null;

                // Set Factor 1, which is overall total split on two factors
                GetConfigDisplayData(z, ref dataArray);
                rootHValue = new HierarchicalValue(chartLabels[0][z] + " = " + dataArray[chartLabels[1].Count * xCount - 1], 0, chartLabels[0][z]);
                if (z == 0)
                {
                    bowTie.LeftTotal = dataArray[chartLabels[1].Count * xCount - 1];
                    bowTie.left = rootHValue;
                }
                else if (z == 1)
                {
                    bowTie.RightTotal = dataArray[chartLabels[1].Count * xCount - 1];
                    bowTie.right = rootHValue;
                }

                // Factor 2
                for (int y = 0; y < chartLabels[1].Count - 1; y++)
                {
                    outerRim1 = new HierarchicalValue(chartLabels[1][y] + " = " + dataArray[xCount * y + xCount - 1].ToString(), dataArray[xCount * y + xCount - 1],
                        chartLabels[1][y]);
                    rootHValue.children.Add(outerRim1);

                    if (xCount > 1)
                    {
                        // Factor 3
                        for (int x = 0; x < xCount - 1; x++)
                        {
                            outerRim2 = new HierarchicalValue(chartLabels[2][x] + " = " + dataArray[xCount * y + x].ToString(), dataArray[xCount * y + x],
                                chartLabels[2][x]);
                            outerRim1.children.Add(outerRim2);
                        }
                    }
                }
            }

            //regenerate multi level bow tie canvas
            bowTie.generate();

            // Add color legend
            int legendRow = 0;
            int legendRowCount = 0;

            // Count number of rows;
            for (int i = 0; i < chartLabels.Length; i++)
            {
                if (chartLabels[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < chartLabels[i].Count; j++)
                {
                    if (((chartLabels[i][j].CompareTo("All")) == 0) ||
                        ((chartLabels[i][j].CompareTo("Team")) == 0) ||
                        ((chartLabels[i][j].CompareTo("Total")) == 0))
                    {
                        continue;
                    }
                    legendRowCount += 2;
                }
                legendRowCount += 2;
            }

            for (int i = 0; i < chartLabels.Length; i++)
            {
                if (chartLabels[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < chartLabels[i].Count; j++)
                {
                    if (((chartLabels[i][j].CompareTo("All")) == 0) ||
                        ((chartLabels[i][j].CompareTo("Team")) == 0) ||
                        ((chartLabels[i][j].CompareTo("Total")) == 0))
                    {
                        continue;
                    }

                    // Show Label
                    Label l = new Label();
                    Typeface myTypeface = new Typeface(l.FontFamily.ToString());
                    FormattedText ft = new FormattedText(chartLabels[i][j],
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        myTypeface, l.FontSize, Brushes.Black);
                    l.Content = chartLabels[i][j];
                    l.HorizontalContentAlignment = HorizontalAlignment.Left;
                    l.VerticalAlignment = VerticalAlignment.Top;
                    double legendTopMargin = (bowTieHeight - topMargin - ft.Height * legendRowCount) / 2;
                    l.RenderTransform = new TranslateTransform(bowTieWidth - legendWidth + 25, topMargin + legendTopMargin + legendRow * ft.Height);
                    Grid.SetColumn(l, 0);
                    Grid.SetRow(l, 0);
                    grid.Children.Add(l);

                    // Show color box
                    Rectangle r = new Rectangle();
                    r.Width = 20;
                    r.Height = 20;
                    if (bowTie.Brushes.Keys.Contains(chartLabels[i][j]))
                    {
                        r.Fill = bowTie.Brushes[chartLabels[i][j]];
                    }
                    else
                    {
                        r.Fill = System.Windows.Media.Brushes.White;
                    }
                    r.Stroke = System.Windows.Media.Brushes.Black;
                    r.StrokeThickness = 0.5;
                    r.HorizontalAlignment = HorizontalAlignment.Left;
                    r.VerticalAlignment = VerticalAlignment.Top;
                    r.RenderTransform = new TranslateTransform(bowTieWidth - legendWidth, topMargin + legendTopMargin +
                        legendRow * ft.Height);
                    Grid.SetColumn(r, 0);
                    Grid.SetRow(r, 0);
                    grid.Children.Add(r);

                    legendRow += 2;
                }
                legendRow += 2;
            }

            Grid.SetColumn(bowTie, 0);
            Grid.SetRow(bowTie, 0);
            grid.Children.Add(bowTie);
        }
    }
}
