using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DashboardDataAccess;

namespace VisualizationDashboard
{
    /// <summary>
    /// Interaction logic for DisplayBuilderControl.xaml
    /// </summary>
    public partial class DisplayBuilderControl : UserControl
    {
        private ConfigDataModel configDataModel = null;
        private List<ComboBox> factorCBList = null;
        private List<ComboBox> blockedFactorCBList = null;

        public ConfigDataModel ConfigDataModel
        {
          get { return configDataModel; }
          set { configDataModel = value;
              DataContext = configDataModel;
          }
        }

        public DisplayBuilderControl()
        {
            factorCBList = new List<ComboBox>();
            blockedFactorCBList = new List<ComboBox>();

            InitializeComponent();

        }

        private void metricNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (metricNameCB.SelectedIndex > 0)
            {
                ConfigDataModel.CurConfigDisplay.MetricName = (string) metricNameCB.SelectedValue;
            }
        }

        public void ResetMetricNameSelection()
        {
            metricNameCB.SelectedIndex = 0;
        }

        private bool handleSelection = true;

        private void factorCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems.Count == 1) && (handleSelection))
            {
                // Make sure this factor is not already selected
                // Need to find the comb box that already has this factor set
                ComboBox combo = null;

                foreach (ComboBox cb in factorCBList)
                {
                    if ((cb != (sender as ComboBox)) &&
                        (cb.SelectedValue != null) &&
                        (cb.SelectedValue.ToString().CompareTo(e.AddedItems[0].ToString()) == 0))
                    {
                        combo = cb;
                        break;
                    }
                }

                if (combo != null)
                {
                    handleSelection = false;
                    combo.SelectedItem = null;
                }
            }

            handleSelection = true;
            ComboBox factorCB = sender as ComboBox;

            // Add the selected factor to the currect display config
            if (e.AddedItems.Count == 1)
            {
                ConfigDataModel.AddConfigFactor((int)factorCB.Tag, e.AddedItems[0].ToString());
            }

            // Remove a factor from the currect display config if neccessary
            if (e.RemovedItems.Count == 1)
            {
                ConfigDataModel.RemoveConfigFactor((int)factorCB.Tag, e.RemovedItems[0].ToString());
            }

            // Populate these changes to the Blocked factors list
            configDataModel.ResetBlockedFactors(configDataModel.CurConfigDisplay.MeasureName,
                configDataModel.CurConfigDisplay.MetricName, true);

            // Update Config Display Info Control
            configDisplayInfo.RefreshDisplay();

        }

        private void factorCB_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox factorCB = sender as ComboBox;

            if (factorCBList != null)
            {
                factorCBList.Add(factorCB);
            }
        }

        private void factorCB_Unloaded(object sender, RoutedEventArgs e)
        {
            ComboBox factorCB = sender as ComboBox;

            if (factorCBList != null)
            {
                factorCBList.Remove(factorCB);
            }
        }

        public void InitConfigDisplayInfo(ConfigDisplay configDisplay)
        {
            configDisplayInfo.ConfigDisplayData = configDisplay;
        }

        private void blockedFactorLevelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox blockedFactorCB = sender as ComboBox;

            // Add the selected factor to the currect display config
            if (e.AddedItems.Count == 1)
            {
                BlockedFactorItem item = (BlockedFactorItem) blockedFactorCB.DataContext;
                ConfigDataModel.AddConfigBlockedFactor(blockedFactorCB.Tag.ToString(), e.AddedItems[0].ToString(), item.MeasureID);
            }

            // Remove a factor from the currect display config if neccessary
            if (e.RemovedItems.Count == 1)
            {
                ConfigDataModel.RemoveConfigBlockedFactor(blockedFactorCB.Tag.ToString(), e.RemovedItems[0].ToString());
            }

            // Update Config Display Info Control
            configDisplayInfo.RefreshDisplay();

        }

        private void blockedFactorLevelCB_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox blockedFactorCB = sender as ComboBox;

            if (blockedFactorCBList != null)
            {
                blockedFactorCBList.Add(blockedFactorCB);
            }
        }

        private void blockedFactorLevelCB_Unloaded(object sender, RoutedEventArgs e)
        {
            ComboBox blockedFactorCB = sender as ComboBox;

            if (blockedFactorCBList != null)
            {
                blockedFactorCBList.Remove(blockedFactorCB);
            }

        }
    }
}
