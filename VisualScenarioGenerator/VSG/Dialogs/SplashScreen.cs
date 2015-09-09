using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace VSG.Dialogs
{
    public partial class SplashScreen : Form
    {
        private Thread thread;
        public int DisplayTime = 3000;
        private String _version = String.Empty;
        
        public SplashScreen(String versionString)
        {
            _version = versionString;
            InitializeComponent();
            label1.Text = _version;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Parent = pictureBox1;
            thread = new Thread(new ThreadStart(this.WaitTime));
            thread.Start();
        }


        public void WaitTime()
        {
            Thread.Sleep(DisplayTime);
            KillDialog();
        }

        public void KillDialog()
        {
            if (!InvokeRequired)
            {
                Dispose();
            }
            else
            {
                BeginInvoke(new ThreadStart(this.KillDialog));
            }
        }


    }
}