namespace VSG.ViewComponents
{
    partial class Emitter
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
            this.listBoxCustomAttribute = new System.Windows.Forms.ListBox();
            this.customParameterEnumBox1 = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.customRadioButton2 = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.customRadioButton1 = new AME.Views.View_Components.CustomRadioButton(this.components);
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
            this.customTabControl1.Size = new System.Drawing.Size(234, 311);
            this.customTabControl1.TabIndex = 39;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(226, 285);
            this.customTabPage1.TabIndex = 1;
            this.customTabPage1.Text = "Emitter";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 250);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxCustomAttribute);
            this.groupBox1.Controls.Add(this.customRadioButton2);
            this.groupBox1.Controls.Add(this.customRadioButton1);
            this.groupBox1.Controls.Add(this.customParameterEnumBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 250);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Emitter Parameters (Required)";
            // 
            // listBoxCustomAttribute
            // 
            this.listBoxCustomAttribute.Enabled = false;
            this.listBoxCustomAttribute.FormattingEnabled = true;
            this.listBoxCustomAttribute.Location = new System.Drawing.Point(25, 153);
            this.listBoxCustomAttribute.Name = "listBoxCustomAttribute";
            this.listBoxCustomAttribute.Size = new System.Drawing.Size(179, 82);
            this.listBoxCustomAttribute.TabIndex = 3;
            this.listBoxCustomAttribute.SelectedValueChanged += new System.EventHandler(this.listBoxCustomAttribute_SelectedValueChanged);
            // 
            // customParameterEnumBox1
            // 
            this.customParameterEnumBox1.ComponentId = -1;
            this.customParameterEnumBox1.Controller = null;
            this.customParameterEnumBox1.EnumName = "Attributes";
            this.customParameterEnumBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customParameterEnumBox1.FormattingEnabled = true;
            this.customParameterEnumBox1.Location = new System.Drawing.Point(25, 42);
            this.customParameterEnumBox1.Name = "customParameterEnumBox1";
            this.customParameterEnumBox1.ParameterCategory = "Emitter";
            this.customParameterEnumBox1.ParameterName = "Attribute";
            this.customParameterEnumBox1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customParameterEnumBox1.Size = new System.Drawing.Size(179, 82);
            this.customParameterEnumBox1.TabIndex = 1;
            this.customParameterEnumBox1.SelectedValueChanged += new System.EventHandler(this.customParameterEnumBox1_SelectedValueChanged);
            // 
            // customRadioButton2
            // 
            this.customRadioButton2.AutoSize = true;
            this.customRadioButton2.Checked = true;
            this.customRadioButton2.ComponentId = 0;
            this.customRadioButton2.Controller = null;
            this.customRadioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customRadioButton2.Location = new System.Drawing.Point(6, 19);
            this.customRadioButton2.Name = "customRadioButton2";
            this.customRadioButton2.ParameterCategory = "Emitter";
            this.customRadioButton2.ParameterName = "Attribute_Emitter";
            this.customRadioButton2.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton2.Size = new System.Drawing.Size(144, 17);
            this.customRadioButton2.TabIndex = 0;
            this.customRadioButton2.TabStop = true;
            this.customRadioButton2.Text = "Emits a standard attribute";
            this.customRadioButton2.UseVisualStyleBackColor = true;
            this.customRadioButton2.CheckedChanged += new System.EventHandler(this.customRadioButton2_CheckedChanged);
            // 
            // customRadioButton1
            // 
            this.customRadioButton1.AutoSize = true;
            this.customRadioButton1.ComponentId = 0;
            this.customRadioButton1.Controller = null;
            this.customRadioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customRadioButton1.Location = new System.Drawing.Point(6, 130);
            this.customRadioButton1.Name = "customRadioButton1";
            this.customRadioButton1.ParameterCategory = "Emitter";
            this.customRadioButton1.ParameterName = "Custom_Attribute_Emitter";
            this.customRadioButton1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customRadioButton1.Size = new System.Drawing.Size(137, 17);
            this.customRadioButton1.TabIndex = 2;
            this.customRadioButton1.Text = "Emits a custom attribute";
            this.customRadioButton1.UseVisualStyleBackColor = true;
            this.customRadioButton1.CheckedChanged += new System.EventHandler(this.customRadioButton1_CheckedChanged);
            // 
            // Emitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "Emitter";
            this.Size = new System.Drawing.Size(234, 311);
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
        private AME.Views.View_Components.CustomParameterEnumBox customParameterEnumBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxCustomAttribute;
        private AME.Views.View_Components.CustomRadioButton customRadioButton2;
        private AME.Views.View_Components.CustomRadioButton customRadioButton1;
    }
}
