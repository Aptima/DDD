using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VSG.Dialogs
{
    public partial class ProgressDialog : Form
    {
        private System.Threading.ManualResetEvent initEvent = new System.Threading.ManualResetEvent(false);
        public delegate void ValueInvoker(int v);
        public delegate void StringInvoker(String v);
        public ProgressDialog()
        {
            InitializeComponent();

            //MinimizeBox = false;
            //MaximizeBox = false;

            ControlBox = false;
        }


        public void SetMinimum(int value)
        {
            if (!InvokeRequired)
            {
                DoSetMinimum(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new ValueInvoker(DoSetMinimum), new object[] { value });
            }
        }
        private void DoSetMinimum(int value)
        {
            progressBar1.Minimum = value;
        }

        public void SetMaximum(int value)
        {
            if (!InvokeRequired)
            {
                DoSetMaximum(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new ValueInvoker(DoSetMaximum), new object[] { value });
            }
        }
        private void DoSetMaximum(int value)
        {
            progressBar1.Maximum = value;
        }

        public void SetValue(int value)
        {
            if (!InvokeRequired)
            {
                DoSetValue(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new ValueInvoker(DoSetValue), new object[] { value });
            }
        }
        private void DoSetValue(int value)
        {
            progressBar1.Value = value;
        }

        public void SetStep(int value)
        {
            if (!InvokeRequired)
            {
                DoSetStep(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new ValueInvoker(DoSetStep), new object[] { value });
            }
        }
        private void DoSetStep(int value)
        {
            progressBar1.Value = value;
        }

        public void SetTitle(String value)
        {
            if (!InvokeRequired)
            {
                DoSetTitle(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new StringInvoker(DoSetTitle), new object[] { value });
            }
        }
        private void DoSetTitle(String value)
        {
            this.Text = value;
        }

        public void SetMessage(String value)
        {
            if (!InvokeRequired)
            {
                DoSetMessage(value);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new StringInvoker(DoSetMessage), new object[] { value });
            }
        }
        private void DoSetMessage(String value)
        {
            label1.Text = value;

        }


        public void SetDone()
        {
            if (!InvokeRequired)
            {
                DoSetDone(0);
            }
            else
            {
                initEvent.WaitOne();
                Invoke(new ValueInvoker(DoSetDone), new object[] { 0 });
            }
        }
        private void DoSetDone(int value)
        {
            this.DialogResult = DialogResult.OK;
        }


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            initEvent.Set();

            this.BringToFront();
        }


    }
}