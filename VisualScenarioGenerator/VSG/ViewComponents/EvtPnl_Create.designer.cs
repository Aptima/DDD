namespace VSG.ViewComponents
{
    partial class EvtPnl_Create
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.unitIDBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ownerLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.kindLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.subplatformLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unit ID:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Kind:";
            // 
            // unitIDBox
            // 
            this.unitIDBox.Enabled = false;
            this.unitIDBox.Location = new System.Drawing.Point(51, 2);
            this.unitIDBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.unitIDBox.Name = "unitIDBox";
            this.unitIDBox.Size = new System.Drawing.Size(91, 20);
            this.unitIDBox.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Owner:";
            // 
            // ownerLinkBox
            // 
            this.ownerLinkBox.CheckLinkLevel = ((uint)(2u));
            this.ownerLinkBox.CheckOnClick = true;
            this.ownerLinkBox.ConnectFromId = -1;
            this.ownerLinkBox.ConnectLinkType = null;
            this.ownerLinkBox.ConnectRootId = 0;
            this.ownerLinkBox.Controller = null;
            this.ownerLinkBox.DisplayComponentType = null;
            this.ownerLinkBox.DisplayLinkType = null;
            this.ownerLinkBox.DisplayParameterCategory = "";
            this.ownerLinkBox.DisplayParameterName = "";
            this.ownerLinkBox.DisplayRecursiveCheck = false;
            this.ownerLinkBox.DisplayRootId = 0;
            this.ownerLinkBox.FilterResult = false;
            this.ownerLinkBox.FormattingEnabled = true;
            this.ownerLinkBox.Location = new System.Drawing.Point(187, 44);
            this.ownerLinkBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ownerLinkBox.Name = "ownerLinkBox";
            this.ownerLinkBox.OneToMany = false;
            this.ownerLinkBox.ParameterFilterCategory = "";
            this.ownerLinkBox.ParameterFilterName = "";
            this.ownerLinkBox.ParameterFilterValue = "";
            this.ownerLinkBox.Size = new System.Drawing.Size(183, 79);
            this.ownerLinkBox.TabIndex = 15;
            // 
            // kindLinkBox
            // 
            this.kindLinkBox.CheckLinkLevel = ((uint)(1u));
            this.kindLinkBox.CheckOnClick = true;
            this.kindLinkBox.ConnectFromId = -1;
            this.kindLinkBox.ConnectLinkType = null;
            this.kindLinkBox.ConnectRootId = 0;
            this.kindLinkBox.Controller = null;
            this.kindLinkBox.DisplayComponentType = null;
            this.kindLinkBox.DisplayLinkType = null;
            this.kindLinkBox.DisplayParameterCategory = "";
            this.kindLinkBox.DisplayParameterName = "";
            this.kindLinkBox.DisplayRecursiveCheck = false;
            this.kindLinkBox.DisplayRootId = 0;
            this.kindLinkBox.FilterResult = false;
            this.kindLinkBox.FormattingEnabled = true;
            this.kindLinkBox.Location = new System.Drawing.Point(7, 44);
            this.kindLinkBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.kindLinkBox.Name = "kindLinkBox";
            this.kindLinkBox.OneToMany = false;
            this.kindLinkBox.ParameterFilterCategory = "";
            this.kindLinkBox.ParameterFilterName = "";
            this.kindLinkBox.ParameterFilterValue = "";
            this.kindLinkBox.Size = new System.Drawing.Size(176, 79);
            this.kindLinkBox.TabIndex = 14;
            // 
            // subplatformLinkBox
            // 
            this.subplatformLinkBox.CheckLinkLevel = ((uint)(1u));
            this.subplatformLinkBox.CheckOnClick = true;
            this.subplatformLinkBox.ConnectFromId = -1;
            this.subplatformLinkBox.ConnectLinkType = null;
            this.subplatformLinkBox.ConnectRootId = 0;
            this.subplatformLinkBox.Controller = null;
            this.subplatformLinkBox.DisplayComponentType = null;
            this.subplatformLinkBox.DisplayLinkType = null;
            this.subplatformLinkBox.DisplayParameterCategory = "";
            this.subplatformLinkBox.DisplayParameterName = "";
            this.subplatformLinkBox.DisplayRecursiveCheck = false;
            this.subplatformLinkBox.DisplayRootId = 0;
            this.subplatformLinkBox.FilterResult = false;
            this.subplatformLinkBox.FormattingEnabled = true;
            this.subplatformLinkBox.Location = new System.Drawing.Point(7, 164);
            this.subplatformLinkBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.subplatformLinkBox.Name = "subplatformLinkBox";
            this.subplatformLinkBox.OneToMany = true;
            this.subplatformLinkBox.ParameterFilterCategory = "";
            this.subplatformLinkBox.ParameterFilterName = "";
            this.subplatformLinkBox.ParameterFilterValue = "";
            this.subplatformLinkBox.Size = new System.Drawing.Size(176, 79);
            this.subplatformLinkBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 141);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Subplatforms:";
            // 
            // EvtPnl_Create
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.subplatformLinkBox);
            this.Controls.Add(this.ownerLinkBox);
            this.Controls.Add(this.kindLinkBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.unitIDBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(242, 162);
            this.Name = "EvtPnl_Create";
            this.Size = new System.Drawing.Size(372, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox unitIDBox;
        private System.Windows.Forms.Label label7;
        private AME.Views.View_Components.CustomLinkBox kindLinkBox;
        private AME.Views.View_Components.CustomLinkBox ownerLinkBox;
        private AME.Views.View_Components.CustomLinkBox subplatformLinkBox;
        private System.Windows.Forms.Label label3;
    }
}
