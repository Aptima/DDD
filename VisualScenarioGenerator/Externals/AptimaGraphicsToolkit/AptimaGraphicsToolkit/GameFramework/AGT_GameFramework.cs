using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.Direct3D;

// Necessary for PeekMsg
using System.Runtime.InteropServices;

using AGT.Sprites;

namespace AGT.GameFramework
{
    public enum MouseEvent: int { None = 0, Click = 1, Down = 2, Up = 3, Move = 4 }
    public delegate void GameOverHandler();
    public delegate void DeviceResizeHandler(System.ComponentModel.CancelEventArgs e, Microsoft.DirectX.Direct3D.Device d);
    public delegate void DeviceResetHandler(Microsoft.DirectX.Direct3D.Device d);
    public delegate void DeviceLost(Microsoft.DirectX.Direct3D.Device d);

    [StructLayout(LayoutKind.Sequential)]
    public struct PeekMsg
    {
        public IntPtr hWnd;
        public Message msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    public struct MouseParams
    {
        public MouseButtons Button;

        public int X;
        public int Y;
        public int Z;


        public static MouseParams Empty
        {
            get
            {
                MouseParams p = new MouseParams();
                p.Button = MouseButtons.None;
                p.X = p.Y = p.Z = 0;
                return p;
            }
        }
        public override bool Equals(object o)
        {
            if (!(o is MouseParams))
                return false;
            return (this == (MouseParams)o);
        }
        
        public static bool operator ==(MouseParams t1, MouseParams t2)
        {
            return ((t1.Button == t2.Button) && (t1.X == t2.X) && (t1.Y == t2.Y) && (t1.Z == t2.Z) );
        }
        public static bool operator !=(MouseParams t1, MouseParams t2)
        {
            return ( (t1.Button != t2.Button) && (t1.X != t2.X) && (t1.Y != t2.Y) && (t1.Z != t2.Z) );
        }
    }


    public struct DeviceParams
    {
        public ClearFlags ClrFlags;
        public bool DirectInput;
        public float ZDepth;
        public int Stencil;
        public Color ClrColor;
        public CreateFlags Flags;
        public PresentParameters PresentationParameters;
        public System.Windows.Forms.Control TargetControl;
        public DeviceResizeHandler OnResize;
        public DeviceResetHandler OnReset;
        public DeviceLost OnLost;

        public static DeviceParams Empty
        {
            get
            {
                DeviceParams p;
                p.DirectInput = true;
                p.ClrFlags = 0;
                p.ZDepth = 1.0f;
                p.Stencil = 0;
                p.ClrColor = Color.Black;
                p.Flags = 0;
                p.PresentationParameters = null;
                p.TargetControl = null;
                p.OnResize = null;
                p.OnReset = null;
                p.OnLost = null;
                return p;
            }
        }

        public override bool Equals(object o)
        {
            if (!(o is DeviceParams))
                return false;
            return (this == (DeviceParams)o);
        }
        public static bool operator ==(DeviceParams t1, DeviceParams t2)
        {
            return ((t1.ZDepth == t2.ZDepth) && (t1.Stencil == t2.Stencil) && (t1.ClrColor == t2.ClrColor) && 
                (t1.ClrFlags == t2.ClrFlags) && (t1.Flags == t2.Flags) && (t1.PresentationParameters == t2.PresentationParameters) &&
                (t1.TargetControl == t2.TargetControl) && (t1.OnLost == t2.OnLost) && (t1.OnReset == t2.OnReset) 
                && (t1.OnResize == t2.OnResize));
        }
        public static bool operator !=(DeviceParams t1, DeviceParams t2)
        {
            return ((t1.ZDepth != t2.ZDepth) && (t1.Stencil != t2.Stencil) && (t1.ClrColor != t2.ClrColor) && (t1.ClrFlags != t2.ClrFlags) && 
                (t1.Flags != t2.Flags) && (t1.PresentationParameters != t2.PresentationParameters) &&
                (t1.TargetControl != t2.TargetControl) && (t1.OnLost != t2.OnLost) && (t1.OnReset != t2.OnReset)
                && (t1.OnResize != t2.OnResize));
        }

    }


    public class AGT_GameFramework: IDisposable
    {
        public bool MouseOver = false;

        #region Private Members
        private MouseEventHandler _mouse_up;
        private MouseEventHandler _mouse_click;
        private MouseEventHandler _mouse_down;
        private MouseEventHandler _mouse_move;
        private MouseEventHandler _mouse_wheel;
        private MouseEventHandler _mouse_double_click;

        private EventHandler _device_lost;
        private EventHandler _device_reset;
        private System.ComponentModel.CancelEventHandler _device_resize;

        private KeyPressEventHandler _key_press;

        private KeyEventHandler _key_down;
        private KeyEventHandler _key_up;



