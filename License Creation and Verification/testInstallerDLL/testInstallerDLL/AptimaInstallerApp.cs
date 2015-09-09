using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace testInstallerDLL
{
    public static class AptimaInstallerApp
    {
        public static AptimaLicenseInfo Main(string licenseKey)
        {
            if (licenseKey == string.Empty)
                return new AptimaLicenseInfo("Missing License Key"); ;

            AptimaLicenseInfo lic = AptimaLicenseVerifier.VerifyLicenseKey(licenseKey);

            if (lic.IsValid == false)
                return lic;

            DateTime expDate = ConvertStringToDateTime(lic.LicenseExpirString);
            DateTime today = DateTime.Now;
            if (expDate < today)
            { //license is expired
                return new AptimaLicenseInfo("ERROR: License key is expired.  Please contact Aptima to renew your key");
            }
            if (lic.ProductName != "_DDD")
            {
                return new AptimaLicenseInfo(String.Format("ERROR: Product license key is not valid.  Product code: {0}", lic.ProductName));
            }

            RegistryKey rk;
            List<string> keys;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Aptima").OpenSubKey("Asim").OpenSubKey("DDD", true);
            }
            catch
            {
                keys = new List<string>(Registry.LocalMachine.GetSubKeyNames());
                if (!keys.Contains("SOFTWARE"))
                {
                    rk = Registry.LocalMachine;
                    rk.CreateSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                keys = new List<string>(rk.GetSubKeyNames());
                if (!keys.Contains("Aptima"))
                {
                    rk.CreateSubKey("Aptima", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                rk = rk.OpenSubKey("Aptima", true);
                keys = new List<string>(rk.GetSubKeyNames());
                if (!keys.Contains("Asim"))
                {
                    rk.CreateSubKey("Asim", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                rk = rk.OpenSubKey("Asim", true);
                keys = new List<string>(rk.GetSubKeyNames());
                if (!keys.Contains("DDD"))
                {
                    rk.CreateSubKey("DDD", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                System.Security.AccessControl.RegistrySecurity rs = new System.Security.AccessControl.RegistrySecurity();
                rs.SetAccessRuleProtection(false, false);
                rk.SetAccessControl(rs);
                rk = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Aptima").OpenSubKey("Asim").OpenSubKey("DDD", true);
            }


            rk.SetValue("LicenseKey", licenseKey, RegistryValueKind.String);
            //System.Security.AccessControl.RegistrySecurity rss = new System.Security.AccessControl.RegistrySecurity();
            ////System.Security.AccessControl.RegistryAccessRule rar = new System.Security.AccessControl.RegistryAccessRule("DDDServer", System.Security.AccessControl.RegistryRights.FullControl, System.Security.AccessControl.AccessControlType.Allow);
            ////rss.SetAccessRule(rar);
            //rss.SetAccessRuleProtection(false, false);
            //rk.SetAccessControl(rss);
            rk.SetValue("InstallDate", String.Format("{0:yyyyMMdd}", DateTime.Now), RegistryValueKind.String);
            rk.SetValue("LatestLogin", String.Format("{0:yyyyMMdd}", DateTime.Now), RegistryValueKind.String);
            string keyType = lic.LicenseKeyTypeString.ToUpper();
            rk.SetValue("LicenseType", keyType);
            rk.Close();


            return lic;

        }
        private static DateTime ConvertStringToDateTime(string dateTimeString)
        {
            if (dateTimeString.Length != 8)
            {
                throw new Exception("Error converting date time string to date time");
            }
            string year = dateTimeString.Substring(0, 4);
            dateTimeString = dateTimeString.Remove(0, 4);
            string month = dateTimeString.Substring(0, 2);
            dateTimeString = dateTimeString.Remove(0, 2);
            string date = dateTimeString.Substring(0, 2);

            int iYear = Convert.ToInt32(year);
            int iMonth = Convert.ToInt32(month);
            int iDay = Convert.ToInt32(date);

            return new DateTime(iYear, iMonth, iDay);
        }
    }
}
