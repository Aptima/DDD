using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class TextboxDialog : Form
    {
        string textValue = string.Empty;

        //public string GetTextValue()
        //{
        //    return textValue;
        //}
        public TextboxDialog()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        public TextboxDialog(string title, string innerLabel, string defaultTextboxValue)
        {
            InitializeComponent(); 
            this.Text = title;
            this.label1.Text = innerLabel;
            this.textBox1.Text = defaultTextboxValue;
            textValue = defaultTextboxValue;
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //textValue = textBox1.Text;
            this.Close();
        }

        public new string ShowDialog(IWin32Window owner)
        {
            string tag = textValue;
            if (base.ShowDialog(owner) == DialogResult.OK)
            {
                tag = textBox1.Text;
            }
            return tag;
        }
    }
}