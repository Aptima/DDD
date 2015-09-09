using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.GameFramework;
using AGT.Sprites;

namespace AGT.Forms
{
    public partial class AGT_CanvasControl : UserControl, IDisposable
    {
        private AGT_GameFramework gf;
        private DeviceParams parm;

        private bool _framework_started = false;
        public bool FrameworkStarted
        {
            get
            {
                return _framework_started;
            }
        }

        private bool _suspended = false;
        public bool Suspended
        {
            get
            {
                return _suspended;
            }
        }

        public Microsoft.DirectX.Direct3D.Device VideoDevice
        {
            get
            {
                return gf.VideoDevice;
            }
        }
        public AGT_Scene CurrentScene
        {
            get
            {
                return gf.Scene;
            }
        }

        public GameOverHandler OnGameOver
        {
            set
            {
                gf.OnGameOver = value;
            }
        }

        public AGT_CanvasControl()
        {
            InitializeComponent();
            InitializeGameFramework();
        }

        private void InitializeGameFramework()
        {
            parm = new DeviceParams();

            DeviceCaps _device_caps = AGT_GameFramework.GetDeviceCapabilities().DeviceCaps;

            parm.ClrFlags = ClearFlags.Target | ClearFlags.ZBuffer;
            parm.ZDepth = 1.0f;
            parm.Stencil = 0;
            parm.TargetControl = this;

            parm.OnResize = new DeviceResizeHandler(OnResize);

            parm.PresentationParameters = new PresentParameters();
            parm.PresentationParameters.Windowed = true;
            parm.PresentationParameters.SwapEffect = SwapEffect.Discard;

            if (parm.PresentationParameters.Windowed)
            {
                parm.PresentationParameters.PresentationInterval = PresentInterval.Default;
                parm.PresentationParameters.EnableAutoDepthStencil = true;
                parm.PresentationParameters.AutoDepthStencilFormat = DepthFormat.D16;
            }
            else
            {
                parm.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
                parm.PresentationParameters.EnableAutoDepthStencil = false;
                parm.PresentationParameters.BackBufferCount = 1;
                parm.PresentationParameters.BackBufferHeight = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Height;
                parm.PresentationParameters.BackBufferWidth = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Width;
                parm.PresentationParameters.BackBufferFormat = Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Format;
            }

            if (_device_caps.SupportsHardwareTransformAndLight)
            {
                parm.Flags = CreateFlags.HardwareVertexProcessing;
                if (_device_caps.SupportsPureDevice)
                {
                    parm.Flags |= CreateFlags.PureDevice;
                    parm.Flags |= CreateFlags.MultiThreaded;
                }
            }
            else
            {
                parm.Flags = CreateFlags.SoftwareVertexProcessing;
                parm.Flags |= CreateFlags.MultiThreaded;
            }

            gf = new AGT_GameFramework(parm);
        }

        public void AddScene(AGT_Scene scene)
        {
            lock (this)
            {
                if (gf != null)
                {
                    gf.AddScene(scene);
                }
            }
        }
        public void StartFramework()
        {
            lock (this)
            {
                if (gf != null)
                {
                    if (!_framework_started)
                    {
                        gf.StartFramework();
                        _framework_started = true;
                    }
                }
            }
        }
        public void SuspendFramework()
        {
            lock (this)
            {
                if (gf != null)
                {
                    if (!_suspended)
                    {
                        gf.Suspend();
                        _suspended = true;
                    }
                }
            }
        }
        public void ResumeFramework()
        {
            lock (this)
            {
                if (gf != null)
                {
                    if (_suspended)
                    {
                        gf.Resume();
                        _suspended = false;
                    }
                }
            }
        }
        public void RestartCurrentScene()
        {
            lock (this)
            {
                if (gf != null)
                {
                    gf.RestartCurrentScene();
                }
            }
        }
        public void GetScene(int index)
        {
            lock (this)
            {
                if (gf != null)
                {
                    gf.GetNextScene(index);
                }
            }
        }
        private void OnResize(CancelEventArgs e, Device d)
        {
            lock (this)
            {
                try
                {
                    if (gf != null)
                    {
                        if (gf.Scene != null)
                        {
                            gf.Scene.State = SceneState.REINIT;
                            gf.Scene.TargetControlRect = Rectangle.FromLTRB(0, 0, d.Viewport.Width, d.Viewport.Height);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor.Hide();
            gf.MouseOver = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor.Show();
            gf.MouseOver = false;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            lock (this)
            {
                if (gf.Scene != null)
                {
                    gf.Scene.SetCursorPosition(e.X, e.Y);
                    gf.Scene.OnMouseMove(this, e);
                }
            }
        }

        private void AGT_CanvasControl_Enter(object sender, EventArgs e)
        {
        }

        private void AGT_CanvasControl_Leave(object sender, EventArgs e)
        {
        }




        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (gf != null)
            {
                gf.Dispose();
            }
        }

        #endregion
    }
}
