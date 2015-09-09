namespace Elsehemy.Controls
{
    partial class ColorWheelContainer
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
            this.btnOK = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.nudSaturation = new System.Windows.Forms.NumericUpDown();
            this.Label7 = new System.Windows.Forms.Label();
            this.nudBrightness = new System.Windows.Forms.NumericUpDown();
            this.nudRed = new System.Windows.Forms.NumericUpDown();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.pnlSelectedColor = new System.Windows.Forms.Panel();
            this.pnlBrightness = new System.Windows.Forms.Panel();
            this.nudBlue = new System.Windows.Forms.NumericUpDown();
            this.Label4 = new System.Windows.Forms.Label();
            this.nudGreen = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.nudHue = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(110, 309);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(64, 24);
            this.btnOK.TabIndex = 72;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(142, 269);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(48, 23);
            this.Label3.TabIndex = 63;
            this.Label3.Text = "Blue:";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudSaturation
            // 
            this.nudSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSaturation.Location = new System.Drawing.Point(86, 245);
            this.nudSaturation.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSaturation.Name = "nudSaturation";
            this.nudSaturation.Size = new System.Drawing.Size(48, 22);
            this.nudSaturation.TabIndex = 60;
            this.nudSaturation.ValueChanged += new System.EventHandler(this.HandleHSVChange);
            // 
            // Label7
            // 
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(6, 269);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(72, 23);
            this.Label7.TabIndex = 68;
            this.Label7.Text = "Brightness:";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudBrightness
            // 
            this.nudBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudBrightness.Location = new System.Drawing.Point(86, 269);
            this.nudBrightness.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBrightness.Name = "nudBrightness";
            this.nudBrightness.Size = new System.Drawing.Size(48, 22);
            this.nudBrightness.TabIndex = 65;
            this.nudBrightness.ValueChanged += new System.EventHandler(this.HandleHSVChange);
            // 
            // nudRed
            // 
            this.nudRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudRed.Location = new System.Drawing.Point(198, 221);
            this.nudRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRed.Name = "nudRed";
            this.nudRed.Size = new System.Drawing.Size(48, 22);
            this.nudRed.TabIndex = 56;
            this.nudRed.ValueChanged += new System.EventHandler(this.HandleRGBChange);
            // 
            // pnlColor
            // 
            this.pnlColor.Location = new System.Drawing.Point(19, 2);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(176, 176);
            this.pnlColor.TabIndex = 69;
            this.pnlColor.Visible = false;
            // 
            // Label6
            // 
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(6, 245);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(72, 23);
            this.Label6.TabIndex = 67;
            this.Label6.Text = "Saturation:";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(142, 221);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 23);
            this.Label1.TabIndex = 61;
            this.Label1.Text = "Red:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label5
            // 
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(6, 221);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(72, 23);
            this.Label5.TabIndex = 66;
            this.Label5.Text = "Hue:";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSelectedColor
            // 
            this.pnlSelectedColor.Location = new System.Drawing.Point(198, 189);
            this.pnlSelectedColor.Name = "pnlSelectedColor";
            this.pnlSelectedColor.Size = new System.Drawing.Size(48, 24);
            this.pnlSelectedColor.TabIndex = 71;
            this.pnlSelectedColor.Visible = false;
            // 
            // pnlBrightness
            // 
            this.pnlBrightness.Location = new System.Drawing.Point(219, 2);
            this.pnlBrightness.Name = "pnlBrightness";
            this.pnlBrightness.Size = new System.Drawing.Size(16, 176);
            this.pnlBrightness.TabIndex = 70;
            this.pnlBrightness.Visible = false;
            // 
            // nudBlue
            // 
            this.nudBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudBlue.Location = new System.Drawing.Point(198, 269);
            this.nudBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBlue.Name = "nudBlue";
            this.nudBlue.Size = new System.Drawing.Size(48, 22);
            this.nudBlue.TabIndex = 58;
            this.nudBlue.ValueChanged += new System.EventHandler(this.HandleRGBChange);
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(142, 189);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(48, 24);
            this.Label4.TabIndex = 64;
            this.Label4.Text = "Color:";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudGreen
            // 
            this.nudGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGreen.Location = new System.Drawing.Point(198, 245);
            this.nudGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudGreen.Name = "nudGreen";
            this.nudGreen.Size = new System.Drawing.Size(48, 22);
            this.nudGreen.TabIndex = 57;
            this.nudGreen.ValueChanged += new System.EventHandler(this.HandleRGBChange);
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(142, 245);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(48, 23);
            this.Label2.TabIndex = 62;
            this.Label2.Text = "Green:";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudHue
            // 
            this.nudHue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHue.Location = new System.Drawing.Point(86, 221);
            this.nudHue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudHue.Name = "nudHue";
            this.nudHue.Size = new System.Drawing.Size(48, 22);
            this.nudHue.TabIndex = 59;
            this.nudHue.ValueChanged += new System.EventHandler(this.HandleHSVChange);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(182, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 24);
            this.btnCancel.TabIndex = 73;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ColorWheelContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.nudSaturation);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.nudBrightness);
            this.Controls.Add(this.nudRed);
            this.Controls.Add(this.pnlColor);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.pnlSelectedColor);
            this.Controls.Add(this.pnlBrightness);
            this.Controls.Add(this.nudBlue);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.nudGreen);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.nudHue);
            this.Name = "ColorWheelContainer";
            this.Size = new System.Drawing.Size(256, 338);
            this.Load += new System.EventHandler(this.ColorChooser1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorChooser1_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HandleMouse);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouse);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.nudSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.NumericUpDown nudSaturation;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.NumericUpDown nudBrightness;
        internal System.Windows.Forms.NumericUpDown nudRed;
        internal System.Windows.Forms.Panel pnlColor;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Panel pnlSelectedColor;
        internal System.Windows.Forms.Panel pnlBrightness;
        internal System.Windows.Forms.NumericUpDown nudBlue;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.NumericUpDown nudGreen;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.NumericUpDown nudHue;
        internal System.Windows.Forms.Button btnCancel;
    }
}
