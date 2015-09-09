namespace VSG.ViewComponents
{
    partial class EvtPnl_SendVoiceMessageToUser
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
            this.label3 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileNameCustomParameterTextBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.customLinkBoxTargetUser = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
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
            this.label1.Location = new System.Drawing.Point(20, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Send to User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 87);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "File Name:";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(236, 86);
            this.browseButton.Margin = new System.Windows.Forms.Padding(2);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(56, 19);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // fileNameCustomParameterTextBox
            // 
            this.fileNameCustomParameterTextBox.ComponentId = -1;
            this.fileNameCustomParameterTextBox.Controller = null;
            this.fileNameCustomParameterTextBox.Location = new System.Drawing.Point(103, 87);
            this.fileNameCustomParameterTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.fileNameCustomParameterTextBox.Name = "fileNameCustomParameterTextBox";
            this.fileNameCustomParameterTextBox.ParameterCategory = "SendVoiceMessageToUser";
            this.fileNameCustomParameterTextBox.ParameterName = "FilePath";
            this.fileNameCustomParameterTextBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.fileNameCustomParameterTextBox.ReadOnly = true;
            this.fileNameCustomParameterTextBox.Size = new System.Drawing.Size(129, 20);
            this.fileNameCustomParameterTextBox.TabIndex = 3;
            this.fileNameCustomParameterTextBox.UseDelimiter = false;
            // 
            // customLinkBoxTargetUser
            // 
            this.customLinkBoxTargetUser.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxTargetUser.CheckOnClick = true;
            this.customLinkBoxTargetUser.ConnectFromId = -1;
            this.customLinkBoxTargetUser.ConnectLinkType = "SendVoiceMessageToUserChannel";
            this.customLinkBoxTargetUser.ConnectRootId = 0;
            this.customLinkBoxTargetUser.Controller = null;
            this.customLinkBoxTargetUser.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxTargetUser.DisplayLinkType = "Scenario";
            this.customLinkBoxTargetUser.DisplayParameterCategory = "";
            this.customLinkBoxTargetUser.DisplayParameterName = "";
            this.customLinkBoxTargetUser.DisplayRecursiveCheck = false;
            this.customLinkBoxTargetUser.DisplayRootId = 0;
            this.customLinkBoxTargetUser.FilterResult = false;
            this.customLinkBoxTargetUser.FormattingEnabled = true;
            this.customLinkBoxTargetUser.Location = new System.Drawing.Point(103, 3);
            this.customLinkBoxTargetUser.Name = "customLinkBoxTargetUser";
            this.customLinkBoxTargetUser.OneToMany = false;
            this.customLinkBoxTargetUser.ParameterFilterCategory = "";
            this.customLinkBoxTargetUser.ParameterFilterName = "";
            this.customLinkBoxTargetUser.ParameterFilterValue = "";
            this.customLinkBoxTargetUser.Size = new System.Drawing.Size(203, 34);
            this.customLinkBoxTargetUser.TabIndex = 1;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(103, 57);
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
            this.timeUpDown.ParameterCategory = "SendVoiceMessageToUser";
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
            // EvtPnl_SendVoiceMessageToUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.fileNameCustomParameterTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.customLinkBoxTargetUser);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(225, 61);
            this.Name = "EvtPnl_SendVoiceMessageToUser";
            this.Size = new System.Drawing.Size(310, 116);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxTargetUser;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomParameterTextBox fileNameCustomParameterTextBox;
        private System.Windows.Forms.Button browseButton;

    }
}
