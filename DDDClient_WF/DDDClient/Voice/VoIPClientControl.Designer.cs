namespace Aptima.Asim.DDD.Client
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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.voiceactTrackBar = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.voiceActRadio = new System.Windows.Forms.RadioButton();
            this.pushToTalkRadio = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.vumeterProgressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mastervolTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.voiceSettingsP = new System.Windows.Forms.Panel();
            this.logP = new System.Windows.Forms.Panel();
            this.optionButtonP = new System.Windows.Forms.Panel();
            this.voiceOptionsB = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.channelsTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.voiceactTrackBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mastervolTrackBar)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.voiceSettingsP.SuspendLayout();
            this.logP.SuspendLayout();
            this.optionButtonP.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "userTalking.bmp");
            this.imageList1.Images.SetKeyName(1, "userNotTalking.bmp");
            this.imageList1.Images.SetKeyName(2, "channel.bmp");
            this.imageList1.Images.SetKeyName(3, "voice1.ico");
            this.imageList1.Images.SetKeyName(4, "voice2.ico");
            this.imageList1.Images.SetKeyName(5, "voice3.ico");
            this.imageList1.Images.SetKeyName(6, "voice4.ico");
            this.imageList1.Images.SetKeyName(7, "voice5.ico");
            this.imageList1.Images.SetKeyName(8, "voice6.ico");
            this.imageList1.Images.SetKeyName(9, "voice7.ico");
            this.imageList1.Images.SetKeyName(10, "voice8.ico");
            // 
            // voiceactTrackBar
            // 
            this.voiceactTrackBar.Location = new System.Drawing.Point(81, 17);
            this.voiceactTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.voiceactTrackBar.Maximum = 20;
            this.voiceactTrackBar.Minimum = 1;
            this.voiceactTrackBar.Value = 1;
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
            this.groupBox2.Location = new System.Drawing.Point(6, 94);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            // vumeterProgressBar
            // 
            this.vumeterProgressBar.Location = new System.Drawing.Point(62, 63);
            this.vumeterProgressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.groupBox5.Location = new System.Drawing.Point(6, 2);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Size = new System.Drawing.Size(274, 90);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sound settings";
            // 
            // mastervolTrackBar
            // 
            this.mastervolTrackBar.Location = new System.Drawing.Point(62, 24);
            this.mastervolTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mastervolTrackBar.Maximum = 255;
            this.mastervolTrackBar.Name = "mastervolTrackBar";
            this.mastervolTrackBar.Size = new System.Drawing.Size(196, 45);
            this.mastervolTrackBar.TabIndex = 3;
            this.mastervolTrackBar.TickFrequency = 20;
            this.mastervolTrackBar.Value = 255;
            this.mastervolTrackBar.Visible = false;
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
            this.label4.Visible = false;
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBoxLog);
            this.groupBox3.Location = new System.Drawing.Point(6, 2);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(370, 64);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.CausesValidation = false;
            this.richTextBoxLog.Location = new System.Drawing.Point(5, 17);
            this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(357, 45);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // voiceSettingsP
            // 
            this.voiceSettingsP.Controls.Add(this.groupBox5);
            this.voiceSettingsP.Controls.Add(this.groupBox2);
            this.voiceSettingsP.Dock = System.Windows.Forms.DockStyle.Top;
            this.voiceSettingsP.Location = new System.Drawing.Point(0, 107);
            this.voiceSettingsP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.voiceSettingsP.Name = "voiceSettingsP";
            this.voiceSettingsP.Size = new System.Drawing.Size(364, 196);
            this.voiceSettingsP.TabIndex = 22;
            // 
            // logP
            // 
            this.logP.Controls.Add(this.groupBox3);
            this.logP.Dock = System.Windows.Forms.DockStyle.Top;
            this.logP.Location = new System.Drawing.Point(0, 35);
            this.logP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logP.Name = "logP";
            this.logP.Size = new System.Drawing.Size(364, 72);
            this.logP.TabIndex = 24;
            // 
            // optionButtonP
            // 
            this.optionButtonP.Controls.Add(this.voiceOptionsB);
            this.optionButtonP.Dock = System.Windows.Forms.DockStyle.Top;
            this.optionButtonP.Location = new System.Drawing.Point(0, 0);
            this.optionButtonP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.optionButtonP.Name = "optionButtonP";
            this.optionButtonP.Size = new System.Drawing.Size(364, 35);
            this.optionButtonP.TabIndex = 25;
            // 
            // voiceOptionsB
            // 
            this.voiceOptionsB.Location = new System.Drawing.Point(11, 11);
            this.voiceOptionsB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.voiceOptionsB.Name = "voiceOptionsB";
            this.voiceOptionsB.Size = new System.Drawing.Size(88, 19);
            this.voiceOptionsB.TabIndex = 0;
            this.voiceOptionsB.Text = "Show Options";
            this.voiceOptionsB.UseVisualStyleBackColor = true;
            this.voiceOptionsB.Click += new System.EventHandler(this.voiceOptionsB_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.channelsTreeView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(247, 436);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // channelsTreeView
            // 
            this.channelsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelsTreeView.ImageIndex = 0;
            this.channelsTreeView.ImageList = this.imageList1;
            this.channelsTreeView.Location = new System.Drawing.Point(9, 14);
            this.channelsTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.channelsTreeView.Name = "channelsTreeView";
            this.channelsTreeView.SelectedImageIndex = 0;
            this.channelsTreeView.ShowNodeToolTips = true;
            this.channelsTreeView.Size = new System.Drawing.Size(231, 413);
            this.channelsTreeView.TabIndex = 2;
            this.channelsTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.channelsTreeView_NodeMouseDoubleClick);
            this.channelsTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.channelsTreeView_BeforeExpand);
            this.channelsTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.channelsTreeView_BeforeCollapse);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.voiceSettingsP);
            this.splitContainer1.Panel2.Controls.Add(this.logP);
            this.splitContainer1.Panel2.Controls.Add(this.optionButtonP);
            this.splitContainer1.Size = new System.Drawing.Size(614, 436);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 24;
            // 
            // VoIPClientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "VoIPClientControl";
            this.Size = new System.Drawing.Size(614, 436);
            this.Load += new System.EventHandler(this.VoIPClientControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.voiceactTrackBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mastervolTrackBar)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.voiceSettingsP.ResumeLayout(false);
            this.logP.ResumeLayout(false);
            this.optionButtonP.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TrackBar voiceactTrackBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar vumeterProgressBar;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar mastervolTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.RadioButton voiceActRadio;
        private System.Windows.Forms.RadioButton pushToTalkRadio;
        private System.Windows.Forms.Panel voiceSettingsP;
        private System.Windows.Forms.Panel logP;
        private System.Windows.Forms.Panel optionButtonP;
        private System.Windows.Forms.Button voiceOptionsB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView channelsTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
