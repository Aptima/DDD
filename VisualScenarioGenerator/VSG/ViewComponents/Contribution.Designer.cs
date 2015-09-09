namespace VSG.ViewComponents
{
    partial class Contribution
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
            this.components = new System.ComponentModel.Container();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.nudIntensity = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nndRange = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.nudProbability = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntensity)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability)).BeginInit();
            this.customTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(115, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Probability ( % ):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(138, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 46;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "Range (m.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "Intensity (Units):";
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(307, 311);
            this.customTabPage1.TabIndex = 1;
            this.customTabPage1.Text = "Contribution";
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.MinimumSize = new System.Drawing.Size(321, 310);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(321, 310);
            this.panel1.TabIndex = 48;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudIntensity);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 167);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Contribution Parameters (Required)";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(196, 21);
            this.comboBox1.TabIndex = 59;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // nudIntensity
            // 
            this.nudIntensity.ComponentId = 0;
            this.nudIntensity.Controller = null;
            this.nudIntensity.Location = new System.Drawing.Point(9, 127);
            this.nudIntensity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudIntensity.Name = "nudIntensity";
            this.nudIntensity.ParameterCategory = "Contribution";
            this.nudIntensity.ParameterName = "Intensity";
            this.nudIntensity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nudIntensity.Size = new System.Drawing.Size(87, 20);
            this.nudIntensity.TabIndex = 51;
            this.nudIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "Vulnerable to capability:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.nndRange);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.nudProbability);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 74);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional Parameters";
            // 
            // nndRange
            // 
            this.nndRange.ComponentId = 0;
            this.nndRange.Controller = null;
            this.nndRange.Location = new System.Drawing.Point(9, 41);
            this.nndRange.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.nndRange.Name = "nndRange";
            this.nndRange.ParameterCategory = "Contribution";
            this.nndRange.ParameterName = "Range";
            this.nndRange.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndRange.Size = new System.Drawing.Size(87, 20);
            this.nndRange.TabIndex = 50;
            this.nndRange.Text = "0";
            this.nndRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndRange.Value = 0;
            // 
            // nudProbability
            // 
            this.nudProbability.ComponentId = 0;
            this.nudProbability.Controller = null;
            this.nudProbability.Location = new System.Drawing.Point(118, 41);
            this.nudProbability.Name = "nudProbability";
            this.nudProbability.ParameterCategory = "Contribution";
            this.nudProbability.ParameterName = "Probability";
            this.nudProbability.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nudProbability.Size = new System.Drawing.Size(87, 20);
            this.nudProbability.TabIndex = 52;
            this.nudProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudProbability.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(315, 337);
            this.customTabControl1.TabIndex = 58;
            // 
            // Contribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "Contribution";
            this.Size = new System.Drawing.Size(315, 337);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntensity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability)).EndInit();
            this.customTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown nudIntensity;
        private System.Windows.Forms.Label label6;
        private AME.Views.View_Components.CustomNonnegativeDouble nndRange;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private AME.Views.View_Components.CustomNumericUpDown nudProbability;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
