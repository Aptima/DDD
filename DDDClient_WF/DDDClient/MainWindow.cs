using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;

namespace Aptima.Asim.DDD.Client
{


    public partial class MainWindow : Form, IGameControl, IController
    {
        private ConnectDialog _LoginDialog;
        private GUIController _Controller;
        private Thread _ControllerThread;


        public MainWindow()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeComponent();
            DDD_Global.Instance.TerminalID = System.Environment.MachineName;
            LoginDialog();
        }


        private void LoginDialog()
        {
            bool Continue = true;

            _LoginDialog = new ConnectDialog();
            while (Continue)
            {
                _LoginDialog.ShowDialog();
                switch (_LoginDialog.DialogResult)
                {
                    case DialogResult.Cancel:
                        Continue = false;
                        return;
                    case DialogResult.OK:
                        Continue = false;
                        GameFramework.Instance().Run(this, (IGameControl)this);
                        break;
                }

            }
       
        }

        public void QuitApplication()
        {
            if (_ControllerThread != null)
            {
                if (_ControllerThread.IsAlive)
                {
                    _ControllerThread.Abort();
                }
            }
            try
            {
                if (DDD_Global.Instance.nc.IsConnected())
                {
                    DDD_Global.Instance.nc.Disconnect();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error disconnecting from DecisionMaker");
            }

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            QuitApplication();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

            // When Application is loaded Check to see if the GameFramework is
            // Running, if not then there is a problem and exit.
            if (!GameFramework.IsRunning)
            {
                Application.Exit();
            }
            else
            {
                _ControllerThread = new Thread(new ParameterizedThreadStart(Controller_Thread));
                _ControllerThread.Start(this);
            }
        }

        private void Controller_Thread(object obj)
        {
            if (_Controller != null)
            {
                _Controller.ScenarioControllerLoop((IController)obj);
            }
        }



        #region IGameControl Members

        public Control GetTargetControl()
        {
            return this;
        }

        public void GameOver(GameFramework g)
        {
        }

        public void InitializeCanvasOptions(CanvasOptions options)
        {
            options.Windowed = true;
            options.Shader = ShadeMode.Gouraud;
            options.Device = DeviceType.Hardware;
            options.BackgroundColor = Color.Black;
            options.AmbientColor = Color.White;
            options.BackfaceCulling = Cull.None;
        }

        public void SceneChanged(int scene_number)
        {
        }

        public Scene[] InitializeScenes(GameFramework g)
        {
            //_Controller = null;
            //return new Scene[] { new SandBox(this) };
            //_Controller = null;
            //return new Scene[] { new SplashScene(this) };

            _Controller = new GUIController(DDD_Global.Instance.PlayerID, DDD_Global.Instance.HostName,
                DDD_Global.Instance.nc, DDD_Global.Instance.SimModel);

            DDD_HeadsUpDisplay h = new DDD_HeadsUpDisplay(this, _Controller);
            return new Scene[] { new SplashScene2(this, _Controller), h };
        }


        #endregion


        #region IController Members
        public void ZoomUpdate() { }
        public void ScoreUpdate(string score_name, double score_value) { }

        public void ViewProAttributeUpdate(ViewProAttributeUpdate update)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).AttributeUpdateObjects(update);
            }
        }
        public void ViewProMotionUpdate(ViewProMotionUpdate update)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).MoveUpdateObjects(update);
            }
        }
        public void ViewProInitializeUpdate(ViewProMotionUpdate update)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).InitializeObjects(update);
            }
        }
        public void TextChatRequest(string user_id, string message, string target_id)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).TextChatRequest(user_id, message, target_id);
            }
        }
        public void SystemMessageUpdate(string message, int argbColor)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).SystemMessageUpdate(message, argbColor);
            }
        }
        public void HandshakeAvailablePlayers(string[] players)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is SplashScene2)
            {
                ((SplashScene2)s).HandshakeAvailablePlayers(players);
            }

        }
        public void ViewProStopObjectUpdate(string object_ID)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).ViewProStopObjectUpdate(object_ID);
            }
        }
        public void RemoveObject(string object_id)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).RemoveObject(object_id);
            }
        }
        public void TimeTick(string time)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).TimeTick(time);
            }
        }
        public void HandshakeInitializeGUI()
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is SplashScene2)
            {
                ((SplashScene2)s).HandshakeInitializeGUI();
            }
        }
        public void PauseScenario()
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).PauseGame();
            }
        }
        public void ResumeScenario()
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).ResumeGame();
            }
        }
        /// <summary>
        /// Internal messaging, when a selection happens set it to default mode.
        /// </summary>
        public void SelectionUpdate()
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).Mode = MenuMode.DEFAULT;
                ((DDD_HeadsUpDisplay)s).ResetVunerabilities();
            }
        }

        public void StopScenario()
        {
            MessageBox.Show("Server has stopped the scenario.", "Game Over");
            Application.ExitThread();
        }
        public void AttackUpdate(string attacker, string target, int time, int end_time)
        {
            Scene s = GameFramework.Instance().GetCurrentScene();
            if (s is DDD_HeadsUpDisplay)
            {
                ((DDD_HeadsUpDisplay)s).AttackUpdate(attacker, target, time, end_time);
            }
        }

        #endregion

    }
}