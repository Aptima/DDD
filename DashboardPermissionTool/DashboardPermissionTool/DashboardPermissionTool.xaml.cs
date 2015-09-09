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
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Windows.Controls;
using DashboardPermissionTool;
using DashboardDataAccess;

namespace DashboardPermissionTool
{
    enum UserRole { Unknown, Administrator, Experimenter, Operator };

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private MeasuresDataModel measureDataModel = null;
        private DisplayDataModel displayDataModel = null;
        private UserDataModel userDataModel = null;
        private ExperimentDataModel experimentDataModel = null;
        private UserRole currentUserRole = UserRole.Unknown;

        public Window1()
        {
            InitializeComponent();

            // Clear the item source until the user login and an experiment is selected/created
            measuresDataGrid.AutoGenerateColumns = false;
            measuresDataGrid.ItemsSource = null;
            displayDataGrid.AutoGenerateColumns = false;
            displayDataGrid.ItemsSource = null;

            // Create user data model
            userDataModel = new UserDataModel();
            userInfoGrid.DataContext = userDataModel;

            // Create experiment data model
            experimentDataModel = new ExperimentDataModel();
            experimentNameCB.DataContext = experimentDataModel;

            // Setup the experiment tab
            experimentTab.DataContext = experimentDataModel;

        }

        private void measureTreeClicked(object sender, RoutedEventArgs e)
        {
            string groupName = null;

            // Need to expand or hide the relavent group
            if (measuresDataGrid == null)
            {
                return;
            }

            MeasureSelection content = (MeasureSelection)measuresDataGrid.CurrentItem;

            if (content.MeasureRowType == MeasureRowType.DataValue)
            {
                return;
            }

            groupName = content.MeasureName;
            content.ExpandedFlag = !content.ExpandedFlag;

            // Loop through all the items and set the visibility correctly
            foreach (MeasureSelection item in measureDataModel.GetAllData())
            {
                if ((content.MeasureRowType == MeasureRowType.CategoryType) && (item.MeasureRowType == MeasureRowType.DataValue) &&
                    (string.Compare(item.Category, groupName) == 0))
                {
                    item.VisibilityMain = content.ExpandedFlag;
                }
                if ((content.MeasureRowType == MeasureRowType.SubCategoryType) && (item.MeasureRowType == MeasureRowType.DataValue) &&
                    (string.Compare(item.SubCategory, groupName) == 0))
                {
                    item.VisibilitySub = content.ExpandedFlag;
                }
                if ((content.MeasureRowType == item.MeasureRowType) && (string.Compare(groupName, item.MeasureName) == 0))
                {
                    item.ExpandedFlag = content.ExpandedFlag;
                }
            }

            // Update the visible measure collection
            measureDataModel.UpdateVisMeasureCollection();

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }

            TabItem ti = (TabItem)experimentTabControl.ItemContainerGenerator.ContainerFromIndex(experimentTabControl.SelectedIndex);
            if (ti.Name == "displaysTab")
            {
                DisplaysTabRadioButton_Click(sender, e);
                return;
            }

            Button button = (Button)sender;
            string operatorName = (string)button.Tag;
            MeasureSelection content = (MeasureSelection)measuresDataGrid.CurrentItem;
            bool allOperator = (string.Compare(operatorName, "All") == 0);
            bool isCategory = (content.MeasureRowType == MeasureRowType.CategoryType);
            bool isSubCategory = (content.MeasureRowType == MeasureRowType.SubCategoryType);
            RadioButtonType newValue = RadioButtonType.Off;
            RadioButtonType currentValue = RadioButtonType.Off;
            bool isOperator = operatorName.Contains("Operator");
            int operatorIndex = 0;

            if (string.Compare("All", operatorName) == 0)
            {
                currentValue = content.All;
            }
            else if (isOperator)
            {
                operatorIndex = int.Parse(operatorName.Replace("Operator", "")) - 1;
                currentValue = content[operatorIndex];
            }
            else
            {
                return;
            }

            if ((currentValue == RadioButtonType.Off) || (currentValue == RadioButtonType.Partial))
            {
                newValue = RadioButtonType.On;
            }
            else
            {
                newValue = RadioButtonType.Off;
            }

