using System;
using System.Collections.Generic;
namespace Aptima.Asim.DDD.ScenarioController
{
    /// <summary>
    /// This is a catchall collection of names whose existence needs to be verified.
    /// Each list is stored as a dictionary so checking for existence is via keys, but we can always check for other properties as neede4d.
    /// </summary>
    public class NameLists
    {

        public static ObjectDictionary activeRegionNames = new ObjectDictionary();
        public static ObjectDictionary teamNames = new ObjectDictionary();
        public static ObjectDictionary ruleNames = new ObjectDictionary();
        public static ObjectDictionary scoreNames = new ObjectDictionary();
        public static ObjectDictionary speciesNames = new ObjectDictionary();
        public static ObjectDictionary unitNames = new ObjectDictionary();
        public static ObjectDictionary chatroomNames = new ObjectDictionary();// For rooms created at scenario time ONLY
  
        public static void Clear()
        {
            activeRegionNames = new ObjectDictionary();
            teamNames = new ObjectDictionary();
            ruleNames = new ObjectDictionary();
            scoreNames = new ObjectDictionary();
            speciesNames = new ObjectDictionary();
            unitNames = new ObjectDictionary();
            chatroomNames = new ObjectDictionary();
        }
    }
}
