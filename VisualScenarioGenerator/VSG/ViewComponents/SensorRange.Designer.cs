namespace VSG.ViewComponents
{
    partial class SensorRange
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
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRange = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtXpos = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtZpos = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txtSpread = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.txtYpos = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(378, 207);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(370, 181);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Sensor Range";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 150);
            this.panel1.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtRange);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 134);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Required Parameters";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 96);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.Text = "<Select Level>";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Range (m.)";
            // 
            // txtRange
            // 
            this.txtRange.ComponentId = 0;
            this.txtRange.Controller = null;
            this.txtRange.Location = new System.Drawing.Point(12, 38);
            this.txtRange.Name = "txtRange";
            this.txtRange.ParameterCategory = "SensorRange";
            this.txtRange.ParameterName = "Range";
            this.txtRange.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.txtRange.Size = new System.Drawing.Size(57, 20);
            this.txtRange.TabIndex = 10;
            this.txtRange.Text = "0";
            this.txtRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRange.Value = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtXpos);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtZpos);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSpread);
            this.groupBox1.Controls.Add(this.txtYpos);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(166, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 134);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional Parameters";
            // 
            // txtXpos
            // 
            this.txtXpos.ComponentId = 0;
            this.txtXpos.Controller = null;
            this.txtXpos.Enabled = false;
            this.txtXpos.Location = new System.Drawing.Point(12, 96);
            this.txtXpos.Name = "txtXpos";
            this.txtXpos.ParameterCategory = "SensorRange";
            this.txtXpos.ParameterName = "X";
            this.txtXpos.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.txtXpos.Size = new System.Drawing.Size(43, 20);
            this.txtXpos.TabIndex = 12;
            this.txtXpos.Text = "0";
            this.txtXpos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtXpos.Value = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(130, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Z";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Spread (Deg.)";
            // 
            // txtZpos
            // 
            this.txtZpos.ComponentId = 0;
            this.txtZpos.Controller = null;
            this.txtZpos.Enabled = false;
            this.txtZpos.Location = new System.Drawing.Point(133, 96);
            this.txtZpos.Name = "txtZpos";
            this.txtZpos.ParameterCategory = "SensorRange";
            this.txtZpos.ParameterName = "Z";
            this.txtZpos.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.txtZpos.Size = new System.Drawing.Size(43, 20);
            this.txtZpos.TabIndex = 14;
            this.txtZpos.Text = "0";
            this.txtZpos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtZpos.Value = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(71, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Y";
            // 
            // txtSpread
            // 
            this.txtSpread.ComponentId = 0;
            this.txtSpread.Controller = null;
            this.txtSpread.Enabled = false;
            this.txtSpread.Location = new System.Drawing.Point(12, 38);
            this.txtSpread.Name = "txtSpread";
            this.txtSpread.ParameterCategory = "SensorRange";
            this.txtSpread.ParameterName = "Spread";
            this.txtSpread.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.txtSpread.Size = new System.Drawing.Size(43, 20);
            this.txtSpread.TabIndex = 11;
            this.txtSpread.Text = "0";
            this.txtSpread.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSpread.Value = 0;
            // 
            // txtYpos
            // 
            this.txtYpos.ComponentId = 0;
            this.txtYpos.Controller = null;
            this.txtYpos.Enabled = false;
            this.txtYpos.Location = new System.Drawing.Point(74, 96);
            this.txtYpos.Name = "txtYpos";
            this.txtYpos.ParameterCategory = "SensorRange";
            this.txtYpos.ParameterName = "Y";
            this.txtYpos.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.txtYpos.Size = new System.Drawing.Size(43, 20);
            this.txtYpos.TabIndex = 13;
            this.txtYpos.Text = "0";
            this.txtYpos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtYpos.Value = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(9, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "X";
            // 
            // SensorRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "SensorRange";
            this.Size = new System.Drawing.Size(378, 207);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private AME.Views.View_Components.CustomNonnegativeDouble txtZpos;
        private AME.Views.View_Components.CustomNonnegativeDouble txtYpos;
        private AME.Views.View_Components.CustomNonnegativeDouble txtXpos;
        private AME.Views.View_Components.CustomNonnegativeDouble txtSpread;
        private AME.Views.View_Components.CustomNonnegativeDouble txtRange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
    }
}