        private Microsoft.DirectX.Direct3D.Device _VIDEO_DEVICE_ = null;
        public Microsoft.DirectX.Direct3D.Device VideoDevice
        {
            get
            {
                return _VIDEO_DEVICE_;
            }
        }

        private DeviceParams _DeviceParams_ = DeviceParams.Empty;
        private AGT_Scene _Scene_ = null;

        private long _ticks_per_second = 0;
        private long _elapsed_ticks = 0;
        private float _frames_per_second = 0;

        private bool _Game_Over = false;

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out PeekMsg msg, IntPtr hWnd,
                uint messageFilterMin, uint messageFilterMax, uint flags);
        #endregion

        private EventHandler _AppIdleHandler = null;
        private AGT_SystemImages _SystemImages = null;
        public AGT_SystemImages SystemImages
        {
            get
            {
                return _SystemImages;
            }
        }
        
        //private Queue<AGT_Scene> _scene_queue = new Queue<AGT_Scene>();
        private List<AGT_Scene> _scene_queue = new List<AGT_Scene>();
        private int _scene_index = 0;

        public AGT_Scene Scene
        {
            get
            {
                lock (this)
                {
                    return _Scene_;
                }
            }
        }

        public GameOverHandler OnGameOver = null;
        public AGT_SpriteId Cursor = AGT_SpriteId.Empty;

        public AGT_GameFramework(DeviceParams parameters) {

            if (parameters == DeviceParams.Empty)
            {
                throw new ArgumentException("Empty Device Parameters");
            }
            if (parameters.PresentationParameters == null)
            {
                throw new ArgumentException("Missing PresentationParameters, in DeviceParams");
            }
            if (parameters.TargetControl == null)
            {
                throw new ArgumentException("Missing TargetControl, in DeviceParams");
            }

            _AppIdleHandler = new EventHandler(this.Application_Idle);
            
            AGT_Scene.QueryPerformanceFrequency(ref _ticks_per_second);
            _DeviceParams_ = parameters;
            BindFormEvents();

            _VIDEO_DEVICE_ = new Microsoft.DirectX.Direct3D.Device(
                Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Adapter,
                Microsoft.DirectX.Direct3D.DeviceType.Hardware,
                parameters.TargetControl,
                parameters.Flags,
                parameters.PresentationParameters);

            _SystemImages = new AGT_SystemImages(_VIDEO_DEVICE_);
            Cursor = _SystemImages.Cursor_Select;

            _device_lost = new EventHandler(_DEVICE__DeviceLost);
            _device_reset = new EventHandler(_DEVICE__DeviceReset);
            _device_resize = new System.ComponentModel.CancelEventHandler(_DEVICE__DeviceResizing);

            _VIDEO_DEVICE_.DeviceLost += _device_lost;
            _VIDEO_DEVICE_.DeviceReset += _device_reset;
            _VIDEO_DEVICE_.DeviceResizing += _device_resize;

        }

        public static Caps GetDeviceCapabilities()
        {
            return Microsoft.DirectX.Direct3D.Manager.GetDeviceCaps(
                Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Adapter,
                Microsoft.DirectX.Direct3D.DeviceType.Hardware
                );
        }

        public void AddScene(AGT_Scene scene)
        {
            lock (this)
            {
                scene.GameFramework = this;
                _scene_queue.Add(scene);
            }
        }

        public void RestartCurrentScene()
        {
            lock (this)
            {
                if (_Scene_ != null)
                {
                    if ((_Scene_ is AGT.Forms.IAGT_SplashDialog) && (_Scene_.State == SceneState.INIT))
                    {
                        if (_Scene_.ShowSplashScreen)
                        {
                            AGT.Forms.AGT_SplashDialog s1 = new AGT.Forms.AGT_SplashDialog(_VIDEO_DEVICE_,
                                (AGT.Forms.IAGT_SplashDialog)_Scene_);
                            s1.ShowDialog();
                            s1.Dispose();
                        }
                    }
                    if ((_Scene_ is AGT.Forms.IAGT_SceneLoadDialog) && (_Scene_.State == SceneState.INIT))
                    {
                        if (!_Scene_.ShowSplashScreen)
                        {
                            AGT.Forms.AGT_SceneLoadDialog s2 = new AGT.Forms.AGT_SceneLoadDialog(_VIDEO_DEVICE_,
                                (AGT.Forms.IAGT_SceneLoadDialog)_Scene_);
                            s2.ShowDialog();
                            s2.Dispose();
                        }
                    }
                    _Scene_.State = SceneState.RENDER;
                }
            }
        }
        public void GetNextScene(int index)
        {
            lock (this)
            {
                if ((_scene_queue.Count > 0) && (index < _scene_queue.Count))
                {
                    Suspend();

                    _Scene_ = _scene_queue[index];
                    if ((_Scene_ is AGT.Forms.IAGT_SplashDialog) && (_Scene_.State == SceneState.INIT))
                    {
                        if (_Scene_.ShowSplashScreen)
                        {
                            AGT.Forms.AGT_SplashDialog s1 = new AGT.Forms.AGT_SplashDialog(_VIDEO_DEVICE_,
                                (AGT.Forms.IAGT_SplashDialog)_Scene_);
                            s1.ShowDialog();
                            s1.Dispose();
                        }
                    }
                    if ((_Scene_ is AGT.Forms.IAGT_SceneLoadDialog) && (_Scene_.State == SceneState.INIT))
                    {
                        if (!_Scene_.ShowSplashScreen)
                        {
                            AGT.Forms.AGT_SceneLoadDialog s2 = new AGT.Forms.AGT_SceneLoadDialog(_VIDEO_DEVICE_,
                                (AGT.Forms.IAGT_SceneLoadDialog)_Scene_);
                            s2.ShowDialog();
                            s2.Dispose();
                        }
                    }

                    Resume();
                }
                else
                {
                    if (OnGameOver == null)
                    {
                        MessageBox.Show("Generic placeholder.", "Game Over");
                        _Game_Over = true;
                    }
                    else
                    {
                        OnGameOver();
                    }
                }
            }
        }

