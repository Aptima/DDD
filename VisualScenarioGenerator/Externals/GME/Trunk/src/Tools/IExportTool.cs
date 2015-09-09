using System;
using System.Collections.Generic;
using System.Text;
using AME.Adapters;

namespace AME.Tools
{
    /// <summary>
    /// Interface for classes responsible for exporting data
    /// </summary>
    ///    
    public interface IExportTool<IntermediateType>
    {
        void Export(IExportDataAdapter<IntermediateType> dataAdapter,
                    IExportDeviceAdapter<IntermediateType> deviceAdapter);

    }

}
