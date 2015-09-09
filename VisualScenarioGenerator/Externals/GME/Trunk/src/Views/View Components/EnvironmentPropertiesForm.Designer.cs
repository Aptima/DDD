namespace AME.Views.View_Components
{
    partial class EnvironmentPropertiesForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxImageLocation = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxNorthing = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxEasting = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxYPixel = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxYRotation = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxXRotation = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxXPixel = new AME.Views.View_Components.CustomParameterTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxUseTransformations = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonBrowse);
            this.groupBox1.Controls.Add(this.textBoxImageLocation);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(12);
            this.groupBox1.Size = new System.Drawing.Size(268, 64);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AutoSize = true;
            this.buttonBrowse.Location = new System.Drawing.Point(227, 26);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(26, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxImageLocation
            // 
            this.textBoxImageLocation.ComponentId = 0;
            this.textBoxImageLocation.Controller = null;
            this.textBoxImageLocation.Location = new System.Drawing.Point(15, 28);
            this.textBoxImageLocation.Name = "textBoxImageLocation";
            this.textBoxImageLocation.ParameterCategory = "Image";
            this.textBoxImageLocation.ParameterName = "Image location";
            this.textBoxImageLocation.ReadOnly = true;
            this.textBoxImageLocation.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxImageLocation.Size = new System.Drawing.Size(206, 20);
            this.textBoxImageLocation.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxUseTransformations);
            this.groupBox2.Controls.Add(this.textBoxNorthing);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxEasting);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxYPixel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxYRotation);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxXRotation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxXPixel);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 82);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(12);
            this.groupBox2.Size = new System.Drawing.Size(268, 210);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Affine Transformation";
            // 
            // textBoxNorthing
            // 
            this.textBoxNorthing.ComponentId = 0;
            this.textBoxNorthing.Controller = null;
            this.textBoxNorthing.Location = new System.Drawing.Point(102, 152);
            this.textBoxNorthing.Name = "textBoxNorthing";
            this.textBoxNorthing.ParameterCategory = "Parameters";
            this.textBoxNorthing.ParameterName = "Northing";
            this.textBoxNorthing.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxNorthing.Size = new System.Drawing.Size(151, 20);
            this.textBoxNorthing.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Northing: ";
            // 
            // textBoxEasting
            // 
            this.textBoxEasting.ComponentId = 0;
            this.textBoxEasting.Controller = null;
            this.textBoxEasting.Location = new System.Drawing.Point(102, 126);
            this.textBoxEasting.Name = "textBoxEasting";
            this.textBoxEasting.ParameterCategory = "Parameters";
            this.textBoxEasting.ParameterName = "Easting";
            this.textBoxEasting.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxEasting.Size = new System.Drawing.Size(151, 20);
            this.textBoxEasting.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Easting: ";
            // 
            // textBoxYPixel
            // 
            this.textBoxYPixel.ComponentId = 0;
            this.textBoxYPixel.Controller = null;
            this.textBoxYPixel.Location = new System.Drawing.Point(102, 100);
            this.textBoxYPixel.Name = "textBoxYPixel";
            this.textBoxYPixel.ParameterCategory = "Parameters";
            this.textBoxYPixel.ParameterName = "Y pixel / meter";
            this.textBoxYPixel.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxYPixel.Size = new System.Drawing.Size(151, 20);
            this.textBoxYPixel.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Y pixel / meter: ";
            // 
            // textBoxYRotation
            // 
            this.textBoxYRotation.ComponentId = 0;
            this.textBoxYRotation.Controller = null;
            this.textBoxYRotation.Location = new System.Drawing.Point(102, 74);
            this.textBoxYRotation.Name = "textBoxYRotation";
            this.textBoxYRotation.ParameterCategory = "Parameters";
            this.textBoxYRotation.ParameterName = "Y rotation";
            this.textBoxYRotation.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxYRotation.Size = new System.Drawing.Size(151, 20);
            this.textBoxYRotation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y rotation: ";
            // 
            // textBoxXRotation
            // 
            this.textBoxXRotation.ComponentId = 0;
            this.textBoxXRotation.Controller = null;
            this.textBoxXRotation.Location = new System.Drawing.Point(102, 48);
            this.textBoxXRotation.Name = "textBoxXRotation";
            this.textBoxXRotation.ParameterCategory = "Parameters";
            this.textBoxXRotation.ParameterName = "X rotation";
            this.textBoxXRotation.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxXRotation.Size = new System.Drawing.Size(151, 20);
            this.textBoxXRotation.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "X rotation: ";
            // 
            // textBoxXPixel
            // 
            this.textBoxXPixel.ComponentId = 0;
            this.textBoxXPixel.Controller = null;
            this.textBoxXPixel.Location = new System.Drawing.Point(102, 22);
            this.textBoxXPixel.Name = "textBoxXPixel";
            this.textBoxXPixel.ParameterCategory = "Parameters";
            this.textBoxXPixel.ParameterName = "X pixel / meter";
            this.textBoxXPixel.ParameterType = AME.Controllers.eParamParentType.Component;
            this.textBoxXPixel.Size = new System.Drawing.Size(151, 20);
            this.textBoxXPixel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X pixel / meter: ";
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(205, 307);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Map Files(*.jpg; *.jpeg; *.gif; *.bmp; *.tif; *.tiff; *.png)|*.jpg; *.jpeg; *.gif" +
                "; *.bmp; *.tif; *.tiff; *.png";
            // 
            // checkBoxUseTransformations
            // 
            this.checkBoxUseTransformations.AutoSize = true;
            this.checkBoxUseTransformations.Location = new System.Drawing.Point(102, 178);
            this.checkBoxUseTransformations.Name = "checkBoxUseTransformations";
            this.checkBoxUseTransformations.Size = new System.Drawing.Size(133, 17);
            this.checkBoxUseTransformations.TabIndex = 12;
            this.checkBoxUseTransformations.Text = "Enable transformations";
            this.checkBoxUseTransformations.UseVisualStyleBackColor = true;
            this.checkBoxUseTransformations.CheckedChanged += new System.EventHandler(this.checkBoxUseTransformations_CheckedChanged);
            // 
            // EnvironmentPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 342);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnvironmentPropertiesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Environment Properties";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonBrowse;
        private CustomParameterTextBox textBoxImageLocation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private CustomParameterTextBox textBoxYRotation;
        private System.Windows.Forms.Label label3;
        private CustomParameterTextBox textBoxXRotation;
        private System.Windows.Forms.Label label2;
        private CustomParameterTextBox textBoxXPixel;
        private CustomParameterTextBox textBoxNorthing;
        private System.Windows.Forms.Label label6;
        private CustomParameterTextBox textBoxEasting;
        private System.Windows.Forms.Label label5;
        private CustomParameterTextBox textBoxYPixel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxUseTransformations;

    }
}