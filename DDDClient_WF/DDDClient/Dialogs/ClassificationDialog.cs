using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class ClassificationDialog : Form
    {
        private String selectedClassification = "";
        public ClassificationDialog()
        {
            InitializeComponent();
            labelObjectId.Text = "";
            comboBoxClassifications.Items.Add("");
        }
        public ClassificationDialog(String objectID, String[] classifications, String currentClassification)
        {
            InitializeComponent();
            labelObjectId.Text = objectID;
            selectedClassification = currentClassification;
            comboBoxClassifications.Items.AddRange(classifications);
            int i = classifications.Length - 1;
            bool loop = true;
            while (loop && i >= 0)
            {
                if (classifications[i] == currentClassification)
                {
                    loop = false;
                    break;
                }
                i--;
            }
            comboBoxClassifications.SelectedIndex = i;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            selectedClassification = comboBoxClassifications.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        internal string GetClassification()
        {
            return selectedClassification;
        }
    }
}
