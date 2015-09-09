namespace VSG.ViewComponents
{
    partial class EvtPnl_VoiceChannelOpen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.voiceChannelName = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.accessLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Voice Channel Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time (s.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Voice Channel Access:";
            // 
            // voiceChannelName
            // 
            this.voiceChannelName.ComponentId = 0;
            this.voiceChannelName.Controller = null;
            this.voiceChannelName.Location = new System.Drawing.Point(116, 5);
            this.voiceChannelName.Margin = new System.Windows.Forms.Padding(2);
            this.voiceChannelName.Name = "voiceChannelName";
            this.voiceChannelName.ParameterCategory = "OpenVoiceChannel";
            this.voiceChannelName.ParameterName = "Name";
            this.voiceChannelName.ParameterType = AME.Controllers.eParamParentType.Component;
            this.voiceChannelName.Size = new System.Drawing.Size(97, 20);
            this.voiceChannelName.TabIndex = 1;
            this.voiceChannelName.UseDelimiter = false;
            this.voiceChannelName.TextChanged += new System.EventHandler(this.voiceChannelName_TextChanged);
            // 
            // accessLinkBox
            // 
            this.accessLinkBox.CheckLinkLevel = ((uint)(1u));
            this.accessLinkBox.CheckOnClick = true;
            this.accessLinkBox.ConnectFromId = -1;
            this.accessLinkBox.ConnectLinkType = null;
            this.accessLinkBox.ConnectRootId = -1;
            this.accessLinkBox.Controller = null;
            this.accessLinkBox.DisplayComponentType = null;
            this.accessLinkBox.DisplayLinkType = null;
            this.accessLinkBox.DisplayParameterCategory = "";
            this.accessLinkBox.DisplayParameterName = "";
            this.accessLinkBox.DisplayRecursiveCheck = false;
            this.accessLinkBox.DisplayRootId = -1;
            this.accessLinkBox.FilterResult = false;
            this.accessLinkBox.FormattingEnabled = true;
            this.accessLinkBox.Location = new System.Drawing.Point(5, 72);
            this.accessLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.accessLinkBox.Name = "accessLinkBox";
            this.accessLinkBox.OneToMany = false;
            this.accessLinkBox.ParameterFilterCategory = "";
            this.accessLinkBox.ParameterFilterName = "";
            this.accessLinkBox.ParameterFilterValue = "";
            this.accessLinkBox.Size = new System.Drawing.Size(208, 124);
            this.accessLinkBox.TabIndex = 3;
            this.accessLinkBox.SelectedIndexChanged += new System.EventHandler(this.accessLinkBox_SelectedIndexChanged);
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(116, 28);
            this.timeUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.timeUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeUpDown.Name = "timeUpDown";
            this.timeUpDown.ParameterCategory = "OpenVoiceChannel";
            this.timeUpDown.ParameterName = "Time";
            this.timeUpDown.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeUpDown.Size = new System.Drawing.Size(97, 20);
            this.timeUpDown.TabIndex = 2;
            this.timeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeUpDown.ValueChanged += new System.EventHandler(this.timeUpDown_ValueChanged);
            // 
            // EvtPnl_VoiceChannelOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accessLinkBox);
            this.Controls.Add(this.voiceChannelName);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(203, 214);
            this.Name = "EvtPnl_VoiceChannelOpen";
            this.Size = new System.Drawing.Size(225, 214);
            this.Load += new System.EventHandler(this.EvtPnl_VoiceChannelOpen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private AME.Views.View_Components.CustomParameterTextBox voiceChannelName;
        private AME.Views.View_Components.CustomLinkBox accessLinkBox;

    }
}
