using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Aptima.Asim.DDD.CommonComponents.SimulationModelTools
{
    /// <summary>
    /// The main object used for reading in the simulation model file and building
    /// a SimulationModelInfo object.
    /// </summary>
    public class SimulationModelReader
    {
        public SimulationModelReader()
        {
            
        }

        static List<EventInfo> getEventInfo(string parentEvent, GeneratedCode.EventTypeInfo eventType)
        {
            List<EventInfo> events = new List<EventInfo>();
            string name = eventType.Name;
            EventInfo e = new EventInfo(name);
            e.parentName = parentEvent;
            e.shouldLog = eventType.Log;
            e.shouldReplay = eventType.Replay;
            e.shouldForkReplay = eventType.ForkReplay;
            e.simCoreSubscribe = eventType.SimCoreSubscribe;
            e.description = eventType.Description;

            if (eventType.Parameter != null)
            {
                for (int i = 0; i < eventType.Parameter.Length; i++)
                {
                    string pName = eventType.Parameter[i].Name;
                    string pType = eventType.Parameter[i].DataType.ToString();
                    string pDescript = eventType.Parameter[i].Value;
                    e.parameters[pName] = new ParameterInfo(pName, pType);
                    e.parameters[pName].description = pDescript;
                }
            }
            events.Add(e);
            if (eventType.EventType != null)
            {
                for (int i = 0; i < eventType.EventType.Length; i++)
                {
                    events.AddRange(getEventInfo(name, eventType.EventType[i]));
                }
            }
            return events;
        }
        static List<ObjectInfo> getObjectInfo(string parentObject, GeneratedCode.ObjectTypeInfo objectType)
        {
            List<ObjectInfo> objects = new List<ObjectInfo>();
            string name = objectType.Name;
            ObjectInfo o = new ObjectInfo(name);
            o.parentName = parentObject;

            if (objectType.Attribute != null)
            {
                for (int i = 0; i < objectType.Attribute.Length; i++)
                {
                    string aName = objectType.Attribute[i].Name;
                    string aType = objectType.Attribute[i].DataType.ToString();
                    bool ownerObservable = objectType.Attribute[i].OwnerObservable;
                    bool otherObservable = objectType.Attribute[i].OtherObservable;
                    bool excludeFromScenario = objectType.Attribute[i].ExcludeFromScenario;
                    o.attributes[aName] = new AttributeInfo(aName, aType,excludeFromScenario,ownerObservable,otherObservable);
                }
            }
            objects.Add(o);
            if (objectType.ObjectType != null)
            {
                for (int i = 0; i < objectType.ObjectType.Length; i++)
                {
                    objects.AddRange(getObjectInfo(name, objectType.ObjectType[i]));
                }
            }
            return objects;
        }
        EventModelInfo getEventModelInfo(GeneratedCode.SimulationModelRoot model)
        {
            EventModelInfo eventModel = new EventModelInfo();
            List<EventInfo> events = null;
            for (int i = 0; i < model.EventModel.Length; i++)
            {
                if (events == null)
                {
                    events = getEventInfo("", model.EventModel[i]);
                }
                else
                {
                    System.Console.WriteLine("Error: there should only be one EventModel section!");
                    System.Environment.Exit(1);
                }
                foreach (EventInfo e in events)
                {
                    if (eventModel.events.ContainsKey(e.name))
                    {
                        System.Console.WriteLine("Error: multiple definition of " + e.name + " event type");
                        System.Environment.Exit(1);
                    }

                    if (e.parentName != "")
                    {
                        foreach (ParameterInfo p in eventModel.events[e.parentName].parameters.Values)
                        {
                            if (e.parameters.ContainsKey(p.name))
                            {
                                System.Console.WriteLine("Error: multiple parameters with name:" + p.name + " defined");
                                System.Environment.Exit(1);
                            }
                            e.parameters[p.name] = p;
                        }
                    }
                    eventModel.events[e.name] = e;
                }
            }
            return eventModel;
        }
        ObjectModelInfo getObjectModelInfo(GeneratedCode.SimulationModelRoot model)
        {
            ObjectModelInfo objectModel = new ObjectModelInfo();
            List<ObjectInfo> objects = null;
            for (int i = 0; i < model.ObjectModel.Length; i++)
            {
                if (objects == null)
                {
                    objects = getObjectInfo("", model.ObjectModel[i]);
                }
                else
                {
                    System.Console.WriteLine("Error: there should only be one EventModel section!");
                    System.Environment.Exit(1);
                }
                foreach (ObjectInfo o in objects)
                {
                    if (objectModel.objects.ContainsKey(o.name))
                    {
                        System.Console.WriteLine("Error: multiple definition of " + o.name + " object type");
                        System.Environment.Exit(1);
                    }

                    if (o.parentName != "")
                    {
                        foreach (AttributeInfo a in objectModel.objects[o.parentName].attributes.Values)
                        {
                            if (o.attributes.ContainsKey(a.name))
                            {
                                System.Console.WriteLine("Error: multiple parameters with name:" + a.name + " defined");
                                System.Environment.Exit(1);
                            }
                            o.attributes[a.name] = a;
                        }
                    }
                    objectModel.objects[o.name] = o;
                }
            }
            return objectModel;
        }

        SimulationExecutionModelInfo getSimulationExecutionModelInfo(GeneratedCode.SimulationModelRoot model)
        {
            SimulationExecutionModelInfo simex = new SimulationExecutionModelInfo();
            SimulatorExecutionInfo sim = null;
            simex.updateFrequency = model.SimulationExecutionModel.UpdateFrequency;
            if (model.SimulationExecutionModel.Simulator != null)
            {
                for (int i = 0; i < model.SimulationExecutionModel.Simulator.Length; i++)
                {
                    sim = new SimulatorExecutionInfo();
                    sim.simulatorName = model.SimulationExecutionModel.Simulator[i].SimulatorName;
                    sim.dllName = model.SimulationExecutionModel.Simulator[i].DLLName;
                    simex.simulators.Add(sim);
                }
            }
            return simex;
        }

        /// <summary>
        /// This method reads in a simulation model xml file and builds a SimulationModelInfo object.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public SimulationModelInfo readModel(string fileName)
        {
            SimulationModelInfo simMod = new SimulationModelInfo();
            GeneratedCode.SimulationModelRoot model = null;
            try
            {

                FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer newSr = new XmlSerializer(typeof(GeneratedCode.SimulationModelRoot));

                model = (GeneratedCode.SimulationModelRoot)newSr.Deserialize(f);
                f.Close();
            }
            catch (Exception err)
            {
                throw new Exception(String.Format("Error reading simulation model file: \"{0}\"",fileName),err);
                
                
                System.Console.WriteLine("file=" + fileName);
                System.Console.WriteLine(
                    "** Parsing error"
                    + ", line "
                    + err.StackTrace);
                System.Console.WriteLine(" " + err.Message);
                System.Environment.Exit(1);
            }
            simMod.eventModel = getEventModelInfo(model);
            simMod.objectModel = getObjectModelInfo(model);
            simMod.simulationExecutionModel = getSimulationExecutionModelInfo(model);
            return simMod;
        }

        public SimulationModelInfo readModel(Stream str)
        {
            SimulationModelInfo simMod = new SimulationModelInfo();
            GeneratedCode.SimulationModelRoot model = null;
            try
            {
                XmlSerializer newSr = new XmlSerializer(typeof(GeneratedCode.SimulationModelRoot));

                model = (GeneratedCode.SimulationModelRoot)newSr.Deserialize(str);
                str.Close();
            }
            catch (Exception err)
            {
                throw new Exception("Error reading simulation model stream:" + err.Message);
            }
            simMod.eventModel = getEventModelInfo(model);
            simMod.objectModel = getObjectModelInfo(model);
            simMod.simulationExecutionModel = getSimulationExecutionModelInfo(model);
            return simMod;
        }

        private bool validFileExtension(string filename)
        {
            return (filename.EndsWith(".xml")
                || filename.EndsWith(".XML")
                || filename.EndsWith(".idx")
                || filename.EndsWith(".IDX"));
        }
    }
}
