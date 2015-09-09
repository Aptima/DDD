using System;
using System.Collections.Generic;
using System.Text;
using Aptima.CommonComponent.AptimaLicenseVerifier;

namespace Aptima.CommonComponent
{
    public static class CommandLineKeyGen
    {
        public static string GenerateKey(string[] args)
        {
            string customerID = string.Empty;
            string productCode = string.Empty;
            string majorVersion = string.Empty;
            string minorVersion = string.Empty;
            string expirationDate = string.Empty;
            string licenseKeyType = string.Empty;
            string storedValue = string.Empty;

            try
            {
                customerID = args[0];
                productCode = args[1];
                majorVersion = args[2];
                minorVersion = args[3];
                expirationDate = args[4];
                licenseKeyType = args[5];
                storedValue = args[6];
            }
            catch
            {
                return "ERROR: Invalid command-line arguments";
            }

            if (majorVersion.Length == 1)
            {
                majorVersion = String.Format("0{0}", majorVersion);
            }
            if (minorVersion.Length == 1)
            {
                minorVersion = String.Format("0{0}", minorVersion);
            }

            if (customerID.Length != 4 ||
                productCode.Length != 4 ||
                majorVersion.Length != 2 ||
                minorVersion.Length != 2 ||
                expirationDate.Length != 8 ||
                licenseKeyType.Length != 1 ||
                storedValue.Length > 3)
            {
                return "ERROR: Invalid command-line arguments";
            }

            StringBuilder inputString = new StringBuilder();
            //[_DDD][04][01][2007][10][10][2][A8][RAND]

            inputString.Append(productCode);
            inputString.Append(majorVersion);
            inputString.Append(minorVersion);
            inputString.Append(expirationDate); //inputString.AppendFormat("{0:yyyyMMdd}", DateTime.Now);
            inputString.Append(licenseKeyType);
            inputString.Append(ConvertFromNumberToHexString(storedValue));
            inputString.Append(customerID);

            string _licenseKey = AptimaLicenseVerifier.AptimaLicenseVerifier.GenerateLicenseKey(inputString.ToString());

            if (_licenseKey.Length == 32)
                return _licenseKey;


            return "ERROR: Error Generating Key";
        }

        private static string ConvertFromNumberToHexString(string numString)
        {
            int numInt = Convert.ToInt32(numString);
            int remainder;
            int quotient;
            StringBuilder sb = new StringBuilder();
                quotient = Math.DivRem(numInt, 16, out remainder);
                if (quotient < 10)
                {
                    sb.Append(quotient);
                }
                else
                {
                    switch (quotient)
                    { 
                        case 10:
                            sb.Append("A");
                            break;
                        case 11:
                            sb.Append("B");
                            break;
                        case 12:
                            sb.Append("C");
                            break;
                        case 13:
                            sb.Append("D");
                            break;
                        case 14:
                            sb.Append("E");
                            break;
                        case 15:
                            sb.Append("F");
                            break;
                    }
                }
                if (remainder < 10)
                {
                    sb.Append(remainder);
                }
                else
                {
                    switch (remainder)
                    {
                        case 10:
                            sb.Append("A");
                            break;
                        case 11:
                            sb.Append("B");
                            break;
                        case 12:
                            sb.Append("C");
                            break;
                        case 13:
                            sb.Append("D");
                            break;
                        case 14:
                            sb.Append("E");
                            break;
                        case 15:
                            sb.Append("F");
                            break;
                    }
                }
            return sb.ToString();
        }

    }
}
