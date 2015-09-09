using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace AGT.Forms
{
    public partial class AGT_SplashDialog : Form
    {
        private delegate void UpdateStatusBarHandler(string message, int current, int maximum);

        private Microsoft.DirectX.Direct3D.Device _video_device = null;

        private IAGT_SplashDialog _resource_loader = null;

        public AGT_SplashDialog(AGT_CanvasControl AGT_Canvas, IAGT_SplashDialog resource_loader)
        {
            _video_device = AGT_Canvas.VideoDevice;
            _resource_loader = resource_loader;
            InitializeComponent();
        }

        public AGT_SplashDialog(Microsoft.DirectX.Direct3D.Device device, IAGT_SplashDialog resource_loader)
        {
            _video_device = device;
            _resource_loader = resource_loader;
            InitializeComponent();
            _resource_loader.DialogInitialize(this);
        }

        public void UpdateStatusBar(string message, int current, int maximum)
        {
            if (!InvokeRequired)
            {
                this.toolStripStatusLabel1.Text = message;
                this.toolStripProgressBar1.Maximum = maximum;
                this.toolStripProgressBar1.Value = current;
            }
            else
            {
                BeginInvoke(new UpdateStatusBarHandler(UpdateStatusBar), message, current, maximum);
            }
        }

        
        private void SplashDialog_Shown(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Thread s = new Thread(new ThreadStart(LoadResources));
                s.Start();
            }
        }

        private void LoadResources()
        {
            if (_resource_loader != null)
            {
                _resource_loader.LoadResources(this, _video_device);
                System.Threading.Thread.Sleep(1500);
                Dispose();
            }
        }


    }
}