using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AME.Views.Forms
{
    public partial class MySqlSetupForm : Form
    {
        public String ServerHost
        {
            get
            {
                return tbServerHost.Text;
            }
        }

        public Int32 Port
        {
            get
            {
                return Convert.ToInt32(tbPort.Text);
            }
        }

        public String Database
        {
            get
            {
                return tbDatabase.Text;
            }
            set
            {
                tbDatabase.Text = value;
            }
        }

        public String Username
        {
            get
            {
                return tbUsername.Text;
            }
            set
            {
                tbUsername.Text = value;
            }
        }
        
        public String Password
        {
            get
            {
                return tbPassword.Text;
            }
            set
            {
                tbPassword.Text = value;
            }
        }

        public MySqlSetupForm()
        {
            InitializeComponent();
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SetupForm_Activated(object sender, EventArgs e)
        {
            tbPassword.Focus(); // password field focused on startup
        }
    }
}