namespace VSG.ViewComponents
{
    partial class Sensor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxCustomAttributes = new System.Windows.Forms.ListBox();
            this.customRadioButton3 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.customRadioButton2 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.customParameterEnumBox1 = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.nndRange = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.customRadioButton1 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.viewComponentPanel1 = new AME.Views.View_Component_Packages.ViewComponentPanel();
            this.panel1.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.customLinkBox1);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(505, 285);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Opposing Teams";
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = "TeamAgainst";
            this.customLinkBox1.ConnectRootId = 0;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = "Team";
            this.customLinkBox1.DisplayLinkType = "Scenario";
            this.customLinkBox1.DisplayParameterCategory = "";
            this.customLinkBox1.DisplayParameterName = "";
            this.customLinkBox1.DisplayRecursiveCheck = false;
            this.customLinkBox1.DisplayRootId = 0;
            this.customLinkBox1.FilterResult = false;
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.Location = new System.Drawing.Point(3, 23);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = true;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(499, 259);
            this.customLinkBox1.TabIndex = 3;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(361, 451);
            this.customTabControl1.TabIndex = 4;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel2);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(353, 425);
            this.customTabPage1.TabIndex = 1;
            this.customTabPage1.Text = "Sensor";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(367, 377);
            this.panel2.TabIndex = 57;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxCustomAttributes);
            this.groupBox1.Controls.Add(this.customRadioButton3);
            this.groupBox1.Controls.Add(this.customRadioButton2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.customParameterEnumBox1);
            this.groupBox1.Controls.Add(this.nndRange);
            this.groupBox1.Controls.Add(this.customRadioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 371);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor Parameters (Required)";
            // 
            // listBoxCustomAttributes
            // 
            this.listBoxCustomAttributes.FormattingEnabled = true;
            this.listBoxCustomAttributes.Location = new System.Drawing.Point(43, 249);
            this.listBoxCustomAttributes.Name = "listBoxCustomAttributes";
            this.listBoxCustomAttributes.Size = new System.Drawing.Size(266, 108);
            this.listBoxCustomAttributes.TabIndex = 6;
            this.listBoxCustomAttributes.SelectedIndexChanged += new System.EventHandler(this.listBoxCustomAttributes_SelectedIndexChanged);
            this.listBoxCustomAttributes.SelectedValueChanged += new System.EventHandler(this.listBoxCustomAttributes_SelectedValueChanged);
            // 
            // customRadioButton3
            // 
            this.customRadioButton3.AutoSize = true;
            this.customRadioButton3.ComponentId = 0;
            this.customRadioButton3.Controller = null;
            this.customRadioButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customRadioButton3.Location = new System.Drawing.Point(24, 225);
            this.customRadioButton3.Name = "customRadioButton3";
            this.customRadioButton3.ParameterCategory = "Sensor";
            this.customRadioButton3.ParameterName = "Custom_Attribute_Sensor";
            this.customRadioButton3.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton3.Size = new System.Drawing.Size(138, 17);
            this.customRadioButton3.TabIndex = 5;
            this.customRadioButton3.Text = "Custom Attribute Sensor";
            this.customRadioButton3.UseVisualStyleBackColor = true;
            this.customRadioButton3.CheckedChanged += new System.EventHandler(this.customRadioButton3_CheckedChanged);
            // 
            // customRadioButton2
            // 
            this.customRadioButton2.AutoSize = true;
            this.customRadioButton2.ComponentId = 0;
            this.customRadioButton2.Controller = null;
            this.customRadioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customRadioButton2.Location = new System.Drawing.Point(24, 79);
            this.customRadioButton2.Name = "customRadioButton2";
            this.customRadioButton2.ParameterCategory = "Sensor";
            this.customRadioButton2.ParameterName = "Attribute_Sensor";
            this.customRadioButton2.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton2.Size = new System.Drawing.Size(100, 17);
            this.customRadioButton2.TabIndex = 3;
            this.customRadioButton2.Text = "Attribute Sensor";
            this.customRadioButton2.UseVisualStyleBackColor = true;
            this.customRadioButton2.CheckedChanged += new System.EventHandler(this.customRadioButton2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(40, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Range (m.)";
            // 
            // customParameterEnumBox1
            // 
            this.customParameterEnumBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customParameterEnumBox1.ComponentId = 0;
            this.customParameterEnumBox1.Controller = null;
            this.customParameterEnumBox1.EnumName = "Attributes";
            this.customParameterEnumBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customParameterEnumBox1.FormattingEnabled = true;
            this.customParameterEnumBox1.IsColorEnum = false;
            this.customParameterEnumBox1.Location = new System.Drawing.Point(43, 102);
            this.customParameterEnumBox1.Name = "customParameterEnumBox1";
            this.customParameterEnumBox1.ParameterCategory = "Sensor";
            this.customParameterEnumBox1.ParameterName = "Attribute";
            this.customParameterEnumBox1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customParameterEnumBox1.Size = new System.Drawing.Size(266, 108);
            this.customParameterEnumBox1.Sorted = true;
            this.customParameterEnumBox1.TabIndex = 4;
            // 
            // nndRange
            // 
            this.nndRange.ComponentId = 0;
            this.nndRange.Controller = null;
            this.nndRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nndRange.Location = new System.Drawing.Point(105, 46);
            this.nndRange.Name = "nndRange";
            this.nndRange.ParameterCategory = "Sensor";
            this.nndRange.ParameterName = "Range";
            this.nndRange.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndRange.Size = new System.Drawing.Size(72, 20);
            this.nndRange.TabIndex = 2;
            this.nndRange.Text = "0";
            this.nndRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndRange.Value = 0;
            // 
            // customRadioButton1
            // 
            this.customRadioButton1.AutoSize = true;
            this.customRadioButton1.Checked = true;
            this.customRadioButton1.ComponentId = 0;
            this.customRadioButton1.Controller = null;
            this.customRadioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customRadioButton1.Location = new System.Drawing.Point(24, 23);
            this.customRadioButton1.Name = "customRadioButton1";
            this.customRadioButton1.ParameterCategory = "Sensor";
            this.customRadioButton1.ParameterName = "Global_Sensor";
            this.customRadioButton1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton1.Size = new System.Drawing.Size(91, 17);
            this.customRadioButton1.TabIndex = 1;
            this.customRadioButton1.TabStop = true;
            this.customRadioButton1.Text = "Global Sensor";
            this.customRadioButton1.UseVisualStyleBackColor = true;
            this.customRadioButton1.CheckedChanged += new System.EventHandler(this.customRadioButton1_CheckedChanged);
            // 
            // viewComponentPanel1
            // 
            this.viewComponentPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewComponentPanel1.Location = new System.Drawing.Point(0, 0);
            this.viewComponentPanel1.Name = "viewComponentPanel1";
            this.viewComponentPanel1.Size = new System.Drawing.Size(150, 150);
            this.viewComponentPanel1.TabIndex = 0;
            // 
            // Sensor
            // 
            this.Controls.Add(this.customTabControl1);
            this.Name = "Sensor";
            this.Size = new System.Drawing.Size(361, 451);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Component_Packages.ViewComponentPanel viewComponentPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private AME.Views.View_Components.CustomNonnegativeDouble nndRange;
        private AME.Views.View_Components.CustomParameterEnumBox customParameterEnumBox1;
        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomRadioButton customRadioButton2;
        private AME.Views.View_Components.CustomRadioButton customRadioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private AME.Views.View_Components.CustomRadioButton customRadioButton3;
        private System.Windows.Forms.ListBox listBoxCustomAttributes;

    }
}
