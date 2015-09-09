using System;
using System.Collections.Generic;
using System.Drawing;
using Aptima.Asim.DDD.ScenarioParser;
using Aptima.Asim.DDD.ScenarioController.Geometry;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.ScenarioController
{

    /// This module tracks data used in building and interpreting events

    /*
     * IMPORTANT
     * The constructors that take scenario Event types as arguments only 'deepcopy' fields "known" to be changeable. 
     * */

    /// <summary>
    /// Root events are the base for all events; the contain a time tick value.
    /// There is no constructor.
    /// </summary>
    //  ----------------------- RootEventType -----------------------
    public class RootEventType
    { // just exists to provide a common type for all elements on the queue
        private int time = 1;
        public int Time
        {
            get
            { return this.time; }
            set { this.time = value; }
        }
        /// <summary>
        /// Tells whether this event is "about" this unit 
        /// </summary>
        /// <param name="unitID">unit identifier</param>
        /// <returns>True if unit is a predicate of event</returns>
        public virtual Boolean Involves(string unitID)
        {
            return false;
        }

    }

    public class EngramSettingType : RootEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private string unit = "";
        public string Unit
        {
            get { return unit; }
        }
        private string value;
        public string Value
        {
            get { return value; }
        }
        private string type;
        public string Type
        {
            get { return type; }
        }
        public EngramSettingType(string name, string unit, string value, string type)
        {
            this.name = name;
            this.unit = unit;
            this.value = value;
            this.type = type;
        }
    }


    public class StartupCompleteNotice : RootEventType
    {
        public StartupCompleteNotice()
        {
            this.Time = 0;
        }
    }
    /// <summary>
    /// Tick events just specify the timer value for the tick.
    /// </summary>
    public class TickEventType : RootEventType
    {
        private string simulationTime;
        public string SimulationTime
        {
            get { return simulationTime; }
        }
        /// <summary> 
        /// Construct a tick event
        /// </summary>
        /// <param name="timer">Timer value</param>
        public TickEventType(int timer, string clockTime)
        {
            this.Time = timer;
            this.simulationTime = clockTime;
        }
    }



    public class PlayfieldEventType : RootEventType
    {
        private string mapFileName;
        public string MapFileName
        {
            get
            { return this.mapFileName; }
            set
            { mapFileName = value; }
        }
        private string iconLibrary;
        public string IconLibrary
        {
            get { return iconLibrary; }
            set { iconLibrary= value; }
        }
        private string uTMZone;
        public string UTMZone
        {
            get
            { return this.uTMZone; }
            set
            { this.uTMZone = value; }
        }
        private double verticalScale;
        public double VerticalScale
        {
            get
            { return this.verticalScale; }
            set
            { verticalScale = value; }
        }
        private double horizontalScale;
        public double HorizontalScale
        {
            get
            { return this.horizontalScale; }
            set
            { horizontalScale = value; }
        }
        private string scenarioName = "";
        public string ScenarioName
        {
            get { return scenarioName; }
            set { scenarioName = value; }
        }
        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string displayTags = "";
        public string DisplayTags
        {
            get { return displayTags; }
            set { displayTags = value; }
        }
        private string displayLabels = "";
        public string DisplayLabels
        {
            get { return displayLabels; }
            set { displayLabels = value; }
        }

        /// <summary>
        /// Create the playfield description (Override 1)
        /// </summary>
        /// <param name="mapFileName">NAme/identifier for map meta file</param>
        /// <param name="uTMZone">UTMZone of lower left of map</param>
        /// <param name="northing">Distance north from lower left</param>
        /// <param name="easting">Distance east from lower left</param>
        public PlayfieldEventType(string mapFileName, string iconLibrary, string uTMZone, double verticalScale, double horizontalScale, string displayLabels, string displayTags)
        {
            this.mapFileName = mapFileName;
            this.iconLibrary = iconLibrary;
            this.uTMZone = uTMZone;
            this.verticalScale = verticalScale;
            this.horizontalScale = horizontalScale;
            this.displayLabels = displayLabels;
            this.displayTags = displayTags;
        }
        public PlayfieldEventType(pPlayfieldType p)
        {
            this.mapFileName = p.MapFileName;
            this.iconLibrary = p.IconLibrary;
            this.uTMZone = p.UTMZone;
            this.verticalScale = p.VerticalScale;
            this.horizontalScale = p.HorizontalScale;
            this.displayLabels = p.DisplayLabels;
            this.displayTags = p.DisplayTags;
        }
    }

    public class TeamType : RootEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private List<string> against;
        public string this[int i]
        {
            get
            {
                if (i < against.Count)
                {
                    return against[i];
                }
                else
                {
                    return "";
                }
            }
        }
        public int Count()
        {
            return against.Count;
        }
        public string GetEnemy(int i)
        {
            return against[i];
        }

        public TeamType(pTeamType t)
        {
            name = t.Name;
            against = new List<string>();
            for (int i = 0; i < t.Count(); i++)
            {
                against.Add(t[i]);
            }
        }

    }
    /// <summary>
    /// Identification of a decision maker
    /// </summary>
    public class DecisionMakerType : RootEventType
    {
        private static Dictionary<string, DecisionMakerType> definedDMs = new Dictionary<string, DecisionMakerType>();
        public static DecisionMakerType GetDM(string s)
        {
            DecisionMakerType returnValue;
            try
            {
                returnValue = definedDMs[s];
                return returnValue;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error retrieving DM '" + s + "': " + e.Message);
            }
        }
        public static Boolean IsExistingDM(string s)
        {
            return definedDMs.ContainsKey(s);
        }
        private string role;
        public string Role
        {
            get { return role; }
        }
        private string identifier;
        public string Identifier
        {
            get { return identifier; }
        }
        private int chroma;
        public int Chroma
        {
            get { return chroma; }
        }
        private string briefing;
        public string Briefing
        {
            get { return briefing; }
        }
        public Boolean CanOwn(string unit)
        {
            SpeciesType species = SpeciesType.GetSpecies(UnitFacts.GetSpecies(unit));
            //return species.CanOwnMe(UnitFacts.GetDM(unit)); //checks that the unit's CURRENT owner can own the species...
            return species.CanOwnMe(Identifier); //Checks that THIS DM can own this species
        }

        /// <summary>
        /// Does the argument report to this object where someone along the way has forced authority?
        /// </summary>


        private List<string> directSupervisors = new List<string>();
        private static Dictionary<string, List<string>> supervises = new Dictionary<string, List<string>>();

        public void AddDirectSupervisor(string s)
        {
            directSupervisors.Add(s);
            if (!supervises.ContainsKey(s))
                supervises[s] = new List<string>();
            supervises[s].Add(this.identifier);
        }

        public List<string> Supervisors
        {
            get { return directSupervisors; }
        }

        private Boolean hasTransferAuthority;
        public Boolean HasTransferAuthority
        {
            get { return hasTransferAuthority; }
        }
        public static Boolean CanTransfer(string s)
        {
            return IsExistingDM(s) && GetDM(s).hasTransferAuthority;

        }
        private Boolean hasForcedTransferAuthority;
        public Boolean HasForcedTransferAuthority
        {
            get { return hasForcedTransferAuthority; }
        }
        public Boolean CanForceTransferOf(DecisionMakerType d)
        {
            return hasForcedTransferAuthority && (null != d) && d.directSupervisors.Contains(this.Identifier);

        }
      private string team;
        public string Team
        {
            get { return team; }
        }

        private List<string> chatPartners = new List<string>();
        public List<string> ChatPartners
        {
            get { return chatPartners; }
        }
        private List<string> whiteboardPartners = new List<string>();
        public List<string> WhiteboardPartners
        {
            get { return whiteboardPartners; }
        }
        private List<string> voicePartners = new List<string>();
        public List<string> VoicePartners
        {
            get { return voicePartners; }
        }
        // Since DMs being added may not exist, there is no way to force symmetry,
        // Hence, will do a sweep before runtime to remove ringers and force symmetry
        /// <summary>
        /*
          //removed prior to 4.1 testing.
        private List<string> voiceChannelAccess = new List<string>();
        public List<string> VoiceChannelAccess
        {
            get { return voiceChannelAccess; }
        }
        public void AddVoiceChannelAccess(List<string> vCL)
        {
            for (int i = 0; i < vCL.Count; i++)
                if (!voiceChannelAccess.Contains(vCL[i]))
                voiceChannelAccess.Add(vCL[1]);
        }
        public void AddVoiceChannelAccess(string vC)
        {
            if (!voiceChannelAccess.Contains(vC))
                voiceChannelAccess.Add(vC);
        }
         public void RemoveVoiceChannelAccess(string vC)
        {
            if (voiceChannelAccess.Contains(vC))
                voiceChannelAccess.Remove(vC);
        }
        public static List<string> GetVoiceChannelMembers(string voiceChannelName)
        {
            List<string>returnValue=new List<string>();
            foreach (string key in definedDMs.Keys)
                if (definedDMs[key].VoiceChannelAccess.Contains(voiceChannelName))
                    returnValue.Add(key);
            return returnValue;
        }
         */ 

        /// <summary>
        /// States that this DM can use chat with <argument>
        /// </summary>
        /// <param name="dM">DM to talk with</param>
        public void MayChatWith(string dM)
        {
            if (!chatPartners.Contains(dM))
                chatPartners.Add(dM);
        }
        /// <summary>
        ///  reports whteher this Dm can be in a chat network with <argument>
        /// </summary>
        /// <param name="dM">DM being wondered about</param>
        /// <returns></returns>
        public Boolean CanChatWith(string dM)
        {
            return chatPartners.Contains(dM);
        }
        /// <summary>
        /// States that this DM can use whiteboard with <argument>
        /// </summary>
        /// <param name="dM">DM to talk with</param>
        public void MayWhiteboardWith(string dM)
        {
            if (!whiteboardPartners.Contains(dM))
                whiteboardPartners.Add(dM);
        }
        /// <summary>
        ///  reports whteher this Dm can be in a whiteboard session with <argument>
        /// </summary>
        /// <param name="dM">DM being wondered about</param>
        /// <returns></returns>
        public Boolean CanWhiteboardWith(string dM)
        {
            return whiteboardPartners.Contains(dM);
        }

        /// <summary>
        /// States that this DM can use voice with <argument>
        /// </summary>
        /// <param name="dM">DM to talk with</param>
        public void MaySpeakWith(string dM)
        {
            if (!voicePartners.Contains(dM))
                voicePartners.Add(dM);
        }
        /// <summary>
        ///  reports whteher this Dm can be in a voice network with argumenyt
        /// </summary>
        /// <param name="dM">DM being wondered about</param>
        /// <returns></returns>
        public Boolean CanSpeakWith(string dM)
        {
            return voicePartners.Contains(dM);
        }


        Boolean isObserver;
        public Boolean IsObserver
        {
            get
            {
                return isObserver;
            }
        }

        /// <summary>
        /// Event identifying decision maker
        /// </summary>
        /// <param name="role">Functional role taken by this player</param>
        /// <param name="identification">Identification for this player</param>
        /*        public DecisionMakerType(string role, string identifier)
                {
                    this.dMRole = role;
                    this.dMIdentifier = identifier;
                }
         */
        public static void ClearDMTable()
        {
            definedDMs = new Dictionary<string, DecisionMakerType>();
        }
        public DecisionMakerType(pDecisionMakerType d)
        {
            this.identifier = d.Identifier;
            definedDMs[d.Identifier] = this;
            this.role = d.Role;

            this.chroma = Color.FromName(d.Color).ToArgb();
            this.briefing = d.Briefing;
            if ("" == this.briefing)
            {
                this.briefing = "(No briefing supplied.)";
            }
            //    this.reportsTo = d.ReportsTo;
            for (int i = 0; i < d.ReportsTo.Count; i++)
            {
                if (!IsExistingDM(d.ReportsTo[i]))
                    throw new ApplicationException("Supervisor '" + d.ReportsTo[i] + "' of DM '" + this.identifier + "' is not a DM");
                else if (this.directSupervisors.Contains(d.ReportsTo[i]))
                    throw new ApplicationException("DM '" + this.identifier + "'");
                else
                {
                    this.AddDirectSupervisor(d.ReportsTo[i]);

                }
            }
            this.hasTransferAuthority = d.CanTransfer;
            this.hasForcedTransferAuthority = d.CanForceTransfers;
            this.team = d.Team;
            //for (int i = 0; i < d.InitialVoiceAccess.Count; i++)
            //    this.voiceChannelAccess.Add(d.InitialVoiceAccess[i]);
            for (int i = 0; i < d.ChatPartners.Count; i++)
                this.chatPartners.Add(d.ChatPartners[i]);
            for (int i = 0; i < d.WhiteboardPartners.Count; i++)
                this.whiteboardPartners.Add(d.WhiteboardPartners[i]);

            isObserver = d.IsObserver;
        }
    }
    public class NetworkType : RootEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private List<string> members = new List<string>();
        public string this[int index]
        {
            get
            {
                if (members.Count > index)
                {
                    return members[index];
                }
                else
                {
                    return "";
                }
            }
        }
        public int Count()
        {
            return members.Count;
        }
        public void Add(string member)
        {
            members.Add(member);
        }
        public NetworkType(pNetworkType n)
        {
            Time = 0;
            name = n.Name;
            for (int i = 0; i < n.Count(); i++)
            {
                members.Add(n[i]);
            }
        }
        public NetworkType(string name)
        {
            Time = 0;
            this.name = name;
        } // have to add member outside

    }



    public class EngramRange
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private string unit = "";
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        private List<string> values;
        public List<string> Values
        {
            get { return values; }
        }
        private string condition = "";
        public string Condition
        {
            get { return condition; }
        }
        private string compareTo = "";
        public string CompareTo
        {
            get { return compareTo; }
        }

        private Boolean testForIncluded;
        public Boolean TestForIncluded
        {
            get { return testForIncluded; }
        }
        public Boolean Valid(string parentUnit, string parentTarget)
        {
            string lhsString;
            Boolean returnValue;
            string substituted = unit;
            if ("UNIT" == unit)
            {
                substituted = parentUnit;
            }
            else if ("TARGET" == unit)
            {
                substituted = parentTarget;
            }
            if ("" != condition)
            {
                if ("" == substituted)
                {
                    lhsString = Engrams.GetValue(name);
                }
                else
                {
                    lhsString = Engrams.GetValue(name, substituted);
                }
                string rhsString = compareTo;// Engrams.GetValue(compareTo);
                double lhs;
                double rhs;
                if (lhsString.ToLower() == "true" || lhsString.ToLower() == "false")
                {
                    switch (lhsString.ToLower())
                    {
                        case "true":
                            lhs = 1;
                            break;
                        default:
                            lhs = 0;
                            break;
                    }
                }
                else
                { 
                    lhs = double.Parse(lhsString);
                }
                if (rhsString.ToLower() == "true" || rhsString.ToLower() == "false")
                {
                    switch (rhsString.ToLower())
                    {
                        case "true":
                            rhs = 1;
                            break;
                        default:
                            rhs = 0;
                            break;
                    }
                }
                else
                {
                    rhs = double.Parse(rhsString);
                }
                switch (condition)
                {
                    case "GT":
                        returnValue = lhs > rhs;
                        break;
                    case "GE":
                        returnValue = lhs >= rhs;
                        break;
                    case "EQ":
                        returnValue = lhs == rhs;
                        break;
                    case "LE":
                        returnValue = lhs <= rhs;
                        break;
                    case "LT":
                        returnValue = lhs < rhs;
                        break;
                    default:
                        throw new ApplicationException("Engram comparison has illegal comparsion operator " + condition);
                }
            }
            else
            {
                string value;
                if ("" == substituted)
                {
                    value = Engrams.GetValue(name);
                }
                else
                {
                    value = Engrams.GetValue(name, substituted);
                }
                if (testForIncluded)
                {
                    returnValue = values.Contains(value);
                }
                else
                {
                    returnValue = !values.Contains(value);
                }
            }
            return returnValue;
        }


        public void Add(string newRangeElement)
        {
            values.Add(newRangeElement);
        }

        public EngramRange(string name, Boolean testForIncluded)
        {
            this.name = name;
            this.testForIncluded = testForIncluded;
            values = new List<string>();
        }
        public EngramRange(pEngramRange range)
        {
            this.name = range.Name;
            this.unit = range.Unit;
            if ("" != range.Condition)
            {
                this.condition = range.Condition;
                this.compareTo = range.CompareTo;
            }
            else
            {
                this.testForIncluded = range.InclusionRange;
                this.values = range.ValueList;
            }
        }
        public EngramRange(EngramRange range)
        {
            this.name = range.Name;
            this.unit = range.Unit;
            this.compareTo = range.compareTo;
            this.condition = range.condition;
            this.values = range.values;
            this.testForIncluded = range.TestForIncluded;
        }

    }

    /// <summary>
    /// Scenario events are root events that also have a unit ID field and a parameter dictionary
    /// </summary>
    public class ScenarioEventType : RootEventType
    {
        private string unitID = null;
        public string UnitID
        {
            get
            { return this.unitID; }
            set
            { this.unitID = value; }
        }
        private List<string> allUnits = new List<string>();
        public List<string> AllUnits
        {
            get { return allUnits; }
            //            set { allUnits = value; }
        }
        private ObjectDictionary parameters;
        public ObjectDictionary Parameters
        {
            get { return parameters; }
            set { parameters = value; } // Should do deep copy?
        }

        private EngramRange range = null;
        public EngramRange Range
        {
            get { return range; }
            set { range = value; }
        }
        public Boolean EngramValid(string parentUnit, string parentTarget)
        {
            if (null == range)
            {
                return true;
            }
            return this.range.Valid(parentUnit, parentTarget);

        }
        /// Tells whether this event is "about" this unit 
        /// </summary>
        /// <param name="unitID">unit identifier</param>
        /// <returns>True if unit is a predicate of event</returns>

        public override Boolean Involves(string unitID)
        {
            Boolean returnValue = false;
            if (this.allUnits.Contains(unitID))
            {
                returnValue = true;
            }
            return returnValue;
        }
        /// <summary>
        /// construct a scenario event (Overload 1)
        /// </summary>
        /// <param name="unitID">Identifier for a unit</param>
        /// <param name="timer">time value -- typically time aty which event is to be queued</param>

        public ScenarioEventType(string unitID, int timer)
        {
            this.UnitID = unitID;
            this.Time = timer;
            this.allUnits.Add(unitID);
            this.parameters = null;
            this.range = null;
        }
        /// construct a scenario event (Overload 2); time is set to 0
        /// </summary>
        /// <param name="unitID">Identifier for a unit</param>
        public ScenarioEventType(string unitID)
        {
            this.UnitID = unitID;
            this.Time = 1;
            this.allUnits.Add(unitID);
            this.parameters = null;
            this.range = null;
        }
        /// construct a scenario event (Overload 2); time is set to 0

        public ScenarioEventType(int time)
        {
            this.unitID = "";
            this.Time = time;
        }


        /// <summary>
        /// Virtual methods (two overloads) for adding a parametyer (name,attribute) pair to s scenario event.
        /// </summary>
        /// <param name="setting"></param>
        /*    public virtual void Add(string attr, string set)
            { }
         * */
        /// <summary>
        /// Add a unit to the list of "other units referenced by this event
        /// </summary>
        /// <param name="otherUnit">unit identifier to add</param>

        // "Needed" because called from other places, like Timer
        public void AddOtherUnit(string otherUnit)
        {
            if (!allUnits.Contains(otherUnit))
            {
                allUnits.Add(otherUnit);
            }
        }

    }
    public class SystemMessage : ScenarioEventType
    {
        private static System.Drawing.Color defaultColor = System.Drawing.Color.OrangeRed;
        private int textColor;
        public int TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }
        private string addressee;
        public string Addressee
        {
            get { return addressee; }
        }
        private string message;
        public string Message
        {
            get { return message; }
        }
        private string displayMode = "Default";
        public string DisplayMode
        {
            get { return displayMode; }
        }
        public SystemMessage(int time, string addressee, string message)
            : base(time)
        {
            this.addressee = addressee;
            this.message = message;
            this.textColor = 256 * 256 * (int)(defaultColor.R) + 256 * (int)(defaultColor.G) + defaultColor.B;
            this.displayMode = "DEFAULT";
        }
        public SystemMessage(int time, string addressee, string message, int textColor)
            : base(time)
        {
            this.addressee = addressee;
            this.message = message;
            this.textColor = textColor;
            this.displayMode = "DEFAULT";

        }
        public void MakePopup()
        {
            this.displayMode = "POP";
        }
    }
    public class OpenChatRoomType : ScenarioEventType
    {

        private string room;
        public string Room
        {
            get { return room; }
        }
        private string owner = "EXP"; // for experimenter
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private List<string> members;
        public List<string> Members
        {
            get { return members; }
        }
        public OpenChatRoomType(pOpenChatRoomType c)
            : base(c.Time)
        {
            this.room = c.Room;
            if (null != c.Owner)
                this.owner = c.Owner;
            NameLists.chatroomNames.New(c.Room, null);//Do not care if open references a previous name as scenario might legally reuse names or use conditionally.
            for (int i = 0; i < c.Members.Count; i++)
            {
                if (!UnitFacts.IsDM(c.Members[i])) throw new ApplicationException("Cannot add nonexistent decision maker " + c.Members[i] + " to chat room " + c.Room);
            }
            this.members = c.Members; // Assignment after because parser creates new list each time
        }
        public OpenChatRoomType(int time, string owner, string room, List<string> members)
            : base(time)
        {
            // existence checking not as this form comes from client 
            this.room = room;
            this.owner = owner;

            this.members = members; // Requires list to be have been created anew by sender
        }
        public OpenChatRoomType(OpenChatRoomType oc)
            : base(oc.Time)
        {

            this.room = oc.Room;
            this.members = oc.Members;
            this.owner = oc.Owner;
        }

    }
    /*
      public class CreateChatRoomFailureType : RootEventType
      {
          private string message;
          public string Message
          {
              get { return message; }
          }
          private string requestingDM;
          public string RequestingDM
          {
              get { return requestingDM; }
          }
          public CreateChatRoomFailureType(string message, string requestingDM)
          {
              this.message = message;
              this.requestingDM = requestingDM;
          }
      }
   
      */

    public class OpenWhiteboardRoomType : ScenarioEventType
    {

        private string room;
        public string Room
        {
            get { return room; }
        }
        private string owner = "EXP"; // for experimenter
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private List<string> members;
        public List<string> Members
        {
            get { return members; }
        }
        public OpenWhiteboardRoomType(pOpenWhiteboardRoomType c)
            : base(c.Time)
        {
            this.room = c.Room;
            if (null != c.Owner)
                this.owner = c.Owner;
            NameLists.whiteboardroomNames.New(c.Room, null);//Do not care if open references a previous name as scenario might legally reuse names or use conditionally.
            for (int i = 0; i < c.Members.Count; i++)
            {
                if (!UnitFacts.IsDM(c.Members[i])) throw new ApplicationException("Cannot add nonexistent decision maker " + c.Members[i] + " to whiteboard room " + c.Room);
            }
            this.members = c.Members; // Assignment after because parser creates new list each time
        }
        public OpenWhiteboardRoomType(int time, string owner, string room, List<string> members)
            : base(time)
        {
            // existence checking not as this form comes from client 
            this.room = room;
            this.owner = owner;

            this.members = members; // Requires list to be have been created anew by sender
        }
        public OpenWhiteboardRoomType(OpenWhiteboardRoomType oc)
            : base(oc.Time)
        {

            this.room = oc.Room;
            this.members = oc.Members;
            this.owner = oc.Owner;
        }

    }

    public class OpenVoiceChannelType : ScenarioEventType
    {
        private string channel;
        public string Channel
        {
            get { return channel; }
        }
        /*
         //deprecated
        private string owner = "EXP"; // for experimenter
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
         */
        private List<string> initialMembers = null;
        public List<string> InitialMembers
        {
            get { return initialMembers; }
            set { initialMembers = value; }
        }
        public OpenVoiceChannelType(pOpenVoiceChannelType c)
            : base(c.Time)
        {
            this.channel = c.Channel;
            this.initialMembers = new List<string>(c.Access.ToArray());
            NameLists.voicechannelNames.New(c.Channel, this.initialMembers);//Do not care if open references a previous name as scenario might legally reuse names or use conditionally.
        }
        public OpenVoiceChannelType(int time, string channel, List<string> accessList)
            : base(time)
        {
            // existence checking not as this form comes from client 
            this.channel = channel;
            this.initialMembers = new List<string>(accessList.ToArray());
            }
        public OpenVoiceChannelType(OpenVoiceChannelType oc)
            : base(oc.Time)
        {
            this.initialMembers = new List<string>(oc.initialMembers.ToArray());
            this.channel = oc.Channel;
        }

    }
    //
    /*public class AddChattersType : ScenarioEventType
    {

    */
    public class CloseVoiceChannelType : ScenarioEventType
    {
        private string requestor = "EXP";
        public string Requestor
        {
            get { return requestor; }
        }
        private string channel;
        public string Channel
        {
            get { return channel; }
        }
        public CloseVoiceChannelType(pCloseVoiceChannelType c)
            : base(c.Time)
        {
            if (!NameLists.voicechannelNames.ContainsKey(c.Channel)) throw new ApplicationException("Cannot close non-existent voice channel " + c.Channel);
            this.Time = c.Time;
            this.channel = c.Channel;
            if (null != c.Requestor)
                this.requestor = c.Requestor;
        }
        public CloseVoiceChannelType(int time, string requestor, string name)
            : base(time)
        {

            this.channel = name;
            this.requestor = requestor;
        }
        public CloseVoiceChannelType(CloseVoiceChannelType c)
            : base(c.Time)
        {

            this.channel = c.channel;
            if (c.Requestor != null)
                this.requestor = c.requestor;
        }
    }

    public class GrantVoiceAccessType:ScenarioEventType
    {
        private EngramRange engramRange = null;
        public EngramRange EngramRange
        {
            get { return engramRange; }

        }
        private string decisionMaker;
        public string DecisionMaker
        {
            get { return decisionMaker; }
        }
        private string voiceChannel;
        public string VoiceChannel
        {
            get { return voiceChannel; }
        }
 
        public GrantVoiceAccessType(pGrantVoiceChannelAccessType pgva):base(pgva.Time)
        {
            if(null!=pgva.EngramRange)
                this.engramRange = new EngramRange(pgva.EngramRange);
            this.decisionMaker = pgva.DecisionMaker;
            this.voiceChannel = pgva.VoiceChannel;
        }
    }
    public class RemoveVoiceAccessType:ScenarioEventType
    {
        private EngramRange engramRange = null;
        public EngramRange EngramRange
        {
            get { return engramRange; }
        }
        private string decisionMaker;
        public string DecisionMaker
        {
            get { return decisionMaker; }
        }
        private string voiceChannel;
        public string VoiceChannel
        {
            get { return voiceChannel; }
        }
   
        public RemoveVoiceAccessType(pRemoveVoiceChannelAccessType prva):base( prva.Time)
        {
            if(null!=prva.EngramRange)
                this.engramRange = new EngramRange(prva.EngramRange);
            this.decisionMaker = prva.DecisionMaker;
            this.voiceChannel = prva.VoiceChannel;
        }
    }
    public class UpdateTagType : ScenarioEventType
    {
        private string newTag = "";
        public string NewTag
        {
            get { return newTag; }
        }
        private List<string> teamMembers = new List<string>();
        public List<String> TeamMembers
        {
            get { return teamMembers; }
        }
        public UpdateTagType(string unitID, string newTag, List<string> teamMembers)
            : base(unitID)
        {
            this.newTag = newTag;
            for (int i = 0; i < teamMembers.Count; i++)
                this.teamMembers.Add(teamMembers[i]);
        }
    }
    /*
        public class AddChattersType : ScenarioEventType
        {
            private string owner = null; // for additions via script
            public string Owner
            {
                get { return owner; }
            }
            private string room;
            public string Room
            {
                get { return room; }
            }
            private List<string> members;
            public List<string> Members
            {
                get { return members; }
            }
            public AddChattersType(pAddChattersType p)
                : base(p.Time)
            {
                this.members = p.Members;
                this.room = p.Room;

            }
            public AddChattersType(int time, string owner, string room, List<string> members)
                : base(time)
            {
                this.owner = owner;
                this.room = room;
                this.members = members;
            }
            public AddChattersType(AddChattersType addC)
                : base(addC.Time)
            {
                this.owner = addC.Owner;
                this.members = addC.Members;
                this.room = addC.Room;
            }
        }
        public class DropChattersType : ScenarioEventType
        {
            private string owner = null;
            public string Owner
            {
                get { return owner; }
            }
            private string room;
            public string Room
            {
                get { return room; }
            }
            private List<string> members;
            public List<string> Members
            {
                get { return members; }
            }
            public DropChattersType(pDropChattersType p)
                : base(p.Time)
            {
                this.members = p.Members;
                this.room = p.Room;
            }
            public DropChattersType(int time, string owner, string room, List<string> members)
                : base(time)
            {
                this.owner = owner;
                this.room = room;
                this.members = members;
            }
            public DropChattersType(DropChattersType d)
                : base(d.Time)
            {
                this.owner = d.Owner;
                this.members = d.Members;
                this.room = d.Room;
            }
        }
        */
    public class CloseChatRoomType : ScenarioEventType
    {
        private string requestor = "EXP";
        public string Requestor
        {
            get { return requestor; }
        }
        private string room;
        public string Room
        {
            get { return room; }
        }

        public CloseChatRoomType(pCloseChatRoomType c)
            : base(c.Time)
        {
            if (!NameLists.chatroomNames.ContainsKey(c.Room)) throw new ApplicationException("Cannot close non-existent chat room " + c.Room);
            this.Time = c.Time;
            this.room = c.Room;
            if (null != c.Requestor)
                this.requestor = c.Requestor;
        }
        public CloseChatRoomType(int time, string requestor, string name)
            : base(time)
        {

            this.room = name;
            this.requestor = requestor;
        }
        public CloseChatRoomType(CloseChatRoomType c)
            : base(c.Time)
        {

            this.room = c.room;
            if (c.Requestor != null)
                this.requestor = c.requestor;
        }
    }

    public class ActiveRegionVisibilityUpdateType : ScenarioEventType
    {
        private Boolean isVisible;
        public Boolean IsVisible
        {
            get { return isVisible; }
        }
        public ActiveRegionVisibilityUpdateType(pSetRegionVisibilityType s)
            : base(s.ID, s.Time)
        {
            this.isVisible = s.IsVisible;
        }
    }
    public class ActiveRegionActivityUpdateType : ScenarioEventType
    {
        private Boolean isActive;
        public Boolean IsActive
        {
            get { return isActive; }
        }
        public ActiveRegionActivityUpdateType(pSetRegionActivityType s)
            : base(s.ID, s.Time)
        {
            this.isActive = s.IsActive;
        }
    }

    public class ActiveRegionUpdateType : ScenarioEventType
    {
        private Boolean isVisible;
        public Boolean IsVisible
        {
            get { return isVisible; }
        }
        private Boolean isActive;
        public Boolean IsActive
        {
            get { return isActive; }
        }
        public ActiveRegionUpdateType(string ID, int time, ActiveRegionStateType s)
            : base(ID, time)
        {
            this.isVisible = s.IsVisible;
            this.isActive = s.IsActive;
        }

    }


    public class PointType
    {
        private double x;
        public double X
        {
            get { return x; }
        }
        private double y;
        public double Y
        {
            get { return y; }
        }
        public PointType(pPointType p)
        {
            this.x = p.X;
            this.y = p.Y;
        }
        public PointType(GeometricPointType g)
        {
            this.x = g.X;
            this.y = g.Y;
        }
    }

    public class RegionEventType : ScenarioEventType
    {
        private bool isDynamicRegion;
        public bool IsDynamicRegion
        {
            get { return isDynamicRegion; }
            set { isDynamicRegion = value; }
        }
        private PointType referencePoint;
        public PointType ReferencePoint
        {
            get { return referencePoint; }
            set { referencePoint = value; }
        }

        private List<PointType> vertices;
        public List<PointType> Vertices
        {

            get { return vertices; }
        }
        private double? start;
        public double? Start
        {
            get { return start; }
            set { start = value; }
        }
        private double? end;
        public double? End
        {
            get { return end; }
            set { end = value; }
        }
        private double? speedMultiplier;
        public double? SpeedMultiplier
        {
            get { return speedMultiplier; }
            set { speedMultiplier = value; }
        }
        private Boolean? blocksMovement;
        public Boolean? BlocksMovement
        {
            get { return blocksMovement; }
            set { blocksMovement = value; }
        }
        private List<string> sensorsBlocked;
        public List<string> SensorsBlocked
        {
            get { return sensorsBlocked; }
        }
        private Boolean isVisible;
        public Boolean IsVisible
        {
            get { return isVisible; }
        }
        private Boolean isActive;
        public Boolean IsActive
        {
            get { return isActive; }
        }
        private String obstructedViewImage;
        public String ObstructedViewImage
        {
            get { return obstructedViewImage; }
        }
        private double obstructionOpacity;
        public double ObstructionOpacity
        {
            get { return obstructionOpacity; }
        }
        private int chroma;
        public int Chroma
        {
            get { return chroma; }
        }

        public void Add(string sensorName)
        {
            sensorsBlocked.Add(sensorName);
        }


        public RegionEventType(pLandRegionType r)
            : base(r.ID)
        {


            //Convert to the form needed by the convexity tester
            List<GeometricPointType> gList = new List<GeometricPointType>();
            for (int i = 0; i < r.Vertices.Count; i++)
            {
                gList.Add(new GeometricPointType(r.Vertices[i].X, r.Vertices[i].Y));
            }
            /* Do not check a land readion for convexity
             * if (!ConvexTest.IsConvex(gList))
                        {
                            throw (new ApplicationException("Region " + r.ID + "  is not convex"));
                        }
            */
            vertices = new List<PointType>();
            for (int i = 0; i < gList.Count; i++)
            {
                vertices.Add(new PointType(gList[i]));
            }
            isActive = true;// Needed for regions that are not Active regions? MAybe better than sending nulls
            isVisible = true;
        }

        public RegionEventType(pActiveRegionType r)
            : base(r.ID)
        {


            //Convert to the form needed by the convexity tester
            List<GeometricPointType> gList = new List<GeometricPointType>();
            for (int i = 0; i < r.Vertices.Count; i++)
            {
                gList.Add(new GeometricPointType(r.Vertices[i].X, r.Vertices[i].Y));
            }
            if (!ConvexTest.IsConvex(gList))
            {
                throw (new ApplicationException("Region " + r.ID + " is not convex"));
            }
            vertices = new List<PointType>();
            for (int i = 0; i < gList.Count; i++)
            {
                vertices.Add(new PointType(gList[i]));
            }

            start = r.Start;
            end = r.End;
            speedMultiplier = r.SpeedMultiplier;
            blocksMovement = r.BlocksMovement;
            sensorsBlocked = new List<string>(); //r.SensorsBlocked;
            obstructedViewImage = r.ObstructedViewImage;
            obstructionOpacity = r.ObstructionOpacity;
            for (int i = 0; i < r.SensorsBlocked.Count; i++)
            {
                sensorsBlocked.Add(r.SensorsBlocked[i]);
            }
            isVisible = r.IsVisible;
            isActive = r.IsActive;
            isDynamicRegion = r.IsDynamicRegion;
            if (r.ReferencePoint == null)
                referencePoint = new PointType(new pPointType(0, 0));
            else
                referencePoint = new PointType(new GeometricPointType(r.ReferencePoint.X,r.ReferencePoint.Y));
     
            
            this.chroma = Color.FromName(r.Color).ToArgb();

        }
    }


    /// <summary>
    /// Specifies that a unit is to be created as part of the game
    /// </summary>
    public class Create_EventType : ScenarioEventType
    {
        private string unitBase;
        public string UnitBase
        {
            get
            { return this.unitBase; }
            set { this.unitBase = value; }
        }

        private string owner;
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private string genus;
        public string Genus
        {
            get { return genus; }
            set { genus = value; }
        }
        private List<string> subplatforms;
        public List<string> Subplatforms
        {
            get { return subplatforms; }
        }
        /// <summary>
        /// Constructor for a create event
        /// </summary>
        /// <param name="unit">identifier of a unit</param>
        public Create_EventType(string unit) // only used in weapon create
            : base(unit)
        {
            Parameters = null;
            subplatforms = null;
        }
        public Create_EventType(pCreateType cEvent)
            : base(cEvent.UnitID)
        {
            this.unitBase = cEvent.UnitBase;
            this.owner = cEvent.Owner;
            Parameters = null;
            subplatforms = new List<string>();
            for (int i = 0; i < cEvent.Platforms.Count; i++)
                if (UnitFacts.Data.ContainsKey(cEvent.Platforms[i]))
                {
                    
                    if (SubplatformRecords.RecordDocking(this.UnitID, cEvent.Platforms[i]) == true)
                    { 
                        subplatforms.Add(cEvent.Platforms[i]);
                    }
                }
                else throw new ApplicationException("Cannot add undefined subplatform '" +
                    cEvent.Platforms[i] + "' to unit '" + cEvent.UnitID + "'");

        }
    }
    public class Reveal_EventType : ScenarioEventType
    {

        private LocationType initialLocation;
        public LocationType InitialLocation
        {
            get { return initialLocation; }
            set { initialLocation = value; }
        }
        private string initialState;
        public string InitialState
        {
            get { return initialState; }
            set { initialState = value; }
        }
        private string genus;
        public string Genus
        {
            get { return genus; }
            set { genus = value; }
        }
        private Boolean isWeapon = false;
        public Boolean IsWeapon
        {
            get { return isWeapon; }
            set { isWeapon = value; }
        }
        private Boolean dockedToParent = false;
        public Boolean DockedToParent
        {
            get { return dockedToParent; }
            set { dockedToParent = value; }
        }


        /// <summary>
        /// Constructor for a reveal event
        /// </summary>
        /// <param name="unit">identifier of a unit</param>
        public Reveal_EventType(string unit)
            : base(unit)
        {
            if (!NameLists.unitNames.ContainsKey(unit)) throw new ApplicationException("Cannot reveal unknown unit " + unit);
            initialLocation = new LocationType(0, 0, 0);//recent add to serve subplatform reveal
            this.Parameters = new ObjectDictionary();

        }
        public Reveal_EventType(pRevealType rEvent)
            : base(rEvent.UnitID, rEvent.Time)
        {
            if (!NameLists.unitNames.ContainsKey(rEvent.UnitID)) throw new ApplicationException("Cannot reveal unknown unit " + rEvent.UnitID);

            if (null != rEvent.EngramRange)
            {
                this.Range = new EngramRange(rEvent.EngramRange);
            }
            this.initialLocation = new LocationType(rEvent.InitialLocation.X,
             rEvent.InitialLocation.Y, rEvent.InitialLocation.Z);


            this.initialState = rEvent.InitialState;
            if ("" == this.initialState)
            {
                this.initialState = "FullyFunctional";
            }
            else
            {
                if (!StatesForUnits.UnitHasState(rEvent.UnitID, rEvent.InitialState)) throw new ApplicationException("Cannot reveal " + rEvent.UnitID + " in initial state " + rEvent.InitialState + ", as state doesn't exist.");
            }


            //        this.Parameters = new ObjectDictionary(rEvent.StartupParameters);
            StateBody thisStartState = StatesForUnits.StateTable[Genealogy.GetBase(this.UnitID)][this.initialState];
            Parameters = thisStartState.Parameters.DeepCopy();
            ObjectDictionary incomingParameters = new ObjectDictionary(rEvent.StartupParameters);
            List<string> KeyList = incomingParameters.GetKeys();
            foreach (string k in KeyList)
            {
                Parameters[k] = incomingParameters[k];
            }
            string initialTag = "";
            if ((null != rEvent.InitialTag))
                initialTag = rEvent.InitialTag;
            Parameters["InitialTag"] = initialTag;
        }

        public Reveal_EventType(Reveal_EventType rEvent)
            : base(rEvent.UnitID, rEvent.Time)
        {
            if (null != rEvent.Range)
            {
                this.Range = new EngramRange(rEvent.Range);
            }
            this.initialLocation = new LocationType(rEvent.InitialLocation.X,
             rEvent.InitialLocation.Y, rEvent.InitialLocation.Z);


            this.initialState = rEvent.InitialState;

            this.Parameters = rEvent.Parameters.DeepCopy();

            this.genus = rEvent.Genus;

        }
 
    
    }

    public class SubplatformLaunchType : ScenarioEventType
    {


        private string parentUnit;
        public string ParentUnit
        {
            get { return parentUnit; }
        }
        private LocationType destination;
        public LocationType Destination
        {
            get { return destination; }
        }

        /// <summary>
        /// Constructs the  request
        /// 
        public SubplatformLaunchType(SubplatformLaunchType s)
            : base(s.UnitID, s.Time)
        {
            this.parentUnit = s.ParentUnit;
            this.destination = new LocationType(s.Destination.X, s.Destination.Y, s.Destination.Z);
        }

        public SubplatformLaunchType(string childUnit, string parentUnit, LocationType destination
           )
            : base(childUnit)
        {

            this.parentUnit = parentUnit;
            this.destination = destination;
            this.AddOtherUnit(parentUnit);

        }
    }


    public class SubplatformDockType : ScenarioEventType
    {


        private string parentUnit;
        public string ParentUnit
        {
            get { return parentUnit; }
        }


        /// <summary>
        /// Constructs the  request

        public SubplatformDockType(SubplatformDockType s)
            : base(s.UnitID, s.Time)
        {
            this.parentUnit = s.ParentUnit;
        }
        public SubplatformDockType(string childUnit, string parentUnit
           )
            : base(childUnit)
        {

            this.parentUnit = parentUnit;
            this.AddOtherUnit(parentUnit);

        }
    }

    public class WeaponLaunchFailure_EventType : ScenarioEventType
    {
        private string weaponID;
        public string WeaponID
        {
            get
            { return this.weaponID; }
        }
        private string parentID;
        public string ParentID
        {
            get
            { return this.parentID; }
        }
        private string reasonForFailure;
        public string ReasonForFailure
        {
            get
            { return this.reasonForFailure; }
        }

        public WeaponLaunchFailure_EventType(string weaponID, string parentID, string reasonForFailure)
            : base(parentID)
        {
            this.parentID = parentID;
            this.weaponID = weaponID;
            this.reasonForFailure = reasonForFailure;
            this.AddOtherUnit(weaponID);
        }
    
    }

    public class WeaponLaunch_EventType : ScenarioEventType
    {
        private string weaponID;
        public string WeaponID
        {
            get
            { return this.weaponID; }
        }
        private string targetID;
        public string TargetID
        {
            get
            { return this.targetID; }
        }
        public WeaponLaunch_EventType(WeaponLaunch_EventType w)
            : base(w.UnitID, w.Time)
        {
            this.targetID = w.TargetID;
            this.weaponID = w.WeaponID;
        }
        public WeaponLaunch_EventType(string unitID, string targetID, string weaponID)
            : base(unitID)
        {
            this.targetID = targetID;
            this.weaponID = weaponID;
            this.AddOtherUnit(targetID);
            this.AddOtherUnit(weaponID);
        }
    }

    /// <summary>
    /// Command that a unit be moved
    /// </summary>
    public class Move_EventType : ScenarioEventType
    {
        private double throttle;
        public double Throttle
        {
            get
            { return this.throttle; }
            set
            { throttle = Math.Min(100, value); }
        }
        private LocationType destination;
        public LocationType Destination
        {
            get
            { return this.destination; }
            set
            { this.destination = value; }

        }
        /// <summary>
        /// Construct a move request  for which location is not provided (override 1)
        /// </summary>
        /// <param name="unit">identifier for unit</param>
        /// <param name="timer">Time for event to occur</param>

        public Move_EventType(Move_EventType m)
            : base(m.UnitID, m.Time)
        {
            this.throttle = m.Throttle;
            this.destination = new LocationType(m.Destination.X, m.Destination.Y, m.Destination.Z);
        }
        public Move_EventType(string unit, int timer)
            : base(unit, timer)
        {
            destination = new LocationType();
        }
        public Move_EventType(pMoveType mEvent)
            : base(mEvent.UnitID,
            mEvent.Timer)
        {

            this.destination = new LocationType(mEvent.Location.X, mEvent.Location.Y,
          mEvent.Location.Z);

            this.throttle = mEvent.Throttle;
            if (null != mEvent.EngramRange)
            {
                this.Range = new EngramRange(mEvent.EngramRange);
            }
        }

        /// <summary>for which location is not provided (override 2)
        /// </summary>
        /// <param name="unit">identifier for unit</param>
        /// <param name="timer">Time for event to occur</param>
        /// <param name="location">Location to which unit is being moved</param>
        public Move_EventType(string unit, int timer, LocationType destination, double throttle)
            : base(unit, timer) // note, here timer is takes literally -- used by Ticker
        {
            this.destination = destination;
            this.throttle = throttle;
        }
    }
    /// <summary>
    /// Describes an event that is to occur as a result of an action completion
    /// </summary>
    public class HappeningCompletionType : ScenarioEventType
    {
        private Boolean holdsReiteration = false;
        public Boolean HoldsReiteration
        {
            get { return holdsReiteration; }
        }
        private string action;
        public string Action
        {
            get
            { return action; }
            set
            { action = value; }
        }
        private string newState;
        public string NewState
        {
            get
            { return newState; }
            set
            { newState = value; }
        }

        private List<ScenarioEventType> doThisList = new List<ScenarioEventType>();
        public List<ScenarioEventType> DoThisList
        {
            get
            { return doThisList; }
            /* set
             { doThisList = value; }*/
        }
        public void Add(ScenarioEventType s)
        {
            doThisList.Add(s);
        }
        public void Add(List<ScenarioEventType> sList)
        {
            for (int i = 0; i < sList.Count; i++)
            {
                doThisList.Add(sList[i]);
            }
        }
        private Boolean isStateChange = false;
        public Boolean IsStateChange
        {
            get { return isStateChange; }
            set { isStateChange = value; }
        }
        private Boolean isAction = false;
        public Boolean IsAction
        {
            get { return isAction; }
            set { isAction = value; }
        }
        /// <summary>
        /// Tells whether this event is "about" this unit 
        /// </summary>
        /// <param name="unitID">unit identifier</param>
        /// <returns>True if unit is a predicate of event of the condition event it packages</returns>

        public override Boolean Involves(string unitID)
        {
            Boolean returnValue = this.AllUnits.Contains(unitID);
            for (int i = 0; i < doThisList.Count; i++)
            {
                returnValue = returnValue || doThisList[i].Involves(unitID);
            }
            return returnValue;
        }

        public void EnqueueEvents()
        {
            List<ScenarioEventType> newEventList = this.DoThisList;
            for (int k = 0; k < newEventList.Count; k++)
            {
                ScenarioEventType newEvent = newEventList[k];
                newEvent.Time = (1 + newEvent.Time) + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/);
                TimerQueueClass.Add(newEvent.Time, newEvent);
            }
        }


        public void SwapList(List<ScenarioEventType> newList)
        {
            List<ScenarioEventType> holder = new List<ScenarioEventType>();
            for (int i = 0; i < newList.Count; i++)
            {
                holder.Add(newList[i]);
            }

            this.DoThisList.Clear();
            for (int j = 0; j < holder.Count; j++)
            {
                this.DoThisList.Add(holder[j]);
            }
        }

        /// <summary>
        /// Creates a completion triggered  event
        /// </summary>
        public HappeningCompletionType(HappeningCompletionType h)
            : base(h.UnitID)
        {
            object performer;
            this.isAction = false;
            this.isStateChange = false;
            this.Range = null;
            if (null != h.Range)
            {
                this.Range = new EngramRange(h.Range);
            }
            if ("" != h.Action)
            {
                this.isAction = true;
            }
            this.action = h.Action;
            if ("" != h.NewState)
            {
                this.isStateChange = true;
            }
            this.newState = h.NewState;
            this.holdsReiteration = h.holdsReiteration;
            for (int doThis = 0; doThis < h.DoThisList.Count; doThis++)
            {
                performer = h.DoThisList[doThis];
                switch (performer.GetType().Name.ToString())
                {
                    case "OpenChatRoomType":
                        this.doThisList.Add(new OpenChatRoomType((OpenChatRoomType)(h.DoThisList[doThis])));
                        break;
                    case "CloseChatRoomType":
                        this.doThisList.Add(new CloseChatRoomType((CloseChatRoomType)(h.DoThisList[doThis])));
                        this.Add(((ReiterateType)(h.doThisList[doThis])).ReiterateList);
                        break;
                    case "OpenWhiteboardRoomType":
                        this.doThisList.Add(new OpenWhiteboardRoomType((OpenWhiteboardRoomType)(h.DoThisList[doThis])));
                        break;
                    /*
            case "AddChattersType":
                this.doThisList.Add(new AddChattersType((AddChattersType)(h.DoThisList[doThis])));
                break;
            case "DropChattersType":
                this.doThisList.Add(new DropChattersType((DropChattersType)(h.DoThisList[doThis])));
                break;
                     * */
                    case "Reveal_EventType":
                        this.doThisList.Add(new Reveal_EventType((Reveal_EventType)(h.DoThisList[doThis])));
                        break;
                    case "SubplatfromLaunchType":
                        this.doThisList.Add(new SubplatformLaunchType((SubplatformLaunchType)(h.DoThisList[doThis])));
                        break;
                    case "SubplatfromDockType":
                        this.doThisList.Add(new SubplatformDockType((SubplatformDockType)(h.DoThisList[doThis])));
                        break;
                    case "WeaponLaunch_Eventype":
                        this.doThisList.Add(new WeaponLaunch_EventType((WeaponLaunch_EventType)(h.DoThisList[doThis])));
                        break;
                    case "Move_EventType":
                        this.doThisList.Add(new Move_EventType((Move_EventType)(h.DoThisList[doThis])));
                        break;
                    case "StateChangeEvent":
                        this.doThisList.Add(new StateChangeEvent((StateChangeEvent)(h.DoThisList[doThis])));
                        break;
                    case "TransferEvent":
                        this.doThisList.Add(new TransferEvent((TransferEvent)(h.DoThisList[doThis])));
                        break;
                    case "LaunchEventType":
                        this.doThisList.Add(new LaunchEventType((LaunchEventType)(h.DoThisList[doThis])));
                        break;
                    case "WeaponLaunchEventType":
                        this.doThisList.Add(new WeaponLaunchEventType((WeaponLaunchEventType)(h.DoThisList[doThis])));
                        break;
                    case "AttackObjectEvent":
                        this.doThisList.Add(new AttackObjectEvent((AttackObjectEvent)(h.DoThisList[doThis])));
                        break;
                    case "ChangeEngramType":
                        this.doThisList.Add(new ChangeEngramType((ChangeEngramType)(h.DoThisList[doThis])));
                        break;
                    case "RemoveEngramEvent":
                        this.doThisList.Add(new RemoveEngramEvent((RemoveEngramEvent)(h.DoThisList[doThis])));
                        break;


                    default:
                        Coordinator.debugLogger.LogError("Scenario reader", "Unknown happening event type for " + performer.ToString());
                        break;
                }
            }
        }

        public HappeningCompletionType(pHappeningCompletionType h)
            : base(h.Unit)
        {
            object performer;
            this.isAction = false;
            this.isStateChange = false;
            this.Range = null;
            if (null != h.EngramRange)
            {
                this.Range = new EngramRange(h.EngramRange);
            }
            if ("" != h.Action)
            {
                this.isAction = true;
            }
            this.action = h.Action;
            if ("" != h.NewState)
            {
                this.isStateChange = true;
            }
            this.newState = h.NewState;
            for (int doThis = 0; doThis < h.DoThisList.Count; doThis++)
            {
                performer = h.DoThisList[doThis];
                switch (performer.GetType().Name.ToString())
                {
                    case "pReiterateType":
                        ReiterateType rt = new ReiterateType((pReiterateType)performer);
                        this.holdsReiteration = true;
                        this.Range = rt.Range;
                        this.Add(rt.ReiterateList);
                        /// when this is called the top element has already been shifted to the end
                        //this.UnitID = rt.ReiterateList[rt.ReiterateList.Count - 1].UnitID;
                        break;
                    case "pMoveType":
                        this.doThisList.Add(new Move_EventType((pMoveType)performer));
                        /*
                         * this is not needed because  in ScenarioEvent constructor(?)
                                              if (!this.AllUnits.Contains(((pMoveType)performer).UnitID))
                                              {
                                                  this.AllUnits.Add(((pMoveType)performer).UnitID);
                                              }
                                              */
                        break;

                    case "pRevealType":
                        Reveal_EventType r = new Reveal_EventType((pRevealType)performer);
                        r.Genus = Genealogy.GetGenus(r.UnitID);
                        this.doThisList.Add(r);
                        break;
                    case "pStateChangeType":
                        this.doThisList.Add(new StateChangeEvent((pStateChangeType)performer));
                        break;
                    case "pTransferType":
                        this.doThisList.Add(new TransferEvent((pTransferType)performer));
                        break;
                    case "pLaunchType":
                        this.doThisList.Add(new LaunchEventType((pLaunchType)performer));
                        if (!this.AllUnits.Contains(((pLaunchType)performer).Child))
                        {
                            this.AllUnits.Add(((pLaunchType)performer).Child);
                        }
                        break;
                    case "pWeaponLaunchType":
                        this.doThisList.Add(new WeaponLaunchEventType((pWeaponLaunchType)performer));
                        if (!this.AllUnits.Contains(((pWeaponLaunchType)performer).Child))
                        {
                            this.AllUnits.Add(((pWeaponLaunchType)performer).Child);
                        }
                        break;
                    case "pChangeEngramType":
                        this.doThisList.Add(new ChangeEngramType((pChangeEngramType)performer));
                        break;
                    case "pRemoveEngramType":
                        this.doThisList.Add(new RemoveEngramEvent((pRemoveEngramType)performer));
                        break;
                    case "pFlushEventType":
                        this.doThisList.Add(new FlushEvents((pFlushEventsType)performer));
                        break;
                    case "pHappeningCompletionType":
                        this.doThisList.Add(new HappeningCompletionType((pHappeningCompletionType)performer));
                        break;
                    default:
                        Coordinator.debugLogger.LogError("Senario reader", "Unknown happening event type for " + performer.ToString());
                        break;
                }
            }
        }
        public HappeningCompletionType(ReiterateType r)
            : base("")
        {
            this.holdsReiteration = true;
            this.isAction = true;
            this.isStateChange = false;
            this.NewState = "";
            this.Range = r.Range;
            this.Add(r.ReiterateList);
            this.Action = "MoveComplete_Event";
            /// when this is called the top element has already been shifted to the end
            this.UnitID = r.ReiterateList[r.ReiterateList.Count - 1].UnitID;

        }
    }

    /// <summary>
    /// Defines a completion event at a species level
    /// </summary>
    public class SpeciesCompletionType : ScenarioEventType
    {

        private string species;
        public string Species
        {
            get { return species; }
        }
        private string action;
        public string Action
        {
            get
            { return action; }
            set
            { action = value; }
        }
        private string newState;
        public string NewState
        {
            get
            { return newState; }
            set
            { newState = value; }
        }

        private List<ScenarioEventType> doThisList = new List<ScenarioEventType>();
        public List<ScenarioEventType> DoThisList
        {
            get
            { return doThisList; }

        }
        public void Add(ScenarioEventType s)
        {
            doThisList.Add(s);
        }
        public void Add(List<ScenarioEventType> sList)
        {
            for (int i = 0; i < sList.Count; i++)
            {
                doThisList.Add(sList[i]);
            }
        }
        private Boolean isStateChange = false;
        public Boolean IsStateChange
        {
            get { return isStateChange; }
            set { isStateChange = value; }
        }
        private Boolean isAction = false;
        public Boolean IsAction
        {
            get { return isAction; }
            set { isAction = value; }
        }
        /// <summary>
        /// Tells whether this event is "about" this unit 
        /// </summary>
        /// <param name="unitID">unit identifier</param>
        /// <returns>True if unit is a predicate of event of the condition event it packages</returns>

        public Boolean Matches(ScenarioEventType incoming, string actionOrState)
        {
            /*
             * Note: This does not pay attention to any parameter other than the species -- i.e.,
             *  to which other units the action involves
             */
            Boolean returnValue = Genealogy.UnderSpecies(incoming.UnitID, this.species);
            if (returnValue)
            {
                returnValue = returnValue && (
                    (isAction && (action == actionOrState))
                    ||
                    (isStateChange && (newState == actionOrState))
                    );
            }
            return returnValue;
        }


        public void EnqueueEvents(string unitID)
        {
            ScenarioEventType performer;
            //     List<ScenarioEventType> newEventList = this.DoThisList;
            List<ScenarioEventType> newEventList = new List<ScenarioEventType>();
            for (int doThis = 0; doThis < this.DoThisList.Count; doThis++)
            {
                  performer = this.DoThisList[doThis];
               
                switch (performer.GetType().Name.ToString())
                {
                    case "OpenChatRoomType":
                    newEventList.Add(new OpenChatRoomType((OpenChatRoomType)(this.DoThisList[doThis])));
                    break;
                case "CloseChatRoomType":
                    newEventList.Add(new CloseChatRoomType((CloseChatRoomType)(this.DoThisList[doThis])));
                    break;
                case "OpenWhiteboardRoomType":
                    newEventList.Add(new OpenWhiteboardRoomType((OpenWhiteboardRoomType)(this.DoThisList[doThis])));
                    break;
                        /*
                case "AddChattersType":
                    newEventList.Add(new AddChattersType((AddChattersType)(this.DoThisList[doThis])));
                    break;
                case "DropChattersType":
                    newEventList.Add(new DropChattersType((DropChattersType)(this.DoThisList[doThis])));
                    break;
                        */
                case "Reveal_EventType":
                    newEventList.Add(new Reveal_EventType((Reveal_EventType)(this.DoThisList[doThis])));
                    break;
                case "SubplatfromLaunchType":
                    newEventList.Add(new SubplatformLaunchType((SubplatformLaunchType)(this.DoThisList[doThis])));
                    break;
                case "SubplatfromDockType":
                    newEventList.Add(new SubplatformDockType((SubplatformDockType)(this.DoThisList[doThis])));
                    break;
                case "WeaponLaunch_Eventype":
                    newEventList.Add(new WeaponLaunch_EventType((WeaponLaunch_EventType)(this.DoThisList[doThis])));
                    break;
                case "Move_EventType":
                    newEventList.Add(new Move_EventType((Move_EventType)(this.DoThisList[doThis])));
                    break;
                case "StateChangeEvent":
                    newEventList.Add(new StateChangeEvent((StateChangeEvent)(this.DoThisList[doThis])));
                    break;
                case "TransferEvent":
                    newEventList.Add(new TransferEvent((TransferEvent)(this.DoThisList[doThis])));
                    break;
                case "LaunchEventType":
                    newEventList.Add(new LaunchEventType((LaunchEventType)(this.DoThisList[doThis])));
                    break;
                case "WeaponLaunchEventType":
                    newEventList.Add(new WeaponLaunchEventType((WeaponLaunchEventType)(this.DoThisList[doThis])));
                    break;
                case "AttackObjectEvent":
                    newEventList.Add(new AttackObjectEvent((AttackObjectEvent)(this.DoThisList[doThis])));
                    break;
                case "ChangeEngramEvent":
                case "ChangeEngramType":
                    newEventList.Add(new ChangeEngramType((ChangeEngramType)(this.DoThisList[doThis])));
                    break;
                case "RemoveEngramEvent":
                    newEventList.Add(new RemoveEngramEvent((RemoveEngramEvent)(this.DoThisList[doThis])));
                    break;
  
  


                    default:
                        Coordinator.debugLogger.LogError("Scenario reader", "Unknown happening event type for " + performer.ToString());
                        break;
                }

      
     
            }
            for (int k = 0; k < newEventList.Count; k++)
            {
                ScenarioEventType newEvent = newEventList[k];
                if ("UNIT" == newEvent.UnitID)
                {
                    newEvent.UnitID = unitID;
                }
                int index = newEvent.AllUnits.IndexOf("UNIT");
                if (-1 < index)
                {
                    newEvent.AllUnits[index] = unitID;
                }
                newEvent.Time = (1 + newEvent.Time) + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/);
                TimerQueueClass.Add(newEvent.Time, newEvent);
            }
        }
        /// <summary>
        /// Creates a completion triggered  event
        /// </summary>

        public SpeciesCompletionType(pSpeciesCompletionType h)
            : base(1)
        {
            object performer;
            this.species = h.Species;
            this.isAction = false;
            this.isStateChange = false;
            if ("" != h.Action)
            {
                this.isAction = true;
            }
            this.action = h.Action;
            if ("" != h.NewState)
            {
                this.isStateChange = true;
            }
            this.newState = h.NewState;
            for (int doThis = 0; doThis < h.DoThisList.Count; doThis++)
            {
                performer = h.DoThisList[doThis];
                switch (performer.GetType().Name.ToString())
                {
                        // Chatropom manipulation is not supported here -- what would be the intent?
                    case "pMoveType":
                        this.doThisList.Add(new Move_EventType((pMoveType)performer));
                        /*
                         * this is not needed because  in ScenarioEvent constructor(?)
                                              if (!this.AllUnits.Contains(((pMoveType)performer).UnitID))
                                              {
                                                  this.AllUnits.Add(((pMoveType)performer).UnitID);
                                              }
                                              */
                        break;

                    case "pRevealType":
                        Reveal_EventType r = new Reveal_EventType((pRevealType)performer);
                        r.Genus = Genealogy.GetGenus(r.UnitID);
                        this.doThisList.Add(r);
                        break;
                    case "pStateChangeType":
                        this.doThisList.Add(new StateChangeEvent((pStateChangeType)performer));
                        break;
                    case "pDockType":
                        this.doThisList.Add(new StateChangeEvent((pStateChangeType)performer));
                        break;

                    case "pTransferType":
                        this.doThisList.Add(new TransferEvent((pTransferType)performer));
                        break;
                    case "pLaunchType":
                        this.doThisList.Add(new LaunchEventType((pLaunchType)performer));
                        if (!this.AllUnits.Contains(((pLaunchType)performer).Child))
                        {
                            this.AllUnits.Add(((pLaunchType)performer).Child);
                        }
                        break;
                    case "pWeaponLaunchType":
                        this.doThisList.Add(new WeaponLaunchEventType((pWeaponLaunchType)performer));
                        if (!this.AllUnits.Contains(((pWeaponLaunchType)performer).Child))
                        {
                            this.AllUnits.Add(((pWeaponLaunchType)performer).Child);
                        }
                        break;
                    case "pChangeEngramType":
                        this.doThisList.Add(new ChangeEngramType((pChangeEngramType)performer));
                        break;
                    case "pRemoveEngramType":
                        this.doThisList.Add(new RemoveEngramEvent((pRemoveEngramType)performer));
                        break;
                    case "pFlushEventType":
                        this.doThisList.Add(new FlushEvents((pFlushEventsType)performer));
                        break;
                    default:
                        Coordinator.debugLogger.LogError("Scenario reader", "Unknown happening event type for " + performer.ToString());
                        break;
                }
            }
        }
        /*
               public SpeciesCompletionType(ReiterateType r)
                   : base("", r.Start)
               {
                   this.isAction = true;
                   this.isStateChange = false;
                   this.NewState = "";
                   this.Range = r.Range;
                   this.Add(r.ReiterateList);
                   this.Action = "MoveComplete_Event";
                   /// when this is called the top element has already been shifted to the end
                   this.UnitID = r.ReiterateList[r.ReiterateList.Capacity - 1].UnitID;

               }
         * */
    }

    /// <summary>
    /// A Reiterate event is a list of actions to be performed one after the other
    /// </summary>
    public class ReiterateType : ScenarioEventType
    {
        /*       private int start;
               public int Start
               {
                   get
                   { return start; }
                   set
                   { start = value; }
               }
         * */
        private List<ScenarioEventType> reiterateList = new List<ScenarioEventType>();
        public List<ScenarioEventType> ReiterateList
        {
            get
            { return reiterateList; }

        }
        public void Add(ScenarioEventType s)
        {
            reiterateList.Add(s);
        }
        /// <summary>
        /// Tells whether this event is "about" this unit 
        /// </summary>
        /// <param name="unitID">unit identifier</param>
        /// <returns>True if unit is a predicate of event of one of the  events it packages</returns>

        public override Boolean Involves(string unitID)
        {
            Boolean returnValue = false;
            for (int i = 0; i < reiterateList.Count; i++)
            {
                returnValue = returnValue || reiterateList[i].Involves(unitID);
            }
            return returnValue;
        }



        /// <summary>
        /// Handles an infinite repeat
        /// </summary>

        public ReiterateType(pReiterateType r)
            : base("", r.Start)
        {
            object performer;
            this.Range = null;
            if (null != r.EngramRange)
            {
                this.Range = new EngramRange(r.EngramRange);
            }

            for (int reiterate = 0; reiterate < r.ReiterateList.Count; reiterate++)
            {
                performer = r.ReiterateList[reiterate];
                switch (performer.GetType().Name.ToString())
                {
                    case "pMoveType":
                        this.reiterateList.Add(new Move_EventType((pMoveType)performer));
                        /*
                         * this is not needed because  in ScenarioEvent constructor(?)
                                              if (!this.AllUnits.Contains(((pMoveType)performer).UnitID))
                                              {
                                                  this.AllUnits.Add(((pMoveType)performer).UnitID);
                                              }
                                              */
                        break;

                    default:
                        Coordinator.debugLogger.LogError("Scenario reader", "Unknown or incorrect reiteration event type for " + performer.ToString());
                        break;
                }
            }
        }
    }


    public class StateChangeEvent : ScenarioEventType
    {
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        private List<string> from = new List<string>();
        public List<string> From
        {
            get { return from; }
        }
        public Boolean HasPrecursors()
        {
            return (from.Count > 0);
        }
        public Boolean IsPrecursor(string s)
        {
            return from.Contains(s);
        }
        private List<string> except = new List<string>();
        public List<string> Except
        {
            get { return except; }
        }

        public Boolean HasExceptions()
        {
            return (except.Count > 0);
        }
        public Boolean IsException(string s)
        {
            return except.Contains(s);
        }
        public StateChangeEvent(StateChangeEvent s)
            : base(s.UnitID, s.Time)
        {
            {
                this.newState = s.NewState;
                for (int i = 0; i < s.From.Count; i++)
                {
                    this.from.Add(s.From[i]);
                }
                for (int i = 0; i < s.Except.Count; i++)
                {
                    this.except.Add(s.Except[i]);
                }

                if (null != s.Range)
                {
                    this.Range = new EngramRange(s.Range);
                }
            }
        }
        public StateChangeEvent(pStateChangeType s)
            : base(s.UnitID, s.Timer)
        {
            this.newState = s.NewState;
            for (int i = 0; i < s.From.Count; i++)
            {
                this.from.Add(s.From[i]);
            }
            for (int i = 0; i < s.Except.Count; i++)
            {
                this.except.Add(s.Except[i]);
            }

            if (null != s.EngramRange)
            {
                this.Range = new EngramRange(s.EngramRange);
            }
        }
    }

    public class TransferEvent : ScenarioEventType
    {
        private string from;
        public string From
        {
            get { return from; }
        }
        private string to;
        public string To
        {
            get { return to; }
           
        }
        public TransferEvent(TransferEvent t)
            : base(t.UnitID, t.Time)
        {
            this.from = t.From;
            this.to = t.To;
        }
        public TransferEvent(string unit, string from, string to):base(unit)
        {
            this.to = to;
            this.from = from;
        }
        public TransferEvent(pTransferType s)
            : base(s.UnitID, s.Timer)
        {
            this.from = s.From;
            this.to = s.To;
            this.Range = null;
            if (null != s.EngramRange)
            {
                this.Range = new EngramRange(s.EngramRange);
            }

        }
        public void Substitute(string unit, string target)
        {
            if ("UNIT" == this.to)
            {
                this.to = UnitFacts.GetDM(unit);
            }
            else if ("UNIT" == this.from)
            {
                this.from = UnitFacts.GetDM(unit);
            }
            if ("TARGET" == this.to)
            {
                this.to = UnitFacts.GetDM(target);
            }
            else
                if ("TARGET" == this.from)
                {
                    this.from = UnitFacts.GetDM(target);
                }
        }
    }

    public class LaunchEventType : ScenarioEventType
    {

        private string child = "";
        public string Child
        {
            get { return child; }
            set { child = value; } // to convert event before sending to EventCommunicator
        }
        private LocationType relativeLocation;
        public LocationType RelativeLocation
        {
            get { return relativeLocation; }
        }

        private string initialState = "FullyFunctional";
        public string InitialState
        {
            get { return initialState; }
            set { initialState = value; }
        }

        public LaunchEventType(LaunchEventType launch)
            : base(launch.UnitID, launch.Time)
        {
            child = launch.Child;

            //       Time = launch.Time;
            if ("" != child)
            {
                this.AddOtherUnit(child);
            }
            if (null != launch.Range)
            {
                this.Range = new EngramRange(launch.Range);
            }
            relativeLocation = new LocationType(launch.RelativeLocation.X, launch.RelativeLocation.Y, launch.RelativeLocation.Z);


        }
        public LaunchEventType(pLaunchType launch)
            :
            base(launch.Parent, launch.Time)
        {
            child = launch.Child;
            //       Time = launch.Time;
            if ("" != child)
            {
                this.AddOtherUnit(child);

            }
            else throw new ApplicationException("A unit must be named to launch from '" + launch.Parent + "'");
            if (null != launch.EngramRange)
            {
                this.Range = new EngramRange(launch.EngramRange);
            }
            relativeLocation = new LocationType(launch.RelativeLocation);


        }
    }


    public class WeaponLaunchEventType : ScenarioEventType
    {

        private string child = "";
        public string Child
        {
            get { return child; }
            set { child = value; } // to convert event before sending to EventCommunicator
        }
        private string target = "";
        public string Target
        {
            get { return target; }
            set { target = value; } // to convert event before sending to EventCommunicator
        }

        private string initialState = "FullyFunctional";
        public string InitialState
        {
            get { return initialState; }
            set { initialState = value; }
        }

        public WeaponLaunchEventType(WeaponLaunchEventType launch)
            : base(launch.UnitID, launch.Time)
        {
            child = launch.Child;

            //       Time = launch.Time;
            if ("" != child)
            {
                this.AddOtherUnit(child);
            }
            if (null != launch.Range)
            {
                this.Range = new EngramRange(launch.Range);
            }
            
            target = launch.Target;

        }
        public WeaponLaunchEventType(pWeaponLaunchType launch)
            :
            base(launch.Parent, launch.Time)
        {
            child = launch.Child;
            target = launch.Target;
            //       Time = launch.Time;
            if ("" != child)
            {
                this.AddOtherUnit(child);

            }
            else throw new ApplicationException("A unit must be named to launch from '" + launch.Parent + "'");
            if (null != launch.EngramRange)
            {
                this.Range = new EngramRange(launch.EngramRange);
            }
            


        }
    }
    /// <summary>
    /// Causes one unit to attack another
    /// </summary> 
    public class AttackObjectEvent : ScenarioEventType
    {
        private string targetObjectID;
        public string TargetObjectID
        {
            get { return targetObjectID; }
            set { targetObjectID = value; }
        }
        private string capabilityName;
        public string CapabilityName
        {
            get { return capabilityName; }
        }
        /// <summary>
        /// Constructs the attack command
        /// </summary>
        /// <param name="unitID">Identifier of the unit oding the attacking</param>
        /// <param name="targetObjectID">Identifier of the unit to be attacked</param>
        public AttackObjectEvent(AttackObjectEvent a)
            : base(a.UnitID, a.Time)
        {
            this.targetObjectID = a.TargetObjectID;
            this.capabilityName = a.CapabilityName;
        }
        
        public AttackObjectEvent(string unitID, string targetObjectID, string capabilityName)
            : base(unitID)
        {
            this.targetObjectID = targetObjectID;
            this.Time = TimerTicker.Timer;
            this.AddOtherUnit(targetObjectID);
            this.capabilityName = capabilityName;
        }
    }


    public class ChangeEngramEvent : ScenarioEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }

        private string engramValue;
        public string EngramValue
        {
            get { return engramValue; }
        }
        public ChangeEngramEvent(ChangeEngramEvent c)
            : base(c.Time)
        {
            this.name = c.Name;
            this.engramValue = c.EngramValue;
        }
        public ChangeEngramEvent(pChangeEngramType p)
            :
            base(p.Time)
        {
            this.name = p.Name;
            this.engramValue = p.EngramValue;
        }

    }

    public class ChangeEngramType : ScenarioEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        /*       private string unit = "";
               public string Unit
               {
                   get{return unit;}
               }*/
        private string engramValue;
        public string EngramValue
        {
            get { return engramValue; }
        }
        public ChangeEngramType(ChangeEngramType c)
            : base(c.Time)
        {
            this.name = c.Name;
            this.UnitID = c.UnitID;
            this.engramValue = c.EngramValue;
        }
        public ChangeEngramType(pChangeEngramType p)
            :
            base(p.Time)
        {
            this.name = p.Name;
            this.UnitID = p.Unit;
            this.engramValue = p.EngramValue;
        }

    }


    public class RemoveEngramEvent : ScenarioEventType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        public RemoveEngramEvent(RemoveEngramEvent r)
            : base(r.Time)
        {
            this.name = r.Name;
        }

        public RemoveEngramEvent(pRemoveEngramType p)
            :
            base(p.Time)
        {
            this.name = p.Name;

        }

    }
    public class FlushEvents : ScenarioEventType
    {

        public FlushEvents(pFlushEventsType p)
            : base(p.Unit, p.Time)
        {

        }
    }

    public class AttackSuccessfulCompletionType : ScenarioEventType
    {
        private string species = "";
        public string Species
        {
            get { return species; }
        }
        private string capability;
        public string Capability
        {
            get { return capability; }
        }
        private string targetSpecies = "";
        public string TargetSpecies
        {
            get { return targetSpecies; }
        }
        private string newState = "";
        public string NewState
        {
            get { return newState; }
        }
        private List<object> doThisList = new List<object>();
        public List<object> DoThisList
        {
            get { return doThisList; }
        }
        /*       private List<object> targetDoThisList = new List<object>();
               public List<object> TargetDoThisList
               {
                   get { return targetDoThisList; }
               }*/
        public Boolean Matches(AttackSuccessfulNotice incoming)
        {

            Boolean returnValue = Genealogy.UnderSpecies(incoming.UnitID, this.species);
            if (!this.EngramValid(incoming.UnitID, incoming.Target))
                returnValue = false;
            if (returnValue)
            {
                if (this.targetSpecies != "")
                {
                    if (!(Genealogy.UnderSpecies(incoming.Target, this.targetSpecies)))
                        returnValue = false;
                }
            }
            if (returnValue)
            {
                if (this.newState != "")
                {
                    if (this.newState != incoming.NewState)
                        returnValue = false;
                }
            }
            if (returnValue && (this.Capability != ""))
            {
                if (!incoming.Capabilities.Contains(this.capability))
                    returnValue = false;
            }
            if (returnValue && (this.Species != ""))
            {
                if (!(Genealogy.UnderSpecies(incoming.UnitID, this.Species)))
                    returnValue = false;

            }

            return returnValue;
        }
        public static void EnqueueEvents(string unitID, string target, List<object> doThese)
        {
            object performer;
            //     List<ScenarioEventType> newEventList = this.DoThisList;
            List<ScenarioEventType> newEventList = new List<ScenarioEventType>();
            for (int doThis = 0; doThis < doThese.Count; doThis++)
            {
                performer = doThese[doThis];

                switch (performer.GetType().Name.ToString())
                {
                    case "OpenChatRoomType":
                        newEventList.Add(new OpenChatRoomType((OpenChatRoomType)(doThese[doThis])));
                        break;
                    case "CloseChatRoomType":
                        newEventList.Add(new CloseChatRoomType((CloseChatRoomType)(doThese[doThis])));
                        break;
                    case "OpenWhiteboardRoomType":
                        newEventList.Add(new OpenWhiteboardRoomType((OpenWhiteboardRoomType)(doThese[doThis])));
                        break;
                    /*
            case "AddChattersType":
                newEventList.Add(new AddChattersType((AddChattersType)(this.DoThisList[doThis])));
                break;
            case "DropChattersType":
                newEventList.Add(new DropChattersType((DropChattersType)(this.DoThisList[doThis])));
                break;
                    */
                    case "Reveal_EventType":
                        newEventList.Add(new Reveal_EventType((Reveal_EventType)(doThese[doThis])));
                        break;
                    case "SubplatformLaunchType":
                        newEventList.Add(new SubplatformLaunchType((SubplatformLaunchType)(doThese[doThis])));
                        break;
                    case "SubplatformDockType":
                        newEventList.Add(new SubplatformDockType((SubplatformDockType)(doThese[doThis])));
                        break;
                    case "WeaponLaunch_EventType":
                        newEventList.Add(new WeaponLaunch_EventType((WeaponLaunch_EventType)(doThese[doThis])));
                        break;
                    case "Move_EventType":
                        newEventList.Add(new Move_EventType((Move_EventType)(doThese[doThis])));
                        break;
                    case "StateChangeEvent":
                        newEventList.Add(new StateChangeEvent((StateChangeEvent)(doThese[doThis])));
                        break;
                    case "TransferEvent":
                        TransferEvent te = new TransferEvent((TransferEvent)(doThese[doThis]));
                        te.Substitute(unitID, target);
                        newEventList.Add(te);
                        break;
                    case "LaunchEventType":
                        newEventList.Add(new LaunchEventType((LaunchEventType)(doThese[doThis])));
                        break;
                    case "AttackObjectEvent":
                        newEventList.Add(new AttackObjectEvent((AttackObjectEvent)(doThese[doThis])));
                        break;
                    case "ChangeEngramType":
                        newEventList.Add(new ChangeEngramType((ChangeEngramType)(doThese[doThis])));
                        break;
                    case "RemoveEngramEvent":
                        newEventList.Add(new RemoveEngramEvent((RemoveEngramEvent)(doThese[doThis])));
                        break;

                    default:
                        Coordinator.debugLogger.LogError("Scenario reader", "Unknown happening event type for " + performer.ToString());
                        break;
                }



            }
            for (int k = 0; k < newEventList.Count; k++)
            {
                ScenarioEventType newEvent = newEventList[k];
                if ("UNIT" == newEvent.UnitID)
                {
                    newEvent.UnitID = unitID;
                }
                else if ("TARGET" == newEvent.UnitID)
                {
                    newEvent.UnitID = target;
                }





                int index = newEvent.AllUnits.IndexOf("UNIT");
                if (-1 < index)
                {
                    newEvent.AllUnits[index] = unitID;
                }
                index = newEvent.AllUnits.IndexOf("TARGET");
                if (-1 < index)
                {
                    newEvent.AllUnits[index] = target;
                }
                if (null != newEvent.Range)
                {
                    EngramRange r = newEvent.Range;
                    if ("UNIT" == r.Unit)
                    {
                        r.Unit = unitID;
                    }
                    else if ("TARGET" == r.Unit)
                    {
                        r.Unit = target;
                    }

                }
                newEvent.Time = (1 + newEvent.Time) + (int)(TimerTicker.Timer / 1000/*Coordinator.UpdateIncrement*/);
                TimerQueueClass.Add(newEvent.Time, newEvent);
            }
        }


        public AttackSuccessfulCompletionType(pAttack_Successful_Completion_Type p)
            : base(1)
        {
            if (null != p.EngramRange)
            {
                this.Range = new EngramRange(p.EngramRange);
            }
            this.species = p.Species;
            this.capability = p.Capability;
            this.targetSpecies = p.TargetSpecies;
            this.newState = p.NewState;
            if (null != p.DoThisList)
            {
                for (int i = 0; i < p.DoThisList.Count; i++)
                {
                    object performer = p.DoThisList[i];
                    switch (performer.GetType().Name.ToString())
                    {
                        // Chatropom manipulation is not supported here -- what would be the intent?
                        case "pMoveType":
                            this.doThisList.Add(new Move_EventType((pMoveType)performer));
                            /*
                             * this is not needed because  in ScenarioEvent constructor(?)
                                                  if (!this.AllUnits.Contains(((pMoveType)performer).UnitID))
                                                  {
                                                      this.AllUnits.Add(((pMoveType)performer).UnitID);
                                                  }
                                                  */
                            break;

                        case "pRevealType":
                            Reveal_EventType r = new Reveal_EventType((pRevealType)performer);
                            r.Genus = Genealogy.GetGenus(r.UnitID);
                            this.doThisList.Add(r);
                            break;
                        case "pStateChangeType":
                            this.doThisList.Add(new StateChangeEvent((pStateChangeType)performer));
                            break;
                        case "pDockType":
                            this.doThisList.Add(new StateChangeEvent((pStateChangeType)performer));
                            break;

                        case "pTransferType":
                            this.doThisList.Add(new TransferEvent((pTransferType)performer));
                            break;
                        case "pLaunchType":
                            this.doThisList.Add(new LaunchEventType((pLaunchType)performer));
                            if (!this.AllUnits.Contains(((pLaunchType)performer).Child))
                            {
                                this.AllUnits.Add(((pLaunchType)performer).Child);
                            }
                            break;
                        case "pWeaponLaunchType":
                            this.doThisList.Add(new WeaponLaunchEventType((pWeaponLaunchType)performer));
                            if (!this.AllUnits.Contains(((pWeaponLaunchType)performer).Child))
                            {
                                this.AllUnits.Add(((pWeaponLaunchType)performer).Child);
                            }
                            break;
                        case "pChangeEngramType":
                            this.doThisList.Add(new ChangeEngramType((pChangeEngramType)performer));
                            break;
                        case "pRemoveEngramType":
                            this.doThisList.Add(new RemoveEngramEvent((pRemoveEngramType)performer));
                            break;
                        case "pFlushEventType":
                            this.doThisList.Add(new FlushEvents((pFlushEventsType)performer));
                            break;
                        default:
                            Coordinator.debugLogger.LogError("Scenario reader", "Unknown happening event type for " + performer.ToString());
                            break;
                    }
                }
            }
        }
    }

    public class AttackRequestApprovalType : ScenarioEventType
    {
        private string capability;
        public string Capability
        {
            get { return capability; }
        }
        private Boolean actorIsSpecies;
        public Boolean ActorIsSpecies
        {
            get { return actorIsSpecies; }
        }
        private string actor;
        public string Actor
        {
            get { return actor; }
        }
        private Boolean targetIsSpecies;
        public Boolean TargetIsSpecies
        {
            get { return targetIsSpecies; }
        }
        private string target;
        public string Target
        {
            get { return target; }
        }
        private List<string> targetStates;
        public List<string> TargetStates
        {
            get { return targetStates; }
        }
        private Boolean useDefault = true;
        public Boolean UseDefault
        {
            get { return useDefault; }
        }

        private string failure;
        public string Failure
        {
            get { return failure; }
        }
        public AttackRequestApprovalType(pAttack_Request_Approval_Type para)
            : base(1)
        {
            this.capability = para.Capability;
            this.actor = para.Actor;
            this.actorIsSpecies = para.ActorIsSpecies;
            this.target = para.Target;
            this.targetIsSpecies = para.TargetIsSpecies;
            this.targetStates = para.TargetStates;
            this.useDefault = para.UseDefault;
            this.failure = para.Failure;
            if (null != para.EngramRange)
                this.Range = new EngramRange(para.EngramRange);


        }
    }


    /// <summary>
    /// A Random Seed
    /// </summary>
    public class RandomSeedType : ScenarioEventType
    {
        public int seed;

        public RandomSeedType()
            : base(null)
        {
            Random r = new Random();
            seed = r.Next();
        }
    }

    public class ClientSideRangeRingVisibilityType : ScenarioEventType
    {
        public string clientSideRangeRingVisibility = "Full";

        public ClientSideRangeRingVisibilityType(string visibility)
            :base(0)
        {
            clientSideRangeRingVisibility = visibility;
        }
        public ClientSideRangeRingVisibilityType()
            : base(0)
        {
            clientSideRangeRingVisibility = "Full"; //default value; enum is: Disabled, Private, SharedSensorNetwork, Full
        }
    }

    public class ClientSideAssetTransferType : ScenarioEventType
    {
        public bool assetTransferEnabled;

        public ClientSideAssetTransferType(bool enabled)
            :base(0)
        {
            assetTransferEnabled = enabled;
        }
        public ClientSideAssetTransferType()
            : base(0)
        {
            assetTransferEnabled = true;
        }
    }
    public class ClientSideStartingLabelVisibleType : ScenarioEventType
    {
        public bool labelsVisible;

        public ClientSideStartingLabelVisibleType(bool enabled)
            : base(0)
        {
            labelsVisible = enabled;
        }
        public ClientSideStartingLabelVisibleType()
            : base(0)
        {
            labelsVisible = true;
        }
    }

    public class ClassificationsType : ScenarioEventType
    {
        public List<String> classifications;

        public ClassificationsType(List<String> classifications)
            : base(0)
        {
            this.classifications = classifications;
        }
        public ClassificationsType()
            : base(0)
        {
            classifications = new List<string>();
        }
    }
    public class SendChatMessageType : ScenarioEventType
    {
        private string roomName = "";
        public string RoomName
        {
            get { return roomName; }
        }
        private string sender = "EXP";
        public string Sender
        {
            get { return sender; }
        }

        private string message = "";
        public string Message
        {
            get { return message; }
        }

        public SendChatMessageType(pSendChatMessageType pScm):base(pScm.Time)
        {
            this.roomName = pScm.RoomName;
            if (pScm.Sender != "")
            this.sender = pScm.Sender;
            this.message = pScm.Message;
        }
    }

    public class ApplyType : ScenarioEventType
    {
        private string fromDM = "";
        private string toDM = "";
        private EngramRange engramRange = null;
        public EngramRange EngramRange
        {
            get { return engramRange; }
        }
        public string FromDM
        {
            get { return fromDM; }
        }
        public string ToDM
        {
            get { return toDM; }
        }

        public ApplyType(pApplyType pAT)
            : base(pAT.Time)
        {
            fromDM = pAT.FromDM;
            toDM = pAT.ToDM;
            if (null != pAT.EngramRange)
                this.engramRange = new EngramRange(pAT.EngramRange);
        }
    }

    public class SendVoiceMessageType : ScenarioEventType
    {
        private string channelName = "";
        private string filePath = "";
        public string ChannelName
        {
            get { return channelName; }
        }
        public string FilePath
        {
            get { return filePath; }
        }
        public SendVoiceMessageType(pSendVoiceMessageType pPVMT) : base(pPVMT.Time)
        {
            this.channelName = pPVMT.ChannelName;
            this.filePath = pPVMT.FilePath;
        }
    }

    public class SendVoiceMessageToUserType : ScenarioEventType
    {
        private string decisionMakerID = "";
        private string filePath = "";
        public string DecisionMakerID
        {
            get { return decisionMakerID; }
        }
        public string FilePath
        {
            get { return filePath; }
        }
        public SendVoiceMessageToUserType(pSendVoiceMessageToUserType pPVMT)
            : base(pPVMT.Time)
        {
            this.decisionMakerID = pPVMT.DecisionMakerID;
            this.filePath = pPVMT.FilePath;
        }
    }

    public class ForkReplayEventType : ScenarioEventType
    {
        SimulationEvent m_event;

        public ForkReplayEventType(SimulationEvent ev)
            : base(((IntegerValue)ev["Time"]).value/1000)
        {
            m_event = ev;
        }

        public SimulationEvent Event
        {
            get { return m_event; }
        }
    }

}
