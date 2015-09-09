namespace VSG.ViewComponents
{
    partial class Species
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.customCheckBoxLaunchedByOwner = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.classificationRule1 = new VSG.ViewComponents.ClassificationRule(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.simpleLinkGrid1 = new VSG.ViewComponents.SimpleLinkGrid(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.customLinkBoxDecisionMakers = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.ckbxIsWeapon = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.ckbxRemoveOnDestruction = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.nndCollisionRadius = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbExistingSpecies = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.rbAir = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.rbSea = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.rbLand = new AME.Views.View_Components.CustomRadioButton(this.components);
            this.lkbxType = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLinkGrid1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // customCheckBoxLaunchedByOwner
            // 
            this.customCheckBoxLaunchedByOwner.ComponentId = -1;
            this.customCheckBoxLaunchedByOwner.Controller = null;
            this.customCheckBoxLaunchedByOwner.Location = new System.Drawing.Point(6, 62);
            this.customCheckBoxLaunchedByOwner.Name = "customCheckBoxLaunchedByOwner";
            this.customCheckBoxLaunchedByOwner.ParameterCategory = "Species";
            this.customCheckBoxLaunchedByOwner.ParameterName = "LaunchedByOwner";
            this.customCheckBoxLaunchedByOwner.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBoxLaunchedByOwner.Size = new System.Drawing.Size(186, 33);
            this.customCheckBoxLaunchedByOwner.TabIndex = 5;
            this.customCheckBoxLaunchedByOwner.Text = "This species can be launched by it\'s owner:";
            this.toolTip1.SetToolTip(this.customCheckBoxLaunchedByOwner, "If true, this species can be launched (as a subplatform or weapon) by the it\'s ow" +
                    "ner.  \r\nOtherwise, only the platform\'s owner can launch the subplatform.");
            this.customCheckBoxLaunchedByOwner.UseVisualStyleBackColor = true;
            this.customCheckBoxLaunchedByOwner.CheckedChanged += new System.EventHandler(this.customCheckBoxLaunchedByOwner_CheckedChanged);
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(867, 917);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Controls.Add(this.classificationRule1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(859, 891);
            this.customTabPage1.TabIndex = 1;
            this.customTabPage1.Text = "Unit Type";
            // 
            // classificationRule1
            // 
            this.classificationRule1.ComponentId = -1;
            this.classificationRule1.Controller = null;
            this.classificationRule1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classificationRule1.Link = "";
            this.classificationRule1.Location = new System.Drawing.Point(4, 522);
            this.classificationRule1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.classificationRule1.Name = "classificationRule1";
            this.classificationRule1.ParameterCategory = null;
            this.classificationRule1.ParameterName = null;
            this.classificationRule1.RootId = -1;
            this.classificationRule1.Size = new System.Drawing.Size(850, 261);
            this.classificationRule1.SourceLink = "";
            this.classificationRule1.TabIndex = 49;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(2, 4);
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 486);
            this.panel1.TabIndex = 48;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.simpleLinkGrid1);
            this.groupBox3.Location = new System.Drawing.Point(12, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(306, 146);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Subplatform Capacity";
            // 
            // simpleLinkGrid1
            // 
            this.simpleLinkGrid1.AllowUserToAddRows = false;
            this.simpleLinkGrid1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.simpleLinkGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.simpleLinkGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simpleLinkGrid1.ComponentFrom = "Species";
            this.simpleLinkGrid1.ComponentId = -1;
            this.simpleLinkGrid1.ComponentTo = "Species";
            this.simpleLinkGrid1.Controller = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.simpleLinkGrid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.simpleLinkGrid1.Link = "SpeciesSubplatformCapacity";
            this.simpleLinkGrid1.Location = new System.Drawing.Point(6, 18);
            this.simpleLinkGrid1.Name = "simpleLinkGrid1";
            this.simpleLinkGrid1.Parameter = "Count";
            this.simpleLinkGrid1.ParameterCategory = "Capacity";
            this.simpleLinkGrid1.RootId = -1;
            this.simpleLinkGrid1.RowTemplate.Height = 24;
            this.simpleLinkGrid1.Size = new System.Drawing.Size(294, 122);
            this.simpleLinkGrid1.SourceLink = "Scenario";
            this.simpleLinkGrid1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.customLinkBoxDecisionMakers);
            this.groupBox2.Controls.Add(this.customCheckBoxLaunchedByOwner);
            this.groupBox2.Controls.Add(this.ckbxIsWeapon);
            this.groupBox2.Controls.Add(this.ckbxRemoveOnDestruction);
            this.groupBox2.Controls.Add(this.nndCollisionRadius);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 183);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optional Parameters";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 44);
            this.label5.TabIndex = 9;
            this.label5.Text = "Select which Decision Makers are allowed to own this species:";
            // 
            // customLinkBoxDecisionMakers
            // 
            this.customLinkBoxDecisionMakers.CheckLinkLevel = ((uint)(1u));
            this.customLinkBoxDecisionMakers.CheckOnClick = true;
            this.customLinkBoxDecisionMakers.ConnectFromId = -1;
            this.customLinkBoxDecisionMakers.ConnectLinkType = "SpeciesDMCanOwn";
            this.customLinkBoxDecisionMakers.ConnectRootId = -1;
            this.customLinkBoxDecisionMakers.Controller = null;
            this.customLinkBoxDecisionMakers.DisplayComponentType = "DecisionMaker";
            this.customLinkBoxDecisionMakers.DisplayLinkType = "Scenario";
            this.customLinkBoxDecisionMakers.DisplayParameterCategory = "";
            this.customLinkBoxDecisionMakers.DisplayParameterName = "";
            this.customLinkBoxDecisionMakers.DisplayRecursiveCheck = false;
            this.customLinkBoxDecisionMakers.DisplayRootId = -1;
            this.customLinkBoxDecisionMakers.FilterResult = false;
            this.customLinkBoxDecisionMakers.FormattingEnabled = true;
            this.customLinkBoxDecisionMakers.Location = new System.Drawing.Point(181, 98);
            this.customLinkBoxDecisionMakers.Name = "customLinkBoxDecisionMakers";
            this.customLinkBoxDecisionMakers.OneToMany = true;
            this.customLinkBoxDecisionMakers.ParameterFilterCategory = "";
            this.customLinkBoxDecisionMakers.ParameterFilterName = "";
            this.customLinkBoxDecisionMakers.ParameterFilterValue = "";
            this.customLinkBoxDecisionMakers.Size = new System.Drawing.Size(120, 64);
            this.customLinkBoxDecisionMakers.TabIndex = 8;
            // 
            // ckbxIsWeapon
            // 
            this.ckbxIsWeapon.AutoSize = true;
            this.ckbxIsWeapon.ComponentId = 0;
            this.ckbxIsWeapon.Controller = null;
            this.ckbxIsWeapon.Location = new System.Drawing.Point(6, 19);
            this.ckbxIsWeapon.Name = "ckbxIsWeapon";
            this.ckbxIsWeapon.ParameterCategory = "Species";
            this.ckbxIsWeapon.ParameterName = "IsWeapon";
            this.ckbxIsWeapon.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ckbxIsWeapon.Size = new System.Drawing.Size(75, 17);
            this.ckbxIsWeapon.TabIndex = 2;
            this.ckbxIsWeapon.Text = "Is weapon";
            this.ckbxIsWeapon.UseVisualStyleBackColor = true;
            // 
            // ckbxRemoveOnDestruction
            // 
            this.ckbxRemoveOnDestruction.AutoSize = true;
            this.ckbxRemoveOnDestruction.Checked = true;
            this.ckbxRemoveOnDestruction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxRemoveOnDestruction.ComponentId = 0;
            this.ckbxRemoveOnDestruction.Controller = null;
            this.ckbxRemoveOnDestruction.Location = new System.Drawing.Point(6, 42);
            this.ckbxRemoveOnDestruction.Name = "ckbxRemoveOnDestruction";
            this.ckbxRemoveOnDestruction.ParameterCategory = "Species";
            this.ckbxRemoveOnDestruction.ParameterName = "RemoveOnDestruction";
            this.ckbxRemoveOnDestruction.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.ckbxRemoveOnDestruction.Size = new System.Drawing.Size(136, 17);
            this.ckbxRemoveOnDestruction.TabIndex = 3;
            this.ckbxRemoveOnDestruction.Text = "Remove on destruction";
            this.ckbxRemoveOnDestruction.UseVisualStyleBackColor = true;
            // 
            // nndCollisionRadius
            // 
            this.nndCollisionRadius.ComponentId = 0;
            this.nndCollisionRadius.Controller = null;
            this.nndCollisionRadius.Location = new System.Drawing.Point(182, 39);
            this.nndCollisionRadius.Name = "nndCollisionRadius";
            this.nndCollisionRadius.ParameterCategory = "Species";
            this.nndCollisionRadius.ParameterName = "CollisionRadius";
            this.nndCollisionRadius.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndCollisionRadius.Size = new System.Drawing.Size(119, 20);
            this.nndCollisionRadius.TabIndex = 4;
            this.nndCollisionRadius.Text = "0";
            this.nndCollisionRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nndCollisionRadius.Value = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Collision radius (m.)";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbExistingSpecies);
            this.groupBox1.Controls.Add(this.rbAir);
            this.groupBox1.Controls.Add(this.rbSea);
            this.groupBox1.Controls.Add(this.rbLand);
            this.groupBox1.Controls.Add(this.lkbxType);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 121);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unit Parameters (Required)";
            // 
            // rbExistingSpecies
            // 
            this.rbExistingSpecies.AutoSize = true;
            this.rbExistingSpecies.ComponentId = 0;
            this.rbExistingSpecies.Controller = null;
            this.rbExistingSpecies.Location = new System.Drawing.Point(6, 88);
            this.rbExistingSpecies.Name = "rbExistingSpecies";
            this.rbExistingSpecies.ParameterCategory = "Species";
            this.rbExistingSpecies.ParameterName = "ExistingSpecies";
            this.rbExistingSpecies.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbExistingSpecies.Size = new System.Drawing.Size(136, 17);
            this.rbExistingSpecies.TabIndex = 13;
            this.rbExistingSpecies.Text = "Use Custom Base Type";
            this.rbExistingSpecies.UseVisualStyleBackColor = true;
            // 
            // rbAir
            // 
            this.rbAir.AutoSize = true;
            this.rbAir.ComponentId = 0;
            this.rbAir.Controller = null;
            this.rbAir.Location = new System.Drawing.Point(6, 65);
            this.rbAir.Name = "rbAir";
            this.rbAir.ParameterCategory = "Species";
            this.rbAir.ParameterName = "AirObject";
            this.rbAir.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbAir.Size = new System.Drawing.Size(69, 17);
            this.rbAir.TabIndex = 11;
            this.rbAir.Text = "Air object";
            this.rbAir.UseVisualStyleBackColor = true;
            // 
            // rbSea
            // 
            this.rbSea.AutoSize = true;
            this.rbSea.ComponentId = 0;
            this.rbSea.Controller = null;
            this.rbSea.Location = new System.Drawing.Point(6, 42);
            this.rbSea.Name = "rbSea";
            this.rbSea.ParameterCategory = "Species";
            this.rbSea.ParameterName = "SeaObject";
            this.rbSea.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbSea.Size = new System.Drawing.Size(76, 17);
            this.rbSea.TabIndex = 10;
            this.rbSea.Text = "Sea object";
            this.rbSea.UseVisualStyleBackColor = true;
            // 
            // rbLand
            // 
            this.rbLand.AutoSize = true;
            this.rbLand.Checked = true;
            this.rbLand.ComponentId = 0;
            this.rbLand.Controller = null;
            this.rbLand.Location = new System.Drawing.Point(6, 19);
            this.rbLand.Name = "rbLand";
            this.rbLand.ParameterCategory = "Species";
            this.rbLand.ParameterName = "LandObject";
            this.rbLand.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.rbLand.Size = new System.Drawing.Size(81, 17);
            this.rbLand.TabIndex = 9;
            this.rbLand.TabStop = true;
            this.rbLand.Text = "Land object";
            this.rbLand.UseVisualStyleBackColor = true;
            // 
            // lkbxType
            // 
            this.lkbxType.CheckLinkLevel = ((uint)(1u));
            this.lkbxType.CheckOnClick = true;
            this.lkbxType.ConnectFromId = -1;
            this.lkbxType.ConnectLinkType = null;
            this.lkbxType.ConnectRootId = 0;
            this.lkbxType.Controller = null;
            this.lkbxType.DisplayComponentType = null;
            this.lkbxType.DisplayLinkType = null;
            this.lkbxType.DisplayParameterCategory = "";
            this.lkbxType.DisplayParameterName = "";
            this.lkbxType.DisplayRecursiveCheck = false;
            this.lkbxType.DisplayRootId = 0;
            this.lkbxType.FilterResult = false;
            this.lkbxType.FormattingEnabled = true;
            this.lkbxType.Location = new System.Drawing.Point(181, 82);
            this.lkbxType.Name = "lkbxType";
            this.lkbxType.OneToMany = false;
            this.lkbxType.ParameterFilterCategory = "";
            this.lkbxType.ParameterFilterName = "";
            this.lkbxType.ParameterFilterValue = "";
            this.lkbxType.Size = new System.Drawing.Size(120, 19);
            this.lkbxType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(255, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Icon";
            // 
            // Species
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.customTabControl1);
            this.Name = "Species";
            this.Size = new System.Drawing.Size(867, 917);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.customTabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.simpleLinkGrid1)).EndInit();
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
        private AME.Views.View_Components.CustomLinkBox lkbxType;
        private AME.Views.View_Components.CustomNonnegativeDouble nndCollisionRadius;
        private AME.Views.View_Components.CustomCheckBox ckbxRemoveOnDestruction;
        private AME.Views.View_Components.CustomCheckBox ckbxIsWeapon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomRadioButton rbLand;
        private System.Windows.Forms.GroupBox groupBox1;
        private AME.Views.View_Components.CustomRadioButton rbAir;
        private AME.Views.View_Components.CustomRadioButton rbSea;
        private AME.Views.View_Components.CustomRadioButton rbExistingSpecies;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private AME.Views.View_Components.CustomCheckBox customCheckBoxLaunchedByOwner;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private AME.Views.View_Components.CustomLinkBox customLinkBoxDecisionMakers;
        private SimpleLinkGrid simpleLinkGrid1;
        private ClassificationRule classificationRule1;

    }
}
