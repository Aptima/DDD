using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using System.Collections;

namespace SeamateAdapter.ItemGenerators
{



    public class Generator
    {
        protected Random random;
        protected List<object> actions;
        protected Dictionary<T_Move, T_Reveal> dict; //Dictionary of actions
        protected DDDAdapter ddd;
        protected List<PolygonValue> entryRegions;
        protected List<DDDAdapter.SeamateObject> revealedSeaVessels;
        protected bool[][] ambiguityTable;

        protected int oneGroupingDistance = Properties.Settings.Default.oneGroupingDistance; //maximum
        protected int twoGroupingDistance = Properties.Settings.Default.twoGroupingDistance;  //minimum

        public Generator(DDDAdapter ddd)
        {
            this.ddd = ddd;
            this.random = new Random();
            this.entryRegions = ddd.GetAllEntryRegions();
            this.revealedSeaVessels = ddd.GetAllRevealedSeaVessels();

            /*
   IFF	Speed  	Course	location	Ambiguity
    */
            this.ambiguityTable = new bool[][] {
                    new bool[] {false, true, false, false, true},
                    new bool[] {true, true, false, false, true},
                    new bool[] {true, false, false, false, true},
                    new bool[] {false, false, true, false, true},
                    new bool[] {false, false, false, true, true},
                    new bool[] {false, false, false, false, true},
                    new bool[] {false, true,	true,	false,	true},
                    new bool[] {false, true, 	false,	true,	true},
                    new bool[] {true,	true,	true,	true,	false},
                    new bool[] {true,	false,	true,	true,	false},
                    new bool[] {true,	false,	false,	true,	false},
                    new bool[] {true,	true, 	false, 	true,	false},
                    new bool[] {true,	true,	true,	false,	false},
                    new bool[] {true,	false,	true,	false,	false},
                    new bool[] {false,	false,	true,	true,	false},
                    new bool[] {false,	true,	true,	true,	false}
                                            };
        }

        /// <summary>
        /// Each generator will add to the referred item based on the current state of the item, and the difficulty values
        /// </summary>
        /// <param name="currentItem"></param>
        public virtual void Generate(T_Item currentItem, String DM_ID)
        {
            
        }

        /// <summary>
        /// Goes through the list of actions to create a dictionary of reveal and move events.
        /// The keys are move events, since every stimulus includes a move and indicates the vessel's ID.
        /// The values are reveals, which may or may not be null.
        /// </summary>
        /// <param name="actions">Array of actions from item</param>
        protected Dictionary<T_Move,T_Reveal> GetActionsAsDictionary(Object[] actions)
        {
            Dictionary<T_Move, T_Reveal> dict = new Dictionary<T_Move, T_Reveal>();

            if (actions.Length == 0) return dict;

            //What we will have at this point: both pirates and merchants

            //Set all moves as keys.  They are good keys because every vessel will be moved exactly once.
            foreach (Object action in actions)
            {
                T_Move tempMove = action as T_Move;
                if (tempMove != null) //it's a move 
                    dict.Add(tempMove, null);
            }

            //Set all reveals as values.  Some keys will have null values because some vessels are already in play.
            foreach (Object action in actions)
            {
                T_Reveal tempReveal = action as T_Reveal;
                if (tempReveal != null)  
                {
                    foreach (T_Move key in dict.Keys)
                    {
                        if (key.ID == tempReveal.ID)
                        {
                            dict[key] = tempReveal;
                            break;
                        }
                    }
                }
            }

            return dict;
        }

        protected Object[] GetActionsFromDictionary(Dictionary<T_Move,T_Reveal> dict) {
            List<Object> list = new List<Object>();
            foreach (T_Move key in dict.Keys) {
                list.Add(key);
                list.Add(dict[key]);
            }
            return list.ToArray();
        }

        protected LocationValue GetLocation(T_Move move, T_Reveal reveal) {
            if (reveal == null)
                return ddd.GetSeamateObject(move.ID).Location;
            else
                return (LocationValue)reveal.Location.Item;
        }

