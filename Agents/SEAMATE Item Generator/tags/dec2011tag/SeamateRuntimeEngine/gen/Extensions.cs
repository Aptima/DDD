using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SeamateRuntimeEngine
{
    public class ActionBase
    {
        public virtual DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        { return null; }
    }
    public partial class T_ChangeState : ActionBase
    {
        public override DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        {
            ChangeStateEvent ev = new ChangeStateEvent();
            ev.ObjectID = this.ID;
            ev.StateName = this.State;
            ev.Time = this.TimeAfter + currentTime; //not sure why it parses an int to a string in the first place...
            return ev;
        }
    }
    public partial class T_ScriptedItem : ActionBase
    {
        public override DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        {
            //this currently does not send out an actual DDD event, it correlates to once which is already scripted.
            return null;
        }
    }
    public partial class T_Move : ActionBase
    {
        public override DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        {
            MoveEvent ev = new MoveEvent();
            if(this.Location.Item is T_AbsolutePosition)
            {
                ev.DestinationLocation = ((T_AbsolutePosition)this.Location.Item).ToLocationValue();
            }else if(this.Location.Item is T_RelativePosition)
            {
                //get the relative object location, distance constraint, and/or absolute position
                Console.WriteLine("TODO Implement this!");
                return null;
            }
            else if (this.Location.Item is LocationValue)
            {
                ev.DestinationLocation = (LocationValue)this.Location.Item;
            }
            else
            {
                Console.WriteLine("TODO Fix this!");
                return null;
            }
            ev.ObjectID = this.ID;
            ev.Time = this.TimeAfter + currentTime;
            ev.Throttle = Throttle;
            return ev;
        }
    }
    public partial class T_Reveal : ActionBase
    {
        public override DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        {
            RevealEvent ev = new RevealEvent();
            if (this.Location.Item is T_AbsolutePosition)
            {
                ev.Location = ((T_AbsolutePosition)this.Location.Item).ToLocationValue();
            }
            else if (this.Location.Item is T_RelativePosition)
            {
                //get the relative object location, distance constraint, and/or absolute position
                Console.WriteLine("TODO Implement this!");
                return null;
            }
            else if (this.Location.Item is LocationValue)
            {
                ev.Location = (LocationValue)this.Location.Item;
            }
            else
            {
                Console.WriteLine("TODO Fix this!");
                return null;
            }
            ev.ObjectID = this.ID;
            ev.State = this.State;
            ev.Time = this.TimeAfter + currentTime;

            //optional params
            if (this.Owner != string.Empty && this.Owner != null)
            {
                ev.OwnerID = this.Owner;
            }
            if (this.Type != String.Empty && this.Type != null)
            {
                ev.ObjectType = this.Type;
            }
            if (this.StartupParameters.Items.Count() > 0)
            {
                for (int x = 0; x < this.StartupParameters.Items.Count() - 1; x += 2)
                {
                    try
                    {
                        ev.StartupParameters.Add(StartupParameters.Items[x], ddd.GetCorrectDataValue(StartupParameters.Items[x], StartupParameters.Items[x + 1])); //this should work
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while parsing a startup parameter. " + ex.Message);
                    }
                }
            }

            return ev;
        }
    }
    public partial class T_Interaction : ActionBase
    {
        public override DDDEvent ToDDDEvent(int currentTime, DDDAdapter ddd)
        {
            InteractionEvent ev = new InteractionEvent();
            ev.CapabilityName = this.Capability;
            ev.ObjectID = this.Instigator;
            ev.TargetID = this.Target;
            ev.Time = this.TimeAfter + currentTime;

            return ev;
        }
    }
    public partial class T_Location
    {
        public override string ToString()
        {
            if (Item == null)
                return "NULL Location";
            if (Item is T_AbsolutePosition)
            {
                return String.Format("{0:0.00},{1:0.00},{2:0.00}", ((T_AbsolutePosition)Item).X, ((T_AbsolutePosition)Item).Y, ((T_AbsolutePosition)Item).Z);
            }
            if (Item is T_RelativePosition)
            {
                return "Some relative position";
            }
            if (Item is DataValue)
            {
                LocationValue lv = Item as LocationValue;
                if (lv.exists == false)
                    return "Given no location";
                return String.Format("{0:0.00},{1:0.00},{2:0.00}", lv.X, lv.Y, lv.Z);
            }

            return "Unable to calculate Location";
        }
    }
    public partial class T_AbsolutePosition
    {
        public LocationValue ToLocationValue()
        {
            LocationValue lv = new LocationValue();
            lv.X = Convert.ToDouble(this.X);
            lv.Y = Convert.ToDouble(this.Y);
            lv.Z = Convert.ToDouble(this.Z);
            lv.exists = true;

            return lv;
        }
    }
    public partial class T_Item
    {
        public T_Item DeepCopy()
        {
            T_Item newItem;
            Exception ex;
            bool result = T_Item.Deserialize(this.Serialize(), out newItem, out ex);
            if (!result || ex != null)
            {
                return null;
            }

            return newItem;
        }
    }
}
