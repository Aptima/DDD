using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using AME.Model;
using System.Xml;
using System.Security.Cryptography;
using System.Xml.XPath;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AME.Controllers.Base.Data_Structures;
using VSG.ConfigFile;
using AME.Views.View_Components.CoordinateTransform;

namespace VSG.Controllers
{
    public class VSGController : Controller
    {
        private String currentMapLocation;
        private String currentIconLibraryLocation;
        private Image currentMap;
        private ImageList currentIconLibrary = new ImageList();

        private Int32 scenarioId = -1;
        private String IDAttribute;
        private String NameAttribute;
        private ICoordinateTransform m_coordinateTransformer = new IdentityCoordinateTransform(); // use identity as default
        public VSGController(AME.Model.Model model, String configType)
            : base(model, configType)
        {
            IDAttribute = XmlSchemaConstants.Display.Component.Id;
            NameAttribute = XmlSchemaConstants.Display.Component.Name;
        }

        public Int32 ScenarioId
        {
            get
            {
                return scenarioId;
            }
            set
            {
                scenarioId = value;
            }
        }

        public String CurrentMapLocation
        {
            get { return currentMapLocation; }
            set 
            { 
                currentMapLocation = value;
                //if (value != String.Empty)
                //{
                //    VSGConfig.MapDir = System.IO.Path.GetDirectoryName(value);
                //    VSGConfig.WriteFile();
                //}
            }
        }

        public String CurrentIconLibraryLocation
        {
            get
            {
                return currentIconLibraryLocation;
            }
            set 
            { 
                currentIconLibraryLocation = value;
                //if (value != String.Empty)
                //{
                //    VSGConfig.IconDir = System.IO.Path.GetDirectoryName(value);
                //    VSGConfig.WriteFile();
                //}
                
            }
        }

        public ICoordinateTransform CoordinateTransform
        {
            get
            {
                return m_coordinateTransformer;
            }
            set
            {
                m_coordinateTransformer = value;
            }
        }

        public int GetComponentID(String componentName)
        {
            DataTable dt = this.GetComponentTable();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["name"].ToString() == componentName)
                {
                    return Convert.ToInt32(dr["id"]);
                }
            }

