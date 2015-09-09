using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using SeamateAdapter.DDD;

namespace SeamateAdapter
{
    public partial class ErrorWindow : Form
    {
        private delegate void SingleStringDelegate(String s);
        protected ErrorWindow(Exception ex, ICloseable parent)
        {
            InitializeComponent();
            textBoxName.Text = ex.Message;
            textBoxDetails.Text = ex.TargetSite.ToString();
            textBoxStackTrace.Text = ex.StackTrace;
        }
        public static DialogResult ShowDialog(Exception ex, ICloseable parent)
        {
            ErrorWindow win = new ErrorWindow(ex, parent);
            return win.ShowDialog();
        }
        private void buttonCopyDetails_Click(object sender, EventArgs e)
        {
            StringBuilder details = new StringBuilder();
            details.AppendFormat("Exception Name: {0}\r\n", textBoxName.Text);
            details.AppendFormat("Exception Details: {0}\r\n", textBoxDetails.Text);
            details.AppendFormat("Exception Stack Trace: {0}", textBoxStackTrace.Text);

            CopyToClipboard(details.ToString());
        }
        private void CopyToClipboard(String s)
        {
            if (InvokeRequired)
            {
                Invoke(new SingleStringDelegate(CopyToClipboard), s);
            }
            else
            { 
                System.Windows.Forms.Clipboard.SetDataObject(s, true);
            }
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            { 
                //close parent application
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
