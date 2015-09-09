using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class DropdownDialog : Form
    {
        public DropdownDialog(string header, string dialogText, List<string> objectsToDisplay)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.Text = header;
            this.labelDialogText.Text = dialogText;
            this.comboBoxObjects.DataSource = objectsToDisplay;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = false;

            if (((ComboBox)sender).Text != string.Empty)
            {
                buttonOk.Enabled = true;
            }

        }

        public new string ShowDialog(IWin32Window owner)
        {
            string selectedObject = string.Empty;
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                selectedObject = comboBoxObjects.Text;
            }
            return selectedObject;
        }
    }
}