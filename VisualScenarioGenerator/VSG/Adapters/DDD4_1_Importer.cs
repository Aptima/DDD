using System;
using System.Collections.Generic;
using System.Text;
using AME.Adapters;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Windows.Forms;
using Saxon.Api;
using System.IO;
using AME.Controllers;
using VSG.Helpers;

namespace VSG.Adapters
{
    class DDD4_1_Importer : ValidationHelper, IImportAdapter
    {
        XPathExpression exprEvents;

//        private const String ProjectConfiguration = "Project";
        private const String VSGConfiguration = "VSG";

        public DDD4_1_Importer(IController validatingController) : base(validatingController)
        {

        }

        #region IImportAdapter Members

        public IXPathNavigable Process(string uriSource)
        {
            IXPathNavigable iNav;
            if (validate(uriSource, out iNav))
            {
                XPathNavigator navigator = iNav.CreateNavigator();
                // Build the xml file
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                document.AppendChild(declaration);
                XmlElement root = document.CreateElement("database");

                Dictionary<String, Boolean> configurations = new Dictionary<String, Boolean>();
                //configurations.Add(ProjectConfiguration, true);
                configurations.Add(VSGConfiguration, false);
                foreach (String configuration in configurations.Keys)
                {
                    XmlElement config = document.CreateElement("configuration");
                    XmlAttribute configName = document.CreateAttribute("name");
                    configName.Value = configuration;
                    XmlAttribute configIsRootContoller = document.CreateAttribute("isRootController");
                    configIsRootContoller.Value = configurations[configuration].ToString().ToLower();
                    config.Attributes.Append(configName);
                    config.Attributes.Append(configIsRootContoller);

                    XmlElement componentTable = document.CreateElement("componentTable");
                    config.AppendChild(componentTable);
                    XmlElement linkTable = document.CreateElement("linkTable");
                    config.AppendChild(linkTable);
                    XmlElement parameterTable = document.CreateElement("parameterTable");
                    config.AppendChild(parameterTable);

                    root.AppendChild(config);
                }

                document.AppendChild(root);

                createScenario(navigator, document);
                return document;
            }
            else
                return iNav;
        }

        #endregion

        private void createComponent(String configuration, XmlDocument document, String type, String name, String description)
        {
            // Create component
            XmlElement component = document.CreateElement("component");
            
            XmlAttribute aType = document.CreateAttribute("type");
            aType.Value = type;
            component.Attributes.Append(aType);
            
            XmlAttribute aName = document.CreateAttribute("name");
            aName.Value = name;
            component.Attributes.Append(aName);
            
            XmlAttribute aDescription = document.CreateAttribute("description");
            aDescription.Value = description;
            component.Attributes.Append(aDescription);

            document.SelectSingleNode(String.Format("database/configuration[@name='{0}']/componentTable", configuration)).AppendChild(component);
        }

        private void createComponent(String configuration, XmlDocument document, String type, String name, String description, Boolean root)
        {
            // Create component
            XmlElement component = document.CreateElement("component");

            XmlAttribute aRoot = document.CreateAttribute("root");
            aRoot.Value = XmlConvert.ToString(root);
            component.Attributes.Append(aRoot);

            XmlAttribute aType = document.CreateAttribute("type");
            aType.Value = type;
            component.Attributes.Append(aType);

            XmlAttribute aName = document.CreateAttribute("name");
            aName.Value = name;
            component.Attributes.Append(aName);

            XmlAttribute aDescription = document.CreateAttribute("description");
            aDescription.Value = description;
            component.Attributes.Append(aDescription);

            document.SelectSingleNode(String.Format("database/configuration[@name='{0}']/componentTable", configuration)).AppendChild(component);
        }

        private void createLink(String configuration, XmlDocument document, String rootComponentId, String fromComponentId, String toComponentId, String type, String description)
        {
            // Create Link
            XmlElement link = document.CreateElement("link");
            
            XmlAttribute aRootComponentId = document.CreateAttribute("rootComponentId");
            aRootComponentId.Value = rootComponentId;
            link.Attributes.Append(aRootComponentId);

            XmlAttribute aFromComponentId = document.CreateAttribute("fromComponentId");
            aFromComponentId.Value = fromComponentId;
            link.Attributes.Append(aFromComponentId);

            XmlAttribute aToComponentId = document.CreateAttribute("toComponentId");
            aToComponentId.Value = toComponentId;
            link.Attributes.Append(aToComponentId);

            XmlAttribute aType = document.CreateAttribute("type");
            aType.Value = type;
            link.Attributes.Append(aType);

            XmlAttribute aDescription = document.CreateAttribute("description");
            aDescription.Value = description;
            link.Attributes.Append(aDescription);

            document.SelectSingleNode(String.Format("database/configuration[@name='{0}']/linkTable", configuration)).AppendChild(link);
        }

        private void createParameter(String configuration, XmlDocument document, String parentId, String parentType, String name, String value, String description)
        {
            if (!value.Equals(String.Empty))
            {
                // Create Parameter
                XmlElement parameter = document.CreateElement("parameter");

                XmlAttribute aParentId = document.CreateAttribute("parentId");
                aParentId.Value = parentId;
                parameter.Attributes.Append(aParentId);

                XmlAttribute aParentType = document.CreateAttribute("parentType");
                aParentType.Value = parentType;
                parameter.Attributes.Append(aParentType);

                XmlAttribute aName = document.CreateAttribute("name");
                aName.Value = name;
                parameter.Attributes.Append(aName);

                XmlAttribute aValue = document.CreateAttribute("value");
                aValue.Value = value;
                parameter.Attributes.Append(aValue);

                XmlAttribute aDescription = document.CreateAttribute("description");
                aDescription.Value = description;
                parameter.Attributes.Append(aDescription);

                document.SelectSingleNode(String.Format("database/configuration[@name='{0}']/parameterTable", configuration)).AppendChild(parameter);
            }
        }

        private void createScenario(XPathNavigator navigator, XmlDocument document)
        {
            // Could move this to its own function if we start to use more.
            exprEvents = navigator.Compile("ChangeEngram | " +
                                           "CloseChatRoom | " +
                                           "CloseVoiceChannel | " +
                                           "SendChatMessage | " +
                                           "SendVoiceMessage | " +
                                           "SendVoiceMessageToUser | " +
                                           "Completion_Event | " +
                                           "Create_Event | " +
                                           "DefineEngram | " +
                                           "FlushEvents | " +
                                           "Launch_Event | " +
                                           "WeaponLaunch_Event | " +
                                           "Move_Event | " +
                                           "OpenChatRoom | " +
                                           "OpenVoiceChannel | " +
                                           "Reiterate | " +
                                           "RemoveEngram | " +
                                           "Reveal_Event | " +
                                           "Species_Completion_Event | " +
                                           "StateChange_Event | " +
                                           "Transfer_Event");

            // Set up component values
            String cType = "Scenario";
            String cName = "Scenario";
            XPathNavigator navScenario = navigator.SelectSingleNode("/Scenario");
            XPathNavigator navScenarioName = navigator.SelectSingleNode("/Scenario/ScenarioName");
            if (navScenarioName != null)
            {
                cName = navScenarioName.Value;
            }
            navScenario.CreateAttribute(String.Empty, "name", navScenario.NamespaceURI, cName);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription, true);

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Scenario Name", (navigator.SelectSingleNode("Scenario/ScenarioName") == null) ? String.Empty : navigator.SelectSingleNode("Scenario/ScenarioName").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Description", (navigator.SelectSingleNode("Scenario/Description") == null) ? String.Empty : navigator.SelectSingleNode("Scenario/Description").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Time To Attack", (navigator.SelectSingleNode("Scenario/TimeToAttack") == null) ? String.Empty : navigator.SelectSingleNode("Scenario/TimeToAttack").Value, String.Empty);
            if (navigator.SelectSingleNode("Scenario/ClientSideAssetTransfer") != null)
            {
                String clientSideAssetTransferValue = navigator.SelectSingleNode("Scenario/ClientSideAssetTransfer").Value;
                Boolean booleanValue = false;
                if (clientSideAssetTransferValue.Equals("0"))
                    booleanValue = false;

                else if (clientSideAssetTransferValue.Equals("1"))
                    booleanValue = true;

                else
                {
                    Boolean result = Boolean.TryParse(clientSideAssetTransferValue, out booleanValue);
                    if (!result)
                        booleanValue = false;
                }

                createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Allow Asset Transfers", booleanValue.ToString(), String.Empty);
            }
            else
                createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Allow Asset Transfers", String.Empty, String.Empty);

            if (navigator.SelectSingleNode("Scenario/ClientSideRangeRingVisibility") != null)
            {
                String clientSideRangeRingValue = navigator.SelectSingleNode("Scenario/ClientSideRangeRingVisibility").Value;

                createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Range Ring Display Type", clientSideRangeRingValue, String.Empty);
            }
            else
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Scenario.Range Ring Display Type", "Full", String.Empty);
            }


            #endregion

            // Playfield is required.
            createPlayfield(navigator, document, navigator);
            // LandRegion is optional
            XPathNodeIterator itLandRegions = navigator.Select("/Scenario/LandRegion");
            while (itLandRegions.MoveNext())
            {
                createLandRegion(itLandRegions.Current, document, navigator);
            }
            // ActiveRegion is optional
            XPathNodeIterator itActiveRegions = navigator.Select("/Scenario/ActiveRegion");
            while (itActiveRegions.MoveNext())
            {
                itActiveRegions.Current.CreateAttribute(String.Empty, "name", itActiveRegions.Current.NamespaceURI, itActiveRegions.Current.SelectSingleNode("ID").Value);
                createActiveRegion(itActiveRegions.Current, document, navigator);
            }
            // Team is optional
            XPathNodeIterator itTeams = navigator.Select("/Scenario/Team");
            while (itTeams.MoveNext())
            {
                createTeam(itTeams.Current, document, navigator);
            }
            
            // Classification is optional
            XPathNodeIterator itClassification = navigator.Select("/Scenario/Classifications/Classification");
            while (itClassification.MoveNext())
            {
                createClassification(itClassification.Current, document, navigator);
            }
            // DecisionMaker is optional
            XPathNodeIterator itDecisionMakers = navigator.Select("/Scenario/DecisionMaker");
            while (itDecisionMakers.MoveNext())
            {
                itDecisionMakers.Current.CreateAttribute(String.Empty, "name", itDecisionMakers.Current.NamespaceURI, itDecisionMakers.Current.SelectSingleNode("Identifier").Value);
                createDecisionMaker(itDecisionMakers.Current, document, navigator);
            }
            // Network is optional
            XPathNodeIterator itNetworks = navigator.Select("/Scenario/Network");
            while (itNetworks.MoveNext())
            {
                createNetwork(itNetworks.Current, document, navigator);
            }
            // DefineEngram is optional
            XPathNodeIterator itDefineEngrams = navigator.Select("/Scenario/DefineEngram");
            while (itDefineEngrams.MoveNext())
            {
                createEngram(itDefineEngrams.Current, document, navigator);
            }
            // Sensor is optional
            XPathNodeIterator itSensors = navigator.Select("/Scenario/Sensor");
            while (itSensors.MoveNext())
            {
                createSensor(itSensors.Current, document, navigator);
            }
            // Genus
            XPathNodeIterator itGenera = navigator.Select("/Scenario/Genus");
            while (itGenera.MoveNext())
            {
                createGenus(itGenera.Current, document, navigator);
            }
            // Species
            //Int32 speciesNum = 0;
            XPathNodeIterator itSpecies = navigator.Select("/Scenario/Species");
            while (itSpecies.MoveNext())
            {
                itSpecies.Current.CreateAttribute(String.Empty, "name", itSpecies.Current.NamespaceURI, itSpecies.Current.SelectSingleNode("Name").Value);
                createSpecies(itSpecies.Current, document, navigator);
            }

