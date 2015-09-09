namespace VSG.ViewComponentPanels
{
    partial class ScenarioViewComponentPanel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scenarioInfo1 = new VSG.ViewComponents.ScenarioInfo();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.iconLibrary1 = new VSG.ViewComponents.IconLibrary();
            this.mapPlayfield1 = new VSG.ViewComponents.MapPlayfield();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scenarioInfo1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1270, 380);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 0;
            // 
            // scenarioInfo1
            // 
            this.scenarioInfo1.Controller = null;
            this.scenarioInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioInfo1.Location = new System.Drawing.Point(0, 0);
            this.scenarioInfo1.Name = "scenarioInfo1";
            this.scenarioInfo1.ScenarioId = -1;
            this.scenarioInfo1.Size = new System.Drawing.Size(420, 380);
            this.scenarioInfo1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.iconLibrary1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mapPlayfield1);
            this.splitContainer2.Size = new System.Drawing.Size(846, 380);
            this.splitContainer2.SplitterDistance = 122;
            this.splitContainer2.TabIndex = 1;
            // 
            // iconLibrary1
            // 
            this.iconLibrary1.Controller = null;
            this.iconLibrary1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iconLibrary1.Location = new System.Drawing.Point(0, 0);
            this.iconLibrary1.Name = "iconLibrary1";
            this.iconLibrary1.PlayfieldId = -1;
            this.iconLibrary1.Size = new System.Drawing.Size(846, 122);
            this.iconLibrary1.TabIndex = 2;
            // 
            // mapPlayfield1
            // 
            this.mapPlayfield1.Controller = null;
            this.mapPlayfield1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPlayfield1.Location = new System.Drawing.Point(0, 0);
            this.mapPlayfield1.Name = "mapPlayfield1";
            this.mapPlayfield1.PlayfieldId = -1;
            this.mapPlayfield1.Size = new System.Drawing.Size(846, 254);
            this.mapPlayfield1.TabIndex = 3;
            // 
            // ScenarioViewComponentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScenarioViewComponentPanel";
            this.Size = new System.Drawing.Size(1270, 380);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private VSG.ViewComponents.ScenarioInfo scenarioInfo1;
        private VSG.ViewComponents.IconLibrary iconLibrary1;
        private VSG.ViewComponents.MapPlayfield mapPlayfield1;


    }
}
