namespace Aptima.Asim.DDD.TestStubs.TextChatTestGUI
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
            this.textBoxChatWindow = new System.Windows.Forms.TextBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBoxChatWindow
            // 
            this.textBoxChatWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChatWindow.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxChatWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChatWindow.Location = new System.Drawing.Point(13, 13);
            this.textBoxChatWindow.Multiline = true;
            this.textBoxChatWindow.Name = "textBoxChatWindow";
            this.textBoxChatWindow.ReadOnly = true;
            this.textBoxChatWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChatWindow.Size = new System.Drawing.Size(646, 151);
            this.textBoxChatWindow.TabIndex = 0;
            this.textBoxChatWindow.TabStop = false;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage.Location = new System.Drawing.Point(8, 176);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(559, 22);
            this.textBoxMessage.TabIndex = 0;
            this.textBoxMessage.WordWrap = false;
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendMessage.Location = new System.Drawing.Point(574, 170);
            this.buttonSendMessage.MaximumSize = new System.Drawing.Size(85, 28);
            this.buttonSendMessage.MinimumSize = new System.Drawing.Size(85, 28);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(85, 28);
            this.buttonSendMessage.TabIndex = 1;
            this.buttonSendMessage.Text = "SEND";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonSendMessage;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 208);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.textBoxChatWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DDD Text Chat Test GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxChatWindow;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Timer timer1;
    }
}

