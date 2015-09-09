namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Species
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
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lboxIconSelection = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRemove = new System.Windows.Forms.CheckBox();
            this.cbIsWeapon = new System.Windows.Forms.CheckBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.clbxStates = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nndCollisionRadius = new VisualScenarioGenerator.NonNegDecimal();
            this.ctlFullyParameters = new VisualScenarioGenerator.Dialogs.Ctl_Node_Parameters();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nndCollisionRadius);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lboxIconSelection);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbRemove);
            this.groupBox1.Controls.Add(this.cbIsWeapon);
            this.groupBox1.Controls.Add(this.cboType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Species";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(366, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(183, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Default Icon";
            // 
            // lboxIconSelection
            // 
            this.lboxIconSelection.Location = new System.Drawing.Point(186, 39);
            this.lboxIconSelection.Name = "lboxIconSelection";
            this.lboxIconSelection.Size = new System.Drawing.Size(158, 88);
            this.lboxIconSelection.TabIndex = 8;
            this.lboxIconSelection.UseCompatibleStateImageBehavior = false;
            this.lboxIconSelection.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(363, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Collision Radius";
            // 
            // cbRemove
            // 
            this.cbRemove.AutoSize = true;
            this.cbRemove.Location = new System.Drawing.Point(12, 105);
            this.cbRemove.Name = "cbRemove";
            this.cbRemove.Size = new System.Drawing.Size(138, 17);
            this.cbRemove.TabIndex = 5;
            this.cbRemove.Text = "Remove on Destruction";
            this.cbRemove.UseVisualStyleBackColor = true;
            // 
            // cbIsWeapon
            // 
            this.cbIsWeapon.AutoSize = true;
            this.cbIsWeapon.Location = new System.Drawing.Point(12, 82);
            this.cbIsWeapon.Name = "cbIsWeapon";
            this.cbIsWeapon.Size = new System.Drawing.Size(78, 17);
            this.cbIsWeapon.TabIndex = 4;
            this.cbIsWeapon.Text = "Is Weapon";
            this.cbIsWeapon.UseVisualStyleBackColor = true;
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Generic Air",
            "Generic Land",
            "Generic Sea"});
            this.cboType.Location = new System.Drawing.Point(46, 39);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(121, 21);
            this.cboType.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Type";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ctlFullyParameters);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 634);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default (Fully Functional) State";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.clbxStates);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 796);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(463, 78);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Additional States";
            // 
            // clbxStates
            // 
            this.clbxStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbxStates.FormattingEnabled = true;
            this.clbxStates.Location = new System.Drawing.Point(3, 16);
            this.clbxStates.Name = "clbxStates";
            this.clbxStates.Size = new System.Drawing.Size(457, 49);
            this.clbxStates.TabIndex = 0;
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
            this.label4.Size = new System.Drawing.Size(463, 26);
            this.label4.TabIndex = 28;
            this.label4.Text = "Species Definition";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nndCollisionRadius
            // 
            this.nndCollisionRadius.Location = new System.Drawing.Point(366, 29);
            this.nndCollisionRadius.Name = "nndCollisionRadius";
            this.nndCollisionRadius.Size = new System.Drawing.Size(91, 21);
            this.nndCollisionRadius.TabIndex = 11;
            // 
            // ctlFullyParameters
            // 
            this.ctlFullyParameters.Location = new System.Drawing.Point(57, 19);
            this.ctlFullyParameters.Name = "ctlFullyParameters";
            this.ctlFullyParameters.Size = new System.Drawing.Size(338, 615);
            this.ctlFullyParameters.TabIndex = 0;
            // 
            // Ctl_Species
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Name = "Ctl_Species";
            this.Size = new System.Drawing.Size(463, 874);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbRemove;
        private System.Windows.Forms.CheckBox cbIsWeapon;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox clbxStates;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lboxIconSelection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private NonNegDecimal nndCollisionRadius;
        private Ctl_Node_Parameters ctlFullyParameters;
    }
}
