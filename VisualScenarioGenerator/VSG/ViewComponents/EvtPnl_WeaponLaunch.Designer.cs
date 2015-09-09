namespace VSG.ViewComponents
{
    partial class EvtPnl_WeaponLaunch
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
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.iconLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.launchDurationLabel = new System.Windows.Forms.Label();
            this.dockingDurationLabel = new System.Windows.Forms.Label();
            this.timeToAttackLabel = new System.Windows.Forms.Label();
            this.maximumSpeedLabel = new System.Windows.Forms.Label();
            this.fuelCapacityLabel = new System.Windows.Forms.Label();
            this.initialFuelLoadLabel = new System.Windows.Forms.Label();
            this.fuelConsumptionRateLabel = new System.Windows.Forms.Label();
            this.fuelDepletionStateLabel = new System.Windows.Forms.Label();
            this.weaponLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.targetLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.initialStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.fuelDepletionStateComboBox1 = new VSG.ViewComponents.StateComboBox(this.components);
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
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Time (s.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "Target:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(518, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Initial State:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 28;
            this.label1.Text = "Launch Weapon:";
            // 
            // iconLabel
            // 
            this.iconLabel.AutoSize = true;
            this.iconLabel.Location = new System.Drawing.Point(91, 247);
            this.iconLabel.Margin = new System.Windows.Forms.Padding(4);
            this.iconLabel.Name = "iconLabel";
            this.iconLabel.Size = new System.Drawing.Size(38, 17);
            this.iconLabel.TabIndex = 40;
            this.iconLabel.Text = "Icon:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.fuelDepletionStateComboBox1);
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
            this.groupBox3.Location = new System.Drawing.Point(9, 255);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(745, 384);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Startup Parameters";
            // 
            // launchDurationLabel
            // 
            this.launchDurationLabel.AutoSize = true;
            this.launchDurationLabel.Location = new System.Drawing.Point(91, 62);
            this.launchDurationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.launchDurationLabel.Name = "launchDurationLabel";
            this.launchDurationLabel.Size = new System.Drawing.Size(142, 17);
            this.launchDurationLabel.TabIndex = 0;
            this.launchDurationLabel.Text = "Launch Duration (s.):";
            // 
            // dockingDurationLabel
            // 
            this.dockingDurationLabel.AutoSize = true;
            this.dockingDurationLabel.Location = new System.Drawing.Point(91, 121);
            this.dockingDurationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dockingDurationLabel.Name = "dockingDurationLabel";
            this.dockingDurationLabel.Size = new System.Drawing.Size(146, 17);
            this.dockingDurationLabel.TabIndex = 1;
            this.dockingDurationLabel.Text = "Docking Duration (s.):";
            // 
            // timeToAttackLabel
            // 
            this.timeToAttackLabel.AutoSize = true;
            this.timeToAttackLabel.Location = new System.Drawing.Point(91, 186);
            this.timeToAttackLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeToAttackLabel.Name = "timeToAttackLabel";
            this.timeToAttackLabel.Size = new System.Drawing.Size(132, 17);
            this.timeToAttackLabel.TabIndex = 2;
            this.timeToAttackLabel.Text = "Time To Attack (s.):";
            // 
            // maximumSpeedLabel
            // 
            this.maximumSpeedLabel.AutoSize = true;
            this.maximumSpeedLabel.Location = new System.Drawing.Point(307, 62);
            this.maximumSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.maximumSpeedLabel.Name = "maximumSpeedLabel";
            this.maximumSpeedLabel.Size = new System.Drawing.Size(151, 17);
            this.maximumSpeedLabel.TabIndex = 3;
            this.maximumSpeedLabel.Text = "Maximum Speed (m/s):";
            // 
            // fuelCapacityLabel
            // 
            this.fuelCapacityLabel.AutoSize = true;
            this.fuelCapacityLabel.Location = new System.Drawing.Point(307, 122);
            this.fuelCapacityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fuelCapacityLabel.Name = "fuelCapacityLabel";
            this.fuelCapacityLabel.Size = new System.Drawing.Size(143, 17);
            this.fuelCapacityLabel.TabIndex = 4;
            this.fuelCapacityLabel.Text = "Fuel Capacity (Units):";
            // 
            // initialFuelLoadLabel
            // 
            this.initialFuelLoadLabel.AutoSize = true;
            this.initialFuelLoadLabel.Location = new System.Drawing.Point(541, 62);
            this.initialFuelLoadLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.initialFuelLoadLabel.Name = "initialFuelLoadLabel";
            this.initialFuelLoadLabel.Size = new System.Drawing.Size(157, 17);
            this.initialFuelLoadLabel.TabIndex = 5;
            this.initialFuelLoadLabel.Text = "Initial Fuel Load (Units):";
            // 
            // fuelConsumptionRateLabel
            // 
            this.fuelConsumptionRateLabel.AutoSize = true;
            this.fuelConsumptionRateLabel.Location = new System.Drawing.Point(307, 186);
            this.fuelConsumptionRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fuelConsumptionRateLabel.Name = "fuelConsumptionRateLabel";
            this.fuelConsumptionRateLabel.Size = new System.Drawing.Size(216, 17);
            this.fuelConsumptionRateLabel.TabIndex = 6;
            this.fuelConsumptionRateLabel.Text = "Fuel Consumption Rate (Units/s):";
            // 
            // fuelDepletionStateLabel
            // 
            this.fuelDepletionStateLabel.AutoSize = true;
            this.fuelDepletionStateLabel.Location = new System.Drawing.Point(541, 121);
            this.fuelDepletionStateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fuelDepletionStateLabel.Name = "fuelDepletionStateLabel";
            this.fuelDepletionStateLabel.Size = new System.Drawing.Size(140, 17);
            this.fuelDepletionStateLabel.TabIndex = 7;
            this.fuelDepletionStateLabel.Text = "Fuel Depletion State:";
            // 
            // weaponLinkBox
            // 
            this.weaponLinkBox.CheckLinkLevel = ((uint)(1u));
            this.weaponLinkBox.CheckOnClick = true;
            this.weaponLinkBox.ConnectFromId = -1;
            this.weaponLinkBox.ConnectLinkType = null;
            this.weaponLinkBox.ConnectRootId = -1;
            this.weaponLinkBox.Controller = null;
            this.weaponLinkBox.DisplayComponentType = null;
            this.weaponLinkBox.DisplayLinkType = null;
            this.weaponLinkBox.DisplayParameterCategory = "";
            this.weaponLinkBox.DisplayParameterName = "";
            this.weaponLinkBox.DisplayRecursiveCheck = false;
            this.weaponLinkBox.DisplayRootId = -1;
            this.weaponLinkBox.FilterResult = false;
            this.weaponLinkBox.FormattingEnabled = true;
            this.weaponLinkBox.Location = new System.Drawing.Point(249, 132);
            this.weaponLinkBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.weaponLinkBox.Name = "weaponLinkBox";
            this.weaponLinkBox.OneToMany = false;
            this.weaponLinkBox.ParameterFilterCategory = "";
            this.weaponLinkBox.ParameterFilterName = "";
            this.weaponLinkBox.ParameterFilterValue = "";
            this.weaponLinkBox.Size = new System.Drawing.Size(251, 106);
            this.weaponLinkBox.TabIndex = 6;
            // 
            // targetLinkBox
            // 
            this.targetLinkBox.CheckLinkLevel = ((uint)(1u));
            this.targetLinkBox.CheckOnClick = true;
            this.targetLinkBox.ConnectFromId = -1;
            this.targetLinkBox.ConnectLinkType = null;
            this.targetLinkBox.ConnectRootId = -1;
            this.targetLinkBox.Controller = null;
            this.targetLinkBox.DisplayComponentType = null;
            this.targetLinkBox.DisplayLinkType = null;
            this.targetLinkBox.DisplayParameterCategory = "";
            this.targetLinkBox.DisplayParameterName = "";
            this.targetLinkBox.DisplayRecursiveCheck = false;
            this.targetLinkBox.DisplayRootId = -1;
            this.targetLinkBox.FilterResult = false;
            this.targetLinkBox.FormattingEnabled = true;
            this.targetLinkBox.Location = new System.Drawing.Point(9, 130);
            this.targetLinkBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.targetLinkBox.Name = "targetLinkBox";
            this.targetLinkBox.OneToMany = false;
            this.targetLinkBox.ParameterFilterCategory = "";
            this.targetLinkBox.ParameterFilterName = "";
            this.targetLinkBox.ParameterFilterValue = "";
            this.targetLinkBox.Size = new System.Drawing.Size(223, 106);
            this.targetLinkBox.TabIndex = 31;
            // 
            // initialStateComboBox
            // 
            this.initialStateComboBox.ComponentId = -1;
            this.initialStateComboBox.Controller = null;
            this.initialStateComboBox.FormattingEnabled = true;
            this.initialStateComboBox.Location = new System.Drawing.Point(507, 160);
            this.initialStateComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.initialStateComboBox.Name = "initialStateComboBox";
            this.initialStateComboBox.ParameterCategory = "WeaponLaunchEvent";
            this.initialStateComboBox.ParameterName = "InitialState";
            this.initialStateComboBox.ShowAllStates = false;
            this.initialStateComboBox.Size = new System.Drawing.Size(236, 24);
            this.initialStateComboBox.SpeciesId = -1;
            this.initialStateComboBox.TabIndex = 7;
            // 
            // fuelDepletionStateComboBox1
            // 
            this.fuelDepletionStateComboBox1.ComponentId = -1;
            this.fuelDepletionStateComboBox1.Controller = null;
            this.fuelDepletionStateComboBox1.FormattingEnabled = true;
            this.fuelDepletionStateComboBox1.Location = new System.Drawing.Point(545, 143);
            this.fuelDepletionStateComboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.fuelDepletionStateComboBox1.Name = "fuelDepletionStateComboBox1";
            this.fuelDepletionStateComboBox1.ParameterCategory = "State";
            this.fuelDepletionStateComboBox1.ParameterName = "FuelDepletionState";
            this.fuelDepletionStateComboBox1.ShowAllStates = false;
            this.fuelDepletionStateComboBox1.Size = new System.Drawing.Size(179, 24);
            this.fuelDepletionStateComboBox1.SpeciesId = -1;
            this.fuelDepletionStateComboBox1.TabIndex = 21;
            // 
            // overrideIcon
            // 
            this.overrideIcon.AutoSize = true;
            this.overrideIcon.ComponentId = 0;
            this.overrideIcon.Controller = null;
            this.overrideIcon.Location = new System.Drawing.Point(7, 304);
            this.overrideIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideIcon.Name = "overrideIcon";
            this.overrideIcon.ParameterCategory = "StartupParameters";
            this.overrideIcon.ParameterName = "OverrideIcon";
            this.overrideIcon.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideIcon.Size = new System.Drawing.Size(85, 21);
            this.overrideIcon.TabIndex = 26;
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
            this.iconListView1.Location = new System.Drawing.Point(101, 271);
            this.iconListView1.Margin = new System.Windows.Forms.Padding(4);
            this.iconListView1.MultiSelect = false;
            this.iconListView1.Name = "iconListView1";
            this.iconListView1.Size = new System.Drawing.Size(599, 91);
            this.iconListView1.TabIndex = 27;
            this.iconListView1.UseCompatibleStateImageBehavior = false;
            // 
            // overrideFuelDepletionState
            // 
            this.overrideFuelDepletionState.AutoSize = true;
            this.overrideFuelDepletionState.ComponentId = 0;
            this.overrideFuelDepletionState.Controller = null;
            this.overrideFuelDepletionState.Location = new System.Drawing.Point(453, 142);
            this.overrideFuelDepletionState.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideFuelDepletionState.Name = "overrideFuelDepletionState";
            this.overrideFuelDepletionState.ParameterCategory = "StartupParameters";
            this.overrideFuelDepletionState.ParameterName = "OverrideFuelDepletionState";
            this.overrideFuelDepletionState.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelDepletionState.Size = new System.Drawing.Size(85, 21);
            this.overrideFuelDepletionState.TabIndex = 20;
            this.overrideFuelDepletionState.Text = "Override";
            this.overrideFuelDepletionState.UseVisualStyleBackColor = true;
            // 
            // overrideFuelConsumptionRate
            // 
            this.overrideFuelConsumptionRate.AutoSize = true;
            this.overrideFuelConsumptionRate.ComponentId = 0;
            this.overrideFuelConsumptionRate.Controller = null;
            this.overrideFuelConsumptionRate.Location = new System.Drawing.Point(219, 204);
            this.overrideFuelConsumptionRate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideFuelConsumptionRate.Name = "overrideFuelConsumptionRate";
            this.overrideFuelConsumptionRate.ParameterCategory = "StartupParameters";
            this.overrideFuelConsumptionRate.ParameterName = "OverrideFuelConsumption";
            this.overrideFuelConsumptionRate.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelConsumptionRate.Size = new System.Drawing.Size(85, 21);
            this.overrideFuelConsumptionRate.TabIndex = 24;
            this.overrideFuelConsumptionRate.Text = "Override";
            this.overrideFuelConsumptionRate.UseVisualStyleBackColor = true;
            // 
            // overrideInitialFuelLoad
            // 
            this.overrideInitialFuelLoad.AutoSize = true;
            this.overrideInitialFuelLoad.ComponentId = 0;
            this.overrideInitialFuelLoad.Controller = null;
            this.overrideInitialFuelLoad.Location = new System.Drawing.Point(453, 82);
            this.overrideInitialFuelLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideInitialFuelLoad.Name = "overrideInitialFuelLoad";
            this.overrideInitialFuelLoad.ParameterCategory = "StartupParameters";
            this.overrideInitialFuelLoad.ParameterName = "OverrideInitialFuel";
            this.overrideInitialFuelLoad.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideInitialFuelLoad.Size = new System.Drawing.Size(85, 21);
            this.overrideInitialFuelLoad.TabIndex = 14;
            this.overrideInitialFuelLoad.Text = "Override";
            this.overrideInitialFuelLoad.UseVisualStyleBackColor = true;
            // 
            // overrideFuelCapacity
            // 
            this.overrideFuelCapacity.AutoSize = true;
            this.overrideFuelCapacity.ComponentId = 0;
            this.overrideFuelCapacity.Controller = null;
            this.overrideFuelCapacity.Location = new System.Drawing.Point(221, 143);
            this.overrideFuelCapacity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideFuelCapacity.Name = "overrideFuelCapacity";
            this.overrideFuelCapacity.ParameterCategory = "StartupParameters";
            this.overrideFuelCapacity.ParameterName = "OverrideFuelCapacity";
            this.overrideFuelCapacity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideFuelCapacity.Size = new System.Drawing.Size(85, 21);
            this.overrideFuelCapacity.TabIndex = 18;
            this.overrideFuelCapacity.Text = "Override";
            this.overrideFuelCapacity.UseVisualStyleBackColor = true;
            // 
            // overrideMaximumSpeed
            // 
            this.overrideMaximumSpeed.AutoSize = true;
            this.overrideMaximumSpeed.ComponentId = 0;
            this.overrideMaximumSpeed.Controller = null;
            this.overrideMaximumSpeed.Location = new System.Drawing.Point(221, 82);
            this.overrideMaximumSpeed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideMaximumSpeed.Name = "overrideMaximumSpeed";
            this.overrideMaximumSpeed.ParameterCategory = "StartupParameters";
            this.overrideMaximumSpeed.ParameterName = "OverrideMaxSpeed";
            this.overrideMaximumSpeed.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideMaximumSpeed.Size = new System.Drawing.Size(85, 21);
            this.overrideMaximumSpeed.TabIndex = 12;
            this.overrideMaximumSpeed.Text = "Override";
            this.overrideMaximumSpeed.UseVisualStyleBackColor = true;
            // 
            // overrideTimeToAttack
            // 
            this.overrideTimeToAttack.AutoSize = true;
            this.overrideTimeToAttack.ComponentId = 0;
            this.overrideTimeToAttack.Controller = null;
            this.overrideTimeToAttack.Location = new System.Drawing.Point(7, 207);
            this.overrideTimeToAttack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideTimeToAttack.Name = "overrideTimeToAttack";
            this.overrideTimeToAttack.ParameterCategory = "StartupParameters";
            this.overrideTimeToAttack.ParameterName = "OverrideTimeToAttack";
            this.overrideTimeToAttack.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideTimeToAttack.Size = new System.Drawing.Size(85, 21);
            this.overrideTimeToAttack.TabIndex = 22;
            this.overrideTimeToAttack.Text = "Override";
            this.overrideTimeToAttack.UseVisualStyleBackColor = true;
            // 
            // overrideDockingDuration
            // 
            this.overrideDockingDuration.AutoSize = true;
            this.overrideDockingDuration.ComponentId = 0;
            this.overrideDockingDuration.Controller = null;
            this.overrideDockingDuration.Location = new System.Drawing.Point(5, 142);
            this.overrideDockingDuration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideDockingDuration.Name = "overrideDockingDuration";
            this.overrideDockingDuration.ParameterCategory = "StartupParameters";
            this.overrideDockingDuration.ParameterName = "OverrideDockingDuration";
            this.overrideDockingDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideDockingDuration.Size = new System.Drawing.Size(85, 21);
            this.overrideDockingDuration.TabIndex = 16;
            this.overrideDockingDuration.Text = "Override";
            this.overrideDockingDuration.UseVisualStyleBackColor = true;
            // 
            // overrideLaunchDuration
            // 
            this.overrideLaunchDuration.AutoSize = true;
            this.overrideLaunchDuration.ComponentId = 0;
            this.overrideLaunchDuration.Controller = null;
            this.overrideLaunchDuration.Location = new System.Drawing.Point(5, 81);
            this.overrideLaunchDuration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideLaunchDuration.Name = "overrideLaunchDuration";
            this.overrideLaunchDuration.ParameterCategory = "StartupParameters";
            this.overrideLaunchDuration.ParameterName = "OverrideLaunchDuration";
            this.overrideLaunchDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideLaunchDuration.Size = new System.Drawing.Size(85, 21);
            this.overrideLaunchDuration.TabIndex = 10;
            this.overrideLaunchDuration.Text = "Override";
            this.overrideLaunchDuration.UseVisualStyleBackColor = true;
            // 
            // overrideStealable
            // 
            this.overrideStealable.AutoSize = true;
            this.overrideStealable.ComponentId = 0;
            this.overrideStealable.Controller = null;
            this.overrideStealable.Location = new System.Drawing.Point(5, 22);
            this.overrideStealable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overrideStealable.Name = "overrideStealable";
            this.overrideStealable.ParameterCategory = "StartupParameters";
            this.overrideStealable.ParameterName = "OverrideStealable";
            this.overrideStealable.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.overrideStealable.Size = new System.Drawing.Size(85, 21);
            this.overrideStealable.TabIndex = 8;
            this.overrideStealable.Text = "Override";
            this.overrideStealable.UseVisualStyleBackColor = true;
            // 
            // stealable
            // 
            this.stealable.AutoSize = true;
            this.stealable.ComponentId = 0;
            this.stealable.Controller = null;
            this.stealable.Location = new System.Drawing.Point(93, 22);
            this.stealable.Margin = new System.Windows.Forms.Padding(4);
            this.stealable.Name = "stealable";
            this.stealable.ParameterCategory = "StartupParameters";
            this.stealable.ParameterName = "Stealable";
            this.stealable.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.stealable.Size = new System.Drawing.Size(89, 21);
            this.stealable.TabIndex = 9;
            this.stealable.Text = "Stealable";
            this.stealable.UseVisualStyleBackColor = true;
            // 
            // fuelConsumption
            // 
            this.fuelConsumption.ComponentId = 0;
            this.fuelConsumption.Controller = null;
            this.fuelConsumption.Location = new System.Drawing.Point(312, 204);
            this.fuelConsumption.Margin = new System.Windows.Forms.Padding(4);
            this.fuelConsumption.Name = "fuelConsumption";
            this.fuelConsumption.ParameterCategory = "StartupParameters";
            this.fuelConsumption.ParameterName = "FuelConsumption";
            this.fuelConsumption.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.fuelConsumption.Size = new System.Drawing.Size(109, 22);
            this.fuelConsumption.TabIndex = 25;
            this.fuelConsumption.Text = "0";
            this.fuelConsumption.Value = 0;
            // 
            // initialFuel
            // 
            this.initialFuel.ComponentId = 0;
            this.initialFuel.Controller = null;
            this.initialFuel.Location = new System.Drawing.Point(545, 81);
            this.initialFuel.Margin = new System.Windows.Forms.Padding(4);
            this.initialFuel.Name = "initialFuel";
            this.initialFuel.ParameterCategory = "StartupParameters";
            this.initialFuel.ParameterName = "InitialFuel";
            this.initialFuel.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.initialFuel.Size = new System.Drawing.Size(109, 22);
            this.initialFuel.TabIndex = 15;
            this.initialFuel.Text = "0";
            this.initialFuel.Value = 0;
            // 
            // maxSpeed
            // 
            this.maxSpeed.ComponentId = 0;
            this.maxSpeed.Controller = null;
            this.maxSpeed.Location = new System.Drawing.Point(311, 81);
            this.maxSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.maxSpeed.Name = "maxSpeed";
            this.maxSpeed.ParameterCategory = "StartupParameters";
            this.maxSpeed.ParameterName = "MaxSpeed";
            this.maxSpeed.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.maxSpeed.Size = new System.Drawing.Size(109, 22);
            this.maxSpeed.TabIndex = 13;
            this.maxSpeed.Text = "0";
            this.maxSpeed.Value = 0;
            // 
            // fuelCapacity
            // 
            this.fuelCapacity.ComponentId = 0;
            this.fuelCapacity.Controller = null;
            this.fuelCapacity.Location = new System.Drawing.Point(311, 142);
            this.fuelCapacity.Margin = new System.Windows.Forms.Padding(4);
            this.fuelCapacity.Name = "fuelCapacity";
            this.fuelCapacity.ParameterCategory = "StartupParameters";
            this.fuelCapacity.ParameterName = "FuelCapacity";
            this.fuelCapacity.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.fuelCapacity.Size = new System.Drawing.Size(109, 22);
            this.fuelCapacity.TabIndex = 19;
            this.fuelCapacity.Text = "0";
            this.fuelCapacity.Value = 0;
            // 
            // timeToAttack
            // 
            this.timeToAttack.ComponentId = 0;
            this.timeToAttack.Controller = null;
            this.timeToAttack.Location = new System.Drawing.Point(95, 202);
            this.timeToAttack.Margin = new System.Windows.Forms.Padding(4);
            this.timeToAttack.Name = "timeToAttack";
            this.timeToAttack.ParameterCategory = "StartupParameters";
            this.timeToAttack.ParameterName = "TimeToAttack";
            this.timeToAttack.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeToAttack.Size = new System.Drawing.Size(109, 22);
            this.timeToAttack.TabIndex = 23;
            this.timeToAttack.Text = "0";
            this.timeToAttack.Value = 0;
            // 
            // dockingDuration
            // 
            this.dockingDuration.ComponentId = 0;
            this.dockingDuration.Controller = null;
            this.dockingDuration.Location = new System.Drawing.Point(93, 142);
            this.dockingDuration.Margin = new System.Windows.Forms.Padding(4);
            this.dockingDuration.Name = "dockingDuration";
            this.dockingDuration.ParameterCategory = "StartupParameters";
            this.dockingDuration.ParameterName = "DockingDuration";
            this.dockingDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.dockingDuration.Size = new System.Drawing.Size(109, 22);
            this.dockingDuration.TabIndex = 17;
            this.dockingDuration.Text = "0";
            this.dockingDuration.Value = 0;
            // 
            // launchDuration
            // 
            this.launchDuration.ComponentId = 0;
            this.launchDuration.Controller = null;
            this.launchDuration.Location = new System.Drawing.Point(93, 80);
            this.launchDuration.Margin = new System.Windows.Forms.Padding(4);
            this.launchDuration.Name = "launchDuration";
            this.launchDuration.ParameterCategory = "StartupParameters";
            this.launchDuration.ParameterName = "LaunchDuration";
            this.launchDuration.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.launchDuration.Size = new System.Drawing.Size(109, 22);
            this.launchDuration.TabIndex = 11;
            this.launchDuration.Text = "0";
            this.launchDuration.Value = 0;
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(245, 0);
            this.eventID1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(509, 124);
            this.eventID1.TabIndex = 2;
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(9, 645);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(372, 293);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(745, 513);
            this.engramRange1.TabIndex = 28;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(97, 26);
            this.timeBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.timeBox.ParameterCategory = "WeaponLaunchEvent";
            this.timeBox.ParameterName = "Time";
            this.timeBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeBox.Size = new System.Drawing.Size(121, 22);
            this.timeBox.TabIndex = 1;
            this.timeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EvtPnl_WeaponLaunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetLinkBox);
            this.Controls.Add(this.initialStateComboBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weaponLinkBox);
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(461, 646);
            this.Name = "EvtPnl_WeaponLaunch";
            this.Size = new System.Drawing.Size(787, 1151);
            this.Load += new System.EventHandler(this.EvtPnl_Launch_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomLinkBox weaponLinkBox;
        private EngramRange engramRange1;
        private EventID eventID1;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomCheckBox overrideInitialFuelLoad;
        private AME.Views.View_Components.CustomCheckBox overrideFuelCapacity;
        private AME.Views.View_Components.CustomCheckBox overrideMaximumSpeed;
        private AME.Views.View_Components.CustomCheckBox overrideTimeToAttack;
        private AME.Views.View_Components.CustomCheckBox overrideDockingDuration;
        private AME.Views.View_Components.CustomCheckBox overrideLaunchDuration;
        private AME.Views.View_Components.CustomCheckBox overrideIcon;
        private IconListView iconListView1;
        private System.Windows.Forms.Label iconLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private AME.Views.View_Components.CustomCheckBox overrideFuelDepletionState;
        private AME.Views.View_Components.CustomCheckBox overrideFuelConsumptionRate;
        private AME.Views.View_Components.CustomCheckBox overrideStealable;
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
        private StateComboBox initialStateComboBox;
        private StateComboBox fuelDepletionStateComboBox1;
        private AME.Views.View_Components.CustomLinkBox targetLinkBox;

    }
}
