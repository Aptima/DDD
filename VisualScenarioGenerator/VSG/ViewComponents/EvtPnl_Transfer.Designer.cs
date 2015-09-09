namespace VSG.ViewComponents
{
    partial class EvtPnl_Transfer
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fromLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.toLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Time (s.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 95);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "From:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 95);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "To:";
            // 
            // fromLinkBox
            // 
            this.fromLinkBox.CheckLinkLevel = ((uint)(1u));
            this.fromLinkBox.CheckOnClick = true;
            this.fromLinkBox.ConnectFromId = -1;
            this.fromLinkBox.ConnectLinkType = null;
            this.fromLinkBox.ConnectRootId = 0;
            this.fromLinkBox.Controller = null;
            this.fromLinkBox.DisplayComponentType = null;
            this.fromLinkBox.DisplayLinkType = null;
            this.fromLinkBox.DisplayParameterCategory = "";
            this.fromLinkBox.DisplayParameterName = "";
            this.fromLinkBox.DisplayRecursiveCheck = false;
            this.fromLinkBox.DisplayRootId = 0;
            this.fromLinkBox.FilterResult = false;
            this.fromLinkBox.FormattingEnabled = true;
            this.fromLinkBox.Location = new System.Drawing.Point(7, 119);
            this.fromLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.fromLinkBox.Name = "fromLinkBox";
            this.fromLinkBox.OneToMany = false;
            this.fromLinkBox.ParameterFilterCategory = "";
            this.fromLinkBox.ParameterFilterName = "";
            this.fromLinkBox.ParameterFilterValue = "";
            this.fromLinkBox.Size = new System.Drawing.Size(181, 109);
            this.fromLinkBox.TabIndex = 3;
            // 
            // toLinkBox
            // 
            this.toLinkBox.CheckLinkLevel = ((uint)(1u));
            this.toLinkBox.CheckOnClick = true;
            this.toLinkBox.ConnectFromId = -1;
            this.toLinkBox.ConnectLinkType = null;
            this.toLinkBox.ConnectRootId = 0;
            this.toLinkBox.Controller = null;
            this.toLinkBox.DisplayComponentType = null;
            this.toLinkBox.DisplayLinkType = null;
            this.toLinkBox.DisplayParameterCategory = "";
            this.toLinkBox.DisplayParameterName = "";
            this.toLinkBox.DisplayRecursiveCheck = false;
            this.toLinkBox.DisplayRootId = 0;
            this.toLinkBox.FilterResult = false;
            this.toLinkBox.FormattingEnabled = true;
            this.toLinkBox.Location = new System.Drawing.Point(192, 119);
            this.toLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.toLinkBox.Name = "toLinkBox";
            this.toLinkBox.OneToMany = false;
            this.toLinkBox.ParameterFilterCategory = "";
            this.toLinkBox.ParameterFilterName = "";
            this.toLinkBox.ParameterFilterValue = "";
            this.toLinkBox.Size = new System.Drawing.Size(178, 109);
            this.toLinkBox.TabIndex = 4;
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(190, 2);
            this.eventID1.Margin = new System.Windows.Forms.Padding(2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(380, 101);
            this.eventID1.TabIndex = 2;
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(7, 241);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(566, 403);
            this.engramRange1.TabIndex = 5;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(58, 24);
            this.timeBox.Margin = new System.Windows.Forms.Padding(2);
            this.timeBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timeBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeBox.Name = "timeBox";
            this.timeBox.ParameterCategory = "TransferEvent";
            this.timeBox.ParameterName = "Time";
            this.timeBox.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.timeBox.Size = new System.Drawing.Size(91, 20);
            this.timeBox.TabIndex = 1;
            this.timeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EvtPnl_Transfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.toLinkBox);
            this.Controls.Add(this.fromLinkBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.MinimumSize = new System.Drawing.Size(309, 448);
            this.Name = "EvtPnl_Transfer";
            this.Size = new System.Drawing.Size(573, 658);
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private AME.Views.View_Components.CustomLinkBox fromLinkBox;
        private AME.Views.View_Components.CustomLinkBox toLinkBox;
        private EngramRange engramRange1;
        private EventID eventID1;

    }
}
