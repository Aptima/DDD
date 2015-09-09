namespace BatchRunner
{
    partial class BatchEditorForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.newRunButton = new System.Windows.Forms.Button();
            this.runListView = new System.Windows.Forms.ListView();
            this.runNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.runDurationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scenarioColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deleteButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(407, 272);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(326, 272);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save As";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // newRunButton
            // 
            this.newRunButton.Location = new System.Drawing.Point(3, 3);
            this.newRunButton.Name = "newRunButton";
            this.newRunButton.Size = new System.Drawing.Size(75, 23);
            this.newRunButton.TabIndex = 2;
            this.newRunButton.Text = "New Run";
            this.newRunButton.UseVisualStyleBackColor = true;
            this.newRunButton.Click += new System.EventHandler(this.newRunButton_Click);
            // 
            // runListView
            // 
            this.runListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.runListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.runNameColumn,
            this.runDurationColumn,
            this.scenarioColumn});
            this.runListView.FullRowSelect = true;
            this.runListView.GridLines = true;
            this.runListView.Location = new System.Drawing.Point(3, 32);
            this.runListView.MultiSelect = false;
            this.runListView.Name = "runListView";
            this.runListView.Size = new System.Drawing.Size(479, 234);
            this.runListView.TabIndex = 3;
            this.runListView.UseCompatibleStateImageBehavior = false;
            this.runListView.View = System.Windows.Forms.View.Details;
            this.runListView.DoubleClick += new System.EventHandler(this.runListView_DoubleClick);
            // 
            // runNameColumn
            // 
            this.runNameColumn.Text = "Run Name";
            this.runNameColumn.Width = 107;
            // 
            // runDurationColumn
            // 
            this.runDurationColumn.Text = "Duration (s)";
            this.runDurationColumn.Width = 80;
            // 
            // scenarioColumn
            // 
            this.scenarioColumn.Text = "Scenario";
            this.scenarioColumn.Width = 287;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(246, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(84, 3);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(75, 23);
            this.moveUpButton.TabIndex = 5;
            this.moveUpButton.Text = "Move Up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(165, 3);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(75, 23);
            this.moveDownButton.TabIndex = 6;
            this.moveDownButton.Text = "Move Down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // BatchEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 297);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.runListView);
            this.Controls.Add(this.newRunButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "BatchEditorForm";
            this.Text = "Edit Batch";
            this.Load += new System.EventHandler(this.BatchEditorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button newRunButton;
        private System.Windows.Forms.ListView runListView;
        private System.Windows.Forms.ColumnHeader runNameColumn;
        private System.Windows.Forms.ColumnHeader runDurationColumn;
        private System.Windows.Forms.ColumnHeader scenarioColumn;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
    }
}