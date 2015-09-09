using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    enum EngagementState
    {
        Tracking, Attacking, Returning
    }
    public class EngageTargetBehavior : BehaviorInterface
    {



        EngagementState m_engagementState;
        private String m_thisID;
        private String m_targetID;
        //private LocationValue m_referenceLocation;
        private WaypointSequence m_relativeLoiterPattern;
        private WaypointSequence m_absoluteLoiterPattern;
        private DMView m_dmView;
        private DDDServerConnection m_dddServer;
        private Waypoint m_destWaypoint;

        private Boolean m_attackInProcess;
        private Boolean m_attackIsWeapon;
        private String m_attackWeaponID;
        private int m_attackEndTime;
        private LocationValue m_originalLocation;

        //private bool m_trackTarget;


        private Boolean m_done = false;

        private Boolean m_returnAfter;


        public EngageTargetBehavior(String thisID, String targetID, WaypointSequence relativeLoiterPattern)
        {
            m_engagementState = EngagementState.Tracking;
            m_thisID = thisID;
            m_targetID = targetID;
            m_relativeLoiterPattern = relativeLoiterPattern;
            m_absoluteLoiterPattern = null;
            m_destWaypoint = null;
            m_attackEndTime = 0;
            m_attackInProcess = false;
            m_attackIsWeapon = false;
            m_originalLocation = null;
            m_returnAfter = true;

        }
        public EngageTargetBehavior(String thisID, String targetID, WaypointSequence relativeLoiterPattern, Boolean returnAfter)
        {
            m_engagementState = EngagementState.Tracking;
            m_thisID = thisID;
            m_targetID = targetID;
            m_relativeLoiterPattern = relativeLoiterPattern;
            m_absoluteLoiterPattern = null;
            m_destWaypoint = null;
            m_attackEndTime = 0;
            m_attackInProcess = false;
            m_attackIsWeapon = false;
            m_originalLocation = null;
            m_returnAfter = returnAfter;
        }

        bool ShouldTrack(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;

            WaypointSequence absoluteLoiterPattern = m_relativeLoiterPattern.ToAbsolute(trackLocation);
            Waypoint destWaypoint = absoluteLoiterPattern.GetWaypointClosestTo(myLocation);

            Double myDis = BehaviorHelper.Distance(myLocation, trackLocation);
            Double destDis = BehaviorHelper.Distance(destWaypoint.Location, trackLocation);

            if (myDis > (destDis * 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void StartLoiter(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;

            m_absoluteLoiterPattern = m_relativeLoiterPattern.ToAbsolute(trackLocation);
            m_destWaypoint = m_absoluteLoiterPattern.GetWaypointClosestTo(myLocation);
            m_relativeLoiterPattern.NextWaypointIndex = m_absoluteLoiterPattern.NextWaypointIndex;
            m_relativeLoiterPattern.CurrentWaypointIndex = m_absoluteLoiterPattern.CurrentWaypointIndex;
            serverConnection.SendMoveObjectRequest(m_thisID, m_destWaypoint.Location, 1);
            
        }

        void ContinueLoiter(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;
            if (BehaviorHelper.LocationIsEqual(myLocation, m_destWaypoint.Location))
            {
                m_destWaypoint = m_absoluteLoiterPattern.GetNextWaypoint();
                m_relativeLoiterPattern.NextWaypointIndex = m_absoluteLoiterPattern.NextWaypointIndex;
                m_relativeLoiterPattern.CurrentWaypointIndex = m_absoluteLoiterPattern.CurrentWaypointIndex;
                serverConnection.SendMoveObjectRequest(m_thisID, m_destWaypoint.Location, 1);
            }

        }

        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;
            m_dddServer = serverConnection;
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;
            m_originalLocation = myLocation;

            //m_trackTarget = ShouldTrack(serverConnection, dmView);

            m_engagementState = EngagementState.Tracking;
            //if (m_trackTarget)
            //{
            //    serverConnection.SendMoveObjectRequest(m_thisID, trackLocation, 1);
            //}
            //else
            //{
            //    StartLoiter(serverConnection, dmView);
            //}


        }



        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            if (m_done)
            {
                return;
            }
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;
            m_absoluteLoiterPattern = m_relativeLoiterPattern.ToAbsolute(trackLocation);


            switch (m_engagementState)
            {
                case EngagementState.Tracking:
                    if (ShouldTrack(serverConnection, dmView))
                    {
                        serverConnection.SendMoveObjectRequest(m_thisID, trackLocation, 1);
                    }
                    else
                    {
                        m_engagementState = EngagementState.Attacking;
                        StartLoiter(serverConnection, dmView);
                    }
                    break;
                case EngagementState.Attacking:
                    ContinueLoiter(serverConnection, dmView);
                    if (!m_attackInProcess) // start the attack
                    {
                        // start with weapons
                        if (me.DockedWeapons.Count > 0)
                        {
                            m_attackWeaponID = me.DockedWeapons[0];
                            serverConnection.SendWeaponLaunchRequest(m_thisID, m_attackWeaponID, m_targetID);
                            m_attackEndTime = dmView.SimTime + 12000; // give a two minute time window to start, AttackUpdate will modify this
                            m_attackInProcess = true;
                            m_attackIsWeapon = true;

                        }
                        else // use native capabilities
                        {
                            // figure out capability/vulnerability match up
                            String cap = DetermineCapability(me.CapabilityList, track.VulnerabilityList);
                            if (cap != String.Empty)
                            {
                                serverConnection.SendAttackObjectRequest(m_thisID, m_targetID, cap);
                                m_attackInProcess = true;
                                m_attackIsWeapon = false;
                                m_attackEndTime = dmView.SimTime + 12000;
                            }
                            else //  I don't have the right capabilities, finish up
                            {
                                ResetAttack();
                                if (m_returnAfter)
                                {
                                    m_engagementState = EngagementState.Returning;
                                    m_dddServer.SendMoveObjectRequest(m_thisID, m_originalLocation, 1);
                                }
                                else
                                {
                                    m_done = true;
                                }
                            }
                        }
                    }
                    else // check to see if the attack was succesful
                    {
                        // if we are still in attack mode 2 seconds after attack was supposed to end
                        // start another attack
                        if (dmView.SimTime > m_attackEndTime + 2000)
                        {
                            ResetAttack();
                        }
                    }
                    break;
                case EngagementState.Returning:
                    if (BehaviorHelper.Distance(myLocation, m_originalLocation) < 1)
                    {
                        m_done = true;
                    }
                    break;
            }
        }

        void ResetAttack()
        {
            m_attackInProcess = false;
            m_attackIsWeapon = false;
            m_attackEndTime = -1;
            m_attackWeaponID = String.Empty;
        }

        String DetermineCapability(List<String> caps, List<String> vulns)
        {
            foreach (String cap in caps)
            {
                if (vulns.Contains(cap))
                {
                    return cap;
                }
            }
            return String.Empty;
        }

        public bool IsDone(DDDServerConnection serverConnection, DMView dmView)
        {
            return m_done;
        }

        public void ViewProInitializeObject(SimulationEvent ev) { }
        public void ViewProAttributeUpdate(SimulationEvent ev) { }
        public void ViewProMotionUpdate(SimulationEvent ev) { }
        public void ViewProAttackUpdate(SimulationEvent ev)
        {
            String attacker = ((StringValue)ev["AttackerID"]).value;
            String target = ((StringValue)ev["TargetID"]).value;
            int endTime = ((IntegerValue)ev["AttackEndTime"]).value;
            if (m_attackInProcess)
            {
                if (target == m_targetID)
                {
                    if ((m_attackIsWeapon && (attacker == m_attackWeaponID)))
                    {
                        m_attackEndTime = endTime;
                    }
                    else if (!m_attackIsWeapon && (attacker == m_thisID))
                    {
                        m_attackEndTime = endTime;
                    }

                }
            }
            
        }
        public void ViewProStopObjectUpdate(SimulationEvent ev) { }
        public void ViewProActiveRegionUpdate(SimulationEvent ev) { }
        public void AttackSucceeded(SimulationEvent ev)
        {
            String attacker = ((StringValue)ev["ObjectID"]).value;
            String target = ((StringValue)ev["TargetID"]).value;
            String newState = ((StringValue)ev["NewState"]).value;
            if (m_attackInProcess)
            {
                if (target == m_targetID)
                {
                    ResetAttack();
                    if (m_returnAfter)
                    {
                        m_engagementState = EngagementState.Returning;
                        m_dddServer.SendMoveObjectRequest(m_thisID, m_originalLocation, 1);
                    }
                    else
                    {
                        m_done = true;
                    }                 
                }
            }
        }

        public String GetName()
        {
            return "EngageTargetBehavior";
        }
        public String GetState()
        {
            string state = "";
            switch (m_engagementState)
            {
                case EngagementState.Tracking:
                    state= String.Format("Tracking:{0}",m_targetID);
                    break;
                case EngagementState.Attacking:
                    state = String.Format("Attacking:{0}", m_targetID);
                    break;
                case EngagementState.Returning:
                    state = "Returning";
                    break;
            }
            return state;
        }
        public BehaviorStatus GetBehaviorStatus()
        {
            BehaviorStatus s = new BehaviorStatus();
            s.Name = GetName();
            s.Status = GetState();
            return s;
        }
    }
}
