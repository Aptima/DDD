using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Dialogs;
using Aptima.Asim.DDD.Client.Controller;
using Aptima.Asim.DDD.Client.Whiteboard;
using System.Threading;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Xml;


using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
//using Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib;

using System.Deployment;
using System.Deployment.Application;


namespace Aptima.Asim.DDD.Client
{
    public partial class DDD_MainWinFormInterface : Form, IGameControl, IController, IVoiceClientController
    {
        private PrintDocument _PrintDocument = new PrintDocument();
        private ScrollWindow _ObjectiveWindow = null;
        private ConnectDialog _LoginDialog;
        private GUIController _Controller;
        private DM_Dialog _DecisionMakerDialog = null;
        private AuthenticationDialog _AuthenticationDialog = null;
        private TabPage ScoreSummaryPage = new TabPage();
        private WebBrowser ScoreSummary = new WebBrowser();
        private OptionsDialog _optionsDialog = null;

        private int _tab_height = 0;
        private bool _game_over = false;

        private List<string> _ManagedUnits = new List<string>();
        private List<string> _UnmanagedUnits = new List<string>();

        private Thread _ControllerThread;
        private MapPlayfieldContainer _map_scene = null;
        private WinForm_Splash _splash_scene = null;
        private bool _center_map = true;
        private String[] _classificationsEnum = { "", "Good", "Bad", "Ugly" };
        private CustomAttributesDialog _customAttributesDialog = null;
        private VoIPClientControl _voiceClientControl = null;

        private DataTable _AttributeTable = new DataTable("AttributeTable");

        private enum ATTRIBUTES : int { NAME = 0, CLASS = 1, STATUS = 2, LOCATION = 3, ALTITUDE = 4, DESTINATION = 5, SPEED = 6, MAX_SPEED = 7, THROTTLE = 8, FUEL_AMOUNT = 9, FUEL_CAPACITY = 10 };
        private enum OBJECT_CONTROL_TAB : int { MOVE = 0, WEAPONS = 1, SUBPLATFORMS = 2, ADDITIONAL = 3 }

        private delegate void DeselectAllDelegate();
        private delegate void RemoveChatRoomDelegate(string room_name);
        private delegate void SingleBoolDelegate(bool val);
        private delegate void ChangeMapStateDelegate();
        private delegate void ShowScoreSummaryDelegate();
        private delegate void CreateChatRoomDelegate(string tab_name, string room_name, List<string> membership_list);
        private delegate void CreateWhiteboardRoomDelegate(string tab_name, string room_name, List<string> membership_list, string senderDM);
        private delegate void CreateVoiceTabDelegate(string tab_name);
        private delegate void StatusPanelUpdateDelegate(string message);
        private delegate void ChangeControlStateDelegate(Control control, bool state);
        private delegate void ChangeMenuStateDelegate(ToolStripMenuItem item, bool state);
        private delegate void FuelGaugeUpdateDelegate(int value, int maximum);
        private delegate void ThrottleUpdateDelegate(int value);
        private delegate void SetUnitControlIndexDelegate(int index);
        private delegate void LabelUpdateDelegate(Label label, string text);
        private delegate void TextBoxUpdateDelegate(TextBox textbox, string text);
        private delegate void TextBoxReadStatusUpdateDelegate(TextBox textbox, bool isReadOnly);
        private delegate void WeaponsUpdateDelegate(DDDObjects obj);
        private delegate void SubplatformUpdateDelegate(DDDObjects obj);
        private delegate void VulnerabilitiesUpdateDelegate(DDDObjects obj);
        private delegate void UnitFinderUpdateDelegate(string objectid);
        private delegate void UnitFinderUpdateItemsDelegate();
        private delegate void ScoreUpdateDelegate(string score_name, double score_value);
        private delegate void SystemMessageDelegate(string message, int color);
        private delegate void TextChatWindowDelegate(string user_id, string message, string target);
        private delegate void WhiteboardLineDelegate(string object_id, string user_id, int mode, LocationValue start, LocationValue end, double width,
            double originalScale, int color, string text, string target_id);
        private delegate void WhiteboardClearDelegate(string user_id, string target_id);
        private delegate void WhiteboardClearAllDelegate(string user_id, string target_id);
        private delegate void WhiteboardUndoDelegate(string object_id, string user_id, string target_id);
        private delegate void WhiteboardScreenViewDelegate(string user_id, int originX, int originY, int screenSizeWidth,
                                      int screenSizeHeight, double screenZoom);
        private delegate void WhiteboardSyncScreenViewDelegate(string user_id, string target_id, string whiteboard_id);
        private delegate void WhiteboardPopScreenViewDelegate(string whiteboard_id);
        private delegate void NotifyCreatedVoiceChannelDelegate(string channelName, List<string> astrMembershipList);
        private delegate void NotifyClosedVoiceChannelDelegate(string channelName);
        private delegate void NotifyJoinVoiceChannelDelegate(string channelName);
        private delegate void NotifyLeaveVoiceChannelDelegate(string channelName);
        private delegate void NotifyStartedTalkingDelegate(string username, string channelName);
        private delegate void NotifyStoppedTalkingDelegate(string username, string channelName);
        private delegate void NotifyMuteUserDelegate(string username, string channelName);
        private delegate void NotifyUnmuteUserDelegate(string username, string channelName);

        private static string MOVE_TIP = "Use the left mouse button to select a unit, use the right mouse button to move.";
        private static string CAPABILITIES_TIP = "Use right mouse button, or the Unit Finder to select your target.";
        private static string SUBPLATFORM_TIP = "Use right mouse button, or the Unit Finder to select your target.";
        private static string DRAW_TIP = "Use the left mouse button to draw the selected shape.";
        public DDD_MainWinFormInterface()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeComponent();
            DDD_Global.Instance.TerminalID = DDD_Global.Instance.NetClient.TerminalID;
            
            _PrintDocument.PrintPage += new PrintPageEventHandler(_PrintDocument_PrintPage);
            Text = Program.App_Name;
            Application.AddMessageFilter(MessageFilter.Instance);
            SubplatformList.Items.Add("Dock this object");
            checkBoxDisplayRangeFinder.Checked = DDD_Global.Instance.RangeFinderEnabled;
        }


        void _PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.DrawString(SystemMessageBox.Text, new System.Drawing.Font("Arial", 12), new SolidBrush(Color.Black), new PointF(0, 0));
                e.HasMorePages = false;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void LoadConfigDocument()
        {
            StreamReader file = null;
            String shareFolder = "DDDClient";
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"\\"+ApplicationDeployment.CurrentDeployment.UpdateLocation.Host);
                    String p = ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsolutePath;
                    DDD_Global.Instance.ClientPath = p.Remove(p.LastIndexOf("/")).Replace("/", @"\");
                    sb.Append(DDD_Global.Instance.ClientPath);
                    sb.Append(@"\aptima.cfg");
                    MessageBox.Show("Reading DDD Client from: " +sb.ToString());
                    file = new StreamReader(sb.ToString());//string.Format(@"\\{0}\DDDClient\Aptima.cfg", ApplicationDeployment.CurrentDeployment.UpdateLocation.Host));
                }
                else
                {
                    file = new StreamReader(string.Format(@"\\{0}\DDDClient\Aptima.cfg", System.Environment.MachineName));
                }

                DDD_Global.Instance.HostName = file.ReadLine();

