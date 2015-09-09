using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using System.Windows.Media.Media3D;
using System.Windows;

namespace SeamateAdapter.ItemGenerators
{
    /// <summary>
    /// Chooses several pirates for reuse or creation, taking into account grouping rules.  
    /// If pirates are newly created, it assigns them an appropriate location, taking into acount grouping rules.
    /// </summary>
    class PirateGenerator : Generator
    {
        String owner = "";
        Polygon2D domain = null;
        

        public PirateGenerator(DDDAdapter ddd)
            : base(ddd)
        {
        }

        //Pirate generator -- generates or chooses several pirates or friendly vessels in DM domain 
        //So either creates a RevealEvent or chooses existing vessel for MoveEvent
        
        public override void Generate(T_Item currentItem, String dmID)
        {
            if (currentItem.Parameters.Threat == T_Threat.None) return;


            actions = currentItem.Action.ToList();

            // Do some setup
            bool isPirate = !(currentItem.Parameters.Threat == T_Threat.None);
            if (isPirate) owner = "Pirate DM";
            else owner = "Merchant DM";

            PolygonValue domainPV = this.ddd.GetDecisionMakersAreaOfResponsibility(dmID);
            domain = new Polygon2D(domainPV);
            List<string> unrevealedObjectIDs = ddd.GetAllUnrevealedObjectIds(isPirate, true, null);


            //How many pirates should we generate?  These numbers may need tweaking
            int minPirates = Properties.Settings.Default.minPirates;
            int maxPirates = Properties.Settings.Default.maxPirates;
            int numPirates = random.Next(minPirates, maxPirates);


            //Are there any pirates we can reuse, already in play?
            List<DDDAdapter.SeamateObject> reusableVessels = revealedSeaVessels.FindAll(IsReusable);

            if (reusableVessels.Count > 0 && Properties.Settings.Default.reusePirates)
            {
                for (int i = 0; i < numPirates; i++)
                {
                    if (reusableVessels.Count > 0)
                    {
                        DDDAdapter.SeamateObject vessel = reusableVessels[0];
                        reusableVessels.Remove(vessel);
                        CreateEmptyMove(vessel);
                    }
                    else
                    {
                        Vec2D location = MakeLocationInEntryRegions(entryRegions);
                        string id = unrevealedObjectIDs[random.Next(unrevealedObjectIDs.Count())];
                        unrevealedObjectIDs.Remove(id);
                        CreateRevealAndEmptyMove(id, owner, location);
                    }
                }


                ////See if any existing pirates can be paired under "grouping" distance constraints
                //List<List<DDDAdapter.SeamateObject>> groupedPirates = GroupVessels(currentItem.Parameters.Groupings, numPirates, reusableVessels);
                //List<List<DDDAdapter.SeamateObject>> groupsOfNumPirateSize = groupedPirates.FindAll(delegate(List<DDDAdapter.SeamateObject> g) { return g.Count >= numPirates; });

                ////Any groups of numPirates?
                //if (groupsOfNumPirateSize.Count > 0)
                //{
                //    //if yes, make dummy move events for them and add to actions list.  done.
                //    foreach (DDDAdapter.SeamateObject pirate in groupsOfNumPirateSize.ElementAt(0))
                //        CreateEmptyMove(pirate);
                //}
                //else
                //{
                //    //Can any of these smaller groupings include an entry region too, so we can add new pirates? 
                //    groupedPirates.Sort(delegate(List<DDDAdapter.SeamateObject> g1, List<DDDAdapter.SeamateObject> g2) { return g1.Count.CompareTo(g2.Count); }); //sort by size, largest first
                //    foreach (List<DDDAdapter.SeamateObject> group in groupedPirates)
                //    {
                //        List<PolygonValue> matchingEntryRegions = GetMatchingEntryRegions(currentItem.Parameters.Groupings, group, entryRegions);
                //        if (matchingEntryRegions.Count > 0)
                //        {
                //            //if yes, then make dummy move events for the pirates in grouping, then
                //            foreach (DDDAdapter.SeamateObject pirate in groupsOfNumPirateSize.ElementAt(0))
                //                CreateEmptyMove(pirate);
                //            //Create as many remaining pirates as we need distributed randomly among suitable entry regions.
                //            List<Vec2D> locations = MakeLocationsInEntryRegions(numPirates - group.Count, matchingEntryRegions);
                //            foreach (Vec2D location in locations)
                //            {
                //                string id = unrevealedObjectIDs[random.Next(unrevealedObjectIDs.Count())];
                //                unrevealedObjectIDs.Remove(id);
                //                CreateRevealAndEmptyMove(id, owner, location);
                //            }
                //            break;
                //        }
                //    }

                //    //if no, we couldn't include an entry region (where new pirates would be created) with any existing pirates
                //    if (actions.Count == 0)
                //        //TODO: maybe should make smaller num of pirates if above min rather than making all new?
                //        MakeAllNewPirates(currentItem.Parameters.Groupings,numPirates,entryRegions,unrevealedObjectIDs);
                //}
            }
            //no reusable vessels, we have to create all new pirates 
            else
            {
                MakeAllNewPirates(currentItem.Parameters.Groupings,numPirates,entryRegions,unrevealedObjectIDs);
            }

            currentItem.Action = actions.ToArray();

        }

