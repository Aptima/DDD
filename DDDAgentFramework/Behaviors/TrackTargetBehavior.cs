using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class TrackTargetBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_targetID;
        //private LocationValue m_referenceLocation;
        private WaypointSequence m_relativeLoiterPattern;
        private WaypointSequence m_absoluteLoiterPattern;
        private DMView m_dmView;
        private Waypoint m_destWaypoint;

        private bool m_trackTarget;


        private Boolean m_done = false;




        public TrackTargetBehavior(String thisID, String targetID, WaypointSequence relativeLoiterPattern)
        {
            m_thisID = thisID;
            m_targetID = targetID;
            m_relativeLoiterPattern = relativeLoiterPattern;
            m_absoluteLoiterPattern = null;
            m_destWaypoint = null;
            m_trackTarget = false;

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

            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;


            m_trackTarget = ShouldTrack(serverConnection,dmView);


            if (m_trackTarget)
            {
                serverConnection.SendMoveObjectRequest(m_thisID, trackLocation, 1);
            }
            else
            {
                StartLoiter(serverConnection,dmView);
            }
            

        }



        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            if (m_done)
            {
                return;
            }
            //SimObject me = dmView.AllObjects[m_thisID];
            //LocationValue myLocation = me.Location;
            SimObject track = dmView.AllObjects[m_targetID];
            LocationValue trackLocation = track.Location;
            m_absoluteLoiterPattern = m_relativeLoiterPattern.ToAbsolute(trackLocation);


            if (ShouldTrack(serverConnection,dmView))
            {

                serverConnection.SendMoveObjectRequest(m_thisID, trackLocation, 1);

                m_trackTarget = true;

            }
            else
            {
                if (m_trackTarget)
                {
                    StartLoiter(serverConnection,dmView);
                    m_trackTarget = false;
                }
                ContinueLoiter(serverConnection, dmView);
            }

            




        }

        public bool IsDone(DDDServerConnection serverConnection, DMView dmView)
        {
            return m_done;
        }

        LocationValue CalculateIntercept(String myID, String targetID)
        {
            LocationValue interceptLoc = new LocationValue();

            LocationValue myLoc = m_dmView.AllObjects[myID].Location;
            LocationValue targetLoc = m_dmView.AllObjects[targetID].Location;

            //VelocityValue targetVel = m_dmView.AllObjects[targetID].Velocity;
            //Double myMaxSpeed = m_dmView.AllObjects[myID].MaximumSpeed;


            interceptLoc = targetLoc;


            return interceptLoc;
        }
        public void ViewProInitializeObject(SimulationEvent ev) { }
        public void ViewProAttributeUpdate(SimulationEvent ev) { }
        public void ViewProMotionUpdate(SimulationEvent ev) { }
        public void ViewProAttackUpdate(SimulationEvent ev) { }
        public void ViewProStopObjectUpdate(SimulationEvent ev) { }
        public void ViewProActiveRegionUpdate(SimulationEvent ev) { }
        public void AttackSucceeded(SimulationEvent ev) { }
        public String GetName()
        {
            return "TrackTargetBehavior";
        }
        public String GetState()
        {
            string state = "";
            if (m_trackTarget)
            {
                state =  String.Format("Tracking:{0}", m_targetID);
            }
            else
            {
                state = String.Format("Loitering:{0}", m_targetID);
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
