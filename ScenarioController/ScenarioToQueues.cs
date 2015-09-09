using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
 
using System.Xml.Schema;
namespace DDD.ScenarioController
{
    /// <summary>
    /// The structures used to track the events in the scenario.
    /// </summary>
    /// 
  //------------------------- Happening List -----------
    public class HappeningList
    {
        private static List<ScenarioEventType> happenings = new List<ScenarioEventType>();
        public static List<ScenarioEventType> Happenings
        {
            get
            { return happenings; }
        }
        public static void Add(ScenarioEventType happeningEvent)
        {
            happenings.Add(happeningEvent);
        }
    
    }

    /// <remarks>
    /// The list theseUnits structure -- one for each time -- maintains a list of the units affected
    /// at this time period, but does not point to their events
    /// </remarks>
    /// 
   
    // ------------------------ Timer Queue ------------------/
        // -------------------- TimeNodeClass ------------
    public class TimeNodeClass
    {
        private int triggerTime;
        public int TriggerTime
        {
            get
            { return this.triggerTime; }
        }

        private Dictionary<string, int> theseUnits;
        public Dictionary<string, int> TheseUnits
        {
            get
            { return theseUnits; }
        }
        private List<ScenarioEventType> theseEvents;
        public List<ScenarioEventType> TheseEvents
        {
            get
            { return theseEvents; }
        }
        public TimeNodeClass(int triggerTime) // like all times, this is an absolute time
        {
            this.triggerTime = triggerTime;
            theseUnits = new Dictionary<string, int>();
            // each unit is mapped to the number of times it appears in the queue
            theseEvents = new List<ScenarioEventType>();
            /* ?? to allow explicit dispose           GC.SuppressFinalize(this); */
        }

        public void AddEvent(ScenarioEventType e)
        {
            theseEvents.Add(e);
            if (e is ScenarioEventType)
            {
                if (!(theseUnits.ContainsKey(e.UnitID)))
                {
                    theseUnits.Add(e.UnitID, 1);
                }
                else
                {
                    theseUnits[e.UnitID] += 1;
                }
            }
        }
        public void DropEvent(int index)
        {
            ScenarioEventType e = theseEvents[(int)index];

                theseUnits[e.UnitID] -= 1;
                if (theseUnits[e.UnitID] <= 0)
                {
                    theseUnits.Remove(e.UnitID);
                }
                theseEvents.RemoveAt((int)index);

        }

    }
    public class TimerQueueClass
    {
        private static Dictionary<int, TimeNodeClass> queue = new Dictionary<int,TimeNodeClass>();
        public static void Add(int time, ScenarioEventType newEvent)
        {
            if (!queue.ContainsKey(time))
            {
                queue.Add(time, new TimeNodeClass(time));
            }
             queue[time].AddEvent(newEvent);           
        }
        public static List<ScenarioEventType> RetrieveEvents(int t)
        {
            if (queue.ContainsKey(t))
            {
                return queue[t].TheseEvents;
            }
            return null;
        }
        public static void DropEvent(int t,int p)
        {
            queue[t].DropEvent(p);
        }
    }
    /// <summary>
    /// This parses a scenario according to the scenario schema
    /// and pushes Events onto the relevent queues
    /// </summary>
    public class ScenarioToQueues
    {

        private static bool allDoneReading = false;
        private Parser parser;
        private static XmlTextReader reader;
        public static Dictionary<string, string> unitDirectory = new Dictionary<string,string>();
        // this is a standin for a greater reposityo of unit data
        
        private string xmlNextNode()

        {
            if (allDoneReading)
            {
                return null;
            }
            return reader.Name;
        }

        static void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            string envelope = "Error discorvered in scenario validationis:" + e.Message;
            throw new ApplicationException(envelope);
        }

