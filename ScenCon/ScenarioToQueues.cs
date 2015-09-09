using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Aptima.Asim.DDD.ScenarioParser;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
using Aptima.Asim.DDD.CommonComponents.ServerOptionsTools;

namespace Aptima.Asim.DDD.ScenarioController
{


    /* --------------------------ScenarioToQueues----------------------------------- */
    public class Defaults
    {
        //     public const int predefinedTimeToRemove = 3000;
        public const int predefinedTimeToAttack = 10000;


        private static int defaultTimeToAttack = Defaults.predefinedTimeToAttack;
        public static int DefaultTimeToAttack
        {
            get { return defaultTimeToAttack; }
            set { defaultTimeToAttack = value; }
        }
        /*      private static int defaultTimeToRemove = Defaults.predefinedTimeToRemove;
              public static int DefaultTimeToRemove
              {
                  get { return defaultTimeToRemove; }
                  set { defaultTimeToRemove = value; }
              }
       */
    }
    /// <summary>
    /// This parses a scenario according to the scenario schema
    /// and pushes Events onto the relevent queues
    /// </summary>
    public class ScenarioToQueues
    {
        /* set up structures to track sequencing of commands*/
 
       private static AbstractParser parser;
        private static XmlReader reader;

  //      private static int generatedId = 0;


        /// <summary>
        /// Resets DMSlotsFilled between scenario runs.
        /// </summary>
 /*
  * public static void Reset()
        {
            //     dMSlotsFilled = 0;
 //           generatedId = 0;
        }
*/
        /// <summary>
        /// Handles exceptions during schema validation
        /// </summary>
        /// <param name="sender">Object issuing exception</param>
        /// <param name="e">Exception</param>
        static void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            string envelope = "Error discovered in scenario validation is:" + e.Message;
            throw new ApplicationException(envelope);
        }

       // static string schemaFile; // set in constructor 
        static string namedSchemaFile;
        private static Boolean ValidateScenario(string scenarioFile)
        {
            Regex schemaFileRegex = new Regex("SchemaLocation=\"(.*?)\"");

            // First, find the schema file name in the scenario file;
            //string dddClientDir = String.Format("\\\\{0}\\DDDClient", System.Environment.MachineName) + "\\";
            string dddClientDir = "\\\\"+ServerOptions.DDDClientPath + "\\";// Path.GetDirectoryName(ServerOptions.ScenarioSchemaPath) + "\\";
            //string dddClientDir = ServerOptions.ScenarioSchemaPath + "\\";
            namedSchemaFile = "DDDSchema_4_0.xsd";//default setting
            string oneLine;
            try
            {
                StreamReader scenarioReader = new StreamReader(scenarioFile);
                while ((oneLine = scenarioReader.ReadLine()) != null)
                {
                    if (schemaFileRegex.IsMatch(oneLine))
                    {
                        Match m = schemaFileRegex.Match(oneLine);

                        namedSchemaFile = m.Groups[1].Value;
                        if (".xsd" != namedSchemaFile.Substring(namedSchemaFile.Length - 4).ToLower())
                            namedSchemaFile += ".xsd";
                        switch (namedSchemaFile)
                        {// 
                      
                            case "DDDSchema_4_0.xsd":
                                parser = new Parser_4_0();//DDD SP1
                                break;
                            case "DDDSchema_4_0_2.xsd":
                                parser = new Parser_4_0_2();//DDD 4.0 SP2
                                break;
                            case "DDDSchema_4_1.xsd":
                                parser = new Parser_4_1();
                                break;
                            case "DDDSchema_4_1_1.xsd":
                                parser = new Parser_4_1();
                                break;
                            case "DDDSchema_4_2.xsd":
                                parser = new Parser_4_2();
                                break;
                            default:
                                System.Windows.Forms.MessageBox.Show("Unknown Schema File: " + namedSchemaFile);
                                throw new ApplicationException("Unknown schema file: " + namedSchemaFile);
                        }
                        scenarioReader.Close();
                        break;
                    }
                }
            }
            catch (SystemException e)// only occurs for i/o errors, not for unfound schema name
            {
                System.Windows.Forms.MessageBox.Show("Error reading scenario file " + scenarioFile + ": " + e.Message);

                throw new ApplicationException("Error reading scenario file " + scenarioFile, e);
            }
/*
 * at this point
 * Have found the schema file (or there was none)
 * Have used schema file to choose appropriate parser
 */
            namedSchemaFile = dddClientDir + namedSchemaFile;
            Boolean returnValue = true;
            // Create the XmlNodeReader object.
            XmlDocument doc = new XmlDocument();
            doc.Load(scenarioFile);
            XmlNodeReader nodeReader = new XmlNodeReader(doc);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
          settings.Schemas.Add(null, namedSchemaFile);//+".xsd");
            settings.ValidationEventHandler += new ValidationEventHandler(SchemaValidationHandler);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            // Create a validating reader that wraps the XmlNodeReader object.
            XmlReader reader = XmlReader.Create(nodeReader, settings);
            parser.SetReader(reader);
            // Parse the XML file.
            try
            {
                while (reader.Read()) ;
            }
            catch (SystemException e)
            {
                returnValue = false;
                System.Windows.Forms.MessageBox.Show("Error in XML Validation:\n"
                    + " Scenario File: " + scenarioFile + "\n"
                    + "Schema File : " + namedSchemaFile + "\n"
                    + e.Message);
            }
            finally
            {
                nodeReader.Close();
                reader.Close();

            }
            return returnValue;


        }

 
        internal static void RevealDocked(string parentID, int time)
        {


            List<string> children = SubplatformRecords.GetDocked(parentID);
      
            for (int i = 0; i < children.Count; i++)
            {
                    string oneChild=children[i];
                    //need initaiState and genus (!=kind)
                    Reveal_EventType re = new Reveal_EventType(oneChild);



                    re.InitialState = "FullyFunctional";
                    re.InitialLocation = null;
                    re.Genus = Genealogy.GetGenus(oneChild);
                    re.IsWeapon = SpeciesType.GetSpecies(UnitFacts.Data[oneChild].Species).IsWeapon;
                    re.DockedToParent = true;

                    StateBody thisStartState = StatesForUnits.StateTable[Genealogy.GetBase(re.UnitID)][re.InitialState];
                    re.Parameters = thisStartState.Parameters.DeepCopy();
                    re.Parameters["ParentObjectID"] = parentID;

                    if (time > 1)
                    {
                        TimerQueueClass.Add(time, re);
                    }
                    else
                    {
                        TimerQueueClass.SecondarySendBeforeStartup(re);
                    }
                    Coordinator.debugLogger.Writeline("ScenarioToQueues", "revealEvent for " + re.UnitID, "test");
                    if (!re.IsWeapon)
                    {
                        RevealDocked(re.UnitID, time);
                    }

                }
            }
        
 
        /// <summary>
        /// Creates the class (one instance only) that parses a scenario and puts events on queues
        /// </summary>
        /// <param name="scenarioFile"></param>
        public ScenarioToQueues(string scenarioFile, string schemaFile)
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
//            ScenarioToQueues.schemaFile = schemaFile;

