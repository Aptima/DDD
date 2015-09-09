using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using System.Windows.Media.Media3D;

namespace SeamateRuntimeEngine.ItemGenerators
{
    class ThreatGenerator: Generator
    {
        public ThreatGenerator(DDDAdapter ddd)
            : base(ddd)
        {
        }

        //Threat generator -- generates or chooses ONE pirate or friendly vessel in DM domain
        //So either creates a RevealEvent or chooses existing vessel for MoveEvent
        public List<DDDAdapter.SeamateObject> GenerateWithTriedItems(T_Item currentItem, String dmID, List<DDDAdapter.SeamateObject> triedVessels)
        {
            //make a copy of the input which we'll modify and return at the end
            List<DDDAdapter.SeamateObject> returnTriedVessels = new List<DDDAdapter.SeamateObject>(triedVessels);

            String owner;
            bool isPirate;

            if (currentItem.Parameters.Threat == T_Threat.None)
            {
                isPirate = false;
                owner = "Merchant DM";
            }
            else
            {
                isPirate = true;
                owner = "Pirate DM";
            }


            PolygonValue domainPV = this.ddd.GetDecisionMakersAreaOfResponsibility(dmID);
            Polygon2D domain = new Polygon2D(domainPV);
            
            List<Object> actions = new List<object>();

            if (shouldAddNewVessel(revealedSeaVessels)) 
            {
                Console.WriteLine("ThreatGenerator: I'm trying to reuse a vessel because there are " + revealedSeaVessels.Count + " in play already and/or probability told me to.");
                List<DDDAdapter.SeamateObject> vessels = new List<DDDAdapter.SeamateObject>();
                //Find all vessels of the appropriate type (pirate or merchant) within the BAMS/Firescout's range.
                foreach (DDDAdapter.SeamateObject vessel in revealedSeaVessels) 
                {
                    if (vessel.Owner == owner && Polygon2D.IsPointInside(domain, new Vec2D(vessel.Location)))
                        vessels.Add(vessel);
                }
                Console.WriteLine("ThreatGenerator: Found " + vessels.Count + " to reuse");
                
                if (vessels.Count > 0)
                {
                    //manage the list of previously tried objects
                    if (triedVessels.Count == vessels.Count) returnTriedVessels = new List<DDDAdapter.SeamateObject>();
                    foreach (DDDAdapter.SeamateObject alreadyTried in returnTriedVessels) vessels.Remove(alreadyTried);

                    Shuffle(vessels);
                    DDDAdapter.SeamateObject vessel = null;
                    //see if any eligible vessels are stopped, if so, use one of them
                    foreach (DDDAdapter.SeamateObject possiblyStoppedVessel in vessels)
                    {
                        if (possiblyStoppedVessel.Throttle == 0) //yes, it is stopped
                        {
                            Console.WriteLine("ThreatGenerator: Picking a stopped vessel " + possiblyStoppedVessel.ID + " to reuse.");
                            vessel = possiblyStoppedVessel;
                            break;
                        }
                    }
                    //we didn't find any stopped vessels, so pick a random eligible one.
                    if (vessel == null)
                        vessel = vessels[random.Next(vessels.Count)];
                    returnTriedVessels.Add(vessel);
                    T_Move move = new T_Move();
                    move.ID = vessel.ID;
                    move.Location = new T_Location();

                    actions.Add(move);

                }
            }

            if (actions.Count() == 0) //we couldn't find a ship or need more vessels
            { //reveal a random new one

                Console.WriteLine("ThreatGenerator: I'm revealing a new vessel, either couldn't find a ship or need more vessels.");
                List<string> unrevealedObjectIDs = ddd.GetAllUnrevealedObjectIds(isPirate,true,null);
               
                T_Reveal reveal = new T_Reveal();
                reveal.ID = unrevealedObjectIDs[random.Next(unrevealedObjectIDs.Count())];
                reveal.Owner = owner;
                reveal.State = "FullyFunctional";
                reveal.Location = new T_Location();
                reveal.StartupParameters = new T_StartupParameters();
                reveal.StartupParameters.Items = new string[0];
            
                T_Move move = new T_Move();
                move.ID = reveal.ID;
                move.Location = new T_Location();
                Vec2D point;
                List<PolygonValue> entryRegions = ddd.GetAllEntryRegions();
                //TODO: Write a safe PointInsideMultipleDomains() function.  This could theoretically loop forever with a terrible scenario
                while (true)
                {
                    point = new Polygon2D(entryRegions[random.Next(entryRegions.Count)]).PointInside();
                    if (Polygon2D.IsPointInside(domain,point)) {
                        break;
                    }
                }
                reveal.Location.Item = point.ToLocationValue();
 
                actions.Add(reveal);
                actions.Add(move);
                Console.WriteLine("ThreatGenerator: Just revealed a pirate " + reveal.ID);
            }


            currentItem.Action = actions.ToArray();
            return returnTriedVessels;
            
        }
    }
}
