namespace VSG.ViewComponentPanels
{
    partial class ScenarioPlayfieldViewComponentPanel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.customTreeView1 = new AME.Views.View_Components.CustomTreeView();
            this.customTabPage3 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panelRegion = new System.Windows.Forms.Panel();
            this.panelActiveRegion = new System.Windows.Forms.Panel();
            this.checkBoxDynamicRegion = new AME.Views.View_Components.CustomParameterCheckBox(this.components);
            this.textBoxReferencePoint = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.colorSwatch = new System.Windows.Forms.Label();
            this.textBoxARSpeedMultiplier = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxAREnd = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxARStart = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.enumBoxARColor = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.linkBoxARSensorsBlocked = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxARIsVisible = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.checkBoxARBlocksMovement = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.textBoxARVertexList = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxActiveRegionRelativeVertexList = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.panelLandRegion = new System.Windows.Forms.Panel();
            this.textBoxLRVertexList = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.diagramPanel1 = new AME.Views.View_Components.DiagramPanel();
            this.customTabPage2 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.customTabControl2 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.split_Aware_West_East_Panel1 = new AME.Views.View_Components.Split_Aware_West_East_Panel();
            this.split_Aware_North_South_Panel1 = new AME.Views.View_Components.Split_Aware_North_South_Panel();
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.customTabPage3.SuspendLayout();
            this.panelActiveRegion.SuspendLayout();
            this.panelLandRegion.SuspendLayout();
            this.customTabPage2.SuspendLayout();
            this.customTabControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_Aware_West_East_Panel1)).BeginInit();
            this.split_Aware_West_East_Panel1.Panel1.SuspendLayout();
            this.split_Aware_West_East_Panel1.Panel2.SuspendLayout();
            this.split_Aware_West_East_Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_Aware_North_South_Panel1)).BeginInit();
            this.split_Aware_North_South_Panel1.Panel1.SuspendLayout();
            this.split_Aware_North_South_Panel1.Panel2.SuspendLayout();
            this.split_Aware_North_South_Panel1.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customTreeView1
            // 
            this.customTreeView1.AllowDrop = true;
            this.customTreeView1.AllowUserInput = true;
            this.customTreeView1.Controller = null;
            this.customTreeView1.DecorateNodes = false;
            this.customTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTreeView1.HideSelection = false;
            this.customTreeView1.Level = ((uint)(1u));
            this.customTreeView1.Location = new System.Drawing.Point(0, 28);
            this.customTreeView1.Name = "customTreeView1";
            this.customTreeView1.ShowRoot = false;
            this.customTreeView1.Size = new System.Drawing.Size(413, 348);
            this.customTreeView1.TabIndex = 1;
            this.customTreeView1.UseNodeMap = false;
            this.customTreeView1.ItemAdd += new AME.Views.View_Components.CustomTreeView.ItemAdded(this.customTreeView1_ItemAdd);
            this.customTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customTreeView1_AfterSelect);
            // 
            // customTabPage3
            // 
            this.customTabPage3.AutoScroll = true;
            this.customTabPage3.Controls.Add(this.panelRegion);
            this.customTabPage3.Description = "Current Region name here";
            this.customTabPage3.Location = new System.Drawing.Point(4, 22);
            this.customTabPage3.Name = "customTabPage3";
            this.customTabPage3.Size = new System.Drawing.Size(413, 389);
            this.customTabPage3.TabIndex = 50;
            this.customTabPage3.Text = "Region Properties";
            // 
            // panelRegion
            // 
            this.panelRegion.Location = new System.Drawing.Point(0, 28);
            this.panelRegion.MinimumSize = new System.Drawing.Size(358, 300);
            this.panelRegion.Name = "panelRegion";
            this.panelRegion.Size = new System.Drawing.Size(358, 340);
            this.panelRegion.TabIndex = 1;
            // 
            // panelActiveRegion
            // 
            this.panelActiveRegion.Controls.Add(this.checkBoxDynamicRegion);
            this.panelActiveRegion.Controls.Add(this.textBoxReferencePoint);
            this.panelActiveRegion.Controls.Add(this.label8);
            this.panelActiveRegion.Controls.Add(this.colorSwatch);
            this.panelActiveRegion.Controls.Add(this.textBoxARSpeedMultiplier);
            this.panelActiveRegion.Controls.Add(this.label7);
            this.panelActiveRegion.Controls.Add(this.textBoxAREnd);
            this.panelActiveRegion.Controls.Add(this.label6);
            this.panelActiveRegion.Controls.Add(this.textBoxARStart);
            this.panelActiveRegion.Controls.Add(this.label5);
            this.panelActiveRegion.Controls.Add(this.label4);
            this.panelActiveRegion.Controls.Add(this.enumBoxARColor);
            this.panelActiveRegion.Controls.Add(this.linkBoxARSensorsBlocked);
            this.panelActiveRegion.Controls.Add(this.label3);
            this.panelActiveRegion.Controls.Add(this.checkBoxARIsVisible);
            this.panelActiveRegion.Controls.Add(this.checkBoxARBlocksMovement);
            this.panelActiveRegion.Controls.Add(this.textBoxARVertexList);
            this.panelActiveRegion.Controls.Add(this.label2);
            this.panelActiveRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActiveRegion.Location = new System.Drawing.Point(0, 0);
            this.panelActiveRegion.Name = "panelActiveRegion";
            this.panelActiveRegion.Padding = new System.Windows.Forms.Padding(6);
            this.panelActiveRegion.Size = new System.Drawing.Size(358, 340);
            this.panelActiveRegion.TabIndex = 3;
            // 
            // checkBoxDynamicRegion
            // 
            this.checkBoxDynamicRegion.AutoSize = true;
            this.checkBoxDynamicRegion.ComponentId = -1;
            this.checkBoxDynamicRegion.Controller = null;
            this.checkBoxDynamicRegion.Location = new System.Drawing.Point(12, 102);
            this.checkBoxDynamicRegion.Name = "checkBoxDynamicRegion";
            this.checkBoxDynamicRegion.ParameterCategory = "Active Region";
            this.checkBoxDynamicRegion.ParameterName = "Is this a Dynamic Region";
            this.checkBoxDynamicRegion.ParameterType = AME.Controllers.eParamParentType.Component;
            this.checkBoxDynamicRegion.Size = new System.Drawing.Size(115, 17);
            this.checkBoxDynamicRegion.TabIndex = 56;
            this.checkBoxDynamicRegion.Text = "Is Dynamic Region";
            this.checkBoxDynamicRegion.UseVisualStyleBackColor = true;
            // 
            // textBoxReferencePoint
            // 
            this.textBoxReferencePoint.ComponentId = -1;
            this.textBoxReferencePoint.Controller = null;
            this.textBoxReferencePoint.Location = new System.Drawing.Point(166, 52);
            this.textBoxReferencePoint.Name = "textBoxReferencePoint";
            this.textBoxReferencePoint.ParameterCategory = "Location";
            this.textBoxReferencePoint.ParameterName = "ReferencePoint";
            this.textBoxReferencePoint.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxReferencePoint.ReadOnly = true;
            this.textBoxReferencePoint.Size = new System.Drawing.Size(183, 20);
            this.textBoxReferencePoint.TabIndex = 55;
            this.textBoxReferencePoint.UseDelimiter = false;
            this.textBoxReferencePoint.TextChanged += new System.EventHandler(this.customParameterTextBox1_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(163, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 54;
            this.label8.Text = "Reference Point:";
            // 
            // colorSwatch
            // 
            this.colorSwatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.colorSwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorSwatch.ForeColor = System.Drawing.Color.White;
            this.colorSwatch.Location = new System.Drawing.Point(103, 227);
            this.colorSwatch.Name = "colorSwatch";
            this.colorSwatch.Size = new System.Drawing.Size(54, 19);
            this.colorSwatch.TabIndex = 53;
            this.colorSwatch.Text = "Sample";
            this.colorSwatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxARSpeedMultiplier
            // 
            this.textBoxARSpeedMultiplier.ComponentId = -1;
            this.textBoxARSpeedMultiplier.Controller = null;
            this.textBoxARSpeedMultiplier.Location = new System.Drawing.Point(9, 201);
            this.textBoxARSpeedMultiplier.Name = "textBoxARSpeedMultiplier";
            this.textBoxARSpeedMultiplier.ParameterCategory = "Active Region";
            this.textBoxARSpeedMultiplier.ParameterName = "Speed Multiplier";
            this.textBoxARSpeedMultiplier.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxARSpeedMultiplier.Size = new System.Drawing.Size(148, 20);
            this.textBoxARSpeedMultiplier.TabIndex = 5;
            this.textBoxARSpeedMultiplier.UseDelimiter = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Speed Multiplier:";
            // 
            // textBoxAREnd
            // 
            this.textBoxAREnd.ComponentId = -1;
            this.textBoxAREnd.Controller = null;
            this.textBoxAREnd.Location = new System.Drawing.Point(73, 7);
            this.textBoxAREnd.Name = "textBoxAREnd";
            this.textBoxAREnd.ParameterCategory = "Active Region";
            this.textBoxAREnd.ParameterName = "End";
            this.textBoxAREnd.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxAREnd.Size = new System.Drawing.Size(67, 20);
            this.textBoxAREnd.TabIndex = 1;
            this.textBoxAREnd.UseDelimiter = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Floor (m.)";
            this.toolTip1.SetToolTip(this.label6, "The Floor represents the lowest 3-Dimensional point in this region.  \nThis value " +
        "can be negative to represent sub-surface regions, and should always be less than" +
        " the Ceiling.");
            // 
            // textBoxARStart
            // 
            this.textBoxARStart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxARStart.ComponentId = -1;
            this.textBoxARStart.Controller = null;
            this.textBoxARStart.Location = new System.Drawing.Point(210, 7);
            this.textBoxARStart.Name = "textBoxARStart";
            this.textBoxARStart.ParameterCategory = "Active Region";
            this.textBoxARStart.ParameterName = "Start";
            this.textBoxARStart.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxARStart.Size = new System.Drawing.Size(139, 20);
            this.textBoxARStart.TabIndex = 7;
            this.textBoxARStart.UseDelimiter = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(146, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ceiling (m.)";
            this.toolTip1.SetToolTip(this.label5, "The Ceiling represents the highest 3-Dimensional point in this region.  \nThis val" +
        "ue can be negative to represent sub-surface regions, and should always be greate" +
        "r than the Floor.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Outline:";
            // 
            // enumBoxARColor
            // 
            this.enumBoxARColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.enumBoxARColor.ComponentId = -1;
            this.enumBoxARColor.Controller = null;
            this.enumBoxARColor.EnumName = "Colors";
            this.enumBoxARColor.FormattingEnabled = true;
            this.enumBoxARColor.IntegralHeight = false;
            this.enumBoxARColor.IsColorEnum = true;
            this.enumBoxARColor.Location = new System.Drawing.Point(9, 252);
            this.enumBoxARColor.Margin = new System.Windows.Forms.Padding(6);
            this.enumBoxARColor.Name = "enumBoxARColor";
            this.enumBoxARColor.ParameterCategory = "Color";
            this.enumBoxARColor.ParameterName = "Color";
            this.enumBoxARColor.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.enumBoxARColor.Size = new System.Drawing.Size(148, 76);
            this.enumBoxARColor.TabIndex = 6;
            this.enumBoxARColor.SelectedIndexChanged += new System.EventHandler(this.enumBoxARColor_SelectedIndexChanged);
            // 
            // linkBoxARSensorsBlocked
            // 
            this.linkBoxARSensorsBlocked.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkBoxARSensorsBlocked.CheckLinkLevel = ((uint)(1u));
            this.linkBoxARSensorsBlocked.CheckOnClick = true;
            this.linkBoxARSensorsBlocked.ConnectFromId = -1;
            this.linkBoxARSensorsBlocked.ConnectLinkType = "ActiveRegionSensor";
            this.linkBoxARSensorsBlocked.ConnectRootId = -1;
            this.linkBoxARSensorsBlocked.Controller = null;
            this.linkBoxARSensorsBlocked.DisplayComponentType = "Sensor";
            this.linkBoxARSensorsBlocked.DisplayLinkType = "Scenario";
            this.linkBoxARSensorsBlocked.DisplayParameterCategory = "";
            this.linkBoxARSensorsBlocked.DisplayParameterName = "";
            this.linkBoxARSensorsBlocked.DisplayRecursiveCheck = false;
            this.linkBoxARSensorsBlocked.DisplayRootId = -1;
            this.linkBoxARSensorsBlocked.FilterResult = false;
            this.linkBoxARSensorsBlocked.FormattingEnabled = true;
            this.linkBoxARSensorsBlocked.IntegralHeight = false;
            this.linkBoxARSensorsBlocked.Location = new System.Drawing.Point(166, 102);
            this.linkBoxARSensorsBlocked.Name = "linkBoxARSensorsBlocked";
            this.linkBoxARSensorsBlocked.OneToMany = true;
            this.linkBoxARSensorsBlocked.ParameterFilterCategory = "";
            this.linkBoxARSensorsBlocked.ParameterFilterName = "";
            this.linkBoxARSensorsBlocked.ParameterFilterValue = "";
            this.linkBoxARSensorsBlocked.Size = new System.Drawing.Size(183, 229);
            this.linkBoxARSensorsBlocked.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sensors Blocked:";
            // 
            // checkBoxARIsVisible
            // 
            this.checkBoxARIsVisible.AutoSize = true;
            this.checkBoxARIsVisible.ComponentId = -1;
            this.checkBoxARIsVisible.Controller = null;
            this.checkBoxARIsVisible.Location = new System.Drawing.Point(9, 155);
            this.checkBoxARIsVisible.Name = "checkBoxARIsVisible";
            this.checkBoxARIsVisible.Padding = new System.Windows.Forms.Padding(3);
            this.checkBoxARIsVisible.ParameterCategory = "Active Region";
            this.checkBoxARIsVisible.ParameterName = "Is Visible";
            this.checkBoxARIsVisible.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.checkBoxARIsVisible.Size = new System.Drawing.Size(73, 23);
            this.checkBoxARIsVisible.TabIndex = 4;
            this.checkBoxARIsVisible.Text = "Is Visible";
            this.checkBoxARIsVisible.UseVisualStyleBackColor = true;
            // 
            // checkBoxARBlocksMovement
            // 
            this.checkBoxARBlocksMovement.AutoSize = true;
            this.checkBoxARBlocksMovement.ComponentId = -1;
            this.checkBoxARBlocksMovement.Controller = null;
            this.checkBoxARBlocksMovement.Location = new System.Drawing.Point(9, 126);
            this.checkBoxARBlocksMovement.Name = "checkBoxARBlocksMovement";
            this.checkBoxARBlocksMovement.Padding = new System.Windows.Forms.Padding(3);
            this.checkBoxARBlocksMovement.ParameterCategory = "Active Region";
            this.checkBoxARBlocksMovement.ParameterName = "Blocks Movement";
            this.checkBoxARBlocksMovement.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.checkBoxARBlocksMovement.Size = new System.Drawing.Size(117, 23);
            this.checkBoxARBlocksMovement.TabIndex = 3;
            this.checkBoxARBlocksMovement.Text = "Blocks Movement";
            this.checkBoxARBlocksMovement.UseVisualStyleBackColor = true;
            // 
            // textBoxARVertexList
            // 
            this.textBoxARVertexList.ComponentId = -1;
            this.textBoxARVertexList.Controller = null;
            this.textBoxARVertexList.Location = new System.Drawing.Point(9, 52);
            this.textBoxARVertexList.Multiline = true;
            this.textBoxARVertexList.Name = "textBoxARVertexList";
            this.textBoxARVertexList.ParameterCategory = "Location";
            this.textBoxARVertexList.ParameterName = "Polygon Points";
            this.textBoxARVertexList.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxARVertexList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxARVertexList.Size = new System.Drawing.Size(148, 44);
            this.textBoxARVertexList.TabIndex = 2;
            this.textBoxARVertexList.UseDelimiter = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vertex List:";
            // 
            // textBoxActiveRegionRelativeVertexList
            // 
            this.textBoxActiveRegionRelativeVertexList.ComponentId = -1;
            this.textBoxActiveRegionRelativeVertexList.Controller = null;
            this.textBoxActiveRegionRelativeVertexList.Location = new System.Drawing.Point(9, 52);
            this.textBoxActiveRegionRelativeVertexList.Multiline = true;
            this.textBoxActiveRegionRelativeVertexList.Name = "textBoxActiveRegionRelativeVertexList";
            this.textBoxActiveRegionRelativeVertexList.ParameterCategory = "Location";
            this.textBoxActiveRegionRelativeVertexList.ParameterName = "Relative Polygon Points";
            this.textBoxActiveRegionRelativeVertexList.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxActiveRegionRelativeVertexList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxActiveRegionRelativeVertexList.Size = new System.Drawing.Size(148, 44);
            this.textBoxActiveRegionRelativeVertexList.TabIndex = 0;
            this.textBoxActiveRegionRelativeVertexList.UseDelimiter = false;
            this.textBoxActiveRegionRelativeVertexList.Visible = false;
            // 
            // panelLandRegion
            // 
            this.panelLandRegion.Controls.Add(this.textBoxLRVertexList);
            this.panelLandRegion.Controls.Add(this.label1);
            this.panelLandRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLandRegion.Location = new System.Drawing.Point(0, 0);
            this.panelLandRegion.Name = "panelLandRegion";
            this.panelLandRegion.Padding = new System.Windows.Forms.Padding(6);
            this.panelLandRegion.Size = new System.Drawing.Size(358, 300);
            this.panelLandRegion.TabIndex = 2;
            // 
            // textBoxLRVertexList
            // 
            this.textBoxLRVertexList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLRVertexList.ComponentId = -1;
            this.textBoxLRVertexList.Controller = null;
            this.textBoxLRVertexList.Location = new System.Drawing.Point(12, 22);
            this.textBoxLRVertexList.Name = "textBoxLRVertexList";
            this.textBoxLRVertexList.ParameterCategory = "Location";
            this.textBoxLRVertexList.ParameterName = "Polygon Points";
            this.textBoxLRVertexList.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxLRVertexList.Size = new System.Drawing.Size(337, 20);
            this.textBoxLRVertexList.TabIndex = 1;
            this.textBoxLRVertexList.UseDelimiter = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vertex List";
            // 
            // diagramPanel1
            // 
            this.diagramPanel1.Controller = null;
            this.diagramPanel1.DiagramGridStripLocation = new System.Drawing.Point(76, 0);
            this.diagramPanel1.DiagramGridStripVisible = false;
            this.diagramPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramPanel1.DropDownStripLocation = new System.Drawing.Point(111, 75);
            this.diagramPanel1.DropDownStripVisible = false;
            this.diagramPanel1.FilterStripLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.FilterStripVisible = false;
            this.diagramPanel1.FilterTypes = new string[] {
        "LandRegion",
        "ActiveRegion"};
            this.diagramPanel1.LinkTypes = new string[] {
        "ScenarioRegions"};
            this.diagramPanel1.Location = new System.Drawing.Point(0, 0);
            this.diagramPanel1.Name = "diagramPanel1";
            this.diagramPanel1.SelectedIndex = -1;
            this.diagramPanel1.SelectLinkLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.SelectLinkStripVisible = false;
            this.diagramPanel1.Size = new System.Drawing.Size(25, 0);
            this.diagramPanel1.TabIndex = 1;
            this.diagramPanel1.TopToolStripVisible = true;
            this.diagramPanel1.UsingGeoReferenceUTM = false;
            this.diagramPanel1.ZoomStripLocation = new System.Drawing.Point(3, 0);
            this.diagramPanel1.ZoomStripVisible = true;
            // 
            // customTabPage2
            // 
            this.customTabPage2.Controls.Add(this.customTreeView1);
            this.customTabPage2.Description = "Create new regions.";
            this.customTabPage2.Location = new System.Drawing.Point(4, 22);
            this.customTabPage2.Name = "customTabPage2";
            this.customTabPage2.Size = new System.Drawing.Size(413, 376);
            this.customTabPage2.TabIndex = 50;
            this.customTabPage2.Text = "Regions";
            // 
            // customTabControl2
            // 
            this.customTabControl2.Controls.Add(this.customTabPage3);
            this.customTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl2.Location = new System.Drawing.Point(0, 0);
            this.customTabControl2.Name = "customTabControl2";
            this.customTabControl2.SelectedIndex = 0;
            this.customTabControl2.Size = new System.Drawing.Size(421, 415);
            this.customTabControl2.TabIndex = 0;
            // 
            // split_Aware_West_East_Panel1
            // 
            this.split_Aware_West_East_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_Aware_West_East_Panel1.Location = new System.Drawing.Point(0, 0);
            this.split_Aware_West_East_Panel1.Name = "split_Aware_West_East_Panel1";
            // 
            // split_Aware_West_East_Panel1.Panel1
            // 
            this.split_Aware_West_East_Panel1.Panel1.Controls.Add(this.split_Aware_North_South_Panel1);
            // 
            // split_Aware_West_East_Panel1.Panel2
            // 
            this.split_Aware_West_East_Panel1.Panel2.Controls.Add(this.diagramPanel1);
            this.split_Aware_West_East_Panel1.Size = new System.Drawing.Size(0, 0);
            this.split_Aware_West_East_Panel1.SplitterDistance = 25;
            this.split_Aware_West_East_Panel1.TabIndex = 2;
            // 
            // split_Aware_North_South_Panel1
            // 
            this.split_Aware_North_South_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_Aware_North_South_Panel1.Location = new System.Drawing.Point(0, 0);
            this.split_Aware_North_South_Panel1.Name = "split_Aware_North_South_Panel1";
            this.split_Aware_North_South_Panel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_Aware_North_South_Panel1.Panel1
            // 
            this.split_Aware_North_South_Panel1.Panel1.Controls.Add(this.customTabControl1);
            // 
            // split_Aware_North_South_Panel1.Panel2
            // 
            this.split_Aware_North_South_Panel1.Panel2.Controls.Add(this.customTabControl2);
            this.split_Aware_North_South_Panel1.Size = new System.Drawing.Size(25, 0);
            this.split_Aware_North_South_Panel1.SplitterDistance = 402;
            this.split_Aware_North_South_Panel1.TabIndex = 0;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage2);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(421, 402);
            this.customTabControl1.TabIndex = 0;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ScenarioPlayfieldViewComponentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.split_Aware_West_East_Panel1);
            this.Name = "ScenarioPlayfieldViewComponentPanel";
            this.Size = new System.Drawing.Size(0, 0);
            this.customTabPage3.ResumeLayout(false);
            this.panelActiveRegion.ResumeLayout(false);
            this.panelActiveRegion.PerformLayout();
            this.panelLandRegion.ResumeLayout(false);
            this.panelLandRegion.PerformLayout();
            this.customTabPage2.ResumeLayout(false);
            this.customTabControl2.ResumeLayout(false);
            this.split_Aware_West_East_Panel1.Panel1.ResumeLayout(false);
            this.split_Aware_West_East_Panel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_Aware_West_East_Panel1)).EndInit();
            this.split_Aware_West_East_Panel1.ResumeLayout(false);
            this.split_Aware_North_South_Panel1.Panel1.ResumeLayout(false);
            this.split_Aware_North_South_Panel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_Aware_North_South_Panel1)).EndInit();
            this.split_Aware_North_South_Panel1.ResumeLayout(false);
            this.customTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTreeView customTreeView1;
        private AME.Views.View_Components.CustomTabPage customTabPage3;
        private AME.Views.View_Components.DiagramPanel diagramPanel1;
        private AME.Views.View_Components.CustomTabPage customTabPage2;
        private AME.Views.View_Components.CustomTabControl customTabControl2;
        private AME.Views.View_Components.Split_Aware_West_East_Panel split_Aware_West_East_Panel1;
        private AME.Views.View_Components.Split_Aware_North_South_Panel split_Aware_North_South_Panel1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private System.Windows.Forms.Panel panelRegion;
        private System.Windows.Forms.Panel panelLandRegion;
        private AME.Views.View_Components.CustomParameterTextBox textBoxLRVertexList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelActiveRegion;
        private AME.Views.View_Components.CustomParameterTextBox textBoxARVertexList;
        private AME.Views.View_Components.CustomParameterTextBox textBoxActiveRegionRelativeVertexList;
        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomCheckBox checkBoxARBlocksMovement;
        private AME.Views.View_Components.CustomCheckBox checkBoxARIsVisible;
        private AME.Views.View_Components.CustomLinkBox linkBoxARSensorsBlocked;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private AME.Views.View_Components.CustomParameterEnumBox enumBoxARColor;
        private System.Windows.Forms.Label label5;
        private AME.Views.View_Components.CustomParameterTextBox textBoxAREnd;
        private System.Windows.Forms.Label label6;
        private AME.Views.View_Components.CustomParameterTextBox textBoxARStart;
        private AME.Views.View_Components.CustomParameterTextBox textBoxARSpeedMultiplier;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label colorSwatch;
        private System.Windows.Forms.ToolTip toolTip1;
        private AME.Views.View_Components.CustomParameterTextBox textBoxReferencePoint;
        private System.Windows.Forms.Label label8;
        private AME.Views.View_Components.CustomParameterCheckBox checkBoxDynamicRegion;
        
        //this.panelRegion.Controls.Add(this.panelLandRegion);
        //this.panelRegion.Controls.Add(this.panelActiveRegion);
    }
}
