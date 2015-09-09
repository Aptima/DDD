using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace DDD.ScenarioController
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            DebugLogger.SetDebugStyleFile();
            Coordinator threadMe = new Coordinator(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
;
        
                ThreadStart stub = new ThreadStart(threadMe.CoordinateNow);
                Thread stubThread = new Thread(stub);
                stubThread.Start();

            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Coordinator.TimerControl((int)Coordinator.TimerControls.Pause);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Coordinator.TimerControl((int)Coordinator.TimerControls.Resume);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Coordinator.TimerControl((int)Coordinator.TimerControls.Reset);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}