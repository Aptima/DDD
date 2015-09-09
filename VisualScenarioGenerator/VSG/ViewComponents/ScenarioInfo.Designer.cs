namespace VSG.ViewComponents
{
    partial class ScenarioInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.scenario_name = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.scenario_description = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.customCheckBoxAssetTransfer = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.scenario_time_to_attack = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customParameterEnumBoxRangeRingDisplay = new AME.Views.View_Components.CustomParameterEnumBox(this.components);
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenario_time_to_attack)).BeginInit();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(589, 497);
            this.customTabControl1.TabIndex = 7;
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "Enter scenario information";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(581, 471);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Scenario";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.customParameterEnumBoxRangeRingDisplay);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.customCheckBoxAssetTransfer);
            this.panel1.Controls.Add(this.scenario_time_to_attack);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.scenario_name);
            this.panel1.Controls.Add(this.scenario_description);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(581, 443);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scenario Name";
            // 
            // scenario_name
            // 
            this.scenario_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenario_name.ComponentId = 0;
            this.scenario_name.Controller = null;
            this.scenario_name.Location = new System.Drawing.Point(3, 25);
            this.scenario_name.Name = "scenario_name";
            this.scenario_name.ParameterCategory = "Scenario";
            this.scenario_name.ParameterName = "Scenario Name";
            this.scenario_name.ParameterType = AME.Controllers.eParamParentType.Component;
            this.scenario_name.Size = new System.Drawing.Size(575, 20);
            this.scenario_name.TabIndex = 0;
            this.scenario_name.UseDelimiter = false;
            // 
            // scenario_description
            // 
            this.scenario_description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenario_description.ComponentId = 0;
            this.scenario_description.Controller = null;
            this.scenario_description.Location = new System.Drawing.Point(3, 70);
            this.scenario_description.Multiline = true;
            this.scenario_description.Name = "scenario_description";
            this.scenario_description.ParameterCategory = "Scenario";
            this.scenario_description.ParameterName = "Description";
            this.scenario_description.ParameterType = AME.Controllers.eParamParentType.Component;
            this.scenario_description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.scenario_description.Size = new System.Drawing.Size(575, 207);
            this.scenario_description.TabIndex = 1;
            this.scenario_description.UseDelimiter = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Scenario Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 283);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Default Engagement Time (seconds)";
            this.toolTip1.SetToolTip(this.label3, "The Time To Attack is the period of time that an asset\'s capability requires to c" +
                    "omplete, in seconds.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Range Ring Display Level";
            // 
            // customCheckBoxAssetTransfer
            // 
            this.customCheckBoxAssetTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customCheckBoxAssetTransfer.AutoSize = true;
            this.customCheckBoxAssetTransfer.ComponentId = 0;
            this.customCheckBoxAssetTransfer.Controller = null;
            this.customCheckBoxAssetTransfer.Location = new System.Drawing.Point(3, 328);
            this.customCheckBoxAssetTransfer.Name = "customCheckBoxAssetTransfer";
            this.customCheckBoxAssetTransfer.ParameterCategory = "Scenario";
            this.customCheckBoxAssetTransfer.ParameterName = "Allow Asset Transfers";
            this.customCheckBoxAssetTransfer.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBoxAssetTransfer.Size = new System.Drawing.Size(304, 17);
            this.customCheckBoxAssetTransfer.TabIndex = 3;
            this.customCheckBoxAssetTransfer.Text = "Allow Decision Makers to be able to Transfer Their Assets?";
            this.customCheckBoxAssetTransfer.UseVisualStyleBackColor = true;
            // 
            // scenario_time_to_attack
            // 
            this.scenario_time_to_attack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenario_time_to_attack.ComponentId = 0;
            this.scenario_time_to_attack.Controller = null;
            this.scenario_time_to_attack.Location = new System.Drawing.Point(3, 302);
            this.scenario_time_to_attack.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scenario_time_to_attack.Name = "scenario_time_to_attack";
            this.scenario_time_to_attack.ParameterCategory = "Scenario";
            this.scenario_time_to_attack.ParameterName = "Time To Attack";
            this.scenario_time_to_attack.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.scenario_time_to_attack.Size = new System.Drawing.Size(575, 20);
            this.scenario_time_to_attack.TabIndex = 2;
            this.scenario_time_to_attack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.scenario_time_to_attack.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // customParameterEnumBoxRangeRingDisplay
            // 
            this.customParameterEnumBoxRangeRingDisplay.ComponentId = -1;
            this.customParameterEnumBoxRangeRingDisplay.Controller = null;
            this.customParameterEnumBoxRangeRingDisplay.EnumName = null;
            this.customParameterEnumBoxRangeRingDisplay.FormattingEnabled = true;
            this.customParameterEnumBoxRangeRingDisplay.Location = new System.Drawing.Point(9, 369);
            this.customParameterEnumBoxRangeRingDisplay.Name = "customParameterEnumBoxRangeRingDisplay";
            this.customParameterEnumBoxRangeRingDisplay.ParameterCategory = "Scenario";
            this.customParameterEnumBoxRangeRingDisplay.ParameterName = "Range Ring Display Type";
            this.customParameterEnumBoxRangeRingDisplay.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customParameterEnumBoxRangeRingDisplay.Size = new System.Drawing.Size(127, 69);
            this.customParameterEnumBoxRangeRingDisplay.TabIndex = 5;
            // 
            // ScenarioInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "ScenarioInfo";
            this.Size = new System.Drawing.Size(589, 497);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenario_time_to_attack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomParameterTextBox scenario_name;
        private AME.Views.View_Components.CustomParameterTextBox scenario_description;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private AME.Views.View_Components.CustomNumericUpDown scenario_time_to_attack;
        private AME.Views.View_Components.CustomCheckBox customCheckBoxAssetTransfer;
        private System.Windows.Forms.Label label4;
        private AME.Views.View_Components.CustomParameterEnumBox customParameterEnumBoxRangeRingDisplay;

    }
}
