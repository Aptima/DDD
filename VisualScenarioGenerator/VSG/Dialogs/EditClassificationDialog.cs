using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VSG.Controllers;

namespace VSG.Dialogs
{
    public partial class EditClassificationDialog : Form
    {
        public VSGController controller = null;
        public String StateName = "";
        public String ClassificationName = "";
        public String IconName = "";
        public List<String> StateEnum = new List<string>();
        public List<String> ClassificationEnum = new List<string>();
        public List<String> IconEnum = new List<string>();

        public EditClassificationDialog()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            comboBoxStates.Items.Clear();
            comboBoxClassifications.Items.Clear();
            comboBox1.Items.Clear();
            comboBoxStates.Items.AddRange(StateEnum.ToArray());
            comboBoxClassifications.Items.AddRange(ClassificationEnum.ToArray());
            comboBox1.Items.AddRange(IconEnum.ToArray());

            comboBoxStates.SelectedIndex = StateEnum.IndexOf(StateName);
            comboBoxClassifications.SelectedIndex = ClassificationEnum.IndexOf(ClassificationName);
            comboBox1.SelectedIndex = IconEnum.IndexOf(IconName);

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (comboBoxStates.SelectedItem == null)
            {
                MessageBox.Show(this, "You need to select a state first, or Cancel");
                return;
            }
            if (comboBoxClassifications.SelectedItem == null)
            {
                MessageBox.Show(this, "You need to select a classification first, or Cancel");
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show(this, "You need to select an icon first, or Cancel");
                return;
            }

            StateName = comboBoxStates.SelectedItem.ToString();
            ClassificationName = comboBoxClassifications.SelectedItem.ToString();
            IconName = comboBox1.SelectedItem.ToString();
            if (IconName == String.Empty || ClassificationName == String.Empty || StateName == String.Empty)
                return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controller != null && ((ComboBox)sender).SelectedIndex >= 0)
            {
                try
                {
                    pictureBox1.Image = controller.CurrentIconLibrary.Images[((ComboBox)sender).Text];
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
