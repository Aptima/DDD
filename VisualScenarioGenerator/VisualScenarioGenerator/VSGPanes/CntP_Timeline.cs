using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Dialogs;
using AGT.Forms;
using AGT.Scenes;

namespace VisualScenarioGenerator.VSGPanes
{
    public struct Struct_Timeline
    {
        public Struct_DlgEvent EventData;
        public SortedDictionary<int, object> Timeline;
        public static Struct_Timeline Empty = new Struct_Timeline(Struct_DlgEvent.Empty);

        public Struct_Timeline(Struct_DlgEvent event_data)
        {
            EventData = event_data;
            Timeline = new SortedDictionary<int, object>();
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Struct_Timeline)
            {
                return ((Struct_Timeline)obj).EventData.EventName == EventData.EventName;
            }
            return false;
        }

    }
    public partial class CntP_Timeline : Ctl_ContentPane, ITimelinePanel
    {
        private DDD_Playfield playfield;
        private bool FrameworkStarted = false;
        private Dlg_Event dlgEvent = new Dlg_Event();
        private Dlg_Node_FlushEvent dlgFlushEvent = new Dlg_Node_FlushEvent();
        private Dlg_Node_MoveEvent dlgMoveEvent = new Dlg_Node_MoveEvent();
        private Dlg_Node_StateChangeEvent dlgStateEvent = new Dlg_Node_StateChangeEvent();

        private Dictionary<string, Struct_Timeline> _timeline = new Dictionary<string, Struct_Timeline>();
        public Dictionary<string, Struct_Timeline> Timeline
        {
            get
            {
                return this._timeline;
            }
        }

        public bool HasSelection
        {
            get
            {
                return timelinePanel1.HasSelection;
            }
        }
        public CntP_Timeline()
        {
            InitializeComponent();
            Console.WriteLine("!!!!!Set content panel instance");
            timelinePanel1.ContentPane = this;
        }

        //public void AddTimelineTrack(string track_name)
        //{
        //    timelinePanel1.AddTimelineTrack(track_name);
        //}

        //public void AddNode()
        //{
        //    timelinePanel1.AddNode();
        //}

        //public void SetTrackName(string name)
        //{
        //    timelinePanel1.ChangeSelectedTrackName(name);
        //}

        //public void RemoveTimelineTrack()
        //{
        //    timelinePanel1.RemoveSelectedTimelineTrack();
        //}

        //public string SelectedTrackName()
        //{
        //    return timelinePanel1.SelectedTrackName();
        //}

        //public void MoveSelectedTrackUp()
        //{
        //    timelinePanel1.MoveSelectedTrackUp();
        //}

        //public void MoveSelectedTrackDown()
        //{
        //    timelinePanel1.MoveSelectedTrackDown();
        //}

        //public bool HasTrack(string track_name)
        //{
        //    return timelinePanel1.HasTrack(track_name);
        //}

        //public void ClearTimelineNodes(string track_name)
        //{
        //    timelinePanel1.ClearTrack(track_name);
        //}

        //public int TimelineNodeCount(string track_name)
        //{
        //    return timelinePanel1.NodeCount(track_name);
        //}

        #region ITimelinePanel Members

        public bool BeforeNodeAdd(string TrackName, int time_tick)
        {
            if (Timeline.ContainsKey(TrackName))
            {
                Struct_Timeline t = Timeline[TrackName];
                // Show Dialog
                if (t.EventData.EventType == Ctl_Event.EVENT_MOVE)
                {
                    if (dlgMoveEvent.ShowDialog(this.Parent) == DialogResult.OK)
                    {
                        if (t.Timeline.ContainsKey(time_tick))
                        {
                            t.Timeline[time_tick] = "Change Value";
                        }
                        else
                        {
                             t.Timeline.Add(time_tick, "New Value");
                        }
                        return true;
                    }
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
            Console.WriteLine("Before Add Timeline Track");
            TrackName = string.Empty;

            dlgEvent.StartPosition = FormStartPosition.CenterParent;
            if (dlgEvent.ShowDialog(Parent.Parent.Parent) == DialogResult.OK)
            {
                TrackName = dlgEvent.Data.EventName;
                Timeline.Add(TrackName, new Struct_Timeline(dlgEvent.Data));
            }            

            return true;
        }

        public void AfterAddTimelineTrack(string TrackName)
        {
            ((View_Timeline)_view).GetNavigationPanel().ShowProperties();
        }

        public void NodeSelectionChange(int time_tick)
        {
            ((View_Timeline)_view).GetNavigationPanel().NodeSelectionChange(time_tick);
        }

        public void TimelineTrackSelectionChange(string track_name)
        {
            if (_view is View_Timeline)
            {
                ((View_Timeline)_view).GetNavigationPanel().TimelineTrackSelectionChange(track_name);
            }
        }

        #endregion

        public void ShowPlayfield(bool Visible)
        {
            Console.WriteLine("Timeline Content: Show Playfield");
            if (Visible)
            {
                if (FrameworkStarted)
                {
                    agT_CanvasControl1.ResumeFramework();
                }
                else
                {

                    agT_CanvasControl1.StartFramework();
                    FrameworkStarted = true;
                }
            }
            else
            {
                if (FrameworkStarted)
                {
                    if (!agT_CanvasControl1.Suspended)
                    {
                        agT_CanvasControl1.SuspendFramework();
                    }
                }
            }

        }

        private void timelinePanel1_Load(object sender, EventArgs e)
        {
            playfield = new DDD_Playfield();
            playfield.UnitTestMode = true;
            playfield.SimulateMotion = false;
            playfield.ImageLibraryPath = string.Format("{0}\\Resources\\{1}", System.Environment.CurrentDirectory, "ImageLib.dll");
            playfield.MapFile = "Resources\\Ramadi.jpg";

            agT_CanvasControl1.AddScene(playfield);


        }

        private void CntP_Timeline_Load(object sender, EventArgs e)
        {
            timelinePanel1.ContentPane = this;
        }


    }
}
