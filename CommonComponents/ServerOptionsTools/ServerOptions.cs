using System;
using System.Collections.Generic;
using System.Text;
//using System.ComponentModel;
using System.Data;
//using System.Drawing;

using System.Windows.Forms;
using System.IO;

//using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
namespace Aptima.Asim.DDD.CommonComponents.ServerOptionsTools
{
    public class ServerOptions
    {
        private static string dddClientDir = String.Format("\\\\{0}\\{1}", System.Environment.MachineName, shareFolderName);
        private static string serverOptionsPath = String.Format("{0}\\ServerOptions.txt", Application.StartupPath);
        public static string DDDClientPath
        {
            get { return String.Format("{0}\\{1}", System.Environment.MachineName, shareFolderName); }
        }
        private static int portNumber = 9999;
        public static int PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }

        private static string shareFolderName = "DDDClient";
        public static string ShareFolderName
        {
            get { return shareFolderName; }
            set { shareFolderName = value; dddClientDir = String.Format("\\\\{0}\\{1}", System.Environment.MachineName, shareFolderName); }
        }

        private static string hostName = "";
        public static string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private static string simulationModelPath = "";
        public static string SimulationModelPath
        {
            get { return simulationModelPath; }
            set { simulationModelPath = value; }
        }

        private static string scenarioSchemaPath = "";
        public static string ScenarioSchemaPath
        {
            get { return scenarioSchemaPath; }
            set { scenarioSchemaPath = value; }
        }

        private static string eventLogDirectory = "";
        public static string EventLogDirectory
        {
            get { return eventLogDirectory; }
            set { eventLogDirectory = value; }
        }

        private static string defaultScenarioPath = "";
        public static string DefaultScenarioPath
        {
            get { return defaultScenarioPath; }
            set { defaultScenarioPath = value; }
        }

        private static bool useStrongPasswords = true;
        public static bool UseStrongPasswords
        {
            get { return useStrongPasswords; }
            set { useStrongPasswords = value; }
        }

        private static string eventLogType = "";
        public static string EventLogType
        {
            get { return eventLogType; }
            set { eventLogType = value; }
        }

        private static bool useObjectLog = false;
        public static bool UseObjectLog
        {
            get { return useObjectLog; }
            set { useObjectLog = value; }
        }

        private static bool usePerformanceLog = false;
        public static bool UsePerformanceLog
        {
            get { return usePerformanceLog; }
            set { usePerformanceLog = value; }
        }

        private static bool showScoreSummary = true;
        public static bool ShowScoreSummary
        {
            get { return showScoreSummary; }
            set { showScoreSummary = value; }
        }

        private static bool hlaExport = false;
        public static bool HLAExport
        {
            get { return hlaExport; }
            set { hlaExport = value; }
        }

        private static string hlaFederationExecutionName = "";
        public static string HLAFederationExecutionName
        {
            get { return hlaFederationExecutionName; }
            set { hlaFederationExecutionName = value; }
        }

        private static string hlaFederationFilePath = "";
        public static string HLAFederationFilePath
        {
            get { return hlaFederationFilePath; }
            set { hlaFederationFilePath = value; }
        }

        private static string hlaXMLFilePath = "";
        public static string HLAXMLFilePath
        {
            get { return hlaXMLFilePath; }
            set { hlaXMLFilePath = value; }
        }

        private static bool enableVoiceServer = false;
        public static bool EnableVoiceServer
        {
            get { return enableVoiceServer; }
            set { enableVoiceServer = value; }
        }

        private static string voiceServerHostname = "";
        public static string VoiceServerHostname
        {
            get { return voiceServerHostname; }
            set { voiceServerHostname = value; }
        }

        private static int voiceServerPort = 10300;
        public static int VoiceServerPort
        {
            get { return voiceServerPort; }
            set { voiceServerPort = value; }
        }

        private static string voiceServerAdminUsername = "";
        public static string VoiceServerAdminUsername
        {
            get { return voiceServerAdminUsername; }
            set { voiceServerAdminUsername = value; }
        }

        private static string voiceServerAdminPassword = "";
        public static string VoiceServerAdminPassword
        {
            get { return voiceServerAdminPassword; }
            set { voiceServerAdminPassword = value; }
        }

