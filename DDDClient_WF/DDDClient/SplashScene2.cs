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
    class SplashScene2: WindowManager
    {
        private enum LoadingStage:int {NONE = 0, TEXTURE = 1, BACKGROUND = 2, FINISHED = 3}
 
        private LoadingStage Stage = LoadingStage.NONE;

        private Microsoft.DirectX.Direct3D.Font font_large;
        private Microsoft.DirectX.Direct3D.Font font_small;
        private Rectangle _background;

        private Aptima.Asim.DDD.Client.Common.GLCore.Window _player_window;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelMenu _player_menu;
        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.Panel _btn_panel;

        private Aptima.Asim.DDD.Client.Common.GLCore.Controls.PanelStaticButton _continue_btn;

        private string Message = "Please make choice ...";
        private Color MsgColor = Color.Yellow;
        private Rectangle bounding_rect = Rectangle.Empty;
        private Rectangle player_rect = Rectangle.Empty;

        private Assembly ImageLib;
        private Color _background_color= Color.FromArgb(130, 135, 138);
        private Material _background_color_material = new Material();
        private Material _background_window_material = new Material();

        private bool _start_scenario = false;
        private bool _continue = false;

        private IGameControl _game_control;
        private ICommand _commands;
        private string _menu_selection = string.Empty;


        private bool _loading_textures = false;

        public SplashScene2(IGameControl game_control, ICommand commands): base(game_control)
        {
            _background_color_material.Diffuse = _background_color;
            _background_window_material.Diffuse = Window.BackgroundColor;

            _commands = commands;
            _game_control = game_control;
        }

        public void Btn_Continue(object sender, MouseEventArgs e)
        {

            if (!_continue)
            {
                _commands.HandshakeGUIRoleRequest(_menu_selection, DDD_Global.Instance.TerminalID);
            }
            else
            {
                _continue_btn.Selected = true;
                _start_scenario = true;
            }
        }

        public override void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (_continue_btn.ClientArea.Contains(e.Location))
            {
                _continue_btn.OnMouseClick(sender, e);
                return;
            }
            base.OnMouseClick(sender, e);
            
        }

        public void PlayerSelection(int choice, string item_str)
        {
            
            _menu_selection = item_str;
        }

        public void HandshakeAvailablePlayers(string[] players)
        {
            _player_window.Resize( _player_window.ClientArea.Width,  _player_menu.LayoutMenuOptions(players, PanelLayout.Vertical));
            _continue_btn.SetClientArea(_player_window.ClientArea.Left,
                _player_window.ClientArea.Bottom + 2,
                _player_window.ClientArea.Right,
                _player_window.ClientArea.Bottom + 27);

        }
        public void HandshakeInitializeGUI()
        {
            Thread t = new Thread(new ParameterizedThreadStart(this.LoadTextures));
            t.Start((object)GameFramework.Instance());
            //DDD_Global.Instance.PlayerID = _menu_selection;
            _player_window.Hide();
            _continue = true;
            _continue_btn.Selected = false;
            _loading_textures = true;
            lock (this)
            {
                Stage = LoadingStage.TEXTURE;
            }

        }

        #region IScene Members

        public override void OnInitializeScene(GameFramework g)
        {
            BindGameController();

            _background.X = 0;
            _background.Y = 0;
            _background.Width = g.CANVAS.Size.Width;
            _background.Height = g.CANVAS.Size.Height;
            
            //string path = System.Environment.CurrentDirectory + @"\ImageLib.dll";
            string path = string.Format(@"\\{0}\DDDClient\ImageLib.dll", DDD_Global.Instance.HostName);
            ImageLib = Assembly.LoadFile(path);
           
            //font_small = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 12));
            font_small = g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 14));

            font_large = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 28));

            bounding_rect = font_large.MeasureString(null, Message, DrawTextFormat.NoClip, Color.Yellow);
            bounding_rect.X = (_background.Width - bounding_rect.Width) / 2;
            bounding_rect.Y = (int)(_background.Height - (2*bounding_rect.Height));

            player_rect.X = (int)((_background.Width - (_background.Width / 4)) / 2);
            player_rect.Y = (int)((_background.Height - (_background.Height / 3)) / 2);
            player_rect.Width = (int)(_background.Width / 4);
            player_rect.Height = (int)( _background.Height / 3);

            _player_window = CreateWindow(g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 12)), "Available Players",
                 player_rect.X,
                 player_rect.Y,
                 player_rect.Right ,
                 player_rect.Bottom);

            _player_window.AllowMove = false;
            _player_window.AllowShade = false;
            _player_window.AllowResize = false;
            _player_window.HasScrollBars = false;
           

            _player_menu = new PanelMenu(
                                                     g.CANVAS.CreateFont(new System.Drawing.Font("MS Sans Serif", 14)),
                                                    new PanelMenuSelectHandler(PlayerSelection)
                                                    );
            _player_menu.BackgroundColor = Color.Black;
             _player_window.BindPanelControl(_player_menu);
           _player_window.Show();
            _player_menu.LayoutMenuOptions(new string[] { "Populating ..." }, PanelLayout.Vertical);

           
           _continue_btn = (PanelStaticButton)new PanelStaticButton(_player_window.ClientArea.Left,
               _player_window.ClientArea.Bottom + 2,
               _player_window.ClientArea.Right,
               _player_window.ClientArea.Bottom + 27);
            _continue_btn.Text = "Continue";
            _continue_btn.BackgroundColor = Color.FromArgb(204, 63, 63, 63);
            _continue_btn.BorderColor = Color.White;
            _continue_btn.Font = font_small;
            _continue_btn.MouseClick = new System.Windows.Forms.MouseEventHandler(this.Btn_Continue);



        }


        public override void OnSceneLoading(GameFramework g)
        {
            if (!DDD_Global.Instance.IsConnected)
            {
                // Assumes we're in Demo mode.
                DDD_Global.Instance.MapName = "Map.jpg";
                DDD_Global.Instance.MapLocation = @"C:\DDDClient\Map.jpg";
                UTM_Mapping.HorizonalMetersPerPixel = 8.49990671075954650f;
                UTM_Mapping.VerticalMetersPerPixel = 8.56718696599567760f;
                DDD_Global.Instance.ScenarioName = "Demo";
                DDD_Global.Instance.ScenarioDescription = "This is a locally connected demo.";
                _menu_selection = "Demo Player";
                HandshakeInitializeGUI();
                SceneMode = MODE.SCENE_RENDER;

                return;
            }
            else
            {

                _commands.HandshakeGUIRegister(DDD_Global.Instance.TerminalID);
                SceneMode = MODE.SCENE_RENDER;
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
                                        if (_start_scenario && DDD_Global.Instance.IsConnected)
                                        {
                                            SceneMode = MODE.SCENE_FINISHED;
                                        }
                                        else
                                            if (!DDD_Global.Instance.IsConnected)
                                            {
                                                SceneMode = MODE.SCENE_FINISHED;
                                            }
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
            canvas.DrawFillRect(_background, _background_color_material);
            canvas.DrawRect(_background, Color.White);
            if (DDD_Global.Instance.IsConnected)
            {
                if (_loading_textures)
                {
                    //player_rect.Height = (font_small.MeasureString(null, DDD_Global.Instance.PlayerBrief, DrawTextFormat.WordBreak, Color.Yellow)).Height;
                    canvas.DrawFillRect(player_rect, _background_window_material);
                    font_small.DrawText(null, DDD_Global.Instance.PlayerBrief, player_rect, DrawTextFormat.WordBreak, Color.Yellow);
                    _continue_btn.SetClientArea(player_rect.Left,
                                                                             player_rect.Bottom + 2,
                                                                             player_rect.Right,
                                                                             player_rect.Bottom + 27);

                    _continue_btn.Text = "Start Game";
                }
                _continue_btn.OnRender(canvas);
            }

            font_small.DrawText(null, Program.Build_ID, 12, 12, Color.Yellow);
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
                    _texture = g.CreateTexture(ImageLib.GetManifestResourceStream(names[i]));
                    g.StoreTexture(names[i], _texture);
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
            try
            {
                 GameTexture Map = g.CreateTexture(DDD_Global.Instance.MapLocation);

                Controller.UTM_Mapping.ImageHeight = Map.height;
                Controller.UTM_Mapping.ImageWidth = Map.width;
                g.StoreTexture("MAP", Map);

            }
            catch (ArgumentException)
            {
                lock (this)
                {
                    SceneMode = MODE.SCENE_ERROR;
                    Message = "Map file missing: " + DDD_Global.Instance.MapName;
                    MsgColor = Color.Red;
                }
                return;
            }

            lock (this)
            {
                Stage = LoadingStage.FINISHED;
            }
        }

 

        #endregion
    }
}
