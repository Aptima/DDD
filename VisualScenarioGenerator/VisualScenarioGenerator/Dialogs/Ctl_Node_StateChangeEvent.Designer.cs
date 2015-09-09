namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Node_StateChangeEvent
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
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ctl_Node_EngramRange1 = new VisualScenarioGenerator.Dialogs.Ctl_ChooseEngramRange();
            this.ctl_Node1 = new VisualScenarioGenerator.Dialogs.Ctl_Node();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 189);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(107, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(123, 189);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(107, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(6, 229);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(107, 21);
            this.comboBox3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Exclude";
            // 
            // ctl_Node_EngramRange1
            // 
            this.ctl_Node_EngramRange1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node_EngramRange1.Location = new System.Drawing.Point(0, 80);
            this.ctl_Node_EngramRange1.Name = "ctl_Node_EngramRange1";
            this.ctl_Node_EngramRange1.Size = new System.Drawing.Size(244, 87);
            this.ctl_Node_EngramRange1.TabIndex = 1;
            // 
            // ctl_Node1
            // 
            this.ctl_Node1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node1.ID = "";
            this.ctl_Node1.Location = new System.Drawing.Point(0, 0);
            this.ctl_Node1.Name = "ctl_Node1";
            this.ctl_Node1.Size = new System.Drawing.Size(244, 80);
            this.ctl_Node1.TabIndex = 0;
            this.ctl_Node1.Tick = 0;
            // 
            // Ctl_Node_StateChangeEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctl_Node_EngramRange1);
            this.Controls.Add(this.ctl_Node1);
            this.Name = "Ctl_Node_StateChangeEvent";
            this.Size = new System.Drawing.Size(244, 332);
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
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label3;


    }
}
