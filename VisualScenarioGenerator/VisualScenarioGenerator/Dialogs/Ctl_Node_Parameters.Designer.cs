namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Node_Parameters
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudLaunchDuration = new System.Windows.Forms.NumericUpDown();
            this.nudDockingDuration = new System.Windows.Forms.NumericUpDown();
            this.nudTimeToAttack = new System.Windows.Forms.NumericUpDown();
            this.clbxSenses = new System.Windows.Forms.CheckedListBox();
            this.btnCreateSensor = new System.Windows.Forms.Button();
            this.btnCreateCapability = new System.Windows.Forms.Button();
            this.clbxCapabilities = new System.Windows.Forms.CheckedListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.clbxEmitters = new System.Windows.Forms.CheckedListBox();
            this.ctnCreateEmitter = new System.Windows.Forms.Button();
            this.lbxDepletionState = new System.Windows.Forms.ListBox();
            this.ckStealable = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbCombo_2 = new VisualScenarioGenerator.Dialogs.Ctl_Combos();
            this.cmbCombo_1 = new VisualScenarioGenerator.Dialogs.Ctl_Combos();
            this.sngSingleton_2 = new VisualScenarioGenerator.Dialogs.Ctl_Singletons();
            this.sngSingleton_3 = new VisualScenarioGenerator.Dialogs.Ctl_Singletons();
            this.sngSingleton_1 = new VisualScenarioGenerator.Dialogs.Ctl_Singletons();
            this.nndFuelUseRate = new VisualScenarioGenerator.NonNegDecimal();
            this.nndInitialFuelLoad = new VisualScenarioGenerator.NonNegDecimal();
            this.nndFuelCapacity = new VisualScenarioGenerator.NonNegDecimal();
            this.nndMaximumSpeed = new VisualScenarioGenerator.NonNegDecimal();
            ((System.ComponentModel.ISupportInitialize)(this.nudLaunchDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDockingDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeToAttack)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Launch Duration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Docking Duration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time To Attack";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Maximum Speed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Fuel Capacity";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(232, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Initial Fuel Load";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Fuel Use Rate";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(120, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Fuel Depletion State";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 169);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Sensors";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 339);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Capabilities";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 430);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Singleton Vulnerabilities";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 744);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Combo Vulnerabilities";
            // 
            // nudLaunchDuration
            // 
            this.nudLaunchDuration.Location = new System.Drawing.Point(6, 28);
            this.nudLaunchDuration.Name = "nudLaunchDuration";
            this.nudLaunchDuration.Size = new System.Drawing.Size(75, 20);
            this.nudLaunchDuration.TabIndex = 13;
            this.nudLaunchDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudDockingDuration
            // 
            this.nudDockingDuration.Location = new System.Drawing.Point(123, 28);
            this.nudDockingDuration.Name = "nudDockingDuration";
            this.nudDockingDuration.Size = new System.Drawing.Size(75, 20);
            this.nudDockingDuration.TabIndex = 14;
            this.nudDockingDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudTimeToAttack
            // 
            this.nudTimeToAttack.Location = new System.Drawing.Point(237, 28);
            this.nudTimeToAttack.Name = "nudTimeToAttack";
            this.nudTimeToAttack.Size = new System.Drawing.Size(75, 20);
            this.nudTimeToAttack.TabIndex = 15;
            this.nudTimeToAttack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // clbxSenses
            // 
            this.clbxSenses.FormattingEnabled = true;
            this.clbxSenses.Location = new System.Drawing.Point(6, 185);
            this.clbxSenses.Name = "clbxSenses";
            this.clbxSenses.Size = new System.Drawing.Size(224, 64);
            this.clbxSenses.TabIndex = 21;
            // 
            // btnCreateSensor
            // 
            this.btnCreateSensor.Location = new System.Drawing.Point(237, 185);
            this.btnCreateSensor.Name = "btnCreateSensor";
            this.btnCreateSensor.Size = new System.Drawing.Size(75, 41);
            this.btnCreateSensor.TabIndex = 22;
            this.btnCreateSensor.Text = "Create Sensor";
            this.btnCreateSensor.UseVisualStyleBackColor = true;
            // 
            // btnCreateCapability
            // 
            this.btnCreateCapability.Location = new System.Drawing.Point(237, 355);
            this.btnCreateCapability.Name = "btnCreateCapability";
            this.btnCreateCapability.Size = new System.Drawing.Size(75, 41);
            this.btnCreateCapability.TabIndex = 24;
            this.btnCreateCapability.Text = "Create Capability";
            this.btnCreateCapability.UseVisualStyleBackColor = true;
            // 
            // clbxCapabilities
            // 
            this.clbxCapabilities.FormattingEnabled = true;
            this.clbxCapabilities.Location = new System.Drawing.Point(6, 355);
            this.clbxCapabilities.Name = "clbxCapabilities";
            this.clbxCapabilities.Size = new System.Drawing.Size(224, 64);
            this.clbxCapabilities.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 252);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "Emitters";
            // 
            // clbxEmitters
            // 
            this.clbxEmitters.FormattingEnabled = true;
            this.clbxEmitters.Location = new System.Drawing.Point(6, 268);
            this.clbxEmitters.Name = "clbxEmitters";
            this.clbxEmitters.Size = new System.Drawing.Size(224, 64);
            this.clbxEmitters.TabIndex = 30;
            // 
            // ctnCreateEmitter
            // 
            this.ctnCreateEmitter.Location = new System.Drawing.Point(237, 268);
            this.ctnCreateEmitter.Name = "ctnCreateEmitter";
            this.ctnCreateEmitter.Size = new System.Drawing.Size(74, 43);
            this.ctnCreateEmitter.TabIndex = 31;
            this.ctnCreateEmitter.Text = "Create Emitter";
            this.ctnCreateEmitter.UseVisualStyleBackColor = true;
            // 
            // lbxDepletionState
            // 
            this.lbxDepletionState.FormattingEnabled = true;
            this.lbxDepletionState.Location = new System.Drawing.Point(103, 131);
            this.lbxDepletionState.Name = "lbxDepletionState";
            this.lbxDepletionState.Size = new System.Drawing.Size(120, 17);
            this.lbxDepletionState.TabIndex = 39;
            // 
            // ckStealable
            // 
            this.ckStealable.AutoSize = true;
            this.ckStealable.Location = new System.Drawing.Point(253, 130);
            this.ckStealable.Name = "ckStealable";
            this.ckStealable.Size = new System.Drawing.Size(70, 17);
            this.ckStealable.TabIndex = 40;
            this.ckStealable.Text = "Stealable";
            this.ckStealable.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 1291);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "Combo Vulnerabilities";
            // 
            // cmbCombo_2
            // 
            this.cmbCombo_2.AutoScroll = true;
            this.cmbCombo_2.Location = new System.Drawing.Point(16, 1558);
            this.cmbCombo_2.Name = "cmbCombo_2";
            this.cmbCombo_2.Size = new System.Drawing.Size(363, 229);
            this.cmbCombo_2.TabIndex = 46;
            // 
            // cmbCombo_1
            // 
            this.cmbCombo_1.AutoScroll = true;
            this.cmbCombo_1.Location = new System.Drawing.Point(13, 1327);
            this.cmbCombo_1.Name = "cmbCombo_1";
            this.cmbCombo_1.Size = new System.Drawing.Size(359, 223);
            this.cmbCombo_1.TabIndex = 45;
            // 
            // sngSingleton_2
            // 
            this.sngSingleton_2.AutoScroll = true;
            this.sngSingleton_2.Location = new System.Drawing.Point(6, 722);
            this.sngSingleton_2.Name = "sngSingleton_2";
            this.sngSingleton_2.Size = new System.Drawing.Size(356, 271);
            this.sngSingleton_2.TabIndex = 43;
            // 
            // sngSingleton_3
            // 
            this.sngSingleton_3.AutoScroll = true;
            this.sngSingleton_3.Location = new System.Drawing.Point(13, 999);
            this.sngSingleton_3.Name = "sngSingleton_3";
            this.sngSingleton_3.Size = new System.Drawing.Size(356, 270);
            this.sngSingleton_3.TabIndex = 42;
            // 
            // sngSingleton_1
            // 
            this.sngSingleton_1.AutoScroll = true;
            this.sngSingleton_1.Location = new System.Drawing.Point(6, 447);
            this.sngSingleton_1.Name = "sngSingleton_1";
            this.sngSingleton_1.Size = new System.Drawing.Size(356, 324);
            this.sngSingleton_1.TabIndex = 41;
            // 
            // nndFuelUseRate
            // 
            this.nndFuelUseRate.Location = new System.Drawing.Point(16, 130);
            this.nndFuelUseRate.Name = "nndFuelUseRate";
            this.nndFuelUseRate.Size = new System.Drawing.Size(57, 21);
            this.nndFuelUseRate.TabIndex = 38;
            this.nndFuelUseRate.Value = 0;
            // 
            // nndInitialFuelLoad
            // 
            this.nndInitialFuelLoad.Location = new System.Drawing.Point(253, 78);
            this.nndInitialFuelLoad.Name = "nndInitialFuelLoad";
            this.nndInitialFuelLoad.Size = new System.Drawing.Size(57, 21);
            this.nndInitialFuelLoad.TabIndex = 37;
            this.nndInitialFuelLoad.Value = 0;
            // 
            // nndFuelCapacity
            // 
            this.nndFuelCapacity.Location = new System.Drawing.Point(134, 80);
            this.nndFuelCapacity.Name = "nndFuelCapacity";
            this.nndFuelCapacity.Size = new System.Drawing.Size(57, 21);
            this.nndFuelCapacity.TabIndex = 36;
            this.nndFuelCapacity.Value = 0;
            // 
            // nndMaximumSpeed
            // 
            this.nndMaximumSpeed.Location = new System.Drawing.Point(16, 78);
            this.nndMaximumSpeed.Name = "nndMaximumSpeed";
            this.nndMaximumSpeed.Size = new System.Drawing.Size(62, 21);
            this.nndMaximumSpeed.TabIndex = 35;
            this.nndMaximumSpeed.Value = 0;
            // 
            // Ctl_Node_Parameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbCombo_2);
            this.Controls.Add(this.cmbCombo_1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.sngSingleton_2);
            this.Controls.Add(this.sngSingleton_3);
            this.Controls.Add(this.sngSingleton_1);
            this.Controls.Add(this.ckStealable);
            this.Controls.Add(this.lbxDepletionState);
            this.Controls.Add(this.nndFuelUseRate);
            this.Controls.Add(this.nndInitialFuelLoad);
            this.Controls.Add(this.nndFuelCapacity);
            this.Controls.Add(this.nndMaximumSpeed);
            this.Controls.Add(this.ctnCreateEmitter);
            this.Controls.Add(this.clbxEmitters);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnCreateCapability);
            this.Controls.Add(this.clbxCapabilities);
            this.Controls.Add(this.btnCreateSensor);
            this.Controls.Add(this.clbxSenses);
            this.Controls.Add(this.nudTimeToAttack);
            this.Controls.Add(this.nudDockingDuration);
            this.Controls.Add(this.nudLaunchDuration);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Ctl_Node_Parameters";
            this.Size = new System.Drawing.Size(406, 1787);
            ((System.ComponentModel.ISupportInitialize)(this.nudLaunchDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDockingDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeToAttack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudLaunchDuration;
        private System.Windows.Forms.NumericUpDown nudDockingDuration;
        private System.Windows.Forms.NumericUpDown nudTimeToAttack;
        private System.Windows.Forms.CheckedListBox clbxSenses;
        private System.Windows.Forms.Button btnCreateSensor;
        private System.Windows.Forms.Button btnCreateCapability;
        private System.Windows.Forms.CheckedListBox clbxCapabilities;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckedListBox clbxEmitters;
        private System.Windows.Forms.Button ctnCreateEmitter;
        private NonNegDecimal nndMaximumSpeed;
        private NonNegDecimal nndFuelCapacity;
        private NonNegDecimal nndInitialFuelLoad;
        private NonNegDecimal nndFuelUseRate;
        private System.Windows.Forms.ListBox lbxDepletionState;
        private System.Windows.Forms.CheckBox ckStealable;
        private Ctl_Singletons sngSingleton_1;
        private Ctl_Singletons sngSingleton_3;
        private Ctl_Singletons sngSingleton_2;
        private System.Windows.Forms.Label label14;
        private Ctl_Combos cmbCombo_1;
        private Ctl_Combos cmbCombo_2;

    }
}