            if (content.MeasureRowType == MeasureRowType.CategoryType)
            {
                string categoryName = content.MeasureName;

                for (int j = 0; j < measureDataModel.Count; j++)
                {
                    if ((measureDataModel[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(categoryName, measureDataModel[j].Category) == 0))
                    {
                        if (string.Compare("All", operatorName) == 0)
                        {
                            measureDataModel[j].All = newValue;

                            for (int i = 0; i < measureDataModel.NumOperators; i++)
                            {
                                measureDataModel[j][i] = newValue;
                            }
                        }
                        else if (isOperator)
                        {
                            measureDataModel[j][operatorIndex] = newValue;
                        }
                    }
                }
            }
            else if (content.MeasureRowType == MeasureRowType.SubCategoryType)
            {
                string subCategoryName = content.MeasureName;

                for (int j = 0; j < measureDataModel.Count; j++)
                {
                    if ((measureDataModel[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(subCategoryName, measureDataModel[j].SubCategory) == 0))
                    {
                        if (string.Compare("All", operatorName) == 0)
                        {
                            measureDataModel[j].All = newValue;

                            for (int i = 0; i < measureDataModel.NumOperators; i++)
                            {
                                measureDataModel[j][i] = newValue;
                            }
                        }
                        else if (isOperator)
                        {
                            measureDataModel[j][operatorIndex] = newValue;
                        }
                    }
                }
            }
            else if (content.MeasureRowType == MeasureRowType.DataValue)
            {
                if (string.Compare("All", operatorName) == 0)
                {
                    content.All = newValue;

                    for (int i = 0; i < measureDataModel.NumOperators; i++)
                    {
                        content[i] = newValue;
                    }
                }
                else if (isOperator)
                {
                    content[operatorIndex] = newValue;
                }
            }

            // Recalculate all rollups
            measureDataModel.RefreshAllCol();

            // Recalculate Subcategory rollups
            measureDataModel.RefreshSubCategory();

            // Recalculated Category rollups
            measureDataModel.RefreshCategory();

        }

        private void DisplaysTabRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }

            Button button = (Button)sender;
            string operatorName = (string)button.Tag;
            DisplaySelection content = (DisplaySelection)displayDataGrid.CurrentItem;
            bool allOperator = (string.Compare(operatorName, "All") == 0);
            RadioButtonType newValue = RadioButtonType.Off;
            RadioButtonType currentValue = RadioButtonType.Off;
            bool isOperator = operatorName.Contains("Operator");
            int operatorIndex = 0;

            if (string.Compare("All", operatorName) == 0)
            {
                currentValue = content.All;
            }
            else if (isOperator)
            {
                operatorIndex = int.Parse(operatorName.Replace("Operator", "")) - 1;
                currentValue = content[operatorIndex];
            }
            else
            {
                return;
            }

            if ((currentValue == RadioButtonType.Off) || (currentValue == RadioButtonType.Partial))
            {
                newValue = RadioButtonType.On;
            }
            else
            {
                newValue = RadioButtonType.Off;
            }

            if (allOperator)
            {
                content.All = newValue;

                for (int i = 0; i < displayDataModel.NumOperators; i++)
                {
                    content[i] = newValue;
                }
            }
            else if (isOperator)
            {
                content[operatorIndex] = newValue;
            }

