using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public interface PlayerAgentInterface
    {
        void Init(ref DDDServerConnection ddd);
        void ViewProInitializeObject(SimulationEvent ev);
        void ViewProAttributeUpdate(SimulationEvent ev);
        void ViewProMotionUpdate(SimulationEvent ev);
        void ViewProAttackUpdate(SimulationEvent ev);
        void ViewProStopObjectUpdate(SimulationEvent ev);
        void ViewProActiveRegionUpdate(SimulationEvent ev);
        void AttackSucceeded(SimulationEvent ev);
        void StateChange(SimulationEvent ev);
        void Update();
    }
}
