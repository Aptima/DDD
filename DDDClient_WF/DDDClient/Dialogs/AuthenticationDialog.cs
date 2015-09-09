using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.UserTools;
using Aptima.Asim.DDD.Client.Controller;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class AuthenticationDialog : Form
    {
        public string Username = string.Empty;
        public string Password = string.Empty;
        private ICommand _controller = null;

        public AuthenticationDialog(ICommand controller)
        {
            InitializeComponent();
            _controller = controller;
            Text = Program.App_Name;
            //textBox2.Enabled = Authenticator.EnablePassword;
            textBox2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Username = textBox1.Text;
            Password = textBox2.Text;
            _controller.AuthenticationRequest(Username, Password, DDD_Global.Instance.TerminalID);

            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Username = Password = string.Empty;
            DialogResult = DialogResult.Cancel;
        }

        private void AuthenticationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Username = Password = string.Empty;
        }

        private void AuthenticationDialog_Load(object sender, EventArgs e)
        {
            this.SelectNextControl(button2, true, false, false, true);
        }
    }
}