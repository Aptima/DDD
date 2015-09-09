namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Node_TransferEvent
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ctl_Node_EngramRange1 = new VisualScenarioGenerator.Dialogs.Ctl_ChooseEngramRange();
            this.ctl_Node1 = new VisualScenarioGenerator.Dialogs.Ctl_Node();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 201);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(107, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(116, 201);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(107, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To";
            // 
            // ctl_Node_EngramRange1
            // 
            this.ctl_Node_EngramRange1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node_EngramRange1.Location = new System.Drawing.Point(0, 85);
            this.ctl_Node_EngramRange1.Name = "ctl_Node_EngramRange1";
            this.ctl_Node_EngramRange1.Size = new System.Drawing.Size(243, 94);
            this.ctl_Node_EngramRange1.TabIndex = 1;
            // 
            // ctl_Node1
            // 
            this.ctl_Node1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node1.ID = "";
            this.ctl_Node1.Location = new System.Drawing.Point(0, 0);
            this.ctl_Node1.Name = "ctl_Node1";
            this.ctl_Node1.Size = new System.Drawing.Size(243, 85);
            this.ctl_Node1.TabIndex = 0;
            this.ctl_Node1.Tick = 0;
            // 
            // Ctl_Node_TransferEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctl_Node_EngramRange1);
            this.Controls.Add(this.ctl_Node1);
            this.Name = "Ctl_Node_TransferEvent";
            this.Size = new System.Drawing.Size(243, 284);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ctl_Node ctl_Node1;
        private Ctl_ChooseEngramRange ctl_Node_EngramRange1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
    }
}
