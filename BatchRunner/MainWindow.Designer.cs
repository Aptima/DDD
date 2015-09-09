namespace BatchRunner
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.batchFilePathTextBox = new System.Windows.Forms.TextBox();
            this.batchFileBrowseButton = new System.Windows.Forms.Button();
            this.loadBatchButton = new System.Windows.Forms.Button();
            this.runBatchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.dddExecutablePathTextBox = new System.Windows.Forms.TextBox();
            this.dddExecutableBrowseButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.newBatchButton = new System.Windows.Forms.Button();
            this.goButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dataTagTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.logPathTextBox = new System.Windows.Forms.TextBox();
            this.logPathBrowseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Batch File:";
            // 
            // batchFilePathTextBox
            // 
            this.batchFilePathTextBox.Location = new System.Drawing.Point(61, 31);
            this.batchFilePathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.batchFilePathTextBox.Name = "batchFilePathTextBox";
            this.batchFilePathTextBox.ReadOnly = true;
            this.batchFilePathTextBox.Size = new System.Drawing.Size(187, 20);
            this.batchFilePathTextBox.TabIndex = 1;
            // 
            // batchFileBrowseButton
            // 
            this.batchFileBrowseButton.Location = new System.Drawing.Point(251, 31);
            this.batchFileBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.batchFileBrowseButton.Name = "batchFileBrowseButton";
            this.batchFileBrowseButton.Size = new System.Drawing.Size(62, 19);
            this.batchFileBrowseButton.TabIndex = 2;
            this.batchFileBrowseButton.Text = "Browse";
            this.batchFileBrowseButton.UseVisualStyleBackColor = true;
            this.batchFileBrowseButton.Click += new System.EventHandler(this.batchFileBrowseButton_Click);
            // 
            // loadBatchButton
            // 
            this.loadBatchButton.Location = new System.Drawing.Point(318, 31);
            this.loadBatchButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadBatchButton.Name = "loadBatchButton";
            this.loadBatchButton.Size = new System.Drawing.Size(64, 19);
            this.loadBatchButton.TabIndex = 3;
            this.loadBatchButton.Text = "Load";
            this.loadBatchButton.UseVisualStyleBackColor = true;
            this.loadBatchButton.Click += new System.EventHandler(this.loadBatchButton_Click);
            // 
            // runBatchButton
            // 
            this.runBatchButton.Location = new System.Drawing.Point(318, 54);
            this.runBatchButton.Margin = new System.Windows.Forms.Padding(2);
            this.runBatchButton.Name = "runBatchButton";
            this.runBatchButton.Size = new System.Drawing.Size(64, 19);
            this.runBatchButton.TabIndex = 4;
            this.runBatchButton.Text = "Start";
            this.runBatchButton.UseVisualStyleBackColor = true;
            this.runBatchButton.Click += new System.EventHandler(this.runBatchButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 184);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Status:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 402);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "DDD Time:";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTextBox.Location = new System.Drawing.Point(276, 399);
            this.timeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(107, 20);
            this.timeTextBox.TabIndex = 8;
            // 
            // statusRichTextBox
            // 
            this.statusRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusRichTextBox.Location = new System.Drawing.Point(3, 201);
            this.statusRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.Size = new System.Drawing.Size(380, 195);
            this.statusRichTextBox.TabIndex = 9;
            this.statusRichTextBox.Text = "";
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // dddExecutablePathTextBox
            // 
            this.dddExecutablePathTextBox.Location = new System.Drawing.Point(61, 55);
            this.dddExecutablePathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.dddExecutablePathTextBox.Name = "dddExecutablePathTextBox";
            this.dddExecutablePathTextBox.ReadOnly = true;
            this.dddExecutablePathTextBox.Size = new System.Drawing.Size(187, 20);
            this.dddExecutablePathTextBox.TabIndex = 10;
            // 
            // dddExecutableBrowseButton
            // 
            this.dddExecutableBrowseButton.Location = new System.Drawing.Point(251, 55);
            this.dddExecutableBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.dddExecutableBrowseButton.Name = "dddExecutableBrowseButton";
            this.dddExecutableBrowseButton.Size = new System.Drawing.Size(62, 19);
            this.dddExecutableBrowseButton.TabIndex = 11;
            this.dddExecutableBrowseButton.Text = "Browse";
            this.dddExecutableBrowseButton.UseVisualStyleBackColor = true;
            this.dddExecutableBrowseButton.Click += new System.EventHandler(this.dddExecutableBrowseButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "DDD Path:";
            // 
            // newBatchButton
            // 
            this.newBatchButton.Location = new System.Drawing.Point(3, 3);
            this.newBatchButton.Name = "newBatchButton";
            this.newBatchButton.Size = new System.Drawing.Size(131, 23);
            this.newBatchButton.TabIndex = 13;
            this.newBatchButton.Text = "Edit Batch";
            this.newBatchButton.UseVisualStyleBackColor = true;
            this.newBatchButton.Click += new System.EventHandler(this.newBatchButton_Click);
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(3, 146);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 15;
            this.goButton.Text = "Run!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Simulation Run ID:";
            // 
            // dataTagTextBox
            // 
            this.dataTagTextBox.Location = new System.Drawing.Point(99, 106);
            this.dataTagTextBox.Name = "dataTagTextBox";
            this.dataTagTextBox.Size = new System.Drawing.Size(149, 20);
            this.dataTagTextBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Log Path:";
            // 
            // logPathTextBox
            // 
            this.logPathTextBox.Location = new System.Drawing.Point(61, 77);
            this.logPathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.logPathTextBox.Name = "logPathTextBox";
            this.logPathTextBox.ReadOnly = true;
            this.logPathTextBox.Size = new System.Drawing.Size(187, 20);
            this.logPathTextBox.TabIndex = 19;
            // 
            // logPathBrowseButton
            // 
            this.logPathBrowseButton.Location = new System.Drawing.Point(252, 77);
            this.logPathBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.logPathBrowseButton.Name = "logPathBrowseButton";
            this.logPathBrowseButton.Size = new System.Drawing.Size(62, 19);
            this.logPathBrowseButton.TabIndex = 20;
            this.logPathBrowseButton.Text = "Browse";
            this.logPathBrowseButton.UseVisualStyleBackColor = true;
            this.logPathBrowseButton.Click += new System.EventHandler(this.logPathBrowseButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 421);
            this.Controls.Add(this.logPathBrowseButton);
            this.Controls.Add(this.logPathTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataTagTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.newBatchButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dddExecutableBrowseButton);
            this.Controls.Add(this.dddExecutablePathTextBox);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.runBatchButton);
            this.Controls.Add(this.loadBatchButton);
            this.Controls.Add(this.batchFileBrowseButton);
            this.Controls.Add(this.batchFilePathTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainWindow";
            this.Text = "DDD Batch Runner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox batchFilePathTextBox;
        private System.Windows.Forms.Button batchFileBrowseButton;
        private System.Windows.Forms.Button loadBatchButton;
        private System.Windows.Forms.Button runBatchButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.TextBox dddExecutablePathTextBox;
        private System.Windows.Forms.Button dddExecutableBrowseButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button newBatchButton;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dataTagTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox logPathTextBox;
        private System.Windows.Forms.Button logPathBrowseButton;
    }
}

