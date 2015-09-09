/*
 * Interface        : IDataEntryController
 * File             : IDataEntryController.cs
 * Description      : 
 * Interface for DataEntryController class.
 * It inherits from IController interface.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace GME.Controllers
{
    public interface ISimulationController : IController
    {
        //List<String> GetSimulations();
        List<String> GetInputs(String name, String type);
        //List<String> GetLinkTypes(String component);
        void Run(Int32 simulationId, List<Int32> simRunInputs, String simRunName);
    } //ISimulationController
} // Controllers namespace
