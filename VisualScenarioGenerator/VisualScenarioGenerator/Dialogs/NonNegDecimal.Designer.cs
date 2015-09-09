namespace VisualScenarioGenerator
{
    partial class NonNegDecimal
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
            this.txValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txValue
            // 
            this.txValue.AcceptsReturn = true;
            this.txValue.Location = new System.Drawing.Point(0, 0);
            this.txValue.Name = "txValue";
            this.txValue.Size = new System.Drawing.Size(53, 20);
            this.txValue.TabIndex = 0;
            this.txValue.Text = "0.0";
            this.txValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txValue_KeyPress);
            // 
            // NonNegDecimal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txValue);
            this.Name = "NonNegDecimal";
            this.Size = new System.Drawing.Size(53, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txValue;
    }
}
