using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DashboardDataAccess;

namespace VisualizationDashboard
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        private ConfigDataModel configDataModel = null;
        private List<StackPanel> configDisplayPanelList = null;
        private ObservableCollection<ConfigDisplay> configDisplays = null;
        private RTPMEConnector rtpmeConnection = null;
        public UpdateCallback measureUpdateCallBack;

        public ConfigDataModel ConfigDataModel
        {
            get { return configDataModel; }
            set { configDataModel = value; }
        }

        public RTPMEConnector RtpmeConnection
        {
            get { return rtpmeConnection; }
            set { rtpmeConnection = value; }
        }

        public delegate void UpdateCallback(object junk);
        
        public DisplayControl()
        {
            ChartDirector.Chart.setLicenseCode("DIST-0000-0536-4cc1-aec1");
            configDisplayPanelList = new List<StackPanel>();
            measureUpdateCallBack = new UpdateCallback(this.UpdateMeasures);

            InitializeComponent();

            ReloadConfigDisplays();
        }

        public void ReloadConfigDisplays()
        {
            if (this.ConfigDataModel != null)
            {
                configDisplays = this.ConfigDataModel.LoadConfigDisplays();
                displaysLB.DataContext = configDisplays;
            }
        }

        private void ConfigDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel configDisplayPanel = sender as StackPanel;

            if (configDisplayPanelList != null)
            {
                configDisplayPanelList.Add(configDisplayPanel);
            }

            int configDisplayID = int.Parse(configDisplayPanel.Tag.ToString());

            // Locate the configDisplay with the matching ID and initialize the visualization
            foreach (ConfigDisplay configDisplay in configDisplays)
            {
                if (configDisplay.ConfigDisplayID == configDisplayID)
                {
                    // Initialize a visualization
                    if (rtpmeConnection.dashboardVisualizationMap.Keys.Contains(configDisplayID))
                    {
                        DashboardVisualization dashboardVisualization = rtpmeConnection.dashboardVisualizationMap[configDisplayID];

                        dashboardVisualization.InitVisualization(configDisplayPanel);
                    }
                }
            }
        }

        private void ConfigDisplay_Unloaded(object sender, RoutedEventArgs e)
        {
            StackPanel configDisplayPanel = sender as StackPanel;

            if (configDisplayPanelList != null)
            {
                configDisplayPanelList.Remove(configDisplayPanel);
            }
        }

        private void UpdateMeasures(object junk)
        {
            // Loop through the configDisplay panels an call visualization update routines
            foreach (StackPanel configDisplayPanel in configDisplayPanelList)
            {
                int configDisplayID = int.Parse(configDisplayPanel.Tag.ToString());

                // Locate the configDisplay with the matching ID and update the visualization
                foreach (ConfigDisplay configDisplay in configDisplays)
                {
                    if (configDisplay.ConfigDisplayID == configDisplayID)
                    {
                        // Found visualization to draw
                        if (rtpmeConnection.dashboardVisualizationMap.Keys.Contains(configDisplayID))
                        {
                            DashboardVisualization dashboardVisualization = rtpmeConnection.dashboardVisualizationMap[configDisplayID];
                            dashboardVisualization.UpdateVisualization();
                        }
                    }
                }
            }
        }

        private void displaysLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                displaysLB.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
            }
        }    
    }
}
