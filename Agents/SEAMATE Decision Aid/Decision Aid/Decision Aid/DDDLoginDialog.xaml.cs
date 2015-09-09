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

namespace Decision_Aid
{
    /// <summary>
    /// Interaction logic for DMSelectorDialog.xaml
    /// </summary>
    public partial class DDDLoginDialog : Window
    {
        private String _selectedUserid = "";
        private String _selectedTeamid = "";
        private String _selectedHostname = "";
        private String _selectedPort = "";
        private String _sharePath = "";
        public String SharePath
        {
            get { return _sharePath; }
        }
        public String SelectedHostname
        {
            get { return _selectedHostname; }
        }
        public String SelectedPort
        {
            get { return _selectedPort; }
        }
        public String SelectedUserId
        {
            get { return _selectedUserid; }
        }
        public String SelectedTeamId
        {
            get { return _selectedTeamid; }
        }
        public DDDLoginDialog()
        {
            InitializeComponent();
            tbHostname.Text = GetHostname();
        }
        public DDDLoginDialog(String startHostname, String startPort, String sharePath)
        {
            InitializeComponent();
            tbHostname.Text = startHostname;
            tbPort.Text = startPort;
            tbSharePath.Text = sharePath;
        }
        private String GetHostname()
        {
            String h = "host";
            try
            {
                h = System.Net.Dns.GetHostName();
            }
            catch (Exception ex)
            { }
            return h;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            int r;
            if (!Int32.TryParse(tbPort.Text, out r))
            {
                MessageBox.Show("Port needs to be a number.");
                return;
            }
            if (tbHostname.Text.Trim() == "")
            {
                MessageBox.Show("Hostname is empty.");
                return;
            }
            if (tbSharePath.Text.Trim() == "")
            {
                MessageBox.Show("Share folder path is empty.");
                return;
            }
            if (tbUserid.Text.Trim() == "")
            {
                MessageBox.Show("User ID is empty.");
                return;
            }
            if (tbTeamid.Text.Trim() == "")
            {
                MessageBox.Show("Team ID is empty.");
                return;
            }
            _sharePath = tbSharePath.Text;
            _selectedHostname = tbHostname.Text;
            _selectedPort = tbPort.Text;
            _selectedUserid = tbUserid.Text;
            _selectedTeamid = tbTeamid.Text;
            DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

    }
}
