using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.CollisionDetection
{

    public class CollisionDetectionSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;

        public CollisionDetectionSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;
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
            //distClient.Subscribe("MoveObject");
            //distClient.Subscribe("TimeTick");
            //distClient.Subscribe("ResetSimulation");

            blackboard.RegisterClient(ref bbClient);
            bbClient.Subscribe("PhysicalObject", "ID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "Velocity", true, false);
            bbClient.Subscribe("PhysicalObject", "Size", true, false);
            bbClient.Subscribe("PhysicalObject", "DockedToParent", true, false);
            bbClient.Subscribe("PhysicalObject", "Capability", true, false);
            bbClient.Subscribe("PhysicalObject", "Vulnerability", true, false);
            bbClient.Subscribe("PhysicalObject", "TargetsInRange", true, true);
            bbClient.Subscribe("PhysicalObject", "State", true, false);
            //bbClient.Subscribe("PhysicalObject", "MaximumSpeed", true, false);
            //bbClient.Subscribe("PhysicalObject", "Throttle", true, false);
            //bbClient.Subscribe("PhysicalObject", "DestinationLocation", true, false);
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
                return;

        }
        
        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
        }
        private void NewObject(SimulationEvent e)
        {
            //objectProxies = bbClient.GetObjectProxies(); // update my objects record
            // initialize any object values I need to.
            string id = ((StringValue)e["ID"]).value;
            string type = ((StringValue)e["ObjectType"]).value;

            if (objectProxies == null)
            {
                objectProxies = new Dictionary<string, SimulationObjectProxy>();
            }
            SimulationObjectProxy prox = bbClient.GetObjectProxy(id);
            if (prox == null)
            { return; }
            if (!objectProxies.ContainsKey(id))
            {
                objectProxies.Add(id, prox);
            }
            else
            {
                objectProxies[id] = prox;
            }
        }

        private void TimeTick(SimulationEvent e)
        {
            int oldTime = time;
            DataValue dv = null;


            //Vec3D curVec = new Vec3D(0, 0, 0);
            //Vec3D destVec = new Vec3D(0, 0, 0);
            //Vec3D velVec = new Vec3D(0, 0, 0);

            Vec3D loc1 = new Vec3D(0, 0, 0);
            Vec3D loc2 = new Vec3D(0, 0, 0);
            Vec3D next1 = new Vec3D(0, 0, 0);
            Vec3D next2 = new Vec3D(0, 0, 0);

            Vec3D vel1 = new Vec3D(0, 0, 0);
            Vec3D vel2 = new Vec3D(0, 0, 0);

            dv = e["Time"];
            time = ((IntegerValue)dv).value;

            double dTime = ((double)(time - oldTime)) / 1000;
            SimulationObjectProxy obProx1 = null;
            SimulationObjectProxy obProx2 = null;
            

            List<string> ids = new List<string>(objectProxies.Keys);

            string id1;

            SimulationEvent collision = null;
            double distance;
            double? d;
            while (ids.Count > 0)
            {
                id1 = ids[0];
                ids.Remove(id1);
                

                foreach (string id2 in ids)
                {
                    d = ObjectDistances.GetScalarDistanceBetweenObjects(id1, id2);

                    if (d == null)
                    { 
                    // Don't look for collisions if they aren't on the screen
                        continue;
                    }
                    distance = d.Value;

                    double objectSize1 = 0;
                    double objectSize2 = 0;
                    obProx1 = objectProxies[id1];
                    obProx2 = objectProxies[id2];


                    //// Don't look for collisions if they aren't on the screen

                    dv = obProx1["Location"].GetDataValue();
                    //if (!((LocationValue)dv).exists)
                    //{
                    //    continue;
                    //}
                    loc1.Set((LocationValue)dv);
                    dv = obProx2["Location"].GetDataValue();
                    //if (!((LocationValue)dv).exists)
                    //{
                    //    continue;
                    //}
                    loc2.Set((LocationValue)dv);

                    if (((BooleanValue)obProx1["DockedToParent"].GetDataValue()).value)
                    {
                        continue;
                    }
                    if (((BooleanValue)obProx2["DockedToParent"].GetDataValue()).value)
                    {
                        continue;
                    }

                    // Don't look for collisions if they are owned by the same player
                    if (StateDB.physicalObjects[id1].ownerID == StateDB.physicalObjects[id2].ownerID)
                    {
                        continue;
                    }
                    // Don't look for collisions if they are on the same team
                    if (StateDB.physicalObjects[id1].teamName == StateDB.physicalObjects[id2].teamName)
                    {
                        continue;
                    }
                    //Don't look for collisions if they are not hostile
                    if (StateDB.teams.ContainsKey(StateDB.physicalObjects[id1].teamName) &&
                        StateDB.teams.ContainsKey(StateDB.physicalObjects[id2].teamName))
                    {
                        if (!StateDB.teams[StateDB.physicalObjects[id1].teamName].hostility.Contains(StateDB.teams[StateDB.physicalObjects[id2].teamName].id) &&
                            !StateDB.teams[StateDB.physicalObjects[id2].teamName].hostility.Contains(StateDB.teams[StateDB.physicalObjects[id1].teamName].id))
                        {//only continue if both teams are not hostile to one another
                            continue;
                        }
                    }

                    CapabilityValue.Effect eff = null;

                    // check ranges and add objects to target lists for self defense sim
                    CapabilityValue cap1 = ((CapabilityValue)obProx1["Capability"].GetDataValue());
                    VulnerabilityValue vul1 = ((VulnerabilityValue)obProx1["Vulnerability"].GetDataValue());
                    CapabilityValue cap2 = ((CapabilityValue)obProx2["Capability"].GetDataValue());
                    VulnerabilityValue vul2 = ((VulnerabilityValue)obProx2["Vulnerability"].GetDataValue());
                    //double distance = loc1.ScalerDistanceTo(loc2);

                    
                    eff = SimUtility.FindCapabilityEffect(cap1, vul2);
                    dv = obProx1["TargetsInRange"].GetDataValue();
                    //AD: TODO need a "TargetsInSensorRange"? which can drive the ViewPro loops
                    if (eff != null && distance <= eff.range &&
                        ((StringValue)obProx2["State"].GetDataValue()).value != "Dead")
                    {
                        if (!((StringListValue)dv).strings.Contains(id2))
                        {
                            ((StringListValue)dv).strings.Add(id2);
                        }
                    }
                    else
                    {
                        if (((StringListValue)dv).strings.Contains(id2))
                        {
                            ((StringListValue)dv).strings.Remove(id2);
                        }
                    }
                    obProx1["TargetsInRange"].SetDataValue(dv);

                    eff = SimUtility.FindCapabilityEffect(cap2, vul1);
                    dv = obProx2["TargetsInRange"].GetDataValue();
                    if (eff != null && distance <= eff.range &&
                        ((StringValue)obProx1["State"].GetDataValue()).value != "Dead")
                    {
                        if (!((StringListValue)dv).strings.Contains(id1))
                        {
                            ((StringListValue)dv).strings.Add(id1);
                        }
                    }
                    else
                    {
                        if (((StringListValue)dv).strings.Contains(id1))
                        {
                            ((StringListValue)dv).strings.Remove(id1);
                        }
                    }
                    obProx2["TargetsInRange"].SetDataValue(dv);


                    // Don't look for collisions if they are dead

                    if (((StringValue)obProx1["State"].GetDataValue()).value == "Dead")
                    {
                        continue;
                    }
                    if (((StringValue)obProx2["State"].GetDataValue()).value == "Dead")
                    {
                        continue;
                    }


                    // Don't look for collisions if they are too small to collide

                    dv = obProx1["Size"].GetDataValue();
                    objectSize1 = ((DoubleValue)dv).value;
                    if (objectSize1 <= 0.000001)
                    {
                        continue;
                    }
                    dv = obProx2["Size"].GetDataValue();
                    objectSize2 = ((DoubleValue)dv).value;
                    if (objectSize2 <= 0.000001)
                    {
                        continue;
                    }

                    dv = obProx1["Velocity"].GetDataValue();
                    vel1.Set((VelocityValue)dv);
                    dv = obProx2["Velocity"].GetDataValue();
                    vel2.Set((VelocityValue)dv);
                    if (vel1.X == 0 && vel2.X == 0 && vel1.Y == 0 && vel2.Y == 0 && vel1.Z == 0 && vel2.Z == 0)
                        continue;

                    next1 = loc1.Add(vel1.Multiply(dTime));
                    next2 = loc2.Add(vel2.Multiply(dTime));

                    if (next1.ScalerDistanceTo(next2) < (objectSize1 + objectSize2))
                    {
                        collision = SimulationEventFactory.BuildEvent(ref simModel, "ObjectCollision");
                        ((StringValue)collision["ObjectID1"]).value = id1;
                        ((StringValue)collision["ObjectID2"]).value = id2;
                        ((IntegerValue)collision["Time"]).value = time;
                        distClient.PutEvent(collision);
                    }
                }
            }

            /*foreach (string id1 in objectProxies.Keys)
            {
                foreach (string id2 in objectProxies.Keys)
                {
                    if (id1 == id2)
                    {
                        continue;
                    }
                    obProx1 = objectProxies[id1];
                    obProx2 = objectProxies[id1];

                    dv = obProx1["Location"].GetDataValue();
                    loc1.Set((LocationValue)dv);
                    dv = obProx2["Location"].GetDataValue();
                    loc2.Set((LocationValue)dv);

                }

            }*/
        }
        public string GetSimulatorName()
        {
            return "CollisionDetection";
        }
    }

}
