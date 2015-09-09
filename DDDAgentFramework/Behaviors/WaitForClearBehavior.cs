using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{

    public class WaitForClearBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_destID;

        private bool m_done = false;
        public WaitForClearBehavior(String thisID, String destID)
        {
            m_thisID = thisID;
            m_destID = destID;
        }

        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {

        }

        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            SimObject dest = dmView.AllObjects[m_destID];

            LocationValue clearLoc = dest.Location;
            LocationValue otherLocation = null;

            bool clear = true;

            foreach (String id in dmView.AllObjects.Keys)
            {
                if (id == m_destID || id == m_thisID)
                {
                    continue;
                }
                otherLocation = dmView.AllObjects[id].Location;
                if (ObjectMath.IsWithinRange(100, clearLoc, otherLocation))
                {
                    clear = false;
                    break;
                }
            }

            if (clear)
            {
                m_done = true;
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
            return "WaitForClearBehavior";
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
