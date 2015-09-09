using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.Threading;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.Controls;



namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    public struct GameTexture {
        public Texture texture;
        public int width;
        public int height;
        public bool rotate;
        public static GameTexture Empty
        {
            get
            {
                GameTexture txt;
                txt.rotate = false;
                txt.texture = null;
                txt.width = 0;
                txt.height = 0;
                return txt;
            }
        }

        public override int GetHashCode()
        {
            return texture.GetHashCode();
        }
        public override bool Equals(object o)
        {
            if (!(o is GameTexture))
                return false;
            return (this == (GameTexture)o);
        }
        public static bool operator ==(GameTexture t1, GameTexture t2)
        {
            return ((t1.texture == t2.texture) && (t1.width == t2.width) &&
                (t1.height == t2.height) && (t1.rotate = t2.rotate));
        }
        public static bool operator !=(GameTexture t1, GameTexture t2)
        {
            return ((t1.texture != t2.texture) && (t1.width != t2.width) &&
                (t1.height != t2.height) && (t1.rotate == t2.rotate));
        }

    }


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


    public class GameFramework
    {
        #region PrivateItems
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out PeekMsg msg, IntPtr hWnd,
                uint messageFilterMin, uint messageFilterMax, uint flags);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceCounter(ref long PerformanceCount);
        private bool ContinueExecution = true;


        private static GameFramework _instance_ = null;
        private EventHandler Idle = null;

        private static long _ticks_per_second;
        private static long _start_time;

        private Form _main_window = null;
        private IGameControl _game_control;
        private Scene[] _AvailableScenes_;

        private int CURRENT_SCENE = 0;
        private Canvas _CANVAS_;

        private Rectangle MousePosition = Rectangle.Empty;

        private Dictionary<string, GameTexture> _Textures_;
        public Dictionary<string, GameTexture> Textures
        {
            get
            {
                lock (this)
                {
                    return new Dictionary<string,GameTexture>(_Textures_);
                }
            }
        }

        #endregion

        public Obj_Sprite HeadsUpDisplay = null;

        public static bool IsRunning
        {
            get
            {
                if (_instance_ != null)
                {
                    return true;
                }
                return false;
            }
        }

        public Canvas CANVAS
        {
            get
            {
                return _CANVAS_;
            }
        }

        public bool IsDefaultScene()
        {
            if (CURRENT_SCENE == 0)
            {
                return true;
            }
            return false;
        }
        public void ChangeScene(int id)
        {
            CURRENT_SCENE = id;
        }


        #region Texture Region
        /* *********************************************************************** *
         * 
         *  Texture Methods
         * 
         * *********************************************************************** */

        public GameTexture CreateTexture (string filename) 
        {
            GameTexture texture = GameTexture.Empty;
            try
            {
                texture.texture = _CANVAS_.LoadTexture(filename, out texture.width, out texture.height);

                return texture;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }

        public GameTexture CreateTexture(System.IO.Stream stream)
        {
            GameTexture texture = GameTexture.Empty;

            try
            {
                texture.texture = _CANVAS_.CreateTexture(stream, out texture.width, out texture.height);

                return texture;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }

        public void StoreTexture(string id, GameTexture texture)
        {
            lock (this)
            {
                if (_Textures_ == null)
                {
                    _Textures_ = new Dictionary<string, GameTexture>();
                }
                if (texture.texture != null)
                {
                    _Textures_.Add(id, texture);
                }
            }
        }
        public bool TextureExists(string id)
        {
            lock (this)
            {
                return _Textures_.ContainsKey(id);
            }
        }
        public GameTexture GetTexture(string id)
        {
            lock (this)
            {
                if (_Textures_ != null)
                {
                    if (_Textures_.ContainsKey(id))
                    {
                        return _Textures_[id];
                    }
                }
                return GameTexture.Empty;
            }
        }

        public void RemoveTexture(string id)
        {
            lock (this)
            {
                _Textures_.Remove(id);
            }
        }

        public void ClearTextureCache()
        {
            lock (this)
            {
                _Textures_.Clear();
            }
        }

        #endregion 

        #region TimerFunctions
        //public static long StartPerformanceTimer()
        //{
        //    QueryPerformanceCounter(ref _start_time);
        //    return _start_time;
        //}

        public static long QueryPerformanceTimer()
        {
            long ticks = 0;
            QueryPerformanceCounter(ref ticks);
            return ticks;
        }

        public static long GetFramePerSecond()
        {
            long end_time = 0;
            QueryPerformanceCounter(ref end_time);
            long ticks_per_frame = end_time - _start_time;

            return _ticks_per_second / ticks_per_frame;
        }

        public static long GetTicksPerSecond()
        {
            return _ticks_per_second;
        }

        public static double ElapsedSeconds(long start_time)
        {
            long current_time = 0;
            QueryPerformanceCounter(ref current_time);
            return ((double)current_time - (double)start_time) / (double)_ticks_per_second;
        }

        public static bool ElapsedSeconds(long start_time, double seconds)
        {
            double elapsed = ElapsedSeconds(start_time);
            if (ElapsedSeconds(start_time) > seconds)
            {
                return true;
            }
            return false;
        }
        #endregion

        public static GameFramework Instance()
        {
            if (_instance_ == null)
            {
                _instance_ = new GameFramework();
            }
            return _instance_;
        }

        public GameFramework()
        {
            MousePosition.Width = 1;
            MousePosition.Height = 1;

        }

        public void SetCursor(Cursor c)
        {
            _main_window.Cursor = c;
        }
         
        /// <summary>
        /// Starts the Game Framework, registers a form and a gamecontroller.
        /// The form is required to intercept keyboard and mouse input.
        /// The GameController initializes scenes and feeds them to the GameController.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="game_control"></param>
        public void Run(Form form, IGameControl game_control)
        {
            if (form == null)
            {
                throw new ApplicationException("Must supply a windows Form.");
            }
            _main_window = form;
            if (game_control == null)
            {
                throw new ApplicationException("Game Control is uninitialized.");
            }
            _CANVAS_ = new Canvas();
            QueryPerformanceFrequency(ref _ticks_per_second);

            _game_control = game_control;
            _CANVAS_.TargetControl = _game_control.GetTargetControl();

            _game_control.InitializeCanvasOptions(_CANVAS_.Options);
            _CANVAS_.InitializeCanvas();

            _AvailableScenes_ = _game_control.InitializeScenes(this);
            _AvailableScenes_[CURRENT_SCENE].Initialize(this);
            _game_control.SceneChanged(CURRENT_SCENE);

            Idle = new EventHandler(GameLoop);
            Application.Idle += Idle;

        }





        /// <summary>
        /// The Game Loop, runs through the following series of actions:
        /// Gets the next scene.
        /// Executes the Scene's Initialize Method.
        /// Executes the Scene's OnSceneLoading Method.
        ///    Loops {
        ///        Executes the Scene's OnBeforeRender Method.
        ///        Passes the Scene to Canvas for rendering.
        ///        Executes the Scene's OnAfterRender Method.
        ///    }
        ///  Executes the Scene's OnSceneCleanup Method.
        /// </summary>
        private void GameLoop(object sender, EventArgs e)
        {

            try
            {
                while (AppStillIdle && ContinueExecution)
                {
                    if (!_AvailableScenes_[CURRENT_SCENE].Initialized)
                    {
                        _AvailableScenes_[CURRENT_SCENE].Initialize(this);
                    }
                    switch (_AvailableScenes_[CURRENT_SCENE].SceneMode)
                    {
                        case Scene.MODE.SCENE_LOAD:
                            _AvailableScenes_[CURRENT_SCENE].OnSceneLoading(this);
                            break;
                        case Scene.MODE.SCENE_RENDER:
                            _AvailableScenes_[CURRENT_SCENE].OnBeforeRender(this);
                            _CANVAS_.Render(_AvailableScenes_[CURRENT_SCENE]);
                            _AvailableScenes_[CURRENT_SCENE].OnAfterRender(this);
                            break;
                        case Scene.MODE.SCENE_FINISHED:
                            _AvailableScenes_[CURRENT_SCENE].OnSceneCleanup(this);
                            NextScene();
                            break;
                        case Scene.MODE.SCENE_ERROR:
                            _CANVAS_.Render(_AvailableScenes_[CURRENT_SCENE]);
                            Thread.Sleep(2000);
                            ExitGameLoop();
                            break;
                    }
                }
            }
            catch (Exception a)
            {
                CANVAS.Quit();
                ExitGameLoop();
                throw new Exception(string.Format("Render Error: Cannot continue.\n({0}): {1}", a.Message, a.StackTrace));
            }
        }

        
        public void ExitGameLoop()
        {
            lock (this)
            {
                ContinueExecution = false;
                Application.Idle -= Idle;
            }
            return;
        }

        public void NextScene()
        {
            lock (this)
            {
                CURRENT_SCENE++;
                if (CURRENT_SCENE >= _AvailableScenes_.Length)
                {
                    _game_control.GameOver(this);
                    return;
                }
                _game_control.SceneChanged(CURRENT_SCENE);
                _AvailableScenes_[CURRENT_SCENE].Initialize(this);
            }
        }


        public Scene GetCurrentScene()
        {
            lock (this)
            {
                return _AvailableScenes_[CURRENT_SCENE];
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

    }


}
