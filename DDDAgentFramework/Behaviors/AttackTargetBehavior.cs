using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class AttackTargetBehavior : BehaviorInterface
    {
        private String m_thisID;
        private String m_targetID;
        private DMView m_dmView;
        private DDDServerConnection m_dddServer;

        private Boolean m_attackInProcess;
        private Boolean m_attackIsWeapon;
        private String m_attackWeaponID;
        private int m_attackEndTime;


        private Boolean m_done = false;




        public AttackTargetBehavior(String thisID, String targetID)
        {
            m_thisID = thisID;
            m_targetID = targetID;
            m_attackEndTime = 0;
            m_attackInProcess = false;
            m_attackIsWeapon = false;
        }







        public void Start(DDDServerConnection serverConnection, DMView dmView)
        {
            m_dmView = dmView;
            m_dddServer = serverConnection;
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
                        m_done = true;
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
            m_done = true;

        }

        public String GetName()
        {
            return "AttackTargetBehavior";
        }
        public String GetState()
        {
            return String.Format("Attacking:{0}", m_targetID);
            
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
