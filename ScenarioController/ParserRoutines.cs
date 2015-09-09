using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
 
using System.Xml.Schema;
namespace DDD.ScenarioController
{
    public class Parser
    {
      /// all routines for implementing parser.
      /// 
        XmlTextReader reader;
        public Parser(XmlTextReader reader)
        {
            this.reader = reader;
            this.reader.WhitespaceHandling = WhitespaceHandling.None;
               // 'None' only prevents whitespace  being returned as a node
               // Now assuming that there is no embedded whitespace that isn't significant.
        }
        
        public  string GetString()
        {// Entered expecting the current Node to be <some simple element>
            reader.Read();
            string returnString = reader.Value;   
            reader.Read();
            reader.ReadEndElement();
            return returnString;
        }
        public double GetDouble()
      {// Entered expecting the current Node to be <some simple element>
            reader.Read(); //pass by <name of element>
            double returnDouble = Convert.ToDouble((string)reader.Value);   // the x value
            reader.Read();
            reader.ReadEndElement();
            return returnDouble;
           }

        public int GetInt()
        {// Entered expecting the current Node to be <some simple element>
            reader.Read(); //pass by <name of element>
            int returnInt = Convert.ToInt32((string)reader.Value);   // the x value
            reader.Read();
            reader.ReadEndElement();
            return returnInt;
        }


        public VectorType GetVectorType()
        {// Entered expecting the current Node to be something that encloses a VectorType
            // i.e. 'Setting'
            reader.Read(); //go past bracket; pointing to <VectorType>
            reader.Read(); //<go past <VectorType>
            double x = GetDouble();
            double y = GetDouble();
            double z = GetDouble();
            reader.ReadEndElement(); //go psst </VectorType>
            reader.ReadEndElement(); // go past bracket
            return new VectorType(x,y,z);
        }

  
        public ParameterSettingType GetParameterSetting()
        {
            object setting;
            string parameter = GetString();
            switch (parameter)
            {
                case "Position":
                    setting = GetVectorType();
                    break;
                default:
                    setting = GetString();
                    break;


            }
    
            return new ParameterSettingType(parameter, setting);          
        }
        public void GetParameters(ScenarioEventType Event)
        {
            // enter pointing to a <ParameterSetting> or there are no parameters
            if (reader.Name !="ParameterSetting")
            {    
                return;
             }
            reader.Read();// points to first  <Parameter> 
            while ("Parameter" == reader.Name)
            {
                ParameterSettingType nextSetting = GetParameterSetting();
                Event.Add(nextSetting);
            }
            // and move past the </ParameterSettings>
            reader.ReadEndElement();

        }
        public Create_EventType GetCreate_EventType()
        {
            reader.Read();// pass the CreateEvent tag
            string unitID = GetString();
            string unitKind = GetString();
            Create_EventType cEvent = new Create_EventType(unitID);
            cEvent.UnitKind = unitKind;
            GetParameters(cEvent);
            reader.ReadEndElement();
            ScenarioToQueues.unitDirectory.Add(unitID, unitKind);
            return cEvent;
        }
        public Move_EventType GetMove_EventType()
        {
            reader.Read();
            string unitID = GetString();
            int timer= (int)GetInt();
            int throttle = GetInt();
            VectorType location = GetVectorType();
            Move_EventType mEvent = new Move_EventType(unitID, timer);
            mEvent.Location=location;
            mEvent.Throttle=throttle;
            reader.ReadEndElement();
            return mEvent;
        }
        public HappeningCompletionType GetHappeningCompletionType()
        {
            ScenarioEventType eventToPerform;
            reader.Read();
            string unitID = GetString();
            string action = GetString();
            reader.Read(); // bypass the DoThis tag
            switch (reader.Name)
            {
                case "Move_Event":
                    eventToPerform=GetMove_EventType();
                    eventToPerform.Timer = 0; //should have been zero anyway
                    break;
                default:
                    Console.WriteLine("Surpising Happening Complete Event: {0}", reader.Name);
                    eventToPerform = null;
                    break;
            }
            reader.ReadEndElement(); //Pass the </DoThis>
            reader.ReadEndElement(); // Pass the </Completion_Event>
            return new HappeningCompletionType(unitID, action, eventToPerform) ;
        }

    }
}
