namespace BatchRunner
{
    partial class BatchRunForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.runNameTextBox = new System.Windows.Forms.TextBox();
            this.scenarioNameTextBox = new System.Windows.Forms.TextBox();
            this.durationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.browseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.runDataTagTextBox = new System.Windows.Forms.TextBox();
            this.externalSetupCommandTextBox = new System.Windows.Forms.TextBox();
            this.externalSetupArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.externalTeardownCommandTextBox = new System.Windows.Forms.TextBox();
            this.externalTeardownArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.runLogDirectoryPathTextBox = new System.Windows.Forms.TextBox();
            this.runLogDirectoryPathBrowseButton = new System.Windows.Forms.Button();
            this.externalSetupWorkingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.externalSetupWorkingDirectoryBrowseButton = new System.Windows.Forms.Button();
            this.externalSetupDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.externalTeardownDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.externalTeardownWorkingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.externalTeardownWorkingDirectoryBrowseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalSetupDelayNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalTeardownDelayNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(454, 373);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(373, 373);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Run Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Scenario File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Run Duration (s):";
            // 
            // runNameTextBox
            // 
            this.runNameTextBox.Location = new System.Drawing.Point(106, 10);
            this.runNameTextBox.Name = "runNameTextBox";
            this.runNameTextBox.Size = new System.Drawing.Size(323, 20);
            this.runNameTextBox.TabIndex = 5;
            // 
            // scenarioNameTextBox
            // 
            this.scenarioNameTextBox.Location = new System.Drawing.Point(106, 38);
            this.scenarioNameTextBox.Name = "scenarioNameTextBox";
            this.scenarioNameTextBox.ReadOnly = true;
            this.scenarioNameTextBox.Size = new System.Drawing.Size(323, 20);
            this.scenarioNameTextBox.TabIndex = 6;
            // 
            // durationNumericUpDown
            // 
            this.durationNumericUpDown.Location = new System.Drawing.Point(106, 64);
            this.durationNumericUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.durationNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.durationNumericUpDown.Name = "durationNumericUpDown";
            this.durationNumericUpDown.Size = new System.Drawing.Size(125, 20);
            this.durationNumericUpDown.TabIndex = 7;
            this.durationNumericUpDown.Value = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(435, 36);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.externalTeardownWorkingDirectoryBrowseButton);
            this.groupBox1.Controls.Add(this.externalTeardownWorkingDirectoryTextBox);
            this.groupBox1.Controls.Add(this.externalTeardownDelayNumericUpDown);
            this.groupBox1.Controls.Add(this.externalSetupDelayNumericUpDown);
            this.groupBox1.Controls.Add(this.externalSetupWorkingDirectoryBrowseButton);
            this.groupBox1.Controls.Add(this.externalSetupWorkingDirectoryTextBox);
            this.groupBox1.Controls.Add(this.runLogDirectoryPathBrowseButton);
            this.groupBox1.Controls.Add(this.runLogDirectoryPathTextBox);
            this.groupBox1.Controls.Add(this.externalTeardownArgumentsTextBox);
            this.groupBox1.Controls.Add(this.externalTeardownCommandTextBox);
            this.groupBox1.Controls.Add(this.externalSetupArgumentsTextBox);
            this.groupBox1.Controls.Add(this.externalSetupCommandTextBox);
            this.groupBox1.Controls.Add(this.runDataTagTextBox);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 259);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced Options:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "RunLogDirectoryPath";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "RunDataTag";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "ExternalSetupCommand";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "ExternalSetupArguments";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(155, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "ExternalSetupWorkingDirectory";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "ExternalSetupDelay";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "ExternalTeardownCommand";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 184);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(143, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "ExternalTeardownArguments";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(175, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "ExternalTeardownWorkingDirectory";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 232);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "ExternalTeardownDelay";
            // 
            // runDataTagTextBox
            // 
            this.runDataTagTextBox.Location = new System.Drawing.Point(200, 37);
            this.runDataTagTextBox.Name = "runDataTagTextBox";
            this.runDataTagTextBox.Size = new System.Drawing.Size(302, 20);
            this.runDataTagTextBox.TabIndex = 10;
            // 
            // externalSetupCommandTextBox
            // 
            this.externalSetupCommandTextBox.Location = new System.Drawing.Point(200, 61);
            this.externalSetupCommandTextBox.Name = "externalSetupCommandTextBox";
            this.externalSetupCommandTextBox.Size = new System.Drawing.Size(301, 20);
            this.externalSetupCommandTextBox.TabIndex = 15;
            // 
            // externalSetupArgumentsTextBox
            // 
            this.externalSetupArgumentsTextBox.Location = new System.Drawing.Point(200, 85);
            this.externalSetupArgumentsTextBox.Name = "externalSetupArgumentsTextBox";
            this.externalSetupArgumentsTextBox.Size = new System.Drawing.Size(302, 20);
            this.externalSetupArgumentsTextBox.TabIndex = 16;
            // 
            // externalTeardownCommandTextBox
            // 
            this.externalTeardownCommandTextBox.Location = new System.Drawing.Point(200, 157);
            this.externalTeardownCommandTextBox.Name = "externalTeardownCommandTextBox";
            this.externalTeardownCommandTextBox.Size = new System.Drawing.Size(301, 20);
            this.externalTeardownCommandTextBox.TabIndex = 17;
            // 
            // externalTeardownArgumentsTextBox
            // 
            this.externalTeardownArgumentsTextBox.Location = new System.Drawing.Point(200, 181);
            this.externalTeardownArgumentsTextBox.Name = "externalTeardownArgumentsTextBox";
            this.externalTeardownArgumentsTextBox.Size = new System.Drawing.Size(301, 20);
            this.externalTeardownArgumentsTextBox.TabIndex = 18;
            // 
            // runLogDirectoryPathTextBox
            // 
            this.runLogDirectoryPathTextBox.Location = new System.Drawing.Point(200, 13);
            this.runLogDirectoryPathTextBox.Name = "runLogDirectoryPathTextBox";
            this.runLogDirectoryPathTextBox.Size = new System.Drawing.Size(221, 20);
            this.runLogDirectoryPathTextBox.TabIndex = 10;
            // 
            // runLogDirectoryPathBrowseButton
            // 
            this.runLogDirectoryPathBrowseButton.Location = new System.Drawing.Point(427, 11);
            this.runLogDirectoryPathBrowseButton.Name = "runLogDirectoryPathBrowseButton";
            this.runLogDirectoryPathBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.runLogDirectoryPathBrowseButton.TabIndex = 10;
            this.runLogDirectoryPathBrowseButton.Text = "Browse";
            this.runLogDirectoryPathBrowseButton.UseVisualStyleBackColor = true;
            this.runLogDirectoryPathBrowseButton.Click += new System.EventHandler(this.runLogDirectoryPathBrowseButton_Click);
            // 
            // externalSetupWorkingDirectoryTextBox
            // 
            this.externalSetupWorkingDirectoryTextBox.Location = new System.Drawing.Point(200, 109);
            this.externalSetupWorkingDirectoryTextBox.Name = "externalSetupWorkingDirectoryTextBox";
            this.externalSetupWorkingDirectoryTextBox.Size = new System.Drawing.Size(221, 20);
            this.externalSetupWorkingDirectoryTextBox.TabIndex = 19;
            // 
            // externalSetupWorkingDirectoryBrowseButton
            // 
            this.externalSetupWorkingDirectoryBrowseButton.Location = new System.Drawing.Point(427, 107);
            this.externalSetupWorkingDirectoryBrowseButton.Name = "externalSetupWorkingDirectoryBrowseButton";
            this.externalSetupWorkingDirectoryBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.externalSetupWorkingDirectoryBrowseButton.TabIndex = 20;
            this.externalSetupWorkingDirectoryBrowseButton.Text = "Browse";
            this.externalSetupWorkingDirectoryBrowseButton.UseVisualStyleBackColor = true;
            this.externalSetupWorkingDirectoryBrowseButton.Click += new System.EventHandler(this.externalSetupWorkingDirectoryBrowseButton_Click);
            // 
            // externalSetupDelayNumericUpDown
            // 
            this.externalSetupDelayNumericUpDown.Location = new System.Drawing.Point(200, 134);
            this.externalSetupDelayNumericUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.externalSetupDelayNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.externalSetupDelayNumericUpDown.Name = "externalSetupDelayNumericUpDown";
            this.externalSetupDelayNumericUpDown.Size = new System.Drawing.Size(125, 20);
            this.externalSetupDelayNumericUpDown.TabIndex = 10;
            this.externalSetupDelayNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // externalTeardownDelayNumericUpDown
            // 
            this.externalTeardownDelayNumericUpDown.Location = new System.Drawing.Point(200, 230);
            this.externalTeardownDelayNumericUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.externalTeardownDelayNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.externalTeardownDelayNumericUpDown.Name = "externalTeardownDelayNumericUpDown";
            this.externalTeardownDelayNumericUpDown.Size = new System.Drawing.Size(125, 20);
            this.externalTeardownDelayNumericUpDown.TabIndex = 21;
            this.externalTeardownDelayNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // externalTeardownWorkingDirectoryTextBox
            // 
            this.externalTeardownWorkingDirectoryTextBox.Location = new System.Drawing.Point(200, 205);
            this.externalTeardownWorkingDirectoryTextBox.Name = "externalTeardownWorkingDirectoryTextBox";
            this.externalTeardownWorkingDirectoryTextBox.Size = new System.Drawing.Size(221, 20);
            this.externalTeardownWorkingDirectoryTextBox.TabIndex = 22;
            // 
            // externalTeardownWorkingDirectoryBrowseButton
            // 
            this.externalTeardownWorkingDirectoryBrowseButton.Location = new System.Drawing.Point(427, 203);
            this.externalTeardownWorkingDirectoryBrowseButton.Name = "externalTeardownWorkingDirectoryBrowseButton";
            this.externalTeardownWorkingDirectoryBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.externalTeardownWorkingDirectoryBrowseButton.TabIndex = 23;
            this.externalTeardownWorkingDirectoryBrowseButton.Text = "Browse";
            this.externalTeardownWorkingDirectoryBrowseButton.UseVisualStyleBackColor = true;
            this.externalTeardownWorkingDirectoryBrowseButton.Click += new System.EventHandler(this.externalTeardownWorkingDirectoryBrowseButton_Click);
            // 
            // BatchRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 397);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.durationNumericUpDown);
            this.Controls.Add(this.scenarioNameTextBox);
            this.Controls.Add(this.runNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "BatchRunForm";
            this.Text = "Edit Run";
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalSetupDelayNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalTeardownDelayNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox runNameTextBox;
        private System.Windows.Forms.TextBox scenarioNameTextBox;
        private System.Windows.Forms.NumericUpDown durationNumericUpDown;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button externalTeardownWorkingDirectoryBrowseButton;
        private System.Windows.Forms.TextBox externalTeardownWorkingDirectoryTextBox;
        private System.Windows.Forms.NumericUpDown externalTeardownDelayNumericUpDown;
        private System.Windows.Forms.NumericUpDown externalSetupDelayNumericUpDown;
        private System.Windows.Forms.Button externalSetupWorkingDirectoryBrowseButton;
        private System.Windows.Forms.TextBox externalSetupWorkingDirectoryTextBox;
        private System.Windows.Forms.Button runLogDirectoryPathBrowseButton;
        private System.Windows.Forms.TextBox runLogDirectoryPathTextBox;
        private System.Windows.Forms.TextBox externalTeardownArgumentsTextBox;
        private System.Windows.Forms.TextBox externalTeardownCommandTextBox;
        private System.Windows.Forms.TextBox externalSetupArgumentsTextBox;
        private System.Windows.Forms.TextBox externalSetupCommandTextBox;
        private System.Windows.Forms.TextBox runDataTagTextBox;
    }
}