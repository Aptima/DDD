using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Aptima.Asim.DDD.ScenarioController
{

  public class EngramPair
        {
            private string engramValue;
            public string EngramValue
            {
                get { return engramValue; }
                set { this.engramValue = value; }
            }
                    private string engramType;
            public string EngramType
            {
                get { return engramType; }
                set { this.engramType = value; }
            }
      private Dictionary<string, string> engramUnitValues = new Dictionary<string, string>();
      public void SetUnitValue(string unit, string value)
      {
          engramUnitValues[unit] = value;
      }
      public string GetUnitValue(string unit)
      {
          string returnValue;
          if(engramUnitValues.ContainsKey(unit))
          {
              returnValue= engramUnitValues[unit];
          }
          else
          {
              returnValue=this.engramValue;
          }
          return returnValue;
      }
public  EngramPair(string value,string type)
{
    if (!Engrams.ValidType(type, value))
        throw new ApplicationException("Illegal assignment of  " + value + " to an engram of type " + type);
    this.engramType=type;
    this.engramValue=value;
}
        }
    /// <summary>
    /// Engrams is  class for managing and tracking engram values
    /// </summary>
    /// 
    public class Engrams
    {
      
        private static Dictionary<string, EngramPair> table = new Dictionary<string, EngramPair>();
        private static Regex decimalRegex = new Regex(@"^\s*-?\s*(\d+.?|\d*.\d+)\s*$");
        public static Boolean ValidType(string type, string value)
        {
            Boolean returnValue=false;
            switch (type) 
            {
                case "String":
                    returnValue = true; //anything goes
                    break;
                case "Logical":
                    string lowValue=value.ToLower();
                    if(("true"==lowValue)||("false"==lowValue))
                        returnValue=true;
                    break;
                case "Double":
                     if (decimalRegex.IsMatch(value))
                         returnValue=true;
                    break;
                default:
                    throw new ApplicationException("Illegal type for an engram: "+value);
            }
return             returnValue ;
        }
        public static Boolean ValidUpdate(string name,string value)
        {
            if(!table.ContainsKey(name))
                throw new ApplicationException("Engram not defined: "+name);
           return ValidType(table[name].EngramType,value);
        }

        public static void Create(string name, string value,string type)
        {
            if (!table.ContainsKey(name))
            {
                table.Add(name, new EngramPair(value,type));
            }
            else
            {
                throw new ApplicationException("Attempt to redefine existing variable: " + name);
            }
        }
        public static void Set(string name, string value)
        {
            if (Engrams.table.ContainsKey(name))
            {
               
                Engrams.table[name].EngramValue = value;
            }
            else
            {
                throw new ApplicationException("Attempt to give value to undefined  variable: " + name);
            }
        }
        public static void Set(string name, string unit, string value)
        {
            if ("" == unit)
            {
                Set(name, value);
            }
            else
            {
                if (!table.ContainsKey(name))
                    Set(name, value);
                table[name].SetUnitValue(unit, value);

            }
        }
        public static string GetType(string name)
        {
            if (table.ContainsKey(name))
            {
                return table[name].EngramType;
            }
            else
            {
                throw new ApplicationException("Attempt to get type of undefined engram: " + name);
            }
        }
        public static string GetValue(string name)
        {
            if (table.ContainsKey(name))
            {
                return table[name].EngramValue;
            }
            else
            {
                throw new ApplicationException("Attempt to get value of undefined engram: " + name);
            }
        }
        public static string GetValue(string name, string unit)
        {
            string returnValue;
            if ("" == unit)
            {
                returnValue = GetValue(name);
            }
            else
            {
                returnValue = table[name].GetUnitValue(unit);
            }
            return returnValue;
        }
        /*
         * public static Boolean InRange(string name, List<string> valueList)
               {
                   try
                   {
                       if (valueList.Contains(Get(name)))
                       {
                           return true;
                       }
                       else
                       {
                           return false;
                       }
                   }
                   catch (SystemException e)
                   {
                       throw e;
                   }
               }
               public static Boolean OutRange(string name, List<string> valueList)
               {
                   try
                   {
                       if (!valueList.Contains(Get(name)))
                       {
                           return true;
                       }
                       else
                       {
                           return false;
                       }
                   }
                   catch (SystemException e)
                   {
                       throw e;
                   }
        
               }
        */
        public static void Remove(string name)
        {
            if (table.ContainsKey(name))
            {
                table.Remove(name);
            }
            else
            {
                throw new ApplicationException("Attempt to destroy undefined  variable: " + name);
            }
        }

        public static void Clear()
        {
            table = new Dictionary<string,EngramPair>();
        }

        public Engrams()
        {

        }
    }

}