            return -1;
        }

        public String GetComponentName(int componentID)
        {
            if (componentID >= 0)
            {
                IXPathNavigable inav = this.GetComponent(componentID);
                if (inav != null)
                {
                    XPathNavigator nav = inav.CreateNavigator();
                    if (nav != null)
                    {
                        XPathNavigator node = nav.SelectSingleNode(String.Format("Components/Component[@ID='{0}']", componentID));
                        if (node != null)
                        {
                            return node.GetAttribute("Name", String.Empty);
                        }
                    }
                }
            }
            //return .CreateNavigator().SelectSingleNode(String.Format("Components/Component[@ID='{0}']", componentID)).GetAttribute("Name", String.Empty);

            return String.Empty;
        }

        public String GetComponentType(int componentID)
        {
            if (componentID >= 0)
            {
                IXPathNavigable inav = this.GetComponent(componentID);
                if (inav != null)
                {
                    XPathNavigator nav = inav.CreateNavigator();
                    if (nav != null)
                    {
                        XPathNavigator node = nav.SelectSingleNode(String.Format("Components/Component[@ID='{0}']", componentID));
                        if (node != null)
                        {
                            return node.GetAttribute("Type", String.Empty);
                        }
                    }
                }
            }
            //return .CreateNavigator().SelectSingleNode(String.Format("Components/Component[@ID='{0}']", componentID)).GetAttribute("Name", String.Empty);

            return String.Empty;
        }
   
        public int GetSpeciesBase(int speciesID)
        {
            List<int> ids = GetChildIDs(speciesID, "Species", "SpeciesType"); //String.Format("SpeciesType_{0}", speciesID));

            if (ids.Count > 0)
            {
                return ids[0];
            }
            else
            {
                return -1;
            }
        }

        public Image CurrentMap
        {
            get
            {
                return currentMap;
            }
            set
            {
                currentMap = value;
            }
        }
        public List<String> CurrentIconLibraryIconNames()
        {
            List<String> names = new List<string>();
            foreach(String i in currentIconLibrary.Images.Keys)
            {
                names.Add(i);
            }

            return names;
        }
        public ImageList CurrentIconLibrary
        {
            get
            {
                return currentIconLibrary;
            }
            set
            {
                currentIconLibrary = value;
            }
        }

        /// <summary>
        /// Initialize all VSGController properties to their default states.
        /// </summary>
        public void Initialize()
        {
            currentMapLocation = String.Empty;
            currentIconLibraryLocation = String.Empty;
            currentMap = null;
            currentIconLibrary = new ImageList();
        }

        public bool IsLinked(int comp1, int comp2, String linkType)
        {
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 1;
            // Select the items that are connected to the destination
            IXPathNavigable document = this.GetComponentAndChildren(comp1, linkType, compOptions);
            XPathNavigator navigator = document.CreateNavigator();
            XPathNodeIterator listofRootElements;
            listofRootElements = navigator.Select(String.Format("/Components/Component/Component"));
            foreach (XPathNavigator paramNav in listofRootElements)
            {
                String nodeName;
                int nodeID;
                nodeName = paramNav.GetAttribute(NameAttribute, paramNav.NamespaceURI);
                nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));
                if (nodeID == comp2)
                {
                    return true;
                }
            }
            return false;
        }

        public List<int> GetChildIDs(int compID, String childCompType, String linkType)
        {
            List<int> r = new List<int>();
            ComponentOptions op = new ComponentOptions();
            op.LevelDown = 1;
            IXPathNavigable inav = this.GetComponentAndChildren(compID, linkType, op);
            XPathNavigator nav = inav.CreateNavigator();

            XPathNodeIterator listofRootElements;
            listofRootElements = nav.Select(String.Format(
                        "/Components/Component/Component[@Type='{0}' or @BaseType='{0}']",
                        childCompType
                        ));

            if (listofRootElements != null)
            {
                int nodeID;

                foreach (XPathNavigator paramNav in listofRootElements)
                {
                    nodeID = Int32.Parse(paramNav.GetAttribute(IDAttribute, paramNav.NamespaceURI));
                    r.Add(nodeID);
                }
            }

            return r;
        }

        /// <summary>
        /// Get the parent containing this child using a specific link id.
        /// </summary>
        /// <param name="linkId">The link id used to get the parent.</param>
        /// <returns>Parent component id.</returns>
        public Int32 GetParentFromLink(Int32 linkId)
        {
            DataRow row = this._GetLink(linkId);
            return row != null ? (Int32)row["fromComponentId"] : -1;
        }

        public Int32 GetSpeciesOfSingleton(Int32 singletonId)
        {
            DataTable table = this.m_model.GetParentComponents(singletonId);

            Int32 speciesId = -1;
            foreach (DataRow row in table.Rows)
            {
                if (row["type"].ToString().Equals("State"))
                {
                    Int32 stateId = (Int32)row["id"];
                    speciesId = GetSpeciesOfState(stateId);
                    break; // Should only be one of these!
                }
            }
            return speciesId;
        }
        public Int32 GetSpeciesOfCombo(Int32 comboId)
        {
            DataTable table = this.m_model.GetParentComponents(comboId);

            Int32 speciesId = -1;
            foreach (DataRow row in table.Rows)
            {
                if (row["type"].ToString().Equals("State"))
                {
                    Int32 stateId = (Int32)row["id"];       
                    speciesId = GetSpeciesOfState(stateId);
                    break; // Should only be one of these!
                }
            }
            return speciesId;
        }

        public Int32 GetSpeciesOfState(Int32 stateId)
        {
            DataTable table = this.m_model.GetParentComponents(stateId);

            Int32 speciesId = -1;
            foreach (DataRow row in table.Rows)
            {
                if (row["type"].ToString().Equals("Species"))
                {
                    speciesId = (Int32)row["id"];
                    break; // Should only be one of these!
                }
            }
            return speciesId;
        }

        public enum VSGEvents
        {
            CreateEvent
        };

        public enum VSG
        {
            DecisionMaker
        };

        public enum LinkTypes
        {
            Scenario, EventID
        };

        /// <summary>
        /// Clones a Create Event in the Event Tree.
        /// <para>
        /// The intent was to make this re-usable, which didn't really work out.
        /// </para>
        /// <para>
        /// As a result the method carries around some IDs and things as baggage for recursion.
        /// </para>
        /// <para>
        /// Arguments should be something like rootID of the tree, CreateEventID, name to use, "Scenario", list with "DecisionMaker",
        /// rootID of tree, "Scenario", -1, -1
        /// </para>
        /// <para>
        /// The new CreateEvent receives as its owner the DecisionMaker of the passed in CreateEvent
        /// </para>
        /// <para>
        /// Cloned events receive as their owner CreateEvent either the newly cloned CreateEvent, if they referenced the original one OR
        /// existing CreateEvents.
        /// </para>
        /// <para>
        /// All other links from the original CreateEvent and other events in the Scenario tree are copied
        /// by %Event in Tree% --> %Something the Event linked to by some link type%.  
        /// </para>
        /// <para>
        /// The %Something the Event linked to% is not cloned or copied,
        /// only a link is formed
        /// </para>
        /// </summary>
        /// <param name="rootID"></param>
        /// <param name="startingComponentID"></param>
        /// <param name="followThisLinkType"></param>
        /// <param name="ignoreTheseTypes"></param>
        public ComponentAndLinkID EventClone(int rootID,
                                             int startingComponentID,
                                             String startingName,
                                             String followThisLinkType,
                                             List<String> doNotCreateTheseTypes,
                                             int cloneAttachToID,
                                             String cloneAttachLinkType,
                                             int originalCreateEventID,
                                             int clonedCreateEventID)
        {
            DataRow component = this._GetComponent(startingComponentID);

            String componentDescription = component[SchemaConstants.Description].ToString();
            String componentType = component[SchemaConstants.Type].ToString();
            Component.eComponentType eType = this._GetComponentType(component);

            int cloneID = -1;
            ComponentAndLinkID fromClone = null;

            if (!doNotCreateTheseTypes.Contains(componentType))
            {
                // clone the starting componentID
                fromClone = this._Clone(rootID, cloneAttachToID, startingComponentID, componentType, startingName,
                                            componentDescription, eType, null, cloneAttachLinkType);

                cloneID = fromClone.ComponentID;

                if (componentType.Equals(VSGEvents.CreateEvent.ToString()))
                {
                    originalCreateEventID = startingComponentID; // save original CreateEvent and Clone
                    clonedCreateEventID = cloneID;
                }
            }

            // follow passed in linktype, clone children
            DataTable children = this.m_model.GetChildComponents(startingComponentID, followThisLinkType);

            foreach (DataRow child in children.Rows)
            {
                int childID = Int32.Parse(child[SchemaConstants.Id].ToString());
                String childName = child[SchemaConstants.Name].ToString();
                String childType = child[SchemaConstants.Type].ToString();

                if (!doNotCreateTheseTypes.Contains(childType))
                {
                    if (cloneID != -1)
                    {
                        // follow and clone each child, attach to the newly created clone off starting above
                        ComponentAndLinkID childCloned = this.EventClone(rootID, 
                                                                         childID, 
                                                                         childName, 
                                                                         followThisLinkType, 
                                                                         doNotCreateTheseTypes,
                                                                         cloneID, 
                                                                         cloneAttachLinkType,
                                                                         originalCreateEventID,
                                                                         clonedCreateEventID
                                                                        );
                    }
                } // do not create
            }

            // special links between cloneID source and uncloned child (e.g. references to things outside of linktype 'scenario'
            if (cloneID != -1)
            {
                // follows ALL linktypes
                DataTable allChildComponents = this.m_model.GetChildComponents(startingComponentID);

                foreach (DataRow aChildFromAll in allChildComponents.Rows)
                {
                    String aChildFromAllType = aChildFromAll[SchemaConstants.Type].ToString();
                    int aChildFromAllID = Int32.Parse(aChildFromAll[SchemaConstants.Id].ToString());
                    int aChildFromAllLinkID = Int32.Parse(aChildFromAll[SchemaConstants.LinkID].ToString());

                    DataTable linkTable = this.m_model.GetLink(aChildFromAllLinkID);

                    if (linkTable.Rows.Count == 1)
                    {
                        String linkType = linkTable.Rows[0][SchemaConstants.Type].ToString();
                        // CreateEvent to DecisionMaker, Scenario
                        if (componentType.Equals(VSGEvents.CreateEvent.ToString()) && aChildFromAllType.Equals(VSG.DecisionMaker.ToString()))
                        {
                            this.Connect(rootID, cloneID, aChildFromAllID, LinkTypes.Scenario.ToString());

                        }
                        // Event to CreateEvent, EventID // is component an event?
                        else if (linkType.Equals(LinkTypes.EventID.ToString()) &&
                                    aChildFromAllType.Equals(VSGEvents.CreateEvent.ToString()) &&
                                        aChildFromAllID == originalCreateEventID)
                        {
                            // replace with CLONED CreateEvent, not the original
                            if (clonedCreateEventID != -1)
                            {
                                this.Connect(cloneID, cloneID, clonedCreateEventID, LinkTypes.EventID.ToString());
                            }
                        }
                        else
                        {
                            // all other links
                            if (!linkType.Equals(LinkTypes.Scenario.ToString()))
                            {
                                this.Connect(cloneID, cloneID, aChildFromAllID, linkType);
                            }
                        }
                    }
                }
            }
            return fromClone;
        } // method
    }
}




            


