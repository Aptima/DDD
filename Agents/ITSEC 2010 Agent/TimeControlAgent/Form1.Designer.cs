namespace TimeControlAgent
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelDDDStatus = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxHostname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSetSpeed = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerStopTime = new System.Windows.Forms.DateTimePicker();
            this.textBoxStopSeconds = new System.Windows.Forms.TextBox();
            this.radioButtonTime = new System.Windows.Forms.RadioButton();
            this.radioButtonTicks = new System.Windows.Forms.RadioButton();
            this.comboBoxSpeedMultiplier = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDDDSimTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelCurrentEndTime = new System.Windows.Forms.Label();
            this.labelCurrentSpeed = new System.Windows.Forms.Label();
            this.buttonClearSpeed = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelDDDStatus);
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.textBoxHostname);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DDD Connection";
            // 
            // labelDDDStatus
            // 
            this.labelDDDStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDDDStatus.Location = new System.Drawing.Point(7, 77);
            this.labelDDDStatus.Name = "labelDDDStatus";
            this.labelDDDStatus.Size = new System.Drawing.Size(156, 23);
            this.labelDDDStatus.TabIndex = 5;
            this.labelDDDStatus.Text = "NOT CONNECTED";
            this.labelDDDStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(172, 77);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(74, 51);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(173, 20);
            this.textBoxPort.TabIndex = 3;
            // 
            // textBoxHostname
            // 
            this.textBoxHostname.Location = new System.Drawing.Point(74, 17);
            this.textBoxHostname.Name = "textBoxHostname";
            this.textBoxHostname.Size = new System.Drawing.Size(173, 20);
            this.textBoxHostname.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hostname: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSetSpeed);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dateTimePickerStopTime);
            this.groupBox2.Controls.Add(this.textBoxStopSeconds);
            this.groupBox2.Controls.Add(this.radioButtonTime);
            this.groupBox2.Controls.Add(this.radioButtonTicks);
            this.groupBox2.Controls.Add(this.comboBoxSpeedMultiplier);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(13, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 134);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time Controls";
            // 
            // buttonSetSpeed
            // 
            this.buttonSetSpeed.Location = new System.Drawing.Point(171, 105);
            this.buttonSetSpeed.Name = "buttonSetSpeed";
            this.buttonSetSpeed.Size = new System.Drawing.Size(75, 23);
            this.buttonSetSpeed.TabIndex = 8;
            this.buttonSetSpeed.Text = "Set Speed";
            this.buttonSetSpeed.UseVisualStyleBackColor = true;
            this.buttonSetSpeed.Click += new System.EventHandler(this.buttonSetSpeed_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(168, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Seconds";
            // 
            // dateTimePickerStopTime
            // 
            this.dateTimePickerStopTime.CustomFormat = "HH:mm:ss";
            this.dateTimePickerStopTime.Enabled = false;
            this.dateTimePickerStopTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStopTime.Location = new System.Drawing.Point(35, 76);
            this.dateTimePickerStopTime.Name = "dateTimePickerStopTime";
            this.dateTimePickerStopTime.Size = new System.Drawing.Size(127, 20);
            this.dateTimePickerStopTime.TabIndex = 5;
            this.dateTimePickerStopTime.Value = new System.DateTime(2010, 10, 29, 0, 0, 0, 0);
            // 
            // textBoxStopSeconds
            // 
            this.textBoxStopSeconds.Location = new System.Drawing.Point(35, 50);
            this.textBoxStopSeconds.Name = "textBoxStopSeconds";
            this.textBoxStopSeconds.Size = new System.Drawing.Size(127, 20);
            this.textBoxStopSeconds.TabIndex = 4;
            // 
            // radioButtonTime
            // 
            this.radioButtonTime.AutoSize = true;
            this.radioButtonTime.Location = new System.Drawing.Point(15, 82);
            this.radioButtonTime.Name = "radioButtonTime";
            this.radioButtonTime.Size = new System.Drawing.Size(14, 13);
            this.radioButtonTime.TabIndex = 3;
            this.radioButtonTime.UseVisualStyleBackColor = true;
            this.radioButtonTime.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioButtonTicks
            // 
            this.radioButtonTicks.AutoSize = true;
            this.radioButtonTicks.Checked = true;
            this.radioButtonTicks.Location = new System.Drawing.Point(15, 53);
            this.radioButtonTicks.Name = "radioButtonTicks";
            this.radioButtonTicks.Size = new System.Drawing.Size(14, 13);
            this.radioButtonTicks.TabIndex = 2;
            this.radioButtonTicks.TabStop = true;
            this.radioButtonTicks.UseVisualStyleBackColor = true;
            this.radioButtonTicks.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // comboBoxSpeedMultiplier
            // 
            this.comboBoxSpeedMultiplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpeedMultiplier.FormattingEnabled = true;
            this.comboBoxSpeedMultiplier.Items.AddRange(new object[] {
            "5x",
            "10x",
            "20x",
            "50x",
            "100x"});
            this.comboBoxSpeedMultiplier.Location = new System.Drawing.Point(125, 17);
            this.comboBoxSpeedMultiplier.Name = "comboBoxSpeedMultiplier";
            this.comboBoxSpeedMultiplier.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSpeedMultiplier.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Speed Multiplier:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelDDDSimTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 272);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(427, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(140, 17);
            this.toolStripStatusLabel1.Text = "Current Simulation Time:";
            // 
            // toolStripStatusLabelDDDSimTime
            // 
            this.toolStripStatusLabelDDDSimTime.Name = "toolStripStatusLabelDDDSimTime";
            this.toolStripStatusLabelDDDSimTime.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabelDDDSimTime.Text = "00:00:00";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelCurrentEndTime);
            this.groupBox3.Controls.Add(this.labelCurrentSpeed);
            this.groupBox3.Controls.Add(this.buttonClearSpeed);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(275, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(140, 134);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Current Speed Info";
            // 
            // labelCurrentEndTime
            // 
            this.labelCurrentEndTime.AutoSize = true;
            this.labelCurrentEndTime.Location = new System.Drawing.Point(55, 76);
            this.labelCurrentEndTime.Name = "labelCurrentEndTime";
            this.labelCurrentEndTime.Size = new System.Drawing.Size(13, 13);
            this.labelCurrentEndTime.TabIndex = 4;
            this.labelCurrentEndTime.Text = "--";
            // 
            // labelCurrentSpeed
            // 
            this.labelCurrentSpeed.AutoSize = true;
            this.labelCurrentSpeed.Location = new System.Drawing.Point(55, 37);
            this.labelCurrentSpeed.Name = "labelCurrentSpeed";
            this.labelCurrentSpeed.Size = new System.Drawing.Size(18, 13);
            this.labelCurrentSpeed.TabIndex = 3;
            this.labelCurrentSpeed.Text = "1x";
            // 
            // buttonClearSpeed
            // 
            this.buttonClearSpeed.Location = new System.Drawing.Point(59, 105);
            this.buttonClearSpeed.Name = "buttonClearSpeed";
            this.buttonClearSpeed.Size = new System.Drawing.Size(75, 23);
            this.buttonClearSpeed.TabIndex = 2;
            this.buttonClearSpeed.Text = "Clear";
            this.buttonClearSpeed.UseVisualStyleBackColor = true;
            this.buttonClearSpeed.Visible = false;
            this.buttonClearSpeed.Click += new System.EventHandler(this.buttonClearSpeed_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Until:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Speed:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 294);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ITSEC 2010 DDD Agent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelDDDStatus;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxHostname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonTicks;
        private System.Windows.Forms.ComboBox comboBoxSpeedMultiplier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerStopTime;
        private System.Windows.Forms.TextBox textBoxStopSeconds;
        private System.Windows.Forms.RadioButton radioButtonTime;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDDDSimTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSetSpeed;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelCurrentEndTime;
        private System.Windows.Forms.Label labelCurrentSpeed;
        private System.Windows.Forms.Button buttonClearSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}

