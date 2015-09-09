using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace PME_DDD_CPE_Publisher
{
    public class CPEPublisher
    {
        private static NetworkClient _client;
        public static void PublishCPE(String dmId, double findFixCpe, double trackTargetCpe)
        {
            SimulationEvent cpe = new SimulationEvent();
            cpe.eventType = "SEAMATE_UpdateCPE";
            cpe.parameters.Add("DM_ID", DataValueFactory.BuildString(dmId));
            cpe.parameters.Add("FF_CPE", DataValueFactory.BuildDouble(findFixCpe));
            cpe.parameters.Add("TT_CPE", DataValueFactory.BuildDouble(trackTargetCpe));

            if (_client == null)
                throw new Exception("DDD Network Client not connected; You need to call Connect successfully before sending events.");
            _client.PutEvent(cpe);
        }
        public static bool Connect(String hostname, int port)
        {
            if (_client == null)
                _client = new NetworkClient();
            if (_client.IsConnected())
                _client.Disconnect();

            return _client.Connect(hostname, port);
        }
        public static void Disconnect()
        {
            if (_client == null)
                return;
            if (_client.IsConnected())
                _client.Disconnect();
        }
    }
}
