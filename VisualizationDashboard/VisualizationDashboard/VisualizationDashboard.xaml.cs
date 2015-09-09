using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using Microsoft.Expression.Interactivity.Core;

namespace VisualizationDashboard
{
    enum UserRole { Unknown, Administrator, Experimenter, Operator };

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private UserDataModel userDataModel = null;
        private ExperimentDataModel experimentDataModel = null;
        private ConfigDataModel configDataModel = null;
        private UserRole currentUserRole = UserRole.Unknown;
        private RTPMEConnector rtpmeConnection = null;

        public Window1()
        {
            InitializeComponent();

            // Create user data model
            userDataModel = new UserDataModel();
            userInfoGrid.DataContext = userDataModel;

            // Create experiment data model
            experimentDataModel = new ExperimentDataModel();
            experimentNameCB.DataContext = experimentDataModel;

            // Create config data model
            configDataModel = new ConfigDataModel();
            configNameCB.DataContext = configDataModel;
            measureNamesLB.DataContext = configDataModel;
            displayNamesLB.DataContext = configDataModel;
            displayBuilder.ConfigDataModel = configDataModel;
            displayPreview.ConfigDataModel = configDataModel;
            displayControl.ConfigDataModel = configDataModel;
            ConfigDisplayInfoControl.ConfigDataModel = configDataModel;
            displayBuilder.InitConfigDisplayInfo(configDataModel.CurConfigDisplay);


        }

        private void loginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            LoginDialog dlg = new LoginDialog();

