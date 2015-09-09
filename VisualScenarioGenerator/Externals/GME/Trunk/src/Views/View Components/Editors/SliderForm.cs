using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace AME.Views.View_Components.Editors
{
    public class SliderForm : Form
    {
        public IWindowsFormsEditorService wfes;
        public TrackBar trackBar;
        private Button trackButton;

        private Container components = null;

        public SliderForm()
        {
            InitializeComponent();
            TopLevel = false;
            trackButton.Text = trackBar.Value.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.trackButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.LargeChange = 10;
            this.trackBar.Location = new System.Drawing.Point(12, 12);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(152, 42);
            this.trackBar.TabIndex = 0;
            this.trackBar.TickFrequency = 10;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackButton
            // 
            this.trackButton.AutoSize = true;
            this.trackButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.trackButton.Location = new System.Drawing.Point(170, 21);
            this.trackButton.Name = "trackButton";
            this.trackButton.Size = new System.Drawing.Size(33, 23);
            this.trackButton.TabIndex = 2;
            this.trackButton.Text = "0";
            this.trackButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trackButton.Click += new System.EventHandler(this.trackButton_Click);
            // 
            // SliderForm
            // 
            this.AcceptButton = this.trackButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(215, 66);
            this.ControlBox = false;
            this.Controls.Add(this.trackButton);
            this.Controls.Add(this.trackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SliderForm";
            this.ShowInTaskbar = false;
            this.Closed += new System.EventHandler(this.constrastForm_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            trackButton.Text = trackBar.Value.ToString();
        }

        private void trackButton_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void constrastForm_Closed(object sender, EventArgs e)
        {
            wfes.CloseDropDown();
        }
    }
}
        
    


