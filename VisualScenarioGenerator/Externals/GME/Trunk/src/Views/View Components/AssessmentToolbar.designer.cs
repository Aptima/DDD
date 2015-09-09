namespace GME.Views.View_Components {
    partial class AssessmentToolbar {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssessmentToolbar));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.simmulationComboBox = new GME.Views.View_Components.CustomComboToolStripItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.displayTableDataButton = new System.Windows.Forms.ToolStripButton();
            this.graphDataButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tablePageButton = new System.Windows.Forms.ToolStripButton();
            this.thumbnailPageButton = new System.Windows.Forms.ToolStripButton();
            this.bigGraphPageButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simmulationComboBox,
            this.toolStripSeparator1,
            this.displayTableDataButton,
            this.graphDataButton,
            this.toolStripSeparator2,
            this.tablePageButton,
            this.thumbnailPageButton,
            this.bigGraphPageButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(342, 29);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // simmulationComboBox
            // 
            this.simmulationComboBox.Controller = null;
            this.simmulationComboBox.DisplayID = -1;
            this.simmulationComboBox.LinkType = null;
            this.simmulationComboBox.Name = "simmulationComboBox";
            this.simmulationComboBox.SelectedID = -1;
            this.simmulationComboBox.Size = new System.Drawing.Size(200, 26);
            this.simmulationComboBox.Type = "";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // displayTableDataButton
            // 
            this.displayTableDataButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.displayTableDataButton.Image = ((System.Drawing.Image)(resources.GetObject("displayTableDataButton.Image")));
            this.displayTableDataButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.displayTableDataButton.Name = "displayTableDataButton";
            this.displayTableDataButton.Size = new System.Drawing.Size(23, 26);
            this.displayTableDataButton.Text = "displayTableDataButton";
            this.displayTableDataButton.ToolTipText = "Raw Data";
            this.displayTableDataButton.Click += new System.EventHandler(this.displayTableDataButton_Click);
            // 
            // graphDataButton
            // 
            this.graphDataButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphDataButton.Image = ((System.Drawing.Image)(resources.GetObject("graphDataButton.Image")));
            this.graphDataButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphDataButton.Name = "graphDataButton";
            this.graphDataButton.Size = new System.Drawing.Size(23, 26);
            this.graphDataButton.Text = "graphDataButton";
            this.graphDataButton.ToolTipText = "Graph Raw Data";
            this.graphDataButton.Click += new System.EventHandler(this.graphDataButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // tablePageButton
            // 
            this.tablePageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tablePageButton.Image = ((System.Drawing.Image)(resources.GetObject("tablePageButton.Image")));
            this.tablePageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tablePageButton.Name = "tablePageButton";
            this.tablePageButton.Size = new System.Drawing.Size(23, 26);
            this.tablePageButton.Text = "tablePageButton";
            this.tablePageButton.ToolTipText = "View Raw Data";
            this.tablePageButton.Click += new System.EventHandler(this.tablePageButton_Click);
            // 
            // thumbnailPageButton
            // 
            this.thumbnailPageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.thumbnailPageButton.Image = ((System.Drawing.Image)(resources.GetObject("thumbnailPageButton.Image")));
            this.thumbnailPageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.thumbnailPageButton.Name = "thumbnailPageButton";
            this.thumbnailPageButton.Size = new System.Drawing.Size(23, 26);
            this.thumbnailPageButton.Text = "thumbnailPageButton";
            this.thumbnailPageButton.ToolTipText = "View Thumbnail Graphs";
            this.thumbnailPageButton.Click += new System.EventHandler(this.thumbnailPageButton_Click);
            // 
            // bigGraphPageButton
            // 
            this.bigGraphPageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bigGraphPageButton.Image = ((System.Drawing.Image)(resources.GetObject("bigGraphPageButton.Image")));
            this.bigGraphPageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bigGraphPageButton.Name = "bigGraphPageButton";
            this.bigGraphPageButton.Size = new System.Drawing.Size(23, 20);
            this.bigGraphPageButton.Text = "bigGraphPageButton";
            this.bigGraphPageButton.ToolTipText = "View Large Graph";
            this.bigGraphPageButton.Click += new System.EventHandler(this.bigGraphPageButton_Click);
            // 
            // AssessmentToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Name = "AssessmentToolbar";
            this.Size = new System.Drawing.Size(342, 29);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private CustomComboToolStripItem simmulationComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton displayTableDataButton;
        private System.Windows.Forms.ToolStripButton graphDataButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tablePageButton;
        private System.Windows.Forms.ToolStripButton thumbnailPageButton;
        private System.Windows.Forms.ToolStripButton bigGraphPageButton;
    }
}
