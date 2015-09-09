using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimulationEventTools
{
    /// <summary>
    /// The SimulationEvent is the object used for transmitting information 
    /// between the DDD Server and any client applications.
    /// The SimulationEvent is effectively a dictionary of DataValue objects.
    /// </summary>
    public class SimulationEvent
    {
        public object eventLock;
        public string eventType;
        public Dictionary<string, DataValue> parameters;
        public SimulationEvent()
        {
            eventLock = new object();
            eventType = null;
            parameters = new Dictionary<string, DataValue>();
        }

        /// <summary>
        /// Get or set a DataValue based on its parameter name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataValue this[string key]
        {
            get
            {
                DataValue dv = null; ;
                lock (eventLock)
                {
                    try
                    {
                        dv = parameters[key];
                    }
                    catch (KeyNotFoundException kex)
                    {
                        throw new ApplicationException(String.Format("Missing a key ({0}) in the event type ({1}). {2}", key, eventType, kex.StackTrace), kex.InnerException);
                    }

                }
                return dv;

            }
            set
            {
                lock (eventLock)
                {
                    parameters[key] = value;
                }
            }
        }



    }
}