                if (!file.EndOfStream)
                {
                    DDD_Global.Instance.Port = Int32.Parse(file.ReadLine());
                }
                else
                {
                    DDD_Global.Instance.HostName = null;
                    DDD_Global.Instance.Port = -1;
                }
                file.Close();
            }
            catch (Exception)
            {
                DDD_Global.Instance.HostName = null;
                DDD_Global.Instance.Port = -1;
                return;
            }
           // DDD_Global.Instance.ClientPath = shareFolder;
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
                        QuitApplication();
                        return;
                    case DialogResult.OK:
                        Continue = false;
                        ReadSimulationModel(DDD_Global.Instance.HostName);
                        //ReadSimulationModel();
                        SubscribeToEvents();
                        GameFramework.Instance().Run(this, (IGameControl)this);
                        _ControllerThread = new Thread(new ParameterizedThreadStart(Controller_Thread));
                        _ControllerThread.Start(this);
                        _DecisionMakerDialog = new DM_Dialog(_Controller);
                        Authenticate();

                        break;
                }
            }
        }

        private void Authenticate()
        {
            try
            {
                if (_AuthenticationDialog == null)
                {
                    _AuthenticationDialog = new AuthenticationDialog(_Controller);
                }
                _AuthenticationDialog.ShowDialog();
                if (_AuthenticationDialog.DialogResult == DialogResult.Cancel)
                {
                    QuitApplication();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void QuitApplication()
        {
            if (_customAttributesDialog != null)
            {
                _customAttributesDialog.Close();
            }
            if (_Controller != null)
            {
                _Controller.InformServerDisconnect = false;
            }
            if (DDD_Global.Instance.IsConnected)
            {
                DDD_Global.Instance.Disconnect();
            }
            else
            {
                if (_Controller != null)
                {
                    _ControllerThread.Abort();
                }
            }
            GameFramework.Instance().ExitGameLoop();
            Application.Exit();
        }



        private void MainWindow_Load(object sender, EventArgs e)
        {
            ChangeMenuState(chatWindowToolStripMenuItem, false);
            ChangeMenuState(whiteboardWindowToolStripMenuItem, false);

            //try
            //{
            //    DDD_Global.Instance.HostName = System.Configuration.ConfigurationSettings.AppSettings["Hostname"];
            //    DDD_Global.Instance.Port = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["Port"]);
            //}
            //catch (Exception)
            //{
            //}
            LoadConfigDocument();

            if (DDD_Global.Instance.HostName == null)
            {
                LoginDialog();
            }
            else
            {
                if (DDD_Global.Instance.Connect(DDD_Global.Instance.HostName, DDD_Global.Instance.Port))
                {
                    ReadSimulationModel(DDD_Global.Instance.HostName);
                    //ReadSimulationModel();
                    SubscribeToEvents();
                    GameFramework.Instance().Run(this, (IGameControl)this);
                    _ControllerThread = new Thread(new ParameterizedThreadStart(Controller_Thread));
                    _ControllerThread.Start(this);
                    _DecisionMakerDialog = new DM_Dialog(_Controller);
                    Authenticate();
                }
                else
                {
                    MessageBox.Show(
                        string.Format("Host ({0}) is unavailable.  If the problem persists, contact your system administrator", DDD_Global.Instance.HostName),
                        "Connect Error");
                    Application.Exit();
                }
            }

            _customAttributesDialog = new CustomAttributesDialog(this);
            //_customAttributesDialog.Show(this);

        }

        private void Controller_Thread(object obj)
        {
            if (_Controller != null)
            {
                _Controller.ScenarioControllerLoop((IController)obj);
            }
        }

        public bool IsCanvasReady()
        {
            return (GameFramework.Instance().CANVAS != null);
        }


        #region IGameControl Members

        public Control GetTargetControl()
        {
            return this.MDX_HostPanel;
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
            try
            {
                switch (scene_number)
                {
                    case 0:
                        // Texture Load Scene             
                        MapScale.Enabled = false;
                        UnitFinder.Enabled = false;
                        UnitFilter.Enabled = false;
                        StatusPanel.Enabled = false;
                        mapoptionsToolStripMenuItem.Enabled = false;
                        _splash_scene = (WinForm_Splash)GameFramework.Instance().GetCurrentScene();
                        break;


                    case 1:
                        // Map Scene
                        _splash_scene = null;
                        UnitFinder_DeselectAll();

                        PlayerID.Text = DDD_Global.Instance.PlayerID;
                        
                        
                        if (DDD_Global.Instance.IsObserver)
                        {
                            Text = Program.App_Name + " OBSERVER";
                        }
                        Scenario_Name.Text = DDD_Global.Instance.ScenarioName;

                        MapScale.Enabled = true;
                        MapScale.Value = 0;
                        StatusPanel.Enabled = true;
                        UnitFinder.Enabled = true;
                        UnitFilter.Enabled = true;
                        UnitFilter.SelectedIndex = 0;
                        UnitControls.Enabled = false;
                        mapoptionsToolStripMenuItem.Enabled = true;

                        Map_Lbl.Text = System.IO.Path.GetFileNameWithoutExtension(DDD_Global.Instance.MapName);

                        _map_scene = (MapPlayfieldContainer)GameFramework.Instance().GetCurrentScene();
                        if (aboveUnitToolStripMenuItem.Checked)
                        {
                            aboveUnitToolStripMenuItem_Click_1(aboveUnitToolStripMenuItem, new EventArgs());
                        }
                        else if (belowUnitToolStripMenuItem.Checked)
                        {
                            belowUnitToolStripMenuItem_Click(belowUnitToolStripMenuItem, new EventArgs());
                        }else if(overlayedToolStripMenuItem.Checked)
                            overlayedToolStripMenuItem_Click(overlayedToolStripMenuItem, new EventArgs());
                        displayUnmanagedUnitLabelToolStripMenuItem_Click(displayUnmanagedUnitLabelToolStripMenuItem, new EventArgs());
                        break;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public Scene[] InitializeScenes(GameFramework g)
        {
            try
            {
                _Controller = new GUIController(DDD_Global.Instance.PlayerID, DDD_Global.Instance.HostName,
                    DDD_Global.Instance.SimModel);


                return new Scene[] { new WinForm_Splash(this, _Controller), new MapPlayfieldContainer(this, _Controller) };
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        #endregion


        #region WinForm
        public static void OpenWebBrowser(string filePath)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = filePath;
                process.Start();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void AboutWindow()
        {
            MessageBox.Show(string.Format("Dynamic Distributed Decision-Making Ver {0}\r\nCompile Date: {1}\r\nFor more info: http://www.aptima.com/a-sim.php", Program.Build_ID, Program.Build_Date), "About Aptima DDD 4.2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ContactWindow()
        {
            MessageBox.Show("Support e-mail address: support@aptima.com\r\nSupport phone number: 866.461.7298", "Aptima DDD Support Information");
        }

        private void ProductHelpWindow()
        {
            //bool success = true;
            //try
            //{
            //    System.Net.IPHostEntry GetIPHost = System.Net.Dns.GetHostEntry(@"dddweb.aptima.com");
            //}
            //catch
            //{
            //    success = false;
            //}

            //if (success)
            //{
            //    OpenWebBrowser(@"http://dddweb.aptima.com/modules/news/");
            //}
            //else
            //{
            //MessageBox.Show("Unable to reach online help, looking on local machine for help page");
            string exePath = string.Empty;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                exePath = String.Format("{0}help\\help.html", DDD_Global.Instance.DDDClientShareFolder);
            }
            else
            {
                exePath = String.Format("{0}help\\help.html", DDD_Global.Instance.DDDClientShareFolder);
            }

            if (File.Exists(exePath))
            {
                OpenWebBrowser(exePath);
            }
            else
                if (!File.Exists(exePath))
                {
                    MessageBox.Show("Missing the Aptima DDD 4.2 help file.  Please try using the internet version, or contact Aptima for a replacement file.");
                }
            //}
        }

        private void UnitFinder_DeselectAll()
        {
            if (!InvokeRequired)
            {
                try
                {
                    UnitFinder.SelectedIndex = -1;
                    MDX_HostPanel.Cursor = Cursors.Default;
                    StatusPanelUpdate(MOVE_TIP);
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new DeselectAllDelegate(UnitFinder_DeselectAll));
            }
        }
        private void StatusPanelUpdate(string message)
        {
            if (!InvokeRequired)
            {
                try
                {
                    ApplicationStatus.Text = message;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new StatusPanelUpdateDelegate(StatusPanelUpdate), message);
            }
        }
        private void ChangeControlState(Control control, bool state)
        {
            if (!InvokeRequired)
            {
                try
                {
                    control.Enabled = state;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new ChangeControlStateDelegate(ChangeControlState), control, state);
            }
        }
        private void ChangeMenuState(ToolStripMenuItem item, bool state)
        {
            if (!InvokeRequired)
            {
                try
                {
                    item.Enabled = state;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new ChangeMenuStateDelegate(ChangeMenuState), item, state);
            }
        }
        private void UnitFinder_Select(string objectid)
        {
            if (!InvokeRequired)
            {
                try
                {
                    if (UnitFinder.Items.Count == 0)
                    {
                        UnitFinder_UpdateItems();
                    }
                    if (!UnitFinder.DroppedDown)
                    {
                        if (UnitFinder.Text != objectid)
                        {
                            _center_map = false;
                            UnitFinder.Text = objectid;
                            _center_map = true;
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new UnitFinderUpdateDelegate(UnitFinder_Select), objectid);
            }
        }


        private void UnitFinder_UpdateItems()
        {
            if (!InvokeRequired)
            {
                try
                {
                    string selected = (string)UnitFinder.SelectedItem;
                    UnitFinder.Items.Clear();
                    if (UnitFilter.SelectedIndex == 0)
                    {
                        UnitFinder.Items.AddRange(_ManagedUnits.ToArray());
                    }
                    else
                    {
                        UnitFinder.Items.AddRange(_UnmanagedUnits.ToArray());
                    }
                    UnitFinder.SelectedItem = selected;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new UnitFinderUpdateItemsDelegate(UnitFinder_UpdateItems));
            }
        }

        private float ScaleMap()
        {
            float quant = 0.0f;
            try
            {
                if (_map_scene != null)
                {
                    float prev_scale = 0.0f;
                    float minscale = 0.0f;

                    float position_ratio = 0.0f;
                    float maxZoom = 0.0f;

                    //AD: 7-30: This section was modified to handle increased zooming
                    DDDObjects obj = _map_scene.GetSelectedObject();
                    maxZoom = _map_scene.MaxZoom;
                    prev_scale = _map_scene.Scale;
                    minscale = _map_scene.GetMinScale();
                    quant = (((maxZoom - minscale) / (MapScale.Maximum)) * MapScale.Value) + minscale;
                    position_ratio = quant / prev_scale;
                    _map_scene.SetMapScale(quant);

                    if (obj != null)
                    {
                        _map_scene.CenterMapToUnit(obj.Position.X * quant, obj.Position.Y * quant);
                    }
                    else
                    {
                        if (prev_scale <= quant && MapScale.Value == 1)
                        {//this is if zooming in, and from full map to first zoom in.
                            _map_scene.CenterMapToUnit(_map_scene.MapTextureWidth * quant * .5f, _map_scene.MapTextureHeight * quant * .5f);
                        }
                        else
                        {
                            _map_scene.CenterMapToUnit(
                                (float)((position_ratio * _map_scene.Position.X * -1) + (position_ratio * .5 * MDX_HostPanel.ClientRectangle.Width)),
                                (float)((position_ratio * _map_scene.Position.Y * -1) + (position_ratio * .5 * MDX_HostPanel.ClientRectangle.Height))
                                );
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return quant * 100;
        }

        private void MapScale_ValueChanged(object sender, EventArgs e)
        {
            int zoomPercent = Convert.ToInt32(ScaleMap());
            SetZoomPercentageLabel(zoomPercent);
        }

        private void SetZoomPercentageLabel(int percentToDisplay)
        {
            labelZoomPercent.Text = string.Format("{0}%", percentToDisplay);
        }

        private void DDD_MainWinFormInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure?", "Exit the DDD", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    QuitApplication();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }



        private void MDX_HostPanel_Resize(object sender, EventArgs e)
        {
            //try
            //{
            //    if (_map_scene != null)
            //    {
            //        _map_scene.RecalculateMinumum(MDX_HostPanel.ClientRectangle);
            //        //ScaleMap();
            //        float prev_scale = _map_scene.Scale;
            //        float minscale = _map_scene.GetMinScale();
            //        float quant = (((1 - minscale) / (MapScale.Maximum)) * MapScale.Value) + minscale;

            //        float position_ratio = quant / prev_scale;
            //        _map_scene.SetMapScale(quant);
            //        if (minscale == 1)
            //        {
            //            MapScale.Enabled = false;
            //        }
            //        else
            //        {
            //            MapScale.Enabled = true;
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    throw new Exception(exc.Message);
            //}
            try
            {
                if (_map_scene != null)
                {
                    _map_scene.RecalculateMinumum(MDX_HostPanel.ClientRectangle);
                    //ScaleMap();
                    float maxZoom = _map_scene.MaxZoom;
                    float prev_scale = _map_scene.Scale;
                    float minscale = _map_scene.GetMinScale();
                    float quant = (((maxZoom - minscale) / (MapScale.Maximum)) * MapScale.Value) + minscale;

                    float position_ratio = quant / prev_scale;
                    _map_scene.SetMapScale(quant);
                    if (minscale == maxZoom)
                    {
                        MapScale.Enabled = false;
                    }
                    else
                    {
                        MapScale.Enabled = true;
                    }
                    SetZoomPercentageLabel(Convert.ToInt32(quant * 100));
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            //int zoomPercent = Convert.ToInt32(ScaleMap());
            //SetZoomPercentageLabel(zoomPercent);
        }
        #endregion



        #region IController Members

        public void AuthenticationResponse(bool authentication_result, string message)
        {
            try
            {
                if (!authentication_result)
                {
                    MessageBox.Show(message, "Authentication Failure");
                    Authenticate();
                    return;
                }
                _Controller.HandshakeGUIRegister(DDD_Global.Instance.TerminalID);

            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void ZoomUpdate()
        {

        }

        public void ScoreUpdate(string score_name, double score_value)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (ScoreList.Items.ContainsKey(score_name))
                        {
                            ScoreList.Items[score_name].SubItems[0].Text =
                                string.Format("{0} : {1}", score_name, score_value.ToString());
                        }
                        else
                        {
                            ScoreList.Items.Add(score_name, string.Format("{0} : {1}", score_name, score_value.ToString()), -1);
                        }
                    }
                    else
                    {
                        BeginInvoke(new ScoreUpdateDelegate(ScoreUpdate), score_name, score_value);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void HandshakeAvailablePlayers(string[] players)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_splash_scene != null)
                    {
                        _DecisionMakerDialog.HandshakeAvailablePlayers(players);
                        _DecisionMakerDialog.ShowDialog();

                        if (_DecisionMakerDialog.SelectedItem != string.Empty)
                        {
                            _Controller.HandshakeGUIRoleRequest((string)_DecisionMakerDialog.SelectedItem, DDD_Global.Instance.TerminalID);
                            _DecisionMakerDialog.Hide();

                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void HandshakeInitializeGUI()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_splash_scene != null)
                    {
                        _DecisionMakerDialog.Hide();
                        ScenarioObjectiveWindow(true);
                        _splash_scene.HandshakeInitializeGUI();
                        //CreateChatRoom("chat - All", "Broadcast", DDD_Global.Instance.DM_List);
                        ChangeMenuState(chatWindowToolStripMenuItem, true);
                        ChangeMenuState(whiteboardWindowToolStripMenuItem, true);
                        if (DDD_Global.Instance.VoiceChatEnabled)
                        {
                            // Create new voice tab
                            CreateVoiceTab("Voice");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void SetClassifications(List<string> classificationEnumeration)
        { 
            classificationEnumeration.Insert(0,"");
            this._classificationsEnum = classificationEnumeration.ToArray();
        }

        public void ViewProAttributeUpdate(ViewProAttributeUpdate update)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.AttributeUpdateObjects(update);
                        DDDObjects selected = _map_scene.GetSelectedObject();
                        if (selected != null)
                        {
                            if (selected.ObjectID == update.ObjectId)
                            {
                                if (!selected.IsWeapon)
                                {
                                    PopulateObjectAttributes(selected);
                                }
                                else
                                {
                                    PopulateObjectAttributes(null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        public void ViewProMotionUpdate(ViewProMotionUpdate update)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.MoveUpdateObjects(update);
                        DDDObjects selected = _map_scene.GetSelectedObject();
                        if (selected != null)
                        {
                            if (selected.ObjectID == update.ObjectId)
                            {
                                selected.ThrottleStr = string.Format("{0} %", selected.Throttle * 100);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        public void ViewProInitializeUpdate(ViewProMotionUpdate update)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        lock (this)
                        {
                            if (DDD_Global.Instance.PlayerID == update.OwnerID)
                            {
                                if (!_ManagedUnits.Contains(update.ObjectId) && !update.IsWeapon)
                                {
                                    _ManagedUnits.Add(update.ObjectId);
                                    Console.WriteLine(String.Format("Owned asset initialized: {0}", update.ObjectId));
                                }
                            }
                            else
                            {
                                if (!_UnmanagedUnits.Contains(update.ObjectId) && !update.IsWeapon)
                                {
                                    _UnmanagedUnits.Add(update.ObjectId);
                                    Console.WriteLine(String.Format("Un-owned asset initialized: {0}", update.ObjectId));
                                }
                            }
                        }
                        _map_scene.InitializeObjects(update);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message + ":" + exc.StackTrace);
            }
        }


        public void TextChatRequest(string user_id, string message, string target_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (TabControls.TabPages.ContainsKey(target_id))
                        {
                            if (TabControls.TabPages[target_id].Controls[0] is ChatDialog)
                            {
                                ((ChatDialog)TabControls.TabPages[target_id].Controls[0]).WriteLine(message);
                            }
                        }
                        else
                        {
                            SystemMessageUpdate(string.Format("Received message for non-existent chat window: {0}", target_id));
                        }

                    }
                    else
                    {
                        BeginInvoke(new TextChatWindowDelegate(TextChatRequest), user_id, message, target_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void CreateChatRoom(string tab_name, string room_name, List<string> membership_list)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        try
                        {
                            if (_Controller != null && !(TabControls.TabPages.ContainsKey(room_name)))
                            {
                                ChatDialog c = new ChatDialog(_Controller);
                                c.GroupId = tab_name;
                                c.Channel = room_name;
                                c.Members = membership_list;
                                c.AllowPropertyChanges = false;
                                TabControls.TabPages.Add(room_name, tab_name);
                                TabControls.TabPages[room_name].Controls.Add(c);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Unable to create chat room.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        BeginInvoke(new CreateChatRoomDelegate(CreateChatRoom), tab_name, room_name, membership_list);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void FailedToCreateChatRoom(string message, string sender_id)
        {
            try
            {
                MessageBox.Show(message, "Unable to create chat room.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void CloseChatRoom(string room_name)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        try
                        {
                            TabControls.TabPages.RemoveByKey(room_name);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Unable to close chat room.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        BeginInvoke(new RemoveChatRoomDelegate(CloseChatRoom), room_name);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void CreateWhiteboardRoom(string tab_name, string room_name, List<string> membership_list, string senderDM)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        try
                        {
                            if (_Controller != null && !(TabControls.TabPages.ContainsKey(room_name)))
                            {
                                // Create new whiteboard room
                                WhiteboardRoom wbRoom = new WhiteboardRoom(room_name, membership_list, _Controller);

                                // Create new whiteboard tab
                                WhiteboardDialog c = new WhiteboardDialog(this, _Controller, wbRoom);
                                c.GroupId = tab_name;
                                c.Channel = room_name;
                                c.Members = membership_list;
                                c.AllowPropertyChanges = false;
                                c.EnableRoomOwnerControls(senderDM == DDD_Global.Instance.PlayerID);
                                TabControls.TabPages.Add(room_name, tab_name);
                                TabControls.TabPages[room_name].Controls.Add(c);

                                // Add this whiteboard to the other whiteboard rooms listbox
                                foreach (TabPage tabPage in TabControls.TabPages)
                                {
                                    if (tabPage.Controls[0] is WhiteboardDialog)
                                    {
                                        if (((WhiteboardDialog)tabPage.Controls[0]).WBRoom != null)
                                        {
                                            WhiteboardRoom otherWBRoom = ((WhiteboardDialog)tabPage.Controls[0]).WBRoom;

                                            if (string.Compare(otherWBRoom.Name, room_name) != 0)
                                            {
                                                // Add new room to listbox
                                                ((WhiteboardDialog)tabPage.Controls[0]).AddOtherRoom(room_name, wbRoom);

                                                // Add existing room to new room's listbox
                                                c.AddOtherRoom(otherWBRoom.Name, otherWBRoom);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Unable to create whiteboard room.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        BeginInvoke(new CreateWhiteboardRoomDelegate(CreateWhiteboardRoom), tab_name, room_name, membership_list, senderDM);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void CreateVoiceTab(string tab_name)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        try
                        {
                            if (!(TabControls.TabPages.ContainsKey(tab_name)))
                            {
                                // Create new voice tab
                                _voiceClientControl = new VoIPClientControl();

                                _voiceClientControl.initialize(_Controller,
                                    DDD_Global.Instance.PlayerID,
                                    DDD_Global.Instance.VoiceServerHostname,
                                    DDD_Global.Instance.VoiceServerPort,
                                    DDD_Global.Instance.ConaitoServerPassword);

                                // Add the tab
                                TabControls.TabPages.Add(tab_name, tab_name);
                                TabControls.TabPages[tab_name].Controls.Add(_voiceClientControl);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Unable to create voice tab.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        BeginInvoke(new CreateVoiceTabDelegate(CreateVoiceTab), tab_name);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardLine(string object_id, string user_id, int mode, LocationValue start, LocationValue end, double width,
            double originalScale, int color, string text, string target_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (TabControls.TabPages.ContainsKey(target_id))
                        {
                            if (TabControls.TabPages[target_id].Controls[0] is WhiteboardDialog)
                            {
                                if (((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom != null)
                                {
                                    WhiteboardRoom wbRoom = ((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom;
                                    wbRoom.AddLine(object_id, (DrawModes) mode, start, end, width, originalScale, color, text);
                                }
                            }
                        }
                        else
                        {
                            SystemMessageUpdate(string.Format("Received message for non-existent whiteboard window: {0}", target_id));
                        }

                    }
                    else
                    {
                        BeginInvoke(new WhiteboardLineDelegate(WhiteboardLine), object_id, user_id, mode, start, end, width,
                            originalScale, color, text, target_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardClear(string user_id, string target_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (TabControls.TabPages.ContainsKey(target_id))
                        {
                            if (TabControls.TabPages[target_id].Controls[0] is WhiteboardDialog)
                            {
                                if (((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom != null)
                                {
                                    WhiteboardRoom wbRoom = ((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom;
                                    wbRoom.Clear(user_id);
                                }
                            }
                        }
                        else
                        {
                            SystemMessageUpdate(string.Format("Received message for non-existent whiteboard window: {0}", target_id));
                        }

                    }
                    else
                    {
                        BeginInvoke(new WhiteboardClearDelegate(WhiteboardClear), user_id, target_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardClearAll(string user_id, string target_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (TabControls.TabPages.ContainsKey(target_id))
                        {
                            if (TabControls.TabPages[target_id].Controls[0] is WhiteboardDialog)
                            {
                                if (((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom != null)
                                {
                                    WhiteboardRoom wbRoom = ((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom;
                                    wbRoom.ClearAll();
                                }
                            }
                        }
                        else
                        {
                            SystemMessageUpdate(string.Format("Received message for non-existent whiteboard window: {0}", target_id));
                        }

                    }
                    else
                    {
                        BeginInvoke(new WhiteboardClearAllDelegate(WhiteboardClearAll), user_id, target_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardUndo(string object_id, string user_id, string target_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        if (TabControls.TabPages.ContainsKey(target_id))
                        {
                            if (TabControls.TabPages[target_id].Controls[0] is WhiteboardDialog)
                            {
                                if (((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom != null)
                                {
                                    WhiteboardRoom wbRoom = ((WhiteboardDialog)TabControls.TabPages[target_id].Controls[0]).WBRoom;
                                    wbRoom.Undo(object_id);
                                }
                            }
                        }
                        else
                        {
                            SystemMessageUpdate(string.Format("Received message for non-existent whiteboard window: {0}", target_id));
                        }

                    }
                    else
                    {
                        BeginInvoke(new WhiteboardUndoDelegate(WhiteboardUndo), object_id, user_id, target_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardScreenView(string user_id, int originX, int originY, int screenSizeWidth,
            int screenSizeHeight, double screenZoom)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        foreach (TabPage page in TabControls.TabPages)
                        {
                            if (page.Controls[0] is WhiteboardDialog)
                            {
                                WhiteboardDialog whiteboardDialog = (WhiteboardDialog)page.Controls[0];
                                if (whiteboardDialog.WBRoom != null)
                                {
                                    WhiteboardRoom wbRoom = whiteboardDialog.WBRoom;
                                    if (wbRoom.UpdateScreenView(user_id, originX, originY, screenSizeWidth,
                                      screenSizeHeight, screenZoom) &&
                                      (string.Compare(user_id, DDD_Global.Instance.PlayerID) != 0))
                                    {
                                        // Need to add a new user to the player list drop down
                                        whiteboardDialog.AddViewUser(user_id);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        BeginInvoke(new WhiteboardScreenViewDelegate(WhiteboardScreenView), user_id, originX, originY, screenSizeWidth,
                                      screenSizeHeight, screenZoom);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardSyncScreenView(string user_id, string target_id, string whiteboard_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        foreach (TabPage page in TabControls.TabPages)
                        {
                            if (page.Controls[0] is WhiteboardDialog)
                            {
                                WhiteboardDialog whiteboardDialog = (WhiteboardDialog)page.Controls[0];
                                if ((whiteboardDialog.WBRoom != null) &&
                                    (string.Compare(whiteboardDialog.WBRoom.Name, whiteboard_id) == 0) &&
                                    (string.Compare(user_id, DDD_Global.Instance.PlayerID)== 0))
                                {
                                    WhiteboardRoom wbRoom = whiteboardDialog.WBRoom;
                                    int originX = 0;
                                    int originY = 0;
                                    int screenSizeWidth = 0;
                                    int screenSizeHeight = 0;
                                    double screenZoom = 0.0;

                                    // Sync this users view to the targets view
                                    if (wbRoom.GetScreenViewInfo(target_id, ref originX, ref originY, ref screenSizeWidth, ref screenSizeHeight,
                                        ref screenZoom))
                                    {
                                        float maxZoom = _map_scene.MaxZoom;
                                        float minscale = _map_scene.GetMinScale();
                                        int newMapScale = (int)((screenZoom - minscale) / ((maxZoom - minscale) / ((double) MapScale.Maximum)) + 0.5);
                                        Vector2 _metersPerPixelValues = new Vector2();
                                        _metersPerPixelValues.X = UTM_Mapping.HorizonalMetersPerPixel;
                                        _metersPerPixelValues.Y = UTM_Mapping.VerticalMetersPerPixel;

                                        // Save the current view information
                                        wbRoom.SaveScreenView();

                                        // Set Zoom Level
                                        _map_scene.SetMapScale((float) screenZoom);
                                        MapScale.Value = newMapScale;
                                        SetZoomPercentageLabel(Convert.ToInt32(screenZoom * 100));

                                        // Set the position
                                        _map_scene.SetMapPosition((int)-((double) originX * screenZoom / _metersPerPixelValues.X),
                                            (int)-((double) originY * screenZoom / _metersPerPixelValues.Y));                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        BeginInvoke(new WhiteboardSyncScreenViewDelegate(WhiteboardSyncScreenView), user_id, target_id, whiteboard_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void WhiteboardPopScreenView(string whiteboard_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        foreach (TabPage page in TabControls.TabPages)
                        {
                            if (page.Controls[0] is WhiteboardDialog)
                            {
                                WhiteboardDialog whiteboardDialog = (WhiteboardDialog)page.Controls[0];
                                if ((whiteboardDialog.WBRoom != null) &&
                                    (string.Compare(whiteboardDialog.WBRoom.Name, whiteboard_id) == 0))
                                {
                                    WhiteboardRoom wbRoom = whiteboardDialog.WBRoom;
                                    int originX = 0;
                                    int originY = 0;
                                    int screenSizeWidth = 0;
                                    int screenSizeHeight = 0;
                                    double screenZoom = 0.0;

                                    // Get a previous view in the view stack                    
                                    if (wbRoom.PopScreenViewInfo(ref originX, ref originY, ref screenSizeWidth, ref screenSizeHeight,
                                        ref screenZoom))
                                    {
                                        float maxZoom = _map_scene.MaxZoom;
                                        float minscale = _map_scene.GetMinScale();
                                        int newMapScale = (int)((screenZoom - minscale) / ((maxZoom - minscale) / ((double)MapScale.Maximum)) + 0.5);
                                        Vector2 _metersPerPixelValues = new Vector2();
                                        _metersPerPixelValues.X = UTM_Mapping.HorizonalMetersPerPixel;
                                        _metersPerPixelValues.Y = UTM_Mapping.VerticalMetersPerPixel;

                                        // Set Zoom Level
                                        _map_scene.SetMapScale((float)screenZoom);
                                        MapScale.Value = newMapScale;
                                        SetZoomPercentageLabel(Convert.ToInt32(screenZoom * 100));

                                        // Set the position
                                        _map_scene.SetMapPosition((int)-((double)originX * screenZoom / _metersPerPixelValues.X),
                                            (int)-((double)originY * screenZoom / _metersPerPixelValues.Y));

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        BeginInvoke(new WhiteboardPopScreenViewDelegate(WhiteboardPopScreenView), whiteboard_id);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void SystemMessageUpdate(string message)
        {
            try
            {
                if (!IsDisposed)
                {
                    SystemMessageUpdate(message, Color.Black.ToArgb());
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void SystemMessageUpdate(string message, int argbColor)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (!InvokeRequired)
                    {
                        Color color = Color.FromArgb(argbColor);
                        SystemMessageBox.SelectionColor = color;
                        SystemMessageBox.SelectedText += message + "\n";
                        SystemMessageBox.ScrollToCaret();
                    }
                    else
                    {
                        BeginInvoke(new SystemMessageDelegate(SystemMessageUpdate), message, argbColor);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        public void ActiveRegionUpdate(string object_id, bool visible, int color, List<CustomVertex.TransformedColored> points)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.ActiveRegionUpdate(object_id, visible, color, points);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("ActiveRegionUpdate: " + e.Message + ":" + e.StackTrace);
            }
        }

        public void ViewProStopObjectUpdate(string object_ID)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.ViewProStopObjectUpdate(object_ID);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void RemoveObject(string object_id)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        lock (this)
                        {
                            if (_ManagedUnits.Contains(object_id))
                            {
                                _ManagedUnits.Remove(object_id);
                            }
                            if (_UnmanagedUnits.Contains(object_id))
                            {
                                _UnmanagedUnits.Remove(object_id);
                            }
                        }
                        _map_scene.RemoveObject(object_id);
                        try
                        {
                            SelectionUpdate(); //this will clear object panel
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0}:{1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void TimeTick(string time)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.TimeTick(time);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void PauseScenario()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.PauseGame();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void ResumeScenario()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.ResumeGame();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void ChangeMapState()
        {
            try
            {
                if (!IsDisposed && _map_scene != null)
                {
                    if (!InvokeRequired)
                    {
                        switch (_map_scene.MapState)
                        {
                            case MapModes.MOVE:
                                MDX_HostPanel.Cursor = Cursors.Default;
                                StatusPanelUpdate(MOVE_TIP);
                                /* //AD: test
                                CapabilitiesList.SelectedIndex = -1;
                                SubplatformList.SelectedIndex = -1;
                                 */
                                break;
                            case MapModes.ATTACK:
                                MDX_HostPanel.Cursor = Cursors.Cross;
                                StatusPanelUpdate(CAPABILITIES_TIP);
                                break;
                            case MapModes.SUBPLATFORM:
                                MDX_HostPanel.Cursor = Cursors.Cross;
                                StatusPanelUpdate(SUBPLATFORM_TIP);
                                break;
                            case MapModes.DRAW:
                                MDX_HostPanel.Cursor = Cursors.Cross;
                                StatusPanelUpdate(DRAW_TIP);
                                break;
                        }
                    }
                    else
                    {
                        BeginInvoke(new ChangeMapStateDelegate(ChangeMapState));
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void ShowScoreSummary()
        {
            try
            {
                if (!InvokeRequired)
                {
                    _game_over = true;
                    MapMessageContainer.Panel2Collapsed = false;
                    MapMessageContainer.Panel1Collapsed = true;
                    SimulationSplitContainer.Panel1Collapsed = true;
                    newToolStripMenuItem.Enabled = false;
                    viewToolStripMenuItem.Enabled = false;
                    mapoptionsToolStripMenuItem.Enabled = false;
                    printScoreToolStripMenuItem.Enabled = true;
                    printPreviewScoreToolStripMenuItem.Enabled = true;

                    foreach (TabPage page in TabControls.TabPages)
                    {
                        TabControls.TabPages.Remove(page);
                    }

                    if (File.Exists(string.Format("{0}ScoreSummary.htm", DDD_Global.Instance.DDDClientShareFolder)))
                    {
                        string url = string.Format("file://{0}/{1}/ScoreSummary.htm", DDD_Global.Instance.HostName, DDD_Global.Instance.ClientPath);
                        StatusPanelUpdate("The simulation has concluded, you may exit the application at any time.");
                        ScoreSummaryPage.Text = "Score Summary";
                        ScoreSummary.Dock = DockStyle.Fill;
                        ScoreSummaryPage.Controls.Add(ScoreSummary);
                        TabControls.TabPages.Add(ScoreSummaryPage);
                        TabControls.SelectedIndex = 1;
                        ScoreSummary.Url = new Uri(url);
                    }
                    else
                    {
                        MessageBox.Show("Unable to locate ScoreSummary.htm", "Scoring Error");
                    }
                }
                else
                {
                    BeginInvoke(new ShowScoreSummaryDelegate(ShowScoreSummary));
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

        }

        /// <summary>
        /// Internal messaging, when a selection happens set it to default mode.
        /// </summary>
        public void SelectionUpdate()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        ChangeMapState();
                        DDDObjects obj = _map_scene.GetSelectedObject();
                        if (obj != null)
                        {
                            SendSelectedObjectEvent(obj.ObjectID);
                            if (!obj.IsWeapon)
                            {
                                UnitFinder_Select(obj.ObjectID);
                                PopulateObjectAttributes(obj);
                                //ThrottleUpdate((int)(obj.Throttle * ThrottleControl.Maximum));
                            }
                            else
                            {
                                PopulateObjectAttributes(null);
                            }
                        }
                        else
                        {
                            SendSelectedObjectEvent(string.Empty);
                            UnitFinder_DeselectAll();
                            PopulateObjectAttributes(null);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        public void StopScenario()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.PauseRendering = true;
                    }
                    MessageBox.Show("Server has stopped the scenario.", "Game Over");
                    ShowScoreSummary();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void StopReplay()
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.PauseRendering = true;
                    }
                    MessageBox.Show("Server has stopped the scenario replay.", "Replay Over");
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }




        public void AttackUpdate(string attacker, string target, int time, int end_time)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_map_scene != null)
                    {
                        _map_scene.AttackUpdate(attacker, target, time, end_time);
                        ChangeMapState();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        #endregion

        private void UpdateLabel(Label label, string text)
        {
            if (!InvokeRequired)
            {
                try
                {
                    label.Text = text;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new LabelUpdateDelegate(UpdateLabel), label, text);
            }
        }

        private void UpdateTextBox(TextBox textbox, string text)
        {
            if (!InvokeRequired)
            {
                try
                {
                    textbox.Text = text;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new TextBoxUpdateDelegate(UpdateTextBox), textbox, text);
            }
        }

        private void SetTextboxReadStatus(TextBox textbox, bool isReadOnly)
        {
            if (!InvokeRequired)
            {
                try
                {
                    textbox.ReadOnly = isReadOnly;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new TextBoxReadStatusUpdateDelegate(SetTextboxReadStatus), textbox, isReadOnly);
            }
        }

        private void SensorsUpdate(DDDObjects obj)
        {
            if (!InvokeRequired)
            {
                try
                {
                    if (obj == null)
                    {
                        return;
                    }
                    string current = obj.CurrentlySelectedSensor;
                    int count = 0;
                    int selectedIndex = -1;
                    listBoxSensors.Items.Clear();

                    if (obj != null)
                    {
                        if (obj.Sensors != null)
                        {
                            if (obj.Sensors.Length > 0)
                            {
                                listBoxSensors.Items.AddRange(obj.Sensors);
                                foreach (object ob in listBoxSensors.Items)
                                {
                                    if (ob.ToString() == current)
                                    {
                                        selectedIndex = count;
                                    }
                                    count++;
                                }
                                //return;
                            }
                        }
                    }
                    if (listBoxSensors.Items.Count == 0)
                    {
                        listBoxSensors.Items.Add("No Detected Sensors.");
                    }
                    listBoxSensors.SelectedIndex = selectedIndex;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new VulnerabilitiesUpdateDelegate(SensorsUpdate), obj);
            }
        }

        private void VulnerabilitiesUpdate(DDDObjects obj)
        {
            if (!InvokeRequired)
            {
                try
                {
                    if (obj == null)
                    {
                        return; //weird exception.
                    }
                    string current = obj.CurrentlySelectedVulnerability;
                    int count = 0;
                    int selectedIndex = -1;
                    VulnerabilitiesList.Items.Clear();

                    if (obj != null)
                    {
                        if (obj.Vulnerabilities != null)
                        {
                            if (obj.Vulnerabilities.Length > 0)
                            {
                                VulnerabilitiesList.Items.AddRange(obj.Vulnerabilities);
                                foreach (object ob in VulnerabilitiesList.Items)
                                {
                                    if (ob.ToString() == current)
                                    {
                                        selectedIndex = count;
                                    }
                                    count++;
                                }
                                //return;
                            }
                        }
                    }
                    if (VulnerabilitiesList.Items.Count == 0)
                    {
                        VulnerabilitiesList.Items.Add("No Detected Vulnerabilities.");
                    }
                    VulnerabilitiesList.SelectedIndex = selectedIndex;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new VulnerabilitiesUpdateDelegate(VulnerabilitiesUpdate), obj);
            }
        }

        private static bool CapabilityListUpdating = false;
        private void WeaponsUpdate(DDDObjects obj)
        {
            if (!InvokeRequired)
            {
                CapabilityListUpdating = true;
                try
                {
                    if (obj != null)
                    {
                        if (obj.CapabilityAndWeapons != null)
                        {
                            if (obj.CapabilityAndWeapons.Length != CapabilitiesList.Items.Count)
                            {
                                string previouslySelected = string.Empty;
                                CapabilitiesList.SelectedIndex = -1;
                                //if (CapabilitiesList.SelectedItem != null)
                                //{
                                //    previouslySelected = CapabilitiesList.SelectedItem.ToString();
                                //    if (previouslySelected.LastIndexOf('(') > 0)
                                //    {
                                //        previouslySelected = previouslySelected.Remove(previouslySelected.LastIndexOf('(')); //weapons
                                //    }
                                //}
                                //else
                                //{
                                previouslySelected = obj.CurrentlySelectedCapability;
                                //}
                                CapabilitiesList.Items.Clear();
                                CapabilitiesList.Items.AddRange(obj.CapabilityAndWeapons);

                                if (previouslySelected != string.Empty)
                                { //find and select the previous item
                                    int index = 0;
                                    bool isDone = false;
                                    while (!isDone && index < CapabilitiesList.Items.Count)
                                    {
                                        if (CapabilitiesList.Items[index].ToString().StartsWith(previouslySelected))
                                        {
                                            CapabilitiesList.SelectedIndex = index;
                                            isDone = true;
                                        }
                                        index++;
                                    }

                                }
                                return;
                            }
                            else
                            {
                                int previousSelectedIndex = -1; // CapabilitiesList.SelectedIndex;
                                CapabilitiesList.SelectedIndex = -1;
                                string previouslySelectedValue = obj.CurrentlySelectedCapability;
                                /*string.Empty;
                            if (CapabilitiesList.SelectedItem != null)
                            {
                                previouslySelectedValue = CapabilitiesList.SelectedItem.ToString();
                            }
                                 */

                                for (int i = 0; i < obj.CapabilityAndWeapons.Length; i++)
                                {
                                    CapabilitiesList.Items[i] = obj.CapabilityAndWeapons[i];
                                    if (previouslySelectedValue == obj.CapabilityAndWeapons[i])
                                    {
                                        previousSelectedIndex = i;
                                    }
                                }

                                if (previousSelectedIndex >= 0)
                                {
                                    string str = CapabilitiesList.Items[previousSelectedIndex].ToString();

                                    if (previouslySelectedValue.Contains("(") && str.Contains("("))
                                    {

                                        if (str.Substring(0, (str.LastIndexOf('('))) ==
                                            previouslySelectedValue.Substring(0, (previouslySelectedValue.LastIndexOf('(')))) //weapons
                                        {
                                            CapabilitiesList.SelectedIndex = previousSelectedIndex;
                                        }

                                    }
                                    else
                                    {
                                        if (CapabilitiesList.Items[previousSelectedIndex].ToString() ==
                                            previouslySelectedValue)
                                        {
                                            CapabilitiesList.SelectedIndex = previousSelectedIndex;
                                        }
                                    }
                                }
                                return;
                            }
                        }
                        CapabilitiesList.Items.Add("No Weapons.");
                    }
                    else
                    {
                        CapabilitiesList.Items.Clear();
                    }
                }
                catch (Exception exc)
                {
                    CapabilityListUpdating = false;
                    throw new Exception(exc.Message);
                }
                CapabilityListUpdating = false;
            }
            else
            {
                BeginInvoke(new WeaponsUpdateDelegate(WeaponsUpdate), obj);
            }
        }


        private void SubplatformUpdate(DDDObjects obj)
        {
            if (!InvokeRequired)
            {
                try
                {
                    //get previously selected.
                    String previouslySelected = string.Empty;
                    if (SubplatformList.Items.Count > 0)
                    {
                        previouslySelected = (String)SubplatformList.SelectedItem;
                    }

                    if (obj != null)
                    {
                    //clear the list
                        SubplatformList.Items.Clear();
                        SubplatformList.Items.Add("Dock this object");

                    //add new list
                        if (obj.SubPlatforms != null)
                        {
                            SubplatformList.Items.AddRange(obj.SubPlatforms);
                        }
                    //reset selected
                        if (previouslySelected != string.Empty)
                        { //find and select the previous item
                            int index = 0;
                            bool isDone = false;
                            while (!isDone && index < SubplatformList.Items.Count)
                            {
                                if (SubplatformList.Items[index].ToString().Equals(previouslySelected))
                                {
                                    SubplatformManuallyChanged = true;
                                    SubplatformList.SelectedIndex = index;
                                    SubplatformManuallyChanged = false;
                                    isDone = true;
                                }
                                index++;
                            }

                        }

                    }
                    else
                    {
                        SubplatformList.Items.Clear();
                        SubplatformList.Items.Add("Dock this object");
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new SubplatformUpdateDelegate(SubplatformUpdate), obj);
            }
        }

        private void SetUnitControlIndex(int index)
        {
            if (!InvokeRequired)
            {
                try
                {
                    UnitControls.SelectedIndex = index;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new SetUnitControlIndexDelegate(SetUnitControlIndex), index);
            }
        }

        private void ThrottleUpdate(int value)
        {
            if (!InvokeRequired)
            {
                try
                {
                    ThrottleControl.Value = value;
                    //ObjectThrottle.Text = value / (double)ThrottleControl.Maximum;
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new ThrottleUpdateDelegate(ThrottleUpdate), value);
            }
        }

        private void FuelGaugeUpdate(int value, int maximum)
        {
            if (!InvokeRequired)
            {
                try
                {
                    if (value > maximum)
                    {
                        FuelGauge.Maximum = FuelGauge.Value = 0;
                    }
                    else
                    {
                        FuelGauge.Maximum = maximum;
                        FuelGauge.Value = value;
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new FuelGaugeUpdateDelegate(FuelGaugeUpdate), value, maximum);
            }
        }
        private void PopulateObjectAttributes(DDDObjects object_data)
        {
            try
            {
                if (object_data != null)
                {
                    //SetUnitControlIndex(0);
                    ChangeControlState(UnitControls, true);
                    UpdateLabel(ObjectAltitude, object_data.AltitudeStr);
                    UpdateLabel(ObjectClass, object_data.ClassName);
                    UpdateLabel(ObjectFuelAmount, object_data.FuelAmountStr);
                    UpdateLabel(ObjectFuelCapacity, object_data.FuelCapacityStr);
                    UpdateLabel(ObjectMaxSpeed, object_data.MaxSpeedStr);
                    UpdateLabel(ObjectName, object_data.ObjectID);
                    UpdateLabel(ObjectStatus, object_data.State);
                    UpdateLabel(ObjectThrottle, object_data.ThrottleStr);
                    UpdateLabel(ObjectLocation, object_data.PositionStr);
                    //UpdateLabel(ThrottleLabel, string.Format("{0} %", object_data.Throttle * 100));
                    //if (!textBoxUnitTag.Focused)
                    //{
                    UpdateLabel(textBoxUnitTag, object_data.Tag);
                    UpdateLabel(textBoxClassification, object_data.Classification);

                    //}
                  //  SetTextboxReadStatus(textBoxUnitTag, false);

                    FuelGaugeUpdate((int)object_data.FuelAmount, (int)object_data.FuelCapacity);
                    //ThrottleUpdate((int)(object_data.Throttle * ThrottleControl.Maximum));
                    VulnerabilitiesUpdate(object_data);
                    SensorsUpdate(object_data);
                    WeaponsUpdate(object_data);
                    SubplatformUpdate(object_data);

                    _customAttributesDialog.SetDataGridView(object_data.ObjectID, object_data.CustomAttributes);
                    EnableButtonsForOwnedObjects(object_data.OwnerID == DDD_Global.Instance.PlayerID);
                }
                else
                {

                    UpdateLabel(ObjectAltitude, string.Empty);
                    UpdateLabel(ObjectClass, string.Empty);
                    UpdateLabel(ObjectFuelAmount, string.Empty);
                    UpdateLabel(ObjectFuelCapacity, string.Empty);
                    UpdateLabel(ObjectLocation, string.Empty);
                    UpdateLabel(ObjectMaxSpeed, string.Empty);
                    UpdateLabel(ObjectName, string.Empty);
                    UpdateLabel(ObjectStatus, string.Empty);
                    UpdateLabel(ObjectThrottle, string.Empty);
                    UpdateLabel(textBoxUnitTag, string.Empty);
                    UpdateLabel(textBoxClassification, string.Empty);
                    //SetTextboxReadStatus(textBoxUnitTag, true);

                    FuelGaugeUpdate(0, 0);
                    ThrottleUpdate(0);
                    VulnerabilitiesUpdate(null);
                    SensorsUpdate(null);
                    WeaponsUpdate(null);
                    SubplatformUpdate(null);
                    ChangeControlState(UnitControls, false);

                    _customAttributesDialog.SetDataGridView(string.Empty, new Dictionary<string, DataValue>());
                    EnableButtonsForOwnedObjects(false);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);

            }
        }


        private void WeaponsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null && CapabilityListUpdating == false)
                {
                    DDDObjects obj = _map_scene.GetSelectedObject();
                    if (obj != null)
                    {
                        obj.CurrentCapabilityAndWeapon = CapabilitiesList.SelectedIndex;
                        if (CapabilitiesList.SelectedIndex >= 0)
                        {
                            SendWeaponOrCapabilitySelectedEvent(CapabilitiesList.Items[obj.CurrentCapabilityAndWeapon].ToString());
                            MDX_HostPanel.Cursor = Cursors.Cross;
                            StatusPanelUpdate(CAPABILITIES_TIP);
                            _map_scene.EnterAttackMode();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
        private static bool _subplatformListManuallyChanged = false;
        private static object _subplatformListLock = new object();
        public static bool SubplatformManuallyChanged
        {
            get {
                lock (_subplatformListLock)
                {
                    return _subplatformListManuallyChanged;
                }
            }
            set {
                lock (_subplatformListLock)
                {
                    _subplatformListManuallyChanged = value;
                }
            }
        }
        private void SubplatformList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null && SubplatformManuallyChanged == false)
                {
                    DDDObjects obj = _map_scene.GetSelectedObject();
                    if (obj != null)
                    {
                        obj.CurrentSubplatform = SubplatformList.SelectedIndex - 1; //-1 accounts for "dock this object" option
                        if (SubplatformList.SelectedIndex >= 0)
                        {
                            if (SubplatformList.Items[obj.CurrentSubplatform + 1] != null)
                            {
                                string item = SubplatformList.Items[obj.CurrentSubplatform + 1].ToString();
                                SendWeaponOrCapabilitySelectedEvent(item);
                                MDX_HostPanel.Cursor = Cursors.Cross;
                                StatusPanelUpdate(SUBPLATFORM_TIP);
                                _map_scene.EnterSubplatformMode();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        private void UnitControls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!InvokeRequired)
            {
                try
                {
                    if (_map_scene != null)
                    {
                        DDDObjects obj = _map_scene.GetSelectedObject();
                        string SelectedCapability = string.Empty;
                        string SelectedVulnerability = string.Empty;
                        string SelectedSensor = string.Empty;
                        //vuls and sensors?
                        if (obj != null)
                        {
                            SelectedCapability = obj.CurrentlySelectedCapability;
                            SelectedVulnerability = obj.CurrentlySelectedVulnerability;
                            SelectedSensor = obj.CurrentlySelectedSensor;
                        }
                        switch (UnitControls.SelectedIndex)
                        {
                            case (int)OBJECT_CONTROL_TAB.MOVE:
                                // Change the MapScene Container to move mode
                                SendTabSelectedEvent("MOVE");
                                _map_scene.EnterMoveMode();
                                MDX_HostPanel.Cursor = Cursors.Default;
                                StatusPanelUpdate(MOVE_TIP);

                                //Sensors
                                if (SelectedSensor != string.Empty)
                                {
                                    int index = 0;
                                    while (index < listBoxSensors.Items.Count)
                                    {
                                        if (listBoxSensors.Items[index].ToString() == SelectedSensor)
                                        {
                                            listBoxSensors.SelectedIndex = index;
                                            index = listBoxSensors.Items.Count; //escape
                                        }
                                        index++;
                                    }
                                }
                                else
                                {
                                    listBoxSensors.SelectedIndex = -1;
                                }

                                //Vulnerabilities
                                if (SelectedVulnerability != string.Empty)
                                {
                                    int index = 0;
                                    while (index < VulnerabilitiesList.Items.Count)
                                    {
                                        if (VulnerabilitiesList.Items[index].ToString() == SelectedVulnerability)
                                        {
                                            VulnerabilitiesList.SelectedIndex = index;
                                            index = VulnerabilitiesList.Items.Count; //escape
                                        }
                                        index++;
                                    }
                                }
                                else
                                {
                                    VulnerabilitiesList.SelectedIndex = -1;
                                }

                                break;
                            case (int)OBJECT_CONTROL_TAB.WEAPONS:
                                SendTabSelectedEvent("CAPABILITIES");
                                if (SelectedCapability != string.Empty)
                                {
                                    int index = 0;
                                    while (index < CapabilitiesList.Items.Count)
                                    {
                                        if (CapabilitiesList.Items[index].ToString().StartsWith(SelectedCapability))
                                        {
                                            CapabilitiesList.SelectedIndex = index;
                                            index = CapabilitiesList.Items.Count; //escape
                                        }
                                        index++;
                                    }
                                }
                                else
                                {
                                    CapabilitiesList.SelectedIndex = -1;
                                }
                                break;
                            case (int)OBJECT_CONTROL_TAB.SUBPLATFORMS:
                                SendTabSelectedEvent("SUBPLATFORMS");
                                SubplatformList.SelectedIndex = -1;
                                break;
                            case (int)OBJECT_CONTROL_TAB.ADDITIONAL:
                                SendTabSelectedEvent("ADDITIONAL");
                                DDDObjects in_obj = _map_scene.GetSelectedObject();
                                bool enabled = false;
                                if (in_obj != null)
                                {
                                    enabled = (in_obj.OwnerID == DDD_Global.Instance.PlayerID && DDD_Global.Instance.AssetTransferEnabled);
                                }

                                buttonTransferAsset.Enabled = enabled;
                                break;
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new EventHandler(UnitControls_SelectedIndexChanged), sender, e);
            }

        }

        private void ThrottleControl_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    DDDObjects obj = _map_scene.GetSelectedObject();
                    if (obj != null)
                    {
                        //obj.Throttle = (double)ThrottleControl.Value / (double)ThrottleControl.Maximum;
                        ThrottleLabel.Text = string.Format("{0} %", ((TrackBar)sender).Value / (double)((TrackBar)sender).Maximum * 100);
                        obj.ThrottleSlider = ((TrackBar)sender).Value / (double)((TrackBar)sender).Maximum;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }





        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure?", "Exit the DDD", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    QuitApplication();
                    //Application.Exit();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void lightenMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    _map_scene.LightenMap();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void darkenMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    _map_scene.DarkenMap();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        private void miniMapColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    colorDialog1.Color = _map_scene.MiniMapColor;
                    if (colorDialog1.ShowDialog() == DialogResult.OK)
                    {
                        _map_scene.MiniMapColor = colorDialog1.Color;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void chatWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Controller != null)
                {
                    ChatDialog c = new ChatDialog(_Controller);
                    List<string> dm_list = DDD_Global.Instance.DM_List;
                    dm_list.Remove(DDD_Global.Instance.PlayerID);
                    c.Members = dm_list;
                    if (c.ShowProperties() == DialogResult.OK)
                    {
                        List<string> selected = c.SelectedMembers;
                        if (selected != null)
                        {
                            if (selected.Count > 0)
                            {
                                selected.Add(DDD_Global.Instance.PlayerID);
                                _Controller.RequestChatRoomCreate(c.GroupId, selected, DDD_Global.Instance.PlayerID);
                            }
                            else
                            {
                                MessageBox.Show("At least one DM is necessary to create a chat room.", "Chat Room Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    c.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void whiteboardWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Controller != null)
                {
                    WhiteboardDialog c = new WhiteboardDialog(this, _Controller, null);
                    List<string> dm_list = DDD_Global.Instance.DM_List;
                    dm_list.Remove(DDD_Global.Instance.PlayerID);
                    c.Members = dm_list;
                    if (c.ShowProperties() == DialogResult.OK)
                    {
                        List<string> selected = c.SelectedMembers;
                        if (selected != null)
                        {
                            if (selected.Count > 0)
                            {
                                selected.Add(DDD_Global.Instance.PlayerID);
                                _Controller.RequestWhiteboardRoomCreate(c.GroupId, selected, DDD_Global.Instance.PlayerID);
                            }
                            else
                            {
                                MessageBox.Show("At least one DM is necessary to create a whiteboard room.", "Whiteboard Room Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    c.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void TabControls_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (_game_over)
                {
                    case true:
                        printPreviewScoreToolStripMenuItem.Enabled = false;
                        printPreviewScoreToolStripMenuItem.Enabled = true;
                        break;

                    case false:
                        if (TabControls.SelectedTab.Controls[0] is ChatDialog)
                        {
                            ((ChatDialog)TabControls.SelectedTab.Controls[0]).MessageCount = 0;
                            TabControls.SelectedTab.Text = ((ChatDialog)TabControls.SelectedTab.Controls[0]).GroupId;
                            ((ChatDialog)TabControls.SelectedTab.Controls[0]).Focus();
                        }

                        if (_map_scene != null)
                        {
                            _map_scene.WBRoom = null;

                            // Leave Draw mode if needed
                            if (_map_scene.MapState == MapModes.DRAW)
                            {
                                _map_scene.EnterMoveMode();
                                ChangeMapState();
                            }
                        }

                        // Give the map scene a way to access the current whiteboard dialog
                        if (TabControls.SelectedTab.Controls[0] is WhiteboardDialog)
                        {
                            if (_map_scene != null)
                            {
                                _map_scene.WBRoom = ((WhiteboardDialog)TabControls.SelectedTab.Controls[0]).WBRoom;

                                // Enter Draw mode if needed
                                if (_map_scene.WBRoom.DrawMode != DrawModes.Selection)
                                {
                                    _map_scene.EnterDrawMode();
                                    ChangeMapState();
                                }
                            }
                        }

                        break;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        private void tabWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MapMessageContainer.Panel2Collapsed = !tabWindowsToolStripMenuItem.Checked;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void miniMapUnitColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    miniMapUnitColorsToolStripMenuItem.Checked = !miniMapUnitColorsToolStripMenuItem.Checked;
                    _map_scene.ShowUnitColorOnMiniMap = miniMapUnitColorsToolStripMenuItem.Checked;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void toolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DDD_ToolTip.Active = toolToolStripMenuItem.Checked;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void UnitFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UnitFinder_UpdateItems();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void toggleDisplayLabels(String value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new UnitFinderUpdateDelegate(toggleDisplayLabels), value);
            }
            else
            {
                switch (value)
                { 
                    case "true":
                        displayUnmanagedUnitLabelToolStripMenuItem.Checked = true;
                        displayUnmanagedUnitLabelToolStripMenuItem_Click(displayUnmanagedUnitLabelToolStripMenuItem, new EventArgs());
                        break;
                    case "false":
                        displayUnmanagedUnitLabelToolStripMenuItem.Checked = false;
                        displayUnmanagedUnitLabelToolStripMenuItem_Click(displayUnmanagedUnitLabelToolStripMenuItem, new EventArgs());
                        break;
                    case "disabled":
                        displayUnmanagedUnitLabelToolStripMenuItem.Checked = false;
                        displayUnmanagedUnitLabelToolStripMenuItem.Enabled = false;
                        displayUnmanagedUnitLabelToolStripMenuItem_Click(displayUnmanagedUnitLabelToolStripMenuItem, new EventArgs());
                        break;
                    default:
                        break;
                }
            }
        }
        public void toggleTagDisplay(String value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new UnitFinderUpdateDelegate(toggleTagDisplay), value);
            }
            else
            {
                switch (value)
                {
                    case "true":
                        belowUnitToolStripMenuItem.Checked = true;
                        belowUnitToolStripMenuItem_Click(belowUnitToolStripMenuItem, new EventArgs());
                        break;
                    case "false":
                        aboveUnitToolStripMenuItem.Checked = false;
                        aboveUnitToolStripMenuItem_Click_1(aboveUnitToolStripMenuItem, new EventArgs());
                        belowUnitToolStripMenuItem.Checked = false;
                        belowUnitToolStripMenuItem_Click(belowUnitToolStripMenuItem, new EventArgs());
                        overlayedToolStripMenuItem.Checked = false;
                        overlayedToolStripMenuItem_Click(overlayedToolStripMenuItem, new EventArgs());
                        break;
                    case "disabled":
                        aboveUnitToolStripMenuItem.Checked = false;
                        aboveUnitToolStripMenuItem_Click_1(aboveUnitToolStripMenuItem, new EventArgs());
                        belowUnitToolStripMenuItem.Checked = false;
                        belowUnitToolStripMenuItem_Click(belowUnitToolStripMenuItem, new EventArgs());
                        overlayedToolStripMenuItem.Checked = false;
                        overlayedToolStripMenuItem_Click(overlayedToolStripMenuItem, new EventArgs());
                        displayUnitTagToolStripMenuItem.Enabled = false;
                        break;
                    case "top":
                        aboveUnitToolStripMenuItem.Checked = true;
                        aboveUnitToolStripMenuItem_Click_1(aboveUnitToolStripMenuItem, new EventArgs());
                        break;
                    case "bottom":
                        belowUnitToolStripMenuItem.Checked = true;
                        belowUnitToolStripMenuItem_Click(belowUnitToolStripMenuItem, new EventArgs());
                        break;
                    case "overlay":
                        overlayedToolStripMenuItem.Checked = true;
                        overlayedToolStripMenuItem_Click(overlayedToolStripMenuItem, new EventArgs());
                        break;
                    default: break;
                }
            }
        }

        private void displayUnmanagedUnitLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    _map_scene.DrawUnmanagedUnitLabels = displayUnmanagedUnitLabelToolStripMenuItem.Checked;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void scaleUnitIconsToMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    _map_scene.ScaleUnitWithMap = scaleUnitIconsToMapToolStripMenuItem.Checked;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void ScenarioObjectiveWindow(bool modal)
        {
            try
            {
                if (_ObjectiveWindow == null)
                {
                    _ObjectiveWindow = new ScrollWindow();
                    _ObjectiveWindow.Text = "Player Briefing";
                }
                if (!_ObjectiveWindow.Visible)
                {
                    _ObjectiveWindow.Content = DDD_Global.Instance.PlayerBrief;
                    if (modal)
                    {
                        _ObjectiveWindow.ShowDialog();
                    }
                    else
                    {
                        _ObjectiveWindow.Show();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        private void printScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (TabControls.SelectedIndex == 0)
                {
                    ScoreSummary.ShowPrintDialog();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void printPreviewScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (TabControls.SelectedIndex == 0)
                {
                    ScoreSummary.ShowPrintPreviewDialog();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void ReadSimulationModel(string hostname)
        {
            try
            {
                SimulationModelReader s = new SimulationModelReader();
                if (hostname != string.Empty)
                {
                    DDD_Global.Instance.SimModel = s.readModel(string.Format("{0}SimulationModel.xml", DDD_Global.Instance.DDDClientShareFolder));
                }
                else
                {
                    DDD_Global.Instance.SimModel = s.readModel("SimulationModel.xml");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't find Simulation Model.");
            }

        }

        private void ReadSimulationModel()
        {
            Stream str = null;
            SimulationModelReader s = new SimulationModelReader();
            try
            {

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                str = assembly.GetManifestResourceStream(this.GetType(), "SimulationModel.xml");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to read the Simulation Model.");
            }
            DDD_Global.Instance.SimModel = s.readModel(str);

        }

        private void SubscribeToEvents()
        {
            try
            {
                DDD_Global.Instance.Subscribe("SimulationTimeEvent");
                DDD_Global.Instance.Subscribe("GameSpeed");
                DDD_Global.Instance.Subscribe("ViewProAttributeUpdate");
                DDD_Global.Instance.Subscribe("WeaponLaunch");
                DDD_Global.Instance.Subscribe("ViewProMotionUpdate");
                DDD_Global.Instance.Subscribe("HandshakeAvailablePlayers");
                DDD_Global.Instance.Subscribe("HandshakeInitializeGUI");
                DDD_Global.Instance.Subscribe("ClientRemoveObject");
                DDD_Global.Instance.Subscribe("PauseScenario");
                DDD_Global.Instance.Subscribe("ViewProAttackUpdate");
                DDD_Global.Instance.Subscribe("ResumeScenario");
                DDD_Global.Instance.Subscribe("StopScenario");
                DDD_Global.Instance.Subscribe("TextChat");
                DDD_Global.Instance.Subscribe("WhiteboardLine");
                DDD_Global.Instance.Subscribe("WhiteboardClear");
                DDD_Global.Instance.Subscribe("WhiteboardClearAll");
                DDD_Global.Instance.Subscribe("WhiteboardUndo");
                DDD_Global.Instance.Subscribe("WhiteboardSyncScreenView");
                DDD_Global.Instance.Subscribe("ClientMeasure_ScreenView");
                DDD_Global.Instance.Subscribe("ViewProInitializeObject");
                DDD_Global.Instance.Subscribe("SystemMessage");
                DDD_Global.Instance.Subscribe("ViewProStopObjectUpdate");
                DDD_Global.Instance.Subscribe("ScoreUpdate");
                DDD_Global.Instance.Subscribe("CreateChatRoom");
                DDD_Global.Instance.Subscribe("CloseChatRoom");
                DDD_Global.Instance.Subscribe("CreateWhiteboardRoom");
                DDD_Global.Instance.Subscribe("AuthenticationResponse");
                DDD_Global.Instance.Subscribe("ViewProActiveRegionUpdate");
                DDD_Global.Instance.Subscribe("StopReplay");
                DDD_Global.Instance.Subscribe("TransferObject");
                DDD_Global.Instance.Subscribe("ClientSideAssetTransferAllowed");
                // The timing is too early to check if DDD_Global.Instance.VoiceChatEnabled
                // so just subscribe anyway  -- shorvitz 05/05/08
                DDD_Global.Instance.Subscribe("CreateVoiceChannel");
                DDD_Global.Instance.Subscribe("CloseVoiceChannel");
                DDD_Global.Instance.Subscribe("JoinVoiceChannel");
                DDD_Global.Instance.Subscribe("LeaveVoiceChannel");

                //added some more events -- adziki 5/6/08
                DDD_Global.Instance.Subscribe("StartedTalking");
                DDD_Global.Instance.Subscribe("PlayVoiceMessage");
                DDD_Global.Instance.Subscribe("StoppedTalking");
                DDD_Global.Instance.Subscribe("RequestJoinVoiceChannel");
                DDD_Global.Instance.Subscribe("RequestLeaveVoiceChannel");

                //added some more event -- dhoward 6/29/09
                DDD_Global.Instance.Subscribe("MuteUser");
                DDD_Global.Instance.Subscribe("UnmuteUser");

                //added HandshakeInitializeGUIDone to be notified when new DMs join
                DDD_Global.Instance.Subscribe("HandshakeInitializeGUIDone");
                DDD_Global.Instance.Subscribe("ForkReplayStarted");
                DDD_Global.Instance.Subscribe("ForkReplayFinished");

                //added some more event -- dhoward 6/29/09
                DDD_Global.Instance.Subscribe("InitializeClassifications");

            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }



        private void UnitFinder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (_map_scene != null)
                {
                    SendSelectedObjectEvent((string)this.UnitFinder.Items[UnitFinder.SelectedIndex]);
                    switch (UnitControls.SelectedIndex)
                    {
                        case (int)OBJECT_CONTROL_TAB.MOVE:
                            if (UnitControls.SelectedIndex == (int)OBJECT_CONTROL_TAB.MOVE)
                                SendTabSelectedEvent("MOVE");
                            int selection = UnitFinder.SelectedIndex;
                            _map_scene.ClearMapSelection(false);
                            if (selection >= 0)
                            {
                                PopulateObjectAttributes(_map_scene.SelectObject((string)this.UnitFinder.Items[selection]));
                                if (_center_map)
                                {
                                    DDDObjects obj = _map_scene.GetSelectedObject();
                                    if (obj != null)
                                    {
                                        _map_scene.CenterMapToUnit(obj.Position.X * _map_scene.Scale, obj.Position.Y * _map_scene.Scale);
                                    }
                                }
                            }
                            break;
                        case (int)OBJECT_CONTROL_TAB.SUBPLATFORMS:
                            SendTabSelectedEvent("SUBPLATFORMS");
                            goto case (int)OBJECT_CONTROL_TAB.MOVE;

                        case (int)OBJECT_CONTROL_TAB.WEAPONS:
                            SendTabSelectedEvent("CAPABILITIES");
                            if (_map_scene.MapState == MapModes.ATTACK)
                            {
                                if (UnitFinder.SelectedIndex >= 0)
                                {
                                    if (CapabilitiesList.SelectedIndex >= 0)
                                    {
                                        _map_scene.SelectObject((string)this.UnitFinder.Items[this.UnitFinder.SelectedIndex]);
                                        _map_scene.EngageTarget();
                                        ChangeMapState();
                                    }
                                }
                            }
                            else
                            {
                                goto case (int)OBJECT_CONTROL_TAB.MOVE;
                            }
                            break;
                        case (int)OBJECT_CONTROL_TAB.ADDITIONAL:
                            SendTabSelectedEvent("ADDITIONAL");
                            int unitSelection = UnitFinder.SelectedIndex;
                            _map_scene.ClearMapSelection(false);
                            if (unitSelection >= 0)
                            {
                                PopulateObjectAttributes(_map_scene.SelectObject((string)this.UnitFinder.Items[unitSelection]));
                                if (_center_map)
                                {
                                    DDDObjects obj = _map_scene.GetSelectedObject();
                                    if (obj != null)
                                    {
                                        _map_scene.CenterMapToUnit(obj.Position.X * _map_scene.Scale, obj.Position.Y * _map_scene.Scale);
                                    }
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Unit Finder Changed, unhandled type: " + ((int)UnitControls.SelectedIndex).ToString());
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }


        }

        private void dDDHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProductHelpWindow();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void contactInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ContactWindow();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AboutWindow();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void scenarioInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ScenarioObjectiveWindow(false);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private void UnitFinder_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                UnitFinder_UpdateItems();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }


        private void UnitFinder_Enter(object sender, EventArgs e)
        {
            try
            {
                UnitFinder_UpdateItems();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void TransferObjectToMe(string objectID)
        { //move from unmanaged to managed
            if (_map_scene == null)
            {
                Console.WriteLine("In TransferObjectToMe: Current Scene is null");
                return;
            }

            if (_map_scene.SelectObject(objectID) == null)
            {
                Console.WriteLine("In TransferObjectToMe: selected object is null");
                return; //?
            }
            if (_UnmanagedUnits.Contains(objectID))
            {
                _UnmanagedUnits.Remove(objectID);
            }

            _ManagedUnits.Add(objectID);

        }

        public void TransferObjectToOther(string objectID)
        {//move from managed to unmanaged
            if (_map_scene == null)
            {
                Console.WriteLine("In TransferObjectToOther: Current Scene is null");
                return;
            }

            if (_map_scene.SelectObject(objectID) == null)
            {
                Console.WriteLine("In TransferObjectToOther: selected object is null");
                return; //?
            }
            if (_map_scene.GetSelectedObject().ObjectID == objectID)
            {
                //de-select object
                _map_scene.ClearMapSelection();
            }
            if (_ManagedUnits.Contains(objectID))
            {
                _ManagedUnits.Remove(objectID);
            }

            _UnmanagedUnits.Add(objectID);

        }

        private void buttonTransferAsset_Click(object sender, EventArgs e)
        {
            if (_map_scene == null)
            {
                Console.WriteLine("In buttonTransferAsset_Click: Current Scene is null");
                return;
            }
            DDDObjects obj = _map_scene.GetSelectedObject();
            if (obj == null)
            {
                Console.WriteLine("In buttonTransferAsset_Click: Selected object is null");
                return;
            }

            string selectedDM;
            string selectedObjectID = obj.ObjectID;
            string objectState = obj.State;
            List<string> availableDMs = DDD_Global.Instance.DM_List;
            availableDMs.Remove(DDD_Global.Instance.PlayerID);

            DropdownDialog dlg = new DropdownDialog("Unit Transfer Dialog", String.Format("Select the Decision Maker to transfer this unit ({0}) to:", selectedObjectID), availableDMs);
            selectedDM = dlg.ShowDialog(this);

            if (selectedDM == string.Empty)
            {
                return;
            }

            _map_scene.TransferObjectRequest(DDD_Global.Instance.PlayerID, selectedObjectID, selectedDM, objectState);

        }
        private void buttonDockToObject_Click(object sender, EventArgs e)
        {
            if (_map_scene == null)
            {
                Console.WriteLine("In buttonDockToObject_Click: Current Scene is null");
                return;
            }
            DDDObjects obj = _map_scene.GetSelectedObject();
            if (obj == null)
            {
                Console.WriteLine("In buttonDockToObject_Click: Selected object is null");
                return;
            }

            string selectedParent;
            string selectedObjectID = obj.ObjectID;
            string requestorID = DDD_Global.Instance.PlayerID;
            List<string> listOfVisibleObjects = new List<string>();
            listOfVisibleObjects.AddRange(_map_scene.GetListOfPlayfieldObjects());
            listOfVisibleObjects.Remove(selectedObjectID);

            DropdownDialog dlg = new DropdownDialog("Asset Docking Dialog", String.Format("Select the Asset to dock this asset ({0}) to:", selectedObjectID), listOfVisibleObjects);
            selectedParent = dlg.ShowDialog(this);

            if (selectedParent == string.Empty)
            {
                return;
            }

            _map_scene.DockObjectRequest(requestorID, selectedObjectID, selectedParent, true);
        }

        /// <summary>
        /// This method will enable/disable a couple of buttons that are enabled for objects that your player owns
        /// and disabled for unmanaged assets.
        /// </summary>
        /// <param name="isSelectedObjectOwnedByPlayer">If your PlayerID is equal to the selected object's ownerID
        /// then this should be true.</param>
        private void EnableButtonsForOwnedObjects(bool isSelectedObjectOwnedByPlayer)
        {
            //buttonDockToObject.Enabled = isSelectedObjectOwnedByPlayer;
            if (InvokeRequired)
            {
                BeginInvoke(new SingleBoolDelegate(EnableButtonsForOwnedObjects), isSelectedObjectOwnedByPlayer);
            }
            else
            {
                buttonTransferAsset.Enabled = (isSelectedObjectOwnedByPlayer && DDD_Global.Instance.AssetTransferEnabled);
            }
            //AD: This is assuming that the asset transfer flag is an absolute yes/no on asset transfers
        }

        private void customAttributesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _customAttributesDialog.Visible = customAttributesToolStripMenuItem.Checked;
            }
            catch
            {
                Console.Out.WriteLine("Cannot toggle custom attributes dialog on/off");
            }
        }

        public void ToggleCustomAttributesToolStripVisibility(bool visible)
        {
            customAttributesToolStripMenuItem.Checked = visible;
            customAttributesToolStripMenuItem_Click(this, new EventArgs());
        }



        private void aboveUnitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (aboveUnitToolStripMenuItem.Checked)
            {
                belowUnitToolStripMenuItem.Checked = false;
                overlayedToolStripMenuItem.Checked = false;
                DDD_Global.Instance.TagPosition = TagPositionEnum.ABOVE;
            }
            else
            {
                DDD_Global.Instance.TagPosition = TagPositionEnum.INVISIBLE;
            }

        }

        private void belowUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (belowUnitToolStripMenuItem.Checked)
            {
                aboveUnitToolStripMenuItem.Checked = false;
                overlayedToolStripMenuItem.Checked = false;
                DDD_Global.Instance.TagPosition = TagPositionEnum.BELOW;
            }
            else
            {
                DDD_Global.Instance.TagPosition = TagPositionEnum.INVISIBLE;
            }
        }

        private void overlayedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (overlayedToolStripMenuItem.Checked)
            {
                aboveUnitToolStripMenuItem.Checked = false;
                belowUnitToolStripMenuItem.Checked = false;
                DDD_Global.Instance.TagPosition = TagPositionEnum.CENTER;
            }
            else
            {
                DDD_Global.Instance.TagPosition = TagPositionEnum.INVISIBLE;
            }

        }

        private void buttonTagObject_Click(object sender, EventArgs e)
        {
            if (_map_scene == null)
            {
                Console.WriteLine("In buttonTagObject_Click: Current Scene is null");
                return;
            }
            DDDObjects obj = _map_scene.GetSelectedObject();
            if (obj == null)
            {
                Console.WriteLine("In buttonTagObject_Click: Selected object is null");
                return;
            }

            string selectedObjectID = obj.ObjectID;
            string prevTag = obj.Tag;
            string newTag;

            TextboxDialog tbDlg = new TextboxDialog("Edit unit tag information", "Enter the unit's new tag information below.  This will be displayed to your team mates.", prevTag);

            newTag = tbDlg.ShowDialog(this);

            if (newTag == prevTag)
            {
                return;
            }

            _map_scene.ChangeTagRequest(DDD_Global.Instance.PlayerID, selectedObjectID, newTag);

        }

 /*
        public void SendDDDVoiceClientEvent( string eventType, string channelName, string channelID )
        {
            System.Console.WriteLine( "DDD_MainWinFormInterface.SendDDDVoiceClientEvent: eventType = {0}", eventType );
            System.Console.WriteLine( "DDD_MainWinFormInterface.SendDDDVoiceClientEvent: PlayerID = {0}", DDD_Global.Instance.PlayerID );
            System.Console.WriteLine( "DDD_MainWinFormInterface.SendDDDVoiceClientEvent: channelName = {0}", channelName );
            System.Console.WriteLine( "DDD_MainWinFormInterface.SendDDDVoiceClientEvent: channelID = {0}", channelID );
            _Controller.SendVoiceClientEvent( eventType, DDD_Global.Instance.PlayerID, channelName, channelID );
        }
 */ 

        #region IVoiceClientController
        public void NotifyCreatedVoiceChannel(string strChannelName, List<string> astrMembershipList)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyVoiceChannelCreated(strChannelName, astrMembershipList);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to display newly created voice channel {0}", strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyCreatedVoiceChannelDelegate(NotifyCreatedVoiceChannel), strChannelName, astrMembershipList);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyClosedVoiceChannel(string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyVoiceChannelClosed(strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to display voice channel {0} is closed", strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyClosedVoiceChannelDelegate(NotifyClosedVoiceChannel), strChannelName);
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyJoinVoiceChannel(string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyJoinVoiceChannel(strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to join voice channel {0}", strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyJoinVoiceChannelDelegate(NotifyJoinVoiceChannel), strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyLeaveVoiceChannel(string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyLeaveVoiceChannel(strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to leave voice channel {0}", strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyLeaveVoiceChannelDelegate(NotifyLeaveVoiceChannel), strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyStartedTalking(string strUsername, string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyStartedTalking(strUsername, strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to have user {0} start talking on voice channel {1}", strUsername, strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyStartedTalkingDelegate(NotifyStartedTalking), strUsername, strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyStoppedTalking(string strUsername, string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyStoppedTalking(strUsername, strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to have user {0} stop talking on voice channel {1}", strUsername, strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyStoppedTalkingDelegate(NotifyStoppedTalking), strUsername, strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyMuteUser(string strUsername, string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyMuteUser(strUsername, strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to have user {0} mute on voice channel {1}", strUsername, strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyMuteUserDelegate(NotifyMuteUser), strUsername, strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public void NotifyUnmuteUser(string strUsername, string strChannelName)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (_voiceClientControl != null)
                    {
                        if (!InvokeRequired)
                        {
                            try
                            {
                                _voiceClientControl.notifyUnmuteUser(strUsername, strChannelName);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, String.Format("Unable to have user {0} unmute on voice channel {1}", strUsername, strChannelName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            BeginInvoke(new NotifyUnmuteUserDelegate(NotifyUnmuteUser), strUsername, strChannelName);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        #endregion IVoiceClientController

        private void MDX_HostPanel_MouseEnter(object sender, EventArgs e)
        {
            Console.WriteLine("Mouse Enter event triggered");
            DDD_Global.Instance.RangeFinderDistanceString = " ";
        }

        private void MDX_HostPanel_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("Mouse Leave event triggered");
            DDD_Global.Instance.RangeFinderDistanceString = String.Empty;
        }

        private void MDX_HostPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void checkBoxDisplayRangeFinder_CheckedChanged(object sender, EventArgs e)
        {
            DDD_Global.Instance.RangeFinderEnabled = checkBoxDisplayRangeFinder.Checked;
        }

        private void VulnerabilitiesList_Click(object sender, EventArgs e)
        {
            if (VulnerabilitiesList.SelectedItem == null)
            {
                Console.WriteLine("Selected Vulnerability was NULL.");
                return;
            }
            Console.WriteLine("VULNERABILITIES list clicked!\r\nSelected value is {0}", VulnerabilitiesList.SelectedItem);
            if (!InvokeRequired)
            {
                try
                {
                    if (_map_scene != null)
                    {
                        DDDObjects current = _map_scene.GetSelectedObject();
                        if (current != null)
                        {
                            string selectedVulnerability = string.Empty;
                            if (VulnerabilitiesList.SelectedItem != null)
                            {
                                selectedVulnerability = VulnerabilitiesList.SelectedItem.ToString();
                            }
                            current.ClearVulnerabilityRingValues();
                            if (selectedVulnerability == current.CurrentlySelectedVulnerability)
                            {
                                VulnerabilitiesList.SelectedIndex = -1; //de-select?
                                current.CurrentlySelectedVulnerability = string.Empty;
                                current.CurrentlySelectedVulnerabilityRange = -1;
                            }
                            else
                            {
                                current.CurrentlySelectedVulnerability = selectedVulnerability;
                                current.CurrentlySelectedVulnerabilityRange = current.GetVulnerabilityLongestRangeRingRadius(selectedVulnerability);

                            }
                        }
                        else
                        { //the vulnerability list shouldn't be populated if no object is selected.  This happens if the object moves out of
                            //range or is destroyed.  Clear the list.
                            VulnerabilitiesList.Items.Clear();
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new EventHandler(VulnerabilitiesList_Click), sender, e);
            }
        }

        private void CapabilitiesList_Click(object sender, EventArgs e)
        {
            if (CapabilitiesList.SelectedItem == null)
            {
                Console.WriteLine("Selected Capability was NULL.");
                return;
            }
            Console.WriteLine("CAPABILITIES list clicked!\r\nSelected value is {0}", CapabilitiesList.SelectedItem);
            if (!InvokeRequired)
            {
                try
                {
                    if (_map_scene != null)
                    {
                        DDDObjects current = _map_scene.GetSelectedObject();
                        if (current != null)
                        {
                            string selectedCapability = string.Empty;
                            if (CapabilitiesList.SelectedItem != null)
                            {
                                selectedCapability = CapabilitiesList.SelectedItem.ToString();
                            }
                            current.ClearCapabilityRingValues();
                            if (selectedCapability == current.CurrentlySelectedCapability)
                            {
                                CapabilitiesList.SelectedIndex = -1; //de-select?
                                current.CurrentlySelectedCapability = string.Empty;
                                current.CurrentlySelectedCapabilityRange = -1;
                                DDD_Global.Instance.RangeFinderIntensityString = string.Empty;
                                current.CurrentCapabilityAndWeapon = -1;
                                _map_scene.EnterMoveMode();
                            }
                            else
                            {
                                current.CurrentlySelectedCapability = selectedCapability;
                                current.CurrentlySelectedCapabilityRange = current.GetCapabilityLongestRangeRingRadius(selectedCapability, true);
                                DDD_Global.Instance.RangeFinderIntensityString = String.Format("Intensity: {0}", current.CurrentlySelectedCapabilityRange);
                                current.CurrentCapabilityAndWeapon = CapabilitiesList.SelectedIndex;
                                _map_scene.EnterAttackMode();
                                Console.WriteLine("Selected Capability: {0}; Selected Cap Range: {1}", selectedCapability, current.CurrentlySelectedCapabilityRange);
                            }
                        }
                        else
                        { // Clear the list.
                            CapabilitiesList.Items.Clear();
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new EventHandler(CapabilitiesList_Click), sender, e);
            }
        }

        private void SimulationSplitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void MovementControls_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Panel size changed: {0} x {1}", SimulationSplitContainer.Panel1.Height, SimulationSplitContainer.Panel1.Width);
            Console.WriteLine("Movement Controls size changed: {0} x {1}", MovementControls.Height, MovementControls.Width);
            //As MovementControls.Height changes, affect location and size of Vul and Cap boxes.  Hack workaround.

            //buffer is 122
            int sensorLabelLocation;
            int sensorListBoxLocation;
            int sensorListBoxHeight;
            int vulnerabilityLabelLocation;
            int vulnerabilityListBoxLocation;
            int vulnerabilityListBoxHeight;

            GetRelativeListBoxInfo(MovementControls.Height, 122, out sensorLabelLocation, labelSensorList.Height,
                out sensorListBoxLocation, out sensorListBoxHeight, out vulnerabilityLabelLocation,
                labelVulnerabilities.Height, out vulnerabilityListBoxLocation, out vulnerabilityListBoxHeight);

            labelSensorList.Location = new Point(labelSensorList.Location.X, sensorLabelLocation);
            listBoxSensors.Location = new Point(listBoxSensors.Location.X, sensorListBoxLocation);
            listBoxSensors.Height = sensorListBoxHeight;
            labelVulnerabilities.Location = new Point(labelVulnerabilities.Location.X, vulnerabilityLabelLocation);
            VulnerabilitiesList.Location = new Point(VulnerabilitiesList.Location.X, vulnerabilityListBoxLocation);
            VulnerabilitiesList.Height = vulnerabilityListBoxHeight;
        }

        private void GetRelativeListBoxInfo(int panelHeight, int topBuffer, out int sensorLabelLocation,
            int sensorLabelHeight, out int sensorListBoxLocation, out int sensorListBoxHeight,
            out int vulnerabilityLabelLocation, int vulnerabilityLabelHeight, out int vulnerabilityListBoxLocation,
            out int vulnerabilityListBoxHeight)
        {
            int heightForPanels = panelHeight - topBuffer;
            const int bufferBetweenControls = 5;

            sensorLabelLocation = topBuffer;

            heightForPanels -= sensorLabelHeight - bufferBetweenControls;
            topBuffer += sensorLabelHeight + bufferBetweenControls;

            int heightForLists = (heightForPanels - (4 * bufferBetweenControls) - vulnerabilityLabelHeight) / 2;

            sensorListBoxHeight = heightForLists;
            vulnerabilityListBoxHeight = heightForLists;

            sensorListBoxLocation = topBuffer;

            heightForPanels -= sensorListBoxHeight - bufferBetweenControls;
            topBuffer += sensorListBoxHeight + bufferBetweenControls;

            vulnerabilityLabelLocation = topBuffer;

            heightForPanels -= vulnerabilityLabelHeight - bufferBetweenControls;
            topBuffer += vulnerabilityLabelHeight + bufferBetweenControls;

            vulnerabilityListBoxLocation = topBuffer;

            heightForPanels -= vulnerabilityListBoxHeight - bufferBetweenControls;
            topBuffer += vulnerabilityListBoxHeight + bufferBetweenControls;
        }

        private void listBoxSensors_Click(object sender, EventArgs e)
        {

            if (listBoxSensors.SelectedItem == null)
            {
                Console.WriteLine("Selected Sensor was NULL.");
                return;
            }
            Console.WriteLine("SENSORS list clicked!\r\nSelected value is {0}", listBoxSensors.SelectedItem);
            if (!InvokeRequired)
            {
                try
                {
                    if (_map_scene != null)
                    {
                        DDDObjects current = _map_scene.GetSelectedObject();
                        if (current != null)
                        {
                            string selectedSensor = string.Empty;
                            if (listBoxSensors.SelectedItem != null)
                            {
                                selectedSensor = listBoxSensors.SelectedItem.ToString();
                            }
                            current.ClearSensorRingValues();
                            if (selectedSensor == current.CurrentlySelectedSensor)
                            {
                                listBoxSensors.SelectedIndex = -1; //de-select?
                                current.CurrentlySelectedSensor = string.Empty;
                                current.CurrentlySelectedSensorRange = -1;
                                //current.CurrentCapabilityAndWeapon = -1;
                                //_map_scene.EnterMoveMode();
                            }
                            else
                            {
                                current.CurrentlySelectedSensor = selectedSensor;
                                current.CurrentlySelectedSensorRange = current.GetSensorLongestRangeRingRadius(selectedSensor);
                                // current.CurrentCapabilityAndWeapon = listBoxSensors.SelectedIndex;
                                //  _map_scene.EnterAttackMode();
                                Console.WriteLine("Selected Sensor: {0}; Selected Sensor Range: {1}", selectedSensor, current.CurrentlySelectedSensorRange);
                            }
                        }
                        else
                        { //Clear the list.
                            listBoxSensors.Items.Clear();
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
            else
            {
                BeginInvoke(new EventHandler(listBoxSensors_Click), sender, e);
            }

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_optionsDialog == null)
            {
                _optionsDialog = new OptionsDialog();
            }
            else
            {
                _optionsDialog.LoadCurrentSettings();
            }

            if (_optionsDialog.ShowDialog(this) == DialogResult.OK)
            { //if ok, call Save Settings; _optionsDialog.SaveDialogSettings()
                _optionsDialog.SaveDialogSettings();

                //somehow update existing rings?
                UpdateRangeRingDisplays(sender, e);
            }
        }

        /// <summary>
        /// This function will iterate over all objects that have range rings defined, and will
        /// re-define them based on current DDD_Global settings.  This is called after the user
        /// modifies the options section.
        /// </summary>
        private void UpdateRangeRingDisplays(object sender, EventArgs e)
        {
            if (!InvokeRequired)
            {
                List<string> objectIds = _map_scene.GetListOfPlayfieldObjects();
                DDDObjects obj = null;
                Color sensor = Color.FromArgb(DDD_RangeRings.GetInstance().SensorRangeRings.OpaqueRingColor);
                Color capability = Color.FromArgb(DDD_RangeRings.GetInstance().CapabilityRangeRings.OpaqueRingColor);
                Color vulnerability = Color.FromArgb(DDD_RangeRings.GetInstance().VulnerabilityRangeRings.OpaqueRingColor);


                foreach (string objectId in objectIds)
                {
                    obj = _map_scene.GetObjectAttributes(objectId);
                    if (obj == null)
                        continue;

                    if (obj.IsCapabilityRingSet())
                    {
                        obj.ClearCapabilityRingValues();
                    }
                    if (obj.IsSensorRingSet())
                    {
                        obj.ClearSensorRingValues();
                    }
                    if (obj.IsVulnerabilityRingSet())
                    {
                        obj.ClearVulnerabilityRingValues();
                    }
                }
            }
            else
            {
                BeginInvoke(new EventHandler(listBoxSensors_Click), sender, e);
            }
        }

        public void SendMapUpdate(bool sendNoMatterWhat)
        {
            if (_map_scene != null)
            {
                _map_scene.SendMapUpdate(sendNoMatterWhat);
            }
        }

        private void SendSelectedObjectEvent(string objectID)
        {
            string userID = DDD_Global.Instance.PlayerID;
            string ownerID = string.Empty;

            if (_map_scene != null)
            {
                DDDObjects obj = _map_scene.GetObjectAttributes(objectID);
                if (obj != null)
                {
                    ownerID = obj.OwnerID;
                }
                else
                {
                    objectID = string.Empty;
                }

                //send event
                _map_scene.SendObjectSelectedUpdate(userID, objectID, ownerID);

                if (objectID != string.Empty)
                {
                    SendTabSelectedEvent(_LastSentTabName);
                }
            }
        }

        private void SendWeaponOrCapabilitySelectedEvent(string weaponOrCapabilityName)
        {
            string userID = DDD_Global.Instance.PlayerID;
            string ownerID = string.Empty;
            string parentObjectID = string.Empty;
            bool isWeapon = false;

            if (_map_scene != null)
            {
                if (_map_scene.GetSelectedObject() != null)
                {
                    parentObjectID = _map_scene.GetSelectedObject().ObjectID;
                }
                //if (_map_scene.GetObjectAttributes(weaponOrCapabilityName) != null)
                //{
                //    isWeapon = _map_scene.GetObjectAttributes(weaponOrCapabilityName).IsWeapon;
                //    if (!isWeapon)
                //    {
                //        return; //in this case, a subplatform is selected, not a weapon.
                //    }
                //}

                _map_scene.SendWeaponSelectedUpdate(userID, parentObjectID, weaponOrCapabilityName, isWeapon);
            }
        }

        private string _LastSentTabName = string.Empty; //kind of a hack, but prevents across-thread access exceptions...

        private void SendTabSelectedEvent(string tabName)
        {
            string userID = DDD_Global.Instance.PlayerID;
            string objectID = string.Empty;
            if (_map_scene != null)
            {
                DDDObjects obj = _map_scene.GetSelectedObject();
                if (obj != null)
                {
                    objectID = obj.ObjectID;
                }
                _LastSentTabName = tabName;
                _map_scene.SendTabSelectionUpdate(userID, objectID, tabName);

            }
        }

        public void SetDrawCursor(bool userDrawCursor)
        {
            if (userDrawCursor)
            {
                _map_scene.EnterDrawMode();
            }
            else
            {
                _map_scene.EnterMoveMode();
            }
            ChangeMapState();
        }

        private void buttonSetClassification_Click(object sender, EventArgs e)
        {
            if (_map_scene.GetSelectedObject() == null)
                return;
            String objID = _map_scene.GetSelectedObject().ObjectID;
            if(objID == "")
                return;
            ClassificationDialog dlg = new ClassificationDialog(objID, _classificationsEnum, _map_scene.GetSelectedObject().Classification);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                String newClassification = dlg.GetClassification();
                
                //send classification
                _Controller.RequestClassification(objID, newClassification, DDD_Global.Instance.PlayerID);
            }
        }

        private void buttonSetTag_Click(object sender, EventArgs e)
        {
            if (_map_scene.GetSelectedObject() == null)
                return;
            String objID = _map_scene.GetSelectedObject().ObjectID;
            if (objID == "")
                return;
            TextboxDialog dlg = new TextboxDialog("Change Asset Tag", "Enter the new Tag for this asset", textBoxUnitTag.Text);
            String newTag = dlg.ShowDialog(this);

            if (newTag != textBoxUnitTag.Text)
            {
                //send tag
                _Controller.ChangeTagRequest(DDD_Global.Instance.PlayerID, objID, newTag);
            }
            /*
            string prevTag = string.Empty;
            ((TextBox)sender).Parent.Focus();
            bool encounteredError = false;

            if (_map_scene == null)
            {
                Console.WriteLine("In buttonTagObject_Click: Current Scene is null");
                encounteredError = true;
            }
            if (!encounteredError)
            {
                DDDObjects obj = _map_scene.GetSelectedObject();
                if (obj == null)
                {
                    Console.WriteLine("In buttonTagObject_Click: Selected object is null");
                    encounteredError = true;
                }

                if (!encounteredError)
                {
                    string selectedObjectID = obj.ObjectID;
                    prevTag = obj.Tag;
                    string newTag = ((TextBox)sender).Text;

                    if (newTag == prevTag)
                    {
                        encounteredError = true;
                    }
                    else
                    {
                        _map_scene.ChangeTagRequest(DDD_Global.Instance.PlayerID, selectedObjectID, newTag);
                    }
                }
            }

            if (encounteredError)
            {
                ((TextBox)sender).Text = prevTag;
            }
             */
        }
    }
}
