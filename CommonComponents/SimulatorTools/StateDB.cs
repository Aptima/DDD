using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
namespace Aptima.Asim.DDD.CommonComponents.SimulatorTools
{
    public class StateDB
    {
        public class PhysicalObject
        {
            public string objectID;
            public string teamName;
            public string objectType;
            public string ownerID;
            public string speciesName;
            public string linkedRegion;
            public List<string> activeRegions;

            public PhysicalObject()
            {
                objectID = "";
                teamName = "";
                objectType = "";
                ownerID = "";
                speciesName = "";
                linkedRegion = "";
                activeRegions = new List<string>();
            }
        }
        public class Team
        {
            public string id;
            public List<string> hostility;
            public Team(string id, List<string> hostility)
            {
                this.id = id;
                this.hostility = hostility;
            }
        }
        public class DecisionMaker
        {
            public enum Availability
            {
                AVAILABLE = 0,
                TAKEN = 1,
                READY = 2,
            }

            public string id;
            public Team team;
            public bool isHuman;
            public int color;
            public string role;
            public Availability availability;
            public string briefing;

            public DecisionMaker(string id, Team team)
            {
                this.isHuman = false;
                this.id = id;
                this.team = team;
                this.color = 000000;
                this.availability = Availability.AVAILABLE;
                this.briefing = string.Empty;
            }

            public bool isHostile(DecisionMaker dm)
            {
                if (team == null)
                {
                    return false;
                }

                if (team.hostility.Contains(dm.team.id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class LandRegion
        {
            public string id;
            public Polygon2D poly;

            public LandRegion(string id, Polygon2D poly)
            {
                this.id = id;
                this.poly = poly;
            }
        }


        public class DynamicRegion
        {
            public string id;

            public Polygon3D poly;
            public List<string> sensorsBlocked;
            public bool blockingRegion;
            public double speedMultiplier;
            public bool isVisible;
            public int displayColor;
            public bool isDynamicRegion;
            public Vec2D referencePoint;


            public DynamicRegion(string id, Polygon3D poly, Vec2D refPoint)
            {
                this.id = id;
                this.poly = poly;
                this.sensorsBlocked = new List<string>();
                this.blockingRegion = false;
                this.speedMultiplier = 1;
                this.isVisible = true;
                this.displayColor = 0;
                this.referencePoint = refPoint;
            }
        }


        public class ActiveRegion
        {
            public string id;
            public Polygon3D poly;
            public List<string> sensorsBlocked;
            public bool blockingRegion;
            public double speedMultiplier;
            public bool isVisible;
            public int displayColor;
            public string linkedObject;
            public bool isDynamicRegion;
            public Vec2D referencePoint;
            public Polygon3D currentAbsolutePolygon;

            public ActiveRegion(string id, Polygon3D poly)
            {
                this.id = id;
                this.poly = poly;
                this.sensorsBlocked = new List<string>();
                this.blockingRegion = false;
                this.speedMultiplier = 1;
                this.isVisible = true;
                this.displayColor = 0;
                this.linkedObject = "";
                this.isDynamicRegion = false;
                this.referencePoint = new Vec2D(0,0);
                this.currentAbsolutePolygon = new Polygon3D(0, 0);

            }
        }

        public class Engram
        {
            public string engramName;
            public string engramValue;
            public string engramDataType;

            public Engram()
            {
                engramName = string.Empty;
                engramValue = string.Empty;
                engramDataType = string.Empty;
            }
        }

        public static Dictionary<string, PhysicalObject> physicalObjects = new Dictionary<string, PhysicalObject>();
        public static Dictionary<string, DecisionMaker> decisionMakers = new Dictionary<string, DecisionMaker>();
        public static Dictionary<string, Team> teams = new Dictionary<string, Team>();

        public static Dictionary<string, LandRegion> landRegions = new Dictionary<string, LandRegion>();
        public static Dictionary<string, ActiveRegion> activeRegions = new Dictionary<string, ActiveRegion>();
        public static Dictionary<string, ActiveRegion> dynamicRegions = new Dictionary<string, ActiveRegion>();
        public static Dictionary<string, Engram> engrams = new Dictionary<string, Engram>();
        
        public static void Reset()
        {
            physicalObjects.Clear();
            teams.Clear();
            decisionMakers.Clear();
            landRegions.Clear();
            activeRegions.Clear();
            dynamicRegions.Clear();
            engrams.Clear();
        }

        public static void UpdateEngrams(string name, string value, string dataType)
        {
            if (engrams.ContainsKey(name))
            {
                engrams[name].engramValue = value;
            }
            else
            {
                Engram e = new Engram();
                e.engramName = name;
                e.engramValue = value;
                e.engramDataType = dataType;
                engrams[name] = e;
            }
        }

        public static void AddPhysicalObject(string objectID, string objectType, string linkedRegion)
        {

            if (!physicalObjects.ContainsKey(objectID))
            {
                PhysicalObject e = new PhysicalObject();
                e.objectID = objectID;
                e.objectType = objectType;
                e.linkedRegion = linkedRegion;

                physicalObjects[objectID] = e;
            }


        }

        public static bool isHostile(string dm1, string dm2)
        {
            if (decisionMakers.ContainsKey(dm1) && decisionMakers.ContainsKey(dm2))
            {
                return decisionMakers[dm1].isHostile(decisionMakers[dm2]);
            }
            else
            {
                return false;
            }
            
        }
    }
}
