namespace Aptima.Asim.DDD.SimCoreGUI
{
    partial class DialogSetScenarioFiles
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.buttonScenarioBrowse = new System.Windows.Forms.Button();
            this.buttonAcceptFiles = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Scenario File:";
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.Location = new System.Drawing.Point(86, 12);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.Size = new System.Drawing.Size(344, 20);
            this.textBoxScenario.TabIndex = 3;
            // 
            // buttonScenarioBrowse
            // 
            this.buttonScenarioBrowse.Location = new System.Drawing.Point(437, 11);
            this.buttonScenarioBrowse.Name = "buttonScenarioBrowse";
            this.buttonScenarioBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonScenarioBrowse.TabIndex = 5;
            this.buttonScenarioBrowse.Text = "Browse...";
            this.buttonScenarioBrowse.UseVisualStyleBackColor = true;
            this.buttonScenarioBrowse.Click += new System.EventHandler(this.buttonScenarioBrowse_Click);
            // 
            // buttonAcceptFiles
            // 
            this.buttonAcceptFiles.Location = new System.Drawing.Point(356, 50);
            this.buttonAcceptFiles.Name = "buttonAcceptFiles";
            this.buttonAcceptFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonAcceptFiles.TabIndex = 6;
            this.buttonAcceptFiles.Text = "OK";
            this.buttonAcceptFiles.UseVisualStyleBackColor = true;
            this.buttonAcceptFiles.Click += new System.EventHandler(this.buttonAcceptFiles_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(437, 50);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // DialogSetScenarioFiles
            // 
            this.AcceptButton = this.buttonAcceptFiles;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(519, 88);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAcceptFiles);
            this.Controls.Add(this.buttonScenarioBrowse);
            this.Controls.Add(this.textBoxScenario);
            this.Controls.Add(this.label2);
            this.Name = "DialogSetScenarioFiles";
            this.Text = "Load Scenario File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxScenario;
        private System.Windows.Forms.Button buttonScenarioBrowse;
        private System.Windows.Forms.Button buttonAcceptFiles;
        private System.Windows.Forms.Button buttonCancel;
    }
}