using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.CommonComponents.ServerViewers
{
    public class TextChatStreamViewer
    {
        private string textStream;
        private static object streamLock = new object();

        public TextChatStreamViewer()
        {
            textStream = string.Empty;
        }

        public void AddTextMessage(SimulationEvent e)
        {
            string msg = ((StringValue)e["ChatBody"]).value;
            lock (streamLock)
            {
                textStream = String.Format("{0}\r\n{1}", msg, textStream);
            }
        }
        public string RetrieveTextStream()
        {
            string returnString;
            lock (streamLock)
            {
                returnString = textStream;
            }
            return returnString;
        }
        public void ResetTextStreamViewer()
        {
            textStream = string.Empty;
        }

    }
}
