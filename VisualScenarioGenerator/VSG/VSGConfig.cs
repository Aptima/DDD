using System;
using System.Collections.Generic;
using System.Text;
//using System.ComponentModel;
using System.Data;
//using System.Drawing;

using System.Windows.Forms;
using System.IO;

namespace VSG.ConfigFile
{
    public class VSGConfig
    {
        private static string vsgConfigPath = String.Format("{0}\\VSGConfig.txt",Application.StartupPath);

        private static string mapDir = "";
        public static string MapDir
        {
            get { return mapDir; }
            set { mapDir = value; }
        }

        private static string iconDir = "";
        public static string IconDir
        {
            get { return iconDir; }
            set { iconDir = value; }
        }

        private static string openSaveDir = "";
        public static string OpenSaveDir
        {
            get { return openSaveDir; }
            set { openSaveDir = value; }
        }

        public static void WriteFile()
        {
            StreamWriter sw = new StreamWriter(vsgConfigPath);

            sw.WriteLine(string.Format("MapDir; {0}", mapDir));
            sw.WriteLine(string.Format("IconDir; {0}", iconDir));
            sw.WriteLine(string.Format("OpenSaveDir; {0}", openSaveDir));

            sw.Close();
        }

        public static void ReadFile()
        {
            string[] delimiters = { ";" };
            if (File.Exists(vsgConfigPath))
            {
                StreamReader sr = new StreamReader(vsgConfigPath);
                string currPath;

                currPath = sr.ReadLine();
                string[] parts;
                while (currPath != null)
                {
                    parts = currPath.Split(delimiters, StringSplitOptions.None);
                    switch (parts[0])
                    {
                        case "MapDir":
                            mapDir = parts[1].Trim();
                            break;
                        case "IconDir":
                            iconDir = parts[1].Trim();
                            break;
                        case "OpenSaveDir":
                            openSaveDir = parts[1].Trim();
                            break;
                        default:
                            throw new Exception(String.Format("Incorrect option \"{0}\" in {0}", parts[0], vsgConfigPath));
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
            mapDir = "";
            iconDir = "";
            openSaveDir = "";
            WriteFile();
        }
    }
}
