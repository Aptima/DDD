namespace VSG.ViewComponents
{
    partial class Effect
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
            this.probabilityCustomNumericUpDownDouble = new AME.Views.View_Components.CustomNumericUpDownDouble(this.components);
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.upDownIntensity = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.label20 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbSingle = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.rbSenseAll = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.xyzDirection = new VSG.ViewComponents.XYZ();
            this.txtLevel = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.nndSpread = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblSpread = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.nndRange = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probabilityCustomNumericUpDownDouble)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIntensity)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(253, 240);
            this.customTabControl1.TabIndex = 41;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(245, 214);
            this.customTabPage1.TabIndex = 2;
            this.customTabPage1.Text = "Effect";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 190);
            this.panel1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.probabilityCustomNumericUpDownDouble);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(216, 76);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional Parameters";
            // 
            // probabilityCustomNumericUpDownDouble
            // 
            this.probabilityCustomNumericUpDownDouble.ComponentId = -1;
            this.probabilityCustomNumericUpDownDouble.Controller = null;
            this.probabilityCustomNumericUpDownDouble.Location = new System.Drawing.Point(108, 28);
            this.probabilityCustomNumericUpDownDouble.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.probabilityCustomNumericUpDownDouble.Name = "probabilityCustomNumericUpDownDouble";
            this.probabilityCustomNumericUpDownDouble.ParameterCategory = "Effect";
            this.probabilityCustomNumericUpDownDouble.ParameterName = "Probability";
            this.probabilityCustomNumericUpDownDouble.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.probabilityCustomNumericUpDownDouble.Size = new System.Drawing.Size(63, 20);
            this.probabilityCustomNumericUpDownDouble.TabIndex = 17;
            this.probabilityCustomNumericUpDownDouble.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.probabilityCustomNumericUpDownDouble.ValueMultiplier = 100;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(15, 30);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 13);
            this.label21.TabIndex = 15;
            this.label21.Text = "Probability %:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.upDownIntensity);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.groupBox4.Size = new System.Drawing.Size(216, 76);
            this.groupBox4.TabIndex = 41;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Required Parameters";
            // 
            // upDownIntensity
            // 
            this.upDownIntensity.ComponentId = 0;
            this.upDownIntensity.Controller = null;
            this.upDownIntensity.Location = new System.Drawing.Point(108, 28);
            this.upDownIntensity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.upDownIntensity.Name = "upDownIntensity";
            this.upDownIntensity.ParameterCategory = "Effect";
            this.upDownIntensity.ParameterName = "Intensity";
            this.upDownIntensity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.upDownIntensity.Size = new System.Drawing.Size(63, 20);
            this.upDownIntensity.TabIndex = 24;
            this.upDownIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(15, 30);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 13);
            this.label20.TabIndex = 13;
            this.label20.Text = "Intensity(Units):";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(504, 277);
            this.panel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbSingle);
            this.groupBox2.Controls.Add(this.rbSenseAll);
            this.groupBox2.Controls.Add(this.xyzDirection);
            this.groupBox2.Controls.Add(this.txtLevel);
            this.groupBox2.Controls.Add(this.nndSpread);
            this.groupBox2.Controls.Add(this.lblDirection);
            this.groupBox2.Controls.Add(this.lblLevel);
            this.groupBox2.Controls.Add(this.lblSpread);
            this.groupBox2.Controls.Add(this.lblAttribute);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.nndRange);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(371, 274);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // rbSingle
            // 
            this.rbSingle.ComponentId = 0;
            this.rbSingle.Controller = null;
            this.rbSingle.Location = new System.Drawing.Point(0, 0);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.ParameterCategory = null;
            this.rbSingle.ParameterName = null;
            this.rbSingle.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbSingle.Size = new System.Drawing.Size(104, 24);
            this.rbSingle.TabIndex = 0;
            // 
            // rbSenseAll
            // 
            this.rbSenseAll.ComponentId = 0;
            this.rbSenseAll.Controller = null;
            this.rbSenseAll.Location = new System.Drawing.Point(0, 0);
            this.rbSenseAll.Name = "rbSenseAll";
            this.rbSenseAll.ParameterCategory = null;
            this.rbSenseAll.ParameterName = null;
            this.rbSenseAll.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbSenseAll.Size = new System.Drawing.Size(104, 24);
            this.rbSenseAll.TabIndex = 1;
            // 
            // xyzDirection
            // 
            this.xyzDirection.ComponentId = 0;
            this.xyzDirection.Controller = null;
            this.xyzDirection.Location = new System.Drawing.Point(128, 221);
            this.xyzDirection.Margin = new System.Windows.Forms.Padding(4);
            this.xyzDirection.Name = "xyzDirection";
            this.xyzDirection.NndX = 0;
            this.xyzDirection.NndY = 0;
            this.xyzDirection.NndZ = 0;
            this.xyzDirection.Size = new System.Drawing.Size(244, 61);
            this.xyzDirection.TabIndex = 20;
            this.xyzDirection.XYZId = -1;
            // 
            // txtLevel
            // 
            this.txtLevel.ComponentId = 0;
            this.txtLevel.Controller = null;
            this.txtLevel.Location = new System.Drawing.Point(139, 183);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.ParameterCategory = null;
            this.txtLevel.ParameterName = null;
            this.txtLevel.ParameterType = AME.Controllers.eParamParentType.Component;
            this.txtLevel.Size = new System.Drawing.Size(52, 20);
            this.txtLevel.TabIndex = 19;
            this.txtLevel.UseDelimiter = false;
            // 
            // nndSpread
            // 
            this.nndSpread.ComponentId = 0;
            this.nndSpread.Controller = null;
            this.nndSpread.Location = new System.Drawing.Point(139, 143);
            this.nndSpread.Name = "nndSpread";
            this.nndSpread.ParameterCategory = null;
            this.nndSpread.ParameterName = null;
            this.nndSpread.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndSpread.Size = new System.Drawing.Size(61, 20);
            this.nndSpread.TabIndex = 18;
            this.nndSpread.Text = "0";
            this.nndSpread.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndSpread.Value = 0;
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(78, 232);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(49, 13);
            this.lblDirection.TabIndex = 17;
            this.lblDirection.Text = "Direction";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(78, 183);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(33, 13);
            this.lblLevel.TabIndex = 9;
            this.lblLevel.Text = "Level";
            // 
            // lblSpread
            // 
            this.lblSpread.AutoSize = true;
            this.lblSpread.Location = new System.Drawing.Point(78, 150);
            this.lblSpread.Name = "lblSpread";
            this.lblSpread.Size = new System.Drawing.Size(41, 13);
            this.lblSpread.TabIndex = 8;
            this.lblSpread.Text = "Spread";
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(96, 70);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(46, 13);
            this.lblAttribute.TabIndex = 6;
            this.lblAttribute.Text = "Attribute";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(148, 70);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(172, 52);
            this.listBox1.TabIndex = 4;
            // 
            // nndRange
            // 
            this.nndRange.ComponentId = 0;
            this.nndRange.Controller = null;
            this.nndRange.Location = new System.Drawing.Point(148, 13);
            this.nndRange.Name = "nndRange";
            this.nndRange.ParameterCategory = null;
            this.nndRange.ParameterName = null;
            this.nndRange.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndRange.Size = new System.Drawing.Size(72, 20);
            this.nndRange.TabIndex = 1;
            this.nndRange.Text = "0";
            this.nndRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndRange.Value = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Range";
            // 
            // Effect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "Effect";
            this.Size = new System.Drawing.Size(253, 240);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probabilityCustomNumericUpDownDouble)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIntensity)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private AME.Views.View_Components.CustomNumericUpDown upDownIntensity;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private AME.Views.View_Components.CustomRadioButton rbSingle;
        private AME.Views.View_Components.CustomRadioButton rbSenseAll;
        private XYZ xyzDirection;
        private AME.Views.View_Components.CustomParameterTextBox txtLevel;
        private AME.Views.View_Components.CustomNonnegativeDouble nndSpread;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblSpread;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.ListBox listBox1;
        private AME.Views.View_Components.CustomNonnegativeDouble nndRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private AME.Views.View_Components.CustomNumericUpDownDouble probabilityCustomNumericUpDownDouble;
    }
}
