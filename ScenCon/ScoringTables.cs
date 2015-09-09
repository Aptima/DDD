using System;
using System.IO;
using System.Collections.Generic;
using Aptima.Asim.DDD.ScenarioParser;
using Aptima.Asim.DDD.CommonComponents.SimulatorTools;
using Aptima.Asim.DDD.Simulators.Scoring;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
namespace Aptima.Asim.DDD.ScenarioController
{

    public class ScoringLocationType
    {

        private List<string> zone=new List<string>();
        public List<string> Zone
        {
            get { return zone; }
        }
        private ScoringDB.ActorInfo.LocationInfo.LocationType relationship; // how is the unit related to the zone?
        public ScoringDB.ActorInfo.LocationInfo.LocationType Relationship
        {
            get { return relationship; }
        }
        public ScoringLocationType(pScoringLocationType s)
        {
            for (int i = 0; i < s.Zone.Count; i++) this.zone.Add(s.Zone[i]);

            if ("Anywhere" == s.Zone[0])
            {
                this.relationship = ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere;
            }
            else if (("InZone" == s.Relationship)||(""==s.Relationship))
            {
                this.relationship = ScoringDB.ActorInfo.LocationInfo.LocationType.InZone;
            }
            else if ("NotInZone" == s.Relationship)
            {
                this.relationship = ScoringDB.ActorInfo.LocationInfo.LocationType.NotInZone;
            }
      
            else
            {
                throw new ApplicationException("Unknown scoring region relationship: " + s.Relationship);
            }
        }
        public ScoringLocationType(List<string> zone, ScoringDB.ActorInfo.LocationInfo.LocationType relationship)
        {
            this.zone = zone;
            this.relationship = relationship;
        }
    }
    public class ActorType
    {
        private ScoringDB.ActorInfo.OwnerInfo.OwnerType owner;
        public ScoringDB.ActorInfo.OwnerInfo.OwnerType Owner
        {
            get { return owner; }
        }

        private ScoringDB.ActorInfo.IdentifierInfo id;
        public ScoringDB.ActorInfo.IdentifierInfo ID
        {
            get { return id; }
        }

