using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    
    public class ObjectControlAgent
    {

        
        private List<BehaviorInterface> m_behaviorQueue;
        public List<BehaviorInterface> BehaviorQueue
        {
            get { return m_behaviorQueue; }
        }
        private BehaviorInterface m_currentBehavior;
        public BehaviorInterface CurrentBehavior
        {
            get { return m_currentBehavior; }
        }
        public SimObject simObject = null;

        public ObjectControlAgent()
        {
            m_behaviorQueue = new List<BehaviorInterface>();
        }
        public void AddBehavior(BehaviorInterface b)
        {
            m_behaviorQueue.Add(b);
        }

        public void ClearBehaviorQueue()
        {
            m_behaviorQueue.Clear();
        }

        public void Update(DDDServerConnection serverConnection,DMView dmView)
        {
            if (m_behaviorQueue.Count >= 1)
            {
                //Console.Out.WriteLine(String.Format("ObjectControlAgent.Update() {0}",simObject.ID));
                if (m_currentBehavior != m_behaviorQueue[0])
                {
                    
                    m_currentBehavior = m_behaviorQueue[0];
                    //Console.Out.WriteLine(String.Format("ObjectControlAgent.Update() starting {0} for {1}",m_currentBehavior.GetName(), simObject.ID));
                    m_currentBehavior.Start(serverConnection,dmView);
                }
                m_currentBehavior.Update(serverConnection, dmView);
                if (m_currentBehavior.IsDone(serverConnection, dmView))
                {
                    //Console.Out.WriteLine(String.Format("ObjectControlAgent.Update(){0} is done for {1}", m_currentBehavior.GetName(), simObject.ID));
                    m_behaviorQueue.Remove(m_currentBehavior);
                    m_currentBehavior = null;
                }

            }
            else
            {
                m_currentBehavior = null;
            }



        }
        public void ViewProInitializeObject(SimulationEvent ev) { }
        public void ViewProAttributeUpdate(SimulationEvent ev) { }
        public void ViewProMotionUpdate(SimulationEvent ev) { }
        public void ViewProAttackUpdate(SimulationEvent ev)
        {
            if (m_currentBehavior != null)
            {
                m_currentBehavior.ViewProAttackUpdate(ev);
            }
        }
        public void ViewProStopObjectUpdate(SimulationEvent ev) { }
        public void ViewProActiveRegionUpdate(SimulationEvent ev) { }
        public void AttackSucceeded(SimulationEvent ev)
        {
            if (m_currentBehavior != null)
            {
                m_currentBehavior.AttackSucceeded(ev);
            }
        }
    }
}
