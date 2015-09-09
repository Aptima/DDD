using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.TestStubs.TextChatTestGUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            string OFDXMLFilter = "XML File(*.xml)|*.xml";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = OFDXMLFilter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxSimModel.Text = ofd.FileName;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (textBoxHostName.Text == string.Empty ||
                textBoxPortNumber.Text == string.Empty ||
                textBoxSimModel.Text == string.Empty ||
                textBoxUserID.Text == string.Empty)
            {
                MessageBox.Show("All boxes not filled in!");
            }
            else
            { 
            //Continue
                Program.HostName = textBoxHostName.Text;
                Program.PortNumber = textBoxPortNumber.Text;
                Program.SimModelName = textBoxSimModel.Text;
                Program.User = textBoxUserID.Text;
                this.Close();
            }
        }
    }
}