        private static Boolean ValidateSchema(string xmlPath)
        {
            Boolean returnValue = false;
            Boolean fileOpen = false;
            Boolean readerOpen = false;
            // see Espositio, Applied XML programming,p 81
            FileStream fs;
            try
            {
                fs = File.Open(xmlPath, FileMode.Open);
            }
            catch (SystemException f)
            {
                ApplicationException e = new ApplicationException("Could not open scenario file.");
                throw e;

            }
            fileOpen = true;
            XmlTextReader _coreReader = new XmlTextReader(fs);
            XmlValidatingReader reader = new XmlValidatingReader(_coreReader);

            // Prepare for validation 
            reader.ValidationType = ValidationType.Schema;
            reader.ValidationEventHandler += new ValidationEventHandler(SchemaValidationHandler);
            //           XmlSchemaCollection schemaCollection = new XmlSchemaCollection();
            //           schemaCollection.Add

            reader.Schemas.Add(null, "genSchema.xsd");
            readerOpen = true;


            try
            {
                while (reader.Read()) { };
                returnValue = true;
            }
            catch (SystemException e)
            {
                Console.WriteLine("Failed to validate scenario against schema.");

                if (null != e.Message)
                {
                    Console.WriteLine("Validation error: " + e.Message);
                }
                else
                {
                    Console.WriteLine("No other information os available. Sorry");
                }

            }
            finally
            {
                if (readerOpen)
                {
                    reader.Close();
                }
                if (fileOpen)
                {
                    fs.Close();
                }
            }

            return returnValue;


        }

        public ScenarioToQueues(string scenarioFile)
        {
            TimerQueueClass timerQueue = new TimerQueueClass();
            // Extract fields from the XML file (and schema)
            // see http://weblogs.asp.net/dbrowning/articles/114561.aspx  
            // paths to xml/xsd
           // const string path = @"C:\Documents and Settings\dgeller\My Documents\Visual Studio 2005\Projects\";
            //const string scenarioFile = path + @"DDD\Scenario.xml";
           // const string xsdPath = path + @"XMLTrial\XMLTrial\ScenarioSchema.xsd";




            FileStream fs;
            Boolean returnVal;


            try
            {
                returnVal = ValidateSchema(scenarioFile);
            }
            catch (ApplicationException e)
            {
                Console.WriteLine("Could not validate scenario file " + scenarioFile + " against schema");
                if (e.Message != null)
                {
                    Console.WriteLine(e.Message);
                }
                else
                {
                    Console.WriteLine("No further information available.  Sorry.");
                }
                return;
            }

            if (!returnVal)
            {
                return;
            }


            try
            {
                fs = File.Open(scenarioFile, FileMode.Open);
            }
            catch
            {
                ApplicationException e = new ApplicationException("Could not open scenario file.");
                throw e;

            }
 

            reader = new XmlTextReader(fs);
            parser = new Parser(reader);

            reader.Read(); // opens, gets to xml declaration
            reader.Read();
            if ("Scenario" == reader.Name) // only Scenario can be top level
            {
                reader.Read(); // pass by "Scenario"
                while (!reader.EOF  && !((XmlNodeType.EndElement==reader.NodeType)&&("Scenario"==reader.Name)))
                {

                    Console.WriteLine(".");

                    switch (reader.NodeType)
                    {

                        case XmlNodeType.Element:
                            Console.WriteLine("ELEMENT <{0}>", reader.Name);
                            switch(reader.Name)
                            {
                                case "Create_Event":
                           Create_EventType createEvent = parser.GetCreate_EventType();
                           TimerQueueClass.Add(createEvent.Timer, createEvent);
                                    Console.WriteLine(" createEvent ");
                                break;
                                case "Move_Event":
                                Move_EventType moveEvent = parser.GetMove_EventType();
                                TimerQueueClass.Add(moveEvent.Timer, moveEvent);

                                    Console.WriteLine(" moveEvent ");
                                break;
                                case "Completion_Event":
                                    HappeningCompletionType completionType = parser.GetHappeningCompletionType();
                                    HappeningList.Add(completionType);
                                    break;

                                default:
                                Console.WriteLine(" Unknown Element is *{0}* ", reader.Name);


                                break;
                            }

                            break;
                        case XmlNodeType.Text:
                            Console.WriteLine("TEXT <{0}>", reader.Value);
                            break;
                        case XmlNodeType.CDATA:
                            Console.WriteLine("CDATA <![CDATA[{0}]]>", reader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            Console.WriteLine("PROC INS<?{0} {1}?>", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            Console.WriteLine("COMMENT <!--{0}-->", reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Console.WriteLine("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            Console.WriteLine("DOCTYPE <!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            Console.WriteLine(reader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            Console.WriteLine("END ELEMENT </{0}>", reader.Name);
                            break;
                    }

                }

            }

            Console.WriteLine("Done\n");








        }
        private static void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {

            // Report back error information to the console...

            Console.WriteLine("Bad stuff happened");

            Console.WriteLine(e.Exception.Message);

            Console.WriteLine(e.Exception.LineNumber);



        }
    }
}