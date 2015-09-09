namespace TestGUIForAptimaLicenseGenerator
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbMajorVersion = new System.Windows.Forms.TextBox();
            this.tbMinorVersion = new System.Windows.Forms.TextBox();
            this.cbLicenseType = new System.Windows.Forms.ComboBox();
            this.cbProductName = new System.Windows.Forms.ComboBox();
            this.tbNumOfUsers = new System.Windows.Forms.TextBox();
            this.tbRandomNumber = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dtExpirationDate = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelInputString = new System.Windows.Forms.Label();
            this.tbLicenseKey = new System.Windows.Forms.TextBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.labelLength2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelValidity = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Major Version";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Minor Version";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "License Expiration Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "License Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Product Specific Custom Bits";
            this.toolTip1.SetToolTip(this.label6, "Each product will use these bits for different reasons.  Ex. The DDD uses this fo" +
                    "r \"Number of Seats\" for a simulation.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 219);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Customer ID:";
            // 
            // tbMajorVersion
            // 
            this.tbMajorVersion.Location = new System.Drawing.Point(156, 41);
            this.tbMajorVersion.MaxLength = 2;
            this.tbMajorVersion.Name = "tbMajorVersion";
            this.tbMajorVersion.Size = new System.Drawing.Size(121, 20);
            this.tbMajorVersion.TabIndex = 8;
            // 
            // tbMinorVersion
            // 
            this.tbMinorVersion.Location = new System.Drawing.Point(156, 75);
            this.tbMinorVersion.MaxLength = 2;
            this.tbMinorVersion.Name = "tbMinorVersion";
            this.tbMinorVersion.Size = new System.Drawing.Size(121, 20);
            this.tbMinorVersion.TabIndex = 9;
            // 
            // cbLicenseType
            // 
            this.cbLicenseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLicenseType.FormattingEnabled = true;
            this.cbLicenseType.Items.AddRange(new object[] {
            "Eval",
            "Beta",
            "Full"});
            this.cbLicenseType.Location = new System.Drawing.Point(156, 111);
            this.cbLicenseType.Name = "cbLicenseType";
            this.cbLicenseType.Size = new System.Drawing.Size(121, 21);
            this.cbLicenseType.TabIndex = 10;
            this.cbLicenseType.SelectedIndexChanged += new System.EventHandler(this.cbLicenseType_SelectedIndexChanged);
            // 
            // cbProductName
            // 
            this.cbProductName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProductName.FormattingEnabled = true;
            this.cbProductName.Location = new System.Drawing.Point(156, 6);
            this.cbProductName.Name = "cbProductName";
            this.cbProductName.Size = new System.Drawing.Size(121, 21);
            this.cbProductName.TabIndex = 11;
            this.cbProductName.SelectedIndexChanged += new System.EventHandler(this.cbProductName_SelectedIndexChanged);
            // 
            // tbNumOfUsers
            // 
            this.tbNumOfUsers.Location = new System.Drawing.Point(156, 178);
            this.tbNumOfUsers.MaxLength = 3;
            this.tbNumOfUsers.Name = "tbNumOfUsers";
            this.tbNumOfUsers.Size = new System.Drawing.Size(121, 20);
            this.tbNumOfUsers.TabIndex = 12;
            this.tbNumOfUsers.Text = "0";
            // 
            // tbRandomNumber
            // 
            this.tbRandomNumber.Location = new System.Drawing.Point(156, 212);
            this.tbRandomNumber.Name = "tbRandomNumber";
            this.tbRandomNumber.ReadOnly = true;
            this.tbRandomNumber.Size = new System.Drawing.Size(121, 20);
            this.tbRandomNumber.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(283, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Generate!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtExpirationDate
            // 
            this.dtExpirationDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtExpirationDate.Location = new System.Drawing.Point(156, 146);
            this.dtExpirationDate.Name = "dtExpirationDate";
            this.dtExpirationDate.Size = new System.Drawing.Size(121, 20);
            this.dtExpirationDate.TabIndex = 15;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 49);
            this.button2.TabIndex = 16;
            this.button2.Text = "Generate License Key";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 309);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Input String: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 336);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "License Key:";
            // 
            // labelInputString
            // 
            this.labelInputString.AutoSize = true;
            this.labelInputString.Location = new System.Drawing.Point(89, 309);
            this.labelInputString.Name = "labelInputString";
            this.labelInputString.Size = new System.Drawing.Size(70, 13);
            this.labelInputString.TabIndex = 19;
            this.labelInputString.Text = "<InputString>";
            // 
            // tbLicenseKey
            // 
            this.tbLicenseKey.Location = new System.Drawing.Point(92, 333);
            this.tbLicenseKey.Name = "tbLicenseKey";
            this.tbLicenseKey.ReadOnly = true;
            this.tbLicenseKey.Size = new System.Drawing.Size(251, 20);
            this.tbLicenseKey.TabIndex = 20;
            this.tbLicenseKey.TextChanged += new System.EventHandler(this.tbLicenseKey_TextChanged);
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(349, 336);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(46, 13);
            this.labelLength.TabIndex = 21;
            this.labelLength.Text = "Length: ";
            // 
            // labelLength2
            // 
            this.labelLength2.AutoSize = true;
            this.labelLength2.Location = new System.Drawing.Point(349, 309);
            this.labelLength2.Name = "labelLength2";
            this.labelLength2.Size = new System.Drawing.Size(46, 13);
            this.labelLength2.TabIndex = 22;
            this.labelLength2.Text = "Length: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 366);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Key Validity: ";
            // 
            // labelValidity
            // 
            this.labelValidity.AutoSize = true;
            this.labelValidity.Location = new System.Drawing.Point(89, 366);
            this.labelValidity.Name = "labelValidity";
            this.labelValidity.Size = new System.Drawing.Size(0, 13);
            this.labelValidity.TabIndex = 24;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(364, 210);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Use Existing";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(222, 366);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 26);
            this.button4.TabIndex = 26;
            this.button4.Text = "Copy to Clipboard";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(283, 176);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 27;
            this.button5.Text = "Calculate";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 404);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.labelValidity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelLength2);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.tbLicenseKey);
            this.Controls.Add(this.labelInputString);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dtExpirationDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbRandomNumber);
            this.Controls.Add(this.tbNumOfUsers);
            this.Controls.Add(this.cbProductName);
            this.Controls.Add(this.cbLicenseType);
            this.Controls.Add(this.tbMinorVersion);
            this.Controls.Add(this.tbMajorVersion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Aptima Product License Key Generator Test GUI";
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
        private System.Windows.Forms.TextBox tbMajorVersion;
        private System.Windows.Forms.TextBox tbMinorVersion;
        private System.Windows.Forms.ComboBox cbLicenseType;
        private System.Windows.Forms.ComboBox cbProductName;
        private System.Windows.Forms.TextBox tbNumOfUsers;
        private System.Windows.Forms.TextBox tbRandomNumber;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dtExpirationDate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelInputString;
        private System.Windows.Forms.TextBox tbLicenseKey;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Label labelLength2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelValidity;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

