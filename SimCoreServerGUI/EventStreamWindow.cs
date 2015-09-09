using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    public partial class EventStreamWindow : Form
    {
        public EventStreamWindow()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // textBox1.
            textBox1.Text = ((Form1)Owner).GetEventStream();
        }

        ~EventStreamWindow()
        {
            timer1.Stop();
        }
    }
}