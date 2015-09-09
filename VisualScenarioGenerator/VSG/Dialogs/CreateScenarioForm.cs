using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VSG.Dialogs
{
    public partial class CreateScenarioForm : Form
    {
        public CreateScenarioForm()
        {
            InitializeComponent();
        }

        public String ScenarioName
        {
            get
            {
                return textBoxName.Text;
            }
        }

        public String ScenarioDescription
        {
            get
            {
                return textBoxDescription.Text;
            }
        }

        public String ScenarioTimeToAttack
        {
            get
            {
                return textBoxTimeToAttack.Text;
            }
        }

        private void resetForm()
        {
            textBoxName.Text = String.Empty;
            textBoxDescription.Text = String.Empty;
            textBoxTimeToAttack.Text = String.Empty;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            resetForm();
            Close();
        }
    }
}