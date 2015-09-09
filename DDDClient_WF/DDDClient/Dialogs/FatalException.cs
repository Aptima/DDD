using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class FatalException : Form
    {
        public string Message
        {
            set
            {
                this.richTextBox1.SelectedText += value;
            }
            get
            {
                return this.richTextBox1.SelectedText;
            }
        }

        public FatalException()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Application.Exit();
        }
    }
}