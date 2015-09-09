namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_BalanceBoxes
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
            this.lbxCurrent = new System.Windows.Forms.ListBox();
            this.lbxAvailable = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbxCurrent
            // 
            this.lbxCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxCurrent.FormattingEnabled = true;
            this.lbxCurrent.Location = new System.Drawing.Point(260, 37);
            this.lbxCurrent.Name = "lbxCurrent";
            this.lbxCurrent.Size = new System.Drawing.Size(108, 93);
            this.lbxCurrent.TabIndex = 0;
            // 
            // lbxAvailable
            // 
            this.lbxAvailable.ForeColor = System.Drawing.Color.Gray;
            this.lbxAvailable.FormattingEnabled = true;
            this.lbxAvailable.Location = new System.Drawing.Point(16, 37);
            this.lbxAvailable.Name = "lbxAvailable";
            this.lbxAvailable.Size = new System.Drawing.Size(108, 95);
            this.lbxAvailable.TabIndex = 1;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(144, 86);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(93, 23);
            this.btnRemove.TabIndex = 46;
            this.btnRemove.Text = "Remove <<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(144, 56);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 23);
            this.btnAdd.TabIndex = 45;
            this.btnAdd.Text = "Add >>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(263, 19);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(69, 13);
            this.lblCurrent.TabIndex = 47;
            this.lblCurrent.Text = "Current Items";
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(19, 19);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(78, 13);
            this.lblAvailable.TabIndex = 48;
            this.lblAvailable.Text = "Available Items";
            // 
            // Ctl_BalanceBoxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbxAvailable);
            this.Controls.Add(this.lbxCurrent);
            this.Name = "Ctl_BalanceBoxes";
            this.Size = new System.Drawing.Size(391, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxCurrent;
        private System.Windows.Forms.ListBox lbxAvailable;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label lblAvailable;
    }
}
