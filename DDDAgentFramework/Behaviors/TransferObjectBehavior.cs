using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{

    public class TransferObjectBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_newOwner;

        private DMView m_dmView;
        private DDDServerConnection m_dddServer;


        private Boolean m_done = false;




        public TransferObjectBehavior(String thisID, String owner)
        {
            m_newOwner = owner;
            m_thisID = thisID;
        }



        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;
            m_dddServer = serverConnection;

            SimObject ob = dmView.AllObjects[m_thisID];

            m_dddServer.SendTransferObjectRequest(ob.ID, ob.Owner, m_newOwner, ob.State);
            
            m_done = true;
        }



        public void Update(DDDServerConnection serverConnection, DMView dmView)
        {
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
            return "StateChangeBehavior";
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
