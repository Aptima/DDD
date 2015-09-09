using System;
 
using System.IO;
using System.Xml;
using DDD.CommonComponents;
using System.Xml.Schema;
 
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.ScenarioController
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
        /// arg 1: host name
        /// arg 2: port number
        /// arg 3: simulation model name
        /// arg 4: if "NETWORK" (or not provid3ed) program is connected to real network layer;
        ///        else use a simulated mechanism 
        /// arg 5: Update frequency in millisec
        /// arg 6: If  GUI ((or absent) assume connected to real GUI; else simulate incoming messages
        ///  Note: Both arg 4 and arg 5 involve (different) simulated ways to generate incoming events      
        /// </remarks>
        
        static void Main(string[] args)
        {


            Coordinator.Coordinate(args[0],args[1],args[2],args[3],args[4],args[5],args[6]);
 

            /*
             * Temporary event  watcher to allow for insertion of events from below
             */


            Console.WriteLine("The end");
 
        }





    }
}





