namespace Aptima.Asim.DDD.SimCoreGUI
{
    partial class DialogSetLogOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSetLogOptions));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonLimited = new System.Windows.Forms.RadioButton();
            this.radioButtonDetailed = new System.Windows.Forms.RadioButton();
            this.labelLogPath = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonLimited);
            this.groupBox1.Controls.Add(this.radioButtonDetailed);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(97, 71);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log Types";
            // 
            // radioButtonLimited
            // 
            this.radioButtonLimited.AutoSize = true;
            this.radioButtonLimited.Checked = true;
            this.radioButtonLimited.Location = new System.Drawing.Point(7, 44);
            this.radioButtonLimited.Name = "radioButtonLimited";
            this.radioButtonLimited.Size = new System.Drawing.Size(79, 17);
            this.radioButtonLimited.TabIndex = 1;
            this.radioButtonLimited.TabStop = true;
            this.radioButtonLimited.Text = "Replay Log";
            this.radioButtonLimited.UseVisualStyleBackColor = true;
            // 
            // radioButtonDetailed
            // 
            this.radioButtonDetailed.AutoSize = true;
            this.radioButtonDetailed.Location = new System.Drawing.Point(7, 20);
            this.radioButtonDetailed.Name = "radioButtonDetailed";
            this.radioButtonDetailed.Size = new System.Drawing.Size(78, 17);
            this.radioButtonDetailed.TabIndex = 0;
            this.radioButtonDetailed.Text = "Debug Log";
            this.radioButtonDetailed.UseVisualStyleBackColor = true;
            // 
            // labelLogPath
            // 
            this.labelLogPath.AutoSize = true;
            this.labelLogPath.Location = new System.Drawing.Point(134, 9);
            this.labelLogPath.Name = "labelLogPath";
            this.labelLogPath.Size = new System.Drawing.Size(84, 13);
            this.labelLogPath.TabIndex = 1;
            this.labelLogPath.Text = "Event Log Path:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(449, 28);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(332, 61);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(93, 23);
            this.buttonAccept.TabIndex = 4;
            this.buttonAccept.Text = "OK";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(431, 61);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(137, 61);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(120, 23);
            this.buttonClear.TabIndex = 6;
            this.buttonClear.Text = "Clear Options";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Visible = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // DialogSetLogOptions
            // 
            this.AcceptButton = this.buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(542, 99);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelLogPath);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogSetLogOptions";
            this.Text = "Event Logging Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonLimited;
        private System.Windows.Forms.RadioButton radioButtonDetailed;
        private System.Windows.Forms.Label labelLogPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonClear;
    }
}