using System;
using System.Collections.Generic;

namespace AME.Adapters
{
    /// <summary>
    /// Generic Interface for Exporting Data
    /// </summary>
    /// <typeparam name="ReturnType">Object type that the Process Method returns</typeparam>
    public interface IExportDataAdapter<ReturnType>    
    {
         ReturnType Process();
    }

    /// <summary>
    /// Interface for the parameter object 
    /// </summary>
    public interface IExportDataSettings
    {
    
    }
}
