using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.CommonComponents.SimulationObjectTools
{
    public class SimulationObject
    {
        public string objectType;
        public Dictionary<string, DataValue> attributes;
        public SimulationObject()
        {
            objectType = null;
            attributes = new Dictionary<string, DataValue>();
        }

        public DataValue this[string key]
        {
            get
            {
                return attributes[key];
            }
            set
            {
                attributes[key] = value;
            }
        }
    }
}
