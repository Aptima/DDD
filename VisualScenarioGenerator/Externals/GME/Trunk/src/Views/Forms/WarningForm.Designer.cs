namespace Forms
{
    partial class WarningForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningForm));
            this.warninglabel = new System.Windows.Forms.Label();
            this.okbutton = new System.Windows.Forms.Button();
            this.nobutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // warninglabel
            // 
            this.warninglabel.AutoSize = true;
            this.warninglabel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warninglabel.Location = new System.Drawing.Point(25, 20);
            this.warninglabel.Name = "warninglabel";
            this.warninglabel.Size = new System.Drawing.Size(49, 14);
            this.warninglabel.TabIndex = 0;
            this.warninglabel.Text = "label1";
            this.warninglabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okbutton.Location = new System.Drawing.Point(10, 46);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(75, 23);
            this.okbutton.TabIndex = 1;
            this.okbutton.Text = "Yes";
            this.okbutton.UseVisualStyleBackColor = true;
            // 
            // nobutton
            // 
            this.nobutton.Location = new System.Drawing.Point(113, 46);
            this.nobutton.Name = "nobutton";
            this.nobutton.Size = new System.Drawing.Size(75, 23);
            this.nobutton.TabIndex = 2;
            this.nobutton.Text = "No";
            this.nobutton.UseVisualStyleBackColor = true;
            // 
            // WarningForm
            // 
            this.AcceptButton = this.nobutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.okbutton;
            this.ClientSize = new System.Drawing.Size(199, 82);
            this.Controls.Add(this.nobutton);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.warninglabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WarningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WarningForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label warninglabel;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Button nobutton;
    }
}