            // Recalculate all rollups
            displayDataModel.RefreshAllCol();


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
        }

        private void SetupAdminView()
        {
            // Menu Bar
            AdminMenu.Visibility = Visibility.Visible;
            ExperimentMenu.Visibility = Visibility.Collapsed;
        }

        private void SetupExperimenterView()
        {
            // Menu Bar
            AdminMenu.Visibility = Visibility.Collapsed;
            ExperimentMenu.Visibility = Visibility.Visible;

            experimentDataModel.LoadUserExperiments(userDataModel.User);
            if (experimentNameCB.Items.Count > 0)
            {
                experimentNameCB.SelectedIndex = 0;
            }
        }

        private void newExpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create new experiment information
            CreateExperimentInfo newExperimentInfo = new CreateExperimentInfo();
            newExperimentInfo.CreatorID = userDataModel.User.UserID;

            // Instantiate the dialog box
            CreateExperimentDialog dlg = new CreateExperimentDialog();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.DataContext = newExperimentInfo;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Try to add this new user to the database
                if (experimentDataModel.AddNewExperiment(newExperimentInfo))
                {
                    // Reload the experiments
                    experimentDataModel.LoadUserExperiments(userDataModel.User);
                }
            }
        }

        private void deleteExpMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void addUserMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create new user information
            CreateUserInfo newUserInfo = new CreateUserInfo();

            // Instantiate the dialog box
            CreateUserDialog dlg = new CreateUserDialog();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.DataContext = newUserInfo;
            newUserInfo.Username = "";

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Try to add this new user to the database
                if (userDataModel.AddNewUser(newUserInfo))
                {
                }
                else
                {
                }
            }
        }

        private void experimentNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curExpIndex = experimentNameCB.SelectedIndex;

            // Save current experiment index
            experimentDataModel.CurExperimentIndex = curExpIndex;

            experimentDataModel.ReloadExperimentInfo();

            ReloadExperimentModels();
        }

        private void ReloadExperimentModels()
        {
            string curExpName = (string)experimentNameCB.SelectedItem;
            var myRadioButtonTemplate = this.Resources["myRadioButtonAll"] as DataTemplate;
            int numColumns = 0;

            // Clear all but the first (tree) column
            numColumns = measuresDataGrid.Columns.Count;
            for (int i = 1; i < numColumns; i++)
            {
                measuresDataGrid.Columns.RemoveAt(1);
            }
            for (int i = 1; i < numColumns; i++)
            {
                displayDataGrid.Columns.RemoveAt(1);
            }

            // Get the list of experiment users
            string[] experimentUsers = experimentDataModel.CurExperimentUsers;
            if ((experimentUsers == null) || (experimentUsers.Length <= 0))
            {
                measureDataModel = null;
                return;
            }

            // Reload the measure tab
            measureDataModel = new MeasuresDataModel(experimentUsers.Length);
            measureDataModel.LoadExperiementMeasures(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);

            // Create column templates
            DataGridTemplateColumn templateColumn = new DataGridTemplateColumn { Header = "All", Width = 100 };
            templateColumn.CellTemplate = myRadioButtonTemplate;
            measuresDataGrid.Columns.Add(templateColumn);

            for (int i = 0; i < measureDataModel.NumOperators; i++)
            {
                String templateName = "Operator" + (i + 1).ToString();
                myRadioButtonTemplate = this.Resources["myRadioButton" + templateName] as DataTemplate;
                templateColumn = new DataGridTemplateColumn { Header = experimentUsers[i], Width = 100 };
                measuresDataGrid.Columns.Add(templateColumn);
                templateColumn.CellTemplate = myRadioButtonTemplate;
            }

            // Set item source (don't use static one created in XAML
            measuresDataGrid.ItemsSource = measureDataModel;


            // Reload the display tab
            displayDataModel = new DisplayDataModel(experimentUsers.Length);
            displayDataModel.LoadExperiementDisplays(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);

            // Create column templates
            templateColumn = new DataGridTemplateColumn { Header = "All", Width = 100 };
            myRadioButtonTemplate = this.Resources["myRadioButtonAll"] as DataTemplate;
            templateColumn.CellTemplate = myRadioButtonTemplate;
            displayDataGrid.Columns.Add(templateColumn);

            for (int i = 0; i < displayDataModel.NumOperators; i++)
            {
                String templateName = "Operator" + (i + 1).ToString();
                myRadioButtonTemplate = this.Resources["myRadioButton" + templateName] as DataTemplate;
                templateColumn = new DataGridTemplateColumn { Header = experimentUsers[i], Width = 100 };
                displayDataGrid.Columns.Add(templateColumn);
                templateColumn.CellTemplate = myRadioButtonTemplate;
            }

            // Set item source (don't use static one created in XAML
            displayDataGrid.ItemsSource = displayDataModel;

        }

        private void selectExpUsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            SelectExpUsersDialog dlg = new SelectExpUsersDialog();

            // Configure the dialog box
            dlg.Owner = this;
            dlg.DataContext = experimentDataModel;

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Save the changes
                dlg.SaveDatabaseChanges();

                // Reload the user list
                experimentDataModel.ReloadExperimentInfo();

                // Reload the experiment models
                ReloadExperimentModels();
            }
            else
            {
                // Reload the user list
                experimentDataModel.ReloadExperimentInfo();
            }
        }

        private void SaveMeasuresB_Click(object sender, RoutedEventArgs e)
        {
            measureDataModel.SaveExperiementMeasures(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void reloadMeasuresB_Click(object sender, RoutedEventArgs e)
        {
            measureDataModel.LoadExperiementMeasures(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void resetMeasuresB_Click(object sender, RoutedEventArgs e)
        {
            measureDataModel.ResetExperiementMeasures(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void SaveDisplaysB_Click(object sender, RoutedEventArgs e)
        {
            displayDataModel.SaveExperiementDisplays(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void ReloadDisplaysB_Click(object sender, RoutedEventArgs e)
        {
            displayDataModel.LoadExperiementDisplays(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void ResetDisplaysB_Click(object sender, RoutedEventArgs e)
        {
            displayDataModel.ResetExperiementDisplays(experimentDataModel.CurExperiment,
                experimentDataModel.UsersInExperiment);
        }

        private void scenarioPathBrowseB_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "\\\\dhoward\\DDDClient\\CoVEDemo2009Nov12.xml"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "DDD Scenario Files (.xml)|*.xml"; // Filter files by extension

            if ((experimentDataModel == null) || (experimentDataModel.CurExperiment == null))
            {
                return;
            }

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == false)
            {
                return;
            }

            experimentDataModel.CurExperiment.ScenarioFilePath = dlg.FileName;
            experimentDataModel.CurExperiment.ScenarioFileType = "DDD Scenario";

            experimentDataModel.SaveScenarioFileInfo();
        }

        private void scanScenarioB_Click(object sender, RoutedEventArgs e)
        {
            if (experimentDataModel.CurExperiment.ScenarioFileType.CompareTo("DDD Scenario") == 0)
            {
                experimentDataModel.ScanDDDScenario(measureDataModel);
            }
        }
    }

}
