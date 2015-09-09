using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Forms
{
    public partial class WarningForm : Form
    {
        public WarningForm(String warningLabelPass, String caption)
        {
            InitializeComponent();
            this.warninglabel.Text = warningLabelPass;
            this.Text = caption;

            this.Icon = SystemIcons.Warning;

            this.okbutton.Top = this.warninglabel.Bottom + this.okbutton.Height;
            this.okbutton.Left = (this.Width / 2) - (this.okbutton.Width + 10);
            this.nobutton.Top = this.warninglabel.Bottom + this.nobutton.Height;
            this.nobutton.Left = (this.Width / 2) + 10;

            okbutton.Click += new EventHandler(okbutton_Click);
            nobutton.Click += new EventHandler(nobutton_Click);
        }//constructor

        public WarningForm(String warningLabelPass, String caption,
            string yesText, bool yesVisible,
            string noText, bool noVisible)
            : this(warningLabelPass, caption)
        {
            this.YesButton_Text = yesText;
            this.YesButton_Visible = yesVisible;
            this.NoButton_Text = noText;
            this.NoButton_Visible = noVisible;
        }//constructor

        public string YesButton_Text
        {
            get { return this.okbutton.Text; }
            set
            {
                if (value != null)
                {
                    this.okbutton.Text = value;
                }
            }//set
        }//YesButton_Text

        public string NoButton_Text
        {
            get { return this.nobutton.Text; }
            set
            {
                if (value != null)
                {
                    this.nobutton.Text = value;
                }
            }//set
        }//NoButton_Text

        public bool YesButton_Visible
        {
            get { return this.okbutton.Visible; }
            set { this.okbutton.Visible = value; }
        }//YesButton_Visible

        public bool NoButton_Visible
        {
            get { return this.nobutton.Visible; }
            set { this.nobutton.Visible = value; }
        }//NoButton_Visible

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