using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.DDDAgentFramework;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;

namespace Aptima.Asim.DDD.DDDAgentFramework
{
    /// <summary>
    /// This class contains information about specific DDD objects.  It is created and maintained by the DDDServerConnection object and lives inside the DMView object.
    /// </summary>
    public class SimObject
    {
        private Boolean m_shouldProject = false;
        /// <summary>
        /// Used by animation to project locations between time ticks.
        /// </summary>
        public Boolean ShouldProject
        {
            get { return m_shouldProject; }
            set { m_shouldProject = value; }
        }

        private Boolean m_changedFlag = false;
        /// <summary>
        /// Find out if the object has had an attribute change since the last time the ResetChangedFlag() method was called.
        /// </summary>
        public Boolean ChangedFlag
        {
            get { return m_changedFlag; }
        }
        /// <summary>
        /// Reset the ChangedFlag property.  This should be called after inspecting a SimObject and finding that it has changed.
        /// </summary>
        public void ResetChangedFlag()
        {
            m_changedFlag = false;
        }

        private ObjectControlAgent m_agent = null;
        /// <summary>
        /// Access the ObjectControlAgent for this DDD object.  This allows the addition of object behaviors.
        /// </summary>
        public ObjectControlAgent ControlAgent
        {
            get { return m_agent; }
        }

        private String m_id = "";
        /// <summary>
        /// The DDD identity string for the object.
        /// </summary>
        public String ID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        private String m_owner = "";
        /// <summary>
        /// The DDD decision maker that owns the object.
        /// </summary>
        public String Owner
        {
            get { return m_owner; }
            set
            {
                if (value != "")
                {
                    if (value != m_owner)
                    {
                        m_changedFlag = true;
                    }
                    m_owner = value;
                }
            }
        }

        private String m_className = "";
        /// <summary>
        /// The DDD species that the object belongs to.
        /// </summary>
        public String ClassName
        {
            get { return m_className; }
            set
            {
                if (value != "")
                {
                    if (value != m_className)
                    {
                        m_changedFlag = true;
                    }
                    m_className = value;
                }
            }
        }
        private String m_iconName = "";
        /// <summary>
        /// The icon name from the icon library that should be used to display the object.
        /// </summary>
        public String IconName
        {
            get { return m_iconName; }
            set
            {
                if (value != "")
                {
                    if (value != m_iconName)
                    {
                        m_changedFlag = true;
                    }
                    m_iconName = value;
                }
            }
        }

        //private String m_type;
        //public String Type
        //{
        //    get { return m_type; }
        //    set { m_type = value; }
        //}

        private LocationValue m_location;
        /// <summary>
        /// The current location of the object.
        /// </summary>
        public LocationValue Location
        {
            get 
            { 
                return m_location; 
            }
            set 
            {

                if (!BehaviorHelper.LocationIsEqual(value, m_location))
                {
                    m_changedFlag = true;
                }
                m_location = value; 
            }
        }
        private LocationValue m_destination;
        /// <summary>
        /// The current destination of the object.  If it isn't moving, it will be the same as Location.
        /// </summary>
        public LocationValue DestinationLocation
        {
            get { return m_destination; }
            set 
            {
                if (!BehaviorHelper.LocationIsEqual(value, m_destination))
                {
                    m_changedFlag = true;
                }
                m_destination = value; 
            }
        }

        private VelocityValue m_velocity;
        /// <summary>
        /// The current velocity vector of the object.
        /// </summary>
        public VelocityValue Velocity
        {
            get { return m_velocity; }
            set 
            {
                if (!BehaviorHelper.VelocityIsEqual(value, m_velocity))
                {
                    m_changedFlag = true;
                }
                m_velocity = value; 
            }
        }

        private Double m_currentHeading;
        /// <summary>
        /// The maximum speed of the object.
        /// </summary>
        public Double CurrentHeading
        {
            get { return m_currentHeading; }
            set
            {
                if (value != m_currentHeading)
                {
                    m_changedFlag = true;
                }
                m_currentHeading = value;
            }
        }

