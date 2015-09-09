namespace VSG.ViewComponents
{
    partial class Level
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customRadioButton2 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.customNonnegativeDouble1 = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.customRadioButton1 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.customTabControl1.Size = new System.Drawing.Size(230, 145);
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
            this.customTabPage1.Size = new System.Drawing.Size(222, 119);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Level";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 92);
            this.panel1.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customRadioButton2);
            this.groupBox1.Controls.Add(this.customNonnegativeDouble1);
            this.groupBox1.Controls.Add(this.customRadioButton1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 74);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Parameters (Required)";
            // 
            // customRadioButton2
            // 
            this.customRadioButton2.AutoSize = true;
            this.customRadioButton2.ComponentId = 0;
            this.customRadioButton2.Controller = null;
            this.customRadioButton2.Location = new System.Drawing.Point(6, 40);
            this.customRadioButton2.Name = "customRadioButton2";
            this.customRadioButton2.ParameterCategory = "Level";
            this.customRadioButton2.ParameterName = "Probability";
            this.customRadioButton2.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton2.Size = new System.Drawing.Size(73, 17);
            this.customRadioButton2.TabIndex = 1;
            this.customRadioButton2.Text = "Probability";
            this.customRadioButton2.UseVisualStyleBackColor = true;
            this.customRadioButton2.CheckedChanged += new System.EventHandler(this.radioButtons_CheckChanged);
            // 
            // customNonnegativeDouble1
            // 
            this.customNonnegativeDouble1.ComponentId = 0;
            this.customNonnegativeDouble1.Controller = null;
            this.customNonnegativeDouble1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customNonnegativeDouble1.Location = new System.Drawing.Point(117, 37);
            this.customNonnegativeDouble1.Name = "customNonnegativeDouble1";
            this.customNonnegativeDouble1.ParameterCategory = "Level";
            this.customNonnegativeDouble1.ParameterName = "Percentage";
            this.customNonnegativeDouble1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customNonnegativeDouble1.Size = new System.Drawing.Size(59, 20);
            this.customNonnegativeDouble1.TabIndex = 6;
            this.customNonnegativeDouble1.Text = "100";
            this.customNonnegativeDouble1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.customNonnegativeDouble1.Value = 100;
            // 
            // customRadioButton1
            // 
            this.customRadioButton1.AutoSize = true;
            this.customRadioButton1.Checked = true;
            this.customRadioButton1.ComponentId = 0;
            this.customRadioButton1.Controller = null;
            this.customRadioButton1.Location = new System.Drawing.Point(6, 17);
            this.customRadioButton1.Name = "customRadioButton1";
            this.customRadioButton1.ParameterCategory = "Level";
            this.customRadioButton1.ParameterName = "Variance";
            this.customRadioButton1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton1.Size = new System.Drawing.Size(67, 17);
            this.customRadioButton1.TabIndex = 0;
            this.customRadioButton1.TabStop = true;
            this.customRadioButton1.Text = "Variance";
            this.customRadioButton1.UseVisualStyleBackColor = true;
            this.customRadioButton1.CheckedChanged += new System.EventHandler(this.radioButtons_CheckChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(114, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Magnitude (%)";
            // 
            // Level
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "Level";
            this.Size = new System.Drawing.Size(230, 145);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private AME.Views.View_Components.CustomNonnegativeDouble customNonnegativeDouble1;
        private AME.Views.View_Components.CustomRadioButton customRadioButton2;
        private AME.Views.View_Components.CustomRadioButton customRadioButton1;
        private System.Windows.Forms.Panel panel1;
    }
}
