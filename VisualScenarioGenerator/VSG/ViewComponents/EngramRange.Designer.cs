namespace VSG.ViewComponents
{
    partial class EngramRange
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
            this.useEngramRangeCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customCheckBoxHasUnit = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.eventIDEngramUnit = new VSG.ViewComponents.EngramUnitID();
            this.customParameterTextBoxCompareValue = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.comboBoxCompareInequality = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonExclude = new System.Windows.Forms.RadioButton();
            this.radioButtonInclude = new System.Windows.Forms.RadioButton();
            this.radioButtonCompare = new System.Windows.Forms.RadioButton();
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.excludeTextBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.includeTextBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // useEngramRangeCheckbox
            // 
            this.useEngramRangeCheckbox.AutoSize = true;
            this.useEngramRangeCheckbox.Location = new System.Drawing.Point(2, 2);
            this.useEngramRangeCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.useEngramRangeCheckbox.Name = "useEngramRangeCheckbox";
            this.useEngramRangeCheckbox.Size = new System.Drawing.Size(125, 17);
            this.useEngramRangeCheckbox.TabIndex = 0;
            this.useEngramRangeCheckbox.Text = "Use Engram Range?";
            this.useEngramRangeCheckbox.UseVisualStyleBackColor = true;
            this.useEngramRangeCheckbox.CheckedChanged += new System.EventHandler(this.useEngramRangeCheckbox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.customCheckBoxHasUnit);
            this.groupBox1.Controls.Add(this.eventIDEngramUnit);
            this.groupBox1.Controls.Add(this.customParameterTextBoxCompareValue);
            this.groupBox1.Controls.Add(this.comboBoxCompareInequality);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.radioButtonExclude);
            this.groupBox1.Controls.Add(this.radioButtonInclude);
            this.groupBox1.Controls.Add(this.radioButtonCompare);
            this.groupBox1.Controls.Add(this.customLinkBox1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.excludeTextBox);
            this.groupBox1.Controls.Add(this.includeTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(550, 376);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Engram Info";
            // 
            // customCheckBoxHasUnit
            // 
            this.customCheckBoxHasUnit.AutoSize = true;
            this.customCheckBoxHasUnit.ComponentId = -1;
            this.customCheckBoxHasUnit.Controller = null;
            this.customCheckBoxHasUnit.Location = new System.Drawing.Point(7, 106);
            this.customCheckBoxHasUnit.Name = "customCheckBoxHasUnit";
            this.customCheckBoxHasUnit.ParameterCategory = "EngramRange";
            this.customCheckBoxHasUnit.ParameterName = "Engram Unit Selected";
            this.customCheckBoxHasUnit.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBoxHasUnit.Size = new System.Drawing.Size(126, 17);
            this.customCheckBoxHasUnit.TabIndex = 2;
            this.customCheckBoxHasUnit.Text = "Range Specifies Unit";
            this.customCheckBoxHasUnit.UseVisualStyleBackColor = true;
            this.customCheckBoxHasUnit.CheckedChanged += new System.EventHandler(this.checkBoxHasUnit_CheckedChanged);
            // 
            // eventIDEngramUnit
            // 
            this.eventIDEngramUnit.Controller = null;
            this.eventIDEngramUnit.DisplayID = -1;
            this.eventIDEngramUnit.Enabled = false;
            this.eventIDEngramUnit.Location = new System.Drawing.Point(167, 86);
            this.eventIDEngramUnit.Margin = new System.Windows.Forms.Padding(2);
            this.eventIDEngramUnit.Name = "eventIDEngramUnit";
            this.eventIDEngramUnit.ParentID = -1;
            this.eventIDEngramUnit.Size = new System.Drawing.Size(375, 103);
            this.eventIDEngramUnit.TabIndex = 3;
            // 
            // customParameterTextBoxCompareValue
            // 
            this.customParameterTextBoxCompareValue.ComponentId = -1;
            this.customParameterTextBoxCompareValue.Controller = null;
            this.customParameterTextBoxCompareValue.Enabled = false;
            this.customParameterTextBoxCompareValue.Location = new System.Drawing.Point(231, 194);
            this.customParameterTextBoxCompareValue.Name = "customParameterTextBoxCompareValue";
            this.customParameterTextBoxCompareValue.ParameterCategory = "EngramRange";
            this.customParameterTextBoxCompareValue.ParameterName = "Engram Compare Value";
            this.customParameterTextBoxCompareValue.ParameterType = AME.Controllers.eParamParentType.Component;
            this.customParameterTextBoxCompareValue.Size = new System.Drawing.Size(113, 20);
            this.customParameterTextBoxCompareValue.TabIndex = 6;
            this.customParameterTextBoxCompareValue.UseDelimiter = false;
            // 
            // comboBoxCompareInequality
            // 
            this.comboBoxCompareInequality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCompareInequality.Enabled = false;
            this.comboBoxCompareInequality.FormattingEnabled = true;
            this.comboBoxCompareInequality.Items.AddRange(new object[] {
            "",
            ">",
            ">=",
            "<",
            "<=",
            "=="});
            this.comboBoxCompareInequality.Location = new System.Drawing.Point(167, 194);
            this.comboBoxCompareInequality.Name = "comboBoxCompareInequality";
            this.comboBoxCompareInequality.Size = new System.Drawing.Size(58, 21);
            this.comboBoxCompareInequality.TabIndex = 5;
            this.comboBoxCompareInequality.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompareInequality_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(29, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 49);
            this.label4.TabIndex = 4;
            this.label4.Text = "Event happens if Engram Value is";
            // 
            // radioButtonExclude
            // 
            this.radioButtonExclude.AutoSize = true;
            this.radioButtonExclude.Location = new System.Drawing.Point(7, 317);
            this.radioButtonExclude.Name = "radioButtonExclude";
            this.radioButtonExclude.Size = new System.Drawing.Size(14, 13);
            this.radioButtonExclude.TabIndex = 9;
            this.radioButtonExclude.UseVisualStyleBackColor = true;
            this.radioButtonExclude.CheckedChanged += new System.EventHandler(this.EngramRangeRadioButtonChecked);
            // 
            // radioButtonInclude
            // 
            this.radioButtonInclude.AutoSize = true;
            this.radioButtonInclude.Location = new System.Drawing.Point(7, 258);
            this.radioButtonInclude.Name = "radioButtonInclude";
            this.radioButtonInclude.Size = new System.Drawing.Size(14, 13);
            this.radioButtonInclude.TabIndex = 12;
            this.radioButtonInclude.UseVisualStyleBackColor = true;
            this.radioButtonInclude.CheckedChanged += new System.EventHandler(this.EngramRangeRadioButtonChecked);
            // 
            // radioButtonCompare
            // 
            this.radioButtonCompare.AutoSize = true;
            this.radioButtonCompare.Location = new System.Drawing.Point(7, 194);
            this.radioButtonCompare.Name = "radioButtonCompare";
            this.radioButtonCompare.Size = new System.Drawing.Size(14, 13);
            this.radioButtonCompare.TabIndex = 11;
            this.radioButtonCompare.UseVisualStyleBackColor = true;
            this.radioButtonCompare.CheckedChanged += new System.EventHandler(this.EngramRangeRadioButtonChecked);
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = "EventEngram";
            this.customLinkBox1.ConnectRootId = -1;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = "Engram";
            this.customLinkBox1.DisplayLinkType = "Scenario";
            this.customLinkBox1.DisplayParameterCategory = "";
            this.customLinkBox1.DisplayParameterName = "";
            this.customLinkBox1.DisplayRecursiveCheck = false;
            this.customLinkBox1.DisplayRootId = -1;
            this.customLinkBox1.FilterResult = false;
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.Location = new System.Drawing.Point(167, 15);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = false;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(176, 64);
            this.customLinkBox1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 317);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 39);
            this.label5.TabIndex = 9;
            this.label5.Text = "Event happens if\r\nengram is NOT one\r\nof these values";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 258);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 39);
            this.label3.TabIndex = 7;
            this.label3.Text = "Event happens if\r\nengram is one of\r\nthese values";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 240);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "One value per line.";
            // 
            // excludeTextBox
            // 
            this.excludeTextBox.ComponentId = 0;
            this.excludeTextBox.Controller = null;
            this.excludeTextBox.Enabled = false;
            this.excludeTextBox.Location = new System.Drawing.Point(167, 314);
            this.excludeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.excludeTextBox.Multiline = true;
            this.excludeTextBox.Name = "excludeTextBox";
            this.excludeTextBox.ParameterCategory = null;
            this.excludeTextBox.ParameterName = null;
            this.excludeTextBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.excludeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.excludeTextBox.Size = new System.Drawing.Size(176, 50);
            this.excludeTextBox.TabIndex = 10;
            this.excludeTextBox.UseDelimiter = true;
            this.excludeTextBox.TextChanged += new System.EventHandler(this.excludeTextBox_TextChanged);
            // 
            // includeTextBox
            // 
            this.includeTextBox.ComponentId = 0;
            this.includeTextBox.Controller = null;
            this.includeTextBox.Enabled = false;
            this.includeTextBox.Location = new System.Drawing.Point(167, 255);
            this.includeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.includeTextBox.Multiline = true;
            this.includeTextBox.Name = "includeTextBox";
            this.includeTextBox.ParameterCategory = null;
            this.includeTextBox.ParameterName = null;
            this.includeTextBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.includeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.includeTextBox.Size = new System.Drawing.Size(177, 50);
            this.includeTextBox.TabIndex = 8;
            this.includeTextBox.UseDelimiter = true;
            this.includeTextBox.TextChanged += new System.EventHandler(this.includeTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Engram Name:";
            // 
            // EngramRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.useEngramRangeCheckbox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(279, 238);
            this.Name = "EngramRange";
            this.Size = new System.Drawing.Size(566, 403);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox useEngramRangeCheckbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomParameterTextBox excludeTextBox;
        private AME.Views.View_Components.CustomParameterTextBox includeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;
        private System.Windows.Forms.RadioButton radioButtonExclude;
        private System.Windows.Forms.RadioButton radioButtonInclude;
        private System.Windows.Forms.RadioButton radioButtonCompare;
        private AME.Views.View_Components.CustomParameterTextBox customParameterTextBoxCompareValue;
        private System.Windows.Forms.ComboBox comboBoxCompareInequality;
        private System.Windows.Forms.Label label4;
        private EngramUnitID eventIDEngramUnit;
        private AME.Views.View_Components.CustomCheckBox customCheckBoxHasUnit;
    }
}
