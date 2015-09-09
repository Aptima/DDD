namespace Forms
{
    partial class InputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bottom_input_label = new System.Windows.Forms.Label();
            this.bottom_input_textbox = new System.Windows.Forms.TextBox();
            this.top_input_textbox = new System.Windows.Forms.TextBox();
            this.top_input_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(153, 171);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(234, 171);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bottom_input_label);
            this.groupBox1.Controls.Add(this.bottom_input_textbox);
            this.groupBox1.Controls.Add(this.top_input_textbox);
            this.groupBox1.Controls.Add(this.top_input_label);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(297, 150);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter Resource Information";
            // 
            // bottom_input_label
            // 
            this.bottom_input_label.AutoSize = true;
            this.bottom_input_label.Location = new System.Drawing.Point(9, 60);
            this.bottom_input_label.Margin = new System.Windows.Forms.Padding(3);
            this.bottom_input_label.Name = "bottom_input_label";
            this.bottom_input_label.Size = new System.Drawing.Size(66, 13);
            this.bottom_input_label.TabIndex = 14;
            this.bottom_input_label.Text = "Description: ";
            // 
            // bottom_input_textbox
            // 
            this.bottom_input_textbox.Location = new System.Drawing.Point(81, 57);
            this.bottom_input_textbox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.bottom_input_textbox.Multiline = true;
            this.bottom_input_textbox.Name = "bottom_input_textbox";
            this.bottom_input_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bottom_input_textbox.Size = new System.Drawing.Size(207, 80);
            this.bottom_input_textbox.TabIndex = 13;
            // 
            // top_input_textbox
            // 
            this.top_input_textbox.Location = new System.Drawing.Point(81, 22);
            this.top_input_textbox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.top_input_textbox.Name = "top_input_textbox";
            this.top_input_textbox.Size = new System.Drawing.Size(207, 20);
            this.top_input_textbox.TabIndex = 10;
            // 
            // top_input_label
            // 
            this.top_input_label.AutoSize = true;
            this.top_input_label.Location = new System.Drawing.Point(34, 25);
            this.top_input_label.Margin = new System.Windows.Forms.Padding(3);
            this.top_input_label.Name = "top_input_label";
            this.top_input_label.Size = new System.Drawing.Size(41, 13);
            this.top_input_label.TabIndex = 9;
            this.top_input_label.Text = "Name: ";
            // 
            // InputForm
            // 
            this.AcceptButton = this.buttonAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(319, 203);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(260, 50);
            this.Name = "InputForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add Resource";
            this.Activated += new System.EventHandler(this.InputFormActivated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label bottom_input_label;
        private System.Windows.Forms.TextBox bottom_input_textbox;
        private System.Windows.Forms.TextBox top_input_textbox;
        private System.Windows.Forms.Label top_input_label;
    }
}
