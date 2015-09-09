namespace VisualScenarioGenerator.Dialogs
{
    partial class TimelinePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimelinePanel));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.EventMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Timeline = new System.Windows.Forms.Panel();
            this.TimelineMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Timline_HScroll = new System.Windows.Forms.HScrollBar();
            this.TimelineScale = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddTrack = new System.Windows.Forms.ToolStripButton();
            this.DeleteTrack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MoveTrackUp = new System.Windows.Forms.ToolStripButton();
            this.MoveTrackDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TimelineProperties = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.EventMenuStrip.SuspendLayout();
            this.Timeline.SuspendLayout();
            this.TimelineMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.ContextMenuStrip = this.EventMenuStrip;
            this.splitContainer1.Panel1.Controls.Add(this.TrackPanel);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Timeline);
            this.splitContainer1.Panel2.Controls.Add(this.TimelineScale);
            this.splitContainer1.Size = new System.Drawing.Size(414, 107);
            this.splitContainer1.SplitterDistance = 138;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            // 
            // EventMenuStrip
            // 
            this.EventMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEventToolStripMenuItem,
            this.deleteEventToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.EventMenuStrip.Name = "EventMenuStrip";
            this.EventMenuStrip.Size = new System.Drawing.Size(143, 92);
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.addEventToolStripMenuItem.Text = "Add Event";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addTimelineTrackStripMenuItem_Click);
            // 
            // deleteEventToolStripMenuItem
            // 
            this.deleteEventToolStripMenuItem.Name = "deleteEventToolStripMenuItem";
            this.deleteEventToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.deleteEventToolStripMenuItem.Text = "Delete Event";
            this.deleteEventToolStripMenuItem.Click += new System.EventHandler(this.deleteTimelineTrackStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // TrackPanel
            // 
            this.TrackPanel.AutoScroll = true;
            this.TrackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackPanel.Location = new System.Drawing.Point(0, 24);
            this.TrackPanel.Name = "TrackPanel";
            this.TrackPanel.Size = new System.Drawing.Size(138, 66);
            this.TrackPanel.TabIndex = 1;
            this.TrackPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TrackPanel_Scroll);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(138, 17);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Timeline
            // 
            this.Timeline.AutoScroll = true;
            this.Timeline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Timeline.ContextMenuStrip = this.TimelineMenuStrip;
            this.Timeline.Controls.Add(this.Timline_HScroll);
            this.Timeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Timeline.Location = new System.Drawing.Point(0, 24);
            this.Timeline.Name = "Timeline";
            this.Timeline.Size = new System.Drawing.Size(274, 83);
            this.Timeline.TabIndex = 1;
            this.Timeline.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Timeline_MouseDoubleClick);
            this.Timeline.Resize += new System.EventHandler(this.Timeline_Resize);
            this.Timeline.Paint += new System.Windows.Forms.PaintEventHandler(this.Timeline_Paint);
            this.Timeline.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Timeline_MouseUp);
            // 
            // TimelineMenuStrip
            // 
            this.TimelineMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.deleteNodeToolStripMenuItem,
            this.nextNodeToolStripMenuItem,
            this.previousNodeToolStripMenuItem});
            this.TimelineMenuStrip.Name = "TimelineMenuStrip";
            this.TimelineMenuStrip.Size = new System.Drawing.Size(153, 92);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // deleteNodeToolStripMenuItem
            // 
            this.deleteNodeToolStripMenuItem.Name = "deleteNodeToolStripMenuItem";
            this.deleteNodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteNodeToolStripMenuItem.Text = "Delete Node";
            this.deleteNodeToolStripMenuItem.Click += new System.EventHandler(this.deleteNodeToolStripMenuItem_Click);
            // 
            // nextNodeToolStripMenuItem
            // 
            this.nextNodeToolStripMenuItem.Name = "nextNodeToolStripMenuItem";
            this.nextNodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nextNodeToolStripMenuItem.Text = "Next Node";
            this.nextNodeToolStripMenuItem.Click += new System.EventHandler(this.nextNodeToolStripMenuItem_Click);
            // 
            // previousNodeToolStripMenuItem
            // 
            this.previousNodeToolStripMenuItem.Name = "previousNodeToolStripMenuItem";
            this.previousNodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.previousNodeToolStripMenuItem.Text = "Previous Node";
            this.previousNodeToolStripMenuItem.Click += new System.EventHandler(this.previousNodeToolStripMenuItem_Click);
            // 
            // Timline_HScroll
            // 
            this.Timline_HScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Timline_HScroll.Location = new System.Drawing.Point(0, 64);
            this.Timline_HScroll.Name = "Timline_HScroll";
            this.Timline_HScroll.Size = new System.Drawing.Size(272, 17);
            this.Timline_HScroll.TabIndex = 0;
            this.Timline_HScroll.ValueChanged += new System.EventHandler(this.Timeline_HScroll_ValueChanged);
            // 
            // TimelineScale
            // 
            this.TimelineScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TimelineScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.TimelineScale.Location = new System.Drawing.Point(0, 0);
            this.TimelineScale.Name = "TimelineScale";
            this.TimelineScale.Size = new System.Drawing.Size(274, 24);
            this.TimelineScale.TabIndex = 0;
            this.TimelineScale.Paint += new System.Windows.Forms.PaintEventHandler(this.TimelineScale_Paint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddTrack,
            this.DeleteTrack,
            this.toolStripSeparator1,
            this.MoveTrackUp,
            this.MoveTrackDown,
            this.toolStripSeparator2,
            this.TimelineProperties,
            this.toolStripSeparator3,
            this.toolStripSplitButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(414, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.VisibleChanged += new System.EventHandler(this.toolStrip1_VisibleChanged);
            // 
            // AddTrack
            // 
            this.AddTrack.BackColor = System.Drawing.SystemColors.Control;
            this.AddTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddTrack.Image = ((System.Drawing.Image)(resources.GetObject("AddTrack.Image")));
            this.AddTrack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddTrack.Name = "AddTrack";
            this.AddTrack.Size = new System.Drawing.Size(23, 22);
            this.AddTrack.Text = "+";
            this.AddTrack.Click += new System.EventHandler(this.AddTrack_Click);
            // 
            // DeleteTrack
            // 
            this.DeleteTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DeleteTrack.Image = ((System.Drawing.Image)(resources.GetObject("DeleteTrack.Image")));
            this.DeleteTrack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteTrack.Name = "DeleteTrack";
            this.DeleteTrack.Size = new System.Drawing.Size(23, 22);
            this.DeleteTrack.Text = "-";
            this.DeleteTrack.Click += new System.EventHandler(this.DeleteTrack_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MoveTrackUp
            // 
            this.MoveTrackUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MoveTrackUp.Image = ((System.Drawing.Image)(resources.GetObject("MoveTrackUp.Image")));
            this.MoveTrackUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveTrackUp.Name = "MoveTrackUp";
            this.MoveTrackUp.Size = new System.Drawing.Size(53, 22);
            this.MoveTrackUp.Text = "Move Up";
            this.MoveTrackUp.Click += new System.EventHandler(this.MoveTrackUp_Click);
            // 
            // MoveTrackDown
            // 
            this.MoveTrackDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MoveTrackDown.Image = ((System.Drawing.Image)(resources.GetObject("MoveTrackDown.Image")));
            this.MoveTrackDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveTrackDown.Name = "MoveTrackDown";
            this.MoveTrackDown.Size = new System.Drawing.Size(69, 22);
            this.MoveTrackDown.Text = "Move Down";
            this.MoveTrackDown.Click += new System.EventHandler(this.MoveTrackDown_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TimelineProperties
            // 
            this.TimelineProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TimelineProperties.Image = global::VisualScenarioGenerator.Properties.Resources.otheroptions;
            this.TimelineProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TimelineProperties.Name = "TimelineProperties";
            this.TimelineProperties.Size = new System.Drawing.Size(23, 22);
            this.TimelineProperties.Text = "toolStripButton5";
            this.TimelineProperties.Click += new System.EventHandler(this.TimelineProperties_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.toolStripSplitButton1.Enabled = false;
            this.toolStripSplitButton1.Image = global::VisualScenarioGenerator.Properties.Resources.search;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "100%";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "75%";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "50%";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem5.Text = "25%";
            // 
            // TimelinePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TimelinePanel";
            this.Size = new System.Drawing.Size(414, 132);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.EventMenuStrip.ResumeLayout(false);
            this.Timeline.ResumeLayout(false);
            this.TimelineMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel TimelineScale;
        private System.Windows.Forms.ContextMenuStrip EventMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.Panel Timeline;
        private System.Windows.Forms.HScrollBar Timline_HScroll;
        private System.Windows.Forms.ContextMenuStrip TimelineMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.Panel TrackPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem nextNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddTrack;
        private System.Windows.Forms.ToolStripButton DeleteTrack;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton MoveTrackUp;
        private System.Windows.Forms.ToolStripButton MoveTrackDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton TimelineProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
    }
}
