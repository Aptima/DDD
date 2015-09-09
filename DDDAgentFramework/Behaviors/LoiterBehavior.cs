using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class LoiterBehavior : BehaviorInterface
    {
        private String m_thisID;
        //private String m_destID;
        private LocationValue m_referenceLocation;
        private WaypointSequence m_relativeLoiterPattern;
        private WaypointSequence m_absoluteLoiterPattern;
        private DMView m_dmView;
        private Waypoint m_destWaypoint;


        private Boolean m_done = false;


        //public LoiterBehavior(String thisID, String destID, WaypointSequence loiterPattern)
        //{
        //    m_thisID = thisID;
        //    m_destID = destID;
        //    m_destX = 0;
        //    m_destY = 0;
        //    m_loiterPattern = loiterPattern;
        //}

        public LoiterBehavior(String thisID, LocationValue refLoc, WaypointSequence relativeLoiterPattern)
        {
            m_thisID = thisID;
            //m_destID = String.Empty;
            m_referenceLocation = refLoc;
            m_relativeLoiterPattern = relativeLoiterPattern;
            m_absoluteLoiterPattern = relativeLoiterPattern.ToAbsolute(refLoc);
            m_destWaypoint = null;

        }
        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;

            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;
            m_destWaypoint = m_absoluteLoiterPattern.GetWaypointClosestTo(myLocation);
            serverConnection.SendMoveObjectRequest(m_thisID, m_destWaypoint.Location, 1);

        }

    

        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            if (m_done)
            {
                return;
            }
            SimObject me = dmView.AllObjects[m_thisID];
            LocationValue myLocation = me.Location;


            if (BehaviorHelper.LocationIsEqual(myLocation, m_destWaypoint.Location))
            {
                m_destWaypoint = m_absoluteLoiterPattern.GetNextWaypoint();
                serverConnection.SendMoveObjectRequest(m_thisID, m_destWaypoint.Location, 1);
            }

        


        }

        public bool IsDone(DDDServerConnection serverConnection, DMView dmView)
        {
            return m_done;
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
            return "LoiterBehavior";
        }
        public String GetState()
        {
            return String.Empty;
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
