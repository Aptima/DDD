using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DashboardDataAccess;
using System.Xml;

namespace VisualizationDashboard.Visualizations
{
    class StarSpider : DashboardVisualization
    {
        private Aptima.Visualization.StarSpider spider = new Aptima.Visualization.StarSpider();

        public StarSpider(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
            : base(configDisplay, configDataModel, measureInstXmlMap)
        {
            spider.center = new Point(200, 200);
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
                // Create a new RTPMEData structure to hold the data for this pie chart
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

        public override void InitVisualization(System.Windows.Controls.StackPanel configDisplayPanel)
        {
            this.configDisplayPanel = configDisplayPanel;

            SingleChartInit(configDisplayPanel);

            spider.center = new Point(200, 200);
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

            //Get any old colorhweels in the grid
            var visualizationsToBeRemoved = grid.Children.OfType<Aptima.Visualization.StarSpider>();
            //remove the old colorhweels from the grid
            visualizationsToBeRemoved.ToList().ForEach(x => grid.Children.Remove(x));

            for (int i = 0; i < xLabels.Count; i++)
            {
                if (GetConfigDisplayData(i, ref dataArray))
                {
                    //radar.slices = ??;
                    //spider.data = ??
                }
            }

            spider.generate();
            Grid.SetColumn(spider, 0);
            Grid.SetRow(spider, 0);
            grid.Children.Add(spider);
        }
    }
}
