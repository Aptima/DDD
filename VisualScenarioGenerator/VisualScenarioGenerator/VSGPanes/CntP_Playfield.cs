using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator;
using VisualScenarioGenerator.Dialogs;

using AGT.Scenes;
using AGT.Forms;

namespace VisualScenarioGenerator.VSGPanes
{
    public enum ContentPanelMode : int { Playfield, Timeline, Preview }
    

    public partial class CntP_Playfield : Ctl_ContentPane, ITimelinePanel
    {
        private ContentPanelMode Mode = ContentPanelMode.Playfield;
        private DDD_Playfield playfield;
        private bool FrameworkStarted = false;

        private Dlg_Event dlgEvent = new Dlg_Event();
        private Dlg_Node_ChangeEngramEvent dlgChangeEngramEvent = new Dlg_Node_ChangeEngramEvent();
        private Dlg_Node_CloseChatRoomEvent dlgCloseChatRoomEvent = new Dlg_Node_CloseChatRoomEvent();
        private Dlg_Node_FlushEvent dlgFlushEvent = new Dlg_Node_FlushEvent();
        private Dlg_Node_LaunchEvent dlgLaunchEvent = new Dlg_Node_LaunchEvent();
        private Dlg_Node_MoveEvent dlgMoveEvent = new Dlg_Node_MoveEvent();
        private Dlg_Node_OpenChatRoomEvent dlgOpenChatRoomEvent = new Dlg_Node_OpenChatRoomEvent();
        private Dlg_Node_RemoveEngramEvent dlgRemoveEngramEvent = new Dlg_Node_RemoveEngramEvent();
        private Dlg_Node_RevealEvent dlgRevealEvent = new Dlg_Node_RevealEvent();
        private Dlg_Node_StateChangeEvent dlgStateEvent = new Dlg_Node_StateChangeEvent();
        private Dlg_Node_TransferEvent dlgTransferEvent = new Dlg_Node_TransferEvent();



        private const string Flush_Event = "Flush";
        private const string Move_Event = "Move";
        private const string Launch_Event = "Launch";
        private const string Reveal_Event = "Reveal";
        private const string Transfer_Event = "Transfer";
        private const string StateChange_Event = "State Change";

        private const string OpenChatRoom_Event = "Open Chat Room";
        private const string CloseChatRoom_Event = "Close Chat Room";

        private const string ChangeEngram_Event = "Change Engram";
        private const string RemoveEngram_Event = "Remove Engram";



        private string[] AssetEventList = {
            Flush_Event,
            Launch_Event,
            Move_Event,
            Reveal_Event,
            StateChange_Event,
            Transfer_Event
        };
        private string[] TopLevelEventList = {
            OpenChatRoom_Event,
            CloseChatRoom_Event,
            ChangeEngram_Event,
            RemoveEngram_Event
        };


        private string _current_asset = string.Empty;
        private Dictionary<string, AssetTimelines> _timeline = new Dictionary<string, AssetTimelines>();

        private Ctl_PreviewScenario preview;
        public Ctl_PreviewScenario CtlPreview
        {
            get
            {
                return preview;
            }
        }


        public CntP_Playfield()
        {
            InitializeComponent();
            playfield = new DDD_Playfield();
            playfield.OnMouseSelection += new MouseSelectionHandler(AssetSelectionChanged);

            playfield.ImageLibraryPath = string.Format("{0}\\Resources\\{1}", System.Environment.CurrentDirectory, "ImageLib.dll");
            playfield.MapFile = "Resources\\Ramadi.jpg";

            preview = new Ctl_PreviewScenario();
            preview.Parent = splitContainer1.Panel2;
            preview.Hide();

            cc.AddScene(playfield);
            _timeline.Add(string.Empty, new AssetTimelines(new TimelinePanel(), new Dictionary<string, Node>()));
            _timeline[_current_asset].CtlTimeline.ContentPane = this;
            _timeline[_current_asset].CtlTimeline.HeaderText = "Global Events";
            _timeline[_current_asset].CtlTimeline.Parent = splitContainer1.Panel2;
            _timeline[_current_asset].CtlTimeline.Dock = DockStyle.Fill;
            _timeline[_current_asset].CtlTimeline.Hide();

            ChangeMode(ContentPanelMode.Playfield);
        }


