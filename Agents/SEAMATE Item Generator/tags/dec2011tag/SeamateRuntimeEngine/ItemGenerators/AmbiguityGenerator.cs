using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SeamateRuntimeEngine.ItemGenerators
{
    public class AmbiguityGenerator : Generator
    {
        public AmbiguityGenerator(DDDAdapter ddd)
            : base(ddd)
        {

        }
        public bool SpecialGenerate(T_Item currentItem, String dmID)
        {

            Dictionary<T_Move,T_Reveal> dict = GetActionsAsDictionary(currentItem.Action);
            Dictionary<T_Move, T_Reveal> newDict = new Dictionary<T_Move,T_Reveal>();
            
            List<PolygonValue> seaLanes = ddd.GetAllSeaLanes();
            PolygonValue domain = this.ddd.GetDecisionMakersAreaOfResponsibility(dmID);


            foreach (T_Move key in dict.Keys)
            {
                T_Move move = key;
                T_Reveal reveal = dict[key];
                

                //Is this vessel a pirate?
                bool isPirate = false;
                if (reveal!=null) //there's a reveal event
                {
                    if (reveal.Owner == "Pirate DM")
                        isPirate = true;
                } 
                else if (ddd.GetSeamateObject(move.ID).Owner == "Pirate DM")
                    isPirate = true;


                //Vessel's desired perceived ambiguity
                bool ambiguity = true;
                if (currentItem.Parameters.Threat == T_Threat.Unambiguous)
                    ambiguity = false;
                //If it's a merchant and resources aren't unavailable, always make merchants UNambiguous (so IFF is likely to be on)
                if (!isPirate && currentItem.Parameters.PlayerResources == T_ResourceAvailability.Available)
                    ambiguity = false;


                //Is it in a sea lane?  If it is already in play check, Yes if it is being revealed in an entry region
                bool locationInSeaLane = false;
                LocationValue location;
                if (reveal == null) //already in play
                    location = ddd.GetSeamateObject(move.ID).Location;
                else
                    location = (LocationValue)reveal.Location.Item;

                foreach (PolygonValue seaLane in seaLanes)
                {
                    Polygon2D seaLane2D = new Polygon2D(seaLane);
                    if (Polygon2D.IsPointInside(seaLane2D, new Vec2D(location)))
                    {
                        locationInSeaLane = true;
                        break;
                    }
                }

                //Intercept course
               bool onInterceptCourse = false;
               if (currentItem.Parameters.ThreatType == T_ThreatType.Imminent && isPirate)
                    onInterceptCourse = true;

                //IFF
               bool hasIFFon = false;

                //Is its IFF already on?
                if (move == null) //Object already exists in play
                {
                    String displayName = ddd.GetSeamateObject(move.ID).ObjectName; 
                    if (!displayName.StartsWith("unknown")) //Object has IFF set on
                        hasIFFon = true;
                }


                //Pick row from ambiguity table using existing constraints.
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
                {
                    return false;
                    //Console.WriteLine("Impossible scenario");
                    //selectedRow = ambiguityTable[random.Next(ambiguityTable.Length)];
                    //TODO: Wipe out actions, go back to Threat Generator and make a new object
                }
               // Console.WriteLine("AmbiguityGenerator: Making settings for an " + currentItem.Parameters.ThreatType.ToString() + " vessel.  IFF should be on?" + selectedRow[0]);

                //Create scenario described in ambiguity table

                //IFF =================================
                //If we're revealing a new vessel, we can set its IFF on if we need to.  Default is off so we don't 
                // have to do anything.  We can't change existing ships.
                if (reveal != null)
                {
                    if (selectedRow[0] && !isPirate)
                        SetIFF(isPirate, reveal);

                    //If it's a pirate, ignore what it says in the ambiguity table.  
                    //Unambiguous == IFF ON always.  Ambiguous = IFF OFF always
                    if (isPirate && currentItem.Parameters.Threat == T_Threat.Unambiguous)
                        SetIFF(isPirate, reveal);
                    
                }

                //SPEED ===============================   
                double maxSpeed = GetMaxSpeed(move);

                double newSpeed;
                if (selectedRow[1])  //speed 69-84 m/s
                    newSpeed = random.Next(69, Math.Max((int)maxSpeed,84));
                else
                {  //speed <69 or >84 m/s
                    if (maxSpeed > 84)
                        newSpeed = random.Next(84, (int)maxSpeed);
                    else
                        newSpeed = random.Next(20, 68);
                }
                move.Throttle = ((double)newSpeed / (double)maxSpeed);



                //COURSE =================================
                if (selectedRow[2])
                {
                    if (onInterceptCourse) // we will take care of this later
                    {
                        
                    }
                    else //if (locationInSeaLane)
                    {
                        //pick any entry region and send it there.
                        Polygon2D entryRegion2D = new Polygon2D(entryRegions[random.Next(entryRegions.Count)]);
                        Vec2D point = entryRegion2D.PointInside();
                        move.Location = new T_Location();
                        move.Location.Item = point.ToLocationValue();
                    }
                }
                else
                {
                    //just send it anywhere ... probably won't be in a sea lane
                    Vec2D point = new Polygon2D(domain).PointInside(); //TODO: this should be in whole scenario boundaries, not just DM domain
                    move.Location = new T_Location();
                    move.Location.Item = point.ToLocationValue();

                    //TODO:  Send it to the edge of the universe.  Just kidding ... to edge of sea region.
                    ddd.GetDecisionMakersAreaOfResponsibility("BAMS DM");
                    ddd.GetDecisionMakersAreaOfResponsibility("Firescout DM");

                }
                //Add modified move and reveal back to dictionary.
                newDict[move] = reveal;
            }
            currentItem.Action = GetActionsFromDictionary(newDict);
            return true;
        }
    }
}
