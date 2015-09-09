using System;
using System.Collections.Generic;
using System.Windows.Forms;  // WindowStub Version

namespace DDD.ScenarioController
{
    static class readParse
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       /// [STAThread]
        static void Main(string [] args)
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1(args));
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}