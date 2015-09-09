using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using System.Collections;

namespace SeamateRuntimeEngine.ItemGenerators
{
    public class Generator
    {
        public Random random; 
        public DDDAdapter ddd;
        public T_Move primaryMove = null;
        public T_Reveal primaryReveal = null;
        public T_Move secondaryMove = null;
        public T_Reveal secondaryReveal = null;
        public bool isPirate = false;

        public List<PolygonValue> entryRegions;
        public List<DDDAdapter.SeamateObject> revealedSeaVessels;
        public bool[][] ambiguityTable;
        public LocationValue location;

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

        //public void IdentifyActions(Object[] actions)
        //{

        //    if (actions.Length == 0) return;

        //    //What we will have at this point: one pirate (primary), one merchant (primary), or one pirate (primary) and one merchant (secondary).
        //    bool doSecondPass = false;
        //    foreach (Object action in actions)
        //    {
        //        //Get move actions
        //        T_Move tempMove = action as T_Move;

        //        if (tempMove != null) //it's a move 
        //        {
        //            DDDAdapter.SeamateObject seamateObject = ddd.GetSeamateObject(tempMove.ID);

        //            if (seamateObject != null) //object is already in play
        //            {

        //                if (seamateObject.Owner == "Pirate DM")
        //                {
        //                    isPirate = true;
        //                    //Pirate is always primary if he exists
        //                    if (primaryMove == null) primaryMove = tempMove;
        //                    else
        //                    {
        //                        secondaryMove = primaryMove;
        //                        primaryMove = tempMove;
        //                    }
        //                }
        //                else //it's a merchant
        //                {
        //                    if (primaryMove == null) primaryMove = tempMove;
        //                    else secondaryMove = tempMove;
        //                }
        //            }
        //            else //object is not yet in play - has just been revealed 
        //            {
        //                //how do we figure out who owns it?!!
        //                doSecondPass = true;
        //            }
        //        }
        //        else
        //        {
        //            T_Reveal tempReveal = action as T_Reveal;
        //            if (tempReveal.Owner == "Pirate DM")
        //            {
        //                isPirate = true;
        //                if (primaryReveal == null) primaryReveal = tempReveal;
        //                else
        //                {
        //                    secondaryReveal = primaryReveal;
        //                    primaryReveal = tempReveal;
        //                }
        //            }
        //            else //merchant
        //            {
        //                if (primaryReveal == null) primaryReveal = tempReveal;
        //                else secondaryReveal = tempReveal;
        //            }
        //        }
        //    }

        //    //Do second pass only to match up reveals with unmarked move actions
        //    if (doSecondPass)
        //    {
        //        foreach (Object action in actions)
        //        {
        //            T_Move tempMove = action as T_Move;
        //            if (tempMove != null && ddd.GetSeamateObject(tempMove.ID) == null)
        //            {
        //                if (primaryReveal.ID == tempMove.ID)
        //                    primaryMove = tempMove;
        //                else secondaryMove = tempMove;
        //            }
        //        }
        //    }

        //    if (primaryReveal != null)
        //        location = (LocationValue)primaryReveal.Location.Item;
        //    else
        //        location = ddd.GetSeamateObject(primaryMove.ID).Location;
        //}

