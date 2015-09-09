using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
namespace Aptima.Asim.DDD.CommonComponents.DataTypeTools
{
    interface DataValueInterface
    {
        string ToXML();
        void FromXML(string xml);
        string ToString();
    }
    /// <summary>
    /// The base class for all DataValue classes
    /// </summary>
    public class DataValue : DataValueInterface
    {
        public string xmlStartTag = null;
        public string xmlEndTag = null;
        public string dataType = null;
        public string ToXML()
        {
            return "";
        }
        public void FromXML(string xml)
        {

        }
        public string getType()
        {
            return this.dataType;
        }
        override public string ToString()
        {
            return "";
        }
    }

    /// <summary>
    /// Used to represent values of StringType.
    /// </summary>
    public class StringValue : DataValue
    {
        static Regex regex = new Regex(@"^<StringType>(.*)</StringType>$",RegexOptions.Singleline);
        //static Regex newlineregex = new Regex(@"");
        public string value;
        public StringValue()
        {
            dataType = "StringType";
            value = String.Empty;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }
        new public string ToXML()
        {
            value = value.Replace("\n", "\\n");
            value = value.Replace("\r", "");
            return String.Format("{0}{1}{2}", xmlStartTag, value, xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                value = g.Value;
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}",dataType,xml));
            }
        }
        new public string ToString()
        {
            return String.Format("\" {0} \"", value);
        }
    }
    /// <summary>
    /// Used to represent values of DoubleType.
    /// </summary>
    public class DoubleValue : DataValue
    {

        static Regex regex = new Regex(@"^<DoubleType>(.*)</DoubleType>$");
        public double value;
        public DoubleValue()
        {

            dataType = "DoubleType";
            value = 0;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }
        new public string ToXML()
        {
            return String.Format("{0}{1}{2}", xmlStartTag, value.ToString(), xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                double i;
                if (double.TryParse(g.Value, out i))
                {
                    value = i;
                }
                else
                {
                    value = 0;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }
        new public string ToString()
        {
            return value.ToString();
        }
    }
    /// <summary>
    /// Used to represent values of IntegerType.
    /// </summary>
    public class IntegerValue : DataValue
    {
        static Regex regex = new Regex(@"^<IntegerType>(.*)</IntegerType>$");
        public int value;
        public IntegerValue()
        {
            dataType = "IntegerType";
            value = 0;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }
        new public string ToXML()
        {
            return String.Format("{0}{1}{2}", xmlStartTag, value.ToString(), xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                int i;
                if (Int32.TryParse(g.Value, out i))
                {
                    value = i;
                }
                else
                {
                    value = 0;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }
        new public string ToString()
        {
            return value.ToString();
        }
    }

    /// <summary>
    /// Used to represent values of BooleanType.
    /// </summary>
    public class BooleanValue : DataValue
    {
        static Regex regex = new Regex(@"^<BooleanType>(.*)</BooleanType>$");
        public bool value;
        public BooleanValue()
        {
            dataType = "BooleanType";
            value = false;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }
        new public string ToXML()
        {
            return String.Format("{0}{1}{2}", xmlStartTag, value.ToString(), xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                bool i;
                if (Boolean.TryParse(g.Value, out i))
                {
                    value = i;
                }
                else
                {
                    value = false;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }
        new public string ToString()
        {
            return value.ToString();
        }
    }

    /// <summary>
    /// Used to represent values of LocationType.
    /// </summary>
    public class LocationValue : DataValue
    {
        static Regex regex = new Regex(@"^<LocationType><X>(.*)</X><Y>(.*)</Y><Z>(.*)</Z></LocationType>$");
        public double X, Y, Z;
        public bool exists;
        public LocationValue()
        {
            dataType = "LocationType";
            X = 0;
            Y = 0;
            Z = 0;
            exists = false;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            string s;
            if (exists)
            {
                s = String.Format("{0}<X>{1}</X><Y>{2}</Y><Z>{3}</Z>{4}", xmlStartTag, X.ToString(), Y.ToString(), Z.ToString(), xmlEndTag);
            }
            else
            {
                s = String.Format("{0}<X>NULL</X><Y>NULL</Y><Z>NULL</Z>{1}", xmlStartTag, xmlEndTag);
            }
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                double i;

                if (double.TryParse(m.Groups[1].Value, out i))
                {
                    X = i;
                }
                else
                {
                    X = 0;
                    Y = 0;
                    Z = 0;
                    exists = false;
                    return;
                }
                if (double.TryParse(m.Groups[2].Value, out i))
                {
                    Y = i;
                }
                else
                {
                    X = 0;
                    Y = 0;
                    Z = 0;
                    exists = false;
                    return;
                }
                if (double.TryParse(m.Groups[3].Value, out i))
                {
                    Z = i;
                }
                else
                {
                    X = 0;
                    Y = 0;
                    Z = 0;
                    exists = false;
                    return;
                }
                exists = true;
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }
        new public string ToString()
        {
            if (exists)
            {
                return String.Format("({0},{1},{2})", X.ToString(), Y.ToString(), Z.ToString());
            }
            else
            {
                return "(NULL,NULL,NULL)";
            }
        }
    }
    /// <summary>
    /// Used to represent values of VelocityType.
    /// </summary>
    public class VelocityValue : DataValue
    {
        static Regex regex = new Regex(@"^<VelocityType><VX>(.*)</VX><VY>(.*)</VY><VZ>(.*)</VZ></VelocityType>$");
        public double VX, VY, VZ;
        public VelocityValue()
        {
            dataType = "VelocityType";
            VX = 0;
            VY = 0;
            VZ = 0;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            return String.Format("{0}<VX>{1}</VX><VY>{2}</VY><VZ>{3}</VZ>{4}",
                xmlStartTag, VX.ToString(), VY.ToString(), VZ.ToString(), xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {

                double i;

                if (double.TryParse(m.Groups[1].Value, out i))
                {
                    VX = i;
                }
                else
                {
                    VX = 0;
                }
                if (double.TryParse(m.Groups[2].Value, out i))
                {
                    VY = i;
                }
                else
                {
                    VY = 0;
                }
                if (double.TryParse(m.Groups[3].Value, out i))
                {
                    VZ = i;
                }
                else
                {
                    VZ = 0;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }
        new public string ToString()
        {
            return String.Format("({0},{1},{2})", VX.ToString(), VY.ToString(), VZ.ToString());
        }
    }

    /// <summary>
    /// Used to represent values of AttributeCollectionType.  
    /// This data type is essentially a dictionary of other DataValue objects
    /// with a string key.
    /// Note:  Cannot contain another AttributeCollectionValue.
    /// </summary>
    public class AttributeCollectionValue : DataValue
    {
        private static string innerAttNameStart = "<Attribute><Name>";
        private static string innerAttNameMiddle = "</Name><Value>";
        private static string innerAttNameEnd = "</Value></Attribute>";
        static Regex regex = new Regex(@"^<AttributeCollectionType>(<Attribute>.*</Attribute>)*</AttributeCollectionType>$");
        static Regex attregex = new Regex(@"<Attribute><Name>(.*?)</Name><Value>(.*?)</Value></Attribute>");
        public Dictionary<string, DataValue> attributes;
        public AttributeCollectionValue()
        {
            dataType = "AttributeCollectionType";
            attributes = new Dictionary<string, DataValue>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

        }
        public DataValue this[string attName]
        {
            get
            {
                return attributes[attName];
            }
            set
            {
                attributes[attName] = value;
            }
        }
        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (string key in attributes.Keys)
            {
                sb.Append(innerAttNameStart);
                sb.Append(key);
                sb.Append(innerAttNameMiddle);
                sb.Append(DataValueFactory.XMLSerialize(attributes[key]));
                sb.Append(innerAttNameEnd);
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    string val = m2.Groups[2].Value;
                    attributes[name] = DataValueFactory.XMLDeserialize(val);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            StringBuilder sb = new StringBuilder("(");

            foreach (string str in attributes.Keys)
            {
                sb.Append(String.Format("{0},", str));
            }

            sb.Append(")");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Used to represent values of CustomAttributesType.  
    /// This data type is essentially a dictionary of other DataValue objects
    /// with a string key used for storing and sending user defined attributes to the client.
    /// Note:  Cannot contain another AttributeCollectionValue.
    /// </summary>
    public class CustomAttributesValue : DataValue
    {
        private static string innerAttNameStart = "<CustomAttribute><Name>";
        private static string innerAttNameMiddle = "</Name><Value>";
        private static string innerAttNameEnd = "</Value></CustomAttribute>";
        static Regex regex = new Regex(@"^<CustomAttributesType>(<CustomAttribute>.*</CustomAttribute>)*</CustomAttributesType>$");
        static Regex attregex = new Regex(@"<CustomAttribute><Name>(.*?)</Name><Value>(.*?)</Value></CustomAttribute>");
        public Dictionary<string, DataValue> attributes;
        public CustomAttributesValue()
        {
            dataType = "CustomAttributesType";
            attributes = new Dictionary<string, DataValue>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

        }
        public DataValue this[string attName]
        {
            get
            {
                return attributes[attName];
            }
            set
            {
                attributes[attName] = value;
            }
        }
        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (string key in attributes.Keys)
            {
                sb.Append(innerAttNameStart);
                sb.Append(key);
                sb.Append(innerAttNameMiddle);
                sb.Append(DataValueFactory.XMLSerialize(attributes[key]));
                sb.Append(innerAttNameEnd);
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    string val = m2.Groups[2].Value;
                    attributes[name] = DataValueFactory.XMLDeserialize(val);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            StringBuilder sb = new StringBuilder("(");

            foreach (string str in attributes.Keys)
            {
                sb.Append(String.Format("{0}={1},", str,DataValueFactory.XMLSerialize(attributes[str])));
            }

            sb.Append(")");
            return sb.ToString();
        }
    }



    /// <summary>
    /// Used to represent values of StateTableType.  This is essentially an AttributeCollectionValue.
    /// This collection can contain AttributeCollectionValue objects.
    /// </summary>
    public class StateTableValue : DataValue
    {
        private static string innerStateNameStart = "<StateName><Name>";
        private static string innerStateNameMiddle = "</Name><Value>";
        private static string innerStateNameEnd = "</Value></StateName>";

        static Regex regex = new Regex(@"^<StateTableType>(<StateName>.*</StateName>)*</StateTableType>$");
        static Regex attregex = new Regex(@"<StateName><Name>(.*?)</Name><Value>(.*?)</Value></StateName>");
        public Dictionary<string, DataValue> states;
        public StateTableValue()
        {
            dataType = "StateTableType";
            states = new Dictionary<string, DataValue>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

        }
        public DataValue this[string stateName]
        {
            get
            {
                return states[stateName];
            }
            set
            {
                states[stateName] = value;
            }
        }
        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);

            foreach (string key in states.Keys)
            {
                sb.Append(innerStateNameStart);
                sb.Append(key);
                sb.Append(innerStateNameMiddle);
                sb.Append(DataValueFactory.XMLSerialize(states[key]));
                sb.Append(innerStateNameEnd);
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    string val = m2.Groups[2].Value;
                    states[name] = DataValueFactory.XMLDeserialize(val);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            string r = string.Empty;

            foreach (string str in states.Keys)
            {
                r = String.Format("{0}{1},", r, str);
            }

            return String.Format("{0}{1}{2}", "(", r, ")");
        }
    }
    
    /// <summary>
    /// Used to represent values of CapablityType.
    /// This is used to store and object's capability info.
    /// </summary>
    public class CapabilityValue : DataValue
    {
        public class Effect
        {
            public string name;
            public double range;
            public int intensity;
            public double probability;

            public Effect(string name, double range, int intensity, double probability)
            {
                this.name = name;
                this.range = range;
                this.intensity = intensity;
                this.probability = probability;
            }
        }
        static Regex regex = new Regex(@"^<CapabilityType>(<Effect>.*</Effect>)*</CapabilityType>$");
        static Regex effectregex = new Regex(@"<Effect><Name>(.*?)</Name><Range>(.*?)</Range><Intensity>(.*?)</Intensity><Probability>(.*?)</Probability></Effect>");
        public List<Effect> effects;

        private static string innerEffectNameStart = "<Effect><Name>";
        private static string innerEffectNameSecond = "</Name><Range>";
        private static string innerEffectNameThird = "</Range><Intensity>";
        private static string innerEffectNameFourth = "</Intensity><Probability>";
        private static string innerEffectNameEnd = "</Probability></Effect>";

        public CapabilityValue()
        {
            dataType = "CapabilityType";
            effects = new List<Effect>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        public List<string> GetCapabilityNames()
        {
            List<string> capabilitiesNames = new List<string>();

            foreach (Effect s in effects)
            {
                capabilitiesNames.Add(s.name);
            }
            return capabilitiesNames;
        }

        public List<CapabilityValue.Effect> GetOrderedEffectsByCapability(String capabilityName)
        {
            LinkedList<CapabilityValue.Effect> newEffects = new LinkedList<Effect>();
            foreach (CapabilityValue.Effect ef in this.effects)
            {
                if (ef.name == capabilityName)
                {
                    if (newEffects.Count == 0)
                    {
                        newEffects.AddFirst(ef);
                    }
                    else
                    {
                        bool isInserted = false;
                        LinkedListNode<CapabilityValue.Effect> current = newEffects.First;
                        while (current != null && !isInserted)
                        {
                            if (ef.range < current.Value.range)
                            {
                                newEffects.AddBefore(current, ef);
                                isInserted = true;
                            }
                            else if(current.Next != null)
                            {
                                if (ef.range > current.Value.range && ef.range < current.Next.Value.range)
                                {
                                    newEffects.AddAfter(current, ef);
                                    isInserted = true;
                                }
                            }
                            current = current.Next;
                        }
                        if(!isInserted)
                        {
                            newEffects.AddLast(ef);
                        }
                    }
                }
            }

            return new List<Effect>(newEffects);
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);

            foreach (Effect e in effects)
            {
                sb.Append(innerEffectNameStart);
                sb.Append(e.name);
                sb.Append(innerEffectNameSecond);
                sb.Append(e.range.ToString());
                sb.Append(innerEffectNameThird);
                sb.Append(e.intensity.ToString());
                sb.Append(innerEffectNameFourth);
                sb.Append(e.probability.ToString());
                sb.Append(innerEffectNameEnd);
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in effectregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    double range = Double.Parse(m2.Groups[2].Value);
                    int intensity = Int32.Parse(m2.Groups[3].Value);
                    double probability = Double.Parse(m2.Groups[4].Value);
                    Effect e = new Effect(name, range, intensity, probability);
                    this.effects.Add(e);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            string r = string.Empty;
            List<String> names = new List<string>();
            foreach (Effect e in effects)
            {
                if (!names.Contains(e.name))
                {
                    names.Add(e.name);
                    r = String.Format("{0}{1},", r, e.name);
                }
            }

            return String.Format("{0}{1}{2}", "(", r, ")");
        }
    }
    /// <summary>
    /// Used to represent values of VulnerabilityType.
    /// This is used to store and object's vulnerability info.
    /// </summary>
    public class VulnerabilityValue : DataValue
    {
        public class EffectApplied
        {
            public int effect;
            public double rangeApplied;

            public EffectApplied(int effect, double rangeApplied)
            {
                this.effect = effect;
                this.rangeApplied = rangeApplied;
            }


        }

        public class TransitionCondition
        {
            public string capability;
            public int effect;
            public double range;
            public double probability;


            public List<EffectApplied> effectsApplied;

            public TransitionCondition(string capability, int effect, double range, double probability)
            {
                this.capability = capability;
                this.effect = effect;
                this.range = range;
                this.probability = probability;
                this.effectsApplied = new List<EffectApplied>();
            }
            public void ClearAppliedEffect()
            {
                effectsApplied.Clear();
            }

        }

        public class Transition
        {
            public List<TransitionCondition> conditions;
            public string state;

            public Transition(string state)
            {
                this.state = state;
                conditions = new List<TransitionCondition>();
            }
            public void ApplyEffect(string capName, int effect, double appliedRange, ref System.Random random)
            {
                foreach (TransitionCondition tc in conditions)
                {
                    if (tc.capability == capName)
                    {
                        int r = random.Next(0, 100);
                        if (r <= ((int)(tc.probability * 100)))
                        {
                            tc.effectsApplied.Add(new EffectApplied(effect, appliedRange));
                        }
                    }
                }
            }
            public bool RemoveSingleEffect(string capabilityName, int effect)
            {
                foreach (TransitionCondition tc in conditions)
                {
                    if (tc.capability == capabilityName)
                    {
                        for (int x = 0; x < tc.effectsApplied.Count; x++)
                        {
                            if (tc.effectsApplied[x].effect == effect)
                            {
                                tc.effectsApplied.RemoveAt(x);
                                return true;//this assumes only a single transition condition per capability name...
                            }
                        }
                    }
                }


                //if we get here should we apply a negative effect?  how will we know that th effect is correct?
                return false;
            }
            public void ClearAppliedEffects()
            {
                foreach (TransitionCondition tc in conditions)
                {
                    tc.ClearAppliedEffect();
                }
            }
            public List<string> GetAppliedCapabilities()
            {
                List<string> r = new List<string>();
                foreach (TransitionCondition tc in conditions)
                {
                    if (tc.effectsApplied.Count > 0)
                    {
                        r.Add(tc.capability);
                    }
                    
                }
                return r;
            }
            public bool ConditionsMet()
            {
                bool r = true;
                foreach (TransitionCondition tc in conditions)
                {
                    double effectApplied = 0;
                    foreach (EffectApplied ea in tc.effectsApplied)
                    {
                        if (tc.range <= 0 || ea.rangeApplied <= tc.range)
                        {
                            effectApplied += ea.effect;
                        }
                    }

                    if (effectApplied < tc.effect)
                    {
                        r = false;
                        break;
                    }
                }
                return r;
            }
        }


        static Regex regex = new Regex(@"^<VulnerabilityType>(<Transition>.*</Transition>)*</VulnerabilityType>$");
        static Regex transregex = new Regex(@"<Transition><State>(.*?)</State><Conditions>(.*?)</Conditions></Transition>");
        static Regex condregex = new Regex(@"<Condition><Capability>(.*?)</Capability><Effect>(.*?)</Effect><Range>(.*?)</Range><Probability>(.*?)</Probability></Condition>");
        public List<Transition> transitions;

        public void ApplyEffect(string capName, int effect, double appliedRange, ref System.Random random)
        {
            foreach (Transition t in transitions)
            {
                t.ApplyEffect(capName, effect, appliedRange,ref random);
            }
        }

        public VulnerabilityValue()
        {
            dataType = "VulnerabilityType";
            transitions = new List<Transition>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

        }

        private static string innerTransState = "<Transition><State>";
        private static string innerStateConds = "</State><Conditions>";
        private static string innerCondCap = "<Condition><Capability>";
        private static string innerCapEff = "</Capability><Effect>";
        private static string innerEffRange = "</Effect><Range>";
        private static string innerRangeProb = "</Range><Probability>";
        private static string innerProbCond = "</Probability></Condition>";
        private static string innerCondTrans = "</Conditions></Transition>";

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);

            foreach (Transition t in transitions)
            {
                sb.Append(innerTransState);
                sb.Append(t.state);
                sb.Append(innerStateConds);

                foreach (TransitionCondition c in t.conditions)
                {
                    sb.Append(innerCondCap);
                    sb.Append(c.capability);
                    sb.Append(innerCapEff);
                    sb.Append(c.effect.ToString());
                    sb.Append(innerEffRange);
                    sb.Append(c.range.ToString());
                    sb.Append(innerRangeProb);
                    sb.Append(c.probability.ToString());
                    sb.Append(innerProbCond);
                }
                sb.Append(innerCondTrans);
            }

            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            Transition t = null;
            int effect;
            string capability;
            double range;
            double probability;
            if (m.Success)
            {
                string s = m.Groups[1].Value;

                foreach (Match m2 in transregex.Matches(s))
                {
                    t = new Transition(m2.Groups[1].Value);

                    foreach (Match m3 in condregex.Matches(m2.Groups[2].Value))
                    {
                            capability = m3.Groups[1].Value;
                            effect = Int32.Parse(m3.Groups[2].Value);
                            //range = Int32.Parse(m3.Groups[3].Value);
                            range = Double.Parse(m3.Groups[3].Value);
                            probability = Double.Parse(m3.Groups[4].Value);
                            t.conditions.Add(new TransitionCondition(capability, effect, range, probability));
 
                    }



                    transitions.Add(t);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            string r = string.Empty;
            List<String> names = new List<string>();
            foreach (Transition t in transitions)
            {
                foreach (TransitionCondition tc in t.conditions)
                {
                    if (!names.Contains(tc.capability))
                    {
                        names.Add(tc.capability);
                        r = String.Format("{0}{1},", r, tc.capability);
                    }
                }
            }
            return String.Format("{0}{1}{2}", "(", r, ")");
        }
    }
    /// <summary>
    /// Used to represent values of StringListType.
    /// A list of strings.
    /// </summary>
    public class StringListValue : DataValue
    {
        static Regex regex = new Regex(@"^<StringListType>(.*)</StringListType>$");
        static Regex attregex = new Regex(@"<Value>(.*?)</Value>");
        public List<string> strings;
        public StringListValue()
        {
            dataType = "StringListType";
            strings = new List<string>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (string str in strings)
            {
                sb.Append(String.Format("<Value>{0}</Value>", str));
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    strings.Add(m2.Groups[1].Value);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            string r = string.Empty;

            foreach (string str in strings)
            {
                r = String.Format("{0}{1},", r, str);
            }

            return String.Format("({0})", r);
        }
    }
    public class PolygonValue : DataValue
    {
        public class PolygonPoint
        {
            public double X;
            public double Y;
            public PolygonPoint()
            {
                X = 0;
                Y = 0;
            }
            public PolygonPoint(double x, double y)
            {
                X = x;
                Y = y;
            }
            new public string ToString()
            {
                return String.Format("({0},{1})", X.ToString(), Y.ToString());
            }
            public string ToXML()
            {
                return String.Format("<X>{0}</X><Y>{1}</Y>", X.ToString(), Y.ToString());
            }
        }
        static Regex regex = new Regex(@"^<PolygonType>(.*)</PolygonType>$");
        static Regex pointregex = new Regex(@"<Point><X>(.*?)</X><Y>(.*?)</Y></Point>");
        public List<PolygonPoint> points;
        public PolygonValue()
        {
            dataType = "PolygonType";
            points = new List<PolygonPoint>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (PolygonPoint p in points)
            {
                sb.Append(String.Format("<Point>{0}</Point>", p.ToXML()));
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in pointregex.Matches(s))
                {
                    double x = Double.Parse(m2.Groups[1].Value);
                    double y = Double.Parse(m2.Groups[2].Value);
                    points.Add(new PolygonPoint(x, y));
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            string r = string.Empty;

            foreach (PolygonPoint p in points)
            {
                r = String.Format("{0}{1},", r, p.ToString());
            }

            return String.Format("[{0}]", r);
        }
    }
    /// <summary>
    /// Used to represent values of EmmitterType.
    /// Used to keep track of an objects emmitters.
    /// </summary>
    public class EmitterValue : DataValue
    {
        static Regex regex = new Regex(@"^<EmitterType>(.*)</EmitterType>$");
        static Regex emitregex = new Regex(@"<Emitter><AttributeName>(.*?)</AttributeName><IsEngram>(.*?)</IsEngram><Levels>(.*?)</Levels></Emitter>");
        static Regex pairregex = new Regex(@"<Level>(.*?)</Level><Variance>(.*?)</Variance>");

        private static string innerEmitAtt = "<Emitter><AttributeName>";
        private static string innerAttEngram = "</AttributeName><IsEngram>";
        private static string innerEngramLevel = "</IsEngram><Levels>";
        private static string innerLevel = "<Level>";
        private static string innerLevVari = "</Level><Variance>";
        private static string innerVariance = "</Variance>";
        private static string innerLevEmit = "</Levels></Emitter>";

        public Dictionary<string, Dictionary<string, double>> emitters;
        public Dictionary<string, Boolean> attIsEngram;
        public EmitterValue()
        {
            dataType = "EmitterType";
            emitters = new Dictionary<string, Dictionary<string, double>>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
            attIsEngram = new Dictionary<string, bool>();
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);

            foreach (string attName in emitters.Keys)
            {
                sb.Append(innerEmitAtt);
                sb.Append(attName);
                sb.Append(innerAttEngram);
                if (attIsEngram.ContainsKey(attName))
                {
                    sb.Append(attIsEngram[attName].ToString());
                }
                else
                {
                    sb.Append(false.ToString());
                }
                sb.Append(innerEngramLevel);
                foreach (KeyValuePair<string, double> kvp in emitters[attName])
                {
                    sb.Append(innerLevel);
                    sb.Append(kvp.Key);
                    sb.Append(innerLevVari);
                    sb.Append(kvp.Value.ToString());
                    sb.Append(innerVariance);
                }
                sb.Append(innerLevEmit);
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }

        new public void FromXML(string xml)
        {
            Dictionary<string, double> emitterDictionary;
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in emitregex.Matches(s))
                {
                    emitterDictionary = new Dictionary<string, double>();
                    string attributeName = m2.Groups[1].Value;
                    bool isEngram = Boolean.Parse(m2.Groups[2].Value);
                    attIsEngram[attributeName] = isEngram;
                    foreach (Match m3 in pairregex.Matches(m2.Groups[3].Value))
                    {
                        string level = m3.Groups[1].Value;
                        string strVariance = m3.Groups[2].Value;
                        double variance = Convert.ToDouble(strVariance);
                        emitterDictionary.Add(level, variance);
                    }
                    emitters.Add(attributeName, emitterDictionary);
                    emitterDictionary = null;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }

        new public string ToString()
        {
            string r = string.Empty;
            foreach (string s in emitters.Keys)
            {
                r = String.Format("{0}{1},", r, s);
            }

            return String.Format("({0})", r);
        }

    }

    /// <summary>
    /// Used to represent values of ConeType.
    /// This is used as part of the SensorValue type.
    /// </summary>
    public class ConeValue : DataValue
    {
        static Regex regex = new Regex(@"^<ConeType><Direction><LocationType>(.*)</LocationType></Direction><Level>(.*)</Level><Extent>(.*)</Extent><Spread>(.*)</Spread></ConeType>$");
        static Regex locationregex = new Regex(@"<X>(.*)</X><Y>(.*)</Y><Z>(.*)</Z>");

        public LocationValue direction;
        public string level;
        public double extent;
        public double spread;

        public ConeValue()
        {
            dataType = "ConeType";
            direction = new LocationValue();
            direction.exists = true;
            level = string.Empty;
            extent = 0.0;
            spread = 0.0;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            return String.Format("{0}<Direction>{1}</Direction><Level>{2}</Level><Extent>{3}</Extent><Spread>{4}</Spread>{5}",
                xmlStartTag, direction.ToXML(), level, extent.ToString(), spread.ToString(), xmlEndTag); ;
        }

        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                LocationValue lv = new LocationValue();
                lv.FromXML("<LocationType>" + m.Groups[1].Value + "</LocationType>");
                direction = lv;
                level = m.Groups[2].Value;
                extent = Convert.ToDouble(m.Groups[3].Value);
                spread = Convert.ToDouble(m.Groups[4].Value);
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }

        new public string ToString()
        {
            return String.Format("({0} up to {1})", level, extent.ToString());
        }

    }
    /// <summary>
    /// Used to represent values of SensorType.
    /// This is used to keep track of an individual sensor.
    /// </summary>
    public class SensorValue : DataValue
    {
        static Regex regex = new Regex(@"^<Sensor><SensorType><SensorName>(.*)</SensorName><MaxRange>(.*)</MaxRange><AttributesSensed>(<AttributeSensed>.*</AttributeSensed>)</AttributesSensed></SensorType></Sensor>$");
        static Regex attsregex = new Regex(@"<AttributeSensed><Name>(.*?)</Name><IsEngram>(.*?)</IsEngram>(<Cones>(.*?)</Cones>?)</AttributeSensed>");
        static Regex attregex = new Regex(@"<ConeType>(.*?)</ConeType>");
        public string sensorName;
        public double maxRange;
        public Dictionary<string, List<ConeValue>> ranges;
        public Dictionary<string, Boolean> attIsEngram;

        private static string innerAttSenName = "<AttributeSensed><Name>";
        private static string innerNameEngram = "</Name><IsEngram>";
        private static string innerEngramCone = "</IsEngram><Cones>";
        private static string innerConeAttSens = "</Cones></AttributeSensed>";
        private static string innerAttSensEnd = "</AttributesSensed>";

        public SensorValue()
        {
            dataType = "SensorType";
            ranges = new Dictionary<string, List<ConeValue>>();
            sensorName = string.Empty;
            maxRange = 0.0;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
            attIsEngram = new Dictionary<string, bool>();
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(String.Format("{0}<SensorName>{1}</SensorName><MaxRange>{2}</MaxRange><AttributesSensed>",
                                      xmlStartTag, sensorName, maxRange.ToString()));
            foreach (string attName in ranges.Keys)
            {
                sb.Append(innerAttSenName);
                sb.Append(attName);
                sb.Append(innerNameEngram);
                if (attIsEngram.ContainsKey(attName))
                {
                    sb.Append(attIsEngram[attName].ToString());
                }
                else
                {
                    sb.Append(false.ToString());
                }
                sb.Append(innerEngramCone);
                foreach (ConeValue cv in ranges[attName])
                {
                    sb.Append(cv.ToXML());
                }
                sb.Append(innerConeAttSens);
            }
            sb.Append(innerAttSensEnd);
            sb.Append(xmlEndTag);
            return sb.ToString();
        }

        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            List<ConeValue> attributesCones;
            if (m.Success)
            {
                ranges = new Dictionary<string, List<ConeValue>>();
                sensorName = m.Groups[1].Value;
                maxRange = Convert.ToDouble(m.Groups[2].Value);
                string attributeLists = m.Groups[3].Value;

                foreach (Match m3 in attsregex.Matches(attributeLists))
                {
                    attributesCones = new List<ConeValue>();
                    string attname = m3.Groups[1].Value;
                    bool isEngram = Boolean.Parse(m3.Groups[2].Value);
                    attIsEngram[attname] = isEngram;
                    foreach (Match m4 in attregex.Matches(m3.Groups[3].Value))
                    {
                        ConeValue cv = new ConeValue();
                        cv.FromXML(m4.Value);
                        attributesCones.Add(cv);
                    }

                    ranges.Add(attname, attributesCones);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }
        }

        new public string ToString()
        {
            return String.Format("({0})", sensorName);
        }

    }
    /// <summary>
    /// Used to represent values of SensorArrayType.
    /// This is composed of SensorValue objects is used to keep track of the overall sensor
    /// system of an object.
    /// </summary>
    public class SensorArrayValue : DataValue
    {
        static Regex regex = new Regex(@"^<SensorArrayType>(<Sensor>.*</Sensor>)*</SensorArrayType>$");
        static Regex sensorsregex = new Regex(@"<Sensor><SensorType>(.*?)</SensorType></Sensor>");

        public List<SensorValue> sensors;

        public SensorArrayValue()
        {
            dataType = "SensorArrayType";
            sensors = new List<SensorValue>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);

            foreach (SensorValue sv in sensors)
            {
                sb.Append(String.Format("<Sensor>{0}</Sensor>", sv.ToXML()));
            }

            sb.Append(xmlEndTag);
            return sb.ToString();
        }

        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);

            if (m.Success)
            {
                sensors = new List<SensorValue>();
                foreach (Match m2 in sensorsregex.Matches(m.Groups[1].Value))
                {
                    SensorValue sv = new SensorValue();
                    sv.FromXML(m2.Value);
                    sensors.Add(sv);
                    sv = null;
                }
            }
        }

        new public string ToString()
        {
            string r = string.Empty;

            foreach (SensorValue sv in sensors)
            {
                r = String.Format("{0}{1},", r, sv.ToString());
            }
            return String.Format("({0})", r);
        }

    }

    /// <summary>
    /// This data value class will only be used internally.
    /// </summary>
    /// <exclude/>
    public class DetectedAttributeValue : DataValue
    {
        //This data value class will only be used internally (no network sending) in the
        //ViewPro simulator to determine which attributes sensed have the most confidence.
// This data structed is being used in an overloaded manner. As input to the fuzz routines 
        //it contains a value and a std dev or a value and a probability
        // as output it contains a value and a confidence level
        public DataValue value;
        public double stdDev;
        public double confidence
        {
            get { return stdDev; }
            set { stdDev = value; }
        }
        public double probability
        {
            get { return stdDev; }
            set { stdDev = value; }
        }

        public DetectedAttributeValue()
        {
            dataType = "DetectedAttributeType";
            value = new DataValue();
            stdDev = 0.0;
        }
        public void SetValues(DataValue dv, double conf)
        {
            value = dv;
            stdDev = conf;
        }
        new public string ToString()
        {
            return String.Format("Detected: {0}, Confidence: {1}", DataValueFactory.ToString(value), stdDev);
        }
    }

    /// <summary>
    /// Used to represent values of RangeRing info, aggregated on the server side and sent to the client side in an AttributeCollectionValue.
    /// This is used to store and object's capability info.
    /// </summary>
    public class RangeRingDisplayValue : DataValue
    {

        static Regex regex = new Regex(@"^<RangeRingDisplayType><RangeName>(.*?)</RangeName><Type>(.*?)</Type><IsWeapon>(.*?)</IsWeapon><RR_Ranges>(<RR_Range>.*</RR_Range>)*</RR_Ranges></RangeRingDisplayType>$");
        static Regex rangeregex = new Regex(@"<RR_Range><Distance>(.*?)</Distance><Intensity>(.*?)</Intensity></RR_Range>");

        private static string outerFirst = "<RangeRingDisplayType><RangeName>";
        private static string outerSecond = "</RangeName><Type>";
        private static string outerThird = "</Type><IsWeapon>";
        private static string outerFourth = "</IsWeapon><RR_Ranges>";
        private static string innerRangeFirst = "<RR_Range><Distance>";
        private static string innerRangeSecond = "</Distance><Intensity>";
        private static string innerRangeThird = "</Intensity></RR_Range>";
        private static string outerFifth = "</RR_Ranges></RangeRingDisplayType>";

        public String type;
        public String name;
        public Boolean isWeapon;
        public Dictionary<int, int> rangeIntensities;

        public RangeRingDisplayValue()
        {
            dataType = "RangeRingDisplayType";
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

            type = string.Empty;
            name = string.Empty;
            isWeapon = false;
            rangeIntensities = new Dictionary<int, int>();//[Distance],[Value]
        }

        /// <summary>
        /// This function ASSUMES that the ranges are placed in ASCENDING order.
        /// </summary>
        /// <param name="range"></param>
        /// <returns>-1 for no intensity, otherwise an integer</returns>
        public int GetIntensityForRange(int range)
        {
            int intensity = -1;
            foreach (KeyValuePair<int, int> kvp in rangeIntensities)
            {
                if (kvp.Key >= range)
                {
                    return intensity; //returns the previous intensity which satisfied input_range > range.
                }
                intensity = kvp.Value;
            }

            return -1; //if the loop couldnt find one case where the range was >= the incoming range, then no ranges will succeed.
        }

        public void AddAndSortRanges(Dictionary<int, int> newRanges)
        {
            if (newRanges == null)
            {
                newRanges = new Dictionary<int, int>();
            }
            foreach (int range in newRanges.Keys)
            {
                if (!rangeIntensities.ContainsKey(range))
                {
                    rangeIntensities.Add(range, newRanges[range]);
                }
                else
                {
                    if (rangeIntensities[range] < newRanges[range])
                    {
                        rangeIntensities[range] = newRanges[range];
                    }
                }
            }
            //SORT
            List<int> orderedDistances = new List<int>(rangeIntensities.Keys);
            Dictionary<int, int> sortedDistances = new Dictionary<int, int>();
            orderedDistances.Sort();
            orderedDistances.Reverse(); //needed?
            foreach (int dist in orderedDistances)
            {
                sortedDistances.Add(dist, rangeIntensities[dist]);
            }

            this.rangeIntensities = sortedDistances;
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(outerFirst);
            
            sb.Append(name);
            sb.Append(outerSecond);
            sb.Append(type);
            sb.Append(outerThird);
            if (isWeapon)
            {
                sb.Append("True");
            }
            else
            {
                sb.Append("False");
            }
            sb.Append(outerFourth);
            //ranges
            foreach (KeyValuePair<int, int> kvp in rangeIntensities)
            {
                sb.Append(innerRangeFirst);
                sb.Append(kvp.Key);
                sb.Append(innerRangeSecond);
                sb.Append(kvp.Value);
                sb.Append(innerRangeThird);
            }           

            sb.Append(outerFifth);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string name = m.Groups[1].Value;
                string type = m.Groups[2].Value;
                string isWeapon = m.Groups[3].Value;
                string ranges = m.Groups[4].Value;
                Dictionary<int, int> tempDistances = new Dictionary<int, int>();
                foreach (Match m2 in rangeregex.Matches(ranges))
                {
                    int distance = Int32.Parse(m2.Groups[1].Value);
                    int intensity = Int32.Parse(m2.Groups[2].Value);
                    //will this handle sorting distances?
                    if (!tempDistances.ContainsKey(distance))
                    {
                        tempDistances.Add(distance, intensity);
                    }
                    else
                    {
                        if (tempDistances[distance] < intensity)
                        {
                            tempDistances[distance] = intensity;
                        }
                    }
                }
                List<int> orderedDistances = new List<int>(tempDistances.Keys);
                orderedDistances.Sort();
                orderedDistances.Reverse(); //needed?
                this.rangeIntensities = new Dictionary<int, int>();
                foreach (int dist in orderedDistances)
                {
                    rangeIntensities.Add(dist, tempDistances[dist]);
                }
                this.name = name;
                this.type = type;
                if (isWeapon.Trim().ToLower() == "true")
                {
                    this.isWeapon = true;
                }
                else
                {
                    this.isWeapon = false;
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            StringBuilder sb = new StringBuilder("(");

            sb.AppendFormat("Name: {0}; ", name);
            sb.AppendFormat("Type: {0}; ", type);
            sb.AppendFormat("Is Weapon: {0}; ", isWeapon.ToString());
            sb.Append("Ranges: [ ");

            //ranges
            foreach (KeyValuePair<int, int> kvp in rangeIntensities)
            {
                sb.AppendFormat("(Distance: {0}m;", kvp.Key);
                if (type.ToLower() == "capability")
                {
                    sb.AppendFormat("Intensity: {0};", kvp.Value);
                }
                sb.Append("), ");
            }
            sb.Append("])");

            return sb.ToString();
        }
    }

    /// <summary>
    /// Used to represent values of attack info, aggregated on the server side.
    /// </summary>
    public class AttackCollectionValue : DataValue
    {
        static Regex regex = new Regex(@"^<AttackCollectionType>(<AttackType>.*</AttackType>)*</AttackCollectionType>$");
        static Regex attackregex = new Regex(@"<AttackType><CapabilityName>(.*?)</CapabilityName><AttackStartTime>(.*?)</AttackStartTime><AttackTimeWindow>(.*?)</AttackTimeWindow><TargetObjectId>(.*?)</TargetObjectId><AttackingObjectId>(.*?)</AttackingObjectId><PercentageApplied>(.*?)</PercentageApplied><IsSelfDefense>(.*?)</IsSelfDefense></AttackType>");

        /// <summary>
        /// Represents a single attack between exactly two objects.
        /// </summary>
        public class AttackValue : DataValue
        {
            public string capabilityName;
            public int attackStartTime;
            public int attackTimeWindow;
            public string targetObjectId;
            public string attackingObjectId;
            public int percentageApplied;
            public bool isSelfdefense;

            public int appliedIntensity; //this is set in attack sim, and not stored in XML

            public AttackValue()
            { 
                attackStartTime = -1;
                attackTimeWindow = -1;
                targetObjectId = "";
                attackingObjectId = "";
                capabilityName = "";
                percentageApplied = 0;
                isSelfdefense = false;
                appliedIntensity = 0;
            }
            public AttackValue(int startTime, int timeWindow, string targetObject, string attackingObject, string capability, int percentage, bool selfDefense)
            {
                attackStartTime = startTime;
                attackTimeWindow = timeWindow;
                targetObjectId = targetObject;
                attackingObjectId = attackingObject;
                capabilityName = capability;
                percentageApplied = percentage;
                isSelfdefense = selfDefense;
                appliedIntensity = 0;
            }
        }
        private List<AttackValue> currentAttacks;
        private Dictionary<string,int> currentPercentages; //[CapabilityName]/[CurrentPercentagesApplied]
        private static int MaximumPercentage = 100;

        public AttackCollectionValue()
        {
            dataType = "AttackCollectionType";
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);

            currentAttacks = new List<AttackValue>();
            currentPercentages = new Dictionary<string, int>();
        }
        /// <summary>
        /// Adds an attack to this collection.  
        /// </summary>
        /// <param name="attack">The attack object to add to this collection.</param>
        /// <param name="errorMessage">If this has anything besides String.Empty upon return, then the attack was not added in the FULL
        /// capacity of the original request.  If this function returned TRUE, then it was added partially.  If it was returned FALSE,
        /// then this attack was not added at all.</param>
        /// <returns></returns>
        public bool AddAttack(AttackValue attack, out String errorMessage)
        {
            bool returnValue = true;
            errorMessage = String.Empty;
            int possiblePercentageToApply = MaximumPercentage;

            foreach (AttackValue av in currentAttacks)
            {
                if (av.capabilityName != attack.capabilityName)
                    continue;

                possiblePercentageToApply -= av.percentageApplied;
            }

            if (possiblePercentageToApply < attack.percentageApplied)
            {
                if (possiblePercentageToApply > 0)
                {
                    errorMessage = "Applying " + possiblePercentageToApply.ToString() + "% instead of " + attack.percentageApplied.ToString() + "%, as that's all available";
                }
                else
                {// percentage applied is 0 or less 
                    errorMessage = "There is no more available percentage to apply for this capability.  Try again later.";
                    returnValue = false;
                    return returnValue;
                }
            }

            attack.percentageApplied = Math.Min(attack.percentageApplied, possiblePercentageToApply);

            currentAttacks.Add(attack);

            if (!currentPercentages.ContainsKey(attack.capabilityName))
            {
                currentPercentages.Add(attack.capabilityName, 0);
            }

            currentPercentages[attack.capabilityName] += attack.percentageApplied;

            return returnValue;
        }
        public bool RemoveAttack(AttackValue attack)
        {
            bool returnValue = true;
            int indexToRemove = -1;
            int counter = 0;
            foreach (AttackValue av in currentAttacks)
            {
                if (av == attack)
                {
                    indexToRemove = counter;
                    break;
                }
                counter++;
            }
            if (indexToRemove >= 0)
            {
                if (currentPercentages.ContainsKey(attack.capabilityName))
                {
                    currentPercentages[attack.capabilityName] -= attack.percentageApplied;
                }

                currentAttacks.RemoveAt(indexToRemove);
            }
            else
            {
                returnValue = false;
            }
            return returnValue;
        }
        public bool RemoveAttack(string capabilityName, string targetObjectId, string attackingObjectId, int attackStartTime)
        {
            for (int x = 0; x < currentAttacks.Count; x++)
            {
                if (currentAttacks[x].capabilityName == capabilityName && currentAttacks[x].targetObjectId == targetObjectId && currentAttacks[x].attackingObjectId == attackingObjectId && currentAttacks[x].attackStartTime == attackStartTime)
                {
                    return this.RemoveAttack(currentAttacks[x]);
                }
            }

            return false;
        }
        public int GetCurrentPercentageApplied(String capabilityName)
        {
            if (!currentPercentages.ContainsKey(capabilityName))
                return 0;

            return currentPercentages[capabilityName];
        }
        public List<AttackValue> GetCurrentAttacks()
        {
            List<AttackValue> current = new List<AttackValue>(currentAttacks);
            return current;
        }
        public List<AttackValue> GetCurrentAttacksOnTarget(string targetId)
        {
            List<AttackValue> current = new List<AttackValue>();
            foreach (AttackValue av in currentAttacks)
            {
                if (av.targetObjectId == targetId)
                    current.Add(av);
            }
            return current;
        }
        public List<AttackValue> GetCurrentSelfDefenseAttacks()
        {
            List<AttackValue> current = new List<AttackValue>();
            foreach (AttackValue av in currentAttacks)
            {
                if (av.isSelfdefense)
                    current.Add(av);
            }
            return current;
        }

        new public string ToString()
        {
            StringBuilder sb = new StringBuilder("(");

            sb.Append("Attack Collection: ");
            sb.Append("Ranges: [ ");

            //ranges
            foreach (AttackValue av in currentAttacks)
            {
                sb.Append("(");

                sb.AppendFormat("Capability Name: {0};", av.capabilityName);
                sb.AppendFormat("Attack Started: {0};", av.attackStartTime);
                sb.AppendFormat("Attack Duration: {0};", av.attackTimeWindow);
                sb.AppendFormat("Target Object ID: {0};", av.targetObjectId);
                sb.AppendFormat("Attacker Object ID: {0};", av.attackingObjectId);
                sb.AppendFormat("Percentage Applied: {0}%;", av.percentageApplied);
                sb.AppendFormat("Is Self Defense?: {0};", av.isSelfdefense);

                sb.Append("), ");
            }
            sb.Append("])");

            return sb.ToString();
        }
        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder("<AttackCollectionType>");

            //attacks
            foreach (AttackValue av in currentAttacks)
            {
                sb.Append("<AttackType>");
                sb.AppendFormat("<CapabilityName>{0}</CapabilityName>", av.capabilityName);
                sb.AppendFormat("<AttackStartTime>{0}</AttackStartTime>", av.attackStartTime);
                sb.AppendFormat("<AttackTimeWindow>{0}</AttackTimeWindow>", av.attackTimeWindow);
                sb.AppendFormat("<TargetObjectId>{0}</TargetObjectId>", av.targetObjectId);
                sb.AppendFormat("<AttackingObjectId>{0}</AttackingObjectId>", av.attackingObjectId);
                sb.AppendFormat("<PercentageApplied>{0}</PercentageApplied>", av.percentageApplied);
                sb.AppendFormat("<IsSelfDefense>{0}</IsSelfDefense>", av.isSelfdefense);
                sb.Append("</AttackType>");
            }

            sb.Append("</AttackCollectionType>");
            return sb.ToString();
        }
        new public void FromXML(string xml)
        { 
            this.currentAttacks = new List<AttackValue>();
            this.currentPercentages = new Dictionary<string, int>();

            Match m = regex.Match(xml);
            if (m.Success)
            {
                
                string attacks = m.Groups[1].Value;

                foreach (Match m2 in attackregex.Matches(attacks))
                {
                    AttackValue av;
                    string capabilityName = String.Empty;
                    int attackStartTime = 0;
                    int attackTimeWindow = 0;
                    string targetObjectId = String.Empty;
                    string attackingObjectId = String.Empty;
                    int percentageApplied = 0;
                    bool isSelfdefense = false;
                    string errMsg = String.Empty;
                    try
                    {
                        capabilityName = m2.Groups[1].Value;
                        attackStartTime = Int32.Parse(m2.Groups[2].Value);
                        attackTimeWindow = Int32.Parse(m2.Groups[3].Value);
                        targetObjectId = m2.Groups[4].Value;
                        attackingObjectId = m2.Groups[5].Value;
                        percentageApplied = Int32.Parse(m2.Groups[6].Value);
                        isSelfdefense = Boolean.Parse(m2.Groups[7].Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in AttackCollectionValue.FromXML; " + ex.Message);
                    }
                    av = new AttackValue(attackStartTime, attackTimeWindow, targetObjectId, attackingObjectId, capabilityName, percentageApplied, isSelfdefense);
                    this.AddAttack(av, out errMsg);
                    if (errMsg != string.Empty)
                    {
                        Console.WriteLine("Error in AttackCollectionValue.AddAttack: " + errMsg);
                    }
                }
            }
        }
    }

    public class WrapperValue : DataValue
    { 
        static Regex regex = new Regex(@"^<WrapperType>(.*)</WrapperType>$",RegexOptions.Singleline);
        
        public DataValue value;
        public WrapperValue()
        {
            dataType = "WrapperType";
            value = null;
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }
        new public string ToXML()
        {
            string strValue = DataValueFactory.XMLSerialize(value);
            return String.Format("{0}{1}{2}", xmlStartTag, strValue, xmlEndTag);
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                value = DataValueFactory.XMLDeserialize(g.Value);
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}",dataType,xml));
            }
        }
        new public string ToString()
        {
            return String.Format("Wrapper on: {0} ", value.ToString());
        }
    }

    /// <summary>
    /// Used to represent values of StringListType.
    /// A list of strings.
    /// </summary>
    public class ClassificationDisplayRulesValue : DataValue
    {
        public class ClassificationDisplayRule
        {
            String state;

            public String State
            {
                get { return state; }
                set { state = value; }
            }

            String classification;

            public String Classification
            {
                get { return classification; }
                set { classification = value; }
            }
            String displayIcon;

            public String DisplayIcon
            {
                get { return displayIcon; }
                set { displayIcon = value; }
            }

            public ClassificationDisplayRule()
            {
                classification = String.Empty;
                displayIcon = String.Empty;
            }
        }
        static Regex regex = new Regex(@"^<ClassificationDisplayRulesType>(.*)</ClassificationDisplayRulesType>$");
        static Regex attregex = new Regex(@"<Rule><State>(.*?)</State><Classification>(.*?)</Classification><DisplayIcon>(.*?)</DisplayIcon></Rule>");

        public List<ClassificationDisplayRule> rules;
        public ClassificationDisplayRulesValue()
        {
            dataType = "ClassificationDisplayRulesType";
            rules = new List<ClassificationDisplayRule>();
            xmlStartTag = String.Format("<{0}>", dataType);
            xmlEndTag = String.Format("</{0}>", dataType);
        }

        new public string ToXML()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (ClassificationDisplayRule rule in rules)
            {
                sb.Append(String.Format("<Rule><State>{0}</State><Classification>{1}</Classification><DisplayIcon>{2}</DisplayIcon></Rule>", rule.State,rule.Classification, rule.DisplayIcon));
            }
            sb.Append(xmlEndTag);
            return sb.ToString();
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    ClassificationDisplayRule r = new ClassificationDisplayRule();
                    r.State = m2.Groups[1].Value;
                    r.Classification = m2.Groups[2].Value;
                    r.DisplayIcon = m2.Groups[3].Value;
                    rules.Add(r);
                }
            }
            else
            {
                throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

        }
        new public string ToString()
        {
            StringBuilder sb = new StringBuilder(xmlStartTag);
            foreach (ClassificationDisplayRule rule in rules)
            {
                if (rules.IndexOf(rule) != 0)
                {
                    sb.Append(",");
                }
                sb.Append(String.Format("{0}:{1}:{2}", rule.State,rule.Classification, rule.DisplayIcon));
                
            }
            sb.Append(xmlEndTag);
            

            return String.Format("({0})", sb.ToString());
        }
    }

    /// <summary>
    /// The DataValueFactory is a utility class used for building DataValue
    /// objects, and for serializing and deserializing DataValue objects to and from XML.
    /// </summary>
    public class DataValueFactory
    {
        //Dictionary<string, DataValue> dataTypes;
        static Regex typeregex = new Regex(@"^<(\w+)>");
        public DataValueFactory()
        {
            //typeregex = new Regex(@"^<(\w+)>.*$");
            //typeregex = new Regex(@"^<(\w+)>");
            //dataTypes = new Dictionary<string,DataValue>();
            //dataTypes["StringType"] = new StringValue();
            //dataTypes["IntegerType"] = new IntegerValue();
            //dataTypes["DoubleType"] = new DoubleValue();
            //dataTypes["LocationType"] = new LocationValue();
            //dataTypes["VelocityType"] = new VelocityValue();
            //dataTypes["AttributeCollectionType"] = new AttributeCollectionValue();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static DataValue BuildFromDataValue(DataValue dv)
        {
            DataValue returnDV;
            switch (dv.dataType)
            {
                case "WrapperType":
                    returnDV = DataValueFactory.BuildWrapper(((WrapperValue)dv).value);
                    return returnDV;
                    break;
                case "StringType":
                    returnDV = DataValueFactory.BuildString(((StringValue)dv).value);
                    return returnDV;
                    break;
                case "IntegerType":
                    returnDV = DataValueFactory.BuildInteger(((IntegerValue)dv).value);
                    return returnDV;
                    break;
                case "DoubleType":
                    returnDV = DataValueFactory.BuildDouble(((DoubleValue)dv).value);
                    return returnDV;
                    break;
                case "LocationType":
                    returnDV = DataValueFactory.BuildLocation(((LocationValue)dv).X, ((LocationValue)dv).Y, ((LocationValue)dv).Z, ((LocationValue)dv).exists);
                    return returnDV;
                    break;
                case "VelocityType":
                    returnDV = DataValueFactory.BuildVelocity(((VelocityValue)dv).VX, ((VelocityValue)dv).VY, ((VelocityValue)dv).VZ);
                    return returnDV;
                    break;
                case "StringListType":
                    returnDV = new StringListValue();
                    foreach (string s in ((StringListValue)dv).strings)
                    {
                        ((StringListValue)returnDV).strings.Add(s);
                    }
                    return returnDV;
                    break;
                case "RangeRingDisplayType":
                    returnDV = DataValueFactory.BuildRangeRingDisplayValue(((RangeRingDisplayValue)dv).name, ((RangeRingDisplayValue)dv).type, ((RangeRingDisplayValue)dv).isWeapon, ((RangeRingDisplayValue)dv).rangeIntensities);
                    return returnDV;
                    break;
                case "AttackCollectionType":
                    returnDV = new AttackCollectionValue();
                    String errMsg = String.Empty;
                    foreach (AttackCollectionValue.AttackValue av in ((AttackCollectionValue)dv).GetCurrentAttacks())
                    {
                        AttackCollectionValue.AttackValue newAttack = new AttackCollectionValue.AttackValue(av.attackStartTime, av.attackTimeWindow, av.targetObjectId, av.attackingObjectId, av.capabilityName, av.percentageApplied, av.isSelfdefense);
                        ((AttackCollectionValue)returnDV).AddAttack(newAttack, out errMsg);
                    }
                    return returnDV;
                    break;
                default:
                    returnDV = new DataValue();
                    returnDV = dv;
                    return returnDV;
            }
            return null;
        }

        /// <summary>
        /// A helper method for building and populating a WrapperValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildWrapper(DataValue value)
        {
            DataValue dv = BuildValue("WrapperType");
            ((WrapperValue)dv).value = DataValueFactory.BuildFromDataValue(value);
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a StringValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildString(string value)
        {
            DataValue dv = BuildValue("StringType");
            ((StringValue)dv).value = value;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a StringListValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildStringList(List<string> valueList)
        {
            DataValue dv = BuildValue("StringListType");
            ((StringListValue)dv).strings = new List<string>(valueList.ToArray());
            return dv;
        }


        /// <summary>
        /// A helper method for building and populating an IntegerValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildInteger(int value)
        {
            DataValue dv = BuildValue("IntegerType");
            ((IntegerValue)dv).value = value;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a BooleanValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildBoolean(bool value)
        {
            DataValue dv = BuildValue("BooleanType");
            ((BooleanValue)dv).value = value;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a DoubleValue object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataValue BuildDouble(double value)
        {
            DataValue dv = BuildValue("DoubleType");
            ((DoubleValue)dv).value = value;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a LocationValue object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        public static DataValue BuildLocation(double x, double y, double z, bool exists)
        {
            DataValue dv = BuildValue("LocationType");
            ((LocationValue)dv).X = x;
            ((LocationValue)dv).Y = y;
            ((LocationValue)dv).Z = z;
            ((LocationValue)dv).exists = exists;
            return dv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static DataValue BuildDouble(double x, double y, double z)
        {
            return BuildLocation(x, y, z, false);
        }
        /// <summary>
        /// A helper method for building and populating a VelocityValue object.
        /// </summary>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        /// <param name="vz"></param>
        /// <returns></returns>
        public static DataValue BuildVelocity(double vx, double vy, double vz)
        {
            DataValue dv = BuildValue("VelocityType");
            ((VelocityValue)dv).VX = vx;
            ((VelocityValue)dv).VY = vy;
            ((VelocityValue)dv).VZ = vz;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a AttributeCollectionValue object.
        /// </summary>
        /// <param name="attCollection"></param>
        /// <returns></returns>
        public static DataValue BuildAttributeCollection(Dictionary<string, DataValue> attCollection)
        {
            DataValue dv = BuildValue("AttributeCollectionType");
            ((AttributeCollectionValue)dv).attributes = attCollection;
            return dv;
        }

        /// <summary>
        /// A helper method for building and populating a CustomAttributesValue object.
        /// </summary>
        /// <param name="attCollection"></param>
        /// <returns></returns>
        public static DataValue BuildCustomAttributes(Dictionary<string, DataValue> attCollection)
        {
            DataValue dv = BuildValue("CustomAttributesType");
            ((CustomAttributesValue)dv).attributes = attCollection;
            return dv;
        }

        /// <summary>
        /// A Helper method for building and populating a DetectedValue object
        /// </summary>
        /// <exclude/>
        /// <param name="dv"></param>
        /// <param name="confidence"></param>
        /// <returns></returns>
        public static DataValue BuildDetectedValue(DataValue dv, int confidence)
        {
            DataValue dav = new DetectedAttributeValue() as DataValue;
            ((DetectedAttributeValue)dav).value = dv;
            ((DetectedAttributeValue)dav).stdDev = confidence;
            return dav;
        }
        /// <summary>
        /// A helper method for building and populating a RangeRingDisplayValue object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isWeapon"></param>
        /// <param name="rangeIntensities"></param>
        /// <returns></returns>
        public static DataValue BuildRangeRingDisplayValue(string name, string type, bool isWeapon, Dictionary<int, int> rangeIntensities)
        {
            DataValue rrdv = new RangeRingDisplayValue() as DataValue;
            ((RangeRingDisplayValue)rrdv).name = name;
            ((RangeRingDisplayValue)rrdv).type = type;
            ((RangeRingDisplayValue)rrdv).isWeapon = isWeapon;
            ((RangeRingDisplayValue)rrdv).rangeIntensities = new Dictionary<int,int>();

            foreach (KeyValuePair<int, int> kvp in rangeIntensities)
            {
                ((RangeRingDisplayValue)rrdv).rangeIntensities.Add(kvp.Key, kvp.Value);
            }
            return rrdv;
        }

        /// <summary>
        /// A method that takes a data type name, and returns a new DataValue object that corresponds
        /// to that type.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static DataValue BuildValue(string dataType)
        {
            switch (dataType)
            {
                case "StringType":
                    return new StringValue();
                case "DoubleType":
                    return new DoubleValue();
                case "IntegerType":
                    return new IntegerValue();
                case "BooleanType":
                    return new BooleanValue();
                case "LocationType":
                    return new LocationValue();
                case "VelocityType":
                    return new VelocityValue();
                case "AttributeCollectionType":
                    return new AttributeCollectionValue();
                case "CustomAttributesType":
                    return new CustomAttributesValue();
                case "StringListType":
                    return new StringListValue();
                case "PolygonType":
                    return new PolygonValue();
                case "StateTableType":
                    return new StateTableValue();
                case "CapabilityType":
                    return new CapabilityValue();
                case "VulnerabilityType":
                    return new VulnerabilityValue();
                case "ConeType":
                    return new ConeValue();
                case "SensorType":
                    return new SensorValue();
                case "SensorArrayType":
                    return new SensorArrayValue();
                case "EmitterType":
                    return new EmitterValue();
                case "RangeRingDisplayType":
                    return new RangeRingDisplayValue(); 
                case "AttackCollectionType":
                    return new AttackCollectionValue();
                case "WrapperType":
                    return new WrapperValue();
                case "ClassificationDisplayRulesType":
                    return new ClassificationDisplayRulesValue();
                default:
                    throw new Exception("DataValueFactory.BuildValue dataType is invalid");

            }
            //return dataTypes[dataType];

        }

        /// <summary>
        /// Takes a DataValue, and returns the xml representation.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string XMLSerialize(DataValue v)
        {
            if (v == null)
            {
                return "";
            }
            switch (v.dataType)
            {
                case "StringType":
                    return ((StringValue)v).ToXML();
                case "DoubleType":
                    return ((DoubleValue)v).ToXML();
                case "IntegerType":
                    return ((IntegerValue)v).ToXML();
                case "BooleanType":
                    return ((BooleanValue)v).ToXML();
                case "LocationType":
                    return ((LocationValue)v).ToXML();
                case "VelocityType":
                    return ((VelocityValue)v).ToXML();
                case "AttributeCollectionType":
                    return ((AttributeCollectionValue)v).ToXML();
                case "CustomAttributesType":
                    return ((CustomAttributesValue)v).ToXML();
                case "StringListType":
                    return ((StringListValue)v).ToXML();
                case "PolygonType":
                    return ((PolygonValue)v).ToXML();
                case "StateTableType":
                    return ((StateTableValue)v).ToXML();
                case "CapabilityType":
                    return ((CapabilityValue)v).ToXML();
                case "VulnerabilityType":
                    return ((VulnerabilityValue)v).ToXML();
                case "ConeType":
                    return ((ConeValue)v).ToXML();
                case "SensorType":
                    return ((SensorValue)v).ToXML();
                case "SensorArrayType":
                    return ((SensorArrayValue)v).ToXML();
                case "EmitterType":
                    return ((EmitterValue)v).ToXML();
                case "RangeRingDisplayType":
                    return ((RangeRingDisplayValue)v).ToXML();
                case "AttackCollectionType":
                    return ((AttackCollectionValue)v).ToXML();
                case "WrapperType":
                    return ((WrapperValue)v).ToXML();
                case "ClassificationDisplayRulesType":
                    return ((ClassificationDisplayRulesValue)v).ToXML();
                default:
                    return "";
            }
        }

        /// <summary>
        /// Takes a DataValue, and returns a human-readable string representation.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(DataValue v)
        {
            if (v == null)
            {
                return "";
            }
            switch (v.dataType)
            {
                case "StringType":
                    return ((StringValue)v).ToString();
                case "DoubleType":
                    return ((DoubleValue)v).ToString();
                case "IntegerType":
                    return ((IntegerValue)v).ToString();
                case "BooleanType":
                    return ((BooleanValue)v).ToString();
                case "LocationType":
                    return ((LocationValue)v).ToString();
                case "VelocityType":
                    return ((VelocityValue)v).ToString();
                case "AttributeCollectionType":
                    return ((AttributeCollectionValue)v).ToString();
                case "CustomAttributesType":
                    return ((CustomAttributesValue)v).ToString();
                case "StringListType":
                    return ((StringListValue)v).ToString();
                case "PolygonType":
                    return ((PolygonValue)v).ToString();
                case "StateTableType":
                    return ((StateTableValue)v).ToString();
                case "CapabilityType":
                    return ((CapabilityValue)v).ToString();
                case "VulnerabilityType":
                    return ((VulnerabilityValue)v).ToString();
                case "ConeType":
                    return ((ConeValue)v).ToString();
                case "SensorType":
                    return ((SensorValue)v).ToString();
                case "SensorArrayType":
                    return ((SensorArrayValue)v).ToString();
                case "EmitterType":
                    return ((EmitterValue)v).ToString();
                case "RangeRingDisplayType":
                    return ((RangeRingDisplayValue)v).ToString();
                case "AttackCollectionType":
                    return ((AttackCollectionValue)v).ToString();
                case "WrapperType":
                    return ((WrapperValue)v).ToString();
                case "ClassificationDisplayRulesType":
                    return ((ClassificationDisplayRulesValue)v).ToString();
                default:
                    return "";
            }
        }

        /// <summary>
        /// Takes an xml string, and returns a DataValue object.
        /// Returns null if the xml doesn't represent a DataValue.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataValue XMLDeserialize(string xml)
        {
            Match m = typeregex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];

                string dataType = g.ToString();
                switch (dataType)
                {
                    case "StringType":
                        StringValue sv = new StringValue();
                        sv.FromXML(xml);
                        return sv;
                    case "DoubleType":
                        DoubleValue dv = new DoubleValue();
                        dv.FromXML(xml);
                        return dv;
                    case "IntegerType":
                        IntegerValue iv = new IntegerValue();
                        iv.FromXML(xml);
                        return iv;
                    case "BooleanType":
                        BooleanValue bv = new BooleanValue();
                        bv.FromXML(xml);
                        return bv;
                    case "LocationType":
                        LocationValue lv = new LocationValue();
                        lv.FromXML(xml);
                        return lv;
                    case "VelocityType":
                        VelocityValue vv = new VelocityValue();
                        vv.FromXML(xml);
                        return vv;
                    case "AttributeCollectionType":
                        AttributeCollectionValue av = new AttributeCollectionValue();
                        av.FromXML(xml);
                        return av;
                    case "CustomAttributesType":
                        CustomAttributesValue cav = new CustomAttributesValue();
                        cav.FromXML(xml);
                        return cav;
                    case "StringListType":
                        StringListValue slv = new StringListValue();
                        slv.FromXML(xml);
                        return slv;
                    case "PolygonType":
                        PolygonValue polyv = new PolygonValue();
                        polyv.FromXML(xml);
                        return polyv;
                    case "StateTableType":
                        StateTableValue stv = new StateTableValue();
                        stv.FromXML(xml);
                        return stv;
                    case "CapabilityType":
                        CapabilityValue cv = new CapabilityValue();
                        cv.FromXML(xml);
                        return cv;
                    case "VulnerabilityType":
                        VulnerabilityValue vv2 = new VulnerabilityValue();
                        vv2.FromXML(xml);
                        return vv2;
                    case "ConeType":
                        ConeValue cv2 = new ConeValue();
                        cv2.FromXML(xml);
                        return cv2;
                    case "SensorType":
                        SensorValue sv2 = new SensorValue();
                        sv2.FromXML(xml);
                        return sv2;
                    case "SensorArrayType":
                        SensorArrayValue sav = new SensorArrayValue();
                        sav.FromXML(xml);
                        return sav;
                    case "EmitterType":
                        EmitterValue ev = new EmitterValue();
                        ev.FromXML(xml);
                        return ev;
                    case "RangeRingDisplayType":
                        RangeRingDisplayValue rrdv = new RangeRingDisplayValue();
                        rrdv.FromXML(xml);
                        return rrdv;
                    case "AttackCollectionType":
                        AttackCollectionValue attCV = new AttackCollectionValue();
                        attCV.FromXML(xml);
                        return attCV;
                    case "WrapperType":
                        WrapperValue wrapper = new WrapperValue();
                        wrapper.FromXML(xml);
                        return wrapper;
                    case "ClassificationDisplayRulesType":
                        ClassificationDisplayRulesValue cdrv = new ClassificationDisplayRulesValue();
                        cdrv.FromXML(xml);
                        return cdrv;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// This method takes in two data values, and returns true if their values are equal.
        /// </summary>
        /// <param name="firstDV"></param>
        /// <param name="secondDV"></param>
        /// <returns></returns>
        public static bool CompareDataValues(DataValue firstDV, DataValue secondDV)
        {
            if (firstDV.getType() != secondDV.getType())
            {
                return false;
            }

            switch (firstDV.getType())
            {
                case "StringType":
                    if (((StringValue)firstDV).value == ((StringValue)secondDV).value)
                    {
                        return true;
                    }
                    break;
                case "IntegerType":
                    if (((IntegerValue)firstDV).value == ((IntegerValue)secondDV).value)
                    {
                        return true;
                    }
                    break;
                case "DoubleType":
                    if (((DoubleValue)firstDV).value == ((DoubleValue)secondDV).value)
                    {
                        return true;
                    }
                    break;
                case "BooleanType":
                    if (((BooleanValue)firstDV).value == ((BooleanValue)secondDV).value)
                    {
                        return true;
                    }
                    break;
                case "LocationType":
                    if (Math.Abs(((LocationValue)firstDV).X - ((LocationValue)secondDV).X) < 0.000001 &&
                        Math.Abs(((LocationValue)firstDV).Y - ((LocationValue)secondDV).Y) < 0.000001 &&
                        Math.Abs(((LocationValue)firstDV).Z - ((LocationValue)secondDV).Z) < 0.000001)
                    //if (((LocationValue)firstDV).X == ((LocationValue)secondDV).X &&
                    //    ((LocationValue)firstDV).Y == ((LocationValue)secondDV).Y &&
                    //    ((LocationValue)firstDV).Z == ((LocationValue)secondDV).Z)
                    {
                        return true;
                    }
                    break;
                case "VelocityType":
                    if (Math.Abs(((VelocityValue)firstDV).VX - ((VelocityValue)secondDV).VX) < 0.000001 &&
                        Math.Abs(((VelocityValue)firstDV).VY - ((VelocityValue)secondDV).VY) < 0.000001 &&
                        Math.Abs(((VelocityValue)firstDV).VZ - ((VelocityValue)secondDV).VZ) < 0.000001)
                    //if (((VelocityValue)firstDV).VX == ((VelocityValue)secondDV).VX &&
                    //    ((VelocityValue)firstDV).VY == ((VelocityValue)secondDV).VY &&
                    //    ((VelocityValue)firstDV).VZ == ((VelocityValue)secondDV).VZ)
                    {
                        return true;
                    }
                    break;
                case "AttributeCollectionType":
                    if (((AttributeCollectionValue)firstDV).ToString() == ((AttributeCollectionValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "CustomAttributesType":
                    if (((CustomAttributesValue)firstDV).ToString() == ((CustomAttributesValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "CapabilityType":
                    if (((CapabilityValue)firstDV).ToString() == ((CapabilityValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "ConeType":
                    if (((ConeValue)firstDV).ToString() == ((ConeValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "DetectedAttributeType":
                    if (CompareDataValues(((DetectedAttributeValue)firstDV).value, ((DetectedAttributeValue)secondDV).value))
                    {
                        return true;
                    }
                    break;
                case "EmitterType":
                    if (((EmitterValue)firstDV).ToString() == ((EmitterValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "PolygonType":
                    if (((PolygonValue)firstDV).ToString() == ((PolygonValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "SensorArrayType":
                    if (((SensorArrayValue)firstDV).ToString() == ((SensorArrayValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "SensorType":
                    if (((SensorValue)firstDV).ToString() == ((SensorValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "StateTableType":
                    if (((StateTableValue)firstDV).ToString() == ((StateTableValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "StringListType":
                    if (((StringListValue)firstDV).ToString() == ((StringListValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "VulnerabilityType":
                    if (((VulnerabilityValue)firstDV).ToString() == ((VulnerabilityValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "RangeRingDisplayType":
                    if (((RangeRingDisplayValue)firstDV).ToString() == ((RangeRingDisplayValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "AttackCollectionType":
                    if (((AttackCollectionValue)firstDV).ToString() == ((AttackCollectionValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                case "WrapperType":
                    DataValue firstNestedDV = ((WrapperValue)firstDV).value;
                    DataValue secondNestedDV = ((WrapperValue)firstDV).value;
                    return CompareDataValues(firstNestedDV, secondNestedDV);
                    break;
                case "ClassificationDisplayRulesType":
                    if (((ClassificationDisplayRulesValue)firstDV).ToString() == ((ClassificationDisplayRulesValue)secondDV).ToString())
                    {
                        return true;
                    }
                    break;
                default:
                    throw new Exception("There is no definition for this data type");
                    break;
            }

            return false;

        }
    }

}
