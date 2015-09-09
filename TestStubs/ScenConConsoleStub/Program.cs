using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using System.Xml.Schema;
 
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace  Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// This is the main program
    /// </summary>
    public class readParse
    {


        /*
            STAThread means the app uses Single-Threaded Apartment threading. 
            It specifies the threading model used when you use COM interop. 
            Likely it won't affect your program, since COM isn't usually 
            needed in .NET in most cases.
    */
        /// <summary>
        /// Argumnts to main are
        /// hostname
        /// port number
        /// simulation moel name
        /// 'LIVE" to use network connections, else use crude simulation
        /// Override the sleep interval given in the sim model file
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>arg 0: Scenario file
        /// arg 1: schema file
        /// arg 2: host name
        /// arg 3: port number
        /// arg 4: simulation model name
        /// arg 5: if "NETWORK" (or not provid3ed) program is connected to real network layer;
        ///        else use a simulated mechanism 
        /// arg 6: Update frequency in millisec
        /// arg 7: If  GUI ((or absent) assume connected to real GUI; else simulate incoming messages
        ///  Note: Both arg 5 and arg 6 involve (different) simulated ways to generate incoming events      
        /// </remarks>
        
        static void Main(string[] args)
        {
            //This does not get called by SimCoreTestGUI
            DebugLogger.SetDebugStyleConsole();
            List<string> loggingTypes = new List<string>();
            loggingTypes.Add("test");
            loggingTypes.Add("debug");
            SimulationEventDistributor dist = null;
            Coordinator boss = new Coordinator(args[0],args[1],null,ref dist,args[4],args[5],args[6],args[7], loggingTypes, @"C:\");
            //add types of logging here
            boss.CoordinateNow();



            Coordinator.debugLogger.Writeline("Program.cs", "The end", "all");
 
        }





    }
}





