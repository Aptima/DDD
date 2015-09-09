namespace VSG.ViewComponentPanels
{
    partial class ObjectTypesViewComponentPanel
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
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.customTreeView1 = new AME.Views.View_Components.CustomTreeView();
            this.customTabPage3 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.customTreeView2 = new AME.Views.View_Components.CustomTreeView();
            this.customTabControlBlank = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage2 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.customTabPage3.SuspendLayout();
            this.customTabControlBlank.SuspendLayout();
            this.customTabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.customTabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customTabControlBlank);
            this.splitContainer1.Size = new System.Drawing.Size(1354, 897);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Controls.Add(this.customTabPage3);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(300, 897);
            this.customTabControl1.TabIndex = 1;
            this.customTabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.customTabControl1_Selected);
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.customTreeView1);
            this.customTabPage1.Description = "Create and define decision maker objects";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(292, 871);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Players";
            // 
            // customTreeView1
            // 
            this.customTreeView1.AllowDrop = true;
            this.customTreeView1.AllowUserInput = true;
            this.customTreeView1.Controller = null;
            this.customTreeView1.DecorateNodes = false;
            this.customTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTreeView1.HideSelection = false;
            this.customTreeView1.Level = ((uint)(0u));
            this.customTreeView1.Location = new System.Drawing.Point(0, 28);
            this.customTreeView1.Name = "customTreeView1";
            this.customTreeView1.ShowRoot = true;
            this.customTreeView1.Size = new System.Drawing.Size(292, 843);
            this.customTreeView1.TabIndex = 2;
            this.customTreeView1.UseNodeMap = false;
            this.customTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customTreeView1_AfterSelect);
            this.customTreeView1.ItemAdd += new AME.Views.View_Components.CustomTreeView.ItemAdded(this.customTreeView1_ItemAdd);
            // 
            // customTabPage3
            // 
            this.customTabPage3.Controls.Add(this.customTreeView2);
            this.customTabPage3.Description = "Create and define scenario resources";
            this.customTabPage3.Location = new System.Drawing.Point(4, 22);
            this.customTabPage3.Name = "customTabPage3";
            this.customTabPage3.Size = new System.Drawing.Size(292, 871);
            this.customTabPage3.TabIndex = 1;
            this.customTabPage3.Text = "Resources";
            // 
            // customTreeView2
            // 
            this.customTreeView2.AllowDrop = true;
            this.customTreeView2.AllowUserInput = true;
            this.customTreeView2.Controller = null;
            this.customTreeView2.DecorateNodes = false;
            this.customTreeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTreeView2.HideSelection = false;
            this.customTreeView2.Level = ((uint)(0u));
            this.customTreeView2.Location = new System.Drawing.Point(0, 28);
            this.customTreeView2.Name = "customTreeView2";
            this.customTreeView2.ShowRoot = true;
            this.customTreeView2.Size = new System.Drawing.Size(292, 843);
            this.customTreeView2.TabIndex = 1;
            this.customTreeView2.UseNodeMap = false;
            this.customTreeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customTreeView2_AfterSelect);
            this.customTreeView2.ItemAdd += new AME.Views.View_Components.CustomTreeView.ItemAdded(this.customTreeView2_ItemAdd);
            // 
            // customTabControlBlank
            // 
            this.customTabControlBlank.Controls.Add(this.customTabPage2);
            this.customTabControlBlank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControlBlank.Location = new System.Drawing.Point(0, 0);
            this.customTabControlBlank.Name = "customTabControlBlank";
            this.customTabControlBlank.SelectedIndex = 0;
            this.customTabControlBlank.Size = new System.Drawing.Size(1050, 897);
            this.customTabControlBlank.TabIndex = 0;
            // 
            // customTabPage2
            // 
            this.customTabPage2.AutoScroll = true;
            this.customTabPage2.Controls.Add(this.webBrowser1);
            this.customTabPage2.Description = "";
            this.customTabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage2.Location = new System.Drawing.Point(4, 22);
            this.customTabPage2.Name = "customTabPage2";
            this.customTabPage2.Size = new System.Drawing.Size(1042, 871);
            this.customTabPage2.TabIndex = 0;
            this.customTabPage2.Text = "Object Definitions";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 28);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1042, 843);
            this.webBrowser1.TabIndex = 1;
            // 
            // ObjectTypesViewComponentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ObjectTypesViewComponentPanel";
            this.Size = new System.Drawing.Size(1354, 897);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.customTabPage3.ResumeLayout(false);
            this.customTabControlBlank.ResumeLayout(false);
            this.customTabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private AME.Views.View_Components.CustomTreeView customTreeView1;
        private AME.Views.View_Components.CustomTabControl customTabControlBlank;
        private AME.Views.View_Components.CustomTabPage customTabPage2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private AME.Views.View_Components.CustomTabPage customTabPage3;
        private AME.Views.View_Components.CustomTreeView customTreeView2;
    }
}
