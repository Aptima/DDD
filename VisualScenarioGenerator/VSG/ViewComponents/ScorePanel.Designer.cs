namespace VSG.ViewComponents
{
    partial class ScorePanel
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
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInitialValue = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.dmCalculateBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.dmViewBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.rulesLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.customTabControl1 = new AME.Views.View_Components.CustomTabControl(this.components);
            this.tabPage = new AME.Views.View_Components.CustomTabPage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.customTabControl1.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Initial Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(196, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "Calculate score for DMs:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(196, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Show score on DM displays:";
            // 
            // txtInitialValue
            // 
            this.txtInitialValue.ComponentId = -1;
            this.txtInitialValue.Controller = null;
            this.txtInitialValue.Location = new System.Drawing.Point(25, 32);
            this.txtInitialValue.Name = "txtInitialValue";
            this.txtInitialValue.ParameterCategory = "Score";
            this.txtInitialValue.ParameterName = "Initial";
            this.txtInitialValue.ParameterType = AME.Controllers.eParamParentType.Component;
            this.txtInitialValue.Size = new System.Drawing.Size(100, 20);
            this.txtInitialValue.TabIndex = 3;
            this.txtInitialValue.UseDelimiter = false;
            // 
            // dmCalculateBox
            // 
            this.dmCalculateBox.CheckLinkLevel = ((uint)(1u));
            this.dmCalculateBox.ConnectFromId = -1;
            this.dmCalculateBox.ConnectLinkType = null;
            this.dmCalculateBox.ConnectRootId = 0;
            this.dmCalculateBox.Controller = null;
            this.dmCalculateBox.DisplayComponentType = "DecisionMaker";
            this.dmCalculateBox.DisplayLinkType = "Scenario";
            this.dmCalculateBox.DisplayParameterCategory = "";
            this.dmCalculateBox.DisplayParameterName = "";
            this.dmCalculateBox.DisplayRecursiveCheck = false;
            this.dmCalculateBox.DisplayRootId = 1;
            this.dmCalculateBox.FilterResult = false;
            this.dmCalculateBox.FormattingEnabled = true;
            this.dmCalculateBox.Location = new System.Drawing.Point(199, 35);
            this.dmCalculateBox.Name = "dmCalculateBox";
            this.dmCalculateBox.OneToMany = false;
            this.dmCalculateBox.ParameterFilterCategory = "";
            this.dmCalculateBox.ParameterFilterName = "";
            this.dmCalculateBox.ParameterFilterValue = "";
            this.dmCalculateBox.Size = new System.Drawing.Size(163, 94);
            this.dmCalculateBox.TabIndex = 2;
            // 
            // dmViewBox
            // 
            this.dmViewBox.CheckLinkLevel = ((uint)(1u));
            this.dmViewBox.ConnectFromId = -1;
            this.dmViewBox.ConnectLinkType = null;
            this.dmViewBox.ConnectRootId = 0;
            this.dmViewBox.Controller = null;
            this.dmViewBox.DisplayComponentType = null;
            this.dmViewBox.DisplayLinkType = null;
            this.dmViewBox.DisplayParameterCategory = "";
            this.dmViewBox.DisplayParameterName = "";
            this.dmViewBox.DisplayRecursiveCheck = false;
            this.dmViewBox.DisplayRootId = 0;
            this.dmViewBox.FilterResult = false;
            this.dmViewBox.FormattingEnabled = true;
            this.dmViewBox.Location = new System.Drawing.Point(199, 35);
            this.dmViewBox.Name = "dmViewBox";
            this.dmViewBox.OneToMany = false;
            this.dmViewBox.ParameterFilterCategory = "";
            this.dmViewBox.ParameterFilterName = "";
            this.dmViewBox.ParameterFilterValue = "";
            this.dmViewBox.Size = new System.Drawing.Size(161, 94);
            this.dmViewBox.TabIndex = 4;
            // 
            // rulesLinkBox
            // 
            this.rulesLinkBox.CheckLinkLevel = ((uint)(1u));
            this.rulesLinkBox.ConnectFromId = -1;
            this.rulesLinkBox.ConnectLinkType = null;
            this.rulesLinkBox.ConnectRootId = -1;
            this.rulesLinkBox.Controller = null;
            this.rulesLinkBox.DisplayComponentType = null;
            this.rulesLinkBox.DisplayLinkType = null;
            this.rulesLinkBox.DisplayParameterCategory = "";
            this.rulesLinkBox.DisplayParameterName = "";
            this.rulesLinkBox.DisplayRecursiveCheck = false;
            this.rulesLinkBox.DisplayRootId = -1;
            this.rulesLinkBox.FilterResult = false;
            this.rulesLinkBox.FormattingEnabled = true;
            this.rulesLinkBox.Location = new System.Drawing.Point(21, 35);
            this.rulesLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.rulesLinkBox.Name = "rulesLinkBox";
            this.rulesLinkBox.OneToMany = false;
            this.rulesLinkBox.ParameterFilterCategory = "";
            this.rulesLinkBox.ParameterFilterName = "";
            this.rulesLinkBox.ParameterFilterValue = "";
            this.rulesLinkBox.Size = new System.Drawing.Size(163, 94);
            this.rulesLinkBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Score Rules:";
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.tabPage);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(437, 410);
            this.customTabControl1.TabIndex = 55;
            // 
            // tabPage
            // 
            this.tabPage.AutoScroll = true;
            this.tabPage.Controls.Add(this.panel1);
            this.tabPage.Description = "Score";
            this.tabPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage.Name = "tabPage";
            this.tabPage.Size = new System.Drawing.Size(429, 384);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Scoring Parameters";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.MinimumSize = new System.Drawing.Size(429, 361);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 361);
            this.panel1.TabIndex = 57;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rulesLinkBox);
            this.groupBox1.Controls.Add(this.dmCalculateBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 153);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scoring Parameters (Required)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.dmViewBox);
            this.groupBox2.Controls.Add(this.txtInitialValue);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 149);
            this.groupBox2.TabIndex = 56;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optional Parameters";
            // 
            // ScorePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "ScorePanel";
            this.Size = new System.Drawing.Size(437, 410);
            this.customTabControl1.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.tabPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomParameterTextBox txtInitialValue;
        private AME.Views.View_Components.CustomLinkBox dmCalculateBox;
        private AME.Views.View_Components.CustomLinkBox dmViewBox;
        private AME.Views.View_Components.CustomLinkBox rulesLinkBox;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage tabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        //private Ctl_BalanceBoxes bbSelectedRules;
    }
}
