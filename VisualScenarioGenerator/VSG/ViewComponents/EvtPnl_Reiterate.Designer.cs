namespace VSG.ViewComponents
{
    partial class EvtPnl_Reiterate
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
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Time (s.):";
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(5, 34);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(566, 403);
            this.engramRange1.TabIndex = 2;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(57, 1);
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
            this.timeBox.ParameterCategory = "ReiterateEvent";
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
            // EvtPnl_Reiterate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.MinimumSize = new System.Drawing.Size(296, 300);
            this.Name = "EvtPnl_Reiterate";
            this.Size = new System.Drawing.Size(570, 447);
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private EngramRange engramRange1;

    }
}
