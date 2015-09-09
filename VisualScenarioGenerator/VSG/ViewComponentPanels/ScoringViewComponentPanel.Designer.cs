namespace VSG.ViewComponentPanels
{
    partial class ScoringViewComponentPanel
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customTreeView1 = new AME.Views.View_Components.CustomTreeView();
            this.customTabControlBlank = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPageBlank = new AME.Views.View_Components.CustomTabPage(this.components);
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.customTabControlBlank.SuspendLayout();
            this.customTabPageBlank.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.customTabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.customTabControlBlank);
            this.splitContainer1.Size = new System.Drawing.Size(740, 533);
            this.splitContainer1.SplitterDistance = 237;
            this.splitContainer1.TabIndex = 0;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.tabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(237, 533);
            this.customTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.customTreeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(229, 507);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scoring";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // customTreeView1
            // 
            this.customTreeView1.AllowDrop = true;
            this.customTreeView1.AllowUserInput = true;
            this.customTreeView1.Controller = null;
            this.customTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTreeView1.HideSelection = false;
            this.customTreeView1.Level = ((uint)(0u));
            this.customTreeView1.Location = new System.Drawing.Point(3, 3);
            this.customTreeView1.Name = "customTreeView1";
            this.customTreeView1.ShowRoot = true;
            this.customTreeView1.Size = new System.Drawing.Size(223, 501);
            this.customTreeView1.TabIndex = 0;
            this.customTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customTreeView1_AfterSelect);
            // 
            // customTabControlBlank
            // 
            this.customTabControlBlank.Controls.Add(this.customTabPageBlank);
            this.customTabControlBlank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControlBlank.Location = new System.Drawing.Point(0, 0);
            this.customTabControlBlank.Name = "customTabControlBlank";
            this.customTabControlBlank.SelectedIndex = 0;
            this.customTabControlBlank.Size = new System.Drawing.Size(499, 533);
            this.customTabControlBlank.TabIndex = 0;
            // 
            // customTabPageBlank
            // 
            this.customTabPageBlank.Controls.Add(this.webBrowser1);
            this.customTabPageBlank.Description = "label1";
            this.customTabPageBlank.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPageBlank.Location = new System.Drawing.Point(4, 22);
            this.customTabPageBlank.Name = "customTabPageBlank";
            this.customTabPageBlank.Size = new System.Drawing.Size(491, 507);
            this.customTabPageBlank.TabIndex = 0;
            this.customTabPageBlank.Text = "Scoring Parameters";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 28);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(491, 479);
            this.webBrowser1.TabIndex = 1;
            // 
            // ScoringViewComponentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScoringViewComponentPanel";
            this.Size = new System.Drawing.Size(740, 533);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.customTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.customTabControlBlank.ResumeLayout(false);
            this.customTabPageBlank.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private AME.Views.View_Components.CustomTreeView customTreeView1;
        private AME.Views.View_Components.CustomTabControl customTabControlBlank;
        private AME.Views.View_Components.CustomTabPage customTabPageBlank;
        private System.Windows.Forms.WebBrowser webBrowser1;

    }
}
