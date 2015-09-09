using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Dlg_CreateAssetInstance : Form
    {
        public string Name = string.Empty;

        public Dlg_CreateAssetInstance()
        {
            InitializeComponent();
        }

        public void SetDecisionMakers(string[] decisionmakers)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(decisionmakers);
        }
        public void SetDecisionMakers(List<string> decisionmakers)
        {
            comboBox1.Items.Clear();
            foreach (string s in decisionmakers)
            {
                comboBox1.Items.Add(s);
            }
        }
        public void SetSpecies(string[] species)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(species);
        }
        public void SetSpecies(List<string> species)
        {
            comboBox2.Items.Clear();
            foreach (string s in species)
            {
                comboBox2.Items.Add(s);
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                Name = textBox1.Text;
            }
            DialogResult = DialogResult.OK;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}