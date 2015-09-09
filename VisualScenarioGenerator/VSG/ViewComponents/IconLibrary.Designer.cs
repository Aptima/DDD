namespace VSG.ViewComponents
{
    partial class IconLibrary
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
            this.OpenImageDlg = new System.Windows.Forms.OpenFileDialog();
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonNewIconLib = new System.Windows.Forms.Button();
            this.icon_lib = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.buttonOpenIconLib = new System.Windows.Forms.Button();
            this.iconListView1 = new VSG.ViewComponents.IconListView();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenImageDlg
            // 
            this.OpenImageDlg.FileName = "OpenImageDlg";
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(560, 251);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "Open or create icon library to use in playfield.";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(552, 225);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Icon Library";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.iconListView1);
            this.panel1.Controls.Add(this.buttonNewIconLib);
            this.panel1.Controls.Add(this.icon_lib);
            this.panel1.Controls.Add(this.buttonOpenIconLib);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 197);
            this.panel1.TabIndex = 1;
            // 
            // buttonNewIconLib
            // 
            this.buttonNewIconLib.Location = new System.Drawing.Point(78, 3);
            this.buttonNewIconLib.Name = "buttonNewIconLib";
            this.buttonNewIconLib.Size = new System.Drawing.Size(69, 23);
            this.buttonNewIconLib.TabIndex = 1;
            this.buttonNewIconLib.Text = "Create...";
            this.buttonNewIconLib.UseVisualStyleBackColor = true;
            this.buttonNewIconLib.Click += new System.EventHandler(this.buttonNewIconLib_Click);
            // 
            // icon_lib
            // 
            this.icon_lib.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.icon_lib.ComponentId = 0;
            this.icon_lib.Controller = null;
            this.icon_lib.Location = new System.Drawing.Point(153, 5);
            this.icon_lib.Name = "icon_lib";
            this.icon_lib.ParameterCategory = "Playfield";
            this.icon_lib.ParameterName = "Icon Library";
            this.icon_lib.ReadOnly = true;
            this.icon_lib.ParameterType = AME.Controllers.eParamParentType.Component;
            this.icon_lib.Size = new System.Drawing.Size(396, 20);
            this.icon_lib.TabIndex = 0;
            this.icon_lib.TabStop = false;
            this.icon_lib.UseDelimiter = false;
            // 
            // buttonOpenIconLib
            // 
            this.buttonOpenIconLib.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenIconLib.Name = "buttonOpenIconLib";
            this.buttonOpenIconLib.Size = new System.Drawing.Size(69, 23);
            this.buttonOpenIconLib.TabIndex = 0;
            this.buttonOpenIconLib.Text = "Open...";
            this.buttonOpenIconLib.UseVisualStyleBackColor = true;
            this.buttonOpenIconLib.Click += new System.EventHandler(this.buttonOpenIconLib_Click);
            // 
            // iconListView1
            // 
            this.iconListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.iconListView1.ComponentId = -1;
            this.iconListView1.Controller = null;
            this.iconListView1.HideSelection = false;
            this.iconListView1.IconParameterCategory = null;
            this.iconListView1.IconParameterName = null;
            this.iconListView1.IconParameterType = AME.Controllers.eParamParentType.Component;
            this.iconListView1.Location = new System.Drawing.Point(3, 32);
            this.iconListView1.MultiSelect = false;
            this.iconListView1.Name = "iconListView1";
            this.iconListView1.Size = new System.Drawing.Size(546, 162);
            this.iconListView1.TabIndex = 0;
            this.iconListView1.TabStop = false;
            this.iconListView1.UseCompatibleStateImageBehavior = false;
            this.iconListView1.View = System.Windows.Forms.View.List;
            // 
            // IconLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "IconLibrary";
            this.Size = new System.Drawing.Size(560, 251);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonNewIconLib;
        private AME.Views.View_Components.CustomParameterTextBox icon_lib;
        private System.Windows.Forms.Button buttonOpenIconLib;
        private System.Windows.Forms.OpenFileDialog OpenImageDlg;
        private IconListView iconListView1;
    }
}
