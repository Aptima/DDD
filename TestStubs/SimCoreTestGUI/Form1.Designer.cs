namespace Aptima.Asim.DDD.TestStubs.SimCoreTestGUI
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
            this.simulationModelTextBox = new System.Windows.Forms.TextBox();
            this.simulationModelButton = new System.Windows.Forms.Button();
            this.stateButton = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.serverPortTextBox = new System.Windows.Forms.TextBox();
            this.serverPortLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.hostnameLabel = new System.Windows.Forms.Label();
            this.hostnameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objectStatus = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.objectListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.connectionsListBox = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.schemaTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.scenarioStartButton = new System.Windows.Forms.Button();
            this.scenarioBrowseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.scenarioTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxReplayLog = new System.Windows.Forms.CheckBox();
            this.labelReplayLog = new System.Windows.Forms.Label();
            this.textBoxReplayLog = new System.Windows.Forms.TextBox();
            this.buttonReplayLog = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxLogDetail = new System.Windows.Forms.GroupBox();
            this.radioButtonLimited = new System.Windows.Forms.RadioButton();
            this.radioButtonDetailed = new System.Windows.Forms.RadioButton();
            this.pauseButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.replaySpeedLabel = new System.Windows.Forms.Label();
            this.speedRadioButton6 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton5 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton4 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton3 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton2 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton1 = new System.Windows.Forms.RadioButton();
            this.replayLoopCheckbox = new System.Windows.Forms.CheckBox();
            this.startReplayButton = new System.Windows.Forms.Button();
            this.replayBrowseButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.replayTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxLogDetail.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // simulationModelTextBox
            // 
            this.simulationModelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simulationModelTextBox.Location = new System.Drawing.Point(101, 15);
            this.simulationModelTextBox.Name = "simulationModelTextBox";
            this.simulationModelTextBox.Size = new System.Drawing.Size(367, 20);
            this.simulationModelTextBox.TabIndex = 0;
            // 
            // simulationModelButton
            // 
            this.simulationModelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simulationModelButton.Location = new System.Drawing.Point(473, 12);
            this.simulationModelButton.Name = "simulationModelButton";
            this.simulationModelButton.Size = new System.Drawing.Size(75, 23);
            this.simulationModelButton.TabIndex = 1;
            this.simulationModelButton.Text = "Browse";
            this.simulationModelButton.UseVisualStyleBackColor = true;
            this.simulationModelButton.Click += new System.EventHandler(this.simulationModelButton_Click);
            // 
            // stateButton
            // 
            this.stateButton.Location = new System.Drawing.Point(14, 154);
            this.stateButton.Name = "stateButton";
            this.stateButton.Size = new System.Drawing.Size(107, 23);
            this.stateButton.TabIndex = 2;
            this.stateButton.Text = "Start SimEngine";
            this.stateButton.UseVisualStyleBackColor = true;
            this.stateButton.Click += new System.EventHandler(this.stateButton_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(125, 154);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(410, 20);
            this.statusTextBox.TabIndex = 3;
            this.statusTextBox.Text = "inactive";
            // 
            // serverPortTextBox
            // 
            this.serverPortTextBox.Location = new System.Drawing.Point(311, 106);
            this.serverPortTextBox.Name = "serverPortTextBox";
            this.serverPortTextBox.Size = new System.Drawing.Size(100, 20);
            this.serverPortTextBox.TabIndex = 4;
            this.serverPortTextBox.Text = "9999";
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.AutoSize = true;
            this.serverPortLabel.Location = new System.Drawing.Point(218, 109);
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(60, 13);
            this.serverPortLabel.TabIndex = 5;
            this.serverPortLabel.Text = "Server Port";
            this.serverPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(13, 424);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(59, 13);
            this.timeLabel.TabIndex = 7;
            this.timeLabel.Text = "Time (sec):";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(78, 421);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(106, 20);
            this.timeTextBox.TabIndex = 8;
            // 
            // hostnameLabel
            // 
            this.hostnameLabel.AutoSize = true;
            this.hostnameLabel.Location = new System.Drawing.Point(8, 110);
            this.hostnameLabel.Name = "hostnameLabel";
            this.hostnameLabel.Size = new System.Drawing.Size(58, 13);
            this.hostnameLabel.TabIndex = 9;
            this.hostnameLabel.Text = "Hostname:";
            this.hostnameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hostnameTextBox
            // 
            this.hostnameTextBox.Location = new System.Drawing.Point(97, 106);
            this.hostnameTextBox.Name = "hostnameTextBox";
            this.hostnameTextBox.ReadOnly = true;
            this.hostnameTextBox.Size = new System.Drawing.Size(104, 20);
            this.hostnameTextBox.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.objectStatus);
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Controls.Add(this.objectListBox);
            this.groupBox1.Location = new System.Drawing.Point(196, 384);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 268);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Objects";
            // 
            // objectStatus
            // 
            this.objectStatus.AutoSize = true;
            this.objectStatus.Location = new System.Drawing.Point(7, 17);
            this.objectStatus.Name = "objectStatus";
            this.objectStatus.Size = new System.Drawing.Size(90, 17);
            this.objectStatus.TabIndex = 10;
            this.objectStatus.Text = "Object Status";
            this.objectStatus.UseVisualStyleBackColor = true;
            this.objectStatus.CheckedChanged += new System.EventHandler(this.objectStatus_CheckedChanged);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(102, 40);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(237, 218);
            this.treeView1.TabIndex = 9;
            // 
            // objectListBox
            // 
            this.objectListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.objectListBox.FormattingEnabled = true;
            this.objectListBox.IntegralHeight = false;
            this.objectListBox.Location = new System.Drawing.Point(7, 40);
            this.objectListBox.Name = "objectListBox";
            this.objectListBox.Size = new System.Drawing.Size(88, 218);
            this.objectListBox.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.connectionsListBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 447);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 205);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client Connections";
            // 
            // connectionsListBox
            // 
            this.connectionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionsListBox.FormattingEnabled = true;
            this.connectionsListBox.IntegralHeight = false;
            this.connectionsListBox.Location = new System.Drawing.Point(7, 12);
            this.connectionsListBox.Name = "connectionsListBox";
            this.connectionsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.connectionsListBox.Size = new System.Drawing.Size(165, 184);
            this.connectionsListBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.schemaTextBox);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.scenarioStartButton);
            this.groupBox3.Controls.Add(this.scenarioBrowseButton);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.scenarioTextBox);
            this.groupBox3.Location = new System.Drawing.Point(10, 184);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(531, 102);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scenario Controller";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Schema File";
            // 
            // schemaTextBox
            // 
            this.schemaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaTextBox.Location = new System.Drawing.Point(81, 15);
            this.schemaTextBox.Name = "schemaTextBox";
            this.schemaTextBox.Size = new System.Drawing.Size(364, 20);
            this.schemaTextBox.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(450, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.schemaBrowseButton_Click);
            // 
            // scenarioStartButton
            // 
            this.scenarioStartButton.Enabled = false;
            this.scenarioStartButton.Location = new System.Drawing.Point(6, 76);
            this.scenarioStartButton.Name = "scenarioStartButton";
            this.scenarioStartButton.Size = new System.Drawing.Size(107, 23);
            this.scenarioStartButton.TabIndex = 3;
            this.scenarioStartButton.Text = "Start Scenario";
            this.scenarioStartButton.UseVisualStyleBackColor = true;
            this.scenarioStartButton.Click += new System.EventHandler(this.scenarioStartButton_Click);
            // 
            // scenarioBrowseButton
            // 
            this.scenarioBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioBrowseButton.Location = new System.Drawing.Point(450, 49);
            this.scenarioBrowseButton.Name = "scenarioBrowseButton";
            this.scenarioBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.scenarioBrowseButton.TabIndex = 2;
            this.scenarioBrowseButton.Text = "Browse";
            this.scenarioBrowseButton.UseVisualStyleBackColor = true;
            this.scenarioBrowseButton.Click += new System.EventHandler(this.scenarioBrowseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scenario File";
            // 
            // scenarioTextBox
            // 
            this.scenarioTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioTextBox.Location = new System.Drawing.Point(80, 50);
            this.scenarioTextBox.Name = "scenarioTextBox";
            this.scenarioTextBox.Size = new System.Drawing.Size(364, 20);
            this.scenarioTextBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Simulation Model";
            // 
            // checkBoxReplayLog
            // 
            this.checkBoxReplayLog.Location = new System.Drawing.Point(76, 43);
            this.checkBoxReplayLog.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxReplayLog.Name = "checkBoxReplayLog";
            this.checkBoxReplayLog.Size = new System.Drawing.Size(16, 17);
            this.checkBoxReplayLog.TabIndex = 15;
            this.checkBoxReplayLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxReplayLog.UseVisualStyleBackColor = true;
            this.checkBoxReplayLog.CheckedChanged += new System.EventHandler(this.checkBoxReplayLog_CheckedChanged);
            // 
            // labelReplayLog
            // 
            this.labelReplayLog.AutoSize = true;
            this.labelReplayLog.Location = new System.Drawing.Point(8, 43);
            this.labelReplayLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelReplayLog.Name = "labelReplayLog";
            this.labelReplayLog.Size = new System.Drawing.Size(61, 13);
            this.labelReplayLog.TabIndex = 16;
            this.labelReplayLog.Text = "Replay Log";
            // 
            // textBoxReplayLog
            // 
            this.textBoxReplayLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplayLog.Enabled = false;
            this.textBoxReplayLog.Location = new System.Drawing.Point(196, 41);
            this.textBoxReplayLog.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxReplayLog.Name = "textBoxReplayLog";
            this.textBoxReplayLog.Size = new System.Drawing.Size(273, 20);
            this.textBoxReplayLog.TabIndex = 17;
            this.textBoxReplayLog.Text = "log.txt";
            // 
            // buttonReplayLog
            // 
            this.buttonReplayLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReplayLog.Enabled = false;
            this.buttonReplayLog.Location = new System.Drawing.Point(473, 41);
            this.buttonReplayLog.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReplayLog.Name = "buttonReplayLog";
            this.buttonReplayLog.Size = new System.Drawing.Size(75, 23);
            this.buttonReplayLog.TabIndex = 18;
            this.buttonReplayLog.Text = "Browse";
            this.buttonReplayLog.UseVisualStyleBackColor = true;
            this.buttonReplayLog.Click += new System.EventHandler(this.buttonReplayLog_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBoxLogDetail
            // 
            this.groupBoxLogDetail.Controls.Add(this.radioButtonLimited);
            this.groupBoxLogDetail.Controls.Add(this.radioButtonDetailed);
            this.groupBoxLogDetail.Enabled = false;
            this.groupBoxLogDetail.Location = new System.Drawing.Point(97, 39);
            this.groupBoxLogDetail.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxLogDetail.Name = "groupBoxLogDetail";
            this.groupBoxLogDetail.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxLogDetail.Size = new System.Drawing.Size(93, 60);
            this.groupBoxLogDetail.TabIndex = 19;
            this.groupBoxLogDetail.TabStop = false;
            this.groupBoxLogDetail.Text = "Level of Detail";
            // 
            // radioButtonLimited
            // 
            this.radioButtonLimited.AutoSize = true;
            this.radioButtonLimited.Location = new System.Drawing.Point(5, 41);
            this.radioButtonLimited.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonLimited.Name = "radioButtonLimited";
            this.radioButtonLimited.Size = new System.Drawing.Size(58, 17);
            this.radioButtonLimited.TabIndex = 1;
            this.radioButtonLimited.Text = "Limited";
            this.radioButtonLimited.UseVisualStyleBackColor = true;
            // 
            // radioButtonDetailed
            // 
            this.radioButtonDetailed.AutoSize = true;
            this.radioButtonDetailed.Checked = true;
            this.radioButtonDetailed.Location = new System.Drawing.Point(5, 18);
            this.radioButtonDetailed.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDetailed.Name = "radioButtonDetailed";
            this.radioButtonDetailed.Size = new System.Drawing.Size(64, 17);
            this.radioButtonDetailed.TabIndex = 0;
            this.radioButtonDetailed.TabStop = true;
            this.radioButtonDetailed.Text = "Detailed";
            this.radioButtonDetailed.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(14, 395);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(107, 23);
            this.pauseButton.TabIndex = 20;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.replaySpeedLabel);
            this.groupBox4.Controls.Add(this.speedRadioButton6);
            this.groupBox4.Controls.Add(this.speedRadioButton5);
            this.groupBox4.Controls.Add(this.speedRadioButton4);
            this.groupBox4.Controls.Add(this.speedRadioButton3);
            this.groupBox4.Controls.Add(this.speedRadioButton2);
            this.groupBox4.Controls.Add(this.speedRadioButton1);
            this.groupBox4.Controls.Add(this.replayLoopCheckbox);
            this.groupBox4.Controls.Add(this.startReplayButton);
            this.groupBox4.Controls.Add(this.replayBrowseButton);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.replayTextBox);
            this.groupBox4.Location = new System.Drawing.Point(10, 292);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(535, 86);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Replay";
            // 
            // replaySpeedLabel
            // 
            this.replaySpeedLabel.AutoSize = true;
            this.replaySpeedLabel.Location = new System.Drawing.Point(219, 62);
            this.replaySpeedLabel.Name = "replaySpeedLabel";
            this.replaySpeedLabel.Size = new System.Drawing.Size(50, 13);
            this.replaySpeedLabel.TabIndex = 11;
            this.replaySpeedLabel.Text = "Speed: 1";
            // 
            // speedRadioButton6
            // 
            this.speedRadioButton6.AutoSize = true;
            this.speedRadioButton6.Location = new System.Drawing.Point(494, 60);
            this.speedRadioButton6.Name = "speedRadioButton6";
            this.speedRadioButton6.Size = new System.Drawing.Size(31, 17);
            this.speedRadioButton6.TabIndex = 10;
            this.speedRadioButton6.Text = "8";
            this.speedRadioButton6.UseVisualStyleBackColor = true;
            this.speedRadioButton6.CheckedChanged += new System.EventHandler(this.speedRadioButton6_CheckedChanged);
            // 
            // speedRadioButton5
            // 
            this.speedRadioButton5.AutoSize = true;
            this.speedRadioButton5.Location = new System.Drawing.Point(457, 60);
            this.speedRadioButton5.Name = "speedRadioButton5";
            this.speedRadioButton5.Size = new System.Drawing.Size(31, 17);
            this.speedRadioButton5.TabIndex = 9;
            this.speedRadioButton5.Text = "4";
            this.speedRadioButton5.UseVisualStyleBackColor = true;
            this.speedRadioButton5.CheckedChanged += new System.EventHandler(this.speedRadioButton5_CheckedChanged);
            // 
            // speedRadioButton4
            // 
            this.speedRadioButton4.AutoSize = true;
            this.speedRadioButton4.Location = new System.Drawing.Point(420, 60);
            this.speedRadioButton4.Name = "speedRadioButton4";
            this.speedRadioButton4.Size = new System.Drawing.Size(31, 17);
            this.speedRadioButton4.TabIndex = 8;
            this.speedRadioButton4.Text = "2";
            this.speedRadioButton4.UseVisualStyleBackColor = true;
            this.speedRadioButton4.CheckedChanged += new System.EventHandler(this.speedRadioButton4_CheckedChanged);
            // 
            // speedRadioButton3
            // 
            this.speedRadioButton3.AutoSize = true;
            this.speedRadioButton3.Checked = true;
            this.speedRadioButton3.Location = new System.Drawing.Point(383, 60);
            this.speedRadioButton3.Name = "speedRadioButton3";
            this.speedRadioButton3.Size = new System.Drawing.Size(31, 17);
            this.speedRadioButton3.TabIndex = 7;
            this.speedRadioButton3.TabStop = true;
            this.speedRadioButton3.Text = "1";
            this.speedRadioButton3.UseVisualStyleBackColor = true;
            this.speedRadioButton3.CheckedChanged += new System.EventHandler(this.speedRadioButton3_CheckedChanged);
            // 
            // speedRadioButton2
            // 
            this.speedRadioButton2.AutoSize = true;
            this.speedRadioButton2.Location = new System.Drawing.Point(335, 60);
            this.speedRadioButton2.Name = "speedRadioButton2";
            this.speedRadioButton2.Size = new System.Drawing.Size(42, 17);
            this.speedRadioButton2.TabIndex = 6;
            this.speedRadioButton2.Text = "1/2";
            this.speedRadioButton2.UseVisualStyleBackColor = true;
            this.speedRadioButton2.CheckedChanged += new System.EventHandler(this.speedRadioButton2_CheckedChanged);
            // 
            // speedRadioButton1
            // 
            this.speedRadioButton1.AutoSize = true;
            this.speedRadioButton1.Location = new System.Drawing.Point(288, 60);
            this.speedRadioButton1.Name = "speedRadioButton1";
            this.speedRadioButton1.Size = new System.Drawing.Size(42, 17);
            this.speedRadioButton1.TabIndex = 5;
            this.speedRadioButton1.Text = "1/4";
            this.speedRadioButton1.UseVisualStyleBackColor = true;
            this.speedRadioButton1.CheckedChanged += new System.EventHandler(this.speedRadioButton1_CheckedChanged);
            // 
            // replayLoopCheckbox
            // 
            this.replayLoopCheckbox.AutoSize = true;
            this.replayLoopCheckbox.Location = new System.Drawing.Point(115, 61);
            this.replayLoopCheckbox.Name = "replayLoopCheckbox";
            this.replayLoopCheckbox.Size = new System.Drawing.Size(50, 17);
            this.replayLoopCheckbox.TabIndex = 4;
            this.replayLoopCheckbox.Text = "Loop";
            this.replayLoopCheckbox.UseVisualStyleBackColor = true;
            // 
            // startReplayButton
            // 
            this.startReplayButton.Enabled = false;
            this.startReplayButton.Location = new System.Drawing.Point(4, 57);
            this.startReplayButton.Name = "startReplayButton";
            this.startReplayButton.Size = new System.Drawing.Size(107, 23);
            this.startReplayButton.TabIndex = 3;
            this.startReplayButton.Text = "Start Replay";
            this.startReplayButton.UseVisualStyleBackColor = true;
            this.startReplayButton.Click += new System.EventHandler(this.startReplayButton_Click);
            // 
            // replayBrowseButton
            // 
            this.replayBrowseButton.Location = new System.Drawing.Point(451, 16);
            this.replayBrowseButton.Name = "replayBrowseButton";
            this.replayBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.replayBrowseButton.TabIndex = 2;
            this.replayBrowseButton.Text = "Browse";
            this.replayBrowseButton.UseVisualStyleBackColor = true;
            this.replayBrowseButton.Click += new System.EventHandler(this.replayBrowseButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Replay Log";
            // 
            // replayTextBox
            // 
            this.replayTextBox.Location = new System.Drawing.Point(81, 20);
            this.replayTextBox.Name = "replayTextBox";
            this.replayTextBox.Size = new System.Drawing.Size(363, 20);
            this.replayTextBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 664);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.groupBoxLogDetail);
            this.Controls.Add(this.buttonReplayLog);
            this.Controls.Add(this.textBoxReplayLog);
            this.Controls.Add(this.labelReplayLog);
            this.Controls.Add(this.checkBoxReplayLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.hostnameTextBox);
            this.Controls.Add(this.hostnameLabel);
            this.Controls.Add(this.serverPortLabel);
            this.Controls.Add(this.serverPortTextBox);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.stateButton);
            this.Controls.Add(this.simulationModelButton);
            this.Controls.Add(this.simulationModelTextBox);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "SimCoreTestGUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxLogDetail.ResumeLayout(false);
            this.groupBoxLogDetail.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox simulationModelTextBox;
        private System.Windows.Forms.Button simulationModelButton;
        private System.Windows.Forms.Button stateButton;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label serverPortLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.Label hostnameLabel;
        private System.Windows.Forms.TextBox hostnameTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox connectionsListBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox scenarioTextBox;
        private System.Windows.Forms.Button scenarioBrowseButton;
        private System.Windows.Forms.Button scenarioStartButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox schemaTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxReplayLog;
        private System.Windows.Forms.Label labelReplayLog;
        private System.Windows.Forms.TextBox textBoxReplayLog;
        private System.Windows.Forms.Button buttonReplayLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox objectListBox;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox objectStatus;
        private System.Windows.Forms.GroupBox groupBoxLogDetail;
        private System.Windows.Forms.RadioButton radioButtonLimited;
        private System.Windows.Forms.RadioButton radioButtonDetailed;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button replayBrowseButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox replayTextBox;
        private System.Windows.Forms.Button startReplayButton;
        private System.Windows.Forms.CheckBox replayLoopCheckbox;
        private System.Windows.Forms.RadioButton speedRadioButton1;
        private System.Windows.Forms.RadioButton speedRadioButton5;
        private System.Windows.Forms.RadioButton speedRadioButton4;
        private System.Windows.Forms.RadioButton speedRadioButton3;
        private System.Windows.Forms.RadioButton speedRadioButton2;
        private System.Windows.Forms.RadioButton speedRadioButton6;
        private System.Windows.Forms.Label replaySpeedLabel;
    }
}

