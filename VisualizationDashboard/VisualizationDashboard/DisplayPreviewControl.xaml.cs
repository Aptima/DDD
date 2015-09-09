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
    /// Interaction logic for DisplayPreviewControl.xaml
    /// </summary>
    public partial class DisplayPreviewControl : UserControl
    {

        private ConfigDataModel configDataModel = null;

        public ConfigDataModel ConfigDataModel
        {
            get { return configDataModel; }
            set { configDataModel = value; }
        }


        public DisplayPreviewControl()
        {
            InitializeComponent();

            ReloadConfigDisplays();
        }

        private void DisplayPreview_Drop(object sender, DragEventArgs e)
        {
            // Add the current display builder info into the database for this config
            if (ConfigDataModel != null)
            {
                this.ConfigDataModel.RemoveCurrentDisplay();

                if (this.ConfigDataModel.AddCurrentDisplay())
                {
                    configDataModel.LoadConfigInfo();
                    ReloadConfigDisplays();
                }
            }

        }

        public void ReloadConfigDisplays()
        {
            if (this.ConfigDataModel != null)
            {
                displaysLB.DataContext = this.ConfigDataModel.LoadConfigDisplays();
            }
        }

        private void displaysLB_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is ListBox))
            {
                return;
            }

            ListBox listBox = sender as ListBox;

            foreach (ConfigDisplay configDisplay in listBox.SelectedItems)
            {
                configDataModel.DeleteConfigDisplay(configDisplay);
            }

            ReloadConfigDisplays();
        }
    }
}
