namespace VSG.ViewComponents
{
    partial class DecisionMaker
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
            this.colorSwatch = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRole = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.customParameterEnumBox1 = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.customLinkBoxWhiteboardWith = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.customLinkBoxSpeakWith = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.customCheckBox2 = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.customCheckBox1 = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.customLinkBoxReportTo = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.customLinkBoxChatWith = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBriefing = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.customCheckBox3 = new AME.Views.View_Components.CustomCheckBox(this.components);
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
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(588, 763);
            this.customTabControl1.TabIndex = 46;
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
            this.customTabPage1.Size = new System.Drawing.Size(580, 734);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Decision Maker";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 697);
            this.panel1.TabIndex = 48;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.colorSwatch);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtRole);
            this.groupBox2.Controls.Add(this.customParameterEnumBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.customLinkBox1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 9);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(452, 206);
            this.groupBox2.TabIndex = 53;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Decision Maker Parameters (Required)";
            // 
            // colorSwatch
            // 
            this.colorSwatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.colorSwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorSwatch.ForeColor = System.Drawing.Color.White;
            this.colorSwatch.Location = new System.Drawing.Point(264, 20);
            this.colorSwatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colorSwatch.Name = "colorSwatch";
            this.colorSwatch.Size = new System.Drawing.Size(72, 23);
            this.colorSwatch.TabIndex = 52;
            this.colorSwatch.Text = "Sample";
            this.colorSwatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 41;
            this.label2.Text = "Role:";
            // 
            // txtRole
            // 
            this.txtRole.ComponentId = 0;
            this.txtRole.Controller = null;
            this.txtRole.Location = new System.Drawing.Point(8, 47);
            this.txtRole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRole.Name = "txtRole";
            this.txtRole.ParameterCategory = "DecisionMaker";
            this.txtRole.ParameterName = "Role";
            this.txtRole.ParameterType = AME.Controllers.eParamParentType.Component;
            this.txtRole.Size = new System.Drawing.Size(201, 23);
            this.txtRole.TabIndex = 1;
            this.txtRole.UseDelimiter = false;
            // 
            // customParameterEnumBox1
            // 
            this.customParameterEnumBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.customParameterEnumBox1.ComponentId = 0;
            this.customParameterEnumBox1.Controller = null;
            this.customParameterEnumBox1.EnumName = "Colors";
            this.customParameterEnumBox1.FormattingEnabled = true;
            this.customParameterEnumBox1.IsColorEnum = true;
            this.customParameterEnumBox1.ItemHeight = 17;
            this.customParameterEnumBox1.Location = new System.Drawing.Point(219, 50);
            this.customParameterEnumBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customParameterEnumBox1.Name = "customParameterEnumBox1";
            this.customParameterEnumBox1.ParameterCategory = "DecisionMaker";
            this.customParameterEnumBox1.ParameterName = "Color";
            this.customParameterEnumBox1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customParameterEnumBox1.Size = new System.Drawing.Size(224, 106);
            this.customParameterEnumBox1.TabIndex = 3;
            this.customParameterEnumBox1.SelectedIndexChanged += new System.EventHandler(this.customParameterEnumBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 51;
            this.label1.Text = "Color:";
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = "DecisionMakerTeam";
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
            this.customLinkBox1.Location = new System.Drawing.Point(8, 101);
            this.customLinkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = false;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(201, 58);
            this.customLinkBox1.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 75);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 46;
            this.label7.Text = "Team:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customCheckBox3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.customLinkBoxWhiteboardWith);
            this.groupBox1.Controls.Add(this.customLinkBoxSpeakWith);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.customCheckBox2);
            this.groupBox1.Controls.Add(this.customCheckBox1);
            this.groupBox1.Controls.Add(this.customLinkBoxReportTo);
            this.groupBox1.Controls.Add(this.customLinkBoxChatWith);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtBriefing);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 222);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.MinimumSize = new System.Drawing.Size(452, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(452, 470);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional Parameters";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(233, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 17);
            this.label8.TabIndex = 63;
            this.label8.Text = "Can Share Whiteboard with:";
            // 
            // customLinkBoxWhiteboardWith
            // 
            this.customLinkBoxWhiteboardWith.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxWhiteboardWith.CheckOnClick = true;
            this.customLinkBoxWhiteboardWith.ConnectFromId = -1;
            this.customLinkBoxWhiteboardWith.ConnectLinkType = "DecisionMakerCanWhiteboard";
            this.customLinkBoxWhiteboardWith.ConnectRootId = 0;
            this.customLinkBoxWhiteboardWith.Controller = null;
            this.customLinkBoxWhiteboardWith.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxWhiteboardWith.DisplayLinkType = "Scenario";
            this.customLinkBoxWhiteboardWith.DisplayParameterCategory = "";
            this.customLinkBoxWhiteboardWith.DisplayParameterName = "";
            this.customLinkBoxWhiteboardWith.DisplayRecursiveCheck = false;
            this.customLinkBoxWhiteboardWith.DisplayRootId = 0;
            this.customLinkBoxWhiteboardWith.FilterResult = false;
            this.customLinkBoxWhiteboardWith.FormattingEnabled = true;
            this.customLinkBoxWhiteboardWith.Location = new System.Drawing.Point(235, 43);
            this.customLinkBoxWhiteboardWith.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customLinkBoxWhiteboardWith.Name = "customLinkBoxWhiteboardWith";
            this.customLinkBoxWhiteboardWith.OneToMany = true;
            this.customLinkBoxWhiteboardWith.ParameterFilterCategory = "";
            this.customLinkBoxWhiteboardWith.ParameterFilterName = "";
            this.customLinkBoxWhiteboardWith.ParameterFilterValue = "";
            this.customLinkBoxWhiteboardWith.Size = new System.Drawing.Size(205, 76);
            this.customLinkBoxWhiteboardWith.TabIndex = 6;
            // 
            // customLinkBoxSpeakWith
            // 
            this.customLinkBoxSpeakWith.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxSpeakWith.CheckOnClick = true;
            this.customLinkBoxSpeakWith.ConnectFromId = -1;
            this.customLinkBoxSpeakWith.ConnectLinkType = "DecisionMakerCanSpeak";
            this.customLinkBoxSpeakWith.ConnectRootId = 0;
            this.customLinkBoxSpeakWith.Controller = null;
            this.customLinkBoxSpeakWith.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxSpeakWith.DisplayLinkType = "Scenario";
            this.customLinkBoxSpeakWith.DisplayParameterCategory = "";
            this.customLinkBoxSpeakWith.DisplayParameterName = "";
            this.customLinkBoxSpeakWith.DisplayRecursiveCheck = false;
            this.customLinkBoxSpeakWith.DisplayRootId = 0;
            this.customLinkBoxSpeakWith.FilterResult = false;
            this.customLinkBoxSpeakWith.FormattingEnabled = true;
            this.customLinkBoxSpeakWith.Location = new System.Drawing.Point(235, 162);
            this.customLinkBoxSpeakWith.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customLinkBoxSpeakWith.Name = "customLinkBoxSpeakWith";
            this.customLinkBoxSpeakWith.OneToMany = true;
            this.customLinkBoxSpeakWith.ParameterFilterCategory = "";
            this.customLinkBoxSpeakWith.ParameterFilterName = "";
            this.customLinkBoxSpeakWith.ParameterFilterValue = "";
            this.customLinkBoxSpeakWith.Size = new System.Drawing.Size(205, 76);
            this.customLinkBoxSpeakWith.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(232, 142);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 17);
            this.label5.TabIndex = 60;
            this.label5.Text = "Can Speak With:";
            // 
            // customCheckBox2
            // 
            this.customCheckBox2.AutoSize = true;
            this.customCheckBox2.ComponentId = -1;
            this.customCheckBox2.Controller = null;
            this.customCheckBox2.Location = new System.Drawing.Point(237, 265);
            this.customCheckBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customCheckBox2.Name = "customCheckBox2";
            this.customCheckBox2.ParameterCategory = "DecisionMaker";
            this.customCheckBox2.ParameterName = "CanForceAssetTransfers";
            this.customCheckBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.customCheckBox2.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBox2.Size = new System.Drawing.Size(203, 21);
            this.customCheckBox2.TabIndex = 9;
            this.customCheckBox2.Text = "Can Force Asset Transfers:";
            this.customCheckBox2.UseVisualStyleBackColor = true;
            // 
            // customCheckBox1
            // 
            this.customCheckBox1.AutoSize = true;
            this.customCheckBox1.ComponentId = -1;
            this.customCheckBox1.Controller = null;
            this.customCheckBox1.Location = new System.Drawing.Point(8, 265);
            this.customCheckBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customCheckBox1.Name = "customCheckBox1";
            this.customCheckBox1.ParameterCategory = "DecisionMaker";
            this.customCheckBox1.ParameterName = "CanTransferAssets";
            this.customCheckBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.customCheckBox1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBox1.Size = new System.Drawing.Size(163, 21);
            this.customCheckBox1.TabIndex = 8;
            this.customCheckBox1.Text = "Can Transfer Assets:";
            this.customCheckBox1.UseVisualStyleBackColor = true;
            // 
            // customLinkBoxReportTo
            // 
            this.customLinkBoxReportTo.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxReportTo.CheckOnClick = true;
            this.customLinkBoxReportTo.ConnectFromId = -1;
            this.customLinkBoxReportTo.ConnectLinkType = "DecisionMakerReportsTo";
            this.customLinkBoxReportTo.ConnectRootId = 0;
            this.customLinkBoxReportTo.Controller = null;
            this.customLinkBoxReportTo.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxReportTo.DisplayLinkType = "Scenario";
            this.customLinkBoxReportTo.DisplayParameterCategory = "";
            this.customLinkBoxReportTo.DisplayParameterName = "";
            this.customLinkBoxReportTo.DisplayRecursiveCheck = false;
            this.customLinkBoxReportTo.DisplayRootId = 0;
            this.customLinkBoxReportTo.FilterResult = false;
            this.customLinkBoxReportTo.FormattingEnabled = true;
            this.customLinkBoxReportTo.Location = new System.Drawing.Point(8, 43);
            this.customLinkBoxReportTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customLinkBoxReportTo.Name = "customLinkBoxReportTo";
            this.customLinkBoxReportTo.OneToMany = false;
            this.customLinkBoxReportTo.ParameterFilterCategory = "";
            this.customLinkBoxReportTo.ParameterFilterName = "";
            this.customLinkBoxReportTo.ParameterFilterValue = "";
            this.customLinkBoxReportTo.Size = new System.Drawing.Size(205, 76);
            this.customLinkBoxReportTo.TabIndex = 4;
            // 
            // customLinkBoxChatWith
            // 
            this.customLinkBoxChatWith.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxChatWith.CheckOnClick = true;
            this.customLinkBoxChatWith.ConnectFromId = -1;
            this.customLinkBoxChatWith.ConnectLinkType = "DecisionMakerCanChat";
            this.customLinkBoxChatWith.ConnectRootId = 0;
            this.customLinkBoxChatWith.Controller = null;
            this.customLinkBoxChatWith.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxChatWith.DisplayLinkType = "Scenario";
            this.customLinkBoxChatWith.DisplayParameterCategory = "";
            this.customLinkBoxChatWith.DisplayParameterName = "";
            this.customLinkBoxChatWith.DisplayRecursiveCheck = false;
            this.customLinkBoxChatWith.DisplayRootId = 0;
            this.customLinkBoxChatWith.FilterResult = false;
            this.customLinkBoxChatWith.FormattingEnabled = true;
            this.customLinkBoxChatWith.Location = new System.Drawing.Point(8, 162);
            this.customLinkBoxChatWith.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customLinkBoxChatWith.Name = "customLinkBoxChatWith";
            this.customLinkBoxChatWith.OneToMany = true;
            this.customLinkBoxChatWith.ParameterFilterCategory = "";
            this.customLinkBoxChatWith.ParameterFilterName = "";
            this.customLinkBoxChatWith.ParameterFilterValue = "";
            this.customLinkBoxChatWith.Size = new System.Drawing.Size(205, 76);
            this.customLinkBoxChatWith.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 53;
            this.label4.Text = "Reports To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 142);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 17);
            this.label3.TabIndex = 52;
            this.label3.Text = "Can Chat With:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 303);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 48;
            this.label6.Text = "Mission Briefing:";
            // 
            // txtBriefing
            // 
            this.txtBriefing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBriefing.ComponentId = 0;
            this.txtBriefing.Controller = null;
            this.txtBriefing.Location = new System.Drawing.Point(8, 327);
            this.txtBriefing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBriefing.Multiline = true;
            this.txtBriefing.Name = "txtBriefing";
            this.txtBriefing.ParameterCategory = "DecisionMaker";
            this.txtBriefing.ParameterName = "Briefing";
            this.txtBriefing.ParameterType = AME.Controllers.eParamParentType.Component;
            this.txtBriefing.Size = new System.Drawing.Size(435, 134);
            this.txtBriefing.TabIndex = 10;
            this.txtBriefing.UseDelimiter = false;
            // 
            // customCheckBox3
            // 
            this.customCheckBox3.AutoSize = true;
            this.customCheckBox3.ComponentId = -1;
            this.customCheckBox3.Controller = null;
            this.customCheckBox3.Location = new System.Drawing.Point(260, 294);
            this.customCheckBox3.Margin = new System.Windows.Forms.Padding(4);
            this.customCheckBox3.Name = "customCheckBox3";
            this.customCheckBox3.ParameterCategory = "DecisionMaker";
            this.customCheckBox3.ParameterName = "IsObserver";
            this.customCheckBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.customCheckBox3.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBox3.Size = new System.Drawing.Size(180, 21);
            this.customCheckBox3.TabIndex = 64;
            this.customCheckBox3.Text = "DM Station is Observer:";
            this.customCheckBox3.UseVisualStyleBackColor = true;
            // 
            // DecisionMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DecisionMaker";
            this.Size = new System.Drawing.Size(588, 763);
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
        private System.Windows.Forms.Panel panel1;
        private AME.Views.View_Components.CustomParameterTextBox txtBriefing;
        private System.Windows.Forms.Label label6;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomParameterTextBox txtRole;
        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomParameterEnumBox customParameterEnumBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label colorSwatch;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxReportTo;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxChatWith;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomCheckBox customCheckBox2;
        private AME.Views.View_Components.CustomCheckBox customCheckBox1;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxSpeakWith;
        private System.Windows.Forms.Label label5;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxWhiteboardWith;
        private System.Windows.Forms.Label label8;
        private AME.Views.View_Components.CustomCheckBox customCheckBox3;
    }
}
