namespace Aptima.Asim.DDD.VoIPClientApp
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
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Form1 ) );
            this.channelsTreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList( this.components );
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.joinchannelTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.joinButton = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.vumeterProgressBar = new System.Windows.Forms.ProgressBar();
            this.mastervolTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.vkTextBox = new System.Windows.Forms.TextBox();
            this.ctrlCheckBox = new System.Windows.Forms.CheckBox();
            this.altCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.shiftCheckBox = new System.Windows.Forms.CheckBox();
            this.winCheckBox = new System.Windows.Forms.CheckBox();
            this.voiceactCheckBox = new System.Windows.Forms.CheckBox();
            this.voiceactTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.mastervolTrackBar ) ).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.voiceactTrackBar ) ).BeginInit();
            this.SuspendLayout();
            // 
            // channelsTreeView
            // 
            this.channelsTreeView.ImageIndex = 0;
            this.channelsTreeView.ImageList = this.imageList1;
            this.channelsTreeView.Location = new System.Drawing.Point( 6, 21 );
            this.channelsTreeView.Name = "channelsTreeView";
            this.channelsTreeView.SelectedImageIndex = 0;
            this.channelsTreeView.Size = new System.Drawing.Size( 155, 352 );
            this.channelsTreeView.TabIndex = 2;
            this.channelsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.channelsTreeView_AfterSelect );
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ( ( System.Windows.Forms.ImageListStreamer )( resources.GetObject( "imageList1.ImageStream" ) ) );
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName( 0, "userTalking.bmp" );
            this.imageList1.Images.SetKeyName( 1, "userNotTalking.bmp" );
            this.imageList1.Images.SetKeyName( 2, "channel.bmp" );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.channelsTreeView );
            this.groupBox1.Location = new System.Drawing.Point( 13, 13 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 168, 379 );
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server state";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add( this.joinchannelTextBox );
            this.groupBox4.Controls.Add( this.label1 );
            this.groupBox4.Controls.Add( this.joinButton );
            this.groupBox4.Location = new System.Drawing.Point( 187, 13 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size( 352, 64 );
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Join Channel";
            // 
            // joinchannelTextBox
            // 
            this.joinchannelTextBox.Location = new System.Drawing.Point( 82, 22 );
            this.joinchannelTextBox.Name = "joinchannelTextBox";
            this.joinchannelTextBox.Size = new System.Drawing.Size( 199, 22 );
            this.joinchannelTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 6, 25 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 60, 17 );
            this.label1.TabIndex = 6;
            this.label1.Text = "Channel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point( 287, 20 );
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size( 56, 26 );
            this.joinButton.TabIndex = 5;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler( this.joinButton_Click );
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add( this.vumeterProgressBar );
            this.groupBox5.Controls.Add( this.mastervolTrackBar );
            this.groupBox5.Controls.Add( this.label4 );
            this.groupBox5.Controls.Add( this.label3 );
            this.groupBox5.Location = new System.Drawing.Point( 187, 83 );
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size( 352, 111 );
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sound settings";
            // 
            // vumeterProgressBar
            // 
            this.vumeterProgressBar.Location = new System.Drawing.Point( 82, 78 );
            this.vumeterProgressBar.Maximum = 20;
            this.vumeterProgressBar.Name = "vumeterProgressBar";
            this.vumeterProgressBar.Size = new System.Drawing.Size( 261, 23 );
            this.vumeterProgressBar.TabIndex = 1;
            // 
            // mastervolTrackBar
            // 
            this.mastervolTrackBar.Location = new System.Drawing.Point( 82, 30 );
            this.mastervolTrackBar.Maximum = 255;
            this.mastervolTrackBar.Name = "mastervolTrackBar";
            this.mastervolTrackBar.Size = new System.Drawing.Size( 261, 56 );
            this.mastervolTrackBar.TabIndex = 3;
            this.mastervolTrackBar.TickFrequency = 20;
            this.mastervolTrackBar.ValueChanged += new System.EventHandler( this.mastervolTrackBar_ValueChanged );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 6, 30 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 55, 17 );
            this.label4.TabIndex = 2;
            this.label4.Text = "Volume";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 6, 81 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 68, 17 );
            this.label3.TabIndex = 0;
            this.label3.Text = "VU-Meter";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add( this.textBoxLog );
            this.groupBox3.Location = new System.Drawing.Point( 12, 398 );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size( 527, 202 );
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point( 17, 21 );
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.Size = new System.Drawing.Size( 501, 175 );
            this.textBoxLog.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.groupBox6 );
            this.groupBox2.Controls.Add( this.voiceactCheckBox );
            this.groupBox2.Controls.Add( this.voiceactTrackBar );
            this.groupBox2.Controls.Add( this.label2 );
            this.groupBox2.Location = new System.Drawing.Point( 187, 201 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 352, 191 );
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Voice Transmission";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add( this.registerButton );
            this.groupBox6.Controls.Add( this.vkTextBox );
            this.groupBox6.Controls.Add( this.ctrlCheckBox );
            this.groupBox6.Controls.Add( this.altCheckBox );
            this.groupBox6.Controls.Add( this.label5 );
            this.groupBox6.Controls.Add( this.shiftCheckBox );
            this.groupBox6.Controls.Add( this.winCheckBox );
            this.groupBox6.Location = new System.Drawing.Point( 6, 70 );
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size( 337, 115 );
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Push to talk";
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point( 210, 83 );
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size( 121, 26 );
            this.registerButton.TabIndex = 9;
            this.registerButton.Text = "Register hotkey";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler( this.registerButton_Click );
            // 
            // vkTextBox
            // 
            this.vkTextBox.Location = new System.Drawing.Point( 87, 85 );
            this.vkTextBox.MaxLength = 1;
            this.vkTextBox.Name = "vkTextBox";
            this.vkTextBox.Size = new System.Drawing.Size( 100, 22 );
            this.vkTextBox.TabIndex = 8;
            // 
            // ctrlCheckBox
            // 
            this.ctrlCheckBox.AutoSize = true;
            this.ctrlCheckBox.Location = new System.Drawing.Point( 6, 22 );
            this.ctrlCheckBox.Name = "ctrlCheckBox";
            this.ctrlCheckBox.Size = new System.Drawing.Size( 75, 21 );
            this.ctrlCheckBox.TabIndex = 3;
            this.ctrlCheckBox.Text = "Control";
            this.ctrlCheckBox.UseVisualStyleBackColor = true;
            // 
            // altCheckBox
            // 
            this.altCheckBox.AutoSize = true;
            this.altCheckBox.Location = new System.Drawing.Point( 6, 52 );
            this.altCheckBox.Name = "altCheckBox";
            this.altCheckBox.Size = new System.Drawing.Size( 46, 21 );
            this.altCheckBox.TabIndex = 5;
            this.altCheckBox.Text = "Alt";
            this.altCheckBox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 3, 85 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 74, 17 );
            this.label5.TabIndex = 7;
            this.label5.Text = "Virtual key";
            // 
            // shiftCheckBox
            // 
            this.shiftCheckBox.AutoSize = true;
            this.shiftCheckBox.Location = new System.Drawing.Point( 87, 22 );
            this.shiftCheckBox.Name = "shiftCheckBox";
            this.shiftCheckBox.Size = new System.Drawing.Size( 58, 21 );
            this.shiftCheckBox.TabIndex = 4;
            this.shiftCheckBox.Text = "Shift";
            this.shiftCheckBox.UseVisualStyleBackColor = true;
            // 
            // winCheckBox
            // 
            this.winCheckBox.AutoSize = true;
            this.winCheckBox.Location = new System.Drawing.Point( 87, 52 );
            this.winCheckBox.Name = "winCheckBox";
            this.winCheckBox.Size = new System.Drawing.Size( 54, 21 );
            this.winCheckBox.TabIndex = 6;
            this.winCheckBox.Text = "Win";
            this.winCheckBox.UseVisualStyleBackColor = true;
            // 
            // voiceactCheckBox
            // 
            this.voiceactCheckBox.AutoSize = true;
            this.voiceactCheckBox.Location = new System.Drawing.Point( 12, 43 );
            this.voiceactCheckBox.Name = "voiceactCheckBox";
            this.voiceactCheckBox.Size = new System.Drawing.Size( 65, 21 );
            this.voiceactCheckBox.TabIndex = 2;
            this.voiceactCheckBox.Text = "Voice";
            this.voiceactCheckBox.UseVisualStyleBackColor = true;
            this.voiceactCheckBox.CheckedChanged += new System.EventHandler( this.voiceactCheckBox_CheckedChanged );
            // 
            // voiceactTrackBar
            // 
            this.voiceactTrackBar.Location = new System.Drawing.Point( 108, 21 );
            this.voiceactTrackBar.Maximum = 20;
            this.voiceactTrackBar.Name = "voiceactTrackBar";
            this.voiceactTrackBar.Size = new System.Drawing.Size( 235, 56 );
            this.voiceactTrackBar.TabIndex = 1;
            this.voiceactTrackBar.ValueChanged += new System.EventHandler( this.voiceactTrackBar_ValueChanged );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 6, 21 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 103, 17 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Voice act. level";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point( 463, 607 );
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size( 75, 26 );
            this.closeButton.TabIndex = 13;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler( this.closeButton_Click );
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 554, 642 );
            this.Controls.Add( this.closeButton );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.groupBox3 );
            this.Controls.Add( this.groupBox5 );
            this.Controls.Add( this.groupBox4 );
            this.Controls.Add( this.groupBox1 );
            this.Name = "Form1";
            this.Text = "VoIP Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Form1_FormClosing );
            this.Load += new System.EventHandler( this.Form1_Load );
            this.groupBox1.ResumeLayout( false );
            this.groupBox4.ResumeLayout( false );
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout( false );
            this.groupBox5.PerformLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.mastervolTrackBar ) ).EndInit();
            this.groupBox3.ResumeLayout( false );
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout( false );
            this.groupBox6.PerformLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.voiceactTrackBar ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TreeView channelsTreeView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox joinchannelTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ProgressBar vumeterProgressBar;
        private System.Windows.Forms.TrackBar mastervolTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar voiceactTrackBar;
        private System.Windows.Forms.CheckBox voiceactCheckBox;
        private System.Windows.Forms.CheckBox ctrlCheckBox;
        private System.Windows.Forms.CheckBox altCheckBox;
        private System.Windows.Forms.CheckBox shiftCheckBox;
        private System.Windows.Forms.CheckBox winCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox vkTextBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button closeButton;
    }
}

