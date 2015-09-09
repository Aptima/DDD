using System;
using System.IO;
using System.Collections.Generic;
using Aptima.Asim.DDD.CommonComponents.ScenarioControllerUnits;

namespace Aptima.Asim.DDD.ScenarioController
{



    /// <summary>
    /// a structure containing all the resolved states for each species 
    /// </summary>
    public class StatesForUnits
    {
        static private Dictionary<string, Dictionary<string, StateBody>> stateTable
            = new Dictionary<string, Dictionary<string, StateBody>>();

        static public Dictionary<string, Dictionary<string, StateBody>> StateTable
        {
            get { return stateTable; }
        }
        /// <summary>
        /// Empties the static state table; use when restarting server
        /// </summary>
        static public void Clear()
        {
            stateTable = new Dictionary<string, Dictionary<string, StateBody>>();
        }

        /// <summary>
        /// Applies all inheritance rules to produce a complete state table for a unit 
        /// </summary>
        /// <param name="unit">ID of a unit</param>
        /// <returns></returns>
        public static Dictionary<string, StateBody> GetExpandedStatesOf(string unit)
        {
            if (stateTable.ContainsKey(unit)) // for a species
            {
                return stateTable[unit];
            }
            else if (stateTable.ContainsKey(Genealogy.GetBase(unit)))//for an instance
            {
                return stateTable[Genealogy.GetBase(unit)];
            }
            else
            {
                return new Dictionary<string, StateBody>();
            }
        }

        /// <summary>
        /// Tels whether the given unit has the given state in its state table
        /// </summary>
        /// <param name="unit">ID of unit</param>
        /// <param name="state">name of state</param>
        /// <returns></returns>
        public static Boolean UnitHasState(string unit, string state)
        {
            Boolean returnValue = true;
            string baseSpecies = Genealogy.GetBase(unit);
            if (!stateTable.ContainsKey(baseSpecies))
            {
                returnValue = false;
            }
            else if (!stateTable[baseSpecies].ContainsKey(state))
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// Enters the given state information for the named state of the unit into the state table
        /// </summary>
        /// <param name="unit">ID of unit</param>
        /// <param name="stateName">Name of state</param>
        /// <param name="stateBody">Information about state</param>
        public static void AddStateOf(string unit, string stateName, StateBody stateBody)
        {

            if (!stateTable.ContainsKey(unit))
            {
                stateTable[unit] = new Dictionary<string, StateBody>();
            }
            stateTable[unit][stateName] = stateBody;
        }

        /// <summary>
        /// Add information about many states for a fiven unit (see AddStateOf)
        /// </summary>
        /// <param name="unit">ID of unit</param>
        /// <param name="states">Collection of state names and their associated information</param>
        public static void AddStatesOf(string unit, Dictionary<string, StateBody> states)
        {
            foreach (string k in states.Keys)
            {
                AddStateOf(unit, k, states[k]);
            }
        }
    }

    /// <summary>
    /// Working area for all of the state information for one unit (includign genera and species).
    /// </summary>
    /// <remarks> The idea here is that as state data comes in it is saved in 
    /// two structures -- one for FullyFunctional which is bthe asis for all
    /// others.
    /// On signal, the states are completed first using the data for based on,
    /// to get the best version of FullyFunctional (may be moot if fully functional must have
    /// all parameters defined, and then updating each individual state by applying it over FullyFunctional.</remarks>
    public class WorkStates
    {
        static private Dictionary<string, StateBody> allOtherStates;
        static public Dictionary<string, StateBody> AllOtherStates
        {
            get { return allOtherStates; }
        }
        static private StateBody fullyFunctional;
        static public StateBody FullyFunctional
        {
            get { return fullyFunctional; }
        }
        /// <summary>
        /// Adds theinformation about a state to the working tables
        /// </summary>
        /// <param name="state">Name of state</param>
        /// <param name="body">Information about state</param>
        static public void Add(string state, StateBody body)
        {
            if ("FullyFunctional" == state)
            {
                fullyFunctional = body;
                // Check for cases where defined defaults may be needed

            }
            else
            {
                allOtherStates[state] = body;
            }
        }

        /// <summary>
        /// Adds information about a collection of states to the working tables
        /// </summary>
        /// <param name="states">Collection of names and associated information</param>
        static public void Add(Dictionary<string, StateBody> states)
        {
            foreach (string k in states.Keys)
            {
                Add(k, states[k]);
            }
            if (!states.ContainsKey("Dead"))
            {
                StateBody deadState = new StateBody();
                Add("Dead", deadState);
            }
            allOtherStates["Dead"].Parameters["MaximumSpeed"] = 0;
        }

        /// <summary>
        /// Clears static tables
        /// </summary>
        static public void Clear()
        {
            allOtherStates = new Dictionary<string, StateBody>();
            fullyFunctional = new StateBody();
        }

