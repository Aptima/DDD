using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{

    public class DelayBehavior : BehaviorInterface
    {
        private String m_thisID;
        private int m_delayMS;

        private DMView m_dmView;
        private DDDServerConnection m_dddServer;


        private Boolean m_done = false;

        private DateTime m_endTime;


        public DelayBehavior(String thisID, int delayMS)
        {

            m_thisID = thisID;
            m_delayMS = delayMS;
        }



        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;
            m_dddServer = serverConnection;
            
            m_done = false;
            m_endTime = DateTime.Now + new TimeSpan(0, 0, 0, 0, m_delayMS);
        }



        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
            if (DateTime.Now >= m_endTime)
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
        public void ViewProAttackUpdate(SimulationEvent ev)
        {
            
        }
        public void ViewProStopObjectUpdate(SimulationEvent ev) { }
        public void ViewProActiveRegionUpdate(SimulationEvent ev) { }
        public void AttackSucceeded(SimulationEvent ev)
        {
        }

        public String GetName()
        {
            return "DelayBehavior";
        }
        public String GetState()
        {
            TimeSpan delay = m_endTime - DateTime.Now;

            return String.Format("DELAY:{0:0.##} sec", delay.TotalSeconds);
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
