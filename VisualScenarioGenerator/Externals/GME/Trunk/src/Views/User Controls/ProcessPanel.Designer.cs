using AME.Views.View_Components;
namespace User_Controls
{
    partial class ProcessPanel
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
            this.bottomTabControl = new AME.Views.View_Components.CustomTabControl(this.components);
            this.bottomTabPage = new AME.Views.View_Components.CustomTabPage(this.components);
            this.botomPanel = new System.Windows.Forms.Panel();
            this.runButton = new System.Windows.Forms.Button();
            this.inputsLabel = new System.Windows.Forms.Label();
            this.inputsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.topTabControl = new AME.Views.View_Components.CustomTabControl(this.components);
            this.topTabPage = new AME.Views.View_Components.CustomTabPage(this.components);
            this.topPanel = new System.Windows.Forms.Panel();
            this.parametersLabel = new System.Windows.Forms.Label();
            this.processCustomCombo = new AME.Views.View_Components.CustomCombo();
            this.parameterTable = new AME.Views.View_Components.ParameterTablePanel();
            this.bottomTabControl.SuspendLayout();
            this.bottomTabPage.SuspendLayout();
            this.botomPanel.SuspendLayout();
            this.topTabControl.SuspendLayout();
            this.topTabPage.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomTabControl
            // 
            this.bottomTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomTabControl.Controls.Add(this.bottomTabPage);
            this.bottomTabControl.Location = new System.Drawing.Point(0, 291);
            this.bottomTabControl.Name = "bottomTabControl";
            this.bottomTabControl.SelectedIndex = 0;
            this.bottomTabControl.Size = new System.Drawing.Size(259, 288);
            this.bottomTabControl.TabIndex = 5;
            // 
            // bottomTabPage
            // 
            this.bottomTabPage.Controls.Add(this.botomPanel);
            this.bottomTabPage.Description = "label1";
            this.bottomTabPage.Location = new System.Drawing.Point(4, 22);
            this.bottomTabPage.Name = "bottomTabPage";
            this.bottomTabPage.Size = new System.Drawing.Size(251, 262);
            this.bottomTabPage.TabIndex = 0;
            // 
            // botomPanel
            // 
            this.botomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.botomPanel.Controls.Add(this.runButton);
            this.botomPanel.Controls.Add(this.inputsLabel);
            this.botomPanel.Controls.Add(this.inputsFlowLayoutPanel);
            this.botomPanel.Location = new System.Drawing.Point(0, 28);
            this.botomPanel.Name = "botomPanel";
            this.botomPanel.Size = new System.Drawing.Size(251, 234);
            this.botomPanel.TabIndex = 1;
            // 
            // runButton
            // 
            this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.runButton.Location = new System.Drawing.Point(92, 200);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 9;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputsLabel
            // 
            this.inputsLabel.AutoSize = true;
            this.inputsLabel.Location = new System.Drawing.Point(3, 3);
            this.inputsLabel.Margin = new System.Windows.Forms.Padding(3);
            this.inputsLabel.Name = "inputsLabel";
            this.inputsLabel.Size = new System.Drawing.Size(39, 13);
            this.inputsLabel.TabIndex = 7;
            this.inputsLabel.Text = "Inputs:";
            // 
            // inputsFlowLayoutPanel
            // 
            this.inputsFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputsFlowLayoutPanel.AutoScroll = true;
            this.inputsFlowLayoutPanel.AutoScrollMargin = new System.Drawing.Size(3, 3);
            this.inputsFlowLayoutPanel.AutoScrollMinSize = new System.Drawing.Size(3, 3);
            this.inputsFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.inputsFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.inputsFlowLayoutPanel.Location = new System.Drawing.Point(3, 22);
            this.inputsFlowLayoutPanel.Name = "inputsFlowLayoutPanel";
            this.inputsFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(3);
            this.inputsFlowLayoutPanel.Size = new System.Drawing.Size(244, 172);
            this.inputsFlowLayoutPanel.TabIndex = 5;
            this.inputsFlowLayoutPanel.WrapContents = false;
            // 
            // topTabControl
            // 
            this.topTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topTabControl.Controls.Add(this.topTabPage);
            this.topTabControl.Location = new System.Drawing.Point(0, 0);
            this.topTabControl.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.topTabControl.Name = "topTabControl";
            this.topTabControl.SelectedIndex = 0;
            this.topTabControl.Size = new System.Drawing.Size(259, 276);
            this.topTabControl.TabIndex = 4;
            // 
            // topTabPage
            // 
            this.topTabPage.Controls.Add(this.topPanel);
            this.topTabPage.Description = "label1";
            this.topTabPage.Location = new System.Drawing.Point(4, 22);
            this.topTabPage.Name = "topTabPage";
            this.topTabPage.Size = new System.Drawing.Size(251, 250);
            this.topTabPage.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.Controls.Add(this.parametersLabel);
            this.topPanel.Controls.Add(this.processCustomCombo);
            this.topPanel.Controls.Add(this.parameterTable);
            this.topPanel.Location = new System.Drawing.Point(0, 28);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(251, 222);
            this.topPanel.TabIndex = 1;
            // 
            // parametersLabel
            // 
            this.parametersLabel.AutoSize = true;
            this.parametersLabel.Location = new System.Drawing.Point(3, 27);
            this.parametersLabel.Name = "parametersLabel";
            this.parametersLabel.Size = new System.Drawing.Size(63, 13);
            this.parametersLabel.TabIndex = 9;
            this.parametersLabel.Text = "Parameters:";
            // 
            // processCustomCombo
            // 
            this.processCustomCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.processCustomCombo.Controller = null;
            this.processCustomCombo.DisplayID = -1;
            this.processCustomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processCustomCombo.FormattingEnabled = true;
            this.processCustomCombo.LinkType = null;
            this.processCustomCombo.Location = new System.Drawing.Point(3, 3);
            this.processCustomCombo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.processCustomCombo.Name = "processCustomCombo";
            this.processCustomCombo.SelectedID = -1;
            this.processCustomCombo.Size = new System.Drawing.Size(245, 21);
            this.processCustomCombo.TabIndex = 6;
            this.processCustomCombo.Type = "";
            this.processCustomCombo.SelectedIDChangedEvent += new AME.Views.View_Components.CustomCombo.SelectedIDChanged(this.CustomComboControl_SelectedIDChangedEvent);
            // 
            // parameterTable
            // 
            this.parameterTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTable.BackColor = System.Drawing.SystemColors.Window;
            this.parameterTable.Controller = null;
            this.parameterTable.Label = "";
            this.parameterTable.Location = new System.Drawing.Point(3, 43);
            this.parameterTable.Name = "parameterTable";
            this.parameterTable.SelectedID = 0;
            this.parameterTable.SelectedIDType = AME.Controllers.eParamParentType.Component;
            this.parameterTable.Size = new System.Drawing.Size(245, 176);
            this.parameterTable.TabIndex = 8;
            // 
            // ProcessPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bottomTabControl);
            this.Controls.Add(this.topTabControl);
            this.Name = "ProcessPanel";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.Size = new System.Drawing.Size(261, 579);
            this.bottomTabControl.ResumeLayout(false);
            this.bottomTabPage.ResumeLayout(false);
            this.botomPanel.ResumeLayout(false);
            this.botomPanel.PerformLayout();
            this.topTabControl.ResumeLayout(false);
            this.topTabPage.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AME.Views.View_Components.CustomTabControl topTabControl;
        private AME.Views.View_Components.CustomTabPage topTabPage;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label parametersLabel;
        private AME.Views.View_Components.ParameterTablePanel parameterTable;
        private AME.Views.View_Components.CustomCombo processCustomCombo;
        private AME.Views.View_Components.CustomTabControl bottomTabControl;
        private AME.Views.View_Components.CustomTabPage bottomTabPage;
        private System.Windows.Forms.Panel botomPanel;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Label inputsLabel;
        private System.Windows.Forms.FlowLayoutPanel inputsFlowLayoutPanel;

    }
}
