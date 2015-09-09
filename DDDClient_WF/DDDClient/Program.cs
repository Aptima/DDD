using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.Client
{
    static class Program
    {
        public static string Build_ID = "4.2";
        public static string Build_Date = "05/17/2011";
        public static string App_Name = "Aptima DDD 4.2 Client";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //Application.AddMessageFilter(new MessageFilter());
            Application.Run(new DDD_MainWinFormInterface());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Dialogs.FatalException f = new Aptima.Asim.DDD.Client.Dialogs.FatalException();
            f.Message = e.Exception.Message;
            f.ShowDialog();

            Environment.Exit(1);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exc = e.ExceptionObject as Exception;
            Dialogs.FatalException f = new Aptima.Asim.DDD.Client.Dialogs.FatalException();
            f.Message = exc.Message;
            f.ShowDialog();
            Environment.Exit(1);
        }

    }
}