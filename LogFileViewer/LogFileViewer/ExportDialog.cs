using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LogFileViewer.ViewController;

namespace LogFileViewer
{
    public partial class ExportDialog : Form
    {
        private ViewController.ViewController controller;
        private String export_type;

        public ExportDialog(ViewController.ViewController controller, String export_type)
        {
            InitializeComponent();
            this.controller = controller;
            this.export_type = export_type;
            checkBox1.Checked = controller.SaveWithColumnHeaders;
            if (controller.reportTransforms[export_type].HasCSVHeader)
            {
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = "csv";
            save.Filter = "CSV files [*.csv]|*.csv";
            save.AddExtension = true;
            save.CreatePrompt = false;
            save.OverwritePrompt = true;
            save.FileName = export_type;
            if (save.ShowDialog() == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
                try
                {
                    controller.SaveAsCSV(export_type, save.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show( ex.Message,"Error on save.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return; 
            }
            DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controller.SaveWithColumnHeaders = checkBox1.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}