namespace AME.Views.View_Components
{
    partial class GanttChart
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
            this.label1 = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.legend = new System.Windows.Forms.ListView();
            this.legendLabel = new System.Windows.Forms.Label();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.scaleTextBox = new System.Windows.Forms.TextBox();
            this.paletteButton = new System.Windows.Forms.Button();
            this.filterButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.showMilestones = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Blue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(758, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mission Schedule";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(129, 446);
            this.hScrollBar1.Maximum = 1000000000;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(616, 19);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(745, 32);
            this.vScrollBar1.Maximum = 1000000000;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 414);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.showMilestones);
            this.panel1.Controls.Add(this.legend);
            this.panel1.Controls.Add(this.legendLabel);
            this.panel1.Controls.Add(this.scaleLabel);
            this.panel1.Controls.Add(this.scaleTextBox);
            this.panel1.Controls.Add(this.paletteButton);
            this.panel1.Controls.Add(this.filterButton);
            this.panel1.Location = new System.Drawing.Point(3, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 410);
            this.panel1.TabIndex = 3;
            // 
            // legend
            // 
            this.legend.Location = new System.Drawing.Point(3, 113);
            this.legend.Name = "legend";
            this.legend.Size = new System.Drawing.Size(110, 250);
            this.legend.TabIndex = 5;
            this.legend.UseCompatibleStateImageBehavior = false;
            this.legend.View = System.Windows.Forms.View.SmallIcon;
            // 
            // legendLabel
            // 
            this.legendLabel.AutoSize = true;
            this.legendLabel.Location = new System.Drawing.Point(0, 97);
            this.legendLabel.Name = "legendLabel";
            this.legendLabel.Size = new System.Drawing.Size(43, 13);
            this.legendLabel.TabIndex = 4;
            this.legendLabel.Text = "Legend";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(3, 58);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(79, 13);
            this.scaleLabel.TabIndex = 3;
            this.scaleLabel.Text = "Scale (minutes)";
            // 
            // scaleTextBox
            // 
            this.scaleTextBox.Location = new System.Drawing.Point(3, 74);
            this.scaleTextBox.Name = "scaleTextBox";
            this.scaleTextBox.Size = new System.Drawing.Size(83, 20);
            this.scaleTextBox.TabIndex = 2;
            this.scaleTextBox.Text = "30";
            // 
            // paletteButton
            // 
            this.paletteButton.Location = new System.Drawing.Point(3, 32);
            this.paletteButton.Name = "paletteButton";
            this.paletteButton.Size = new System.Drawing.Size(83, 23);
            this.paletteButton.TabIndex = 1;
            this.paletteButton.Text = "View Setup";
            this.paletteButton.UseVisualStyleBackColor = true;
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(3, 3);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(83, 23);
            this.filterButton.TabIndex = 0;
            this.filterButton.Text = "Data Setup";
            this.filterButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.winChartViewer1);
            this.panel2.Location = new System.Drawing.Point(129, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(613, 411);
            this.panel2.TabIndex = 4;
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winChartViewer1.Location = new System.Drawing.Point(0, 0);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(611, 409);
            this.winChartViewer1.TabIndex = 5;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged_1);
            this.winChartViewer1.Resize += new System.EventHandler(this.winChartViewer1_Resize);
            // 
            // showMilestones
            // 
            this.showMilestones.AutoSize = true;
            this.showMilestones.Checked = true;
            this.showMilestones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMilestones.Location = new System.Drawing.Point(3, 369);
            this.showMilestones.Name = "showMilestones";
            this.showMilestones.Size = new System.Drawing.Size(112, 17);
            this.showMilestones.TabIndex = 6;
            this.showMilestones.Text = "Show Milestones?";
            this.showMilestones.UseVisualStyleBackColor = true;
            // 
            // GanttChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.label1);
            this.Name = "GanttChart";
            this.Size = new System.Drawing.Size(764, 465);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Panel panel2;
        private ChartDirector.WinChartViewer winChartViewer1;
        private System.Windows.Forms.Button paletteButton;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.TextBox scaleTextBox;
        private System.Windows.Forms.ListView legend;
        private System.Windows.Forms.Label legendLabel;
        private System.Windows.Forms.CheckBox showMilestones;
    }
}
