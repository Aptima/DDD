using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
namespace DDD_Test_Agent
{
    class Program
    {
       
  
        static void Main(string[] args)
        {
      
            ConnectionManager cm = ConnectionManager.MakeConnection("DGeller2", 9999);
            ScudLauncher.Corners = new Dictionary<string, Location>();
           
            ScudLauncher.Corners["NE"]=new Location(603,531);
            ScudLauncher.Corners["SE"] = new Location(603, 254);
            ScudLauncher.Corners["NW"]=new Location(500,533);
            ScudLauncher.Corners["SW"]=new Location(500,254);
// Get the network
            NetworkClient nc = cm.NetClient;
// Create the listener
            cm.StartListening();
         }
    }
}