        public void GetNextScene()
        {
            GetNextScene(_scene_index);
            _scene_index++;
        }

        public void StartFramework()
        {
            if (_scene_queue.Count != 0)
            {
                GetNextScene();
                return;
            }
            throw new MissingMemberException("No Scenes have been specified");
        }

        public void Suspend()
        {
            lock (this)
            {
                if (_AppIdleHandler != null)
                {
                    Application.Idle -= _AppIdleHandler;
                }

                if (_DeviceParams_ != DeviceParams.Empty)
                {
                    if (_DeviceParams_.TargetControl != null)
                    {
                        _DeviceParams_.TargetControl.MouseUp -= _mouse_up;
                        _DeviceParams_.TargetControl.MouseClick -= _mouse_click;
                        _DeviceParams_.TargetControl.MouseDown -= _mouse_down;
                        _DeviceParams_.TargetControl.MouseMove -= _mouse_move;
                        _DeviceParams_.TargetControl.MouseWheel -= _mouse_wheel;
                        _DeviceParams_.TargetControl.MouseDoubleClick -= _mouse_double_click;
                        _DeviceParams_.TargetControl.KeyPress -= _key_press;
                        _DeviceParams_.TargetControl.KeyDown -= _key_down;
                        _DeviceParams_.TargetControl.KeyUp -= _key_up;
                    }
                }

            }
        }

        public void Resume()
        {
            lock (this)
            {
                if (_AppIdleHandler != null)
                {
                    Application.Idle += _AppIdleHandler;
                }
                if (_DeviceParams_ != DeviceParams.Empty)
                {
                    if (_DeviceParams_.TargetControl != null)
                    {
                        _DeviceParams_.TargetControl.MouseUp += _mouse_up;
                        _DeviceParams_.TargetControl.MouseClick += _mouse_click;
                        _DeviceParams_.TargetControl.MouseDown += _mouse_down;
                        _DeviceParams_.TargetControl.MouseMove += _mouse_move;
                        _DeviceParams_.TargetControl.MouseWheel += _mouse_wheel;
                        _DeviceParams_.TargetControl.MouseDoubleClick += _mouse_double_click;
                        _DeviceParams_.TargetControl.KeyPress += _key_press;
                        _DeviceParams_.TargetControl.KeyDown += _key_down;
                        _DeviceParams_.TargetControl.KeyUp += _key_up;
                    }
                }

            }
        }

        #region PrivateMethods


        private void BindFormEvents()
        {
            _mouse_up = new MouseEventHandler(OnMouseUp);
            _DeviceParams_.TargetControl.MouseUp += _mouse_up;

            _mouse_click = new MouseEventHandler(OnMouseClick);
            _DeviceParams_.TargetControl.MouseClick += _mouse_click;

            _mouse_down = new MouseEventHandler(OnMouseDown);
            _DeviceParams_.TargetControl.MouseDown += _mouse_down;

            _mouse_move = new MouseEventHandler(OnMouseMove);
            _DeviceParams_.TargetControl.MouseMove += _mouse_move;

            _mouse_wheel = new MouseEventHandler(OnMouseWheel);
            _DeviceParams_.TargetControl.MouseWheel += _mouse_wheel;

            _mouse_double_click = new MouseEventHandler(OnMouseDoubleClick);
            _DeviceParams_.TargetControl.MouseDoubleClick += _mouse_double_click;

            _key_press = new KeyPressEventHandler(OnKeyPress);
            _DeviceParams_.TargetControl.KeyPress += _key_press;

            _key_down = new KeyEventHandler(OnKeyDown);
            _DeviceParams_.TargetControl.KeyDown += _key_down;

            _key_up = new KeyEventHandler(OnKeyUp);
            _DeviceParams_.TargetControl.KeyUp += _key_up;
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnKeyUp(sender, e);
            }
        }

