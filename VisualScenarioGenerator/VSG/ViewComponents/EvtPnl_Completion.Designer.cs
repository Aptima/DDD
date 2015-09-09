namespace VSG.ViewComponents
{
    partial class EvtPnl_Completion
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
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
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
            this.actionRadio.TabIndex = 2;
            this.actionRadio.TabStop = true;
            this.actionRadio.Text = "Action";
            this.actionRadio.UseVisualStyleBackColor = true;
            this.actionRadio.CheckedChanged += new System.EventHandler(this.actionRadio_CheckedChanged);
            // 
            // stateRadio
            // 
            this.stateRadio.AutoSize = true;
            this.stateRadio.Location = new System.Drawing.Point(207, 17);
            this.stateRadio.Margin = new System.Windows.Forms.Padding(2);
            this.stateRadio.Name = "stateRadio";
            this.stateRadio.Size = new System.Drawing.Size(50, 17);
            this.stateRadio.TabIndex = 3;
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
            this.groupBox1.Location = new System.Drawing.Point(3, 108);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(387, 94);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Condition";
            // 
            // stateComboBox
            // 
            this.stateComboBox.ComponentId = -1;
            this.stateComboBox.Controller = null;
            this.stateComboBox.FormattingEnabled = true;
            this.stateComboBox.Location = new System.Drawing.Point(207, 49);
            this.stateComboBox.Name = "stateComboBox";
            this.stateComboBox.ParameterCategory = "State";
            this.stateComboBox.ParameterName = "FuelDepletionState";
            this.stateComboBox.ShowAllStates = false;
            this.stateComboBox.Size = new System.Drawing.Size(178, 21);
            this.stateComboBox.SpeciesId = -1;
            this.stateComboBox.TabIndex = 5;
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
            this.actionCombo.Size = new System.Drawing.Size(198, 21);
            this.actionCombo.TabIndex = 4;
            this.actionCombo.SelectedIndexChanged += new System.EventHandler(this.actionCombo_SelectedIndexChanged);
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(3, 3);
            this.eventID1.Margin = new System.Windows.Forms.Padding(2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(383, 101);
            this.eventID1.TabIndex = 1;
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(3, 206);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(566, 403);
            this.engramRange1.TabIndex = 6;
            // 
            // EvtPnl_Completion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(332, 440);
            this.Name = "EvtPnl_Completion";
            this.Size = new System.Drawing.Size(572, 620);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton actionRadio;
        private System.Windows.Forms.RadioButton stateRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox actionCombo;
        private EngramRange engramRange1;
        private EventID eventID1;
        private StateComboBox stateComboBox;

    }
}
