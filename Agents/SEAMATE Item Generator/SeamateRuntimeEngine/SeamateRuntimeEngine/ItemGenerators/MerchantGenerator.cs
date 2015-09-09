using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace SeamateAdapter.ItemGenerators
{
    public class MerchantGenerator : Generator
    {
        T_Groupings grouping;


        public MerchantGenerator(DDDAdapter ddd)
            : base(ddd)
        {
            
        }


        public override void Generate(T_Item currentItem, String dmID)
        {
            actions = currentItem.Action.ToList();
            dict = GetActionsAsDictionary(currentItem.Action);
            grouping = currentItem.Parameters.Groupings;


            List<string> unrevealedMerchantIDs = ddd.GetAllUnrevealedObjectIds(false, true, null); //Gets only merchants

           
            int minStimuli = 1;
            int maxStimuli;
            //If resources need to be available, then two is maximum of stimulus events.
            if (T_ResourceAvailability.Available == currentItem.Parameters.PlayerResources)
                maxStimuli = Properties.Settings.Default.resourceThreshold-1;
            //If resources are unavailable, there must be at least 3 total stimulus events, and max can be higher.
            else
            {
                minStimuli = Properties.Settings.Default.resourceThreshold;
                maxStimuli = Properties.Settings.Default.resourceThreshold*2-1;
            }

            int numPirates = dict.Keys.Count;
            int numMerchants = Math.Max(0, random.Next(minStimuli, maxStimuli) - numPirates);
            if (numMerchants == 0)
                return;


            //If our probability criteria says we should add vessels, let's see if we can reuse any existing vessels 
            List<DDDAdapter.SeamateObject> reuseableVessels = new List<DDDAdapter.SeamateObject>();
            List<DDDAdapter.SeamateObject> allExistingMerchants = revealedSeaVessels.FindAll(IsMerchant);
            if (shouldAddNewVessel(revealedSeaVessels))
                reuseableVessels = allExistingMerchants.FindAll(MatchesGroupingConstraints);

            for (int i = 0; i < numMerchants; i++)  //Do once for each merchant we need to create.
            {
                //Try and recycle objects to create stimulus.
                if (reuseableVessels.Count > 0)
                {
                    DDDAdapter.SeamateObject objectForReuse = null;
                    //Make sure we reuse any stopped objects first
                    List<DDDAdapter.SeamateObject> stoppedObjects = reuseableVessels.FindAll(IsStopped);
                    if (stoppedObjects.Count > 0) objectForReuse = stoppedObjects[random.Next(stoppedObjects.Count)];
                    else objectForReuse = reuseableVessels[random.Next(reuseableVessels.Count)];

                    CreateEmptyMove(objectForReuse);
                    reuseableVessels.Remove(objectForReuse);

                }
                else //Reveal and move new merchant starting at an appropriate point for grouping constraints.
                {
                    Vec2D location = null;
                    if (currentItem.Parameters.Groupings == T_Groupings.One) //Get point in closest entry region
                    {
                        location = GetPointInClosestEntryRegion(dict, entryRegions);
                    }
                    else if (currentItem.Parameters.Groupings == T_Groupings.Two) //Get point in any entry region
                    {
                        location = new Polygon2D(entryRegions[random.Next(entryRegions.Count)]).PointInside();
                    }

                    string id = unrevealedMerchantIDs[random.Next(unrevealedMerchantIDs.Count)];
                    CreateRevealAndEmptyMove(id, "Merchant DM", location);
                    unrevealedMerchantIDs.Remove(id); 
                }
         
            }               
            currentItem.Action = actions.ToArray();
        }

        /// <summary>
        /// Determines whether a merchant is within grouping constraints of previously chosen pirates.
        /// </summary>
        /// <param name="vessel">Merchant</param>
        /// <returns>Boolean</returns>
        private bool MatchesGroupingConstraints(DDDAdapter.SeamateObject vessel)
        {
            if (vessel.Intent != "")
                return false;

            if (grouping == T_Groupings.One)
            {
                foreach (T_Move key in dict.Keys)
                {
                    Vec2D otherVesselLocation = new Vec2D(GetLocation(key, dict[key]));
                    double distance = new Vec2D(vessel.Location).ScalerDistanceTo(otherVesselLocation);
                    if (distance > oneGroupingDistance) return false;
                }
                return true;
            }
            else 
            {
                return true;
            }      
        }

        private bool IsMerchant(DDDAdapter.SeamateObject vessel)
        {
            return vessel.Owner == "Merchant DM";
        }

        /// <summary>
        /// Makes a point which is in the closest entry region to the group of pirates, 
        /// TODO: but not in the same entry region as any of them.
        /// </summary>
        /// <param name="pirates">The move events representing pirates</param>
        /// <param name="entryRegions">All entry regions</param>
        /// <returns>A random point in the closest entry region to the group of pirates</returns>
        private Vec2D GetPointInClosestEntryRegion(Dictionary<T_Move,T_Reveal> pirates, List<PolygonValue> entryRegions) {
            Vec2D point = new Vec2D(0,0);
            double closestDistance = 100000000;
            PolygonValue closestRegion = null;
            Shuffle(entryRegions);
            T_Move randomPirate = pirates.Keys.ToList().ElementAt(0);
            LocationValue location = GetLocation(randomPirate, pirates[randomPirate]);
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
                point = new Polygon2D(closestRegion).PointInside();
            }
            return point;
        }

    }


}