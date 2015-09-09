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
    public partial class AGT_SceneLoadDialog : Form
    {
        private delegate void UpdateStatusBarHandler(string message, int current, int maximum);

        private Microsoft.DirectX.Direct3D.Device _video_device = null;

        private IAGT_SceneLoadDialog _resource_loader = null;


        public AGT_SceneLoadDialog(AGT_CanvasControl AGT_Canvas, IAGT_SceneLoadDialog resource_loader)
        {
            _video_device = AGT_Canvas.VideoDevice;
            _resource_loader = resource_loader;
            InitializeComponent();
        }

        public AGT_SceneLoadDialog(Microsoft.DirectX.Direct3D.Device device, IAGT_SceneLoadDialog resource_loader)
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
                this.label1.Text = message;
                this.progressBar1.Maximum = maximum;
                this.progressBar1.Value = current;
            }
            else
            {
                BeginInvoke(new UpdateStatusBarHandler(UpdateStatusBar), message, current, maximum);
            }
        }


        private void AGT_SceneLoadDialog_Shown(object sender, EventArgs e)
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