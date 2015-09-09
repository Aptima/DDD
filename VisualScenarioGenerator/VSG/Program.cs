using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VSG.ConfigFile;
namespace VSG
{
    static class Program
    {
        public const int _defaultEngagementDuration = 0;
        public const int _defaultTimeToAttack = 0;
        public const int _productMajorVersion = 4;
        public const int _productMinorVersion = 2;
        public const string _licenseKeySoftwareVersion = "4.2";
        public const int _productReleaseVersion = 4;
        public const string _productFamily = "Asim";
        public const string _productCode = "_VSG";
        public const string _productProductName = "VSG";
        public const string _productDisplayName = "Visual Scenario Generator";
        public const string _productSupportNumber = "866.461.7298";
        public const string _productEmail = "support@aptima.com";
        public const string _buildDate = "11/07/2011";
        public const string _moreInfo = "http://aptima.com/products/ddd/visual-scenario-generator";

        public static String GetProductVersionString()
        {
            return String.Format("version " + _licenseKeySoftwareVersion);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            string error = string.Empty;
            
            VSGConfig.ReadFile();

            Application.Run(new VSGForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           // Ignore all unhandled exceptions when exitting.
            if (!e.IsTerminating)
            {
                Exception exc = e.ExceptionObject as Exception;
                MessageBox.Show(exc.StackTrace, exc.Message);
            }
            Environment.Exit(1);
        }
    }
}