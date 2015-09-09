using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Transactions;
using System.Data.SqlClient;

namespace DashboardDataAccess
{
    public class UserDataModel : INotifyPropertyChanged
    {
        private User currentUser = null;

        public User User
        {
            get {
                if (currentUser != null)
                {
                    return currentUser;
                }
                else
                {
                    return null;
                }
            }
        }

        public string RoleName
        {
            get {
                if (currentUser != null)
                {
                    if (currentUser.UserInRoles.Count == 1)
                    {
                        return currentUser.UserInRoles[0].Role.RoleName;
                    }
                }

                return null;
            }
        }

        public UserDataModel()
        {
            currentUser = new User();
            currentUser.Username = "Unknown";
        }

        public bool LoadUser(string username, string password)
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            User newUser = null;
            var query = from p in db.Users where p.Username == username select p;

            try
            {
                newUser = query.Single();
            }
            catch (Exception)
            {
                MessageBox.Show("Username or password is incorrect.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Check password
            if (string.Compare(password, newUser.Password) != 0)
            {
                MessageBox.Show("Username or password is incorrect.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if ((newUser.UserInRoles != null) && (newUser.UserInRoles.Count == 1))
            {
                if (newUser.UserInRoles[0].Role.RoleName.CompareTo("Administrator") == 0)
                {
                }
                else if (newUser.UserInRoles[0].Role.RoleName.CompareTo("Experimenter") == 0)
                {
                }
                else
                {
                    MessageBox.Show("Account must be either an Administrator or an Experimenter to use this interface.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                if (newUser.UserInRoles.Count > 1)
                {
                    MessageBox.Show("User account has more than one role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("User account has no assigned role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return false;
            }

            currentUser = newUser;
            OnPropertyChanged("");
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool AddNewUser(CreateUserInfo newUserInfo)
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    // Lookup the role ID
                    Role desiredRole = null;
                    var query = from p in db.Roles where p.RoleName == newUserInfo.UserInRole select p;

                    try
                    {
                        desiredRole = query.Single();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not process user role.", "User Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    // Try to add new user to database
                    User newUser = new User { Username = newUserInfo.Username, Password = newUserInfo.Password };
                    UserInRole newUserInRole = new UserInRole
                        {
                            ApplicationName = "DashboardPermissionTool",
                            RoleID = desiredRole.RoleID                            
                        };

                    newUserInRole.User = newUser;
                    db.UserInRoles.InsertOnSubmit(newUserInRole);
                    db.SubmitChanges();
                    ts.Complete();
                }
                catch (SqlException e)
                {
                    if (e.Message.Contains("Violation of UNIQUE KEY constraint 'U_Username'. Cannot insert duplicate key in object 'dbo.User'"))
                    {
                        MessageBox.Show("This user already exists.", "User Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Database Error: " + e.Message, "User Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return true;
        }
    }

    public class CreateUserInfo
    {
        private string _username = null;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
            }
        }

        private string _password = null;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }

        private string _userInRole = null;

        public string UserInRole
        {
            get { return _userInRole; }
            set { _userInRole = value; }
        }
    }

}
