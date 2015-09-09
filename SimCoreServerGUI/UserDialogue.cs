using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.UserTools;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class UserDialogue : Form
    {


        public UserDialogue()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Password confirmation didn't match password!");
                return;
            }


            this.DialogResult = DialogResult.OK;

        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            confirmPasswordTextBox.Text = "";
        }

        private void UserDialogue_Load(object sender, EventArgs e)
        {
            confirmPasswordTextBox.Text = passwordTextBox.Text;

            passwordTextBox.Enabled = true;
            confirmPasswordTextBox.Enabled = true;

        }
    }
}