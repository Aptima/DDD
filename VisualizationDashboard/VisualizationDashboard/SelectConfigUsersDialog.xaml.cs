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

namespace VisualizationDashboard
{
    /// <summary>
    /// Interaction logic for SelectConfigUsersDialog.xaml
    /// </summary>
    public partial class SelectConfigUsersDialog : Window
    {
        private ConfigDataModel ConfigDataModel { get { return DataContext as ConfigDataModel; } }

        private VisDashboardDataDataContext dbContext = null;
        TransactionScope ts = null;

        private List<UsersInConfig> pendingAdds = null;
        private List<int> pendingDeletes = null;

        public SelectConfigUsersDialog()
        {
            InitializeComponent();

            dbContext = new VisDashboardDataDataContext();
            ts = new TransactionScope();

            pendingAdds = new List<UsersInConfig>();
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
            //this.configNameTextBox.GetBindingConfigression(TextBox.TextProperty).UpdateSource();
        }

        private void AddUserB_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User) usersNotInConfigLB.SelectedItem;

            if (selectedUser == null)
            {
                return;
            }

            // Remove this user from usersNotInConfigLB
            ConfigDataModel.UsersNotInConfig.Remove(selectedUser);

            // Add this user to usersInConfigLB
            int i;

            for (i = 0; i < ConfigDataModel.UsersInConfig.Count; i++)
			{
			    if (string.Compare(ConfigDataModel.UsersInConfig[i].Username, selectedUser.Username) >= 0)
                {
                    ConfigDataModel.UsersInConfig.Insert(i, selectedUser);
                    break;
                }
			}
            if (i == ConfigDataModel.UsersInConfig.Count)
            {
                ConfigDataModel.UsersInConfig.Insert(i, selectedUser);
            }
 
            // Add user to config
            UsersInConfig newUserInConfig = new UsersInConfig { ConfigID = ConfigDataModel.CurConfig.ConfigID,
                    UserID= selectedUser.UserID};
            pendingAdds.Add(newUserInConfig);

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
            User selectedUser = (User)usersInConfigLB.SelectedItem;

            if (selectedUser == null)
            {
                return;
            }

            // Remove this user from usersInConfigLB
            ConfigDataModel.UsersInConfig.Remove(selectedUser);

            // Add this user to usersNotInConfigLB
            int i;

            for (i = 0; i < ConfigDataModel.UsersNotInConfig.Count; i++)
            {
                if (string.Compare(ConfigDataModel.UsersNotInConfig[i].Username, selectedUser.Username) >= 0)
                {
                    ConfigDataModel.UsersNotInConfig.Insert(i, selectedUser);
                    break;
                }
            }
            if (i == ConfigDataModel.UsersNotInConfig.Count)
            {
                ConfigDataModel.UsersNotInConfig.Insert(i, selectedUser);
            }

            // Remove user from config
            UsersInConfig [] userInConfig = (from uIe in dbContext.UsersInConfigs
                                                     where ((uIe.UserID == selectedUser.UserID) && (uIe.ConfigID == ConfigDataModel.CurConfig.ConfigID))
                                   select uIe).ToArray();

            if (userInConfig.Length == 1)
            {
                pendingDeletes.Add(userInConfig[0].UserID);

            }

            // Make sure this item is not listed on the pending add list
            foreach (UsersInConfig addObject in pendingAdds)
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
                foreach (UsersInConfig addObject in pendingAdds)
                {
                    // Make sure this add object does not already exist
                    UsersInConfig[] userInConfig = (from uIc in dbContext.UsersInConfigs
                                                            where ((uIc.UserID == addObject.UserID) && (uIc.ConfigID == ConfigDataModel.CurConfig.ConfigID))
                                                            select uIc).ToArray();

                    if (userInConfig.Length == 0)
                    {
                        dbContext.UsersInConfigs.InsertOnSubmit(addObject);
                    }
                }

                // Add pending deletes
                foreach (int deleteUserID in pendingDeletes)
                {
                    UsersInConfig[] userInConfig = (from uIe in dbContext.UsersInConfigs
                                                            where ((uIe.UserID == deleteUserID) && (uIe.ConfigID == ConfigDataModel.CurConfig.ConfigID))
                                                            select uIe).ToArray();
                    if (userInConfig.Length == 1)
                    {
                        DatabaseHelper.DeleteByPK<UsersInConfig, int>(userInConfig[0].ID, dbContext);                    
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