        public void AssetSelectionChanged(string asset_id)
        {
            switch (Mode)
            {
                case ContentPanelMode.Preview:
                case ContentPanelMode.Timeline:
                    if (asset_id != _current_asset)
                    {
                        if (_timeline.ContainsKey(_current_asset))
                        {
                            _timeline[_current_asset].CtlTimeline.Hide();
                        }
                        _current_asset = asset_id;

                        SelectAssetInstanceDataStruct d = new SelectAssetInstanceDataStruct();
                        d.ID = _current_asset;
                        Notify(d);

                        if (_timeline.ContainsKey(_current_asset))
                        {
                            _timeline[_current_asset].CtlTimeline.Show();
                        }
                        else
                        {
                            _timeline.Add(_current_asset, new AssetTimelines(new TimelinePanel(), new Dictionary<string, Node>()));
                            _timeline[_current_asset].CtlTimeline.ContentPane = this;
                            if (_current_asset == string.Empty)
                            {
                                _timeline[_current_asset].CtlTimeline.HeaderText = "Global Events";
                            }
                            else
                            {
                                _timeline[_current_asset].CtlTimeline.HeaderText = _current_asset + " Events";
                            }
                            _timeline[_current_asset].CtlTimeline.Parent = splitContainer1.Panel2;
                            _timeline[_current_asset].CtlTimeline.Dock = DockStyle.Fill;
                            _timeline[_current_asset].CtlTimeline.Show();
                        }
                    }
                    break;
                case ContentPanelMode.Playfield:
                    break;
            }
        }

        private void CntP_Playfield_Load(object sender, EventArgs e)
        {


        }

        public void SetScene(string IconLibrary, string Mapfile, Bitmap Map)
        {
            if (playfield != null)
            {
                if ((playfield.ImageLibraryPath != IconLibrary) || (playfield.MapFile != Mapfile))
                {
                    playfield.ImageLibraryPath = IconLibrary;
                    playfield.MapFile = Mapfile;

                    if (FrameworkStarted)
                    {
                        playfield.State = AGT.GameFramework.SceneState.INIT;
                        cc.RestartCurrentScene();
                    }
                    else
                    {
                        if (cc != null)
                        {
                            cc.StartFramework();
                            FrameworkStarted = true;
                        }
                    }
                }
            }
        }


        public void ChangeMode(ContentPanelMode mode)
        {
            Mode = mode;
            switch (Mode)
            {
                case ContentPanelMode.Playfield:
                    preview.Hide();
                    _timeline[_current_asset].CtlTimeline.Hide();

                    playfield.SimulateMotion = false;
                    playfield.ChangeMode(PlayfieldModeType.Zone);
                    splitContainer1.Panel2Collapsed = true;
                    break;

                case ContentPanelMode.Timeline:
                    preview.Hide();
                    _timeline[_current_asset].CtlTimeline.Show();

                    splitContainer1.Panel2Collapsed = false;
                    playfield.SimulateMotion = false;
                    playfield.ChangeMode(PlayfieldModeType.Select);
                    break;

                case ContentPanelMode.Preview:
                    preview.Show();
                    _timeline[_current_asset].CtlTimeline.Hide();

                    splitContainer1.Panel2Collapsed = false;
                    playfield.SimulateMotion = true;
                    break;            
            }
        }
        public void ShowPlayfield(bool Visible)
        {
            if (Visible)
            {
                if (FrameworkStarted)
                {
                    cc.ResumeFramework();
                }
                else
                {
                    cc.StartFramework();
                    FrameworkStarted = true;
                }
            }
            else
            {
                if (FrameworkStarted)
                {
                    if (!cc.Suspended)
                    {
                        cc.SuspendFramework();
                    }
                }
            }

        }

        #region ITimelinePanel Members

