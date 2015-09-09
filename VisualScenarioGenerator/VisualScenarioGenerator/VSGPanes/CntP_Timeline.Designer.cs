namespace VisualScenarioGenerator.VSGPanes
{
    partial class CntP_Timeline
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
            this.timelinePanel1 = new VisualScenarioGenerator.Dialogs.TimelinePanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.agT_CanvasControl1 = new AGT.Forms.AGT_CanvasControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timelinePanel1
            // 
            this.timelinePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timelinePanel1.Location = new System.Drawing.Point(0, 0);
            this.timelinePanel1.Name = "timelinePanel1";
            this.timelinePanel1.Size = new System.Drawing.Size(563, 157);
            this.timelinePanel1.TabIndex = 1;
            this.timelinePanel1.Ticks = 7600;
            this.timelinePanel1.Load += new System.EventHandler(this.timelinePanel1_Load);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.agT_CanvasControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.timelinePanel1);
            this.splitContainer1.Size = new System.Drawing.Size(567, 386);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.TabIndex = 2;
            // 
            // agT_CanvasControl1
            // 
            this.agT_CanvasControl1.BackColor = System.Drawing.Color.Maroon;
            this.agT_CanvasControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agT_CanvasControl1.Location = new System.Drawing.Point(0, 0);
            this.agT_CanvasControl1.Name = "agT_CanvasControl1";
            this.agT_CanvasControl1.Size = new System.Drawing.Size(563, 217);
            this.agT_CanvasControl1.TabIndex = 0;
            // 
            // CntP_Timeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CntP_Timeline";
            this.Size = new System.Drawing.Size(567, 386);
            this.Load += new System.EventHandler(this.CntP_Timeline_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VisualScenarioGenerator.Dialogs.TimelinePanel timelinePanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private AGT.Forms.AGT_CanvasControl agT_CanvasControl1;
    }
}
