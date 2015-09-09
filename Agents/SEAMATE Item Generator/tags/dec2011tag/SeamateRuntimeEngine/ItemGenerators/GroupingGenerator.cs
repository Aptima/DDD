using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SeamateRuntimeEngine.ItemGenerators
{
    public class GroupingGenerator : Generator
    {

        private int oneGroupingDistance = 33000;
        private int twoGroupingDistance = 33000;

        public GroupingGenerator(DDDAdapter ddd)
            : base(ddd)
        {
            
        }
        public override void Generate(T_Item currentItem, String dmID)
        {
            //Identify what's already in actions list.
            foreach (Object action in currentItem.Action)
            {
                if (action as T_Move != null)
                    primaryMove = (T_Move)action;
                if (action as T_Reveal != null)
                    primaryReveal = (T_Reveal)action;
            }
            LocationValue location = GetLocation(primaryMove, primaryReveal);
            bool isPirate = GetOwner(primaryMove, primaryReveal)=="Pirate DM";
            PolygonValue pirateEntryRegion = null;
            if (isPirate)
            {
                foreach (PolygonValue region in entryRegions)
                {
                    if (Polygon2D.IsPointInside(new Polygon2D(region), new Vec2D(location)))
                    {
                        pirateEntryRegion = region;
                        break;
                    }
                }
            }


            List<Object> list = currentItem.Action.ToList();

            List<string> unrevealedObjectIDs = ddd.GetAllUnrevealedObjectIds(false, true, null);
            if (primaryReveal != null)
                unrevealedObjectIDs.Remove(primaryReveal.ID);

           
            int minStimuli = 0;

            //If threat type is imminent, we need to guarantee there will be a merchant for the pirate to attack.
            if (currentItem.Parameters.ThreatType == T_ThreatType.Imminent)
                minStimuli = 1;


            int maxStimuli;
            //If resources need to be available, then two is maximum of stimulus events.
            if (T_ResourceAvailability.Available == currentItem.Parameters.PlayerResources)
                maxStimuli = 2;
            //If resources are unavailable, there must be at least 3 total stimulus events, and max can be higher.
            else
            {
                minStimuli = 3;
                maxStimuli = 5;
            }

            int numStimuliToGenerate = this.changingNumber(minStimuli, 60, minStimuli, maxStimuli); //probably minStimuli but could be up to maxStimuli


            //If our probability criteria says we should add vessels, try and populate reusable objects list 
            List<DDDAdapter.SeamateObject> reuseableObjects = new List<DDDAdapter.SeamateObject>();
            if (shouldAddNewVessel(revealedSeaVessels))
                reuseableObjects = findObjectsInPlay(location, pirateEntryRegion, currentItem.Parameters.Groupings, false);

            for (int i = 0; i < numStimuliToGenerate; i++)
            {

                T_Move move = new T_Move();

                //Try and recycle objects to create stimulus.
                if (reuseableObjects.Count != 0)
                {
                    Shuffle(reuseableObjects);
                    DDDAdapter.SeamateObject objectForReuse = null;
                    //see if any eligible vessels are stopped, if so, use one of them
                    foreach (DDDAdapter.SeamateObject possiblyStoppedVessel in reuseableObjects)
                    {
                        if (possiblyStoppedVessel.Throttle == 0) //yes, it is stopped
                        {
                            //Console.WriteLine("GRoupingGenerator: Picking a stopped vessel " + possiblyStoppedVessel.ID + " to reuse.");
                            objectForReuse = possiblyStoppedVessel;
                            break;
                        }
                    }
                    //randomly pick one of the suitable objects to create a stimulus
                    if (objectForReuse == null)
                        objectForReuse = reuseableObjects[random.Next(reuseableObjects.Count)];

                    move.ID = objectForReuse.ID;
                    reuseableObjects.Remove(objectForReuse);
                    //Console.WriteLine("GroupingGenerator: Moving existing merchant " + objectForReuse.ID);

                }
                else //Reveal and move new merchant.
                {
                    T_Reveal reveal = new T_Reveal();
                    reveal.Owner = "Merchant DM";
                    reveal.State = "FullyFunctional";
                    reveal.StartupParameters = new T_StartupParameters();
                    reveal.StartupParameters.Items = new string[0];
                    reveal.ID = unrevealedObjectIDs[random.Next(unrevealedObjectIDs.Count)]; //pick a random new object
                    unrevealedObjectIDs.Remove(reveal.ID); //take it out of the available list

                    //ADDED 11/10/11:  To make sure that the merchants don't end up in the same entry region as a newly created pirate, 
                    //if there's a new pirate, instead of iterating over all entry regions we'll remove the pirate's region from the list first.
                    if (pirateEntryRegion != null)
                        entryRegions.Remove(pirateEntryRegion);

                    Vec2D point = null;

                    //If two grouping, pick points in entry regions until one is > 100 away
                    if (currentItem.Parameters.Groupings == T_Groupings.Two)
                    {
                        while (true)
                        {
                            point = new Polygon2D(entryRegions[random.Next(entryRegions.Count)]).PointInside();
                            if (point.ScalerDistanceTo(new Vec2D(location)) > twoGroupingDistance)
                            {
                                break;
                            }
                        }
                    }
                    //If one grouping, see if any entry region is within 50km.  If so, pick a point in that entry region.  
                    //It's possible that no entry region may be within 50km.  In that case, pick a point in the closest one.

                    if (currentItem.Parameters.Groupings == T_Groupings.One)
                    {
                        double closestDistance = 100000000;
                        PolygonValue closestRegion = null;
                        Shuffle(entryRegions);

                        foreach (PolygonValue entryRegion in entryRegions)
                        {
                            Polygon2D entry2D = new Polygon2D(entryRegion);
                            double distance = Polygon2D.ScalarDistanceToPolygon(entry2D, new Vec2D(location));

                            if (distance < oneGroupingDistance)
                            {
                                point = entry2D.PointInside();
                                break;
                            }
                            else
                            {
                                if (distance < closestDistance)
                                {
                                    closestDistance = distance;
                                    closestRegion = entryRegion;
                                }
                            }
                        }
                        if (point == null)
                        {
                            //Console.WriteLine("GroupingGenerator: Unable to place vessel in an entry region AND <100km away.  Placing in closest entry region " + closestDistance + " away.");
                            point = new Polygon2D(closestRegion).PointInside();
                        }
                    }
                    reveal.Location = new T_Location();
                    reveal.Location.Item = point.ToLocationValue();
                    list.Add(reveal);

                    move.ID = reveal.ID;
                    //Console.WriteLine("GroupingGenerator: Revealing new merchant " + reveal.ID + " and moving it");
                }

                move.Location = new T_Location();
                list.Add(move);
            }               

            currentItem.Action = list.ToArray();
        }

        //isPirate here refers to the objects we are trying to find, e.g. isPirate = true means find pirates
        private List<SeamateAdapter.DDD.DDDAdapter.SeamateObject> findObjectsInPlay(LocationValue nearby, PolygonValue pirateEntryRegion, T_Groupings grouping, bool isPirate)
        {
            List<DDDAdapter.SeamateObject> vessels = new List<DDDAdapter.SeamateObject>();
            foreach (DDDAdapter.SeamateObject vessel in ddd.GetAllRevealedSeaVessels()) { 
                if (isPirate && vessel.Owner == "Pirate DM" || !isPirate && vessel.Owner == "Merchant DM") { }
                else continue;
                double distance = new Vec2D(vessel.Location).ScalerDistanceTo(new Vec2D(nearby));
                bool inSameEntryRegionAsPirate = false;
                if (pirateEntryRegion != null) 
                    inSameEntryRegionAsPirate = Polygon2D.IsPointInside(new Polygon2D(pirateEntryRegion),new Vec2D(vessel.Location));

                if (grouping == T_Groupings.One && distance < oneGroupingDistance && !inSameEntryRegionAsPirate)
                {
                    vessels.Add(vessel);
                }
                else if (grouping == T_Groupings.Two && distance > twoGroupingDistance && !inSameEntryRegionAsPirate)
                {
                    vessels.Add(vessel);

                }
            }
            return vessels;
        }
    }


}