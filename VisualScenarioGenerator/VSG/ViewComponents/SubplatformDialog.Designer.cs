namespace VSG.ViewComponents
{
    partial class SubplatformDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.kindLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(321, 289);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // kindLinkBox
            // 
            this.kindLinkBox.CheckLinkLevel = ((uint)(1u));
            this.kindLinkBox.ConnectFromId = -1;
            this.kindLinkBox.ConnectLinkType = null;
            this.kindLinkBox.ConnectRootId = -1;
            this.kindLinkBox.Controller = null;
            this.kindLinkBox.DisplayComponentType = null;
            this.kindLinkBox.DisplayLinkType = null;
            this.kindLinkBox.DisplayRootId = -1;
            this.kindLinkBox.FormattingEnabled = true;
            this.kindLinkBox.Location = new System.Drawing.Point(12, 37);
            this.kindLinkBox.Name = "kindLinkBox";
            this.kindLinkBox.OneToMany = false;
            this.kindLinkBox.Size = new System.Drawing.Size(120, 89);
            this.kindLinkBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Kind";
            // 
            // SubplatformDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 324);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kindLinkBox);
            this.Controls.Add(this.okButton);
            this.Name = "SubplatformDialog";
            this.Text = "Define Subplatform";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private AME.Views.View_Components.CustomLinkBox kindLinkBox;
        private System.Windows.Forms.Label label1;
    }
}