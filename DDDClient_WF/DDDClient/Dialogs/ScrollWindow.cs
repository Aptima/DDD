using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class ScrollWindow : Form
    {
        public string Content
        {
            set
            {
                richTextBox1.Text = value;
            }
        }
        public ScrollWindow()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

    }
}