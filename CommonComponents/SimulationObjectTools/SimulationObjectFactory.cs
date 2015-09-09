using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.SimulationObjectTools
{
    public class SimulationObjectFactory
    {
        static public SimulationObject BuildObject(ref SimulationModelInfo model, string objectType)
        {
            SimulationObject o = new SimulationObject();
            
            if (!model.objectModel.objects.ContainsKey(objectType))
            {
                throw new Exception("Object type doesn't exist");
            }

            foreach (AttributeInfo aInfo in model.objectModel.objects[objectType].attributes.Values)
            {
                o[aInfo.name] = DataValueFactory.BuildValue(aInfo.dataType);
            }
            o.objectType = objectType;

            return o;
        }
    }
}