        private Double m_maximumSpeed;
        /// <summary>
        /// The maximum speed of the object.
        /// </summary>
        public Double MaximumSpeed
        {
            get { return m_maximumSpeed; }
            set 
            {
                if (value != m_maximumSpeed)
                {
                    m_changedFlag = true;
                }
                m_maximumSpeed = value; 
            }
        }
        private Double m_throttle;
        /// <summary>
        /// The current throttle of the object.  This should be the same as the throttle set by the MoveObjectRequest sent out (assuming it was successful and hasn't reached it destination yet).
        /// </summary>
        public Double Throttle
        {
            get { return m_throttle; }
            set 
            {
                if (value != m_throttle)
                {
                    m_changedFlag = true;
                }
                m_throttle = value; 
            }
        }
        
        private Boolean m_isWeapon;
        /// <summary>
        /// Is this object a weapon?
        /// </summary>
        public Boolean IsWeapon
        {
            get { return m_isWeapon; }
            set 
            {
                if (value != m_isWeapon)
                {
                    m_changedFlag = true;
                }
                m_isWeapon = value; 
            }
        }

        private List<String> m_vulnerabilityList;
        /// <summary>
        /// What vulnerabilities does the object have?
        /// </summary>
        public List<String> VulnerabilityList
        {
            get { return m_vulnerabilityList; }
            set 
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_vulnerabilityList))
                {
                    m_changedFlag = true;
                    m_vulnerabilityList = value;
                }
            }
        }
        private List<String> m_capabilityList;
        /// <summary>
        /// What capabilities does the this object have?
        /// </summary>
        public List<String> CapabilityList
        {
            get { return m_capabilityList; }
            set 
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_capabilityList))
                {
                    m_changedFlag = true;
                    m_capabilityList = value;
                }
            }
        }

        private List<String> m_dockedObjects;
        /// <summary>
        /// What objects are currently docked to this object?
        /// </summary>
        public List<String> DockedObjects
        {
            get { return m_dockedObjects; }
            set 
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_dockedObjects))
                {
                    m_changedFlag = true;
                    m_dockedObjects = value;
                }
            }
        }
        private List<String> m_dockedWeapons;
        /// <summary>
        /// What weapons are currently docked to this object?
        /// </summary>
        public List<String> DockedWeapons
        {
            get { return m_dockedWeapons; }
            set 
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_dockedWeapons))
                {
                    m_changedFlag = true;
                    m_dockedWeapons = value;
                }
            }
        }

        private String m_state = "";
        /// <summary>
        /// What is the current DDD state of this object?
        /// </summary>
        public String State
        {
            get { return m_state; }
            set
            {
                if (value != "")
                {
                    m_changedFlag = true;
                    m_state = value;
                }
            }
        }

        private CustomAttributesValue m_customAttributes;
        /// <summary>
        /// The object's custom attributes as defined in the scenario.
        /// </summary>
        public CustomAttributesValue CustomAttributes
        {
            get { return m_customAttributes; }
            set
            {
                m_changedFlag = true;
                m_customAttributes = value;
            }
        }

        private VulnerabilityValue m_vulnerabilities;
        /// <summary>
        /// The object's vulnerabilities.
        /// </summary>
        public VulnerabilityValue Vulnerabilities
        {
            get
            {
                return m_vulnerabilities;
            }
            set
            {
                m_changedFlag = true;
                m_vulnerabilities = value;
            }
        }

        private CapabilityValue m_capabilities;
        /// <summary>
        /// The object's capabilities.
        /// </summary>
        public CapabilityValue Capabilities
        {
            get
            {
                return m_capabilities;
            }
            set
            {
                m_changedFlag = true;
                m_capabilities = value;
            }
        }

        private CustomAttributesValue m_capabilityRangeRings;
        /// <summary>
        /// The object's capability range rings.
        /// </summary>
        public CustomAttributesValue CapabilityRangeRings
        {
            get
            {
                return m_capabilityRangeRings;
            }
            set
            {
                m_changedFlag = true;
                m_capabilityRangeRings = value;
            }
        }

        private CustomAttributesValue m_vulnerabilityRangeRings;
        /// <summary>
        /// The object's vulnerability range rings.
        /// </summary>
        public CustomAttributesValue VulnerabilityRangeRings
        {
            get
            {
                return m_vulnerabilityRangeRings;
            }
            set
            {
                m_changedFlag = true;
                m_vulnerabilityRangeRings = value;
            }
        }

        private Double m_fuelConsumptionRate;
        /// <summary>
        /// The object's fuel consumption rate in generic units per second.
        /// </summary>
        public Double FuelConsumptionRate
        {
            get
            {
                return m_fuelConsumptionRate;
            }
            set
            {
                m_changedFlag = true;
                m_fuelConsumptionRate = value;
            }
        }

        private Double m_fuelAmount;
        /// <summary>
        /// The object's current amount of fuel in generic units.
        /// </summary>
        public Double FuelAmount
        {
            get
            {
                return m_fuelAmount;
            }
            set
            {
                if (m_fuelAmount != value)
                {
                    m_changedFlag = true;
                    m_fuelAmount = value;
                }
            }
        }

        private Double m_fuelCapacity;
        /// <summary>
        /// The object's maximum amount of fuel in generic units.
        /// </summary>
        public Double FuelCapacity
        {
            get
            {
                return m_fuelCapacity;
            }
            set
            {
                if (m_fuelCapacity != value)
                {
                    m_changedFlag = true;
                    m_fuelCapacity = value;
                }
            }
        }

        private SensorArrayValue m_sensors;
        /// <summary>
        /// The object's sensors.
        /// </summary>
        public SensorArrayValue Sensors
        {
            get
            {
                return m_sensors;
            }
            set
            {
                m_changedFlag = true;
                m_sensors = value;
            }
        }
        private String m_teamName;
        /// <summary>
        /// The object's owner's team name.
        /// </summary>
        public String TeamName
        {
            get
            {
                return m_teamName;
            }
            set
            {
                if (m_teamName != value)
                {
                    m_changedFlag = true;
                    m_teamName = value;
                }
            }
        }

        private List<String> m_childObjects;
        /// <summary>
        /// The object's list of child objects.
        /// </summary>
        public List<String> ChildObjects
        {
            get
            {
                return m_childObjects;
            }
            set
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_childObjects))
                {
                    m_changedFlag = true;
                    m_childObjects = value;
                }
            }
        }

        private Boolean m_dockedToParent;
        /// <summary>
        /// Is this object currently docked to its parent.
        /// </summary>
        public Boolean DockedToParent
        {
            get
            {
                return m_dockedToParent;
            }
            set
            {
                if (m_dockedToParent != value)
                {
                    m_changedFlag = true;
                    m_dockedToParent = value;
                }
            }
        }

        private String m_parentObjectID;
        /// <summary>
        /// The id of the parent of this object.
        /// </summary>
        public String ParentObjectID
        {
            get
            {
                return m_parentObjectID;
            }
            set
            {
                if (m_parentObjectID != value)
                {
                    m_changedFlag = true;
                    m_parentObjectID = value;
                }
            }
        }

        private List<String> m_inActiveRegions;
        public List<String> InActiveRegions
        {
            get { return m_inActiveRegions; }
            set
            {
                if (!BehaviorHelper.StringListIsEqual(value, m_inActiveRegions))
                {
                    m_changedFlag = true;
                    m_inActiveRegions = value;
                }
            }
        }


        /// <summary>
        /// The constructor for the SimObject.
        /// </summary>
        /// <param name="id"></param>
        public SimObject(String id)
        {
            m_id = id;
            m_velocity = new VelocityValue();
            m_location = new LocationValue();
            m_destination = new LocationValue();
            m_vulnerabilityList = new List<string>();
            m_capabilityList = new List<string>();
            m_dockedObjects = new List<string>();
            m_dockedWeapons = new List<string>();
            m_childObjects = new List<string>();
            m_agent = new ObjectControlAgent();
            m_agent.simObject = this;
            m_inActiveRegions = new List<string>();
        }

    }

    /// <summary>
    /// An object containing information about the active regions in the DDD simulation.
    /// </summary>
    public class SimActiveRegion
    {
        private Color m_color;
        /// <summary>
        /// The color that the active region should be displayed as.
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }
        
        private String m_id = "";
        /// <summary>
        /// The ddd identity string for the region.
        /// </summary>
        public String ID
        {
            get { return m_id; }
            set { m_id = value; }
        }


        private PolygonValue m_shape;
        /// <summary>
        /// The polygon shape of the region.
        /// </summary>
        public PolygonValue Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }


        /// <summary>
        /// A constructor.
        /// </summary>
        public SimActiveRegion()
        {
            m_shape = new PolygonValue();
        }

    }
}
