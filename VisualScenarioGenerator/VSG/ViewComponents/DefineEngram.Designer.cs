namespace VSG.ViewComponents
{
    partial class DefineEngram
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
            this.label5 = new System.Windows.Forms.Label();
            this.txtInitialValue = new GME.Views.View_Components.CustomParameterTextBox(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.customLinkBox1 = new GME.Views.View_Components.CustomLinkBox(this.components);
            this.customTabControl1 = new GME.Views.View_Components.CustomTabControl(this.components);
            this.customTabPage1 = new GME.Views.View_Components.CustomTabPage(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Initial Value";
            // 
            // txtInitialValue
            // 
            this.txtInitialValue.ComponentId = 0;
            this.txtInitialValue.Controller = null;
            this.txtInitialValue.Location = new System.Drawing.Point(-1, 31);
            this.txtInitialValue.Name = "txtInitialValue";
            this.txtInitialValue.ParameterCategory = "DefineEngram";
            this.txtInitialValue.ParameterName = "InitialValue";
            this.txtInitialValue.SelectedIDType = GME.Controllers.eParamParentType.Component;
            this.txtInitialValue.Size = new System.Drawing.Size(258, 20);
            this.txtInitialValue.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.customTabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 520);
            this.panel1.TabIndex = 38;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.customLinkBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(505, 285);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Opposing Teams";
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectLinkType = "TeamAgainst";
            this.customLinkBox1.ConnectRootId = 0;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = "Team";
            this.customLinkBox1.DisplayLinkType = "Scenario";
            this.customLinkBox1.DisplayRootId = 0;
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.Location = new System.Drawing.Point(3, 23);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = true;
            this.customLinkBox1.Size = new System.Drawing.Size(499, 259);
            this.customLinkBox1.TabIndex = 3;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(574, 520);
            this.customTabControl1.TabIndex = 38;
            // 
            // customTabPage1
            // 
            this.customTabPage1.Controls.Add(this.panel3);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(566, 494);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Engram";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.txtInitialValue);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(566, 466);
            this.panel3.TabIndex = 3;
            // 
            // DefineEngram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(378, 206);
            this.Name = "DefineEngram";
            this.Size = new System.Drawing.Size(574, 520);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private GME.Views.View_Components.CustomParameterTextBox txtInitialValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private GME.Views.View_Components.CustomLinkBox customLinkBox1;
        private GME.Views.View_Components.CustomTabControl customTabControl1;
        private GME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel3;
    }
}
