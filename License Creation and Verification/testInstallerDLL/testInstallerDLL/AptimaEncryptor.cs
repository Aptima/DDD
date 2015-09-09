using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace testInstallerDLL
{
    static class AptimaEncryptor
    {
        
        public static string EncryptKey(string inputString, string securityKey)
        {
            //Used encrypt/decrypt examples from http://www.codeproject.com/useritems/Cryptography.asp

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(inputString);

            keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static AptimaLicenseInfo DecryptKey(string licenseKey, string securityKey, int inputStringLength)
        {
            ////Decrypt

            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(licenseKey);
 
            keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            string outputString = UTF8Encoding.UTF8.GetString(resultArray);
            if (outputString.Length != inputStringLength)
            {//invalid output string! 
                return new AptimaLicenseInfo("Invalid license key! (Return string has incorrect length)");
            }

            ////populate

            return PopulateLicenseInfo(outputString);            
        }
        private static AptimaLicenseInfo PopulateLicenseInfo(string outputString)
        {
            string productName = outputString.Substring(0, 4);
            outputString = outputString.Remove(0, 4);
            int majorVersion = GetMajorVersionFromString(outputString.Substring(0,2));
            outputString = outputString.Remove(0, 2);
            int minorVersion = GetMinorVersionFromString(outputString.Substring(0, 2));
            outputString = outputString.Remove(0, 2);
            string licenseExpirString = outputString.Substring(0, 8);
            outputString = outputString.Remove(0, 8);
            LicenseKeyTypes licenseKeyType = GetLicenseKeyTypeFromString(outputString.Substring(0, 1));
            outputString = outputString.Remove(0, 1);
            int numberOfSeats = GetNumberOfSeatsFromString(outputString.Substring(0, 2));
            outputString = outputString.Remove(0, 2);
            string randomNumber = outputString.Substring(0, 4);
            outputString = outputString.Remove(0, 4);

            return new AptimaLicenseInfo(productName, majorVersion, minorVersion, licenseKeyType, numberOfSeats, licenseExpirString, randomNumber);
        }
        private static int GetMajorVersionFromString(string strMajorVersion)
        {
            return Convert.ToInt32(strMajorVersion);
        }
        private static int GetMinorVersionFromString(string strMinorVersion)
        {
            return Convert.ToInt32(strMinorVersion);
        }
        private static LicenseKeyTypes GetLicenseKeyTypeFromString(string strLicenseKeyType)
        {
            int intg = Convert.ToInt32(strLicenseKeyType);
            LicenseKeyTypes retVal = LicenseKeyTypes.NONE;
            switch (intg)
            { 
                case 0:
                    retVal = LicenseKeyTypes.Eval;
                    break;
                case 1:
                    retVal = LicenseKeyTypes.Beta;
                    break;

                case 2:
                    retVal = LicenseKeyTypes.Full;
                    break;
                default:
                    retVal = LicenseKeyTypes.NONE;
                    break;
            }

            return retVal;
        }
        private static int GetNumberOfSeatsFromString(string strNumberOfSeats)
        {
            //need to convert from hex \/
            return Convert.ToInt32(strNumberOfSeats, 16);
        }
    }
}