            // Configure the dialog box
            dlg.Owner = this;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Attempt to login the user
                if (userDataModel.LoadUser(dlg.Username, dlg.Password))
                {
                    if ((userDataModel.User.UserInRoles != null) && (userDataModel.User.UserInRoles.Count == 1))
                    {
                        if (userDataModel.User.UserInRoles[0].Role.RoleName.CompareTo("Administrator") == 0)
                        {
                            currentUserRole = UserRole.Administrator;
                        }
                        else if (userDataModel.User.UserInRoles[0].Role.RoleName.CompareTo("Experimenter") == 0)
                        {
                            currentUserRole = UserRole.Experimenter;
                        }
                        else if (userDataModel.User.UserInRoles[0].Role.RoleName.CompareTo("Operator") == 0)
                        {
                            currentUserRole = UserRole.Experimenter;
                        }
                        else
                        {
                            return;
                        }
                        SetupViewByRole();
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SetupViewByRole()
        {
            if (currentUserRole == UserRole.Administrator)
            {
                SetupAdminView();
            }
            else if (currentUserRole == UserRole.Experimenter)
            {
                SetupExperimenterView();
            }
            else if (currentUserRole == UserRole.Operator)
            {
                SetupOperatorView();
            }
        }

        private void SetupAdminView()
        {
            // Menu Bar
            AdminMenu.Visibility = Visibility.Visible;
            ConfigurationMenu.Visibility = Visibility.Collapsed;
        }

        private void SetupExperimenterView()
        {
            // Menu Bar
            AdminMenu.Visibility = Visibility.Collapsed;
            ConfigurationMenu.Visibility = Visibility.Visible;

            experimentDataModel.LoadUserExperiments(userDataModel.User);

            if (experimentNameCB.Items.Count > 0)
            {
                experimentNameCB.SelectedIndex = 0;
            }
        }

        private void SetupOperatorView()
        {
            // Menu Bar
            AdminMenu.Visibility = Visibility.Collapsed;
            ConfigurationMenu.Visibility = Visibility.Visible;
        }

        private void experimentNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curExpIndex = experimentNameCB.SelectedIndex;

            // Save current experiment index
            experimentDataModel.CurExperimentIndex = curExpIndex;

            experimentDataModel.ReloadExperimentInfo();

            // Reload Configurations
            configNameCB.SelectedIndex = -1;
            configDataModel.LoadExperimentConfigs(experimentDataModel.CurExperiment);

            // Clear Display Builder
            configDataModel.ClearDisplayBuilder();

            if (configNameCB.Items.Count > 0)
            {
                configNameCB.SelectedIndex = 0;
            }




        }

        private void newConfMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create new configuration information
            CreateConfigInfo newConfigInfo = new CreateConfigInfo();

            if (experimentDataModel.CurExperiment == null)
            {
                return;
            }

            newConfigInfo.ExperimentID = experimentDataModel.CurExperiment.ExperimentID;

            // Instantiate the dialog box
            CreateConfigurationDialog dlg = new CreateConfigurationDialog();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.DataContext = newConfigInfo;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Try to add this new user to the database
                if (configDataModel.AddNewConfig(newConfigInfo))
                {
                    // Reload the Configurations
                    configDataModel.LoadExperimentConfigs(experimentDataModel.CurExperiment);
                }
            }
        }

        private void selectConfUsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            SelectConfigUsersDialog dlg = new SelectConfigUsersDialog();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.DataContext = configDataModel;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Save the changes
                dlg.SaveDatabaseChanges();

                // Reload the user list
                configDataModel.LoadConfigInfo();
            }
            else
            {
                // Reload the user list
                configDataModel.LoadConfigInfo();
            }
        }

        private void deleteConfMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void configNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curConfigIndex = configNameCB.SelectedIndex;

            // Save current experiment index
            configDataModel.CurConfigIndex = curConfigIndex;

            // Refresh the preview panel
            displayPreview.ReloadConfigDisplays();
        }

        private void MeasureSelectButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigDisplay configDisplay = configDataModel.CurConfigDisplay;
            RadioButton clickedRadioButton = null;

            if (!(sender is RadioButton))
            {
                return;
            }

            clickedRadioButton = sender as RadioButton;

            if (configDisplay == null)
            {
                clickedRadioButton.IsChecked = false;
                return;
            }

            // Clear out the current config
            configDisplay.Display = null;
            configDisplay.MetricName = null;

            configDisplay.DisplayFactors = null;
            configDisplay.DisplayBlockedFactors = null;

            configDataModel.ConfigFactorTable.Clear();
            configDataModel.ConfigBlockFactorTable.Clear();

            configDataModel.LoadConfigDisplayInfo();

            // Set the measure name in the config display
            configDisplay.MeasureName = (string) clickedRadioButton.Content;

            // Load available metrics
            configDataModel.LoadMetricDisplayInfo(configDisplay.MeasureName);
            displayBuilder.ResetMetricNameSelection();
        }

        private void DisplaySelectButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigDisplay configDisplay = configDataModel.CurConfigDisplay;
            RadioButton clickedRadioButton = null;
            int factorCount = 0;
            int blockedFactorCount = 0;

            if (!(sender is RadioButton)) 
            {
                return;
            }

            clickedRadioButton = sender as RadioButton;

            if ((configDisplay == null) || (configDisplay.MeasureName == null))
            {
                clickedRadioButton.IsChecked = false;
                configDataModel.LoadConfigDisplayInfo();
                return;
            }

            // Set the display name in the config display
            configDataModel.SelectDisplay((string)clickedRadioButton.Content);

            // Clear out the current config display
            configDisplay.MetricName = null;
            configDisplay.DisplayFactors = null;
            configDisplay.DisplayBlockedFactors = null;
            factorCount = configDataModel.ResetFactors(configDataModel.CurConfigDisplay.MeasureName,
                configDataModel.CurConfigDisplay.MetricName);
            blockedFactorCount = configDataModel.ResetBlockedFactors(configDataModel.CurConfigDisplay.MeasureName,
                configDataModel.CurConfigDisplay.MetricName, false);

            // Setup the conditions for a complete display (one ready to drag to
            // dashboard diagram
            configDataModel.CurConfigDisplay.NumFactors = factorCount;
            configDataModel.CurConfigDisplay.NumBlockedFactors = blockedFactorCount - factorCount;
        }

        private void LaunchB_Click(object sender, RoutedEventArgs e)
        {
            rtpmeConnection = new RTPMEConnector();
            displayControl.RtpmeConnection = rtpmeConnection;
            rtpmeConnection.DetermineVisDataReq(configDataModel);
            rtpmeConnection.ConnectToServer(displayControl);

            // Configure Display
            userInfoGrid.Visibility = Visibility.Collapsed;
            dashboardConfigGrid.Visibility = Visibility.Collapsed;
            displayControl.Visibility = Visibility.Visible;

            // Refresh the Display panel data
            displayControl.ReloadConfigDisplays();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (rtpmeConnection != null)
            {
                rtpmeConnection.DisconnectFromServer();
            }
        }

    }
}