            //    try
            //  {
            returnVal = ValidateScenario(scenarioFile);
            // }
            /*    catch (System.Exception e)
                {

                    string message = "Failed to validate schema. " + e.Message;
                    Coordinator.debugLogger.LogException("Scenario Reader", message);
                    throw new ApplicationException(message, e);
                }*/

            if (!returnVal)
            {
                return;
            }


            try
            {
                fs = File.Open(scenarioFile, FileMode.Open);
            }
            catch (System.Exception e)
            {
                string message = "Could not open scenario file on pass 2." + e.Message;
                Coordinator.debugLogger.LogException("Scenario Reader", message);
                throw new ApplicationException(message, e);
            }

            /// XmlReader readerTwo;
            try
            {
                XmlSchemaSet sc = new XmlSchemaSet();
                sc.Add(null, ScenarioToQueues.namedSchemaFile);//+".xsd");
                XmlReaderSettings readerSettings = new XmlReaderSettings();

                // readerSettings.ValidationType = ValidationType.Schema;
                readerSettings.IgnoreWhitespace = true;
                // makes no difference ?!               readerSettings.IgnoreWhitespace = true;
                //   readerSettings.ValidationEventHandler += new ValidationEventHandler(SchemaValidationHandler);

                reader = XmlReader.Create(fs, readerSettings);
                parser.SetReader(reader);

            }
            catch (System.Exception e)
            {
                fs.Close();
                string message = "Could not open schema file. " + e.Message;
                Coordinator.debugLogger.LogException("Scenario Reader", message);
                throw new ApplicationException(message, e);

            }

            //Build reverse dictionary of commnds->stage
            //This lets us enforce moving through the commands in a prescribed order
            Dictionary<string, int> stages = new Dictionary<string, int>();
            string[][] stageMembers = parser.StageMembers();
            for (int level = 0; level < stageMembers.Length; level++)
            {
                for (int member = 0; member < stageMembers[level].Length; member++)
                {
                    stages[stageMembers[level][member]] = level;
                }
            }
            int currentStage = 0;
            string scenarioName = "";
            string description = "";


