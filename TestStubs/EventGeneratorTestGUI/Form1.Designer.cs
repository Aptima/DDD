namespace Aptima.Asim.DDD.TestStubs.EventGeneratorTestGUI
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
            this.components = new System.ComponentModel.Container();
            this.simulationModelButton = new System.Windows.Forms.Button();
            this.simulationModelTextBox = new System.Windows.Forms.TextBox();
            this.hostnameTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.connectTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.networkGroupBox = new System.Windows.Forms.GroupBox();
            this.clockGroupBox = new System.Windows.Forms.GroupBox();
            this.clockTextBox = new System.Windows.Forms.TextBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.simulationModelGroupBox = new System.Windows.Forms.GroupBox();
            this.simModelBrowseButton = new System.Windows.Forms.Button();
            this.networkCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.eventsGroupBox = new System.Windows.Forms.GroupBox();
            this.eventsListBox = new System.Windows.Forms.ListBox();
            this.saveEventsButton = new System.Windows.Forms.Button();
            this.loadEventsButton = new System.Windows.Forms.Button();
            this.sendEventButton = new System.Windows.Forms.Button();
            this.manualCheckBox = new System.Windows.Forms.CheckBox();
            this.networkGroupBox.SuspendLayout();
            this.clockGroupBox.SuspendLayout();
            this.simulationModelGroupBox.SuspendLayout();
            this.eventsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // simulationModelButton
            // 
            this.simulationModelButton.Location = new System.Drawing.Point(6, 15);
            this.simulationModelButton.Name = "simulationModelButton";
            this.simulationModelButton.Size = new System.Drawing.Size(56, 23);
            this.simulationModelButton.TabIndex = 0;
            this.simulationModelButton.Text = "Set";
            this.simulationModelButton.UseVisualStyleBackColor = true;
            this.simulationModelButton.Click += new System.EventHandler(this.simulationModelButton_Click);
            // 
            // simulationModelTextBox
            // 
            this.simulationModelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simulationModelTextBox.Location = new System.Drawing.Point(68, 17);
            this.simulationModelTextBox.Name = "simulationModelTextBox";
            this.simulationModelTextBox.Size = new System.Drawing.Size(230, 20);
            this.simulationModelTextBox.TabIndex = 1;
            // 
            // hostnameTextBox
            // 
            this.hostnameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hostnameTextBox.Location = new System.Drawing.Point(110, 10);
            this.hostnameTextBox.Name = "hostnameTextBox";
            this.hostnameTextBox.Size = new System.Drawing.Size(109, 20);
            this.hostnameTextBox.TabIndex = 2;
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Location = new System.Drawing.Point(110, 37);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(109, 20);
            this.portTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Hostname:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port:";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(5, 66);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(99, 23);
            this.connectButton.TabIndex = 6;
            this.connectButton.Text = "Connect...";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // connectTextBox
            // 
            this.connectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.connectTextBox.Location = new System.Drawing.Point(111, 68);
            this.connectTextBox.Name = "connectTextBox";
            this.connectTextBox.ReadOnly = true;
            this.connectTextBox.Size = new System.Drawing.Size(108, 20);
            this.connectTextBox.TabIndex = 7;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(6, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(54, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // networkGroupBox
            // 
            this.networkGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.networkGroupBox.Controls.Add(this.label1);
            this.networkGroupBox.Controls.Add(this.hostnameTextBox);
            this.networkGroupBox.Controls.Add(this.connectTextBox);
            this.networkGroupBox.Controls.Add(this.portTextBox);
            this.networkGroupBox.Controls.Add(this.connectButton);
            this.networkGroupBox.Controls.Add(this.label2);
            this.networkGroupBox.Location = new System.Drawing.Point(2, 65);
            this.networkGroupBox.Name = "networkGroupBox";
            this.networkGroupBox.Size = new System.Drawing.Size(224, 97);
            this.networkGroupBox.TabIndex = 9;
            this.networkGroupBox.TabStop = false;
            this.networkGroupBox.Text = "Network";
            // 
            // clockGroupBox
            // 
            this.clockGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clockGroupBox.Controls.Add(this.clockTextBox);
            this.clockGroupBox.Controls.Add(this.resetButton);
            this.clockGroupBox.Controls.Add(this.startButton);
            this.clockGroupBox.Location = new System.Drawing.Point(233, 65);
            this.clockGroupBox.Name = "clockGroupBox";
            this.clockGroupBox.Size = new System.Drawing.Size(134, 97);
            this.clockGroupBox.TabIndex = 10;
            this.clockGroupBox.TabStop = false;
            this.clockGroupBox.Text = "Clock";
            // 
            // clockTextBox
            // 
            this.clockTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clockTextBox.Location = new System.Drawing.Point(6, 45);
            this.clockTextBox.Name = "clockTextBox";
            this.clockTextBox.Size = new System.Drawing.Size(114, 44);
            this.clockTextBox.TabIndex = 10;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(66, 19);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(54, 23);
            this.resetButton.TabIndex = 9;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // clockTimer
            // 
            this.clockTimer.Tick += new System.EventHandler(this.clockTimer_Tick);
            // 
            // simulationModelGroupBox
            // 
            this.simulationModelGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simulationModelGroupBox.Controls.Add(this.simModelBrowseButton);
            this.simulationModelGroupBox.Controls.Add(this.simulationModelButton);
            this.simulationModelGroupBox.Controls.Add(this.simulationModelTextBox);
            this.simulationModelGroupBox.Location = new System.Drawing.Point(2, 3);
            this.simulationModelGroupBox.Name = "simulationModelGroupBox";
            this.simulationModelGroupBox.Size = new System.Drawing.Size(365, 44);
            this.simulationModelGroupBox.TabIndex = 11;
            this.simulationModelGroupBox.TabStop = false;
            this.simulationModelGroupBox.Text = "Simulation Model";
            // 
            // simModelBrowseButton
            // 
            this.simModelBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simModelBrowseButton.Location = new System.Drawing.Point(304, 14);
            this.simModelBrowseButton.Name = "simModelBrowseButton";
            this.simModelBrowseButton.Size = new System.Drawing.Size(54, 23);
            this.simModelBrowseButton.TabIndex = 2;
            this.simModelBrowseButton.Text = "Browse";
            this.simModelBrowseButton.UseVisualStyleBackColor = true;
            this.simModelBrowseButton.Click += new System.EventHandler(this.simModelBrowseButton_Click);
            // 
            // networkCheckTimer
            // 
            this.networkCheckTimer.Tick += new System.EventHandler(this.networkCheckTimer_Tick);
            // 
            // eventsGroupBox
            // 
            this.eventsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsGroupBox.Controls.Add(this.eventsListBox);
            this.eventsGroupBox.Controls.Add(this.saveEventsButton);
            this.eventsGroupBox.Controls.Add(this.loadEventsButton);
            this.eventsGroupBox.Location = new System.Drawing.Point(2, 168);
            this.eventsGroupBox.Name = "eventsGroupBox";
            this.eventsGroupBox.Size = new System.Drawing.Size(224, 272);
            this.eventsGroupBox.TabIndex = 12;
            this.eventsGroupBox.TabStop = false;
            this.eventsGroupBox.Text = "Events";
            // 
            // eventsListBox
            // 
            this.eventsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsListBox.FormattingEnabled = true;
            this.eventsListBox.Location = new System.Drawing.Point(7, 50);
            this.eventsListBox.Name = "eventsListBox";
            this.eventsListBox.Size = new System.Drawing.Size(212, 212);
            this.eventsListBox.TabIndex = 3;
            // 
            // saveEventsButton
            // 
            this.saveEventsButton.Location = new System.Drawing.Point(56, 20);
            this.saveEventsButton.Name = "saveEventsButton";
            this.saveEventsButton.Size = new System.Drawing.Size(48, 23);
            this.saveEventsButton.TabIndex = 1;
            this.saveEventsButton.Text = "Save";
            this.saveEventsButton.UseVisualStyleBackColor = true;
            this.saveEventsButton.Click += new System.EventHandler(this.saveEventsButton_Click);
            // 
            // loadEventsButton
            // 
            this.loadEventsButton.Location = new System.Drawing.Point(6, 20);
            this.loadEventsButton.Name = "loadEventsButton";
            this.loadEventsButton.Size = new System.Drawing.Size(44, 23);
            this.loadEventsButton.TabIndex = 0;
            this.loadEventsButton.Text = "Load";
            this.loadEventsButton.UseVisualStyleBackColor = true;
            this.loadEventsButton.Click += new System.EventHandler(this.loadEventsButton_Click);
            // 
            // sendEventButton
            // 
            this.sendEventButton.Location = new System.Drawing.Point(239, 201);
            this.sendEventButton.Name = "sendEventButton";
            this.sendEventButton.Size = new System.Drawing.Size(75, 24);
            this.sendEventButton.TabIndex = 4;
            this.sendEventButton.Text = "Send Now";
            this.sendEventButton.UseVisualStyleBackColor = true;
            this.sendEventButton.Click += new System.EventHandler(this.sendEventButton_Click);
            // 
            // manualCheckBox
            // 
            this.manualCheckBox.AutoSize = true;
            this.manualCheckBox.Location = new System.Drawing.Point(239, 178);
            this.manualCheckBox.Name = "manualCheckBox";
            this.manualCheckBox.Size = new System.Drawing.Size(97, 17);
            this.manualCheckBox.TabIndex = 13;
            this.manualCheckBox.Text = "Manual Control";
            this.manualCheckBox.UseVisualStyleBackColor = true;
            this.manualCheckBox.CheckedChanged += new System.EventHandler(this.manualCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 443);
            this.Controls.Add(this.manualCheckBox);
            this.Controls.Add(this.sendEventButton);
            this.Controls.Add(this.eventsGroupBox);
            this.Controls.Add(this.simulationModelGroupBox);
            this.Controls.Add(this.clockGroupBox);
            this.Controls.Add(this.networkGroupBox);
            this.Name = "Form1";
            this.Text = "Event Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.networkGroupBox.ResumeLayout(false);
            this.networkGroupBox.PerformLayout();
            this.clockGroupBox.ResumeLayout(false);
            this.clockGroupBox.PerformLayout();
            this.simulationModelGroupBox.ResumeLayout(false);
            this.simulationModelGroupBox.PerformLayout();
            this.eventsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button simulationModelButton;
        private System.Windows.Forms.TextBox simulationModelTextBox;
        private System.Windows.Forms.TextBox hostnameTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox connectTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox networkGroupBox;
        private System.Windows.Forms.GroupBox clockGroupBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.TextBox clockTextBox;
        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.GroupBox simulationModelGroupBox;
        private System.Windows.Forms.Button simModelBrowseButton;
        private System.Windows.Forms.Timer networkCheckTimer;
        private System.Windows.Forms.GroupBox eventsGroupBox;
        private System.Windows.Forms.Button loadEventsButton;
        private System.Windows.Forms.Button saveEventsButton;
        private System.Windows.Forms.ListBox eventsListBox;
        private System.Windows.Forms.Button sendEventButton;
        private System.Windows.Forms.CheckBox manualCheckBox;
    }
}

