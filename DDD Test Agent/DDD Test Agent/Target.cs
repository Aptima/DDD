using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
namespace DDD_Test_Agent
{

    class Target
    {
        private static Regex identifier = new Regex("^Post");
        public static Boolean IsTarget(string id)
        {
            Boolean returnValue = false;
            if (Target.identifier.IsMatch(id))
                returnValue = true;
            return returnValue;

        }

        private static Dictionary<string, Target> allUnits = new Dictionary<string, Target>();
        public static Target Get(string iD)
        {
            return allUnits[iD];
        }
        public static string GetRandom()
        {
            string returnValue="";
            if (0 < allUnits.Keys.Count)
            {
                string[] keys = new string[allUnits.Keys.Count];
                allUnits.Keys.CopyTo(keys, 0);
                returnValue = keys[randInt(keys.Length)];
            }
            return returnValue;
        }
        public static void AddUnit(string iD)
        {
            allUnits.Add(iD, new Target());
        }
        public static void DropUnit(string iD)
        {
            if (allUnits.ContainsKey(iD))
                allUnits.Remove(iD);
        }



   
 

        static Random randomGenerator = new Random();
        /// <summary>
        /// randInt generates a random integer that can take and value in low,...,high-1
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        private static int randInt(int low, int high)
        {
            return low + randInt(high - low);

        }
        private static int randInt(int high)
        {
            double randDbl = randomGenerator.NextDouble();
            return (int)Math.Truncate(randDbl * high);
        }
    }

}
