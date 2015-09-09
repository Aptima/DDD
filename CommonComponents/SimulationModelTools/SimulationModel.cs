using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.CommonComponents.SimulationModelTools
{
    /// <summary>
    /// Used for keeping track of the object model, a subset of the simulation model.
    /// Will be built  as part of SimulationModelInfo object by 
    /// the SimulationModelReader.
    /// </summary>
    public class ObjectModelInfo
    {
        public Dictionary<string, ObjectInfo> objects;
        public ObjectModelInfo()
        {
            objects = new Dictionary<string, ObjectInfo>();
        }
        /// <summary>
        /// Is the child object type derrived from the parent object type?
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsDerivedFrom(string child, string parent)
        {
            if (child == parent)
            {
                return true;
            }
            else if (!objects.ContainsKey(objects[child].parentName))
            {
                return false;
            }
            else if (objects[child].parentName == parent)
            {
                return true;
            }
            else if (IsDerivedFrom(objects[child].parentName, parent))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    /// <summary>
    /// Used for keeping track of what attributes can be part of a simulation object.
    /// </summary>
    public class AttributeInfo
    {
        public string name;
        public string dataType;
        public bool excludeFromScenario;
        public bool ownerObservable;
        public bool otherObservable;
        public AttributeInfo(string name, 
                             string dataType,
                             bool excludeFromScenario,
                             bool ownerObservable,
                             bool otherObservable)
        {
            this.name = name;
            this.dataType = dataType;
            this.excludeFromScenario = excludeFromScenario;
            this.ownerObservable = ownerObservable;
            this.otherObservable = otherObservable;

        }
    }
    /// <summary>
    /// Used for keeping track of the types of objects that are possible in the simulation
    /// based on the simulation model.
    /// Contains a dictionary of AttributeInfo objects.
    /// </summary>
    public class ObjectInfo
    {
        public string name;
        public string parentName;
        public Dictionary<string, AttributeInfo> attributes;
        public ObjectInfo(string name)
        {
            this.name = name;
            this.parentName = null;
            attributes = new Dictionary<string, AttributeInfo>();
        }
    }
    /// <summary>
    /// Used for keeping track of what parameters can be part of a simulation event.
    /// </summary>
    public class ParameterInfo
    {
        public string name;
        public string dataType;
        public string description;
        public ParameterInfo(string name,string dataType)
        {
            this.name = name;
            this.dataType = dataType;
            this.description = "";
        }
    }
    /// <summary>
    /// Used for keeping track of the types of events that are possible in the simulation
    /// based on the simulation model.
    /// Contains a dictionary of ParameterInfo objects.
    /// </summary>
    public class EventInfo
    {
        public string name;
        public bool shouldLog;
        public bool shouldReplay;
        public bool shouldForkReplay;
        public bool simCoreSubscribe;
        public string parentName;
        public string description;
        public Dictionary<string, ParameterInfo> parameters;
        public EventInfo(string name)
        {
            this.name = name;
            this.shouldLog = true;
            this.shouldReplay = false;
            this.shouldForkReplay = false;
            this.simCoreSubscribe = true;
            this.parentName = null;
            this.description = "";
            parameters = new Dictionary<string, ParameterInfo>();
        }
    }
    /// <summary>
    /// Used for keeping track of the event model, a subset of the simulation model.
    /// Will be built as part of SimulationModelInfo object by 
    /// the SimulationModelReader.
    /// </summary>
    public class EventModelInfo
    {
        public Dictionary<string, EventInfo> events;
        public EventModelInfo()
        {
            events = new Dictionary<string, EventInfo>();
        }
    }

    /// <summary>
    /// Contains the ObjectModelInfo, EventModelInfo, and SimulationExecutionModelInfo objects.
    /// This is build by the SimulationModelReader.readModel() method.
    /// </summary>
    public class SimulationModelInfo
    {
        public ObjectModelInfo objectModel;
        public EventModelInfo eventModel;
        public SimulationExecutionModelInfo simulationExecutionModel;
        public SimulationModelInfo()
        {
            objectModel = null;
            eventModel = null;
            simulationExecutionModel = null;
        }
        public int GetUpdateFrequency()
        {
            return simulationExecutionModel.updateFrequency;
        }
    }

    /// <summary>
    /// Used to keep track of individual simulator info in the simulation model.
    /// </summary>
    public class SimulatorExecutionInfo
    {
        public string simulatorName;
        public string dllName;
        public SimulatorExecutionInfo()
        {
            simulatorName = null;
            dllName = null;
        }
        public SimulatorExecutionInfo(string simName, string dll)
        {
            simulatorName = simName;
            dllName = dll;
        }
    }
    /// <summary>
    /// Used to keep track of the order that simulators are executed in the DDD Server.
    /// Allows access to the updateFrequency, which governs the time-resolution of the DDD Simulation.
    /// </summary>
    public class SimulationExecutionModelInfo
    {
        public List<SimulatorExecutionInfo> simulators;
        public int updateFrequency;
        public SimulationExecutionModelInfo()
        {
            simulators = new List<SimulatorExecutionInfo>();
            updateFrequency = 0;
        }

    }
}