        /// <summary>
        /// Returns a list of entry regions in which new pirates can be added while satisfying grouping constraints, given a group of existing pirates.
        /// </summary>
        /// <param name="grouping">Grouping constraints</param>
        /// <param name="group">Group of existing pirates</param>
        /// <param name="entryRegions">All avilable entry regions</param>
        /// <returns></returns>
        private List<PolygonValue> GetMatchingEntryRegions(T_Groupings grouping, List<DDDAdapter.SeamateObject> group, List<PolygonValue>entryRegions)
        {
            List<PolygonValue> matchingEntryRegions = new List<PolygonValue>();
            //One grouping: matching entry regions must be within certain distance of every individual vessel in group
            if (grouping == T_Groupings.One)
            {
                foreach (PolygonValue region in entryRegions) 
                {
                    Vec2D regionPoint = new Vec2D(region.points.ElementAt(0).X, region.points.ElementAt(0).Y);
                    bool matching = true;
                    foreach (DDDAdapter.SeamateObject vessel in group)
                    {
                        Vec2D vesselPoint = new Vec2D(vessel.Location);
                        if (vesselPoint.ScalerDistanceTo(regionPoint) > oneGroupingDistance)
                        {
                            matching = false;
                            break;
                        }
                    }
                    if (matching)
                        matchingEntryRegions.Add(region);
                }
                return matchingEntryRegions;
            }
            //Two grouping: any entry region will do, the existing group satisfies constraint
            else 
                return entryRegions;
        }

        /// <summary>
        /// Makes a full set of new pirates not involving any existing pirates.
        /// </summary>
        /// <param name="grouping"></param>
        /// <param name="numPirates"></param>
        /// <param name="entryRegions"></param>
        /// <param name="unrevealedObjectIDs"></param>
        private void MakeAllNewPirates(T_Groupings grouping, int numPirates, List<PolygonValue> entryRegions, List<string> unrevealedObjectIDs)
        {
            if (unrevealedObjectIDs.Count == 0) MessageBox.Show("Runtime Engine is out of pirates.");
            if (numPirates > unrevealedObjectIDs.Count) numPirates = unrevealedObjectIDs.Count;
            List<Vec2D> locations = MakeLocationsForGroup(grouping, numPirates, entryRegions);
            foreach (Vec2D location in locations)
            {
                string id = unrevealedObjectIDs[random.Next(unrevealedObjectIDs.Count())];
                unrevealedObjectIDs.Remove(id);
                CreateRevealAndEmptyMove(id, owner, location);
            }
        }

        /// <summary>
        /// Organizes vessels into lists up to the maximum desired grouping size by comparing their locations using grouping constraints.
        /// </summary>
        /// <param name="grouping">Grouping constraint from item</param>
        /// <param name="numPirates">Desired number of vessels in group</param>
        /// <param name="reusableVessels">A list of vessels to sort</param>
        /// <returns></returns>
        private List<List<DDDAdapter.SeamateObject>> GroupVessels(T_Groupings grouping, int numPirates, List<DDDAdapter.SeamateObject> reusableVessels)
        {
            List<List<DDDAdapter.SeamateObject>> groupedPirates = new List<List<DDDAdapter.SeamateObject>>();
            foreach (DDDAdapter.SeamateObject vessel in reusableVessels)
            {
                List<DDDAdapter.SeamateObject> group = new List<DDDAdapter.SeamateObject>();
                group.Add(vessel);
            }
            for (int i = 0; i < (Math.Min(numPirates, reusableVessels.Count)); i++)
            {
                foreach (List<DDDAdapter.SeamateObject> group in groupedPirates)
                {
                    foreach (DDDAdapter.SeamateObject vessel in reusableVessels)
                    {
                        if (MatchesGroupingCriteria(grouping, vessel, group))
                            group.Add(vessel);
                    }
                }
            }
            return groupedPirates;
        }



