namespace GME.Views.View_Components {
    partial class GraphSetupTabPage {
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
            this.activateCheckBox = new System.Windows.Forms.CheckBox();
            this.typeToGraphCombo = new System.Windows.Forms.ComboBox();
            this.itemsToGraphListBox = new System.Windows.Forms.CheckedListBox();
            this.xAxisCombo = new System.Windows.Forms.ComboBox();
            this.yAxisCombo = new System.Windows.Forms.ComboBox();
            this.typeToGraphLabel = new System.Windows.Forms.Label();
            this.itemsToGraphLabel = new System.Windows.Forms.Label();
            this.yAxisLabel = new System.Windows.Forms.Label();
            this.simRunAxisCheckBox = new System.Windows.Forms.CheckBox();
            this.xAxisGroupBox = new System.Windows.Forms.GroupBox();
            this.xAxisGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // activateCheckBox
            // 
            this.activateCheckBox.AutoSize = true;
            this.activateCheckBox.Location = new System.Drawing.Point(12, 12);
            this.activateCheckBox.Name = "activateCheckBox";
            this.activateCheckBox.Size = new System.Drawing.Size(92, 17);
            this.activateCheckBox.TabIndex = 0;
            this.activateCheckBox.Text = "Display Graph";
            this.activateCheckBox.UseVisualStyleBackColor = true;
            this.activateCheckBox.CheckedChanged += new System.EventHandler(this.activateCheckBox_CheckedChanged);
            // 
            // typeToGraphCombo
            // 
            this.typeToGraphCombo.FormattingEnabled = true;
            this.typeToGraphCombo.Location = new System.Drawing.Point(22, 60);
            this.typeToGraphCombo.Name = "typeToGraphCombo";
            this.typeToGraphCombo.Size = new System.Drawing.Size(139, 21);
            this.typeToGraphCombo.TabIndex = 1;
            this.typeToGraphCombo.Tag = "";
            this.typeToGraphCombo.SelectedIndexChanged += new System.EventHandler(this.typeToGraphCombo_SelectedIndexChanged);
            // 
            // itemsToGraphListBox
            // 
            this.itemsToGraphListBox.Enabled = false;
            this.itemsToGraphListBox.FormattingEnabled = true;
            this.itemsToGraphListBox.Location = new System.Drawing.Point(178, 60);
            this.itemsToGraphListBox.Name = "itemsToGraphListBox";
            this.itemsToGraphListBox.Size = new System.Drawing.Size(151, 139);
            this.itemsToGraphListBox.TabIndex = 2;
            // 
            // xAxisCombo
            // 
            this.xAxisCombo.Enabled = false;
            this.xAxisCombo.FormattingEnabled = true;
            this.xAxisCombo.Location = new System.Drawing.Point(10, 14);
            this.xAxisCombo.Name = "xAxisCombo";
            this.xAxisCombo.Size = new System.Drawing.Size(139, 21);
            this.xAxisCombo.TabIndex = 3;
            // 
            // yAxisCombo
            // 
            this.yAxisCombo.Enabled = false;
            this.yAxisCombo.FormattingEnabled = true;
            this.yAxisCombo.Location = new System.Drawing.Point(22, 179);
            this.yAxisCombo.Name = "yAxisCombo";
            this.yAxisCombo.Size = new System.Drawing.Size(139, 21);
            this.yAxisCombo.TabIndex = 4;
            // 
            // typeToGraphLabel
            // 
            this.typeToGraphLabel.AutoSize = true;
            this.typeToGraphLabel.Location = new System.Drawing.Point(19, 44);
            this.typeToGraphLabel.Name = "typeToGraphLabel";
            this.typeToGraphLabel.Size = new System.Drawing.Size(105, 13);
            this.typeToGraphLabel.TabIndex = 5;
            this.typeToGraphLabel.Text = "Item Type To Graph:";
            // 
            // itemsToGraphLabel
            // 
            this.itemsToGraphLabel.AutoSize = true;
            this.itemsToGraphLabel.Location = new System.Drawing.Point(186, 44);
            this.itemsToGraphLabel.Name = "itemsToGraphLabel";
            this.itemsToGraphLabel.Size = new System.Drawing.Size(83, 13);
            this.itemsToGraphLabel.TabIndex = 6;
            this.itemsToGraphLabel.Text = "Items To Graph:";
            // 
            // yAxisLabel
            // 
            this.yAxisLabel.AutoSize = true;
            this.yAxisLabel.Location = new System.Drawing.Point(19, 163);
            this.yAxisLabel.Name = "yAxisLabel";
            this.yAxisLabel.Size = new System.Drawing.Size(39, 13);
            this.yAxisLabel.TabIndex = 8;
            this.yAxisLabel.Text = "Y-Axis:";
            // 
            // simRunAxisCheckBox
            // 
            this.simRunAxisCheckBox.AutoSize = true;
            this.simRunAxisCheckBox.Enabled = false;
            this.simRunAxisCheckBox.Location = new System.Drawing.Point(10, 41);
            this.simRunAxisCheckBox.Name = "simRunAxisCheckBox";
            this.simRunAxisCheckBox.Size = new System.Drawing.Size(114, 17);
            this.simRunAxisCheckBox.TabIndex = 9;
            this.simRunAxisCheckBox.Text = "SimRuns as X-Axis";
            this.simRunAxisCheckBox.UseVisualStyleBackColor = true;
            // 
            // xAxisGroupBox
            // 
            this.xAxisGroupBox.Controls.Add(this.simRunAxisCheckBox);
            this.xAxisGroupBox.Controls.Add(this.xAxisCombo);
            this.xAxisGroupBox.Location = new System.Drawing.Point(12, 95);
            this.xAxisGroupBox.Name = "xAxisGroupBox";
            this.xAxisGroupBox.Size = new System.Drawing.Size(160, 65);
            this.xAxisGroupBox.TabIndex = 10;
            this.xAxisGroupBox.TabStop = false;
            this.xAxisGroupBox.Text = "X-Axis:";
            // 
            // GraphSetupTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xAxisGroupBox);
            this.Controls.Add(this.yAxisLabel);
            this.Controls.Add(this.itemsToGraphLabel);
            this.Controls.Add(this.typeToGraphLabel);
            this.Controls.Add(this.yAxisCombo);
            this.Controls.Add(this.itemsToGraphListBox);
            this.Controls.Add(this.typeToGraphCombo);
            this.Controls.Add(this.activateCheckBox);
            this.Name = "GraphSetupTabPage";
            this.Size = new System.Drawing.Size(342, 222);
            this.EnabledChanged += new System.EventHandler(this.this_OnEnabledChange);
            this.xAxisGroupBox.ResumeLayout(false);
            this.xAxisGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox activateCheckBox;
        private System.Windows.Forms.ComboBox typeToGraphCombo;
        private System.Windows.Forms.CheckedListBox itemsToGraphListBox;
        private System.Windows.Forms.ComboBox xAxisCombo;
        private System.Windows.Forms.ComboBox yAxisCombo;
        private System.Windows.Forms.Label typeToGraphLabel;
        private System.Windows.Forms.Label itemsToGraphLabel;
        private System.Windows.Forms.Label yAxisLabel;
        private System.Windows.Forms.CheckBox simRunAxisCheckBox;
        private System.Windows.Forms.GroupBox xAxisGroupBox;
    }
}
