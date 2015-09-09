using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.DDDAgentFramework;

namespace Aptima.Asim.DDD.DDDAgentFramework.UIHelpers
{
    public partial class ServerConnectDialog : Form
    {
        String serverHostname;

        public String ServerHostname
        {
            get { return serverHostname; }
            set { serverHostname = value; }
        }

        DDDServerConnection m_serverConnection = null;
        public ServerConnectDialog(ref DDDServerConnection serverConnection)
        {
            try
            {
                ServerHostname = System.Net.Dns.GetHostName();
            }
            catch
            {

            }
            InitializeComponent();
            m_serverConnection = serverConnection;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (m_serverConnection.IsConnected())
            {
                MessageBox.Show("Already connected!");
                DialogResult = DialogResult.OK;
                
            }
            if (!m_serverConnection.ConnectToServer(serverHostnameTextBox.Text, Int32.Parse(serverPortNumberTextBox.Text)))
            {
                MessageBox.Show(String.Format("Unable to connect to {0}:{1}", serverHostnameTextBox.Text, serverPortNumberTextBox.Text));
                return;
            }
            m_serverConnection.DDDClientPath = String.Format("\\\\{0}\\DDDClient", serverHostnameTextBox.Text);
            //String path = String.Format("\\\\{0}\\DDDClient\\SimulationModel.xml", serverHostnameTextBox.Text);
            if (!m_serverConnection.ReadSimModel())
            {
                MessageBox.Show(String.Format("Unable to read sim model"));
                return;
            }
            ServerHostname = serverHostnameTextBox.Text;
            DialogResult = DialogResult.OK;
            //LoginDialog loginDialog = new LoginDialog(serverConnection);
            //Hide();
            //loginDialog.Show();
        }

        private void ServerConnectDialog_Load(object sender, EventArgs e)
        {
            serverHostnameTextBox.Text = ServerHostname;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}