        /// <summary>
        /// Determines the attributes of a state following inheritance rules
        /// </summary>
        /// <param name="name">Name of state</param>
        /// <param name="basedOn">Name of immediate predecessor in species hierarchy</param>
        /// <remarks>
        /// In collapsing we assume that the immediate predecessor has already been collapsed
        /// thereby avoiding all kinds of ugly recursion
        /// </remarks>
        static public Dictionary<string, StateBody> CollapseStates(string name, string basedOn)
        {
            List<string> excludedAttributes = new List<string>();
            excludedAttributes.Add("InitialFuelLoad");
            excludedAttributes.Add("Location");
            //"name" is the name of the thing being defined
            if (Genealogy.GetGenus(name) != basedOn)
            {
                //FullyFunctional inherits from previous FullyFunctional
                StateBody newFully = StatesForUnits.StateTable[basedOn]["FullyFunctional"].DeepCopy();
                List<string> KeyList = fullyFunctional.Parameters.GetKeys();
                foreach (string k in KeyList)
                {
                    try
                    {
                        // copy the parameters name by name
                        if (fullyFunctional.Parameters[k] == null)
                            continue;
                        String type = fullyFunctional.Parameters[k].GetType().ToString();
                        switch (type)
                        {

                            case "System.Double":
                            case "System.String":
                            case "System.Int32":
                            case "System.Boolean":
                                newFully.Parameters[k] = fullyFunctional.Parameters[k];
                                break;
                            case "Aptima.Asim.DDD.ScenarioController.LocationType":
                                newFully.Parameters[k] = ((LocationType)fullyFunctional.Parameters[k]).DeepCopy();
                                break;
                            case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                                newFully.Parameters[k] = ((VelocityType)fullyFunctional.Parameters[k]).DeepCopy();
                                break;
                            case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                                newFully.Parameters[k] = fullyFunctional.Parameters[k]; //Don't need a deep copy because these won't change
                                break;

                        }

                    }
                    catch (System.Exception e)
                    {
                        throw new ApplicationException("Could not DeepCopy Parameter in StateBody", e);

                    }


                }
                /* If no sensors are defined for a unit, then it inherits
                 * Otherwise it totally overrides
                 */
                if (fullyFunctional.Sensors.Count > 0)
                {
                    newFully.Sensors.Clear();
                    for (int i = 0; i < fullyFunctional.Sensors.Count; i++)
                    {
                        newFully.Sensors.Add(fullyFunctional.Sensors[i]);
                    }
                }

                foreach (string k in fullyFunctional.Capabilities.Keys)
                {
                    newFully.Capabilities[k] = fullyFunctional.Capabilities[k].DeepCopy();
                }

                foreach (string k in fullyFunctional.Vulnerabilities.Keys)
                {
                    newFully.Vulnerabilities[k] = fullyFunctional.Vulnerabilities[k].DeepCopy();
                }
                for (int i = 0; i < fullyFunctional.Combinations.Count; i++)
                {
                    newFully.Combinations.Add(fullyFunctional.Combinations[i].DeepCopy());
                }

                /// with emitters there is strict, attribute-by-attribute, inheritance
                foreach (string attribute in fullyFunctional.Emitters.Keys)
                {
                    newFully.Emitters[attribute] = fullyFunctional.Emitters[attribute].DeepCopy();
                }
                fullyFunctional = newFully;
                // Add in states of base that are not in this species
                Dictionary<string, StateBody> parent = StatesForUnits.GetExpandedStatesOf(basedOn);
                // Other states inherit from base species
                // First, states in base that are not in this species
                foreach (string k in parent.Keys)
                {
                    if (("FullyFunctional" != k) && (!allOtherStates.ContainsKey(k)))
                    {
                        WorkStates.Add(k, parent[k]);
                    }
                }
                string[] ikeys = new string[allOtherStates.Keys.Count];
                allOtherStates.Keys.CopyTo(ikeys, 0);
                foreach (string s in ikeys) // for each state name
                {
                    StateBody tempState = fullyFunctional.DeepCopy();

                    //Do not inherit capabilities or vulnerabilities of any kind from FF
                    tempState.Vulnerabilities.Clear();
                    tempState.Combinations.Clear();
                    tempState.Capabilities.Clear();
                    // But do inherit from previous state with same name
                    if (parent.ContainsKey(s))
                    {
                        tempState.Vulnerabilities = parent[s].DeepCopyVulnerabilities();
                        tempState.Combinations = parent[s].DeepCopyCombos();
                        tempState.Capabilities = parent[s].DeepCopyCapabilities();
                    }

                    // If current state has any sensors, Current state's sensors replace all previous
                    if (allOtherStates[s].Sensors.Count > 0)
                    {
                        tempState.Sensors.Clear();
                        for (int i = 0; i < allOtherStates[s].Sensors.Count; i++)
                        {
                            tempState.Sensors.Add(allOtherStates[s].Sensors[i]);
                        }
                    }
                    List<string> allStatesKeyList = allOtherStates[s].Parameters.GetKeys();

                    foreach (string p in allStatesKeyList)
                    {
                        if (allOtherStates[s].Parameters[p] == null)
                            continue;
                        switch (allOtherStates[s].Parameters[p].GetType().ToString())
                        {
                            case "System.Double":
                            case "System.String":
                            case "System.Int32":
                            case "System.Boolean":
                                tempState.Parameters[p] = allOtherStates[s].Parameters[p];
                                break;
                            case "Aptima.Asim.DDD.ScenarioController.LocationType":
                                tempState.Parameters[p] = ((LocationType)allOtherStates[s].Parameters[p]).DeepCopy();
                                break;
                            case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                                tempState.Parameters[p] = ((VelocityType)allOtherStates[s].Parameters[p]).DeepCopy();
                                break;
                            case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                                tempState.Parameters[p] = allOtherStates[s].Parameters[p];
                                break;
                        }
                    }
                    foreach (string k in allOtherStates[s].Capabilities.Keys)
                    {
                        tempState.Capabilities[k] = allOtherStates[s].Capabilities[k].DeepCopy();
                    }
                    //Do allow inheritance of vulnerabilities from previous state with same name

                    foreach (string k in allOtherStates[s].Vulnerabilities.Keys)
                    {
                        tempState.Vulnerabilities[k] = allOtherStates[s].Vulnerabilities[k].DeepCopy();
                    }
                    for (int i = 0; i < allOtherStates[s].Combinations.Count; i++)
                    {
                        tempState.Combinations.Add(allOtherStates[s].Combinations[i].DeepCopy());
                    }
                    foreach (string a in allOtherStates[s].Emitters.Keys)
                    {
                        tempState.Emitters[a] = allOtherStates[s].Emitters[a].DeepCopy();
                    }

                    allOtherStates[s] = tempState.DeepCopy();
                }
            }
            if ("" != basedOn) /// I THINK IT NEVER HAPPENS  that basedOn is ""
            {
                if ((!fullyFunctional.Parameters.ContainsKey("Icon"))
                ||
                ("" == (string)fullyFunctional.Parameters["Icon"]))
                {
                    throw new ApplicationException("Icon is required for FullyFunctional state " +
                        "in definition of " + name + " -- based on " + basedOn);
                }
            }
            Dictionary<string, StateBody> returnStates = new Dictionary<string, StateBody>();
            returnStates["FullyFunctional"] = fullyFunctional.DeepCopy();
            // Now make sure all parameters are defined for each non-dead state

            string[] keyList = new string[allOtherStates.Keys.Count];
            allOtherStates.Keys.CopyTo(keyList, 0);
            foreach (string stateName in keyList)
            {
                if ("FullyFunctional" == stateName)
                {
                    continue;
                }
                returnStates.Add(stateName, allOtherStates[stateName].DeepCopy());

                // Inherit from fully functional these things if not already defined:
                if (0 == returnStates[stateName].Capabilities.Count)
                    returnStates[stateName].Capabilities = returnStates["FullyFunctional"].Capabilities;
                if (0 == returnStates[stateName].Sensors.Count)
                    returnStates[stateName].Sensors = returnStates["FullyFunctional"].Sensors;
                if (0 == returnStates[stateName].Combinations.Count)
                    returnStates[stateName].Combinations = returnStates["FullyFunctional"].Combinations;

                /* inheriting vulnerabilities and emitters case-by-case. That means 
                First do a copy from allOtherstates and the add from FF if not defined
              */

                foreach (string vul in returnStates["FullyFunctional"].Vulnerabilities.Keys)
                    if (!(returnStates[stateName].Vulnerabilities.ContainsKey(vul)))
                        returnStates[stateName].Vulnerabilities.Add(vul, returnStates["FullyFunctional"].Vulnerabilities[vul]);
                foreach (string emit in returnStates["FullyFunctional"].Emitters.Keys)
                    if (!(returnStates[stateName].Emitters.ContainsKey(emit)))
                        returnStates[stateName].Emitters.Add(emit, returnStates["FullyFunctional"].Emitters[emit]);




                // first copy the whole state, though don't want the Parameters part
                //so replace the Parameters with the full set from FullyFunctional
                returnStates[stateName].Parameters = fullyFunctional.Parameters.DeepCopy(excludedAttributes);
                List<string> paramList = allOtherStates[stateName].Parameters.GetKeys();
                foreach (string param in paramList)
                {
                    if (allOtherStates[stateName].Parameters[param] == null)
                        continue;
                    switch (allOtherStates[stateName].Parameters[param].GetType().ToString())
                    {
                        case "System.Double":
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            returnStates[stateName].Parameters[param] = allOtherStates[stateName].Parameters[param];
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.LocationType":
                            returnStates[stateName].Parameters[param] = ((LocationType)allOtherStates[stateName].Parameters[param]).DeepCopy();
                            break;
                        case "Aptima.Asim.DDD.ScenarioController.VelocityType":
                            returnStates[stateName].Parameters[param] = ((VelocityType)allOtherStates[stateName].Parameters[param]).DeepCopy();
                            break;
                        case "System.Collections.Generic.List`1[System.String]":
                            returnStates[stateName].Parameters[param] = new List<string>(((List<string>)allOtherStates[stateName].Parameters[param]).ToArray());
                            break;
                        case "Aptima.Asim.DDD.CommonComponents.DataTypeTools.ClassificationDisplayRulesValue":
                            returnStates[stateName].Parameters[param] = allOtherStates[stateName].Parameters[param];
                            break;
                        default:
                            break;
                    }
                }

            }
            return returnStates;
        }
    }

