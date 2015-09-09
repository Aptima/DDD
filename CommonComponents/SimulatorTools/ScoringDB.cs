using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;

namespace Aptima.Asim.DDD.CommonComponents.SimulatorTools
{
    public class ScoringDB
    {
        public static Dictionary<string, Score> scores = new Dictionary<string, Score>();

        public static void UpdateScore_StateChange(ActorFrame actorFrame, string oldState,string newState, ActorFrame acteeFrame)
        {
            foreach (Score score in scores.Values)
            {
                score.UpdateScore_StateChange(actorFrame, oldState,newState, acteeFrame);
            }
        }

        public static void UpdateScore_ObjectExists(ActorFrame actorFrame)
        {
            foreach (Score score in scores.Values)
            {
                score.UpdateScore_ObjectExists(actorFrame);
            }
        }

        public static void Reset()
        {
            foreach (Score score in scores.Values)
            {
                score.Reset();
            }
        }

        public class ActorInfo
        {
            public class OwnerInfo
            {
                public OwnerInfo(OwnerType type)
                {
                    ownerType = type;
                }
                public enum OwnerType
                {
                    Myself,
                    FriendlyDM,
                    HostileDM
                }
                public OwnerType ownerType;
            }

            public class IdentifierInfo
            {
                public IdentifierInfo(IdentifierType type, string identifier)
                {
                    this.identifierType = type;
                    this.identifier = identifier;
                }
                public enum IdentifierType
                {
                    Instance,
                    Species,
                    Any
                }
                public IdentifierType identifierType;
                public string identifier;
            }

            public class LocationInfo
            {
                public LocationInfo(LocationType type, string zoneName)
                {
                    this.zoneName = zoneName;
                    this.locationType = type;
                }
                public enum LocationType
                {
                    Anywhere,
                    InZone,
                    NotInZone
                }
                public LocationType locationType;

                public string zoneName;
            }

            public OwnerInfo ownerInfo;
            public IdentifierInfo identifierInfo;
            public LocationInfo locationInfo;
            public ActorInfo(OwnerInfo owner, IdentifierInfo identity, LocationInfo location)
            {
                ownerInfo = owner;
                identifierInfo = identity;
                locationInfo = location;
            }

            public bool MatchFrame(ActorFrame frame, List<string> forDMs)
            {
                switch (ownerInfo.ownerType)
                {
                    case OwnerInfo.OwnerType.Myself:
                        if (!forDMs.Contains(frame.ownerID))
                        {
                            return false;
                        }
                        break;
                    case OwnerInfo.OwnerType.FriendlyDM:
                        foreach (string forDM in forDMs)
                        {
                            if (StateDB.isHostile(forDM, frame.ownerID))
                            {
                                return false;
                            }
                        }
                        break;
                    case OwnerInfo.OwnerType.HostileDM:
                        foreach (string forDM in forDMs)
                        {
                            if (!StateDB.isHostile(forDM, frame.ownerID))
                            {
                                return false;
                            }
                        }
                        break;
                }

                switch (identifierInfo.identifierType)
                {
                    case IdentifierInfo.IdentifierType.Instance:
                        if (identifierInfo.identifier != frame.objectID)
                        {
                            return false;
                        }
                        break;
                    case IdentifierInfo.IdentifierType.Species:
                        //if (!Genealogy.UnderSpecies( identifierInfo.identifier, frame.speciesName))

                        if (identifierInfo.identifier != frame.speciesName)
                        {
                            return false;
                        }
                        break;
                    case IdentifierInfo.IdentifierType.Any:
                        break;
                }

                switch (locationInfo.locationType)
                {
                    case LocationInfo.LocationType.InZone:
                        if (!frame.activeRegions.Contains(locationInfo.zoneName))
                        {
                            return false;
                        }
                        break;
                    case LocationInfo.LocationType.NotInZone:
                        if (frame.activeRegions.Contains(locationInfo.zoneName))
                        {
                            return false;
                        }
                        break;
                    case LocationInfo.LocationType.Anywhere:
                        break;
                }

                return true;
            }


        }

        public class ConditionInfo
        {
            public enum ConditionType
            {
                StateChange,
                ObjectExists
            }

            public string oldState;
            public string newState;
            public ConditionType conditionType;

            public ConditionInfo(ConditionType condition, string oldState, string newState)
            {
                this.oldState = oldState;
                this.newState = newState;
                this.conditionType = condition;
            }
        }

        public class ScoringRule
        {


            public ConditionInfo condition;
            public ActorInfo actor, actee;
            public double deltaValue;

            public ScoringRule(ActorInfo actor, ConditionInfo condition, ActorInfo actee, double deltaValue)
            {
                this.actor = actor;
                this.condition = condition;
                this.actee = actee;
                this.deltaValue = deltaValue;

                if (condition.conditionType == ConditionInfo.ConditionType.ObjectExists)
                {
                    this.actee = null;
                }
            }
        }

        public class Score
        {
            public string name;
            public List<string> calculateDMs;
            public List<string> displayDMs;
            public List<ScoringRule> rules;
            public double initialValue;

            public double scoreValue;
            public Score(string name, List<string> calculateDMs, List<string> displayDMs, double initialValue)
            {
                this.name = name;
                this.calculateDMs = calculateDMs;
                this.displayDMs = displayDMs;
                
                this.initialValue = initialValue;
                this.scoreValue = initialValue;
                this.rules = new List<ScoringRule>();
            }

            public void Reset()
            {
                scoreValue = initialValue;
            }


            public double GetScore()
            {
                return scoreValue;
            }

            public void UpdateScore_StateChange(ActorFrame actorFrame, string oldState, string newState, ActorFrame acteeFrame)
            {
                foreach (ScoringRule r in rules)
                {
                    if (r.condition.conditionType == ConditionInfo.ConditionType.StateChange)
                    {
                        if (r.actor.MatchFrame(actorFrame, calculateDMs) &&
                            r.actee.MatchFrame(acteeFrame, calculateDMs) &&
                            (r.condition.oldState == "" || r.condition.oldState == oldState) &&
                            r.condition.newState == newState)
                        {
                            this.scoreValue += r.deltaValue;
                        }
                    }
                }
            }

            public void UpdateScore_ObjectExists(ActorFrame actorFrame)
            {
                foreach (ScoringRule r in rules)
                {
                    if (r.condition.conditionType == ConditionInfo.ConditionType.ObjectExists)
                    {
                        if (r.actor.MatchFrame(actorFrame, calculateDMs))
                        {
                            this.scoreValue += r.deltaValue;
                        }
                    }
                }
            }
        }

        public class ActorFrame
        {
            public string objectID;
            public string speciesName;
            public string ownerID;
            public List<string> activeRegions;
            public ActorFrame()
            {
                this.objectID = "";
                this.speciesName = "";
                this.ownerID = "";
                this.activeRegions = null; ;
            }
            public ActorFrame(string objectID,
                              string speciesName,
                              string ownerID,
                              List<string> activeRegions)
            {
                this.objectID = objectID;
                this.speciesName = speciesName;
                this.ownerID = ownerID;
                this.activeRegions = activeRegions;
            }
        }
    }
}
