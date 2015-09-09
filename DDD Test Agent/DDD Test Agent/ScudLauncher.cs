using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
namespace DDD_Test_Agent
{
    public class Location
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Location(double x, double y)
        {
            this.x = (int)Math.Truncate(x);
            this.y = (int)Math.Truncate(y);
        }
        public Location(Location p)
        {
            this.x = p.x;
            this.y = p.y;
        }
        public Boolean Near(Location p)
        {
            return ((Math.Abs(this.x - p.x)<15) && (Math.Abs(this.y - p.y)<15));
        }
        public Boolean Equals(int x, int y)
        {
            return ((this.x == x) && (this.y == y));
        }
    }
    class ScudLauncher
    {
        private static SimulationModelReader modelReader = new SimulationModelReader();
        private static SimulationModelInfo simModel;

       private static int nextEventAt = 0;
        public static void Tick(int tick)
        {
            if (tick> nextEventAt+2000)
            {
                nextEventAt = tick + 1000*(randInt(13,37));
                simModel = modelReader.readModel("C:\\Program Files\\Aptima\\DDD 4.0\\Client\\SimulationModel.xml");
            }
            else if (Math.Truncate(tick/1000.0) == Math.Truncate(nextEventAt/1000.0))
            {// do something now
                // first update nextEventTime
                nextEventAt +=  1000*randInt(3,20);
                if (allUnits.Keys.Count > 0)
                {
                    string[] keyArray = new string[allUnits.Keys.Count];
                    allUnits.Keys.CopyTo(keyArray, 0);
                    string unitToUse = keyArray[randInt(keyArray.Length)];
                    SimulationEvent simEvent;
                    switch (randInt(2))
                    {

                        case 0://move a unit
                            Location whereToGo = allUnits[unitToUse].newLocation();
                            Console.WriteLine("Moving " + unitToUse + " from (" + allUnits[unitToUse].position.X + "," + allUnits[unitToUse].position.Y + ") to (" + whereToGo.X.ToString() + "," + whereToGo.Y.ToString() + ")");


                            // Generate a moveObject request
                            simEvent = SimulationEventFactory.BuildEvent(ref simModel, "MoveObjectRequest");
                            simEvent["UserID"] = DataValueFactory.BuildString("red dm");
                            simEvent["ObjectID"] = DataValueFactory.BuildString(unitToUse);
                            simEvent["DestinationLocation"] = DataValueFactory.BuildLocation((double)whereToGo.X, (double)whereToGo.Y, 0.0, true);
                            simEvent["Throttle"] = DataValueFactory.BuildDouble(randInt(75, 101) / 100.0);
                            simEvent["Time"] = DataValueFactory.BuildInteger(tick + 3000);// '+3000" is not magic -- just a clumsy attempt to avoid a race condition
                            EventGetter.Network.PutEvent(simEvent);
                            break;
                        case 1: //attack a unit

                            string myTarget = Target.GetRandom();
                            if ("" != myTarget)
                            {
                                Console.WriteLine("Using " + unitToUse + " to attack " + myTarget);


                                //generate an attack request
                                simEvent = SimulationEventFactory.BuildEvent(ref simModel, "AttackObjectRequest");
                                simEvent["UserID"] = DataValueFactory.BuildString("red dm");

                                simEvent["ObjectID"] = DataValueFactory.BuildString(unitToUse);
                                simEvent["TargetObjectID"] = DataValueFactory.BuildString(myTarget);
                                simEvent["CapabilityName"] = DataValueFactory.BuildString("Missile");
                                simEvent["Time"] = DataValueFactory.BuildInteger(tick + 3000);// '+3000" is not magic -- just a clumsy attempt to avoid a race condition
                                EventGetter.Network.PutEvent(simEvent);
                            }
                            
                            break;
                    }
                }

            }


        }
        private static Regex identifier = new Regex("^Red");
        public static Boolean AgentControls(string id)
        {
            Boolean returnValue = false;
            if (ScudLauncher.identifier.IsMatch(id))
                returnValue = true;
            return returnValue;

        }
        public static ScudLauncher GetScudLauncher(string iD)
        {
            return allUnits[iD];
        }
        private static Dictionary<string, ScudLauncher> allUnits = new Dictionary<string, ScudLauncher>();
        public static void AddUnit(string iD,double X, double Y)
        {
            allUnits.Add(iD, new ScudLauncher());
            allUnits[iD].position=new Location(X,Y);
        }
        public static void DropUnit(string iD)
        {
            if (allUnits.ContainsKey(iD))
                allUnits.Remove(iD);
        }
        private Boolean inMotion = false;
        public Boolean InMotion
        {
            get { return inMotion; }
            set { inMotion = value; }
        }
        private Location position;
        public Location Position
        {
            get { return position; }
            set { position = value; }
        }
        private static Dictionary<string, Location> corners;
        public static Dictionary<string, Location> Corners
        {
            get { return corners; }
            set { corners = value; }
        }
        static Random randomGenerator = new Random();
        /// <summary>
        /// randInt generates a random integer that can take and value in low,...,high-1
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        private static int randInt(int low, int high)
        {
            return low + randInt(high - low);

        }
        private static int randInt(int high)
        {
            double randDbl = randomGenerator.NextDouble();
            return (int)Math.Truncate(randDbl * high);
        }
        public static Boolean IsMoving(string unitId)
        {
            return allUnits[unitId].inMotion;
        }
        public static void SetMovement(string unitId,Boolean motion)
        {
            allUnits[unitId].inMotion = motion;
           
        }
        public static void SetLocation(string unitId, Location p)
        {
            allUnits[unitId].position.X = p.X;
            allUnits[unitId].position.Y = p.Y;
        }
        private static string nearCorner(Location p)
        {
            string returnValue = "";
            if (p.Near(corners["SW"]))
                returnValue = "SW";
            else if (p.Near(corners["NW"]))
                returnValue = "NW";
            else if (p.Near(corners["SE"]))
                returnValue = "SE";
            else if (p.Near(corners["NE"]))
                returnValue = "NE";
            return returnValue;
        }


        /// <summary>
        /// onSide assumes point is not at a corner
        /// </summary>
        /// <param name="p">current locatiob\n</param>
        /// <returns>"S","E","W",or "N"</returns>
        private static string onSide(Location p)
        {
            // since not a corner, only one of the values can match a value at a corner
            string returnValue;
            if (Math.Abs( p.X - corners["NW"].X)<15)
                returnValue = "W";
            else if (Math.Abs(p.X - corners["NE"].X)<15)
                returnValue = "E";
            else if (Math.Abs(p.Y - corners["NE"].Y)<15)
                returnValue = "N";
            else returnValue = "S";
            return returnValue;
        }
        /// <summary>
        /// newLocation generates a location along the box that a unit can move to
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Location newLocation()
        {

            Location returnValue;

            Location p = this.position;
            string corner = nearCorner(p);
            if ("" == corner)
            {// along a side
                string side = onSide(p);
                switch (side)
                {
                    case "S":
                    case "N":
                        returnValue = new Location(randInt(corners["SW"].X, 1 + Corners["SE"].X), p.Y);
                        break;
                    default: //"E" or "W" side
                        returnValue = new Location(p.X, randInt(Corners["SW"].Y, 1 + Corners["NW"].Y));
                        break;
                }
            }
            else
            {
                //Disallow move from corner to corner
                int moveX = randInt(1 + Corners["SW"].X, Corners["SE"].X);
                int moveY = randInt(1 + Corners["SW"].Y, Corners["NW"].Y);
                int choiceOfMove = randInt(2);
                switch (choiceOfMove)
                {
                    case 0:
                        returnValue = new Location(p.X, moveY);
                        break;
                    default:// without a default case compiler thinks returnValue might not be assigned!
                        returnValue = new Location(moveX, p.Y);
                        break;
                }

            }
            return returnValue;
        }
 
    }

}