        protected double GetMaxSpeed(T_Move move)
        {
            DDDAdapter.SeamateObject seamateObject = ddd.GetSeamateObject(move.ID);
            if (seamateObject.MaximumSpeed > 0)
                return seamateObject.MaximumSpeed;
            String className = ddd.GetSeamateObject(move.ID).ClassName;
            if (className.Trim() == "") //handles erroneous blank className, remove this code when Adam fixes
                return 75;
            Dictionary<string, DataValue> speciesStateParameters = ddd.GetSpeciesStateParameters(className, "FullyFunctional");
            return ((DoubleValue)speciesStateParameters["MaximumSpeed"]).value;
        }

        protected double GetMaxSpeed(DDDAdapter.SeamateObject seamateObject)
        {
            
            if (seamateObject.MaximumSpeed > 0)
                return seamateObject.MaximumSpeed;
            String className = seamateObject.ClassName;
            if (className.Trim() == "") //handles erroneous blank className, remove this code when Adam fixes
                return 75;
            Dictionary<string, DataValue> speciesStateParameters = ddd.GetSpeciesStateParameters(className, "FullyFunctional");
            return ((DoubleValue)speciesStateParameters["MaximumSpeed"]).value;
        }

        protected String GetOwner(T_Move move, T_Reveal reveal)
        {
            if (reveal == null)
                return ddd.GetSeamateObject(move.ID).Owner;
            else
                return reveal.Owner;
        }
        
        /// <summary>
        /// Makes a random point on the boundary of a polygon.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        protected LocationValue GetPointOnBoundary(PolygonValue area)
        {
            int i = random.Next(area.points.Count);
            PolygonValue.PolygonPoint startPP = area.points[i];
            PolygonValue.PolygonPoint endPP;
            if (i == 0)
                endPP = area.points[i + 1];
            else
                endPP = area.points[i - 1];
            //Use slope intercept form 
            Vec2D start = new Vec2D(startPP.X, startPP.Y);
            Vec2D end = new Vec2D(endPP.X, endPP.Y);
            double slope = (end.Y - start.Y) / (end.X - start.X);
            double intercept = start.Y - start.X * slope;
            double newX = random.Next((int)Math.Min(start.X,end.X),(int)Math.Max(start.X,end.X)); //choose new X between existing X values
            double newY = slope * newX + intercept;
            return new Vec2D(newX, newY).ToLocationValue();
        }

        /// <summary> 
        /// Returns a random point on a line segment
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected Vec2D GetPointOnLine(Vec2D start, Vec2D end)
        {
            double slope = (end.Y - start.Y) / (end.X - start.X);
            double intercept = start.Y - start.X * slope;
            double newX = random.Next((int)Math.Min(start.X, end.X), (int)Math.Max(start.X, end.X)); //choose new X between existing X values
            double newY = slope * newX + intercept;
            return new Vec2D(newX, newY);
        }

        /// <summary>
        /// Returns a point which is the given distance from the start point, along a line segment defined by the start and end points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        protected Vec2D GetPointAlongLine(Vec2D start, Vec2D end, double distance)
        {
            double slope = (end.Y - start.Y) / (end.X - start.X);
            double intercept = start.Y - start.X * slope;
            double newX = random.Next((int)Math.Min(start.X, end.X), (int)Math.Max(start.X, end.X)); //choose new X between existing X values
            double newY = slope * newX + intercept;
            return new Vec2D(newX, newY);
        }

        /// <summary>
        /// Returns a random point at which a second vessel could start, in order to intercept the first in exactly the given time.
        /// The point will always be within in valid screen area (not offscreen).
        /// </summary>
        /// <param name="intercept">intercept point</param>
        ///  <param name="maxSpeed">Interceptor's max speed</param>
        /// <param name="minTimeToIntercept"></param>
        /// <param name="maxTimeToIntercept"></param>
        /// <returns></returns>
        protected Vec2D PointToStartForIntercept(Vec2D intercept, double time, double maxSpeed)
        {
            Polygon2D wholeScreen = new Polygon2D(ddd.GetWholeScreenMinusSomalia());
            //double targetTimeToFinishMove = intercept.ScalerDistanceTo(targetEnd) / targetSpeed;
            //double time = random.Next((int)minTimeToIntercept,Math.Min((int)maxTimeToIntercept,(int)targetTimeToFinishMove));
            double distance = time * maxSpeed;
            while (true) {
                double m = random.Next(-1000000, 1000000);//slope
                double x2 = intercept.X;
                double y2 = intercept.Y;
                double x1 = distance/Math.Sqrt(Math.Pow(m,2) +1) + x2;
                double y1 = m * x1 - m * x2 + y2;
                Vec2D possiblePoint = new Vec2D(x1,y1);
                if (Polygon2D.IsPointInside(wholeScreen, possiblePoint))
                    return possiblePoint;
            }
        }