    /// <summary>
    /// All the data about sensors
    /// </summary>
    public class SensorTable
    {
        private static Dictionary<string, SensorType> theTable = new Dictionary<string, SensorType>();
        /// <summary>
        ///  Clears the static sensor table; should be called when server is restarted
        /// </summary>
        public static void Clear()
        {
            theTable = new Dictionary<string, SensorType>();
        }

        /// <summary>
        /// Add a named sensor to the sensor table
        /// </summary>
        /// <param name="name">Name of the sensor</param>
        /// <param name="sensor">Definition of the sensor</param>
        public static void Add(string name, SensorType sensor)
        {
            theTable[name] = sensor.DeepCopy();
        }

        /// <summary>
        /// Tells whether a sensor name is in use
        /// </summary>
        /// <param name="name">Name being checked</param>
        /// <returns></returns>
        public static Boolean IsKnownSensor(string name)
        {
            return theTable.ContainsKey(name);
        }

        /// <summary>
        /// Gets the information about a sensor from the table
        /// </summary>
        /// <param name="name">Name of sensor</param>
        /// <returns></returns>
        public static SensorType Retrieve(string name)
        {
            if (theTable.ContainsKey(name))
            {
                return theTable[name];
            }
            else
            {
                return null;
            }
        }
    }

    //Tracks the namnes of weapons in the simulation
    public class WeaponTable
    {

