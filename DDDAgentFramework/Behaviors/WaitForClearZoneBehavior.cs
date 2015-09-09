using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class WaitForClearZoneBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_zoneID;

        private bool m_done = false;
        public WaitForClearZoneBehavior(String thisID, String zoneID)
        {
            m_thisID = thisID;
            m_zoneID = zoneID;
        }

        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {

        }

        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            SimObject me = dmView.AllObjects[m_thisID];
            SimActiveRegion zone = dmView.ActiveRegions[m_zoneID];

            LocationValue otherLocation = null;
            Polygon2D azPoly = new Polygon2D();
            foreach (PolygonValue.PolygonPoint p in zone.Shape.points)
            {
                azPoly.AddVertex(new Vec2D(p.X, p.Y));
            }
            bool clear = true;

            foreach (String id in dmView.AllObjects.Keys)
            {
                if (id == m_thisID)
                {
                    continue;
                }
                otherLocation = dmView.AllObjects[id].Location;
                if (Polygon2D.IsPointInside(azPoly, new Vec2D(otherLocation)))
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
            return "WaitForClearZoneBehavior";
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