        private List<Vec2D> MakeLocationsForGroup(T_Groupings grouping, int numPirates, List<PolygonValue> entryRegions)
        {
            //For two groupings, just return all the entry regions.  We'll probably get a good distribution.
            if (grouping == T_Groupings.Two)
                return MakeLocationsInEntryRegions(numPirates, entryRegions);

            //For one groupings, pick a random entry region to start the group.
            PolygonValue entryRegion = entryRegions.ElementAt(random.Next(entryRegions.Count));
            List<PolygonValue> acceptableEntryRegions = new List<PolygonValue>();
            foreach (PolygonValue region in entryRegions)
            {
                Vec2D pointFromNewRegion = new Vec2D(region.points.ElementAt(0).X, region.points.ElementAt(0).Y);
                Vec2D pointFromFirstRegion = new Vec2D(entryRegion.points.ElementAt(0).X, entryRegion.points.ElementAt(0).Y);
                if (pointFromNewRegion.ScalerDistanceTo(pointFromFirstRegion) < oneGroupingDistance)
                {
                    acceptableEntryRegions.Add(region);
                }
            }
            return MakeLocationsInEntryRegions(numPirates, acceptableEntryRegions);
        }


        private Vec2D MakeLocationInEntryRegions(List<PolygonValue> acceptableEntryRegions)
        {
            PolygonValue entryRegion = acceptableEntryRegions.ElementAt(random.Next(acceptableEntryRegions.Count));
            Vec2D location = new Polygon2D(entryRegion).PointInside();
            return location;
        }
        private List<Vec2D> MakeLocationsInEntryRegions(int numPirates, List<PolygonValue> acceptableEntryRegions)
        {
            List<Vec2D> locations = new List<Vec2D>();
            for (int i = 0; i < numPirates; i++)
            {
                PolygonValue entryRegion = acceptableEntryRegions.ElementAt(random.Next(acceptableEntryRegions.Count));
                Vec2D location = new Polygon2D(entryRegion).PointInside();
                locations.Add(location);
            }
            return locations;
        }


        /// <summary>
        /// Returns a boolean value expressing whether or not a vessel is suitable to be chosen to express a stimuli.
        /// </summary>
        /// <param name="vessel"></param>
        /// <returns></returns>
        private bool IsReusable(DDDAdapter.SeamateObject vessel)
        {
            //merchants may be attacked by multiple pirates, so it's ok to choose them even if they
            //already have an intent.
            bool isMerchantOrNotOnAttackCourse = ((vessel.Owner == "Merchant DM") || (vessel.Intent == "")); 
            return (vessel.Owner == owner && Polygon2D.IsPointInside(domain, new Vec2D(vessel.Location)) && isMerchantOrNotOnAttackCourse);
        }



        /// <summary>
        /// Returns a boolean value expressing whether or not a new vessel can be added to an existing group while fulfilling 
        /// grouping constraints.
        /// One grouping: Every item in the group must be within the one-grouping parameter of each other.
        /// Two grouping: At least one pair in the group must be separated by the two-grouping parameter.
        /// </summary>
        /// <param name="oneOrTwo">Must be "One" or "Two" for a one- or two-grouping</param>
        /// <param name="newVessel">Vessel to be added to the group</param>
        /// <param name="otherVessels">Existing group of vessels</param>
        /// <returns></returns>
        private bool MatchesGroupingCriteria(T_Groupings grouping, DDDAdapter.SeamateObject newVessel, List<DDDAdapter.SeamateObject> otherVessels) {
            if (otherVessels.Contains(newVessel))
                return false;
            if (grouping == T_Groupings.One) {
                foreach (DDDAdapter.SeamateObject vessel in otherVessels) {
                    double distance = new Vec2D(vessel.Location).ScalerDistanceTo(new Vec2D(newVessel.Location));
                    if (distance > oneGroupingDistance)
                        return false;
                }
                return true;
            } else {
                foreach (DDDAdapter.SeamateObject vessel in otherVessels) {
                    double distance = new Vec2D(vessel.Location).ScalerDistanceTo(new Vec2D(newVessel.Location));
                    if (distance > twoGroupingDistance)
                        return true;
                }
                return false;
            } 
        }
    }
}