            // Events
            createEvents(navigator.SelectSingleNode("Scenario"), document, navigator);

            // Rule
            XPathNodeIterator itRules = navigator.Select("/Scenario/Rule");
            while (itRules.MoveNext())
            {
                itRules.Current.CreateAttribute(String.Empty, "name", itSpecies.Current.NamespaceURI, itRules.Current.SelectSingleNode("Name").Value);
                createRule(itRules.Current, document, navigator);
            }

            // Score
            XPathNodeIterator itScores = navigator.Select("/Scenario/Score");
            while (itScores.MoveNext())
            {
                itScores.Current.CreateAttribute(String.Empty, "name", itScores.Current.NamespaceURI, itScores.Current.SelectSingleNode("Name").Value);
                createScore(itScores.Current, document, navigator);
            }
        }

        private void createPlayfield(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Playfield";
            String cName = "Playfield";
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Map Filename", (navLocal.SelectSingleNode("Scenario/Playfield/MapFileName") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/MapFileName").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Icon Library", (navLocal.SelectSingleNode("Scenario/Playfield/IconLibrary") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/IconLibrary").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.UTM Zone", (navLocal.SelectSingleNode("Scenario/Playfield/UtmZone") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/UtmZone").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Vertical Scale", (navLocal.SelectSingleNode("Scenario/Playfield/VerticalScale") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/VerticalScale").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Horizontal Scale", (navLocal.SelectSingleNode("Scenario/Playfield/HorizontalScale") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/HorizontalScale").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Display Labels", (navLocal.SelectSingleNode("Scenario/Playfield/ClientSideDisplayLabels") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/ClientSideDisplayLabels").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Playfield.Display Tags", (navLocal.SelectSingleNode("Scenario/Playfield/ClientSideTagDisplay") == null) ? String.Empty : navLocal.SelectSingleNode("Scenario/Playfield/ClientSideTagDisplay").Value, String.Empty);

            #endregion
        }

        private void createLandRegion(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "LandRegion";
            String cName = navLocal.SelectSingleNode("ID").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            // ScenarioRegions
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "ScenarioRegions", String.Empty);

            #endregion

            // Create parameters
            #region parameters
            
            List<String> verticies = new List<String>();
            XPathNodeIterator itVertices = navLocal.Select("Vertex");
            while (itVertices.MoveNext())
            {
                String vertex = itVertices.Current.Value;
                vertex = vertex.Trim();
                verticies.Add("(" + String.Join(", ", vertex.Split(' ')) + ")");
            }
            createParameter(VSGConfiguration, document, cName, "Component", "Location.Polygon Points", String.Join(", ", verticies.ToArray()), String.Empty);

            #endregion
        }

        private void createActiveRegion(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "ActiveRegion";
            String cName = navLocal.SelectSingleNode("ID").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            // ScenarioRegions
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "ScenarioRegions", String.Empty);
            // ActiveRegionsSensors
            String blockedSensors = (navLocal.SelectSingleNode("SensorsBlocked") == null) ? String.Empty : navLocal.SelectSingleNode("SensorsBlocked").Value;
            String[] sensors = blockedSensors.Split(',');
            foreach (String sensor in sensors)
            {
                String s = sensor.Trim();
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/Sensor[Name='{0}']", s)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, s, "ActiveRegionSensor", String.Empty);
                }
            }
            
            #endregion

            // Create parameters
            #region parameters

            List<String> verticies = new List<String>();
            XPathNodeIterator itVertices = navLocal.Select("Vertex");
            while (itVertices.MoveNext())
            {
                String vertex = itVertices.Current.Value;
                vertex = vertex.Trim();
                verticies.Add("(" + String.Join(", ", vertex.Split(' ')) + ")");
            }
            String refPoint = "0 0";
            createParameter(VSGConfiguration, document, cName, "Component", "Location.Polygon Points", String.Join(", ", verticies.ToArray()), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Location.Relative Polygon Points", String.Join(", ", verticies.ToArray()), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Location.ReferencePoint", refPoint, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.Is this a Dynamic Region", String.Empty, String.Empty);
           

            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.Start", (navLocal.SelectSingleNode("Start") == null) ? String.Empty : navLocal.SelectSingleNode("Start").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.End", (navLocal.SelectSingleNode("End") == null) ? String.Empty : navLocal.SelectSingleNode("End").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.Speed Multiplier", (navLocal.SelectSingleNode("SpeedMultiplier") == null) ? String.Empty : navLocal.SelectSingleNode("SpeedMultiplier").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.Blocks Movement", (navLocal.SelectSingleNode("BlocksMovement") == null) ? String.Empty : navLocal.SelectSingleNode("BlocksMovement").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Active Region.Is Visible", (navLocal.SelectSingleNode("IsVisible") == null) ? String.Empty : navLocal.SelectSingleNode("IsVisible").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Color.Color", (navLocal.SelectSingleNode("Color") == null) ? String.Empty : navLocal.SelectSingleNode("Color").Value, String.Empty);

            #endregion
        }

        private void createTeam(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Team";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            // TeamAgainst
            String teamsAgainst = (navLocal.SelectSingleNode("Against") == null) ? String.Empty : navLocal.SelectSingleNode("Against").Value;
            String[] teams = teamsAgainst.Split(',');
            foreach (String team in teams)
            {
                String t = team.Trim();
                // Check to ensure team is valid
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/Team[Name='{0}']", t)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, t, "TeamAgainst" + AME.Tools.ImportTool.Delimitter + cName, String.Empty);
                }
            }

            #endregion
        }
        private void createClassification(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Classification";
            String cName = navLocal.InnerXml;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion
        }

        private void createDecisionMaker(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "DecisionMaker";
            String cName = navLocal.SelectSingleNode("Identifier").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            // DecisionMakerTeam
            // Check to ensure team is valid
            String team = (navLocal.SelectSingleNode("Team") == null) ? String.Empty : navLocal.SelectSingleNode("Team").Value;
            team = team.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Team[Name='{0}']", team)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, team, "DecisionMakerTeam", String.Empty);
            }


            // DecisionMakerCanChat
            String canChatDM = (navLocal.SelectSingleNode("CanChat") == null) ? String.Empty : navLocal.SelectSingleNode("CanChat").Value;
            if (canChatDM != String.Empty)
            {
                String[] dms = canChatDM.Split(',');
                foreach (String dm in dms)
                {
                    String dmName = dm.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", dmName)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, dmName, "DecisionMakerCanChat", String.Empty);
                    }
                }
            }

            // DecisionMakerCanSpeak
            String canSpeakDM = (navLocal.SelectSingleNode("CanSpeak") == null) ? String.Empty : navLocal.SelectSingleNode("CanSpeak").Value;
            if (canSpeakDM != String.Empty)
            {
                String[] dms = canSpeakDM.Split(',');
                foreach (String dm in dms)
                {
                    String dmName = dm.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", dmName)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, dmName, "DecisionMakerCanSpeak", String.Empty);
                    }
                }
            }

            // DecisionMakerCanWhiteboard
            String canWhiteboardDM = (navLocal.SelectSingleNode("CanWhiteboard") == null) ? String.Empty : navLocal.SelectSingleNode("CanWhiteboard").Value;
            if (canWhiteboardDM != String.Empty)
            {
                String[] dms = canWhiteboardDM.Split(',');
                foreach (String dm in dms)
                {
                    String dmName = dm.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", dmName)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, dmName, "DecisionMakerCanWhiteboard", String.Empty);
                    }
                }
            }

            // DecisionMakerReportsTo
            String reportsToDM = (navLocal.SelectSingleNode("ReportsTo") == null) ? String.Empty : navLocal.SelectSingleNode("ReportsTo").Value;
            if (reportsToDM != String.Empty)
            {
                String[] dms = reportsToDM.Split(',');
                foreach (String dm in dms)
                {
                    String dmName = dm.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", dmName)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, dmName, "DecisionMakerReportsTo", String.Empty);
                    }
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.Role", (navLocal.SelectSingleNode("Role") == null) ? String.Empty : navLocal.SelectSingleNode("Role").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.Briefing", (navLocal.SelectSingleNode("Briefing") == null) ? String.Empty : navLocal.SelectSingleNode("Briefing").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.Color", (navLocal.SelectSingleNode("Color") == null) ? String.Empty : navLocal.SelectSingleNode("Color").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.CanTransferAssets", (navLocal.SelectSingleNode("CanTransfer") == null) ? String.Empty : navLocal.SelectSingleNode("CanTransfer").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.CanForceAssetTransfers", (navLocal.SelectSingleNode("CanForceTransfers") == null) ? String.Empty : navLocal.SelectSingleNode("CanForceTransfers").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "DecisionMaker.IsObserver", (navLocal.SelectSingleNode("IsObserver") == null) ? String.Empty : navLocal.SelectSingleNode("IsObserver").Value, String.Empty);
            #endregion
        }

        private void createNetwork(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Network";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            // NetworkMembers
            if (navLocal.Select("Member").Count > 1)
            {
                XPathNodeIterator itMembers = navLocal.Select("Member");
                while (itMembers.MoveNext())
                {
                    String member = itMembers.Current.Value;
                    member = member.Trim();
                    // Check to ensure member is valid
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", member)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, member, "NetworkMembers", String.Empty);
                    }
                }
            }
            else
            {
                String networkMembers = (navLocal.SelectSingleNode("Member") == null) ? String.Empty : navLocal.SelectSingleNode("Member").Value;
                String[] members = networkMembers.Split(',');
                foreach (String member in members)
                {
                    String m = member.Trim();
                    // Check to ensure member is valid
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", m)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, m, "NetworkMembers", String.Empty);
                    }
                }
            }

            #endregion
        }

        private void createEngram(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Engram";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Engram.Initial Value", (navLocal.SelectSingleNode("Value") == null) ? String.Empty : navLocal.SelectSingleNode("Value").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Engram.Type", (navLocal.SelectSingleNode("Type") == null) ? String.Empty : navLocal.SelectSingleNode("Type").Value, String.Empty);

            #endregion

        }

        private void createEmitter(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navStateName = navLocal.SelectSingleNode("parent::*/parent::node()");
            String stateName = navStateName.GetAttribute("name", navStateName.NamespaceURI);

            // Set up component values
            String cType = "Emitter";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), stateName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            // The next node should be a Attribute or Engram.
            XPathNavigator nextNode = navLocal.SelectSingleNode("child::*[position()=1]");
            if (nextNode.Name.Equals("Attribute"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Attribute_Emitter", "true", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Custom_Attribute_Emitter", "false", String.Empty);
                // Attribute could be wrong!
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Attribute", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);
            }
            else if (nextNode.Name.Equals("Engram"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Attribute_Emitter", "false", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Custom_Attribute_Emitter", "true", String.Empty);
                // Attribute could be wrong!
                createParameter(VSGConfiguration, document, cName, "Component", "Emitter.Custom_Attribute", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);
            }

            // Level (NormalEmitter) is optional
            XPathNodeIterator itNormalEmitters = navLocal.Select("NormalEmitter");
            while (itNormalEmitters.MoveNext())
            {
                createLevel(itNormalEmitters.Current, document, navGlobal);
            }
            #endregion

        }

        private void createLevel(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navParentEmitterName = navLocal.SelectSingleNode("parent::node()");
            String parentEmitterName = navParentEmitterName.GetAttribute("name", navParentEmitterName.NamespaceURI);
            Int32 levelNum = 0;
            XPathNodeIterator itLevels = navLocal.Select("Level");
            while (itLevels.MoveNext())
            {
                // Set up component values
                String cType = "Level";
                String cName = parentEmitterName + cType + levelNum++ + AME.Tools.ImportTool.Delimitter + itLevels.Current.Value;
                String cDescription = String.Empty;

                // REDO: Levels do not have names so we need to auto generate one!!!
                itLevels.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName);
                // Create component
                createComponent(VSGConfiguration, document, cType, cName, cDescription);

                // Create links
                #region links

                // Scenario Link
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), parentEmitterName, cName, "Scenario", String.Empty);

                #endregion

                // Create parameters
                #region parameters

                // The next node should be a Variance or Percen.
                XPathNavigator nextNode = itLevels.Current.SelectSingleNode("following-sibling::*");
                if (nextNode.Name.Equals("Variance"))
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Variance", "true", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Probability", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Percentage", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);
                }
                else if (nextNode.Name.Equals("Percent"))
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Probability", "true", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Variance", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Level.Percentage", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);
                }

                #endregion
            }
        }

        private void createSensor(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Sensor";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;

            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            // The next node should be a (Attribute or Engram) or Extent - position is 2 because Name is position 1 and we want the next child node.
            XPathNavigator nextNode = navLocal.SelectSingleNode("child::*[position()=2]");
            if (nextNode.Name.Equals("Attribute"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Attribute_Sensor", "true", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Custom_Attribute_Sensor", "false", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Global_Sensor", "false", String.Empty);
                // Attribute could be wrong!
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Attribute", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);

                // Create SensorRanges
                Int32 coneNum = 0;
                XPathNodeIterator itCones = navLocal.Select("Cone");
                while (itCones.MoveNext())
                {
                    itCones.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Cone" + coneNum++ + AME.Tools.ImportTool.Delimitter + "SensorRange");
                    createSensorRange(itCones.Current, document, navGlobal);
                }

            }
            else if (nextNode.Name.Equals("Engram"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Attribute_Sensor", "false", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Custom_Attribute_Sensor", "true", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Global_Sensor", "false", String.Empty);
                // Custom_Attribute could be wrong!
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Custom_Attribute", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);

                // Create SensorRanges
                Int32 coneNum = 0;
                XPathNodeIterator itCones = navLocal.Select("Cone");
                while (itCones.MoveNext())
                {
                    itCones.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Cone" + coneNum++ + AME.Tools.ImportTool.Delimitter + "SensorRange");
                    createSensorRange(itCones.Current, document, navGlobal);
                }

            }
            else if (nextNode.Name.Equals("Extent"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Attribute_Sensor", "false", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Custom_Attribute_Sensor", "false", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Global_Sensor", "true", String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "Sensor.Range", navLocal.Value == null ? String.Empty : nextNode.Value, String.Empty);                
            }

            #endregion
        }

        private void createSensorRange(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String sensorName = navLocal.SelectSingleNode("../Name").Value;

            // Set up component values
            String cType = "SensorRange";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), sensorName, cName, "Scenario", String.Empty);
            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.Spread", (navLocal.SelectSingleNode("Spread") == null) ? String.Empty : navLocal.SelectSingleNode("Spread").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.Range", (navLocal.SelectSingleNode("Extent") == null) ? String.Empty : navLocal.SelectSingleNode("Extent").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.Level", (navLocal.SelectSingleNode("Level") == null) ? String.Empty : navLocal.SelectSingleNode("Level").Value, String.Empty);
            
            // Need to split this up
            String direction = (navLocal.SelectSingleNode("Direction") == null) ? String.Empty : navLocal.SelectSingleNode("Direction").Value;
            if (!direction.Equals(String.Empty))
            {
                String[] xyz = direction.Split(' ');
                createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.X", xyz[0], String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.Y", xyz[1], String.Empty);
                if (xyz.Length > 2)
                    createParameter(VSGConfiguration, document, cName, "Component", "SensorRange.Z", xyz[2], String.Empty);
            }

            #endregion

        }

        private void createGenus(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Genus";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion
        }

        private void createCapability(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navStateName = navLocal.SelectSingleNode("parent::*/parent::node()");
            String stateName = navStateName.GetAttribute("name", navStateName.NamespaceURI);

            // Set up component values
            String cType = "Capability";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), stateName, cName, "Scenario", String.Empty);

            // Create Proximities
            Int32 proxNum = 0;
            XPathNodeIterator itProximities = navLocal.Select("Proximity");
            while (itProximities.MoveNext())
            {
                itProximities.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Proximity" + proxNum++ + AME.Tools.ImportTool.Delimitter + "Proximity");
                createProximity(itProximities.Current, document, navGlobal);
            }

            #endregion
        }

        private void createSingleton(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navStateName = navLocal.SelectSingleNode("parent::*/parent::node()");
            String stateName = navStateName.GetAttribute("name", navStateName.NamespaceURI);

            // Set up component values
            String cType = "Singleton";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), stateName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Singleton.Capability", (navLocal.SelectSingleNode("Capability") == null) ? String.Empty : navLocal.SelectSingleNode("Capability").Value, String.Empty);

            #endregion

            // Create Transitions - Should only be one of these
            XPathNodeIterator itTransitions = navLocal.Select("Transitions");
            while (itTransitions.MoveNext())
            {
                itTransitions.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Transitions" + AME.Tools.ImportTool.Delimitter + "Transitions");
                createTransitions(itTransitions.Current, document, navGlobal);
            }
        }

        private void createCombo(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navStateName = navLocal.SelectSingleNode("parent::*/parent::node()");
            String stateName = navStateName.GetAttribute("name", navStateName.NamespaceURI);

            // Set up component values
            String cType = "Combo";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), stateName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Combo.State", (navLocal.SelectSingleNode("NewState") == null) ? String.Empty : navLocal.SelectSingleNode("NewState").Value, String.Empty);

            #endregion

            // Create Contributions
            Int32 contribNum = 0;
            XPathNodeIterator itContributions = navLocal.Select("Contribution");
            while (itContributions.MoveNext())
            {
                itContributions.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Contribution" + AME.Tools.ImportTool.Delimitter + "Contribution" + contribNum++);
                createContribution(itContributions.Current, document, navGlobal);
            }
        }

        private void createContribution(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navComboName = navLocal.SelectSingleNode("parent::ComboVulnerability");
            String comboName = navComboName.GetAttribute("name", navComboName.NamespaceURI);

            // Set up component values
            String cType = "Contribution";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), comboName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Contribution.Capability", (navLocal.SelectSingleNode("Capability") == null) ? String.Empty : navLocal.SelectSingleNode("Capability").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Contribution.Range", (navLocal.SelectSingleNode("Range") == null) ? String.Empty : navLocal.SelectSingleNode("Range").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Contribution.Intensity", (navLocal.SelectSingleNode("Effect") == null) ? String.Empty : navLocal.SelectSingleNode("Effect").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Contribution.Probability", (navLocal.SelectSingleNode("Probability") == null) ? String.Empty : navLocal.SelectSingleNode("Probability").Value, String.Empty);

            #endregion
        }

        private void createTransitions(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navSingletonName = navLocal.SelectSingleNode("parent::SingletonVulnerability");
            String singletonName = navSingletonName.GetAttribute("name", navSingletonName.NamespaceURI);
            // Create Transition - Should be at least one of these!
            Int32 effectNum = 0;
            XPathNodeIterator itChildNodes = navLocal.Select("child::*");
            while (itChildNodes.MoveNext())
            {
                if (itChildNodes.Current.Name.Equals("Effect")) // Signals new transition.
                {
                    itChildNodes.Current.CreateAttribute(String.Empty, "Name", String.Empty, singletonName + AME.Tools.ImportTool.Delimitter + "Transition" + effectNum++);
                    createTransition(itChildNodes.Current, document, navGlobal);
                }
            }
        }

        private void createTransition(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navSingletonName = navLocal.SelectSingleNode("parent::*/parent::node()");
            String singletonName = navSingletonName.GetAttribute("name", navSingletonName.NamespaceURI);

            // Set up component values
            String cType = "Transition";
            String cName = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), singletonName, cName, "Scenario", String.Empty);

            // Extent parameter
            createParameter(VSGConfiguration, document, cName, "Component", "Transition.Intensity", navLocal.Value, String.Empty);

            // Range, Probabilty and State
            XPathNavigator nextNode = navLocal.SelectSingleNode("following-sibling::*");
            if (nextNode.Name.Equals("Range"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Transition.Range", nextNode.Value, String.Empty);
                nextNode = nextNode.SelectSingleNode("following-sibling::*");
            }
            if (nextNode.Name.Equals("Probability"))
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Transition.Probability", nextNode.Value, String.Empty);
                nextNode = nextNode.SelectSingleNode("following-sibling::*");
            }
            if (nextNode.Name.Equals("State"))
            {
                //String speciesName = navLocal.SelectSingleNode("ancestor::Species/Name").Value;
                createParameter(VSGConfiguration, document, cName, "Component", "Transition.State", nextNode.Value, String.Empty);
            }
        }

        private void createProximity(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            XPathNavigator navCapabilityName = navLocal.SelectSingleNode("parent::node()");
            String capabilityName = navCapabilityName.GetAttribute("name", navCapabilityName.NamespaceURI);

            // Set up component values
            String cType = "Proximity";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), capabilityName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Proximity.Range", (navLocal.SelectSingleNode("Range") == null) ? String.Empty : navLocal.SelectSingleNode("Range").Value, String.Empty);

            #endregion

            // Create Effects
            Int32 effectNum = 0;
            XPathNodeIterator itEffects = navLocal.Select("Effect");
            while (itEffects.MoveNext())
            {
                itEffects.Current.AppendChildElement(String.Empty, "Name", String.Empty, cName + "Effect" + effectNum++ + AME.Tools.ImportTool.Delimitter + "Effect");
                createEffect(itEffects.Current, document, navGlobal);
            }
        }

        private void createEffect(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String proximityName = navLocal.SelectSingleNode("../Name").Value;

            // Set up component values
            String cType = "Effect";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), proximityName, cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Effect.Intensity", (navLocal.SelectSingleNode("Intensity") == null) ? String.Empty : navLocal.SelectSingleNode("Intensity").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Effect.Probability", (navLocal.SelectSingleNode("Probability") == null) ? String.Empty : navLocal.SelectSingleNode("Probability").Value, String.Empty);

            #endregion
        }

        private void createSpecies(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Species";
            String cName = navLocal.SelectSingleNode("Name").Value;
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            
            // SpeciesType
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "SpeciesType", String.Empty);
            String speciesBase = (navLocal.SelectSingleNode("Base") == null) ? String.Empty : navLocal.SelectSingleNode("Base").Value;
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", speciesBase)) != null)
            {
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, speciesBase, "SpeciesType", String.Empty);
                speciesBase = "ExistingSpecies";
            }

            // SpeciesDMCanOwn
            String canDMOwn = (navLocal.SelectSingleNode("CanOwn") == null) ? String.Empty : navLocal.SelectSingleNode("CanOwn").Value;
            if (canDMOwn != String.Empty)
            {
                String[] dms = canDMOwn.Split(',');
                foreach (String dm in dms)
                {
                    String dmName = dm.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", dmName)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, dmName, "SpeciesDMCanOwn", String.Empty);
                    }
                }
            }

            XPathNodeIterator itCapacities = navLocal.Select("SubplatformCapacity");
            while (itCapacities.MoveNext())
            {
                String subplatformSpeciesName = itCapacities.Current.SelectSingleNode("SpeciesName").Value;
                int subplatformSpeciesCount = Int32.Parse(itCapacities.Current.SelectSingleNode("Count").Value);
                String dynamicLinkName = GetDynamicLinkType("SpeciesSubplatformCapacity", cName);
                createLink(VSGConfiguration, document, cName, cName, subplatformSpeciesName, dynamicLinkName, String.Empty);
                String linkID = GetLinkID(cName, subplatformSpeciesName, dynamicLinkName);
                createParameter(VSGConfiguration, document, linkID, eParamParentType.Link.ToString(), "Capacity.Count", subplatformSpeciesCount.ToString(), "");
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Species.IsWeapon", (navLocal.SelectSingleNode("IsWeapon") == null) ? String.Empty : navLocal.SelectSingleNode("IsWeapon").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Species.RemoveOnDestruction", (navLocal.SelectSingleNode("RemoveOnDestruction") == null) ? String.Empty : navLocal.SelectSingleNode("RemoveOnDestruction").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Species.DefaultClassification", (navLocal.SelectSingleNode("DefaultClassification") == null) ? String.Empty : navLocal.SelectSingleNode("DefaultClassification").Value, String.Empty);

            if (navLocal.SelectSingleNode("ClassificationDisplayRules") != null)
            {
                String rules = navLocal.SelectSingleNode("ClassificationDisplayRules").OuterXml;
                ClassificationDisplayRules CDRs = new ClassificationDisplayRules();
                int i = rules.IndexOf("\r\n");
                int start = rules.IndexOf(">",i-1);
                int end = rules.IndexOf("<", i);
                
                while (i >= 0)
                {
                    start = rules.IndexOf(">", i-1);
                    end = rules.IndexOf("<", start);
                    if (end - start > 1)
                    {
                        rules = rules.Remove(start + 1, end - start-1);
                    }
                    i = rules.IndexOf("\r\n");                    
                }

                CDRs.Rules = ClassificationDisplayRules.FromXML(rules);
                createParameter(VSGConfiguration, document, cName, "Component", "Species.ClassificationDisplayRules", CDRs.ToXML(), String.Empty);
            }

            createParameter(VSGConfiguration, document, cName, "Component", "Species.CollisionRadius", (navLocal.SelectSingleNode("Size") == null) ? String.Empty : navLocal.SelectSingleNode("Size").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Species.LaunchedByOwner", (navLocal.SelectSingleNode("LaunchedByOwner") == null) ? String.Empty : navLocal.SelectSingleNode("LaunchedByOwner").Value, String.Empty);
            switch (speciesBase)
            {
                case "LandObject":
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.LandObject", "true", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.AirObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.SeaObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.ExistingSpecies", "false", String.Empty); 
                    break;

                case "AirObject":
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.LandObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.AirObject", "true", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.SeaObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.ExistingSpecies", "false", String.Empty); 
                    break;

                case "SeaObject":
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.LandObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.AirObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.SeaObject", "true", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.ExistingSpecies", "false", String.Empty); 
                    break;

                case "ExistingSpecies":
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.LandObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.AirObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.SeaObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.ExistingSpecies", "true", String.Empty);
                    break;

                default:
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.LandObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.AirObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.SeaObject", "false", String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "Species.ExistingSpecies", "false", String.Empty); 
                    break;
            }

            // Name States
            // Because of the State to State links we need to create the names first. Then create them. 
            XPathNodeIterator itStates = navLocal.Select("FullyFunctional | DefineState");
            while (itStates.MoveNext())
            {
                if (itStates.Current.SelectSingleNode("State") != null)
                {
                    String stateName = itStates.Current.SelectSingleNode("State").Value;
                    //itStates.Current.SelectSingleNode("State").SetValue(cName + AME.Tools.ImportTool.Delimitter + stateName);
                    itStates.Current.CreateAttribute(String.Empty, "name", itStates.Current.NamespaceURI, cName + AME.Tools.ImportTool.Delimitter + stateName);
                }
                else
                    //itStates.Current.AppendChildElement(String.Empty, "State", String.Empty, cName + AME.Tools.ImportTool.Delimitter + "FullyFunctional");
                    itStates.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + AME.Tools.ImportTool.Delimitter + "FullyFunctional");
            }


            // Create States
            itStates = navLocal.Select("FullyFunctional | DefineState");
            Boolean isDeadState = false;
            while (itStates.MoveNext())
            {
                String stateName = itStates.Current.GetAttribute("name", itStates.Current.NamespaceURI);
                if (stateName.ToLower().Equals(cName.ToLower() + AME.Tools.ImportTool.Delimitter + "Dead".ToLower()))
                    isDeadState = true;
            }
            if (!isDeadState)
            {
                String name = cName + AME.Tools.ImportTool.Delimitter + "Dead";

                XmlElement deadState = document.CreateElement("DefineState");
                XmlAttribute deadStateName = document.CreateAttribute("name");
                deadStateName.InnerXml = name;
                deadState.Attributes.Append(deadStateName);
                XmlElement state = document.CreateElement("State");
                state.InnerXml = name;
                deadState.AppendChild(state);
                navLocal.AppendChild(deadState.CreateNavigator());
            }
            itStates = navLocal.Select("FullyFunctional | DefineState");
            while (itStates.MoveNext())
            {
                createState(itStates.Current, document, navGlobal);
            }

            #endregion
        }
        protected String GetLinkID(String from, String to, String type)
        {
            return from + to + type;
        }
        protected String GetDynamicLinkType(String linkType, String dynamicPivot)
        {
            string seperator = AME.Tools.ImportTool.Delimitter; // "_@_"

            // is the linkType already dynamic?
            if (linkType.Contains(seperator))
            {
                int indexOfSeperator = linkType.IndexOf(seperator);
                string justAfterSeperator = linkType.Substring(0, indexOfSeperator + 1);

                // combine and form dynamic
                return justAfterSeperator + dynamicPivot;
            }
            else  // combine and form dynamic
            {
                return linkType + seperator + dynamicPivot;
            }
        }
        private void createState(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String speciesName = navLocal.SelectSingleNode("../Name").Value;

            // Set up component values
            String cType = "State";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesName, cName, "Scenario", String.Empty);

            // StateSensor
            XPathNodeIterator itSensors = navLocal.Select("StateParameters/Sense");
            while (itSensors.MoveNext())
            {
                String sense = itSensors.Current.Value;
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/Sensor[Name='{0}']", sense)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, sense, "StateSensor", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            // Name Capabilities
            XPathNodeIterator itCapabilities = navLocal.Select("StateParameters/Capability");
            Int32 capNum = 0;
            while (itCapabilities.MoveNext())
            {
                String capabilityName = itCapabilities.Current.SelectSingleNode("Name").Value;
                itCapabilities.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + capNum++ + AME.Tools.ImportTool.Delimitter + capabilityName);
            }

            // Name Singletons
            XPathNodeIterator itSingletons = navLocal.Select("StateParameters/SingletonVulnerability");
            Int32 singleNum = 0;
            while (itSingletons.MoveNext())
            {
                itSingletons.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + singleNum++ + AME.Tools.ImportTool.Delimitter + "Singleton");
            }

            // Name Combo
            XPathNodeIterator itCombos = navLocal.Select("StateParameters/ComboVulnerability");
            Int32 comboNum = 0;
            while (itCombos.MoveNext())
            {
                itCombos.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + comboNum++ + AME.Tools.ImportTool.Delimitter + "Combo");
            }
            
            // Name Emitters
            XPathNodeIterator itEmitters = navLocal.Select("StateParameters/Emitter");
            Int32 emitterNum = 0;
            while (itEmitters.MoveNext())
            {
                itEmitters.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + emitterNum++ + AME.Tools.ImportTool.Delimitter + "Emitter");
            }

            // Create Capabilities
            itCapabilities = navLocal.Select("StateParameters/Capability");
            while (itCapabilities.MoveNext())
            {
                createCapability(itCapabilities.Current, document, navGlobal);
            }

            // Create Singletons
            itSingletons = navLocal.Select("StateParameters/SingletonVulnerability");
            while (itSingletons.MoveNext())
            {
                createSingleton(itSingletons.Current, document, navGlobal);
            }

            // Create Combos
            itCombos = navLocal.Select("StateParameters/ComboVulnerability");
            while (itCombos.MoveNext())
            {
                createCombo(itCombos.Current, document, navGlobal);
            }

            // Create Emitters
            itEmitters = navLocal.Select("StateParameters/Emitter");
            while (itEmitters.MoveNext())
            {
                createEmitter(itEmitters.Current, document, navGlobal);
            }

            XPathNavigator navIcon = navLocal.SelectSingleNode("Icon");
            if (navIcon != null)
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Image.Icon", navIcon.Value, String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideIcon", "true", String.Empty);
            }

            if (navLocal.SelectSingleNode("StateParameters") != null)
            {
                XPathNodeIterator itChildNodes = navLocal.Select("StateParameters/LaunchDuration | " +
                                                                 "StateParameters/DockingDuration | " +
                                                                 "StateParameters/TimeToAttack | " +
                                                                 "StateParameters/EngagementDuration | " +
                                                                 "StateParameters/MaximumSpeed | " +
                                                                 "StateParameters/FuelCapacity | " +
                                                                 "StateParameters/InitialFuelLoad | " +
                                                                 "StateParameters/FuelConsumptionRate | " +                                                                                                                                 "StateParameters/FuelConsumptionRate | " +
                                                                 "StateParameters/FuelDepletionState | " +
                                                                 "StateParameters/Stealable");
                while (itChildNodes.MoveNext())
                {
                    switch (itChildNodes.Current.Name)
                    {
                        case "Stealable":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.Stealable", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideStealable", "true", String.Empty);
                            break;
                        case "LaunchDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.LaunchDuration", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideLaunchDuration", "true", String.Empty);
                            break;
                        case "DockingDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.DockingDuration", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideDockingDuration", "true", String.Empty);
                            break;
                        case "TimeToAttack":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.TimeToAttack", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideTimeToAttack", "true", String.Empty);
                            break;
                        case "EngagementDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.EngagementDuration", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideEngagementDuration", "true", String.Empty);
                            break;
                        case "MaximumSpeed":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.MaxSpeed", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideMaxSpeed", "true", String.Empty);
                            break;
                        case "FuelCapacity":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.FuelCapacity", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideFuelCapacity", "true", String.Empty);
                            break;
                        case "InitialFuelLoad":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.InitialFuel", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideInitialFuel", "true", String.Empty);
                            break;
                        case "FuelConsumptionRate":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.FuelConsumption", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideFuelConsumption", "true", String.Empty);
                            break;
                        //case "Icon":
                        //    createParameter(VSGConfiguration, document, cName, "Component", "Image.Icon", itChildNodes.Current.Value, String.Empty);
                        //    createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideIcon", "true", String.Empty);
                        //    break;
                        case "FuelDepletionState":
                            createParameter(VSGConfiguration, document, cName, "Component", "State.FuelDepletionState", itChildNodes.Current.Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "State.OverrideFuelDepletionState", "true", String.Empty);
                            break;
                    }
                }
            }
 
            #endregion
        }

        private void createChangeEngramEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            // Set up component values
            String cType = "ChangeEngramEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link - Determine where to link
            if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
            {
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
            {
                String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
            {
                String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
            {
                String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
            }
            
            // EngramID
            // Check to ensure engram is valid
            String engramId = (navLocal.SelectSingleNode("Name") == null) ? String.Empty : navLocal.SelectSingleNode("Name").Value;
            engramId = engramId.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DefineEngram[Name='{0}']", engramId)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, engramId, "EngramID", String.Empty);
            }

            // EngramUnitID
            String id = (navLocal.SelectSingleNode("Unit") == null) ? String.Empty : navLocal.SelectSingleNode("Unit").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (isUnit)
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "ChangeEngram.Unit Specified", "true", String.Empty);
                }
                else if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null)
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "ChangeEngram.Unit Specified", "true", String.Empty);

                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // EngramUnitID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EngramUnitID", String.Empty);
                }
            }
            else
                createParameter(VSGConfiguration, document, cName, "Component", "ChangeEngram.Unit Specified", "false", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "ChangeEngram.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "ChangeEngram.Value", (navLocal.SelectSingleNode("Value") == null) ? String.Empty : navLocal.SelectSingleNode("Value").Value, String.Empty);

            #endregion
        }

        private void createRemoveEngramEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "RemoveEngramEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
            {
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
            {
                String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
            {
                String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
            {
                String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
            }

            // EngramID
            // Check to ensure engram is valid
            String engramId = (navLocal.SelectSingleNode("Name") == null) ? String.Empty : navLocal.SelectSingleNode("Name").Value;
            engramId = engramId.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DefineEngram[Name='{0}']", engramId)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, engramId, "EngramID", String.Empty);
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "RemoveEngram.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion
        }

        private void createCloseVoiceChannelEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            //Set up component
            String cType = "CloseVoiceChannelEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;

            //Create Component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            //Create Links
            #region links

            //Scenario link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            //CloseVoiceChannel
            String voiceChannel = (navLocal.SelectSingleNode("Name") == null) ? String.Empty : navLocal.SelectSingleNode("Name").Value;
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/CloseVoiceChannel[Channel='{0}']", voiceChannel)) != null)
            {
                String room = navGlobal.SelectSingleNode(String.Format("/Scenario/CloseVoiceChannel[Channel='{0}']", voiceChannel)).GetAttribute("name", navGlobal.NamespaceURI);
                createLink(VSGConfiguration, document, cName, cName, room, "CloseVoiceChannel", String.Empty);
            }

            #endregion
            //Create Parameters
            createParameter(VSGConfiguration, document, cName, "Component", "CloseVoiceChannel.Name", (navLocal.SelectSingleNode("Name") == null) ? String.Empty : navLocal.SelectSingleNode("Name").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "CloseVoiceChannel.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
        }

        private void createOpenVoiceChannelEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "OpenVoiceChannelEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // openVoiceChannelEventAccess
            String voiceChannelAccess = (navLocal.SelectSingleNode("Access") == null) ? String.Empty : navLocal.SelectSingleNode("Access").Value;
            String[] accessList = voiceChannelAccess.Split(',');
            foreach (String access in accessList)
            {
                String m = access.Trim();
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", m)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, m, "OpenVoiceChannelEventAccess", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "OpenVoiceChannel.Name", (navLocal.SelectSingleNode("Name") == null) ? String.Empty : navLocal.SelectSingleNode("Name").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "OpenVoiceChannel.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion
        }

        private void createCloseChatRoomEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "CloseChatRoomEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // CloseChatRoom
            String chatroom = (navLocal.SelectSingleNode("Room") == null) ? String.Empty : navLocal.SelectSingleNode("Room").Value;
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/OpenChatRoom[Room='{0}']", chatroom)) != null)
            {
                String room = navGlobal.SelectSingleNode(String.Format("/Scenario/OpenChatRoom[Room='{0}']", chatroom)).GetAttribute("name", navGlobal.NamespaceURI);
                createLink(VSGConfiguration, document, cName, cName, room, "CloseChatRoom", String.Empty);
            }


            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "CloseChatRoom.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion
        }

        private void createSendChatMessageEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "SendChatMessageEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // SendChatMessageRoom
            String chatroom = (navLocal.SelectSingleNode("Room") == null) ? String.Empty : navLocal.SelectSingleNode("Room").Value;
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/OpenChatRoom[Room='{0}']", chatroom)) != null)
            {
                String room = navGlobal.SelectSingleNode(String.Format("/Scenario/OpenChatRoom[Room='{0}']", chatroom)).GetAttribute("name", navGlobal.NamespaceURI);
                createLink(VSGConfiguration, document, cName, cName, room, "SendChatMessageRoom", String.Empty);
            }

            // SendChatMessageSenderDM
            String sender = (navLocal.SelectSingleNode("Sender") == null) ? String.Empty : navLocal.SelectSingleNode("Sender").Value;
            sender.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", sender)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, sender, "SendChatMessageSenderDM", String.Empty);
            }


            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SendChatMessage.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "SendChatMessage.Message", (navLocal.SelectSingleNode("Message") == null) ? String.Empty : navLocal.SelectSingleNode("Message").Value, String.Empty);
            #endregion
        }
        private void createSendVoiceMessageEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "SendVoiceMessageEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // SendVoiceMessageChannel
            String voiceChannel = (navLocal.SelectSingleNode("Channel") == null) ? String.Empty : navLocal.SelectSingleNode("Channel").Value;
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/OpenVoiceChannel[Name='{0}']", voiceChannel)) != null)
            {
                String channel = navGlobal.SelectSingleNode(String.Format("/Scenario/OpenVoiceChannel[Name='{0}']", voiceChannel)).GetAttribute("name", navGlobal.NamespaceURI);
                createLink(VSGConfiguration, document, cName, cName, channel, "SendVoiceMessageChannel", String.Empty);
            }


            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SendVoiceMessage.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "SendVoiceMessage.FilePath", (navLocal.SelectSingleNode("FilePath") == null) ? String.Empty : navLocal.SelectSingleNode("FilePath").Value, String.Empty);
            #endregion
        }
        private void createSendVoiceMessageToUserEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
                {
                    // Set up component values
                    String cType = "SendVoiceMessageToUserEvent";
                    String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
                    String cDescription = String.Empty;
                    // Create component
                    createComponent(VSGConfiguration, document, cType, cName, cDescription);

                    // Create links
                    #region links

                    // Scenario Link
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

                    // SendVoiceMessageDecisionMakerID
                    String decisionMakerID = (navLocal.SelectSingleNode("DecisionMakerID") == null) ? String.Empty : navLocal.SelectSingleNode("DecisionMakerID").Value;
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", decisionMakerID)) != null)
                    {
                        //String dm = navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", decisionMakerID)).GetAttribute("identifier", navGlobal.NamespaceURI);
                        createLink(VSGConfiguration, document, cName, cName, decisionMakerID, "SendVoiceMessageToUserChannel", String.Empty);
                    }


                    #endregion

                    // Create parameters
                    #region parameters

                    createParameter(VSGConfiguration, document, cName, "Component", "SendVoiceMessageToUser.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
                    createParameter(VSGConfiguration, document, cName, "Component", "SendVoiceMessageToUser.FilePath", (navLocal.SelectSingleNode("FilePath") == null) ? String.Empty : navLocal.SelectSingleNode("FilePath").Value, String.Empty);
                    #endregion
                }
        private void createOpenChatRoomEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "OpenChatRoomEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // OpenChatRoomEventMember
            String chatRoomMembers = (navLocal.SelectSingleNode("Members") == null) ? String.Empty : navLocal.SelectSingleNode("Members").Value;
            String[] members = chatRoomMembers.Split(',');
            foreach (String member in members)
            {
                String m = member.Trim();
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", m)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, m, "OpenChatRoomEventMember", String.Empty);
                }
            }            

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "OpenChatRoom.Name", (navLocal.SelectSingleNode("Room") == null) ? String.Empty : navLocal.SelectSingleNode("Room").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "OpenChatRoom.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion
        }

        private void createFlushEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            // Set up component values
            String cType = "FlushEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("Unit") == null) ? String.Empty : navLocal.SelectSingleNode("Unit").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Flush.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion
        }

        private void createCreateEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "CreateEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // Scenario - CreateEvent-DecisionMaker
            String owner = (navLocal.SelectSingleNode("Owner") == null) ? String.Empty : navLocal.SelectSingleNode("Owner").Value;
            owner = owner.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", owner)) != null)
            {
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, owner, "Scenario", String.Empty);
            }

            // CreateEventKind
            String kind = (navLocal.SelectSingleNode("Kind") == null) ? String.Empty : navLocal.SelectSingleNode("Kind").Value;
            kind = kind.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", kind)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, kind, "CreateEventKind", String.Empty);
            }

            #endregion

            //// Subplatforms
            //Int32 subplatNum = 0;
            //XPathNodeIterator itSubplatforms = navLocal.Select("Subplatform");
            //while (itSubplatforms.MoveNext())
            //{
            //    itSubplatforms.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + "Subplatform" + subplatNum++ + AME.Tools.ImportTool.Delimitter + "Subplatform");
            //    createSubplatform(itSubplatforms.Current, document, navGlobal);
            //}

            
            if (navLocal.Select("Subplatform").Count > 0)
            {
                XPathNodeIterator itSubplatforms = navLocal.Select("Subplatform");
                while (itSubplatforms.MoveNext())
                {
                    String subplatformString = itSubplatforms.Current.Value;
                    String[] subplatforms = subplatformString.Split(',');
                    foreach (String subp in subplatforms)
                    {
                        String m = subp.Trim();
                        // Check to ensure member is valid
                        //if (navGlobal.SelectSingleNode(String.Format("/Scenario/CreateEvent[Identifier='{0}']", m)) != null)
                        //{
                            createLink(VSGConfiguration, document, cName, cName, m, "CreateEventSubplatform", String.Empty);
                        //}
                    }
                }
            }
            //else
            //{
            //    String subplatformString = (navLocal.SelectSingleNode("Subplatform") == null) ? String.Empty : navLocal.SelectSingleNode("Subplatform").Value;
            //    String[] subplatforms = subplatformString.Split(',');
            //    foreach (String subp in subplatforms)
            //    {
            //        String m = subp.Trim();
            //        // Check to ensure member is valid
            //        if (navGlobal.SelectSingleNode(String.Format("/Scenario/CreateEvent[Identifier='{0}']", m)) != null)
            //        {
            //            createLink(VSGConfiguration, document, cName, cName, m, "CreateEventSubplatform", String.Empty);
            //        }
            //    }
            //}
            //// Create parameters
            //#region parameters

            //createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", "true", String.Empty);
            //createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            //#endregion
        }

        private void createSubplatform(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            //itSubplatforms.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + "Subplatform" + subplatNum++ + AME.Tools.ImportTool.Delimitter + "Subplatform");
            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "Subplatform";
            String cName;
            String cDescription = String.Empty;

            // Create links
            #region links
            
            // SubplatformKind
            String kind = (navLocal.SelectSingleNode("Kind") == null) ? String.Empty : navLocal.SelectSingleNode("Kind").Value;
            kind = kind.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", kind)) != null)
            {
                // Create component _ Want Kind in the component name.
                cName = navLocal.GetAttribute("name", navLocal.NamespaceURI) + AME.Tools.ImportTool.Delimitter + kind + "_" + cType;
                navLocal.MoveToAttribute("name", navLocal.NamespaceURI);
                navLocal.SetValue(cName);
                navLocal.MoveToParent();
                
                createComponent(VSGConfiguration, document, cType, cName, cDescription);

                createLink(VSGConfiguration, document, cName, cName, kind, "SubplatformKind", String.Empty);
            }
            else
            {
                cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);

                // Create component
                createComponent(VSGConfiguration, document, cType, cName, cDescription);
            }

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), eventName, cName, "Scenario", String.Empty);


            #endregion

            // Create parameters
            #region parameters

            if (navLocal.SelectSingleNode("Docked") != null)
                createParameter(VSGConfiguration, document, cName, "Component", "Subplatform.DockedCount", (navLocal.SelectSingleNode("Docked/Count") == null) ? String.Empty : navLocal.SelectSingleNode("Docked/Count").Value, String.Empty);

            #endregion

            // Armaments
            Int32 armamentNum = 0;
            XPathNodeIterator itArmaments = navLocal.Select("Armament");
            while (itArmaments.MoveNext())
            {
                itArmaments.Current.CreateAttribute(String.Empty, "name", String.Empty, cName + "Armament" + armamentNum++ + AME.Tools.ImportTool.Delimitter + "Armament");
                createArmament(itArmaments.Current, document, navGlobal);
            }
        }

        private void createArmament(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String subPlatformName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "Armament";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), subPlatformName, cName, "Scenario", String.Empty);

            // WeaponKind
            String kind = (navLocal.SelectSingleNode("Weapon") == null) ? String.Empty : navLocal.SelectSingleNode("Weapon").Value;
            kind = kind.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", kind)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, kind, "WeaponKind", String.Empty);
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Armament.WeaponCount", (navLocal.SelectSingleNode("Count") == null) ? String.Empty : navLocal.SelectSingleNode("Count").Value, String.Empty);

            #endregion
        }

        private void createMoveEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            // Set up component values
            String cType = "MoveEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("ID") == null) ? String.Empty : navLocal.SelectSingleNode("ID").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "MoveEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "MoveEvent.Throttle", (navLocal.SelectSingleNode("Throttle") == null) ? String.Empty : navLocal.SelectSingleNode("Throttle").Value, String.Empty);
            // Need to split this up
            String direction = (navLocal.SelectSingleNode("Destination") == null) ? String.Empty : navLocal.SelectSingleNode("Destination").Value;
            if (!direction.Equals(String.Empty))
            {
                String[] xyz = direction.Split(' ');
                createParameter(VSGConfiguration, document, cName, "Component", "DestinationLocation.X", xyz[0], String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "DestinationLocation.Y", xyz[1], String.Empty);
                if (xyz.Length > 2)
                    createParameter(VSGConfiguration, document, cName, "Component", "DestinationLocation.Z", xyz[2], String.Empty);
            }

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createEngramRange(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            String engramName = navLocal.SelectSingleNode("Name").Value;
            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Create links
            engramName = engramName.Trim();

            // EngramUnitID
            String id = (navLocal.SelectSingleNode("Unit") == null) ? String.Empty : navLocal.SelectSingleNode("Unit").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (isUnit)
                {
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Unit Selected", "false", String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Performing Unit", "true", String.Empty);
                }
                else if(navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // EngramUnitID
                    createLink(VSGConfiguration, document, eventName, eventName, createEvent, "EngramUnitID", String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Unit Selected", "true", String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Performing Unit", "false", String.Empty);
                }
            }

            // EventEngram
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DefineEngram[Name='{0}']", engramName)) != null)
            {
                createLink(VSGConfiguration, document, eventName, eventName, engramName, "EventEngram", String.Empty);

                // Create parameters

                // Need to split this up
                String included = (navLocal.SelectSingleNode("Included") == null) ? String.Empty : navLocal.SelectSingleNode("Included").Value;
                String excluded = (navLocal.SelectSingleNode("Excluded") == null) ? String.Empty : navLocal.SelectSingleNode("Excluded").Value;
                if (!included.Equals(String.Empty))
                {
                    if (included.Contains(","))
                    {
                        included = included.Replace(" ", String.Empty);
                        included = included.Replace(",", Environment.NewLine);
                    }
                    else if (included.Contains(" "))
                        included = included.Replace(" ", Environment.NewLine);

                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Range Include", included, String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Selected Engram Type", "Include", String.Empty);

                }
                else if (!excluded.Equals(String.Empty))
                {
                    if (excluded.Contains(","))
                    {
                        excluded = excluded.Replace(" ", String.Empty);
                        excluded = excluded.Replace(",", Environment.NewLine);
                    }
                    else if (excluded.Contains(" "))
                        excluded = excluded.Replace(" ", Environment.NewLine);

                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Range Exclude", excluded, String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Selected Engram Type", "Exclude", String.Empty);

                }
                // Comparison
                XPathNavigator navComparison = navLocal.SelectSingleNode("Comparison");
                if (navComparison != null)
                {
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Compare Inequality", (navComparison.SelectSingleNode("Condition") == null) ? String.Empty : navComparison.SelectSingleNode("Condition").Value, String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Engram Compare Value", (navComparison.SelectSingleNode("CompareTo") == null) ? String.Empty : navComparison.SelectSingleNode("CompareTo").Value, String.Empty);
                    createParameter(VSGConfiguration, document, eventName, "Component", "EngramRange.Selected Engram Type", "Compare", String.Empty);

                }
            }
        }

        private void createRevealEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);
            Boolean isUnit = false;

            // Set up component values
            String cType = "RevealEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("ID") == null) ? String.Empty : navLocal.SelectSingleNode("ID").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "RevealEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "RevealEvent.InitialState", (navLocal.SelectSingleNode("InitialState") == null) ? String.Empty : navLocal.SelectSingleNode("InitialState").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.InitialTag", (navLocal.SelectSingleNode("InitialTag") == null) ? String.Empty : navLocal.SelectSingleNode("InitialTag").Value, String.Empty);
            // Need to split this up
            String initialLocation = (navLocal.SelectSingleNode("InitialLocation") == null) ? String.Empty : navLocal.SelectSingleNode("InitialLocation").Value;
            if (!initialLocation.Equals(String.Empty))
            {
                String[] xyz = initialLocation.Split(' ');
                createParameter(VSGConfiguration, document, cName, "Component", "InitialLocation.X", xyz[0], String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "InitialLocation.Y", xyz[1], String.Empty);
                if (xyz.Length > 2)
                    createParameter(VSGConfiguration, document, cName, "Component", "InitialLocation.Z", xyz[2], String.Empty);
            }

            if (navLocal.SelectSingleNode("StartupParameters") != null)
            {
                XPathNodeIterator itChildNodes = navLocal.Select("StartupParameters/child::Parameter");
                while (itChildNodes.MoveNext())
                {
                    switch (itChildNodes.Current.Value)
                    {
                        case "Stealable":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Stealable", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideStealable", "true", String.Empty);
                            break;
                        case "LaunchDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.LaunchDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideLaunchDuration", "true", String.Empty);
                            break;
                        case "DockingDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.DockingDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideDockingDuration", "true", String.Empty);
                            break;
                        case "TimeToAttack":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.TimeToAttack", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideTimeToAttack", "true", String.Empty);
                            break;
                        case "EngagementDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.EngagementDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideEngagementDuration", "true", String.Empty);
                            break;
                        case "MaximumSpeed":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.MaxSpeed", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideMaxSpeed", "true", String.Empty);
                            break;
                        case "FuelCapacity":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelCapacity", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelCapacity", "true", String.Empty);
                            break;
                        case "InitialFuelLoad":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.InitialFuel", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideInitialFuel", "true", String.Empty);
                            break;
                        case "FuelConsumptionRate":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelConsumption", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelConsumption", "true", String.Empty);
                            break;
                        case "Icon":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Icon", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideIcon", "true", String.Empty);
                            break;
                        case "FuelDepletionState":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelDepletionState", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelDepletionState", "true", String.Empty);

                            break;
                        case "ObjectName":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.ObjectName", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideObjectName", "true", String.Empty);
                            break;
                    }
                }
            }

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createStateChangeEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "StateChangeEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("ID") == null) ? String.Empty : navLocal.SelectSingleNode("ID").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }                          
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "StateChangeEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "StateChangeEvent.State", (navLocal.SelectSingleNode("NewState") == null) ? String.Empty : navLocal.SelectSingleNode("NewState").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "StateChangeEvent.FromState", (navLocal.SelectSingleNode("From") == null) ? String.Empty : navLocal.SelectSingleNode("From").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "StateChangeEvent.ExceptState", (navLocal.SelectSingleNode("Except") == null) ? String.Empty : navLocal.SelectSingleNode("Except").Value, String.Empty);

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createLaunchEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "LaunchEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("Parent") == null) ? String.Empty : navLocal.SelectSingleNode("Parent").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            // LaunchEventSubplatform - Child
            String subplatform = (navLocal.SelectSingleNode("Child") == null) ? String.Empty : navLocal.SelectSingleNode("Child").Value;
            subplatform = subplatform.Trim();
            createLink(VSGConfiguration, document, cName, cName, subplatform, "LaunchEventSubplatform", String.Empty);
            //if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']/Subplatform[Child='{1}']", id, subplatform)) != null)
            //{
            //    subplatform = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']/Subplatform[Child='{1}']", id, subplatform)).GetAttribute("name", navGlobal.NamespaceURI);
            //    createLink(VSGConfiguration, document, cName, cName, subplatform, "LaunchEventSubplatform", String.Empty);
            //}

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "LaunchEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "LaunchEvent.InitialState", (navLocal.SelectSingleNode("InitialState") == null) ? String.Empty : navLocal.SelectSingleNode("InitialState").Value, String.Empty);

            // Need to split this up
            String relativeLocation = (navLocal.SelectSingleNode("RelativeLocation") == null) ? String.Empty : navLocal.SelectSingleNode("RelativeLocation").Value;
            if (!relativeLocation.Equals(String.Empty))
            {
                relativeLocation = relativeLocation.Trim(); // Get rid up empty spaces or they will be included in split.
                String[] xyz = relativeLocation.Split(' ');
                createParameter(VSGConfiguration, document, cName, "Component", "RelativeLocation.X", xyz[0], String.Empty);
                createParameter(VSGConfiguration, document, cName, "Component", "RelativeLocation.Y", xyz[1], String.Empty);
                if (xyz.Length > 2)
                    createParameter(VSGConfiguration, document, cName, "Component", "RelativeLocation.Z", xyz[2], String.Empty);
            }

            if (navLocal.SelectSingleNode("StartupParameters") != null)
            {
                XPathNodeIterator itChildNodes = navLocal.Select("StartupParameters/child::Parameter");
                while (itChildNodes.MoveNext())
                {
                    switch (itChildNodes.Current.Value)
                    {
                        case "Stealable":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Stealable", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideStealable", "true", String.Empty);
                            break;
                        case "LaunchDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.LaunchDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideLaunchDuration", "true", String.Empty);
                            break;
                        case "DockingDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.DockingDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideDockingDuration", "true", String.Empty);
                            break;
                        case "TimeToAttack":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.TimeToAttack", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideTimeToAttack", "true", String.Empty);
                            break;
                        case "EngagementDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.EngagementDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideEngagementDuration", "true", String.Empty);
                            break;
                        case "MaximumSpeed":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.MaxSpeed", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideMaxSpeed", "true", String.Empty);
                            break;
                        case "FuelCapacity":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelCapacity", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelCapacity", "true", String.Empty);
                            break;
                        case "InitialFuelLoad":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.InitialFuel", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideInitialFuel", "true", String.Empty);
                            break;
                        case "FuelConsumptionRate":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelConsumption", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelConsumption", "true", String.Empty);
                            break;
                        case "Icon":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Icon", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideIcon", "true", String.Empty);
                            break;
                        case "FuelDepletionState":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelDepletionState", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelDepletionState", "true", String.Empty);
                            break;
                    }
                }
            }

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createWeaponLaunchEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            String eventName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "WeaponLaunchEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("Parent") == null) ? String.Empty : navLocal.SelectSingleNode("Parent").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            // LaunchEventSubplatform - Child
            String subplatform = (navLocal.SelectSingleNode("Child") == null) ? String.Empty : navLocal.SelectSingleNode("Child").Value;
            subplatform = subplatform.Trim();
            createLink(VSGConfiguration, document, cName, cName, subplatform, "WeaponLaunchEventSubplatform", String.Empty);

            String target = (navLocal.SelectSingleNode("Target") == null) ? String.Empty : navLocal.SelectSingleNode("Target").Value;
            target = target.Trim();
            createLink(VSGConfiguration, document, cName, cName, target, "WeaponLaunchEventTarget", String.Empty);
            //if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']/Subplatform[Child='{1}']", id, subplatform)) != null)
            //{
            //    subplatform = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']/Subplatform[Child='{1}']", id, subplatform)).GetAttribute("name", navGlobal.NamespaceURI);
            //    createLink(VSGConfiguration, document, cName, cName, subplatform, "LaunchEventSubplatform", String.Empty);
            //}

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "WeaponLaunchEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "WeaponLaunchEvent.InitialState", (navLocal.SelectSingleNode("InitialState") == null) ? String.Empty : navLocal.SelectSingleNode("InitialState").Value, String.Empty);

            

            if (navLocal.SelectSingleNode("StartupParameters") != null)
            {
                XPathNodeIterator itChildNodes = navLocal.Select("StartupParameters/child::Parameter");
                while (itChildNodes.MoveNext())
                {
                    switch (itChildNodes.Current.Value)
                    {
                        case "Stealable":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Stealable", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideStealable", "true", String.Empty);
                            break;
                        case "LaunchDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.LaunchDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideLaunchDuration", "true", String.Empty);
                            break;
                        case "DockingDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.DockingDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideDockingDuration", "true", String.Empty);
                            break;
                        case "TimeToAttack":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.TimeToAttack", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideTimeToAttack", "true", String.Empty);
                            break;
                        case "EngagementDuration":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.EngagementDuration", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideEngagementDuration", "true", String.Empty);
                            break;
                        case "MaximumSpeed":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.MaxSpeed", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideMaxSpeed", "true", String.Empty);
                            break;
                        case "FuelCapacity":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelCapacity", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelCapacity", "true", String.Empty);
                            break;
                        case "InitialFuelLoad":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.InitialFuel", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideInitialFuel", "true", String.Empty);
                            break;
                        case "FuelConsumptionRate":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelConsumption", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelConsumption", "true", String.Empty);
                            break;
                        case "Icon":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.Icon", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideIcon", "true", String.Empty);
                            break;
                        case "FuelDepletionState":
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.FuelDepletionState", itChildNodes.Current.SelectSingleNode("following-sibling::node()").Value, String.Empty);
                            createParameter(VSGConfiguration, document, cName, "Component", "StartupParameters.OverrideFuelDepletionState", "true", String.Empty);
                            break;
                    }
                }
            }

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createTransferEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            // Set up component values
            String cType = "TransferEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("ID") == null) ? String.Empty : navLocal.SelectSingleNode("ID").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            // TransferEventFromDM
            String from = (navLocal.SelectSingleNode("From") == null) ? String.Empty : navLocal.SelectSingleNode("From").Value;
            from = from.Trim();
            // Check to ensure member is valid
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", from)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, from, "TransferEventFromDM", String.Empty);
            }

            // TransferEventToDM
            String to = (navLocal.SelectSingleNode("To") == null) ? String.Empty : navLocal.SelectSingleNode("To").Value;
            to = to.Trim();
            // Check to ensure member is valid
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[Identifier='{0}']", to)) != null)
            {
                createLink(VSGConfiguration, document, cName, cName, to, "TransferEventToDM", String.Empty);
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "TransferEvent.Time", (navLocal.SelectSingleNode("Time") == null) ? String.Empty : navLocal.SelectSingleNode("Time").Value, String.Empty);

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }
        }

        private void createReiterateEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "ReiterateEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            // Scenario Link - Determine where to link
            if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
            {
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
            {
                String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
            {
                String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
            }
            else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
            {
                String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
            }
        
            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "ReiterateEvent.Time", (navLocal.SelectSingleNode("Start") == null) ? String.Empty : navLocal.SelectSingleNode("Start").Value, String.Empty);

            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }

            // MoveEvents
            Int32 moveEventNum = 0;
            XPathNodeIterator itMoveEvents = navLocal.Select("ReiterateThis/Move_Event");
            while (itMoveEvents.MoveNext())
            {
                itMoveEvents.Current.CreateAttribute(String.Empty, "name", navLocal.NamespaceURI, cName + moveEventNum++ + AME.Tools.ImportTool.Delimitter + "MoveEvent");
                createMoveEvent(itMoveEvents.Current, document, navGlobal);
            }
        }

        private void createCompletionEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            Boolean isUnit = false;

            // Set up component values
            String cType = "CompletionEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("ID") == null) ? String.Empty : navLocal.SelectSingleNode("ID").Value;
            id = id.Trim();
            if (id.ToLower().Equals("unit"))
            {
                isUnit = true;
            }
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null || isUnit)
            {
                if (!isUnit)
                {
                    String createEvent = navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                    // Scenario Link - Determine where to link
                    if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                    {
                        createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), createEvent, cName, "Scenario", String.Empty);
                    }
                    // EventID
                    createLink(VSGConfiguration, document, cName, cName, createEvent, "EventID", String.Empty);
                }
                if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletion.Unit", isUnit.ToString(), String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "CompletionEvent.Action", (navLocal.SelectSingleNode("Action") == null) ? String.Empty : navLocal.SelectSingleNode("Action").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "CompletionEvent.State", (navLocal.SelectSingleNode("NewState") == null) ? String.Empty : navLocal.SelectSingleNode("NewState").Value, String.Empty);


            #endregion

            // EngramRange
            if (navLocal.SelectSingleNode("EngramRange") != null)
            {
                createEngramRange(navLocal.SelectSingleNode("EngramRange"), document, navGlobal);
            }

            // Child Events
            XPathNodeIterator itDoThese = navLocal.Select("DoThis");
            while (itDoThese.MoveNext())
            {
                createEvents(itDoThese.Current, document, navGlobal);
            }
        }

        private void createSpeciesCompletionEvent(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "SpeciesCompletionEvent";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link and EventID
            String id = (navLocal.SelectSingleNode("Species") == null) ? String.Empty : navLocal.SelectSingleNode("Species").Value;
            id = id.Trim();
            if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", id)) != null)
            {
                String species = navGlobal.SelectSingleNode(String.Format("/Scenario/Species[Name='{0}']", id)).GetAttribute("name", navLocal.NamespaceURI);
                // Scenario Link - Determine where to link
                if (navLocal.SelectSingleNode("parent::node()").Name.Equals("Scenario"))
                {
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Reiterate"))
                {
                    String reiterate = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), reiterate, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Species_Completion_Event"))
                {
                    String speciesCompletionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), speciesCompletionEvent, cName, "Scenario", String.Empty);
                }
                else if (navLocal.SelectSingleNode("parent::*/parent::node()").Name.Equals("Completion_Event"))
                {
                    String completionEvent = navLocal.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", navLocal.NamespaceURI);
                    createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), completionEvent, cName, "Scenario", String.Empty);
                }
                // SpeciesCompletionEventSpecies
                createLink(VSGConfiguration, document, cName, cName, species, "SpeciesCompletionEventSpecies", String.Empty);
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletionEvent.Action", (navLocal.SelectSingleNode("Action") == null) ? String.Empty : navLocal.SelectSingleNode("Action").Value, String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "SpeciesCompletionEvent.State", (navLocal.SelectSingleNode("NewState") == null) ? String.Empty : navLocal.SelectSingleNode("NewState").Value, String.Empty);

            #endregion

            // Child Events
            XPathNodeIterator itDoThese = navLocal.Select("DoThis");
            while (itDoThese.MoveNext())
            {
                createEvents(itDoThese.Current, document, navGlobal);
            }
        }
        
        Int32 eventNum = 0;  // Global due to duplicate event names within container events.
        private void createEvents(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Events - Scenario level, Name them all first using an attribute.
            // Sometimes this was done using a child element. Attribute might be cleaner.
            // If so, the above things should get changed when time permits.
            XPathNodeIterator itEvents = navLocal.Select(exprEvents);
            while (itEvents.MoveNext())
            {
                //String parent = itEvents.Current.SelectSingleNode("parent::node()").GetAttribute("name", itEvents.Current.NamespaceURI);
                //if (parent.Equals(String.Empty))
                //    parent = itEvents.Current.SelectSingleNode("parent::*/parent::node()").GetAttribute("name", itEvents.Current.NamespaceURI);
                switch (itEvents.Current.Name)
                {
                    case "ChangeEngram":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "ChangeEngramEvent");
                        break;
                    case "CloseChatRoom":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "CloseChatRoomEvent");
                        break;
                    case "SendChatMessage":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "SendChatMessage");
                        break;
                    case "SendVoiceMessage":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "SendVoiceMessage");
                        break;
                    case "SendVoiceMessageToUser":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "SendVoiceMessageToUser");
                        break;
                    case "CloseVoiceChannel":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "CloseVoiceChannelEvent");
                        break;
                    case "Completion_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "CompletionEvent");
                        break;
                    case "Create_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.SelectSingleNode("ID").Value);
                        break;
                    case "FlushEvents":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "FlushEvent");
                        break;
                    case "Launch_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "LaunchEvent");
                        break;
                    case "WeaponLaunch_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "WeaponLaunchEvent");
                        break;
                    case "Move_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "MoveEvent");
                        break;
                    case "OpenChatRoom":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "OpenChatRoomEvent");
                        break;
                    case "OpenVoiceChannel":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "OpenVoiceChannelEvent");
                        break;
                    case "Reiterate":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "ReiterateEvent");
                        break;
                    case "RemoveEngram":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "RemoveEngramEvent");
                        break;
                    case "Reveal_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "RevealEvent");
                        break;
                    case "Species_Completion_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "SpeciesCompletionEvent");
                        break;
                    case "StateChange_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "StateChangeEvent");
                        break;
                    case "Transfer_Event":
                        itEvents.Current.CreateAttribute(String.Empty, "name", navGlobal.NamespaceURI, itEvents.Current.Name + eventNum++ + AME.Tools.ImportTool.Delimitter + "TransferEvent");
                        break;
                    default:
                        Console.WriteLine("Error in Import, when creating attribute for {0}.", itEvents.Current.Name);
                        break;
                }
            }
            itEvents = navLocal.Select(exprEvents);
            while (itEvents.MoveNext())
            {
                switch (itEvents.Current.Name)
                {
                    case "ChangeEngram":
                        createChangeEngramEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "CloseChatRoom":
                        createCloseChatRoomEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "SendChatMessage":
                        createSendChatMessageEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "SendVoiceMessage":
                        createSendVoiceMessageEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "SendVoiceMessageToUser":
                        createSendVoiceMessageToUserEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "CloseVoiceChannel":
                        createCloseVoiceChannelEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Completion_Event":
                        createCompletionEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Create_Event":
                        createCreateEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "FlushEvents":
                        createFlushEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Launch_Event":
                        createLaunchEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "WeaponLaunch_Event":
                        createWeaponLaunchEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Move_Event":
                        createMoveEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "OpenChatRoom":
                        createOpenChatRoomEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "OpenVoiceChannel":
                        createOpenVoiceChannelEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Reiterate":
                        createReiterateEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "RemoveEngram":
                        createRemoveEngramEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Reveal_Event":
                        createRevealEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Species_Completion_Event":
                        createSpeciesCompletionEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "StateChange_Event":
                        createStateChangeEvent(itEvents.Current, document, navGlobal);
                        break;
                    case "Transfer_Event":
                        createTransferEvent(itEvents.Current, document, navGlobal);
                        break;
                    default:
                        Console.WriteLine("Error in Import, when creating event {0}.", itEvents.Current.Name);
                        break;
                }
            }
        }

        private void createRule(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Rule";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            if (navLocal.SelectSingleNode("Object") == null)
                createParameter(VSGConfiguration, document, cName, "Component", "Rule.RuleType", "Object_1_Exists", String.Empty);
            else
                createParameter(VSGConfiguration, document, cName, "Component", "Rule.RuleType", "Object_2_State_Transition", String.Empty);
            createParameter(VSGConfiguration, document, cName, "Component", "Rule.Increment", (navLocal.SelectSingleNode("Increment") == null) ? String.Empty : navLocal.SelectSingleNode("Increment").Value, String.Empty);
            // THESE SHOULS BE LINKS!!!
            // NewState
            createParameter(VSGConfiguration, document, cName, "Component", "Rule.NewState", (navLocal.SelectSingleNode("NewState") == null) ? String.Empty : navLocal.SelectSingleNode("NewState").Value, String.Empty);
            // From
            createParameter(VSGConfiguration, document, cName, "Component", "Rule.FromState", (navLocal.SelectSingleNode("From") == null) ? String.Empty : navLocal.SelectSingleNode("From").Value, String.Empty);
            #endregion

            // Unit
            navLocal.SelectSingleNode("Unit").CreateAttribute(String.Empty, "name", navLocal.NamespaceURI, cName + "Unit" + AME.Tools.ImportTool.Delimitter + "Unit");
            createUnit(navLocal.SelectSingleNode("Unit"), document, navGlobal);

            // Object
            if (navLocal.SelectSingleNode("Object") != null)
            {
                navLocal.SelectSingleNode("Object").CreateAttribute(String.Empty, "name", navLocal.NamespaceURI, cName + "Object" + AME.Tools.ImportTool.Delimitter + "Object");
                createUnit(navLocal.SelectSingleNode("Object"), document, navGlobal);
            }
        }

        private void createUnit(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            String ruleName = navLocal.SelectSingleNode("parent::node()").GetAttribute("name", navLocal.NamespaceURI);

            // Set up component values
            String cType = "Actor";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // RuleUnit or RuleObject Link
            if(navLocal.Name.Equals("Unit"))
                createLink(VSGConfiguration, document, ruleName, ruleName, cName, "RuleUnit", String.Empty);
            else if (navLocal.Name.Equals("Object"))
                createLink(VSGConfiguration, document, ruleName, ruleName, cName, "RuleObject", String.Empty);

            #endregion

            // Create parameters
            #region parameters

            // Owner
            if (navLocal.SelectSingleNode("Owner") != null)
            {
                String owner = navLocal.SelectSingleNode("Owner").Value;
                if (owner.Equals("This"))
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.Who", "Myself", String.Empty);
                else if (owner.Equals("Hostile"))
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.Who", "Hostile", String.Empty);
                else if (owner.Equals("Friendly"))
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.Who", "Friendly", String.Empty);
            }
            // ID
            if (navLocal.SelectSingleNode("ID") != null)
            {
                String id = navLocal.SelectSingleNode("ID").Value;
                if (id.Equals("Any"))
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.What", "Any", String.Empty);
                else if (navGlobal.SelectSingleNode(String.Format("/Scenario/Species[@name='{0}']", id)) != null)
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.What", "Of_Species", String.Empty);
                    // ActorKind
                    createLink(VSGConfiguration, document, cName, cName, id, "ActorKind", String.Empty);
                }
                else if (navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)) != null) // C.E must be unique
                {
                    createParameter(VSGConfiguration, document, cName, "Component", "Actor.What", "Unit", String.Empty);
                    // ActorUnit
                    createLink(VSGConfiguration, document, cName, cName, navGlobal.SelectSingleNode(String.Format("/Scenario/Create_Event[ID='{0}']", id)).GetAttribute("name", navGlobal.NamespaceURI), "ActorUnit", String.Empty);
                }
            }
            // Zone
            if (navLocal.SelectSingleNode("Region") != null)
            {
                // ActorRegion
                String zones = (navLocal.SelectSingleNode("Region/Zone") == null) ? String.Empty : navLocal.SelectSingleNode("Region/Zone").Value;
                String[] activeRegions = zones.Split(',');
                foreach (String activeRegion in activeRegions)
                {
                    String r = activeRegion.Trim();
                    if (navGlobal.SelectSingleNode(String.Format("/Scenario/ActiveRegion[@name='{0}']", r)) != null)
                    {
                        createLink(VSGConfiguration, document, cName, cName, r, "ActorRegion", String.Empty);
                    }
                }
                if (navLocal.SelectSingleNode("Region/Relationship") != null)
                {
                    if (navLocal.SelectSingleNode("Region/Relationship").Value.Equals("InZone"))
                        createParameter(VSGConfiguration, document, cName, "Component", "Actor.Where", "In_Region", String.Empty);
                    else if (navLocal.SelectSingleNode("Region/Relationship").Value.Equals("NotInZone"))
                        createParameter(VSGConfiguration, document, cName, "Component", "Actor.Where", "Not_In_Region", String.Empty);
                }
            }
            else
            {
                createParameter(VSGConfiguration, document, cName, "Component", "Actor.Where", "Anywhere", String.Empty);
            }

            #endregion
        }

        private void createScore(XPathNavigator navLocal, XmlDocument document, XPathNavigator navGlobal)
        {
            // Set up component values
            String cType = "Score";
            String cName = navLocal.GetAttribute("name", navLocal.NamespaceURI);
            String cDescription = String.Empty;
            // Create component
            createComponent(VSGConfiguration, document, cType, cName, cDescription);

            // Create links
            #region links

            // Scenario Link
            createLink(VSGConfiguration, document, navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), navGlobal.SelectSingleNode("/Scenario").GetAttribute("name", navGlobal.NamespaceURI), cName, "Scenario", String.Empty);

            // ScoreRules
            String scoreRules = (navLocal.SelectSingleNode("Rules") == null) ? String.Empty : navLocal.SelectSingleNode("Rules").Value;
            String[] rules = scoreRules.Split(',');
            foreach (String rule in rules)
            {
                String r = rule.Trim();
                // Check to ensure member is valid
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/Rule[@name='{0}']", r)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, r, "ScoreRules", String.Empty);
                }
            }

            // ScoreApplies
            String scoresApplies = (navLocal.SelectSingleNode("Applies") == null) ? String.Empty : navLocal.SelectSingleNode("Applies").Value;
            String[] applies = scoresApplies.Split(',');
            foreach (String apply in applies)
            {
                String a = apply.Trim();
                // Check to ensure member is valid
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[@name='{0}']", a)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, a, "ScoreApplies", String.Empty);
                }
            }

            // ScoreViewers
            String scoreViewers = (navLocal.SelectSingleNode("Viewers") == null) ? String.Empty : navLocal.SelectSingleNode("Viewers").Value;
            String[] viewers = scoreViewers.Split(',');
            foreach (String viewer in viewers)
            {
                String v = viewer.Trim();
                // Check to ensure member is valid
                if (navGlobal.SelectSingleNode(String.Format("/Scenario/DecisionMaker[@name='{0}']", v)) != null)
                {
                    createLink(VSGConfiguration, document, cName, cName, v, "ScoreViewers", String.Empty);
                }
            }

            #endregion

            // Create parameters
            #region parameters

            createParameter(VSGConfiguration, document, cName, "Component", "Score.Initial", (navLocal.SelectSingleNode("Initial") == null) ? String.Empty : navLocal.SelectSingleNode("Initial").Value, String.Empty);

            #endregion
        }
        
        private Boolean validate(String uri, out IXPathNavigable iNav)
        {
            XmlDocument document = new XmlDocument();
            document.Load(uri);

            //nav = document.CreateNavigator();

            //XmlNodeReader nodeReader = new XmlNodeReader(document);

            Boolean isValid = false;
            XmlReader schemaDatabase = validatingController.GetXSD("DDDSchema_4_1_1.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaDatabase);
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

            XmlReader reader = XmlReader.Create(uri, settings);

            try
            {
                while (reader.Read());
                isValid = true;
                iNav = document;
            }
            catch (System.Exception e)
            {
                iNav = new XmlDocument();
                System.Windows.Forms.MessageBox.Show(e.Message, "Validation Error");
            }
            finally
            {
                reader.Close();
            }
            return isValid;
        }

        private void validationEventHandler(object sender, ValidationEventArgs args)
        {
            String message = String.Empty;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    message = "Import Error: " + args.Message;
                    break;
                case XmlSeverityType.Warning:
                    message = "Import Warning: " + args.Message;
                    break;
            }

            throw new System.Xml.Schema.XmlSchemaValidationException(String.Format(args.Message + System.Environment.NewLine + "Line number: {0} at Line position: {1}", args.Exception.LineNumber, args.Exception.LinePosition));
        }

        //private XmlElement getGrouping(XmlDocument document)
        //{
        //    XmlElement section = document.CreateElement("grouping");

        //    XmlElement componentTable = document.CreateElement("componentTable");
        //    section.AppendChild(componentTable);
        //    XmlElement linkTable = document.CreateElement("linkTable");
        //    section.AppendChild(linkTable);
        //    XmlElement parameterTable = document.CreateElement("parameterTable");
        //    section.AppendChild(parameterTable);

        //    return section;
        //}
    }
}