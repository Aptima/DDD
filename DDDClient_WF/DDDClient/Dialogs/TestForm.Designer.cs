namespace Aptima.Asim.DDD.Client.Dialogs
{
    partial class TestForm
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
            this.modifiableAttributePanel1 = new Aptima.Asim.DDD.Client.Controls.ModifiableAttributePanel();
            this.textBoxAttributeName = new System.Windows.Forms.TextBox();
            this.textBoxAttributeValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxModify = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // modifiableAttributePanel1
            // 
            this.modifiableAttributePanel1.Location = new System.Drawing.Point(12, 12);
            this.modifiableAttributePanel1.MinimumSize = new System.Drawing.Size(222, 197);
            this.modifiableAttributePanel1.Name = "modifiableAttributePanel1";
            this.modifiableAttributePanel1.Size = new System.Drawing.Size(222, 222);
            this.modifiableAttributePanel1.TabIndex = 0;
            // 
            // textBoxAttributeName
            // 
            this.textBoxAttributeName.Location = new System.Drawing.Point(71, 234);
            this.textBoxAttributeName.Name = "textBoxAttributeName";
            this.textBoxAttributeName.Size = new System.Drawing.Size(100, 20);
            this.textBoxAttributeName.TabIndex = 1;
            // 
            // textBoxAttributeValue
            // 
            this.textBoxAttributeValue.Location = new System.Drawing.Point(71, 260);
            this.textBoxAttributeValue.Name = "textBoxAttributeValue";
            this.textBoxAttributeValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxAttributeValue.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Attribute: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value:";
            // 
            // checkBoxModify
            // 
            this.checkBoxModify.AutoSize = true;
            this.checkBoxModify.Location = new System.Drawing.Point(6, 286);
            this.checkBoxModify.Name = "checkBoxModify";
            this.checkBoxModify.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxModify.Size = new System.Drawing.Size(80, 17);
            this.checkBoxModify.TabIndex = 6;
            this.checkBoxModify.Text = "Modifiable?";
            this.checkBoxModify.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 319);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxModify);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAttributeValue);
            this.Controls.Add(this.textBoxAttributeName);
            this.Controls.Add(this.modifiableAttributePanel1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Aptima.Asim.DDD.Client.Controls.ModifiableAttributePanel modifiableAttributePanel1;
        private System.Windows.Forms.TextBox textBoxAttributeName;
        private System.Windows.Forms.TextBox textBoxAttributeValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxModify;
        private System.Windows.Forms.Button button1;
    }
}