using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    static public class LogWriter
    {
        static Dictionary<String, StreamWriter> m_logs = new Dictionary<string,StreamWriter>();
        static public void Write(String logFileName, int time,String message)
        {
            if (!m_logs.ContainsKey(logFileName))
            {
                m_logs[logFileName] = new StreamWriter(logFileName + ".agentlog");
            }

            m_logs[logFileName].WriteLine(String.Format("{0}:{1}",time,message));
            m_logs[logFileName].Flush();
        }
    }
}
