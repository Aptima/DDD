using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Aptima.Asim.DDD.CommonComponents;
using System.Xml.Schema;

using System.Threading;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// Chooses and uses a debug logger
    /// All details are static except writing to logger, thus permitting
    /// different streams with different identifications
    /// </summary>
    public class DebugLogger
    {
        private static int loggerNumber = 0;
        private string loggerID;
        private static Boolean isLogging = true;
        public static Boolean IsLogging
        {
            get { return isLogging; }
            set { isLogging = value; }
        }

        public enum DebugStyleValues
        {
            NotDebugging = 0,
            ConsoleReporting = 1,
            FileReporting = 2,
        }
 /*       public enum TestLogValues
        { 
            All = 0,
            Debug = 1,
            Test = 2,
        }
  * */
        //private static bool[] levelOfLogging = null;
        private static Dictionary<string, bool> loggingLevel = new Dictionary<string, bool>();
        static DebugStyleValues debugStyle = DebugStyleValues.FileReporting;

        private static Boolean logFileOpen = false;
        private static string logFileName;
        public static string LogFileName
        {
            set { logFileName = value; }
        }
        private static FileStream logFileStream;
        private static StreamWriter logger;

        private static void SetDebugStyleFile()
        {
            if (!logFileOpen)
            {
                try
                {
                    isLogging = true;
                    logFileStream = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.None);
                    logger = new StreamWriter(logFileStream);
                    logFileOpen = true;
                    logger.AutoFlush = true;
                }
                catch 
                {
                    debugStyle = DebugStyleValues.NotDebugging;
                }
            }
            debugStyle = DebugStyleValues.FileReporting;
        }
        public static void SetDebugStyle(DebugStyleValues style)
        {
            debugStyle = style;
            if(style == DebugStyleValues.FileReporting)
                SetDebugStyleFile();
        }
        public static void SetDebugStyleConsole()
        {
            isLogging = true;
            debugStyle = DebugStyleValues.ConsoleReporting;
        }
        public static void SetDebugStyleFile(string fileName)
        {
   
            // We have to close the previously open file, if any
            if (logFileOpen)
            {
                try
                {
                    logFileStream.Flush();
                    logFileStream.Close();
                    logFileOpen = false;
                }
                finally { }
            }
            if (null != fileName)
            {
                logFileName = fileName;
            }
      
            SetDebugStyleFile();
        }
        //public static void SetTestValueDebug()
        //{
        //    testLog = TestLogValues.Debug;
        //}

        //public static void SetTestValueTest()
        //{
        //    testLog = TestLogValues.Test;
        //}

        //public static void SetTestValueAll()
        //{
        //    testLog = TestLogValues.All;
        //}
        //public static void SetTestValue(bool[] testValue)
        //{
        //    levelOfLogging = testValue;
        //}
        public static void SetLoggingType(string type, bool value)
        {
            type = type.ToLower();
            if (loggingLevel.ContainsKey(type))
            {
                loggingLevel[type] = value;
            }
            else
                if (!loggingLevel.ContainsKey(type))
                {
                    loggingLevel.Add(type, value);
                }
        }
        public static void StopLogging()
        {
            if (logFileOpen)
            {
                logFileStream.Flush();
                logFileOpen = false;
                logFileStream.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendingComponent"></param>
        /// <param name="message"></param>
        /// <param name="typeOfMessage"></param>
        public void Writeline(string sendingComponent,string message, string typeOfMessage)
        {
            typeOfMessage = typeOfMessage.ToLower();
            if (isLogging)
            {
                if (loggingLevel["all"] ||
                    loggingLevel.ContainsKey(typeOfMessage))
                {
                    if (loggingLevel["all"] || loggingLevel[typeOfMessage])
                    {
                        DateTime currTime = DateTime.Now;
                        string time = currTime.ToString("HH:mm:ss");
                        string toWrite = string.Empty;

                        toWrite += time + " | " + sendingComponent;
                        if (sendingComponent.Length < 12)
                            toWrite += "\t";
                        toWrite += "\t| " + typeOfMessage + " - " + message;

                        switch (debugStyle)
                        {
                            case DebugStyleValues.ConsoleReporting:
                                Console.WriteLine(toWrite);
                                break;
                            case DebugStyleValues.FileReporting:
                                logger.WriteLine(toWrite);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public void LogError(string sendingComponent, string message)
        {
           
            Boolean logState = isLogging;
            isLogging = true;
            try
            {
                Writeline(sendingComponent, message, "error");
                 isLogging = logState;
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error in Logging Message "+message,e);
            }
        }
        public void LogException( string sendingComponent, string message)
        {
            try
            {
                LogError(sendingComponent, message);
                logFileStream.Flush();
                logFileStream.Close();
                logFileOpen = false;
            }
            finally { }
        }
        //public void WriteLine(string s)
        //{
        //    //WriteTest("Internal", s, TestLogValues.All);
        //    //if (isDebugging)
        //    //{
        //    //    DateTime dt = DateTime.Now;
        //    //    // Sets the CurrentCulture property to U.S. English.
        //    //    string time = dt.ToString("T");


        //    //    string toWrite = time + loggerID + s;
        //    //    switch (debugStyle)
        //    //    {
        //    //        case DebugStyleValues.ConsoleReporting:
        //    //            Console.WriteLine(toWrite);
        //    //            break;
        //    //        case DebugStyleValues.FileReporting:
        //    //            logger.WriteLine(toWrite);
        //    //            break;
        //    //        default:
        //    //            break;
        //    //    }

        //    //}
        //}


        public DebugLogger()
        {
            loggerNumber += 1;
            loggerID = ":" + loggerNumber.ToString() + ">";
            DateTime dt = DateTime.Now;
            logFileName = String.Format(@"C:\DebugLog-{0:MM-dd-yy-HHmmss}.txt", dt);
            if (loggingLevel.ContainsKey("all"))
            {
                loggingLevel["all"] = true;
            }
            else
            {
                loggingLevel.Add("all", true);
            }
            if (loggingLevel.ContainsKey("error"))
            {
                loggingLevel["error"] = true;
            }
            else
            {
                loggingLevel.Add("error", true);
            }

        }

        ~DebugLogger()
        {
            //if (false)//(logFileOpen)
            //{
            //    try
            //    {
            //        logFileStream.Flush();
            //        logFileOpen = false;
            //        logFileStream.Close();
            //    }
            //    finally { }
            //}
        }

    }
}
