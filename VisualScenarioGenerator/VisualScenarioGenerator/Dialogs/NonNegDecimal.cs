using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator
{
    /// <summary>
    /// Captures a non-negative double value
    /// </summary>
    public partial class NonNegDecimal : UserControl
    {
        public double Value
        {
            get { return double.Parse(txValue.Text); }
            set { txValue.Text=  value.ToString(); }
        }
        public NonNegDecimal()
        {
            InitializeComponent();
        }

        private void txValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            char c = e.KeyChar;
            if(
                (!(Char.IsDigit(c)||('.'==c)))
              ||(('.'==c)&&(txValue.Text.IndexOf('.')>-1))
               )
            {
                Console.Beep();
                e.Handled=true;
            }
            else if(txValue.Text!=".")
            {
                try{
                  Double.Parse(txValue.Text);
                }
                catch
                {
      Console.Beep();
                e.Handled=true;
                }
            }
        }


    }
}
