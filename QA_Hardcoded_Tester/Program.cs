using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QA_Hardcoded_Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String hostname = "";
            int port = 0;
            ServerInfo inf = new ServerInfo();
            if (inf.ShowDialog() == DialogResult.OK)
            {
                inf.GetHostnameAndPort(out hostname, out port);
                inf.Dispose();
                MainForm mainFrm = new MainForm(hostname, port);
                if (!mainFrm.initialize())
                {
                    mainFrm.Dispose();
                    return;
                }
                mainFrm.ShowDialog();
                //Application.Run(new MainForm(hostname, port));
                mainFrm.Dispose();
            }
            else
            { 
            
            }
        }
    }
}
