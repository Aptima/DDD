using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Database
{
    public partial class WarningForm : Form
    {
        public WarningForm(String warningLabelPass, String caption)
        {
            InitializeComponent();
            this.warninglabel.Text = warningLabelPass;
            this.Text = caption;

            this.Icon = SystemIcons.Warning;

            okbutton.Click += new EventHandler(okbutton_Click);
            nobutton.Click += new EventHandler(nobutton_Click);
        }

        void nobutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        void okbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}