using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Forms
{
    public partial class NumberForm : Form
    {
        public NumberForm(String title, String prompt,String addText, String closeText)
        {
            InitializeComponent();
            this.Text = title;
            this.groupBox1.Text = prompt;
            this.buttonAdd.Text = addText;
            this.buttonCancel.Text = closeText;
            this.numericUpDown1.Value = 1;
            this.numericUpDown1.Minimum = 0;
        }

        public Decimal NumberValue
        {
            get { return numericUpDown1.Value; }
        }

        private void buttonAdd_click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value >= 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please enter a positive value.");
            }
        }

        private void buttonCancel_click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void InputFormActivated(object sender, EventArgs e)
        {
            this.numericUpDown1.Focus(); // focus, select text field
            this.numericUpDown1.Select(0, 1);
        }
    }
}