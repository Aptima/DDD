namespace AME.Views.View_Components {
    partial class AssessmentToolStrip {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssessmentToolStrip));
            this.processComboBox = new AME.Views.View_Components.CustomComboToolStripItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.graphButton = new System.Windows.Forms.ToolStripButton();
            this.processButton = new System.Windows.Forms.ToolStripButton();
            this.applyResultsButton  = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // processComboBox
            // 
            this.processComboBox.Controller = null;
            this.processComboBox.DisplayID = -1;
            this.processComboBox.LinkType = null;
            this.processComboBox.Name = "processComboBox";
            this.processComboBox.SelectedID = -1;
            this.processComboBox.Size = new System.Drawing.Size(200, 22);
            this.processComboBox.Type = "";
            this.processComboBox.SelectedIDChangedEvent += new CustomComboToolStripItem.SelectedIDChanged(processComboBox_SelectedIDChangedEvent);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // graphButton
            // 
            this.graphButton.Image = ((System.Drawing.Image)(resources.GetObject("graphDataButton.Image")));
			this.graphButton.ToolTipText = "Graph Results";
            this.graphButton.Name = "graphButton";
            this.graphButton.Size = new System.Drawing.Size(23, 22);
            //this.graphButton.Click += new System.EventHandler(this.graphButton_Click);
			this.graphButton.Click += new System.EventHandler(this.graphMeasuresButton_Click);
			// 
            // processButton
            // 
            this.processButton.Image = ((System.Drawing.Image)(resources.GetObject("simulationButton.Image")));
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(23, 22);
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // applyResultsButton
            // 
            this.applyResultsButton.Image = ((System.Drawing.Image)(resources.GetObject("applyResultsButton.Image")));
			this.applyResultsButton.ToolTipText = "Apply Results";
			this.applyResultsButton.Name = "applyResultsButton";
            this.applyResultsButton.Size = new System.Drawing.Size(23, 22);
            this.applyResultsButton.Click += new System.EventHandler(this.applyResultsButton_Click);

            // 
            // AssessmentToolStrip
            // 
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.Location = new System.Drawing.Point(3, 0);
            this.Name = "assessmentToolStrip";
            this.Size = new System.Drawing.Size(264, 25);
            this.ResumeLayout(false);

        }


        #endregion

        private AME.Views.View_Components.CustomComboToolStripItem processComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton graphButton;
        private System.Windows.Forms.ToolStripButton processButton;
        private System.Windows.Forms.ToolStripButton applyResultsButton;
    }
}