        /// <summary>
        /// Takes an EXISTING merchant and pirate and returns their shortest potential intercept time.
        /// </summary>
        /// <param name="merchant"></param>
        /// <param name="pirate"></param>
        /// <returns></returns>

        protected double PotentialTimeToIntercept(DDDAdapter.SeamateObject pirate, DDDAdapter.SeamateObject merchant)
        {
            Vec2D pirateLocation = new Vec2D(pirate.Location);
            double pirateSpeed = GetMaxSpeed(pirate);

            double merchantSpeed = merchant.Throttle * merchant.MaximumSpeed;
            Vec2D merchantStart = new Vec2D(merchant.Location);
            Vec2D merchantDestination = new Vec2D(merchant.DestinationLocation);
            Vec2D interceptPoint = GetInterceptPoint(merchantStart, merchantDestination, merchantSpeed, pirateLocation, pirateSpeed);

            double timeToIntercept = merchantStart.ScalerDistanceTo(interceptPoint) / merchantSpeed;

            return timeToIntercept;
        }

        /// <summary>
        /// Criteria under which to add new vessels.
        /// Try and reuse vessels: 80% if count > 20, with 40% probability if count >10, otherwise never.
        /// </summary>
        /// <param name="revealedSeaVessels"></param>
        /// <returns></returns>
        protected bool shouldAddNewVessel(List<DDDAdapter.SeamateObject> revealedSeaVessels)
        {

            if ((revealedSeaVessels.Count > 20) && (random.Next(1, 10) < 3))
                return true;
            else if ((revealedSeaVessels.Count > 10) && (random.Next(1, 10) < 7))
                return true;
            else
                return false;
        }





        protected Vec2D GetInterceptPoint(Vec2D targetStart, Vec2D targetEnd, double targetSpeed, Vec2D start, double speed)
        {
            Vec2D D_o = start.Add(targetStart.Multiply(-1)); //Vector from vessel to target start
            //Vec2D D_o = targetStart.Add(start.Multiply(-1)); //Vector from vessel to target start
            double d_o = (Math.Sqrt(Math.Pow(D_o.X, 2)+ Math.Pow(D_o.Y, 2)));
            Vec2D V_m = targetEnd.Add(targetStart.Multiply(-1)); 
            Vec2D unitVectorTarget = new Vec2D(V_m);
            unitVectorTarget.Normalize();
            V_m = unitVectorTarget.Multiply(targetSpeed);//Vector target velocity
  
            double v_m = targetSpeed;
            double targetEndToStartDistance = targetEnd.ScalerDistanceTo(targetStart);
            double v_p = speed;
            Vec2D P = start;
            Vec2D M = targetStart;
            double cos_B = (D_o.X * V_m.X + D_o.Y * V_m.Y) / (d_o * v_m);
            double a = (Math.Pow((v_p / v_m), 2) - 1);
            double b = 2*(Math.Sqrt(Math.Pow(D_o.X, 2)+ Math.Pow(D_o.Y, 2)))*cos_B;
            double c = -1*Math.Pow(d_o,2);
            double x1 = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            double x2 = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            double interceptDistance;
            if (double.IsNaN(x1) && !double.IsNaN(x2))
                interceptDistance = x2;
            else if (double.IsNaN(x2) && !double.IsNaN(x1))
                interceptDistance = x1;
            else if (double.IsNaN(x1) && double.IsNaN(x2))
                interceptDistance = targetEndToStartDistance + 100; 
            else if (x1 > 0 && x2 > 0)
                interceptDistance = Math.Min(x1, x2);
            else
                interceptDistance = Math.Max(x1, x2);

            Vec2D interceptPoint;
            if ((interceptDistance > targetEndToStartDistance)) //vessel can't catch target in the length of the move
                interceptPoint = targetEnd;
            else
                interceptPoint = unitVectorTarget.Multiply(interceptDistance).Add(targetStart);
            return interceptPoint;     
        }

        /// <summary>
        /// Returns a boolean value expressing whether or not a vessel is stopped.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        protected bool IsStopped(DDDAdapter.SeamateObject vessel)
        {
            return vessel.Throttle == 0;
        }

