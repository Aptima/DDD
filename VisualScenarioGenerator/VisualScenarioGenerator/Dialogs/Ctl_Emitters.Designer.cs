namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Emitters
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbUnlimited = new System.Windows.Forms.CheckBox();
            this.lbxAttribute = new System.Windows.Forms.ListBox();
            this.nndVariance_2 = new VisualScenarioGenerator.NonNegDecimal();
            this.lblVariance_2 = new System.Windows.Forms.Label();
            this.txLevel_2 = new System.Windows.Forms.TextBox();
            this.lblLevel_2 = new System.Windows.Forms.Label();
            this.lblVariance_1 = new System.Windows.Forms.Label();
            this.txLevel_1 = new System.Windows.Forms.TextBox();
            this.nndVariance_1 = new VisualScenarioGenerator.NonNegDecimal();
            this.lblLevel_1 = new System.Windows.Forms.Label();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbInvisible = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(356, 26);
            this.label4.TabIndex = 30;
            this.label4.Text = "Emitter";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(44, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(278, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Location = new System.Drawing.Point(278, 29);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 35;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbUnlimited);
            this.groupBox1.Controls.Add(this.lbxAttribute);
            this.groupBox1.Controls.Add(this.nndVariance_2);
            this.groupBox1.Controls.Add(this.lblVariance_2);
            this.groupBox1.Controls.Add(this.txLevel_2);
            this.groupBox1.Controls.Add(this.lblLevel_2);
            this.groupBox1.Controls.Add(this.lblVariance_1);
            this.groupBox1.Controls.Add(this.txLevel_1);
            this.groupBox1.Controls.Add(this.nndVariance_1);
            this.groupBox1.Controls.Add(this.lblLevel_1);
            this.groupBox1.Controls.Add(this.rbSingle);
            this.groupBox1.Controls.Add(this.rbInvisible);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Location = new System.Drawing.Point(6, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 163);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // ckbUnlimited
            // 
            this.ckbUnlimited.AutoSize = true;
            this.ckbUnlimited.Location = new System.Drawing.Point(111, 61);
            this.ckbUnlimited.Name = "ckbUnlimited";
            this.ckbUnlimited.Size = new System.Drawing.Size(69, 17);
            this.ckbUnlimited.TabIndex = 14;
            this.ckbUnlimited.Text = "Unlimited";
            this.ckbUnlimited.UseVisualStyleBackColor = true;
            this.ckbUnlimited.Click += new System.EventHandler(this.ckbUnlimited_Click);
            // 
            // lbxAttribute
            // 
            this.lbxAttribute.FormattingEnabled = true;
            this.lbxAttribute.Items.AddRange(new object[] {
            "ID",
            "State",
            "Location",
            "Velocity",
            "Throttle",
            "ObjectName",
            "OwnerID",
            "ClassName",
            "TeamName",
            "Size",
            "StateTable",
            "Vulnerability"});
            this.lbxAttribute.Location = new System.Drawing.Point(206, 24);
            this.lbxAttribute.Name = "lbxAttribute";
            this.lbxAttribute.Size = new System.Drawing.Size(120, 69);
            this.lbxAttribute.TabIndex = 13;
            // 
            // nndVariance_2
            // 
            this.nndVariance_2.Location = new System.Drawing.Point(236, 136);
            this.nndVariance_2.Name = "nndVariance_2";
            this.nndVariance_2.Size = new System.Drawing.Size(90, 21);
            this.nndVariance_2.TabIndex = 12;
            this.nndVariance_2.Value = 0;
            // 
            // lblVariance_2
            // 
            this.lblVariance_2.AutoSize = true;
            this.lblVariance_2.Location = new System.Drawing.Point(171, 136);
            this.lblVariance_2.Name = "lblVariance_2";
            this.lblVariance_2.Size = new System.Drawing.Size(49, 13);
            this.lblVariance_2.TabIndex = 11;
            this.lblVariance_2.Text = "Variance";
            // 
            // txLevel_2
            // 
            this.txLevel_2.Location = new System.Drawing.Point(83, 131);
            this.txLevel_2.Name = "txLevel_2";
            this.txLevel_2.Size = new System.Drawing.Size(65, 20);
            this.txLevel_2.TabIndex = 10;
            // 
            // lblLevel_2
            // 
            this.lblLevel_2.AutoSize = true;
            this.lblLevel_2.Location = new System.Drawing.Point(31, 131);
            this.lblLevel_2.Name = "lblLevel_2";
            this.lblLevel_2.Size = new System.Drawing.Size(33, 13);
            this.lblLevel_2.TabIndex = 9;
            this.lblLevel_2.Text = "Level";
            // 
            // lblVariance_1
            // 
            this.lblVariance_1.AutoSize = true;
            this.lblVariance_1.Location = new System.Drawing.Point(170, 110);
            this.lblVariance_1.Name = "lblVariance_1";
            this.lblVariance_1.Size = new System.Drawing.Size(49, 13);
            this.lblVariance_1.TabIndex = 8;
            this.lblVariance_1.Text = "Variance";
            // 
            // txLevel_1
            // 
            this.txLevel_1.Location = new System.Drawing.Point(83, 105);
            this.txLevel_1.Name = "txLevel_1";
            this.txLevel_1.Size = new System.Drawing.Size(65, 20);
            this.txLevel_1.TabIndex = 7;
            // 
            // nndVariance_1
            // 
            this.nndVariance_1.Location = new System.Drawing.Point(236, 104);
            this.nndVariance_1.Name = "nndVariance_1";
            this.nndVariance_1.Size = new System.Drawing.Size(91, 21);
            this.nndVariance_1.TabIndex = 6;
            this.nndVariance_1.Value = 0;
            // 
            // lblLevel_1
            // 
            this.lblLevel_1.AutoSize = true;
            this.lblLevel_1.Location = new System.Drawing.Point(31, 109);
            this.lblLevel_1.Name = "lblLevel_1";
            this.lblLevel_1.Size = new System.Drawing.Size(33, 13);
            this.lblLevel_1.TabIndex = 5;
            this.lblLevel_1.Text = "Level";
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(83, 20);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(96, 17);
            this.rbSingle.TabIndex = 2;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single Attribute";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.Click += new System.EventHandler(this.rbSingle_Click);
            // 
            // rbInvisible
            // 
            this.rbInvisible.AutoSize = true;
            this.rbInvisible.Location = new System.Drawing.Point(7, 44);
            this.rbInvisible.Name = "rbInvisible";
            this.rbInvisible.Size = new System.Drawing.Size(63, 17);
            this.rbInvisible.TabIndex = 1;
            this.rbInvisible.TabStop = true;
            this.rbInvisible.Text = "Invisible";
            this.rbInvisible.UseVisualStyleBackColor = true;
            this.rbInvisible.Click += new System.EventHandler(this.rbInvisible_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(7, 20);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.Click += new System.EventHandler(this.rbAll_Click);
            // 
            // Ctl_Emitters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "Ctl_Emitters";
            this.Size = new System.Drawing.Size(356, 256);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbInvisible;
        private System.Windows.Forms.RadioButton rbSingle;
        private NonNegDecimal nndVariance_1;
        private System.Windows.Forms.Label lblLevel_1;
        private System.Windows.Forms.Label lblVariance_1;
        private System.Windows.Forms.TextBox txLevel_1;
        private NonNegDecimal nndVariance_2;
        private System.Windows.Forms.Label lblVariance_2;
        private System.Windows.Forms.TextBox txLevel_2;
        private System.Windows.Forms.Label lblLevel_2;
        private System.Windows.Forms.ListBox lbxAttribute;
        private System.Windows.Forms.CheckBox ckbUnlimited;
    }
}
