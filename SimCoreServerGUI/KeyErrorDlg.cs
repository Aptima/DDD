using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class KeyErrorDlg : Form
    {
        public KeyErrorDlg(string customError)
        {
            InitializeComponent();
            labelCustomError.Text = customError;
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}