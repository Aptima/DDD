using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class ConnectDialog : Form
    {

        private string _hostname = string.Empty;
        private string _clientSharePath = String.Empty;
        private string _playername = string.Empty;

        private int _port = 0;

        public ConnectDialog()
        {
            InitializeComponent();
            Text = Program.App_Name;
            comboBox1.Text = System.Environment.MachineName;
            _clientSharePath = textBox1.Text;
            _hostname = comboBox1.Text;
            _port = (int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.OK;
            _clientSharePath = textBox1.Text;
            _clientSharePath = _clientSharePath.Replace('/', '\\'); //make sure they don't do anything foolish
            if (comboBox1.SelectedIndex == 0)
            {
                // If user chooses "demo" from dropdown.
                _hostname = System.Environment.MachineName;
                
                DDD_Global.Instance.HostName = _hostname;
                DDD_Global.Instance.ClientPath = _clientSharePath;

                return;
            }

            // Validate Host Name;
            if (_hostname != string.Empty)
            {
                try
                {
                    if (DDD_Global.Instance.Connect(_hostname, _port))
                    {
                        comboBox1.ForeColor = SystemColors.ControlText;
                        DDD_Global.Instance.HostName = _hostname;
                        DDD_Global.Instance.Port = _port;
                        DDD_Global.Instance.ClientPath = _clientSharePath;
                    }
                    else
                    {
                        DialogResult = DialogResult.Retry;
                        MessageBox.Show("Cannot connect using current Hostname and/or Port", "Connect Error");
                        return;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message + ":" + exc.StackTrace);
                }
            }

            // Validate Player Name
            if (comboBox1.Text == string.Empty)
            {
                DialogResult = DialogResult.Retry;
            }


        }





        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _port = (int)numericUpDown1.Value;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _hostname = comboBox1.Text;
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            _hostname = comboBox1.Text;
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            if (((TextBox)sender).ReadOnly)
            {
                ((TextBox)sender).ReadOnly = false;
            }
        }
    }
}