using System;
using System.Collections.Generic;
 
namespace DDD.ScenarioController
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    //  ----------------------- RootEventType -----------------------
    public class RootEventType
    { // just exists to provide a common type for all elements on the queue
        private int timer = 0;
        public int Timer
        {
            get
            { return this.timer; }
            set { this.timer = value; }
        }

    }
    //   ----------------------- ScenarioEventType -----------------------
    public class ScenarioEventType : RootEventType
    {
        private string unitID = null;
        public string UnitID
        {
            get
            { return this.unitID; }
            set { this.unitID = value; }
        }



        public ScenarioEventType(string unitID, int timer)
        {
            this.UnitID = unitID;
            this.Timer = timer;
        }
        public ScenarioEventType(string unitID)
        {
            this.UnitID = unitID;
            this.Timer = 0;
        }

        public virtual void Add(ParameterSettingType setting)
        { }
        public virtual void Add(string attr, string set)
        { }

    }
    // ----------------------- TickEventType  -----------------------
    public class TickEventType : RootEventType
    {


        public TickEventType(int timer)
        {
            this.Timer = timer;
        }
    }
    //  ----------------------- Create_EventType  -----------------------
    public class Create_EventType : ScenarioEventType
    {
        private string unitKind;
        public string UnitKind
        {
            get
            { return this.unitKind; }
            set { this.unitKind = value; }

        }
        private List<ParameterSettingType> parameters;
        public List<ParameterSettingType> Parameters
        {
            get
            { return this.parameters; }
        }
        public Create_EventType(string unit)
            : base(unit)
        {
            parameters = new List<ParameterSettingType>();
        }
        public override void Add(ParameterSettingType attr)
        {
            parameters.Add(attr); //START BY ADDING A 2 argument method here
        }
        public override void Add(string attr, string set)
        {
            parameters.Add(new ParameterSettingType(attr, set));
        }
    }
    // ----------------------- StateChangeEventType
    public class StateChange_EventType : ScenarioEventType
    {
        private List<ParameterSettingType> parameters;

        public List<ParameterSettingType> Parameters
        {
            get
            { return this.parameters; }

        }
        public StateChange_EventType(string unit)
            : base(unit)
        {
            parameters = new List<ParameterSettingType>();
        }
        public override void Add(ParameterSettingType setting)
        {
            Parameters.Add(setting);
        }
        public override void Add(string attr, string set)
        {
            Parameters.Add(new ParameterSettingType(attr, set));
        }
    }
    //  ----------------------- MoveEventType  -----------------------
    public class Move_EventType : ScenarioEventType
    {
        private int throttle;
        public int Throttle
        {
            get
            { return this.throttle; }
            set
            { throttle = Math.Min(100, value); }
        }
        private VectorType location;
        public VectorType Location
        {
            get
            { return this.location; }
            set
            { this.location = value; }

        }
        public Move_EventType(string unit, int timer)
            : base(unit, timer)
        {
            location = new VectorType();
        }
        public Move_EventType(string unit, int timer,VectorType location)
            : base(unit, timer)
        {
            this.location = location;
        }
    }

    public class HappeningCompletionType : ScenarioEventType
    {
        private string action;
        public string Action
        {
            get
            { return action; }
            set
            { action = value; }
        }
        private ScenarioEventType doThis;
        public ScenarioEventType DoThis
        {
            get
            { return doThis; }
            set
            { doThis = value; }

        }
        public HappeningCompletionType(string unit, string action, ScenarioEventType doThis)
            : base(unit)
        {
            this.action = action;
            this.doThis = doThis;
        }

    }



    public class MoveComplete_Event : ScenarioEventType
    {
        public MoveComplete_Event(string unit)
            : base(unit)
        { }
    }

}