        static private List<string> weapons = new List<string>();

        /// <summary>
        /// Reports whether a given usnit is a weapon
        /// </summary>
        /// <param name="s">Name of unit</param>
        static public Boolean IsWeapon(string s)
        {
            if (weapons.Contains(s))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears static weapon table; should be called upon server restart
        /// </summary>
        static public void Clear()
        {
            weapons = new List<string>();
        }

        /// <summary>
        /// Adds a weapon name to the table
        /// </summary>
        /// <param name="s">Name of weapon</param>
        static public void Add(string s)
        {
            if (!weapons.Contains(s))
            {
                weapons.Add(s);
            }
        }

    }

    /// <summary>
    /// Tracks membership in tables
    /// </summary>
    public class NetworkTable
    {
        private static Dictionary<string, List<string>> theListTable = new Dictionary<string, List<string>>();
        // dmsInNetworks tracks which units are in some network
        // so that dummy networks can be generated for those that weren't assigned to a unit
        private static List<string> dmsInNetworks = new List<string>();

        /// <summary>
        /// Clears static network table; should be called on server restart
        /// </summary>
        public static void Clear()
        {
            theListTable = new Dictionary<string, List<string>>();
            dmsInNetworks = new List<string>();
        }

        /// <summary>
        /// Reports whether a network name is known
        /// </summary>
        /// <param name="s">Name being investigated</param>
        /// <returns></returns>
        public static Boolean Known(string s)
        {
            return theListTable.ContainsKey(s);
        }

        /// <summary>
        /// Adds a member to a network
        /// </summary>
        /// <param name="ID">ID of network</param>
        /// <param name="member">ID of DM</param>
        public static void AddMember(string ID, string member)
        {
            if (!theListTable.ContainsKey(ID))
            {
                theListTable.Add(ID, new List<string>());
            }
            if (!theListTable[ID].Contains(member))
            {
                theListTable[ID].Add(member);
                if (!dmsInNetworks.Contains(member))
                {
                    dmsInNetworks.Add(member);
                }
            }
        }

        /// <summary>
        /// Adds information about a network to the table
        /// </summary>
        /// <param name="n">Information about network</param>
        public static void AddNetwork(NetworkType n)
        {
            if (!theListTable.ContainsKey(n.Name))
            {
                theListTable.Add(n.Name, new List<string>());
            }
            for (int i = 0; i < n.Count(); i++)
                if (!theListTable[n.Name].Contains(n[i]))
                {
                    if (!DecisionMakerType.IsExistingDM(n[i])) throw new ApplicationException("Network " + n.Name + " contains unknown Decision Maker " + n[i]);
                    AddMember(n.Name, n[i]);
                }
        }

        /// <summary>
        /// Tells whether a dm belongs to any network at all
        /// </summary>
        /// <param name="dm">ID of DM</param>
        /// <returns></returns>
        public static Boolean IsNetworkMember(string dm)
        {
            return dmsInNetworks.Contains(dm);
        }
    }

    /// <summary>
    /// Lists of current subplatform-platform relationships
    /// </summary>
    public class SubplatformRecords
    {
        private static Dictionary<string, List<string>> platformLists = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> parents = new Dictionary<string, string>();

        //In re: RecordDocking, Adam changed this to enforce that only a single subplatform can be associated with a single
        // platform.  This results in the method being changed to a BOOL, indicating the success of the addition to 
        //the platformLists.  If this returns false, the caller should NOT add the subplatform to whatever collection it was 
        //attempting to add to.

        /// <summary>
        /// Record that a unit has docked
        /// </summary>
        /// <param name="parent">Unit docked to</param>
        /// <param name="child">Unit being docked</param>
        public static bool RecordDocking(string parent, string child)
        {
            foreach (string platform in platformLists.Keys)
            {
                if (platformLists[platform] != null)
                {
                    foreach (string subplatform in platformLists[platform])
                    {
                        if (subplatform == child)// && platform != parent)
                        {
                            return false;
                        }
                    }
                }
            }
            if (!platformLists.ContainsKey(parent))
                platformLists.Add(parent, new List<string>());
            if (!platformLists[parent].Contains(child))
                platformLists[parent].Add(child);

            if (!parents.ContainsKey(child))
            {
                parents.Add(child, parent);
            }
            else
            {
                parents[child] = parent;
            }

            return true;
        }

