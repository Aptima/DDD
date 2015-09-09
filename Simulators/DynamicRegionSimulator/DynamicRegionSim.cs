using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.Simulators.DynamicRegion
{
    public class DynamicRegionSim : ISimulator
    {
        //look at MotionSim for initialization
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;
        private Dictionary<string, SimulationObjectProxy> objectProxies;
        private Dictionary<String, Polygon3D> collisionShapes;

        public DynamicRegionSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;

        }

        public string GetSimulatorName()
        {
            return "DynamicRegion";
        }

        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;

            distributor.RegisterClient(ref distClient);
            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);

            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        public void ProcessEvent(SimulationEvent e)
        {
            objectProxies = bbClient.GetObjectProxies();
            switch (e.eventType)
            {
                case "TimeTick":
                    TimeTick(e);
                    break;
                default:
                    break;
            }
        }

        private void TimeTick(SimulationEvent e)
        {

            foreach (StateDB.ActiveRegion region in StateDB.dynamicRegions.Values)
            {
                if (region.linkedObject == "")
                    continue; //don't draw unlinked regions
                SimulationObjectProxy obProx = objectProxies[region.linkedObject];
                //Calculate new absolute poly for region and send ViewPro event
                LocationValue lvLoc = (LocationValue)obProx["Location"].GetDataValue();
                Vec2D loc = new Vec2D(lvLoc);
                if (!lvLoc.exists)
                {
                    return; //possible that the tractor object doesn't exist, so don't show it.
                }
                Polygon3D absolutePoly = GetAbsolutePolygon(loc, region.poly.Footprint);
                absolutePoly.TopZ = region.poly.TopZ;
                absolutePoly.BottomZ = region.poly.BottomZ;
                region.referencePoint = loc;
                region.currentAbsolutePolygon = absolutePoly;

                this.SendViewProActiveRegionUpdate(region.id, region.isVisible, region.displayColor, absolutePoly);
                

            }

        }

        /// <summary>
        /// Calculates the absolute polygon of a region given a new location,
        /// using the region's reference point and relative polygon
        /// </summary>
        /// <param name="location"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private Polygon3D GetAbsolutePolygon(Vec2D refPoint, Polygon2D relativePolygon)
        {
            //Vec2D difference = refPoint.VectorDistanceTo(location);
            Polygon3D absolute = new Polygon3D(0,0);

            foreach (Vec2D vertex in relativePolygon.getVertices())
            {
                absolute.AddVertex(vertex.Add(refPoint));
            }
            return absolute;
        }



        private void SendViewProActiveRegionUpdate(string objectID, bool isVisible, int displayColor, Polygon3D poly)
        {
            SimulationEvent vpmu = null;
            vpmu = SimulationEventFactory.BuildEvent(ref simModel, "ViewProActiveRegionUpdate");

            vpmu["ObjectID"] = DataValueFactory.BuildString(objectID);
            vpmu["IsVisible"] = DataValueFactory.BuildBoolean(isVisible);
            vpmu["DisplayColor"] = DataValueFactory.BuildInteger(displayColor);
            vpmu["Shape"] = poly.Footprint.GetPolygonValue();
            //Should pass other fields too?  Or does ViewPro care? -Lisa

            distClient.PutEvent(vpmu);
        }
    }


}
