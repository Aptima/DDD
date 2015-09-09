namespace VSG.ViewComponents
{
    partial class EvtPnl_EngramRemove
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.engramNameBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 119);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Time (s.):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Engram Name:";
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
            this.engramNameBox.Location = new System.Drawing.Point(11, 24);
            this.engramNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.engramNameBox.Name = "engramNameBox";
            this.engramNameBox.OneToMany = false;
            this.engramNameBox.ParameterFilterCategory = "";
            this.engramNameBox.ParameterFilterName = "";
            this.engramNameBox.ParameterFilterValue = "";
            this.engramNameBox.Size = new System.Drawing.Size(186, 64);
            this.engramNameBox.TabIndex = 21;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(106, 117);
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
            this.timeUpDown.ParameterCategory = "RemoveEngram";
            this.timeUpDown.ParameterName = "Time";
            this.timeUpDown.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeUpDown.Size = new System.Drawing.Size(91, 20);
            this.timeUpDown.TabIndex = 20;
            this.timeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EvtPnl_EngramRemove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.engramNameBox);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "EvtPnl_EngramRemove";
            this.Size = new System.Drawing.Size(232, 164);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox engramNameBox;

    }
}
