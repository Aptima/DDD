namespace VSG.Dialogs
{
    partial class AboutDialog
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
            this.labelProductInfo = new System.Windows.Forms.Label();
            this.labelCompileDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelAsimSite = new System.Windows.Forms.LinkLabel();
            this.linkLabelNoticeFile = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelProductInfo
            // 
            this.labelProductInfo.AutoSize = true;
            this.labelProductInfo.Location = new System.Drawing.Point(12, 9);
            this.labelProductInfo.Name = "labelProductInfo";
            this.labelProductInfo.Size = new System.Drawing.Size(66, 13);
            this.labelProductInfo.TabIndex = 0;
            this.labelProductInfo.Text = "[PRODUCT]";
            // 
            // labelCompileDate
            // 
            this.labelCompileDate.AutoSize = true;
            this.labelCompileDate.Location = new System.Drawing.Point(12, 36);
            this.labelCompileDate.Name = "labelCompileDate";
            this.labelCompileDate.Size = new System.Drawing.Size(68, 13);
            this.labelCompileDate.TabIndex = 1;
            this.labelCompileDate.Text = "[COMPILED]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "For more info: ";
            // 
            // linkLabelAsimSite
            // 
            this.linkLabelAsimSite.AutoSize = true;
            this.linkLabelAsimSite.Location = new System.Drawing.Point(92, 63);
            this.linkLabelAsimSite.Name = "linkLabelAsimSite";
            this.linkLabelAsimSite.Size = new System.Drawing.Size(53, 13);
            this.linkLabelAsimSite.TabIndex = 4;
            this.linkLabelAsimSite.TabStop = true;
            this.linkLabelAsimSite.Text = "[APTIMA]";
            this.linkLabelAsimSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAsimSite_LinkClicked);
            // 
            // linkLabelNoticeFile
            // 
            this.linkLabelNoticeFile.AutoSize = true;
            this.linkLabelNoticeFile.Location = new System.Drawing.Point(12, 90);
            this.linkLabelNoticeFile.Name = "linkLabelNoticeFile";
            this.linkLabelNoticeFile.Size = new System.Drawing.Size(53, 13);
            this.linkLabelNoticeFile.TabIndex = 5;
            this.linkLabelNoticeFile.TabStop = true;
            this.linkLabelNoticeFile.Text = "[NOTICE]";
            this.linkLabelNoticeFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNoticeFile_LinkClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(181, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 151);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.linkLabelNoticeFile);
            this.Controls.Add(this.linkLabelAsimSite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelCompileDate);
            this.Controls.Add(this.labelProductInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProductInfo;
        private System.Windows.Forms.Label labelCompileDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabelAsimSite;
        private System.Windows.Forms.LinkLabel linkLabelNoticeFile;
        private System.Windows.Forms.Button button1;
    }
}