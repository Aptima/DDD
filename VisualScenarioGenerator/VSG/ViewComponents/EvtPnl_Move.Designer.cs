namespace VSG.ViewComponents
{
    partial class EvtPnl_Move
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.zBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.yBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.xBox = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
            this.throttleBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.throttleBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Time (s.):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 179);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Z:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 179);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 179);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "X:";
            // 
            // zBox
            // 
            this.zBox.ComponentId = -1;
            this.zBox.Controller = null;
            this.zBox.Location = new System.Drawing.Point(208, 176);
            this.zBox.Margin = new System.Windows.Forms.Padding(2);
            this.zBox.Name = "zBox";
            this.zBox.ParameterCategory = "DestinationLocation";
            this.zBox.ParameterName = "Z";
            this.zBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.zBox.Size = new System.Drawing.Size(72, 20);
            this.zBox.TabIndex = 19;
            this.zBox.UseDelimiter = false;
            // 
            // yBox
            // 
            this.yBox.ComponentId = -1;
            this.yBox.Controller = null;
            this.yBox.Location = new System.Drawing.Point(114, 176);
            this.yBox.Margin = new System.Windows.Forms.Padding(2);
            this.yBox.Name = "yBox";
            this.yBox.ParameterCategory = "DestinationLocation";
            this.yBox.ParameterName = "Y";
            this.yBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.yBox.Size = new System.Drawing.Size(72, 20);
            this.yBox.TabIndex = 18;
            this.yBox.UseDelimiter = false;
            // 
            // xBox
            // 
            this.xBox.ComponentId = -1;
            this.xBox.Controller = null;
            this.xBox.Location = new System.Drawing.Point(21, 176);
            this.xBox.Margin = new System.Windows.Forms.Padding(2);
            this.xBox.Name = "xBox";
            this.xBox.ParameterCategory = "DestinationLocation";
            this.xBox.ParameterName = "X";
            this.xBox.ParameterType = AME.Controllers.eParamParentType.Component;
            this.xBox.Size = new System.Drawing.Size(72, 20);
            this.xBox.TabIndex = 17;
            this.xBox.UseDelimiter = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 160);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Destination:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 133);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Throttle (%):";
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(3, 3);
            this.eventID1.Margin = new System.Windows.Forms.Padding(2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(379, 101);
            this.eventID1.TabIndex = 14;
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(8, 207);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(566, 403);
            this.engramRange1.TabIndex = 20;
            // 
            // throttleBox
            // 
            this.throttleBox.ComponentId = 0;
            this.throttleBox.Controller = null;
            this.throttleBox.Location = new System.Drawing.Point(70, 131);
            this.throttleBox.Margin = new System.Windows.Forms.Padding(2);
            this.throttleBox.Name = "throttleBox";
            this.throttleBox.ParameterCategory = "MoveEvent";
            this.throttleBox.ParameterName = "Throttle";
            this.throttleBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.throttleBox.Size = new System.Drawing.Size(91, 20);
            this.throttleBox.TabIndex = 16;
            this.throttleBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(70, 108);
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
            this.timeBox.ParameterCategory = "MoveEvent";
            this.timeBox.ParameterName = "Time";
            this.timeBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeBox.Size = new System.Drawing.Size(91, 20);
            this.timeBox.TabIndex = 15;
            this.timeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EvtPnl_Move
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.throttleBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.zBox);
            this.Controls.Add(this.yBox);
            this.Controls.Add(this.xBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.MinimumSize = new System.Drawing.Size(326, 370);
            this.Name = "EvtPnl_Move";
            this.Size = new System.Drawing.Size(586, 626);
            ((System.ComponentModel.ISupportInitialize)(this.throttleBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private AME.Views.View_Components.CustomParameterTextBox zBox;
        private AME.Views.View_Components.CustomParameterTextBox yBox;
        private AME.Views.View_Components.CustomParameterTextBox xBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomNumericUpDown throttleBox;
        private EngramRange engramRange1;
        private EventID eventID1;
    }
}
