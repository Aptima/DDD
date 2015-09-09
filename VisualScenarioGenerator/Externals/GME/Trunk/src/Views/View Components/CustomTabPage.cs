using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AME.Views.View_Components
{
    [System.ComponentModel.ToolboxItemAttribute(true)]
    //[System.ComponentModel.Designer(typeof(System.Windows.Forms.Design.ScrollableControlDesigner))]
    public partial class CustomTabPage : TabPage
    {       
        public CustomTabPage()
        {
            InitializeComponent();

            label1.Dock = DockStyle.Top;
            this.label1.Text = label1.Name;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        public CustomTabPage(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            label1.Dock = DockStyle.Top;
            this.label1.Text = label1.Name;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Description.")]
        [DefaultValue("description")]
        public String Description
        {
            get
            {
                return label1.Text;
            }

            set
            {
                DrawingUtility.SuspendDrawing(label1);
                label1.Text = value;
                label1.Dock = DockStyle.Top;
                DrawingUtility.ResumeDrawing(label1);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (!this.Controls.Contains(label1))
                this.Controls.Add(label1);
            //if (e.Control.Dock.Equals(DockStyle.Fill))
            //{
                e.Control.BringToFront();
            //    e.Control.Invalidate(true);
            //    e.Control.PerformLayout();
            //}
        }
    }
}
