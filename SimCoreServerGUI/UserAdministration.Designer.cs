namespace Aptima.Asim.DDD.SimCoreGUI
{
    partial class UserAdministration
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
            this.userList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.addUserButton = new System.Windows.Forms.Button();
            this.deleteUserButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.strongPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // userList
            // 
            this.userList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userList.FormattingEnabled = true;
            this.userList.Location = new System.Drawing.Point(11, 60);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(238, 199);
            this.userList.TabIndex = 0;
            this.userList.DoubleClick += new System.EventHandler(this.userList_DoubleClick);
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Users:";
            // 
            // addUserButton
            // 
            this.addUserButton.Location = new System.Drawing.Point(11, 265);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(69, 23);
            this.addUserButton.TabIndex = 6;
            this.addUserButton.Text = "New User";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // deleteUserButton
            // 
            this.deleteUserButton.Location = new System.Drawing.Point(180, 265);
            this.deleteUserButton.Name = "deleteUserButton";
            this.deleteUserButton.Size = new System.Drawing.Size(69, 23);
            this.deleteUserButton.TabIndex = 7;
            this.deleteUserButton.Text = "Delete";
            this.deleteUserButton.UseVisualStyleBackColor = true;
            this.deleteUserButton.Click += new System.EventHandler(this.deleteUserButton_Click);
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(152, 294);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(97, 23);
            this.doneButton.TabIndex = 8;
            this.doneButton.Text = "OK";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(95, 265);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(69, 23);
            this.editButton.TabIndex = 9;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // strongPasswordCheckBox
            // 
            this.strongPasswordCheckBox.AutoSize = true;
            this.strongPasswordCheckBox.Location = new System.Drawing.Point(15, 12);
            this.strongPasswordCheckBox.Name = "strongPasswordCheckBox";
            this.strongPasswordCheckBox.Size = new System.Drawing.Size(139, 17);
            this.strongPasswordCheckBox.TabIndex = 10;
            this.strongPasswordCheckBox.Text = "Use Strong Passwords?";
            this.strongPasswordCheckBox.UseVisualStyleBackColor = true;
            this.strongPasswordCheckBox.CheckedChanged += new System.EventHandler(this.strongPasswordCheckBox_CheckedChanged);
            // 
            // UserAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 321);
            this.Controls.Add(this.strongPasswordCheckBox);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.deleteUserButton);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userList);
            this.Name = "UserAdministration";
            this.Text = "User Administration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserAdministration_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Button deleteUserButton;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.CheckBox strongPasswordCheckBox;
    }
}