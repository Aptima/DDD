namespace VSG.ViewComponents
{
    partial class EvtPnl_SpeciesCompletion
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
            this.actionRadio = new System.Windows.Forms.RadioButton();
            this.stateRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.actionCombo = new System.Windows.Forms.ComboBox();
            this.speciesLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionRadio
            // 
            this.actionRadio.AutoSize = true;
            this.actionRadio.Checked = true;
            this.actionRadio.Location = new System.Drawing.Point(4, 17);
            this.actionRadio.Margin = new System.Windows.Forms.Padding(2);
            this.actionRadio.Name = "actionRadio";
            this.actionRadio.Size = new System.Drawing.Size(55, 17);
            this.actionRadio.TabIndex = 6;
            this.actionRadio.TabStop = true;
            this.actionRadio.Text = "Action";
            this.actionRadio.UseVisualStyleBackColor = true;
            this.actionRadio.CheckedChanged += new System.EventHandler(this.actionRadio_CheckedChanged);
            // 
            // stateRadio
            // 
            this.stateRadio.AutoSize = true;
            this.stateRadio.Location = new System.Drawing.Point(251, 17);
            this.stateRadio.Margin = new System.Windows.Forms.Padding(2);
            this.stateRadio.Name = "stateRadio";
            this.stateRadio.Size = new System.Drawing.Size(50, 17);
            this.stateRadio.TabIndex = 7;
            this.stateRadio.Text = "State";
            this.stateRadio.UseVisualStyleBackColor = true;
            this.stateRadio.CheckedChanged += new System.EventHandler(this.stateRadio_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stateComboBox);
            this.groupBox1.Controls.Add(this.actionCombo);
            this.groupBox1.Controls.Add(this.actionRadio);
            this.groupBox1.Controls.Add(this.stateRadio);
            this.groupBox1.Location = new System.Drawing.Point(8, 164);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(483, 87);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Condition";
            // 
            // stateComboBox
            // 
            this.stateComboBox.ComponentId = -1;
            this.stateComboBox.Controller = null;
            this.stateComboBox.FormattingEnabled = true;
            this.stateComboBox.Location = new System.Drawing.Point(251, 49);
            this.stateComboBox.Name = "stateComboBox";
            this.stateComboBox.ParameterCategory = "State";
            this.stateComboBox.ParameterName = "FuelDepletionState";
            this.stateComboBox.ShowAllStates = false;
            this.stateComboBox.Size = new System.Drawing.Size(227, 21);
            this.stateComboBox.SpeciesId = -1;
            this.stateComboBox.TabIndex = 44;
            // 
            // actionCombo
            // 
            this.actionCombo.FormattingEnabled = true;
            this.actionCombo.Items.AddRange(new object[] {
            "MoveComplete_Event",
            "StateChangeNotice",
            "AttackObjectRequestType",
            "WeaponLaunchRequestType",
            "SubplatformLaunchRequestType",
            "SubplatformDockRequestType",
            "MoveObjectRequestType",
            "TransferObjectRequest",
            "SelfDefenseAttackNotice",
            "TransferObject"});
            this.actionCombo.Location = new System.Drawing.Point(5, 49);
            this.actionCombo.Margin = new System.Windows.Forms.Padding(2);
            this.actionCombo.Name = "actionCombo";
            this.actionCombo.Size = new System.Drawing.Size(241, 21);
            this.actionCombo.TabIndex = 9;
            this.actionCombo.SelectedIndexChanged += new System.EventHandler(this.actionCombo_SelectedIndexChanged);
            // 
            // speciesLinkBox
            // 
            this.speciesLinkBox.CheckLinkLevel = ((uint)(1u));
            this.speciesLinkBox.CheckOnClick = true;
            this.speciesLinkBox.ConnectFromId = -1;
            this.speciesLinkBox.ConnectLinkType = null;
            this.speciesLinkBox.ConnectRootId = -1;
            this.speciesLinkBox.Controller = null;
            this.speciesLinkBox.DisplayComponentType = null;
            this.speciesLinkBox.DisplayLinkType = null;
            this.speciesLinkBox.DisplayParameterCategory = "";
            this.speciesLinkBox.DisplayParameterName = "";
            this.speciesLinkBox.DisplayRootId = -1;
            this.speciesLinkBox.FilterResult = false;
            this.speciesLinkBox.FormattingEnabled = true;
            this.speciesLinkBox.Location = new System.Drawing.Point(8, 24);
            this.speciesLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.speciesLinkBox.Name = "speciesLinkBox";
            this.speciesLinkBox.OneToMany = false;
            this.speciesLinkBox.ParameterFilterCategory = "";
            this.speciesLinkBox.ParameterFilterName = "";
            this.speciesLinkBox.ParameterFilterValue = "";
            this.speciesLinkBox.Size = new System.Drawing.Size(177, 124);
            this.speciesLinkBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Species:";
            // 
            // EvtPnl_SpeciesCompletion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.speciesLinkBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "EvtPnl_SpeciesCompletion";
            this.Size = new System.Drawing.Size(493, 262);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton actionRadio;
        private System.Windows.Forms.RadioButton stateRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox actionCombo;
        private AME.Views.View_Components.CustomLinkBox speciesLinkBox;
        private System.Windows.Forms.Label label1;
        private StateComboBox stateComboBox;

    }
}