            reader.Read(); // opens, gets to xml declaration
            reader.Read();
            try
            {
                if ("Scenario" == reader.Name) // only Scenario can be top level
                {
                    reader.Read(); // pass by "Scenario"

                    while (!reader.EOF && !((XmlNodeType.EndElement == reader.NodeType) && ("Scenario" == reader.Name)))
                    {

                        //Coordinator.debugLogger.Writeline(".");

                        switch (reader.NodeType)
                        {

                            case XmlNodeType.Element:
                                Coordinator.debugLogger.Writeline("ScenarioToQueues", "ELEMENT " + reader.Name, "test");
                                if (stages.ContainsKey(reader.Name))
                                {
                                    if (stages[reader.Name] < currentStage) 
                                        throw new ApplicationException("Command " + reader.Name + " out of sequence.");
                                    currentStage = stages[reader.Name];
                                    //NB, if command is not found in stages it will be trapped in the switch's default
                                }
                                switch (reader.Name)
                                {

                                    case "ScenarioName":
                                        scenarioName = parser.pGetString();
                                        break;
                                    case "Description":
                                        description = parser.pGetString();
                                        break;
                                    case "ClientSideAssetTransfer":
                                        ClientSideAssetTransferType assetTransfers = new ClientSideAssetTransferType(parser.pGetBoolean());
                                        TimerQueueClass.SendBeforeStartup(assetTransfers);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " ClientSideAssetTransferType ", "test");
                                        break;
                                    case "ClientSideStartingLabelVisible":
                                        ClientSideStartingLabelVisibleType labelsVisible = new ClientSideStartingLabelVisibleType(parser.pGetBoolean());
                                        TimerQueueClass.SendBeforeStartup(labelsVisible);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " ClientSideStartingLabelVisibleType ", "test");
                                        break;
                                    case "ClientSideRangeRingVisibility":
                                        ClientSideRangeRingVisibilityType rangeRingVisibility = new ClientSideRangeRingVisibilityType(parser.pGetString());
                                        TimerQueueClass.SendBeforeStartup(rangeRingVisibility);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "ClientSideRangeRingVisibilityType", "test");
                                        break;
                                    case "Playfield":
                                        // except for the optional name/description
                                        // Playfield must be first scenario item sent
                                        // enforced by schema
                                        string errorText = string.Empty;
                                        string imgLibPath = string.Empty;
                                        string mapFilePath = string.Empty;
                                        string hostname = Aptima.Asim.DDD.CommonComponents.ServerOptionsTools.ServerOptions.HostName;
                                        pPlayfieldType pDefine = parser.pGetPlayfield();
                                        PlayfieldEventType playfield = new PlayfieldEventType(pDefine);
                                        playfield.ScenarioName = scenarioName;
                                        playfield.Description = description;
                                        if (playfield.IconLibrary.EndsWith(".dll"))
                                        {
                                            imgLibPath = String.Format(@"\\{0}\{1}", ServerOptions.DDDClientPath, playfield.IconLibrary); //icon library does include dll extension
                                        }
                                        else
                                        {
                                            imgLibPath = String.Format(@"\\{0}\{1}.dll", ServerOptions.DDDClientPath, playfield.IconLibrary); //icon library doesnt include dll extension
                                        }
                                        mapFilePath = String.Format(@"\\{0}\MapLib\{1}", ServerOptions.DDDClientPath, playfield.MapFileName); //map file includes file extension

                                        //check image library path
                                        if (!File.Exists(imgLibPath))
                                        {
                                            if (System.Windows.Forms.MessageBox.Show(String.Format("The server was not able to locate the image library in {0}.  There is a chance that clients connecting to this machine will have a similar issue locating this file.  If you'd still like to run the simulation, click Yes.  If you'd like to stop the server, click No.", imgLibPath), "File not found", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                            {//User wants to continue

                                            }
                                            else
                                            { //user wants to stop server
                                                throw new Exception("User Cancelled server process.  Server was unable to locate the image library at " + imgLibPath);
                                                return;
                                            }
                                        }

                                        //check map file path
                                        if (!File.Exists(mapFilePath))
                                        {
                                            if (System.Windows.Forms.MessageBox.Show(String.Format("The server was not able to locate the map file in {0}.  There is a chance that clients connecting to this machine will have a similar issue locating this file.  If you'd still like to run the simulation, click Yes.  If you'd like to stop the server, click No.", mapFilePath), "File not found", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                            {//User wants to continue

                                            }
                                            else
                                            { //user wants to stop server
                                                throw new Exception("User Cancelled server process.  Server was unable to locate the map file at " + mapFilePath);
                                                return;
                                            }
                                        }
                                        TimerQueueClass.SendBeforeStartup(playfield);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "Playfield", "test");
                                        
                                        TimerQueueClass.SendBeforeStartup(new RandomSeedType());
                                        break;
                                    case "LandRegion":
                                        pLandRegionType region = parser.pGetLandRegion();
                                        RegionEventType regionEvent = new RegionEventType(region);

                                        TimerQueueClass.SendBeforeStartup(regionEvent);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " LandRegion " + regionEvent.UnitID, "test");

                                        break;
                                    /*
                                case "ScoringRegion":
                                    pScoringRegionType regionS = parser.pGetScoringRegion();
                                    RegionEventType regionEventS = new RegionEventType(regionS);
                                    TimerQueueClass.SendBeforeStartup(regionEventS);
                                    Coordinator.debugLogger.Writeline("ScenarioToQueues", " ScoringRegion " + regionEventS.UnitID, "test");
                                    break;
                                    */
                                    case "ActiveRegion":
                                        pActiveRegionType regionA = parser.pGetActiveRegion();
                                        RegionEventType regionEventA = new RegionEventType(regionA);
                                        if (!NameLists.activeRegionNames.New(regionA.ID, regionEventA)) throw new ApplicationException("Duplicate active region name " + regionA.ID);
                                        TimerQueueClass.SendBeforeStartup(regionEventA);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " ActiveRegion " + regionEventA.UnitID, "test");
                                        break;
                                    case "Team":
                                        pTeamType team = parser.pGetTeam();
                                        TeamType teamEvent = new TeamType(team);
                                        for (int i = 0; i < teamEvent.Count(); i++)
                                        {
                                            UnitFacts.AddEnemy(teamEvent.Name, teamEvent.GetEnemy(i));

                                        }
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " Team " + team.Name.ToString(), "test");
                                        if (!NameLists.teamNames.New(team.Name, teamEvent)) throw new ApplicationException("Duplicate team name " + team.Name);

                                        TimerQueueClass.SendBeforeStartup(teamEvent);
                                        break;
                                    case "DecisionMaker":
                                        DecisionMakerType decisionMaker = new DecisionMakerType(parser.pGetDecisionMaker());
                                        UnitFacts.AddDM(decisionMaker.Identifier, decisionMaker.Chroma, decisionMaker.Team);
                                        TimerQueueClass.SendBeforeStartup(decisionMaker);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "Decision Maker ", "test");
                                        break;
                                    case "Network":
                                        NetworkType network = new NetworkType(parser.pGetNetwork());
                                        NetworkTable.AddNetwork(network);
                                        TimerQueueClass.SendBeforeStartup(network);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "Network ", "test");
                                        break;


