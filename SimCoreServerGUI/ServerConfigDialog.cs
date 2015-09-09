using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class ServerConfigDialog : Form
    {
        public ServerConfigDialog()
        {
            InitializeComponent();

            ServerOptions.ReadFile();

            hostnameTextBox.Text = System.Net.Dns.GetHostName();
            portTextBox.Text = ServerOptions.PortNumber.ToString();

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int newPort = Int32.Parse(portTextBox.Text);
            ServerOptions.PortNumber = newPort;
            ServerOptions.WriteFile();
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Cancel;
            Application.Exit();
        }

        public String DialogMessage
        {
            set { messageLabel.Text = value; }
            get { return messageLabel.Text; }
        }

        
    }
}