        public bool BeforeNodeAdd(string TrackName, int time_tick)
        {
            if (_timeline.ContainsKey(_current_asset))
            {
                Console.WriteLine("BeforeNode Add {0}", _timeline[_current_asset].GetEventType(TrackName));
                switch (_timeline[_current_asset].GetEventType(TrackName))
                {
                    case Ctl_Event.EVENT_CHANGE_ENGRAM:
                        if (dlgChangeEngramEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_CLOSE_CHATROOM:
                        if (dlgCloseChatRoomEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_FLUSH:
                        if (dlgFlushEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_LAUNCH:
                        if (dlgLaunchEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_OPEN_CHATROOM:
                        if (dlgOpenChatRoomEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                        }
                        break;
                    case Ctl_Event.EVENT_REMOVE_ENGRAM:
                        if (dlgRemoveEngramEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_REVEAL:
                        if (dlgRevealEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_STATE_CHANGE:
                        if (dlgStateEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_TRANSFER:
                        if (dlgTransferEvent.ShowDialog(Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                    case Ctl_Event.EVENT_MOVE:
                        if (dlgMoveEvent.ShowDialog(this.Parent) == DialogResult.OK)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        public void AfterNodeAdd(string TrackName)
        {

        }

        public bool BeforeNodeDelete(string TrackName, int time_tick)
        {
            return true;
        }

        public void AfterNodeDelete(string TrackName)
        {
        }

        public bool BeforeRemoveTimelineTrack(string TrackName)
        {
            return true;
        }

        public bool BeforeAddTimelineTrack(out string TrackName)
        {
            TrackName = string.Empty;
            try
            {
                dlgEvent.StartPosition = FormStartPosition.CenterParent;

                if (_current_asset == string.Empty)
                {
                    dlgEvent.SetMode(EventModeEnum.Global);
                }
                else
                {
                    dlgEvent.SetMode(EventModeEnum.Asset);
                }

                if (dlgEvent.ShowDialog(Parent.Parent.Parent) == DialogResult.OK)
                {
                    TrackName = dlgEvent.Data.EventName;
                    _timeline[_current_asset].SetEvent(TrackName, dlgEvent.Data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ", " + e.StackTrace);
            }

            return false;
        }

        public void AfterAddTimelineTrack(string TrackName)
        {
        }

        public void NodeSelectionChange(int time_tick)
        {
        }

        public void TimelineTrackSelectionChange(string track_name)
        {
        }

        #endregion

        public override void Update(object object_data)
        {
            if (object_data is NewAssetInstanceDataStruct)
            {
                NewAssetInstanceDataStruct data = (NewAssetInstanceDataStruct)object_data;
                if (playfield.HasPawn(data.ID))
                {
                    MessageBox.Show(cc.Parent, string.Format("An asset of with an ID of {0} already exists.", data.ID));
                }
                else
                {
                    playfield.AddMapObject(data.ID, "ImageLib.f16_small.png", 36, 14, 0, Color.DodgerBlue);
                    Notify(data);
                }                
            }

            if (object_data is DeleteAssetInstanceDataStruct)
            {
                DeleteAssetInstanceDataStruct data = (DeleteAssetInstanceDataStruct)object_data;
                if (playfield.HasPawn(data.ID))
                {
                    playfield.RemoveMapObject(data.ID);
                }
            }

            if (object_data is SelectAssetInstanceDataStruct)
            {
                SelectAssetInstanceDataStruct data = (SelectAssetInstanceDataStruct)object_data;
                if (playfield.HasPawn(data.ID))
                {
                    playfield.SelectPawn(data.ID);
                    AssetSelectionChanged(data.ID);
                }
            }
        }
    }

    class Node
    {
        public Struct_DlgEvent Event = Struct_DlgEvent.Empty;
        public Dictionary<string, Dictionary<int, object>> Nodes = new Dictionary<string, Dictionary<int, object>>();

        public Node(Struct_DlgEvent event_data)
        {
            Event = event_data;
        }

    }
    class AssetTimelines
    {
        public TimelinePanel CtlTimeline;
        private Dictionary<string, Node> _eventtracks = new Dictionary<string, Node>();

        public static AssetTimelines Empty = new AssetTimelines(null, null);

        public AssetTimelines(TimelinePanel control, Dictionary<string, Node> event_track)
        {
            CtlTimeline = control;
            _eventtracks = event_track;
        }
        public void SetEvent(string trackname, Struct_DlgEvent event_data)
        {
            _eventtracks.Add(trackname, new Node(event_data));
        }
        public string GetEventType(string trackname)
        {
            return _eventtracks[trackname].Event.EventType;
        }
        public void SetTimelineNodes(string trackname, Dictionary<string, Dictionary<int, object>> timeline)
        {
            _eventtracks[trackname].Nodes = timeline;
        }

    }

}
