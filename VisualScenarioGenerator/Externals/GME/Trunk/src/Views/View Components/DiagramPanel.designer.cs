namespace AME.Views.View_Components
{
    partial class DiagramPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramPanel));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.selectLinkToolStrip = new System.Windows.Forms.ToolStrip();
            this.selection = new System.Windows.Forms.ToolStripButton();
            this.linking = new System.Windows.Forms.ToolStripButton();
            this.diagramGridToolStrip = new System.Windows.Forms.ToolStrip();
            this.diagramView = new System.Windows.Forms.ToolStripButton();
            this.gridView = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labelCoord = new System.Windows.Forms.ToolStripStatusLabel();
            this.filterToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.zoomToolStrip = new System.Windows.Forms.ToolStrip();
            this.zoomLabel = new System.Windows.Forms.ToolStripLabel();
            this.zoomComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.dropDownToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.selectLinkToolStrip.SuspendLayout();
            this.diagramGridToolStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.filterToolStrip.SuspendLayout();
            this.zoomToolStrip.SuspendLayout();
            this.dropDownToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(556, 437);
            this.tabControl1.TabIndex = 0;
            // 
            // selectLinkToolStrip
            // 
            this.selectLinkToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.selectLinkToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.selectLinkToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selection,
            this.linking});
            this.selectLinkToolStrip.Location = new System.Drawing.Point(8, 50);
            this.selectLinkToolStrip.Name = "selectLinkToolStrip";
            this.selectLinkToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.selectLinkToolStrip.Size = new System.Drawing.Size(49, 25);
            this.selectLinkToolStrip.TabIndex = 1;
            this.selectLinkToolStrip.Text = "toolStrip1";
            // 
            // selection
            // 
            this.selection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selection.Image = ((System.Drawing.Image)(resources.GetObject("selection.Image")));
            this.selection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selection.Name = "selection";
            this.selection.Size = new System.Drawing.Size(23, 22);
            this.selection.Text = "Select+Move";
            // 
            // linking
            // 
            this.linking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.linking.Image = ((System.Drawing.Image)(resources.GetObject("linking.Image")));
            this.linking.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.linking.Name = "linking";
            this.linking.Size = new System.Drawing.Size(23, 22);
            this.linking.Text = "Create Links";
            // 
            // diagramGridToolStrip
            // 
            this.diagramGridToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.diagramGridToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.diagramGridToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagramView,
            this.gridView});
            this.diagramGridToolStrip.Location = new System.Drawing.Point(70, 50);
            this.diagramGridToolStrip.Name = "diagramGridToolStrip";
            this.diagramGridToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.diagramGridToolStrip.Size = new System.Drawing.Size(49, 25);
            this.diagramGridToolStrip.TabIndex = 2;
            this.diagramGridToolStrip.Text = "toolStrip2";
            // 
            // diagramView
            // 
            this.diagramView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diagramView.Image = ((System.Drawing.Image)(resources.GetObject("diagramView.Image")));
            this.diagramView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diagramView.Name = "diagramView";
            this.diagramView.RightToLeftAutoMirrorImage = true;
            this.diagramView.Size = new System.Drawing.Size(23, 22);
            this.diagramView.Text = "Diagram View";
            // 
            // gridView
            // 
            this.gridView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.gridView.Image = ((System.Drawing.Image)(resources.GetObject("gridView.Image")));
            this.gridView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gridView.Name = "gridView";
            this.gridView.Size = new System.Drawing.Size(23, 22);
            this.gridView.Text = "Grid View";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(556, 437);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(556, 534);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.AccessibleName = "2";
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.selectLinkToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.diagramGridToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.filterToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.zoomToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.dropDownToolStrip);
            this.toolStripContainer1.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelCoord});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(556, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            // 
            // labelCoord
            // 
            this.labelCoord.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.labelCoord.Name = "labelCoord";
            this.labelCoord.Size = new System.Drawing.Size(50, 17);
            this.labelCoord.Text = "X =  Y = ";
            // 
            // filterToolStrip
            // 
            this.filterToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.filterToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.filterToolStrip.Location = new System.Drawing.Point(102, 25);
            this.filterToolStrip.Name = "filterToolStrip";
            this.filterToolStrip.Size = new System.Drawing.Size(48, 25);
            this.filterToolStrip.TabIndex = 3;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel1.Text = "Filter: ";
            // 
            // zoomToolStrip
            // 
            this.zoomToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.zoomToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomLabel,
            this.zoomComboBox});
            this.zoomToolStrip.Location = new System.Drawing.Point(3, 0);
            this.zoomToolStrip.Name = "zoomToolStrip";
            this.zoomToolStrip.Size = new System.Drawing.Size(213, 25);
            this.zoomToolStrip.TabIndex = 4;
            // 
            // zoomLabel
            // 
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(47, 22);
            this.zoomLabel.Text = "Zoom %";
            // 
            // zoomComboBox
            // 
            this.zoomComboBox.Items.AddRange(new object[] {
            "25",
            "50",
            "75",
            "100",
            "125",
            "150",
            "200",
            "400",
            "500"});
            this.zoomComboBox.Name = "zoomComboBox";
            this.zoomComboBox.Size = new System.Drawing.Size(121, 25);
            this.zoomComboBox.Text = "100";
            this.zoomComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomComboBox_SelectedIndexChanged);
            this.zoomComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.zoomComboBox_KeyDown);
            // 
            // dropDownToolStrip
            // 
            this.dropDownToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.dropDownToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.dropDownToolStrip.Location = new System.Drawing.Point(111, 75);
            this.dropDownToolStrip.Name = "dropDownToolStrip";
            this.dropDownToolStrip.Size = new System.Drawing.Size(128, 25);
            this.dropDownToolStrip.TabIndex = 5;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(87, 22);
            this.toolStripDropDownButton1.Text = "Add Resource";
            // 
            // DiagramPanel
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "DiagramPanel";
            this.Size = new System.Drawing.Size(556, 534);
            this.selectLinkToolStrip.ResumeLayout(false);
            this.selectLinkToolStrip.PerformLayout();
            this.diagramGridToolStrip.ResumeLayout(false);
            this.diagramGridToolStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.filterToolStrip.ResumeLayout(false);
            this.filterToolStrip.PerformLayout();
            this.zoomToolStrip.ResumeLayout(false);
            this.zoomToolStrip.PerformLayout();
            this.dropDownToolStrip.ResumeLayout(false);
            this.dropDownToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStrip selectLinkToolStrip;
        private System.Windows.Forms.ToolStripButton selection;
        private System.Windows.Forms.ToolStripButton linking;
        private System.Windows.Forms.ToolStripButton gridView;
        private System.Windows.Forms.ToolStripButton diagramView;
        private System.Windows.Forms.ToolStrip diagramGridToolStrip;
        internal System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labelCoord;
        private System.Windows.Forms.ToolStrip filterToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStrip zoomToolStrip;
        private System.Windows.Forms.ToolStripComboBox zoomComboBox;
        private System.Windows.Forms.ToolStripLabel zoomLabel;
        private System.Windows.Forms.ToolStrip dropDownToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;

    }
}
