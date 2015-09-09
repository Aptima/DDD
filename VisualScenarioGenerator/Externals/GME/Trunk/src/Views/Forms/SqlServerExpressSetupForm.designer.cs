namespace AME.Views.Forms
{
    partial class SqlServerExpressSetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerExpressSetupForm));
            this.gbConfigureDatabase = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.lDatabase = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lUsername = new System.Windows.Forms.Label();
            this.tbServerHost = new System.Windows.Forms.TextBox();
            this.lServerHost = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bAccept = new System.Windows.Forms.Button();
            this.gbConfigureDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfigureDatabase
            // 
            this.gbConfigureDatabase.Controls.Add(this.label1);
            this.gbConfigureDatabase.Controls.Add(this.comboBox1);
            this.gbConfigureDatabase.Controls.Add(this.tbDatabase);
            this.gbConfigureDatabase.Controls.Add(this.lDatabase);
            this.gbConfigureDatabase.Controls.Add(this.tbPassword);
            this.gbConfigureDatabase.Controls.Add(this.lPassword);
            this.gbConfigureDatabase.Controls.Add(this.tbUsername);
            this.gbConfigureDatabase.Controls.Add(this.lUsername);
            this.gbConfigureDatabase.Controls.Add(this.tbServerHost);
            this.gbConfigureDatabase.Controls.Add(this.lServerHost);
            this.gbConfigureDatabase.Location = new System.Drawing.Point(12, 12);
            this.gbConfigureDatabase.Name = "gbConfigureDatabase";
            this.gbConfigureDatabase.Size = new System.Drawing.Size(280, 150);
            this.gbConfigureDatabase.TabIndex = 0;
            this.gbConfigureDatabase.TabStop = false;
            this.gbConfigureDatabase.Text = "Database Information:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Authentication:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(90, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(184, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(90, 45);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(184, 20);
            this.tbDatabase.TabIndex = 1;
            // 
            // lDatabase
            // 
            this.lDatabase.Location = new System.Drawing.Point(18, 48);
            this.lDatabase.Name = "lDatabase";
            this.lDatabase.Size = new System.Drawing.Size(66, 13);
            this.lDatabase.TabIndex = 8;
            this.lDatabase.Text = "Database:";
            this.lDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(90, 124);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(184, 20);
            this.tbPassword.TabIndex = 4;
            // 
            // lPassword
            // 
            this.lPassword.Location = new System.Drawing.Point(18, 127);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(66, 13);
            this.lPassword.TabIndex = 6;
            this.lPassword.Text = "Password:";
            this.lPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(90, 98);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(184, 20);
            this.tbUsername.TabIndex = 3;
            // 
            // lUsername
            // 
            this.lUsername.Location = new System.Drawing.Point(18, 101);
            this.lUsername.Name = "lUsername";
            this.lUsername.Size = new System.Drawing.Size(66, 13);
            this.lUsername.TabIndex = 4;
            this.lUsername.Text = "Username:";
            this.lUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbServerHost
            // 
            this.tbServerHost.Location = new System.Drawing.Point(90, 19);
            this.tbServerHost.Name = "tbServerHost";
            this.tbServerHost.Size = new System.Drawing.Size(184, 20);
            this.tbServerHost.TabIndex = 0;
            // 
            // lServerHost
            // 
            this.lServerHost.AutoSize = true;
            this.lServerHost.Location = new System.Drawing.Point(18, 22);
            this.lServerHost.Name = "lServerHost";
            this.lServerHost.Size = new System.Drawing.Size(66, 13);
            this.lServerHost.TabIndex = 0;
            this.lServerHost.Text = "Server Host:";
            this.lServerHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bAccept
            // 
            this.bAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAccept.Location = new System.Drawing.Point(217, 168);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(75, 23);
            this.bAccept.TabIndex = 5;
            this.bAccept.Text = "OK";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // SqlServerExpressSetupForm
            // 
            this.AcceptButton = this.bAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 203);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.gbConfigureDatabase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlServerExpressSetupForm";
            this.Text = "Select database to access";
            this.Load += new System.EventHandler(this.SqlServerExpressSetupForm_Load);
            this.Activated += new System.EventHandler(this.SqlServerExpressSetupForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SqlServerExpressSetupForm_FormClosed);
            this.gbConfigureDatabase.ResumeLayout(false);
            this.gbConfigureDatabase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfigureDatabase;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}