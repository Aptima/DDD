namespace AME.Views.View_Components {
    partial class ParameterTablePanel {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label = new System.Windows.Forms.Label();
            this.customPropertyGrid1 = new AME.Views.View_Components.CustomPropertyGrid();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label.Dock = System.Windows.Forms.DockStyle.Top;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(1041, 21);
            this.label.TabIndex = 0;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label.Visible = false;
            // 
            // customPropertyGrid1
            // 
            this.customPropertyGrid1.Controller = null;
            this.customPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPropertyGrid1.Location = new System.Drawing.Point(0, 21);
            this.customPropertyGrid1.Name = "customPropertyGrid1";
            this.customPropertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.customPropertyGrid1.SelectedID = 0;
            this.customPropertyGrid1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customPropertyGrid1.Size = new System.Drawing.Size(1041, 693);
            this.customPropertyGrid1.TabIndex = 1;
            // 
            // ParameterTablePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.customPropertyGrid1);
            this.Controls.Add(this.label);
            this.Name = "ParameterTablePanel";
            this.Size = new System.Drawing.Size(1041, 714);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label;
        private CustomPropertyGrid customPropertyGrid1;
    }
}
