using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace DDDEmailBridge
{
    public class ConfigFile
    {
        public string dddServerHostname;
        public int dddServerPortNumber;
        public string emailServerHostname;
        public int emailServerPortNumber;
        public bool emailServerUseSSL;
        public int emailCheckFrequency;
        public string emailUsername;
        public string emailPassword;
        
        public string attachmentBaseDirectory;
        public ConfigFile()
        {
            dddServerHostname = null;
            dddServerPortNumber = -1;
            emailServerHostname = null;
            emailServerPortNumber = 143;
            emailServerUseSSL = false;
            emailCheckFrequency = -1;
            emailUsername = null;
            emailPassword = null;
            attachmentBaseDirectory = null;
        }

        

        public void readFile(string fileName)
        {
            string[] delimiters = { "=" };
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                string line;

                line = sr.ReadLine();
                string[] parts;
                while (line != null)
                {
                    parts = line.Split(delimiters, StringSplitOptions.None);
                    switch (parts[0].Trim())
                    {
                        case "DDDServerHostname":
                            try
                            {
                                dddServerHostname = parts[1].Trim();
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "DDDServerPortNumber":
                            try
                            {
                                dddServerPortNumber = int.Parse(parts[1].Trim());
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailServerHostname":
                            try
                            {
                                emailServerHostname = parts[1].Trim();
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailServerPortNumber":
                            try
                            {
                                emailServerPortNumber = int.Parse(parts[1].Trim());
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailServerUseSSL":
                            try
                            {
                                emailServerUseSSL = bool.Parse(parts[1].Trim());
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailCheckFrequency":
                            try
                            {
                                emailCheckFrequency = int.Parse(parts[1].Trim());
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailUsername":
                            try
                            {
                                emailUsername = parts[1].Trim();
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "EmailPassword":
                            try
                            {
                                emailPassword = parts[1].Trim();
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        case "AttachmentBaseDirectory":
                            try
                            {
                                attachmentBaseDirectory = parts[1].Trim();
                            }
                            catch
                            {
                                throw new Exception(String.Format("Error reading config file: couldn't read option value in line \"{0}\"", line));
                            }
                            break;
                        default:
                            throw new Exception(String.Format("Error reading config file: \"{0}\" is invalid option", parts[0].Trim()));
                            
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            else
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" file doesn't exist",fileName));
            }
        }

        public void verifyConfig()
        {
            if (dddServerHostname == null)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "DDDServerHostname"));
            }
            if (dddServerPortNumber < 0)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "DDDServerPortNumber"));
            }
            if (emailServerHostname == null)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "EmailServerHostname"));
            }
            
            if (emailCheckFrequency < 0)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "EmailCheckFrequency"));
            }
            if (emailUsername == null)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "EmailUsername"));
            }
            if (emailPassword == null)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "EmailPassword"));
            }
            if (attachmentBaseDirectory == null)
            {
                throw new Exception(String.Format("Error reading config file: \"{0}\" option missing", "AttachmentBaseDirectory"));
            }
        }
    }

 
}
