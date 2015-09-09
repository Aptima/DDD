namespace Aptima.Asim.DDD.Client.Dialogs
{
    partial class OptionsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageRangeRings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelVulnerabilitiesOpacity = new System.Windows.Forms.Label();
            this.trackBarVulnerabilitiesOpacity = new System.Windows.Forms.TrackBar();
            this.panelVulnerabilitiesColorSwatch = new System.Windows.Forms.Panel();
            this.comboBoxVulnerabilitiesColor = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelCapabilitiesOpacity = new System.Windows.Forms.Label();
            this.trackBarCapabilitiesOpacity = new System.Windows.Forms.TrackBar();
            this.panelCapabilitiesColorSwatch = new System.Windows.Forms.Panel();
            this.comboBoxCapabilitiesColor = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSensorsOpacity = new System.Windows.Forms.Label();
            this.trackBarSensorsOpacity = new System.Windows.Forms.TrackBar();
            this.panelSensorsColorSwatch = new System.Windows.Forms.Panel();
            this.comboBoxSensorsColor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageRangeRings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVulnerabilitiesOpacity)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCapabilitiesOpacity)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSensorsOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(184, 403);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(266, 403);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageRangeRings);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(328, 384);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageRangeRings
            // 
            this.tabPageRangeRings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageRangeRings.Controls.Add(this.groupBox3);
            this.tabPageRangeRings.Controls.Add(this.groupBox2);
            this.tabPageRangeRings.Controls.Add(this.groupBox1);
            this.tabPageRangeRings.Location = new System.Drawing.Point(4, 22);
            this.tabPageRangeRings.Name = "tabPageRangeRings";
            this.tabPageRangeRings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRangeRings.Size = new System.Drawing.Size(320, 358);
            this.tabPageRangeRings.TabIndex = 0;
            this.tabPageRangeRings.Text = "Range Rings";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.labelVulnerabilitiesOpacity);
            this.groupBox3.Controls.Add(this.trackBarVulnerabilitiesOpacity);
            this.groupBox3.Controls.Add(this.panelVulnerabilitiesColorSwatch);
            this.groupBox3.Controls.Add(this.comboBoxVulnerabilitiesColor);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(7, 187);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(307, 84);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vulnerabilities";
            // 
            // labelVulnerabilitiesOpacity
            // 
            this.labelVulnerabilitiesOpacity.AutoSize = true;
            this.labelVulnerabilitiesOpacity.Location = new System.Drawing.Point(185, 47);
            this.labelVulnerabilitiesOpacity.Name = "labelVulnerabilitiesOpacity";
            this.labelVulnerabilitiesOpacity.Size = new System.Drawing.Size(47, 13);
            this.labelVulnerabilitiesOpacity.TabIndex = 12;
            this.labelVulnerabilitiesOpacity.Text = "[opacity]";
            // 
            // trackBarVulnerabilitiesOpacity
            // 
            this.trackBarVulnerabilitiesOpacity.AutoSize = false;
            this.trackBarVulnerabilitiesOpacity.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackBarVulnerabilitiesOpacity.LargeChange = 1;
            this.trackBarVulnerabilitiesOpacity.Location = new System.Drawing.Point(58, 44);
            this.trackBarVulnerabilitiesOpacity.Name = "trackBarVulnerabilitiesOpacity";
            this.trackBarVulnerabilitiesOpacity.Size = new System.Drawing.Size(121, 33);
            this.trackBarVulnerabilitiesOpacity.TabIndex = 11;
            this.trackBarVulnerabilitiesOpacity.Tag = "Vulnerabilities";
            this.trackBarVulnerabilitiesOpacity.ValueChanged += new System.EventHandler(this.trackBarValueChanged);
            // 
            // panelVulnerabilitiesColorSwatch
            // 
            this.panelVulnerabilitiesColorSwatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelVulnerabilitiesColorSwatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelVulnerabilitiesColorSwatch.Location = new System.Drawing.Point(186, 17);
            this.panelVulnerabilitiesColorSwatch.Name = "panelVulnerabilitiesColorSwatch";
            this.panelVulnerabilitiesColorSwatch.Size = new System.Drawing.Size(20, 20);
            this.panelVulnerabilitiesColorSwatch.TabIndex = 10;
            this.panelVulnerabilitiesColorSwatch.Click += new System.EventHandler(this.colorSwatchPanel_Click);
            // 
            // comboBoxVulnerabilitiesColor
            // 
            this.comboBoxVulnerabilitiesColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVulnerabilitiesColor.FormattingEnabled = true;
            this.comboBoxVulnerabilitiesColor.Items.AddRange(new object[] {
            "Beige",
            "Black",
            "Blue",
            "Brown",
            "Gold",
            "Gray",
            "Green",
            "Light Blue",
            "Light Green",
            "Orange",
            "Purple",
            "Red",
            "Silver",
            "White",
            "Yellow"});
            this.comboBoxVulnerabilitiesColor.Location = new System.Drawing.Point(58, 17);
            this.comboBoxVulnerabilitiesColor.Name = "comboBoxVulnerabilitiesColor";
            this.comboBoxVulnerabilitiesColor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVulnerabilitiesColor.TabIndex = 9;
            this.comboBoxVulnerabilitiesColor.Tag = "Vulnerabilities";
            this.toolTip1.SetToolTip(this.comboBoxVulnerabilitiesColor, "Select the color for displayed Sensor range rings.");
            this.comboBoxVulnerabilitiesColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Opacity:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Color:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.labelCapabilitiesOpacity);
            this.groupBox2.Controls.Add(this.trackBarCapabilitiesOpacity);
            this.groupBox2.Controls.Add(this.panelCapabilitiesColorSwatch);
            this.groupBox2.Controls.Add(this.comboBoxCapabilitiesColor);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(6, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 84);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Capabilities";
            // 
            // labelCapabilitiesOpacity
            // 
            this.labelCapabilitiesOpacity.AutoSize = true;
            this.labelCapabilitiesOpacity.Location = new System.Drawing.Point(186, 51);
            this.labelCapabilitiesOpacity.Name = "labelCapabilitiesOpacity";
            this.labelCapabilitiesOpacity.Size = new System.Drawing.Size(47, 13);
            this.labelCapabilitiesOpacity.TabIndex = 12;
            this.labelCapabilitiesOpacity.Text = "[opacity]";
            // 
            // trackBarCapabilitiesOpacity
            // 
            this.trackBarCapabilitiesOpacity.AutoSize = false;
            this.trackBarCapabilitiesOpacity.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackBarCapabilitiesOpacity.LargeChange = 1;
            this.trackBarCapabilitiesOpacity.Location = new System.Drawing.Point(59, 48);
            this.trackBarCapabilitiesOpacity.Name = "trackBarCapabilitiesOpacity";
            this.trackBarCapabilitiesOpacity.Size = new System.Drawing.Size(121, 33);
            this.trackBarCapabilitiesOpacity.TabIndex = 11;
            this.trackBarCapabilitiesOpacity.Tag = "Capabilities";
            this.trackBarCapabilitiesOpacity.ValueChanged += new System.EventHandler(this.trackBarValueChanged);
            // 
            // panelCapabilitiesColorSwatch
            // 
            this.panelCapabilitiesColorSwatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCapabilitiesColorSwatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelCapabilitiesColorSwatch.Location = new System.Drawing.Point(187, 17);
            this.panelCapabilitiesColorSwatch.Name = "panelCapabilitiesColorSwatch";
            this.panelCapabilitiesColorSwatch.Size = new System.Drawing.Size(20, 20);
            this.panelCapabilitiesColorSwatch.TabIndex = 10;
            this.panelCapabilitiesColorSwatch.Click += new System.EventHandler(this.colorSwatchPanel_Click);
            // 
            // comboBoxCapabilitiesColor
            // 
            this.comboBoxCapabilitiesColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCapabilitiesColor.FormattingEnabled = true;
            this.comboBoxCapabilitiesColor.Items.AddRange(new object[] {
            "Beige",
            "Black",
            "Blue",
            "Brown",
            "Gold",
            "Gray",
            "Green",
            "Light Blue",
            "Light Green",
            "Orange",
            "Purple",
            "Red",
            "Silver",
            "White",
            "Yellow"});
            this.comboBoxCapabilitiesColor.Location = new System.Drawing.Point(59, 17);
            this.comboBoxCapabilitiesColor.Name = "comboBoxCapabilitiesColor";
            this.comboBoxCapabilitiesColor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCapabilitiesColor.TabIndex = 9;
            this.comboBoxCapabilitiesColor.Tag = "Capabilities";
            this.toolTip1.SetToolTip(this.comboBoxCapabilitiesColor, "Select the color for displayed Sensor range rings.");
            this.comboBoxCapabilitiesColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Opacity:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Color:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.labelSensorsOpacity);
            this.groupBox1.Controls.Add(this.trackBarSensorsOpacity);
            this.groupBox1.Controls.Add(this.panelSensorsColorSwatch);
            this.groupBox1.Controls.Add(this.comboBoxSensorsColor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensors";
            // 
            // labelSensorsOpacity
            // 
            this.labelSensorsOpacity.AutoSize = true;
            this.labelSensorsOpacity.Location = new System.Drawing.Point(185, 47);
            this.labelSensorsOpacity.Name = "labelSensorsOpacity";
            this.labelSensorsOpacity.Size = new System.Drawing.Size(47, 13);
            this.labelSensorsOpacity.TabIndex = 6;
            this.labelSensorsOpacity.Text = "[opacity]";
            // 
            // trackBarSensorsOpacity
            // 
            this.trackBarSensorsOpacity.AutoSize = false;
            this.trackBarSensorsOpacity.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackBarSensorsOpacity.LargeChange = 1;
            this.trackBarSensorsOpacity.Location = new System.Drawing.Point(58, 44);
            this.trackBarSensorsOpacity.Name = "trackBarSensorsOpacity";
            this.trackBarSensorsOpacity.Size = new System.Drawing.Size(121, 33);
            this.trackBarSensorsOpacity.TabIndex = 5;
            this.trackBarSensorsOpacity.Tag = "Sensors";
            this.trackBarSensorsOpacity.ValueChanged += new System.EventHandler(this.trackBarValueChanged);
            // 
            // panelSensorsColorSwatch
            // 
            this.panelSensorsColorSwatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSensorsColorSwatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelSensorsColorSwatch.Location = new System.Drawing.Point(186, 17);
            this.panelSensorsColorSwatch.Name = "panelSensorsColorSwatch";
            this.panelSensorsColorSwatch.Size = new System.Drawing.Size(20, 20);
            this.panelSensorsColorSwatch.TabIndex = 4;
            this.panelSensorsColorSwatch.Click += new System.EventHandler(this.colorSwatchPanel_Click);
            // 
            // comboBoxSensorsColor
            // 
            this.comboBoxSensorsColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSensorsColor.FormattingEnabled = true;
            this.comboBoxSensorsColor.Items.AddRange(new object[] {
            "Beige",
            "Black",
            "Blue",
            "Brown",
            "Gold",
            "Gray",
            "Green",
            "Light Blue",
            "Light Green",
            "Orange",
            "Purple",
            "Red",
            "Silver",
            "White",
            "Yellow"});
            this.comboBoxSensorsColor.Location = new System.Drawing.Point(58, 17);
            this.comboBoxSensorsColor.Name = "comboBoxSensorsColor";
            this.comboBoxSensorsColor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSensorsColor.TabIndex = 2;
            this.comboBoxSensorsColor.Tag = "Sensors";
            this.toolTip1.SetToolTip(this.comboBoxSensorsColor, "Select the color for displayed Sensor range rings.");
            this.comboBoxSensorsColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Opacity:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Color:";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(355, 440);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(361, 472);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(361, 472);
            this.Name = "OptionsDialog";
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tabPageRangeRings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVulnerabilitiesOpacity)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCapabilitiesOpacity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSensorsOpacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageRangeRings;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelSensorsColorSwatch;
        private System.Windows.Forms.ComboBox comboBoxSensorsColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelSensorsOpacity;
        private System.Windows.Forms.TrackBar trackBarSensorsOpacity;
        private System.Windows.Forms.Label labelVulnerabilitiesOpacity;
        private System.Windows.Forms.TrackBar trackBarVulnerabilitiesOpacity;
        private System.Windows.Forms.Panel panelVulnerabilitiesColorSwatch;
        private System.Windows.Forms.ComboBox comboBoxVulnerabilitiesColor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelCapabilitiesOpacity;
        private System.Windows.Forms.TrackBar trackBarCapabilitiesOpacity;
        private System.Windows.Forms.Panel panelCapabilitiesColorSwatch;
        private System.Windows.Forms.ComboBox comboBoxCapabilitiesColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}