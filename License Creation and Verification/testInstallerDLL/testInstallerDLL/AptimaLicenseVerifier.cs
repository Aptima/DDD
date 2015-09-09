using System;
using System.Collections.Generic;
using System.Text;

namespace testInstallerDLL
{
    public enum LicenseKeyTypes
    { 
        NONE = -1,
        Eval = 0,
        Beta = 1,
        Full = 2,
    }

    public class AptimaLicenseInfo
    {
        private string _productName;
        private int _majorVersion;
        private int _minorVersion;
        private LicenseKeyTypes _licenseKeyType;
        private int _numberOfSeats;
        private string _licenseExpirString;
        private bool _isValid;
        private string _randomUniqueID;
        private string _errorMessage;

        #region GetMethods
        public bool IsValid
        {
            get { return _isValid; }
        }
        public string ProductName
        {
            get { return _productName; }
        }
        public string LicenseExpirString
        {
            get { return _licenseExpirString; }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }
        public string RandomUniqueID
        {
            get { return _randomUniqueID; }
        }
        public int MajorVersion
        {
            get { return _majorVersion; }
        }
        public int MinorVersion
        {
            get { return _minorVersion; }
        }
        public int NumberOfSeats
        {
            get { return _numberOfSeats; }
        }
        public string LicenseKeyTypeString
        {
            get
            {
                string retStr = string.Empty;
                switch (_licenseKeyType)
                { 
                    case LicenseKeyTypes.Beta:
                        retStr = "Beta";
                        break;
                    case LicenseKeyTypes.Eval:
                        retStr = "Eval";
                        break;
                    case LicenseKeyTypes.Full:
                        retStr = "Full";
                        break;
                    default:
                        retStr = "Erroneous";
                        break;
                }
                return retStr;
            }
        }
        #endregion

        /// <summary>
        /// Returns a data structure for a valid license key
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        /// <param name="licenseKeyType"></param>
        /// <param name="numberOfSeats"></param>
        /// <param name="licenseExpirString"></param>
        /// <param name="randomUniqueID"></param>
        public AptimaLicenseInfo(string productName, int majorVersion, int minorVersion, LicenseKeyTypes licenseKeyType, int numberOfSeats, string licenseExpirString, string randomUniqueID)
        {
            _productName = productName;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
            _licenseKeyType = licenseKeyType;
            _numberOfSeats = numberOfSeats;
            _licenseExpirString = licenseExpirString;
            _randomUniqueID = randomUniqueID;
            _errorMessage = string.Empty;
            _isValid = true;
        }
        /// <summary>
        /// Returns a data structure for an invalid license key
        /// </summary>
        /// <param name="errorMessage"></param>
        public AptimaLicenseInfo(string errorMessage)
        {
            _isValid = false;
            _productName = string.Empty;
            _majorVersion = -1;
            _minorVersion = -1;
            _licenseKeyType = LicenseKeyTypes.NONE;
            _numberOfSeats = -1;
            _licenseExpirString = string.Empty;
            _randomUniqueID = string.Empty;
            _errorMessage = errorMessage;
        }
    }

    public static class AptimaLicenseVerifier
    {
        /// <summary>
        /// Must be 24 bytes long, exactly.
        /// </summary>
        private static string encryptionKey = "AptimaSoftwareInternal07";
        private static int validInputStringLength = 23;

        public static AptimaLicenseInfo VerifyLicenseKey(string licenseKey)
        {
            AptimaLicenseInfo license = AptimaEncryptor.DecryptKey(licenseKey, encryptionKey, validInputStringLength);

            return license;
        }

        public static string GenerateLicenseKey(string inputString)
        {
            string licenseKey = AptimaEncryptor.EncryptKey(inputString, encryptionKey);
            return licenseKey;
        }
    }
}