        public void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnKeyPress(sender, e);
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnKeyDown(sender, e);
            }
        }

        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseWheel(sender, e);
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseUp(sender, e);
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseMove(sender, e);
            }
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseDown(sender, e);
            }
        }

        public void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseDoubleClick(sender, e);
            }
        }

        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (Scene != null)
            {
                Scene.OnMouseClick(sender, e);
            }
        }

        private bool AppStillIdle
        {
            get
            {
                PeekMsg msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);

            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (Scene != null)
            {
                RenderScene();
            }
        }

        private void RenderScene()
        {
            long current_ticks = 0;

            while (AppStillIdle && !_Game_Over)
            {
                AGT_Scene.QueryPerformanceCounter(ref _elapsed_ticks);
                
                _VIDEO_DEVICE_.Clear(_DeviceParams_.ClrFlags,
                    _DeviceParams_.ClrColor,
                    _DeviceParams_.ZDepth,
                    _DeviceParams_.Stencil);

                switch (Scene.State)
                {
                    case SceneState.INIT:
                        goto case SceneState.RENDER;
                    case SceneState.REINIT:
                        goto case SceneState.RENDER;

                    case SceneState.RENDER:
                        if (Scene._Projection_ != null)
                        {
                            _VIDEO_DEVICE_.Transform.Projection = Scene._Projection_;
                        }
                        if (Scene._View_ != null)
                        {
                            _VIDEO_DEVICE_.Transform.View = Scene._View_;
                            _VIDEO_DEVICE_.Transform.World = Matrix.Translation(0, 0, 0);
                        }

                        _VIDEO_DEVICE_.BeginScene();

                        Scene.RenderScene(_VIDEO_DEVICE_, _frames_per_second);

                        if (Scene.ShowMouseCursor && MouseOver)
                        {
                            if (Cursor != AGT_SpriteId.Empty)
                            {
                                _SystemImages.DrawSingle(Cursor, Scene.CursorX, Scene.CursorY, 0);
                            }
                        }

                        _VIDEO_DEVICE_.EndScene();

                        try
                        {
                            _VIDEO_DEVICE_.Present();
                        }
                        catch (DeviceLostException)
                        {
                            RecoverLostDevice();
                        }

                        AGT_Scene.QueryPerformanceCounter(ref current_ticks);
                        _elapsed_ticks = current_ticks - _elapsed_ticks;
                        _frames_per_second = (float)Math.Round((float)_ticks_per_second / (float)_elapsed_ticks);

                        break;
                    case SceneState.END:
                        GetNextScene();
                        break;
                    case SceneState.ERROR:
                        // TBD - Eric, Port over the exception handler.
                        _Game_Over = true;
                        break;
                }

            }
        }

        private void _DEVICE__DeviceResizing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_DeviceParams_.OnResize != null)
            {
                _DeviceParams_.OnResize(e, _VIDEO_DEVICE_);
            }
        }

        private void _DEVICE__DeviceReset(object sender, EventArgs e)
        {
            try
            {
                if (_DeviceParams_.OnReset != null)
                {
                    _DeviceParams_.OnReset(_VIDEO_DEVICE_);
                }
            }
            catch (DeviceLostException)
            {
                RecoverLostDevice();
            }
            catch (DriverInternalErrorException)
            {
                RecoverLostDevice();
            }
        }

        private void _DEVICE__DeviceLost(object sender, EventArgs e)
        {
            if (_VIDEO_DEVICE_ != null)
            {
                if (_DeviceParams_.OnLost != null)
                {
                    _DeviceParams_.OnLost(_VIDEO_DEVICE_);
                }
            }
        }
        #endregion

        private void RecoverLostDevice()
        {
            if (_VIDEO_DEVICE_ == null)
            {
                Console.WriteLine("Bad DEVICE");
            }
            try
            {
                _VIDEO_DEVICE_.TestCooperativeLevel(); //let's check what the state of the device is, if we can reset the device or not.
            }
            catch (DeviceLostException)
            {
            }
            catch (DeviceNotResetException) //The device can be reset
            {
                try
                {
                    _VIDEO_DEVICE_.Reset(_DeviceParams_.PresentationParameters); //Reset the device.
                }
                catch (DeviceLostException)
                {
                    Application.ExitThread();
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _SystemImages.Dispose();
        }

        #endregion

    }
}
