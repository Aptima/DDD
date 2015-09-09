using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class WhiteboardWindowProperties : Form
    {
        private bool _allow_changes = true;
        public bool AllowChanges
        {
            set
            {
                button1.Enabled = value;
                textBox1.Enabled = value;
                _allow_changes = value;
            }
            get
            {
                return _allow_changes;
            }
        }
        public string GroupId
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }
        public List<string> Members
        {
            get
            {
                List<string> list = new List<string>();
                foreach (string item in listBox1.Items)
                {
                    list.Add(item);
                }
                return list;
            }
            set
            {
                listBox1.Items.AddRange(value.ToArray());
            }
        }
        public List<string> SelectedMembers
        {
            get
            {
                List<string> list = new List<string>();
                foreach (string item in listBox1.SelectedItems)
                {
                    list.Add(item);
                }
                return list;
            }
        }
        public WhiteboardWindowProperties()
        {
            InitializeComponent();
            if (AllowChanges)
            {
                button1.Enabled = false;
            }
        }

        private void ItemsChangeHandler()
        {
            if (AllowChanges)
            {
                if ((textBox1.Text.Length > 0) && (listBox1.SelectedItems.Count > 0))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                DialogResult = DialogResult.OK;
                return;
            }
            DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ItemsChangeHandler();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemsChangeHandler();
        }

    }
}