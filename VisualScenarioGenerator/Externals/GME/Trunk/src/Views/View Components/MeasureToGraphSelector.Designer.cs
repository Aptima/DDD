namespace AME.Views.View_Components {
	partial class MeasureToGraphSelector {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.measureListBox = new System.Windows.Forms.CheckedListBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// measureListBox
			// 
			this.measureListBox.CheckOnClick = true;
			this.measureListBox.FormattingEnabled = true;
			this.measureListBox.Location = new System.Drawing.Point(1, 2);
			this.measureListBox.Name = "measureListBox";
			this.measureListBox.Size = new System.Drawing.Size(248, 184);
			this.measureListBox.TabIndex = 0;
			this.measureListBox.SelectedIndexChanged += new System.EventHandler(this.measureListBox_SelectedIndexChanged);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(174, 192);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(93, 192);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// MeasureToGraphSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(250, 220);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.measureListBox);
			this.MaximumSize = new System.Drawing.Size(258, 247);
			this.MinimumSize = new System.Drawing.Size(258, 247);
			this.Name = "MeasureToGraphSelector";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Measure to Graph";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox measureListBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;

	}
}