        protected bool IsPointInsideMultipleDomains(List<Polygon2D> domains, Vec2D point)
        {
            foreach (Polygon2D domain in domains)
            {
                if (!Polygon2D.IsPointInside(domain,point))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a new move and reveal event to action list.  THis adds a previously unrevealed vessel into play
        /// Move event will be a "dummy" empty move with no destination defined yet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="location"></param>
        protected void CreateRevealAndEmptyMove(String id, String owner, Vec2D location)
        {
            T_Reveal reveal = new T_Reveal();
            reveal.ID = id;
            reveal.Owner = owner;
            reveal.State = "FullyFunctional";
            reveal.Location = new T_Location();
            reveal.StartupParameters = new T_StartupParameters();
            reveal.StartupParameters.Items = new string[0];
            reveal.Location.Item = location.ToLocationValue();

            T_Move move = new T_Move();
            move.ID = id;
            move.Location = new T_Location();

            actions.Add(reveal);
            actions.Add(move);
        }

        /// <summary>
        /// Adds a new move event to the action list.  Does not contain a destination yet
        /// </summary>
        /// <param name="vessel"></param>
        protected void CreateEmptyMove(DDDAdapter.SeamateObject vessel)
        {
            T_Move move = new T_Move();
            move.ID = vessel.ID;
            move.Location = new T_Location();
            actions.Add(move);
        }


        /// <summary>
        /// Sets a vessel's IFF on in the RevealEvent.
        /// </summary>
        /// <param name="isPirate">true for pirate, false for merchant</param>
        /// <param name="reveal"></param>
        protected void SetIFFOn(bool isPirate, T_Reveal reveal)
        {
            String iff = "";
            if (isPirate) iff = "Pirate";
            else iff = "Friendly";

            reveal.StartupParameters.Items = new string[2];
            reveal.StartupParameters.Items.SetValue("ObjectName", 0);
            reveal.StartupParameters.Items.SetValue(iff, 1);
            Console.WriteLine("Just set IFF for vessel " + reveal.ID);

        }

        /// <summary>
        /// Creates a new reveal event and returns it.
        /// </summary>
        /// <param name="id">Vessel ID</param>
        /// <param name="owner">Name of DM</param>
        /// <param name="location">Location for reveal</param>
        /// <returns></returns>
        protected T_Reveal MakeReveal(String id, String owner, Vec2D location)
        {
            T_Reveal reveal = new T_Reveal();
            reveal.ID = id;
            reveal.Owner = owner;
            reveal.State = "FullyFunctional";
            reveal.Location = new T_Location();
            reveal.StartupParameters = new T_StartupParameters();
            reveal.StartupParameters.Items = new string[0];
            reveal.Location.Item = location.ToLocationValue();
            return reveal;
        }

        /// <summary>
        /// Checks whether an existing pirate is either positioned within a 60s attack range of an existing vessel, or within 60s of an entry region.
        /// </summary>
        /// <param name="pirate"></param>
        /// <returns></returns>
        Boolean PirateIsWithinOneMinuteAttackRange(DDDAdapter.SeamateObject pirate, List<PolygonValue> entryRegions)
        {
            foreach (DDDAdapter.SeamateObject vessel in revealedSeaVessels)
            {
                //First compare all the existing merchants' positions to see if they are within range.
                if (vessel.Owner == "Merchant DM")
                {
                    if (PotentialTimeToIntercept(pirate, vessel) < 60) return true;
                }
                else continue; //ignore merchants or fleet ships
            }

            //There was no existing merchant in range.  Now we need to check if any entry region is in range
            //to possibly create a merchant there.  I do this by iterating over each boundary point and checking if any of them 
            //is reachable by the pirate in 60 seconds.

            foreach (PolygonValue entryRegion in entryRegions)
            {
                foreach (PolygonValue.PolygonPoint entryPoint in entryRegion.points)
                {
                    Vec2D point = new Vec2D(entryPoint.X, entryPoint.Y);
                    double distance = point.ScalerDistanceTo(new Vec2D(pirate.Location));
                    double timeToPoint = distance / pirate.MaximumSpeed;
                    if (timeToPoint < 60) return true;
                }
            }

            //If we got this far, there is no existing merchant or entry region in which to create a merchant
            //reachable by the pirate in 60 sec.
            return false;
        }
    

        /// <summary>
        /// Shuffles a list randomly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
