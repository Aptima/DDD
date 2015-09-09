using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.TestStubs.TextChatTestGUI
{
    static class Program
    {
        private static string simModelName;
        public static string SimModelName
        {
            set { simModelName = value; }
        }
        private static string hostName = null;
        public static string HostName
        {
            set { hostName = value; }
        }
        private static string portNumber;
        public static string PortNumber
        {
            set { portNumber = value; }
        }
        private static string user;
        public static string User
        {
            set { user = value; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            if(hostName != null)
                Application.Run(new Form1(simModelName, hostName, portNumber, user));
            
        }
    }
}