        /// <summary>
        /// Returns the list of current subplatforms of a unit; not recursive
        /// </summary>
        /// <param name="s">ID of  unit</param>
        /// <returns></returns>
        public static List<string> GetDocked(string s)
        {
            if (!platformLists.ContainsKey(s))
                platformLists.Add(s, new List<string>());// but no subplatforms get added by this
            return platformLists[s];
        }

        public static int CountDockedOfSpecies(string parentID, string dockedSpecies)
        {
            int returnValue = 0;
            if (platformLists.ContainsKey(parentID))
            {
                foreach (String subp in platformLists[parentID])
                {
                    if (UnitFacts.GetSpecies(parentID) == dockedSpecies)
                    {
                        returnValue++;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Returns number of subplatforms of a unit (not recursive)
        /// </summary>
        /// <param name="s">ID of unit</param>
        /// <returns></returns>
        public static int CountDocked(string s)
        {
            int returnValue = 0;
            if (platformLists.ContainsKey(s))
                returnValue = platformLists[s].Count;
            return returnValue;
        }

        /// <summary>
        /// Returns  a list of all units docked under the argument, all those docked beneath each of those, etc.; recursive
        /// </summary>
        /// <param name="s">parent unit</param>
        /// <returns></returns>
        public static List<string> DeepDockedList(string s)
        {
            List<string> returnList = new List<string>();
            Stack<string> childstack = new Stack<string>();
            childstack.Push(s);
            while (childstack.Count > 0)
            {
                string parent = childstack.Pop();
                if (!platformLists.ContainsKey(parent)
                    || (0 == platformLists[parent].Count))
                    continue;
                else
                {
                    for (int i = 0; i < platformLists[parent].Count; i++)
                    {
                        string child = platformLists[parent][i];
                        returnList.Add(child);
                        childstack.Push(child);
                    }
                }
            }
            return returnList;

        }

        /// <summary>
        /// Reports whether a unit is docked to a particular other unit
        /// </summary>
        /// <param name="ID of platform being investigated"></param>
        /// <param name="child">ID of unit</param>
        /// <returns></returns>
        public static Boolean IsDocked(string parent, string child)
        {
            Boolean returnValue = false;
            if (platformLists.ContainsKey(parent) && platformLists[parent].Contains(child))
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Records that a unit is no longer docked to a given platform
        /// </summary>
        /// <param name="parent">ID of platform</param>
        /// <param name="child">ID of unit that has been launched</param>
        public static void UnDock(string parent, string child)
        {
            if (platformLists.ContainsKey(parent) && platformLists[parent].Contains(child))
            {
                platformLists[parent].Remove(child);
                parents.Remove(child);
            }
        }

        /// <summary>
        /// Reports whether one unit is a weapon that is a subplatform of another unit
        /// </summary>
        /// <param name="unitID">Platform</param>
        public static Boolean IsLaunchableWeapon(string unitID, string weaponID)
        {
            Boolean returnValue = false;
            if (
                platformLists.ContainsKey(unitID)
                && platformLists[unitID].Contains(weaponID)
                && WeaponTable.IsWeapon(UnitFacts.GetSpecies(weaponID))
                )
                returnValue = true;
            return returnValue;
        }


        /// <summary>
        /// Returns the name of the platform to which a unit is docked
        /// Returns an empty string if child is not a subplatform
        /// <param name="child">Unit being enquired about</param>
        /// </summary>
        public static string GetParent(string child)
        {
            string returnValue = "";
            if (parents.ContainsKey(child))
                returnValue = parents[child];
            return returnValue;
        }

        /// <summary>
        /// Clears static weapons table; should be called before server reset
        /// </summary>
        public static void Clear()
        {
            platformLists = new Dictionary<string, List<string>>();
            parents = new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// A block of information (owner and species)  about an active unit
    /// </summary>
    public class UnitRecord
    {
        private string owner;
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private string species;
        public string Species
        {
            get { return species; }
        }
        public UnitRecord(string dMID, string species)
        {
            owner = dMID;
            this.species = species;

        }
    }

    /// <summary>
    /// Dictionary associating unit ID with the data known about the unit
    /// </summary>
    public class UnitFacts
    {
        /*Unclear what the purpose of this is/was
        private static bool UnlimitedAssetTransfer = false;
        TODO:get,set
        */
        private static Dictionary<string, UnitRecord> data = new Dictionary<string, UnitRecord>();
        public static Dictionary<string, UnitRecord> Data
        {
            get { return data; }
            set { data = value; }
        }
        /// <summary>
        /// Returns a list of names of all live units
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUnits()
        {
            List<string> returnValue = new List<string>();
            foreach (string unitName in data.Keys)
            {
                returnValue.Add(unitName);
            }
            return returnValue;
        }

        /// <summary>
        /// Reprorts whether a name is the ID of an active unit
        /// </summary>
        /// <param name="s">Name being investigated</param>
        public static Boolean IsAUnit(string s)
        {
            return data.ContainsKey(s);
        }

        /// <summary>
        /// Removes the data about a unit
        /// </summary>
        /// <param name="unit"></param>
        /// 
        public static void RemoveUnit(string unit)
        {

            int position = dMAssetsTable[data[unit].Owner].IndexOf(unit);
            if (position > -1)
            {
                dMAssetsTable[data[unit].Owner].RemoveAt(position);
            }
            data.Remove(unit);
        }

        /// <summary>
        /// Adds a unit and the data about it to the tables
        /// </summary>
        /// <param name="unit">ID of unit to add</param>
        /// <param name="dMID">ID of DM owning unit</param>
        /// <param name="species">The species of the unit</param>
        public static void AddUnit(string unit, string dMID, string species)
        {
            if (!DecisionMakerType.IsExistingDM(dMID)) throw new ApplicationException("Definition of " + unit + " references unknown Decision Maker" + dMID);
            data[unit] = new UnitRecord(dMID, species);
            //  AddDM(dMID);
            dMAssetsTable[dMID].Add(unit);
        }

        private static Dictionary<string, string> currentUnitStates = new Dictionary<string, string>();
        public static Dictionary<string, string> CurrentUnitStates
        {
            get { return currentUnitStates; }
            set { currentUnitStates = value; }
        }

        /// <summary>
        /// Reports whether any of the names on a list is dead
        /// </summary>
        /// <param name="sL">List being interrogated</param>
        /// <returns></returns>
        public static Boolean AnyDead(List<string> sL)
        {
            int i;
            int j = -1;
            try
            {
                for (i = 0; i < sL.Count; i++)
                {
                    j = i; // just to capture unit for error messages
                    if ("Dead" == CurrentUnitStates[sL[i]])
                    {
                        return true;
                    }
                }
            }
            catch (SystemException e)
            {
                throw new ApplicationException("Could not find unit " + sL[j] + " in AnyDead:", e);
            }
            return false;
        }

        /// <summary>
        /// Reports the name of the species of a unit
        /// </summary>
        /// <param name="unit">Name of unit</param>
        public static string GetSpecies(string unit)
        {
            if (data.ContainsKey(unit))
            {
                return data[unit].Species;
            }
            return "";
        }

        /// <summary>
        /// Reports the name of the owning DM of a unit
        /// </summary>
        /// <param name="unit">Name of unit</param>
        public static string GetDM(string unit)
        {
            if (data.ContainsKey(unit))
            {
                return data[unit].Owner;
            }
            return "";
        }

        /// <summary>
        /// Changes the recorded ownership of a unit
        /// </summary>
        /// <param name="unit">Name of unit</param>
        /// <param name="to">DM gaining unit</param>
        /// <returns></returns>
        public static Boolean ChangeDM(string unit,/* string from,*/ string to)
        {
            Boolean returnValue = false;
            string owner = GetDM(unit);
            if ("" != owner)
            {
                if ((SpeciesType.GetSpecies(UnitFacts.Data[unit].Species).CanOwnMe(to)))
                {
                    if (!DMAssetsTable.ContainsKey(to))
                        DMAssetsTable[to] = new List<string>();
                    data[unit].Owner = to;
                    DMAssetsTable[owner].Remove(unit);
                    DMAssetsTable[to].Add(unit);
                    returnValue = true;
                }
            }

            return returnValue;
        }


        /// <summary>
        /// Static table of which teams are enemies of which others
        /// </summary>
        private static Dictionary<string, List<string>> enemies = new Dictionary<string, List<string>>();
        public static Boolean HostileTo(string actorTeam, string targetTeam)
        {
            try
            {
                return enemies[actorTeam].Contains(targetTeam);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unknown team '" + actorTeam + "':"+e.Message);
            }
        }
        /// <summary>
        /// Add a hostility record (Note: these are not necessarily symmetric
        /// </summary>
        /// <param name="team">ID of team shoing hostility</param>
        /// <param name="opponentTeam">ID of team it is hostile to</param>
        public static void AddEnemy(string team, string opponentTeam)
        {
            if (!enemies.ContainsKey(team))
            {
                enemies.Add(team, new List<string>());
            }
            if (!enemies[team].Contains(opponentTeam))
            {
                enemies[team].Add(opponentTeam);
            }
        }

        /// <summary>
        /// Static table to track ownership of units by DMs
        /// </summary>
        private static Dictionary<string, List<string>> dMAssetsTable = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> DMAssetsTable
        {
            get { return dMAssetsTable; }
            set { dMAssetsTable = value; }
        }

        /// <summary>
        /// Reports whether a name belongs to a DM
        /// </summary>
        /// <param name="player">Name being examined</param>
        /// <returns></returns>
        public static Boolean IsDM(string player)
        {
            return DecisionMakerType.IsExistingDM(player);
        }


        /// <summary>
        /// Static table of colors associated with DM names
        /// </summary>
        private static Dictionary<string, int> dMColors = new Dictionary<string, int>();

        /// <summary>
        /// Static table showing membership of teams
        /// </summary>
        private static Dictionary<string, string> dMTeams = new Dictionary<string, string>();
        public static Dictionary<string, string> DMTeams
        {
            get { return dMTeams; }
        }
      
        /// <summary>
        /// Static table showing who is on each team
        /// </summary>
        private static Dictionary<string, List<string>> teamDMs = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> TeamDMs
        {
            get { return teamDMs; }
        }
        /// <summary>
        /// Reports whether two DMs are on the same team
        /// </summary>
        /// <param name="dM1">ID of one of the DMs</param>
        /// <param name="dM2">ID of the other DM</param>
        /// <returns></returns>
        public static Boolean SameTeam(string dM1, string dM2)
        {
            // We allow a DM to not have a team membership
            if ((!dMTeams.ContainsKey(dM1)) || (!dMTeams.ContainsKey(dM2)))
                return true;
            return (dMTeams[dM1] == dMTeams[dM2]);

        }
        /// <summary>
        /// Adds a Dm and information about it to the tables
        /// </summary>
        /// <param name="dm">ID of DM being added</param>
        /// <param name="color">Name of its color</param>
        /// <param name="team">Name of its team</param>
        public static void AddDM(string dm, int color, string team)
        {
            if (dMAssetsTable.ContainsKey(dm))
            {
                throw new ApplicationException("Attempt to reuse Decision Maker " + dm);
            }
            if (!NameLists.teamNames.ContainsKey(team)) throw new ApplicationException("Definition of DM " + dm + " references unknown team " + team);
            if (dMTeams.ContainsKey(dm))
            {
                throw new ApplicationException("Attempt to reassign team for  Decision Maker " + dm);
            }

            dMAssetsTable.Add(dm, new List<string>());
            dMColors.Add(dm, color);
            dMTeams.Add(dm, team);
            if (!teamDMs.ContainsKey(team))
                teamDMs[team] = new List<string>();
            teamDMs[team].Add(dm);
            if (!enemies.ContainsKey(team))
            {
                enemies.Add(team, new List<string>());
            }
        }

        /// <summary>
        /// Clear static tables; should be called before server restart
        /// </summary>
        public static void ClearDMTables()
        {
            dMAssetsTable.Clear();
            dMColors.Clear();
            dMTeams.Clear();
            enemies.Clear();
        }

        /// <summary>
        /// Returns a list of all DMs
        /// </summary>
        public static List<string> GetAllDms()
        {
            List<string> dmList = new List<string>();
            foreach (string k in dMAssetsTable.Keys)
            {
                dmList.Add(k);
            }
            return dmList;
        }
    }

    /// <summary>
    ///  Tracks membership of one chat room
    /// </summary>
    public class OneRoom
    {
        private string owner;
        public string Owner
        {
            get { return owner; }
        }
        private List<string> members;
        public List<string> Members
        {
            get { return members; }
        }

        public OneRoom(string owner, List<string> members)
        {
            this.owner = owner;
            this.members = members;
        }
    }

    /// <summary>
    /// Tracks chat rooms
    /// </summary>
    public class ChatRooms
    {

        private static Dictionary<string, OneRoom> ChatRoomBlock = new Dictionary<string, OneRoom>();
        public static Boolean IsRoom(string s)
        {
            return ChatRoomBlock.ContainsKey(s);
        }
        public static string IsOwner(string owner, string room)
        {
            // note this may be called before AddRoom; so be careful about indexing
            string returnValue = "";
            if (!ChatRoomBlock.ContainsKey(room))
                returnValue = "No such chatroom " + room;
            else if (ChatRoomBlock[room].Owner != owner)
            {
                returnValue = "ChatRoom " + room + "is not owned by " + owner;
            }
            return returnValue;
        }
        public static string AddRoom(OpenChatRoomType c)
        {
            string returnValue = "";
            if (ChatRoomBlock.ContainsKey(c.Room))
            {
                returnValue = "Duplicate ChatRoom Name";
            }
            else
            {
                ChatRoomBlock[c.Room] = new OneRoom(c.Owner, c.Members);
            }

            // send event to chatserver here
            return returnValue;
        }

        public static List<string> GetChatMembers(string roomName)
        {
            List<string> returnList = new List<string>();
            if (ChatRoomBlock.ContainsKey(roomName))
            {
                returnList = ChatRoomBlock[roomName].Members;
            }
            return returnList;
        }
        public static void AddChatMember(string roomName, string dM)
        {// called after membership was deemed to be allowed
            if (!ChatRoomBlock[roomName].Members.Contains(dM))
                ChatRoomBlock[roomName].Members.Add(dM);
        }

        public static void DropAllRooms()
        {
            //Only call if  the chat server is being reset
            ChatRoomBlock = new Dictionary<string, OneRoom>();
        }
        public static Boolean DropChatRoom(string room)
        { 
            Boolean returnValue=false;
        if(ChatRoomBlock.ContainsKey(room))
        {
            returnValue=true;
            ChatRoomBlock.Remove(room);
        }
            return returnValue;
        }

   
    }
        /* Not ready to be implemented
public static string DropRoom(CloseChatRoomType c)
{
    string returnValue = ChatRooms.IsOwner(c.Owner, c.Room);
    if ("" != returnValue) ChatRoomBlock.Remove(c.Room);
    return returnValue;
}

public static string AddChatters(AddChattersType a)
{
    string returnValue = ChatRooms.IsOwner(a.Owner, a.Room);
    if ("" != returnValue)
    {
        for (int i = 0; i < a.Members.Count; i++)
        {
            if (!UnitFacts.IsADM(a.Members[i]))
            {
                throw new ApplicationException("Attempted to add non-DM " + a.Members[i] + "to chatroom " + a.Room);
            }
            else
            {
                if (!ChatRoomBlock[a.Room].Members.Contains(a.Owner))
                {
                    ChatRoomBlock[a.Room].Members.Add(a.Members[i]);
                }
            }
        }
        returnValue = "";
    }
    return returnValue;
}
public static string DropChatters(DropChattersType d)
{
    string returnValue = ChatRooms.IsOwner(d.Owner, d.Room);
    if ("" != returnValue)
    {
        for (int i = 0; i < d.Members.Count; i++)
        {
            if (!UnitFacts.IsADM(d.Members[i]))
            {
                throw new ApplicationException("Attempted to remove non-DM " + d.Members[i] + "from chatroom " + d.Room);
            }
            else
            {
                if (ChatRoomBlock[d.Room].Members.Contains(d.Owner))
                {
                    ChatRoomBlock[d.Room].Members.Remove(d.Members[i]);
                }
            }
        }
        returnValue = "";
    }
    return returnValue;
}



           /// <summary>
    ///  Tracks membership of one voice channel
    /// </summary>
    public class OneChannel
    {
        private string owner;
        public string Owner
        {
            get { return owner; }
        }
  

        public OneChannel(string owner)
        {
            this.owner = owner;
  
        }
    }
*/

    /// <summary>
    /// Tracks whiteboard rooms
    /// </summary>
    public class WhiteboardRooms
    {

        private static Dictionary<string, OneRoom> WhiteboardRoomBlock = new Dictionary<string, OneRoom>();
        public static Boolean IsRoom(string s)
        {
            return WhiteboardRoomBlock.ContainsKey(s);
        }
        public static string IsOwner(string owner, string room)
        {
            // note this may be called before AddRoom; so be careful about indexing
            string returnValue = "";
            if (!WhiteboardRoomBlock.ContainsKey(room))
                returnValue = "No such whiteboardroom " + room;
            else if (WhiteboardRoomBlock[room].Owner != owner)
            {
                returnValue = "WhiteboardRoom " + room + "is not owned by " + owner;
            }
            return returnValue;
        }
        public static string AddRoom(OpenWhiteboardRoomType c)
        {
            string returnValue = "";
            if (WhiteboardRoomBlock.ContainsKey(c.Room))
            {
                returnValue = "Duplicate WhiteboardRoom Name";
            }
            else
            {
                WhiteboardRoomBlock[c.Room] = new OneRoom(c.Owner, c.Members);
            }

            // send event to whiteboardserver here
            return returnValue;
        }

        public static List<string> GetWhiteboardMembers(string roomName)
        {
            List<string> returnList = new List<string>();
            if (WhiteboardRoomBlock.ContainsKey(roomName))
            {
                returnList = WhiteboardRoomBlock[roomName].Members;
            }
            return returnList;
        }
        public static void AddWhiteboardMember(string roomName, string dM)
        {// called after membership was deemed to be allowed
            if (!WhiteboardRoomBlock[roomName].Members.Contains(dM))
                WhiteboardRoomBlock[roomName].Members.Add(dM);
        }

        public static void DropAllRooms()
        {
            //Only call if  the whiteboard server is being reset
            WhiteboardRoomBlock = new Dictionary<string, OneRoom>();
        }
        public static Boolean DropWhiteboardRoom(string room)
        {
            Boolean returnValue = false;
            if (WhiteboardRoomBlock.ContainsKey(room))
            {
                returnValue = true;
                WhiteboardRoomBlock.Remove(room);
            }
            return returnValue;
        }


    }

    /// <summary>
    /// Tracks voice channels
    /// </summary>
    public class VoiceChannels
    {

       // private static Dictionary<string, OneChannel> VoiceChannelSpectrum = new Dictionary<string, OneChannel>();
        private static List<string> VoiceChannelSpectrum = new List<string>();
       /*
        //deprecated owner member
        public static string IsOwner(string owner, string channel)
        {
            // note this may be called before AddChannel; so be careful about indexing
            string returnValue = "";
            if (!VoiceChannelSpectrum.ContainsKey(channel))
                returnValue = "No such channel " + channel;
            else if (VoiceChannelSpectrum[channel].Owner != owner)
            {
                returnValue = "Voice Channel " + channel + "is not owned by " + owner;
            }
            return returnValue;
        }
        */
        public static string AddChannel(OpenVoiceChannelType c)
        {
            string returnValue = "";
            if (VoiceChannelSpectrum.Contains(c.Channel))
            {
                returnValue = "Duplicate Voice channel Name";
            }
            else
            {
                VoiceChannelSpectrum.Add(c.Channel);//] = new OneChannel(c.Owner);
            }

            return returnValue;
        }

        public static Boolean IsOpen(string s)
        {
            return VoiceChannelSpectrum.Contains(s);
        }

        public static void DropAllChannels()
        {
            //Only call if  the chat server is being reset
            VoiceChannelSpectrum = new List<string>();
        }
        public static Boolean DropVoiceChannel(string channel)
        {
            Boolean returnValue = false;
            if (VoiceChannelSpectrum.Contains(channel))
            {
                returnValue = true;
                VoiceChannelSpectrum.Remove(channel);
            }
            return returnValue;
        }

    }

}
