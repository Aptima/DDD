using System;
using System.Collections.Generic;
using Aptima.Asim.DDD.ScenarioParser;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.ScenarioController
{

    /// <summary>
    /// This modukle tracks data structures used by the controller
    /// </summary>

    public class ObjectDictionary : BaseObjectDictionary
    {
        /*
                Dictionary<string, object> ContainedData;
                public object this[string name]
                {
                    get { return ContainedData[name]; }
                    set { ContainedData[name] = value; }
                }
                public Boolean New(string s, object o)
                {
                    Boolean returnValue=false;
                    if (!this.ContainsKey(s))
                    {
                        this[s] = o;
                        returnValue = true;
                    }
                    return returnValue;
                }
                public List<string> GetKeys()
                {
                    return new List<string>(ContainedData.Keys);
                }
                public void Add(string s, object o)
                {
                    ContainedData[s] = o;
                }

                public Boolean ContainsKey(string s)
                {
                    return ContainedData.ContainsKey(s);
                }
                */

        public Dictionary<string, object> GetDictionary()
        {

            Dictionary<string, object> returnValue = new Dictionary<string, object>();
            foreach (string s in ContainedData.Keys)
            {


                try
                {
                    switch (ContainedData[s].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            returnValue[s] = ContainedData[s];
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.LocationType":
                            returnValue[s] = ((LocationType)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                            returnValue[s] = ((VelocityType)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.StateBody":
                            returnValue[s] = ((StateBody)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.ExtendedStateBody":
                            returnValue[s] = ((ExtendedStateBody)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.ExtendedStateType":
                            returnValue[s] = ((ExtendedStateType)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.ActiveRegionStateType":
                            returnValue[s] = ((ActiveRegionStateType)ContainedData[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            returnValue[s] = ContainedData[s];
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Could not DeepCopy DictionaryType in *.GetDictionary", e);
                }


            }
            return returnValue;
        }
        /*
        public List<string> Keys
        {
            get { return new List<string>(this.ContainedData.Keys); }
        }
*/
        public ObjectDictionary()
            : base()
        {
            ContainedData = new Dictionary<string, object>();
        }
        public ObjectDictionary(Dictionary<string, object> d)
            : base()
        {
            ContainedData = new Dictionary<string, object>();
            foreach (string s in d.Keys)
            {
                try
                {
                    switch (d[s].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            ContainedData[s] = d[s];
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.LocationType":
                            ContainedData[s] = ((LocationType)d[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                            ContainedData[s] = ((VelocityType)d[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.ActiveRegionStateType":
                            ContainedData[s] = ((ActiveRegionStateType)d[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            ContainedData[s] = d[s];
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Could not DeepCopy DictionaryType", e);
                }
            }
        }

        public ObjectDictionary DeepCopy()
        {

            ObjectDictionary returnValue = new ObjectDictionary();
            foreach (string s in this.ContainedData.Keys)
            {
                try
                {
                    if (this[s] == null)
                        continue;
                    switch (this[s].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            returnValue[s] = this[s];
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.LocationType":
                            returnValue[s] = ((LocationType)this[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                            returnValue[s] = ((VelocityType)this[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.EngramType":
                            returnValue[s] = ((EmitterType)this[s]).DeepCopy();
                            break;
                        case "System.Collections.Generic.List`1[System.String]":
                            returnValue[s] = new List<string>(((List<string>)this[s]).ToArray());
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            returnValue[s] = this[s];
                            break;
                        default:
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Could not DeepCopy DictionaryType", e);
                }
            }

            return returnValue;

        }

        public ObjectDictionary DeepCopy(List<string> excludeList)
        {
            if (excludeList == null)
            {
                return DeepCopy();
            }
            ObjectDictionary returnValue = new ObjectDictionary();
            foreach (string s in this.ContainedData.Keys)
            {
                if (excludeList.Contains(s))
                {
                    continue;
                }
                try
                {
                    if (this[s] == null)
                        continue;
                    switch (this[s].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            returnValue[s] = this[s];
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.LocationType":
                            returnValue[s] = ((LocationType)this[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                            returnValue[s] = ((VelocityType)this[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.EngramType":
                            returnValue[s] = ((EmitterType)this[s]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            returnValue[s] = this[s];
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Could not DeepCopy DictionaryType", e);
                }
            }

            return returnValue;
        }
    }



    public class ActiveRegionStateType
    {
        private Boolean isActive = true;
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private Boolean isVisible = true;
        public Boolean IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        public ActiveRegionStateType(Boolean isActive, Boolean isVisible)
        {
            this.isVisible = isVisible;
            this.isActive = isActive;
        }
        public ActiveRegionStateType(RegionEventType r)
        {
            this.isActive = r.IsActive;
            this.isVisible = r.IsVisible;
        }
        public ActiveRegionStateType DeepCopy()
        {
            ActiveRegionStateType returnValue = new ActiveRegionStateType(this.isActive, this.isVisible);
            return returnValue;
        }
    }

    public class Cone
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
        private LocationType direction;
        public LocationType Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private string level;
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        public Cone()
        {
            this.direction = new LocationType();
        }
        public Cone(pCone cone)
        {
            this.spread = cone.Spread;
            this.extent = cone.Extent;
            this.direction = new LocationType(cone.Direction);
            this.level = cone.Level;
        }
        public Cone DeepCopy()
        {
            Cone returnValue = new Cone();
            returnValue.Spread = this.Spread;
            returnValue.Extent = this.Extent;
            returnValue.Direction = this.direction.DeepCopy();
            // surely this is an error        this.level = this.Level;
            returnValue.Level = this.Level;
            return returnValue;
        }
    }
    public class SensorType
    {
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
        private List<Cone> cones = new List<Cone>();
        public List<Cone> Cones
        {
            get { return cones; }
        }

        public SensorType(string attribute, Boolean isEngram, string typeIfEngram, List<pCone> cones)
        {
            this.attribute = attribute;
            this.isEngram = isEngram;
            this.typeIfEngram = typeIfEngram;
            for (int i = 0; i < cones.Count; i++)
            {
                this.cones.Add(new Cone(cones[i]));
            }
        }
        public SensorType(string attribute, Boolean isEngram, string typeIfEngram, List<Cone> cones)
        {
            this.attribute = attribute;
            this.isEngram = isEngram;
            this.typeIfEngram = typeIfEngram;
            for (int i = 0; i < cones.Count; i++)
            {
                this.cones.Add(cones[i]);
            }
        }
        public SensorType DeepCopy()
        {
            return new SensorType(this.attribute, this.isEngram, this.typeIfEngram, this.cones);
        }
    }


    public class TransitionType : IComparable<TransitionType>
    {
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
        private int effect;
        public int Effect
        {
            get { return effect; }
        }
        private string state;
        public string State
        {
            get { return state; }
        }

        public int CompareTo(TransitionType rhs)
        {
            // return -1 if this is smaller than rhs, 0 or +1 are therefore obvious
            int returnValue = (this.range).CompareTo(rhs.Range);
            // but a range of 0 stands for no range limit, so is never smaller
            if (0 > this.range)
            {
                returnValue = -returnValue;
            }
            if (0 == returnValue)
            {
                returnValue = -(this.effect).CompareTo(rhs.Effect);
            }
            return returnValue;
        }
        public TransitionType DeepCopy()
        {
            TransitionType t = new TransitionType(this.range, this.probability, this.effect, this.state);
            return t;
        }
        public TransitionType(double range, double probability, int effect, string state)
        {
            this.range = range;
            this.probability = probability;
            this.effect = effect;
            this.state = state;
        }
        public TransitionType(pTransitionType t)
        {
            this.range = t.Range;
            this.probability = t.Probability;
            this.effect = t.Effect;
            this.state = t.State;
        }
        public TransitionType(TransitionType t)
        {
            this.range = t.Range;
            this.probability = t.Probability;
            this.effect = t.Effect;
            this.state = t.State;
        }
    }
    public class SingletonVulnerabilityType
    {

        private List<TransitionType> transitions;
        public List<TransitionType> Transitions
        {
            get { return transitions; }
        }
        public SingletonVulnerabilityType(List<TransitionType> transitions)
        {

            this.transitions = transitions;
        }
        public SingletonVulnerabilityType DeepCopy()
        {
            SingletonVulnerabilityType s = new SingletonVulnerabilityType();
            for (int i = 0; i < transitions.Count; i++)
                s.transitions.Add(this.transitions[i]);
            return s;
        }

        public SingletonVulnerabilityType(pSingletonVulnerabilityType v)
        {
            this.transitions = new List<TransitionType>();
            for (int t = 0; t < v.Transitions.Count; t++)
            {
                this.transitions.Add(new TransitionType(v.Transitions[t]));
            }

        }
        public SingletonVulnerabilityType(SingletonVulnerabilityType v)
        {
            this.transitions = new List<TransitionType>();
            for (int t = 0; t < v.Transitions.Count; t++)
            {
                this.transitions.Add(new TransitionType(v.Transitions[t]));
            }

        }
        public SingletonVulnerabilityType()
        {
            this.transitions = new List<TransitionType>();
        }
    }
    public class ContributionType
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
        private double probability;
        public double Probability
        {
            get { return probability; }
        }
        public ContributionType DeepCopy()
        {
            return new ContributionType(this.capability, this.effect, this.range, this.probability);
        }
        public ContributionType(string capability, int effect, double range, double probability)
        {
            this.capability = capability;
            this.effect = effect;
            this.range = range;
            this.probability = probability;
        }
        public ContributionType(pContributionType c)
        {
            this.capability = c.Capability;
            this.effect = c.Effect;
            this.range = c.Range;
            this.probability = c.Probability;
        }
        public ContributionType(ContributionType c)
        {
            this.capability = c.Capability;
            this.effect = c.Effect;
            this.range = c.Range;
            this.probability = c.Probability;
        }
    }
    public class ComboVulnerabilityType
    {
        private List<ContributionType> contributions;
        public List<ContributionType> Contributions
        {
            get { return contributions; }
        }
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        public ComboVulnerabilityType DeepCopy()
        {
            ComboVulnerabilityType c = new ComboVulnerabilityType(this.newState);
            for (int i = 0; i < this.contributions.Count; i++)
            {
                c.Contributions.Add(this.contributions[i]);
            }
            return c;
        }

        public ComboVulnerabilityType()
        {
            contributions = new List<ContributionType>();
            newState = "";
        }
        public ComboVulnerabilityType(string state)
        {
            contributions = new List<ContributionType>();
            newState = state;

        }
        public ComboVulnerabilityType(List<ContributionType> contributions, string newState)
        {
            this.contributions = contributions;
            /*        for (int c = 0; c < contributions.Count; c++)
                    {
                        this.contributions.Add(contributions[c]);
                    }
             * */
            this.newState = newState;
        }

        public ComboVulnerabilityType(pComboVulnerabilityType c)
        {
            this.contributions = new List<ContributionType>();
            for (int n = 0; n < c.Contributions.Count; n++)
            {
                this.contributions.Add(new ContributionType(c.Contributions[n]));
            }
            this.newState = c.NewState;
        }

        public ComboVulnerabilityType(ComboVulnerabilityType c)
        {
            this.contributions = new List<ContributionType>();
            for (int n = 0; n < c.Contributions.Count; n++)
            {
                this.contributions.Add(new ContributionType(c.Contributions[n]));
            }
            this.newState = c.NewState;
        }
    }
    public class EffectType : IComparable<EffectType>
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

        public EffectType DeepCopy()
        {
            return new EffectType(this.intensity, this.probability);
        }
        public int CompareTo(EffectType rhs)
        {
            // return -1 is this < rhs
            // < defined by: if prob A < prob B then A<B
            //               if probs equal then larger intensity comes first
            int returnValue = this.Probability.CompareTo(rhs.Probability);
            if (0 == returnValue)
            {
                returnValue = -this.Intensity.CompareTo(rhs.Intensity);
            }
            return returnValue;
        }

        public EffectType(int intensity, double probability)
        {
            this.intensity = intensity;
            this.probability = probability;
        }
        public EffectType(pEffectType e)
        {
            this.intensity = e.Intensity;
            this.probability = e.Probability;
        }
        public EffectType(EffectType e)
        {
            this.intensity = e.Intensity;
            this.probability = e.Probability;
        }
    }
    public class ProximityType : IComparable<ProximityType>
    {
        private double range;
        public double Range
        {
            get { return range; }
        }
        private List<EffectType> effectList;
        public List<EffectType> EffectList
        {
            get { return effectList; }
        }
        public ProximityType DeepCopy()
        {
            this.EffectList.Sort();
            ProximityType p = new ProximityType(this.range);
            for (int i = 0; i < this.EffectList.Count; i++)
            {
                p.EffectList.Add(EffectList[i].DeepCopy());

            }
            return p;
        }
        public int CompareTo(ProximityType rhs)
        {
            // this < rhs if this.range < rhs.range
            // if equal there's no reasonable way to compare the effectlists
            return this.range.CompareTo(rhs.Range);
        }
        public ProximityType(double radius)
        {
            this.range = radius;
            this.effectList = new List<EffectType>();
        }
        public ProximityType(pProximityType proximity)
        {
            this.range = proximity.Range;
            this.effectList = new List<EffectType>();
            for (int a = 0; a < proximity.EffectList.Count; a++)
            {
                effectList.Add(new EffectType(proximity.EffectList[a]));
            }
        }
        public ProximityType(ProximityType proximity)
        {
            this.range = proximity.Range;
            this.effectList = new List<EffectType>();
            for (int a = 0; a < proximity.EffectList.Count; a++)
            {
                effectList.Add(new EffectType(proximity.EffectList[a]));
            }
        }
        public void Add(EffectType effect)
        {
            effectList.Add(effect);
        }
    }
    public class CapabilityType
    {

        private List<ProximityType> proximityList;
        public List<ProximityType> ProximityList
        {
            get { return proximityList; }
        }
        public CapabilityType DeepCopy()
        {
            this.proximityList.Sort();
            CapabilityType c = new CapabilityType();
            for (int i = 0; i < this.proximityList.Count; i++)
            {
                c.Add(this.proximityList[i].DeepCopy());
            }
            return c;
        }
        public CapabilityType()
        {
            this.proximityList = new List<ProximityType>();
        }
        public CapabilityType(pCapabilityType capability)
        {

            this.proximityList = new List<ProximityType>();
            for (int c = 0; c < capability.ProximityList.Count; c++)
            {
                proximityList.Add(new ProximityType(capability.ProximityList[c]));
            }

        }
        public CapabilityType(CapabilityType capability)
        {

            this.proximityList = new List<ProximityType>();
            for (int c = 0; c < capability.ProximityList.Count; c++)
            {
                proximityList.Add(new ProximityType(capability.ProximityList[c]));
            }

        }
        public void Add(ProximityType proximity)
        {
            proximityList.Add(proximity);
        }

    }
    public class EmitterType
    {
        private Boolean isEngram;
        public Boolean IsEngram
        {
            get { return isEngram; }
        }
        private string typeIfEngram = "";
        public string TypeIfEngram
        {
            get { return typeIfEngram; }
        }
        private Dictionary<string, double> levels = new Dictionary<string, double>();
        public Dictionary<string, double> Levels
        {
            get { return levels; }
        }
        public EmitterType DeepCopy()
        {
            EmitterType returnValue = new EmitterType();
            returnValue.isEngram = this.isEngram;
            returnValue.typeIfEngram = this.typeIfEngram;
            returnValue.levels = new Dictionary<string, double>();

            foreach (string k in this.levels.Keys)
                returnValue.levels.Add(k, this.levels[k]);

            return returnValue;
        }

        public EmitterType()
        { }
        public EmitterType(pEmitterType emitter)
        {
            this.isEngram = emitter.IsEngram;
            this.typeIfEngram = emitter.TypeIfEngram;
            this.levels = new Dictionary<string, double>();
            foreach (string k in emitter.Levels.Keys)
            {
                this.levels.Add(k, emitter.Levels[k]);
            }
        }
    }
    public class StateBody
    {

        private List<string> sensors = new List<string>();
        public List<string> Sensors
        {
            get { return sensors; }
            set { sensors = value; }
        }
        private Dictionary<string, CapabilityType> capabilities = new Dictionary<string, CapabilityType>();
        public Dictionary<string, CapabilityType> Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }
        private Dictionary<string, SingletonVulnerabilityType> vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
        public Dictionary<string, SingletonVulnerabilityType> Vulnerabilities
        {
            get { return vulnerabilities; }
            set { vulnerabilities = value; }
        }
        private List<ComboVulnerabilityType> combinations = new List<ComboVulnerabilityType>();
        public List<ComboVulnerabilityType> Combinations
        {
            get { return combinations; }
            set { combinations = value; }
        }

        private ObjectDictionary parameters = new ObjectDictionary();
        public ObjectDictionary Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        private Dictionary<string, EmitterType> emitters = new Dictionary<string, EmitterType>();
        public Dictionary<string, EmitterType> Emitters
        {
            get { return emitters; }
            set { emitters = value; }
        }
        public Dictionary<string, CapabilityType> DeepCopyCapabilities()
        {
            Dictionary<string, CapabilityType> returnValue = new Dictionary<string, CapabilityType>();
            foreach (string k in this.capabilities.Keys)
            {
                returnValue.Add(k, this.capabilities[k].DeepCopy());
            }
            return returnValue;

        }

        public Dictionary<string, SingletonVulnerabilityType> DeepCopyVulnerabilities()
        {
            Dictionary<string, SingletonVulnerabilityType> returnValue = new Dictionary<string, SingletonVulnerabilityType>();
            foreach (string k in this.vulnerabilities.Keys)
            {
                returnValue.Add(k, this.vulnerabilities[k].DeepCopy());
            }
            return returnValue;
        }
        public List<ComboVulnerabilityType> DeepCopyCombos()
        {
            List<ComboVulnerabilityType> returnValue = new List<ComboVulnerabilityType>();
            for (int i = 0; i < this.combinations.Count; i++)
            {
                returnValue.Add(this.combinations[i].DeepCopy());
            }
            return returnValue;
        }
        public StateBody DeepCopy()
        {
            StateBody s = new StateBody();
            for (int i = 0; i < sensors.Count; i++)
            {
                s.Sensors.Add(sensors[i]);
            }
            foreach (string k in this.capabilities.Keys)
            {
                s.Capabilities.Add(k, this.capabilities[k].DeepCopy());
            }
            foreach (string k in this.vulnerabilities.Keys)
            {
                s.Vulnerabilities.Add(k, this.vulnerabilities[k].DeepCopy());
            }
            for (int i = 0; i < this.combinations.Count; i++)
            {
                s.Combinations.Add(this.combinations[i].DeepCopy());
            }

            ObjectDictionary temp = this.Parameters.DeepCopy();
            List<string> KeyList = temp.GetKeys();
            foreach (string k in KeyList)
            {
                s.Parameters.Add(k, temp[k]);
            }
            foreach (string k in this.emitters.Keys)
            {
                s.Emitters.Add(k, this.emitters[k].DeepCopy());
            }
            
            return s;
        }
        public StateBody()
        {
            /* replacd these by explicit initialization in definitions
            this.parameters = new ObjectDictionary();
            this.sensors = new List<string>();
            this.capabilities = new Dictionary<string, CapabilityType>();
            this.vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
            this.combinations = new List<ComboVulnerabilityType>();
            this.emitters = new Dictionary<string, EmitterType>();
             * */
        }

        public StateBody(pStateBody stateBody)
        {
            /* Here, converting from a pType to a ScenCon type, we have to use the
             * explicit conversions as the pType is not an ObjectDictionary
             */
            //   this.parameters = new ObjectDictionary();
            if ("" != stateBody.Icon) this.parameters["Icon"] = stateBody.Icon;
            if (null != stateBody.LaunchDuration) this.parameters["LaunchDuration"] = stateBody.LaunchDuration;
            if (null != stateBody.DockingDuration) this.parameters["DockingDuration"] = stateBody.DockingDuration;
            //    if (null != stateBody.TimeToRemove) this.parameters["TimeToRemove"] = stateBody.TimeToRemove;
            if (null != stateBody.TimeToAttack) this.parameters["TimeToAttack"] = stateBody.TimeToAttack;
            if (null != stateBody.EngagementDuration) this.parameters["EngagementDuration"] = stateBody.EngagementDuration;
            if (null != stateBody.MaximumSpeed) this.parameters["MaximumSpeed"] = stateBody.MaximumSpeed;
            this.parameters["FuelCapacity"] = 0;
            if (null != stateBody.FuelCapacity) this.parameters["FuelCapacity"] = stateBody.FuelCapacity;
            if (null != stateBody.InitialFuelLoad) this.parameters["InitialFuelLoad"] = stateBody.InitialFuelLoad;
            if (null != stateBody.FuelConsumptionRate) this.parameters["FuelConsumptionRate"] = stateBody.FuelConsumptionRate;
            if (null != stateBody.FuelDepletionState) this.parameters["FuelDepletionState"] = stateBody.FuelDepletionState;
            if (null != stateBody.Stealable) this.parameters["Stealable"] = stateBody.Stealable;
            //if (null != stateBody.ClassificationDisplayRules) this.parameters["ClassificationDisplayRules"] = stateBody.ClassificationDisplayRules;
            for (int i = 0; i < stateBody.Sensors.Count; i++)
            {

                this.sensors.Add(stateBody.Sensors[i]);
            }

            //        this.capabilities = new Dictionary<string, CapabilityType>();
            foreach (string s in stateBody.Capabilities.Keys)
            {
                this.capabilities[s] = new CapabilityType(stateBody.Capabilities[s]);
            }
            //         this.vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
            foreach (string v in stateBody.Vulnerabilities.Keys)
            {
                this.vulnerabilities[v] = new SingletonVulnerabilityType(stateBody.Vulnerabilities[v]);
            }
            //        this.combinations = new List<ComboVulnerabilityType>();
            for (int c = 0; c < stateBody.Combinations.Count; c++)
            {
                this.combinations.Add(new ComboVulnerabilityType(stateBody.Combinations[c]));
            }
            /* a pEmitterType is a quad (attribute,isEngram,typeIfEngram,Levels)
             * where Levels itself is an object dictionary
             * this.emitters is a dictionary attribute->Levels
             * (so here we drop the attribute from the rhs
             */

            //       this.emitters = new Dictionary<string, EmitterType>();



            foreach (string a in stateBody.Emitters.Keys)
            {
                if ("Default" == a)
                {
                    this.emitters["All"] = new EmitterType();

                }
                else if ("Invisible" == a)
                {
                    this.emitters["Invisible"] = new EmitterType();

                }
                else
                {
                    this.emitters[a] = new EmitterType(stateBody.Emitters[a]);
                    /*   this.emitters[a] = new         ObjectDictionary();
                       foreach (string lName in stateBody.Emitters[a].Levels.Keys)
                       {
                           this.emitters[a][lName] = stateBody.Emitters[a].Levels[lName];
                       }*/
                }
            }

        }



    }
    /// <summary>
    /// A state type is used internally to carry the components of a state
    /// </summary>
    public class StateType
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private StateBody body;
        public StateBody Body
        {
            get { return body; }
        }
        public StateType DeepCopy()
        {
            return new StateType(this.name, this.body.DeepCopy());
        }

        public object GetSetting(string s)
        {
            object parameterValue = null;
            if (body.Parameters.ContainsKey(s))
            {
                parameterValue = body.Parameters[s];
            }
            return parameterValue;
        }




        /// <summary>
        /// SetSetting will override an existing key-value pair or add a new one.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterSetting"></param>
        public void SetSetting(string parameterName, object parameterSetting)
        {
            this.body.Parameters[parameterName] = parameterSetting;
        }

        public StateType(string name)
        {
            this.name = name;
            this.body = new StateBody();

        }
        public StateType(string name, StateBody body)
        {
            this.name = name;
            this.body = body;
        }
        public StateType(pStateType state)
        {
            this.name = state.Name;
            this.body = new StateBody(state.Body);
        }
    }
    public class ExtendedStateBody
    {
        private Dictionary<string, SensorType> sensors = new Dictionary<string, SensorType>();
        public Dictionary<string, SensorType> Sensors
        {
            get { return sensors; }
            set { sensors = value; }
        }
        private Dictionary<string, CapabilityType> capabilities = new Dictionary<string, CapabilityType>();
        public Dictionary<string, CapabilityType> Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }
        private Dictionary<string, SingletonVulnerabilityType> vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
        public Dictionary<string, SingletonVulnerabilityType> Vulnerabilities
        {
            get { return vulnerabilities; }
            set { vulnerabilities = value; }
        }
        private List<ComboVulnerabilityType> combinations = new List<ComboVulnerabilityType>();
        public List<ComboVulnerabilityType> Combinations
        {
            get { return combinations; }
            set { combinations = value; }
        }

        private ObjectDictionary parameters = new ObjectDictionary();
        public ObjectDictionary Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        private Dictionary<string, EmitterType> emitters = new Dictionary<string, EmitterType>();
        public Dictionary<string, EmitterType> Emitters
        {
            get { return emitters; }
            set { emitters = value; }
        }
        public ExtendedStateBody DeepCopy()
        {
            ExtendedStateBody s = new ExtendedStateBody();
            foreach (string sensorName in sensors.Keys)
            {
                s.Sensors.Add(sensorName, sensors[sensorName]);
            }
            foreach (string k in this.capabilities.Keys)
            {
                s.Capabilities.Add(k, this.capabilities[k].DeepCopy());
            }
            foreach (string k in this.vulnerabilities.Keys)
            {
                s.Vulnerabilities.Add(k, this.vulnerabilities[k].DeepCopy());
            }
            for (int i = 0; i < this.combinations.Count; i++)
            {
                s.Combinations.Add(this.combinations[i].DeepCopy());
            }

            ObjectDictionary temp = this.Parameters.DeepCopy();
            List<string> KeyList = temp.GetKeys();
            foreach (string k in KeyList)
            {
                s.Parameters.Add(k, temp[k]);
            }
            foreach (string k in this.emitters.Keys)
            {
                s.Emitters.Add(k, this.emitters[k].DeepCopy());
            }
            return s;
        }
        public ExtendedStateBody()
        {
            /*replaced by explicit initializations in definitions
            this.parameters = new ObjectDictionary();
            this.sensors = new Dictionary<string, SensorType>();
            this.capabilities = new Dictionary<string, CapabilityType>();
            this.vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
            this.combinations = new List<ComboVulnerabilityType>();
            this.emitters = new Dictionary<string, EmitterType>();
            */
        }

        public ExtendedStateBody(StateBody stateBody)
        {
            /* Here, converting from a StateBody to an ExtendedStateBody, 
             * we have to expand the sensor definitions
             */
            //     this.parameters = new ObjectDictionary();
            //      this.sensors = new Dictionary<string, SensorType>();
            for (int i = 0; i < stateBody.Sensors.Count; i++)
            {
                this.sensors.Add(stateBody.Sensors[i], SensorTable.Retrieve(stateBody.Sensors[i]));
            }
            foreach (string s in stateBody.Parameters.Keys)
            {
                try
                {
                    switch (stateBody.Parameters[s].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            this.parameters[s] = stateBody.Parameters[s];
                            break;
                        case "Aptima.Asim.DDD.ScenarioParser.pLocationType":
                            this.parameters[s] = new LocationType((pLocationType)stateBody.Parameters[s]);
                            break;
                        case "Aptima.Asim.DDD.ScenarioParser.pVelocityType":
                            this.parameters[s] = new VelocityType((pVelocityType)stateBody.Parameters[s]);
                            break;
                        case "System.Collections.Generic.List`1[System.String]":
                            this.parameters[s] = new List<string>(((List<string>)stateBody.Parameters[s]).ToArray());
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            this.parameters[s] = stateBody.Parameters[s];
                            break;
                        default:
                            break;

                    }
                }
                catch (System.Exception e)
                {
                    throw new ApplicationException("Could not DeepCopy Parameter" +
                        parameters[s].GetType().ToString() + "in Genus", e);
                }
            }
            //     this.capabilities = new Dictionary<string, CapabilityType>();
            foreach (string s in stateBody.Capabilities.Keys)
            {
                this.capabilities[s] = new CapabilityType(stateBody.Capabilities[s]);
            }
            //       this.vulnerabilities = new Dictionary<string, SingletonVulnerabilityType>();
            foreach (string v in stateBody.Vulnerabilities.Keys)
            {
                this.vulnerabilities[v] = new SingletonVulnerabilityType(stateBody.Vulnerabilities[v]);
            }
            //            this.combinations = new List<ComboVulnerabilityType>();
            for (int c = 0; c < stateBody.Combinations.Count; c++)
            {
                this.combinations.Add(new ComboVulnerabilityType(stateBody.Combinations[c]));
            }
            /* 
             * this.emitters is a dictionary attribute->Levels       
             */

            //        this.emitters = new Dictionary<string, EmitterType>();
            foreach (string a in stateBody.Emitters.Keys)
            {
                this.emitters[a] = stateBody.Emitters[a].DeepCopy();
                /*              foreach (string aName in stateBody.Emitters[a].Keys)
                              {
                                  this.emitters[a][aName] = stateBody.Emitters[a][aName];
                              }*/
            }

        }



    }
    /// <summary>
    /// A state type is used to carry the components of a state
    /// </summary>
    public class ExtendedStateType
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private ExtendedStateBody body;
        public ExtendedStateBody Body
        {
            get { return body; }
        }
        public ExtendedStateType DeepCopy()
        {
            return new ExtendedStateType(this.name, this.body.DeepCopy());
        }

        public object GetSetting(string s)
        {
            object parameterValue = null;
            if (body.Parameters.ContainsKey(s))
            {
                parameterValue = body.Parameters[s];
            }
            return parameterValue;
        }
        /// <summary>
        /// SetSetting will override an existing key-value pair or add a new one.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterSetting"></param>
        public void SetSetting(string parameterName, object parameterSetting)
        {
            this.body.Parameters[parameterName] = parameterSetting;
        }

        public ExtendedStateType(string name)
        {
            this.name = name;
            this.body = new ExtendedStateBody();

        }
        public ExtendedStateType(string name, StateBody body)
        {
            this.name = name;
            this.body = new ExtendedStateBody(body);
        }
        public ExtendedStateType(string name, ExtendedStateBody body)
        {
            this.name = name;
            this.body = body;
        }
    }
    /// <summary>
    /// Represents an (x,y,z) vector
    /// </summary>
    public class LocationType
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

        public LocationType DeepCopy()
        {
            return new LocationType(this.x, this.y, this.z);
        }

        /// <summary>
        /// Constructs an (x,y,z) vector
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public LocationType(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public LocationType(pLocationType loc)
        {
            x = loc.X;
            y = loc.Y;
            z = loc.Z;
        }
        /// <summary>
        /// Constructs a default vector (0,0,0)
        /// </summary>
        public LocationType()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /*    /// <summary>
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
        }*/

    }

    /// <summary>
    /// Represents an (x,y,z) vector
    /// </summary>
    public class VelocityType
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

        public VelocityType DeepCopy()
        {
            return new VelocityType(this.vx, this.vy, this.vz);
        }

        /// <summary>
        /// Constructs an (x,y,z) vector
        /// </summary>
        /// <param name="vx">X value</param>
        /// <param name="vy">Y value</param>
        /// <param name="vz">Z value</param>
        public VelocityType(double vx, double vy, double vz)
        {
            this.vx = vx;
            this.vy = vy;
            this.vz = vz;
        }
        public VelocityType(pVelocityType v)
        {
            vx = v.VX;
            vy = v.VY;
            vz = v.VZ;
        }

        /// <summary>
        /// Constructs a default vector (0,0,0)
        /// </summary>
        public VelocityType()
        {
            this.vx = 0;
            this.vy = 0;
            this.vz = 0;
        }
    }


    /// <summary>
    /// Holds information defining a genus
    /// </summary>
    public class GenusType
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }




        public GenusType(pGenusType g)
        {
            this.name = g.Name;
        }
    }
    /// <summary>
    /// Holds information defining a species
    /// </summary>
    public class SpeciesType
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

        public int GetSubplatformCapacity(String speciesName)
        {
            int result = 0;

            foreach (SubplatformCapacity c in SubplatformCapacities)
            {
                if (c.SpeciesName == speciesName)
                {
                    result += c.Count;
                }
            }

            return result;
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
        private Boolean launchedByOwner = false;
        public Boolean LaunchedByOwner
        {
            get { return launchedByOwner; }
        }
        //private int subplatformLimit;
        //public int SubplatformLimit
        //{
        //    get { return subplatformLimit; }
        //    set { subplatformLimit = value; }
        //}
        //private List<string> possibleSubplatforms;
        public Boolean CanHaveSubplatform(string s)
        {
            foreach (SubplatformCapacity sc in m_subplatformCapacities)
            {
                if (sc.SpeciesName == s)
                {
                    return true;
                }
            }
            return false;
        }

        private Dictionary<string, StateBody> states = new Dictionary<string, StateBody>();
        public Dictionary<string, StateBody> States
        {
            get { return states; }
            ///            set { states = value; }
        }

        public void AddState(StateType state)
        {
            this.states[state.Name] = state.Body;
        }
        //       private List<CapabilityType> capabilities;
        //       public List<CapabilityType> Capabilities
        //       {
        //           get { return capabilities; }
        //       }

        private Boolean isWeapon = false;
        public Boolean IsWeapon
        {
            get { return isWeapon; }
        }

        private String defaultClassification;
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

        private static Dictionary<string, SpeciesType> definedSpecies = new Dictionary<string, SpeciesType>();
        public static SpeciesType GetSpecies(string s)
        {
            SpeciesType returnValue;
            try
            {
                returnValue = definedSpecies[s];
                return returnValue;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error retrieving species '" + s + "': " + e.Message);
            }
        }
        public static Boolean IsExistingSpecies(string s)
        {
            return definedSpecies.ContainsKey(s);
        }
        public static void ClearSpeciesTable()
        {
            definedSpecies = new Dictionary<string, SpeciesType>();
        }
        private Boolean anyoneCanOwnMe = false;
        public void AnyoneCanOwn(Boolean b)
        {
            anyoneCanOwnMe = b;
        }
        private List<string> dMCanOwnMe = new List<string>();
        public void AddOwner(string s)
        {
            if (DecisionMakerType.IsExistingDM(s))
                dMCanOwnMe.Add(s);
            else
                throw new ApplicationException("Illegal DM name '" + s + "' cannot own species '" + this.name + "'");

        }

        public void DropOwner(string s)
        {
            if (DecisionMakerType.IsExistingDM(s))
                dMCanOwnMe.Remove(s);
        }
        public Boolean CanOwnMe(string s)
        {
            return anyoneCanOwnMe || dMCanOwnMe.Contains(s);
        }
        public void AddSubplatformCapacity(String speciesName, int count)
        {
            SubplatformCapacities.Add(new SubplatformCapacity(speciesName, count));
        }
        public SpeciesType(pSpeciesType species)
        {
            double mySize = 0;
            //        double myMaximumSpeed = 0;

            Boolean myRemoveOnDestruction = true;

            this.name = species.Name;
            this.basedOn = species.BasedOn;
            definedSpecies[this.name] = this;

            if (species.Size != null)
            {
                mySize = (double)species.Size;
            }


            if (species.IsWeapon != null)
            {
                isWeapon = (Boolean)(species.IsWeapon);
            }

            DefaultClassification = species.DefaultClassification;
            ClassificationDisplayRules = species.ClassificationDisplayRules;

            if (species.RemoveOnDestruction != null)
            {
                myRemoveOnDestruction = (Boolean)(species.RemoveOnDestruction);
            }
            anyoneCanOwnMe = species.AnyoneCanOwn;
            if (species.CountOwners() > 0)
            {
                List<string> owners = species.GetOwners();
                for (int i = 0; i < owners.Count; i++)
                    if (DecisionMakerType.IsExistingDM(owners[i]))
                    {
                        this.AddOwner(owners[i]);
                        //dMCanOwnMe.Add(owners[i]);
                    }
                    else
                        throw new ApplicationException("Owner '" + owners[i] + "' for species '"
                            + name + "' is not a valid Decision Maker name.");


               }
               launchedByOwner = species.LaunchedByOwner;
               m_subplatformCapacities = new List<SubplatformCapacity>();
               for (int i = 0; i < species.SubplatformCapacities.Count; i++)
               {
                   AddSubplatformCapacity(species.SubplatformCapacities[i].SpeciesName, species.SubplatformCapacities[i].Count);
               }

            

            foreach (string stateName in species.States.Keys)
            {
                for (int sensorName = 0; sensorName < species.States[stateName].Sensors.Count; sensorName++)
                {
                    if (!SensorTable.IsKnownSensor(species.States[stateName].Sensors[sensorName]))
                    {
                        throw new ApplicationException("Unknown sensor name: " + species.States[stateName].Sensors[sensorName]);
                    }
                }

                this.states[stateName] = new StateBody(species.States[stateName]);
                if (!this.states[stateName].Parameters.ContainsKey("TimeToAttack")) this.states[stateName].Parameters["TimeToAttack"] = Defaults.DefaultTimeToAttack;
                if (!this.States[stateName].Parameters.ContainsKey("Stealable"))
                    this.States[stateName].Parameters["Stealable"] = false;
                this.states[stateName].Parameters["Size"] = mySize;
                this.states[stateName].Parameters["IsWeapon"] = isWeapon;
                this.states[stateName].Parameters["DefaultClassification"] = DefaultClassification;
                this.states[stateName].Parameters["ClassificationDisplayRules"] = ClassificationDisplayRules;
                this.states[stateName].Parameters["RemoveOnDestruction"] = myRemoveOnDestruction;
                //4.1 new species parameters.
                this.states[stateName].Parameters["CanOwn"] = dMCanOwnMe;
                //this.states[stateName].Parameters["SubplatformLimit"] = subplatformLimit;
                //this.states[stateName].Parameters["Subplatforms"] = possibleSubplatforms;
                this.states[stateName].Parameters["LaunchedByOwner"] = launchedByOwner;

            }
            //this.states["FullyFunctional"].Parameters["Size"] = mySize;
            //            this.states["FullyFunctional"].Parameters["MaximumSpeed"] = myMaximumSpeed;
            //this.states["FullyFunctional"].Parameters["IsWeapon"] = isWeapon;
            //this.states["FullyFunctional"].Parameters["RemoveOnDestruction"] = myRemoveOnDestruction;
            //      if (!this.states["FullyFunctional"].Parameters.ContainsKey("TimeToRemove")) this.states["FullyFunctional"].Parameters["TimeToRemove"] =  Defaults.DefaultTimeToRemove;
            //if (!this.states["FullyFunctional"].Parameters.ContainsKey("TimeToAttack")) this.states["FullyFunctional"].Parameters["TimeToAttack"] = Defaults.DefaultTimeToAttack;
            //if (!this.States["FullyFunctional"].Parameters.ContainsKey("Stealable"))
            //    this.States["FullyFunctional"].Parameters["Stealable"] = false;
        }
    }

}
