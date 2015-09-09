using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Aptima.Asim.DDD.Client.Common.GLCore;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class SplashDialog : Form
    {
        private Thread ResourceThread;

        public SplashDialog(GameFramework g)
        {
            InitializeComponent();
            ResourceThread = new Thread(new ParameterizedThreadStart(this.LoadResources));
            ResourceThread.Start(g);

        }

        private void LoadResources(object obj)
        {

            GameFramework g = (GameFramework)obj;
            g.StoreTexture("AptimaLogo",
            g.CreateTexture(
                this.GetType().Assembly.GetManifestResourceStream("DDD_GUI.images.AptimaLogo.jpg")));

            Quit();
        }


        private delegate void QuitDelegate();
        private void Quit()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new QuitDelegate(Quit));
                return;
            }
            this.Dispose();
        }
    }
}