                                    case "Sensor":
                                        //sensor follows Playfield
                                        pSensor sen = parser.pGetSensor();
                                        // if Attribute is "all" there will be one cone; only extent is filled in from parser
                                        if (sen.Attribute == "All")
                                        {
                                            sen.Cones[0].Spread = 360;
                                            sen.Cones[0].Level = "Total";
                                        }
                                        SensorTable.Add(sen.Name, new SensorType(sen.Attribute, sen.IsEngram, sen.TypeIfEngram, sen.Cones));

                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " Sensor " + sen.Name.ToString(), "test");

                                        break;
                                    /*    case "TimeToRemove":
                                                Defaults.DefaultTimeToRemove = parser.pGetInt();
                                                break;
                                     */
                                    case "Classifications":
                                        ClassificationsType classifications = new ClassificationsType(parser.pGetClassifications());
                                        TimerQueueClass.SendBeforeStartup(classifications);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " Classifications ", "test");
                                        break;
                                    case "TimeToAttack":
                                        Defaults.DefaultTimeToAttack = parser.pGetInt()*1000;
                                        break;

                                    case "Genus":
                                        pGenusType g = parser.pGetGenus();
                                        if (!NameLists.speciesNames.New(g.Name, g))
                                            throw new ApplicationException("Duplicate use of genus name " + g.Name);
  
                                        Genealogy.Add(g.Name);


                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "Genus " + g.Name, "test");
                                        // StartupForUnits.Add(genus);
                                        break;

                                    case "Species":
                                        // Genus and species come after playfield


                                        pSpeciesType s = parser.pGetSpecies();
                                        SpeciesType species = new SpeciesType(s);
                                        if (!NameLists.speciesNames.New(species.Name, species))
                                            throw new ApplicationException("Duplicate use of species name " + species.Name);
                                        Genealogy.Add(species.Name, species.BasedOn);
                                        if (species.IsWeapon)
                                        {
                                            WeaponTable.Add(species.Name);
                                        }

                                        WorkStates.Clear();
                                        WorkStates.Add(species.States);
                                        StatesForUnits.AddStatesOf(species.Name, WorkStates.CollapseStates(species.Name, species.BasedOn));
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "Species " + s.Name, "test");
                                        // StartupForUnits.Add(species);
                                        break;

                                    case "Create_Event":
                                        //string newId;
                                     //   Create_EventType platformCreate;//used for sub platforms
                                        pCreateType c = parser.pGetCreate();
                                        Create_EventType createEvent = new Create_EventType(c);
                                        Genealogy.Add(createEvent.UnitID, createEvent.UnitBase);
                                        createEvent.Genus = Genealogy.GetGenus(createEvent.UnitID);
                                        UnitFacts.AddUnit(createEvent.UnitID, createEvent.Owner,createEvent.UnitBase);
                                        createEvent.Parameters = new ObjectDictionary();

                                        SpeciesType speciesInfo = (SpeciesType)NameLists.speciesNames[createEvent.UnitBase];

                                        foreach (KeyValuePair<string, StateBody> kvp in StatesForUnits.StateTable[createEvent.UnitBase])
                                        {
                                            string stateName = kvp.Key;
                                            ExtendedStateBody extended = new ExtendedStateBody(kvp.Value);
                                            
                                            //end species dependent atts
                                            createEvent.Parameters.Add(stateName, (object)extended);
                                            //               EngramDependants.Add(createEvent.UnitID, extended);

                                           
                                        }

                                        


                                        //       TimerQueueClass.Add(1, createEvent);
                                        NameLists.unitNames.New(createEvent.UnitID, null);
                                        TimerQueueClass.SecondarySendBeforeStartup(createEvent);
                                        UnitFacts.CurrentUnitStates.Add(createEvent.UnitID, "");
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "createEvent for " + createEvent.UnitID, "test");
                                  
                                        //for some reason this docs everyone at time 1.  this seems to be the heart of the bug
                                        /*
                                        for (int i = 0; i < createEvent.Subplatforms.Count;i++ )
                                   {
                                       SubplatformDockType dockNotice = new SubplatformDockType(createEvent.Subplatforms[i], createEvent.UnitID);
                                       dockNotice.Time = 1;
                                       TimerQueueClass.SecondarySendBeforeStartup(dockNotice);

                                   }
                                         */
                                   break;
                                    case "Reveal_Event":
                                        pRevealType r = parser.pGetReveal();
                                        Reveal_EventType revealEvent = new Reveal_EventType(r);
                                        revealEvent.Genus = Genealogy.GetGenus(revealEvent.UnitID);
                                        if (r.Time > 1)
                                        {
                                            TimerQueueClass.Add(r.Time, revealEvent);
                                        }
                                        else
                                        {
                                            TimerQueueClass.SecondarySendBeforeStartup(revealEvent);
                                        }
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", "revealEvent for  " + revealEvent.UnitID + " at time" + r.Time.ToString(), "test");
                                        RevealDocked(r.UnitID, r.Time);
                                        break;

                                    case "Move_Event":
                                        pMoveType m = parser.pGetMove();
                                        if (!UnitFacts.IsAUnit(m.UnitID)) throw new ApplicationException("Cannot move non-exsitant unit " + m.UnitID);
                                        Move_EventType moveEvent = new Move_EventType(m);
                                        TimerQueueClass.Add(moveEvent.Time, moveEvent);
                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " moveEvent for  " + moveEvent.UnitID, "test");
                                        break;
                                    case "Completion_Event":
                                        HappeningCompletionType completionType = new HappeningCompletionType(parser.pGetHappeningCompletion());
                                        HappeningList.Add(completionType);
                                        break;
                                    case "Species_Completion_Event":
                                        pSpeciesCompletionType sct = parser.pGetSpeciesCompletion();
                                        SpeciesCompletionType speciesCompletion = new SpeciesCompletionType(sct);
                                        SpeciesHappeningList.Add(speciesCompletion);
                                        break;
                                    case "Reiterate":
                                        ReiterateType reiterate = new ReiterateType(parser.pGetReiterate());
                                        Move_EventType mQueued = (Move_EventType)(reiterate.ReiterateList[0]);
                                        Move_EventType mQCopy = (Move_EventType)(reiterate.ReiterateList[0]);
                                        mQueued.Range = reiterate.Range;
                                        mQueued.Time = reiterate.Time;
                                        TimerQueueClass.Add(mQueued.Time, mQueued);

                                        Coordinator.debugLogger.Writeline("ScenarioToQueues", " moveEvent from reiterate for  " + mQueued.UnitID, "test");
                                        reiterate.ReiterateList.RemoveAt(0);
                                        reiterate.ReiterateList.Add(mQCopy);
                                        HappeningCompletionType envelope = new HappeningCompletionType(reiterate);
                                        HappeningList.Add(envelope);
                                        break;
                                    case "StateChange_Event":
                                        StateChangeEvent change = new StateChangeEvent(parser.pGetStateChange());
                                        if (!UnitFacts.IsAUnit(change.UnitID)) throw new ApplicationException("State change event references unknown unit " + change.UnitID);
                                        if (!StatesForUnits.UnitHasState(change.UnitID, change.NewState)) throw new ApplicationException("State chage for " + change.UnitID + " refers to unknwon state " + change.NewState);
                                        for (int i = 0; i < change.Except.Count; i++)
                                        {
                        //                    if (!StatesForUnits.UnitHasState(Genealogy.GetBase(change.UnitID), change.Except[i])) throw new ApplicationException("State change for " + change.UnitID + " refers to unknown state " + change.Except);
                                            if (!StatesForUnits.UnitHasState(change.UnitID, change.Except[i])) throw new ApplicationException("State change for " + change.UnitID + " refers to unknown state " + change.Except);
                            
                                        }
                                        for (int i = 0; i < change.From.Count; i++)
                                        {
                            //                if (!StatesForUnits.UnitHasState(Genealogy.GetBase(change.UnitID), change.From[i])) throw new ApplicationException("State change for " + change.UnitID + " refers to unknown state " + change.From);
                                            if (!StatesForUnits.UnitHasState(change.UnitID, change.From[i])) throw new ApplicationException("State change for " + change.UnitID + " refers to unknown state " + change.From);
                               
                                        }
                                        TimerQueueClass.Add(change.Time, change);
                                        break;
                                    case "Transfer_Event":
                                        TransferEvent t = new TransferEvent(parser.pGetTransfer());

                                        if (!UnitFacts.IsDM(t.From)) throw new ApplicationException("Transfer event references unknown DM (from) " + t.From);
                                        if (!UnitFacts.IsDM(t.To)) throw new ApplicationException("Transfer event references unknown DM (to) " + t.To);
                                        if (!UnitFacts.IsAUnit(t.UnitID)) throw new ApplicationException("Transfer event references unknown unit " + t.UnitID);
                                        TimerQueueClass.Add(t.Time, t);
                                        break;
                                    case "Launch_Event":
                                        LaunchEventType launch = new LaunchEventType(parser.pGetLaunch());
                                        if (!UnitFacts.IsAUnit(launch.UnitID)) throw new ApplicationException("Cannot launch from non-existent unit " + launch.UnitID);
                                        TimerQueueClass.Add(launch.Time, launch);


                                        break;
                                    case "WeaponLaunch_Event":

                                        WeaponLaunchEventType weaponLaunch = new WeaponLaunchEventType(parser.pGetWeaponLaunch());
                                        if (!UnitFacts.IsAUnit(weaponLaunch.UnitID)) throw new ApplicationException("Cannot launch from non-existent unit " + weaponLaunch.UnitID);
                                        TimerQueueClass.Add(weaponLaunch.Time, weaponLaunch);


                                        break;
                                    case "DefineEngram":
                                        pDefineEngramType defineEngram = parser.pGetDefineEngram();
                                        Engrams.Create(defineEngram.Name, defineEngram.EngramValue, defineEngram.Type);
                                        TimerQueueClass.SendBeforeStartup(new EngramSettingType(defineEngram.Name, "", defineEngram.EngramValue, defineEngram.Type));
                                        break;
                                    case "ChangeEngram":
                                        ChangeEngramType changeEngram = new ChangeEngramType(parser.pGetChangeEngram());
                                        if (!Engrams.ValidUpdate(changeEngram.Name, changeEngram.EngramValue))
                                            throw new ApplicationException("Illegal value " + changeEngram.EngramValue + " for engram " + changeEngram.Name);
                                        TimerQueueClass.Add(changeEngram.Time, changeEngram);
                                        //  Engrams.SendUpdate(changeEngram.Name);
                                        break;
                                    case "RemoveEngram":
                                        RemoveEngramEvent removeEngram = new RemoveEngramEvent(parser.pGetRemoveEngram());
                                        TimerQueueClass.Add(removeEngram.Time, removeEngram);
                                        break;
                                    //These chat commands are those from the scenario only;
                                    // thos from client are handled immediately
                                    case "OpenChatRoom":
                                        OpenChatRoomType openChatRoom = new OpenChatRoomType(parser.pGetOpenChatRoom());
                                        TimerQueueClass.Add(openChatRoom.Time, openChatRoom);
                                        break;
                                    case "CloseChatRoom":
                                        CloseChatRoomType closeChatRoom = new CloseChatRoomType(parser.pGetCloseChatRoom());

                                        TimerQueueClass.Add(closeChatRoom.Time, closeChatRoom);
                                        break;
                                    /* Not implemented yet 
                                       case "DropChatters":
                                           DropChattersType dropChatters = new DropChattersType(parser.pGetDropChatters());
                                           TimerQueueClass.Add(dropChatters.Time, dropChatters);
                                           break;
                                       case "AddChatters":
                                           AddChattersType addChatters = new AddChattersType(parser.pGetAddChatters());
                                           TimerQueueClass.Add(addChatters.Time, addChatters);
                                           break;

                                   */
                                    case "OpenWhiteboardRoom":
                                        OpenWhiteboardRoomType openWhiteboardRoom = new OpenWhiteboardRoomType(parser.pGetOpenWhiteboardRoom());
                                        TimerQueueClass.Add(openWhiteboardRoom.Time, openWhiteboardRoom);
                                        break;
                                    case "OpenVoiceChannel":
                                        OpenVoiceChannelType openVoiceChannel = new OpenVoiceChannelType(parser.pGetOpenVoiceChannel());
                                        TimerQueueClass.Add(openVoiceChannel.Time, openVoiceChannel);
                                        break;
                                    case "CloseVoiceChannel":
                                        CloseVoiceChannelType closeVoiceChannel = new CloseVoiceChannelType(parser.pGetCloseVoiceChannel());
                                        TimerQueueClass.Add(closeVoiceChannel.Time, closeVoiceChannel);
                                        break;
                                        /*
                                         //Removed before 4.1
                                    case "GrantVoiceChannelAccess":
                                        GrantVoiceAccessType grantVoiceChannelAccess = new GrantVoiceAccessType(parser.pGetGrantVoiceChannelAccess());
                                        TimerQueueClass.Add(grantVoiceChannelAccess.Time, grantVoiceChannelAccess);
                                        break;
                                    case "RemoveVoiceChannelAccess":
                                        RemoveVoiceAccessType removeVoiceChannelAccess = new RemoveVoiceAccessType(parser.pGetRemoveVoiceChannelAccess());
                                        TimerQueueClass.Add(removeVoiceChannelAccess.Time, removeVoiceChannelAccess);
                                        break;
                                        */
                                    case "Rule":
                                        pScoringRuleType srt = parser.pGetScoringRule();
                                        ScoringRuleType scoreRule = new ScoringRuleType(srt);
                                        if (!NameLists.ruleNames.New(srt.Name, scoreRule)) throw new ApplicationException("Duplicate scoring rule name " + srt.Name);
                                        ScoringRules.Add(scoreRule);
                                        break;
                                    case "Score":
                                        pScoreType pst = parser.pGetScore();
                                        ScoreType st = new ScoreType(pst);
                                        if (!NameLists.scoreNames.New(pst.Name, st)) throw new ApplicationException("Duplicate score name " + pst.Name);
                                        Scores.Register(st);
                                        break;

                                    case "FlushEvents":
                                        FlushEvents flush = new FlushEvents(parser.pGetFlushEventsType());
                                        TimerQueueClass.Add(flush.Time, flush);
                                        break;

                                    case "SendChatMessage":
                                        SendChatMessageType sendChat = new SendChatMessageType(parser.pGetSendChatMessage());
                                        if (!UnitFacts.IsDM(sendChat.Sender) && !("EXP" == sendChat.Sender))
                                            throw new ApplicationException("In SendChatMessage, '" + sendChat.Sender + "' is not a valid DM name.");
                            // Note: Can't validate chat room name at parse time; it might not have been created yet
                                        TimerQueueClass.Add(sendChat.Time,sendChat);
                                        break;
                                    case "Apply":
                                        ApplyType apply = new ApplyType(parser.pGetApply());
                                        //what does this do?
                                        TimerQueueClass.Add(apply.Time, apply);
                                        break;
                                    case "SendVoiceMessage":
                                        SendVoiceMessageType playVoiceMessage = new SendVoiceMessageType(parser.pGetSendVoiceMessage());
                                        //what does this do?
                                        TimerQueueClass.Add(playVoiceMessage.Time, playVoiceMessage);
                                        break;
                                    case "SendVoiceMessageToUser":
                                        SendVoiceMessageToUserType playVoiceMessageToUser = new SendVoiceMessageToUserType(parser.pGetSendVoiceMessageToUser());
                                        //what does this do?
                                        TimerQueueClass.Add(playVoiceMessageToUser.Time, playVoiceMessageToUser);
                                        break;
                                    default:
                                        throw new ApplicationException("ScenarioToQueues: Unknown Scenario Element is *" + reader.Name);


                                }//switch
                                break;
                            default:
                                Coordinator.debugLogger.Writeline("ScenarioToQueues", "Unhandled or out-of-sequence XML tag " + reader.Value, "test");
                                reader.Read();
                                break;



                        } //switch

                    } //while
                    // All of scenario processed. Now do last things

                    //verify that the Chat, Whiteboard, and Voice lists contain only DMs and make them symmetric.
                    List<string> allDMs = UnitFacts.GetAllDms();
                    for (int i = 0; i < allDMs.Count; i++)
                    {
                        DecisionMakerType thisDM = DecisionMakerType.GetDM(allDMs[i]);

                        for (int j = 0; j < thisDM.ChatPartners.Count; j++)
                        {
                            if (!allDMs.Contains(thisDM.ChatPartners[j]))
                                throw new ApplicationException("Unknown decision maker name '" + thisDM.ChatPartners[j] + "' found as chat partner of '" + allDMs[i] + "'");
                            DecisionMakerType partnerDM = DecisionMakerType.GetDM(thisDM.ChatPartners[j]);
                            if (!partnerDM.ChatPartners.Contains(allDMs[i]))
                                partnerDM.MayChatWith(allDMs[i]);
                        }

                        for (int j = 0; j < thisDM.WhiteboardPartners.Count; j++)
                        {
                            if (!allDMs.Contains(thisDM.WhiteboardPartners[j]))
                                throw new ApplicationException("Unknown decision maker name '" + thisDM.WhiteboardPartners[j] + "' found as whiteboard partner of '" + allDMs[i] + "'");
                            DecisionMakerType partnerDM = DecisionMakerType.GetDM(thisDM.WhiteboardPartners[j]);
                            if (!partnerDM.WhiteboardPartners.Contains(allDMs[i]))
                                partnerDM.MayWhiteboardWith(allDMs[i]);
                        }

                        for (int j = 0; j < thisDM.VoicePartners.Count; j++)
                        {
                            if (!allDMs.Contains(thisDM.ChatPartners[j]))
                                throw new ApplicationException("Unknown decision maker name '" + thisDM.VoicePartners[j] + "' found as voice partner of '" + allDMs[i] + "'");
                            DecisionMakerType partnerDM = DecisionMakerType.GetDM(thisDM.VoicePartners[j]);
                            if (!partnerDM.VoicePartners.Contains(allDMs[i]))
                                partnerDM.MaySpeakWith(allDMs[i]);
                        }

                    }

                    // Add networks for DMs that have none
                    List<string> dmList = UnitFacts.GetAllDms();
                    for (int nextDM = 0; nextDM < dmList.Count; nextDM++)
                    {
                        if (!NetworkTable.IsNetworkMember(dmList[nextDM]))
                        {
                            string netName = "Network-For-" + dmList[nextDM];
                            NetworkTable.AddMember(netName, dmList[nextDM]);
                            NetworkType newNet = new NetworkType(netName);
                            newNet.Add(dmList[nextDM]);
                            TimerQueueClass.SendBeforeStartup(newNet);
                            Coordinator.debugLogger.Writeline("ScenarioToQueues", "Network ", "test");

                        }
                    }
                    //AD: Don't create default chat room, user should have control of this
                    // Create the detault Broadcast chatroom
                    //OpenChatRoomType broadcastChatRoom = new OpenChatRoomType(1, "", "Broadcast", dmList);
                    //TimerQueueClass.SendBeforeStartup(broadcastChatRoom);

                }//if
            }
            catch (System.Exception e)
            {
                if (e.Message.StartsWith("User Cancelled"))
                {//This means a missing map or icon library, and the user wanted to stop the server.  Do not write to error log, just stop the server. 
                    throw e;
                }
                string message = "Failure in Parsing Control for next tag=" + reader.Name + " : " + e.Message;
                Coordinator.debugLogger.LogException("ScenarioReader", message);
                throw new ApplicationException(message, e);
            }
            finally
            {
                reader.Close();
                fs.Close();
                // Coordinator.debugLogger.Writeline("ScenarioToQueues", "Done", "general");
            }
        }//
    }

}

