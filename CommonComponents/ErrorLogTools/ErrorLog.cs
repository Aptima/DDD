using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.CommonComponents.ErrorLogTools
{
    public class LogFile
    {
        string baseName = "";
        FileStream file = null;
        StreamWriter sr = null;
        FileMode mode;
        public string filePath;
        private object fileLock;

        public LogFile(string baseName, FileMode mode)
        {
            this.baseName = baseName;
            this.mode = mode;
            fileLock = new object();
        }

        public void Write(string message)
        {
            lock (fileLock)
            {
                if (file == null)
                {
                    OpenFile();
                }

                DateTime time = DateTime.Now;
                string timeStr = String.Format("{0:yyyyMMddHHmmss}:", time);
                sr.WriteLine();//buffer
                sr.Write(timeStr);
                sr.Write(message);
                sr.Flush();
            }
        }

        private void OpenFile()
        {
            int num = 1;
            while (file == null)
            {
                try
                {
                    string number = "";
                    if (num > 1)
                    {
                        number = num.ToString();
                    }

                    filePath = String.Format("{0}\\{1}{2}.txt", System.Windows.Forms.Application.StartupPath, baseName, number);
                    file = new FileStream(filePath, mode);
                    sr = new StreamWriter(file);
                }
                catch
                {
                    file = null;
                    sr = null;
                    filePath = "";
                }
                num++;
            }
        }
    }

    public class ErrorLog
    {
        static LogFile logFile = new LogFile("DDDErrorLog", FileMode.Append);

        public static void Write(string m)
        {
            logFile.Write(m);
            
        }
    }

    public class PerformanceLog
    {
        static LogFile logFile = new LogFile("DDDPerformanceLog", FileMode.Append);

        public static void Write(string m)
        {
            if (ServerOptions.UsePerformanceLog)
            {
                logFile.Write(m);
            }
        }
    }

    public class ObjectLog
    {
        static LogFile logFile = new LogFile("DDDObjectLog", FileMode.Create);
        //static public bool enabled = false;
        public static void Write(int time, string objectID, string attName,string attValue)
        {
            if (ServerOptions.UseObjectLog)
            {
                if (attName == "StateTable")
                {
                    return;
                }
                logFile.Write(String.Format("{0},{1},{2},{3}", time, objectID, attName, attValue));
            }
        }
    }
}
