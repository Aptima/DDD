namespace VSG.ViewComponents
{
    partial class VoiceChannel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customNumericUpDown1 = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.customParameterTextBox1 = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customLinkBox1 = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.customTabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Controls.Add(this.customTabPage1);
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTabControl1.Location = new System.Drawing.Point(0, 0);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(433, 439);
            this.customTabControl1.TabIndex = 0;
            // 
            // customTabPage1
            // 
            this.customTabPage1.AutoScroll = true;
            this.customTabPage1.Controls.Add(this.panel1);
            this.customTabPage1.Description = "label1";
            this.customTabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabPage1.Location = new System.Drawing.Point(4, 22);
            this.customTabPage1.Name = "customTabPage1";
            this.customTabPage1.Size = new System.Drawing.Size(425, 413);
            this.customTabPage1.TabIndex = 0;
            this.customTabPage1.Text = "Voice Channel";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 382);
            this.panel1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customNumericUpDown1);
            this.groupBox1.Controls.Add(this.customParameterTextBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.customLinkBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 372);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Voice Channel Parameters (Required)";
            // 
            // customNumericUpDown1
            // 
            this.customNumericUpDown1.ComponentId = 0;
            this.customNumericUpDown1.Controller = null;
            this.customNumericUpDown1.Location = new System.Drawing.Point(92, 61);
            this.customNumericUpDown1.Name = "customNumericUpDown1";
            this.customNumericUpDown1.ParameterCategory = "VoiceChannel";
            this.customNumericUpDown1.ParameterName = "Time";
            this.customNumericUpDown1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.customNumericUpDown1.Size = new System.Drawing.Size(84, 20);
            this.customNumericUpDown1.TabIndex = 9;
            this.customNumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // customParameterTextBox1
            // 
            this.customParameterTextBox1.ComponentId = -1;
            this.customParameterTextBox1.Controller = null;
            this.customParameterTextBox1.Location = new System.Drawing.Point(92, 24);
            this.customParameterTextBox1.Name = "customParameterTextBox1";
            this.customParameterTextBox1.ParameterCategory = "VoiceChannel";
            this.customParameterTextBox1.ParameterName = "ChannelName";
            this.customParameterTextBox1.ParameterType = AME.Controllers.eParamParentType.Component;
            this.customParameterTextBox1.Size = new System.Drawing.Size(166, 20);
            this.customParameterTextBox1.TabIndex = 8;
            this.customParameterTextBox1.UseDelimiter = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Open at time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Channel Name:";
            // 
            // customLinkBox1
            // 
            this.customLinkBox1.CheckLinkLevel = ((uint)(1u));
            this.customLinkBox1.CheckOnClick = true;
            this.customLinkBox1.ConnectFromId = -1;
            this.customLinkBox1.ConnectLinkType = "VoiceChannelMembers";
            this.customLinkBox1.ConnectRootId = 0;
            this.customLinkBox1.Controller = null;
            this.customLinkBox1.DisplayComponentType = "DecisionMaker";
            this.customLinkBox1.DisplayLinkType = "Scenario";
            this.customLinkBox1.DisplayParameterCategory = "";
            this.customLinkBox1.DisplayParameterName = "";
            this.customLinkBox1.DisplayRootId = 0;
            this.customLinkBox1.FilterResult = false;
            this.customLinkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customLinkBox1.FormattingEnabled = true;
            this.customLinkBox1.IntegralHeight = false;
            this.customLinkBox1.Location = new System.Drawing.Point(92, 99);
            this.customLinkBox1.Name = "customLinkBox1";
            this.customLinkBox1.OneToMany = true;
            this.customLinkBox1.ParameterFilterCategory = "";
            this.customLinkBox1.ParameterFilterName = "";
            this.customLinkBox1.ParameterFilterValue = "";
            this.customLinkBox1.Size = new System.Drawing.Size(166, 111);
            this.customLinkBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 99);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Members:";
            // 
            // VoiceChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTabControl1);
            this.Name = "VoiceChannel";
            this.Size = new System.Drawing.Size(433, 439);
            this.customTabControl1.ResumeLayout(false);
            this.customTabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl customTabControl1;
        private AME.Views.View_Components.CustomTabPage customTabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomLinkBox customLinkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private AME.Views.View_Components.CustomNumericUpDown customNumericUpDown1;
        private AME.Views.View_Components.CustomParameterTextBox customParameterTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
