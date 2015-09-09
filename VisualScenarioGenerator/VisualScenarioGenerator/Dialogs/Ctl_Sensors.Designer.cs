namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Sensors
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
            this.txtID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.xyzDirection = new XYZ();
            this.lblDirection = new System.Windows.Forms.Label();
            this.txtLevel = new System.Windows.Forms.TextBox();
            this.lbxAttribute = new System.Windows.Forms.ListBox();
            this.nndSpread = new VisualScenarioGenerator.NonNegDecimal();
            this.nndRange = new VisualScenarioGenerator.NonNegDecimal();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblSpread = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.rbOneAttribute = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
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
            this.label4.Size = new System.Drawing.Size(357, 26);
            this.label4.TabIndex = 30;
            this.label4.Text = "Sensor";
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
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(44, 34);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 20);
            this.txtID.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(257, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(257, 29);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 35;
            this.btnOK.Text = "Accept";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.xyzDirection);
            this.groupBox1.Controls.Add(this.lblDirection);
            this.groupBox1.Controls.Add(this.txtLevel);
            this.groupBox1.Controls.Add(this.lbxAttribute);
            this.groupBox1.Controls.Add(this.nndSpread);
            this.groupBox1.Controls.Add(this.nndRange);
            this.groupBox1.Controls.Add(this.lblLevel);
            this.groupBox1.Controls.Add(this.lblSpread);
            this.groupBox1.Controls.Add(this.lblAttribute);
            this.groupBox1.Controls.Add(this.rbOneAttribute);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Location = new System.Drawing.Point(6, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 262);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // xyzDirection
            // 
            this.xyzDirection.Location = new System.Drawing.Point(126, 192);
            this.xyzDirection.Name = "xyzDirection";
            this.xyzDirection.Size = new System.Drawing.Size(218, 64);
            this.xyzDirection.TabIndex = 17;
            this.xyzDirection.Load += new System.EventHandler(this.xyzDirection_Load);
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(73, 195);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(49, 13);
            this.lblDirection.TabIndex = 16;
            this.lblDirection.Text = "Direction";
            // 
            // txtLevel
            // 
            this.txtLevel.Location = new System.Drawing.Point(123, 169);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(63, 20);
            this.txtLevel.TabIndex = 15;
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
            this.lbxAttribute.Location = new System.Drawing.Point(120, 75);
            this.lbxAttribute.Name = "lbxAttribute";
            this.lbxAttribute.Size = new System.Drawing.Size(203, 56);
            this.lbxAttribute.TabIndex = 14;
            // 
            // nndSpread
            // 
            this.nndSpread.Location = new System.Drawing.Point(123, 139);
            this.nndSpread.Name = "nndSpread";
            this.nndSpread.Size = new System.Drawing.Size(53, 21);
            this.nndSpread.TabIndex = 13;
            this.nndSpread.Value = 0;
            this.nndSpread.MouseLeave += new System.EventHandler(this.nndSpread_MouseLeave);
            // 
            // nndRange
            // 
            this.nndRange.Location = new System.Drawing.Point(120, 16);
            this.nndRange.Name = "nndRange";
            this.nndRange.Size = new System.Drawing.Size(56, 21);
            this.nndRange.TabIndex = 12;
            this.nndRange.Value = 0;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(73, 171);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(33, 13);
            this.lblLevel.TabIndex = 8;
            this.lblLevel.Text = "Level";
            // 
            // lblSpread
            // 
            this.lblSpread.AutoSize = true;
            this.lblSpread.Location = new System.Drawing.Point(73, 147);
            this.lblSpread.Name = "lblSpread";
            this.lblSpread.Size = new System.Drawing.Size(41, 13);
            this.lblSpread.TabIndex = 7;
            this.lblSpread.Text = "Spread";
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(73, 78);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(46, 13);
            this.lblAttribute.TabIndex = 5;
            this.lblAttribute.Text = "Attribute";
            // 
            // rbOneAttribute
            // 
            this.rbOneAttribute.AutoSize = true;
            this.rbOneAttribute.Location = new System.Drawing.Point(1, 75);
            this.rbOneAttribute.Name = "rbOneAttribute";
            this.rbOneAttribute.Size = new System.Drawing.Size(54, 17);
            this.rbOneAttribute.TabIndex = 3;
            this.rbOneAttribute.Text = "Single";
            this.rbOneAttribute.UseVisualStyleBackColor = true;
            this.rbOneAttribute.Click += new System.EventHandler(this.rb_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Range";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(1, 44);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(68, 17);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Sense all";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.Click += new System.EventHandler(this.rb_Click);
            // 
            // Ctl_Sensors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "Ctl_Sensors";
            this.Size = new System.Drawing.Size(357, 353);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbOneAttribute;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.Label lblSpread;
        private System.Windows.Forms.Label lblLevel;
        private NonNegDecimal nndSpread;
        private NonNegDecimal nndRange;
        private System.Windows.Forms.ListBox lbxAttribute;
        private System.Windows.Forms.TextBox txtLevel;
        private System.Windows.Forms.Label lblDirection;
        private XYZ xyzDirection;
    }
}
