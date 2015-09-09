using System;
using System.Collections.Generic;
using System.Text;
using DDD.CommonComponents.DataTypeTools;

namespace ScenarioController
{
    class SupportClasses
    {
    }

    public class RootEventType
    {
        private UnitType Unit = new UnitType();
        private string EventType = null;
        private int? Time = null;

        public RootEventType()
        { 
            EventType = "RootEventType";
        }

        public int? time
        {
            get { return Time; }
            set { Time = value; }
        }
        public UnitType unit
        {
            get { return Unit; }
            set { Unit = value; }
        }

        public string eventType
        {
            get { return EventType; }
            set { EventType = value; }
        }

        public string SerializeToXML()
        {
            string returnString = "";

            return returnString;
        }
    }

    public class ScenarioItemType : RootEventType
    {
        public ScenarioItemType()
        {
           base.eventType = "ScenarioItemType";
        }

        public ScenarioItemType(UnitType u)
        {
            unit = u;
            base.eventType = "ScenarioItemType";
        }

        public new string SerializeToXML()
        {
            string returnString = "<"+ base.eventType.ToString() + ">";

            returnString += "<Parameter><Name>ID</Name><Value>" + unit.id + "</Value></Parameter>";
            returnString += "<Parameter><Name>Kind</Name><Value>" + unit.kind + "</Value></Parameter>";

            returnString += "</" + base.eventType.ToString() + ">";

            return returnString;
        }
    }

    
    public class TickItemType
    {
        private UInt32 Tick;
        private string EventType = null;

        public string eventType
        {
            get { return EventType; }
            set { EventType = value; }
        }

        public UInt32 tick
        {
            get { return Tick; }
            set { Tick = value; }
        }

        public TickItemType()
        {
            Tick = 0;
        }

        public TickItemType(UInt32 tick)
        {
            Tick = tick;
        }
    }

    public class Create_ItemType : ScenarioItemType
    {
        private Dictionary<string, DataValue> Attributes;

        public void addAttribute(string name, DataValue dv)
        {
            Attributes[name] = dv;
        }

        public DataValue getAttribute(string name)
        {
            return Attributes[name];
        }


        public Create_ItemType()
        {
            Attributes = new Dictionary<string,DataValue>();
            base.eventType = "NewObject"; //not same as object type, used on Gabe's end.
        }

        //public Create_ItemType(UnitType unit)
        //    : base(unit)
        //{
        //    Attributes = new List<AttributeSettingType>();
        //    base.eventType = "NewObject"; //not same as object type, used on Gabe's end.
        //}

        //public Create_ItemType(UnitType u, List<AttributeSettingType> atts) :
        //    base(u)
        //{
        //    Attributes = atts;
        //    base.eventType = "NewObject"; //not same as object type, used on Gabe's end.
        //}

        public void Add(string name, DataValue attr)
        {
            DataValue newAttr = new DataValue();

            newAttr = attr;

            Attributes[name] = newAttr;
        }
        /*
        public void Add(string attr, string set)
        {
            attributes.Add(new AttributeSettingType(attr, set));
        }
*/
        /*public new string SerializeToXML()
        {
            string returnString = "<" + base.eventType.ToString() + ">";
            int looper = 0;

            returnString += "<Parameter><Name>ID</Name><Value>" + unit.id + "</Value></Parameter>";
            returnString += "<Parameter><Name>Kind</Name><Value>" + unit.kind + "</Value></Parameter>";

            while (looper < attributes.Count)
            {
                returnString += "<Parameter><Name>" + attributes[looper].attribute.ToString() + "</Name>";
                returnString += "<Value>";




                looper++;
            }
            returnString += "</" + base.eventType.ToString() + ">";

            return returnString;
        }*/
    }

    public class MoveItemType : ScenarioItemType
    {
        private int Throttle;
        private VectorType Direction;

        public int throttle
        {
            get { return Throttle; }
            set { Throttle = Math.Min(100, value); }
        }

        public VectorType direction
        {
            get { return Direction; }
            set { Direction = value; }
        }

        public MoveItemType()
        {
            unit = null;
            Throttle = 0;
            Direction = null;
        }

        public MoveItemType(UnitType u, int throttle, VectorType direction)
        {
            unit = u;
            Throttle = throttle;
            Direction = direction;
        }
    }

    public class MoveComplete_Item : ScenarioItemType
    {
        public MoveComplete_Item(UnitType incUnit)
        {
            unit = incUnit;
        }

        public MoveComplete_Item()
        { }
    }

    public class UnitType
    {
        private int? Id;
        private string Kind;

        public int? id
        {
            get { return Id; }
            set { Id = value; }
        }

        public string kind
        {
            get { return Kind; }
            set { Kind = value; }
        }

        public UnitType(int id, string kind)
        {
            Id = id;
            Kind = kind;
        }

        public UnitType()
        {
            Id = null;
            Kind = null;
        }
    }

    public class VectorType
    {
        private double X;
        private double Y;
        private double Z;

        public double x
        {
            get { return X; }
            set { X = value; }
        }

        public double y
        {
            get { return Y; }
            set { Y = value; }        
        }

        public double z
        {
            get { return Z; }
            set { Z = value; }
        }

        public VectorType()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public VectorType(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void NewDirection(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class AttributeSettingType
    {// no longer used, using Gabe's common component for abstract DataValue items
        private string Attribute;
        private string Setting;

        public string attribute
        { 
            get {return Attribute;}
            set { Attribute = value; }
        }

        public string setting
        {
            get { return Setting; }
            set { Setting = value; }
        }

        public AttributeSettingType()
        {
            Attribute = null;
            Setting = null;
        }

        public AttributeSettingType(string att, string setting)
        {
            Attribute = att;
            Setting = setting;
        }
    }


}
