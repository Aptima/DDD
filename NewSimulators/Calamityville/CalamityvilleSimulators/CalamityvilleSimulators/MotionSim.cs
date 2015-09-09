using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.CalamityvilleSimulators.Motion
{
    public class MotionSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;
        private double _maxTurnAngle = 3;
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
            bbClient.Subscribe("PhysicalObject", "PursueStarted", true, true);
            bbClient.Subscribe("PhysicalObject", "Heading", true, true);

            bbClient.Subscribe("PhysicalObject", "PursueTargetID", true, true);
            bbClient.Subscribe("PhysicalObject", "ParentObjectID", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchIsWeapon", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchWeaponTargetID", true, false);
            bbClient.Subscribe("PhysicalObject", "LaunchDestinationLocation", true, false);
            bbClient.Subscribe("PhysicalObject", "InActiveRegions", true, true);
            bbClient.Subscribe("PhysicalObject", "ActiveRegionSpeedMultiplier", true, true);
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
                    ChangeObjectVelocity(attributeValue as VelocityValue, objectId);
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
        private void ChangeObjectVelocity(VelocityValue newVelocity, string objectID)
        { 
        
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


                //b.displayColor = -10185235;
                //b.isVisible = true;
                StateDB.activeRegions[b.id] = b;
            }
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
                if (!atts.attributes.ContainsKey("Heading"))
                {
                    //default heading is 0,1,0 for now, which should be heading 0, due north
                    Vec3D defaultHeading = new Vec3D(0, 1, 0);
                    double degHeading = 180 / Math.PI * Math.Atan2(defaultHeading.X, defaultHeading.Y);
                    prox["Heading"].SetDataValue(DataValueFactory.BuildDouble(degHeading));
                }
                else
                {
                }
                // TODO: Kill object if revealed inside an obstructing region
            }



        }

        Polygon3D createRelativeCollisionShape(Double size)
        {
            Polygon3D shape = new Polygon3D(-size, size);
            for (int i = 0; i < 8; i++)
            {
                Double theta = (2 * Math.PI / 8) * i;
                double x = size * Math.Cos(theta);
                double y = size * Math.Sin(theta);
                shape.AddVertex(new Vec2D(x, y));
            }
            return shape;

        }

        Polygon3D createAbsoluteCollisionShape(Polygon3D relShape, LocationValue loc)
        {
            Polygon3D absShape = new Polygon3D(relShape.BottomZ + loc.Z, relShape.TopZ + loc.Z);
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
            //AD: Inserted a return here as all motion logic lives in the DIS adapter now.
            return;


            //TODO: adjust heading (current, expected, delta)
            //use radius math to calculate turn radius

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

            return false;
        }
        private List<String> GetActiveRegions(Vec3D cur)
        {
            List<string> regions = new List<string>();

            foreach (StateDB.ActiveRegion reg in StateDB.activeRegions.Values)
            {
                if (Polygon3D.IsPointInside(reg.poly, cur))
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

            return r;
        }

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
            Vec3D headingVector = new Vec3D(0, 0, 0);


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
                #region Launch Logic
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
                #endregion

                #region Pursue Logic
                /*if (pursueStarted)
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
                }*/
                #endregion

                //*********************************
                //   MoveObject logic
                //*********************************
                //
                /*
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
                */

                //
                // Update what active regions the object is in.
                //
                StateDB.physicalObjects[id].activeRegions = GetActiveRegions(curLocVec);
                dv = obProx["InActiveRegions"].GetDataValue();
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
                /*
                if (curLocVec.ScalerDistanceTo(destVec) > 0.01)
                {
                    double degCurHeading = 0.0;
                    double degDestHeading = 0.0;
                    // headingVector = new Vec3D(((VelocityValue)obProx["Heading"].GetDataValue()));
                    degCurHeading = ((DoubleValue)obProx["Heading"].GetDataValue()).value;//180 / Math.PI * Math.Atan2(headingVector.X, headingVector.Y);
                    double tx = Math.Sin(degCurHeading / 180 * Math.PI);// /180*Math.PI;
                    double ty = Math.Cos(degCurHeading / 180 * Math.PI);///180*Math.PI;
                    headingVector = new Vec3D(tx, ty, 0);
                    //TODO: Adjust velocity vector based on heading
                    if (curLocVec.ScalerDistanceTo(destVec) >= (maxSpeed * throttle * dTime * speedMultiplier))
                    {
                        velVec = headingVector;
                        velVec = velVec.Multiply(maxSpeed * throttle * speedMultiplier);
                        //
                        // Update the object's location based on its velocity
                        //
                        newLocVec.X = curLocVec.X + (velVec.X * dTime);
                        newLocVec.Y = curLocVec.Y + (velVec.Y * dTime);
                        newLocVec.Z = curLocVec.Z + (velVec.Z * dTime);
                    }
                    else
                    {
                        velVec = destVec.Subtract(curLocVec);
                        //
                        // Update the object's location based on its velocity
                        //
                        newLocVec.X = curLocVec.X + (velVec.X);
                        newLocVec.Y = curLocVec.Y + (velVec.Y);
                        newLocVec.Z = curLocVec.Z + (velVec.Z);
                    }

                    //TODO: adjust position based on velocity vector
                    obProx["Velocity"].SetDataValue(velVec.ToVelocityValue());
                    //
                    // Update the object's location based on its velocity
                    // -- AD: Moved up, as dTime was exponentially reducing object speed rather than stopping quickly.
                    //newLocVec.X = curLocVec.X + (velVec.X * dTime);
                    //newLocVec.Y = curLocVec.Y + (velVec.Y * dTime);
                    //newLocVec.Z = curLocVec.Z + (velVec.Z * dTime);

                    //TODO: Adjust heading if needed                    

                    if (curLocVec.ScalerDistanceTo(destVec) >= (maxSpeed * throttle * dTime * speedMultiplier))
                    {
                        headingVector = curLocVec.VectorDistanceTo(destVec);
                        headingVector.Normalize();
                        //velVec = velVec.Multiply(maxSpeed * throttle * speedMultiplier);
                    }
                    else
                    {
                        headingVector = destVec.Subtract(curLocVec);
                        headingVector.Normalize();
                    }
                    degDestHeading = 180 / Math.PI * Math.Atan2(headingVector.X, headingVector.Y);
                    double newDiff = 0.0;
                    if (degDestHeading > degCurHeading)
                    {
                        newDiff = degDestHeading - degCurHeading;
                    }
                    else
                    {
                        newDiff = degCurHeading - degDestHeading;
                    }
                    if (degDestHeading < 0)
                        degDestHeading = 360 + degDestHeading;
                    if (degCurHeading < 0)
                        degCurHeading = 360 + degCurHeading;

                    if (degDestHeading != degCurHeading)
                    {
                        double deltaDegrees = _maxTurnAngle;
                        double diff = Math.Abs(degDestHeading - degCurHeading);
                        deltaDegrees = Math.Min(deltaDegrees, diff);

                        if ((degDestHeading > degCurHeading && diff < 180) || (degDestHeading < degCurHeading && diff > 180))
                        {
                            //add to heading
                            degCurHeading += deltaDegrees;
                        }
                        else
                        {
                            //subtract from heading
                            degCurHeading -= deltaDegrees;
                        }
                        degCurHeading = degCurHeading % 360;
                        Vec3D newHeadingVector = new Vec3D(Math.Sin(degCurHeading / 180 * Math.PI), Math.Cos(degCurHeading / 180 * Math.PI), 0);
                        headingVector = newHeadingVector;
                    }
                    else
                    {

                    }
                    //obProx["Heading"].SetDataValue(headingVector.ToVelocityValue());
                    obProx["Heading"].SetDataValue(DataValueFactory.BuildDouble(degCurHeading));
                    Console.WriteLine(String.Format("Tick; New Location:({0},{1}); New Heading Vector:({2},{3}); Heading:{4}", newLocVec.X, newLocVec.Y, headingVector.X, headingVector.Y, degCurHeading));
                    //Console.WriteLine("New Location: {"+newLocVec.X+","+newLocVec.Y+"}");
                    //Console.WriteLine("New Velocity Vector: {" + velVec.X + "," + velVec.Y + "}");



                    // TODO: Do obstruction check here

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

                    if (IsObstructed(curLocVec, newLocVec, id))
                    {
                        stopMotion = true;
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
                        // TODO: Make sure MoveDone is being sent correctly.

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
                }*/
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
