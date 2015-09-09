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

namespace AME.Controllers
{
    public interface IModelingController
    {
        Dictionary<String, String> GetInputNames(Int32 componentId, Boolean selectable);
		/// <summary>
		/// Runs input, tool, and output adapters and creates all IEngineModel xml documents
		/// </summary>
		/// <param name="selectedId"></param>
		/// <param name="selectedInputIds"></param>
		/// <returns>Database IDs of Runs created</returns>
        Int32[] Run(Int32 selectedId, List<Int32> selectedInputIds);//, String simRunName);
    } // IModelingController
} // AME.Controllers namespace