        private ScoringLocationType region = null;
        public ScoringLocationType Region
        {
            get { return region; }
        }
        public ActorType(pActorType a)
        {
            switch (a.Owner)
            {
                case "Hostile":
                    this.owner = ScoringDB.ActorInfo.OwnerInfo.OwnerType.HostileDM;
                    break;
                case "Friendly":
                    this.owner = ScoringDB.ActorInfo.OwnerInfo.OwnerType.FriendlyDM;
                    break;
                case "This":
                    this.owner = ScoringDB.ActorInfo.OwnerInfo.OwnerType.Myself;
                    break;
                default:
                    throw new ApplicationException("Scoring rule contains unknown actor specifier " + a.Owner);
            }

            if ("Any" == a.Id)
            {
                this.id = new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Any, a.Id);
            }
            else if (NameLists.speciesNames.ContainsKey(a.Id))
            {
                this.id = new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Species, a.Id); ;
            }
            else if (NameLists.unitNames.ContainsKey(a.Id))
            {
                this.id = new ScoringDB.ActorInfo.IdentifierInfo(ScoringDB.ActorInfo.IdentifierInfo.IdentifierType.Instance, a.Id); ;
            }
            else
            {
                throw new ApplicationException("Unknown scoring rule identifier " + a.Id);
            }
            if (null != a.Region)
            {
                this.region = new ScoringLocationType(a.Region);
            }
            else
            {
                List<string> anywhere = new List<string>();
                anywhere.Add("Anywhere");
                this.region = new ScoringLocationType(anywhere, ScoringDB.ActorInfo.LocationInfo.LocationType.Anywhere);
            }
        }

    }
    public class ScoringRuleType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private ActorType unit;
        public ActorType Unit
        {
            get { return unit; }
        }
        private ActorType objectID;
        public ActorType ObjectID
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
        }
        private ScoringDB.ConditionInfo condition;
        public ScoringDB.ConditionInfo Condition
        {
            get { return condition; }
        }
        public ScoringRuleType(pScoringRuleType s)
        {
            this.name = s.Name;
            this.unit = new ActorType(s.Unit);
            this.objectID = null;
             this.condition = new ScoringDB.ConditionInfo(ScoringDB.ConditionInfo.ConditionType.ObjectExists, null, null);
 

            if (null != s.ObjectID)
            {
                this.objectID = new ActorType(s.ObjectID);
                this.condition = new ScoringDB.ConditionInfo( ScoringDB.ConditionInfo.ConditionType.StateChange,s.From,s.NewState);
            }

            this.newState = s.NewState;
            this.from = s.From;
            this.increment = s.Increment;
        }
    }
    public class ScoreType
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private List<string> rules = new List<string>();
        public List<string> Rules
        {
            get { return rules; }
        }
        private List<string> applies = new List<string>();
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
        public ScoreType(pScoreType s)
        {
            this.name = s.Name;
            this.initial = s.Initial;
            for (int i = 0; i < s.Rules.Count; i++)
            {
                if (NameLists.ruleNames.ContainsKey(s.Rules[i]))
                {
                    this.rules.Add(s.Rules[i]);
                }
                else
                {
                    throw new ApplicationException("Unknown scoring rule " + s.Rules[i] + " in score " + s.Name);
                }
            } 
            for (int i = 0; i < s.Applies.Count; i++) this.applies.Add(s.Applies[i]);
            if (0 < s.Viewers.Count)
            {
                for (int i = 0; i < s.Viewers.Count; i++) this.viewers.Add(s.Viewers[i]);
            }
            else
            {
                this.viewers = this.applies;
            }
        }
    }

    /// <summary>
    /// ScoringRules is a table that holds or returns entire scoring rules
    /// </summary>
    public class ScoringRules
    {
        private static Dictionary<string, ScoringRuleType> rulesTable = new Dictionary<string, ScoringRuleType>();
        public static void Clear()
        {
            // this should be called from Scores.Clear() only
            rulesTable = new Dictionary<string, ScoringRuleType>();
        }
        public static void Add(ScoringRuleType s)
        {
            if (rulesTable.ContainsKey(s.Name))
            {
                throw new ApplicationException("Duplicate scoring rule name: " + s.Name);
            }
            rulesTable[s.Name] = s;
        }
        public static ScoringRuleType Get(string name)
        {
            
                if (!rulesTable.ContainsKey(name))
                {
                    throw new ApplicationException("Attempt to access non-existant rule " + name);
                }
                return rulesTable[name];
           
        }

    }
    /// <summary>
    /// Scores holds all the defined scores
    /// </summary>
    public class Scores
    {
        public static void Clear()
        {
            // Clear any data structures here, and the rules
            ScoringRules.Clear();
        }
        public static void Register(ScoreType st)
        {
            ScoringDB.Score s = new ScoringDB.Score(st.Name, new List<string>(), new List<string>(), st.Initial);
            for (int i = 0; i < st.Applies.Count; i++) s.calculateDMs.Add(st.Applies[i]);
            for (int i = 0; i < st.Viewers.Count; i++) s.displayDMs.Add(st.Viewers[i]);
            for (int i = 0; i < st.Rules.Count; i++)
            {
                string thisRuleName = st.Rules[i];
                ScoringRuleType thisRule = ScoringRules.Get(thisRuleName);
                if (ScoringDB.ConditionInfo.ConditionType.ObjectExists == thisRule.Condition.conditionType)
                {
                    for (int unitLoc = 0; unitLoc < thisRule.Unit.Region.Zone.Count; unitLoc++)
                    {
                        ScoringDB.ActorInfo.OwnerInfo unitOwnerInfo = new ScoringDB.ActorInfo.OwnerInfo(thisRule.Unit.Owner);
                        ScoringDB.ActorInfo.IdentifierInfo unitIdentifierInfo = thisRule.Unit.ID;
                        ScoringDB.ActorInfo.LocationInfo unitLocationInfo = new ScoringDB.ActorInfo.LocationInfo(thisRule.Unit.Region.Relationship, thisRule.Unit.Region.Zone[unitLoc]);
                                     ScoringDB.ConditionInfo conditionInfo = new ScoringDB.ConditionInfo(thisRule.Condition.conditionType, thisRule.Condition.oldState, thisRule.Condition.newState);
                        s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(unitOwnerInfo, unitIdentifierInfo, unitLocationInfo), conditionInfo, null,thisRule.Increment));

                    }             
                }
                else
                {
                  for(int unitLoc=0; unitLoc<thisRule.Unit.Region.Zone.Count;unitLoc++)
                    {
                      for (int objectLoc=0;objectLoc<thisRule.Unit.Region.Zone.Count;objectLoc++)
                      {
                    ScoringDB.ActorInfo.OwnerInfo unitOwnerInfo = new ScoringDB.ActorInfo.OwnerInfo(thisRule.Unit.Owner);
                    ScoringDB.ActorInfo.IdentifierInfo unitIdentifierInfo = thisRule.Unit.ID;
                    ScoringDB.ActorInfo.LocationInfo unitLocationInfo = new ScoringDB.ActorInfo.LocationInfo(thisRule.Unit.Region.Relationship, thisRule.Unit.Region.Zone[unitLoc]);
                    ScoringDB.ActorInfo.OwnerInfo objectOwnerInfo = new ScoringDB.ActorInfo.OwnerInfo(thisRule.ObjectID.Owner);
                    ScoringDB.ActorInfo.IdentifierInfo objectIdentifierInfo = thisRule.ObjectID.ID;
                    ScoringDB.ActorInfo.LocationInfo objectLocationInfo = new ScoringDB.ActorInfo.LocationInfo(thisRule.ObjectID.Region.Relationship, thisRule.ObjectID.Region.Zone[objectLoc]);
                    ScoringDB.ConditionInfo conditionInfo = new ScoringDB.ConditionInfo(thisRule.Condition.conditionType, thisRule.Condition.oldState, thisRule.Condition.newState);
                    s.rules.Add(new ScoringDB.ScoringRule(new ScoringDB.ActorInfo(unitOwnerInfo, unitIdentifierInfo, unitLocationInfo), conditionInfo, new ScoringDB.ActorInfo(objectOwnerInfo, objectIdentifierInfo, objectLocationInfo), thisRule.Increment));
                      }
                    }             

                }
                }
            ScoringDB.scores[st.Name] = s;
        }
    }
}


