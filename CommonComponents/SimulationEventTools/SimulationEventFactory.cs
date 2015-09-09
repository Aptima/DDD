using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using System.Text.RegularExpressions;
namespace Aptima.Asim.DDD.CommonComponents.SimulationEventTools
{
    public class SimulationEventFactory
    {
        static Regex regex = new Regex(@"\A<(\w+)>(<Parameter>.*</Parameter>)*</(\w+)>\Z",RegexOptions.Singleline);
        static Regex paramregex = new Regex(@"<Parameter><Name>(.*?)</Name><Value>(.*?)</Value></Parameter>", RegexOptions.Singleline);

        static public SimulationEvent BuildEvent(ref SimulationModelInfo model, string eventType)
        {
            SimulationEvent e = new SimulationEvent();

            if (!model.eventModel.events.ContainsKey(eventType))
            {
                throw new Exception("Event type doesn't exist");
            }

            foreach (ParameterInfo pInfo in model.eventModel.events[eventType].parameters.Values)
            {
                e[pInfo.name] = DataValueFactory.BuildValue(pInfo.dataType);
            }
            e.eventType = eventType;

            return e;
        }
        static public bool ValidateEvent(ref SimulationModelInfo model, SimulationEvent e)
        {
            try
            {
                string eventType = e.eventType;
                if (!model.eventModel.events.ContainsKey(eventType))
                {
                    return false;
                }

                foreach (ParameterInfo pInfo in model.eventModel.events[eventType].parameters.Values)
                {
                    if (!e.parameters.ContainsKey(pInfo.name))
                    {
                        return false;
                    }
                    if (e[pInfo.name].dataType != pInfo.dataType)
                    {
                        return false;
                    }
                }
                foreach (string attname in e.parameters.Keys)
                {
                    
                    if (!model.eventModel.events[eventType].parameters.ContainsKey(attname))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /*
        static public SimulationEvent DeepCopy(ref SimulationEvent e)
        {
            SimulationEvent n = new SimulationEvent();
            n.eventType = e.eventType;
            foreach (string p in e.parameters.Keys)
            {
                n.parameters[p] = e.parameters[p];
            }

            return n;
        }
        */

        static public string XMLSerialize(SimulationEvent e)
        {
            StringBuilder sb;
            lock (e.eventLock)
            {
                sb = new StringBuilder(String.Format("<{0}>", e.eventType));

                foreach (string key in e.parameters.Keys)
                {
                    sb.Append(String.Format("<Parameter><Name>{0}</Name><Value>{1}</Value></Parameter>", key, DataValueFactory.XMLSerialize(e.parameters[key])));
                }
                sb.Append(String.Format("</{0}>", e.eventType));
            }
            return sb.ToString();
        }
        static public SimulationEvent XMLDeserialize(string xml)
        {
            SimulationEvent e = new SimulationEvent();
            Match m = regex.Match(xml);
            if (m.Success)
            {
                e.eventType = m.Groups[1].Value;
                string s = m.Groups[2].Value;
                foreach (Match m2 in paramregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    string val = m2.Groups[2].Value;
                    e.parameters[name] = DataValueFactory.XMLDeserialize(val);
                }
                return e;
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in SimulationEvent: {0}", xml));
            }

        }
    }
}
