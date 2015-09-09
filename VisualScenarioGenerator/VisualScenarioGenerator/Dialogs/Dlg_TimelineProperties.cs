using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Dlg_TimelineProperties : Form
    {
        public int Ticks
        {
            set
            {
                if ((value >= numericUpDown1.Minimum) && (value <= numericUpDown1.Maximum))
                {
                    numericUpDown1.Value = (int)value;
                }
            }
            get
            {
                return (int) numericUpDown1.Value;
            }
        }
        public Dlg_TimelineProperties()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}