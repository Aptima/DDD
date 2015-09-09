namespace Aptima.Asim.DDD.Client.Dialogs
{
    partial class CustomAttributesDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewAttributes = new System.Windows.Forms.DataGridView();
            this.attribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAttributes
            // 
            this.dataGridViewAttributes.AllowUserToAddRows = false;
            this.dataGridViewAttributes.AllowUserToDeleteRows = false;
            this.dataGridViewAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attribute,
            this.Value});
            this.dataGridViewAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAttributes.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewAttributes.Name = "dataGridViewAttributes";
            this.dataGridViewAttributes.ReadOnly = true;
            this.dataGridViewAttributes.RowHeadersWidth = 40;
            this.dataGridViewAttributes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewAttributes.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewAttributes.Size = new System.Drawing.Size(592, 45);
            this.dataGridViewAttributes.TabIndex = 0;
            // 
            // attribute
            // 
            this.attribute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.attribute.HeaderText = "Attribute";
            this.attribute.MinimumWidth = 200;
            this.attribute.Name = "attribute";
            this.attribute.ReadOnly = true;
            this.attribute.Width = 200;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 350;
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 350;
            // 
            // CustomAttributesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(592, 45);
            this.Controls.Add(this.dataGridViewAttributes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(200, 47);
            this.Name = "CustomAttributesDialog";
            this.Text = "Custom Attributes";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomAttributesDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAttributes;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}