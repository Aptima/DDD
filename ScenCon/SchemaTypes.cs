using System;
using System.Collections.Generic;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.ScenarioParser
{

    public class pNoteType
    {
        public string content;
        private string Content
        {
            get { return content; }
        }
        public pNoteType(string content)
        {
            this.content = content;
        }
    }

    /// <summary>
    /// Event describing the playfield
    /// </summary>
    public class pPlayfieldType
    {
        private string mapFileName;
        public string MapFileName
        {
            get
            { return this.mapFileName; }

        }
        private string iconLibrary;
        public string IconLibrary
        {
            get { return iconLibrary; }
        }
        private string uTMZone;
        public string UTMZone
        {
            get
            { return this.uTMZone; }


        }
        private double verticalScale;
        public double VerticalScale
        {
            get
            { return this.verticalScale; }

        }
        private double horizontalScale;
        public double HorizontalScale
        {
            get
            { return this.horizontalScale; }


        }
        private string displayLabels;
        public string DisplayLabels
        {
            get { return displayLabels; }
        }
        private string displayTags;
        public string DisplayTags
        {
            get
            { return this.displayTags; }


        }
        public pPlayfieldType(string mapFileName, string iconLibrary, string uTMZone,
            double northing, double easting, string displayLabels, string displayTags)
        {
            this.mapFileName = mapFileName;
            this.iconLibrary = iconLibrary;
            this.uTMZone = uTMZone;
            this.verticalScale = northing;
            this.horizontalScale = easting;
            this.displayLabels = displayLabels;
            this.displayTags = displayTags;
        }
    }
    public class pLandRegionType
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private List<pPointType> vertices;
        public List<pPointType> Vertices
        {
            get { return vertices; }
        }

        public void Add(pPointType p)
        {
            vertices.Add(p);
        }
        private pLocationType referencePoint;
        public pLocationType ReferencePoint
        {
            get { return referencePoint; }
            set { referencePoint = value; }
        }
        private double scale;
        public double Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        private double rotation;
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public pLandRegionType(string id)
        {
            this.id = id;
            vertices = new List<pPointType>();

        }

    }

    public class pActiveRegionType
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private List<pPointType> vertices;
        public List<pPointType> Vertices
        {
            get { return vertices; }
        }
        public double start;
        public double Start
        {
            get { return start; }
            set { start = value; }
        }
        public double end;
        public double End
        {
            get { return end; }
            set { end = value; }
        }
        private double speedMultiplier = 1.0;
        public double SpeedMultiplier
        {
            get { return speedMultiplier; }
            set { speedMultiplier = value; }
        }
        private Boolean blocksMovement = false;
        public Boolean BlocksMovement
        {
            get { return blocksMovement; }
            set { blocksMovement = value; }
        }

        private List<string> sensorsBlocked;
        public List<string> SensorsBlocked
        {
            get { return sensorsBlocked; }
        }
        private Boolean isVisible = true;
        public Boolean IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private Boolean isActive = true;
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private String obstructedViewImage = "";
        public String ObstructedViewImage
        {
            get { return obstructedViewImage; }
            set { obstructedViewImage = value; }
        }
        private double obstructionOpacity = 0.0;
        public double ObstructionOpacity
        {
            get { return obstructionOpacity; }
            set { obstructionOpacity = value; }
        }
        private string color = "Transparent";
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        public void Add(pPointType p)
        {
            vertices.Add(p);
        }
        public void Add(List<string> sensorNames)
        {
            this.sensorsBlocked = sensorNames;
        }
        private pLocationType referencePoint;
        public pLocationType ReferencePoint
        {
            get { return referencePoint; }
            set { referencePoint = value; }
        }
        private bool isDynamicRegion;
        public bool IsDynamicRegion
        {
            get { return isDynamicRegion; }
            set { isDynamicRegion = value; }
        }
        private double scale;
        public double Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        private double rotation;
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public pActiveRegionType(string id)
        {
            this.id = id;
            vertices = new List<pPointType>();
            sensorsBlocked = new List<string>();

        }

    }
    public class pTeamType
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
        public void Add(string newFoe)
        {
            if (!against.Contains(newFoe))
            {
                against.Add(newFoe);
            }
        }
        public pTeamType(string name)
        {
            this.name = name;
            against = new List<string>();
        }
    }
    /// Identification of a decision maker
    /// </summary>
    public class pDecisionMakerType
    {
        private string role;
        public string Role
        {
            get { return role; }
            //           set { dMRole = value; }
        }
        private string identifier;
        public string Identifier
        {
            get { return identifier; }
            //          set { dMIdentifier = value; }
        }
        private string color;
        public string Color
        {
            get { return color; }
        }
        private string briefing;
        public string Briefing
        {
            get { return briefing; }
        }
        private List<string> reportsTo = new List<string>();
        public List<string> ReportsTo
        {
            get { return reportsTo; }
        }
        private Boolean canTransfer = false;
        public Boolean CanTransfer
        {
            get { return canTransfer; }
        }
        private Boolean canForceTransfers = false;
        public Boolean CanForceTransfers
        {
            get { return canForceTransfers; }
        }
        private string team;
        public string Team
        {
            get { return team; }
        }
        /*
        private List<string> initialVoiceAccess = new List<string>();
        public List<string> InitialVoiceAccess
        {
            get { return initialVoiceAccess; }
        }
        public void AddVoiceChannelAccess(List<string> vP)
        {
            for (int i = 0; i < vP.Count; i++)
                initialVoiceAccess.Add(vP[1]);
        }
         */
        private List<string> chatPartners = new List<string>();
        public List<string> ChatPartners
        {
            get { return chatPartners; }
        }
        public void AddChatPartners(List<string> cP)
        {
            for (int i = 0; i < cP.Count; i++)
                chatPartners.Add(cP[1]);
        }

        private List<string> whiteboardPartners = new List<string>();
        public List<string> WhiteboardPartners
        {
            get { return whiteboardPartners; }
        }
        public void AddWhiteboardPartners(List<string> cP)
        {
            for (int i = 0; i < cP.Count; i++)
                whiteboardPartners.Add(cP[1]);
        }

        private List<string> voicePartners = new List<string>();
        public List<string> VoicePartners
        {
            get { return voicePartners; }
        }
        public void AddVoicePartners(List<string> cP)
        {
            for (int i = 0; i < cP.Count; i++)
                voicePartners.Add(cP[1]);
        }

        private Boolean isObserver = false;
        public Boolean IsObserver
        {
            get { return isObserver; }
        }
        /// <summary>
        /// Event identifying decision maker
        /// </summary>
        /// <param name="role">Functional role taken by this player</param>
        /// <param name="identification">Identification for this player</param>
        /*
        public pDecisionMakerType(string role, string identifier, string color, string briefing, List<string> reportsTo, Boolean canTransfer, Boolean canForceTransfers, string team, List<string>voiceChannelAccess,List<string> chatPartners)
        {
            this.role = role;
            this.identifier = identifier;
            this.color = color;
            this.briefing = briefing;
            this.reportsTo = reportsTo;
            this.canTransfer = canTransfer;
            this.canForceTransfers = canForceTransfers;
            this.team = team;
           // this.initialVoiceAccess = voiceChannelAccess;
            this.chatPartners = chatPartners;
        }
 */

         /*4.1*/   public pDecisionMakerType(string role, string identifier, string color, string briefing, List<string> reportsTo, Boolean canTransfer, Boolean canForceTransfers, string team,  List<string> chatPartners, List<string> whiteboardPartners, List<String> voicePartners, Boolean isObserver)
        {
            this.role = role;
            this.identifier = identifier;
            this.color = color;
            this.briefing = briefing;
            this.reportsTo = reportsTo;
            this.canTransfer = canTransfer;
            this.canForceTransfers = canForceTransfers;
            this.team = team;
            this.chatPartners = chatPartners;
            this.whiteboardPartners = whiteboardPartners;
            this.voicePartners = voicePartners;
            this.isObserver = isObserver;
        }
     /*4.0*/  public pDecisionMakerType(string role, string identifier, string color, string briefing, string team)
        {// needed below DDD4
            this.role = role;
            this.identifier = identifier;
            this.color = color;
            this.briefing = briefing;
            this.team = team;
        }
        /*        public pDecisionMakerType(string role, string identifier)
                {
                    this.role = role;
                    this.identifier = identifier;
                    this.team = "";
                }*/

    }
    public class pNetworkType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private List<string> members;
        public string this[int index]
        {
            get
            {
                try
                {
                    return members[index];
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Indexing Failure in pDMNetwork.get"
                        + "for Network " + this.name + ":" + e.Message.ToString());
                }
            }
        }
        public int Count()
        {
            if (null != members)
            {
                return members.Count;
            }
            return 0;
        }
        public void Add(string s)
        {
            members.Add(s);
        }

        public pNetworkType()
        {
            members = new List<string>();
        }
        public pNetworkType(string s)
        {
            members = new List<string>();
            name = s;
        }
    }

    /// <summary>
    /// Represents an (x,y,z) vector
    /// </summary>
    public class pLocationType
    {
        private double x;
        public double X
        {
            get
            { return this.x; }
            set
            { this.x = value; }
        }
        private double y;
        public double Y
        {
            get
            { return this.y; }
            set
            { this.y = value; }
        }
        private double z;
        public double Z
        {
            get
            { return this.z; }
            set
            { this.z = value; }
        }

        public override string ToString()
        {
            return "(" + this.x.ToString() + "," + this.y.ToString() + "," + this.z.ToString() + ")";
        }

        /// <summary>
        /// Constructs an (x,y,z) vector
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        /// 
        public pLocationType(pLocationType location)
        {
            this.x = location.X;
            this.y = location.Y;
            this.z = location.Z;
        }
        public pLocationType(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// Constructs a default vector (0,0,0)
        /// </summary>
        public pLocationType()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Changes the value of a vector
        /// </summary>
        /// <param name="x">New x value</param>
        /// <param name="y">New y value</param>
        /// <param name="z">New z value</param>
        public void NewLocation(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public class pVelocityType
    {
        private double vx;
        public double VX
        {
            get
            { return this.vx; }
            set
            { this.vx = value; }
        }
        private double vy;
        public double VY
        {
            get
            { return this.vy; }
            set
            { this.vy = value; }
        }
        private double vz;
        public double VZ
        {
            get
            { return this.vz; }
            set
            { this.vz = value; }
        }

        public override string ToString()
        {
            return "(" + this.vx.ToString() + "," + this.vy.ToString() + "," + this.vz.ToString() + ")";
        }

        /// <summary>
        /// Constructs a velocity vector
        /// </summary>
        /// <param name="vx">X value</param>
        /// <param name="vy">Y value</param>
        /// <param name="vz">Z value</param>
        public pVelocityType(double vx, double vy, double vz)
        {
            this.vx = vx;
            this.vy = vy;
            this.vz = vz;
        }
        /// <summary>
        /// Constructs a default vector (0,0,0)
        /// </summary>
        public pVelocityType()
        {
            this.vx = 0;
            this.vy = 0;
            this.vz = 0;
        }
        /// <summary>
        /// Changes the value of a vector
        /// </summary>
        /// <param name="vx">New x value</param>
        /// <param name="vy">New y value</param>
        /// <param name="vz">New z value</param>
        public void NewLocation(double vx, double vy, double vz)
        {
            this.vx = vx;
            this.vy = vy;
            this.vz = vz;
        }
    }
    public class pCone
    {
        private double spread;
        public double Spread
        {
            get { return spread; }
            set { spread = value; }
        }
        private double extent;
        public double Extent
        {
            get { return extent; }
            set { extent = value; }
        }
        private pLocationType direction;
        public pLocationType Direction
        {
            get { return direction; }
        }
        private string level;
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        public pCone(double spread, double extent, pLocationType direction, string level)
        {
            this.spread = spread;
            this.extent = extent;
            this.direction = new pLocationType(direction);
            this.level = level;
        }

    }
    public class pSensor
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private string attribute;
        public string Attribute
        {
            get { return attribute; }
        }
        private Boolean isEngram = false;
        public Boolean IsEngram
        {
            get { return isEngram; }
        }
        private string typeIfEngram;
        public string TypeIfEngram
        {
            get { return typeIfEngram; }
        }
        private List<pCone> cones = new List<pCone>();
        public List<pCone> Cones
        {
            get { return cones; }
            set { cones = value; }
        }
        public void Add(pCone cone)
        {
            cones.Add(cone);
        }
        public pSensor(string name, string attribute)
        {
            this.name = name;
            this.attribute = attribute;
        }
        public pSensor(string name, string attribute, Boolean isEngram, string typeIfEngram)
        {
            this.name = name;
            this.attribute = attribute;
            this.isEngram = isEngram;
            this.typeIfEngram = typeIfEngram;


        }
    }
    public class pEmitterType
    {
        private string attribute;
        public string Attribute
        {
            get { return attribute; }
        }
        private Boolean isEngram;
        public Boolean IsEngram
        {
            get { return isEngram; }
        }
        private string typeIfEngram = "string";
        public string TypeIfEngram
        {
            get { return typeIfEngram; }
        }
        private Dictionary<string, double> levels;
        public Dictionary<string, double> Levels
        {
            get { return levels; }
        }
        public pEmitterType(string attribute, Dictionary<string, double> levels)
        {
            this.attribute = attribute;
            this.levels = levels;

        }

        public pEmitterType(string attribute, Boolean isEngram, string typeIfEngram, Dictionary<string, double> levels)
        {
            this.attribute = attribute;
            this.isEngram = isEngram;
            this.typeIfEngram = typeIfEngram;
            this.levels = levels;

        }
    }
    public class pStateBody
    {
        private string icon = null;
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private double? launchDuration = null;
        public double? LaunchDuration
        {
            get { return launchDuration; }
            set { launchDuration = value; }
        }
        private double? dockingDuration = null;
        public double? DockingDuration
        {
            get { return dockingDuration; }
            set { dockingDuration = value; }
        }

        /*       private int? timeToRemove = null;
            public int? TimeToRemove
            {
                get { return timeToRemove; }
                set { timeToRemove = value; }

            }
         */
        private int? timeToAttack = null;
        public int? TimeToAttack
        {
            get { return timeToAttack; }
            set { timeToAttack = value; }

        }

        private int? engagementDuration = null;
        public int? EngagementDuration
        {
            get { return engagementDuration; }
            set { engagementDuration = value; }

        }

        private double? maximumSpeed = null;
        public double? MaximumSpeed
        {
            get { return maximumSpeed; }
            set { maximumSpeed = value; }
        }
        private double? fuelCapacity = null;
        public double? FuelCapacity
        {
            get { return fuelCapacity; }
            set { fuelCapacity = value; }
        }

        private double? initialFuelLoad = null;
        public double? InitialFuelLoad
        {
            get { return initialFuelLoad; }
            set { initialFuelLoad = value; }
        }

        private double? fuelConsumptionRate = null;
        public double? FuelConsumptionRate
        {
            get { return fuelConsumptionRate; }
            set { fuelConsumptionRate = value; }
        }


        private string fuelDepletionState = null;
        public string FuelDepletionState
        {
            get { return fuelDepletionState; }
            set { fuelDepletionState = value; }
        }
        private Boolean? stealable;
        public Boolean? Stealable
        {
            get { return stealable; }
            set { stealable = value; }
        }

        private List<string> sensors = new List<string>();
        public List<string> Sensors
        {
            get { return sensors; }
            set { sensors = value; }
        }

        private Dictionary<string, pCapabilityType> capabilities = new Dictionary<string, pCapabilityType>();
        public Dictionary<string, pCapabilityType> Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }

        private Dictionary<string, pSingletonVulnerabilityType> vulnerabilities = new Dictionary<string, pSingletonVulnerabilityType>();
        public Dictionary<string, pSingletonVulnerabilityType> Vulnerabilities
        {
            get { return vulnerabilities; }
            set { vulnerabilities = value; }
        }
        private List<pComboVulnerabilityType> combinations = new List<pComboVulnerabilityType>();
        public List<pComboVulnerabilityType> Combinations
        {
            get { return combinations; }
            set { combinations = value; }
        }


        private Dictionary<string, pEmitterType> emitters = new Dictionary<string, pEmitterType>();
        public Dictionary<string, pEmitterType> Emitters
        {
            get { return emitters; }
            set { emitters = value; }
        }

        public pStateBody()
        {

        }
        /*
         * public pStateBody(string icon, List<string> sensors,
                   Dictionary<string, pCapabilityType> capabilities,
                   Dictionary<string, pSingletonVulnerabilityType> vulnerabilities,
                   List<pComboVulnerabilityType> combinations,
                   Dictionary<string, object> settings,
               Dictionary<string, pEmitterType> emitters)
               {
                   this.icon = icon;
                   this.sensors = sensors;
                   this.capabilities = capabilities;
                   this.vulnerabilities = vulnerabilities;
                   this.combinations = combinations;
                   this.settings = settings;
                   this.emitters = emitters;
               }
         * */
    }

    /// <summary>
    /// A state type is used internally to carry the components of a state
    /// </summary>
    public class pStateType
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private pStateBody body;
        public pStateBody Body
        {
            get { return body; }
        }



        public pStateType(string name)
        {
            this.name = name;
            this.body = new pStateBody();

        }
        public pStateType(string name, pStateBody body)
        {
            this.name = name;
            this.body = body;
        }
    }
    public class pTransitionType
    {

        private int effect;
        public int Effect
        {
            get { return effect; }
        }
        private double range = -1;
        public double Range
        {
            get { return range; }
        }
        private double probability = 1.0;
        public double Probability
        {
            get { return probability; }
        }

        private string state;
        public string State
        {
            get { return state; }
        }


        public pTransitionType(int effect, double range, double probability, string state)
        {
            this.effect = effect;
            this.range = range;
            this.probability = probability;

            this.state = state;
        }
    }
    public class pSingletonVulnerabilityType
    {
        private List<pTransitionType> transitions;
        public List<pTransitionType> Transitions
        {
            get { return transitions; }
        }
        public pSingletonVulnerabilityType(List<pTransitionType> transitions)
        {

            this.transitions = transitions;
        }

    }
    public class pContributionType
    {
        private string capability;
        public string Capability
        {
            get { return capability; }
        }
        private int effect;
        public int Effect
        {
            get { return effect; }
        }
        private double range = 0;
        public double Range
        {
            get { return range; }
        }
        private double probability = 1.0;
        public double Probability
        {
            get { return probability; }
        }
        public pContributionType(string capability, int effect, double range, double probability)
        {
            this.capability = capability;
            this.effect = effect;
            this.range = range;
            this.probability = probability;
        }

    }
    public class pComboVulnerabilityType
    {
        private List<pContributionType> contributions;
        public List<pContributionType> Contributions
        {
            get { return contributions; }
        }
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        public pComboVulnerabilityType(List<pContributionType> contributions, string newState)
        {
            this.contributions = contributions;
            this.newState = newState;
        }
    }


    public class pPointType
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
        public pPointType(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class pRandomIntervalType
    {
        private int start;
        public int Start
        {
            get { return start; }
            set { start = value; }
        }
        private int end;
        public int End
        {
            get { return end; }
            set { end = value; }
        }
        public pRandomIntervalType(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    /*    public class pDmTransitionType
        {
            private string dmRole;
            public string DmRole
            {
                get { return DmRole; }
            }
            private string dmName;
            public string DmName
            {
                get { return DmName; }
            }
            private int dmTimer;
            public int DmTimer
            {
                get { return dmTimer; }
            }

            public pDmTransitionType(int timer, string name, string number)
            {
                dmTimer = timer;
                dmRole = name;
                dmName = number;
            }
        }
    
      */
    /// <summary>
    /// Information about a genus as taken from the scenario file
    /// </summary>
    public class pGenusType
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public pGenusType(string name)
        {
            this.name = name;


        }


    }


    /// <summary>
    /// Information about a Weapon 
    /// </summary>
    public class pWeaponType //obsolete in 4.1
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Dictionary<string, pStateBody> states;
        public Dictionary<string, pStateBody> States
        {
            get { return states; }
            set { states = value; }
        }
        public void AddState(pStateType state)
        {
            this.states[state.Name] = state.Body;
        }


        public pWeaponType(string name, Dictionary<string, pStateBody> states)
        {
            this.name = name;
            this.states = states;

        }
    }
    public class pArmamentType //obsolete in 4.1
    {
        private string arm;
        public string Arm
        {
            get { return arm; }
        }
        private int count;
        public int Count
        {
            get { return count; }
        }
        public pArmamentType(string weapon, int count)
        {
            this.arm = weapon;
            this.count = count;
        }
    }
    public class pDockedPlatformType //onbsolete in 4.1
    {
        private int count;
        public int Count
        {
            get { return count; }
        }


        public pDockedPlatformType(int count)
        {
            this.count = count;


        }
    }
    public class pSubplatformType// Obsolete in 4.1
    {
        private string kind;
        public string Kind
        {
            get { return kind; }
        }
        private List<pArmamentType> arms;
        public List<pArmamentType> Arms
        {
            get { return arms; }
        }
        private List<pDockedPlatformType> docked;
        public List<pDockedPlatformType> Docked
        {
            get { return docked; }
        }
        private List<pLaunchedPlatformType> launched;
        public List<pLaunchedPlatformType> Launched
        {
            get { return launched; }
        }
        public void Add(pArmamentType a)
        {
            arms.Add(a);
        }
        public void Add(pDockedPlatformType d)
        {
            docked.Add(d);
        }
        public void Add(pLaunchedPlatformType lau)
        {
            launched.Add(lau);
        }
        public pSubplatformType(string kind)
        {
            this.kind = kind;
            arms = new List<pArmamentType>();
            docked = new List<pDockedPlatformType>();
            launched = new List<pLaunchedPlatformType>();
        }
    }



    /// <summary>
    /// Information about a species taken from the scenario file
    /// </summary>
    public class pSpeciesType
    {
        public class SubplatformCapacity
        {
            public SubplatformCapacity(String speciesName, int count)
            {
                m_speciesName = speciesName;
                m_count = count;
            }

            private string m_speciesName;
            public string SpeciesName
            {
                get { return m_speciesName; }
                set { m_speciesName = value; }
            }
            private int m_count;
            public int Count
            {
                get { return m_count; }
                set { m_count = value; }
            }
        }

        private List<SubplatformCapacity> m_subplatformCapacities;
        public List<SubplatformCapacity> SubplatformCapacities
        {
            get { return m_subplatformCapacities; }
            set { m_subplatformCapacities = value; }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string basedOn;
        public string BasedOn
        {
            get { return basedOn; }
            set { basedOn = value; }
        }
        // Note: with collisions disabled size has become vestigial.
        private double? size = null;
        public double? Size
        {
            get { return size; }
            set { size = value; }
        }

        private Boolean? isWeapon = null;
        public Boolean? IsWeapon
        {
            get { return isWeapon; }
            set { isWeapon = value; }
        }

        private String defaultClassification = String.Empty;
        public String DefaultClassification
        {
            get { return defaultClassification; }
            set { defaultClassification = value; }
        }

        private ClassificationDisplayRulesValue classificationDisplayRules;

        public ClassificationDisplayRulesValue ClassificationDisplayRules
        {
            get { return classificationDisplayRules; }
            set { classificationDisplayRules = value; }
        }

        private Boolean? removeOnDestruction = null;
        public Boolean? RemoveOnDestruction
        {
            get { return removeOnDestruction; }
            set { removeOnDestruction = value; }
        }

        private Boolean anyoneCanOwn = true;
        public Boolean AnyoneCanOwn
        {
            get { return anyoneCanOwn; }
        }
        private List<string> canOwn = new List<string>();
        public List<string> CanOwn
        {
            get { return canOwn; }
        }
        //private int subplatformLimit = -1;// to allow for distinguishing between value 0 and value not given
        //public int SubplatformLimit
        //{
        //    get { return subplatformLimit; }
        //    set { subplatformLimit = value; }
        //}
        //private List<string> subplatforms = new List<string>();// those that this species can host
        //public List<string> Subplatforms
        //{
        //    get { return subplatforms; }
        //}
        //public void AddSubplatforms(List<string> subList)
        //{
        //    for (int i = 0; i < subList.Count; i++)
        //        subplatforms.Add(subList[i]);
        //}

        public void AddSubplatformCapacity(String speciesName, int count)
        {
            SubplatformCapacities.Add(new SubplatformCapacity(speciesName, count));
        }
        public void AddOwners(List<string> dMList)
        {
            if (dMList.Count > 0)
            {
                for (int i = 0; i < dMList.Count; i++)
                    canOwn.Add(dMList[i]);
                anyoneCanOwn = false;
            }
        }
        public List<string> GetOwners()
        {
            return canOwn;
        }
        public int CountOwners()
        {
            return canOwn.Count;
        }
        private Boolean launchedByOwner = false; //otherwsie launched by platfom owner
        public Boolean LaunchedByOwner
        {
            get { return launchedByOwner; }
            set { launchedByOwner = value; }
        }
        private Dictionary<string, pStateBody> states = new Dictionary<string, pStateBody>();
        public Dictionary<string, pStateBody> States
        {
            get { return states; }
            //          set { states = value; }
        }


        public void AddState(pStateType state)
        {
            this.states[state.Name] = state.Body;
        }


        public pSpeciesType()
        {
            m_subplatformCapacities = new List<SubplatformCapacity>();

        }
    }

    public class pEffectType
    {
        private int intensity;
        public int Intensity
        {
            get { return intensity; }
        }
        private double probability;
        public double Probability
        {
            get { return probability; }
        }
        public pEffectType(int intensity, double probability)
        {
            this.intensity = intensity;
            this.probability = probability;
        }
    }
    public class pProximityType
    {
        private double range;
        public double Range
        {
            get { return range; }
        }
        private List<pEffectType> effectList;
        public List<pEffectType> EffectList
        {
            get { return effectList; }
        }
        public pProximityType(double range)
        {
            this.range = range;
            this.effectList = new List<pEffectType>();
        }
        public void Add(pEffectType effect)
        {
            effectList.Add(effect);
        }
    }
    public class pCapabilityType
    {

        private List<pProximityType> proximityList;
        public List<pProximityType> ProximityList
        {
            get { return proximityList; }
        }
        public pCapabilityType()
        {

            this.proximityList = new List<pProximityType>();
        }
        public void Add(pProximityType action)
        {
            proximityList.Add(action);
        }

    }




    /// <summary>
    /// information about creating a unit
    /// </summary>
    public class pCreateType
    {
        private string unitID;
        public string UnitID
        {
            get { return this.unitID; }
        }

        private string unitBase;
        public string UnitBase
        {
            get { return this.unitBase; }
        }
        private string owner;
        public string Owner
        {
            get { return owner; }
        }

        private List<string> platforms = new List<string>();
        public List<string> Platforms
        {
            get { return platforms; }
        }
        public void DockFromList(List<string> subPList)
        {
            for (int i = 0; i < subPList.Count; i++)
                platforms.Add(subPList[i]);
        }

        public void Add(pSubplatformType p)// obsolete in 4.1
        {
            //    platforms.Add(p);
        }

        public void Add(pAdoptType a) // obsolete in 4.1
        {
            //       adoptions.Add(a);
        }

        public pCreateType(string unitID, string unitBase,
             string owner)
        {
            this.unitID = unitID;
            this.unitBase = unitBase;
            this.owner = owner;

        }


    }

    public class pRevealType
    {
        private string unitID;
        public string UnitID
        {
            get { return this.unitID; }
        }
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }

        private pLocationType initialLocation;
        public pLocationType InitialLocation
        {
            get { return initialLocation; }
        }
        private string initialState = "";
        public string InitialState
        {
            get { return initialState; }
        }
        private Dictionary<string, object> startupParameters;
        public Dictionary<string, object> StartupParameters
        {
            get { return startupParameters; }
        }
        private string initialTag = "";
        public string InitialTag
        {
            get { return initialTag; }
            set { initialTag = value; }
        }
        public pRevealType(string unitID, pEngramRange engramRange, int time, pLocationType initialLocation,
                           string initialState, Dictionary<string, object> startupParameters)
        {
            this.unitID = unitID;
            this.engramRange = engramRange;
            this.time = time;
            this.initialLocation = initialLocation;
            this.initialState = initialState;
            this.startupParameters = startupParameters;
        }

    }

    /// <summary>
    /// Information about moving a unit
    /// </summary>
    public class pMoveType
    {
        private string unitID;
        public string UnitID
        {
            get { return unitID; }
        }
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private int timer = 0;
        public int Timer
        {
            get
            { return this.timer; }
            set { this.timer = value; }
        }
        private pRandomIntervalType randomInterval = new pRandomIntervalType(0, 0);
        public pRandomIntervalType RandomInterval
        {
            get { return randomInterval; }
            set { randomInterval = value; }
            /* randomInterval is not an arg to constructor because that would mean changing/adding the abstract definition and then defining in all parsers */
        }
        private double throttle;
        public double Throttle
        {
            get
            { return this.throttle; }
            set
            { throttle = Math.Min(100, value); }
        }
        private pLocationType location;
        public pLocationType Location
        {
            get
            { return this.location; }
            set
            { this.location = value; }

        }
        public pMoveType(string unitID, pEngramRange engramRange, int timer,
            pLocationType location, double throttle)
        {
            this.engramRange = engramRange;
            this.unitID = unitID;
            this.timer = timer;
            this.throttle = throttle;
            this.location = location;
            randomInterval = new pRandomIntervalType(0, 0);
        }

    }
    /// <summary>
    /// What to do when some event occurs
    /// </summary>
    public class pHappeningCompletionType
    {
        private string unit;
        public string Unit
        {
            get { return unit; }
        }
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        // Now action and new state are alternatives, but it is possible to imagine combining them
        private string action = "";
        public string Action
        {
            get
            { return action; }

        }
        private string newState = "";
        public string NewState
        {
            get { return newState; }
            set { newState = value; }
        }
        private List<object> doThisList = new List<object>();
        public List<object> DoThisList
        {
            get
            { return doThisList; }
        }
        public void AddAction(object NewAction)
        {
            doThisList.Add(NewAction);
        }

        /// <summary>
        /// Creates a completion triggered  event
        /// </summary>
        /// <param name="unit">Indentifier for unit</param>
        /// <param name="action">action that triuggers new event</param>
        /// <param name="doThis">New event</param>
        public pHappeningCompletionType(string unit, pEngramRange engramRange, string action, string newState)
        {
            this.unit = unit;
            this.engramRange = engramRange;
            this.action = action;
            this.newState = newState;


        }
    }

    public class pSpeciesCompletionType
    {
        private string species;
        public string Species
        {
            get { return species; }
        }
        /* may someday define a way to handle engrams  on a species level
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
         */
        // Now action and new state are alternatives, but it is possible to imagine combining them
        private string action = "";
        public string Action
        {
            get
            { return action; }

        }
        private string newState = "";
        public string NewState
        {
            get { return newState; }
            set { newState = value; }
        }
        private List<object> doThisList = new List<object>();
        public List<object> DoThisList
        {
            get
            { return doThisList; }
        }
        public void AddAction(object NewAction)
        {
            doThisList.Add(NewAction);
        }

        /// <summary>
        /// Creates a completion triggered  event
        /// </summary>
        /// <param name="unit">Indentifier for unit</param>
        /// <param name="action">action that triuggers new event</param>
        /// <param name="doThis">New event</param>
        public pSpeciesCompletionType(string species, string action, string newState)
        {
            this.species = species;
            this.action = action;
            this.newState = newState;
        }
    }

    public class pReiterateType
    {
        private int start;
        public int Start
        {
            get { return start; }
        }
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }

        private List<object> reiterateList = new List<object>();
        public List<object> ReiterateList
        {
            get
            { return reiterateList; }
        }
        public void AddAction(object NewAction)
        {
            reiterateList.Add(NewAction);
        }

        public pReiterateType(int start, pEngramRange engramRange)
        {
            this.start = start;
            this.engramRange = engramRange;
        }
    }

    public class pStateChangeType
    {
        private string unitID;
        public string UnitID
        {
            get { return unitID; }
        }

        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private int timer = 0;
        public int Timer
        {
            get
            { return this.timer; }
            set { this.timer = value; }
        }

        private string newState;
        public string NewState
        {
            get
            { return this.newState; }
        }
        private List<string> from = new List<string>();
        public List<string> From
        {
            get
            { return this.from; }

        }
        public void AddPrecursor(string s)
        {
            from.Add(s);
        }

        private List<string> except = new List<string>();
        public List<string> Except
        {
            get { return this.except; }
        }
        public void AddException(string s)
        {
            except.Add(s);
        }
        public pStateChangeType(string unitID, pEngramRange engramRange, int timer,
            string newState)
        {
            this.unitID = unitID;
            this.engramRange = engramRange;
            this.timer = timer;
            this.newState = newState;

        }
    }
    public class pTransferType
    {
        private string unitID;
        public string UnitID
        {
            get { return unitID; }
        }


        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private int timer = 0;
        public int Timer
        {
            get
            { return this.timer; }
            set { this.timer = value; }
        }
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
        public pTransferType(string unitID, pEngramRange engramRange, int timer, string from, string to)
        {
            this.unitID = unitID;
            this.engramRange = engramRange;
            this.timer = timer;
            this.from = from;
            this.to = to;
        }
    }

    public class pDefineEngramType
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
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public pDefineEngramType(string name, string engramValue, string type)
        {
            this.name = name;
            this.engramValue = engramValue;
            this.type = type;
        }
        public pDefineEngramType(string name, string engramValue)
        {
            this.name = name;
            this.engramValue = engramValue;
            this.type = "String";
        }

    }

    public class pChangeEngramType
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

        private int time;
        public int Time
        {
            get { return time; }
        }
        private string engramValue;
        public string EngramValue
        {
            get { return engramValue; }
        }
        public pChangeEngramType(string name, string unit, int time, string engramValue)
        {
            this.name = name;
            this.unit = unit;
            this.time = time;
            this.engramValue = engramValue;
        }
        public pChangeEngramType(string name, int time, string engramValue) //for compatibility
        {
            this.name = name;
            this.unit = "";
            this.time = time;
            this.engramValue = engramValue;
        }

    }

    public class pRemoveEngramType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }

        public pRemoveEngramType(string name, int time)
        {
            this.name = name;
            this.time = time;
        }

    }
    public class pEngramRange
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
        private Boolean inclusionRange;
        public Boolean InclusionRange
        {
            get { return inclusionRange; }
        }
        private List<string> valueList;
        public List<string> ValueList
        {
            get { return valueList; }
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

        public void Add(string val)
        {
            valueList.Add(val);
        }
        public pEngramRange(string name, Boolean inclusionRange)
        {
            this.name = name;
            this.inclusionRange = inclusionRange;
            this.valueList = new List<string>();
        }
        public pEngramRange(string name, string condition
            , string compareTo)
        {
            this.name = name;
            this.condition = condition;
            this.compareTo = compareTo;
        }
        public pEngramRange(string name, string unit, Boolean inclusionRange)
        {
            this.name = name;
            this.unit = unit;
            this.inclusionRange = inclusionRange;
            this.valueList = new List<string>();
        }
        public pEngramRange(string name, string unit, string condition
            , string compareTo)
        {
            this.name = name;
            this.unit = unit;
            this.condition = condition;
            this.compareTo = compareTo;
        }
    }
    public class pFlushEventsType
    {
        private string unit;
        public string Unit
        {
            get { return unit; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }

        public pFlushEventsType(string unit, int time)
        {
            this.unit = unit;
            this.time = time;
        }
    }
    public class pOpenChatRoomType
    {
        private string room;
        public string Room
        {
            get { return room; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string owner = null;
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
        public void Add(List<String> memberList)
        {
            for (int i = 0; i < memberList.Count; i++) members.Add(memberList[i]);
        }

        public pOpenChatRoomType(string room, int time)
        {
            members = new List<string>();
            this.room = room;
            this.time = time;
        }
    }

    public class pOpenWhiteboardRoomType
    {
        private string room;
        public string Room
        {
            get { return room; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string owner = null;
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
        public void Add(List<String> memberList)
        {
            for (int i = 0; i < memberList.Count; i++) members.Add(memberList[i]);
        }

        public pOpenWhiteboardRoomType(string room, int time)
        {
            members = new List<string>();
            this.room = room;
            this.time = time;
        }
    }

    public class pOpenVoiceChannelType
    {
        private string channel;
        public string Channel
        {
            get { return channel; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        /*
         //deprecated
        private string owner = null;
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
         */
        private List<string> access = null;
        public List<string> Access
        {
            get { return access; }
            set { access = value; }
        }

        public pOpenVoiceChannelType(string channel, int time, List<string> access)
        {
            this.channel = channel;
            this.time = time;
            this.access = access;
        }

        public pOpenVoiceChannelType(string channel, int time)
        {
            this.channel = channel;
            this.time = time;
            this.access = new List<string>();
        }
    }
    /*
        public class pAddChattersType
        {
            private string room;
            public string Room
            {
                get { return room; }
            }
            private int time;
            public int Time
            {
                get { return time; }
            }
            private List<string> members;
            public List<string> Members
            {
                get { return members; }
            }
            public void Add(List<String> memberList)
            {
                for (int i = 0; i < memberList.Count; i++) members.Add(memberList[i]);
            }
            public pAddChattersType(string room, int time)
            {
                members = new List<string>();
                this.room = room;
                this.time = time;
            }
        }
        public class pDropChattersType
        {
            private string room;
            public string Room
            {
                get { return room; }
            }
            private int time;
            public int Time
            {
                get { return time; }
            }
            private List<string> members;
            public List<string> Members
            {
                get { return members; }
            }
            public void Add(string member)
            {
                members.Add(member);
            }
            public void Add(List<String> memberList)
            {
                for (int i = 0; i < memberList.Count; i++) members.Add(memberList[i]);
            }

            public pDropChattersType(string room, int time)
            {
                members = new List<string>();
                this.room = room;
                this.time = time;
            }
        }
        */
    public class pCloseChatRoomType
    {
        private string room;
        public string Room
        {
            get { return room; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string requestor = null;
        public string Requestor
        {
            get { return requestor; }
            set { requestor = value; }
        }

        public pCloseChatRoomType(string room, int time)
        {
            this.room = room;
            this.time = time;
        }
    }
    public class pCloseVoiceChannelType
    {
        private string channel;
        public string Channel
        {
            get { return channel; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string requestor = null;
        public string Requestor
        {
            get { return requestor; }
            set { requestor = value; }
        }

        public pCloseVoiceChannelType(string channel, int time)
        {
            this.channel = channel;
            this.time = time;
        }
    }
    public class pGrantVoiceChannelAccessType
    {
        private pEngramRange engramRange=null;
        public pEngramRange EngramRange
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
        private int time;
        public int Time
        {
            get { return time; }
        }
        public pGrantVoiceChannelAccessType(pEngramRange engramRange, string decisionMaker, string voiceChannel, int time)
        {
            this.engramRange = engramRange;
            this.decisionMaker = decisionMaker;
            this.voiceChannel = voiceChannel;
            this.time = time;
        }
    }
    public class pRemoveVoiceChannelAccessType
    {
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
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
        private int time;
        public int Time
        {
            get { return time; }
        }
                public pRemoveVoiceChannelAccessType(pEngramRange engramRange, string decisionMaker, string voiceChannel, int time)
        {
            this.engramRange = engramRange;
            this.decisionMaker = decisionMaker;
            this.voiceChannel = voiceChannel;
            this.time = time;
        }
    }
    public class pSetRegionVisibilityType
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private Boolean isVisible;
        public Boolean IsVisible
        {
            get { return isVisible; }
        }
        public pSetRegionVisibilityType(string id, int time, Boolean isVisible)
        {
            this.id = id;
            this.time = time;
            this.isVisible = isVisible;
        }
    }
    public class pSetRegionActivityType
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private Boolean isActive;
        public Boolean IsActive
        {
            get { return isActive; }
        }
        public pSetRegionActivityType(string id, int time, Boolean isActive)
        {
            this.id = id;
            this.time = time;
            this.isActive = isActive;
        }
    }
    public class pScoringLocationType
    {
        private List<string> zone;
        public List<string> Zone
        {
            get { return zone; }
        }
        private string relationship; // how is the unit related to the zone?
        public string Relationship
        {
            get { return relationship; }
        }
        public pScoringLocationType(List<string> zone, string relationship)
        {
            this.zone = zone;
            this.relationship = relationship;
        }
        public pScoringLocationType(List<string> zone)
        {
            this.zone = zone;
            this.relationship = "";
        }
    }
    public class pActorType
    {
        private string owner;
        public string Owner
        {
            get { return owner; }
        }

        private string id;
        public string Id
        {
            get { return id; }
        }

        private pScoringLocationType region;
        public pScoringLocationType Region
        {
            get { return region; }
        }
        public pActorType(string owner, string id, pScoringLocationType region)
        {
            this.owner = owner;
            this.id = id;
            this.region = region;
        }
        public pActorType(string owner, string id)
        {
            this.owner = owner;
            this.id = id;
            this.region = null;
        }

    }
    public class pScoringRuleType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private pActorType unit;
        public pActorType Unit
        {
            get { return unit; }
        }
        private pActorType objectID;
        public pActorType ObjectID
        {
            get { return objectID; }
        }
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        private string from;
        public string From
        {
            get { return from; }
        }
        private double increment;
        public double Increment
        {
            get { return increment; }
            set { increment = value; }
        }
        public pScoringRuleType(string name, pActorType unit, pActorType objectID, string newState, string from)
        {
            this.name = name;
            this.unit = unit;
            this.objectID = objectID;
            this.newState = newState;
            this.from = from;
        }
        public pScoringRuleType(string name, pActorType unit, pActorType objectID, string newState)
        {
            this.name = name;
            this.unit = unit;
            this.objectID = objectID;
            this.newState = newState;
            this.from = "";
        }
        public pScoringRuleType(string name, pActorType unit)
        {
            this.name = name;
            this.unit = unit;
            this.objectID = null;
            this.newState = "";
            this.from = "";
        }
    }
    public class pScoreType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private List<string> rules;
        public List<string> Rules
        {
            get { return rules; }
        }
        private List<string> applies;
        public List<string> Applies
        {
            get { return applies; }
        }
        private List<string> viewers = new List<string>();
        public List<string> Viewers
        {
            get { return viewers; }
        }
        private double initial = 0;
        public double Initial
        {
            get { return initial; }
        }
        public pScoreType(string name, List<string> rules, List<string> applies, List<string> viewers, double initial)
        {
            this.name = name;
            this.rules = rules;
            this.applies = applies;
            this.viewers = viewers;
            this.initial = initial;
        }
    }
    public class pAttack_Successful_Completion_Type
    {
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private string species;
        public string Species
        {
            get { return species; }
        }
        private string capability;
        public string Capability
        {
            get { return capability; }
        }
        private List<object> doThisList = new List<object>();
        public List<object> DoThisList
        {
            get
            { return doThisList; }
            set { doThisList = value; }
        }
        private string targetSpecies;
        public string TargetSpecies
        {
            get { return targetSpecies; }
        }
        private string newState = "";
        public string NewState
        {
            get { return newState; }
        }
        /*       private List<object> targetDoThisList = new List<object>();
               public List<object> TargetDoThisList
               {
                   get
                   { return targetDoThisList; }
                   set { targetDoThisList = value; }
               }
               */
        public pAttack_Successful_Completion_Type(string species, string capability, List<object> doThisList, string targetSpecies, /*List<object> targetDoThisList,*/ pEngramRange engramRange, string newState)
        {
            // nb -- either list or Target Species can be null/empty
            this.species = species;
            if (null == targetSpecies)
            {
                this.targetSpecies = "";
            }
            else
            {
                this.targetSpecies = targetSpecies;
            }
            this.capability = capability;
            if (doThisList != null)
            {
                for (int i = 0; i < doThisList.Count; i++)
                {
                    this.doThisList.Add(doThisList[i]);
                }
            }
            /*          if (targetDoThisList != null)
                      {
                          for (int i = 0; i < targetDoThisList.Count; i++)
                          {
                              this.targetDoThisList.Add(targetDoThisList[i]);
                          }
                      }*/
            this.engramRange = engramRange;
            if (newState != null)
                this.newState = newState;
        }

    }
    public class pAttack_Request_Approval_Type
    {
        private string capability;
        public string Capability
        {
            get { return capability; }
        }
        private Boolean actorIsSpecies = false;
        public Boolean ActorIsSpecies
        {
            get { return actorIsSpecies; }
        }
        private string actor = "";
        public string Actor
        {
            get { return actor; }
        }
        private Boolean targetIsSpecies = false;
        public Boolean TargetIsSpecies
        {
            get { return targetIsSpecies; }
        }
        private string target = "";
        public string Target
        {
            get { return target; }
        }
        private List<string> targetStates = new List<string>();
        public List<string> TargetStates
        {
            get { return targetStates; }
            //         set { targetStates = value; }
        }
        private Boolean useDefault = true;
        public Boolean UseDefault
        {
            get { return useDefault; }
        }
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private string failure = "";
        public string Failure
        {
            get { return failure; }
        }
        public pAttack_Request_Approval_Type(string capability, string actor, Boolean actorIsSpecies, string target, Boolean targetIsSpecies,
List<string> targetStates, Boolean useDefault, pEngramRange engramRange, string failure)
        {
            this.capability = capability;
            this.actor = actor;
            this.actorIsSpecies = actorIsSpecies;
            this.target = target;
            this.targetIsSpecies = targetIsSpecies;
            this.useDefault = useDefault;
            this.engramRange = engramRange;
            this.failure = failure;
            if (null != targetStates)
                for (int i = 0; i < targetStates.Count; i++)
                {
                    this.targetStates.Add(targetStates[i]);
                }
        }
    }

    public class pLaunchType
    {
        private string parent;
        public string Parent
        {
            get { return parent; }
        }

        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private string child = "";
        public string Child
        {
            get { return child; }
        }
        private string kind = "";
        public string Kind
        {
            get { return kind; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private pLocationType relativeLocation = new pLocationType();
        public pLocationType RelativeLocation
        {
            get { return relativeLocation; }
        }
        private string initialState = "";
        public string InitialState
        {
            get { return initialState; }
        }
        private Dictionary<string, object> startupParameters;
        public Dictionary<string, object> StartupParameters
        {
            get { return startupParameters; }
        }
        public pLaunchType(string parent, pEngramRange engramRange, string child, string kind, int time, pLocationType relativeLocation,
                           string initialState, Dictionary<string, object> startupParameters)
        {
            this.parent = parent;
            this.engramRange = engramRange;
            this.child = child;
            this.kind = kind;
            this.time = time;
            this.relativeLocation = relativeLocation;
            this.initialState = initialState;
            this.startupParameters = startupParameters;

        }

    }

    public class pWeaponLaunchType
    {
        private string parent;
        public string Parent
        {
            get { return parent; }
        }

        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
        {
            get { return engramRange; }
        }
        private string child = "";
        public string Child
        {
            get { return child; }
        }
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string target = "";
        public string Target
        {
            get { return target; }
        }
        private string initialState = "";
        public string InitialState
        {
            get { return initialState; }
        }
        private Dictionary<string, object> startupParameters;
        public Dictionary<string, object> StartupParameters
        {
            get { return startupParameters; }
        }
        public pWeaponLaunchType(string parent, pEngramRange engramRange, string child, string target, int time,
                           string initialState, Dictionary<string, object> startupParameters)
        {
            this.parent = parent;
            this.engramRange = engramRange;
            this.child = child;
            this.time = time;
            this.target = target;
            this.initialState = initialState;
            this.startupParameters = startupParameters;

        }

    }

    //Types that must be kept so that earlier parsers will compile, even though they don't run against this
    //parser

    public class pAdoptType //obsolete in 4.1
    {

        private string child;
        public string Child
        {
            get { return child; }
            set { child = value; }
        }

        private pLocationType location = new pLocationType();
        public pLocationType Location
        {
            get { return location; }
            set { location = value; }
        }
        private string initialState;
        public string InitialState
        {
            get { return initialState; }
            set { initialState = value; }
        }

        private Dictionary<string, object> initialParameters;
        public Dictionary<string, object> InitialParameters
        {
            get { return initialParameters; }
        }

        public void Add(Dictionary<string, object> d)
        {
            if (null == initialParameters)
            {
                initialParameters = new Dictionary<string, object>();
            }
            foreach (string name in d.Keys)
            {
                initialParameters[name] = d[name]; ;
            }

        }
        public pAdoptType(string child)
        {
            this.child = child;
            this.location = null;
            this.InitialState = "";
            this.initialParameters = null;
        }

    }

    public class pLaunchedPlatformType// Obsolete in 4.1
    {
        private string launchedID = "";
        public string LaunchedID
        {
            get { return launchedID; }
            set { launchedID = value; }
        }
        private List<pLocationType> locations;
        public List<pLocationType> Locations
        {
            get { return locations; }
        }
        private string initialState;
        public string InitialState
        {
            get { return initialState; }
            set { initialState = value; }
        }

        private Dictionary<string, object> initialParameters;
        public Dictionary<string, object> InitialParameters
        {
            get { return initialParameters; }
        }
        public void Add(pLocationType loc)
        {
            locations.Add(loc);
        }
        public void Add(Dictionary<string, object> d)
        {
            foreach (string name in d.Keys)
            {
                initialParameters[name] = d[name]; ;
            }

        }


        public pLaunchedPlatformType()
        {
            initialState = "";
            locations = new List<pLocationType>();
            initialParameters = new Dictionary<string, object>();
        }

    }
    public class pSendChatMessageType
    {
        private int time;
        public int Time
        {
            get { return time; }
        }
        private string roomName = "";
        public string RoomName
        {
            get { return roomName; }
        }
        private string sender = "";
        public string Sender
        {
            get { return sender; }
        }
        private string message = "";
        public string Message
        {
            get { return message; }
        }

        public pSendChatMessageType(string roomName, string sender, int time,string message)
        {
            this.roomName = roomName;
            this.sender = sender;
            this.time = time;
            this.message = message;
        }
    }

    public class pApplyType
    {
        private string fromDM = "";
        private string toDM = "";
        private int time;
        private pEngramRange engramRange = null;
        public pEngramRange EngramRange
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
        public int Time
        {
            get { return time; }
        }
        public pApplyType(pEngramRange engramRange, string fromDM, string toDM, int time)
        {
            this.engramRange = engramRange;
            this.fromDM = fromDM;
            this.toDM = toDM;
            this.time = time;
        }
    }

    public class pSendVoiceMessageType
    {
        private string channelName = "";
        private string filePath = "";
        private int time;
        public string ChannelName
        {
            get { return channelName; }
        }
        public string FilePath
        {
            get { return filePath; }
        }
        public int Time
        {
            get { return time; }
        }
        public pSendVoiceMessageType(string channelName, string filePath, int time)
        {
            this.channelName = channelName;
            this.filePath = filePath;
            this.time = time;
        }
    }

    public class pSendVoiceMessageToUserType
    {
        private string decisionMakerID = "";
        private string filePath = "";
        private int time;
        public string DecisionMakerID
        {
            get { return decisionMakerID; }
        }
        public string FilePath
        {
            get { return filePath; }
        }
        public int Time
        {
            get { return time; }
        }
        public pSendVoiceMessageToUserType(string username, string filePath, int time)
        {
            this.decisionMakerID = username;
            this.filePath = filePath;
            this.time = time;
        }
    }

}