        private static string voiceServerUserPassword = "";
        public static string VoiceServerUserPassword
        {
            get { return voiceServerUserPassword; }
            set { voiceServerUserPassword = value; }
        }

        private static bool enableVoiceServerRecordings = false;
        public static bool EnableVoiceServerRecordings
        {
            get { return enableVoiceServerRecordings; }
            set { enableVoiceServerRecordings = value; }
        }

        private static string voiceServerAudioLogDir = "";
        public static string VoiceServerAudioLogDir
        {
            get { return voiceServerAudioLogDir; }
            set { voiceServerAudioLogDir = value; }
        }

        private static double forkReplaySpeed = 20;
        public static double ForkReplaySpeed
        {
            get { return forkReplaySpeed; }
            set { forkReplaySpeed = value; }
        }

        public static void WriteFile()
        {
            StreamWriter sw = new StreamWriter(serverOptionsPath);

            sw.WriteLine(string.Format("HostName; {0}", hostName));
            sw.WriteLine(string.Format("PortNumber; {0}", portNumber));
            sw.WriteLine(string.Format("SimulationModelPath; {0}", simulationModelPath));
            sw.WriteLine(string.Format("ScenarioSchemaPath; {0}", scenarioSchemaPath));
            sw.WriteLine(string.Format("EventLogDirectory; {0}", eventLogDirectory));
            sw.WriteLine(string.Format("EventLogType; {0}", eventLogType));
            sw.WriteLine(string.Format("DefaultScenarioPath; {0}", defaultScenarioPath));
            sw.WriteLine(string.Format("UseStrongPasswords; {0}", useStrongPasswords));
            sw.WriteLine(string.Format("UseObjectLog; {0}", useObjectLog));
            sw.WriteLine(string.Format("UsePerformanceLog; {0}", usePerformanceLog));
            sw.WriteLine(string.Format("ShowScoreSummary; {0}", showScoreSummary));
            sw.WriteLine(string.Format("HLAExport; {0}", hlaExport));
            sw.WriteLine(string.Format("HLAFederationExecutionName; {0}", hlaFederationExecutionName));
            sw.WriteLine(string.Format("HLAFederationFilePath; {0}", hlaFederationFilePath));
            sw.WriteLine(string.Format("HLA XML FilePath; {0}", hlaXMLFilePath));
            sw.WriteLine(string.Format("Enable Voice Server; {0}", enableVoiceServer));
            sw.WriteLine(string.Format("Voice Server Hostname; {0}", voiceServerHostname));
            sw.WriteLine(string.Format("Voice Server Port; {0}", voiceServerPort));
            sw.WriteLine(string.Format("Voice Server Admin Username; {0}", voiceServerAdminUsername));
            sw.WriteLine(string.Format("Voice Server Admin Password; {0}", voiceServerAdminPassword));
            sw.WriteLine(string.Format("Voice Server User Password; {0}", voiceServerUserPassword));
            sw.WriteLine(string.Format("Voice Server Record Channel Traffic; {0}", enableVoiceServerRecordings));
            sw.WriteLine(string.Format("Voice Server Audio Log Directory; {0}", voiceServerAudioLogDir));
            sw.WriteLine(string.Format("ForkReplaySpeed; {0}", ForkReplaySpeed));
            if(shareFolderName != "DDDClient")
                sw.WriteLine(string.Format("ClientSharePath; {0}", ShareFolderName));

            sw.Close();
        }

