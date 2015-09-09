using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator
{
    public partial class Decimal : UserControl
    {
        private TextBox txValue;
    
        public double Value
        {
            get { return double.Parse(txValue.Text); }
            set { txValue.Text = value.ToString(); }
        }
        public Decimal()
        {
            InitializeComponent();
        }

        private void txValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            char c = e.KeyChar;
            if (
                (!(Char.IsDigit(c) || ('.' == c)||('-'==c)))
              || (('.' == c) && (txValue.Text.IndexOf('.') > -1))
              || (('-' == c) && (txValue.Text.IndexOf('-') > -1))
               )
            {
                Console.Beep();
                e.Handled = true;
            }
            else if ((txValue.Text != ".")&&(txValue.Text!="-"))
            {
                try
                {
                    Double.Parse(txValue.Text);
                }
                catch
                {
                    Console.Beep();
                    e.Handled = true;
                }
            }
        }

        private void InitializeComponent()
        {
            this.txValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txValue
            // 
            this.txValue.AcceptsReturn = true;
            this.txValue.Location = new System.Drawing.Point(0, 1);
            this.txValue.Name = "txValue";
            this.txValue.Size = new System.Drawing.Size(53, 20);
            this.txValue.TabIndex = 1;
            this.txValue.Text = "0.0";
            this.txValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txValue_KeyPress);
            // 
            // Decimal
            // 
            this.Controls.Add(this.txValue);
            this.Name = "Decimal";
            this.Size = new System.Drawing.Size(53, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}
