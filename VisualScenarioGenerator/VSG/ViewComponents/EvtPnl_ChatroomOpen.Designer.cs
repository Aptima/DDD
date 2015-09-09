namespace VSG.ViewComponents
{
    partial class EvtPnl_ChatroomOpen
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
            this.chatRoomName = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.membersLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
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
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chat Room Name:";
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
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chat Room Members:";
            // 
            // chatRoomName
            // 
            this.chatRoomName.ComponentId = 0;
            this.chatRoomName.Controller = null;
            this.chatRoomName.Location = new System.Drawing.Point(100, 5);
            this.chatRoomName.Margin = new System.Windows.Forms.Padding(2);
            this.chatRoomName.Name = "chatRoomName";
            this.chatRoomName.ParameterCategory = "OpenChatRoom";
            this.chatRoomName.ParameterName = "Name";
            this.chatRoomName.ParameterType = AME.Controllers.eParamParentType.Component;
            this.chatRoomName.Size = new System.Drawing.Size(91, 20);
            this.chatRoomName.TabIndex = 1;
            this.chatRoomName.UseDelimiter = false;
            // 
            // membersLinkBox
            // 
            this.membersLinkBox.CheckLinkLevel = ((uint)(1u));
            this.membersLinkBox.CheckOnClick = true;
            this.membersLinkBox.ConnectFromId = -1;
            this.membersLinkBox.ConnectLinkType = null;
            this.membersLinkBox.ConnectRootId = -1;
            this.membersLinkBox.Controller = null;
            this.membersLinkBox.DisplayComponentType = null;
            this.membersLinkBox.DisplayLinkType = null;
            this.membersLinkBox.DisplayParameterCategory = "";
            this.membersLinkBox.DisplayParameterName = "";
            this.membersLinkBox.DisplayRecursiveCheck = false;
            this.membersLinkBox.DisplayRootId = -1;
            this.membersLinkBox.FilterResult = false;
            this.membersLinkBox.FormattingEnabled = true;
            this.membersLinkBox.Location = new System.Drawing.Point(5, 72);
            this.membersLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.membersLinkBox.Name = "membersLinkBox";
            this.membersLinkBox.OneToMany = false;
            this.membersLinkBox.ParameterFilterCategory = "";
            this.membersLinkBox.ParameterFilterName = "";
            this.membersLinkBox.ParameterFilterValue = "";
            this.membersLinkBox.Size = new System.Drawing.Size(186, 124);
            this.membersLinkBox.TabIndex = 3;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(100, 28);
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
            this.timeUpDown.ParameterCategory = "OpenChatRoom";
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
            // EvtPnl_ChatroomOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.membersLinkBox);
            this.Controls.Add(this.chatRoomName);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(203, 214);
            this.Name = "EvtPnl_ChatroomOpen";
            this.Size = new System.Drawing.Size(203, 214);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private AME.Views.View_Components.CustomParameterTextBox chatRoomName;
        private AME.Views.View_Components.CustomLinkBox membersLinkBox;

    }
}
