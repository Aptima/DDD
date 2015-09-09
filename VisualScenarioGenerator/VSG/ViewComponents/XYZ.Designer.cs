namespace VSG.ViewComponents
{
    partial class XYZ
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nndX = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.nndY = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.nndZ = new AME.Views.View_Components.CustomNonnegativeDouble(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "X";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nndX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nndY);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nndZ);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 55);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // nndX
            // 
            this.nndX.ComponentId = 0;
            this.nndX.Controller = null;
            this.nndX.Location = new System.Drawing.Point(6, 26);
            this.nndX.Name = "nndX";
            this.nndX.ParameterCategory = null;
            this.nndX.ParameterName = null;
            this.nndX.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndX.Size = new System.Drawing.Size(54, 20);
            this.nndX.TabIndex = 0;
            this.nndX.Text = "0";
            // 
            // nndY
            // 
            this.nndY.ComponentId = 0;
            this.nndY.Controller = null;
            this.nndY.Location = new System.Drawing.Point(87, 26);
            this.nndY.Name = "nndY";
            this.nndY.ParameterCategory = null;
            this.nndY.ParameterName = null;
            this.nndY.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndY.Size = new System.Drawing.Size(54, 20);
            this.nndY.TabIndex = 1;
            this.nndY.Text = "0";
            // 
            // nndZ
            // 
            this.nndZ.ComponentId = 0;
            this.nndZ.Controller = null;
            this.nndZ.Location = new System.Drawing.Point(169, 26);
            this.nndZ.Name = "nndZ";
            this.nndZ.ParameterCategory = null;
            this.nndZ.ParameterName = null;
            this.nndZ.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.nndZ.Size = new System.Drawing.Size(54, 20);
            this.nndZ.TabIndex = 2;
            this.nndZ.Text = "0";
            // 
            // XYZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "XYZ";
            this.Size = new System.Drawing.Size(244, 56);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomNonnegativeDouble nndX;
        private AME.Views.View_Components.CustomNonnegativeDouble nndY;
        private AME.Views.View_Components.CustomNonnegativeDouble nndZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
