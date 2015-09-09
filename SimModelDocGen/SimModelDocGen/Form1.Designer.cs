namespace SimModelDocGen
{
    partial class Form1
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
            this.simModelTextBox = new System.Windows.Forms.TextBox();
            this.ouputTextBox = new System.Windows.Forms.TextBox();
            this.simModelLabel = new System.Windows.Forms.Label();
            this.ouputLabel = new System.Windows.Forms.Label();
            this.simModelBrowseButton = new System.Windows.Forms.Button();
            this.outputBrowseButton = new System.Windows.Forms.Button();
            this.generateDocButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // simModelTextBox
            // 
            this.simModelTextBox.Location = new System.Drawing.Point(108, 6);
            this.simModelTextBox.Name = "simModelTextBox";
            this.simModelTextBox.Size = new System.Drawing.Size(206, 20);
            this.simModelTextBox.TabIndex = 0;
            // 
            // ouputTextBox
            // 
            this.ouputTextBox.Location = new System.Drawing.Point(108, 32);
            this.ouputTextBox.Name = "ouputTextBox";
            this.ouputTextBox.Size = new System.Drawing.Size(206, 20);
            this.ouputTextBox.TabIndex = 1;
            // 
            // simModelLabel
            // 
            this.simModelLabel.AutoSize = true;
            this.simModelLabel.Location = new System.Drawing.Point(12, 9);
            this.simModelLabel.Name = "simModelLabel";
            this.simModelLabel.Size = new System.Drawing.Size(90, 13);
            this.simModelLabel.TabIndex = 2;
            this.simModelLabel.Text = "Simulation Model:";
            // 
            // ouputLabel
            // 
            this.ouputLabel.AutoSize = true;
            this.ouputLabel.Location = new System.Drawing.Point(12, 35);
            this.ouputLabel.Name = "ouputLabel";
            this.ouputLabel.Size = new System.Drawing.Size(61, 13);
            this.ouputLabel.TabIndex = 3;
            this.ouputLabel.Text = "Output File:";
            // 
            // simModelBrowseButton
            // 
            this.simModelBrowseButton.Location = new System.Drawing.Point(320, 4);
            this.simModelBrowseButton.Name = "simModelBrowseButton";
            this.simModelBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.simModelBrowseButton.TabIndex = 4;
            this.simModelBrowseButton.Text = "Browse";
            this.simModelBrowseButton.UseVisualStyleBackColor = true;
            this.simModelBrowseButton.Click += new System.EventHandler(this.simModelBrowseButton_Click);
            // 
            // outputBrowseButton
            // 
            this.outputBrowseButton.Location = new System.Drawing.Point(320, 28);
            this.outputBrowseButton.Name = "outputBrowseButton";
            this.outputBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.outputBrowseButton.TabIndex = 5;
            this.outputBrowseButton.Text = "Browse";
            this.outputBrowseButton.UseVisualStyleBackColor = true;
            this.outputBrowseButton.Click += new System.EventHandler(this.outputBrowseButton_Click);
            // 
            // generateDocButton
            // 
            this.generateDocButton.Location = new System.Drawing.Point(259, 58);
            this.generateDocButton.Name = "generateDocButton";
            this.generateDocButton.Size = new System.Drawing.Size(136, 23);
            this.generateDocButton.TabIndex = 6;
            this.generateDocButton.Text = "Generate Doc";
            this.generateDocButton.UseVisualStyleBackColor = true;
            this.generateDocButton.Click += new System.EventHandler(this.generateDocButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 91);
            this.Controls.Add(this.generateDocButton);
            this.Controls.Add(this.outputBrowseButton);
            this.Controls.Add(this.simModelBrowseButton);
            this.Controls.Add(this.ouputLabel);
            this.Controls.Add(this.simModelLabel);
            this.Controls.Add(this.ouputTextBox);
            this.Controls.Add(this.simModelTextBox);
            this.Name = "Form1";
            this.Text = "SimModelDocGen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox simModelTextBox;
        private System.Windows.Forms.TextBox ouputTextBox;
        private System.Windows.Forms.Label simModelLabel;
        private System.Windows.Forms.Label ouputLabel;
        private System.Windows.Forms.Button simModelBrowseButton;
        private System.Windows.Forms.Button outputBrowseButton;
        private System.Windows.Forms.Button generateDocButton;
    }
}

