using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class TaxiBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_destID;

        private bool m_done = false;
        public TaxiBehavior(String thisID, String destID)
        {
            m_thisID = thisID;
            m_destID = destID;
        }

        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            SimObject dest = dmView.AllObjects[m_destID];

            LocationValue myLocation = me.Location;
            LocationValue destLocation = dest.Location;

            if (ObjectMath.IsWithinRange(0.1, myLocation, destLocation))
            {
                m_done = true;
                return;
            }

            serverConnection.SendMoveObjectRequest(m_thisID, destLocation, 1);


        }

        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            if (m_done)
            {
                return;
            }
            SimObject me = dmView.AllObjects[m_thisID];
            SimObject dest = dmView.AllObjects[m_destID];

            LocationValue myLocation = me.Location;
            LocationValue destLocation = dest.Location;
            VelocityValue myVelocity = me.Velocity;


            if (ObjectMath.IsWithinRange(0.1, myLocation, destLocation))
            {
                m_done = true;
                return;
            }

            if (myVelocity.VX == 0 && myVelocity.VY == 0 && myVelocity.VZ == 0)
            {
                serverConnection.SendMoveObjectRequest(m_thisID, destLocation, 1);
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
            return "TaxiBehavior";
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
