using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator.VSGPanes
{


    public partial class CntP_Types : Ctl_ContentPane
    {

        private Control _selected_control = null;
        public CntP_Types()
        {
            InitializeComponent();
            this.ctl_Species1.Hide();
            this.ctl_Species1.ContentPane = this;
            ctl_Species1.Dock = DockStyle.Fill;

            this.ctl_DecisionMaker1.Hide();
            this.ctl_DecisionMaker1.ContentPane = this;
            ctl_DecisionMaker1.Dock = DockStyle.Fill;

            this.ctl_Engram1.Hide();
            this.ctl_Engram1.ContentPane = this;
            ctl_Engram1.Dock = DockStyle.Fill;

            this.ctl_Emitters1.Hide();
            this.ctl_Emitters1.ContentPane = this;
            ctl_Emitters1.Dock = DockStyle.Fill;

            this.ctl_Sensors1.Hide();
            this.ctl_Sensors1.ContentPane = this;
            ctl_Sensors1.Dock = DockStyle.Fill;

            this.ctl_States1.Hide();
            this.ctl_States1.ContentPane = this;
            ctl_States1.Dock = DockStyle.Fill;

            this.ctl_Teams1.Hide();
            this.ctl_Teams1.ContentPane = this;
            ctl_Teams1.Dock = DockStyle.Fill;

            this.ctl_Networks1.Hide();
            this.ctl_Networks1.ContentPane = this;
            ctl_Networks1.Dock = DockStyle.Fill;

            this.ctl_Capabilities1.Hide();
            this.ctl_Capabilities1.ContentPane = this;
            ctl_Capabilities1.Dock = DockStyle.Fill;
        }

        public void ShowPanel(Types_NodeTypes type, Boolean newNode)
        {
            switch (type)
            {
                case Types_NodeTypes.Team_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_Teams1.ResetForNewEntry();
                    _selected_control = ctl_Teams1;
                    _selected_control.Show();
                    break;
                case Types_NodeTypes.Network_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_Networks1.ResetForNewEntry();
                    _selected_control = ctl_Networks1;
                    _selected_control.Show();
                    break;

                case Types_NodeTypes.DM_Node:
                    //   if (_selected_control != ctl_DecisionMaker1)
                    // {
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_DecisionMaker1.ResetForNewEntry();

                    _selected_control = ctl_DecisionMaker1;
                    _selected_control.Show();

                    break;
                case Types_NodeTypes.Define_Engram_Node:
                    //                   if (_selected_control != ctl_Engram1)
                    //                   {
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_Engram1.ResetForNewEntry();
                    _selected_control = ctl_Engram1;
                    _selected_control.Show();
                    //                 }
                    break;
                case Types_NodeTypes.Emitter_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_Emitters1.ResetForNewEntry();
                    _selected_control = ctl_Emitters1;
                    _selected_control.Show();
                    break;
                case Types_NodeTypes.Sensor_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    if (newNode)
                        ctl_Sensors1.ResetForNewEntry();
                    _selected_control = ctl_Sensors1;
                    _selected_control.Show();
                    break;
                case Types_NodeTypes.Species_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    _selected_control = ctl_Species1;
                    _selected_control.Show();

                    break;

                case Types_NodeTypes.State_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    _selected_control = ctl_States1;
                    _selected_control.Show();
                    break;
                case Types_NodeTypes.Capability_Node:
                    if (_selected_control != null)
                    {
                        _selected_control.Hide();
                    }
                    _selected_control = ctl_Capabilities1;
                    _selected_control.Show();

                    break;
            }
        }

        public override void Update(object object_data)
        {
            if (object_data is DecisionMakerDataStruct)
            {
                ctl_DecisionMaker1.Update(object_data);
            }
            if (object_data is EngramDataStruct)
            {
                ctl_Engram1.Update(object_data);
            }
            if (object_data is SpeciesDataStruct)
            {
                ctl_Species1.Update(object_data);
            }
            if (object_data is StateDataStruct)
            {
                ctl_States1.Update(object_data);
            }
            if (object_data is EmitterDataStruct)
            {
                ctl_Emitters1.Update(object_data);
            }
            if (object_data is SensorDataStruct)
            {
                ctl_Sensors1.Update(object_data);
            }
            if (object_data is TeamDataStruct)
            {
                ctl_Teams1.Update(object_data);
            }
            if (object_data is NetworkDataStruct)
            {
                ctl_Networks1.Update(object_data);
            }
            if (object_data is CapabilityDataStruct)
            {
                ctl_Capabilities1.Update(object_data);
            }

        }


    }


    public static class UniqueNames
    {
        private static List<string> _All = new List<string>();
        public static string Get(int i)
        {
            if (i < _All.Count)
            {
                return _All[i];
            }
            throw (new ArgumentOutOfRangeException("Access to Unique Names with index " + i.ToString()));

        }
        public static void Add(string s)
        {
            if (!_All.Contains(s))
                _All.Add(s);
        }
        public static Boolean Contains(string name)
        {
            return _All.Contains(name);
        }
        public static void Remove(string s)
        {
            if (_All.Contains(s))
                _All.Remove(s);
        }






    }

    public class ObjectTypeLists<T> where T : ObjectTypeStructure
    {
        private static List<T> _List = new List<T>();
        public void AddToList(T item)
        {
            Boolean found = false;
            for (int i = 0; i < _List.Count; i++)
            {
                if (_List[i].ID == item.ID)
                {
                    found = true;
                    _List[i] = (T)(item.Clone());
                    break;
                }
            }
            if (!found)
            {
                T intermediate = (T)(item.Clone());
                _List.Add(intermediate);
            }   //     _List.Add( (T)(  ( (T)(item) ).Clone()) );
        }
        public T GetFromList(string s)
        {
            T returnValue = null;
            for (int i = 0; i < _List.Count; i++)
            {
                if (_List[i].ID == s)
                {
                    returnValue = (T)(_List[i].Clone());
                    break;
                }
            }
            return returnValue;

        }
        public Boolean NameInUse(string name)
        {
            Boolean returnValue = false;
            foreach (T item in _List)
            {
                if (item.ID == name)
                {
                    MessageBox.Show(string.Format(" ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }
        public List<string> GetNames()
        {
            List<string> returnValue = new List<string>();
            if (null != _List)
            {
                for (int i = 0; i < _List.Count; i++)
                    returnValue.Add(_List[i].ID);
            }
            return returnValue;
        }
        /*    public  List<T> GetListItems()
            {
                List<T> returnValue = new List<T>();
                for (int i = 0; i < _List.Count; i++)
                    returnValue.Add(((T)(_List[i].Clone())));

                return returnValue;
            }*/
        public void RemoveListItem(int i)
        {
            if (i < _List.Count)
                _List.RemoveAt(i);
        }
        public void RemoveListItem(string s)
        {
            for (int i = 0; i < _List.Count; i++)
            {
                if (_List[i].ID == s)
                {
                    _List.RemoveAt(i);
                    break;
                }
            }
        }


    }


    public class ObjectTypeStructure : ICloneable
    {
        public string ID = "";
        public Boolean globallyUnique = false;
        public Boolean CheckUniquenessOnSave = false;
        public ObjectTypeStructure() { }
        public ObjectTypeStructure(string name, Boolean unique)
        {
            ID = name;
            globallyUnique = unique;
            if (globallyUnique)
                UniqueNames.Add(name);

        }
        #region ICloneable Members


        public virtual object Clone()
        {
            ObjectTypeStructure obj = new ObjectTypeStructure();
            obj.ID = ID;
            obj.globallyUnique = globallyUnique;
            return (object)obj;
        }

        #endregion
    }

    public class EngramDataStruct : ObjectTypeStructure
    {
        public string Value = "";
        public static EngramDataStruct Empty()
        {
            return new EngramDataStruct("");
        }

        public EngramDataStruct(string name)
            : base(name, true)
        {

        }
        public override bool Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is EngramDataStruct))
            {
                returnValue = false;
            }
            else
            {
                EngramDataStruct alias = (EngramDataStruct)obj;
                if (alias.ID != this.ID)
                {
                    returnValue = false;
                }
                else
                {
                    if (alias.Value != this.Value)
                    {
                        returnValue = false;
                    }

                }

            }

            return returnValue;


        }
        #region ICloneable Members

        public override object Clone()
        {
            EngramDataStruct obj = new EngramDataStruct(ID);
            obj.Value = this.Value;
            return obj;
        }

        #endregion
    }

    public class SensorDataStruct : ObjectTypeStructure
    {
        public Boolean SingleSelect = true;
        public string Attribute;
        public double Range;
        public double Spread;
        public string Level;
        public double X;
        public double Y;
        public double Z;

        public SensorDataStruct(string name)
            : base(name, true)
        {

        }
        public static SensorDataStruct Empty()
        {
            return new SensorDataStruct("");
        }
        public override Boolean Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is SensorDataStruct))
            {
                returnValue = false;
            }
            else
            {
                SensorDataStruct alias = (SensorDataStruct)obj;
                if (alias.ID != ID)
                    returnValue = false;
                if (alias.SingleSelect != SingleSelect)
                    returnValue = false;
                if (alias.Range != Range)
                    returnValue = false;
                if (alias.Attribute != Attribute)
                    returnValue = false;
                if (alias.Level != Level)
                    returnValue = false;
                if ((alias.X != X) || (alias.Y != Y) || (alias.Z != Z))
                    returnValue = false;
            }
            return returnValue;
        }

        #region ICloneable Members

        public override object Clone()
        {
            SensorDataStruct obj = new SensorDataStruct(ID);
            obj.Attribute = Attribute;
            obj.Range = Range;
            obj.Level = Level;
            obj.Spread = Spread;
            obj.SingleSelect = SingleSelect;
            obj.X = X;
            obj.Y = Y;
            obj.Z = Z;

            return obj;
        }

        #endregion
    }

    public class Transition : ICloneable
    {
        public double Range = 0;
        public int Intensity = 0;
        public int Probability = 100;
        public string State = "";
        public Transition() { }
        public Transition(Transition t)
        {
            if (null != t)
            {
                Range = t.Range;
                Intensity = t.Intensity;
                Probability = t.Probability;
                State = t.State;
            }
        }


        public Boolean Equals(Transition t)
        {
            Boolean returnValue = true;
            if (null == t)
            {
                returnValue = false;
            }
            else
            {
                if ((t.Range != Range) || (t.Intensity != Intensity)
                    || (t.Probability != Probability) || (t.State != State))
                    returnValue = false;

            }
            return returnValue;
        }

        #region ICloneable Members

        public object Clone()
        {
            Transition obj = new Transition(this);

            return obj;
        }

        #endregion

    }

    public class Singleton : ICloneable
    {

        public string Capability = "";
        public Transition Transition_1 = new Transition();
        public Transition Transition_2 = new Transition();

        public Singleton() { }
        public Singleton(Singleton s)
        {
            Capability = s.Capability;
            Transition_1 = s.Transition_1;
            Transition_2 = s.Transition_2;

        }
        public Boolean Equals(Singleton s)
        {
            Boolean returnValue = true;
            if (null == s)
            {
                returnValue = false;
            }
            else if (Capability != s.Capability)
            {
                returnValue = false;
            }
            else if (!(s.Transition_1.Equals(Transition_1) || s.Transition_1.Equals(Transition_2)))
                returnValue = false;
            else if (!(s.Transition_2.Equals(Transition_1) || s.Transition_2 == Transition_2))
                returnValue = false;
            else if (!(Transition_1.Equals(s.Transition_1) || Transition_1.Equals(s.Transition_2)))
                returnValue = false;
            else if (!(Transition_2.Equals(s.Transition_1) || Transition_2.Equals(s.Transition_2)))
                returnValue = false;

            return returnValue;

        }

        #region ICloneable Members
        public object Clone()
        {
            Singleton obj = new Singleton(this);
            return obj;
        }

        #endregion

    }

    public class Contribution : ICloneable
    {
        public string Capability = "";
        public double Range = 0;
        public int Effect = 0;
        public int Probability = 100;
        public  Contribution() { }
        public Contribution(Contribution c)
        {
            Capability = c.Capability;
            Range = c.Range;
            Probability = c.Probability;
            Effect = c.Effect;
        }
        public Boolean Equals(Contribution c)
        {
            Boolean returnValue = true;
            if (!((Capability == c.Capability) && (Range == c.Range) &&
                (Probability == c.Probability) && (Effect == c.Effect)))
                returnValue = false;
            return returnValue;
        }
        #region ICloneable Members

        public object Clone()
        {
            Contribution obj = new Contribution(this);

            return obj;
        }

        #endregion
    }
    public class Combo : ICloneable
    {
        public Contribution Contribution_1 = new Contribution();
        public Contribution Contribution_2 = new Contribution();
        public string NewState = "";
        public Combo()
        { }
        public Combo(Combo c)
        {
            if (null != c)
            {
                NewState = c.NewState;
                Contribution_1 = (Contribution)(c.Contribution_1.Clone());
                Contribution_2 = (Contribution)(c.Contribution_2.Clone());
            }
        }

        public static Combo Empty()
        {
            return new Combo();
        }
        public Boolean Equals(Combo c)
        {
            Boolean returnValue = true;
            if (c.NewState != NewState)
                returnValue = false;
            else if (!c.Contribution_1.Equals(Contribution_1))
                returnValue = false;
            else if (!c.Contribution_2.Equals(Contribution_2))
                returnValue = true;
            return returnValue;
        }
        #region ICloneable Members

        public object Clone()
        {
            Combo obj = new Combo(this);

            return obj;
        }

        #endregion
    }

    public class Effect
    {
        public int Intensity = 0;
        public int Probability = 0;
        public Effect()
        {
        }
        public Effect(int intensity, int probability)
        {
            Intensity = intensity;
            Probability = probability;
        }
        public Effect(decimal intensity, decimal probability)
        {
            Intensity = (int)intensity;
            Probability = (int)probability;
        }
    }
    public class Proximity
    {
        public double Range;
        public List<Effect> Effects = new List<Effect>();
        public Proximity()
        {

        }

        public Boolean Equals(Proximity p)
        {
            Boolean returnValue = true;
            if (p.Range != Range)
            {
                returnValue = false;
            }
            else if (p.Effects.Count != Effects.Count)
            {
                returnValue = false;
            }
            else
            {

                int countFinds = 0;
                for (int i = 0; i < p.Effects.Count; i++)
                {
                    for (int j = 0; j < Effects.Count; j++)
                    {
                        if ((p.Effects[i].Intensity == Effects[j].Intensity)
                            &&
                            (p.Effects[i].Probability == Effects[j].Probability))
                        {
                            countFinds += 1;
                            break;
                        }
                    }

                }
                if (countFinds != p.Effects.Count)
                    returnValue = false;
                countFinds = 0;
                for (int i = 0; i < Effects.Count; i++)
                {

                    for (int j = 0; j < p.Effects.Count; j++)
                    {
                        if ((p.Effects[i].Intensity == Effects[j].Intensity)
                             &&
                              (p.Effects[i].Probability == Effects[j].Probability))
                        {
                            countFinds += 1;
                            break;
                        }
                    }

                }
                if (countFinds != Effects.Count)
                    returnValue = false;
            }
            return returnValue;
        }

    }
    public class StateParameters : ICloneable
    {
        public int LaunchDuration = 0;
        public int DockingDuration = 0;
        public int TimeToAttack = 0;
        public double MaximumSpeed = 0;
        public double FuelCapacity = 0;
        public double InitialFuelLoad = 0;
        public double FuelUseRate = 0;
        public string FuelDepletionState = "Dead";
        public Boolean Stealable = false;
        public List<string> Sensors = new List<string>();
        public List<string> Emitters = new List<string>();
        public List<string> Capabilities = new List<string>();
        public Singleton[] Singletons = new Singleton[3];
        public Combo[] Combos = new Combo[2];
        // Too many compares of List<string> to do in-line.
        // But this should be more generally donw
        private static Boolean Equals(List<string> a, List<string> b)
        {
            Boolean returnValue = true;
            int countMatches = 0;
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    if (a[i] == b[j])
                    {
                        countMatches += 1;
                        break;
                    }
                }
            }
            if (a.Count != countMatches)
            {
                returnValue = false;
            }
            else
            {
                for (int j = 0; j < b.Count; j++)
                {
                    for (int i = 0; i < a.Count; i++)
                    {
                        if (a[i] == b[j])
                        {
                            countMatches += 1;
                            break;
                        }
                    }
                }
                if (b.Count != countMatches)
                    returnValue = false;
            }
            return returnValue;

        }

        public static StateParameters Empty()
        {
            return new StateParameters();
        }
        public Boolean Equals(StateParameters s)
        {
            Boolean returnValue = true;
            if ((LaunchDuration != s.LaunchDuration)
                || (DockingDuration != s.DockingDuration)
                || (TimeToAttack != s.TimeToAttack)
                || (MaximumSpeed != s.MaximumSpeed)
                || (FuelCapacity != s.FuelCapacity)
                || (InitialFuelLoad != s.InitialFuelLoad)
                || (FuelUseRate != s.FuelUseRate)
                || (FuelDepletionState != s.FuelDepletionState)
                || (Stealable != s.Stealable)
            )
            {
                returnValue = false;
            }
            else
            {
                if (!Equals(Sensors, s.Sensors))
                {
                    returnValue = false;
                }
                else if (!Equals(Emitters, s.Emitters))
                {
                    returnValue = false;
                }
                else if (!Equals(Capabilities, s.Capabilities))
                {
                    returnValue = false;
                }
                else if ((Singletons.Length != s.Singletons.Length)
                    || (Combos.Length != s.Combos.Length))
                {
                    returnValue = false;
                }
                else //Don't check for shuffles on speciess or combos.Too ugly
                {
                    try
                    {// to avoid nulls in comparisons
                        for (int i = 0; i < Singletons.Length; i++)
                        {
                            if (!Singletons[i].Equals(s.Singletons[i]))
                            {
                                returnValue = false;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        returnValue = false;
                    }
                    try
                    {
                        for (int i = 0; i < Combos.Length; i++)
                        {
                            if (!Combos[i].Equals(s.Combos[i]))
                            {
                                returnValue = false;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        returnValue = false;
                    }
                }


            }




            return returnValue;
        }
        #region ICloneable Members

        public object Clone()
        {
            StateParameters obj = new StateParameters();
            obj.LaunchDuration = LaunchDuration;
            obj.DockingDuration = DockingDuration;
            obj.TimeToAttack = TimeToAttack;
            obj.MaximumSpeed = MaximumSpeed;
            obj.FuelCapacity = FuelCapacity;
            obj.InitialFuelLoad = InitialFuelLoad;
            obj.FuelUseRate = FuelUseRate;
            obj.FuelDepletionState = FuelDepletionState;
            obj.Stealable = Stealable;
            for (int i = 0; i < Sensors.Count; i++)
                obj.Sensors.Add(Sensors[i]);
            for (int i = 0; i < Emitters.Count; i++)
                obj.Emitters.Add(Emitters[i]);
            for (int i = 0; i < Capabilities.Count; i++)
                obj.Capabilities.Add(Capabilities[i]);
            for (int i = 0; i < Singletons.Length; i++)
                obj.Singletons[i]=(Singleton)(Singletons[i].Clone());
            for (int i = 0; i < Combos.Length; i++)
                obj.Combos[i]=(Combo)(Combos[i].Clone());
            return obj;
        }

        #endregion

    }

    public class CapabilityDataStruct : ObjectTypeStructure
    {

        public List<Proximity> Proximities = new List<Proximity>();
        public CapabilityDataStruct(string name)
            : base(name, false)
        {
            Proximities.Add(new Proximity()); //semi hacj sine only 2 are allowed now
            Proximities[0].Effects.Add(new Effect(0, 100));// hack since only 2 are allowed
            Proximities[0].Effects.Add(new Effect(0, 100));
            Proximities.Add(new Proximity());
            Proximities[1].Effects.Add(new Effect(0, 100));
            Proximities[1].Effects.Add(new Effect(0, 100));
        }
        public void Add(Proximity p)
        {
            Proximity newP = new Proximity();
            newP.Effects.Clear();
            newP.Range = p.Range;
            for (int i = 0; i < p.Effects.Count; i++)
            {
                Effect eff = new Effect(p.Effects[i].Intensity, p.Effects[i].Probability);
                newP.Effects.Add(eff);
            }
            Proximities.Add(p);
        }
        public static CapabilityDataStruct Empty()
        {
            return new CapabilityDataStruct("");
        }

        #region ICloneable Members

        public override object Clone()
        {
            CapabilityDataStruct obj = new CapabilityDataStruct(ID);
            obj.Proximities.Clear();
            for (int i = 0; i < Proximities.Count; i++)
            {
                obj.Add(Proximities[i]);

            }


            return obj;
        }

        public override Boolean Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is CapabilityDataStruct))
            {
                returnValue = false;
            }
            else
            {
                CapabilityDataStruct alias = (CapabilityDataStruct)obj;
                if (alias.ID != ID)
                {
                    returnValue = false;
                }
                else if (alias.Proximities.Count != Proximities.Count)
                {
                    returnValue = false;

                }
                {
                    int countFinds = 0;
                    for (int i = 0; i < alias.Proximities.Count; i++)
                    {

                        for (int j = 0; j < Proximities.Count; j++)
                        {
                            if (Proximities[j] == alias.Proximities[i])
                            {
                                countFinds += 1;
                                break;
                            }
                        }

                    }
                    if (countFinds != alias.Proximities.Count)
                        returnValue = false;
                }
            }
            return returnValue;
        }
        #endregion
    }
    public class EmitterDataStruct : ObjectTypeStructure
    {
        public string Attribute = "";
        public Boolean AllAttributes = true;
        public List<string> Levels = new List<string>();
        public List<double> Variances = new List<double>();
        public Boolean Unlimited = true;
        //NB: Invisible defined by attribute=="" and !AllAttributes


        public EmitterDataStruct(string name)
            : base(name, false)
        {

        }
        public static EmitterDataStruct Empty()
        {
            return new EmitterDataStruct("");
        }

        public override Boolean Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is EmitterDataStruct))
            {
                returnValue = false;
            }
            else
            {
                EmitterDataStruct alias = (EmitterDataStruct)obj;
                if (alias.ID != ID)
                    returnValue = false;
                if (alias.AllAttributes != AllAttributes)
                    returnValue = false;
                if (alias.Attribute != Attribute)
                    returnValue = false;
                if (alias.Unlimited != Unlimited)
                    returnValue = false;
                for (int i = 0; i < alias.Levels.Count; i++)
                {
                    if (!Levels.Contains(alias.Levels[i]))
                    {
                        returnValue = false;
                        break;
                    }
                }
                for (int i = 0; i < Levels.Count; i++)
                {
                    if (!alias.Levels.Contains(Levels[i]))
                    {
                        returnValue = false;
                        break;
                    }
                }
            }
            return returnValue;
        }

        #region ICloneable Members

        public override object Clone()
        {
            EmitterDataStruct obj = new EmitterDataStruct(ID);
            obj.AllAttributes = AllAttributes;
            obj.Attribute = Attribute;
            obj.Unlimited = Unlimited;
            for (int i = 0; i < Levels.Count; i++)
            {
                obj.Levels.Add(Levels[i]);
                obj.Variances.Add(Variances[i]);
            }

            return obj;
        }

        #endregion
    }

    public class StateDataStruct : ObjectTypeStructure
    {
        // Note: Species is subsumed into ID field
        public string Icon = "";
        public StateParameters Parameters = new StateParameters();
        public StateDataStruct(string name)
            : base(name, true)
            /* Note: By appending each statename with the species name
             * wemake the global uniqueness check into the type-only one.
             */ 
        {

        }
        public static StateDataStruct Empty()
        {
            return new StateDataStruct("");
        }

        #region ICloneable Members

        public override Object Clone()
        {
            StateDataStruct obj = new StateDataStruct(ID);
            obj.Icon = Icon;
            obj.Parameters = (StateParameters)(Parameters.Clone());
            return obj;
        }

        #endregion
    }

    public class TeamDataStruct : ObjectTypeStructure
    {
        //   (Removed from all xxxDataStructs ) public string ID;
        private List<string> opponents = new List<string>();
        public List<string> Opponents
        {
            get
            {
                if (null == opponents)
                    opponents = new List<string>();
                return opponents;
            }
        }
        public void AddOpponent(string s)
        {
            if (null == opponents)
                opponents = new List<string>();
            if (!opponents.Contains(s))
                opponents.Add(s);
        }
        public void DropOpponent(string s)
        {
            if (opponents.Contains(s))
                opponents.Remove(s);
        }
        public static TeamDataStruct Empty()
        {
            return new TeamDataStruct("");
        }
        public override bool Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is TeamDataStruct))
            {
                returnValue = false;
            }
            else
            {
                TeamDataStruct alias = (TeamDataStruct)obj;
                if (alias.ID != this.ID)
                {
                    returnValue = false;
                }
                else
                {// Neater to check sizes befoe doing array compares, but the lists are so small it hardly matters
                    for (int i = 0; i < alias.Opponents.Count; i++)
                    {
                        if (!this.Opponents.Contains(alias.Opponents[i]))
                        {
                            returnValue = false;
                            break;
                        }
                    }
                    if (returnValue)
                    {
                        for (int i = 0; i < this.Opponents.Count; i++)
                        {
                            if (!alias.Opponents.Contains(this.Opponents[i]))
                            {
                                returnValue = false;
                                break;
                            }
                        }
                    }

                }

            }

            return returnValue;


        }

        public TeamDataStruct(string name)
            : base(name, true)
        {

        }
        #region ICloneable Members

        public override object Clone()
        {
            TeamDataStruct obj = new TeamDataStruct(ID);
            for (int i = 0; i < Opponents.Count; i++)
                obj.Opponents.Add(Opponents[i]);
            return obj;
        }

        #endregion
    }
    public class NetworkDataStruct : ObjectTypeStructure
    {
        //   (Removed from all xxxDataStructs ) public string ID;
        private List<string> members = new List<string>();
        public List<string> Members
        {
            get
            {
                if (null == members)
                    members = new List<string>();
                return members;
            }
        }
        public void AddMember(string s)
        {
            if (null == members)
                members = new List<string>();
            if (!members.Contains(s))
                members.Add(s);
        }
        public void DropMember(string s)
        {
            if (members.Contains(s))
                members.Remove(s);
        }
        public static NetworkDataStruct Empty()
        {
            return new NetworkDataStruct("");
        }
        public override bool Equals(object obj)
        {
            Boolean returnValue = true;
            if (!(obj is NetworkDataStruct))
            {
                returnValue = false;
            }
            else
            {
                NetworkDataStruct alias = (NetworkDataStruct)obj;
                if (alias.ID != this.ID)
                {
                    returnValue = false;
                }
                else
                {// Neater to check sizes befoe doing array compares, but the lists are so small it hardly matters
                    for (int i = 0; i < alias.members.Count; i++)
                    {
                        if (!this.members.Contains(alias.members[i]))
                        {
                            returnValue = false;
                            break;
                        }
                    }
                    if (returnValue)
                    {
                        for (int i = 0; i < this.members.Count; i++)
                        {
                            if (!alias.members.Contains(this.members[i]))
                            {
                                returnValue = false;
                                break;
                            }
                        }
                    }

                }

            }

            return returnValue;


        }

        public NetworkDataStruct(string name)
            : base(name, true)
        {

        }
        #region ICloneable Members

        public override object Clone()
        {
            NetworkDataStruct obj = new NetworkDataStruct(ID);
            for (int i = 0; i < members.Count; i++)
                obj.members.Add(members[i]);
            return obj;
        }

        #endregion
    }

}
