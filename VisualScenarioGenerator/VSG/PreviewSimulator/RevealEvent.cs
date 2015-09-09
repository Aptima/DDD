using System;
using System.Collections.Generic;
using System.Text;

namespace VSG.PreviewSimulator
{
    public class RevealEvent : AbstractPreviewEvent
    {
        public static RevealEvent Empty = new RevealEvent();
        public string Icon;
        public string ColorName;

        public RevealEvent()
        {
            this.ID = string.Empty;
            this.Name = string.Empty;
            this.Icon = string.Empty;
            this.Time = 0;
            this.X = Y = Z = 0;
        }
        public RevealEvent(string ID, string Name, string Icon, int Time, float X, float Y, float Z)
        {
            this.ID = ID;
            this.Name = Name;
            this.Icon = Icon;
            this.Time = Time;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is RevealEvent)
            {
                return (
                    (((RevealEvent)obj).Name == Name) &&
                    (((RevealEvent)obj).ID == ID) &&
                    (((RevealEvent)obj).Icon == Icon) &&
                    (((RevealEvent)obj).Time == Time) &&
                    (((RevealEvent)obj).X == X) &&
                    (((RevealEvent)obj).Y == Y) &&
                    (((RevealEvent)obj).Z == Z)
                    );
            }
            return false;
        }

        public static bool operator ==(RevealEvent e1, RevealEvent e2)
        {
            return (
                (e1.Name == e2.Name) &&
                (e1.ID == e2.ID) &&
                (e1.Icon == e2.Icon) &&
                (e1.Time == e2.Time) &&
                (e1.X == e2.X) &&
                (e1.Y == e2.Y) &&
                (e1.Z == e2.Z)
                );
        }
        public static bool operator !=(RevealEvent e1, RevealEvent e2)
        {
            return !(
                (e1.Name == e2.Name) &&
                (e1.ID == e2.ID) &&
                (e1.Icon == e2.Icon) &&
                (e1.Time == e2.Time) &&
                (e1.X == e2.X) &&
                (e1.Y == e2.Y) &&
                (e1.Z == e2.Z)
                );
        }

        public void ToString()
        {
            Console.WriteLine("RevealEvent: {0}: {1}", Name, ID);
            Console.WriteLine("Time: {0}", Time);
            Console.WriteLine("Icon: {0}", Icon);
            Console.WriteLine("XPos: {0}", X);
            Console.WriteLine("YPos: {0}", Y);
            Console.WriteLine("ZPos: {0}", Z);
        }
    }
}
