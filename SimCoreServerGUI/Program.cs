using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;

namespace Aptima.Asim.DDD.SimCoreGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 frm = new Form1();
                if (frm.IsDisposed)
                    return;
                Application.Run(frm);
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error '" + exc.Message + "' has occurred in the Simulation Server.\nPlease email the C:\\DDDErrorLog.txt file to Aptima customer support with a description of what you were doing at the time of the error.");
                ErrorLog.Write(exc.ToString() + "\n");

            }
            
        }
    }
}