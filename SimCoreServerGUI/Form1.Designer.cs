namespace Aptima.Asim.DDD.SimCoreGUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripUserAdministration = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openReplayLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutDDD40ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactUsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.softwareActivationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutDDD40ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.regToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSimStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSimLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonLoadScenario = new System.Windows.Forms.Button();
            this.buttonPauseSim = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewDMs = new System.Windows.Forms.DataGridView();
            this.DMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DMRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DMAvail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.replayLogPathTextBox = new System.Windows.Forms.TextBox();
            this.replayLogBrowse = new System.Windows.Forms.Button();
            this.replayStartButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.eventLogPathButton = new System.Windows.Forms.Button();
            this.eventLogTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.portNumberTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hostNameTextBox = new System.Windows.Forms.TextBox();
            this.labelScenarioName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.buttonScenario = new System.Windows.Forms.Button();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelScenarioIsLoading = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.forkReplayCheckbox = new System.Windows.Forms.CheckBox();
            this.comboBoxReplaySpeed = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxRecordSimVoices = new System.Windows.Forms.CheckBox();
            this.buttonToggleVoice = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDMs)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.aboutDDD40ToolStripMenuItem,
            this.regToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripUserAdministration});
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // toolStripUserAdministration
            // 
            this.toolStripUserAdministration.Name = "toolStripUserAdministration";
            this.toolStripUserAdministration.Size = new System.Drawing.Size(179, 22);
            this.toolStripUserAdministration.Text = "User Administration";
            this.toolStripUserAdministration.Click += new System.EventHandler(this.toolStripUserAdministration_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openReplayLogToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.fileToolStripMenuItem.Text = "Utilities";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // openReplayLogToolStripMenuItem
            // 
            this.openReplayLogToolStripMenuItem.Name = "openReplayLogToolStripMenuItem";
            this.openReplayLogToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.openReplayLogToolStripMenuItem.Text = "Replay Log Viewer";
            this.openReplayLogToolStripMenuItem.Click += new System.EventHandler(this.openReplayLogToolStripMenuItem_Click);
            // 
            // aboutDDD40ToolStripMenuItem
            // 
            this.aboutDDD40ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.contactUsToolStripMenuItem,
            this.softwareActivationToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutDDD40ToolStripMenuItem1});
            this.aboutDDD40ToolStripMenuItem.Name = "aboutDDD40ToolStripMenuItem";
            this.aboutDDD40ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutDDD40ToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // contactUsToolStripMenuItem
            // 
            this.contactUsToolStripMenuItem.Name = "contactUsToolStripMenuItem";
            this.contactUsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.contactUsToolStripMenuItem.Text = "Contact Us";
            this.contactUsToolStripMenuItem.Click += new System.EventHandler(this.contactUsToolStripMenuItem_Click);
            // 
            // softwareActivationToolStripMenuItem
            // 
            this.softwareActivationToolStripMenuItem.Name = "softwareActivationToolStripMenuItem";
            this.softwareActivationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.softwareActivationToolStripMenuItem.Text = "License...";
            //this.softwareActivationToolStripMenuItem.Click += new System.EventHandler(this.softwareActivationToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutDDD40ToolStripMenuItem1
            // 
            this.aboutDDD40ToolStripMenuItem1.Name = "aboutDDD40ToolStripMenuItem1";
            this.aboutDDD40ToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.aboutDDD40ToolStripMenuItem1.Text = "About DDD 4.2";
            this.aboutDDD40ToolStripMenuItem1.Click += new System.EventHandler(this.aboutDDD40ToolStripMenuItem1_Click);
            // 
            // regToolStripMenuItem
            // 
            this.regToolStripMenuItem.Enabled = false;
            this.regToolStripMenuItem.Name = "regToolStripMenuItem";
            this.regToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.regToolStripMenuItem.Text = "Reg";
            this.regToolStripMenuItem.Visible = false;
            this.regToolStripMenuItem.Click += new System.EventHandler(this.regToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelServerStatus,
            this.toolStripStatusLabelSimStatus,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelSimLength});
            this.statusStrip1.Location = new System.Drawing.Point(0, 330);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // toolStripStatusLabelServerStatus
            // 
            this.toolStripStatusLabelServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelServerStatus.Name = "toolStripStatusLabelServerStatus";
            this.toolStripStatusLabelServerStatus.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripStatusLabelServerStatus.Size = new System.Drawing.Size(120, 17);
            this.toolStripStatusLabelServerStatus.Text = "Server: STOPPED";
            //this.toolStripStatusLabelServerStatus.TextChanged += new System.EventHandler(this.toolStripStatusLabelServerStatus_TextChanged);
            // 
            // toolStripStatusLabelSimStatus
            // 
            this.toolStripStatusLabelSimStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelSimStatus.Name = "toolStripStatusLabelSimStatus";
            this.toolStripStatusLabelSimStatus.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripStatusLabelSimStatus.Size = new System.Drawing.Size(141, 17);
            this.toolStripStatusLabelSimStatus.Text = "Simulation: STOPPED";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.AutoToolTip = true;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBar1.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripProgressBar1.RightToLeftLayout = true;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(244, 16);
            this.toolStripProgressBar1.ToolTipText = "Current Simulation Progress";
            this.toolStripProgressBar1.Value = 33;
            this.toolStripProgressBar1.Visible = false;
            this.toolStripProgressBar1.Click += new System.EventHandler(this.toolStripProgressBar1_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(110, 17);
            this.toolStripStatusLabel1.Text = "Simulation Length: ";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // toolStripStatusLabelSimLength
            // 
            this.toolStripStatusLabelSimLength.Name = "toolStripStatusLabelSimLength";
            this.toolStripStatusLabelSimLength.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabelSimLength.Text = "Unknown";
            this.toolStripStatusLabelSimLength.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Scenario File:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Current Simulation Time: ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(348, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(77, 20);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "HH:MM:SS";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonLoadScenario
            // 
            this.buttonLoadScenario.Location = new System.Drawing.Point(15, 264);
            this.buttonLoadScenario.Name = "buttonLoadScenario";
            this.buttonLoadScenario.Size = new System.Drawing.Size(133, 23);
            this.buttonLoadScenario.TabIndex = 17;
            this.buttonLoadScenario.Text = "Load Scenario";
            this.buttonLoadScenario.UseVisualStyleBackColor = true;
            this.buttonLoadScenario.Click += new System.EventHandler(this.buttonLoadScenario_Click);
            // 
            // buttonPauseSim
            // 
            this.buttonPauseSim.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPauseSim.Enabled = false;
            this.buttonPauseSim.Location = new System.Drawing.Point(348, 64);
            this.buttonPauseSim.Name = "buttonPauseSim";
            this.buttonPauseSim.Size = new System.Drawing.Size(153, 23);
            this.buttonPauseSim.TabIndex = 20;
            this.buttonPauseSim.Text = "Start Simulation";
            this.buttonPauseSim.UseVisualStyleBackColor = true;
            this.buttonPauseSim.Click += new System.EventHandler(this.button8_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // dataGridViewDMs
            // 
            this.dataGridViewDMs.AllowUserToAddRows = false;
            this.dataGridViewDMs.AllowUserToDeleteRows = false;
            this.dataGridViewDMs.AllowUserToOrderColumns = true;
            this.dataGridViewDMs.AllowUserToResizeRows = false;
            this.dataGridViewDMs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDMs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewDMs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridViewDMs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDMs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DMName,
            this.DMRole,
            this.DMAvail});
            this.dataGridViewDMs.Location = new System.Drawing.Point(9, 19);
            this.dataGridViewDMs.MultiSelect = false;
            this.dataGridViewDMs.Name = "dataGridViewDMs";
            this.dataGridViewDMs.ReadOnly = true;
            this.dataGridViewDMs.RowHeadersVisible = false;
            this.dataGridViewDMs.RowTemplate.Height = 24;
            this.dataGridViewDMs.RowTemplate.ReadOnly = true;
            this.dataGridViewDMs.Size = new System.Drawing.Size(328, 145);
            this.dataGridViewDMs.TabIndex = 28;
            // 
            // DMName
            // 
            this.DMName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DMName.DefaultCellStyle = dataGridViewCellStyle1;
            this.DMName.HeaderText = "DM Name";
            this.DMName.Name = "DMName";
            this.DMName.ReadOnly = true;
            // 
            // DMRole
            // 
            this.DMRole.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DMRole.HeaderText = "DM Role";
            this.DMRole.Name = "DMRole";
            this.DMRole.ReadOnly = true;
            this.DMRole.Width = 74;
            // 
            // DMAvail
            // 
            this.DMAvail.HeaderText = "Availability";
            this.DMAvail.Name = "DMAvail";
            this.DMAvail.ReadOnly = true;
            this.DMAvail.Width = 81;
            // 
            // replayLogPathTextBox
            // 
            this.replayLogPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.replayLogPathTextBox.Location = new System.Drawing.Point(6, 23);
            this.replayLogPathTextBox.Name = "replayLogPathTextBox";
            this.replayLogPathTextBox.ReadOnly = true;
            this.replayLogPathTextBox.Size = new System.Drawing.Size(239, 20);
            this.replayLogPathTextBox.TabIndex = 29;
            this.replayLogPathTextBox.Text = "[Replay Log]";
            this.replayLogPathTextBox.TextChanged += new System.EventHandler(this.replayLogPathTextBox_TextChanged);
            // 
            // replayLogBrowse
            // 
            this.replayLogBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.replayLogBrowse.Location = new System.Drawing.Point(256, 21);
            this.replayLogBrowse.Name = "replayLogBrowse";
            this.replayLogBrowse.Size = new System.Drawing.Size(75, 23);
            this.replayLogBrowse.TabIndex = 30;
            this.replayLogBrowse.Text = "Browse";
            this.replayLogBrowse.UseVisualStyleBackColor = true;
            this.replayLogBrowse.Click += new System.EventHandler(this.replayLogBrowse_Click);
            // 
            // replayStartButton
            // 
            this.replayStartButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.replayStartButton.Location = new System.Drawing.Point(179, 56);
            this.replayStartButton.Name = "replayStartButton";
            this.replayStartButton.Size = new System.Drawing.Size(84, 23);
            this.replayStartButton.TabIndex = 31;
            this.replayStartButton.Text = "Load Replay";
            this.replayStartButton.UseVisualStyleBackColor = true;
            this.replayStartButton.Click += new System.EventHandler(this.replayStartButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eventLogPathButton);
            this.groupBox1.Controls.Add(this.eventLogTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.portNumberTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.hostNameTextBox);
            this.groupBox1.Controls.Add(this.labelScenarioName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxGroupName);
            this.groupBox1.Controls.Add(this.buttonScenario);
            this.groupBox1.Controls.Add(this.textBoxScenario);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonLoadScenario);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 296);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // eventLogPathButton
            // 
            this.eventLogPathButton.Location = new System.Drawing.Point(162, 140);
            this.eventLogPathButton.Name = "eventLogPathButton";
            this.eventLogPathButton.Size = new System.Drawing.Size(75, 23);
            this.eventLogPathButton.TabIndex = 43;
            this.eventLogPathButton.Text = "Browse...";
            this.eventLogPathButton.UseVisualStyleBackColor = true;
            this.eventLogPathButton.Click += new System.EventHandler(this.eventLogPathButton_Click);
            // 
            // eventLogTextBox
            // 
            this.eventLogTextBox.Location = new System.Drawing.Point(15, 142);
            this.eventLogTextBox.Name = "eventLogTextBox";
            this.eventLogTextBox.ReadOnly = true;
            this.eventLogTextBox.Size = new System.Drawing.Size(136, 20);
            this.eventLogTextBox.TabIndex = 42;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Event Log Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Port Number:";
            // 
            // portNumberTextBox
            // 
            this.portNumberTextBox.Location = new System.Drawing.Point(15, 89);
            this.portNumberTextBox.Name = "portNumberTextBox";
            this.portNumberTextBox.ReadOnly = true;
            this.portNumberTextBox.Size = new System.Drawing.Size(136, 20);
            this.portNumberTextBox.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Host Name:";
            // 
            // hostNameTextBox
            // 
            this.hostNameTextBox.Enabled = false;
            this.hostNameTextBox.Location = new System.Drawing.Point(15, 38);
            this.hostNameTextBox.Name = "hostNameTextBox";
            this.hostNameTextBox.ReadOnly = true;
            this.hostNameTextBox.Size = new System.Drawing.Size(136, 20);
            this.hostNameTextBox.TabIndex = 37;
            // 
            // labelScenarioName
            // 
            this.labelScenarioName.AutoSize = true;
            this.labelScenarioName.Location = new System.Drawing.Point(92, 177);
            this.labelScenarioName.Name = "labelScenarioName";
            this.labelScenarioName.Size = new System.Drawing.Size(89, 13);
            this.labelScenarioName.TabIndex = 36;
            this.labelScenarioName.Text = "<ScenarioName>";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Simulation Run ID:";
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Location = new System.Drawing.Point(15, 238);
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(136, 20);
            this.textBoxGroupName.TabIndex = 34;
            // 
            // buttonScenario
            // 
            this.buttonScenario.Location = new System.Drawing.Point(162, 191);
            this.buttonScenario.Name = "buttonScenario";
            this.buttonScenario.Size = new System.Drawing.Size(75, 23);
            this.buttonScenario.TabIndex = 31;
            this.buttonScenario.Text = "Browse...";
            this.buttonScenario.UseVisualStyleBackColor = true;
            this.buttonScenario.Click += new System.EventHandler(this.buttonScenario_Click);
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxScenario.Location = new System.Drawing.Point(15, 193);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.ReadOnly = true;
            this.textBoxScenario.Size = new System.Drawing.Size(136, 20);
            this.textBoxScenario.TabIndex = 30;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelScenarioIsLoading);
            this.groupBox2.Controls.Add(this.dataGridViewDMs);
            this.groupBox2.Controls.Add(this.buttonPauseSim);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(261, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 200);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulation";
            // 
            // labelScenarioIsLoading
            // 
            this.labelScenarioIsLoading.AutoSize = true;
            this.labelScenarioIsLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScenarioIsLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelScenarioIsLoading.Location = new System.Drawing.Point(72, 170);
            this.labelScenarioIsLoading.Name = "labelScenarioIsLoading";
            this.labelScenarioIsLoading.Size = new System.Drawing.Size(207, 24);
            this.labelScenarioIsLoading.TabIndex = 29;
            this.labelScenarioIsLoading.Text = "Scenario is loading...";
            this.labelScenarioIsLoading.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.forkReplayCheckbox);
            this.groupBox3.Controls.Add(this.comboBoxReplaySpeed);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.replayLogBrowse);
            this.groupBox3.Controls.Add(this.replayLogPathTextBox);
            this.groupBox3.Controls.Add(this.replayStartButton);
            this.groupBox3.Location = new System.Drawing.Point(261, 233);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 90);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Replay";
            // 
            // forkReplayCheckbox
            // 
            this.forkReplayCheckbox.AutoSize = true;
            this.forkReplayCheckbox.Location = new System.Drawing.Point(280, 61);
            this.forkReplayCheckbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.forkReplayCheckbox.Name = "forkReplayCheckbox";
            this.forkReplayCheckbox.Size = new System.Drawing.Size(35, 14);
            this.forkReplayCheckbox.TabIndex = 35;
            this.forkReplayCheckbox.Text = "Fork";
            this.forkReplayCheckbox.UseVisualStyleBackColor = true;
            // 
            // comboBoxReplaySpeed
            // 
            this.comboBoxReplaySpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReplaySpeed.FormattingEnabled = true;
            this.comboBoxReplaySpeed.Items.AddRange(new object[] {
            "500%",
            "400%",
            "250%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%"});
            this.comboBoxReplaySpeed.Location = new System.Drawing.Point(89, 58);
            this.comboBoxReplaySpeed.Name = "comboBoxReplaySpeed";
            this.comboBoxReplaySpeed.Size = new System.Drawing.Size(84, 21);
            this.comboBoxReplaySpeed.TabIndex = 34;
            this.comboBoxReplaySpeed.SelectedIndexChanged += new System.EventHandler(this.comboBoxReplaySpeed_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Replay Speed:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxRecordSimVoices);
            this.groupBox4.Controls.Add(this.buttonToggleVoice);
            this.groupBox4.Location = new System.Drawing.Point(604, 233);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(164, 90);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Voice";
            // 
            // checkBoxRecordSimVoices
            // 
            this.checkBoxRecordSimVoices.AutoSize = true;
            this.checkBoxRecordSimVoices.Location = new System.Drawing.Point(6, 58);
            this.checkBoxRecordSimVoices.Name = "checkBoxRecordSimVoices";
            this.checkBoxRecordSimVoices.Size = new System.Drawing.Size(71, 14);
            this.checkBoxRecordSimVoices.TabIndex = 1;
            this.checkBoxRecordSimVoices.Text = "Record Sound";
            this.checkBoxRecordSimVoices.UseVisualStyleBackColor = true;
            this.checkBoxRecordSimVoices.CheckedChanged += new System.EventHandler(this.checkBoxRecordSimVoices_CheckedChanged);
            // 
            // buttonToggleVoice
            // 
            this.buttonToggleVoice.Location = new System.Drawing.Point(6, 21);
            this.buttonToggleVoice.Name = "buttonToggleVoice";
            this.buttonToggleVoice.Size = new System.Drawing.Size(120, 23);
            this.buttonToggleVoice.TabIndex = 0;
            this.buttonToggleVoice.Text = "Disable/Enable";
            this.buttonToggleVoice.UseVisualStyleBackColor = true;
            this.buttonToggleVoice.Click += new System.EventHandler(this.buttonToggleVoice_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 352);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(802, 374);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Aptima DDD 4.2 Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDMs)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonLoadScenario;
        private System.Windows.Forms.Button buttonPauseSim;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.DataGridView dataGridViewDMs;
        private System.Windows.Forms.TextBox replayLogPathTextBox;
        private System.Windows.Forms.Button replayLogBrowse;
        private System.Windows.Forms.Button replayStartButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMAvail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxScenario;
        private System.Windows.Forms.Button buttonScenario;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServerStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSimStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSimLength;
        private System.Windows.Forms.ToolStripMenuItem aboutDDD40ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutDDD40ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem contactUsToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxReplaySpeed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelScenarioIsLoading;
        private System.Windows.Forms.Label labelScenarioName;
        private System.Windows.Forms.ToolStripMenuItem regToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripUserAdministration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hostNameTextBox;
        private System.Windows.Forms.TextBox portNumberTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox eventLogTextBox;
        private System.Windows.Forms.Button eventLogPathButton;
        private System.Windows.Forms.ToolStripMenuItem softwareActivationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openReplayLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxRecordSimVoices;
        private System.Windows.Forms.Button buttonToggleVoice;
        private System.Windows.Forms.CheckBox forkReplayCheckbox;
    }
}

