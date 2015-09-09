namespace VSG.ViewComponents
{
    partial class EvtPnl_Flush
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
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.timeUpDown = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Flush Time (s.):";
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
            this.eventID1.TabIndex = 6;
            // 
            // timeUpDown
            // 
            this.timeUpDown.ComponentId = 0;
            this.timeUpDown.Controller = null;
            this.timeUpDown.Location = new System.Drawing.Point(68, 105);
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
            this.timeUpDown.ParameterCategory = "Flush";
            this.timeUpDown.ParameterName = "Time";
            this.timeUpDown.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeUpDown.Size = new System.Drawing.Size(91, 20);
            this.timeUpDown.TabIndex = 5;
            this.timeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EvtPnl_Flush
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.timeUpDown);
            this.Controls.Add(this.label2);
            this.MinimumSize = new System.Drawing.Size(284, 136);
            this.Name = "EvtPnl_Flush";
            this.Size = new System.Drawing.Size(388, 136);
            ((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AME.Views.View_Components.CustomNumericUpDown timeUpDown;
        private System.Windows.Forms.Label label2;
        private EventID eventID1;

    }
}
