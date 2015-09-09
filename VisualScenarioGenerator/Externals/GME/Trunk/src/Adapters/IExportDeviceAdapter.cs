using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AME.Adapters
{
    /// <summary>
    /// Generic Interface for classes responsible for storing exported data in a specific device
    /// </summary>
    /// <typeparam name="WriteType">Object type that the Process Method returns</typeparam>
    public interface IExportDeviceAdapter<WriteType>
    {
        //IExportDeviceAdapter<WriteType> Create(IExportDeviceAdapterPO ParamObject);

        void InitializeDevice();

        void Write(WriteType data);

        void FinalizeDevice();
    }

    public interface IExportDeviceSettings
    {
    
    }
}
