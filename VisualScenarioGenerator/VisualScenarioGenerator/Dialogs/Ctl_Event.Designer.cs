namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Event
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbGlobalEventTypes = new System.Windows.Forms.ComboBox();
            this.txtEventName = new System.Windows.Forms.TextBox();
            this.cmbAssetEventTypes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbCompletionEvent = new System.Windows.Forms.ComboBox();
            this.radOnCompletion = new System.Windows.Forms.RadioButton();
            this.radReiterate = new System.Windows.Forms.RadioButton();
            this.radDefault = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbGlobalEventTypes);
            this.groupBox1.Controls.Add(this.txtEventName);
            this.groupBox1.Controls.Add(this.cmbAssetEventTypes);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Event";
            // 
            // cmbGlobalEventTypes
            // 
            this.cmbGlobalEventTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGlobalEventTypes.FormattingEnabled = true;
            this.cmbGlobalEventTypes.Items.AddRange(new object[] {
            "Open Chat Room",
            "Close Chat Room",
            "Flush"});
            this.cmbGlobalEventTypes.Location = new System.Drawing.Point(9, 77);
            this.cmbGlobalEventTypes.Name = "cmbGlobalEventTypes";
            this.cmbGlobalEventTypes.Size = new System.Drawing.Size(233, 21);
            this.cmbGlobalEventTypes.TabIndex = 5;
            // 
            // txtEventName
            // 
            this.txtEventName.Location = new System.Drawing.Point(9, 37);
            this.txtEventName.Name = "txtEventName";
            this.txtEventName.Size = new System.Drawing.Size(170, 20);
            this.txtEventName.TabIndex = 4;
            // 
            // cmbAssetEventTypes
            // 
            this.cmbAssetEventTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAssetEventTypes.FormattingEnabled = true;
            this.cmbAssetEventTypes.Items.AddRange(new object[] {
            "Move",
            "Launch",
            "State Change",
            "Reveal",
            "Transfer",
            "Change Engram",
            "Remove Engram"});
            this.cmbAssetEventTypes.Location = new System.Drawing.Point(9, 77);
            this.cmbAssetEventTypes.Name = "cmbAssetEventTypes";
            this.cmbAssetEventTypes.Size = new System.Drawing.Size(233, 21);
            this.cmbAssetEventTypes.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Event Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbCompletionEvent);
            this.groupBox2.Controls.Add(this.radOnCompletion);
            this.groupBox2.Controls.Add(this.radReiterate);
            this.groupBox2.Controls.Add(this.radDefault);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 133);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Event Options";
            // 
            // cmbCompletionEvent
            // 
            this.cmbCompletionEvent.FormattingEnabled = true;
            this.cmbCompletionEvent.Items.AddRange(new object[] {
            "List of created events"});
            this.cmbCompletionEvent.Location = new System.Drawing.Point(28, 97);
            this.cmbCompletionEvent.Name = "cmbCompletionEvent";
            this.cmbCompletionEvent.Size = new System.Drawing.Size(151, 21);
            this.cmbCompletionEvent.TabIndex = 3;
            // 
            // radOnCompletion
            // 
            this.radOnCompletion.AutoSize = true;
            this.radOnCompletion.Location = new System.Drawing.Point(9, 74);
            this.radOnCompletion.Name = "radOnCompletion";
            this.radOnCompletion.Size = new System.Drawing.Size(94, 17);
            this.radOnCompletion.TabIndex = 2;
            this.radOnCompletion.TabStop = true;
            this.radOnCompletion.Text = "On Completion";
            this.radOnCompletion.UseVisualStyleBackColor = true;
            // 
            // radReiterate
            // 
            this.radReiterate.AutoSize = true;
            this.radReiterate.Location = new System.Drawing.Point(9, 51);
            this.radReiterate.Name = "radReiterate";
            this.radReiterate.Size = new System.Drawing.Size(68, 17);
            this.radReiterate.TabIndex = 1;
            this.radReiterate.TabStop = true;
            this.radReiterate.Text = "Reiterate";
            this.radReiterate.UseVisualStyleBackColor = true;
            // 
            // radDefault
            // 
            this.radDefault.AutoSize = true;
            this.radDefault.Location = new System.Drawing.Point(9, 28);
            this.radDefault.Name = "radDefault";
            this.radDefault.Size = new System.Drawing.Size(59, 17);
            this.radDefault.TabIndex = 0;
            this.radDefault.TabStop = true;
            this.radDefault.Text = "Default";
            this.radDefault.UseVisualStyleBackColor = true;
            this.radDefault.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Ctl_Event
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Ctl_Event";
            this.Size = new System.Drawing.Size(257, 338);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAssetEventTypes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEventName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radReiterate;
        private System.Windows.Forms.RadioButton radDefault;
        private System.Windows.Forms.ComboBox cmbCompletionEvent;
        private System.Windows.Forms.RadioButton radOnCompletion;
        private System.Windows.Forms.ComboBox cmbGlobalEventTypes;
    }
}
