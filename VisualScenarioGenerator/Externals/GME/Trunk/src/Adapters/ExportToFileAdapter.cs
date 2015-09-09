using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AME.Adapters
{

    public class ExportToFileAdapter : IExportDeviceAdapter<String>
    {
        
        ExportToFileDeviceSettings settings;
        StreamWriter streamWriter;
        
        public ExportToFileAdapter(ExportToFileDeviceSettings settings)
        {
            this.settings = settings;
        }

        #region IExportDeviceAdapter<string> Members

        void IExportDeviceAdapter<string>.InitializeDevice()
        {
                streamWriter = new StreamWriter(settings.outputFilePath);
        }

        void IExportDeviceAdapter<string>.Write(string data)
        {
            streamWriter.Write(data);
        }

        void IExportDeviceAdapter<string>.FinalizeDevice()
        {
            streamWriter.Close();   
        }

        #endregion
    }

    public class ExportToFileDeviceSettings : IExportDeviceSettings
    {
        public String outputFilePath;
    }

}
