namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_ChooseEngramRange
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
            this.rbIncluded = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.cboEngramRange = new System.Windows.Forms.ComboBox();
            this.cboSelectEngram = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Engram";
            // 
            // rbIncluded
            // 
            this.rbIncluded.AutoSize = true;
            this.rbIncluded.Checked = true;
            this.rbIncluded.Location = new System.Drawing.Point(6, 9);
            this.rbIncluded.Name = "rbIncluded";
            this.rbIncluded.Size = new System.Drawing.Size(101, 17);
            this.rbIncluded.TabIndex = 0;
            this.rbIncluded.TabStop = true;
            this.rbIncluded.Text = "Included Values";
            this.rbIncluded.UseVisualStyleBackColor = true;
            this.rbIncluded.Click += new System.EventHandler(this.rbIncludedExcluded_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(131, 9);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(103, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "Excluded values";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.rbIncludedExcluded_Click);
            // 
            // cboEngramRange
            // 
            this.cboEngramRange.FormattingEnabled = true;
            this.cboEngramRange.Location = new System.Drawing.Point(83, 100);
            this.cboEngramRange.Name = "cboEngramRange";
            this.cboEngramRange.Size = new System.Drawing.Size(139, 21);
            this.cboEngramRange.TabIndex = 2;
            // 
            // cboSelectEngram
            // 
            this.cboSelectEngram.FormattingEnabled = true;
            this.cboSelectEngram.Location = new System.Drawing.Point(101, 20);
            this.cboSelectEngram.Name = "cboSelectEngram";
            this.cboSelectEngram.Size = new System.Drawing.Size(121, 21);
            this.cboSelectEngram.TabIndex = 1;
            this.cboSelectEngram.SelectionChangeCommitted += new System.EventHandler(this.cboSelectEngram_SelectionChangeCommitted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbIncluded);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new System.Drawing.Point(10, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 26);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current list";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(169, 140);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(21, 140);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // Ctl_ChooseEngramRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboEngramRange);
            this.Controls.Add(this.cboSelectEngram);
            this.Controls.Add(this.label1);
            this.Name = "Ctl_ChooseEngramRange";
            this.Size = new System.Drawing.Size(290, 184);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbIncluded;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cboEngramRange;
        private System.Windows.Forms.ComboBox cboSelectEngram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;


    }
}
