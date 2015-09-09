using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator.VSGPanes
{
    public partial class NavP_Scenario : Ctl_NavigatorPane
    {
        public NavP_Scenario()
        {
            InitializeComponent();
        }
        private void Launch(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                try
                {
                    System.Diagnostics.Process.Start(filename);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error launching application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Couldn't locate: " + filename, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAttachments_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "Doc files (*.doc)|*.doc|Excel files (*.xls)|*.xls|Powerpoint files (*.ppt)|*.ppt|All files (*.*)|*.*";
            file.RestoreDirectory = true;
            file.Multiselect = true;
            file.InitialDirectory = Environment.CurrentDirectory;
            if (file.ShowDialog() == DialogResult.OK)
            {
                lboxAttachments.Items.AddRange(file.FileNames);
            }

            file.Dispose();
        }

        private void lboxAttachments_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lboxAttachments.SelectedItem != null)
            {
                Launch(lboxAttachments.SelectedItem.ToString());
            }
        }


    }
}
