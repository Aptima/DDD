namespace AME.Views.View_Components {
    partial class AssessemntResults {
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.rawDataPage = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.customGraphsPage = new System.Windows.Forms.TabPage();
            this.customGraphsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.rToRGraphsPage = new System.Windows.Forms.TabPage();
            this.rToRGraphsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.measureDataPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.measureWebBrowser = new System.Windows.Forms.WebBrowser();
            this.visualizationPage = new System.Windows.Forms.TabPage();
            this.ganttChart1 = new AME.Views.View_Components.GanttChart();
            this.measureGraphPage = new System.Windows.Forms.TabPage();
            this.measureGraphsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.reportCardPage = new System.Windows.Forms.TabPage();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.rawDataPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.customGraphsPage.SuspendLayout();
            this.rToRGraphsPage.SuspendLayout();
            this.measureDataPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.visualizationPage.SuspendLayout();
            this.measureGraphPage.SuspendLayout();
            this.reportCardPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.rawDataPage);
            this.tabControl.Controls.Add(this.customGraphsPage);
            this.tabControl.Controls.Add(this.rToRGraphsPage);
            this.tabControl.Controls.Add(this.measureDataPage);
            this.tabControl.Controls.Add(this.visualizationPage);
            this.tabControl.Controls.Add(this.measureGraphPage);
            this.tabControl.Controls.Add(this.reportCardPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(777, 569);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // rawDataPage
            // 
            this.rawDataPage.Controls.Add(this.dataGridView);
            this.rawDataPage.Location = new System.Drawing.Point(4, 22);
            this.rawDataPage.Name = "rawDataPage";
            this.rawDataPage.Padding = new System.Windows.Forms.Padding(3);
            this.rawDataPage.Size = new System.Drawing.Size(769, 543);
            this.rawDataPage.TabIndex = 0;
            this.rawDataPage.Text = "Raw Data";
            this.rawDataPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(763, 537);
            this.dataGridView.TabIndex = 0;
            // 
            // customGraphsPage
            // 
            this.customGraphsPage.Controls.Add(this.customGraphsFlowPanel);
            this.customGraphsPage.Location = new System.Drawing.Point(4, 22);
            this.customGraphsPage.Name = "customGraphsPage";
            this.customGraphsPage.Padding = new System.Windows.Forms.Padding(3);
            this.customGraphsPage.Size = new System.Drawing.Size(769, 543);
            this.customGraphsPage.TabIndex = 1;
            this.customGraphsPage.Text = "Custom Graphs";
            this.customGraphsPage.UseVisualStyleBackColor = true;
            // 
            // customGraphsFlowPanel
            // 
            this.customGraphsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGraphsFlowPanel.Location = new System.Drawing.Point(3, 3);
            this.customGraphsFlowPanel.Name = "customGraphsFlowPanel";
            this.customGraphsFlowPanel.Size = new System.Drawing.Size(763, 537);
            this.customGraphsFlowPanel.TabIndex = 0;
            // 
            // rToRGraphsPage
            // 
            this.rToRGraphsPage.Controls.Add(this.rToRGraphsFlowPanel);
            this.rToRGraphsPage.Location = new System.Drawing.Point(4, 22);
            this.rToRGraphsPage.Name = "rToRGraphsPage";
            this.rToRGraphsPage.Padding = new System.Windows.Forms.Padding(3);
            this.rToRGraphsPage.Size = new System.Drawing.Size(769, 543);
            this.rToRGraphsPage.TabIndex = 2;
            this.rToRGraphsPage.Text = "Run-To-Run Graphs";
            this.rToRGraphsPage.UseVisualStyleBackColor = true;
            // 
            // rToRGraphsFlowPanel
            // 
            this.rToRGraphsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rToRGraphsFlowPanel.Location = new System.Drawing.Point(3, 3);
            this.rToRGraphsFlowPanel.Name = "rToRGraphsFlowPanel";
            this.rToRGraphsFlowPanel.Size = new System.Drawing.Size(763, 537);
            this.rToRGraphsFlowPanel.TabIndex = 0;
            // 
            // measureDataPage
            // 
            this.measureDataPage.Controls.Add(this.panel1);
            this.measureDataPage.Location = new System.Drawing.Point(4, 22);
            this.measureDataPage.Name = "measureDataPage";
            this.measureDataPage.Padding = new System.Windows.Forms.Padding(3);
            this.measureDataPage.Size = new System.Drawing.Size(769, 543);
            this.measureDataPage.TabIndex = 3;
            this.measureDataPage.Text = "Measure Data";
            this.measureDataPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.measureWebBrowser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 537);
            this.panel1.TabIndex = 0;
            // 
            // measureWebBrowser
            // 
            this.measureWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measureWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.measureWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.measureWebBrowser.Name = "measureWebBrowser";
            this.measureWebBrowser.Size = new System.Drawing.Size(763, 537);
            this.measureWebBrowser.TabIndex = 0;
            // 
            // visualizationPage
            // 
            this.visualizationPage.AutoScroll = true;
            this.visualizationPage.Controls.Add(this.ganttChart1);
            this.visualizationPage.Location = new System.Drawing.Point(4, 22);
            this.visualizationPage.Name = "visualizationPage";
            this.visualizationPage.Padding = new System.Windows.Forms.Padding(3);
            this.visualizationPage.Size = new System.Drawing.Size(769, 543);
            this.visualizationPage.TabIndex = 4;
            this.visualizationPage.Text = "Visualization";
            this.visualizationPage.UseVisualStyleBackColor = true;
            // 
            // ganttChart1
            // 
            this.ganttChart1.Category = "category";
            this.ganttChart1.ComponentId = -1;
            this.ganttChart1.Controller = null;
            this.ganttChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ganttChart1.Location = new System.Drawing.Point(3, 3);
            this.ganttChart1.Name = "ganttChart1";
            this.ganttChart1.Parameter = "filename";
            this.ganttChart1.Size = new System.Drawing.Size(763, 537);
            this.ganttChart1.TabIndex = 2;
            // 
            // measureGraphPage
            // 
            this.measureGraphPage.Controls.Add(this.measureGraphsFlowPanel);
            this.measureGraphPage.Location = new System.Drawing.Point(4, 22);
            this.measureGraphPage.Name = "measureGraphPage";
            this.measureGraphPage.Padding = new System.Windows.Forms.Padding(3);
            this.measureGraphPage.Size = new System.Drawing.Size(769, 543);
            this.measureGraphPage.TabIndex = 5;
            this.measureGraphPage.Text = "Measure Graphs";
            this.measureGraphPage.UseVisualStyleBackColor = true;
            // 
            // measureGraphsFlowPanel
            // 
            this.measureGraphsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measureGraphsFlowPanel.Location = new System.Drawing.Point(3, 3);
            this.measureGraphsFlowPanel.Name = "measureGraphsFlowPanel";
            this.measureGraphsFlowPanel.Size = new System.Drawing.Size(763, 537);
            this.measureGraphsFlowPanel.TabIndex = 0;
            // 
            // reportCardPage
            // 
            this.reportCardPage.Controls.Add(this.panelLeft);
            this.reportCardPage.Location = new System.Drawing.Point(4, 22);
            this.reportCardPage.Name = "reportCardPage";
            this.reportCardPage.Padding = new System.Windows.Forms.Padding(3);
            this.reportCardPage.Size = new System.Drawing.Size(769, 543);
            this.reportCardPage.TabIndex = 6;
            this.reportCardPage.Text = "Report Card";
            this.reportCardPage.UseVisualStyleBackColor = true;
            // 
            // panelLeft
            // 
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(3, 3);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(532, 537);
            this.panelLeft.TabIndex = 2;
            // 
            // AssessemntResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "AssessemntResults";
            this.Size = new System.Drawing.Size(777, 569);
            this.tabControl.ResumeLayout(false);
            this.rawDataPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.customGraphsPage.ResumeLayout(false);
            this.rToRGraphsPage.ResumeLayout(false);
            this.measureDataPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.visualizationPage.ResumeLayout(false);
            this.measureGraphPage.ResumeLayout(false);
            this.reportCardPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage rawDataPage;
        private System.Windows.Forms.TabPage customGraphsPage;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.FlowLayoutPanel customGraphsFlowPanel;
        private System.Windows.Forms.TabPage rToRGraphsPage;
        private System.Windows.Forms.FlowLayoutPanel rToRGraphsFlowPanel;
        private System.Windows.Forms.TabPage measureDataPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser measureWebBrowser;
 		private System.Windows.Forms.TabPage measureGraphPage;
		private System.Windows.Forms.FlowLayoutPanel measureGraphsFlowPanel;
        private System.Windows.Forms.TabPage visualizationPage;
        private AME.Views.View_Components.GanttChart ganttChart1;
        private System.Windows.Forms.TabPage reportCardPage;
        private System.Windows.Forms.Panel panelLeft;
        //private Timeline timeline1;
    }
}
