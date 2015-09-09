namespace AME.Views.View_Components {

    partial class GraphSetupControl {

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
            this.typeToGraphCombo = new System.Windows.Forms.ComboBox();
            this.itemsToGraphListBox = new System.Windows.Forms.CheckedListBox();
            this.yAxisCombo = new System.Windows.Forms.ComboBox();
            this.typeToGraphLabel = new System.Windows.Forms.Label();
            this.itemsToGraphLabel = new System.Windows.Forms.Label();
            this.yAxisLabel = new System.Windows.Forms.Label();
            this.simRunAxisCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.graphNameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.xAxisCombo = new System.Windows.Forms.ComboBox();
            this.xAxisGroupBox = new System.Windows.Forms.GroupBox();
            this.xAxisLabel = new System.Windows.Forms.Label();
            this.xAxisGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeToGraphCombo
            // 
            this.typeToGraphCombo.FormattingEnabled = true;
            this.typeToGraphCombo.Location = new System.Drawing.Point(14, 74);
            this.typeToGraphCombo.Name = "typeToGraphCombo";
            this.typeToGraphCombo.Size = new System.Drawing.Size(160, 21);
            this.typeToGraphCombo.TabIndex = 1;
            this.typeToGraphCombo.Tag = "";
            this.typeToGraphCombo.SelectedIndexChanged += new System.EventHandler(this.typeToGraphCombo_SelectedIndexChanged);
            // 
            // itemsToGraphListBox
            // 
            this.itemsToGraphListBox.Enabled = false;
            this.itemsToGraphListBox.FormattingEnabled = true;
            this.itemsToGraphListBox.Location = new System.Drawing.Point(194, 67);
            this.itemsToGraphListBox.Name = "itemsToGraphListBox";
            this.itemsToGraphListBox.Size = new System.Drawing.Size(161, 139);
            this.itemsToGraphListBox.TabIndex = 2;
            // 
            // yAxisCombo
            // 
            this.yAxisCombo.Enabled = false;
            this.yAxisCombo.FormattingEnabled = true;
            this.yAxisCombo.Location = new System.Drawing.Point(15, 167);
            this.yAxisCombo.Name = "yAxisCombo";
            this.yAxisCombo.Size = new System.Drawing.Size(160, 21);
            this.yAxisCombo.TabIndex = 4;
            // 
            // typeToGraphLabel
            // 
            this.typeToGraphLabel.AutoSize = true;
            this.typeToGraphLabel.Location = new System.Drawing.Point(12, 58);
            this.typeToGraphLabel.Name = "typeToGraphLabel";
            this.typeToGraphLabel.Size = new System.Drawing.Size(105, 13);
            this.typeToGraphLabel.TabIndex = 99;
            this.typeToGraphLabel.Text = "Item Type To Graph:";
            // 
            // itemsToGraphLabel
            // 
            this.itemsToGraphLabel.AutoSize = true;
            this.itemsToGraphLabel.Location = new System.Drawing.Point(191, 51);
            this.itemsToGraphLabel.Name = "itemsToGraphLabel";
            this.itemsToGraphLabel.Size = new System.Drawing.Size(83, 13);
            this.itemsToGraphLabel.TabIndex = 99;
            this.itemsToGraphLabel.Text = "Items To Graph:";
            // 
            // yAxisLabel
            // 
            this.yAxisLabel.AutoSize = true;
            this.yAxisLabel.Location = new System.Drawing.Point(12, 151);
            this.yAxisLabel.Name = "yAxisLabel";
            this.yAxisLabel.Size = new System.Drawing.Size(39, 13);
            this.yAxisLabel.TabIndex = 99;
            this.yAxisLabel.Text = "Y-Axis:";
            // 
            // simRunAxisCheckBox
            // 
            this.simRunAxisCheckBox.AutoSize = true;
            this.simRunAxisCheckBox.Enabled = false;
            this.simRunAxisCheckBox.Location = new System.Drawing.Point(18, 9);
            this.simRunAxisCheckBox.Name = "simRunAxisCheckBox";
            this.simRunAxisCheckBox.Size = new System.Drawing.Size(114, 17);
            this.simRunAxisCheckBox.TabIndex = 99;
            this.simRunAxisCheckBox.Text = "SimRuns as X-Axis";
            this.simRunAxisCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 99;
            this.label1.Text = "Graph Name:";
            // 
            // graphNameTextBox
            // 
            this.graphNameTextBox.Location = new System.Drawing.Point(14, 26);
            this.graphNameTextBox.Name = "graphNameTextBox";
            this.graphNameTextBox.Size = new System.Drawing.Size(341, 20);
            this.graphNameTextBox.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(118, 220);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(199, 220);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(280, 220);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 7;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // xAxisCombo
            // 
            this.xAxisCombo.Enabled = false;
            this.xAxisCombo.FormattingEnabled = true;
            this.xAxisCombo.Location = new System.Drawing.Point(15, 118);
            this.xAxisCombo.Name = "xAxisCombo";
            this.xAxisCombo.Size = new System.Drawing.Size(159, 21);
            this.xAxisCombo.TabIndex = 3;
            // 
            // xAxisGroupBox
            // 
            this.xAxisGroupBox.Controls.Add(this.simRunAxisCheckBox);
            this.xAxisGroupBox.Location = new System.Drawing.Point(108, 3);
            this.xAxisGroupBox.Name = "xAxisGroupBox";
            this.xAxisGroupBox.Size = new System.Drawing.Size(178, 19);
            this.xAxisGroupBox.TabIndex = 99;
            this.xAxisGroupBox.TabStop = false;
            this.xAxisGroupBox.Text = "X-Axis:";
            this.xAxisGroupBox.Visible = false;
            // 
            // xAxisLabel
            // 
            this.xAxisLabel.AutoSize = true;
            this.xAxisLabel.Location = new System.Drawing.Point(15, 102);
            this.xAxisLabel.Name = "xAxisLabel";
            this.xAxisLabel.Size = new System.Drawing.Size(39, 13);
            this.xAxisLabel.TabIndex = 99;
            this.xAxisLabel.Text = "X-Axis:";
            // 
            // GraphSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xAxisLabel);
            this.Controls.Add(this.xAxisCombo);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.graphNameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xAxisGroupBox);
            this.Controls.Add(this.yAxisLabel);
            this.Controls.Add(this.itemsToGraphLabel);
            this.Controls.Add(this.typeToGraphLabel);
            this.Controls.Add(this.yAxisCombo);
            this.Controls.Add(this.itemsToGraphListBox);
            this.Controls.Add(this.typeToGraphCombo);
            this.Name = "GraphSetupControl";
            this.Size = new System.Drawing.Size(365, 252);
            this.xAxisGroupBox.ResumeLayout(false);
            this.xAxisGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox typeToGraphCombo;
        private System.Windows.Forms.CheckedListBox itemsToGraphListBox;
        private System.Windows.Forms.ComboBox yAxisCombo;
        private System.Windows.Forms.Label typeToGraphLabel;
        private System.Windows.Forms.Label itemsToGraphLabel;
        private System.Windows.Forms.Label yAxisLabel;
        private System.Windows.Forms.CheckBox simRunAxisCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox graphNameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ComboBox xAxisCombo;
        private System.Windows.Forms.GroupBox xAxisGroupBox;
        private System.Windows.Forms.Label xAxisLabel;
    }
}
