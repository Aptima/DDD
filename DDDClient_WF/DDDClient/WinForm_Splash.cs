using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.Client.Common.GLCore;

using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using Aptima.Asim.DDD.Client.Common.GLCore.Controls;
using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;

using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Threading;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client
{
    class WinForm_Splash: WindowManager
    {
        private enum LoadingStage:int {NONE = 0, TEXTURE = 1, BACKGROUND = 2, FINISHED = 3}
        public DM_Dialog _dm_dialog;

        private LoadingStage Stage = LoadingStage.NONE;

        private Microsoft.DirectX.Direct3D.Font font_large;
        private Microsoft.DirectX.Direct3D.Font font_small;
        private Rectangle _background;



        private string Message = "Please make choice ...";
        private Color MsgColor = Color.Yellow;
        private Rectangle bounding_rect = Rectangle.Empty;
        private Rectangle player_rect = Rectangle.Empty;

        private Assembly ImageLib;
        private Color _background_color= Color.FromArgb(130, 135, 138);
        private Material _background_color_material = new Material();
        private Material _background_window_material = new Material();


        private IGameControl _game_control;
        private ICommand _commands;
        private string _menu_selection = string.Empty;


        private bool _loading_textures = false;

        public WinForm_Splash(IGameControl game_control, ICommand commands)
            : base(game_control)
        {
            _background_color_material.Diffuse = _background_color;
            _background_window_material.Diffuse = Window.BackgroundColor;

            _commands = commands;
            _game_control = game_control;
            _dm_dialog = new DM_Dialog(_commands);
        }

        public void HandshakeInitializeGUI()
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.LoadTextures));
            t.Start((object)GameFramework.Instance());
            _loading_textures = true;
            lock (this)
            {
                Stage = LoadingStage.TEXTURE;
            }
            SceneMode = MODE.SCENE_RENDER;
        }

        #region IScene Members
        public override void OnInitializeScene(GameFramework g)
        {
            BindGameController();

            
           
            //font_small = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 12));
            font_small = g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 14));

            font_large = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 28));

        }


        public override void OnSceneLoading(GameFramework g)
        {
            if (!DDD_Global.Instance.IsConnected)
            {
                // Assumes we're in Demo mode.
                DDD_Global.Instance.MapName = "Map.jpg";
                DDD_Global.Instance.MapLocation = string.Format("{0}MapLib\\Map.jpg", DDD_Global.Instance.DDDClientShareFolder);
                UTM_Mapping.HorizonalMetersPerPixel = 8.49990671075954650f;
                UTM_Mapping.VerticalMetersPerPixel = 8.56718696599567760f;
                DDD_Global.Instance.ScenarioName = "Demo";
                DDD_Global.Instance.ScenarioDescription = "This is a locally connected demo.";
                _menu_selection = "Demo Player";
                SceneMode = MODE.SCENE_RENDER;

                HandshakeInitializeGUI();
                return;
            }
        }


        public override void OnBeforeRender(GameFramework g)
        {
            switch (SceneMode)
            {
                case MODE.SCENE_RENDER:

                    if (_loading_textures)
                    {
                        lock (this)
                        {
                            switch (Stage)
                            {
                                case LoadingStage.TEXTURE:
                                        //Message = "LOADING TEXTURES ...";
                                        MsgColor = Color.Yellow;
                                    return;
                                case LoadingStage.BACKGROUND:
                                        Message = "LOADING MAP ...";
                                        MsgColor = Color.Yellow;
                                    return;
                                case LoadingStage.FINISHED:
                                        Message = "READY ...";
                                        SceneMode = MODE.SCENE_FINISHED;
                                        MsgColor = Color.Yellow;
                                    return;
                                default:
                                        SceneMode = MODE.SCENE_ERROR;
                                        Message = "Unknown State.";
                                        MsgColor = Color.Red;
                                    return;
                            }
                        }

                    }
                    break;
                    
                case MODE.SCENE_ERROR:
                    break;

            }
           

        }

        public override void OnRender(Canvas canvas)
        {
            _background.X = 0;
            _background.Y = 0;
            _background.Width = canvas.Size.Width;
            _background.Height = canvas.Size.Height;

            bounding_rect = font_large.MeasureString(null, Message, DrawTextFormat.NoClip, Color.Yellow);
            bounding_rect.X = (_background.Width - bounding_rect.Width) / 2;
            bounding_rect.Y = (int)(_background.Height - (2 * bounding_rect.Height));

            canvas.DrawFillRect(_background, _background_color_material);
            canvas.DrawRect(_background, Color.White);

            font_large.DrawText(null, Message, bounding_rect, DrawTextFormat.None, MsgColor);

            base.OnRender(canvas);
        }

        public override void OnSceneCleanup(GameFramework g)
        {
            font_small.Dispose();
            font_large.Dispose();
            UnbindGameController();
        }

        public void LoadTextures(object obj) {
            string path = string.Empty;
            try
            {
                path = string.Format("{0}\\{1}.dll", DDD_Global.Instance.DDDClientShareFolder, DDD_Global.Instance.ImageLibrary);
                if (!File.Exists(path))
                {
                    path = string.Format(@"{0}\DDDClient\{1}.dll", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DDD_Global.Instance.ImageLibrary);
                }
                ImageLib = Assembly.LoadFile(path);
            }
            catch (Exception e)
            {
                lock (this)
                {
                    SceneMode = MODE.SCENE_ERROR;
                    Message = "Missing Image Libary";
                    MsgColor = Color.Red;
                    Thread.Sleep(2000);

                    throw new Exception(Message + ":" + path + ":" + e.Message);

                }
                return;
            }

            
            GameFramework g = (GameFramework)obj;
            GameTexture _texture;
            List<string> names = new List<string>();


            try
            {
                StreamReader s = new StreamReader(ImageLib.GetManifestResourceStream("ImageLibrary.mf"));

                while (!s.EndOfStream)
                {
                    names.Add(s.ReadLine());
                }
                for (int i = 0; i < names.Count; i++)
                {
                    string[] texture_name = names[i].Split(':');
                    _texture = g.CreateTexture(ImageLib.GetManifestResourceStream(texture_name[0]));
                    _texture.rotate = bool.Parse(texture_name[1]);
                    g.StoreTexture(texture_name[0], _texture);
                    lock (this)
                    {
                        Message = string.Format("Texture {0} of {1}", i + 1, names.Count);
                    }
                }
                s.Close();
            }
            catch (Exception)
            {
                lock (this)
                {
                    SceneMode = MODE.SCENE_ERROR;
                    Message = "Error loading texture";
                    MsgColor = Color.Red;
                }
                return;
            }

            lock (this)
            {
                Stage = LoadingStage.BACKGROUND;
            }

            bool continue_looping = true;
            GameTexture Map;
            while (continue_looping)
            {
                if (DDD_Global.Instance.MapLocation != string.Empty)
                {
                    try
                    {
                        if (File.Exists(DDD_Global.Instance.MapLocation))
                        {
                            Map = g.CreateTexture(DDD_Global.Instance.MapLocation);
                        }
                        else
                        {
                            Map = g.CreateTexture(string.Format("{0}\\MapLib\\{1}", DDD_Global.Instance.DDDClientShareFolder, DDD_Global.Instance.MapName));
                        }
                        Controller.UTM_Mapping.ImageHeight = Map.height;
                        Controller.UTM_Mapping.ImageWidth = Map.width;
                        g.StoreTexture("MAP", Map);
                        continue_looping = false;
                    }
                    catch (Exception)
                    {
                        SceneMode = MODE.SCENE_ERROR;
                        Message = "Map file missing: " + DDD_Global.Instance.MapName;
                        MsgColor = Color.Red;
                        continue_looping = false;
                        Thread.Sleep(2000);

                        throw new Exception(Message);
                    }

                }
                else
                {
                    Thread.Sleep(2000);
                }
            }

            
            lock (this)
            {
                Stage = LoadingStage.FINISHED;
                DDD_RangeRings.SetCanvasPtr(GameFramework.Instance().CANVAS);
            }
        }

 

        #endregion
    }
}