        /// <summary>
        /// Goes through the list of actions to create a dictionary of reveal and move events.
        /// </summary>
        /// <param name="actions"></param>
        public Dictionary<T_Move,T_Reveal> GetActionsAsDictionary(Object[] actions)
        {
            Dictionary<T_Move, T_Reveal> dict = new Dictionary<T_Move, T_Reveal>();

            if (actions.Length == 0) return dict;

            //What we will have at this point: one pirate (primary) and a bunch of merchants!

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
                if (tempReveal != null) //&& ddd.GetSeamateObject(tempMove.ID) == null) // ? weird? 
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

        public Object[] GetActionsFromDictionary(Dictionary<T_Move,T_Reveal> dict) {
          //  Object[] array = new Object[(dict.Keys.Count * 2)];
            List<Object> list = new List<Object>();
            foreach (T_Move key in dict.Keys) {
                list.Add(key);
                list.Add(dict[key]);
            }
            return list.ToArray();
        }

        public LocationValue GetLocation(T_Move move, T_Reveal reveal) {
            if (reveal == null)
                return ddd.GetSeamateObject(move.ID).Location;
            else
                return (LocationValue)reveal.Location.Item;
        }

        public double GetMaxSpeed(T_Move move)
        {
            DDDAdapter.SeamateObject seamateObject = ddd.GetSeamateObject(move.ID);
            if (seamateObject.MaximumSpeed > 0)
                return seamateObject.MaximumSpeed;
            String className = ddd.GetSeamateObject(move.ID).ClassName;
            //if (className.Trim() == "") //handles erroneous blank className, remove this code when Adam fixes
            //    return 75;
            Dictionary<string, DataValue> speciesStateParameters = ddd.GetSpeciesStateParameters(className, "FullyFunctional");
            return ((DoubleValue)speciesStateParameters["MaximumSpeed"]).value;
        }

        public String GetOwner(T_Move move, T_Reveal reveal)
        {
            if (reveal == null)
                return ddd.GetSeamateObject(move.ID).Owner;
            else
                return reveal.Owner;

        }

        

        /// <summary>
        /// Criteria under which to add new vessels.
        /// Try and reuse vessels: always if count > 20, with 40% probability if count >10, otherwise never.
        /// </summary>
        /// <param name="revealedSeaVessels"></param>
        /// <returns></returns>
        public bool shouldAddNewVessel(List<DDDAdapter.SeamateObject> revealedSeaVessels) {

            return (revealedSeaVessels.Count > 20 || ((random.Next(1, 10) < 5) && revealedSeaVessels.Count > 10));
        }

        /// <summary>
        /// Each generator will add to the referred item based on the current state of the item, and the difficulty values
        /// </summary>
        /// <param name="currentItem"></param>
        public virtual void Generate(T_Item currentItem, String DM_ID)
        {

        }

        public Vec2D GetInterceptPoint(Vec2D targetStart, Vec2D targetEnd, double targetSpeed, Vec2D start, double speed)
        {
            Vec2D D_o = start.Add(targetStart.Multiply(-1)); //Vector from vessel to target start
            //Vec2D D_o = targetStart.Add(start.Multiply(-1)); //Vector from vessel to target start
            double d_o = (Math.Sqrt(Math.Pow(D_o.X, 2)+ Math.Pow(D_o.Y, 2)));
            Vec2D V_m = targetEnd.Add(targetStart.Multiply(-1)); 
            Vec2D unitVectorTarget = new Vec2D(V_m);
            unitVectorTarget.Normalize();
            V_m = unitVectorTarget.Multiply(targetSpeed);//Vector target velocity
            Console.WriteLine("TargetStart " + targetStart.ToString() + "TargetEnd " + targetEnd.ToString() + "targetVelocity " + V_m.ToString() + "pirateStart" + start.ToString() + "pirateSpeed "+speed);
  
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
            if (x1 > 0 && x2 > 0)
                interceptDistance = Math.Min(x1, x2);
            else
                interceptDistance = Math.Max(x1, x2);

            Vec2D interceptPoint;
            if (interceptDistance > targetEndToStartDistance) //vessel can't catch target in the length of the move
                interceptPoint = targetEnd;
            else
                interceptPoint = unitVectorTarget.Multiply(interceptDistance).Add(targetStart);
            Console.WriteLine("intercept point = "+interceptPoint.ToString());
            return interceptPoint;     
        }

        

        public Vec2D GetInterceptPointOld(Vec2D lineStart, Vec2D lineEnd, Vec2D point)
        {
            double x1 = lineStart.X;
            double y1 = lineStart.Y;
            double x2 = lineEnd.X;
            double y2 = lineEnd.Y;
            double x3 = point.X;
            double y3 = point.Y;
            if (x1 == x2) //vertical line
                if (((y1 < y2) && (y1 < y3) && (y3 < y2)) || ((y1 > y2) && (y2 < y3) && (y3 < y2)))
                    //The perpendicular intersector lies on the line segment
                    return new Vec2D(x1, y3);
                else
                    return lineEnd;
            double slope = (y2 - y1) / (x2 - x1);
            double x = ((1 / slope) * x3 + slope * x1 - y1 + y3) / (slope + (1 / slope));
            double y = slope * x - slope * x1 + y1;
            if ((((x1 < x2) && (x1 < x) && (x < x2)) || ((x1 > x2) && (x2 < x) && (x < x2))) &&
                (((y1 < y2) && (y1 < y) && (y < y2)) || ((y1 > y2) && (y2 < y) && (y < y2))))
                //intersection point is between ends of line segment
                return new Vec2D(x, y);
            else
            {

                return lineEnd;
            }
        }


        //Returns a kind of random int.  It has probabilityOfBase % chance of being "baseNumber" then evenly varies over the rest of the range.
        //Eventually change to a linear variation?
        public int changingNumber(int baseNumber, int probabilityOfBase, int lowerBound, int upperBound)
        {
            Random rand = new Random();
            if (rand.Next(100) > probabilityOfBase)
            {
                return rand.Next(lowerBound, upperBound);
            }
            return baseNumber;
        }

        public bool IsPointInsideMultipleDomains(List<Polygon2D> domains, Vec2D point) {
            foreach (Polygon2D domain in domains)
            {
                if (!Polygon2D.IsPointInside(domain,point))
                    return false;
            }
            return true;
        }



        public void SetIFF(bool isPirate, T_Reveal reveal)
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
