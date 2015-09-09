namespace GME.Views.View_Components {
    partial class AssessmentResultsPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssessmentResultsPanel));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.resultsTabControl = new System.Windows.Forms.TabControl();
            this.rawDataTab = new System.Windows.Forms.TabPage();
            this.runRawData = new System.Windows.Forms.DataGridView();
            this.multiGraphTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.expandedGraphTab = new System.Windows.Forms.TabPage();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.graphDataButton = new System.Windows.Forms.ToolStripButton();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwardButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.resultsTabControl.SuspendLayout();
            this.rawDataTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runRawData)).BeginInit();
            this.multiGraphTab.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            this.toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.resultsTabControl);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(658, 414);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size(658, 439);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            //this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // resultsTabControl
            // 
            this.resultsTabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.resultsTabControl.Controls.Add(this.rawDataTab);
            this.resultsTabControl.Controls.Add(this.multiGraphTab);
            this.resultsTabControl.Controls.Add(this.expandedGraphTab);
            this.resultsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsTabControl.ItemSize = new System.Drawing.Size(0, 1);
            this.resultsTabControl.Location = new System.Drawing.Point(0, 0);
            this.resultsTabControl.Name = "resultsTabControl";
            this.resultsTabControl.SelectedIndex = 0;
            this.resultsTabControl.Size = new System.Drawing.Size(658, 414);
            this.resultsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.resultsTabControl.TabIndex = 0;
            this.resultsTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // rawDataTab
            // 
            this.rawDataTab.Controls.Add(this.runRawData);
            this.rawDataTab.Location = new System.Drawing.Point(4, 5);
            this.rawDataTab.Name = "rawDataTab";
            this.rawDataTab.Padding = new System.Windows.Forms.Padding(3);
            this.rawDataTab.Size = new System.Drawing.Size(650, 405);
            this.rawDataTab.TabIndex = 0;
            this.rawDataTab.Text = "tabPage1";
            this.rawDataTab.UseVisualStyleBackColor = true;
            // 
            // runRawData
            // 
            this.runRawData.AllowUserToAddRows = false;
            this.runRawData.AllowUserToDeleteRows = false;
            this.runRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.runRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runRawData.Location = new System.Drawing.Point(3, 3);
            this.runRawData.Name = "runRawData";
            this.runRawData.ReadOnly = true;
            this.runRawData.Size = new System.Drawing.Size(644, 399);
            this.runRawData.TabIndex = 0;
            // 
            // multiGraphTab
            // 
            this.multiGraphTab.Controls.Add(this.flowLayoutPanel);
            this.multiGraphTab.Location = new System.Drawing.Point(4, 5);
            this.multiGraphTab.Name = "multiGraphTab";
            this.multiGraphTab.Padding = new System.Windows.Forms.Padding(3);
            this.multiGraphTab.Size = new System.Drawing.Size(650, 405);
            this.multiGraphTab.TabIndex = 1;
            this.multiGraphTab.Text = "tabPage2";
            this.multiGraphTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(644, 399);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // expandedGraphTab
            // 
            this.expandedGraphTab.Location = new System.Drawing.Point(4, 5);
            this.expandedGraphTab.Name = "expandedGraphTab";
            this.expandedGraphTab.Padding = new System.Windows.Forms.Padding(3);
            this.expandedGraphTab.Size = new System.Drawing.Size(650, 405);
            this.expandedGraphTab.TabIndex = 2;
            this.expandedGraphTab.Text = "tabPage3";
            this.expandedGraphTab.UseVisualStyleBackColor = true;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphDataButton,
            this.backButton,
            this.forwardButton});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(103, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // graphDataButton
            // 
            this.graphDataButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphDataButton.Image = ((System.Drawing.Image)(resources.GetObject("graphDataButton.Image")));
            this.graphDataButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphDataButton.Name = "graphDataButton";
            this.graphDataButton.Size = new System.Drawing.Size(23, 22);
            this.graphDataButton.Text = "Graph Data";
            this.graphDataButton.Click += new System.EventHandler(this.graphDataButton_Click);
            // 
            // backButton
            // 
            this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(23, 22);
            this.backButton.Text = "Back";
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.forwardButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardButton.Image")));
            this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(23, 22);
            this.forwardButton.Text = "Forward";
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // AssessmentResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer);
            this.Name = "AssessmentResultsPanel";
            this.Size = new System.Drawing.Size(658, 439);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.resultsTabControl.ResumeLayout(false);
            this.rawDataTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runRawData)).EndInit();
            this.multiGraphTab.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton graphDataButton;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwardButton;
        private System.Windows.Forms.TabControl resultsTabControl;
        private System.Windows.Forms.TabPage rawDataTab;
        private System.Windows.Forms.TabPage multiGraphTab;
        private System.Windows.Forms.TabPage expandedGraphTab;
        private System.Windows.Forms.DataGridView runRawData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
    }
}
