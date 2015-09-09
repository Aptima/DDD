namespace VSG.ViewComponents
{
    partial class EvtPnl_EngramChange
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
            this.engramValue = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.engramNameBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customCheckBoxSpecificUnit = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.engramUnitID1 = new VSG.ViewComponents.EngramUnitID();
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // engramValue
            // 
            this.engramValue.ComponentId = 0;
            this.engramValue.Controller = null;
            this.engramValue.Location = new System.Drawing.Point(5, 271);
            this.engramValue.Margin = new System.Windows.Forms.Padding(2);
            this.engramValue.Name = "engramValue";
            this.engramValue.ParameterCategory = "ChangeEngram";
            this.engramValue.ParameterName = "Value";
            this.engramValue.ParameterType = AME.Controllers.eParamParentType.Component;
            this.engramValue.Size = new System.Drawing.Size(186, 20);
            this.engramValue.TabIndex = 12;
            this.engramValue.UseDelimiter = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 256);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Engram Value:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Engram Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 219);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Time (s.):";
            // 
            // engramNameBox
            // 
            this.engramNameBox.CheckLinkLevel = ((uint)(1u));
            this.engramNameBox.CheckOnClick = true;
            this.engramNameBox.ConnectFromId = -1;
            this.engramNameBox.ConnectLinkType = null;
            this.engramNameBox.ConnectRootId = -1;
            this.engramNameBox.Controller = null;
            this.engramNameBox.DisplayComponentType = null;
            this.engramNameBox.DisplayLinkType = null;
            this.engramNameBox.DisplayParameterCategory = "";
            this.engramNameBox.DisplayParameterName = "";
            this.engramNameBox.DisplayRootId = -1;
            this.engramNameBox.FilterResult = false;
            this.engramNameBox.FormattingEnabled = true;
            this.engramNameBox.Location = new System.Drawing.Point(4, 26);
            this.engramNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.engramNameBox.Name = "engramNameBox";
            this.engramNameBox.OneToMany = false;
            this.engramNameBox.ParameterFilterCategory = "";
            this.engramNameBox.ParameterFilterName = "";
            this.engramNameBox.ParameterFilterValue = "";
            this.engramNameBox.Size = new System.Drawing.Size(186, 64);
            this.engramNameBox.TabIndex = 15;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(5, 234);
            this.timeUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.timeUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeUpDown.Name = "timeUpDown";
            this.timeUpDown.ParameterCategory = "ChangeEngram";
            this.timeUpDown.ParameterName = "Time";
            this.timeUpDown.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeUpDown.Size = new System.Drawing.Size(186, 20);
            this.timeUpDown.TabIndex = 14;
            this.timeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // customCheckBoxSpecificUnit
            // 
            this.customCheckBoxSpecificUnit.AutoSize = true;
            this.customCheckBoxSpecificUnit.ComponentId = -1;
            this.customCheckBoxSpecificUnit.Controller = null;
            this.customCheckBoxSpecificUnit.Location = new System.Drawing.Point(5, 96);
            this.customCheckBoxSpecificUnit.Name = "customCheckBoxSpecificUnit";
            this.customCheckBoxSpecificUnit.ParameterCategory = "ChangeEngram";
            this.customCheckBoxSpecificUnit.ParameterName = "Unit Specified";
            this.customCheckBoxSpecificUnit.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customCheckBoxSpecificUnit.Size = new System.Drawing.Size(180, 17);
            this.customCheckBoxSpecificUnit.TabIndex = 16;
            this.customCheckBoxSpecificUnit.Text = "Change Engram for Specific Unit";
            this.customCheckBoxSpecificUnit.UseVisualStyleBackColor = true;
            // 
            // engramUnitID1
            // 
            this.engramUnitID1.Controller = null;
            this.engramUnitID1.DisplayID = -1;
            this.engramUnitID1.Location = new System.Drawing.Point(24, 118);
            this.engramUnitID1.Margin = new System.Windows.Forms.Padding(2);
            this.engramUnitID1.Name = "engramUnitID1";
            this.engramUnitID1.ParentID = -1;
            this.engramUnitID1.Size = new System.Drawing.Size(375, 99);
            this.engramUnitID1.TabIndex = 17;
            // 
            // EvtPnl_EngramChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.engramUnitID1);
            this.Controls.Add(this.customCheckBoxSpecificUnit);
            this.Controls.Add(this.engramNameBox);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.engramValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EvtPnl_EngramChange";
            this.Size = new System.Drawing.Size(419, 402);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomParameterTextBox engramValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label3;
        private AME.Views.View_Components.CustomLinkBox engramNameBox;
        private AME.Views.View_Components.CustomCheckBox customCheckBoxSpecificUnit;
        private EngramUnitID engramUnitID1;

    }
}
