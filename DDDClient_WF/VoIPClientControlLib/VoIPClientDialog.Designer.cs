namespace Aptima.Asim.DDD.Client
{
    partial class VoIPClientDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.voIPClientControl1 = new Aptima.Asim.DDD.Client.VoIPClientControl();
            this.SuspendLayout();
            // 
            // voIPClientControl1
            // 
            this.voIPClientControl1.Location = new System.Drawing.Point(10, 11);
            this.voIPClientControl1.Name = "voIPClientControl1";
            this.voIPClientControl1.Size = new System.Drawing.Size(465, 393);
            this.voIPClientControl1.TabIndex = 0;
            this.voIPClientControl1.Load += new System.EventHandler(this.voIPClientControl1_Load);
            // 
            // VoIPClientDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 408);
            this.ControlBox = false;
            this.Controls.Add(this.voIPClientControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VoIPClientDialog";
            this.Text = "VoIP Client";
            this.ResumeLayout(false);

        }

        #endregion

        private VoIPClientControl voIPClientControl1;

    }
}