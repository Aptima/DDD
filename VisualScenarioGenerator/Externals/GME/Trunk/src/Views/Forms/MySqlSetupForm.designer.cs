namespace AME.Views.Forms
{
    partial class MySqlSetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MySqlSetupForm));
            this.gbConfigureDatabase = new System.Windows.Forms.GroupBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.lDatabase = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lUsername = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lPort = new System.Windows.Forms.Label();
            this.tbServerHost = new System.Windows.Forms.TextBox();
            this.lServerHost = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bAccept = new System.Windows.Forms.Button();
            this.gbConfigureDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfigureDatabase
            // 
            this.gbConfigureDatabase.Controls.Add(this.tbDatabase);
            this.gbConfigureDatabase.Controls.Add(this.lDatabase);
            this.gbConfigureDatabase.Controls.Add(this.tbPassword);
            this.gbConfigureDatabase.Controls.Add(this.lPassword);
            this.gbConfigureDatabase.Controls.Add(this.tbUsername);
            this.gbConfigureDatabase.Controls.Add(this.lUsername);
            this.gbConfigureDatabase.Controls.Add(this.tbPort);
            this.gbConfigureDatabase.Controls.Add(this.lPort);
            this.gbConfigureDatabase.Controls.Add(this.tbServerHost);
            this.gbConfigureDatabase.Controls.Add(this.lServerHost);
            this.gbConfigureDatabase.Location = new System.Drawing.Point(12, 12);
            this.gbConfigureDatabase.Name = "gbConfigureDatabase";
            this.gbConfigureDatabase.Size = new System.Drawing.Size(280, 124);
            this.gbConfigureDatabase.TabIndex = 0;
            this.gbConfigureDatabase.TabStop = false;
            this.gbConfigureDatabase.Text = "Database Information:";
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(78, 45);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(100, 20);
            this.tbDatabase.TabIndex = 9;
            this.tbDatabase.Text = "gme";
            // 
            // lDatabase
            // 
            this.lDatabase.Location = new System.Drawing.Point(6, 48);
            this.lDatabase.Name = "lDatabase";
            this.lDatabase.Size = new System.Drawing.Size(66, 13);
            this.lDatabase.TabIndex = 8;
            this.lDatabase.Text = "Database:";
            this.lDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(78, 97);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 7;
            // 
            // lPassword
            // 
            this.lPassword.Location = new System.Drawing.Point(6, 100);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(66, 13);
            this.lPassword.TabIndex = 6;
            this.lPassword.Text = "Password:";
            this.lPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(78, 71);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(100, 20);
            this.tbUsername.TabIndex = 5;
            this.tbUsername.Text = "root";
            // 
            // lUsername
            // 
            this.lUsername.Location = new System.Drawing.Point(6, 74);
            this.lUsername.Name = "lUsername";
            this.lUsername.Size = new System.Drawing.Size(66, 13);
            this.lUsername.TabIndex = 4;
            this.lUsername.Text = "Username:";
            this.lUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(217, 19);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(57, 20);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "3306";
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(184, 22);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(29, 13);
            this.lPort.TabIndex = 2;
            this.lPort.Text = "Port:";
            // 
            // tbServerHost
            // 
            this.tbServerHost.Location = new System.Drawing.Point(78, 19);
            this.tbServerHost.Name = "tbServerHost";
            this.tbServerHost.Size = new System.Drawing.Size(100, 20);
            this.tbServerHost.TabIndex = 1;
            this.tbServerHost.Text = "localhost";
            // 
            // lServerHost
            // 
            this.lServerHost.AutoSize = true;
            this.lServerHost.Location = new System.Drawing.Point(6, 22);
            this.lServerHost.Name = "lServerHost";
            this.lServerHost.Size = new System.Drawing.Size(66, 13);
            this.lServerHost.TabIndex = 0;
            this.lServerHost.Text = "Server Host:";
            this.lServerHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bAccept
            // 
            this.bAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAccept.Location = new System.Drawing.Point(217, 143);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(75, 23);
            this.bAccept.TabIndex = 2;
            this.bAccept.Text = "OK";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // SetupForm
            // 
            this.AcceptButton = this.bAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 178);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.gbConfigureDatabase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.Text = "Select database to access";
            this.Activated += new System.EventHandler(this.SetupForm_Activated);
            this.gbConfigureDatabase.ResumeLayout(false);
            this.gbConfigureDatabase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfigureDatabase;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.TextBox tbServerHost;
        private System.Windows.Forms.Label lServerHost;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lUsername;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.Label lDatabase;
    }
}