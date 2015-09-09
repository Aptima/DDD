namespace VSG.ViewComponents
{
    partial class EvtPnl_StateChange
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exceptStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.fromStateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.eventID1 = new VSG.ViewComponents.EventID();
            this.engramRange1 = new VSG.ViewComponents.EngramRange();
            this.timeBox = new AME.Views.View_Components.CustomNumericUpDown(this.components);
            this.stateComboBox = new VSG.ViewComponents.StateComboBox(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Time (s.):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 122);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Transition to new state:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.exceptStateComboBox);
            this.groupBox1.Controls.Add(this.fromStateComboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(185, 107);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(369, 80);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional";
            // 
            // exceptStateComboBox
            // 
            this.exceptStateComboBox.ComponentId = -1;
            this.exceptStateComboBox.Controller = null;
            this.exceptStateComboBox.FormattingEnabled = true;
            this.exceptStateComboBox.Location = new System.Drawing.Point(194, 48);
            this.exceptStateComboBox.Name = "exceptStateComboBox";
            this.exceptStateComboBox.ParameterCategory = "State";
            this.exceptStateComboBox.ParameterName = "FuelDepletionState";
            this.exceptStateComboBox.ShowAllStates = false;
            this.exceptStateComboBox.Size = new System.Drawing.Size(170, 21);
            this.exceptStateComboBox.SpeciesId = -1;
            this.exceptStateComboBox.TabIndex = 5;
            // 
            // fromStateComboBox
            // 
            this.fromStateComboBox.ComponentId = -1;
            this.fromStateComboBox.Controller = null;
            this.fromStateComboBox.FormattingEnabled = true;
            this.fromStateComboBox.Location = new System.Drawing.Point(5, 48);
            this.fromStateComboBox.Name = "fromStateComboBox";
            this.fromStateComboBox.ParameterCategory = "State";
            this.fromStateComboBox.ParameterName = "FuelDepletionState";
            this.fromStateComboBox.ShowAllStates = false;
            this.fromStateComboBox.Size = new System.Drawing.Size(183, 21);
            this.fromStateComboBox.SpeciesId = -1;
            this.fromStateComboBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 26);
            this.label5.TabIndex = 3;
            this.label5.Text = "unless coming \r\nfrom these states ...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 26);
            this.label4.TabIndex = 2;
            this.label4.Text = "only if coming\r\nfrom these states ...";
            // 
            // eventID1
            // 
            this.eventID1.Controller = null;
            this.eventID1.DisplayID = -1;
            this.eventID1.Location = new System.Drawing.Point(185, 2);
            this.eventID1.Margin = new System.Windows.Forms.Padding(2);
            this.eventID1.Name = "eventID1";
            this.eventID1.ParentID = -1;
            this.eventID1.Size = new System.Drawing.Size(386, 101);
            this.eventID1.TabIndex = 2;
            // 
            // engramRange1
            // 
            this.engramRange1.Controller = null;
            this.engramRange1.DisplayID = -1;
            this.engramRange1.Location = new System.Drawing.Point(5, 191);
            this.engramRange1.Margin = new System.Windows.Forms.Padding(2);
            this.engramRange1.MinimumSize = new System.Drawing.Size(279, 238);
            this.engramRange1.Name = "engramRange1";
            this.engramRange1.Size = new System.Drawing.Size(566, 403);
            this.engramRange1.TabIndex = 6;
            // 
            // timeBox
            // 
            this.timeBox.ComponentId = -1;
            this.timeBox.Controller = null;
            this.timeBox.Location = new System.Drawing.Point(62, 23);
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
            this.timeBox.ParameterCategory = "StateChangeEvent";
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
            // stateComboBox
            // 
            this.stateComboBox.ComponentId = -1;
            this.stateComboBox.Controller = null;
            this.stateComboBox.FormattingEnabled = true;
            this.stateComboBox.Location = new System.Drawing.Point(11, 155);
            this.stateComboBox.Name = "stateComboBox";
            this.stateComboBox.ParameterCategory = "State";
            this.stateComboBox.ParameterName = "FuelDepletionState";
            this.stateComboBox.ShowAllStates = false;
            this.stateComboBox.Size = new System.Drawing.Size(169, 21);
            this.stateComboBox.SpeciesId = -1;
            this.stateComboBox.TabIndex = 3;
            // 
            // EvtPnl_StateChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stateComboBox);
            this.Controls.Add(this.eventID1);
            this.Controls.Add(this.engramRange1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.MinimumSize = new System.Drawing.Size(362, 466);
            this.Name = "EvtPnl_StateChange";
            this.Size = new System.Drawing.Size(577, 600);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AME.Views.View_Components.CustomNumericUpDown timeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private EngramRange engramRange1;
        private EventID eventID1;
        private StateComboBox stateComboBox;
        private StateComboBox fromStateComboBox;
        private StateComboBox exceptStateComboBox;

    }
}
