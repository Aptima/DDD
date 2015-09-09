using ChartDirector;
namespace AME.Views.View_Components.Chart {
	partial class PercentDistributionPieChartPanel {
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
			this.viewer = new ChartDirector.WinChartViewer();
			((System.ComponentModel.ISupportInitialize)(this.viewer)).BeginInit();
			this.SuspendLayout();
			// 
			// viewer
			// 
			this.viewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewer.Location = new System.Drawing.Point(0, 0);
			this.viewer.Name = "viewer";
			this.viewer.Size = new System.Drawing.Size(330, 275);
			this.viewer.TabIndex = 0;
			this.viewer.TabStop = false;
			// 
			// PercentDistributionPieChartPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.viewer);
			this.Name = "PercentDistributionPieChartPanel";
			this.Size = new System.Drawing.Size(330, 275);
			((System.ComponentModel.ISupportInitialize)(this.viewer)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private WinChartViewer viewer;
	}
}
