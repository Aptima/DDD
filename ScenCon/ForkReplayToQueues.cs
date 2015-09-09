using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
using Aptima.Asim.DDD.CommonComponents.ErrorLogTools;
namespace Aptima.Asim.DDD.ScenarioController
{
    public class ForkReplayToQueues
    {
        SimulationModelInfo m_simModelInfo;
        List<SimulationEvent> LoadForkReplayFile(String filename)
        {
            List<SimulationEvent> events = new List<SimulationEvent>();

            if (filename == null)
            {
                return events;
            }



            StreamReader re = File.OpenText(filename);
            string input = null;
            SimulationEvent ev = null;

            input = re.ReadLine(); // thriow away the line with the version number
            

            while ((input = re.ReadLine()) != null)
            {
                try
                {
                    ev = SimulationEventFactory.XMLDeserialize(input);
                    if (SimulationEventFactory.ValidateEvent(ref m_simModelInfo, ev))
                    {
                        if (m_simModelInfo.eventModel.events[ev.eventType].shouldForkReplay)
                        {
                            events.Add(ev);
                        }
                    }
                    else
                    {
                        throw new Exception("error reading: " + input);
                    }
                }
                catch (Exception e)
                {
                    ErrorLog.Write(String.Format("NONFATAL Deserialize Error in ForkReplayToQueues: {0}", input));
                    ErrorLog.Write(e.ToString());
                }
            }
            re.Close();

            return events;
        }

        public ForkReplayToQueues(String replayName, SimulationModelInfo simModelInfo)
        {
            m_simModelInfo = simModelInfo;

            List<SimulationEvent> events = LoadForkReplayFile(replayName);

            if (events.Count == 0) // this is not a fork replay
            {
                return;
            }

            SimulationEvent start = SimulationEventFactory.BuildEvent(ref simModelInfo, "ForkReplayStarted");
            SimulationEvent finish = SimulationEventFactory.BuildEvent(ref simModelInfo, "ForkReplayFinished");

            TimerQueueClass.SendBeforeStartup(new ForkReplayEventType(start));

            int lastTime = 1;
            foreach (SimulationEvent ev in events)
            {
                ForkReplayEventType fr = new ForkReplayEventType(ev);
                TimerQueueClass.Add(fr.Time, fr);
                lastTime = fr.Time;
            }
            ForkReplayEventType fr2 = new ForkReplayEventType(finish);
            fr2.Time = lastTime + 1;
            TimerQueueClass.Add(fr2.Time, fr2);
        }
    }
}
