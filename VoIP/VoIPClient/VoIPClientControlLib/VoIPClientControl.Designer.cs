namespace Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib
{
    partial class VoIPClientControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoIPClientControl));
            this.channelsTreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.voiceactTrackBar = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.voiceActRadio = new System.Windows.Forms.RadioButton();
            this.pushToTalkRadio = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.joinchannelTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.joinButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vumeterProgressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mastervolTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.voiceactTrackBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mastervolTrackBar)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelsTreeView
            // 
            this.channelsTreeView.ImageIndex = 0;
            this.channelsTreeView.ImageList = this.imageList1;
            this.channelsTreeView.Location = new System.Drawing.Point(4, 17);
            this.channelsTreeView.Margin = new System.Windows.Forms.Padding(2);
            this.channelsTreeView.Name = "channelsTreeView";
            this.channelsTreeView.SelectedImageIndex = 0;
            this.channelsTreeView.Size = new System.Drawing.Size(171, 225);
            this.channelsTreeView.TabIndex = 2;
            this.channelsTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.channelsTreeView_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "userTalking.bmp");
            this.imageList1.Images.SetKeyName(1, "userNotTalking.bmp");
            this.imageList1.Images.SetKeyName(2, "channel.bmp");
            // 
            // voiceactTrackBar
            // 
            this.voiceactTrackBar.Location = new System.Drawing.Point(81, 17);
            this.voiceactTrackBar.Margin = new System.Windows.Forms.Padding(2);
            this.voiceactTrackBar.Maximum = 20;
            this.voiceactTrackBar.Name = "voiceactTrackBar";
            this.voiceactTrackBar.Size = new System.Drawing.Size(176, 45);
            this.voiceactTrackBar.TabIndex = 1;
            this.voiceactTrackBar.ValueChanged += new System.EventHandler(this.voiceactTrackBar_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.voiceActRadio);
            this.groupBox2.Controls.Add(this.pushToTalkRadio);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.voiceactTrackBar);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(183, 153);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(274, 96);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Voice Transmission";
            // 
            // voiceActRadio
            // 
            this.voiceActRadio.AutoSize = true;
            this.voiceActRadio.Location = new System.Drawing.Point(110, 56);
            this.voiceActRadio.Name = "voiceActRadio";
            this.voiceActRadio.Size = new System.Drawing.Size(100, 17);
            this.voiceActRadio.TabIndex = 5;
            this.voiceActRadio.TabStop = true;
            this.voiceActRadio.Text = "Voice Activated";
            this.voiceActRadio.UseVisualStyleBackColor = true;
            this.voiceActRadio.CheckedChanged += new System.EventHandler(this.voiceActRadio_CheckedChanged);
            // 
            // pushToTalkRadio
            // 
            this.pushToTalkRadio.AutoSize = true;
            this.pushToTalkRadio.Checked = true;
            this.pushToTalkRadio.Location = new System.Drawing.Point(8, 56);
            this.pushToTalkRadio.Name = "pushToTalkRadio";
            this.pushToTalkRadio.Size = new System.Drawing.Size(83, 17);
            this.pushToTalkRadio.TabIndex = 4;
            this.pushToTalkRadio.TabStop = true;
            this.pushToTalkRadio.Text = "PushToTalk";
            this.pushToTalkRadio.UseVisualStyleBackColor = true;
            this.pushToTalkRadio.CheckedChanged += new System.EventHandler(this.pushToTalkRadio_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 76);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "To talk, push F12";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Voice act. level";
            // 
            // joinchannelTextBox
            // 
            this.joinchannelTextBox.Location = new System.Drawing.Point(59, 18);
            this.joinchannelTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.joinchannelTextBox.Name = "joinchannelTextBox";
            this.joinchannelTextBox.ReadOnly = true;
            this.joinchannelTextBox.Size = new System.Drawing.Size(150, 20);
            this.joinchannelTextBox.TabIndex = 7;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.joinchannelTextBox);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.joinButton);
            this.groupBox4.Location = new System.Drawing.Point(184, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(274, 52);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Channel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Channel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(213, 16);
            this.joinButton.Margin = new System.Windows.Forms.Padding(2);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(57, 21);
            this.joinButton.TabIndex = 5;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.channelsTreeView);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(179, 249);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // vumeterProgressBar
            // 
            this.vumeterProgressBar.Location = new System.Drawing.Point(62, 63);
            this.vumeterProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.vumeterProgressBar.Maximum = 20;
            this.vumeterProgressBar.Name = "vumeterProgressBar";
            this.vumeterProgressBar.Size = new System.Drawing.Size(196, 19);
            this.vumeterProgressBar.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.vumeterProgressBar);
            this.groupBox5.Controls.Add(this.mastervolTrackBar);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(183, 57);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(274, 90);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sound settings";
            // 
            // mastervolTrackBar
            // 
            this.mastervolTrackBar.Location = new System.Drawing.Point(62, 24);
            this.mastervolTrackBar.Margin = new System.Windows.Forms.Padding(2);
            this.mastervolTrackBar.Maximum = 255;
            this.mastervolTrackBar.Name = "mastervolTrackBar";
            this.mastervolTrackBar.Size = new System.Drawing.Size(196, 45);
            this.mastervolTrackBar.TabIndex = 3;
            this.mastervolTrackBar.TickFrequency = 20;
            this.mastervolTrackBar.ValueChanged += new System.EventHandler(this.mastervolTrackBar_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 24);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Volume";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "VU-Meter";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(397, 363);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(56, 21);
            this.closeButton.TabIndex = 19;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBoxLog);
            this.groupBox3.Location = new System.Drawing.Point(0, 253);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(457, 106);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.CausesValidation = false;
            this.richTextBoxLog.Location = new System.Drawing.Point(5, 17);
            this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(448, 85);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // VoIPClientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Name = "VoIPClientControl";
            this.Size = new System.Drawing.Size(463, 401);
            this.Load += new System.EventHandler(this.VoIPClientControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.voiceactTrackBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mastervolTrackBar)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView channelsTreeView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TrackBar voiceactTrackBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox joinchannelTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar vumeterProgressBar;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar mastervolTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.RadioButton voiceActRadio;
        private System.Windows.Forms.RadioButton pushToTalkRadio;
    }
}
