namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Combos
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudProbability_1 = new System.Windows.Forms.NumericUpDown();
            this.lbxCapability_1 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudEffect_1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nndRange_1 = new VisualScenarioGenerator.NonNegDecimal();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudProbability_2 = new System.Windows.Forms.NumericUpDown();
            this.nndRange_2 = new VisualScenarioGenerator.NonNegDecimal();
            this.lbxCapability_2 = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudEffect_2 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxNewState = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffect_1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffect_2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudProbability_1);
            this.groupBox1.Controls.Add(this.lbxCapability_1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.nudEffect_1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nndRange_1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 84);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(329, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "%";
            // 
            // nudProbability_1
            // 
            this.nudProbability_1.Location = new System.Drawing.Point(281, 42);
            this.nudProbability_1.Name = "nudProbability_1";
            this.nudProbability_1.Size = new System.Drawing.Size(42, 20);
            this.nudProbability_1.TabIndex = 18;
            this.nudProbability_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbxCapability_1
            // 
            this.lbxCapability_1.FormattingEnabled = true;
            this.lbxCapability_1.Location = new System.Drawing.Point(59, 19);
            this.lbxCapability_1.Name = "lbxCapability_1";
            this.lbxCapability_1.Size = new System.Drawing.Size(134, 17);
            this.lbxCapability_1.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(208, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Probability";
            // 
            // nudEffect_1
            // 
            this.nudEffect_1.Location = new System.Drawing.Point(143, 42);
            this.nudEffect_1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudEffect_1.Name = "nudEffect_1";
            this.nudEffect_1.Size = new System.Drawing.Size(50, 20);
            this.nudEffect_1.TabIndex = 14;
            this.nudEffect_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Effect";
            // 
            // nndRange_1
            // 
            this.nndRange_1.Location = new System.Drawing.Point(271, 19);
            this.nndRange_1.Name = "nndRange_1";
            this.nndRange_1.Size = new System.Drawing.Size(62, 21);
            this.nndRange_1.TabIndex = 12;
            this.nndRange_1.Value = 0;
            this.nndRange_1.Load += new System.EventHandler(this.nndRange_1_Load);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Capability";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Range";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.nudProbability_2);
            this.groupBox2.Controls.Add(this.nndRange_2);
            this.groupBox2.Controls.Add(this.lbxCapability_2);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.nudEffect_2);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(6, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 85);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(318, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "%";
            // 
            // nudProbability_2
            // 
            this.nudProbability_2.Location = new System.Drawing.Point(281, 53);
            this.nudProbability_2.Name = "nudProbability_2";
            this.nudProbability_2.Size = new System.Drawing.Size(42, 20);
            this.nudProbability_2.TabIndex = 18;
            this.nudProbability_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nndRange_2
            // 
            this.nndRange_2.Location = new System.Drawing.Point(261, 15);
            this.nndRange_2.Name = "nndRange_2";
            this.nndRange_2.Size = new System.Drawing.Size(62, 21);
            this.nndRange_2.TabIndex = 12;
            this.nndRange_2.Value = 0;
            // 
            // lbxCapability_2
            // 
            this.lbxCapability_2.FormattingEnabled = true;
            this.lbxCapability_2.Location = new System.Drawing.Point(59, 19);
            this.lbxCapability_2.Name = "lbxCapability_2";
            this.lbxCapability_2.Size = new System.Drawing.Size(134, 17);
            this.lbxCapability_2.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(208, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Range";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(208, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Probability";
            // 
            // nudEffect_2
            // 
            this.nudEffect_2.Location = new System.Drawing.Point(143, 55);
            this.nudEffect_2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudEffect_2.Name = "nudEffect_2";
            this.nudEffect_2.Size = new System.Drawing.Size(50, 20);
            this.nudEffect_2.TabIndex = 14;
            this.nudEffect_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Effect";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Capability";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "New State";
            // 
            // lbxNewState
            // 
            this.lbxNewState.FormattingEnabled = true;
            this.lbxNewState.Location = new System.Drawing.Point(152, 193);
            this.lbxNewState.Name = "lbxNewState";
            this.lbxNewState.Size = new System.Drawing.Size(116, 17);
            this.lbxNewState.TabIndex = 40;
            // 
            // groupBox3
            // 
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.Location = new System.Drawing.Point(4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 219);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            // 
            // Ctl_Combos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.lbxNewState);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "Ctl_Combos";
            this.Size = new System.Drawing.Size(363, 229);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffect_1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffect_2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private NonNegDecimal nndRange_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudEffect_1;
        private System.Windows.Forms.ListBox lbxCapability_1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudProbability_1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudProbability_2;
        private System.Windows.Forms.ListBox lbxCapability_2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudEffect_2;
        private System.Windows.Forms.Label label9;
        private NonNegDecimal nndRange_2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxNewState;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
