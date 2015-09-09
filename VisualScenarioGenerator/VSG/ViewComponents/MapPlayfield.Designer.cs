namespace VSG.ViewComponents
{
    partial class MapPlayfield
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
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.utm_zone = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.vert_mmp = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.hor_mmp = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.map_file = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.buttonImportMap = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.easting = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.northing = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.map_picturebox = new System.Windows.Forms.PictureBox();
            this.OpenImageDlg = new System.Windows.Forms.OpenFileDialog();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_picturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(676, 516);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "Open Map to use in playfield.";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(668, 490);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Map Playfield";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.utm_zone);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.vert_mmp);
            this.panel1.Controls.Add(this.hor_mmp);
            this.panel1.Controls.Add(this.map_file);
            this.panel1.Controls.Add(this.buttonImportMap);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.easting);
            this.panel1.Controls.Add(this.northing);
            this.panel1.Controls.Add(this.map_picturebox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 462);
            this.panel1.TabIndex = 1;
            // 
            // utm_zone
            // 
            this.utm_zone.ComponentId = 0;
            this.utm_zone.Controller = null;
            this.utm_zone.Location = new System.Drawing.Point(3, 60);
            this.utm_zone.Name = "utm_zone";
            this.utm_zone.ParameterCategory = "Playfield";
            this.utm_zone.ParameterName = "UTM Zone";
            this.utm_zone.ParameterType = AME.Controllers.eParamParentType.Component;
            this.utm_zone.Size = new System.Drawing.Size(156, 20);
            this.utm_zone.TabIndex = 1;
            this.utm_zone.UseDelimiter = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 41);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "UTM Zone";
            // 
            // vert_mmp
            // 
            this.vert_mmp.ComponentId = 0;
            this.vert_mmp.Controller = null;
            this.vert_mmp.Location = new System.Drawing.Point(3, 150);
            this.vert_mmp.Name = "vert_mmp";
            this.vert_mmp.ParameterCategory = "Playfield";
            this.vert_mmp.ParameterName = "Vertical Scale";
            this.vert_mmp.ParameterType = AME.Controllers.eParamParentType.Component;
            this.vert_mmp.Size = new System.Drawing.Size(156, 20);
            this.vert_mmp.TabIndex = 3;
            this.vert_mmp.UseDelimiter = false;
            // 
            // hor_mmp
            // 
            this.hor_mmp.ComponentId = 0;
            this.hor_mmp.Controller = null;
            this.hor_mmp.Location = new System.Drawing.Point(3, 105);
            this.hor_mmp.Name = "hor_mmp";
            this.hor_mmp.ParameterCategory = "Playfield";
            this.hor_mmp.ParameterName = "Horizontal Scale";
            this.hor_mmp.ParameterType = AME.Controllers.eParamParentType.Component;
            this.hor_mmp.Size = new System.Drawing.Size(156, 20);
            this.hor_mmp.TabIndex = 2;
            this.hor_mmp.UseDelimiter = false;
            // 
            // map_file
            // 
            this.map_file.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.map_file.ComponentId = 0;
            this.map_file.Controller = null;
            this.map_file.Location = new System.Drawing.Point(90, 5);
            this.map_file.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.map_file.Name = "map_file";
            this.map_file.ParameterCategory = "Playfield";
            this.map_file.ParameterName = "Map Filename";
            this.map_file.ReadOnly = true;
            this.map_file.ParameterType = AME.Controllers.eParamParentType.Component;
            this.map_file.Size = new System.Drawing.Size(575, 20);
            this.map_file.TabIndex = 0;
            this.map_file.TabStop = false;
            this.map_file.UseDelimiter = false;
            // 
            // buttonImportMap
            // 
            this.buttonImportMap.Location = new System.Drawing.Point(3, 3);
            this.buttonImportMap.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.buttonImportMap.Name = "buttonImportMap";
            this.buttonImportMap.Size = new System.Drawing.Size(81, 23);
            this.buttonImportMap.TabIndex = 0;
            this.buttonImportMap.Text = "Open...";
            this.buttonImportMap.UseVisualStyleBackColor = true;
            this.buttonImportMap.Click += new System.EventHandler(this.buttonImportMap_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 176);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Easting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 221);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Northing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 131);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vertical m/px";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Horizontal m/px";
            // 
            // easting
            // 
            this.easting.ComponentId = 0;
            this.easting.Controller = null;
            this.easting.Location = new System.Drawing.Point(3, 240);
            this.easting.Name = "easting";
            this.easting.ParameterCategory = "Playfield";
            this.easting.ParameterName = "Easting";
            this.easting.ParameterType = AME.Controllers.eParamParentType.Component;
            this.easting.Size = new System.Drawing.Size(156, 20);
            this.easting.TabIndex = 5;
            this.easting.UseDelimiter = false;
            // 
            // northing
            // 
            this.northing.ComponentId = 0;
            this.northing.Controller = null;
            this.northing.Location = new System.Drawing.Point(3, 195);
            this.northing.Name = "northing";
            this.northing.ParameterCategory = "Playfield";
            this.northing.ParameterName = "Northing";
            this.northing.ParameterType = AME.Controllers.eParamParentType.Component;
            this.northing.Size = new System.Drawing.Size(156, 20);
            this.northing.TabIndex = 4;
            this.northing.UseDelimiter = false;
            // 
            // map_picturebox
            // 
            this.map_picturebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.map_picturebox.BackColor = System.Drawing.Color.Black;
            this.map_picturebox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.map_picturebox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.map_picturebox.Location = new System.Drawing.Point(168, 34);
            this.map_picturebox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.map_picturebox.Name = "map_picturebox";
            this.map_picturebox.Size = new System.Drawing.Size(497, 425);
            this.map_picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.map_picturebox.TabIndex = 14;
            this.map_picturebox.TabStop = false;
            // 
            // OpenImageDlg
            // 
            this.OpenImageDlg.FileName = "OpenImageDlg";
            // 
            // MapPlayfield
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "MapPlayfield";
            this.Size = new System.Drawing.Size(676, 516);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_picturebox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private AME.Views.View_Components.CustomParameterTextBox utm_zone;
        private System.Windows.Forms.Label label5;
        private AME.Views.View_Components.CustomParameterTextBox vert_mmp;
        private AME.Views.View_Components.CustomParameterTextBox hor_mmp;
        private AME.Views.View_Components.CustomParameterTextBox map_file;
        private System.Windows.Forms.Button buttonImportMap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomParameterTextBox easting;
        private AME.Views.View_Components.CustomParameterTextBox northing;
        private System.Windows.Forms.PictureBox map_picturebox;
        private System.Windows.Forms.OpenFileDialog OpenImageDlg;
    }
}
