namespace VSG.ViewComponents
{
    partial class ScoringRulePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ob1WhereLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.ob1WhatLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.ob1WhereEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.ob1WhatEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.ob1WhoEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toStateComboBox1 = new VSG.ViewComponents.StateComboBox(this.components);
            this.fromStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.ob2WhereLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.ob2WhereEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.ob2WhatLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.ob2WhatEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.ob2WhoEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.scoreIncrementTextBox1 = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.ruleTypeEnumBox = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.tabPage = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 53;
            this.label8.Text = "Score Increment";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 51;
            this.label7.Text = "From State";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "New State";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ob1WhereLinkBox);
            this.groupBox1.Controls.Add(this.ob1WhatLinkBox);
            this.groupBox1.Controls.Add(this.ob1WhereEnumBox);
            this.groupBox1.Controls.Add(this.ob1WhatEnumBox);
            this.groupBox1.Controls.Add(this.ob1WhoEnumBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 209);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object 1 (Required)";
            // 
            // ob1WhereLinkBox
            // 
            this.ob1WhereLinkBox.CheckLinkLevel = ((uint)(1u));
            this.ob1WhereLinkBox.CheckOnClick = true;
            this.ob1WhereLinkBox.ConnectFromId = -1;
            this.ob1WhereLinkBox.ConnectLinkType = null;
            this.ob1WhereLinkBox.ConnectRootId = -1;
            this.ob1WhereLinkBox.Controller = null;
            this.ob1WhereLinkBox.DisplayComponentType = null;
            this.ob1WhereLinkBox.DisplayLinkType = null;
            this.ob1WhereLinkBox.DisplayParameterCategory = "";
            this.ob1WhereLinkBox.DisplayParameterName = "";
            this.ob1WhereLinkBox.DisplayRecursiveCheck = false;
            this.ob1WhereLinkBox.DisplayRootId = -1;
            this.ob1WhereLinkBox.FilterResult = false;
            this.ob1WhereLinkBox.FormattingEnabled = true;
            this.ob1WhereLinkBox.Location = new System.Drawing.Point(202, 138);
            this.ob1WhereLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob1WhereLinkBox.Name = "ob1WhereLinkBox";
            this.ob1WhereLinkBox.OneToMany = false;
            this.ob1WhereLinkBox.ParameterFilterCategory = "";
            this.ob1WhereLinkBox.ParameterFilterName = "";
            this.ob1WhereLinkBox.ParameterFilterValue = "";
            this.ob1WhereLinkBox.Size = new System.Drawing.Size(140, 49);
            this.ob1WhereLinkBox.TabIndex = 7;
            // 
            // ob1WhatLinkBox
            // 
            this.ob1WhatLinkBox.CheckLinkLevel = ((uint)(1u));
            this.ob1WhatLinkBox.CheckOnClick = true;
            this.ob1WhatLinkBox.ConnectFromId = -1;
            this.ob1WhatLinkBox.ConnectLinkType = null;
            this.ob1WhatLinkBox.ConnectRootId = -1;
            this.ob1WhatLinkBox.Controller = null;
            this.ob1WhatLinkBox.DisplayComponentType = null;
            this.ob1WhatLinkBox.DisplayLinkType = null;
            this.ob1WhatLinkBox.DisplayParameterCategory = "";
            this.ob1WhatLinkBox.DisplayParameterName = "";
            this.ob1WhatLinkBox.DisplayRecursiveCheck = false;
            this.ob1WhatLinkBox.DisplayRootId = -1;
            this.ob1WhatLinkBox.FilterResult = false;
            this.ob1WhatLinkBox.FormattingEnabled = true;
            this.ob1WhatLinkBox.Location = new System.Drawing.Point(202, 78);
            this.ob1WhatLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob1WhatLinkBox.Name = "ob1WhatLinkBox";
            this.ob1WhatLinkBox.OneToMany = false;
            this.ob1WhatLinkBox.ParameterFilterCategory = "";
            this.ob1WhatLinkBox.ParameterFilterName = "";
            this.ob1WhatLinkBox.ParameterFilterValue = "";
            this.ob1WhatLinkBox.Size = new System.Drawing.Size(140, 49);
            this.ob1WhatLinkBox.TabIndex = 5;
            // 
            // ob1WhereEnumBox
            // 
            this.ob1WhereEnumBox.ComponentId = -1;
            this.ob1WhereEnumBox.Controller = null;
            this.ob1WhereEnumBox.EnumName = null;
            this.ob1WhereEnumBox.FormattingEnabled = true;
            this.ob1WhereEnumBox.IsColorEnum = false;
            this.ob1WhereEnumBox.Location = new System.Drawing.Point(63, 138);
            this.ob1WhereEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob1WhereEnumBox.Name = "ob1WhereEnumBox";
            this.ob1WhereEnumBox.ParameterCategory = null;
            this.ob1WhereEnumBox.ParameterName = null;
            this.ob1WhereEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob1WhereEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob1WhereEnumBox.TabIndex = 6;
            // 
            // ob1WhatEnumBox
            // 
            this.ob1WhatEnumBox.ComponentId = -1;
            this.ob1WhatEnumBox.Controller = null;
            this.ob1WhatEnumBox.EnumName = null;
            this.ob1WhatEnumBox.FormattingEnabled = true;
            this.ob1WhatEnumBox.IsColorEnum = false;
            this.ob1WhatEnumBox.Location = new System.Drawing.Point(63, 78);
            this.ob1WhatEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob1WhatEnumBox.Name = "ob1WhatEnumBox";
            this.ob1WhatEnumBox.ParameterCategory = null;
            this.ob1WhatEnumBox.ParameterName = null;
            this.ob1WhatEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob1WhatEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob1WhatEnumBox.TabIndex = 4;
            // 
            // ob1WhoEnumBox
            // 
            this.ob1WhoEnumBox.ComponentId = -1;
            this.ob1WhoEnumBox.Controller = null;
            this.ob1WhoEnumBox.EnumName = null;
            this.ob1WhoEnumBox.FormattingEnabled = true;
            this.ob1WhoEnumBox.IsColorEnum = false;
            this.ob1WhoEnumBox.Location = new System.Drawing.Point(63, 18);
            this.ob1WhoEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob1WhoEnumBox.Name = "ob1WhoEnumBox";
            this.ob1WhoEnumBox.ParameterCategory = null;
            this.ob1WhoEnumBox.ParameterName = null;
            this.ob1WhoEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob1WhoEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob1WhoEnumBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Where?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "What?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "Who?";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(117, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Scoring Rule Type:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.toStateComboBox1);
            this.groupBox2.Controls.Add(this.fromStateComboBox);
            this.groupBox2.Controls.Add(this.ob2WhereLinkBox);
            this.groupBox2.Controls.Add(this.ob2WhereEnumBox);
            this.groupBox2.Controls.Add(this.ob2WhatLinkBox);
            this.groupBox2.Controls.Add(this.ob2WhatEnumBox);
            this.groupBox2.Controls.Add(this.ob2WhoEnumBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(12, 328);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 262);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Object 2 (Optional)";
            // 
            // toStateComboBox1
            // 
            this.toStateComboBox1.ComponentId = -1;
            this.toStateComboBox1.Controller = null;
            this.toStateComboBox1.FormattingEnabled = true;
            this.toStateComboBox1.Location = new System.Drawing.Point(63, 219);
            this.toStateComboBox1.Name = "toStateComboBox1";
            this.toStateComboBox1.ParameterCategory = "Rule";
            this.toStateComboBox1.ParameterName = "FromState";
            this.toStateComboBox1.ShowAllStates = true;
            this.toStateComboBox1.Size = new System.Drawing.Size(176, 21);
            this.toStateComboBox1.SpeciesId = -1;
            this.toStateComboBox1.TabIndex = 14;
            // 
            // fromStateComboBox
            // 
            this.fromStateComboBox.ComponentId = -1;
            this.fromStateComboBox.Controller = null;
            this.fromStateComboBox.FormattingEnabled = true;
            this.fromStateComboBox.Location = new System.Drawing.Point(63, 195);
            this.fromStateComboBox.Name = "fromStateComboBox";
            this.fromStateComboBox.ParameterCategory = "Rule";
            this.fromStateComboBox.ParameterName = "NewState";
            this.fromStateComboBox.ShowAllStates = true;
            this.fromStateComboBox.Size = new System.Drawing.Size(176, 21);
            this.fromStateComboBox.SpeciesId = -1;
            this.fromStateComboBox.TabIndex = 13;
            // 
            // ob2WhereLinkBox
            // 
            this.ob2WhereLinkBox.CheckLinkLevel = ((uint)(1u));
            this.ob2WhereLinkBox.CheckOnClick = true;
            this.ob2WhereLinkBox.ConnectFromId = -1;
            this.ob2WhereLinkBox.ConnectLinkType = null;
            this.ob2WhereLinkBox.ConnectRootId = -1;
            this.ob2WhereLinkBox.Controller = null;
            this.ob2WhereLinkBox.DisplayComponentType = null;
            this.ob2WhereLinkBox.DisplayLinkType = null;
            this.ob2WhereLinkBox.DisplayParameterCategory = "";
            this.ob2WhereLinkBox.DisplayParameterName = "";
            this.ob2WhereLinkBox.DisplayRecursiveCheck = false;
            this.ob2WhereLinkBox.DisplayRootId = -1;
            this.ob2WhereLinkBox.FilterResult = false;
            this.ob2WhereLinkBox.FormattingEnabled = true;
            this.ob2WhereLinkBox.Location = new System.Drawing.Point(201, 138);
            this.ob2WhereLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob2WhereLinkBox.Name = "ob2WhereLinkBox";
            this.ob2WhereLinkBox.OneToMany = false;
            this.ob2WhereLinkBox.ParameterFilterCategory = "";
            this.ob2WhereLinkBox.ParameterFilterName = "";
            this.ob2WhereLinkBox.ParameterFilterValue = "";
            this.ob2WhereLinkBox.Size = new System.Drawing.Size(140, 49);
            this.ob2WhereLinkBox.TabIndex = 12;
            // 
            // ob2WhereEnumBox
            // 
            this.ob2WhereEnumBox.ComponentId = -1;
            this.ob2WhereEnumBox.Controller = null;
            this.ob2WhereEnumBox.EnumName = null;
            this.ob2WhereEnumBox.FormattingEnabled = true;
            this.ob2WhereEnumBox.IsColorEnum = false;
            this.ob2WhereEnumBox.Location = new System.Drawing.Point(63, 138);
            this.ob2WhereEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob2WhereEnumBox.Name = "ob2WhereEnumBox";
            this.ob2WhereEnumBox.ParameterCategory = null;
            this.ob2WhereEnumBox.ParameterName = null;
            this.ob2WhereEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob2WhereEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob2WhereEnumBox.TabIndex = 11;
            // 
            // ob2WhatLinkBox
            // 
            this.ob2WhatLinkBox.CheckLinkLevel = ((uint)(1u));
            this.ob2WhatLinkBox.CheckOnClick = true;
            this.ob2WhatLinkBox.ConnectFromId = -1;
            this.ob2WhatLinkBox.ConnectLinkType = null;
            this.ob2WhatLinkBox.ConnectRootId = -1;
            this.ob2WhatLinkBox.Controller = null;
            this.ob2WhatLinkBox.DisplayComponentType = null;
            this.ob2WhatLinkBox.DisplayLinkType = null;
            this.ob2WhatLinkBox.DisplayParameterCategory = "";
            this.ob2WhatLinkBox.DisplayParameterName = "";
            this.ob2WhatLinkBox.DisplayRecursiveCheck = false;
            this.ob2WhatLinkBox.DisplayRootId = -1;
            this.ob2WhatLinkBox.FilterResult = false;
            this.ob2WhatLinkBox.FormattingEnabled = true;
            this.ob2WhatLinkBox.Location = new System.Drawing.Point(202, 78);
            this.ob2WhatLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob2WhatLinkBox.Name = "ob2WhatLinkBox";
            this.ob2WhatLinkBox.OneToMany = false;
            this.ob2WhatLinkBox.ParameterFilterCategory = "";
            this.ob2WhatLinkBox.ParameterFilterName = "";
            this.ob2WhatLinkBox.ParameterFilterValue = "";
            this.ob2WhatLinkBox.Size = new System.Drawing.Size(140, 49);
            this.ob2WhatLinkBox.TabIndex = 10;
            // 
            // ob2WhatEnumBox
            // 
            this.ob2WhatEnumBox.ComponentId = -1;
            this.ob2WhatEnumBox.Controller = null;
            this.ob2WhatEnumBox.EnumName = null;
            this.ob2WhatEnumBox.FormattingEnabled = true;
            this.ob2WhatEnumBox.IsColorEnum = false;
            this.ob2WhatEnumBox.Location = new System.Drawing.Point(63, 78);
            this.ob2WhatEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob2WhatEnumBox.Name = "ob2WhatEnumBox";
            this.ob2WhatEnumBox.ParameterCategory = null;
            this.ob2WhatEnumBox.ParameterName = null;
            this.ob2WhatEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob2WhatEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob2WhatEnumBox.TabIndex = 9;
            // 
            // ob2WhoEnumBox
            // 
            this.ob2WhoEnumBox.ComponentId = -1;
            this.ob2WhoEnumBox.Controller = null;
            this.ob2WhoEnumBox.EnumName = null;
            this.ob2WhoEnumBox.FormattingEnabled = true;
            this.ob2WhoEnumBox.IsColorEnum = false;
            this.ob2WhoEnumBox.Location = new System.Drawing.Point(63, 18);
            this.ob2WhoEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ob2WhoEnumBox.Name = "ob2WhoEnumBox";
            this.ob2WhoEnumBox.ParameterCategory = null;
            this.ob2WhoEnumBox.ParameterName = null;
            this.ob2WhoEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ob2WhoEnumBox.Size = new System.Drawing.Size(134, 56);
            this.ob2WhoEnumBox.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 45;
            this.label10.Text = "Where?";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 43;
            this.label11.Text = "What?";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "Who?";
            // 
            // scoreIncrementTextBox1
            // 
            this.scoreIncrementTextBox1.ComponentId = 0;
            this.scoreIncrementTextBox1.Controller = null;
            this.scoreIncrementTextBox1.Location = new System.Drawing.Point(12, 37);
            this.scoreIncrementTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.scoreIncrementTextBox1.Name = "scoreIncrementTextBox1";
            this.scoreIncrementTextBox1.ParameterCategory = "Rule";
            this.scoreIncrementTextBox1.ParameterName = "Increment";
            this.scoreIncrementTextBox1.ParameterType = AME.Controllers.eParamParentType.Component;
            this.scoreIncrementTextBox1.Size = new System.Drawing.Size(76, 20);
            this.scoreIncrementTextBox1.TabIndex = 1;
            this.scoreIncrementTextBox1.UseDelimiter = false;
            // 
            // ruleTypeEnumBox
            // 
            this.ruleTypeEnumBox.ComponentId = -1;
            this.ruleTypeEnumBox.Controller = null;
            this.ruleTypeEnumBox.EnumName = null;
            this.ruleTypeEnumBox.FormattingEnabled = true;
            this.ruleTypeEnumBox.IsColorEnum = false;
            this.ruleTypeEnumBox.Location = new System.Drawing.Point(120, 37);
            this.ruleTypeEnumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ruleTypeEnumBox.Name = "ruleTypeEnumBox";
            this.ruleTypeEnumBox.ParameterCategory = null;
            this.ruleTypeEnumBox.ParameterName = null;
            this.ruleTypeEnumBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ruleTypeEnumBox.Size = new System.Drawing.Size(134, 43);
            this.ruleTypeEnumBox.TabIndex = 2;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.tabPage);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(392, 649);
            this.customTabControl1.TabIndex = 60;
            // 
            // tabPage
            // 
            this.tabPage.AutoScroll = true;
            this.tabPage.Controls.Add(this.panel1);
            this.tabPage.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabPage.Description = "label1";
            this.tabPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage.Name = "tabPage";
            this.tabPage.Size = new System.Drawing.Size(384, 623);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Scoring Parameters";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.MinimumSize = new System.Drawing.Size(427, 600);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 600);
            this.panel1.TabIndex = 60;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.scoreIncrementTextBox1);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.ruleTypeEnumBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(363, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rule Parameters (Required)";
            // 
            // ScoringRulePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "ScoringRulePanel";
            this.Size = new System.Drawing.Size(392, 649);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.customTabControl1.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private AME.Views.View_Components.CustomParameterEnumBox ob1WhoEnumBox;
        private AME.Views.View_Components.CustomParameterEnumBox ob1WhatEnumBox;
        private AME.Views.View_Components.CustomParameterEnumBox ob1WhereEnumBox;
        private AME.Views.View_Components.CustomLinkBox ob1WhereLinkBox;
        private AME.Views.View_Components.CustomLinkBox ob1WhatLinkBox;
        private AME.Views.View_Components.CustomParameterEnumBox ob2WhoEnumBox;
        private AME.Views.View_Components.CustomParameterEnumBox ob2WhatEnumBox;
        private AME.Views.View_Components.CustomLinkBox ob2WhatLinkBox;
        private AME.Views.View_Components.CustomParameterEnumBox ob2WhereEnumBox;
        private AME.Views.View_Components.CustomLinkBox ob2WhereLinkBox;
        private AME.Views.View_Components.CustomParameterEnumBox ruleTypeEnumBox;
        private AME.Views.View_Components.CustomParameterTextBox scoreIncrementTextBox1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage tabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private StateComboBox toStateComboBox1;
        private StateComboBox fromStateComboBox;
    }
}
