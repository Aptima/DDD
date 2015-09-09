using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.Simulators.Scoring
{
   

    public class ScoringSim : ISimulator
    {
        private Blackboard blackboard;
        private BlackboardClient bbClient;
        private SimulationEventDistributor distributor;
        private SimulationEventDistributorClient distClient;
        private SimulationModelInfo simModel;

        private int time;

        private Dictionary<string, SimulationObjectProxy> objectProxies;


        private Dictionary<string, double> lastScore;

        public ScoringSim()
        {
            time = 0;
            blackboard = null;
            bbClient = null;
            distributor = null;
            distClient = null;
            simModel = null;
            objectProxies = null;

            lastScore = new Dictionary<string, double>();
            //regions = new Dictionary<string, BlockingRegion>();
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
            bbClient.Subscribe("PhysicalObject", "State", true, false);
            bbClient.Subscribe("PhysicalObject", "OwnerID", true, false);
            bbClient.Subscribe("PhysicalObject", "Location", true, false);
            bbClient.Subscribe("PhysicalObject", "DockedToParent", true, false);

            ScoringDB.Score s = new ScoringDB.Score("DEFENSE", null, null, 1000);
            s.calculateDMs = new List<string>();
            s.calculateDMs.Add("BluePlayer01");
            s.calculateDMs.Add("BluePlayer02");
            s.displayDMs = s.calculateDMs;

            
            s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.HostileDM),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  new ScoringDB.ConditionInfo(ScoringDB.ConditionInfo.ConditionType.ObjectExists,"",""),
                                                  null,
                                                  -1));
            /*
            s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.Myself),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  new ScoringDB.ConditionInfo(ScoringDB.ConditionInfo.ConditionType.StateChange, "Dead"),
                                                  new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.HostileDM),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  200));
            s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.Myself),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  new ScoringDB.ConditionInfo(ScoringDB.ConditionInfo.ConditionType.StateChange, "PartlyFunctional"),
                                                  new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.FriendlyDM),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  -300));
            s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.HostileDM),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  new ScoringDB.ConditionInfo(ScoringDB.ConditionInfo.ConditionType.StateChange, "Dead"),
                                                  new ScoringDB.ActorInfo(new ScoringDB.ActorInfo.OwnerInfo(ScoringDB.ActorInfo.OwnerInfo.OwnerType.Myself),
                                                                          new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, ""),
                                                                          new ScoringDB.ActorInfo.LocationInfo(ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere, "")),
                                                  -1000));
            */

            //ScoringDB.scores["DEFENSE"] = s;

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
                return;

        }

        private void ResetSimulation()
        {
            objectProxies = bbClient.GetObjectProxies();
            lastScore.Clear();

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
        private void RevealObject(SimulationEvent e)
        {

            
        }
        
        private void MoveObject(SimulationEvent e)
        {
           

        }
        
        private void SubplatformDock(SimulationEvent e)
        {
        }
        private void ObjectCollision(SimulationEvent e)
        {
        }

        private void TimeTick(SimulationEvent e)
        {
            int oldTime = time;
            DataValue dv = null;

            dv = e["Time"];
            time = ((IntegerValue)dv).value;

            if (time % 1000 != 0)
            {
                return;
            }



            foreach (string s in ScoringDB.scores.Keys)
            {
                if (lastScore.ContainsKey(s))
                {
                    if (ScoringDB.scores[s].scoreValue != lastScore[s])
                    {
                        lastScore[s] = ScoringDB.scores[s].scoreValue;
                        foreach (string dm in ScoringDB.scores[s].displayDMs)
                        {
                            distClient.PutEvent(SimUtility.BuildScoreUpdateEvent(ref simModel, time, dm, s, ScoringDB.scores[s].scoreValue));
                        }
                    }
                }
                else
                {
                    lastScore[s] = ScoringDB.scores[s].scoreValue;
                    foreach (string dm in ScoringDB.scores[s].displayDMs)
                    {
                        distClient.PutEvent(SimUtility.BuildScoreUpdateEvent(ref simModel, time, dm, s, ScoringDB.scores[s].scoreValue));
                    }
                }
            }

        }
        private void StateChange(SimulationEvent e)
        {


        }
        public string GetSimulatorName()
        {
            return "Scoring";
        }
    }
    
}
