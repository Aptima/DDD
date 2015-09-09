namespace VisualScenarioGenerator.Dialogs
{
    partial class Dlg_Node_OpenChatRoomEvent
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
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.ctl_Node_OpenChatRoomEvent1 = new VisualScenarioGenerator.Dialogs.Ctl_Node_OpenChatRoomEvent();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(205, 172);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(124, 172);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 4;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // ctl_Node_OpenChatRoomEvent1
            // 
            this.ctl_Node_OpenChatRoomEvent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node_OpenChatRoomEvent1.Location = new System.Drawing.Point(0, 0);
            this.ctl_Node_OpenChatRoomEvent1.Name = "ctl_Node_OpenChatRoomEvent1";
            this.ctl_Node_OpenChatRoomEvent1.Size = new System.Drawing.Size(292, 150);
            this.ctl_Node_OpenChatRoomEvent1.TabIndex = 6;
            // 
            // Dlg_Node_OpenChatRoomEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 207);
            this.Controls.Add(this.ctl_Node_OpenChatRoomEvent1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Dlg_Node_OpenChatRoomEvent";
            this.Text = "Open Chat Room";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private Ctl_Node_OpenChatRoomEvent ctl_Node_OpenChatRoomEvent1;
    }
}