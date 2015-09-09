using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aptima.Asim.DDD.DDDAgentFramework;
using Aptima.Asim.DDD.DDDAgentFramework.UIHelpers;
namespace BatchRunner
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
            //DDDServerConnection ddd = new DDDServerConnection();
            //ServerConnectDialog serverConnect = new ServerConnectDialog(ref ddd);
            //DialogResult r = serverConnect.ShowDialog();
            //if (r != DialogResult.OK)
            //{
            //    return;
            //}
            
            Application.Run(new MainWindow());
        }
    }
}
