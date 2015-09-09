namespace VSG.ViewComponents
{
    partial class EvtPnl_SendChatMessage
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chatRoomLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.senderDMLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.messageTextCustomParameterTextBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Message Time (s.):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Chat Room:";
            // 
            // chatRoomLinkBox
            // 
            this.chatRoomLinkBox.CheckLinkLevel = ((uint)(1u));
            this.chatRoomLinkBox.CheckOnClick = true;
            this.chatRoomLinkBox.ConnectFromId = -1;
            this.chatRoomLinkBox.ConnectLinkType = "SendChatMessageRoom";
            this.chatRoomLinkBox.ConnectRootId = -1;
            this.chatRoomLinkBox.Controller = null;
            this.chatRoomLinkBox.DisplayComponentType = "OpenChatRoomEvent";
            this.chatRoomLinkBox.DisplayLinkType = "Scenario";
            this.chatRoomLinkBox.DisplayParameterCategory = "OpenChatRoom";
            this.chatRoomLinkBox.DisplayParameterName = "Name";
            this.chatRoomLinkBox.DisplayRecursiveCheck = false;
            this.chatRoomLinkBox.DisplayRootId = -1;
            this.chatRoomLinkBox.FilterResult = false;
            this.chatRoomLinkBox.FormattingEnabled = true;
            this.chatRoomLinkBox.Location = new System.Drawing.Point(103, 3);
            this.chatRoomLinkBox.Name = "chatRoomLinkBox";
            this.chatRoomLinkBox.OneToMany = false;
            this.chatRoomLinkBox.ParameterFilterCategory = "";
            this.chatRoomLinkBox.ParameterFilterName = "";
            this.chatRoomLinkBox.ParameterFilterValue = "";
            this.chatRoomLinkBox.Size = new System.Drawing.Size(203, 34);
            this.chatRoomLinkBox.TabIndex = 1;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(103, 57);
            this.timeUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.timeUpDown.ParameterCategory = "SendChatMessage";
            this.timeUpDown.ParameterName = "Time";
            this.timeUpDown.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeUpDown.Size = new System.Drawing.Size(91, 20);
            this.timeUpDown.TabIndex = 2;
            this.timeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 87);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Message Text:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Sending DM:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.senderDMLinkBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 129);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(254, 85);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional";
            // 
            // senderDMLinkBox
            // 
            this.senderDMLinkBox.CheckLinkLevel = ((uint)(1u));
            this.senderDMLinkBox.CheckOnClick = true;
            this.senderDMLinkBox.ConnectFromId = -1;
            this.senderDMLinkBox.ConnectLinkType = "SendChatMessageSenderDM";
            this.senderDMLinkBox.ConnectRootId = -1;
            this.senderDMLinkBox.Controller = null;
            this.senderDMLinkBox.DisplayComponentType = "DecisionMaker";
            this.senderDMLinkBox.DisplayLinkType = "Scenario";
            this.senderDMLinkBox.DisplayParameterCategory = "";
            this.senderDMLinkBox.DisplayParameterName = "";
            this.senderDMLinkBox.DisplayRecursiveCheck = false;
            this.senderDMLinkBox.DisplayRootId = -1;
            this.senderDMLinkBox.FilterResult = false;
            this.senderDMLinkBox.FormattingEnabled = true;
            this.senderDMLinkBox.Location = new System.Drawing.Point(99, 18);
            this.senderDMLinkBox.Name = "senderDMLinkBox";
            this.senderDMLinkBox.OneToMany = false;
            this.senderDMLinkBox.ParameterFilterCategory = "";
            this.senderDMLinkBox.ParameterFilterName = "";
            this.senderDMLinkBox.ParameterFilterValue = "";
            this.senderDMLinkBox.Size = new System.Drawing.Size(150, 49);
            this.senderDMLinkBox.TabIndex = 4;
            // 
            // messageTextCustomParameterTextBox
            // 
            this.messageTextCustomParameterTextBox.ComponentId = -1;
            this.messageTextCustomParameterTextBox.Controller = null;
            this.messageTextCustomParameterTextBox.Location = new System.Drawing.Point(103, 87);
            this.messageTextCustomParameterTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.messageTextCustomParameterTextBox.Multiline = true;
            this.messageTextCustomParameterTextBox.Name = "messageTextCustomParameterTextBox";
            this.messageTextCustomParameterTextBox.ParameterCategory = "SendChatMessage";
            this.messageTextCustomParameterTextBox.ParameterName = "Message";
            this.messageTextCustomParameterTextBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.messageTextCustomParameterTextBox.Size = new System.Drawing.Size(203, 38);
            this.messageTextCustomParameterTextBox.TabIndex = 3;
            this.messageTextCustomParameterTextBox.UseDelimiter = false;
            // 
            // EvtPnl_SendChatMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.messageTextCustomParameterTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chatRoomLinkBox);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(225, 61);
            this.Name = "EvtPnl_SendChatMessage";
            this.Size = new System.Drawing.Size(310, 227);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox chatRoomLinkBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private AME.Views.View_Components.CustomLinkBox senderDMLinkBox;
        private AME.Views.View_Components.CustomParameterTextBox messageTextCustomParameterTextBox;

    }
}
