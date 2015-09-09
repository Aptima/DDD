using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.UserTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;
using Aptima.Asim.DDD.CommonComponents.PasswordHashing;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class UserAdministration : Form
    {

        static string passwordRulesString = "Password does not meet strong password requirements:\nMinimum of ten characters\n2+ numbers\n2+ uppercase letters\n2+ lowercase letters\n2+ punctuation\nCannot re-use the last 10 passwords";

        //List<User> users;
        public UserAdministration()
        {
            InitializeComponent();
            LoadUserList();

            strongPasswordCheckBox.Checked = ServerOptions.UseStrongPasswords;
            strongPasswordCheckBox.Enabled = true;

        }



        private void LoadUserList()
        {
            userList.Items.Clear();

            Authenticator.LoadUserFile();

            List<User> users = Authenticator.GetUsers();
            foreach (User u in users)
            {
                userList.Items.Add(u);
            }

            SetButtons();

        }

        private void SetButtons()
        {

            if (userList.Items.Count > 0 && userList.SelectedItem != null)
            {
                editButton.Enabled = true;
                deleteUserButton.Enabled = true;
            }
            else
            {
                editButton.Enabled = false;
                deleteUserButton.Enabled = false;
            }
        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {
            User selected = (User)userList.SelectedItem;
            SetButtons();
        }

        private void EditUser()
        {
            User selected = (User)userList.SelectedItem;

            UserDialogue userDialog = new UserDialogue();
            userDialog.Text = "Edit User";
            userDialog.userTextBox.Text = selected.Username;
            userDialog.userTextBox.Enabled = false;
            string oldPassword = selected.Password;
            userDialog.passwordTextBox.Text = oldPassword;
            userDialog.ShowDialog();

            if (userDialog.DialogResult == DialogResult.OK)
            {
                if (userDialog.passwordTextBox.Text == oldPassword)
                {
                    // do nothing
                }
                else if (ServerOptions.UseStrongPasswords)
                {
                    if (Authenticator.IsStrongPassword(selected, userDialog.passwordTextBox.Text))
                    {
                        selected.Password = PasswordHashUtility.HashPassword(userDialog.passwordTextBox.Text);

                    }
                    else
                    {
                        MessageBox.Show(passwordRulesString);
                    }
                }
                else
                {
                    selected.Password = PasswordHashUtility.HashPassword(userDialog.passwordTextBox.Text);                    
                }

            }
            SetButtons();
            //ud.Show();
        }

        private void userList_DoubleClick(object sender, EventArgs e)
        {
            EditUser();
        }

        private void saveUsers()
        {
            List<User> ul = new List<User>();
            foreach (object i in userList.Items)
            {
                ul.Add((User)i);
            }

            Authenticator.SetUsers(ul);
            Authenticator.WriteUserFile();

            ServerOptions.UseStrongPasswords = strongPasswordCheckBox.Checked;

        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            // saveUsers will be called by form closing function.
            this.Close();
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            User selected = (User)userList.SelectedItem;
            userList.Items.Remove(selected);
            SetButtons();
        }

        private void NewUser()
        {
            UserDialogue userDialog = new UserDialogue();
            userDialog.Text = "New User";
            userDialog.userTextBox.Text = "";
            userDialog.userTextBox.Enabled = true;
            userDialog.passwordTextBox.Text = "";
            userDialog.ShowDialog();
            if (userDialog.DialogResult == DialogResult.OK)
            {
                if (userDialog.userTextBox.Text != "")
                {
                    bool add = true;
                    foreach (User u in userList.Items)
                    {
                        if (userDialog.userTextBox.Text == u.Username)
                        {
                            MessageBox.Show("Username: \"" + u.Username + "\" already taken.");
                            add = false;
                            break;
                        }
                    }
                    if (add)
                    {
                        User u2 = new User(userDialog.userTextBox.Text);
                        if (ServerOptions.UseStrongPasswords)
                        {

                            if (Authenticator.IsStrongPassword(u2, userDialog.passwordTextBox.Text))
                            {
                                u2.Password = PasswordHashUtility.HashPassword(userDialog.passwordTextBox.Text);

                                userList.Items.Add(u2);

                            }
                            else
                            {
                                MessageBox.Show(passwordRulesString);
                            }
                        }
                        else
                        {
                            u2.Password = PasswordHashUtility.HashPassword(userDialog.passwordTextBox.Text);
                            
                            userList.Items.Add(u2);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username");
                }

            }
            SetButtons();
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            NewUser();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            EditUser();
        }

        private void strongPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ServerOptions.UseStrongPasswords = strongPasswordCheckBox.Checked;
        }

        private void UserAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveUsers();
        }

        public static int GetUserCount()
        {
            Authenticator.LoadUserFile();

            List<User> users = Authenticator.GetUsers();

            if (users == null)
            {
                return 0;
            }

            return users.Count;
        }

    }
}