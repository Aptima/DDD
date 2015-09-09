using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SeamateAdapter.ItemGenerators
{
    public class CourseGenerator : Generator
    {
        public CourseGenerator(DDDAdapter ddd)
            : base(ddd)
        {

        }

        public override void Generate(T_Item currentItem, String dmID)
        {
            dict = GetActionsAsDictionary(currentItem.Action);

            //This generator modifies move events e.g. keys, so write changes to this one because we'll be
            //iterating over the original.
            Dictionary<T_Move, T_Reveal> newDict = new Dictionary<T_Move,T_Reveal>();


            foreach (T_Move key in dict.Keys)
            {
                T_Move move = key;
                T_Reveal reveal = dict[key];
                
                bool isPirate = (GetOwner(move, reveal) == "Pirate DM");
                bool ambiguity = !(currentItem.Parameters.Threat == T_Threat.Unambiguous); //Vessel's desired perceived ambiguity
                //If it's a merchant and resources are available, always make merchants UNambiguous (so IFF is likely to be on). Per Courtney -Lisa
                if (!isPirate && currentItem.Parameters.PlayerResources == T_ResourceAvailability.Available)
                    ambiguity = false;

                //Is it in a sea lane?  If it is being newly revealed, yes because it's already in an entry region 
                //and entry regions are the ends of sea lanes. Otherwise, check. 
                List<PolygonValue> seaLanes = ddd.GetAllSeaLanes();
                bool locationInSeaLane = false;
                if (reveal != null) locationInSeaLane = true;
                else
                {
                    LocationValue location = GetLocation(move, reveal);
                    foreach (PolygonValue seaLane in seaLanes)
                    {
                        if (Polygon2D.IsPointInside(new Polygon2D(seaLane), new Vec2D(location)))
                            locationInSeaLane = true;
                    }
                }
                bool onInterceptCourse = isPirate;  //all pirates are on intercept course, no merchant can be
                bool hasIFFon = false;
                if (reveal == null) //Object already exists in play
                {
                    String displayName = ddd.GetSeamateObject(move.ID).ObjectName; 
                    if (!displayName.StartsWith("unknown")) hasIFFon = true;
                }


                //Pick row from ambiguity table using existing constraints.
                bool[] selectedRow = PickRowFromAmbiguityTable(onInterceptCourse, hasIFFon, locationInSeaLane, ambiguity);


                reveal = SetIFF(selectedRow[0], isPirate, currentItem.Parameters.Threat, reveal); //alters reveal event
                move.Throttle = GetNewSpeed(selectedRow[1], move);
                move.Location.Item = GetNewDestination(selectedRow[2], onInterceptCourse).ToLocationValue();
                
                //Add modified move and reveal to new dictionary.
                newDict[move] = reveal;
            }
            currentItem.Action = GetActionsFromDictionary(newDict);
        }


        /// <summary>
        /// Sets IFF on or off.  This is a setting in the reveal event.
        /// 
        /// </summary>
        /// <param name="on"></param>
        /// <param name="isPirate"></param>
        /// <param name="reveal"></param>
        protected T_Reveal SetIFF(bool on, bool isPirate, T_Threat threat, T_Reveal reveal)
        {
            //If we're revealing a new vessel, we can set its IFF on if we need to.  Default is off so we don't 
            // have to do anything.  We can't change existing ships.
            if (reveal != null)
            {
                if (on && !isPirate)
                    SetIFFOn(isPirate, reveal);

                //If it's a pirate, ignore what it says in the ambiguity table.  
                //Unambiguous == IFF ON always.  Ambiguous = IFF OFF always
                if (isPirate && threat == T_Threat.Unambiguous)
                    SetIFFOn(isPirate, reveal);

            }
            return reveal;
        }

        /// <summary>
        /// Returns a speed for a vessel.  
        /// If speed is "on", the speed returned is between 69-84 m/s (or the vessel's limits)
        /// If speed is "off", the speed return is above 84 or below 69.
        /// </summary>
        /// <param name="on"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        protected double GetNewSpeed(bool on, T_Move move)
        {
            double maxSpeed = GetMaxSpeed(move);
            double newSpeed;
            if (on)  //speed 69-84 m/s
                newSpeed = random.Next(69, Math.Max((int)maxSpeed, 84));
            else
            {  //speed <69 or >84 m/s
                if (maxSpeed > 84)
                    newSpeed = random.Next(84, (int)maxSpeed);
                else
                    newSpeed = random.Next(20, 68);
            }
            return ((double)newSpeed / (double)maxSpeed);

        }

        /// <summary>
        /// Plots a new destination for a merchant or pirate.
        /// If a merchant's course is "on", they must travel to a port (entry region).  If "off", they travel to the boundary but not a port(per Courtney)
        /// If a pirate's course is "on", it's on intercept course.  This will be plotted later once we know where all the merchants are going.
        /// </summary>
        /// <param name="on"></param>
        /// <param name="onInterceptCourse"></param>
        /// <returns></returns>
        protected Vec2D GetNewDestination(bool on, bool onInterceptCourse)
        {
            if (on)
            {
                // Intercept course will be plotted later
                if (onInterceptCourse) return new Vec2D(0,0);
                // Send it to an entry region
                else return (new Polygon2D(entryRegions[random.Next(entryRegions.Count)])).PointInside();
            }
            else
            {
                //Send it anywhere on the boundary ... probably won't be in a sea lane
                return new Vec2D(GetPointOnBoundary(ddd.GetWholeScreenMinusSomalia()));
                //TODO:  Check that it is actually not in a sea lane.
            }
        }

        /// <summary>
        /// Picks a row from the "ambiguity table", trying to match existing characteristics of an item
        /// </summary>
        /// <param name="onInterceptCourse"></param>
        /// <param name="hasIFFon"></param>
        /// <param name="locationInSeaLane">item is currently located in sea lane</param>
        /// <param name="ambiguity"></param>
        /// <returns></returns>
        protected bool[] PickRowFromAmbiguityTable(bool onInterceptCourse, bool hasIFFon, bool locationInSeaLane, bool ambiguity)
        {
            Shuffle(ambiguityTable);
            bool[] selectedRow = null;
            foreach (bool[] row in ambiguityTable)
            {
                if ((!onInterceptCourse || row[2])          //if onInterceptCourse is true, only allow course to be "on"/true ... if onInterceptCourse is false, either value is OK
                     && (!hasIFFon || row[0])                //same as above for hasIFFon and IFF
                    && (!locationInSeaLane || row[3])       //same as above for inSeaLane and location
                    && ambiguity == row[4])                 // ambiguity must match

                    selectedRow = row;
            }

            if (selectedRow == null)
                selectedRow = ambiguityTable[random.Next(ambiguityTable.Count())]; //Pick a random row.  TODO: Think of more ideal alternative.

            return selectedRow;
        }
    }
}
