using System;
using System.Collections.Generic;
namespace Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits
{
    public class Genealogy
    {
        /// <summary>
        /// Ancestors maps unit IDs to the names of their genera
        /// </summary>
        static Dictionary<string, string> ancestors = new Dictionary<string, string>();
        public static void Add(string genus)
        {
            ancestors[genus] = null;
        }
        public static void Add(string unit, string kind)
        {
            if (!NameLists.speciesNames.ContainsKey(kind)) throw new ApplicationException("Definition of " + unit + " references unknown species/genus " + kind);
            ancestors[unit] = kind;
        }
        public static string GetBase(string unit)
        {
            return ancestors[unit];
        }
        public static string GetGenus(string unit)
        {
            string basedOn = ancestors[unit]; // based on is a species or a genus
            while (null != ancestors[basedOn])
            {
                basedOn = ancestors[basedOn];
            }
            return basedOn;
        }
        public static void Clear()
        {
            ancestors.Clear();
        }
        public static Boolean KnownSpecies(string s)
        {
            return (ancestors.ContainsKey(s));
        }
        public static Boolean UnderSpecies(string unit, string species)
        {
            try
            {
                Boolean returnValue = false;
                if (!ancestors.ContainsKey(unit))
                {
                    return false;
                }
                string zadeh = ancestors[unit];// Yiddish for grandfather

                while (null != zadeh)
                {
                    if (zadeh == species)
                    {
                        returnValue = true;
                        break;
                    }
                    if (!ancestors.ContainsKey(zadeh))
                    {
                        return false;
                    }
                    zadeh = ancestors[zadeh];
                }
                return returnValue; ;
            }
            catch (SystemException e)
            {
                throw new SystemException("error matching unit " + unit + " to species " + species, e);
            }
        }
    }
    public class BaseObjectDictionary
    {
        Dictionary<string, object> containedData;
        public Dictionary<string, object> ContainedData
        {
            get { return containedData; }
            set { containedData = value; }
        }
        public object this[string name]
        {
            get { return containedData[name]; }
            set { containedData[name] = value; }
        }
        public Boolean New(string s, object o)
        {
            Boolean returnValue = false;
            if (!this.ContainsKey(s))
            {
                this[s] = o;
                returnValue = true;
            }
            return returnValue;
        }
        public List<string> GetKeys()
        {
            return new List<string>(containedData.Keys);
        }
        public void Add(string s, object o)
        {
            containedData[s] = o;
        }

        public Boolean ContainsKey(string s)
        {
            return containedData.ContainsKey(s);
        }
  
        public List<string> Keys
        {
            get { return new List<string>(this.containedData.Keys); }
        }

        public BaseObjectDictionary()
        {
            containedData = new Dictionary<string, object>();
        }


 
    }

    /// <summary>
    /// This is a catchall collection of names whose existence needs to be verified.
    /// Each list is stored as a dictionary so checking for existence is via keys, but we can always check for other properties as neede4d.
    /// </summary>
    /// 
   
    public class NameLists
    {

        public static BaseObjectDictionary activeRegionNames = new BaseObjectDictionary();
        public static BaseObjectDictionary teamNames = new BaseObjectDictionary();
        public static BaseObjectDictionary ruleNames = new BaseObjectDictionary();
        public static BaseObjectDictionary scoreNames = new BaseObjectDictionary();
        public static BaseObjectDictionary speciesNames = new BaseObjectDictionary();
        public static BaseObjectDictionary unitNames = new BaseObjectDictionary();
        public static BaseObjectDictionary chatroomNames = new BaseObjectDictionary();// For rooms created at scenario time ONLY
        public static BaseObjectDictionary whiteboardroomNames = new BaseObjectDictionary();// For rooms created at scenario time ONLY
        public static BaseObjectDictionary voicechannelNames = new BaseObjectDictionary();// For channels created at scenario time ONLY
 
        public static void Clear()
        {
            activeRegionNames = new BaseObjectDictionary();
            teamNames = new BaseObjectDictionary();
            ruleNames = new BaseObjectDictionary();
            scoreNames = new BaseObjectDictionary();
            speciesNames = new BaseObjectDictionary();
            unitNames = new BaseObjectDictionary();
            chatroomNames = new BaseObjectDictionary();
            whiteboardroomNames = new BaseObjectDictionary();
        }
    }
}
