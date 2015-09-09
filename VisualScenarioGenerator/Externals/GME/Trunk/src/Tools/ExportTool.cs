using System;
using System.Collections.Generic;
using System.Text;
using AME.Adapters;
using System.IO;
using System.Reflection;

namespace AME.Tools
{
    public class ExportTool<T> : IExportTool<T>
    {
        private IExportDataAdapter<T> dataAdapter;
        private IExportDeviceAdapter<T> deviceAdapter;
        #region IDataRunExportTool Members

        /// <summary>
        /// Exports the Data held in the DataAdapter and writes it to the DeviceAdapter.
        /// </summary>
        /// <param name="inDataAdapter"></param>
        /// <param name="inDeviceAdapter"></param>
        public void Export(IExportDataAdapter<T> inDataAdapter, IExportDeviceAdapter<T> inDeviceAdapter)
        {
            this.dataAdapter = inDataAdapter;
            this.deviceAdapter = inDeviceAdapter;

            CheckAdapterGenericArguments();

            this.deviceAdapter.InitializeDevice();

            this.deviceAdapter.Write(this.dataAdapter.Process());

            this.deviceAdapter.FinalizeDevice();
        }

        #endregion

        /// <summary>
        /// Checks to ensure that the Device Adapter and the Data Adapter have compatible Generic Arguments.
        /// </summary>
        private void CheckAdapterGenericArguments()
        { 
            Type dataAdapterType = dataAdapter.GetType();
            Type dataAdapterInterface = dataAdapterType.GetInterface("IExportDataAdapter`1");
            Type[] dataAdapterGenArgs = dataAdapterInterface.GetGenericArguments();

            Type deviceAdapterType = deviceAdapter.GetType();
            Type deviceAdapterInterface = deviceAdapterType.GetInterface("IExportDeviceAdapter`1");
            Type[] deviceIntGenArgs = deviceAdapterInterface.GetGenericArguments();

            Type thisType = this.GetType();
            Type thisInterface = thisType.GetInterface("IExportTool`1");
            Type[] thisIntGenArgs = thisInterface.GetGenericArguments();

            if ( (dataAdapterGenArgs[0].Name != deviceIntGenArgs[0].Name) ||
                 (dataAdapterGenArgs[0].Name != thisIntGenArgs[0].Name) )
            {
                throw new Exception("DataAdapter and Device Adapter must have the same Generic Arguments.");
            }
            
        }

    }
}
