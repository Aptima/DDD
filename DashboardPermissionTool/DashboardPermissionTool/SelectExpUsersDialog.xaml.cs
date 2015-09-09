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
using System.Windows.Shapes;

using System.Transactions;
using System.Data.Linq;

using DashboardDataAccess;

namespace DashboardPermissionTool
{
    /// <summary>
    /// Interaction logic for SelectExpUsersDialog.xaml
    /// </summary>
    public partial class SelectExpUsersDialog : Window
    {
        public object SelectedRole { get; set; }

        private ExperimentDataModel ExpDataModel { get { return DataContext as ExperimentDataModel; } }

        private VisDashboardDataDataContext dbContext = null;
        TransactionScope ts = null;

        private List<UsersInExperiment> pendingAdds = null;
        private List<int> pendingDeletes = null;

        public SelectExpUsersDialog()
        {
            InitializeComponent();

            dbContext = new VisDashboardDataDataContext();
            ts = new TransactionScope();

            pendingAdds = new List<UsersInExperiment>();
            pendingDeletes = new List<int>();
        }

        void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box canceled
            this.DialogResult = false;
        }

        void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Don't accept the dialog box if there is invalid data
            if (!IsValid(this)) return;

            // Dialog box accepted
            this.DialogResult = true;
        }

        // Validate all dependency objects in a window
        bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Force initial validation on controls
            //this.experimentNameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void AddUserB_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User) usersNotInExpLB.SelectedItem;

            if (selectedUser == null)
            {
                return;
            }

            // Remove this user from usersNotInExpLB
            ExpDataModel.UsersNotInExperiment.Remove(selectedUser);

            // Add this user to usersInExpLB
            int i;

            for (i = 0; i < ExpDataModel.UsersInExperiment.Count; i++)
			{
			    if (string.Compare(ExpDataModel.UsersInExperiment[i].Username, selectedUser.Username) >= 0)
                {
                    ExpDataModel.UsersInExperiment.Insert(i, selectedUser);
                    break;
                }
			}
            if (i == ExpDataModel.UsersInExperiment.Count)
            {
                ExpDataModel.UsersInExperiment.Insert(i, selectedUser);
            }
 
            // Add user to experiment
            UsersInExperiment newUserInExperiment = new UsersInExperiment { ExperimentID = ExpDataModel.CurExperiment.ExperimentID,
                    IsExperimentor= false, UserID= selectedUser.UserID, ActiveConfig = null};
           pendingAdds.Add(newUserInExperiment);

            // Make sure this item is not listed on the pending delete list
            foreach (int deleteUserID in pendingDeletes)
            {
                if (deleteUserID == selectedUser.UserID)
                {
                    pendingDeletes.Remove(deleteUserID);
                    break;
                }
            }

        }


        private void RemoveUserB_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)usersInExpLB.SelectedItem;

            if (selectedUser == null)
            {
                return;
            }

            // Remove this user from usersInExpLB
            ExpDataModel.UsersInExperiment.Remove(selectedUser);

            // Add this user to usersNotInExpLB
            int i;

            for (i = 0; i < ExpDataModel.UsersNotInExperiment.Count; i++)
            {
                if (string.Compare(ExpDataModel.UsersNotInExperiment[i].Username, selectedUser.Username) >= 0)
                {
                    ExpDataModel.UsersNotInExperiment.Insert(i, selectedUser);
                    break;
                }
            }
            if (i == ExpDataModel.UsersNotInExperiment.Count)
            {
                ExpDataModel.UsersNotInExperiment.Insert(i, selectedUser);
            }

            // Remove user from experiment
            UsersInExperiment [] userInExperiment = (from uIe in dbContext.UsersInExperiments
                                                     where ((uIe.UserID == selectedUser.UserID) && (uIe.ExperimentID == ExpDataModel.CurExperiment.ExperimentID))
                                   select uIe).ToArray();

            if (userInExperiment.Length == 1)
            {
                pendingDeletes.Add(userInExperiment[0].UserID);

            }

            // Make sure this item is not listed on the pending add list
            foreach (UsersInExperiment addObject in pendingAdds)
	        {
                if (addObject.UserID == selectedUser.UserID)
                {
                    pendingAdds.Remove(addObject);
                    break;
                }        		 
	        }
        }

        public void SaveDatabaseChanges()
        {
            if (dbContext != null)
            {
                // Add pending adds
                foreach (UsersInExperiment addObject in pendingAdds)
                {
                    // Make sure this add object does not already exist
                    UsersInExperiment[] userInExperiment = (from uIe in dbContext.UsersInExperiments
                                                            where ((uIe.UserID == addObject.UserID) && (uIe.ExperimentID == ExpDataModel.CurExperiment.ExperimentID))
                                                            select uIe).ToArray();

                    if (userInExperiment.Length == 0)
                    {
                        dbContext.UsersInExperiments.InsertOnSubmit(addObject);
                    }
                }

                // Add pending deletes
                foreach (int deleteUserID in pendingDeletes)
                {
                    UsersInExperiment[] userInExperiment = (from uIe in dbContext.UsersInExperiments
                                                            where ((uIe.UserID == deleteUserID) && (uIe.ExperimentID == ExpDataModel.CurExperiment.ExperimentID))
                                                            select uIe).ToArray();
                    if (userInExperiment.Length == 1)
                    {
                        DatabaseHelper.DeleteByPK<UsersInExperiment, int>(userInExperiment[0].ID, dbContext);                    
                    }
                }

                dbContext.SubmitChanges();
                ts.Complete();
                ts.Dispose();
                dbContext = null;
            }
        }
    }
}
