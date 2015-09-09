using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Visualization;
using System.Windows.Controls;
using System.Windows;
using DashboardDataAccess;
using System.Xml;

namespace VisualizationDashboard.Visualizations
{
    class Heatmap : DashboardVisualization
    {
        private HeatMap heatmap = new HeatMap();

        public Heatmap(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
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
            for (int x = 0; x < chartLabels[0].Count; x++)
            {
                // Create a new RTPMEData structure to hold the data for this pie chart
                instDataList = new InstCurValueList();
                instDataList.instanceIDs = new List<string>();
                instDataList.dataValues = new List<double>();

                for (int y = 0; y < chartLabels[1].Count; y++)
                {
                    List<string> factorLevels = new List<string>();

                    factorLevels.Add(chartLabels[0][x]);
                    factorLevels.Add(chartLabels[1][y]);

                    instIDs[x, y] = CreateRTPMEInstanceDef(configDisplay, factorLevels, null);

                    // Add this instance to the data list for this stacked histogram
                    instDataList.instanceIDs.Add(instIDs[x, y]);
                    instDataList.dataValues.Add(100.0);
                }

                // Add new data object
                rtPMEData.Add(instDataList);
            }
        }

        public override void InitVisualization(System.Windows.Controls.StackPanel configDisplayPanel)
        {
            this.configDisplayPanel = configDisplayPanel;

            SingleChartInit(configDisplayPanel, true);

            heatmap.numBuckets = 5;
            heatmap.Width = configDisplay.Width;
            heatmap.Height = configDisplay.Height;
        }

        public override void UpdateVisualization()
        {
            List<string>[] chartLabels = null;
            double[] dataArray = null;
            Image img = new Image();
            List<string> xLabels = null;
            List<string> yLabels = null;

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            xLabels = chartLabels[0];
            yLabels = chartLabels[1];

            // Obtain grid
            if ((configDisplayPanel.Children == null) || (configDisplayPanel.Children.Count != 1) ||
                (!(configDisplayPanel.Children[0] is Grid)))
            {
                return;
            }
            Grid grid = configDisplayPanel.Children[0] as Grid;

            //Get any old heatmaps in the grid
            var visualizationsToBeRemoved = grid.Children.OfType<HeatMap>();
            //remove the old heatmaps from the grid
            visualizationsToBeRemoved.ToList().ForEach(x => grid.Children.Remove(x));

            heatmap.columnHeaders = xLabels;
            heatmap.rowHeaders = yLabels;

            heatmap.data = new double[xLabels.Count()][];

            for (int i = 0; i < xLabels.Count; i++)
            {
                if (GetConfigDisplayData(i, ref dataArray))
                {
                    heatmap.data[i] = dataArray;
                }
            }

            //regenerate heatmap canvas
            heatmap.generate();

            Grid.SetColumn(heatmap, 0);
            Grid.SetRow(heatmap, 2);
            grid.Children.Add(heatmap);
        }
    }
}
