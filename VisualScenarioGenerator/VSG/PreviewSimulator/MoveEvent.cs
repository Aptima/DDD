using System;
using System.Collections.Generic;
using System.Text;

namespace VSG.PreviewSimulator
{
    public class MoveEvent : AbstractPreviewEvent
    {
        public static MoveEvent Empty = new MoveEvent();
        public int MoveDuration = -1;
        private float _throttle = 0;

        public float Throttle
        {
            get
            {
                return _throttle;
            }
            set
            {
                _throttle = value / 100f;
            }
        }

        public MoveEvent()
        {
            ID = string.Empty;
            Name = string.Empty;
            Time = 0;
            Throttle = 0;
            X = Y = Z = 0;
        }

        public MoveEvent(string ID, string Name, int Time, float Throttle, float X, float Y, float Z, int time_of_next_event)
        {
            this.ID = ID;
            this.Name = Name;
            this.Time = Time;
            this._throttle = Throttle / 100f;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            if (time_of_next_event != -1)
            {
                this.MoveDuration = time_of_next_event - Time;
            }
            else
            {
                MoveDuration = -1;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is MoveEvent)
            {
                return (
                    (((MoveEvent)obj).Name == Name) &&
                    (((MoveEvent)obj).ID == ID) &&
                    (((MoveEvent)obj).Time == Time) &&
                    (((MoveEvent)obj).Throttle == Throttle) &&
                    (((MoveEvent)obj).X == X) &&
                    (((MoveEvent)obj).Y == Y) &&
                    (((MoveEvent)obj).Z == Z)
                    );
            }
            return false;
        }
        public static bool operator ==(MoveEvent e1, MoveEvent e2)
        {
            return (
                (e1.Name == e2.Name) &&
                (e1.ID == e2.ID) &&
                (e1.Time == e2.Time) &&
                (e1.Throttle == e2.Throttle) &&
                (e1.X == e2.X) &&
                (e1.Y == e2.Y) &&
                (e1.Z == e2.Z)
                );
        }
        public static bool operator !=(MoveEvent e1, MoveEvent e2)
        {
            return !(
                (e1.Name == e2.Name) &&
                (e1.ID == e2.ID) &&
                (e1.Time == e2.Time) &&
                (e1.Throttle == e2.Throttle) &&
                (e1.X == e2.X) &&
                (e1.Y == e2.Y) &&
                (e1.Z == e2.Z)
                );
        }


        public void ToString()
        {
            Console.WriteLine("MoveEvent: {0}: {1}", Name, ID);
            Console.WriteLine("Time: {0}", Time);
            Console.WriteLine("Duration: {0}", MoveDuration);
            Console.WriteLine("Throttle: {0}", Throttle);
            Console.WriteLine("XPos: {0}", X);
            Console.WriteLine("YPos: {0}", Y);
            Console.WriteLine("ZPos: {0}", Z);
        }
    }
}
