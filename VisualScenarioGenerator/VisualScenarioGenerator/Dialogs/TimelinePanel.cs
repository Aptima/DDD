using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public struct EventTimeline
    {

        public string Name;
        public Control TimelineHeader;
        public SortedDictionary<int, RectangleF> Timeline;
        public static EventTimeline Empty = new EventTimeline(string.Empty, new Control(), new SortedDictionary<int, RectangleF>());

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object o)
        {
            if (!(o is EventTimeline))
                return false;
            return (this == (EventTimeline)o);
        }
        public static bool operator ==(EventTimeline t1, EventTimeline t2)
        {
            return ((t1.Name == t2.Name) && (t1.Timeline == t2.Timeline) && (t1.TimelineHeader == t2.TimelineHeader));
        }
        public static bool operator !=(EventTimeline t1, EventTimeline t2)
        {
            return ((t1.Name != t2.Name) && (t1.Timeline != t2.Timeline) && (t1.TimelineHeader != t2.TimelineHeader));
        }
        public EventTimeline(string name, Control control, SortedDictionary<int, RectangleF> time)
        {
            this.Name = name;
            Timeline = time;
            TimelineHeader = control;
        }
    }
    
    public struct Node
    {
        public int time_tick;
        public RectangleF position;
        public static Node Empty = new Node(-1, RectangleF.Empty);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object o)
        {
            if (!(o is Node))
                return false;
            return (this == (Node)o);
        }
        public static bool operator ==(Node t1, Node t2)
        {
            return ((t1.time_tick == t2.time_tick) && (t1.position == t2.position));
        }
        public static bool operator !=(Node t1, Node t2)
        {
            return ((t1.time_tick != t2.time_tick) && (t1.position != t2.position));
        }

        public Node(int tick, RectangleF rect)
        {
            time_tick = tick;
            position = rect;
        }
    }



    public partial class TimelinePanel : UserControl, ICtl_ContentPane__OutboundUpdate
    {
        private Dlg_TimelineProperties _dlg_properties = new Dlg_TimelineProperties();

        public Ctl_ContentPane ContentPane = null;
        public string HeaderText
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }


        #region private
        private string _selected_trackname = string.Empty;
        private Bitmap _node_image = null;

        private Rectangle _highlight_rect = Rectangle.Empty;
        private Node _node_position = Node.Empty;

        private Font _timeline_font = new Font("Lucida Console", 6);


        private SortedDictionary<string, EventTimeline> _timeline_tracks = new SortedDictionary<string, EventTimeline>();
        #endregion


        public SortedDictionary<string, EventTimeline> Tracks
        {
            get
            {
                return _timeline_tracks;
            }
            set
            {
                _timeline_tracks = value;
                Timeline.Invalidate();
                TimelineScale.Invalidate();
            }
        }

        public bool AreYouSure = false;

        public bool HasSelection
        {
            get
            {
                return (_selected_trackname != string.Empty);
            }
        }

        
        private static int _Ticks = 0;
        public static int Ticks
        {
            get
            {
                return _Ticks;
            }
        }
        public float TickSpacing = 25.5f;
        public int TickHeight = 24;

        public Brush AlternateColor1 = Brushes.LightGray;
        public Brush AlternateColor2 = Brushes.Transparent;
        public Pen LineColor = Pens.Gray;


        public TimelinePanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            SetTicks(7600);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            _node_image = new Bitmap(assembly.GetManifestResourceStream("VisualScenarioGenerator.images.Node.png"));
        }

        public void SetTicks(int ticks)
        {
            _Ticks = ticks;
            Timline_HScroll.Maximum = _Ticks;
            Timline_HScroll.Minimum = 0;
            Timline_HScroll.Value = 0;
            Timline_HScroll.SmallChange = 1;
            Timline_HScroll.LargeChange = 10;

            Timeline.Invalidate();
            TimelineScale.Invalidate();

        }
        public void ClearTrack(string track_name)
        {
            _timeline_tracks[track_name].Timeline.Clear();
            Timeline.Invalidate();
        }

        public int  NodeCount(string track_name)
        {
            if (_timeline_tracks.ContainsKey(track_name))
            {
                return _timeline_tracks[track_name].Timeline.Count;
            }
            return 0;
        }

        public bool HasTrack(string track_name)
        {
            return _timeline_tracks.ContainsKey(track_name);
        }

        public void SelectTimelineTrack(string event_name)
        {
            if ((_selected_trackname != string.Empty) &&(_timeline_tracks.ContainsKey(_selected_trackname)))
            {
                _timeline_tracks[_selected_trackname].TimelineHeader.ForeColor = SystemColors.GrayText;
            }

            _selected_trackname = event_name;

            Control c = _timeline_tracks[_selected_trackname].TimelineHeader;
            c.ForeColor = SystemColors.ControlText;
            c.Select();

            _highlight_rect.X = c.Location.X;
            _highlight_rect.Y = c.Location.Y;
            _highlight_rect.Width = Timeline.Width-1;
            _highlight_rect.Height = c.Height - 2;

            Timeline.Invalidate();
        }

        public void AddNode()
        {
            if ((_node_position != Node.Empty) && (_selected_trackname != string.Empty))
            {
                if (!_timeline_tracks[_selected_trackname].Timeline.ContainsKey(_node_position.time_tick))
                {

                    _timeline_tracks[_selected_trackname].Timeline.Add(_node_position.time_tick, _node_position.position);
                    Timeline.Height = TrackPanel.Height;
                }
                _node_position = Node.Empty;
                Timeline.Invalidate();
                TimelineScale.Invalidate();
            }
        }

        public void RemoveTimelineTrack(string _selected_timeline_str)
        {
            try
            {
                int order = TrackPanel.Controls.IndexOf(_timeline_tracks[_selected_timeline_str].TimelineHeader);
                TrackPanel.Controls.Remove(_timeline_tracks[_selected_timeline_str].TimelineHeader);
                _timeline_tracks.Remove(_selected_timeline_str);

                if (_timeline_tracks.Count > 0)
                {
                    if (_timeline_tracks.Count > order)
                    {
                        SelectTimelineTrack(TrackPanel.Controls[order].Text);
                    }
                    else
                    {
                        SelectTimelineTrack(TrackPanel.Controls[TrackPanel.Controls.Count - 1].Text);
                    }
                }
                else
                {
                    _selected_timeline_str = string.Empty;
                    _highlight_rect = Rectangle.Empty;
                }

                Timeline.Invalidate();
                TimelineScale.Invalidate();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void RemoveSelectedTimelineTrack()
        {
            RemoveTimelineTrack(_selected_trackname);
        }

        public string SelectedTrackName()
        {
            return _selected_trackname;
        }

        public void ChangeSelectedTrackName(string name)
        {
            if (!_timeline_tracks.ContainsKey(name))
            {
                EventTimeline event_timeline = _timeline_tracks[_selected_trackname];
                _timeline_tracks.Remove(_selected_trackname);

                event_timeline.Name = name;
                event_timeline.TimelineHeader.Text = name;

                _timeline_tracks.Add(name, event_timeline);
                SelectTimelineTrack(name);
            }
            else
            {
                MessageBox.Show("The requested Track exists already.", "Error renaming Timeline Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void AddTimelineTrack(string name)
        {
            if (!_timeline_tracks.ContainsKey(name))
            {
                Button b = new Button();
                b.Height = TickHeight;
                TrackPanel.VerticalScroll.SmallChange = TickHeight;
                TrackPanel.VerticalScroll.LargeChange = TickHeight;
                b.Text = name;
                b.MouseClick += new MouseEventHandler(this.TimelineTrackClicked);
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Parent = TrackPanel;
                b.Dock = DockStyle.Top;
                b.BringToFront();
                b.Show();

                _timeline_tracks.Add(b.Text, new EventTimeline(b.Text, b, new SortedDictionary<int, RectangleF>()));
                SelectTimelineTrack(name);
            }
            else
            {
                MessageBox.Show("The requested Track exists already.", "Error adding Timeline Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        public void MoveSelectedTrackDown()
        {
            int order = TrackPanel.Controls.IndexOf(_timeline_tracks[_selected_trackname].TimelineHeader);
            Console.WriteLine("MoveSelectedEventDown:  {0}, {1}", _selected_trackname, order);
            if (order > 0)
            {
                order--;
                TrackPanel.Controls.SetChildIndex(_timeline_tracks[_selected_trackname].TimelineHeader, order);
                _highlight_rect.Y = _timeline_tracks[_selected_trackname].TimelineHeader.Location.Y;
                Timeline.Invalidate();
            }
        }
        
        public void MoveSelectedTrackUp()
        {
            Console.WriteLine("MoveSelectedEventUp:  {0}", _selected_trackname);
            int order = TrackPanel.Controls.IndexOf(_timeline_tracks[_selected_trackname].TimelineHeader);
            if (order < (TrackPanel.Controls.Count  -1))
            {
                order++;
                TrackPanel.Controls.SetChildIndex(_timeline_tracks[_selected_trackname].TimelineHeader, order);
                _highlight_rect.Y = _timeline_tracks[_selected_trackname].TimelineHeader.Location.Y;
                Timeline.Invalidate();
            }
        }



        #region  Events
        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectedTrackDown();
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveSelectedTrackUp();
        }

        private void addTimelineTrackStripMenuItem_Click(object sender, EventArgs e)
        {
            bool result = true;
            string track_name = string.Format("New Event #{0}", _timeline_tracks.Count + 1);
            
            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    result = ((ITimelinePanel)ContentPane).BeforeAddTimelineTrack(out track_name);
                }
            }
            catch (Exception)
            {
            }

            if (result)
            {
                AddTimelineTrack(track_name);
            }

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    ((ITimelinePanel)ContentPane).AfterAddTimelineTrack(track_name);
                }
            }
            catch (Exception)
            {
            }

        }

        private void deleteTimelineTrackStripMenuItem_Click(object sender, EventArgs e)
        {
            bool result = true;

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    result = ((ITimelinePanel)ContentPane).BeforeRemoveTimelineTrack(_selected_trackname);
                }
            }
            catch (Exception)
            {
            }

            if (result)
            {
                if (_selected_trackname != string.Empty)
                {
                    RemoveTimelineTrack(_selected_trackname);
                }
            }
        }
        
        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool result = true;

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    result = ((ITimelinePanel)ContentPane).BeforeNodeAdd(_selected_trackname, _node_position.time_tick);
                }
            }
            catch (Exception)
            {
            }

            if (result)
            {
                AddNode();

                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        ((ITimelinePanel)ContentPane).AfterNodeAdd(_selected_trackname);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
       
        private void deleteNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if ((_node_position != Node.Empty) && (_selected_trackname != string.Empty))
            {
                bool result = true;

                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        result = ((ITimelinePanel)ContentPane).BeforeNodeDelete(_selected_trackname, _node_position.time_tick);
                    }
                }
                catch (Exception)
                {
                }

                if (result)
                {
                    if (_timeline_tracks[_selected_trackname].Timeline.ContainsKey(_node_position.time_tick))
                    {
                        _timeline_tracks[_selected_trackname].Timeline.Remove(_node_position.time_tick);
                        try
                        {
                            if (ContentPane is ITimelinePanel)
                            {
                                ((ITimelinePanel)ContentPane).AfterNodeDelete(_selected_trackname);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    _node_position = Node.Empty;
                    Timeline.Invalidate();
                    TimelineScale.Invalidate();
                }
            }

        }

        private void nextNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int current_node = Timline_HScroll.Value;
            if (_node_position != Node.Empty)
            {
                current_node = _node_position.time_tick;
            }
            if (_selected_trackname != string.Empty)
            {
                foreach (int tick in _timeline_tracks[_selected_trackname].Timeline.Keys)
                {
                    if (tick > current_node)
                    {

                        Timline_HScroll.Value = tick;
                        _node_position.time_tick = tick;
                        _node_position.position.Width = TickSpacing;
                        _node_position.position.Height = _highlight_rect.Height;
                        _node_position.position.X = 0;
                        _node_position.position.Y = _highlight_rect.Y;

                        try
                        {
                            if (ContentPane is ITimelinePanel)
                            {
                                ((ITimelinePanel)ContentPane).NodeSelectionChange(tick);
                            }
                        }
                        catch (Exception)
                        {
                        }

                        Timeline.Invalidate();
                        return;
                    }
                }
            }
        }

        private void previousNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int current_node = Timline_HScroll.Value;
            if (_node_position != Node.Empty)
            {
                current_node = _node_position.time_tick;
            }
            if (_selected_trackname != string.Empty)
            {
                int prev_tick = current_node;
                foreach (int tick in _timeline_tracks[_selected_trackname].Timeline.Keys)
                {
                    System.Console.WriteLine("\nTimeline Value: {0}, {1}, {2}", tick, prev_tick, Timline_HScroll.Value);

                    if (tick == current_node)
                    {
                        break;
                    }

                    prev_tick = tick;
                }

                try
                {
                    Timline_HScroll.Value -= Math.Abs(current_node - prev_tick);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Timline_HScroll.Value = 0;
                }
                System.Console.WriteLine("Timeline Value: {0}", Timline_HScroll.Value);
                _node_position.time_tick = prev_tick;
                _node_position.position.Width = TickSpacing;
                _node_position.position.Height = _highlight_rect.Height;
                _node_position.position.X = (prev_tick - Timline_HScroll.Value) * TickSpacing;
                _node_position.position.Y = _highlight_rect.Y;

                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        ((ITimelinePanel)ContentPane).NodeSelectionChange(prev_tick);
                    }
                }
                catch (Exception)
                {
                }

                Timeline.Invalidate();
            }

        }

 
        
        private void Timeline_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_node_position.time_tick < Ticks)
            {
                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        ((ITimelinePanel)ContentPane).BeforeNodeAdd(_selected_trackname, _node_position.time_tick);
                    }
                }
                catch (Exception)
                {
                }

                AddNode();

                if (e.X > (Timeline.Width - TickSpacing))
                {
                    Timline_HScroll.Value++;
                }

                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        ((ITimelinePanel)ContentPane).AfterNodeAdd(_selected_trackname);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        
        private void Timeline_MouseUp(object sender, MouseEventArgs e)
        {
            if (_highlight_rect.Contains(e.Location) && (e.Button != MouseButtons.Right))
            {
                int box = (int)((e.X) / TickSpacing);
                _node_position.time_tick = box + Timline_HScroll.Value;
                _node_position.position.Width = TickSpacing;
                _node_position.position.Height = _highlight_rect.Height;
                _node_position.position.X = box * TickSpacing;
                _node_position.position.Y = _highlight_rect.Y;

                if (!_timeline_tracks[_selected_trackname].Timeline.ContainsKey(_node_position.time_tick))
                {
                    TimelineMenuStrip.Items[0].Enabled = true;
                    TimelineMenuStrip.Items[1].Enabled = false;
                }
                else
                {
                    TimelineMenuStrip.Items[0].Enabled = false;
                    TimelineMenuStrip.Items[1].Enabled = true;
                }

                Timeline.Invalidate();

                try
                {
                    if (ContentPane is ITimelinePanel)
                    {
                        ((ITimelinePanel)ContentPane).NodeSelectionChange(_node_position.time_tick);
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                foreach (EventTimeline t in _timeline_tracks.Values)
                {
                    int y1 = t.TimelineHeader.Top;
                    int y2 = t.TimelineHeader.Bottom;

                    if ((e.Y >= y1) && (e.Y <= y2))
                    {
                        t.TimelineHeader.Select();

                        _highlight_rect.X = 0;
                        _highlight_rect.Y = t.TimelineHeader.Location.Y;
                        _highlight_rect.Width = Timeline.Width-1;
                        _highlight_rect.Height = t.TimelineHeader.Height - 2;

                        _selected_trackname = t.TimelineHeader.Text;

                        Timeline.Invalidate();

                        try
                        {
                            if (ContentPane is ITimelinePanel)
                            {
                                ((ITimelinePanel)ContentPane).TimelineTrackSelectionChange(_selected_trackname);
                            }
                        }
                        catch (Exception)
                        {
                        }

                    }
                }
            }
        }
        

        private void Timeline_Resize(object sender, EventArgs e)
        {
            if (_highlight_rect != Rectangle.Empty)
            {
                _highlight_rect.Width = Timeline.Width-1;
                _node_position = Node.Empty;
            }
        }

        private void TimelineTrackClicked(object sender, MouseEventArgs e)
        {
            _node_position = Node.Empty;

            Button event_label = ((Button)sender);

            _highlight_rect.X = 0;
            _highlight_rect.Y = event_label.Location.Y;
            _highlight_rect.Width = Timeline.Width-1;
            _highlight_rect.Height = event_label.Height - 2;

            SelectTimelineTrack(event_label.Text);

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    ((ITimelinePanel)ContentPane).TimelineTrackSelectionChange(event_label.Text);
                }
            }
            catch (Exception)
            {
            }

        }

        private void Timeline_HScroll_ValueChanged(object sender, EventArgs e)
        {
            _node_position = Node.Empty;
            Timeline.Invalidate();
            TimelineScale.Invalidate();
        }
        
        private void TrackPanel_Scroll(object sender, ScrollEventArgs e)
        {
            if (_highlight_rect != Rectangle.Empty)
            {
                _highlight_rect.Y = _highlight_rect.Y - (e.NewValue - e.OldValue);
            }
            Timeline.Invalidate();
        }



        private void TimelineScale_Paint(object sender, PaintEventArgs e)
        {
            string tick_str;

            for (int i = 0; i < (_Ticks - Timline_HScroll.Value); i++)
            {
                float x = (TickSpacing * .5f) + (i * TickSpacing);
                if (x > Timeline.Width)
                {
                    break;
                }
                int val = (i + Timline_HScroll.Value);
                tick_str = val.ToString();
                SizeF size = e.Graphics.MeasureString(tick_str, _timeline_font);
                e.Graphics.DrawLine(Pens.Black, x, 3, x, 6);
                e.Graphics.DrawString(tick_str, _timeline_font, Brushes.Black, x - (size.Width * .5f), 10);
            }
        }

        private void Timeline_Paint(object sender, PaintEventArgs e)
        {
            if ((TickSpacing * _Ticks) >= Timeline.Width)
            {
                Timline_HScroll.Visible = true;
            }
            else
            {
                Timline_HScroll.Visible = false;
            }

            bool odd_even = true;
            foreach (Control control in TrackPanel.Controls)
            {
                int y = control.Location.Y;
                if (odd_even)
                {
                    e.Graphics.FillRectangle(AlternateColor1, 0, y, Timeline.Width, TickHeight );
                }
                else
                {
                    e.Graphics.FillRectangle(AlternateColor2, 0, y, Timeline.Width, TickHeight );
                }
                odd_even = !odd_even;
            }

            for (int i = Timline_HScroll.Value; i < _Ticks; i++)
            {
                int line = (int)(((i - Timline_HScroll.Value)+1) * TickSpacing);
                if (line > Timeline.Width)
                {
                    break;
                }
                e.Graphics.DrawLine(LineColor, line, 0, line, Timeline.Height);

                foreach (EventTimeline event_timeline in _timeline_tracks.Values)
                {

                    if (event_timeline.Timeline.ContainsKey(i))
                    {
                        RectangleF rect = event_timeline.Timeline[i];
                        rect.Y = event_timeline.TimelineHeader.Location.Y;

                        float x = (i - Timline_HScroll.Value) * TickSpacing;
                        //e.Graphics.DrawRectangle(Pens.Black, x, rect.Y, rect.Width, rect.Height);
                        //e.Graphics.DrawImage(_node_image, rect);
                        e.Graphics.DrawImage(_node_image, x, rect.Y, rect.Width, rect.Height);
                        string tick_str = i.ToString();
                        SizeF size = e.Graphics.MeasureString(tick_str, _timeline_font);
                        e.Graphics.DrawString(tick_str, _timeline_font, Brushes.Black, x + ((TickSpacing * .5f) - (size.Width * .5f)), rect.Y + 10);
                    }
                }
            }
            if (_highlight_rect != Rectangle.Empty)
            {
                e.Graphics.DrawRectangle(Pens.Black, _highlight_rect);
            }
            if (_node_position != Node.Empty)
            {
                e.Graphics.DrawRectangle(Pens.Red, _node_position.position.X,
                    _node_position.position.Y, _node_position.position.Width, _node_position.position.Height);
            }


        }



        #endregion


        #region IVSG_ControlStateOutboundUpdate Members

        public void Update(Control control, object object_data)
        {
            TrackStateInfo t = (TrackStateInfo)object_data;
            if (_selected_trackname != t.TrackName)
            {
                ChangeSelectedTrackName(t.TrackName);
            }

        }

        #endregion

        private void AddTrack_Click(object sender, EventArgs e)
        {
            string track_name = string.Empty;
            bool result = false;
            try
            {
                Console.WriteLine("Checking content pane");
                if (ContentPane is ITimelinePanel)
                {
                    Console.WriteLine("Content pane is timeline");
                    result = ((ITimelinePanel)ContentPane).BeforeAddTimelineTrack(out track_name);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("{0}, ContentPane is null", HeaderText);
            }

            Console.WriteLine("Here is result {0}", result);
            if (result)
            {
                AddTimelineTrack(track_name);
            }

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    ((ITimelinePanel)ContentPane).AfterAddTimelineTrack(track_name);
                }
            }
            catch (Exception)
            {
            }


        }

        private void DeleteTrack_Click(object sender, EventArgs e)
        {
            bool result = false;

            try
            {
                if (ContentPane is ITimelinePanel)
                {
                    result = ((ITimelinePanel)ContentPane).BeforeRemoveTimelineTrack(_selected_trackname);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("{0}, ContentPane is null", HeaderText);
            }

            Console.WriteLine("Before RemoveTimelineTrack is result {0}", result);
            if (result)
            {
                RemoveSelectedTimelineTrack();
            }

        }

        private void MoveTrackUp_Click(object sender, EventArgs e)
        {
            MoveSelectedTrackUp();
        }

        private void MoveTrackDown_Click(object sender, EventArgs e)
        {
            MoveSelectedTrackDown();
        }

        private void TimelineProperties_Click(object sender, EventArgs e)
        {
            _dlg_properties.Ticks = _Ticks;
            if (_dlg_properties.ShowDialog(this) == DialogResult.OK)
            {
               SetTicks( _dlg_properties.Ticks);
            }
        }

        private void toolStrip1_VisibleChanged(object sender, EventArgs e)
        {
        }

    }
}
