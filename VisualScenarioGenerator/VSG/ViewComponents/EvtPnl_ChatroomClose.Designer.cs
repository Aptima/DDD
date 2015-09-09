namespace VSG.ViewComponents
{
    partial class EvtPnl_ChatroomClose
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
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Time (s.):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Close Room Name:";
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = "CloseChatRoom";
            this.customLinkBox1.ConnectRootId = -1;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = "OpenChatRoomEvent";
            this.customLinkBox1.DisplayLinkType = "Scenario";
            this.customLinkBox1.DisplayParameterCategory = "OpenChatRoom";
            this.customLinkBox1.DisplayParameterName = "Name";
            this.customLinkBox1.DisplayRecursiveCheck = false;
            this.customLinkBox1.DisplayRootId = -1;
            this.customLinkBox1.FilterResult = false;
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.Location = new System.Drawing.Point(103, 3);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = false;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(203, 34);
            this.customLinkBox1.TabIndex = 1;
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
            this.timeUpDown.ParameterCategory = "CloseChatRoom";
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
            // EvtPnl_ChatroomClose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customLinkBox1);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(225, 61);
            this.Name = "EvtPnl_ChatroomClose";
            this.Size = new System.Drawing.Size(326, 97);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;

    }
}
