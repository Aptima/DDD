using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.Motion
{



    public class MotionSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;

        //private Dictionary<string,LandRegion> landRegions;
        //private Dictionary<string, ActiveRegion> activeRegions;

        private Dictionary<String, Polygon3D> collisionShapes;

        public MotionSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
            collisionShapes = new Dictionary<string, Polygon3D>();

            //landRegions = new Dictionary<string, LandRegion>();
            //activeRegions = new Dictionary<string, ActiveRegion>();
        }
        public void Initialize(ref SimulationModelInfo simModel, ref Blackboard blackboard, ref SimulationEventDistributor distributor)
        {
            this.blackboard = blackboard;
            this.bbClient = new BlackboardClient();
            this.distributor = distributor;
            this.distClient = new SimulationEventDistributorClient();
            this.simModel = simModel;

            distributor.RegisterClient(ref distClient);
            //distClient.Subscribe("NewObject");
            //distClient.Subscribe("RevealObject");
            //distClient.Subscribe("MoveObject");
            //distClient.Subscribe("SubplatformLaunch");
            //distClient.Subscribe("WeaponLaunch");
            //distClient.Subscribe("StateChange");
            //distClient.Subscribe("ObjectCollision");
            //distClient.Subscribe("TimeTick");
            //distClient.Subscribe("SubplatformDock");
            //distClient.Subscribe("ResetSimulation");


            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "StateTable", true, false);
            bbClient.Subscribe("PhysicalObject", "State", true, false);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, true);
            bbClient.Subscribe("PhysicalObject", "Size", true, false);
            bbClient.Subscribe("PhysicalObject", "Velocity", true, true);
            bbClient.Subscribe("PhysicalObject", "MaximumSpeed", true, false);
            bbClient.Subscribe("PhysicalObject", "Throttle", true, true);
            bbClient.Subscribe("PhysicalObject", "DestinationLocation", true, true);
            bbClient.Subscribe("PhysicalObject", "DockedToParent", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchStarted", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchEndTime", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchDone", true, true);
            bbClient.Subscribe("PhysicalObject", "LinkedRegion", true, true);
            bbClient.Subscribe("PhysicalObject", "PursueStarted", true, true);
            bbClient.Subscribe("PhysicalObject", "PursueTargetID", true, true);
            bbClient.Subscribe("PhysicalObject", "ParentObjectID", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchIsWeapon", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchWeaponTargetID", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchDestinationLocation", true, false);
            bbClient.Subscribe("PhysicalObject", "InActiveRegions", true, true);
            bbClient.Subscribe("PhysicalObject", "ActiveRegionSpeedMultiplier", true, true);

           // bbClient.Subscribe("Region", "LinkedObject", true, true);
            objectProxies = new Dictionary<string, SimulationObjectProxy>();
        }

        public void ProcessEvent(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();
            switch (e.eventType)
            {
                case "NewObject":
                    NewObject(e);
                    break;
                case "RevealObject":
                    RevealObject(e);
                    break;
                case "MoveObject":
                    MoveObject(e);
                    break;
                case "SubplatformDock":
                    SubplatformDock(e);
                    break;
                case "ObjectCollision":
                    ObjectCollision(e);
                    break;
                case "StateChange":
                    StateChange(e);
                    break;
                case "TimeTick":
                    TimeTick(e);
                    break;
                case "ExternalApp_SimStop":
                    ResetSimulation();
                    break;
                case "ForceUpdateObjectAttribute":
                    ForceUpdateObjectAttribute(e);
                    break;
                default:
                    break;
            }
        }

        private void ForceUpdateObjectAttribute(SimulationEvent e)
        {

            string objectId = ((StringValue)e["ObjectID"]).value;
            string attributeName = ((StringValue)e["AttributeName"]).value;
            if (!objectProxies.ContainsKey(objectId))
            {
                return;
            }
            try
            {
                
                if (!objectProxies[objectId][attributeName].IsOwner())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            DataValue attributeValue = e["AttributeValue"];
            if (attributeValue.dataType != "WrapperType")
                return;
            attributeValue = ((WrapperValue)attributeValue).value;
            SimulationObjectProxy obj = null;

            //depending on the attribute, you might have specific functionality
            switch (attributeName)
            {
                case "Location":
                    ChangeObjectLocation(attributeValue as LocationValue, objectId);
                    break;
                case "Throttle":
                    ChangeObjectThrottle(attributeValue as DoubleValue, objectId);
                    break;
                case "Velocity":
                    //(?)
                    break;
                case "DestinationLocation":
                    ChangeObjectDestinationLocation(attributeValue as LocationValue, objectId);
                    break;
                default:
                    break;
            }

            //try
            //{
            //    obj[attributeName].SetDataValue(attributeValue);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}

            attributeValue = null;
            obj = null;
        }

        private void ChangeObjectLocation(LocationValue newLocation, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            obj["Location"].SetDataValue(newLocation);
            //Now that location has been updated, set in helper library:
            if (newLocation.exists)
            { ObjectDistances.UpdateObjectLocation(objectID, new Vec3D(newLocation)); }
            else
            { ObjectDistances.RemoveAnObject(objectID); }
        }

        private void ChangeObjectThrottle(DoubleValue newThrottle, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            LocationValue destination = obj["DestinationLocation"].GetDataValue() as LocationValue;

            if (destination != null)
            {
                if (destination.exists)
                {
                    SendMoveObjectEvent(objectID, newThrottle.value, destination);
                }
            }

            obj["Throttle"].SetDataValue(newThrottle);
        }

        private void ChangeObjectDestinationLocation(LocationValue newDestinationLocation, string objectID)
        {
            SimulationObjectProxy obj = objectProxies[objectID];
            if (obj == null)
                return;
            double throttle = ((DoubleValue)obj["Throttle"].GetDataValue()).value;

            if (newDestinationLocation.exists)
            {
                SendMoveObjectEvent(objectID, throttle, newDestinationLocation);
            }
        }

        private void SendMoveObjectEvent(string objectID, double throttle, LocationValue destinationLocation)
        {
            SimulationEvent moveObject = SimulationEventFactory.BuildEvent(ref simModel, "MoveObject");
            moveObject["ObjectID"] = DataValueFactory.BuildString(objectID);
            moveObject["DestinationLocation"] = destinationLocation;
            moveObject["Throttle"] = DataValueFactory.BuildDouble(throttle);

            distClient.PutEvent(moveObject);
        }

        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
            //landRegions.Clear();
            //activeRegions.Clear();
        }
        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();

            string id = ((StringValue)e["ID"]).value;
            string type = ((StringValue)e["ObjectType"]).value;

            if (objectProxies == null)
            {
                objectProxies = new Dictionary<string, SimulationObjectProxy>();
            }
            SimulationObjectProxy prox = bbClient.GetObjectProxy(id);
            if (prox != null)
            {
                if (!objectProxies.ContainsKey(id))
                {
                    objectProxies.Add(id, prox);
                }
                else
                {
                    objectProxies[id] = prox;
                }
            }

            if (((StringValue)e["ObjectType"]).value == "LandRegion")
            {
                StateDB.LandRegion l = new StateDB.LandRegion(((StringValue)e["ID"]).value, null);
                Polygon2D poly = new Polygon2D();
                foreach (PolygonValue.PolygonPoint pp in ((PolygonValue)((AttributeCollectionValue)e["Attributes"]).attributes["Polygon"]).points)
                {
                    poly.AddVertex(new Vec2D(pp.X, pp.Y));
                }
                l.poly = poly;
                StateDB.landRegions[l.id] = l;
            }
            else if (((StringValue)e["ObjectType"]).value == "ActiveRegion")
            {
                StateDB.ActiveRegion b = new StateDB.ActiveRegion(((StringValue)e["ID"]).value, null);
                double start = ((DoubleValue)((AttributeCollectionValue)e["Attributes"]).attributes["StartHeight"]).value;
                double end = ((DoubleValue)((AttributeCollectionValue)e["Attributes"]).attributes["EndHeight"]).value;
                Polygon3D poly2 = new Polygon3D(start, end);
                foreach (PolygonValue.PolygonPoint pp in ((PolygonValue)((AttributeCollectionValue)e["Attributes"]).attributes["Polygon"]).points)
                {
                    poly2.AddVertex(new Vec2D(pp.X, pp.Y));
                }
                b.poly = poly2;
                b.currentAbsolutePolygon = poly2;
                if (((BooleanValue)((AttributeCollectionValue)e["Attributes"]).attributes["BlocksMovement"]).value)
                {
                    b.blockingRegion = true;
                }
                else
                {
                    b.blockingRegion = false;
                }
                if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("SpeedMultiplier"))
                {
                    b.speedMultiplier = ((DoubleValue)((AttributeCollectionValue)e["Attributes"]).attributes["SpeedMultiplier"]).value;
                }
                else
                {
                    b.speedMultiplier = 1;
                }

                if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("IsVisible"))
                {
                    b.isVisible = ((BooleanValue)((AttributeCollectionValue)e["Attributes"]).attributes["IsVisible"]).value;
                }
                else
                {
                    b.isVisible = false;
                }

                if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("DisplayColor"))
                {
                    b.displayColor = ((IntegerValue)((AttributeCollectionValue)e["Attributes"]).attributes["DisplayColor"]).value;
                }
                else
                {
                    b.displayColor = -10185235;
                }

                if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("IsDynamicRegion"))
                {
                    b.isDynamicRegion = ((BooleanValue)((AttributeCollectionValue)e["Attributes"]).attributes["IsDynamicRegion"]).value;
                } else {
                    b.isDynamicRegion = false;
                }
                if (((AttributeCollectionValue)e["Attributes"]).attributes.ContainsKey("ReferencePoint") && !b.isDynamicRegion) //if isDynamic, will use its LinkedObject as Ref.
                {
                    b.referencePoint = new Vec2D(((LocationValue)((AttributeCollectionValue)e["Attributes"]).attributes["ReferencePoint"]));
                    b.currentAbsolutePolygon = GetAbsolutePolygon(b.referencePoint, poly2.Footprint);
                } else {
                    b.referencePoint = new Vec2D(0,0);
                }


                //Make separate lists for dynamic and active regions in StateDB
                if (b.isDynamicRegion) {
                    StateDB.dynamicRegions[b.id] = b;
                } else {
                    StateDB.activeRegions[b.id] = b;
                }

            }
        }
        private Polygon3D GetAbsolutePolygon(Vec2D refPoint, Polygon2D relativePolygon)
        {
            //Vec2D difference = refPoint.VectorDistanceTo(location);
            Polygon3D absolute = new Polygon3D(0, 0);

            foreach (Vec2D vertex in relativePolygon.getVertices())
            {
                absolute.AddVertex(vertex.Add(refPoint));
            }
            return absolute;
        }
        private SimulationObjectProxy GetObjectProxy(string id)
        {
            SimulationObjectProxy prox = null;

            try
            {
                prox = objectProxies[id];
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error retrieving id = '{0}' from object proxies; {1}", id, ex.Message), ex);
                return null;
            }

            return prox;
        }
        private void RevealObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            AttributeCollectionValue atts = (AttributeCollectionValue)e["Attributes"];



            if (e["ObjectID"] != null)
            {
                string id = ((StringValue)e["ObjectID"]).value;
                SimulationObjectProxy prox = null;// objectProxies[id];

                prox = GetObjectProxy(id);
                if (prox == null)
                    return;

                // initialize any object values I need to.

                foreach (string attname in atts.attributes.Keys)
                {
                    if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                    {
                        prox[attname].SetDataValue(atts[attname]);
                    }
                }
       
                //Set linked region and object to know about each other in StateDB
                if (atts.attributes.ContainsKey("LinkedRegion"))
                {
                    StringValue linkedRegion = (StringValue)atts.attributes["LinkedRegion"];
                    StateDB.physicalObjects[id].linkedRegion = linkedRegion.value;

                    StateDB.dynamicRegions[linkedRegion.value].linkedObject = id;

                }

                /*
                 * If object is docked to parent, ignore location 
                 */

                DataValue dv = prox["DockedToParent"].GetDataValue();
                if (((BooleanValue)dv).value)
                {
                    dv = prox["Location"].GetDataValue();
                    ((LocationValue)dv).exists = false;
                    prox["Location"].SetDataValue(dv);

                }

                //Initialize collision shape

                DoubleValue size = (DoubleValue)prox["Size"].GetDataValue();

                if (size.value > 0)
                {
                    collisionShapes[id] = createRelativeCollisionShape(size.value);
                }
                else
                {
                    if (collisionShapes.ContainsKey(id))
                    {
                        collisionShapes.Remove(id);
                    }
                }

                // TODO: Kill object if revealed inside an obstructing region
            }



        }

        Polygon3D createRelativeCollisionShape(Double size)
        {
            Polygon3D shape = new Polygon3D(-size,size);
            for (int i = 0; i < 8; i++)
            {
                Double theta = (2*Math.PI / 8)*i;
                double x = size * Math.Cos(theta);
                double y = size * Math.Sin(theta);
                shape.AddVertex(new Vec2D(x, y));
            }
            return shape;
            
        }

        Polygon3D createAbsoluteCollisionShape(Polygon3D relShape, LocationValue loc)
        {
            Polygon3D absShape = new Polygon3D(relShape.BottomZ + loc.Z,relShape.TopZ + loc.Z);
            foreach (Vec2D v in relShape.Footprint.getVertices())
            {
                v.X += loc.X;
                v.Y += loc.Y;
                absShape.AddVertex(v);
            }
            return absShape;

        }

        private void MoveObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies();
            DataValue dv = null;
            string id = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy obProx = objectProxies[id];

            double throttle = 0;
            double maxSpeed = 0;
            double speedMultiplier = 0;

            Vec3D curVec = new Vec3D(0, 0, 0);
            Vec3D destVec = new Vec3D(0, 0, 0);
            Vec3D velVec = new Vec3D(0, 0, 0);

            dv = obProx["Location"].GetDataValue();
            curVec.Set((LocationValue)dv);


            dv = obProx["MaximumSpeed"].GetDataValue();
            maxSpeed = ((DoubleValue)dv).value;

            dv = e["DestinationLocation"];
            destVec.Set((LocationValue)dv);

            obProx["DestinationLocation"].SetDataValue(dv);

            dv = e["Throttle"];
            throttle = ((DoubleValue)dv).value;
            obProx["Throttle"].SetDataValue(dv);

            dv = obProx["ActiveRegionSpeedMultiplier"].GetDataValue();
            speedMultiplier = ((DoubleValue)dv).value;

            velVec = curVec.VectorDistanceTo(destVec);
            velVec.Normalize();
            velVec = velVec.Multiply(maxSpeed * throttle * speedMultiplier);

            obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());

           // System.Console.WriteLine("Object {0} Velocity={1}", id.ToString(), velVec.ToString());


        }

        private void SubplatformDock(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record

            string objectID = ((StringValue)e["ObjectID"]).value;


            SimulationObjectProxy obProx = objectProxies[objectID];

            bool dockedToParent = ((BooleanValue)obProx["DockedToParent"].GetDataValue()).value;

            if (dockedToParent)
            {
                // TODO Should log an error
                return;
            }

            //string parentID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;
            string parentID = ((StringValue)e["ParentObjectID"]).value;

            DataValue dv = obProx["Location"].GetDataValue();
            ((LocationValue)dv).exists = false;
            obProx["Location"].SetDataValue(dv);
            //Now that location has been updated, set in helper library:
            ObjectDistances.RemoveAnObject(objectID);

            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                time,
                                ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                objectID + " has been docked to " + parentID));
        }
        private void ObjectCollision(SimulationEvent e)
        {
            DataValue dv = null;
            string id1 = ((StringValue)e["ObjectID1"]).value;
            SimulationObjectProxy obProx1 = objectProxies[id1];
            string id2 = ((StringValue)e["ObjectID2"]).value;
            SimulationObjectProxy obProx2 = objectProxies[id2];




            //Vec3D velVec = new Vec3D(0, 0, 0);



            //obProx1["Velocity"].SetDataValue(velVec.ToVelocityValue());
            //obProx2["Velocity"].SetDataValue(velVec.ToVelocityValue());

            /*
            SimulationEvent done = SimulationEventFactory.BuildEvent(ref simModel, "MoveDone");
            ((IntegerValue)done["Time"]).value = time;
            ((StringValue)done["ObjectID"]).value = id1;
            distClient.PutEvent(done);
            ((StringValue)done["ObjectID"]).value = id2;
            distClient.PutEvent(done);
             */

            distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel,
                                                              time,
                                                              id1,
                                                              "ObjectCollision"));
            distributor.viewProBackChannelEvents.Add(SimUtility.BuildMoveDoneEvent(ref simModel,
                                                              time,
                                                              id1,
                                                              "ObjectCollision"));
            distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel,
                                                              time,
                                                              id2,
                                                              "ObjectCollision"));
            distributor.viewProBackChannelEvents.Add(SimUtility.BuildMoveDoneEvent(ref simModel,
                                                              time,
                                                              id2,
                                                              "ObjectCollision"));

            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(obProx1["OwnerID"].GetDataValue())).value,
                                                                   id1 + " was stopped by collision with " + id2));
            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                                   time,
                                                                   ((StringValue)(obProx2["OwnerID"].GetDataValue())).value,
                                                                   id2 + " was stopped by collision with " + id1));

            //dv = obProx1["Throttle"].GetDataValue();
            //((DoubleValue)dv).value = 0;
            //obProx1["Throttle"].SetDataValue(dv);
            //obProx2["Throttle"].SetDataValue(dv);
            // TODO:  handle with state change later on


        }


        private bool OnLandRegion(Vec2D loc)
        {
            foreach (StateDB.LandRegion reg in StateDB.landRegions.Values)
            {
                if (Polygon2D.IsPointInside(reg.poly, loc))
                {
                    return true;
                }
            }

            return false;
        }



        private bool IsOnLand(Vec3D loc)
        {
            if (loc.Z > 0)
            {
                return false;
            }
            else
            {
                return OnLandRegion(new Vec2D(loc.X, loc.Y));
            }

        }



        private bool IsOnSea(Vec3D loc)
        {
            if (loc.Z > 0)
            {
                return false;
            }
            else
            {
                return !OnLandRegion(new Vec2D(loc.X, loc.Y));
            }

        }


        private bool IsObstructed(Vec3D cur, Vec3D dest, String myID)
        {
            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
                if (reg.blockingRegion && Polygon3D.DoesLineCross(reg.poly, cur, dest))
                {
                    return true;
                }
            }
            foreach (StateDB.ActiveRegion reg in StateDB.dynamicRegions.Values)
            {
                if (reg.linkedObject!= myID && (reg.blockingRegion && Polygon3D.DoesLineCross(reg.currentAbsolutePolygon, cur, dest) || Polygon3D.IsPointInside(reg.currentAbsolutePolygon, cur)))
                {
                    return true;
                }
            }

            foreach (String id in collisionShapes.Keys)
            {
                if (id != myID)
                {
                    SimulationObjectProxy obProx = objectProxies[id];
                    LocationValue loc = (LocationValue)obProx["Location"].GetDataValue();
                    if (loc.exists)
                    {
                        Polygon3D absShape = createAbsoluteCollisionShape(collisionShapes[id], loc);
                        if (Polygon3D.DoesLineCross(absShape, cur, dest))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsInObstruction(Vec3D cur)
        {
            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
                if (reg.blockingRegion && Polygon3D.IsPointInside(reg.poly, cur))
                {
                    return true;
                }
            }
            foreach (StateDB.ActiveRegion reg in StateDB.dynamicRegions.Values)
            {
                if (reg.blockingRegion && Polygon3D.IsPointInside(reg.poly, cur))
                {
                    return true;
                }
            }

            return false;
        }
        private List<String> GetActiveRegions(Vec3D cur)
        {
            List<string> regions = new List<string>();

            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
                if (Polygon3D.IsPointInside(reg.currentAbsolutePolygon, cur))
                {
                    regions.Add(reg.id);
                }
            }
            foreach (StateDB.ActiveRegion reg in StateDB.dynamicRegions.Values)
            {
                if (Polygon3D.IsPointInside(reg.currentAbsolutePolygon, cur))
                {
                    regions.Add(reg.id);
                }
            }

            return regions;
        }

        private double GetActiveRegionSpeedMultiplier(List<string> regionIDs)
        {
            double r = 1;

            foreach (string regID in regionIDs)
            {
                if (StateDB.activeRegions.ContainsKey(regID) && !StateDB.activeRegions[regID].blockingRegion)
                {
                    r *= StateDB.activeRegions[regID].speedMultiplier;
                }
            }
            foreach (string regID in regionIDs)
            {
                if (StateDB.dynamicRegions.ContainsKey(regID) && !StateDB.dynamicRegions[regID].blockingRegion)
                {
                    r *= StateDB.dynamicRegions[regID].speedMultiplier;
                }
            }

            return r;
        }
        /*
        Vec3D FindLandBoundaryIntersection(Vec3D fromP, Vec3D toP)
        {
            Vec2D r = null;

            Vec2D myFromP = new Vec2D(fromP.X, fromP.Y);
            Vec2D myToP = new Vec2D(toP.X, toP.Y);

            List<Vec2D> points = new List<Vec2D>();
            foreach (LandRegion reg in landRegions.Values)
            {
                r = Polygon2D.FindIntersect(reg.poly, myFromP, myToP);
                if (r != null)
                {
                    points.Add(r);
                }
            }

            if (points.Count == 0)
            {
                return null;
            }
            Vec2D closest = new Vec2D(points[0]);

            foreach (Vec2D p in points)
            {
                if (myFromP.ScalerDistanceTo(p) < myFromP.ScalerDistanceTo(closest))
                {
                    closest.Set(p);
                }
            }

            // return point on from side of intersection.
            if (myFromP.X < closest.X)
            {
                closest.X -= 1;
            }
            else if (myFromP.X > closest.X)
            {
                closest.X += 1;
            }
            if (myFromP.Y < closest.Y)
            {
                closest.Y -= 1;
            }
            else if (myFromP.Y > closest.Y)
            {
                closest.Y += 1;
            }

            return new Vec3D(closest.X, closest.Y, fromP.Z);
        }
        */
        /*
        Vec3D FindObstructionBoundaryIntersection(Vec3D fromP, Vec3D toP)
        {
            Vec3D r = null;

            List<Vec3D> points = new List<Vec3D>();
            foreach (BlockingRegion reg in blockingRegions.Values)
            {
                r = Polygon3D.FindIntersect(reg.poly, fromP, toP);
                if (r != null)
                {
                    points.Add(r);
                }
            }

            if (points.Count == 0)
            {
                return null;
            }
            Vec3D closest = new Vec3D(points[0]);

            foreach (Vec3D p in points)
            {
                if (fromP.ScalerDistanceTo(p) < fromP.ScalerDistanceTo(closest))
                {
                    closest.Set(p);
                }
            }

            // return point on from side of intersection.
            if (fromP.X < closest.X)
            {
                closest.X -= 1;
            }
            else if (fromP.X > closest.X)
            {
                closest.X += 1;
            }
            if (fromP.Y < closest.Y)
            {
                closest.Y -= 1;
            }
            else if (fromP.Y > closest.Y)
            {
                closest.Y += 1;
            }

            return closest;
        }
        */
        private void TimeTick(SimulationEvent e)
        {
            int oldTime = time;
            DataValue dv = null;

            Vec3D landIntersect;
            Vec3D obstructionIntersect;

            Vec3D curLocVec = new Vec3D(0, 0, 0);
            Vec3D newLocVec = new Vec3D(0, 0, 0);
            Vec3D destVec = new Vec3D(0, 0, 0);
            Vec3D velVec = new Vec3D(0, 0, 0);

            dv = e["Time"];
            time = ((IntegerValue)dv).value;

            double dTime = ((double)(time - oldTime)) / 1000;
            SimulationObjectProxy obProx = null;
            SimulationObjectProxy parentProx = null;

            bool launchStarted, launchDone;
            int launchEndTime;

            bool pursueStarted;
            string pursueTargetID;
            string parentObjectID;

            ScoringDB.ActorFrame actorFrame = new ScoringDB.ActorFrame();
            foreach (string id in objectProxies.Keys)
            {
                obProx = objectProxies[id];


                //*********************************
                // Launch movement logic 
                //*********************************

                launchStarted = ((BooleanValue)obProx["LaunchStarted"].GetDataValue()).value;
                launchDone = ((BooleanValue)obProx["LaunchDone"].GetDataValue()).value;

                pursueStarted = ((BooleanValue)obProx["PursueStarted"].GetDataValue()).value;
                pursueTargetID = ((StringValue)obProx["PursueTargetID"].GetDataValue()).value;

                if (launchStarted == false && launchDone == true)
                {
                    dv = obProx["LaunchDone"].GetDataValue();
                    ((BooleanValue)dv).value = false;
                    obProx["LaunchDone"].SetDataValue(dv);
                    launchDone = false;
                }
                if (launchStarted == true && launchDone == false)
                {
                    parentObjectID = ((StringValue)obProx["ParentObjectID"].GetDataValue()).value;
                    parentProx = objectProxies[parentObjectID];
                    launchEndTime = ((IntegerValue)obProx["LaunchEndTime"].GetDataValue()).value;
                    if (time >= launchEndTime)
                    {
                        Vec3D parentVec = new Vec3D((LocationValue)parentProx["Location"].GetDataValue());
                        dv = obProx["LaunchDestinationLocation"].GetDataValue();
                        Vec3D launchDestVec = new Vec3D((LocationValue)dv);
                        bool sendMoveRequest = ((LocationValue)dv).exists;

                        obProx["Location"].SetDataValue(parentVec.ToLocationValue());
                        //Now that location has been updated, set in helper library:
                        ObjectDistances.UpdateObjectLocation(id, parentVec);

                        dv = obProx["LaunchDone"].GetDataValue();
                        ((BooleanValue)dv).value = true;
                        obProx["LaunchDone"].SetDataValue(dv);

                        if (sendMoveRequest)
                        {
                            string userID = ((StringValue)obProx["OwnerID"].GetDataValue()).value;
                            distClient.PutEvent(SimUtility.BuildMoveObjectRequestEvent(ref simModel, time, userID, id, launchDestVec, 1.0));
                        }

                        if (((BooleanValue)obProx["LaunchIsWeapon"].GetDataValue()).value == true)
                        {
                            dv = obProx["PursueStarted"].GetDataValue();
                            ((BooleanValue)dv).value = true;
                            obProx["PursueStarted"].SetDataValue(dv);
                            pursueStarted = true;

                            dv = obProx["LaunchWeaponTargetID"].GetDataValue();
                            pursueTargetID = ((StringValue)dv).value;

                            dv = obProx["PursueTargetID"].GetDataValue();
                            ((StringValue)dv).value = pursueTargetID;
                            obProx["PursueTargetID"].SetDataValue(dv);

                            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                           time,
                                                           ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                                           id + " has been launched from " + parentObjectID + " at " + pursueTargetID));
                            distClient.PutEvent(SimUtility.BuildHistory_SubplatformLaunchEvent(ref simModel,
                                                                                               time,
                                                                                               id,
                                                                                               parentObjectID,
                                                                                               parentVec,
                                                                                               launchDestVec,
                                                                                               true,
                                                                                               pursueTargetID));
                        }
                        else
                        {
                            distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                                           time,
                                                           ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                                           id + " has been undocked from " + parentObjectID));
                            distClient.PutEvent(SimUtility.BuildHistory_SubplatformLaunchEvent(ref simModel,
                                                                                               time,
                                                                                               id,
                                                                                               parentObjectID,
                                                                                               parentVec,
                                                                                               launchDestVec,
                                                                                               false,
                                                                                               ""));
                        }
                    }
                }

                if (pursueStarted)
                {
                    if (objectProxies.ContainsKey(pursueTargetID))
                    {
                        dv = objectProxies[pursueTargetID]["Location"].GetDataValue();

                        String targetState = ((StringValue)objectProxies[pursueTargetID]["State"].GetDataValue()).value;
                        if (!((LocationValue)dv).exists || targetState == "Dead")
                        {
                            //if my target is dead or not existant, kill this weapon too.

                            obProx["PursueStarted"].SetDataValue(DataValueFactory.BuildBoolean(false));
                            obProx["PursueTargetID"].SetDataValue(DataValueFactory.BuildString(String.Empty));
                            distClient.PutEvent(SimUtility.BuildStateChangeEvent(ref simModel, time, id, "Dead"));
                            continue;
                        }

                        destVec.Set((LocationValue)dv);
                        obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());

                        dv = obProx["Throttle"].GetDataValue();
                        ((DoubleValue)dv).value = 1;
                        obProx["Throttle"].SetDataValue(dv);

                        distClient.PutEvent(SimUtility.BuildHistory_PursueEvent(ref simModel,
                                                                                time,
                                                                                id,
                                                                                curLocVec,
                                                                                pursueTargetID,
                                                                                destVec));
                    }
                }


                //*********************************
                //   MoveObject logic
                //*********************************

                //
                // Ignore objects that don't have a location
                //
                dv = obProx["Location"].GetDataValue();
                if (!((LocationValue)dv).exists)
                {
                    continue;
                }
                curLocVec.Set((LocationValue)dv);
                

                //dv = obProx["Velocity"].GetDataValue();
                //velVec.Set((VelocityValue)dv);

                dv = obProx["DestinationLocation"].GetDataValue();
                if (!((LocationValue)dv).exists)
                {
                    destVec.Set(curLocVec);
                    obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());
                }
                else
                {
                    destVec.Set((LocationValue)dv);
                }

                dv = obProx["Throttle"].GetDataValue();
                double throttle = ((DoubleValue)dv).value;

                dv = obProx["MaximumSpeed"].GetDataValue();
                double maxSpeed = ((DoubleValue)dv).value;




                //
                // If the object's throttle or maximum speed are zero, set velocity to (0,0,0) and 
                // set the destination to the current location
                //
                if (throttle == 0 || maxSpeed == 0 || ((StringValue)obProx["State"].GetDataValue()).value == "Dead")
                {
                    velVec.Set(0, 0, 0);
                    obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());

                    destVec.Set(curLocVec);
                    obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());

                    dv = obProx["Throttle"].GetDataValue();
                    ((DoubleValue)dv).value = 0;
                    obProx["Throttle"].SetDataValue(dv);

                }


                //
                // Update what active regions the object is in.
                //
                StateDB.physicalObjects[id].activeRegions = GetActiveRegions(curLocVec);
                dv = obProx["InActiveRegions"].GetDataValue();
                StateDB.physicalObjects[id].activeRegions.Remove(id);// dont want to be blocked by itself.
                ((StringListValue)dv).strings = StateDB.physicalObjects[id].activeRegions;
                obProx["InActiveRegions"].SetDataValue(dv);


                double speedMultiplier = GetActiveRegionSpeedMultiplier(StateDB.physicalObjects[id].activeRegions);
                dv = obProx["ActiveRegionSpeedMultiplier"].GetDataValue();
                if (((DoubleValue)dv).value != speedMultiplier)
                {
                    ((DoubleValue)dv).value = speedMultiplier;
                    obProx["ActiveRegionSpeedMultiplier"].SetDataValue(dv);
                    //distClient.PutEvent(SimUtility.BuildActiveRegionSpeedMultiplierUpdateEvent(ref simModel, time, id));
                    distributor.viewProBackChannelEvents.Add(SimUtility.BuildActiveRegionSpeedMultiplierUpdateEvent(ref simModel, time, id));
                }

                if (time % 1000 == 0)
                {
                    actorFrame.objectID = id;
                    actorFrame.speciesName = StateDB.physicalObjects[id].speciesName;
                    actorFrame.ownerID = StateDB.physicalObjects[id].ownerID;
                    actorFrame.activeRegions = StateDB.physicalObjects[id].activeRegions;

                    ScoringDB.UpdateScore_ObjectExists(actorFrame);
                }

                //
                // Update the object's velocity based on its destinations, throttle, and maximum speed.
                //

                if (curLocVec.ScalerDistanceTo(destVec) > 0.01)
                {
                    if (curLocVec.ScalerDistanceTo(destVec) >= (maxSpeed * throttle * dTime * speedMultiplier))
                    {
                        velVec = curLocVec.VectorDistanceTo(destVec);
                        velVec.Normalize();
                        velVec = velVec.Multiply(maxSpeed * throttle * speedMultiplier);
                    }
                    else
                    {
                        velVec = destVec.Subtract(curLocVec);
                    }

                    obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());
                    //
                    // Update the object's location based on its velocity
                    //
                    newLocVec.X = curLocVec.X + (velVec.X * dTime);
                    newLocVec.Y = curLocVec.Y + (velVec.Y * dTime);
                    newLocVec.Z = curLocVec.Z + (velVec.Z * dTime);





                    // TODO: Do obstruction check here
                    /*
                    obstructionIntersect = FindObstructionBoundaryIntersection(curLocVec, newLocVec);
                    landIntersect = FindLandBoundaryIntersection(curLocVec, newLocVec);
                    if (obstructionIntersect != null)
                    {
                        curLocVec.Set(obstructionIntersect);
                        obProx["Location"].SetDataValue(curLocVec.ToLocationValue());
                        destVec.Set(curLocVec);
                        obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());
                        distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel, time, id));
                        dv = obProx["Throttle"].GetDataValue();
                        ((DoubleValue)dv).value = 0;
                        obProx["Throttle"].SetDataValue(dv);
                    }
                    else if (landIntersect != null && (obProx.GetObjectType() != "AirObject"))
                    {
                        curLocVec.Set(landIntersect);
                        obProx["Location"].SetDataValue(curLocVec.ToLocationValue());
                        destVec.Set(curLocVec);
                        obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());
                        distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel, time, id));
                        dv = obProx["Throttle"].GetDataValue();
                        ((DoubleValue)dv).value = 0;
                        obProx["Throttle"].SetDataValue(dv);
                    }
                    else
                    {
                        curLocVec.Set(newLocVec);
                        obProx["Location"].SetDataValue(curLocVec.ToLocationValue());
                    } */

                    bool stopMotion = false;
                    switch (obProx.GetObjectType())
                    {
                        case "SeaObject":
                            if (!IsOnSea(newLocVec))
                            {
                                stopMotion = true;
                                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                            time,
                                            ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                            id + " has been blocked by land"));
                            }
                            break;
                        case "LandObject":
                            if (!IsOnLand(newLocVec))
                            {
                                stopMotion = true;
                                distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                            time,
                                            ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                            id + " has been blocked by sea"));
                            }
                            break;
                        default:
                            break;
                    }

                    if (IsObstructed(curLocVec, newLocVec,id))
                    {
                        stopMotion = true;
                        //distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                        //                    time,
                        //                    ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                        //                    id + " has been blocked by an obstruction"));
                    }

                    if (!stopMotion)
                    {
                        curLocVec.Set(newLocVec);
                        obProx["Location"].SetDataValue(curLocVec.ToLocationValue());
                        //Now that location has been updated, set in helper library:
                        ObjectDistances.UpdateObjectLocation(id, curLocVec);
                    }
                    else
                    {
                        obProx["Location"].SetDataValue(curLocVec.ToLocationValue());
                        //Now that location has been updated, set in helper library:
                        ObjectDistances.UpdateObjectLocation(id, curLocVec);
                        destVec.Set(curLocVec);
                        obProx["DestinationLocation"].SetDataValue(destVec.ToLocationValue());
                        distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel, time, id, "Obstruction"));
                        distributor.viewProBackChannelEvents.Add(SimUtility.BuildMoveDoneEvent(ref simModel, time, id, "Obstruction"));
                        dv = obProx["Throttle"].GetDataValue();
                        ((DoubleValue)dv).value = 0;
                        obProx["Throttle"].SetDataValue(dv);
                        distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                            time,
                                            ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                            id + " was stopped by an obstruction"));
                    }

                }
                else // if the object has reached its destination and is not pursuing, send MoveDone
                {


                    velVec.Set(0, 0, 0);
                    newLocVec.Set(destVec);
                    obProx["Location"].SetDataValue(newLocVec.ToLocationValue());
                    //Now that location has been updated, set in helper library:
                    ObjectDistances.UpdateObjectLocation(id, newLocVec);
                    obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());


                    if (!pursueStarted && throttle > 0)
                    {
                        /* TODO: Make sure MoveDone is being sent correctly.*/

                        distClient.PutEvent(SimUtility.BuildMoveDoneEvent(ref simModel, time, id, "ReachedDestination"));
                        distributor.viewProBackChannelEvents.Add(SimUtility.BuildMoveDoneEvent(ref simModel, time, id, "ReachedDestination"));
                        dv = obProx["Throttle"].GetDataValue();
                        ((DoubleValue)dv).value = 0;
                        obProx["Throttle"].SetDataValue(dv);

                        distClient.PutEvent(SimUtility.BuildSystemMessageEvent(ref simModel,
                                            time,
                                            ((StringValue)(obProx["OwnerID"].GetDataValue())).value,
                                            id + " has reached its destination"));
                    }



                }

                

                /* TODO: Ignore obstruction code for now */
                /*
                if (((StringValue)obProx["State"].GetDataValue()).value != "Dead" &&
                    ((obProx.GetObjectType() == "LandObject" && !IsOnLand(newLocVec)) ||
                    (obProx.GetObjectType() == "SeaObject" && !IsOnSea(newLocVec)) ||
                    IsObstructed(curLocVec,newLocVec) || IsInObstruction(newLocVec)))
                {
                    velVec.Set(0, 0, 0);
                    dv = obProx["Velocity"].GetDataValue();

                    obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());
                    SimulationEvent done = SimulationEventFactory.BuildEvent(ref simModel, "MoveDone");
                    ((IntegerValue)done["Time"]).value = time;
                    ((StringValue)done["ObjectID"]).value = id;
                    distClient.PutEvent(done);
                    System.Console.WriteLine("Motion: object {0} stopped by land boundery", id);

                    dv = obProx["Throttle"].GetDataValue();
                    ((DoubleValue)dv).value = 0;
                    obProx["Throttle"].SetDataValue(dv);

                    if (((obProx.GetObjectType() == "LandObject" && !IsOnLand(curLocVec)) ||
                        obProx.GetObjectType() == "SeaObject" && !IsOnSea(curLocVec)) ||
                        IsInObstruction(curLocVec))
                    {
                        SimulationEvent dead = SimulationEventFactory.BuildEvent(ref simModel, "StateChange");
                        ((IntegerValue)dead["Time"]).value = time;
                        ((StringValue)dead["ObjectID"]).value = id;
                        ((StringValue)dead["NewState"]).value = "Dead";
                        distClient.PutEvent(dead);
                    }

                    return;
                }
                 * */

            }
        }
        private void StateChange(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            string id = ((StringValue)e["ObjectID"]).value;
            SimulationObjectProxy prox = null; // objectProxies[id];

            prox = GetObjectProxy(id);
            if (prox == null)
                return;

            string newState = ((StringValue)e["NewState"]).value;

            if (newState == "Dead")
            {
                LocationValue newLoc = prox["Location"].GetDataValue() as LocationValue;
                newLoc.exists = false;
                prox["Location"].SetDataValue(newLoc);
                newLoc = prox["DestinationLocation"].GetDataValue() as LocationValue;
                newLoc.exists = false;
                prox["DestinationLocation"].SetDataValue(newLoc);
                //prox["IsVisible"].SetDataValue(DataValueFactory.BuildBoolean(false));

                //Unlink region and object in StateDB
                StringValue linkedRegion = prox["LinkedRegion"].GetDataValue() as StringValue;
                String regionID = linkedRegion.value;
                if (regionID.Trim() != "")
                {
                    StateDB.dynamicRegions[linkedRegion.value].isVisible = false;
                    StateDB.dynamicRegions[linkedRegion.value].linkedObject = "";
                    StateDB.physicalObjects[id].linkedRegion = "";
                    prox["LinkedRegion"].SetDataValue(DataValueFactory.BuildString(""));
                }
            }

            DataValue dv = prox["StateTable"].GetDataValue();
            if (((StateTableValue)dv).states.ContainsKey(newState))
            {
                DataValue dv2 = ((StateTableValue)dv).states[newState];
                foreach (string attname in ((AttributeCollectionValue)dv2).attributes.Keys)
                {
                    if (prox.GetKeys().Contains(attname) && prox[attname].IsOwner())
                    {
                        prox[attname].SetDataValue(((AttributeCollectionValue)dv2).attributes[attname]);
                    }
                }
            }

            DoubleValue size = (DoubleValue)prox["Size"].GetDataValue();
            if (size.value > 0 && newState != "Dead")
            {
                collisionShapes[id] = createRelativeCollisionShape(size.value);
            }
            else
            {
                if (collisionShapes.ContainsKey(id))
                {
                    collisionShapes.Remove(id);
                }
            }

        }
        public string GetSimulatorName()
        {
            return "Motion";
        }
    }

}
