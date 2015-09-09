namespace VSG.ViewComponents
{
    partial class EvtPnl_Reveal
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.iconLabel = new System.Windows.Forms.Label();
            this.launchDurationLabel = new System.Windows.Forms.Label();
            this.dockingDurationLabel = new System.Windows.Forms.Label();
            this.timeToAttackLabel = new System.Windows.Forms.Label();
            this.maximumSpeedLabel = new System.Windows.Forms.Label();
            this.fuelCapacityLabel = new System.Windows.Forms.Label();
            this.initialFuelLoadLabel = new System.Windows.Forms.Label();
            this.fuelConsumptionRateLabel = new System.Windows.Forms.Label();
            this.fuelDepletionStateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.customLinkComboBoxLinkedRegion = new AME.Views.View_Components.CustomLinkComboBox(this.components);
            this.initialTagParameterTextBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.zBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.yBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.xBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.initialStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.fuelDepletionStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.overrideIcon = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.iconListView1 = new VSG.ViewComponents.IconListView();
            this.overrideFuelDepletionState = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideFuelConsumptionRate = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideInitialFuelLoad = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideFuelCapacity = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideMaximumSpeed = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideTimeToAttack = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideDockingDuration = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideLaunchDuration = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.overrideStealable = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.stealable = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.fuelConsumption = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.initialFuel = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.maxSpeed = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.fuelCapacity = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.timeToAttack = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.dockingDuration = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.launchDuration = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.engramRange = new VSG.ViewComponents.EngramRange();
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time (s.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 57);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Location:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 77);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "X:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 101);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 125);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Z:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(220, 105);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Initial State:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.fuelDepletionStateComboBox);
            this.groupBox3.Controls.Add(this.overrideIcon);
            this.groupBox3.Controls.Add(this.iconListView1);
            this.groupBox3.Controls.Add(this.iconLabel);
            this.groupBox3.Controls.Add(this.overrideFuelDepletionState);
            this.groupBox3.Controls.Add(this.overrideFuelConsumptionRate);
            this.groupBox3.Controls.Add(this.overrideInitialFuelLoad);
            this.groupBox3.Controls.Add(this.overrideFuelCapacity);
            this.groupBox3.Controls.Add(this.overrideMaximumSpeed);
            this.groupBox3.Controls.Add(this.overrideTimeToAttack);
            this.groupBox3.Controls.Add(this.overrideDockingDuration);
            this.groupBox3.Controls.Add(this.overrideLaunchDuration);
            this.groupBox3.Controls.Add(this.overrideStealable);
            this.groupBox3.Controls.Add(this.stealable);
            this.groupBox3.Controls.Add(this.fuelConsumption);
            this.groupBox3.Controls.Add(this.launchDurationLabel);
            this.groupBox3.Controls.Add(this.initialFuel);
            this.groupBox3.Controls.Add(this.dockingDurationLabel);
            this.groupBox3.Controls.Add(this.maxSpeed);
            this.groupBox3.Controls.Add(this.timeToAttackLabel);
            this.groupBox3.Controls.Add(this.fuelCapacity);
            this.groupBox3.Controls.Add(this.maximumSpeedLabel);
            this.groupBox3.Controls.Add(this.timeToAttack);
            this.groupBox3.Controls.Add(this.fuelCapacityLabel);
            this.groupBox3.Controls.Add(this.dockingDuration);
            this.groupBox3.Controls.Add(this.initialFuelLoadLabel);
            this.groupBox3.Controls.Add(this.launchDuration);
            this.groupBox3.Controls.Add(this.fuelConsumptionRateLabel);
            this.groupBox3.Controls.Add(this.fuelDepletionStateLabel);
            this.groupBox3.Location = new System.Drawing.Point(15, 148);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(583, 312);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Startup Parameters";
            // 
            // iconLabel
            // 
            this.iconLabel.AutoSize = true;
            this.iconLabel.Location = new System.Drawing.Point(68, 201);
            this.iconLabel.Margin = new System.Windows.Forms.Padding(3);
            this.iconLabel.Name = "iconLabel";
            this.iconLabel.Size = new System.Drawing.Size(31, 13);
            this.iconLabel.TabIndex = 40;
            this.iconLabel.Text = "Icon:";
            // 
            // launchDurationLabel
            // 
            this.launchDurationLabel.AutoSize = true;
            this.launchDurationLabel.Location = new System.Drawing.Point(68, 50);
            this.launchDurationLabel.Name = "launchDurationLabel";
            this.launchDurationLabel.Size = new System.Drawing.Size(106, 13);
            this.launchDurationLabel.TabIndex = 0;
            this.launchDurationLabel.Text = "Launch Duration (s.):";
            // 
            // dockingDurationLabel
            // 
            this.dockingDurationLabel.AutoSize = true;
            this.dockingDurationLabel.Location = new System.Drawing.Point(68, 98);
            this.dockingDurationLabel.Name = "dockingDurationLabel";
            this.dockingDurationLabel.Size = new System.Drawing.Size(110, 13);
            this.dockingDurationLabel.TabIndex = 1;
            this.dockingDurationLabel.Text = "Docking Duration (s.):";
            // 
            // timeToAttackLabel
            // 
            this.timeToAttackLabel.AutoSize = true;
            this.timeToAttackLabel.Location = new System.Drawing.Point(68, 151);
            this.timeToAttackLabel.Name = "timeToAttackLabel";
            this.timeToAttackLabel.Size = new System.Drawing.Size(100, 13);
            this.timeToAttackLabel.TabIndex = 2;
            this.timeToAttackLabel.Text = "Time To Attack (s.):";
            // 
            // maximumSpeedLabel
            // 
            this.maximumSpeedLabel.AutoSize = true;
            this.maximumSpeedLabel.Location = new System.Drawing.Point(230, 50);
            this.maximumSpeedLabel.Name = "maximumSpeedLabel";
            this.maximumSpeedLabel.Size = new System.Drawing.Size(115, 13);
            this.maximumSpeedLabel.TabIndex = 3;
            this.maximumSpeedLabel.Text = "Maximum Speed (m/s):";
            // 
            // fuelCapacityLabel
            // 
            this.fuelCapacityLabel.AutoSize = true;
            this.fuelCapacityLabel.Location = new System.Drawing.Point(230, 99);
            this.fuelCapacityLabel.Name = "fuelCapacityLabel";
            this.fuelCapacityLabel.Size = new System.Drawing.Size(107, 13);
            this.fuelCapacityLabel.TabIndex = 4;
            this.fuelCapacityLabel.Text = "Fuel Capacity (Units):";
            // 
            // initialFuelLoadLabel
            // 
            this.initialFuelLoadLabel.AutoSize = true;
            this.initialFuelLoadLabel.Location = new System.Drawing.Point(230, 151);
            this.initialFuelLoadLabel.Name = "initialFuelLoadLabel";
            this.initialFuelLoadLabel.Size = new System.Drawing.Size(117, 13);
            this.initialFuelLoadLabel.TabIndex = 5;
            this.initialFuelLoadLabel.Text = "Initial Fuel Load (Units):";
            // 
            // fuelConsumptionRateLabel
            // 
            this.fuelConsumptionRateLabel.AutoSize = true;
            this.fuelConsumptionRateLabel.Location = new System.Drawing.Point(406, 50);
            this.fuelConsumptionRateLabel.Name = "fuelConsumptionRateLabel";
            this.fuelConsumptionRateLabel.Size = new System.Drawing.Size(163, 13);
            this.fuelConsumptionRateLabel.TabIndex = 6;
            this.fuelConsumptionRateLabel.Text = "Fuel Consumption Rate (Units/s):";
            // 
            // fuelDepletionStateLabel
            // 
            this.fuelDepletionStateLabel.AutoSize = true;
            this.fuelDepletionStateLabel.Location = new System.Drawing.Point(406, 98);
            this.fuelDepletionStateLabel.Name = "fuelDepletionStateLabel";
            this.fuelDepletionStateLabel.Size = new System.Drawing.Size(106, 13);
            this.fuelDepletionStateLabel.TabIndex = 7;
            this.fuelDepletionStateLabel.Text = "Fuel Depletion State:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Initial Tag:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(426, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 48;
            this.label7.Text = "Linked To Region:";
            // 
            // customLinkComboBoxLinkedRegion
            // 
            this.customLinkComboBoxLinkedRegion.ConnectDynamicId = -1;
            this.customLinkComboBoxLinkedRegion.ConnectFromId = -1;
            this.customLinkComboBoxLinkedRegion.ConnectLinkDynamic = false;
            this.customLinkComboBoxLinkedRegion.ConnectLinkType = null;
            this.customLinkComboBoxLinkedRegion.ConnectRootId = -1;
            this.customLinkComboBoxLinkedRegion.Controller = null;
            this.customLinkComboBoxLinkedRegion.DisplayComponent = null;
            this.customLinkComboBoxLinkedRegion.DisplayLinkType = null;
            this.customLinkComboBoxLinkedRegion.DisplayRootId = -1;
            this.customLinkComboBoxLinkedRegion.FormattingEnabled = true;
            this.customLinkComboBoxLinkedRegion.LinkLevel = ((uint)(1u));
            this.customLinkComboBoxLinkedRegion.Location = new System.Drawing.Point(248, 81);
            this.customLinkComboBoxLinkedRegion.Name = "customLinkComboBoxLinkedRegion";
            this.customLinkComboBoxLinkedRegion.Size = new System.Drawing.Size(172, 21);
            this.customLinkComboBoxLinkedRegion.TabIndex = 49;
            this.customLinkComboBoxLinkedRegion.Visible = false;
            this.customLinkComboBoxLinkedRegion.Xsl = null;
            // 
            // initialTagParameterTextBox
            // 
            this.initialTagParameterTextBox.ComponentId = -1;
            this.initialTagParameterTextBox.Controller = null;
            this.initialTagParameterTextBox.Location = new System.Drawing.Point(508, 121);
            this.initialTagParameterTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.initialTagParameterTextBox.Name = "initialTagParameterTextBox";
            this.initialTagParameterTextBox.ParameterCategory = "StartupParameters";
            this.initialTagParameterTextBox.ParameterName = "InitialTag";
            this.initialTagParameterTextBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.initialTagParameterTextBox.Size = new System.Drawing.Size(91, 20);
            this.initialTagParameterTextBox.TabIndex = 9;
            this.initialTagParameterTextBox.UseDelimiter = false;
            // 
            // zBox
            // 
            this.zBox.ComponentId = -1;
            this.zBox.Controller = null;
            this.zBox.Location = new System.Drawing.Point(63, 122);
            this.zBox.Margin = new System.Windows.Forms.Padding(2);
            this.zBox.Name = "zBox";
            this.zBox.ParameterCategory = "InitialLocation";
            this.zBox.ParameterName = "Z";
            this.zBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.zBox.Size = new System.Drawing.Size(91, 20);
            this.zBox.TabIndex = 7;
            this.zBox.UseDelimiter = false;
            // 
            // yBox
            // 
            this.yBox.ComponentId = -1;
            this.yBox.Controller = null;
            this.yBox.Location = new System.Drawing.Point(63, 98);
            this.yBox.Margin = new System.Windows.Forms.Padding(2);
            this.yBox.Name = "yBox";
            this.yBox.ParameterCategory = "InitialLocation";
            this.yBox.ParameterName = "Y";
            this.yBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.yBox.Size = new System.Drawing.Size(91, 20);
            this.yBox.TabIndex = 6;
            this.yBox.UseDelimiter = false;
            // 
            // xBox
            // 
            this.xBox.ComponentId = -1;
            this.xBox.Controller = null;
            this.xBox.Location = new System.Drawing.Point(63, 74);
            this.xBox.Margin = new System.Windows.Forms.Padding(2);
            this.xBox.Name = "xBox";
            this.xBox.ParameterCategory = "InitialLocation";
            this.xBox.ParameterName = "X";
            this.xBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.xBox.Size = new System.Drawing.Size(91, 20);
            this.xBox.TabIndex = 5;
            this.xBox.UseDelimiter = false;
            // 
            // initialStateComboBox
            // 
            this.initialStateComboBox.ComponentId = -1;
            this.initialStateComboBox.Controller = null;
            this.initialStateComboBox.FormattingEnabled = true;
            this.initialStateComboBox.Location = new System.Drawing.Point(222, 121);
            this.initialStateComboBox.Name = "initialStateComboBox";
            this.initialStateComboBox.ParameterCategory = "State";
            this.initialStateComboBox.ParameterName = "FuelDepletionState";
            this.initialStateComboBox.ShowAllStates = false;
            this.initialStateComboBox.Size = new System.Drawing.Size(178, 21);
            this.initialStateComboBox.SpeciesId = -1;
            this.initialStateComboBox.TabIndex = 8;
            // 
            // fuelDepletionStateComboBox
            // 
            this.fuelDepletionStateComboBox.ComponentId = -1;
            this.fuelDepletionStateComboBox.Controller = null;
            this.fuelDepletionStateComboBox.FormattingEnabled = true;
            this.fuelDepletionStateComboBox.Location = new System.Drawing.Point(409, 116);
            this.fuelDepletionStateComboBox.Name = "fuelDepletionStateComboBox";
            this.fuelDepletionStateComboBox.ParameterCategory = "State";
            this.fuelDepletionStateComboBox.ParameterName = "FuelDepletionState";
            this.fuelDepletionStateComboBox.ShowAllStates = false;
            this.fuelDepletionStateComboBox.Size = new System.Drawing.Size(168, 21);
            this.fuelDepletionStateComboBox.SpeciesId = -1;
            this.fuelDepletionStateComboBox.TabIndex = 23;
            // 
            // overrideIcon
            // 
            this.overrideIcon.AutoSize = true;
            this.overrideIcon.ComponentId = 0;
            this.overrideIcon.Controller = null;
            this.overrideIcon.Location = new System.Drawing.Point(5, 247);
            this.overrideIcon.Margin = new System.Windows.Forms.Padding(2);
            this.overrideIcon.Name = "overrideIcon";
            this.overrideIcon.ParameterCategory = "StartupParameters";
            this.overrideIcon.ParameterName = "OverrideIcon";
            this.overrideIcon.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideIcon.Size = new System.Drawing.Size(66, 17);
            this.overrideIcon.TabIndex = 28;
            this.overrideIcon.Text = "Override";
            this.overrideIcon.UseVisualStyleBackColor = true;
            // 
            // iconListView1
            // 
            this.iconListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iconListView1.ComponentId = 0;
            this.iconListView1.Controller = null;
            this.iconListView1.HideSelection = false;
            this.iconListView1.IconParameterCategory = "StartupParameters";
            this.iconListView1.IconParameterName = "Icon";
            this.iconListView1.IconParameterType = AME.Controllers.eParamParentType.Component;
            this.iconListView1.Location = new System.Drawing.Point(76, 220);
            this.iconListView1.MultiSelect = false;
            this.iconListView1.Name = "iconListView1";
            this.iconListView1.Size = new System.Drawing.Size(474, 75);
            this.iconListView1.TabIndex = 29;
            this.iconListView1.UseCompatibleStateImageBehavior = false;
            this.iconListView1.View = System.Windows.Forms.View.List;
            // 
            // overrideFuelDepletionState
            // 
            this.overrideFuelDepletionState.AutoSize = true;
            this.overrideFuelDepletionState.ComponentId = 0;
            this.overrideFuelDepletionState.Controller = null;
            this.overrideFuelDepletionState.Location = new System.Drawing.Point(340, 115);
            this.overrideFuelDepletionState.Margin = new System.Windows.Forms.Padding(2);
            this.overrideFuelDepletionState.Name = "overrideFuelDepletionState";
            this.overrideFuelDepletionState.ParameterCategory = "StartupParameters";
            this.overrideFuelDepletionState.ParameterName = "OverrideFuelDepletionState";
            this.overrideFuelDepletionState.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelDepletionState.Size = new System.Drawing.Size(66, 17);
            this.overrideFuelDepletionState.TabIndex = 22;
            this.overrideFuelDepletionState.Text = "Override";
            this.overrideFuelDepletionState.UseVisualStyleBackColor = true;
            // 
            // overrideFuelConsumptionRate
            // 
            this.overrideFuelConsumptionRate.AutoSize = true;
            this.overrideFuelConsumptionRate.ComponentId = 0;
            this.overrideFuelConsumptionRate.Controller = null;
            this.overrideFuelConsumptionRate.Location = new System.Drawing.Point(340, 65);
            this.overrideFuelConsumptionRate.Margin = new System.Windows.Forms.Padding(2);
            this.overrideFuelConsumptionRate.Name = "overrideFuelConsumptionRate";
            this.overrideFuelConsumptionRate.ParameterCategory = "StartupParameters";
            this.overrideFuelConsumptionRate.ParameterName = "OverrideFuelConsumption";
            this.overrideFuelConsumptionRate.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelConsumptionRate.Size = new System.Drawing.Size(66, 17);
            this.overrideFuelConsumptionRate.TabIndex = 16;
            this.overrideFuelConsumptionRate.Text = "Override";
            this.overrideFuelConsumptionRate.UseVisualStyleBackColor = true;
            // 
            // overrideInitialFuelLoad
            // 
            this.overrideInitialFuelLoad.AutoSize = true;
            this.overrideInitialFuelLoad.ComponentId = 0;
            this.overrideInitialFuelLoad.Controller = null;
            this.overrideInitialFuelLoad.Location = new System.Drawing.Point(166, 167);
            this.overrideInitialFuelLoad.Margin = new System.Windows.Forms.Padding(2);
            this.overrideInitialFuelLoad.Name = "overrideInitialFuelLoad";
            this.overrideInitialFuelLoad.ParameterCategory = "StartupParameters";
            this.overrideInitialFuelLoad.ParameterName = "OverrideInitialFuel";
            this.overrideInitialFuelLoad.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideInitialFuelLoad.Size = new System.Drawing.Size(66, 17);
            this.overrideInitialFuelLoad.TabIndex = 26;
            this.overrideInitialFuelLoad.Text = "Override";
            this.overrideInitialFuelLoad.UseVisualStyleBackColor = true;
            // 
            // overrideFuelCapacity
            // 
            this.overrideFuelCapacity.AutoSize = true;
            this.overrideFuelCapacity.ComponentId = 0;
            this.overrideFuelCapacity.Controller = null;
            this.overrideFuelCapacity.Location = new System.Drawing.Point(166, 116);
            this.overrideFuelCapacity.Margin = new System.Windows.Forms.Padding(2);
            this.overrideFuelCapacity.Name = "overrideFuelCapacity";
            this.overrideFuelCapacity.ParameterCategory = "StartupParameters";
            this.overrideFuelCapacity.ParameterName = "OverrideFuelCapacity";
            this.overrideFuelCapacity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelCapacity.Size = new System.Drawing.Size(66, 17);
            this.overrideFuelCapacity.TabIndex = 20;
            this.overrideFuelCapacity.Text = "Override";
            this.overrideFuelCapacity.UseVisualStyleBackColor = true;
            // 
            // overrideMaximumSpeed
            // 
            this.overrideMaximumSpeed.AutoSize = true;
            this.overrideMaximumSpeed.ComponentId = 0;
            this.overrideMaximumSpeed.Controller = null;
            this.overrideMaximumSpeed.Location = new System.Drawing.Point(166, 67);
            this.overrideMaximumSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.overrideMaximumSpeed.Name = "overrideMaximumSpeed";
            this.overrideMaximumSpeed.ParameterCategory = "StartupParameters";
            this.overrideMaximumSpeed.ParameterName = "OverrideMaxSpeed";
            this.overrideMaximumSpeed.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideMaximumSpeed.Size = new System.Drawing.Size(66, 17);
            this.overrideMaximumSpeed.TabIndex = 14;
            this.overrideMaximumSpeed.Text = "Override";
            this.overrideMaximumSpeed.UseVisualStyleBackColor = true;
            // 
            // overrideTimeToAttack
            // 
            this.overrideTimeToAttack.AutoSize = true;
            this.overrideTimeToAttack.ComponentId = 0;
            this.overrideTimeToAttack.Controller = null;
            this.overrideTimeToAttack.Location = new System.Drawing.Point(5, 168);
            this.overrideTimeToAttack.Margin = new System.Windows.Forms.Padding(2);
            this.overrideTimeToAttack.Name = "overrideTimeToAttack";
            this.overrideTimeToAttack.ParameterCategory = "StartupParameters";
            this.overrideTimeToAttack.ParameterName = "OverrideTimeToAttack";
            this.overrideTimeToAttack.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideTimeToAttack.Size = new System.Drawing.Size(66, 17);
            this.overrideTimeToAttack.TabIndex = 24;
            this.overrideTimeToAttack.Text = "Override";
            this.overrideTimeToAttack.UseVisualStyleBackColor = true;
            // 
            // overrideDockingDuration
            // 
            this.overrideDockingDuration.AutoSize = true;
            this.overrideDockingDuration.ComponentId = 0;
            this.overrideDockingDuration.Controller = null;
            this.overrideDockingDuration.Location = new System.Drawing.Point(4, 115);
            this.overrideDockingDuration.Margin = new System.Windows.Forms.Padding(2);
            this.overrideDockingDuration.Name = "overrideDockingDuration";
            this.overrideDockingDuration.ParameterCategory = "StartupParameters";
            this.overrideDockingDuration.ParameterName = "OverrideDockingDuration";
            this.overrideDockingDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideDockingDuration.Size = new System.Drawing.Size(66, 17);
            this.overrideDockingDuration.TabIndex = 18;
            this.overrideDockingDuration.Text = "Override";
            this.overrideDockingDuration.UseVisualStyleBackColor = true;
            // 
            // overrideLaunchDuration
            // 
            this.overrideLaunchDuration.AutoSize = true;
            this.overrideLaunchDuration.ComponentId = 0;
            this.overrideLaunchDuration.Controller = null;
            this.overrideLaunchDuration.Location = new System.Drawing.Point(4, 66);
            this.overrideLaunchDuration.Margin = new System.Windows.Forms.Padding(2);
            this.overrideLaunchDuration.Name = "overrideLaunchDuration";
            this.overrideLaunchDuration.ParameterCategory = "StartupParameters";
            this.overrideLaunchDuration.ParameterName = "OverrideLaunchDuration";
            this.overrideLaunchDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideLaunchDuration.Size = new System.Drawing.Size(66, 17);
            this.overrideLaunchDuration.TabIndex = 12;
            this.overrideLaunchDuration.Text = "Override";
            this.overrideLaunchDuration.UseVisualStyleBackColor = true;
            // 
            // overrideStealable
            // 
            this.overrideStealable.AutoSize = true;
            this.overrideStealable.ComponentId = 0;
            this.overrideStealable.Controller = null;
            this.overrideStealable.Location = new System.Drawing.Point(4, 18);
            this.overrideStealable.Margin = new System.Windows.Forms.Padding(2);
            this.overrideStealable.Name = "overrideStealable";
            this.overrideStealable.ParameterCategory = "StartupParameters";
            this.overrideStealable.ParameterName = "OverrideStealable";
            this.overrideStealable.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideStealable.Size = new System.Drawing.Size(66, 17);
            this.overrideStealable.TabIndex = 10;
            this.overrideStealable.Text = "Override";
            this.overrideStealable.UseVisualStyleBackColor = true;
            // 
            // stealable
            // 
            this.stealable.AutoSize = true;
            this.stealable.ComponentId = 0;
            this.stealable.Controller = null;
            this.stealable.Location = new System.Drawing.Point(70, 18);
            this.stealable.Name = "stealable";
            this.stealable.ParameterCategory = "StartupParameters";
            this.stealable.ParameterName = "Stealable";
            this.stealable.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.stealable.Size = new System.Drawing.Size(70, 17);
            this.stealable.TabIndex = 11;
            this.stealable.Text = "Stealable";
            this.stealable.UseVisualStyleBackColor = true;
            // 
            // fuelConsumption
            // 
            this.fuelConsumption.ComponentId = 0;
            this.fuelConsumption.Controller = null;
            this.fuelConsumption.Location = new System.Drawing.Point(410, 65);
            this.fuelConsumption.Name = "fuelConsumption";
            this.fuelConsumption.ParameterCategory = "StartupParameters";
            this.fuelConsumption.ParameterName = "FuelConsumption";
            this.fuelConsumption.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.fuelConsumption.Size = new System.Drawing.Size(83, 20);
            this.fuelConsumption.TabIndex = 17;
            this.fuelConsumption.Text = "0";
            this.fuelConsumption.Value = 0D;
            // 
            // initialFuel
            // 
            this.initialFuel.ComponentId = 0;
            this.initialFuel.Controller = null;
            this.initialFuel.Location = new System.Drawing.Point(233, 167);
            this.initialFuel.Name = "initialFuel";
            this.initialFuel.ParameterCategory = "StartupParameters";
            this.initialFuel.ParameterName = "InitialFuel";
            this.initialFuel.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.initialFuel.Size = new System.Drawing.Size(83, 20);
            this.initialFuel.TabIndex = 27;
            this.initialFuel.Text = "0";
            this.initialFuel.Value = 0D;
            // 
            // maxSpeed
            // 
            this.maxSpeed.ComponentId = 0;
            this.maxSpeed.Controller = null;
            this.maxSpeed.Location = new System.Drawing.Point(233, 66);
            this.maxSpeed.Name = "maxSpeed";
            this.maxSpeed.ParameterCategory = "StartupParameters";
            this.maxSpeed.ParameterName = "MaxSpeed";
            this.maxSpeed.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.maxSpeed.Size = new System.Drawing.Size(83, 20);
            this.maxSpeed.TabIndex = 15;
            this.maxSpeed.Text = "0";
            this.maxSpeed.Value = 0D;
            // 
            // fuelCapacity
            // 
            this.fuelCapacity.ComponentId = 0;
            this.fuelCapacity.Controller = null;
            this.fuelCapacity.Location = new System.Drawing.Point(233, 115);
            this.fuelCapacity.Name = "fuelCapacity";
            this.fuelCapacity.ParameterCategory = "StartupParameters";
            this.fuelCapacity.ParameterName = "FuelCapacity";
            this.fuelCapacity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.fuelCapacity.Size = new System.Drawing.Size(83, 20);
            this.fuelCapacity.TabIndex = 21;
            this.fuelCapacity.Text = "0";
            this.fuelCapacity.Value = 0D;
            // 
            // timeToAttack
            // 
            this.timeToAttack.ComponentId = 0;
            this.timeToAttack.Controller = null;
            this.timeToAttack.Location = new System.Drawing.Point(76, 167);
            this.timeToAttack.Name = "timeToAttack";
            this.timeToAttack.ParameterCategory = "StartupParameters";
            this.timeToAttack.ParameterName = "TimeToAttack";
            this.timeToAttack.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeToAttack.Size = new System.Drawing.Size(77, 20);
            this.timeToAttack.TabIndex = 25;
            this.timeToAttack.Text = "0";
            this.timeToAttack.Value = 0D;
            // 
            // dockingDuration
            // 
            this.dockingDuration.ComponentId = 0;
            this.dockingDuration.Controller = null;
            this.dockingDuration.Location = new System.Drawing.Point(70, 115);
            this.dockingDuration.Name = "dockingDuration";
            this.dockingDuration.ParameterCategory = "StartupParameters";
            this.dockingDuration.ParameterName = "DockingDuration";
            this.dockingDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.dockingDuration.Size = new System.Drawing.Size(83, 20);
            this.dockingDuration.TabIndex = 19;
            this.dockingDuration.Text = "0";
            this.dockingDuration.Value = 0D;
            // 
            // launchDuration
            // 
            this.launchDuration.ComponentId = 0;
            this.launchDuration.Controller = null;
            this.launchDuration.Location = new System.Drawing.Point(70, 65);
            this.launchDuration.Name = "launchDuration";
            this.launchDuration.ParameterCategory = "StartupParameters";
            this.launchDuration.ParameterName = "LaunchDuration";
            this.launchDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.launchDuration.Size = new System.Drawing.Size(83, 20);
            this.launchDuration.TabIndex = 13;
            this.launchDuration.Text = "0";
            this.launchDuration.Value = 0D;
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(223, 2);
            this.eventID1.Margin = new System.Windows.Forms.Padding(2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(198, 101);
            this.eventID1.TabIndex = 28;
            // 
            // engramRange
            // 
            this.engramRange.Controller = null;
            this.engramRange.DisplayID = -1;
            this.engramRange.Location = new System.Drawing.Point(11, 465);
            this.engramRange.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange.Name = "engramRange";
            this.engramRange.Size = new System.Drawing.Size(566, 403);
            this.engramRange.TabIndex = 30;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(63, 15);
            this.timeBox.Margin = new System.Windows.Forms.Padding(2);
            this.timeBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timeBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeBox.Name = "timeBox";
            this.timeBox.ParameterCategory = "RevealEvent";
            this.timeBox.ParameterName = "Time";
            this.timeBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeBox.Size = new System.Drawing.Size(91, 20);
            this.timeBox.TabIndex = 1;
            this.timeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = null;
            this.customLinkBox1.ConnectRootId = -1;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = null;
            this.customLinkBox1.DisplayLinkType = null;
            this.customLinkBox1.DisplayParameterCategory = "";
            this.customLinkBox1.DisplayParameterName = "";
            this.customLinkBox1.DisplayRecursiveCheck = false;
            this.customLinkBox1.DisplayRootId = -1;
            this.customLinkBox1.FilterResult = false;
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.Location = new System.Drawing.Point(429, 33);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = false;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(169, 79);
            this.customLinkBox1.TabIndex = 50;
            // 
            // EvtPnl_Reveal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customLinkBox1);
            this.Controls.Add(this.customLinkComboBoxLinkedRegion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.initialTagParameterTextBox);
            this.Controls.Add(this.initialStateComboBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.zBox);
            this.Controls.Add(this.yBox);
            this.Controls.Add(this.xBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.MinimumSize = new System.Drawing.Size(365, 483);
            this.Name = "EvtPnl_Reveal";
            this.Size = new System.Drawing.Size(601, 884);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomParameterTextBox xBox;
        private AME.Views.View_Components.CustomParameterTextBox yBox;
        private AME.Views.View_Components.CustomParameterTextBox zBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private EngramRange engramRange;
        private EventID eventID1;
        private System.Windows.Forms.GroupBox groupBox3;
        private AME.Views.View_Components.CustomCheckBox stealable;
        private AME.Views.View_Components.CustomNonnegativeDouble fuelConsumption;
        private System.Windows.Forms.Label launchDurationLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble initialFuel;
        private System.Windows.Forms.Label dockingDurationLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble maxSpeed;
        private System.Windows.Forms.Label timeToAttackLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble fuelCapacity;
        private System.Windows.Forms.Label maximumSpeedLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble timeToAttack;
        private System.Windows.Forms.Label fuelCapacityLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble dockingDuration;
        private System.Windows.Forms.Label initialFuelLoadLabel;
        private AME.Views.View_Components.CustomNonnegativeDouble launchDuration;
        private System.Windows.Forms.Label fuelConsumptionRateLabel;
        private System.Windows.Forms.Label fuelDepletionStateLabel;
        private AME.Views.View_Components.CustomCheckBox overrideStealable;
        private AME.Views.View_Components.CustomCheckBox overrideFuelDepletionState;
        private AME.Views.View_Components.CustomCheckBox overrideFuelConsumptionRate;
        private AME.Views.View_Components.CustomCheckBox overrideInitialFuelLoad;
        private AME.Views.View_Components.CustomCheckBox overrideFuelCapacity;
        private AME.Views.View_Components.CustomCheckBox overrideMaximumSpeed;
        private AME.Views.View_Components.CustomCheckBox overrideTimeToAttack;
        private AME.Views.View_Components.CustomCheckBox overrideDockingDuration;
        private AME.Views.View_Components.CustomCheckBox overrideLaunchDuration;
        private AME.Views.View_Components.CustomCheckBox overrideIcon;
        private IconListView iconListView1;
        private System.Windows.Forms.Label iconLabel;
        private StateComboBox initialStateComboBox;
        private StateComboBox fuelDepletionStateComboBox;
        private AME.Views.View_Components.CustomParameterTextBox initialTagParameterTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomLinkComboBox customLinkComboBoxLinkedRegion;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;
    }
}
