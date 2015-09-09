namespace VSG.ViewComponents
{
    partial class Transition
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
            this.label9 = new System.Windows.Forms.Label();
            this.nndRange = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.nudProbability = new AME.Views.View_Components.CustomNumericUpDownDouble(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stateComboBox1 = new VSG.ViewComponents.StateComboBox(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudIntensity = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(439, 448);
            this.customTabControl1.TabIndex = 57;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 25);
            this.customTabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(431, 419);
            this.customTabPage1.TabIndex = 1;
            this.customTabPage1.Text = "Transition";
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.MinimumSize = new System.Drawing.Size(428, 382);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 382);
            this.panel1.TabIndex = 48;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nndRange);
            this.groupBox2.Controls.Add(this.nudProbability);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(16, 222);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(391, 91);
            this.groupBox2.TabIndex = 55;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optional Parameters";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 17);
            this.label9.TabIndex = 48;
            this.label9.Text = "Range (m.):";
            // 
            // nndRange
            // 
            this.nndRange.ComponentId = 0;
            this.nndRange.Controller = null;
            this.nndRange.Location = new System.Drawing.Point(12, 39);
            this.nndRange.Margin = new System.Windows.Forms.Padding(4, 4, 16, 4);
            this.nndRange.Name = "nndRange";
            this.nndRange.ParameterCategory = "Transition";
            this.nndRange.ParameterName = "Range";
            this.nndRange.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndRange.Size = new System.Drawing.Size(115, 23);
            this.nndRange.TabIndex = 50;
            this.nndRange.Text = "0";
            this.nndRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndRange.Value = 0;
            // 
            // nudProbability
            // 
            this.nudProbability.ComponentId = 0;
            this.nudProbability.Controller = null;
            this.nudProbability.Location = new System.Drawing.Point(168, 39);
            this.nudProbability.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudProbability.Name = "nudProbability";
            this.nudProbability.ParameterCategory = "Transition";
            this.nudProbability.ParameterName = "Probability";
            this.nudProbability.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nudProbability.Size = new System.Drawing.Size(116, 23);
            this.nudProbability.TabIndex = 52;
            this.nudProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudProbability.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudProbability.ValueMultiplier = 100;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(164, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 17);
            this.label6.TabIndex = 43;
            this.label6.Text = "Probability ( % ):";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.stateComboBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudIntensity);
            this.groupBox1.Location = new System.Drawing.Point(16, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(391, 206);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transition Parameters (Required)";
            // 
            // stateComboBox1
            // 
            this.stateComboBox1.ComponentId = -1;
            this.stateComboBox1.Controller = null;
            this.stateComboBox1.FormattingEnabled = true;
            this.stateComboBox1.Location = new System.Drawing.Point(8, 39);
            this.stateComboBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.stateComboBox1.Name = "stateComboBox1";
            this.stateComboBox1.ParameterCategory = "Transition";
            this.stateComboBox1.ParameterName = "State";
            this.stateComboBox1.ShowAllStates = false;
            this.stateComboBox1.Size = new System.Drawing.Size(373, 25);
            this.stateComboBox1.SpeciesId = -1;
            this.stateComboBox1.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 17);
            this.label8.TabIndex = 46;
            this.label8.Text = "New State:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 135);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 39;
            this.label3.Text = "Intensity (Units):";
            // 
            // nudIntensity
            // 
            this.nudIntensity.ComponentId = 0;
            this.nudIntensity.Controller = null;
            this.nudIntensity.Location = new System.Drawing.Point(12, 155);
            this.nudIntensity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudIntensity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudIntensity.Name = "nudIntensity";
            this.nudIntensity.ParameterCategory = "Transition";
            this.nudIntensity.ParameterName = "Intensity";
            this.nudIntensity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nudIntensity.Size = new System.Drawing.Size(116, 23);
            this.nudIntensity.TabIndex = 51;
            this.nudIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Transition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Transition";
            this.Size = new System.Drawing.Size(439, 448);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProbability)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntensity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private AME.Views.View_Components.CustomNumericUpDownDouble nudProbability;
        private AME.Views.View_Components.CustomNumericUpDown nudIntensity;
        private AME.Views.View_Components.CustomNonnegativeDouble nndRange;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private StateComboBox stateComboBox1;
    }
}
