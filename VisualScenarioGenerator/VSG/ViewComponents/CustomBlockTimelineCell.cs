using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;

using AME.Controllers;

namespace VSG.ViewComponents
{
    public partial class CustomBlockTimelineCell : UserControl
    {
        public int Order
        {
            set
            {
                if (value >= 0)
                {
                    label1.Text = value.ToString();
                }
                else
                {
                    label1.Text = "?";
                }
            }
            get
            {
                if (label1.Text != "*")
                {
                    return Int32.Parse(label1.Text);
                }
                else
                {
                    return -1;
                }
            }
        }
        new public string Text
        {
            set
            {
                label2.Text = value;
            }
            get
            {
                return label2.Text;
            }
        }

        public bool IsSelected
        {
            get
            {
                return (this.BorderStyle == BorderStyle.FixedSingle);
            }
            set
            {
                switch (value)
                {
                    case true:
                        BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case false:
                        BorderStyle = BorderStyle.None;
                        break;
                }
            }
        }

        public object Payload = null;

        public CustomBlockTimelineCell()
        {
            InitializeComponent();
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }
    }
}
