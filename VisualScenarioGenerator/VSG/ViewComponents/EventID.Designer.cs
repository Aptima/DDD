namespace VSG.ViewComponents
{
    partial class EventID
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
            this.unitLinkBox = new AME.Views.View_Components.CustomLinkBox(this.components);
            this.speciesCompletionUnitCheckBox1 = new AME.Views.View_Components.CustomCheckBox(this.components);
            this.orLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unit:";
            // 
            // unitLinkBox
            // 
            this.unitLinkBox.CheckLinkLevel = ((uint)(1u));
            this.unitLinkBox.CheckOnClick = true;
            this.unitLinkBox.ConnectFromId = -1;
            this.unitLinkBox.ConnectLinkType = null;
            this.unitLinkBox.ConnectRootId = -1;
            this.unitLinkBox.Controller = null;
            this.unitLinkBox.DisplayComponentType = null;
            this.unitLinkBox.DisplayLinkType = null;
            this.unitLinkBox.DisplayParameterCategory = "";
            this.unitLinkBox.DisplayParameterName = "";
            this.unitLinkBox.DisplayRootId = -1;
            this.unitLinkBox.FilterResult = false;
            this.unitLinkBox.FormattingEnabled = true;
            this.unitLinkBox.Location = new System.Drawing.Point(2, 19);
            this.unitLinkBox.Margin = new System.Windows.Forms.Padding(2);
            this.unitLinkBox.Name = "unitLinkBox";
            this.unitLinkBox.OneToMany = false;
            this.unitLinkBox.ParameterFilterCategory = "";
            this.unitLinkBox.ParameterFilterName = "";
            this.unitLinkBox.ParameterFilterValue = "";
            this.unitLinkBox.Size = new System.Drawing.Size(189, 64);
            this.unitLinkBox.TabIndex = 1;
            // 
            // speciesCompletionUnitCheckBox1
            // 
            this.speciesCompletionUnitCheckBox1.AutoSize = true;
            this.speciesCompletionUnitCheckBox1.ComponentId = 0;
            this.speciesCompletionUnitCheckBox1.Controller = null;
            this.speciesCompletionUnitCheckBox1.Location = new System.Drawing.Point(242, 43);
            this.speciesCompletionUnitCheckBox1.Margin = new System.Windows.Forms.Padding(2);
            this.speciesCompletionUnitCheckBox1.Name = "speciesCompletionUnitCheckBox1";
            this.speciesCompletionUnitCheckBox1.ParameterCategory = "SpeciesCompletion";
            this.speciesCompletionUnitCheckBox1.ParameterName = "Unit";
            this.speciesCompletionUnitCheckBox1.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.speciesCompletionUnitCheckBox1.Size = new System.Drawing.Size(131, 17);
            this.speciesCompletionUnitCheckBox1.TabIndex = 2;
            this.speciesCompletionUnitCheckBox1.Text = "Unit completing action";
            this.speciesCompletionUnitCheckBox1.UseVisualStyleBackColor = true;
            // 
            // orLabel
            // 
            this.orLabel.AutoSize = true;
            this.orLabel.Location = new System.Drawing.Point(211, 43);
            this.orLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.orLabel.Name = "orLabel";
            this.orLabel.Size = new System.Drawing.Size(16, 13);
            this.orLabel.TabIndex = 3;
            this.orLabel.Text = "or";
            // 
            // EventID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orLabel);
            this.Controls.Add(this.speciesCompletionUnitCheckBox1);
            this.Controls.Add(this.unitLinkBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EventID";
            this.Size = new System.Drawing.Size(375, 103);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private AME.Views.View_Components.CustomCheckBox speciesCompletionUnitCheckBox1;
        private System.Windows.Forms.Label orLabel;
        public AME.Views.View_Components.CustomLinkBox unitLinkBox;
    }
}
