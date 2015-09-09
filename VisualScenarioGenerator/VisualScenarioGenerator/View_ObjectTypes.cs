using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;
using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    public struct StructObjectTypes
    {
        public List<object> DecisionMakerList;
        public List<object> SpeciesList;
        public List<object> EngramList;
        public List<object> SensorList;
        public List<object> StateList;
        public List<object> EmitterList;

        public static StructObjectTypes Empty = new StructObjectTypes(new List<object>(), new List<object>(), new List<object>(), new List<object>(), new List<object>(),new List<object>());
        public StructObjectTypes(List<object> dm, List<object> species, List<object> engrams, List<object> sensors,List<object>emitters, List<object> states)
        {
            DecisionMakerList = dm;
            SpeciesList = species;
            EngramList = engrams;
            SensorList = sensors;
            EmitterList = emitters;
            StateList = states;
        }
    }
    public class View_ObjectTypes : View //, ICtl_ContentPane__OutboundUpdate
    {
        public string CurrentAssetID = string.Empty;

        //private List<object> _DecisionMakerList = new List<object>();
        //private List<object> _SpeciesList = new List<object>();
        //private List<object> _EngramList = new List<object>();
        //private List<object> _SensorList = new List<object>();
        //private List<object> _StateList = new List<object>();
        //      private List<object> _TeamList = new List<object>();
        private StructObjectTypes Types = StructObjectTypes.Empty;


        public View_ObjectTypes()
            : base(new NavP_Types(), new CntP_Types())
        {
        }

        public CntP_Types GetContentPanel()
        {
            return ((CntP_Types)ContentPanel);
        }

        public NavP_Types GetNavigationPanel()
        {
            return ((NavP_Types)NavigatorPanel);
        }

        public bool NameInUse(Types_NodeTypes type, string name,Boolean globallyUnique)
        {
            bool returnValue = false;

            switch (type)
            {
                case Types_NodeTypes.DM_Node:
                    foreach (DecisionMakerDataStruct s1 in Types.DecisionMakerList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("A decision maker with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case Types_NodeTypes.Define_Engram_Node:
                    foreach (EngramDataStruct s1 in Types.EngramList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("An engram with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case Types_NodeTypes.Emitter_Node:
                    foreach (EmitterDataStruct em1 in Types.EmitterList)
                    {
                        if (em1.ID == name)
                        {
                            MessageBox.Show(string.Format("An emitter with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case Types_NodeTypes.Sensor_Node:
                    foreach (SensorDataStruct s1 in Types.SensorList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("A sensor with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case Types_NodeTypes.Species_Node:
                    foreach (SpeciesDataStruct s1 in Types.SpeciesList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("A species with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case Types_NodeTypes.State_Node:
                    returnValue = Ctl_States.States.NameInUse(name);
                    break;
                case Types_NodeTypes.Network_Node:

                    returnValue = Ctl_Networks.Networks.NameInUse(name);
                    break;
            }
            if (!returnValue)
            {
                if (globallyUnique&& UniqueNames.Contains(name))
                {
                    MessageBox.Show(string.Format("The name ID ({0}) has been used for another object in this scenario.  Please choose another name.", name), "Cannot complete update.");

                }
            }
            return returnValue;
        }

        public bool NameInUse(object object_data)
        {
            bool returnValue = false;
            if (((ObjectTypeStructure)(object_data)).CheckUniquenessOnSave)
            {
                if (object_data is DecisionMakerDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.DM_Node, ((DecisionMakerDataStruct)object_data).ID,((DecisionMakerDataStruct)object_data).globallyUnique);
                }
                if (object_data is SpeciesDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Species_Node, ((SpeciesDataStruct)object_data).ID,((SpeciesDataStruct)object_data).globallyUnique);
                }
                if (object_data is EngramDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Define_Engram_Node, ((EngramDataStruct)object_data).ID, ((EngramDataStruct)object_data).globallyUnique);
                }
                if (object_data is StateDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.State_Node, ((StateDataStruct)object_data).ID, ((StateDataStruct)object_data).globallyUnique);
                }
                if (object_data is EmitterDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Emitter_Node, ((EmitterDataStruct)object_data).ID, ((EmitterDataStruct)object_data).globallyUnique);
                }

                if (object_data is SensorDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Sensor_Node, ((SensorDataStruct)object_data).ID, ((SensorDataStruct)object_data).globallyUnique);
                }
                if (object_data is TeamDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Team_Node, ((TeamDataStruct)object_data).ID, ((TeamDataStruct)object_data).globallyUnique);
                }
                if (object_data is NetworkDataStruct)
                {
                    returnValue = NameInUse(Types_NodeTypes.Network_Node, ((NetworkDataStruct)object_data).ID, ((NetworkDataStruct)object_data).globallyUnique);
                }
 
            }

            if (!returnValue)
                ((ObjectTypeStructure)(object_data)).CheckUniquenessOnSave = false;
            return returnValue;
        }
        public DecisionMakerDataStruct CreateDecisionMaker(string name)
        {
            DecisionMakerDataStruct dm = new DecisionMakerDataStruct(name);

            Types.DecisionMakerList.Add(dm);
            if (!UniqueNames.Contains(name))
            {
                UniqueNames.Add(name);
            }

            return dm;
        }
        public SpeciesDataStruct CreateSpecies(string name)
        {
            SpeciesDataStruct sp = SpeciesDataStruct.Empty();
            sp.ID = name;
            Types.SpeciesList.Add(sp);
            if (!UniqueNames.Contains(name))
            {
                UniqueNames.Add(name);
            }

            return sp;
        }
        public EngramDataStruct CreateEngram(string name)
        {
            EngramDataStruct en = EngramDataStruct.Empty();
            en.ID = name;
            Types.EngramList.Add(en);
            if (!UniqueNames.Contains(name))
            {
                UniqueNames.Add(name);
            }
            return en;
        }
        public EmitterDataStruct CreateEmitter(string name)
        {
            EmitterDataStruct em = EmitterDataStruct.Empty();
            em.ID = name;
            Types.EmitterList.Add(em);
            /*
            if (!UniqueNames.Contains(name))
            {
                UniqueNames.Add(name);
            }
            */
            return em;
        }

        public SensorDataStruct CreateSensor(string name)
        {
            SensorDataStruct sn = SensorDataStruct.Empty();
            sn.ID = name;
            Types.SensorList.Add(sn);
            if (!UniqueNames.Contains(name))
            {
                UniqueNames.Add(name);
            }

            return sn;
        }
      //  This would have a problem because state names are stored as name+species
        // but will probably never be used
        public StateDataStruct CreateState(string name)
        {
            StateDataStruct st = StateDataStruct.Empty();
            st.ID = name;
            Types.StateList.Add(st);
            // Note: Uniqueness required only within a species

            return st;
        }
        public TeamDataStruct CreateTeam(string name)
        {
            TeamDataStruct tm = new TeamDataStruct(name);
            //          tm.ID = name       
            Ctl_Teams.Teams.AddToList(tm);
            if (!UniqueNames.Contains(name))
                UniqueNames.Add(name);
        

            return tm;
        }
        public NetworkDataStruct CreateNetwork(string name)
        {
            NetworkDataStruct nd = new NetworkDataStruct(name);
         
            Ctl_Networks.Networks.AddToList(nd);
            if (!UniqueNames.Contains(name))
                UniqueNames.Add(name);
           

            return nd;
        }
        public CapabilityDataStruct CreateCapability(string name)
        {
            CapabilityDataStruct cp = new CapabilityDataStruct(name);
            Ctl_Capabilities.Capabilities.AddToList(cp);
            return cp;
        }
        public object GetDecisionMaker(int index)
        {
            try
            {
                return Types.DecisionMakerList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }
        public object GetSpecies(int index)
        {
            try
            {
                return Types.SpeciesList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }
        public object GetEngram(int index)
        {
            try
            {
                return Types.EngramList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }
        public object GetEmitter(int index)
        {
            try
            {
                return Types.EmitterList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }

        public object GetSensor(int index)
        {
            try
            {
                return Types.SensorList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }
        public object GetState(int index)
        {
            try
            {
                return Types.StateList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }


        public void RemoveDecisionMaker(int index)
        {
            try
            {
                UniqueNames.Remove(((DecisionMakerDataStruct)(Types.DecisionMakerList[index])).ID);
                Types.DecisionMakerList.RemoveAt(index);

            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        public void RemoveSpecies(int index)
        {
            try
            {
                UniqueNames.Remove(((SpeciesDataStruct)(Types.SpeciesList[index])).ID);
                Types.SpeciesList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        public void RemoveEngram(int index)
        {
            try
            {
                UniqueNames.Remove(((EngramDataStruct)(Types.EngramList[index])).ID);
                Types.EngramList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        public void RemoveSensor(int index)
        {
            try
            {
                UniqueNames.Remove(((SensorDataStruct)(Types.SensorList[index])).ID);

                Types.SensorList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        public void RemoveState(int index)
        {
            try
            {
                //Note: Uniqueness required only within a species
                Types.StateList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }


        public override void UpdateContentPanel(object object_data)
        {
            GetContentPanel().Update(object_data);
        }
        public override void UpdateNavigatorPanel(Control control, object object_data)
        {
            if (NameInUse(object_data))
            {
                /* Dont continue if object data contains an id already in use for the subtree
                 * */
                return;
            }
            // or a name that must be unique is re-used
            if (object_data is VisualScenarioGenerator.VSGPanes.ObjectTypeStructure)
            {
                if (((VisualScenarioGenerator.VSGPanes.ObjectTypeStructure)(object_data)).globallyUnique)
                    UniqueNames.Add(((VisualScenarioGenerator.VSGPanes.ObjectTypeStructure)(object_data)).ID);

            }
            GetNavigationPanel().Update(object_data);
            int node_index = GetNavigationPanel().SelectedNode.Index;
            List<object> list = null;




            if (object_data is DecisionMakerDataStruct)
            {
                Ctl_DecisionMaker.DecisionMakers.AddToList((DecisionMakerDataStruct)object_data);
 
                
            }

            if (object_data is NetworkDataStruct)
            {
                Ctl_Networks.Networks.AddToList((NetworkDataStruct)object_data);


            }
            if (object_data is EngramDataStruct)
            {
                Ctl_Engram.Engrams.AddToList((EngramDataStruct)object_data);
               
            }
            if (object_data is SpeciesDataStruct)
            {
                list = Types.SpeciesList;
                Ctl_Species.Species.AddToList((SpeciesDataStruct)object_data);
                
            }
            // P
            if (object_data is StateDataStruct)
            {

                Ctl_States.States.AddToList((StateDataStruct)object_data);
            }
            if (object_data is SensorDataStruct)
            {
                Ctl_Sensors.Sensors.AddToList((SensorDataStruct)object_data);

            }
            if (object_data is EmitterDataStruct)

                Ctl_Emitters.Emitters.AddToList((EmitterDataStruct)object_data);
 

            if (object_data is TeamDataStruct)
                Ctl_Teams.Teams.AddToList((TeamDataStruct)object_data);

            if (object_data is CapabilityDataStruct)
                Ctl_Capabilities.Capabilities.AddToList((CapabilityDataStruct)object_data);

        }


        public override void Notify(object object_data)
        {
            VSG_Panel.Views[ViewType.Timeline].UpdateView((object)Types);
        }
    }
}
