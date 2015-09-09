using System.ComponentModel;
namespace Forms
{
    partial class DBForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.componentView = new System.Windows.Forms.DataGridView();
            this.paramView = new System.Windows.Forms.DataGridView();
            this.paramlabel = new System.Windows.Forms.Label();
            this.linkView = new System.Windows.Forms.DataGridView();
            this.linkLabel = new System.Windows.Forms.Label();
            this.componentsLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.componentView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkView)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // componentView
            // 
            this.componentView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.componentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentView.Location = new System.Drawing.Point(0, 0);
            this.componentView.Name = "componentView";
            this.componentView.Size = new System.Drawing.Size(312, 488);
            this.componentView.TabIndex = 4;
            // 
            // paramView
            // 
            this.paramView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.paramView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramView.Location = new System.Drawing.Point(0, 0);
            this.paramView.Name = "paramView";
            this.paramView.Size = new System.Drawing.Size(359, 488);
            this.paramView.TabIndex = 6;
            // 
            // paramlabel
            // 
            this.paramlabel.AutoSize = true;
            this.paramlabel.Location = new System.Drawing.Point(795, 9);
            this.paramlabel.Name = "paramlabel";
            this.paramlabel.Size = new System.Drawing.Size(60, 13);
            this.paramlabel.TabIndex = 7;
            this.paramlabel.Text = "Parameters";
            // 
            // linkView
            // 
            this.linkView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.linkView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkView.Location = new System.Drawing.Point(0, 0);
            this.linkView.Name = "linkView";
            this.linkView.Size = new System.Drawing.Size(262, 488);
            this.linkView.TabIndex = 8;
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(462, 9);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(32, 13);
            this.linkLabel.TabIndex = 10;
            this.linkLabel.Text = "Links";
            // 
            // componentsLabel
            // 
            this.componentsLabel.AutoSize = true;
            this.componentsLabel.Location = new System.Drawing.Point(95, 9);
            this.componentsLabel.Name = "componentsLabel";
            this.componentsLabel.Size = new System.Drawing.Size(66, 13);
            this.componentsLabel.TabIndex = 11;
            this.componentsLabel.Text = "Components";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.componentView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(941, 488);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 12;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.linkView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.paramView);
            this.splitContainer2.Size = new System.Drawing.Size(625, 488);
            this.splitContainer2.SplitterDistance = 262;
            this.splitContainer2.TabIndex = 0;
            // 
            // DBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 540);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.componentsLabel);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.paramlabel);
            this.Name = "DBForm";
            this.Text = "DB";
            ((System.ComponentModel.ISupportInitialize)(this.componentView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView componentView;
        private System.Windows.Forms.DataGridView paramView;
        private System.Windows.Forms.Label paramlabel;
        private System.Windows.Forms.DataGridView linkView;
        private System.Windows.Forms.Label linkLabel;
        private System.Windows.Forms.Label componentsLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}