        public static void ReadFile()
        {
            string[] delimiters = { ";" };
            if (File.Exists(serverOptionsPath))
            {
                StreamReader sr = new StreamReader(serverOptionsPath);
                string currPath;

                currPath = sr.ReadLine();
                string[] parts;
                while (currPath != null)
                {
                    parts = currPath.Split(delimiters, StringSplitOptions.None);
                    switch (parts[0])
                    {
                        case "HostName":
                            hostName = parts[1].Trim();
                            break;
                        case "PortNumber":
                            portNumber = Int32.Parse(parts[1].Trim());
                            break;
                        case "SimulationModelPath":
                            simulationModelPath = parts[1].Trim(); ;
                            break;
                        case "ScenarioSchemaPath":
                            scenarioSchemaPath = parts[1].Trim();
                            break;
                        case "EventLogDirectory":
                            eventLogDirectory = parts[1].Trim();
                            break;
                        case "EventLogType":
                            eventLogType = parts[1].Trim();
                            break;
                        case "DefaultScenarioPath":
                            defaultScenarioPath = parts[1].Trim();
                            break;
                        case "UseStrongPasswords":
                            useStrongPasswords = Boolean.Parse(parts[1].Trim());
                            break;
                        case "UseObjectLog":
                            useObjectLog = Boolean.Parse(parts[1].Trim());
                            break;
                        case "UsePerformanceLog":
                            usePerformanceLog = Boolean.Parse(parts[1].Trim());
                            break;
                        case "ShowScoreSummary":
                            showScoreSummary = Boolean.Parse(parts[1].Trim());
                            break;
                        case "HLAExport":
                            hlaExport = Boolean.Parse(parts[1].Trim());
                            break;
                        case "HLAFederationExecutionName":
                            hlaFederationExecutionName = parts[1].Trim();
                            break;
                        case "HLAFederationFilePath":
                            hlaFederationFilePath = parts[1].Trim();
                            break;
                        case "HLA XML FilePath":
                            hlaXMLFilePath = parts[1].Trim();
                            break;
                        case "Enable Voice Server":
                            enableVoiceServer = Boolean.Parse(parts[1].Trim());
                            break;
                        case "Voice Server Hostname":
                            voiceServerHostname = parts[1].Trim();
                            break;
                        case "Voice Server Port":
                            try
                            {
                                voiceServerPort = Int32.Parse(parts[1].Trim());
                            }
                            catch (Exception ex)
                            {
                                VoiceServerPort = -1;
                            }
                            break;
                        case "Voice Server Admin Username":
                            voiceServerAdminUsername = parts[1].Trim();
                            break;
                        case "Voice Server Admin Password":
                            voiceServerAdminPassword = parts[1].Trim();
                            break;
                        case "Voice Server User Password":
                            voiceServerUserPassword = parts[1].Trim();
                            break;
                        case "Voice Server Record Channel Traffic":
                            enableVoiceServerRecordings = Boolean.Parse(parts[1].Trim());
                            break;
                        case "Voice Server Audio Log Directory":
                            voiceServerAudioLogDir = parts[1].Trim();
                            break;
                        case "ForkReplaySpeed":
                            ForkReplaySpeed = Convert.ToDouble(parts[1].Trim());
                            break;
                        case "ClientSharePath":
                            ShareFolderName = parts[1].Trim();
                            break;
                        default:
                            throw new Exception(String.Format("Incorrect option \"{0}\" in {0}", parts[0], serverOptionsPath));
                    }
                    currPath = sr.ReadLine();
                }
                sr.Close();
            }
            else
            {
                SetDefaults();
            }
        }

        public static void SetDefaults()
        {
            portNumber = 9999;
            hostName = System.Net.Dns.GetHostName();
            ShareFolderName = "DDDClient";// "Users\\Public\\DDDClient";
            dddClientDir = String.Format("\\\\{0}\\{1}", System.Environment.MachineName, shareFolderName);
            simulationModelPath = String.Format("{0}\\SimulationModel.xml", dddClientDir);
            scenarioSchemaPath = String.Format("{0}\\DDDSchema_4_2.xsd", dddClientDir);
            defaultScenarioPath = String.Format("{0}\\DDD_4_2_Scenario.xml", dddClientDir);
            eventLogDirectory = String.Format("{0}\\", dddClientDir);
            useStrongPasswords = true;
            eventLogType = "LIMITED";
            useObjectLog = false;
            usePerformanceLog = false;
            showScoreSummary = false;
            hlaExport = false;
            hlaFederationExecutionName = "";
            hlaFederationFilePath = "";
            hlaXMLFilePath = "";
            enableVoiceServer = false;
            voiceServerHostname = "localhost";
            voiceServerPort = 10300;
            voiceServerAdminUsername = "admin";
            voiceServerAdminPassword = "admin";
            voiceServerUserPassword = "user";
            enableVoiceServerRecordings = false;
            ForkReplaySpeed = 20;
            string assemblyDir = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            string drive = Path.GetPathRoot(assemblyDir);
            voiceServerAudioLogDir = Path.Combine(drive, "DDDVoiceRecordings");
            
            WriteFile();
        }
    }
}
