namespace _4_to_4_1Converter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.transformBtn = new System.Windows.Forms.Button();
            this.outputXMLBrowseBtn = new System.Windows.Forms.Button();
            this.inputXMLBrowseBtn = new System.Windows.Forms.Button();
            this.outputXMLTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputXMLTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearBtn);
            this.groupBox1.Controls.Add(this.cancelBtn);
            this.groupBox1.Controls.Add(this.transformBtn);
            this.groupBox1.Controls.Add(this.outputXMLBrowseBtn);
            this.groupBox1.Controls.Add(this.inputXMLBrowseBtn);
            this.groupBox1.Controls.Add(this.outputXMLTxtBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.inputXMLTxtBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 170);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Please Select/Specify Details below";
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(9, 138);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(75, 23);
            this.clearBtn.TabIndex = 9;
            this.clearBtn.Text = "Clear All";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(311, 138);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // transformBtn
            // 
            this.transformBtn.Location = new System.Drawing.Point(230, 138);
            this.transformBtn.Name = "transformBtn";
            this.transformBtn.Size = new System.Drawing.Size(75, 23);
            this.transformBtn.TabIndex = 7;
            this.transformBtn.Text = "Transform";
            this.transformBtn.UseVisualStyleBackColor = true;
            this.transformBtn.Click += new System.EventHandler(this.transformBtn_Click);
            // 
            // outputXMLBrowseBtn
            // 
            this.outputXMLBrowseBtn.Location = new System.Drawing.Point(322, 94);
            this.outputXMLBrowseBtn.Name = "outputXMLBrowseBtn";
            this.outputXMLBrowseBtn.Size = new System.Drawing.Size(64, 21);
            this.outputXMLBrowseBtn.TabIndex = 6;
            this.outputXMLBrowseBtn.Text = "Browse";
            this.outputXMLBrowseBtn.UseVisualStyleBackColor = true;
            this.outputXMLBrowseBtn.Click += new System.EventHandler(this.outputXMLBrowseBtn_Click);
            // 
            // inputXMLBrowseBtn
            // 
            this.inputXMLBrowseBtn.Location = new System.Drawing.Point(322, 48);
            this.inputXMLBrowseBtn.Name = "inputXMLBrowseBtn";
            this.inputXMLBrowseBtn.Size = new System.Drawing.Size(64, 20);
            this.inputXMLBrowseBtn.TabIndex = 2;
            this.inputXMLBrowseBtn.Text = "Browse";
            this.inputXMLBrowseBtn.UseVisualStyleBackColor = true;
            this.inputXMLBrowseBtn.Click += new System.EventHandler(this.inputXMLBrowseBtn_Click);
            // 
            // outputXMLTxtBox
            // 
            this.outputXMLTxtBox.Location = new System.Drawing.Point(9, 95);
            this.outputXMLTxtBox.Name = "outputXMLTxtBox";
            this.outputXMLTxtBox.Size = new System.Drawing.Size(312, 20);
            this.outputXMLTxtBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Specify name of output file";
            // 
            // inputXMLTxtBox
            // 
            this.inputXMLTxtBox.Enabled = false;
            this.inputXMLTxtBox.Location = new System.Drawing.Point(9, 48);
            this.inputXMLTxtBox.Name = "inputXMLTxtBox";
            this.inputXMLTxtBox.Size = new System.Drawing.Size(312, 20);
            this.inputXMLTxtBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select XML file  to be transformed :";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 183);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(416, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 205);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "DDD Scenario Converter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox inputXMLTxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputXMLTxtBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button inputXMLBrowseBtn;
        private System.Windows.Forms.Button outputXMLBrowseBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button transformBtn;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button clearBtn;
    }
}

