using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class DMView
    {
        DateTime m_lastLocationUpdateTime;

        private PlayerAgentInterface m_playerAgent;
        public PlayerAgentInterface PlayerAgent
        {
            get { return m_playerAgent; }
        }

        private String m_decisionMakerID;
        public String DecisionMakerID
        {
            get { return m_decisionMakerID; }
        }

        private Dictionary<String, SimObject> m_myObjects;
        public Dictionary<String, SimObject> MyObjects
        {
            get { return m_myObjects; }
        }
        private Dictionary<String, SimObject> m_allObjects;
        public Dictionary<String, SimObject> AllObjects
        {
            get { return m_allObjects; }
        }

        private Dictionary<String, SimActiveRegion> m_activeRegions;
        public Dictionary<String, SimActiveRegion> ActiveRegions
        {
            get { return m_activeRegions; }
        }


        private int m_simTime;
        public int SimTime
        {
            get { return m_simTime; }
            set { m_simTime = value; }
        }
        private String m_simTimeString;
        public String SimTimeString
        {
            get { return m_simTimeString; }
            set { m_simTimeString = value; }
        }

        private WaypointRouteTable m_routeDB;
        public WaypointRouteTable RouteDB
        {
            set { m_routeDB = value; }
            get { return m_routeDB; }
        }

        public void ComputeRouteDBIntersections()
        {
            RouteDB.ComputeIntersections();
        }
        public DMView(String dm)
        {
            m_lastLocationUpdateTime = DateTime.Now;
            m_decisionMakerID = dm;
            m_myObjects = new Dictionary<string, SimObject>();
            m_allObjects = new Dictionary<string, SimObject>();
            m_activeRegions = new Dictionary<string, SimActiveRegion>();
            m_playerAgent = null;
            m_simTime = 0;
            m_simTimeString = String.Empty;
            m_routeDB = new WaypointRouteTable();
        }

        public void AddPlayerAgent(ref PlayerAgentInterface agent, ref DDDServerConnection ddd)
        {
            m_playerAgent = agent;
            agent.Init(ref ddd);
        }


        public double Distance(String ob1, String ob2)
        {
            if (!AllObjects.ContainsKey(ob1) || !AllObjects.ContainsKey(ob2))
            {
                return -1;
            }
            Vec3D ob1V = new Vec3D(AllObjects[ob1].Location);
            Vec3D ob2V = new Vec3D(AllObjects[ob2].Location);
            return ob1V.ScalerDistanceTo(ob2V);
        }
        public List<String> FindClosestObjectsOfClassToObject(String ofClass, String toID)
        {
            List<String> orderedByDistance = new List<string>();
            Dictionary<Double, String> dict = new Dictionary<double, string>();

            if (AllObjects.Count < 2)
            {
                return orderedByDistance;
            }

            double dis;

            foreach (String id in AllObjects.Keys)
            {
                if (id == toID)
                {
                    continue;
                }

                if (ofClass != String.Empty && AllObjects[id].ClassName != String.Empty)
                {
                    if (ofClass != AllObjects[id].ClassName)
                    {
                        continue;
                    }
                }

                dis = Distance(id, toID);
                dict[dis] = id;

            }
            List<Double> distances = new List<Double>(dict.Keys);
            distances.Sort();
            foreach (Double distance in distances)
            {
                orderedByDistance.Add(dict[distance]);
            }

            return orderedByDistance;
        }
        public String FindClosestObjectOfClassToObject(String ofClass, String toID)
        {
            String currID = String.Empty;
            double currDistance = -1;
            if (AllObjects.Count < 2)
            {
                return String.Empty;
            }

            double dis;

            foreach (String id in AllObjects.Keys)
            {
                if (id == toID)
                {
                    continue;
                }

                if (ofClass != String.Empty)
                {
                    if (ofClass != AllObjects[id].ClassName)
                    {
                        continue;
                    }
                }

                dis = Distance(id, toID);

                if (currDistance < 0)
                {
                    currID = id;
                    currDistance = dis;
                }
                else if (dis < currDistance)
                {
                    currID = id;
                    currDistance = dis;
                }
            }

            return currID;
        }
        public String FindClosestObjectOfClassToClass(String ofClass, String toClass)
        {
            String currID = String.Empty;
            double currDistance = -1;
            if (AllObjects.Count < 2)
            {
                return String.Empty;
            }

            double dis;
            String toID;
            foreach (String id in AllObjects.Keys)
            {
                if (ofClass != String.Empty && AllObjects[id].ClassName != String.Empty)
                {
                    if (ofClass != AllObjects[id].ClassName)
                    {
                        continue;
                    }
                }
                toID = FindClosestObjectOfClassToObject(toClass, id);

                dis = Distance(id, toID);

                if (currDistance < 0)
                {
                    currID = id;
                    currDistance = dis;
                }
                else if (dis < currDistance)
                {
                    currID = id;
                    currDistance = dis;
                }


            }

            return currID;
        }

        //public String FindClosestOfClassInDirectionOf(String thisID, String className, String destID)
        //{

        //}

        public List<String> PlanRoute(String fromID, String toID, List<String> segmentClasses)
        {
            List<String> route = new List<string>();
            route.Add(fromID);
            String curr = fromID;
            String next = String.Empty;
            String segClass;
            String segDest;
            List<String> orderedByDistance;
            curr = fromID;
            route.Add(curr);
            for (int i = 0; i < segmentClasses.Count; i++)
            {
                segClass = segmentClasses[i];
                //next = FindClosestObjectOfClassToObject(segClass, curr);
                //route.Add(next);
                //curr = next;
                // if i'm not on the last segment, thge seg dest should be where this segment intersects with the next
                if (i < segmentClasses.Count - 1) 
                {
                    segDest = FindClosestObjectOfClassToClass(segClass, segmentClasses[i + 1]);
                }
                // if I'm on the last segment, the segment dest should be the one closest to the goal
                else 
                {
                    segDest = FindClosestObjectOfClassToObject(segClass, toID);
                }

                while (next != segDest)
                {
                    orderedByDistance = FindClosestObjectsOfClassToObject(segClass, curr);
                    foreach (String wp in route)
                    {
                        if (orderedByDistance.Contains(wp))
                        {
                            orderedByDistance.Remove(wp);
                        }
                    }
                    List<String> remList = new List<string>();
                    foreach (String wp in orderedByDistance)
                    {
                        if (Distance(wp, segDest) > Distance(curr, segDest))
                        {
                            remList.Add(wp);
                        }
                    }
                    foreach (String wp in remList)
                    {
                        orderedByDistance.Remove(wp);
                    }
                    remList.Clear();

                    if (orderedByDistance.Count < 1)
                    {
                        next = segDest;
                        route.Add(next);
                        curr = next;
                    }
                    else if (orderedByDistance.Count == 1)
                    {
                        next = orderedByDistance[0];
                        route.Add(next);
                        curr = next;
                    }
                    else if (Distance(segDest, orderedByDistance[0]) < Distance(segDest, orderedByDistance[1]))
                    {
                        next = orderedByDistance[0];
                        route.Add(next);
                        curr = next;
                    }
                    else
                    {
                        next = orderedByDistance[1];
                        route.Add(next);
                        curr = next;
                    }
                }

                


            }

            route.Add(toID);

            return route;
        }

        //public WaypointRoute GenerateRoute(LocationValue currentLocation, LocationValue destination, List<WaypointRoute> routes)
        //{
        //    WaypointRoute result = null;

        //    LocationGraph graph = LocationGraph.GenerateRouteGraph("start",currentLocation, "end",destination, routes);

        //    result = graph.GenerateWaypointRoute("start","end");
        //    return result;
        //}

        internal void ViewProInitializeObject(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            SimObject ob;
            if (AllObjects.ContainsKey(id))
            {
                ob = AllObjects[id];
            }
            else
            {
                ob = new SimObject(id);
            }
            ob.Owner = ((StringValue)ev["OwnerID"]).value;
            ob.Location = ((LocationValue)ev["Location"]);
            ob.IconName = ((StringValue)ev["IconName"]).value;
            
            ob.IsWeapon = ((BooleanValue)ev["IsWeapon"]).value;
            if (ob.Owner == m_decisionMakerID)
            {
                m_myObjects[ob.ID] = ob;
            }
            else
            {
                if (m_myObjects.ContainsKey(id))
                {
                    m_myObjects.Remove(id);
                }
            }
            

            m_allObjects[ob.ID] = ob;

            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProInitializeObject(ev);
            }
        }
        internal void ViewProAttributeUpdate(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            if (m_allObjects.ContainsKey(id))
            {
                SimObject ob = m_allObjects[id];

                AttributeCollectionValue atts = (AttributeCollectionValue)ev["Attributes"];
                foreach (String attName in atts.attributes.Keys)
                {
                    switch (attName)
                    {
                        case "Location":
                            ob.Location = (LocationValue)atts[attName];
                            m_lastLocationUpdateTime = DateTime.Now;
                            UpdateInActiveRegions(ref ob);
                            break;
                        case "DestinationLocation":
                            ob.DestinationLocation = (LocationValue)atts[attName];
                            break;
                        case "Velocity":
                            ob.Velocity = (VelocityValue)atts[attName];
                            break;

                        case "ClassName":
                            ob.ClassName = ((StringValue)atts[attName]).value;
                            break;
                        case "Heading":
                            ob.CurrentHeading = ((DoubleValue)atts[attName]).value;
                            break;
                        case "MaximumSpeed":
                            ob.MaximumSpeed = ((DoubleValue)atts[attName]).value;
                            break;
                        case "Throttle":
                            ob.Throttle = ((DoubleValue)atts[attName]).value;
                            break;
                        case "OwnerID":
                            String newOwner = ((StringValue)atts[attName]).value;
                            if (newOwner != ob.Owner)
                            {
                                ob.Owner = newOwner;                                
                            }
                            break;
                        case "IconName":
                            ob.IconName = ((StringValue)atts[attName]).value;
                            break;
                        case "VulnerabilityList":
                            ob.VulnerabilityList = ((StringListValue)atts[attName]).strings;
                            break;
                        case "CapabilitiesList":
                            ob.CapabilityList = ((StringListValue)atts[attName]).strings;
                            break;
                        case "DockedObjects":
                            ob.DockedObjects = ((StringListValue)atts[attName]).strings;
                            if (ob.DockedObjects.Contains("Dock To Parent"))
                            {
                                ob.DockedObjects.Remove("Dock To Parent");
                            }
                            break;
                        case "DockedWeapons":
                            ob.DockedWeapons = ((StringListValue)atts[attName]).strings;
                            break;
                        case "State":
                            ob.State = ((StringValue)atts[attName]).value;
                            break;
                        case "CustomAttributes":
                            ob.CustomAttributes = (CustomAttributesValue)atts[attName];
                            break;
                        case "Capability":
                            ob.Capabilities = (CapabilityValue)atts[attName];
                            break;
                        case "Vulnerability":
                            ob.Vulnerabilities = (VulnerabilityValue)atts[attName];
                            break;
                        case "CapabilityRangeRings":
                            ob.CapabilityRangeRings = (CustomAttributesValue)atts[attName];
                            break;
                        case "VulnerabilityRangeRings":
                            ob.VulnerabilityRangeRings = (CustomAttributesValue)atts[attName];
                            break;
                        case "IsWeapon":
                            ob.IsWeapon = ((BooleanValue)atts[attName]).value;
                            break;
                        case "TeamName":
                            ob.TeamName = ((StringValue)atts[attName]).value;
                            break;
                        case "FuelConsumptionRate":
                            ob.FuelConsumptionRate = ((DoubleValue)atts[attName]).value;
                            break;
                        case "FuelAmount":
                            ob.FuelAmount = ((DoubleValue)atts[attName]).value;
                            break;
                        case "FuelCapacity":
                            ob.FuelCapacity = ((DoubleValue)atts[attName]).value;
                            break;
                        case "Sensors":
                            ob.Sensors = (SensorArrayValue)atts[attName];
                            break;
                        case "ChildObjects":
                            ob.ChildObjects = ((StringListValue)atts[attName]).strings;
                            break;
                        case "DockedToParent":
                            ob.DockedToParent = ((BooleanValue)atts[attName]).value;
                            break;
                        case "ParentObjectID":
                            ob.ParentObjectID = ((StringValue)atts[attName]).value;
                            break;
                        default:
                            //Console.WriteLine(String.Format("ViewProAttributeUpdate:{0} unsupported",attName));
                            break;
                    }
                }
            }
            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProAttributeUpdate(ev);
            }

        }
        internal void ViewProMotionUpdate(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            if (AllObjects.ContainsKey(id))
            {
                SimObject ob = AllObjects[id];
                ob.ShouldProject = true;
                ob.Location = ((LocationValue)ev["Location"]);
                ob.DestinationLocation = ((LocationValue)ev["DestinationLocation"]);
                ob.Throttle = ((DoubleValue)ev["Throttle"]).value;
                ob.Velocity = BehaviorHelper.ComputeVelocityVector(ob.Location, ob.DestinationLocation, ob.MaximumSpeed, ob.Throttle);
            }
            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProMotionUpdate(ev);
            }
        }
        internal void ViewProAttackUpdate(SimulationEvent ev)
        {
            foreach (SimObject ob in AllObjects.Values)
            {
                ob.ControlAgent.ViewProAttackUpdate(ev);
            }
            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProAttackUpdate(ev);
            }
        }
        internal void ViewProStopObjectUpdate(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            if (AllObjects.ContainsKey(id))
            {
                AllObjects[id].ShouldProject = false;
            }
            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProStopObjectUpdate(ev);
            }
        }
        internal void ViewProActiveRegionUpdate(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            SimActiveRegion ar = null;

            int displayColor = ((IntegerValue)ev["DisplayColor"]).value;

            if (m_activeRegions.ContainsKey(id))
            {
                ar = m_activeRegions[id];
                ar.Shape = ((PolygonValue)ev["Shape"]);
                ar.Color = Color.FromArgb(displayColor);
            }
            else
            {
                ar = new SimActiveRegion();
                ar.ID = id;
                ar.Shape = ((PolygonValue)ev["Shape"]);
                ar.Color = Color.FromArgb(displayColor);
            }
            m_activeRegions[ar.ID] = ar;

            if (PlayerAgent != null)
            {
                PlayerAgent.ViewProActiveRegionUpdate(ev);
            }
        }
        internal void AttackSucceeded(SimulationEvent ev)
        {
            foreach (SimObject ob in AllObjects.Values)
            {
                ob.ControlAgent.AttackSucceeded(ev);
            }
            
            if (PlayerAgent != null)
            {
                PlayerAgent.AttackSucceeded(ev);
            }
        }
        internal void StateChange(SimulationEvent ev)
        {
            String id = ((StringValue)ev["ObjectID"]).value;
            String state = ((StringValue)ev["NewState"]).value;

            if (AllObjects.ContainsKey(id))
            {
                AllObjects[id].State = state; // update based on this because view pro doesn't come out after its been killed
                if (state == "Dead")
                {
                    if (AllObjects[id].ControlAgent != null)
                    {
                        AllObjects[id].ControlAgent.ClearBehaviorQueue();
                    }
                }
            }

            if (PlayerAgent != null)
            {
                PlayerAgent.StateChange(ev);
            }
        }
        internal void TimeTick(SimulationEvent ev)
        {
            SimTimeString = ((StringValue)ev["SimulationTime"]).value;
            SimTime = ((IntegerValue)ev["Time"]).value;
        }

        void UpdateInActiveRegions(ref SimObject ob)
        {
            ob.InActiveRegions.Clear();
            foreach (SimActiveRegion sar in m_activeRegions.Values)
            {
                Polygon2D azPoly = new Polygon2D();
                foreach (PolygonValue.PolygonPoint p in sar.Shape.points)
                {
                    azPoly.AddVertex(new Vec2D(p.X, p.Y));
                }
                if (Polygon2D.IsPointInside(azPoly, new Vec2D(ob.Location)))
                {
                    ob.InActiveRegions.Add(sar.ID);
                }
            }
        }
        public void ProjectObjectLocations()
        {
            DateTime currenTime = DateTime.Now;
            TimeSpan ts= currenTime - m_lastLocationUpdateTime;
            m_lastLocationUpdateTime = currenTime;

            foreach (SimObject ob in AllObjects.Values)
            {
                if (ob.ShouldProject)
                {
                    
                    ob.Location.X = ob.Location.X + (ob.Velocity.VX * (Double)ts.Milliseconds / 1000);
                    ob.Location.Y = ob.Location.Y + (ob.Velocity.VY * (Double)ts.Milliseconds / 1000);
                    ob.Location.Z = ob.Location.Z + (ob.Velocity.VZ * (Double)ts.Milliseconds / 1000);
                    //if (System.Double.IsNaN(ob.Location.X))
                    //{
                    //    ob.ID = ob.ID;
                    //}
                    if (BehaviorHelper.LocationIsEqual(ob.Location, ob.DestinationLocation))
                    {
                        ob.ShouldProject = false;
                    }
                    
                }
            }
        }

    }
}
