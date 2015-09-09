namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Score
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInitialValue = new System.Windows.Forms.TextBox();
            this.clbDMApplied = new System.Windows.Forms.CheckedListBox();
            this.clbDMViewers = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bbSelectedRules = new VisualScenarioGenerator.Dialogs.Ctl_BalanceBoxes();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(397, 26);
            this.label4.TabIndex = 30;
            this.label4.Text = "Score";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Score Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(92, 36);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 32;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(241, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(322, 403);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Initial Value:";
            // 
            // txtInitialValue
            // 
            this.txtInitialValue.Location = new System.Drawing.Point(92, 70);
            this.txtInitialValue.Name = "txtInitialValue";
            this.txtInitialValue.Size = new System.Drawing.Size(100, 20);
            this.txtInitialValue.TabIndex = 38;
            // 
            // clbDMApplied
            // 
            this.clbDMApplied.FormattingEnabled = true;
            this.clbDMApplied.Location = new System.Drawing.Point(9, 264);
            this.clbDMApplied.Name = "clbDMApplied";
            this.clbDMApplied.Size = new System.Drawing.Size(160, 109);
            this.clbDMApplied.TabIndex = 39;
            // 
            // clbDMViewers
            // 
            this.clbDMViewers.FormattingEnabled = true;
            this.clbDMViewers.Location = new System.Drawing.Point(202, 264);
            this.clbDMViewers.Name = "clbDMViewers";
            this.clbDMViewers.Size = new System.Drawing.Size(158, 109);
            this.clbDMViewers.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "Calculate score for DMs:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(202, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Show score on DM displays:";
            // 
            // bbSelectedRules
            // 
            this.bbSelectedRules.Location = new System.Drawing.Point(4, 90);
            this.bbSelectedRules.Name = "bbSelectedRules";
            this.bbSelectedRules.Size = new System.Drawing.Size(391, 150);
            this.bbSelectedRules.TabIndex = 50;
            // 
            // Ctl_Score
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bbSelectedRules);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.clbDMViewers);
            this.Controls.Add(this.clbDMApplied);
            this.Controls.Add(this.txtInitialValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "Ctl_Score";
            this.Size = new System.Drawing.Size(397, 429);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInitialValue;
        private System.Windows.Forms.CheckedListBox clbDMApplied;
        private System.Windows.Forms.CheckedListBox clbDMViewers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Ctl_BalanceBoxes bbSelectedRules;
    }
}
