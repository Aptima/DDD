using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.ScenarioParser
{
    /// <summary>
    /// A collection of routines for parsing the XML read from the scenario file
    /// </summary>

    public class Parser_4_2 : AbstractParser
    {

        //The order of commands
        private static string[][] stageMembers = new string[][]{
            new string[]{"ScenarioName"},
            new string[]{"Description"},
            new string[]{"ClientSideAssetTransfer"},
            new string[]{"ClientSideRangeRingVisibility"},
            new string[]{"Playfield"},
            new string[]{"LandRegion","ActiveRegion"},
            new string[]{"Team"},
            new string[]{"DecisionMaker"},
            new string[]{"Network"}, 
            new string[]{"DefineEngram"}, 
            new string[]{"Sensor"},
            new string[]{"Classifications"},
            new string[]{"TimeToAttack"},
            new string[]{"Genus","Species"},
            new string[]{"Create_Event","Reveal_Event","Move_Event","Completion_Event",
                         "Species_Completion_Event","StateChange_Event","Transfer_Event","Launch_Event","WeaponLaunch_Event",
                         "ChangeEngram","RemoveEngram","FlushEvents","Reiterate","OpenChatRoom","CloseChatRoom",
                        "OpenVoiceChannel","CloseVoiceChannel", "Attack_Successful_Event","Attack_Request_Approval",
                        "SetRegionVisibility","SendChatMessage", "Apply", "SendVoiceMessage", "SendVoiceMessageToUser"},
            new string[]{"Rule"},
            new string[]{"Score"}
       };
        public override string[][] StageMembers()
        {
            return stageMembers;
        }



        /// <summary>
        /// The text reader is created outside the parser
        /// </summary>
        XmlReader reader;
        /// <summary>
        /// Constructs a parser -- there should be only one at any time
        /// </summary>
        /// <param name="reader">A previously opened XML reader</param>
        public override void SetReader(XmlReader r)
        {
            this.reader = r;
        }
        public Parser_4_2()//XmlReader reader)
        {
            //   this.reader = reader;


            // 'None' only prevents whitespace  being returned as a node
            // Now assuming that there is no embedded whitespace that isn't significant.
        }

        private static string[] colorArray =
       {
"AliceBlue", "Aqua", "Azure", "Blue", "BlueViolet", "Brown",
"CornflowerBlue", "Crimson", "Cyan", "DarkBlue", "DarkGray", "DarkGreen",
"DarkKhaki", "DarkOliveGreen", "DarkRed", "DarkSlateGray", "DodgerBlue", "Fuchsia",
"Gray", "Green", "GreenYellow", "HotPink", "Khaki", "Lavender",
"LimeGreen", "Magenta", "Maroon", "Navy", "Olive", "OliveDrab",
"Orange", "OrangeRed", "Plum", "PowderBlue", "Purple", "Red",
"RosyBrown", "RoyalBlue", "SandyBrown", "SeaGreen", "Silver", "SkyBlue",
"SlateBlue", "SlateGray","SteelBlue", "Tan", "Teal", "Transparent", "Turquoise",
"Violet", "Yellow", "YellowGreen"
            };
        private static List<string> Colors = new List<string>(colorArray);
        //Used to split strings
        private static Regex whitespaceRegex = new Regex(@"\s+");
        private static Regex commaRegex = new Regex(@"\s*,\s*");
        //Used to match strings
        private static Regex integerRegex = new Regex(@"^-?\d+$");
        private static Regex decimalRegex = new Regex(@"^\s*-?\s*(\d+.?|\d*.\d+)\s*$");



        public override List<string> pGetStringList(Regex separator)
        {
            return pRetrieveStringList(pGetString, separator);
        }
        public override List<string> pRetrieveStringList(StringFromInput getString, Regex separator)
        { // Note: does not have any tags to bypass
            // tags are pulled off in innermost pGetString
            string[] parseOfString;
            string stringToParse = "unknown list of strings";
            List<string> returnValue = new List<string>();
            try
            {
                stringToParse = getString();
                parseOfString = separator.Split(stringToParse);
                for (int i = 0; i < parseOfString.Length; i++)
                {
                    if ("" != parseOfString[i]) returnValue.Add(parseOfString[i].Trim(' '));
                }
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Failure parsing string list " + stringToParse + ": ", e);
            }
        }
        public override List<double> pGetDoublesList(int numberExpected)
        {
            return pGetDoublesList(numberExpected, numberExpected);
        }

        public override List<double> pGetDoublesList(int lowNumberExpected, int highNumberExpected)
        {
            // Note: does not have any tags to bypass
            string valueBeingChecked = "Unknown string";
            List<string> toCheck;
            List<double> returnValue = new List<double>();
            toCheck = pRetrieveStringList(pGetNakedString, whitespaceRegex);
            if (!(
                (lowNumberExpected <= toCheck.Count)
                && (highNumberExpected >= toCheck.Count))
                ) throw new ApplicationException("Wrong number of entries in decimal list " + toCheck);
            Boolean validDecimal = true;
            for (int i = 0; i < toCheck.Count; i++)
            {
                valueBeingChecked = toCheck[i];
                if (!decimalRegex.IsMatch(toCheck[i]))
                {
                    validDecimal = false;
                }
                else
                {
                    returnValue.Add(Convert.ToDouble(toCheck[i]));
                }
            }
            if (false == validDecimal) throw new ApplicationException("String " + valueBeingChecked + " contains non decimal element.");
            return returnValue;
        }

        public override List<int> pGetIntList(int lowNumberExpected, int highNumberExpected)
        {
            // Note: does not have any tags to bypass
            string valueBeingChecked = "Unknown string";
            List<string> toCheck;
            List<int> returnValue = new List<int>();
            toCheck = pRetrieveStringList(pGetNakedString, whitespaceRegex);
            if (!(
                (lowNumberExpected <= toCheck.Count)
                && (highNumberExpected >= toCheck.Count))
                )
                throw new ApplicationException("Wrong number of entries in integer list " + toCheck);
            Boolean validInts = true;
            for (int i = 0; i < toCheck.Count; i++)
            {

                valueBeingChecked = toCheck[i];
                if (!integerRegex.IsMatch(toCheck[i]))
                {
                    validInts = false;
                }
                else
                {
                    returnValue.Add(Convert.ToInt32(toCheck[i]));
                }
            }
            if (false == validInts) throw new ApplicationException("String " + valueBeingChecked + " does not represent an integert.");
            return returnValue;
        }

        /// <summary>
        /// Get a boolean either as "true"/"false" or as "1"/"0"
        /// </summary>
        /// <returns>Boolean</returns>
        /// 
        public override Boolean pGetBoolean()
        {
            //Representations of true are "true" and "1"; false is denoted as "false" or "0".

            string maybeBoolean = pGetString();
            Boolean returnBoolean = false;
            /* This doesn't work for all cases
            
            Boolean foundBoolean = true;
            if ("1" == maybeBoolean)
            {
                returnBoolean = true;
                foundBoolean = true;
            }
            else if ("true" == maybeBoolean)
            {
                returnBoolean = true;
                foundBoolean = true;
            }
            else if ("0" == maybeBoolean)
            {
                returnBoolean = false;
                foundBoolean = true;
            }
            else if ("false" == maybeBoolean)
            {
                returnBoolean = false;
                foundBoolean = true;
            }
            if (!foundBoolean)
            {
                throw new ApplicationException("The value " + maybeBoolean.ToString() + " is not a legal Boolean value");
            }
            return returnBoolean;
             */
            try
            {
                returnBoolean = Convert.ToBoolean(maybeBoolean);
            }
            catch (Exception exception)
            {
                if (maybeBoolean == "1")
                {
                    returnBoolean = true;
                }
                else
                {
                    returnBoolean = false;
                }
            }

            return returnBoolean;
        }

        /// <summary>
        /// Reads/returns a string from the input
        /// </summary>
        public override string pGetString()
        {// Entered expecting the current Node to be <some simple element>
            reader.Read();
            string returnString = reader.Value;
            reader.Read();
            reader.ReadEndElement();
            return returnString;
        }
        public override string pGetNakedString()
        {// Entered expecting the current Node to be data

            string returnString = reader.Value;
            reader.Read();
            return returnString;
        }
        /// <summary>
        /// Reads/returns a double from the input
        /// </summary>
        public override double pGetDouble()
        {// Entered expecting the current Node to be <some simple element>
            reader.Read(); //pass by <name of element>
            double returnDouble = Convert.ToDouble((string)reader.Value);   // the x value
            reader.Read();
            reader.ReadEndElement();
            return returnDouble;
        }
        /// <summary>
        /// Reads/returns an integer from the input
        /// </summary>
        public override int pGetInt()
        {// Entered expecting the current Node to be <some simple element>
            reader.Read(); //pass by <name of element>
            int returnInt = Convert.ToInt32((string)reader.Value);   // the x value
            reader.Read();
            reader.ReadEndElement();
            return returnInt;
        }

        public override pNoteType pGetNote()
        {
            pNoteType returnValue;
            //Note, the brackets are stripped by Getstring
            returnValue = new pNoteType(pGetString());
            return returnValue;

        }
        /// <summary>
        /// Retrieves the integer value of a time
        /// </summary>
        public override int pGetTime()
        { // Entered expecting the current Node to be <some simple element>
            return pGetInt();
        }

        public override pPointType pGetPoint()
        {
            double x, y;
            List<double> coordinates;
            try
            {
                reader.Read(); // bypass 'Vertex'

                coordinates = pGetDoublesList(2);
                x = coordinates[0];
                y = coordinates[1];

                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading point in LandRegion. ", e);
            }

            return new pPointType(x, y);

        }

        public override pRandomIntervalType pGetRandomInterval()
        {
            int start, end;

            try
            {
                reader.Read(); // bypass 'RandomInterval'
                List<int> limits = pGetIntList(2, 2);
                start = limits[0];
                end = limits[1];
                if (end < start)
                {
                    start += end;
                    end = start - end;
                    start = start - end;
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Random Interval. ", e);
            }

            return new pRandomIntervalType(start, end);

        }

        public override string pGetColor()
        {
            string color = pGetString();
            if (!Colors.Contains(color))
            {
                throw new ApplicationException("Illegal color value " + color);
            }
            return color;
        }


        /// <summary>
        /// Reads/returns a Location Type from the input
        /// </summary>
        public override pLocationType pGetLocation()
        {// Entered expecting the current Node to be something that encloses a Location
            // i.e. 'Setting'
            double x, y, z;
            List<Double> coordinates;
            try
            {
                reader.Read(); //go past bracket; pointing to <VectorType>
                if ("X" == reader.Name)
                {
                    x = pGetDouble();
                    y = pGetDouble();
                    z = 0;
                    if ("Z" == reader.Name)
                    {
                        z = pGetDouble();
                    }
                }
                else
                {
                    coordinates = pGetDoublesList(2, 3);
                    x = coordinates[0];
                    y = coordinates[1];
                    z = 0;
                    if (3 == coordinates.Count) z = coordinates[2];
                }
                reader.ReadEndElement(); // go past bracket
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Location value: ", e);
            }
            return new pLocationType(x, y, z);
        }

        public override pTeamType pGetTeam()
        {
            string name = "unknown name";
            pTeamType team;
            List<string> againstList;
            try
            {
                reader.Read();
                name = pGetString();
                team = new pTeamType(name);
                while ("Against" == reader.Name) // allowing backward compatibility
                {
                    //                  team.Add(pGetString());
                    againstList = pGetStringList(commaRegex);
                    for (int i = 0; i < againstList.Count; i++) team.Add(againstList[i]);

                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Team named "
                    + name + ": ", e);
            }
            return team;
        }

        public override pNetworkType pGetNetwork()
        {
            string name = "Unknown network";
            pNetworkType network;
            List<string> memberList;
            try
            {
                reader.Read();
                name = pGetString();
                network = new pNetworkType(name);
                while ("Member" == reader.Name) // Maintain backward compatibility
                {
                    memberList = pGetStringList(commaRegex);
                    for (int i = 0; i < memberList.Count; i++) network.Add(memberList[i]);

                    //  network.Add(member);
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Network named "
                    + name + ": ", e);
            }
            return network;
        }

        /*     public   pDmTransitionType pGetDmTransition()
             {
                 reader.Read(); //that was the type of transition; it is known to caller
                 string groupID = pGetString();
                 int timer = pGetInt();
                 string memberID = pGetString();
                 reader.ReadEndElement();
                 return new pDmTransitionType(timer, groupID, memberID);
            }
         * */
        /// <summary>
        /// Reads/returns a Velocity Type from the input
        /// </summary>
        public override pVelocityType pGetVelocity()
        {// Entered expecting the current Node to be something that encloses a VelocityType
            // i.e. 'Setting'
            double vx, vy, vz;
            List<double> coordinates;
            try
            {
                reader.Read(); //go past bracket; pointing to <VectorType>
                if ("VX" == reader.Name)
                {
                    vx = pGetDouble();
                    vy = pGetDouble();
                    vz = pGetDouble();
                }
                else
                {
                    coordinates = pGetDoublesList(3);
                    vx = coordinates[0];
                    vy = coordinates[1];
                    vz = coordinates[2];
                }

                reader.ReadEndElement(); // go past bracket
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Velocity: ", e);
            }

            return new pVelocityType(vx, vy, 1000 * vz);
        }

        public override pCone pGetCone()
        {
            double spread, extent;
            pLocationType direction;
            string level;
            try
            {
                reader.Read();
                spread = pGetDouble();
                extent = pGetDouble();
                direction = pGetLocation();
                level = pGetString();
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Cone: ", e);
            }
            return new pCone(spread, extent, direction, level);
        }

        public override pSensor pGetSensor()
        {
            /*
             * Either a <Name> followed by <Attribute> followed by a list of cones
             * or
             * Name followed by <Extent>
             */
            string name = "Unknown Sensor ";
            double extent;
            pSensor returnSensor;
            try
            {
                reader.Read(); // bypass 'Sensor'
                name = pGetString();

                if ("Extent" == reader.Name)
                {
                    // No 'cone' element in input  but
                    //  use first cone to hold "extent" and ignore other fields 

                    returnSensor = new pSensor(name, "All");
                    extent = pGetDouble();
                    returnSensor.Cones.Add(new pCone(0, extent, new pLocationType(0, 0, 0), ""));
                }

                else
                {
                    if ("Attribute" == reader.Name)
                    {
                        string attribute = pGetString();
                        returnSensor = new pSensor(name, attribute);
                    }
                    else//if ("Engram" == reader.Name)
                    {
                        string engram = pGetString();
                        string typeName = "string";
                        if ("Type" == reader.Name)
                            typeName = pGetString();
                        returnSensor = new pSensor(name, engram, true, typeName);
                    }
                    while ("Cone" == reader.Name)
                    {
                        returnSensor.Add(pGetCone());
                    }
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Sensor named " +
                    name + ": ", e);
            }

            return returnSensor;

        }


        public override List<String> pGetClassifications()
        {
            List<String> classifications = new List<string>();
            try
            {
                reader.Read();


                while (reader.Name == "Classification")
                {
                    classifications.Add(pGetString());
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Classifications:", e);
            }

            return classifications;

        }

        public override ClassificationDisplayRulesValue pGetClassificationDisplayRules()
        {
            ClassificationDisplayRulesValue rules = new ClassificationDisplayRulesValue();
            try
            {
                reader.Read();


                while (reader.Name == "ClassificationDisplayRule")
                {
                    ClassificationDisplayRulesValue.ClassificationDisplayRule rule = new ClassificationDisplayRulesValue.ClassificationDisplayRule();
                    reader.Read();
                    rule.State = pGetString();
                    rule.Classification = pGetString();
                    rule.DisplayIcon = pGetString();
                    reader.ReadEndElement();
                    rules.rules.Add(rule);
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Classifications:", e);
            }

            return rules;

        }

        /// <summary>
        /// Retrieves a ist of Capabilities
        /// </summary>
        public override Dictionary<string, pCapabilityType> pGetCapabilities()
        {
            // Entered expecting the current Node to be something that encloses a list
            // of Capability definitions
            pCapabilityType capability;
            Dictionary<string, pCapabilityType> capabilities = new Dictionary<string, pCapabilityType>();
            string capabilityName = "Unknown capability";
            try
            {
                pProximityType proximity;
                double range;
                //          pEffectType effect;
                int intensity;
                double probability = 1;
                while ("Capability" == reader.Name)
                {
                    reader.Read();
                    capabilityName = pGetString();
                    if (capabilities.ContainsKey(capabilityName)) throw new ApplicationException("Cannot have two independent capabilities with same name " + capabilityName);
                    capability = new pCapabilityType();
                    while ("Proximity" == reader.Name)
                    {
                        reader.Read(); // pass the 'Proximity'
                        range = pGetDouble();
                        proximity = new pProximityType(range);
                        while ("Effect" == reader.Name)
                        {
                            reader.Read();
                            intensity = pGetInt();
                            if ("Probability" == reader.Name)
                            {
                                probability = pGetDouble();
                            }
                            proximity.Add(new pEffectType(intensity, probability));
                            reader.ReadEndElement(); // end effect
                        }
                        capability.Add(proximity);
                        reader.ReadEndElement();
                    }
                    capabilities[capabilityName] = capability;

                    reader.ReadEndElement(); // pass the close </capability>
                }
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Capability named "
                    + capabilityName + ": ", e);
            }



            return capabilities;
        }

        public override pTransitionType pGetTransition()
        {
            double range = 0;
            double probability = 1;
            int effect;
            string state;
            pTransitionType returnValue;
            try
            {
                effect = pGetInt();
                if ("Range" == reader.Name)
                {
                    range = pGetDouble();
                }

                if ("Probability" == reader.Name)
                {
                    probability = pGetDouble();
                }
                state = pGetString();

                returnValue = new pTransitionType(effect, range, probability, state);

            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Transition: ", e);
            }


            returnValue = new pTransitionType(effect, range, probability, state);

            return returnValue;
        }
        public override pSingletonVulnerabilityType pGetSingletonVulnerability()
        {
            List<pTransitionType> transitions = new List<pTransitionType>();
            try
            {
                reader.Read();// pass over <Transitions>
                while ("Effect" == reader.Name)
                {
                    transitions.Add(pGetTransition());
                }
                reader.ReadEndElement();//<end Transitions>
                reader.ReadEndElement(); //<End SingletonVulnerability>
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading SingletonVulnerability: " + e.Message);
            }
            return new pSingletonVulnerabilityType(transitions);
        }

        public override Dictionary<string, pSingletonVulnerabilityType> pGetVulnerabilities()
        {
            string capability = "Unknown capability";
            Dictionary<string, pSingletonVulnerabilityType> vulnerabilities = new Dictionary<string, pSingletonVulnerabilityType>();
            try
            {
                while ("SingletonVulnerability" == reader.Name)
                {
                    reader.Read();//pass singleton vulnerability
                    capability = pGetString();
                    if (vulnerabilities.ContainsKey(capability))
                    {
                        throw new ApplicationException("Cannot have two Iindependent singleton Vulnerabilities with same capability " + capability);
                    }
                    vulnerabilities[capability] = pGetSingletonVulnerability();
                }
            }

            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Vulnerabilities with capability "
                    + capability + ": " + e.Message);
            }
            return vulnerabilities;
        }

        public override pContributionType pGetContribution()
        {
            string capability;
            int effect;
            double range = 0.0;
            double probability = 1.0;
            try
            {
                reader.Read(); // pass over <contribution>
                capability = pGetString();
                effect = pGetInt();
                if ("Range" == reader.Name)
                {
                    range = pGetDouble();
                }
                if ("Probability" == reader.Name)
                {
                    probability = pGetDouble();
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Contribution: " + e.Message);
            }
            return new pContributionType(capability, effect, range, probability);
        }

        public override pComboVulnerabilityType pGetComboVulnerability()
        {
            List<pContributionType> contributions = new List<pContributionType>();
            string newState;
            try
            {
                reader.Read();
                while ("Contribution" == reader.Name)
                {
                    contributions.Add(pGetContribution());
                }
                newState = pGetString();
                reader.Read();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading ComboVulnerability: " + e.Message);
            }
            return new pComboVulnerabilityType(contributions, newState);

        }
        public override List<pComboVulnerabilityType> pGetCombinations()
        {
            List<pComboVulnerabilityType> combinations = new List<pComboVulnerabilityType>();
            try
            {
                while ("ComboVulnerability" == reader.Name)
                {
                    combinations.Add(pGetComboVulnerability());
                }
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading List of ComboVulnerabilities: ", e);
            }
            return combinations;
        }

        public override pEmitterType pGetEmitter()
        {

            /* either an attribute followed by a sequence of (LevelName,statistic)
             * or only
             * <attribute>Default</Attribute>
           */
            string attribute = "Unknown attribute";
            Boolean isEngram = false;
            string typeIfEngram = "string";
            Dictionary<string, double> level = new Dictionary<string, double>();
            try
            {
                reader.Read();// pass <Emitter>          
                if ("Attribute" == reader.Name)
                {
                    attribute = pGetString();
                }
                else
                {
                    attribute = pGetString();
                    isEngram = true;
                    if ("Type" == reader.Name)
                        typeIfEngram = pGetString();

                }
                // If there are no levels then the dictionary will be empty
                if (("Default" != attribute) && ("Invisible" != attribute))
                {
                    if ("NormalEmitter" != reader.Name) throw new ApplicationException("Expected NormalEmitter Definition.");
                    reader.Read();
                    while ("Level" == reader.Name)
                    {
                        string levelName = pGetString();
                        double statistic = pGetDouble();
                        level[levelName] = statistic;
                    }
                    reader.ReadEndElement();
                }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading emitter for attribute "
                    + attribute + ": ", e);
            }
            return new pEmitterType(attribute, isEngram, typeIfEngram, level);
        }

        /// <summary>
        /// Retrieves a set of parameters
        /// </summary>
        public override Dictionary<string, object> pGetParameters()
        {
            // Entered expecting the current Node to be something that encloses a list
            // of parameter setting pairs, such as Parameters

            reader.Read(); //go past bracket; pointing to first <ParameterSettingType>
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string name = "Unknown parameter";
            object setting;
            try
            {
                while ("Parameter" == reader.Name)
                {
                    name = pGetString();
                    switch (name)
                    {
                        case "InitialLocationLocation": // all names that appear in Parametrs ne to be covered here
                        case "Destination":
                            setting = pGetLocation();
                            break;
                        case "Velocity":
                            setting = pGetVelocity();
                            break;
                        default:
                            setting = pGetString();
                            break;
                    }
                    parameters[name] = setting;
                }
                reader.ReadEndElement(); // pass the close </Parameters>
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading parameters list when parameter="
                    + name + ": ", e);
            }
            return parameters;
        }

        public override pArmamentType pGetArmament()// obsolete
        {

            return null;
        }
        public override pDockedPlatformType pGetDockedPlatform()// obsolete
        {

            return null;
        }
        public override pAdoptType pGetAdoptType()// obsolete
        {
            return null;
        }

        public override pLaunchedPlatformType pGetLaunchedPlatform()// obsolete
        {

            return null;
        }
        public override pSubplatformType pGetSubplatform() //obsolete
        {

            return null;
        }
        public override pStateBody pGetStateBody(Boolean requireIcon)
        {
            pStateBody body = new pStateBody();
            try
            {
                body.Icon = "";

                body.FuelDepletionState = "Dead";


                if ("Icon" == reader.Name)
                {
                    body.Icon = pGetString();
                }
                else if (requireIcon)
                {
                    throw new ApplicationException("Required icon missing");
                }
                if ("StateParameters" == reader.Name)
                {
                    reader.Read();
                    if ("LaunchDuration" == reader.Name) body.LaunchDuration = pGetDouble();
                    if ("DockingDuration" == reader.Name) body.DockingDuration = pGetDouble();
                    //       if ("TimeToRemove" == reader.Name) body.TimeToRemove = pGetInt()*1000;
                    if ("TimeToAttack" == reader.Name) body.TimeToAttack = pGetInt() * 1000;
                    if ("EngagementDuration" == reader.Name) body.EngagementDuration = pGetInt() * 1000;
                    if ("MaximumSpeed" == reader.Name) body.MaximumSpeed = pGetDouble();
                    if ("FuelCapacity" == reader.Name) body.FuelCapacity = pGetDouble();
                    if ("InitialFuelLoad" == reader.Name) body.InitialFuelLoad = pGetDouble();
                    if ("FuelConsumptionRate" == reader.Name) body.FuelConsumptionRate = pGetDouble();
                    if ("FuelDepletionState" == reader.Name) body.FuelDepletionState = pGetString();
                    if ("Stealable" == reader.Name) body.Stealable = pGetBoolean();
                    while ("Sense" == reader.Name)
                    {
                        body.Sensors.Add(pGetString());

                    }
                    if ("Capability" == reader.Name)
                    {
                        body.Capabilities = pGetCapabilities();
                    }


                    if ("SingletonVulnerability" == reader.Name)
                    {
                        body.Vulnerabilities = pGetVulnerabilities();

                    }


                    if ("ComboVulnerability" == reader.Name)
                    {
                        body.Combinations = pGetCombinations();
                    }



                    while ("Emitter" == reader.Name)
                    {
                        pEmitterType emitter = pGetEmitter();
                        body.Emitters[emitter.Attribute] = emitter;

                    }
                    
                    reader.ReadEndElement();
                }
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading state  body: ", e);
            }
            return body;
        }
        /// <summary>
        /// Retrieves the information about a single state
        /// </summary>
        public override pStateType pGetState()
        {
            string name = "Unknown state";
            pStateBody body;
            try
            {

                name = pGetString();
                body = pGetStateBody(false);
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading state named " +
 ": ", e);
            }

            return new pStateType(name, body);
        }
        /// <summary>
        /// Reads/returns a Playfield directive from the input
        /// </summary>
        public override pPlayfieldType pGetPlayfield()
        {
            string mapFileName = "Unknown map file";
            string iconLibrary = "";
            string displayLabels = "";
            string displayTags = "";
            string utmZone;
            double verticalScale;
            double horizontalScale;
            try
            {
                reader.Read();
                mapFileName = pGetString();
                if ("IconLibrary" == reader.Name)
                    iconLibrary = pGetString();
                utmZone = pGetString();
                verticalScale = pGetDouble();
                horizontalScale = pGetDouble();
                if ("ClientSideDisplayLabels" == reader.Name)
                    displayLabels = pGetString().ToLower();
                if ("ClientSideTagDisplay" == reader.Name)
                    displayTags = pGetString().ToLower();
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading playfield with map " +
                    mapFileName + ": ", e);
            }

            return new pPlayfieldType(mapFileName, iconLibrary, utmZone, verticalScale, horizontalScale, displayLabels, displayTags);
        }


        public override pLandRegionType pGetLandRegion()
        {
            string id = "Unknown Land Region";
            pLandRegionType region;
            try
            {
                reader.Read(); // bypass 'xxxRegion'
                id = pGetString();
                region = new pLandRegionType(id);
                while ("Vertex" == reader.Name)
                {
                    region.Add(pGetPoint());
                }
                if (3 > region.Vertices.Count)
                {
                    throw new ApplicationException("Found vertex list for region "
                        + id + " with fewer than three vertices.");
                }

                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Land Region with id=" +
                    id + ": ", e);
            }

            return region;
        }


        public override pActiveRegionType pGetActiveRegion()
        {
            string id = "Unknown Active Region";
            pActiveRegionType region;

            try
            {
                reader.Read(); // bypass 'xxxRegion'
                id = pGetString();
                region = new pActiveRegionType(id);
                region.ReferencePoint = new pLocationType();
                while ("Vertex" == reader.Name)
                {
                    region.Add(pGetPoint());
                }
                if (3 > region.Vertices.Count)
                {
                    throw new ApplicationException("Found vertex list for region "
                        + id + " with fewer than three vertices.");
                }
                region.ReferencePoint = new pLocationType();
                region.Start = 0.0;
                region.IsDynamicRegion = false;
                if ("ReferencePoint" == reader.Name || "IsDynamicRegion" == reader.Name || "Start" == reader.Name)
                {
                    if ("ReferencePoint" == reader.Name)
                    {
                        region.ReferencePoint = pGetLocation();
                        region.IsDynamicRegion = pGetBoolean();
                        region.Start = pGetDouble();
                    }
                    else if ("IsDynamicRegion" == reader.Name)
                    {
                        region.IsDynamicRegion = pGetBoolean();
                        region.Start = pGetDouble();
                    }
                    else
                        region.Start = pGetDouble();
                }

                //if ("IsDynamicRegion" == reader.Name)
                //    region.IsDynamicRegion = pGetBoolean();
                //if ("Start" == reader.Name)
                //    region.Start = pGetDouble();
                region.End = 0.0;
                if ("End" == reader.Name)
                    region.End = pGetDouble();
                region.SpeedMultiplier = 1;
                if ("SpeedMultiplier" == reader.Name) region.SpeedMultiplier = pGetDouble();
                region.BlocksMovement = false;
                if ("BlocksMovement" == reader.Name) region.BlocksMovement = pGetBoolean();
                if ("SensorsBlocked" == reader.Name)
                {
                    region.Add(pGetStringList(commaRegex));
                }
                if ("IsVisible" == reader.Name)
                    region.IsVisible = pGetBoolean();
                if ("IsActive" == reader.Name)
                    region.IsActive = pGetBoolean();
                if ("Color" == reader.Name)
                    region.Color = pGetColor();
                if ("ObstructedViewImage" == reader.Name)
                    region.ObstructedViewImage = pGetString();
                if ("ObstructionOpacity" == reader.Name)
                    region.ObstructionOpacity = pGetDouble();
                //if ("ReferencePoint" == reader.Name)
                //    region.ReferencePoint = pGetLocation();
                //if ("Scale" == reader.Name)
                //    region.Scale = pGetDouble();
                //if ("Rotation" == reader.Name)
                //    region.Rotation = pGetDouble();
                //if ("CurrentAbsolutePolygon" == reader.Name)
                //    region.CurrentAbsolutePolygon = pg
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Active Region with id=" +
                    id + ": ", e);
            }

            return region;
        }


        /// <summary>
        /// Retrieves the definition of a genus
        /// </summary>
        public override pGenusType pGetGenus()
        {
            //    Dictionary<string, pStateBody> states = new Dictionary<string, pStateBody>();
            //           Dictionary<string, object> parameters;
            string name = "Unknown Genus";
            try
            {
                reader.Read();
                name = pGetString();
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Genus named " +
                    name + ": ", e);
            }

            return new pGenusType(name);
        }
        /// <summary>
        /// Retrieves the definition of a species
        /// </summary>
        /// <returns></returns>
        public override pSpeciesType pGetSpecies()
        {
            //     Dictionary<string, pStateBody> states = new Dictionary<string, pStateBody>();
            //Dictionary<string, object> parameters;
            string name = "Unknown Species";
            pStateBody ff;
            pSpeciesType species = new pSpeciesType();

            try
            {
                reader.Read();
                name = pGetString();

                species.Name = name;
                species.BasedOn = pGetString();


                if ("Size" == reader.Name)
                {
                    species.Size = pGetDouble();
                }


                if ("IsWeapon" == reader.Name)
                {
                    species.IsWeapon = pGetBoolean();
                }
                if ("RemoveOnDestruction" == reader.Name)
                {
                    species.RemoveOnDestruction = pGetBoolean();
                }
                if ("DefaultClassification" == reader.Name)
                {
                    species.DefaultClassification = pGetString();
                }
                //species.ClassificationDisplayRules = new ClassificationDisplayRulesValue();
                if ("ClassificationDisplayRules" == reader.Name)
                {
                    species.ClassificationDisplayRules = pGetClassificationDisplayRules();
                }
                if ("CanOwn" == reader.Name)
                {
                    species.AddOwners(pGetStringList(commaRegex));
                }
                if ("LaunchedByOwner" == reader.Name)
                {
                    species.LaunchedByOwner = pGetBoolean();
                }
                //if ("SubplatformLimit" == reader.Name)
               // {
               //     species.SubplatformLimit = pGetInt();
                //}
                String speciesName = String.Empty;
                int speciesCount = 0;
                while ("SubplatformCapacity" == reader.Name)
                {
                    reader.Read();
                    if ("SpeciesName" == reader.Name)
                    {
                        speciesName = pGetString();
                    }
                    if ("Count" == reader.Name)
                    {
                        speciesCount = pGetInt();
                    }
                    species.AddSubplatformCapacity(speciesName, speciesCount);

                    reader.ReadEndElement();
                }

                if (reader.Name != "FullyFunctional")
                {
                    throw new ApplicationException("State FullyFunctional missing from species "
                        + species.Name);
                }
                reader.Read();

                ff = pGetStateBody(false);// Icon had been required
                species.States["FullyFunctional"] = ff;
                reader.ReadEndElement();

                while ("DefineState" == reader.Name)
                {
                    reader.Read();
                    if ("State" != reader.Name)
                    {
                        throw new ApplicationException("State name missing from state definition for species " + name);
                    }
                    {
                        pStateType s = pGetState();
                        species.States[s.Name] = s.Body;
                        reader.ReadEndElement(); // this is the </DefineState>

                    }

                }



                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Species named " +
                    name + ": ", e);
            }

            return species;
        }




        /// <summary>
        /// Reads/returns a Decision maker directive from the input
        /// </summary>
        public override pDecisionMakerType pGetDecisionMaker()
        {
            string role = "Unknown role";
            string identifier, color, briefing, team;
            List<string> reportsTo = new List<string>();
            //List<string> initialVoiceAccess = new List<string>();
            List<string> chatPartners = new List<string>();
            List<string> whiteboardPartners = new List<string>();
            List<string> voicePartners = new List<string>();
            Boolean canTransfer = false, canForceTransfers = false;
            Boolean isObserver = false;
            try
            {
                reader.Read();
                role = pGetString();
                identifier = pGetString();
                color = pGetColor();
                briefing = "";
                if ("Briefing" == reader.Name)
                {
                    briefing = pGetString();
                }

                if ("ReportsTo" == reader.Name)
                {
                    reportsTo = pGetStringList(commaRegex);
                }
                if ("CanTransfer" == reader.Name)
                {
                    canTransfer = pGetBoolean();
                }
                if ("CanForceTransfers" == reader.Name)
                {
                    canForceTransfers = pGetBoolean();
                }

                team = pGetString();


                //if ("InitialVoiceAccess" == reader.Name)
                //    initialVoiceAccess = pGetStringList(commaRegex);
                if ("CanChat" == reader.Name)
                {
                    chatPartners = pGetStringList(commaRegex);
                }

                if ("CanWhiteboard" == reader.Name)
                {
                    whiteboardPartners = pGetStringList(commaRegex);
                }

                if ("CanSpeak" == reader.Name)
                {
                    voicePartners = pGetStringList(commaRegex);
                }

                if ("IsObserver" == reader.Name)
                {
                    isObserver = pGetBoolean();
                }

                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Decision maker with role " +
                    role + ": ", e);
            }

            return new pDecisionMakerType(role, identifier, color, briefing, reportsTo, canTransfer, canForceTransfers, team, chatPartners, whiteboardPartners, voicePartners,isObserver);
        }
        /// <summary>
        /// Retrieves a create unit command from the scenario 
        /// </summary>
        public override pCreateType pGetCreate()
        {
            string unitID = "Unknown object";
            pCreateType c;
            try
            {
                reader.Read();// pass the CreateEvent tag
                unitID = pGetString();
                string unitKind = pGetString();
                string owner = pGetString();
                c = new pCreateType(unitID, unitKind, owner);
                while ("Subplatform" == reader.Name)
                {
                    c.DockFromList(pGetStringList(commaRegex));
                }

                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Create command for " +
                    unitID + ": ", e);
            }

            return c;
        }
        /// <summary>
        /// Retrieves a reveal command from the scenario 
        /// </summary>
        public override pRevealType pGetReveal()
        {
            string unitID = "Unknown object";
            string initialTag = "";
            pRevealType rEvent;
            try
            {
                string initialState = "";
                Dictionary<string, object> startupParameters;
                reader.Read();// pass the RevealEvent tag
                unitID = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                int time = pGetInt();
                pLocationType initialLocation = pGetLocation();
                if ("InitialState" == reader.Name)
                {
                    initialState = pGetString();
                }
                if ("InitialTag" == reader.Name)
                    initialTag = pGetString();
                if ("StartupParameters" == reader.Name)
                {
                    startupParameters = pGetParameters();
                }
                else
                {
                    startupParameters = new Dictionary<string, object>();
                }

                reader.ReadEndElement();
                rEvent = new pRevealType(unitID, engramRange, time, initialLocation, initialState,
                   startupParameters);
                rEvent.InitialTag = initialTag;
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Reveal command for " +
                    unitID + ": ", e);
            }

            return rEvent;
        }




        /// <summary>
        /// Reads/returns a Move Event directive from the input
        /// </summary>
        public override pMoveType pGetMove()
        {
            string unitID = "Unknown object";
            pMoveType mEvent;
            pRandomIntervalType ri = new pRandomIntervalType(0, 0);
            try
            {

                reader.Read();
                unitID = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                int timer = pGetTime();
                if ("RandomInterval" == reader.Name)
                {
                    ri = pGetRandomInterval();
                }
                double throttle = 100.0;
                if ("Throttle" == reader.Name)
                {
                    throttle = (pGetInt() + 0.0) / 100;
                }
                pLocationType destination = pGetLocation();
                mEvent = new pMoveType(unitID, engramRange, timer, destination, throttle);
                mEvent.RandomInterval = ri;
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Move command for " +
                    unitID + ": ", e);
            }

            return mEvent;
        }

        public List<Object> pGetDoThisList()
        {
            object eventToPerform;
            List<object> returnValue = new List<object>();
            while ("DoThis" == reader.Name)
            {
                reader.Read(); // bypass the DoThis tag
                switch (reader.Name)
                {
                    case "Move_Event":
                        eventToPerform = pGetMove();
                        break;
                    case "Reveal_Event":
                        eventToPerform = pGetReveal();
                        break;
                    case "StateChange_Event":
                        eventToPerform = pGetStateChange();
                        break;
                    case "Transfer_Event":
                        eventToPerform = pGetTransfer();
                        break;
                    case "Launch_Event":
                        eventToPerform = pGetLaunch();
                        break;
                    case "WeaponLaunch_Event":
                        eventToPerform = pGetWeaponLaunch();
                        break;
                    case "ChangeEngram":
                        eventToPerform = pGetChangeEngram();
                        break;
                    case "RemoveEngram":
                        eventToPerform = pGetRemoveEngram();
                        break;
                    case "FlushEvents":
                        eventToPerform = pGetFlushEventsType();
                        break;
                    case "SendChatMessage":
                        eventToPerform = pGetSendChatMessage();
                        break;
                    case "SendVoiceMessage":
                        eventToPerform = pGetSendVoiceMessage();
                        break;
                    case "SendVoiceMessageToUser":
                        eventToPerform = pGetSendVoiceMessageToUser();
                        break;
                    case "CloseVoiceChannel":
                        eventToPerform = pGetCloseVoiceChannel();
                        break;
                    case "OpenVoiceChannel":
                        eventToPerform = pGetOpenVoiceChannel();
                        break;
                    case "CloseChatRoom":
                        eventToPerform = pGetCloseChatRoom();
                        break;
                    case "OpenChatRoom":
                        eventToPerform = pGetOpenChatRoom();
                        break;
                        //AD Testing following events to see if it breaks anything.
                    case "Completion_Event":
                        eventToPerform = pGetHappeningCompletion();
                        break;
                    case "DefineEngram":
                        eventToPerform = pGetDefineEngram();
                        break;
                    case "Species_Completion_Event":
                        eventToPerform = pGetSpeciesCompletion();
                        break;
                    case "Reiterate":
                        eventToPerform = pGetReiterate();
                        break;
                        //end
                    default:
                        eventToPerform = null;
                        break;
                }
                if (null != eventToPerform)
                {
                    returnValue.Add(eventToPerform);
                }
                reader.ReadEndElement(); //Pass the </DoThis>
            }
            return returnValue;
        }

        public override pHappeningCompletionType pGetHappeningCompletion()
        {
            string unitID = "Unknown object";
            string action = "", newState = "";
            pHappeningCompletionType returnValue;
            //object eventToPerform;

            try
            {
                reader.Read();
                unitID = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                if ("Action" == reader.Name)
                {
                    action = pGetString();
                }
                else if ("NewState" == reader.Name)
                {
                    newState = pGetString();
                }
                returnValue = new pHappeningCompletionType(unitID, engramRange, action, newState);
                List<object> doThisList = pGetDoThisList();
                for (int i = 0; i < doThisList.Count; i++)
                    returnValue.AddAction(doThisList[i]);
                /*
                while ("DoThis" == reader.Name)
                {
                    reader.Read(); // bypass the DoThis tag
                    switch (reader.Name)
                    {
                        case "Move_Event":
                            eventToPerform = pGetMove();
                            break;
                        case "Reveal_Event":
                            eventToPerform = pGetReveal();
                            break;
                        case "StateChange_Event":
                            eventToPerform = pGetStateChange();
                            break;
                        case "Transfer_Event":
                            eventToPerform = pGetTransfer();
                            break;
                        case "Launch_Event":
                            eventToPerform = pGetLaunch();
                            break;
                        case "ChangeEngram":
                            eventToPerform = pGetChangeEngram();
                            break;
                        case "RemoveEngram":
                            eventToPerform = pGetRemoveEngram();
                            break;
                        case "FlushEvents":
                            eventToPerform = pGetFlushEventsType();
                            break;


                        default:
                            //                    Coordinator.debugLogger.WriteLine("Unknown Happening Complete Event: " + reader.Name);
                            eventToPerform = null;
                            break;
                    }
                    if (null != eventToPerform)
                    {
                        returnValue.AddAction(eventToPerform);
                    }
                
                    reader.ReadEndElement(); //Pass the </DoThis>
                } */
                reader.ReadEndElement(); // Pass the </Completion_Event>
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Happening command for unit named " +
                    unitID + ": ", e);
            }

            return returnValue;
        }

        public override pSpeciesCompletionType pGetSpeciesCompletion()
        {
            string species = "Unknown species";
            string action = "", newState = "";
            pSpeciesCompletionType returnValue;
            //object eventToPerform;

            try
            {
                reader.Read();
                species = pGetString();
                if ("Action" == reader.Name)
                {
                    action = pGetString();
                }
                else if ("NewState" == reader.Name)
                {
                    newState = pGetString();
                }
                returnValue = new pSpeciesCompletionType(species, action, newState);
                List<object> doThisList = pGetDoThisList();
                for (int i = 0; i < doThisList.Count; i++)
                    returnValue.AddAction(doThisList[i]);
                /* 
                while ("DoThis" == reader.Name)
                {
                    reader.Read(); // bypass the DoThis tag
                    switch (reader.Name)
                    {
                        case "Move_Event":
                            eventToPerform = pGetMove();
                            break;
                        case "Reveal_Event":
                            eventToPerform = pGetReveal();
                            break;
                        case "StateChange_Event":
                            eventToPerform = pGetStateChange();
                            break;
                        case "Transfer_Event":
                            eventToPerform = pGetTransfer();
                            break;
                        case "Launch_Event":
                            eventToPerform = pGetLaunch();
                            break;
                        case "ChangeEngram":
                            eventToPerform = pGetChangeEngram();
                            break;
                        case "RemoveEngram":
                            eventToPerform = pGetRemoveEngram();
                            break;
                        case "FlushEvents":
                            eventToPerform = pGetFlushEventsType();
                            break;
                        default:
                            //                    Coordinator.debugLogger.WriteLine("Unknown Happening Complete Event: " + reader.Name);
                            eventToPerform = null;
                            break;
                    }
                    if (null != eventToPerform)
                    {
                        returnValue.AddAction(eventToPerform);
                    }
                    reader.ReadEndElement(); //Pass the </DoThis>
                }
                 */
                reader.ReadEndElement(); // Pass the </Completion_Event>
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Species_Completion command for species named " +
                    species + ": ", e);
            }
            return returnValue;
        }
        /// <summary>
        /// Reads/returns a Reiterate event directive(one that
        /// specifies a sequence of (Move) events that are queued one after the other.
        /// 
        /// </summary>

        public override pReiterateType pGetReiterate()
        {
            //Note -- only actions allowed are those for which there is an upcoming trigger (like MoveComplete)
            int start = 0;
            pReiterateType returnValue;
            object eventToPerform;

            try
            {
                reader.Read();
                start = pGetInt();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                returnValue = new pReiterateType(start, engramRange);


                reader.Read(); // bypass the ReiterateThis tag
                while (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Move_Event":
                            eventToPerform = pGetMove();
                            break;
                        default:
                            eventToPerform = null;
                            break;
                    }
                    if (null != eventToPerform)
                    {
                        returnValue.AddAction(eventToPerform);
                    }
                }
                reader.ReadEndElement(); //Pass the </ReiterateThis>

                reader.ReadEndElement(); // Pass the </Reiterate>
            }
            catch (System.Exception e)
            {
                string message;
                if (0 == start)
                {
                    message = " with unknown or invalid start time ";
                }
                else
                {
                    message = " with start time = " + start.ToString();
                }

                throw new ApplicationException("Error reading Reiterate command" + message +
                     ": ", e);
            }

            return returnValue;
        }


        public override pStateChangeType pGetStateChange()
        {
            string unitID = "Unknown object";
            pStateChangeType returnValue;
            try
            {
                reader.Read();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }

                unitID = pGetString();
                int timer = pGetInt();
                string newState = pGetString();
                returnValue = new pStateChangeType(unitID, engramRange, timer, newState);
                if ("From" == reader.Name)
                {
                    while ("From" == reader.Name)
                    {
                        returnValue.AddPrecursor(pGetString());
                    }
                }
                else while ("Except" == reader.Name)
                    {
                        returnValue.AddException(pGetString());
                    }
                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading StateChange for " +
                    unitID + ": ", e);
            }

            return returnValue;
        }

        public override pTransferType pGetTransfer()
        {
            string unitID = "Unknown object";
            pTransferType returnValue;
            try
            {
                reader.Read();
                unitID = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }


                int timer = pGetInt();
                string from = pGetString();
                string to = pGetString();
                returnValue = new pTransferType(unitID, engramRange, timer, from, to);

                reader.ReadEndElement();
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("Error reading Transfer for " +
                    unitID + ": ", e);
            }

            return returnValue;
        }
        public override pLaunchType pGetLaunch()
        {
            string parent = "Unknown unit";
            string child = "";
            string kind = "";
            string initialState = "";
            Dictionary<string, object> startupParameters = new Dictionary<string, object>(); ;
            int time;
            pLocationType relativeLocation;
            try
            {

                reader.Read();
                parent = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                time = pGetInt();
                if ("Child" == reader.Name)
                {
                    child = pGetString();
                }
                else
                {
                    kind = pGetString(); //obsolete
                }

                relativeLocation = pGetLocation();
                if ("InitialState" == reader.Name)
                {
                    initialState = pGetString();
                }
                if ("StartupParameters" == reader.Name)
                {
                    startupParameters = pGetParameters();
                }
                reader.ReadEndElement();
                return new pLaunchType(parent, engramRange, child, kind, time, relativeLocation, initialState, startupParameters);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading Launch for parent=" +
                     parent + ": ", e);

            }

        }
        public override pWeaponLaunchType pGetWeaponLaunch()
        {
            string parent = "Unknown unit";
            string child = "";
            string target = "";
            string initialState = "";
            Dictionary<string, object> startupParameters = new Dictionary<string, object>(); ;
            int time;

            try
            {

                reader.Read();
                parent = pGetString();
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                time = pGetInt();
                if ("Child" == reader.Name)
                {
                    child = pGetString();
                }
                if ("Target" == reader.Name)
                {
                    target = pGetString();
                }
                if ("InitialState" == reader.Name)
                {
                    initialState = pGetString();
                }
                if ("StartupParameters" == reader.Name)
                {
                    startupParameters = pGetParameters();
                }
                reader.ReadEndElement();
                return new pWeaponLaunchType(parent, engramRange, child, target, time, initialState, startupParameters);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading Launch for parent=" +
                     parent + ": ", e);

            }

        }
        public override pDefineEngramType pGetDefineEngram()
        {
            string name = "Unknown engram name";
            string engramValue;
            string type = "String";
            try
            {
                reader.Read();
                name = pGetString();
                engramValue = pGetString();
                if ("Type" == reader.Name)
                    type = pGetString();

                reader.ReadEndElement();

                return new pDefineEngramType(name, engramValue, type);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading DefineEngram with name " + name + ": " + e.Message);
            }
        }

        public override pChangeEngramType pGetChangeEngram()
        {
            string name = "Unknown value";
            string unit = "";
            int time;
            string engramValue;
            try
            {
                reader.Read();
                name = pGetString();
                if ("Unit" == reader.Name)
                    unit = pGetString();
                time = pGetInt();
                engramValue = pGetString();
                reader.ReadEndElement();
                return new pChangeEngramType(name, unit, time, engramValue);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading ChangeEngram with name " + name + ": " + e.Message);
            }

        }

        public override pRemoveEngramType pGetRemoveEngram()
        {
            string name = "Unknown value";
            int time;
            try
            {
                reader.Read();
                name = pGetString();
                time = pGetInt();
                reader.ReadEndElement();
                return new pRemoveEngramType(name, time);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading RemoveEngram with name " + name + ": " + e.Message);
            }

        }
        public override pEngramRange pGetEngramRange()
        {
            string name = "Unknown item";
            string unit = "";
            Boolean inclusionRange = false;
            List<string> valueList;
            string condition;
            string compareTo;
            pEngramRange returnValue;
            try
            {
                reader.Read();
                name = pGetString();
                if ("Unit" == reader.Name)
                    unit = pGetString();
                if ("Comparison" == reader.Name)
                {
                    reader.Read();//bypass "Comparison"
                    condition = pGetString();
                    compareTo = pGetString();
                    reader.ReadEndElement();
                    returnValue = new pEngramRange(name, unit, condition, compareTo);
                }
                else
                {
                    if ("Included" == reader.Name)
                    {
                        inclusionRange = true;
                    }
                    //Now get the block with the range values
                    //           reader.Read(); // pass the <Inclusion>/<Exclusion>
                    returnValue = new pEngramRange(name, unit, inclusionRange);

                    valueList = pGetStringList(commaRegex);
                    for (int i = 0; i < valueList.Count; i++) returnValue.Add(valueList[i]);
                    //  reader.ReadEndElement();
                }
                reader.ReadEndElement();
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading EngramRange with name " + name + ": " + e.Message);

            }

        }

        public override pOpenChatRoomType pGetOpenChatRoom()
        {
            pOpenChatRoomType returnValue;
            string room = "Unknown chat room";
            int time;
            List<string> memberList;
            try
            {
                reader.Read();
                room = pGetString();
                time = pGetInt();
                returnValue = new pOpenChatRoomType(room, time);
                if ("Owner" == reader.Name)
                {
                    returnValue.Owner = pGetString();
                }
                while ("Members" == reader.Name)
                {
                    memberList = pGetStringList(commaRegex);
                    returnValue.Add(memberList);
                }
                reader.ReadEndElement();
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read OpenChatRoom for room " + room, e);
            }
        }
        /* Not yet ready for implementation
                public pDropChattersType pGetDropChatters()
                {
                    pDropChattersType returnValue;
                    string room = "Unknown chat room";
                    int time;
                    List<string> memberList;
                    try
                    {
                        reader.Read();
                        room = pGetString();
                        time = pGetInt();
                        returnValue = new pDropChattersType(room, time);
                        while ("Members" == reader.Name)
                        {
                            memberList = pGetStringList(commaeRegex);
                            returnValue.Add(memberList);
                        }
                        reader.ReadEndElement();
                        return returnValue;

                    }
                    catch (SystemException e)
                    {
                        throw new ApplicationException("Could not read DropChatters for room " + room, e);
                    }
                }
                public pAddChattersType pGetAddChatters()
                {
                    pAddChattersType returnValue;
                    string room = "Unknown chat room";
                    int time;
                    List<string> memberList;
                    try
                    {
                        reader.Read();
                        room = pGetString();
                        time = pGetInt();
                        returnValue = new pAddChattersType(room, time);
                        while ("Members" == reader.Name)
                        {
                            memberList = pGetStringList(commaRegex     );
                            returnValue.Add(memberList);
                        }

                        reader.ReadEndElement();
                        return returnValue;
                    }
                    catch (SystemException e)
                    {
                        throw new ApplicationException("Could not read AddChatters for room " + room, e);
                    }
                }

         */

        public override pCloseChatRoomType pGetCloseChatRoom()
        {
            pCloseChatRoomType returnValue;
            string room = "Unknown chat room";
            int time;
            try
            {
                reader.Read();
                room = pGetString();
                time = pGetInt();

                returnValue = new pCloseChatRoomType(room, time);
                if ("Requestor" == reader.Name)
                    returnValue.Requestor = pGetString();
                reader.ReadEndElement();
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read CloseChatRoom for room " + room, e);
            }
        }

        public override pOpenWhiteboardRoomType pGetOpenWhiteboardRoom()
        {
            pOpenWhiteboardRoomType returnValue;
            string room = "Unknown whiteboard room";
            int time;
            List<string> memberList;
            try
            {
                reader.Read();
                room = pGetString();
                time = pGetInt();
                returnValue = new pOpenWhiteboardRoomType(room, time);
                if ("Owner" == reader.Name)
                {
                    returnValue.Owner = pGetString();
                }
                while ("Members" == reader.Name)
                {
                    memberList = pGetStringList(commaRegex);
                    returnValue.Add(memberList);
                }
                reader.ReadEndElement();
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read OpenWhiteboardRoom for room " + room, e);
            }
        }

        public override pSetRegionVisibilityType pGetSetRegionVisibility()
        {
            string id = "Unknown region";
            Boolean isVisible;
            int time;
            try
            {
                reader.Read();
                id = pGetString();
                time = pGetInt();
                isVisible = pGetBoolean();

                return new pSetRegionVisibilityType(id, time, isVisible);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error in SetRegionVisibilty for Region " + id, e);
            }
        }

        public override pSetRegionActivityType pGetSetRegionActivity()
        {
            string id = "Unknown region";
            Boolean isActive;
            int time;
            try
            {
                reader.Read();
                id = pGetString();
                time = pGetInt();
                isActive = pGetBoolean();

                return new pSetRegionActivityType(id, time, isActive);
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error in SetRegionActivity for Region " + id, e);
            }
        }

        public override pScoringLocationType pGetScoringLocation()
        {
            pScoringLocationType returnValue;
            List<string> zone = new List<string>(); ;
            string relationship;
            try
            {
                reader.Read(); // ScoringLocation Tag
                zone = pGetStringList(commaRegex);
                if ("Relationship" != reader.Name)
                {
                    returnValue = new pScoringLocationType(zone);
                }
                else
                {
                    relationship = pGetString();
                    returnValue = new pScoringLocationType(zone, relationship);
                }
                reader.ReadEndElement();
                return returnValue;
            }
            catch (SystemException e)
            {
                if (0 == zone.Count) throw new ApplicationException("Error in reading scoring location; zones unknown", e);

                throw new ApplicationException("Error in reading scoring location involving (first) zone " + zone[0], e);
            }
        }
        public override pActorType pGetActor()
        {
            pActorType returnValue;
            pScoringLocationType region;
            string id;
            string owner = "Unknown owner";
            try
            {
                reader.Read();
                owner = pGetString();
                id = pGetString();
                if ("Region" != reader.Name)
                {
                    returnValue = new pActorType(owner, id);
                }
                else
                {
                    region = pGetScoringLocation();
                    returnValue = new pActorType(owner, id, region);
                }
                reader.ReadEndElement();
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Failure reading Actor with owner " + owner, e);
            }
        }

        public override pScoringRuleType pGetScoringRule()
        {
            pScoringRuleType returnValue;
            string name = "unknown scoring rule";
            pActorType unit = null;
            pActorType objectID;
            string newState;
            string from;

            try
            {
                reader.Read();
                name = pGetString();
                unit = pGetActor();
                if ("Object" != reader.Name)
                {
                    returnValue = new pScoringRuleType(name, unit);
                }
                else
                {
                    objectID = pGetActor();
                    newState = pGetString();
                    if ("From" != reader.Name)
                    {

                        returnValue = new pScoringRuleType(name, unit, objectID, newState);

                    }
                    else
                    {
                        from = pGetString();
                        returnValue = new pScoringRuleType(name, unit, objectID, newState, from);
                    }

                }
                returnValue.Increment = pGetDouble();
                reader.ReadEndElement();
                return returnValue;

            }
            catch (SystemException e)
            {

                throw new ApplicationException("Failed to read scoring rule named " + name, e);
            }

        }

        public override pScoreType pGetScore()
        {
            string name = "unknown score name";
            List<string> rules;
            List<string> applies;
            List<string> viewers = new List<string>(); ;
            double initial = 0;
            try
            {
                reader.Read();
                name = pGetString();
                rules = pGetStringList(commaRegex);
                applies = pGetStringList(commaRegex);
                if ("Viewers" == reader.Name) viewers = pGetStringList(commaRegex);
                if ("Initial" == reader.Name) initial = pGetDouble();


                return new pScoreType(name, rules, applies, viewers, initial);
            }

            catch (SystemException e)
            {
                throw new ApplicationException("Problem reading score named " + name, e);
            }
        }


        public override pFlushEventsType pGetFlushEventsType()
        {
            string unit = "Unknown unit";
            int time;

            try
            {
                reader.Read();
                unit = pGetString();
                time = pGetInt();
                pFlushEventsType returnValue = new pFlushEventsType(unit, time);
                reader.ReadEndElement();
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading FlushEvents with unit " + unit + ": " + e.Message);

            }

        }

        public override pAttack_Successful_Completion_Type pGetAttackSuccessfulCompletion()
        {
            string species = "Unknown species";
            try
            {
                reader.Read(); // command name
                pEngramRange engramRange = null;
                if ("EngramRange" == reader.Name)
                {
                    engramRange = pGetEngramRange();
                }
                species = pGetString();
                string capability = pGetString();

                string targetSpecies = "";
                //     List<object> targetDoThisList = new List<object>();
                if ("TargetSpecies" == reader.Name)
                    targetSpecies = pGetString();
                //      targetDoThisList = pGetDoThisList();
                string newState = "";
                if ("NewState" == reader.Name)
                    newState = pGetString();
                List<object> doThisList = pGetDoThisList();
                pAttack_Successful_Completion_Type returnValue = new pAttack_Successful_Completion_Type(species, capability, doThisList, targetSpecies,/* targetDoThisList, */engramRange, newState);
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Problem parsing Attack_Successful_Completion for species: " + species, e);
            }
        }

        public override pAttack_Request_Approval_Type pGetAttackRequestApproval()
        {
            string capability = "Unknown Capability";
            string actor = "";
            Boolean actorIsSpecies = false;
            string target = "";
            Boolean targetIsSpecies = false;
            Boolean useDefault = true;
            pEngramRange engramRange = null;
            string failure = "";
            try
            {
                reader.Read();
                capability = pGetString();
                if ("ActorSpecies" == reader.Name)
                {
                    actorIsSpecies = true;
                    actor = pGetString();
                }
                else if ("ActorUnit" == reader.Name)
                    actor = pGetString();
                if ("TargetSpecies" == reader.Name)
                {
                    targetIsSpecies = true;
                    target = pGetString();
                }
                else if ("TargetUnit" == reader.Name)
                    target = pGetString();
                List<string> targetStates = null;
                if ("TargetStates" == reader.Name)
                {
                    targetStates = pGetStringList(commaRegex);
                }
                if ("UseDefault" == reader.Name)
                    useDefault = pGetBoolean();
                if ("EngramRange" == reader.Name)
                    engramRange = pGetEngramRange();
                if ("Failure" == reader.Name)
                    failure = pGetString();
                reader.ReadEndElement();
                return new pAttack_Request_Approval_Type(capability, actor, actorIsSpecies, target, targetIsSpecies, targetStates, useDefault, engramRange, failure);

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Error reading attackRequestApproval with capability " + capability + ": " + e.Message);

            }
        }
        public override pOpenVoiceChannelType pGetOpenVoiceChannel()
        {
            pOpenVoiceChannelType returnValue;
            string channel = "Unknown voice channel";
            List<string> accessList = new List<string>();
            int time;
            try
            {
                reader.Read();
                channel = pGetString();
                time = pGetInt();
                accessList = pGetStringList(commaRegex);
                returnValue = new pOpenVoiceChannelType(channel, time, accessList);
                //AD: Commented out because schema no longer dictates an Owner field.
                //if ("Owner" == reader.Name)
                //{
                //    returnValue.Owner = pGetString();
                //}

                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read OpenVoiceChannel for channel " + channel, e);
            }
        }

        public override pCloseVoiceChannelType pGetCloseVoiceChannel()
        {
            pCloseVoiceChannelType returnValue;
            string channel = "Unknown voice channel";
            int time;
            try
            {
                reader.Read();
                channel = pGetString();
                time = pGetInt();

                returnValue = new pCloseVoiceChannelType(channel, time);
                if ("Requestor" == reader.Name)
                    returnValue.Requestor = pGetString();
                reader.ReadEndElement();
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read CloseVoiceChannel for channel " + channel, e);
            }
        }
        public override pGrantVoiceChannelAccessType pGetGrantVoiceChannelAccess()
        {
            throw new Exception("The method or operation is not implemented.");

            /*
            pEngramRange engramRange = null;
            string decisionMaker="Unknown Decision Maker";
            string voiceChannel="Unknown Voice Channel";
            pGrantVoiceChannelAccessType returnValue;
            int time;
            try
            {
                reader.Read();
                if ("EngramRange" == reader.Name)
                    engramRange = pGetEngramRange();
                decisionMaker = pGetString();
                voiceChannel = pGetString();
                time = pGetInt();
                reader.ReadEndElement();
                return new pGrantVoiceChannelAccessType(engramRange, decisionMaker, voiceChannel, time);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not read GrantVoiceAccess for decision maker '" + decisionMaker + "' and channel='" + voiceChannel + "': " + e.Message);
            }
             */
        }
        public override pRemoveVoiceChannelAccessType pGetRemoveVoiceChannelAccess()
        {
            throw new Exception("The method or operation is not implemented.");

            /*
            pEngramRange engramRange = null;
            string decisionMaker = "Unknown Decision Maker";
            string voiceChannel = "Unknown Voice Channel";
            pRemoveVoiceChannelAccessType returnValue;
            int time;
            try
            {
                reader.Read();
                if ("EngramRange" == reader.Name)
                    engramRange = pGetEngramRange();
                decisionMaker = pGetString();
                voiceChannel = pGetString();
                time = pGetInt();
                reader.ReadEndElement();
                return new pRemoveVoiceChannelAccessType(engramRange, decisionMaker, voiceChannel, time);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Could not read RemoveVoiceAccess for decision maker '" + decisionMaker + "' and channel='" + voiceChannel + "': " + e.Message);
            }
             */
        }

        public override pSendChatMessageType pGetSendChatMessage()
        {
            string chatRoom = "Unknown Chat Room";
            string sender = "", message;
            int time;
            try
            {
                pSendChatMessageType returnValue;
                reader.Read();
                chatRoom = pGetString();
                if ("Sender" == reader.Name)
                    sender = pGetString();
                time = pGetInt();
                message = pGetString();
                reader.ReadEndElement();
                returnValue = new pSendChatMessageType(chatRoom, sender, time, message);
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read SendChatMessage for room " + chatRoom);
            }
        }

        public override pSendVoiceMessageType pGetSendVoiceMessage()
        {
            string channel = "Unknown Voice Channel";
            string fileName;
            int time;
            try
            {
                pSendVoiceMessageType returnValue;
                reader.Read();
                channel = pGetString();
                fileName = pGetString();
                time = pGetInt();
                reader.ReadEndElement();
                returnValue = new pSendVoiceMessageType(channel, fileName, time);
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read SendChatMessage for room " + channel);
            }
        }

        public override pSendVoiceMessageToUserType pGetSendVoiceMessageToUser()
        {
            string user = "Unknown User";
            string fileName;
            int time;
            try
            {
                pSendVoiceMessageToUserType returnValue;
                reader.Read();
                user = pGetString();
                fileName = pGetString();
                time = pGetInt();
                reader.ReadEndElement();
                returnValue = new pSendVoiceMessageToUserType(user, fileName, time);
                return returnValue;

            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read SendChatMessageToUser for user " + user);
            }
        }


        public override pApplyType pGetApply()
        {
            string fromDM = "Unknown DM";
            string toDM = "Unknown DM";
            int time;
            pEngramRange engramRange = null;
            pApplyType returnValue;

            try
            {
                reader.Read();
                if ("EngramRange" == reader.Name)
                    engramRange = pGetEngramRange();

                fromDM = pGetString();
                toDM = pGetString();
                time = pGetInt();
                returnValue = new pApplyType(engramRange, fromDM, toDM, time);
                return returnValue;
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not read Apply element.");
            }
        }

    }
}
