using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class DialogSetLogOptions : Form
    {
        private static string SFDTXTFilter = "TXT File(*.txt)|*.txt";
        private static SaveFileDialog sfd = new SaveFileDialog();
        private static FolderBrowserDialog fbd = new FolderBrowserDialog();

        public DialogSetLogOptions()
        {
            InitializeComponent();
            fbd.SelectedPath = Form1.LogPath;
            //sfd.InitialDirectory = Form1.ApplicationPath;
            SetBoxesUp();

        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            //sfd.Filter = SFDTXTFilter;
            //if (File.Exists(textBox1.Text))
            //{ 
            //    sfd.InitialDirectory = textBox1.Text.Remove(textBox1.Text.LastIndexOf('\\'));
            //}
            //else
            //{
            //    sfd.InitialDirectory = Form1.ApplicationPath;
            //}
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    textBox1.Text = sfd.FileName;
            //}
            if (Directory.Exists(textBox1.Text))
            {
                fbd.SelectedPath = textBox1.Text;
            }
            else
            {
                fbd.SelectedPath = Form1.LogPath;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("Invalid Event Log Path");
                return; //cancel
                //Form1.LoggingType = SimCoreServer.SimCoreServer.LogType.NOLOG;
            }
            Form1.LogPath = textBox1.Text;
            if (radioButtonDetailed.Checked)
            {
                Form1.LoggingType = SimCoreServer.SimCoreServer.LogType.DETAILED;
            }
            else
            {
                Form1.LoggingType = SimCoreServer.SimCoreServer.LogType.LIMITED;
            }
            
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SetBoxesUp();
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            //Form1.LogPath = String.Empty;
            //textBox1.Text = String.Empty;
            //Form1.LoggingType = SimCoreServer.SimCoreServer.LogType.NOLOG;
            //this.Close();
        }
        public void SetBoxesUp()
        {
            if (Form1.LogPath != string.Empty)
                textBox1.Text = Form1.LogPath;
            if (Form1.LoggingType == Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.LIMITED)
            {
                radioButtonLimited.Checked = true;
            }
            else
                if (Form1.LoggingType == Aptima.Asim.DDD.SimCoreServer.SimCoreServer.LogType.DETAILED)
                {
                    radioButtonDetailed.Checked = true;
                }

        }
    }
}