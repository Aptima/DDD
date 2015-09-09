using System;
using System.Collections.Generic;
using System.Text;
using AME.Adapters;
using AME.Controllers;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.IO;
using System.Text.RegularExpressions;

namespace VSG.Adapters
{
    public class DDD_4_0_2_Exporter : ControllerToXmlAdapter<IXPathNavigable>
    {
        private DDD_4_0_2_ExporterSettings settings;
        private XPathExpression expression;

        public DDD_4_0_2_Exporter(DDD_4_0_2_ExporterSettings settings) : base((ControllerToXmlSettings)settings)
        {
            this.settings = settings;
        }

        public DDD_4_0_2_Exporter(Controller controller,
                                  RootController rootController,
                                  Boolean treeDataFormat) : base(controller,rootController,treeDataFormat)
        {
            this.settings = new DDD_4_0_2_ExporterSettings(controller, rootController, treeDataFormat);
        }

        #region IExportDataAdapter Members

        public override IXPathNavigable Process()
        {
            CreateDoc();
            XPathNavigator navigator = doc.CreateNavigator();
            expression = navigator.Compile("Component[@Type='Engram'] | " +
                                           "Component[@Type='ChangeEngramEvent'] | " +
                                           "Component[@Type='RemoveEngramEvent'] | " +
                                           "Component[@Type='CreateEvent'] | " +
                                           "Component[@Type='RevealEvent'] | " +
                                           "Component[@Type='CompletionEvent'] | " +
                                           "Component[@Type='LaunchEvent'] | " +
                                           "Component[@Type='OpenChatRoomEvent'] | " +
                                           "Component[@Type='CloseChatRoomEvent'] | " +
                                           "Component[@Type='ReiterateEvent'] | " +
                                           "Component[@Type='FlushEvent'] | " +
                                           "Component[@Type='MoveEvent'] | " +
                                           "Component[@Type='SpeciesCompletionEvent'] | " +
                                           "Component[@Type='StateChangeEvent'] | " +
                                           "Component[@Type='TransferEvent']");

            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            document.AppendChild(declaration);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            createScenario(navigator, document);

            if (validate(document))
            {
                return (IXPathNavigable)document;
            }
            else
            {
                return new XmlDocument();
            }
        }

        #endregion

        private void createScenario(XPathNavigator navigator, XmlDocument document)
        {
            // Create element
            XmlElement root = document.CreateElement("Scenario");
            root.IsEmpty = false;

            // Add schema information to root.
            XmlAttribute schema = document.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schema.Value = @"DDDSchema_4_0_2.xsd";
            root.SetAttributeNode(schema);

            // ScenarioName
            XPathNavigator navScenarioName = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']");
            if (navScenarioName != null)
            {
                createScenarioName(navScenarioName, document, root, navigator);
            }

            // Description
            XPathNavigator navDescription = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']");
            if (navDescription != null)
            {
                createDescription(navDescription, document, root, navigator);
            }

            // ClientSideAssetTransfer
            XPathNavigator navAllowAssetTransfers = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']");
            if (navAllowAssetTransfers != null)
            {
                createClientSideAssetTransfer(navAllowAssetTransfers, document, root, navigator);
            }

            // Playfield
            XPathNavigator navPlayfield = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Playfield']");
            if (navPlayfield != null)
            {
                createPlayfield(navPlayfield, document, root, navigator);
            }

            // LandRegions
            XPathNodeIterator itLandRegions = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='LandRegion']");
            while (itLandRegions.MoveNext())
            {
                createLandRegion(itLandRegions.Current, document, root, navigator);
            }

            // ActiveRegions
            XPathNodeIterator itActiveRegions = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='ActiveRegion']");
            while (itActiveRegions.MoveNext())
            {
                createActiveRegion(itActiveRegions.Current, document, root, navigator);
            }

            // Teams
            XPathNodeIterator itTeams = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Team']");
            while (itTeams.MoveNext())
            {
                createTeam(itTeams.Current, document, root, navigator);
            }

            // DeceisionMakers
            XPathNodeIterator itDecisionMakers = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='DecisionMaker']");
            while (itDecisionMakers.MoveNext())
            {
                createDecisionMaker(itDecisionMakers.Current, document, root, navigator);
            }

            // Networks
            XPathNodeIterator itNetworks = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Network']");
            while (itNetworks.MoveNext())
            {
                createNetwork(itNetworks.Current, document, root, navigator);
            }

            // DefineEngram
            XPathNodeIterator itDefineEngrams = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Engram']");
            while (itDefineEngrams.MoveNext())
            {
                createDefineEngram(itDefineEngrams.Current, document, root, navigator);
            }

            // Sensors
            XPathNodeIterator itSensors = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Sensor']");
            while (itSensors.MoveNext())
            {
                createSensor(itSensors.Current, document, root, navigator);
            }

            // TimeToAttack
            XPathNavigator navTimeToAttack = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']");
            if (navTimeToAttack != null)
            {
                createTimeToAttack(navTimeToAttack, document, root, navigator);
            }

            // Genera - Create the 3 default
            createGenera(navigator, document, root, navigator);

            // Species
            XPathNodeIterator itSpecies = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Species']");
            orderSpecies(itSpecies, document, root, navigator);

            // Events
            XPathNavigator navScenario = navigator.SelectSingleNode("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']");
            XPathNodeIterator itScenarioEvents = navScenario.Select(expression);
            while (itScenarioEvents.MoveNext())
            {
                createEvents(itScenarioEvents.Current, document, root, navigator);
            }

            // Events
            XPathNodeIterator itScenarioCreateEvent = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='CreateEvent']");
            while (itScenarioCreateEvent.MoveNext())
            {
                XPathNodeIterator itScenarioCreateEventEvents = itScenarioCreateEvent.Current.Select(expression);
                while (itScenarioCreateEventEvents.MoveNext())
                {
                    createEvents(itScenarioCreateEventEvents.Current, document, root, navigator);
                }
            }

            XPathNodeIterator itRules = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Rule']");
            while (itRules.MoveNext())
            {
                createRule(itRules.Current, document, root, navigator);
            }

            XPathNodeIterator itScores = navigator.Select("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Type='Score']");
            while (itScores.MoveNext())
            {
                createScore(itScores.Current, document, root, navigator);
            }
            
            document.AppendChild(root);
        }

        private void createScenarioName(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create ScenarioName
            if (navLocal != null)
            {
                XPathNavigator navScenarioName = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Scenario Name' and @category='Scenario']");
                if (navScenarioName != null)
                {
                    XmlElement scenarioName = document.CreateElement("ScenarioName");
                    scenarioName.IsEmpty = false;
                    String value = navScenarioName.GetAttribute("value", navScenarioName.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        scenarioName.InnerXml = value;
                        parentElement.AppendChild(scenarioName);
                    }
                }
            }
        }

        private void createDescription(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Description
            if (navLocal != null)
            {
                XPathNavigator navDescription = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Description' and @category='Scenario']");
                if (navDescription != null)
                {
                    XmlElement description = document.CreateElement("Description");
                    description.IsEmpty = false;
                    String value = navDescription.GetAttribute("value", navDescription.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        description.InnerXml = value;
                        parentElement.AppendChild(description);
                    }
                }
            }
        }

        private void createClientSideAssetTransfer(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Description
            if (navLocal != null)
            {
                XPathNavigator navAllowAssetTransfers = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Allow Asset Transfers' and @category='Scenario']");
                if (navAllowAssetTransfers != null)
                {
                    XmlElement clientSideAssetTransfer = document.CreateElement("ClientSideAssetTransfer");
                    clientSideAssetTransfer.IsEmpty = false;
                    String value = navAllowAssetTransfers.GetAttribute("value", navAllowAssetTransfers.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        clientSideAssetTransfer.InnerXml = value;
                        parentElement.AppendChild(clientSideAssetTransfer);
                    }
                }
            }
        }

        private void createPlayfield(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Playfield
            XmlElement playField = document.CreateElement("Playfield");
            playField.IsEmpty = false;

            parentElement.AppendChild(playField);

            // MapFileName
            XPathNavigator navMapFileName = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Map Filename' and @category='Playfield']");
            if (navMapFileName != null)
            {
                XmlElement mapFileName = document.CreateElement("MapFileName");
                mapFileName.IsEmpty = false;
                String value = navMapFileName.GetAttribute("value", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    FileInfo file = new FileInfo(value);
                    mapFileName.InnerXml = file.Name;
                    playField.AppendChild(mapFileName);
                }
            }

            // IconLibrary
            XPathNavigator navIconLibraryName = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon Library' and @category='Playfield']");
            if (navIconLibraryName != null)
            {
                XmlElement iconLibrary = document.CreateElement("IconLibrary");
                iconLibrary.IsEmpty = false;
                String value = navIconLibraryName.GetAttribute("value", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    FileInfo file = new FileInfo(value);
                    iconLibrary.InnerXml = file.Name;
                    playField.AppendChild(iconLibrary);
                }
            }

            // UtmZone
            XPathNavigator navUtmZone = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='UTM Zone' and @category='Playfield']");
            if (navUtmZone != null)
            {
                XmlElement utmZone = document.CreateElement("UtmZone");
                utmZone.IsEmpty = false;
                String value = navUtmZone.GetAttribute("value", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    utmZone.InnerXml = value;
                    playField.AppendChild(utmZone);
                }
            }

            // VerticalScale            
            XPathNavigator navVerticalScale = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Vertical Scale' and @category='Playfield']");
            if (navVerticalScale != null)
            {
                XmlElement verticalScale = document.CreateElement("VerticalScale");
                verticalScale.IsEmpty = false;
                String value = navVerticalScale.GetAttribute("value", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    verticalScale.InnerXml = value;
                    playField.AppendChild(verticalScale);
                }
            }

            // HorizontalScale
            XPathNavigator navHorizontalScale = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Horizontal Scale' and @category='Playfield']");
            if (navHorizontalScale != null)
            {
                XmlElement horizontalScale = document.CreateElement("HorizontalScale");
                horizontalScale.IsEmpty = false;
                String value = navHorizontalScale.GetAttribute("value", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    horizontalScale.InnerXml = value;
                    playField.AppendChild(horizontalScale);
                }
            }
        }

        private void createLandRegion(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create LandRegion
            XmlElement landRegion = document.CreateElement("LandRegion");
            landRegion.IsEmpty = false;

            parentElement.AppendChild(landRegion);

            // ID
            if (navLocal != null)
            {
                XmlElement id = document.CreateElement("ID");
                id.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    id.InnerXml = value;
                    landRegion.AppendChild(id);                    
                }
            }

            // Vertex
            XPathNavigator navVertices = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Polygon Points' and @category='Location']");
            if (navVertices != null)
            {
                String polygonPoints = navVertices.GetAttribute("value", navLocal.NamespaceURI);
                Regex r = new Regex(
                    "\\((?<vertex>[^)]*)\\)",
                    RegexOptions.CultureInvariant |
                    RegexOptions.Compiled
                    );
                MatchCollection matches = r.Matches(polygonPoints);
                foreach (Match match in matches)
                {
                    XmlElement vertex = document.CreateElement("Vertex");
                    vertex.IsEmpty = false;
                    vertex.InnerXml = match.Groups[1].Value.Replace(",", String.Empty);
                    landRegion.AppendChild(vertex);
                }
            }
        }

        private void createActiveRegion(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create ActiveRegion
            XmlElement activeRegion = document.CreateElement("ActiveRegion");
            activeRegion.IsEmpty = false;

            parentElement.AppendChild(activeRegion);

            // ID
            if (navLocal != null)
            {
                XmlElement id = document.CreateElement("ID");
                id.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    id.InnerXml = value;
                    activeRegion.AppendChild(id);
                }
            }

            // Vertex
            XPathNavigator navVertices = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Polygon Points' and @category='Location']");
            if (navVertices != null)
            {
                String polygonPoints = navVertices.GetAttribute("value", navLocal.NamespaceURI);

                Regex r = new Regex(
                    "\\((?<vertex>[^)]*)\\)",
                    RegexOptions.CultureInvariant |
                    RegexOptions.Compiled
                    );
                MatchCollection matches = r.Matches(polygonPoints);
                foreach (Match match in matches)
                {
                    XmlElement vertex = document.CreateElement("Vertex");
                    vertex.IsEmpty = false;
                    vertex.InnerXml = match.Groups[1].Value.Replace(",", String.Empty);
                    activeRegion.AppendChild(vertex);
                }
            }

            // Start
            XPathNavigator navStart = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Start' and @category='Active Region']");
            if (navStart != null)
            {
                XmlElement start = document.CreateElement("Start");
                start.IsEmpty = false;
                String value = navStart.GetAttribute("value", navStart.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    start.InnerXml = value;
                    activeRegion.AppendChild(start);
                }
            }

            // End
            XPathNavigator navEnd = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='End' and @category='Active Region']");
            if (navEnd != null)
            {
                XmlElement end = document.CreateElement("End");
                end.IsEmpty = false;
                String value = navEnd.GetAttribute("value", navEnd.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    end.InnerXml = value;
                    activeRegion.AppendChild(end);
                }
            }

            // SpeedMultiplier
            XPathNavigator navSpeedMultiplier = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Speed Multiplier' and @category='Active Region']");
            if (navSpeedMultiplier != null)
            {
                XmlElement speedMultiplier = document.CreateElement("SpeedMultiplier");
                speedMultiplier.IsEmpty = false;
                String value = navSpeedMultiplier.GetAttribute("value", navSpeedMultiplier.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    speedMultiplier.InnerXml = value;
                    activeRegion.AppendChild(speedMultiplier);
                }
            }

            // BlocksMovement
            XPathNavigator navBlocksMovement = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Blocks Movement' and @category='Active Region']");
            if (navBlocksMovement != null)
            {
                XmlElement blocksMovement = document.CreateElement("BlocksMovement");
                blocksMovement.IsEmpty = false;
                String value = navBlocksMovement.GetAttribute("value", navBlocksMovement.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    blocksMovement.InnerXml = value;
                    activeRegion.AppendChild(blocksMovement);
                }
            }

            // SensorsBlocked
            String activeRegionId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navActiveRegionSensor = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActiveRegionSensor']/Component[@ID='{0}' and @Type='ActiveRegion']", activeRegionId));
            if (navActiveRegionSensor != null)
            {
                XmlElement sensorsBlocked = document.CreateElement("SensorsBlocked");
                sensorsBlocked.IsEmpty = false;
                XPathNodeIterator itSensors = navActiveRegionSensor.Select("Component[@Type='Sensor']");
                List<String> sensors = new List<String>();
                while (itSensors.MoveNext())
                {
                    String value = itSensors.Current.GetAttribute("Name", itSensors.Current.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        sensors.Add(value);
                    }
                }
                if (sensors.Count > 0)
                {
                    sensorsBlocked.InnerXml = String.Join(",", sensors.ToArray());
                    activeRegion.AppendChild(sensorsBlocked);
                }
            }

            // IsVisible
            XPathNavigator navIsVisible = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Is Visible' and @category='Active Region']");
            if (navIsVisible != null)
            {
                XmlElement isVisible = document.CreateElement("IsVisible");
                isVisible.IsEmpty = false;
                String value = navIsVisible.GetAttribute("value", navIsVisible.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    isVisible.InnerXml = value;
                    activeRegion.AppendChild(isVisible);
                }
            }

            // Color
            XPathNavigator navColor = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Color' and @category='Color']");
            if (navColor != null)
            {
                XmlElement color = document.CreateElement("Color");
                color.IsEmpty = false;
                String value = navColor.GetAttribute("value", navColor.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    color.InnerXml = value;
                    activeRegion.AppendChild(color);
                }
            }
        }

        private void createTeam(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Team
            XmlElement team = document.CreateElement("Team");
            team.IsEmpty = false;

            parentElement.AppendChild(team);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    team.AppendChild(name);
                }
            }

            // Against
            String teamId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            String linkType = "TeamAgainst" + "_" + teamId;
            XPathNavigator navTeamAgainst = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='{1}']/Component[@ID='{0}' and @Type='Team']", teamId, linkType));
            if (navTeamAgainst != null)
            {
                XmlElement against = document.CreateElement("Against");
                against.IsEmpty = false;
                XPathNodeIterator itTeams = navTeamAgainst.Select("Component[@Type='Team']");
                List<String> teamList = new List<String>();
                while (itTeams.MoveNext())
                {
                    String value = itTeams.Current.GetAttribute("Name", itTeams.Current.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        teamList.Add(value);
                    }
                }
                if (teamList.Count > 0)
                {
                    against.InnerXml = String.Join(",", teamList.ToArray());
                    team.AppendChild(against);
                }
            }
        }

        private void createDecisionMaker(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create DecisionMaker
            XmlElement decisionMaker = document.CreateElement("DecisionMaker");
            decisionMaker.IsEmpty = false;

            parentElement.AppendChild(decisionMaker);

            // Role
            XPathNavigator navRole = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Role' and @category='DecisionMaker']");
            if (navRole != null)
            {
                XmlElement role = document.CreateElement("Role");
                role.IsEmpty = false;
                String value = navRole.GetAttribute("value", navRole.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    role.InnerXml = value;
                    decisionMaker.AppendChild(role);
                }
            }

            // Identifier
            if (navLocal != null)
            {
                XmlElement identifier = document.CreateElement("Identifier");
                identifier.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    identifier.InnerXml = value;
                    decisionMaker.AppendChild(identifier);
                }
            }

            // Color
            XPathNavigator navColor = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Color' and @category='DecisionMaker']");
            if (navColor != null)
            {
                XmlElement color = document.CreateElement("Color");
                color.IsEmpty = false;
                String value = navColor.GetAttribute("value", navColor.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    color.InnerXml = value;
                    decisionMaker.AppendChild(color);
                }
            }

            // Briefing
            XPathNavigator navBriefing = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Briefing' and @category='DecisionMaker']");
            if (navBriefing != null)
            {
                XmlElement briefing = document.CreateElement("Briefing");
                briefing.IsEmpty = false;
                String value = navBriefing.GetAttribute("value", navBriefing.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    briefing.InnerXml = value;
                    decisionMaker.AppendChild(briefing);
                }
            }

            // Team
            String decisionMakerId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navDecisionMakerTeam = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='DecisionMakerTeam']/Component[@ID='{0}' and @Type='DecisionMaker']", decisionMakerId));
            if (navDecisionMakerTeam != null)
            {
                XPathNavigator navTeam = navDecisionMakerTeam.SelectSingleNode("Component[@Type='Team']");
                if (navTeam != null)
                {
                    XmlElement team = document.CreateElement("Team");
                    team.IsEmpty = false;
                    String value = navTeam.GetAttribute("Name", navTeam.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        team.InnerXml = value;
                        decisionMaker.AppendChild(team);
                    }
                }
            }
        }

        private void createNetwork(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Network
            XmlElement network = document.CreateElement("Network");
            network.IsEmpty = false;

            parentElement.AppendChild(network);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    network.AppendChild(name);
                }
            }

            // Member
            String networkId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navNetworkMembers = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='NetworkMembers']/Component[@ID='{0}' and @Type='Network']", networkId));
            if (navNetworkMembers != null)
            {
                XmlElement member = document.CreateElement("Member");
                member.IsEmpty = false;
                XPathNodeIterator itMembers = navNetworkMembers.Select("Component[@Type='DecisionMaker']");
                List<String> memberList = new List<String>();
                while (itMembers.MoveNext())
                {
                    String value = itMembers.Current.GetAttribute("Name", itMembers.Current.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        memberList.Add(value);
                    }
                }
                if (memberList.Count > 0)
                {
                    member.InnerXml = String.Join(",", memberList.ToArray());
                    network.AppendChild(member);
                }
            }
        }

        private void createSensor(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Sensor
            XmlElement sensor = document.CreateElement("Sensor");
            sensor.IsEmpty = false;

            parentElement.AppendChild(sensor);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    sensor.AppendChild(name);
                }
            }

            // (Attribute or Engram) or Extent
            XPathNavigator navAttributeSensor = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Attribute_Sensor' and @category='Sensor']");
            if (navAttributeSensor != null)
            {
                // Attribute
                Boolean attributeSensor = Boolean.Parse(navAttributeSensor.GetAttribute("value", navAttributeSensor.NamespaceURI));
                if (attributeSensor)
                {
                    XPathNavigator navAttribute = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Attribute' and @category='Sensor']");
                    if (navAttribute != null)
                    {
                        XmlElement attribute = document.CreateElement("Attribute");
                        attribute.IsEmpty = false;
                        String value = navAttribute.GetAttribute("value", navAttribute.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            attribute.InnerXml = value;
                            sensor.AppendChild(attribute);
                        }
                    }
                }
            }
            XPathNavigator navCustomAttributeSensor = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Custom_Attribute_Sensor' and @category='Sensor']");
            if (navCustomAttributeSensor != null)
            {
                // Attribute
                Boolean customAttributeSensor = Boolean.Parse(navCustomAttributeSensor.GetAttribute("value", navCustomAttributeSensor.NamespaceURI));
                if (customAttributeSensor)
                {
                    XPathNavigator navCustomAttribute = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Custom_Attribute' and @category='Sensor']");
                    if (navCustomAttribute != null)
                    {
                        XmlElement engram = document.CreateElement("Engram");
                        engram.IsEmpty = false;
                        String value = navCustomAttribute.GetAttribute("value", navCustomAttribute.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            engram.InnerXml = value;
                            sensor.AppendChild(engram);
                        }
                    }
                }
            }
            XPathNavigator navGlobalSensor = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Global_Sensor' and @category='Sensor']");
            if (navGlobalSensor != null)
            {
                // Extent
                Boolean globalSensor = Boolean.Parse(navGlobalSensor.GetAttribute("value", navGlobalSensor.NamespaceURI));
                if (globalSensor)
                {
                    XPathNavigator navRange = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Range' and @category='Sensor']");
                    if (navRange != null)
                    {
                        XmlElement extent = document.CreateElement("Extent");
                        extent.IsEmpty = false;
                        String value = navRange.GetAttribute("value", navRange.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            extent.InnerXml = value;
                            sensor.AppendChild(extent);
                        }
                    }
                }
            }

            XPathNodeIterator itSensorRanges = navLocal.Select("Component[@Type='SensorRange']");
            while (itSensorRanges.MoveNext())
            {
                createCone(itSensorRanges.Current, document, sensor, navGlobal);
            }
        }

        private void createCone(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Cone
            XmlElement cone = document.CreateElement("Cone");
            cone.IsEmpty = false;

            parentElement.AppendChild(cone);

            // Spread
            XPathNavigator navSpread = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Spread' and @category='SensorRange']");
            if (navSpread != null)
            {
                XmlElement spread = document.CreateElement("Spread");
                spread.IsEmpty = false;
                String value = navSpread.GetAttribute("value", navSpread.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    spread.InnerXml = value;
                    cone.AppendChild(spread);
                }
            }

            // Extent
            XPathNavigator navRange = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Range' and @category='SensorRange']");
            if (navRange != null)
            {
                XmlElement range = document.CreateElement("Extent");
                range.IsEmpty = false;
                String value = navRange.GetAttribute("value", navRange.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    range.InnerXml = value;
                    cone.AppendChild(range);
                }
            }

            // Direction
            List<String> xyz = new List<String>();
            XPathNavigator navX = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='X' and @category='SensorRange']");
            if (navX != null)
            {
                String value = navX.GetAttribute("value", navX.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navY = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Y' and @category='SensorRange']");
            if (navY != null)
            {
                String value = navY.GetAttribute("value", navY.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navZ = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Z' and @category='SensorRange']");
            if (navZ != null)
            {
                String value = navZ.GetAttribute("value", navZ.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            if (xyz.Count > 0)
            {
                XmlElement direction = document.CreateElement("Direction");
                direction.IsEmpty = false;
                direction.InnerXml = String.Join(" ", xyz.ToArray());
                cone.AppendChild(direction);
            }

            // Level
            XPathNavigator navLevel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Level' and @category='SensorRange']");
            if (navLevel != null)
            {
                XmlElement level = document.CreateElement("Level");
                level.IsEmpty = false;
                String value = navLevel.GetAttribute("value", navLevel.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    level.InnerXml = value;
                    cone.AppendChild(level);
                }
            }
        }

        private void createTimeToAttack(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create TimeToAttack
            if (navLocal != null)
            {
                XPathNavigator navTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time To Attack' and @category='Scenario']");
                if (navTimeToAttack != null)
                {
                    XmlElement timeToAttack = document.CreateElement("TimeToAttack");
                    timeToAttack.IsEmpty = false;
                    String value = navTimeToAttack.GetAttribute("value", navTimeToAttack.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        timeToAttack.InnerXml = value;
                        parentElement.AppendChild(timeToAttack);
                    }
                }
            }
        }

        private void createGenera(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Genera

            // AirObject
            XmlElement airObject = document.CreateElement("Genus");
            XmlElement airObjectName = document.CreateElement("Name");
            airObjectName.InnerXml = "AirObject";
            airObject.AppendChild(airObjectName);
            parentElement.AppendChild(airObject);

            // LandObject
            XmlElement landObject = document.CreateElement("Genus");
            XmlElement landObjectName = document.CreateElement("Name");
            landObjectName.InnerXml = "LandObject";
            landObject.AppendChild(landObjectName);
            parentElement.AppendChild(landObject);

            // SeaObject
            XmlElement seaObject = document.CreateElement("Genus");
            XmlElement seaObjectName = document.CreateElement("Name");
            seaObjectName.InnerXml = "SeaObject";
            seaObject.AppendChild(seaObjectName);
            parentElement.AppendChild(seaObject);
        }

        private void orderSpecies(XPathNodeIterator itSpecies, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            Dictionary<String, XPathNavigator> speciesList = new Dictionary<String, XPathNavigator>();
            while (itSpecies.MoveNext())
            {
                speciesList.Add(itSpecies.Current.GetAttribute("Name", itSpecies.Current.NamespaceURI), itSpecies.Current.CreateNavigator());
            }
            Dictionary<String, XPathNavigator> orderedSpeciesList = compareSpeciesByBase(speciesList, navGlobal);

            foreach (XPathNavigator species in orderedSpeciesList.Values)
            {
                createSpecies(species, document, parentElement, navGlobal);
            }
        }

        private Dictionary<String, XPathNavigator> compareSpeciesByBase(Dictionary<String, XPathNavigator> speciesList, XPathNavigator navGlobal)
        {
            Dictionary<String, XPathNavigator> orderedSpeciesList = new Dictionary<String, XPathNavigator>();

            foreach (String speciesName in speciesList.Keys)
            {
                XPathNavigator x = speciesList[speciesName];

                String speciesId = x.GetAttribute("ID", x.NamespaceURI);
                XPathNavigator navSpeciesType = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='SpeciesType']/Component/Component[@ID='{0}' and @Type='Species']", speciesId));
                if (navSpeciesType != null)
                {
                    XPathNodeIterator itInheritedList = navSpeciesType.Select("descendant-or-self::Component[@Type='Species']");
                    // reverse the list
                    List<XPathNavigator> rList = new List<XPathNavigator>();
                    while (itInheritedList.MoveNext())
                    {
                        XPathNavigator current = itInheritedList.Current.CreateNavigator();
                        rList.Insert(0, current);
                    }

                    foreach (XPathNavigator nav in rList)
                    {
                        String baseSpeciesName = nav.GetAttribute("Name", nav.NamespaceURI);
                        XPathNavigator y = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@Name='{0}']", baseSpeciesName));

                        if (y != null)
                        {
                            if (!orderedSpeciesList.ContainsKey(baseSpeciesName))
                            {
                                orderedSpeciesList.Add(baseSpeciesName, y);
                            }
                        }
                    }
                }
            }

            return orderedSpeciesList;
        }

        private void createSpecies(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Species
            XmlElement species = document.CreateElement("Species");
            species.IsEmpty = false;

            parentElement.AppendChild(species);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    species.AppendChild(name);
                }
            }

            // Base
            XPathNavigator navExistingSpecies = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='ExistingSpecies' and @category='Species']");
            if (navExistingSpecies != null)
            {
                Boolean existingSpecies = Boolean.Parse(navExistingSpecies.GetAttribute("value", navExistingSpecies.NamespaceURI));
                if (existingSpecies)
                {
                    String speciesId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
                    XPathNavigator navSpeciesType = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='SpeciesType']/Component/Component[@ID='{0}' and @Type='Species']", speciesId));
                    if (navSpeciesType != null)
                    {
                        XPathNavigator navSpecies = navSpeciesType.SelectSingleNode("Component[@Type='Species']");
                        if (navSpecies != null)
                        {
                            XmlElement baseSpecies = document.CreateElement("Base");
                            baseSpecies.IsEmpty = false;
                            String value = navSpecies.GetAttribute("Name", navSpecies.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                baseSpecies.InnerXml = value;
                                species.AppendChild(baseSpecies);
                            }
                        }
                    }
                }
                else
                {
                    XPathNavigator navLandObject = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='LandObject' and @category='Species']");
                    Boolean landObject = Boolean.Parse(navLandObject.GetAttribute("value", navLandObject.NamespaceURI));
                    if (landObject)
                    {
                        XmlElement baseSpecies = document.CreateElement("Base");
                        baseSpecies.IsEmpty = false;
                        baseSpecies.InnerXml = "LandObject";
                        species.AppendChild(baseSpecies);
                    }
                    XPathNavigator navAirObject = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='AirObject' and @category='Species']");
                    Boolean airObject = Boolean.Parse(navAirObject.GetAttribute("value", navAirObject.NamespaceURI));
                    if (airObject)
                    {
                        XmlElement baseSpecies = document.CreateElement("Base");
                        baseSpecies.IsEmpty = false;
                        baseSpecies.InnerXml = "AirObject";
                        species.AppendChild(baseSpecies);
                    }
                    XPathNavigator navSeaObject = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='SeaObject' and @category='Species']");
                    Boolean seaObject = Boolean.Parse(navSeaObject.GetAttribute("value", navSeaObject.NamespaceURI));
                    if (seaObject)
                    {
                        XmlElement baseSpecies = document.CreateElement("Base");
                        baseSpecies.IsEmpty = false;
                        baseSpecies.InnerXml = "SeaObject";
                        species.AppendChild(baseSpecies);
                    }
                }
            }

            // Size
            XPathNavigator navSize = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Size' and @category='Species']");
            if (navSize != null)
            {
                XmlElement size = document.CreateElement("Size");
                size.IsEmpty = false;
                String value = navSize.GetAttribute("value", navSize.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    size.InnerXml = value;
                    species.AppendChild(size);
                }
            }

            // IsWeapon
            XPathNavigator navIsWeapon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='IsWeapon' and @category='Species']");
            if (navIsWeapon != null)
            {
                XmlElement isWeapon = document.CreateElement("IsWeapon");
                isWeapon.IsEmpty = false;
                String value = navIsWeapon.GetAttribute("value", navIsWeapon.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    isWeapon.InnerXml = value;
                    species.AppendChild(isWeapon);
                }
            }

            // RemoveOnDestruction
            XPathNavigator navRemoveOnDestruction = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='RemoveOnDestruction' and @category='Species']");
            if (navRemoveOnDestruction != null)
            {
                XmlElement removeOnDestruction = document.CreateElement("RemoveOnDestruction");
                removeOnDestruction.IsEmpty = false;
                String value = navRemoveOnDestruction.GetAttribute("value", navRemoveOnDestruction.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    removeOnDestruction.InnerXml = value;
                    species.AppendChild(removeOnDestruction);
                }
            }

            XPathNavigator navFullyFunctional = navLocal.SelectSingleNode("Component[@Name='FullyFunctional']");
            if (navFullyFunctional != null)
            {
                createFullyFunctional(navFullyFunctional, document, species, navGlobal);
            }

            XPathNodeIterator itDefineSates = navLocal.Select("Component[@Name!='FullyFunctional' and @Type='State']");
            while (itDefineSates.MoveNext())
            {
                createDefineState(itDefineSates.Current, document, species, navGlobal);
            }
        }

        private void createFullyFunctional(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create FullyFunctional
            XmlElement fullyFunctional = document.CreateElement("FullyFunctional");
            fullyFunctional.IsEmpty = false;

            parentElement.AppendChild(fullyFunctional);

            Boolean inheritedSpecies = false;
            if (navLocal != null)
            {
                XPathNavigator navSpecies = navLocal.SelectSingleNode("parent::node()");
                if (navSpecies != null)
                {
                    XPathNavigator navExistingSpecies = navSpecies.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='ExistingSpecies' and @category='Species']");
                    if (navExistingSpecies != null)
                    {
                        if (Boolean.Parse(navExistingSpecies.GetAttribute("value", navExistingSpecies.NamespaceURI)))
                        {
                            inheritedSpecies = true;
                        }
                    }
                }
            }
            String stateName = String.Empty;
            if (navLocal != null)
            {
                stateName = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
            }

            // Icon
            XPathNavigator navOverrideIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon' and @category='State']");
            if (navOverrideIcon != null)
            {
                if (Boolean.Parse(navOverrideIcon.GetAttribute("value", navOverrideIcon.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon' and @category='Image']");
                    if (navIcon != null)
                    {
                        XmlElement icon = document.CreateElement("Icon");
                        icon.IsEmpty = false;
                        String value = navIcon.GetAttribute("value", navOverrideIcon.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            icon.InnerXml = value;
                            fullyFunctional.AppendChild(icon);
                        }
                    }
                }
            }

            // StateParameters
            if (navLocal != null)
            {
                createStateParameters(navLocal, document, fullyFunctional, navGlobal);
            }
        }

        private void createStateParameters(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            Boolean inheritedSpecies = false;
            XPathNavigator navSpecies = navLocal.SelectSingleNode("parent::node()");
            if (navSpecies != null)
            {
                XPathNavigator navExistingSpecies = navSpecies.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='ExistingSpecies' and @category='Species']");
                if (navExistingSpecies != null)
                {
                    if (Boolean.Parse(navExistingSpecies.GetAttribute("value", navExistingSpecies.NamespaceURI)))
                    {
                        inheritedSpecies = true;
                    }
                }
            }
            String stateName = String.Empty;
            if (navLocal != null)
            {
                stateName = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
            }

            // Create StateParameters
            XmlElement stateParameters = document.CreateElement("StateParameters");
            stateParameters.IsEmpty = false;

            // Stealable
            XPathNavigator navOverrideStealable = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideStealable' and @category='State']");
            if (navOverrideStealable != null)
            {
                if (Boolean.Parse(navOverrideStealable.GetAttribute("value", navOverrideStealable.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navStealable = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Stealable' and @category='Image']");
                    if (navStealable != null)
                    {
                        XmlElement stealable = document.CreateElement("Stealable");
                        stealable.IsEmpty = false;
                        String value = navStealable.GetAttribute("value", navStealable.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            stealable.InnerXml = value;
                            stateParameters.AppendChild(stealable);
                        }
                    }
                }
            }

            // LaunchDuration
            XPathNavigator navOverrideLaunchDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideLaunchDuration' and @category='State']");
            if (navOverrideLaunchDuration != null)
            {
                if (Boolean.Parse(navOverrideLaunchDuration.GetAttribute("value", navOverrideLaunchDuration.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navLaunchDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='LaunchDuration' and @category='State']");
                    if (navLaunchDuration != null)
                    {
                        XmlElement launchDuration = document.CreateElement("LaunchDuration");
                        launchDuration.IsEmpty = false;
                        String value = navLaunchDuration.GetAttribute("value", navLaunchDuration.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            launchDuration.InnerXml = value;
                            stateParameters.AppendChild(launchDuration);
                        }
                    }
                }
            }

            // DockingDuration
            XPathNavigator navOverrideDockingDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideDockingDuration' and @category='State']");
            if (navOverrideDockingDuration != null)
            {
                if (Boolean.Parse(navOverrideDockingDuration.GetAttribute("value", navOverrideDockingDuration.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navDockingDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='DockingDuration' and @category='State']");
                    if (navDockingDuration != null)
                    {
                        XmlElement dockingDuration = document.CreateElement("DockingDuration");
                        dockingDuration.IsEmpty = false;
                        String value = navDockingDuration.GetAttribute("value", navDockingDuration.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            dockingDuration.InnerXml = value;
                            stateParameters.AppendChild(dockingDuration);
                        }
                    }
                }
            }

            // TimeToAttack
            XPathNavigator navOverrideTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideTimeToAttack' and @category='State']");
            if (navOverrideTimeToAttack != null)
            {
                if (Boolean.Parse(navOverrideTimeToAttack.GetAttribute("value", navOverrideTimeToAttack.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='TimeToAttack' and @category='State']");
                    if (navTimeToAttack != null)
                    {
                        XmlElement timeToAttack = document.CreateElement("TimeToAttack");
                        timeToAttack.IsEmpty = false;
                        String value = navTimeToAttack.GetAttribute("value", navTimeToAttack.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            timeToAttack.InnerXml = value;
                            stateParameters.AppendChild(timeToAttack);
                        }
                    }
                }
            }

            // MaximumSpeed
            XPathNavigator navOverrideMaxSpeed = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideMaxSpeed' and @category='State']");
            if (navOverrideMaxSpeed != null)
            {
                if (Boolean.Parse(navOverrideMaxSpeed.GetAttribute("value", navOverrideMaxSpeed.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navMaxSpeed = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='MaxSpeed' and @category='State']");
                    if (navMaxSpeed != null)
                    {
                        XmlElement maximumSpeed = document.CreateElement("MaximumSpeed");
                        maximumSpeed.IsEmpty = false;
                        String value = navMaxSpeed.GetAttribute("value", navMaxSpeed.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            maximumSpeed.InnerXml = value;
                            stateParameters.AppendChild(maximumSpeed);
                        }
                    }
                }
            }

            // FuelCapacity
            XPathNavigator navOverrideFuelCapacity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelCapacity' and @category='State']");
            if (navOverrideFuelCapacity != null)
            {
                if (Boolean.Parse(navOverrideFuelCapacity.GetAttribute("value", navOverrideFuelCapacity.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navFuelCapacity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelCapacity' and @category='State']");
                    if (navFuelCapacity != null)
                    {
                        XmlElement fuelCapacity = document.CreateElement("FuelCapacity");
                        fuelCapacity.IsEmpty = false;
                        String value = navFuelCapacity.GetAttribute("value", navFuelCapacity.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            fuelCapacity.InnerXml = value;
                            stateParameters.AppendChild(fuelCapacity);
                        }
                    }
                }
            }

            // InitialFuelLoad
            XPathNavigator navOverrideInitialFuel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideInitialFuel' and @category='State']");
            if (navOverrideInitialFuel != null)
            {
                if (Boolean.Parse(navOverrideInitialFuel.GetAttribute("value", navOverrideInitialFuel.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navInitialFuel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialFuel' and @category='State']");
                    if (navInitialFuel != null)
                    {
                        XmlElement initialFuelLoad = document.CreateElement("InitialFuelLoad");
                        initialFuelLoad.IsEmpty = false;
                        String value = navInitialFuel.GetAttribute("value", navInitialFuel.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            initialFuelLoad.InnerXml = value;
                            stateParameters.AppendChild(initialFuelLoad);
                        }
                    }
                }
            }

            // FuelConsumptionRate
            XPathNavigator navOverrideFuelConsumption = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelConsumption' and @category='State']");
            if (navOverrideFuelConsumption != null)
            {
                if (Boolean.Parse(navOverrideFuelConsumption.GetAttribute("value", navOverrideFuelConsumption.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navFuelConsumption = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelConsumption' and @category='State']");
                    if (navFuelConsumption != null)
                    {
                        XmlElement fuelConsumptionRate = document.CreateElement("FuelConsumptionRate");
                        fuelConsumptionRate.IsEmpty = false;
                        String value = navFuelConsumption.GetAttribute("value", navFuelConsumption.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            fuelConsumptionRate.InnerXml = value;
                            stateParameters.AppendChild(fuelConsumptionRate);
                        }
                    }
                }
            }

            // FuelDepletionState
            String speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
            XPathNavigator navOverrideFuelDepletionState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelDepletionState' and @category='State']");
            if (navOverrideFuelDepletionState != null)
            {
                if (Boolean.Parse(navOverrideFuelDepletionState.GetAttribute("value", navOverrideFuelDepletionState.NamespaceURI)) | (!inheritedSpecies & stateName.Equals("FullyFunctional")))
                {
                    XPathNavigator navFuelDepletionState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelDepletionState' and @category='State']");
                    if (navFuelDepletionState != null)
                    {
                        XmlElement fuelDepletionState = document.CreateElement("FuelDepletionState");
                        fuelDepletionState.IsEmpty = false;
                        String value = navFuelDepletionState.GetAttribute("value", navFuelDepletionState.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            if (isStateValid(speciesId, value, navGlobal))
                            {
                                fuelDepletionState.InnerXml = value;
                                stateParameters.AppendChild(fuelDepletionState);
                            }
                        }
                    }
                }
            }

            // Sense
            String stateId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navStateSensor = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='StateSensor']/Component[@ID='{0}' and @Type='State']", stateId));
            if (navStateSensor != null)
            {
                XPathNodeIterator itSensors = navStateSensor.Select("Component[@Type='Sensor']");
                while (itSensors.MoveNext())
                {
                    XmlElement sense = document.CreateElement("Sense");
                    sense.IsEmpty = false;
                    String value = itSensors.Current.GetAttribute("Name", itSensors.Current.NamespaceURI);
                    if (! value.Equals(String.Empty))
                    {
                        sense.InnerXml = value;
                        stateParameters.AppendChild(sense);
                    }
                }
            }

            // Capability
            if (navLocal != null)
            {
                XPathNodeIterator itCapabilities = navLocal.Select("Component[@Type='Capability']");
                while (itCapabilities.MoveNext())
                {
                    createCapability(itCapabilities.Current, document, stateParameters, navGlobal);
                }
            }

            // SingletonVulnerability
            if (navLocal != null)
            {
                XPathNodeIterator itSingletons = navLocal.Select("Component[@Type='Singleton']");
                while (itSingletons.MoveNext())
                {
                    createSingletonVulnerability(itSingletons.Current, document, stateParameters, navGlobal);
                }
            }

            // ComboVulnerability
            if (navLocal != null)
            {
                XPathNodeIterator itCombos = navLocal.Select("Component[@Type='Combo']");
                while (itCombos.MoveNext())
                {
                    createComboVulnerability(itCombos.Current, document, stateParameters, navGlobal);
                }
            }

            // Emitter
            if (navLocal != null)
            {
                XPathNodeIterator itEmitters = navLocal.Select("Component[@Type='Emitter']");
                while (itEmitters.MoveNext())
                {
                       createEmitter(itEmitters.Current, document, stateParameters, navGlobal);
                }
            }

            if (stateParameters.HasChildNodes) // If no children dont create the node.
                parentElement.AppendChild(stateParameters);
        }

        private void createCapability(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Capability
            XmlElement capability = document.CreateElement("Capability");
            capability.IsEmpty = false;

            parentElement.AppendChild(capability);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    capability.AppendChild(name);
                }
            }

            // Proximity
            XPathNodeIterator itProximities = navLocal.Select("Component[@Type='Proximity']");
            while (itProximities.MoveNext())
            {
                createProximity(itProximities.Current, document, capability, navGlobal);
            }
        }

        private void createProximity(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Proximity
            XmlElement proximity = document.CreateElement("Proximity");
            proximity.IsEmpty = false;

            parentElement.AppendChild(proximity);

            // Range
            XPathNavigator navRange = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Range' and @category='Proximity']");
            if (navRange != null)
            {
                XmlElement range = document.CreateElement("Range");
                range.IsEmpty = false;
                String value = navRange.GetAttribute("value", navRange.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    range.InnerXml = value;
                    proximity.AppendChild(range);
                }
            }

            XPathNodeIterator itEffects = navLocal.Select("Component[@Type='Effect']");
            while (itEffects.MoveNext())
            {
                createEffect(itEffects.Current, document, proximity, navGlobal);
            }
        }

        private void createEffect(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Effect
            XmlElement effect = document.CreateElement("Effect");
            effect.IsEmpty = false;

            parentElement.AppendChild(effect);

            // Intensity
            XPathNavigator navIntensity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Intensity' and @category='Effect']");
            if (navIntensity != null)
            {
                XmlElement intensity = document.CreateElement("Intensity");
                intensity.IsEmpty = false;
                String value = navIntensity.GetAttribute("value", navIntensity.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    intensity.InnerXml = value;
                    effect.AppendChild(intensity);
                }
            }

            // Probability
            XPathNavigator navProbability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Probability' and @category='Effect']");
            if (navProbability != null)
            {
                XmlElement probability = document.CreateElement("Probability");
                probability.IsEmpty = false;
                String value = navProbability.GetAttribute("value", navProbability.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    probability.InnerXml = value;
                    effect.AppendChild(probability);
                }
            }
        }

        private void createSingletonVulnerability(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create SingletonVulnerability
            XmlElement singletonVulnerability = document.CreateElement("SingletonVulnerability");
            singletonVulnerability.IsEmpty = false;

            parentElement.AppendChild(singletonVulnerability);

            // Capability
            XPathNavigator navCapability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Capability' and @category='Singleton']");
            if (navCapability != null)
            {
                XmlElement capability = document.CreateElement("Capability");
                capability.IsEmpty = false;
                String value = navCapability.GetAttribute("value", navCapability.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    capability.InnerXml = value;
                    singletonVulnerability.AppendChild(capability);
                }
            }

            // Transitions
            XmlElement transitions = document.CreateElement("Transitions");
            transitions.IsEmpty = false;
            singletonVulnerability.AppendChild(transitions);
            XPathNodeIterator itTransitions = navLocal.Select("Component[@Type='Transition']");
            while (itTransitions.MoveNext())
            {
                createTransitions(itTransitions.Current, document, transitions, navGlobal);
            }           
        }

        private void createTransitions(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            if (navLocal != null)
            {
                // Effect
                XPathNavigator navEffect = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Intensity' and @category='Transition']");
                if (navEffect != null)
                {
                    XmlElement effect = document.CreateElement("Effect");
                    effect.IsEmpty = false;
                    String value = navEffect.GetAttribute("value", navEffect.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        effect.InnerXml = value;
                        parentElement.AppendChild(effect);
                    }
                }

                // Range
                XPathNavigator navRange = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Range' and @category='Transition']");
                if (navRange != null)
                {
                    XmlElement range = document.CreateElement("Range");
                    range.IsEmpty = false;
                    String value = navRange.GetAttribute("value", navRange.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        range.InnerXml = value;
                        parentElement.AppendChild(range);
                    }
                }

                // Probability
                XPathNavigator navProbability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Probability' and @category='Transition']");
                if (navProbability != null)
                {
                    XmlElement probability = document.CreateElement("Probability");
                    probability.IsEmpty = false;
                    String value = navProbability.GetAttribute("value", navProbability.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        probability.InnerXml = value;
                        parentElement.AppendChild(probability);
                    }
                }

                // State
                XPathNavigator navSpecies = navLocal.SelectSingleNode("ancestor::Component[@Type='Species']");
                String speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);

                XPathNavigator navState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='State' and @category='Transition']");
                if (navState != null)
                {
                    XmlElement state = document.CreateElement("State");
                    state.IsEmpty = false;
                    String value = navState.GetAttribute("value", navState.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        if (isStateValid(speciesId, value, navGlobal))
                        {
                            state.InnerXml = value;
                            parentElement.AppendChild(state);
                        }
                    }
                }
            }
        }

        private Boolean isStateValid(String speciedId, String stateName, XPathNavigator navGlobal)
        {
            if (!speciedId.Equals(String.Empty) && !stateName.Equals(String.Empty) && navGlobal != null)
            {
                if (speciedId.ToLower().Equals("unit"))
                {
                    XPathNodeIterator itStates = navGlobal.Select("/LinkTypes/Link[@type='Scenario']/Component/Component[@Type='Species']/Component[@Type='State']");
                    while (itStates.MoveNext())
                    {
                        if (itStates.Current.GetAttribute("Name", itStates.Current.NamespaceURI).Equals(stateName))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    XPathNavigator navSpeciesType = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='SpeciesType']/Component/Component[@ID='{0}']", speciedId));
                    if (navSpeciesType != null)
                    {
                        XPathNodeIterator itInheritedSpecies = navSpeciesType.Select("descendant-or-self::Component[@Type='Species']");
                        while (itInheritedSpecies.MoveNext())
                        {
                            String inheritedSpeciesId = itInheritedSpecies.Current.GetAttribute("ID", itInheritedSpecies.Current.NamespaceURI);
                            XPathNodeIterator itInheritedStates = itInheritedSpecies.Current.Select(String.Format("/LinkTypes/Link[@type='Scenario']/Component/Component[@ID='{0}']/Component[@Type='State']", inheritedSpeciesId));
                            while (itInheritedStates.MoveNext())
                            {
                                if (itInheritedStates.Current.GetAttribute("Name", itInheritedStates.Current.NamespaceURI).Equals(stateName))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void createComboVulnerability(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create ComboVulnerability
            XmlElement comboVulnerability = document.CreateElement("ComboVulnerability");
            comboVulnerability.IsEmpty = false;

            parentElement.AppendChild(comboVulnerability);

            // Contribution
            XPathNodeIterator itContributions = navLocal.Select("Component[@Type='Contribution']");
            while (itContributions.MoveNext())
            {
                createContribution(itContributions.Current, document, comboVulnerability, navGlobal);
            }

            // NewState
            XPathNavigator navSpecies = navLocal.SelectSingleNode("ancestor::Component[@Type='Species']");
            String speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);

            XPathNavigator navState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='State' and @category='Combo']");
            if (navState != null)
            {
                XmlElement newState = document.CreateElement("NewState");
                newState.IsEmpty = false;
                String value = navState.GetAttribute("value", navState.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    if (isStateValid(speciesId, value, navGlobal))
                    {
                        newState.InnerXml = value;
                        comboVulnerability.AppendChild(newState);
                    }
                }
            }
        }

        private void createContribution(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Contribution
            XmlElement contribution = document.CreateElement("Contribution");
            contribution.IsEmpty = false;

            parentElement.AppendChild(contribution);

            // Capability
            XPathNavigator navCapability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Capability' and @category='Contribution']");
            if (navCapability != null)
            {
                XmlElement capability = document.CreateElement("Capability");
                capability.IsEmpty = false;
                String value = navCapability.GetAttribute("value", navCapability.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    capability.InnerXml = value;
                    contribution.AppendChild(capability);
                }
            }

            // Effect
            XPathNavigator navEffect = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Intensity' and @category='Contribution']");
            if (navEffect != null)
            {
                XmlElement effect = document.CreateElement("Effect");
                effect.IsEmpty = false;
                String value = navEffect.GetAttribute("value", navEffect.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    effect.InnerXml = value;
                    contribution.AppendChild(effect);
                }
            }

            // Range
            XPathNavigator navRange = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Range' and @category='Contribution']");
            if (navRange != null)
            {
                XmlElement range = document.CreateElement("Range");
                range.IsEmpty = false;
                String value = navRange.GetAttribute("value", navRange.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    range.InnerXml = value;
                    contribution.AppendChild(range);
                }
            }

            // Probability
            XPathNavigator navProbability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Probability' and @category='Contribution']");
            if (navProbability != null)
            {
                XmlElement probability = document.CreateElement("Probability");
                probability.IsEmpty = false;
                String value = navProbability.GetAttribute("value", navProbability.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    probability.InnerXml = value;
                    contribution.AppendChild(probability);
                }
            }
        }

        private void createEmitter(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Emitter
            XmlElement emitter = document.CreateElement("Emitter");
            emitter.IsEmpty = false;

            parentElement.AppendChild(emitter);

            // Attribute or Engram
            XPathNavigator navAttributeEmitter = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Attribute_Emitter' and @category='Emitter']");
            if (navAttributeEmitter != null)
            {
                // Attribute
                Boolean attributeSensor = Boolean.Parse(navAttributeEmitter.GetAttribute("value", navAttributeEmitter.NamespaceURI));
                if (attributeSensor)
                {
                    XPathNavigator navAttribute = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Attribute' and @category='Emitter']");
                    if (navAttribute != null)
                    {
                        XmlElement attribute = document.CreateElement("Attribute");
                        attribute.IsEmpty = false;
                        String value = navAttribute.GetAttribute("value", navAttribute.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            attribute.InnerXml = value;
                            emitter.AppendChild(attribute);
                        }
                    }
                }
            }
            XPathNavigator navCustomAttributeEmitter = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Custom_Attribute_Emitter' and @category='Emitter']");
            if (navCustomAttributeEmitter != null)
            {
                // Attribute
                Boolean customAttributeSensor = Boolean.Parse(navCustomAttributeEmitter.GetAttribute("value", navCustomAttributeEmitter.NamespaceURI));
                if (customAttributeSensor)
                {
                    XPathNavigator navCustomAttribute = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Custom_Attribute' and @category='Emitter']");
                    if (navCustomAttribute != null)
                    {
                        XmlElement engram = document.CreateElement("Engram");
                        engram.IsEmpty = false;
                        String value = navCustomAttribute.GetAttribute("value", navCustomAttribute.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            engram.InnerXml = value;
                            emitter.AppendChild(engram);
                        }
                    }
                }
            }

            // NormalEmitter
            XmlElement normalEmitter = document.CreateElement("NormalEmitter");
            normalEmitter.IsEmpty = false;
            XPathNodeIterator itLevels = navLocal.Select("Component[@Type='Level']");
            while (itLevels.MoveNext())
            {
                // Level
                XmlElement level = document.CreateElement("Level");
                level.IsEmpty = false;
                String value = itLevels.Current.GetAttribute("Name", itLevels.Current.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    level.InnerXml = value;
                    normalEmitter.AppendChild(level);
                }
                createNormalEmitter(itLevels.Current, document, normalEmitter, navGlobal);
            }
            if (normalEmitter.HasChildNodes)
                emitter.AppendChild(normalEmitter);

        }

        private void createNormalEmitter(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Variance
            XPathNavigator navVariance = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Variance' and @category='Level']");
            if (navVariance != null)
            {
                // Attribute
                Boolean isVariance = Boolean.Parse(navVariance.GetAttribute("value", navVariance.NamespaceURI));
                if (isVariance)
                {
                    XPathNavigator navPercentage = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Percentage' and @category='Level']");
                    if (navPercentage != null)
                    {
                        XmlElement variance = document.CreateElement("Variance");
                        variance.IsEmpty = false;
                        String value = navPercentage.GetAttribute("value", navPercentage.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            variance.InnerXml = value;
                            parentElement.AppendChild(variance);
                        }
                    }
                }
            }

            // Percent
            XPathNavigator navProbability = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Probability' and @category='Level']");
            if (navProbability != null)
            {
                // Attribute
                Boolean isProbability = Boolean.Parse(navProbability.GetAttribute("value", navProbability.NamespaceURI));
                if (isProbability)
                {
                    XPathNavigator navPercentage = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Percentage' and @category='Level']");
                    if (navPercentage != null)
                    {
                        XmlElement percent = document.CreateElement("Percent");
                        percent.IsEmpty = false;
                        String value = navPercentage.GetAttribute("value", navPercentage.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            percent.InnerXml = value;
                            parentElement.AppendChild(percent);
                        }
                    }
                }
            }
        }

        private void createDefineState(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create DefineState
            XmlElement defineState = document.CreateElement("DefineState");
            defineState.IsEmpty = false;

            parentElement.AppendChild(defineState);

            // State
            if (navLocal != null)
            {
                XmlElement state = document.CreateElement("State");
                state.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    state.InnerXml = value;
                    defineState.AppendChild(state);
                }
            }

            // Icon
            XPathNavigator navOverrideIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon' and @category='State']");
            if (navOverrideIcon != null)
            {
                if (Boolean.Parse(navOverrideIcon.GetAttribute("value", navOverrideIcon.NamespaceURI)))
                {
                    XPathNavigator navIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon' and @category='Image']");
                    if (navIcon != null)
                    {
                        XmlElement icon = document.CreateElement("Icon");
                        icon.IsEmpty = false;
                        String value = navIcon.GetAttribute("value", navOverrideIcon.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            icon.InnerXml = value;
                            defineState.AppendChild(icon);
                        }
                    }
                }
            }

            // StateParameters
            if (navLocal != null)
            {
                createStateParameters(navLocal, document, defineState, navGlobal);
            }
        }

        private void createEvents(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            if (navLocal != null)
            {
                switch (navLocal.GetAttribute("Type", navLocal.NamespaceURI))
                {
                    case "ChangeEngramEvent":
                        createChangeEngram(navLocal, document, parentElement, navGlobal);
                        break;
                    case "CloseChatRoomEvent":
                        createCloseChatRoom(navLocal, document, parentElement, navGlobal);
                        break;
                    case "CompletionEvent":
                        createCompletion_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "CreateEvent":
                        createCreate_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "FlushEvent":
                        createFlushEvents(navLocal, document, parentElement, navGlobal);
                        break;
                    case "LaunchEvent":
                        createLaunch_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "MoveEvent":
                        createMove_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "OpenChatRoomEvent":
                        createOpenChatRoom(navLocal, document, parentElement, navGlobal);
                        break;
                    case "ReiterateEvent":
                        createReiterate(navLocal, document, parentElement, navGlobal);
                        break;
                    case "RemoveEngramEvent":
                        createRemoveEngram(navLocal, document, parentElement, navGlobal);
                        break;
                    case "RevealEvent":
                        createReveal_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "SpeciesCompletionEvent":
                        createSpecies_Completion_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "StateChangeEvent":
                        createStateChange_Event(navLocal, document, parentElement, navGlobal);
                        break;
                    case "TransferEvent":
                        createTransfer_Event(navLocal, document, parentElement, navGlobal);
                        break;
                }
            }
        }
        
        private void createCreate_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Create_Event
            XmlElement create_Event = document.CreateElement("Create_Event");
            create_Event.IsEmpty = false;

            parentElement.AppendChild(create_Event);

            // ID
            if (navLocal != null)
            {
                XmlElement id = document.CreateElement("ID");
                id.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    id.InnerXml = value;
                    create_Event.AppendChild(id);
                }
            }

            // Kind
            String createEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navCreateEventKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}' and @Type='CreateEvent']", createEventId));
            if (navCreateEventKind != null)
            {
                XPathNavigator navSpecies = navCreateEventKind.SelectSingleNode("Component[@Type='Species']");
                if (navSpecies != null)
                {
                    XmlElement kind = document.CreateElement("Kind");
                    kind.IsEmpty = false;
                    String value = navSpecies.GetAttribute("Name", navSpecies.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        kind.InnerXml = value;
                        create_Event.AppendChild(kind);
                    }
                }
            }

            // Owner
            XPathNavigator navScenario = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='Scenario']/Component[@Type='Scenario']/Component[@ID='{0}' and @Type='CreateEvent']", createEventId));
            if (navScenario != null)
            {
                XPathNavigator navDecisionMaker = navScenario.SelectSingleNode("Component[@Type='DecisionMaker']");
                if (navDecisionMaker != null)
                {
                    XmlElement owner = document.CreateElement("Owner");
                    owner.IsEmpty = false;
                    String value = navDecisionMaker.GetAttribute("Name", navDecisionMaker.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        owner.InnerXml = value;
                        create_Event.AppendChild(owner);
                    }
                }
            }

            // Subplatform
            XPathNodeIterator itSubplatforms = navLocal.Select("Component[@Type='Subplatform']");
            while (itSubplatforms.MoveNext())
            {
                createSubplatform(itSubplatforms.Current, document, create_Event, navGlobal);
            }

            XPathNodeIterator itAdoptPlatforms = navLocal.Select("Component[@Type='AdoptPlatform']");
            while (itAdoptPlatforms.MoveNext())
            {
                createAdopt_Platform(itAdoptPlatforms.Current, document, create_Event, navGlobal);
            }
        }

        private void createSubplatform(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Subplatform
            XmlElement subplatform = document.CreateElement("Subplatform");
            subplatform.IsEmpty = false;

            parentElement.AppendChild(subplatform);

            // Kind
            String subplatformId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navWeaponKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='SubplatformKind']/Component[@ID='{0}' and @Type='Subplatform']", subplatformId));
            if (navWeaponKind != null)
            {
                XPathNavigator navSpecies = navWeaponKind.SelectSingleNode("Component[@Type='Species']");
                if (navSpecies != null)
                {
                    XmlElement kind = document.CreateElement("Kind");
                    kind.IsEmpty = false;
                    String value = navSpecies.GetAttribute("Name", navSpecies.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        kind.InnerXml = value;
                        subplatform.AppendChild(kind);
                    }
                }
            }

            // Armament
            XPathNodeIterator itArmaments = navLocal.Select("Component[@Type='Armament']");
            while (itArmaments.MoveNext())
            {
                createArmament(itArmaments.Current, document, subplatform, navGlobal);
            }

            // Docked
            XPathNavigator navDocked = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='DockedCount' and @category='Subplatform']");
            if (navDocked != null)
            {
                XmlElement docked = document.CreateElement("Docked");
                docked.IsEmpty = false;
                subplatform.AppendChild(docked);
                XmlElement count = document.CreateElement("Count");
                count.IsEmpty = false;
                String value = navDocked.GetAttribute("value", navDocked.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    count.InnerXml = value;
                    docked.AppendChild(count);
                }
            }
        }

        private void createArmament(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Armament
            XmlElement armament = document.CreateElement("Armament");
            armament.IsEmpty = false;

            parentElement.AppendChild(armament);

            // Weapon
            String armamentId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navScenario = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='WeaponKind']/Component[@ID='{0}' and @Type='Armament']", armamentId));
            if (navScenario != null)
            {
                XPathNavigator navSpecies = navScenario.SelectSingleNode("Component[@Type='Species']");
                if (navSpecies != null)
                {
                    XmlElement weapon = document.CreateElement("Weapon");
                    weapon.IsEmpty = false;
                    String value = navSpecies.GetAttribute("Name", navSpecies.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        weapon.InnerXml = value;
                        armament.AppendChild(weapon);
                    }
                }
            }

            // Count
            XPathNavigator navCount = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='WeaponCount' and @category='Armament']");
            if (navCount != null)
            {
                XmlElement count = document.CreateElement("Count");
                count.IsEmpty = false;
                String value = navCount.GetAttribute("value", navCount.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    count.InnerXml = value;
                    armament.AppendChild(count);
                }
            }
        }

        private void createLaunch_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Launch_Event
            XmlElement launch_Event = document.CreateElement("Launch_Event");
            launch_Event.IsEmpty = false;

            parentElement.AppendChild(launch_Event);

            // Parent
            String speciesId = String.Empty;
            String launchEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement parent = document.CreateElement("Parent");
                    parent.IsEmpty = false;
                    String value = "UNIT";
                    speciesId = value;
                    if (!value.Equals(String.Empty))
                    {
                        parent.InnerXml = value;
                        launch_Event.AppendChild(parent);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='LaunchEvent']", launchEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            String createEventId = navUnit.GetAttribute("ID", navUnit.NamespaceURI);
                            XPathNavigator navSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}']/Component[@Type='Species']", createEventId));
                            if (navSpecies != null)
                            {
                                speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                            }
                            XmlElement parent = document.CreateElement("Parent");
                            parent.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                parent.InnerXml = value;
                                launch_Event.AppendChild(parent);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='LaunchEvent']", launchEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, launch_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //launch_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Unit
                    //    XPathNavigator navIsEngramUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
                    //    if (navIsEngramUnit != null)
                    //    {
                    //        if (Boolean.Parse(navIsEngramUnit.GetAttribute("value", navIsEngramUnit.NamespaceURI)))
                    //        {
                    //            XmlElement parent = document.CreateElement("Parent");
                    //            parent.IsEmpty = false;
                    //            String value = "UNIT";
                    //            speciesId = value;
                    //            if (!value.Equals(String.Empty))
                    //            {
                    //                parent.InnerXml = value;
                    //                launch_Event.AppendChild(parent);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='LaunchEvent']", launchEventId));
                    //            if (navEventID != null)
                    //            {
                    //                XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                    //                if (navUnit != null)
                    //                {
                    //                    String createEventId = navUnit.GetAttribute("ID", navUnit.NamespaceURI);
                    //                    XPathNavigator navSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}']/Component[@Type='Species']", createEventId));
                    //                    if (navSpecies != null)
                    //                    {
                    //                        speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                    //                    }
                    //                    XmlElement parent = document.CreateElement("Parent");
                    //                    parent.IsEmpty = false;
                    //                    String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                    //                    if (!value.Equals(String.Empty))
                    //                    {
                    //                        parent.InnerXml = value;
                    //                        launch_Event.AppendChild(parent);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='LaunchEvent']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    launch_Event.AppendChild(time);
                }
            }

            // Kind
            XPathNavigator navLaunchEventSubplatform = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='LaunchEventSubplatform']/Component[@ID='{0}' and @Type='LaunchEvent']", launchEventId));
            if (navLaunchEventSubplatform != null)
            {
                XPathNavigator navSubplatform = navLaunchEventSubplatform.SelectSingleNode("Component[@Type='Subplatform']");
                if (navSubplatform != null)
                {
                    String subplatformId = navSubplatform.GetAttribute("ID", navSubplatform.NamespaceURI);
                    XPathNavigator navSubplatformKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='SubplatformKind']/Component[@ID='{0}' and @Type='Subplatform']", subplatformId));
                    if (navSubplatformKind != null)
                    {
                        XPathNavigator navKind = navSubplatformKind.SelectSingleNode("Component[@Type='Species']");
                        if (navKind != null)
                        {
                            XmlElement kind = document.CreateElement("Kind");
                            kind.IsEmpty = false;
                            String value = navKind.GetAttribute("Name", navSubplatform.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                kind.InnerXml = value;
                                launch_Event.AppendChild(kind);
                            }
                        }
                    }
                }
            }

            // RelativeLocation
            List<String> xyz = new List<String>();
            XPathNavigator navX = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='X' and @category='RelativeLocation']");
            if (navX != null)
            {
                String value = navX.GetAttribute("value", navX.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navY = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Y' and @category='RelativeLocation']");
            if (navY != null)
            {
                String value = navY.GetAttribute("value", navY.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navZ = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Z' and @category='RelativeLocation']");
            if (navZ != null)
            {
                String value = navZ.GetAttribute("value", navZ.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            if (xyz.Count > 0)
            {
                XmlElement relativeLocation = document.CreateElement("RelativeLocation");
                relativeLocation.IsEmpty = false;
                relativeLocation.InnerXml = String.Join(" ", xyz.ToArray());
                launch_Event.AppendChild(relativeLocation);
            }


            // InitialState
            XPathNavigator navInitialState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialState' and @category='LaunchEvent']");
            if (navInitialState != null)
            {
                XmlElement initialState = document.CreateElement("InitialState");
                initialState.IsEmpty = false;
                String value = navInitialState.GetAttribute("value", navInitialState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        initialState.InnerXml = value;
                        launch_Event.AppendChild(initialState);
                    }
                }
            }

            // StartupParameters
            XPathNodeIterator itStartupParameters = navLocal.Select("ComponentParameters/Parameter/Parameter[@displayedName='OverrideStealable' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideLaunchDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideDockingDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideTimeToAttack' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideMaxSpeed' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelCapacity' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideInitialFuel' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelConsumption' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelDepletionState' and @category='StartupParameters']");
            if (itStartupParameters.Count > 0)
            {
                XmlElement startupParameters = document.CreateElement("StartupParameters");
                startupParameters.IsEmpty = false;

                while (itStartupParameters.MoveNext())
                {
                    switch (itStartupParameters.Current.GetAttribute("displayedName", itStartupParameters.Current.NamespaceURI))
                    {
                        // Stealable
                        case "OverrideStealable":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navStealable = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Stealable' and @category='StartupParameters']");
                                if (navStealable != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "Stealable";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navStealable.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // LaunchDuration
                        case "OverrideLaunchDuration":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navLaunchDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='LaunchDuration' and @category='StartupParameters']");
                                if (navLaunchDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "LaunchDuration";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navLaunchDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // DockingDuration
                        case "OverrideDockingDuration":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navDockingDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='DockingDuration' and @category='StartupParameters']");
                                if (navDockingDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "DockingDuration";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navDockingDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // TimeToAttack
                        case "OverrideTimeToAttack":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='TimeToAttack' and @category='StartupParameters']");
                                if (navTimeToAttack != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "TimeToAttack";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navTimeToAttack.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // MaximumSpeed
                        case "OverrideMaxSpeed":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navMaxSpeed = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='MaxSpeed' and @category='StartupParameters']");
                                if (navMaxSpeed != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "MaximumSpeed";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navMaxSpeed.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelCapacity
                        case "OverrideFuelCapacity":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelCapacity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelCapacity' and @category='StartupParameters']");
                                if (navFuelCapacity != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelCapacity";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelCapacity.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // InitialFuelLoad
                        case "OverrideInitialFuel":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navInitialFuel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialFuel' and @category='StartupParameters']");
                                if (navInitialFuel != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "InitialFuelLoad";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navInitialFuel.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelConsumptionRate
                        case "OverrideFuelConsumption":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelConsumption = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelConsumption' and @category='StartupParameters']");
                                if (navFuelConsumption != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelConsumptionRate";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelConsumption.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // Icon
                        case "OverrideIcon":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon' and @category='StartupParameters']");
                                if (navIcon != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "Icon";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navIcon.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelDepletionState
                        case "OverrideFuelDepletionState":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelDepletionState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelDepletionState' and @category='StartupParameters']");
                                if (navFuelDepletionState != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelDepletionState";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelDepletionState.GetAttribute("value", String.Empty);
                                    if (isStateValid(speciesId, value, navGlobal))
                                    {
                                        if (!value.Equals(String.Empty))
                                        {
                                            setting.InnerXml = value;
                                            startupParameters.AppendChild(setting);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                if (startupParameters.HasChildNodes)
                    launch_Event.AppendChild(startupParameters);

            }
        }

        private void createEngramRange(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            try
            {
                XPathNavigator navEvent = navLocal.SelectSingleNode("parent::node()");
                String eventId = navEvent.GetAttribute("ID", navEvent.NamespaceURI);

                XmlElement engramRange = document.CreateElement("EngramRange");
                engramRange.IsEmpty = false;
                parentElement.AppendChild(engramRange);
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;

                String valueName = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!valueName.Equals(String.Empty))
                {
                    name.InnerXml = valueName;
                    engramRange.AppendChild(name);

                    // Unit
                    XPathNavigator navIsEngramUnit = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Unit Selected' and @category='EngramRange']");
                    if (navIsEngramUnit != null)
                    {
                        if (Boolean.Parse(navIsEngramUnit.GetAttribute("value", navIsEngramUnit.NamespaceURI)))
                        {
                            XPathNavigator navIsPerformingUnit = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Performing Unit' and @category='EngramRange']");
                            if (navIsPerformingUnit != null)
                            {
                                if (Boolean.Parse(navIsPerformingUnit.GetAttribute("value", navIsEngramUnit.NamespaceURI)))
                                {
                                    XmlElement unit = document.CreateElement("Unit");
                                    unit.IsEmpty = false;
                                    String valueUnit = "UNIT";
                                    if (!valueUnit.Equals(String.Empty))
                                    {
                                        unit.InnerXml = valueUnit;
                                        engramRange.AppendChild(unit);
                                    }
                                }
                                else
                                {
                                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EngramUnitID']/Component[@ID='{0}']", eventId));
                                    if (navEventID != null)
                                    {
                                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                                        if (navUnit != null)
                                        {
                                            XmlElement unit = document.CreateElement("Unit");
                                            unit.IsEmpty = false;
                                            String valueUnit = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                                            if (!valueUnit.Equals(String.Empty))
                                            {
                                                unit.InnerXml = valueUnit;
                                                engramRange.AppendChild(unit);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Included
                    XPathNavigator navIncluded = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    if (navIncluded != null)
                    {
                        XmlElement included = document.CreateElement("Included");
                        included.IsEmpty = false;
                        String valueIncluded = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                        if (!valueIncluded.Equals(String.Empty))
                        {
                            String[] valueIncludes = Regex.Split(valueIncluded, System.Environment.NewLine);
                            included.InnerXml = String.Join(",", valueIncludes);
                            engramRange.AppendChild(included);
                        }
                    }

                    // Excluded
                    XPathNavigator navExcluded = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    if (navExcluded != null)
                    {
                        XmlElement excluded = document.CreateElement("Excluded");
                        excluded.IsEmpty = false;
                        String valueExcluded = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                        if (!valueExcluded.Equals(String.Empty))
                        {
                            String[] value2Excludes = Regex.Split(valueExcluded, System.Environment.NewLine);
                            excluded.InnerXml = String.Join(",", value2Excludes);
                            engramRange.AppendChild(excluded);
                        }
                    }
                    Boolean usesComparison = false;
                    // Comparison
                    XmlElement comparison = document.CreateElement("Comparison");
                    // Condition
                    XPathNavigator navComparison = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Compare Inequality' and @category='EngramRange']");
                    if (navComparison != null)
                    {
                        XmlElement condition = document.CreateElement("Condition");
                        condition.IsEmpty = false;
                        String valueCondition = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                        if (!valueCondition.Equals(String.Empty))
                        {
                            condition.InnerXml = valueCondition;
                            comparison.AppendChild(condition);
                        }

                    }
                    // CompareTo
                    XPathNavigator navComparisonValue = navEvent.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Compare Value' and @category='EngramRange']");
                    if (navComparisonValue != null)
                    {
                        XmlElement compareTo = document.CreateElement("CompareTo");
                        compareTo.IsEmpty = false;
                        String valueCompareTo = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                        if (!valueCompareTo.Equals(String.Empty))
                        {
                            compareTo.InnerXml = valueCompareTo;
                            comparison.AppendChild(compareTo);

                            usesComparison = true;
                        }
                    }
                    if (usesComparison)//(comparison.HasChildNodes)
                        engramRange.AppendChild(comparison);
                }
            }
            catch(Exception e)
            { }
        }

        private void createAdopt_Platform(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Adopt_Platform
            XmlElement adopt_Platform = document.CreateElement("Adopt_Platform");
            adopt_Platform.IsEmpty = false;

            parentElement.AppendChild(adopt_Platform);

            // Child
            String speciesId = String.Empty;
            String adoptPlatformId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navAdoptPlatformUnit = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='AdoptPlatformUnit']/Component[@ID='{0}' and @Type='AdoptPlatform']", adoptPlatformId));
            if (navAdoptPlatformUnit != null)
            {
                XPathNavigator navCreateEvent = navAdoptPlatformUnit.SelectSingleNode("Component[@Type='CreateEvent']");
                if (navCreateEvent != null)
                {
                    XmlElement child = document.CreateElement("Child");
                    child.IsEmpty = false;
                    String value = navCreateEvent.GetAttribute("Name", navCreateEvent.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        child.InnerXml = value;
                        adopt_Platform.AppendChild(child);
                    }
                    String createEventId = navCreateEvent.GetAttribute("ID", navCreateEvent.NamespaceURI);
                    XPathNavigator navCreateEventKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}' and @Type='CreateEvent']", createEventId));
                    if (navCreateEventKind != null)
                    {
                        XPathNavigator navSpecies = navCreateEventKind.SelectSingleNode("Component[@Type='Species']");
                        if (navSpecies != null)
                        {
                            speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                        }
                    }
                }
            }

            // Direction
            List<String> xyz = new List<String>();
            XPathNavigator navX = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='X' and @category='AdoptPlatformLocation']");
            if (navX != null)
            {
                String value = navX.GetAttribute("value", navX.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navY = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Y' and @category='AdoptPlatformLocation']");
            if (navY != null)
            {
                String value = navY.GetAttribute("value", navY.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navZ = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Z' and @category='AdoptPlatformLocation']");
            if (navZ != null)
            {
                String value = navZ.GetAttribute("value", navZ.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            if (xyz.Count > 0)
            {
                XmlElement location = document.CreateElement("Location");
                location.IsEmpty = false;
                location.InnerXml = String.Join(" ", xyz.ToArray());
                adopt_Platform.AppendChild(location);
            }

            // InitialState
            XPathNavigator navInitialState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialState' and @category='AdoptPlatform']");
            if (navInitialState != null)
            {
                XmlElement initialState = document.CreateElement("InitialState");
                initialState.IsEmpty = false;
                String value = navInitialState.GetAttribute("value", navInitialState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        initialState.InnerXml = value;
                        adopt_Platform.AppendChild(initialState);
                    }
                }
            }

            // InitialParameters
            XPathNodeIterator itInitialParameters = navLocal.Select("ComponentParameters/Parameter/Parameter[@displayedName='OverrideStealable' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideLaunchDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideDockingDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideTimeToAttack' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideMaxSpeed' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelCapacity' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideInitialFuel' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelConsumption' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelDepletionState' and @category='StartupParameters']");
            if (itInitialParameters.Count > 0)
            {
                XmlElement initialParameters = document.CreateElement("InitialParameters");
                initialParameters.IsEmpty = false;

                while (itInitialParameters.MoveNext())
                {
                    switch (itInitialParameters.Current.GetAttribute("displayedName", itInitialParameters.Current.NamespaceURI))
                    {
                        // Stealable
                        case "OverrideStealable":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navStealable = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Stealable' and @category='StartupParameters']");
                                if (navStealable != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parentElement.IsEmpty = false;
                                    parameter.InnerXml = "Stealable";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navStealable.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // LaunchDuration
                        case "OverrideLaunchDuration":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navLaunchDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='LaunchDuration' and @category='StartupParameters']");
                                if (navLaunchDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "LaunchDuration";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navLaunchDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // DockingDuration
                        case "OverrideDockingDuration":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navDockingDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='DockingDuration' and @category='StartupParameters']");
                                if (navDockingDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "DockingDuration";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navDockingDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // TimeToAttack
                        case "OverrideTimeToAttack":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='TimeToAttack' and @category='StartupParameters']");
                                if (navTimeToAttack != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "TimeToAttack";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navTimeToAttack.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // MaximumSpeed
                        case "OverrideMaxSpeed":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navMaxSpeed = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='MaxSpeed' and @category='StartupParameters']");
                                if (navMaxSpeed != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "MaximumSpeed";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navMaxSpeed.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelCapacity
                        case "OverrideFuelCapacity":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelCapacity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelCapacity' and @category='StartupParameters']");
                                if (navFuelCapacity != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelCapacity";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelCapacity.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // InitialFuelLoad
                        case "OverrideInitialFuel":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navInitialFuel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialFuel' and @category='StartupParameters']");
                                if (navInitialFuel != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "InitialFuelLoad";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navInitialFuel.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelConsumptionRate
                        case "OverrideFuelConsumption":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelConsumption = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelConsumption' and @category='StartupParameters']");
                                if (navFuelConsumption != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parentElement.IsEmpty = false;
                                    parameter.InnerXml = "FuelConsumptionRate";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelConsumption.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // Icon
                        case "OverrideIcon":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon' and @category='StartupParameters']");
                                if (navIcon != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "Icon";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navIcon.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        initialParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelDepletionState
                        case "OverrideFuelDepletionState":
                            if (Boolean.Parse(itInitialParameters.Current.GetAttribute("value", itInitialParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelDepletionState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelDepletionState' and @category='StartupParameters']");
                                if (navFuelDepletionState != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelDepletionState";
                                    initialParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelDepletionState.GetAttribute("value", String.Empty);
                                    if (isStateValid(speciesId, value, navGlobal))
                                    {
                                        if (!value.Equals(String.Empty))
                                        {
                                            setting.InnerXml = value;
                                            initialParameters.AppendChild(setting);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                if (initialParameters.HasChildNodes)
                    adopt_Platform.AppendChild(initialParameters);

            }
        }

        private void createMove_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Move_Event
            XmlElement move_Event = document.CreateElement("Move_Event");
            move_Event.IsEmpty = false;

            parentElement.AppendChild(move_Event);

            // ID
            String moveEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement id = document.CreateElement("ID");
                    id.IsEmpty = false;
                    String value = "UNIT";
                    if (!value.Equals(String.Empty))
                    {
                        id.InnerXml = value;
                        move_Event.AppendChild(id);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='MoveEvent']", moveEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            XmlElement id = document.CreateElement("ID");
                            id.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                id.InnerXml = value;
                                move_Event.AppendChild(id);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='MoveEvent']", moveEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, move_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //move_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='MoveEvent']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    move_Event.AppendChild(time);
                }
            }

            // Throttle
            XPathNavigator navThrottle = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Throttle' and @category='MoveEvent']");
            if (navThrottle != null)
            {
                XmlElement throttle = document.CreateElement("Throttle");
                throttle.IsEmpty = false;
                String value = navThrottle.GetAttribute("value", navThrottle.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    throttle.InnerXml = value;
                    move_Event.AppendChild(throttle);
                }
            }

            // Direction
            List<String> xyz = new List<String>();
            XPathNavigator navX = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='X' and @category='DestinationLocation']");
            if (navX != null)
            {
                String value = navX.GetAttribute("value", navX.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navY = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Y' and @category='DestinationLocation']");
            if (navY != null)
            {
                String value = navY.GetAttribute("value", navY.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navZ = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Z' and @category='DestinationLocation']");
            if (navZ != null)
            {
                String value = navZ.GetAttribute("value", navZ.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            if (xyz.Count > 0)
            {
                XmlElement destination = document.CreateElement("Destination");
                destination.IsEmpty = false;
                destination.InnerXml = String.Join(" ", xyz.ToArray());
                move_Event.AppendChild(destination);
            }
        }

        private void createStateChange_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create StateChange_Event
            XmlElement stateChange_Event = document.CreateElement("StateChange_Event");
            stateChange_Event.IsEmpty = false;

            parentElement.AppendChild(stateChange_Event);

            // ID
            String speciesId = String.Empty;
            String stateChangeEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement id = document.CreateElement("ID");
                    id.IsEmpty = false;
                    String value = "UNIT";
                    speciesId = value;
                    if (!value.Equals(String.Empty))
                    {
                        id.InnerXml = value;
                        stateChange_Event.AppendChild(id);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='StateChangeEvent']", stateChangeEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            String createEventId = navUnit.GetAttribute("ID", navUnit.NamespaceURI);
                            XPathNavigator navSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}']/Component[@Type='Species']", createEventId));
                            if (navSpecies != null)
                            {
                                speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                            }
                            XmlElement id = document.CreateElement("ID");
                            id.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                id.InnerXml = value;
                                stateChange_Event.AppendChild(id);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='StateChangeEvent']", stateChangeEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, stateChange_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //stateChange_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='StateChangeEvent']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    stateChange_Event.AppendChild(time);
                }
            }

            // NewState
            XPathNavigator navState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='State' and @category='StateChangeEvent']");
            if (navState != null)
            {
                XmlElement newState = document.CreateElement("NewState");
                newState.IsEmpty = false;
                String value = navState.GetAttribute("value", navState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        newState.InnerXml = value;
                        stateChange_Event.AppendChild(newState);
                    }
                }
            }

            // From
            XPathNavigator navFromState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FromState' and @category='StateChangeEvent']");
            if (navFromState != null)
            {
                XmlElement from = document.CreateElement("From");
                from.IsEmpty = false;
                String value = navFromState.GetAttribute("value", navFromState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        from.InnerXml = value;
                        stateChange_Event.AppendChild(from);
                    }
                }
            }

            // Except
            XPathNavigator navExceptState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='ExceptState' and @category='StateChangeEvent']");
            if (navExceptState != null)
            {
                XmlElement except = document.CreateElement("Except");
                except.IsEmpty = false;
                String value = navExceptState.GetAttribute("value", navExceptState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        except.InnerXml = value;
                        stateChange_Event.AppendChild(except);
                    }
                }
            }
        }

        private void createTransfer_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Transfer_Event
            XmlElement transfer_Event = document.CreateElement("Transfer_Event");
            transfer_Event.IsEmpty = false;

            parentElement.AppendChild(transfer_Event);

            // ID
            String transferEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement id = document.CreateElement("ID");
                    id.IsEmpty = false;
                    String value = "UNIT";
                    if (!value.Equals(String.Empty))
                    {
                        id.InnerXml = value;
                        transfer_Event.AppendChild(id);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='TransferEvent']", transferEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            XmlElement id = document.CreateElement("ID");
                            id.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                id.InnerXml = value;
                                transfer_Event.AppendChild(id);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='TransferEvent']", transferEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, transfer_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //transfer_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='TransferEvent']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    transfer_Event.AppendChild(time);
                }
            }

            // From
            XPathNavigator navTransferEventFromDM = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='TransferEventFromDM']/Component[@ID='{0}' and @Type='TransferEvent']", transferEventId));
            if (navTransferEventFromDM != null)
            {
                XPathNavigator navDecisionMaker = navTransferEventFromDM.SelectSingleNode("Component[@Type='DecisionMaker']");
                if (navDecisionMaker != null)
                {
                    XmlElement from = document.CreateElement("From");
                    from.IsEmpty = false;
                    String value = navDecisionMaker.GetAttribute("Name", navDecisionMaker.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        from.InnerXml = value;
                        transfer_Event.AppendChild(from);
                    }
                }
            }

            // To
            XPathNavigator navTransferEventToDM = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='TransferEventToDM']/Component[@ID='{0}' and @Type='TransferEvent']", transferEventId));
            if (navTransferEventToDM != null)
            {
                XPathNavigator navDecisionMaker = navTransferEventToDM.SelectSingleNode("Component[@Type='DecisionMaker']");
                if (navDecisionMaker != null)
                {
                    XmlElement to = document.CreateElement("To");
                    to.IsEmpty = false;
                    String value = navDecisionMaker.GetAttribute("Name", navDecisionMaker.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        to.InnerXml = value;
                        transfer_Event.AppendChild(to);
                    }
                }
            }
        }

        private void createReveal_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Reveal_Event
            XmlElement reveal_Event = document.CreateElement("Reveal_Event");
            reveal_Event.IsEmpty = false;

            parentElement.AppendChild(reveal_Event);

            // ID
            String speciesId = String.Empty;
            String revealEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement id = document.CreateElement("ID");
                    id.IsEmpty = false;
                    String value = "UNIT";
                    speciesId = value;
                    if (!value.Equals(String.Empty))
                    {
                        id.InnerXml = value;
                        reveal_Event.AppendChild(id);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='RevealEvent']", revealEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            String createEventId = navUnit.GetAttribute("ID", navUnit.NamespaceURI);
                            XPathNavigator navSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}']/Component[@Type='Species']", createEventId));
                            if (navSpecies != null)
                            {
                                speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                            }
                            XmlElement id = document.CreateElement("ID");
                            id.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                id.InnerXml = value;
                                reveal_Event.AppendChild(id);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='RevealEvent']", revealEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, reveal_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //reveal_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='RevealEvent']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    reveal_Event.AppendChild(time);
                }
            }

            // InitialLocation
            List<String> xyz = new List<String>();
            XPathNavigator navX = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='X' and @category='InitialLocation']");
            if (navX != null)
            {
                String value = navX.GetAttribute("value", navX.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navY = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Y' and @category='InitialLocation']");
            if (navY != null)
            {
                String value = navY.GetAttribute("value", navY.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            XPathNavigator navZ = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Z' and @category='InitialLocation']");
            if (navZ != null)
            {
                String value = navZ.GetAttribute("value", navZ.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    xyz.Add(value);
                }
            }
            if (xyz.Count > 0)
            {
                XmlElement initialLocation = document.CreateElement("InitialLocation");
                initialLocation.IsEmpty = false;
                initialLocation.InnerXml = String.Join(" ", xyz.ToArray());
                reveal_Event.AppendChild(initialLocation);
            }

            // InitialState
            XPathNavigator navInitialState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialState' and @category='RevealEvent']");
            if (navInitialState != null)
            {
                XmlElement initialState = document.CreateElement("InitialState");
                initialState.IsEmpty = false;
                String value = navInitialState.GetAttribute("value", navInitialState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        initialState.InnerXml = value;
                        reveal_Event.AppendChild(initialState);
                    }

                }
            }
            // StartupParameters
            XPathNodeIterator itStartupParameters = navLocal.Select("ComponentParameters/Parameter/Parameter[@displayedName='OverrideStealable' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideLaunchDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideDockingDuration' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideTimeToAttack' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideMaxSpeed' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelCapacity' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideInitialFuel' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelConsumption' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon' and @category='StartupParameters'] | " +
                                "ComponentParameters/Parameter/Parameter[@displayedName='OverrideFuelDepletionState' and @category='StartupParameters']");
            if (itStartupParameters.Count > 0)
            {
                XmlElement startupParameters = document.CreateElement("StartupParameters");
                startupParameters.IsEmpty = false;

                while (itStartupParameters.MoveNext())
                {
                    switch (itStartupParameters.Current.GetAttribute("displayedName", itStartupParameters.Current.NamespaceURI))
                    {
                        // Stealable
                        case "OverrideStealable":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navStealable = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Stealable' and @category='StartupParameters']");
                                if (navStealable != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "Stealable";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navStealable.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // LaunchDuration
                        case "OverrideLaunchDuration":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navLaunchDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='LaunchDuration' and @category='StartupParameters']");
                                if (navLaunchDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "LaunchDuration";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navLaunchDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // DockingDuration
                        case "OverrideDockingDuration":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navDockingDuration = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='DockingDuration' and @category='StartupParameters']");
                                if (navDockingDuration != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "DockingDuration";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navDockingDuration.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // TimeToAttack
                        case "OverrideTimeToAttack":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navTimeToAttack = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='TimeToAttack' and @category='StartupParameters']");
                                if (navTimeToAttack != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "TimeToAttack";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navTimeToAttack.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // MaximumSpeed
                        case "OverrideMaxSpeed":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navMaxSpeed = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='MaxSpeed' and @category='StartupParameters']");
                                if (navMaxSpeed != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "MaximumSpeed";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navMaxSpeed.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelCapacity
                        case "OverrideFuelCapacity":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelCapacity = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelCapacity' and @category='StartupParameters']");
                                if (navFuelCapacity != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelCapacity";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelCapacity.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // InitialFuelLoad
                        case "OverrideInitialFuel":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navInitialFuel = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='InitialFuel' and @category='StartupParameters']");
                                if (navInitialFuel != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "InitialFuelLoad";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navInitialFuel.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelConsumptionRate
                        case "OverrideFuelConsumption":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelConsumption = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelConsumption' and @category='StartupParameters']");
                                if (navFuelConsumption != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelConsumptionRate";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelConsumption.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // Icon
                        case "OverrideIcon":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navIcon = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Icon' and @category='StartupParameters']");
                                if (navIcon != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "Icon";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navIcon.GetAttribute("value", String.Empty);
                                    if (!value.Equals(String.Empty))
                                    {
                                        setting.InnerXml = value;
                                        startupParameters.AppendChild(setting);
                                    }
                                }
                            }
                            break;
                        // FuelDepletionState
                        case "OverrideFuelDepletionState":
                            if (Boolean.Parse(itStartupParameters.Current.GetAttribute("value", itStartupParameters.Current.NamespaceURI)))
                            {
                                XPathNavigator navFuelDepletionState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FuelDepletionState' and @category='StartupParameters']");
                                if (navFuelDepletionState != null)
                                {
                                    XmlElement parameter = document.CreateElement("Parameter");
                                    parameter.IsEmpty = false;
                                    parameter.InnerXml = "FuelDepletionState";
                                    startupParameters.AppendChild(parameter);
                                    XmlElement setting = document.CreateElement("Setting");
                                    setting.IsEmpty = false;
                                    String value = navFuelDepletionState.GetAttribute("value", String.Empty);
                                    if (isStateValid(speciesId, value, navGlobal))
                                    {
                                        if (!value.Equals(String.Empty))
                                        {
                                            setting.InnerXml = value;
                                            startupParameters.AppendChild(setting);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                if (startupParameters.HasChildNodes)
                    reveal_Event.AppendChild(startupParameters);

            }
        }

        private void createDefineEngram(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGloabl)
        {
            // Create DefineEngram
            XmlElement defineEngram = document.CreateElement("DefineEngram");
            defineEngram.IsEmpty = false;

            parentElement.AppendChild(defineEngram);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    defineEngram.AppendChild(name);
                }
            }

            // Value
            XPathNavigator navInitialValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Initial Value' and @category='Engram']");
            if (navInitialValue != null)
            {
                XmlElement initialValue = document.CreateElement("Value");
                initialValue.IsEmpty = false;
                String value = navInitialValue.GetAttribute("value", navInitialValue.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    initialValue.InnerXml = value;
                    defineEngram.AppendChild(initialValue);
                }
            }

            // Type
            XPathNavigator navType = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Type' and @category='Engram']");
            if (navType != null)
            {
                XmlElement type = document.CreateElement("Type");
                type.IsEmpty = false;
                String value = navType.GetAttribute("value", navType.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    type.InnerXml = value;
                    defineEngram.AppendChild(type);
                }
            }
        }

        private void createChangeEngram(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create ChangeEngram
            XmlElement changeEngram = document.CreateElement("ChangeEngram");
            changeEngram.IsEmpty = false;

            parentElement.AppendChild(changeEngram);

            // Name
            String changeEngramEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navEngramID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EngramID']/Component[@ID='{0}' and @Type='ChangeEngramEvent']", changeEngramEventId));
            if (navEngramID != null)
            {
                XPathNavigator navEngram = navEngramID.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    XmlElement name = document.CreateElement("Name");
                    name.IsEmpty = false;
                    String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        name.InnerXml = value;
                        changeEngram.AppendChild(name);
                    }
                }
            }

            // Unit
            XPathNavigator navIsEngramUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit Specified' and @category='ChangeEngram']");
            if (navIsEngramUnit != null)
            {
                if (Boolean.Parse(navIsEngramUnit.GetAttribute("value", navIsEngramUnit.NamespaceURI)))
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EngramUnitID']/Component[@ID='{0}']", changeEngramEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            XmlElement unit = document.CreateElement("Unit");
                            unit.IsEmpty = false;
                            String valueUnit = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!valueUnit.Equals(String.Empty))
                            {
                                unit.InnerXml = valueUnit;
                                changeEngram.AppendChild(unit);
                            }
                        }
                        else
                        {
                            XmlElement unit = document.CreateElement("Unit");
                            unit.IsEmpty = false;
                            String valueUnit = "UNIT";
                            if (!valueUnit.Equals(String.Empty))
                            {
                                unit.InnerXml = valueUnit;
                                changeEngram.AppendChild(unit);
                            }
                        }
                    }

                    //XPathNavigator navIsPerformingUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Performing Unit' and @category='EngramRange']");
                    //if (navIsPerformingUnit != null)
                    //{
                    //    if (Boolean.Parse(navIsPerformingUnit.GetAttribute("value", navIsEngramUnit.NamespaceURI)))
                    //    {
                    //        XmlElement unit = document.CreateElement("Unit");
                    //        unit.IsEmpty = false;
                    //        String valueUnit = "UNIT";
                    //        if (!valueUnit.Equals(String.Empty))
                    //        {
                    //            unit.InnerXml = valueUnit;
                    //            changeEngram.AppendChild(unit);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EngramUnitID']/Component[@ID='{0}']", changeEngramEventId));
                    //        if (navEventID != null)
                    //        {
                    //            XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                    //            if (navUnit != null)
                    //            {
                    //                XmlElement unit = document.CreateElement("Unit");
                    //                unit.IsEmpty = false;
                    //                String valueUnit = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                    //                if (!valueUnit.Equals(String.Empty))
                    //                {
                    //                    unit.InnerXml = valueUnit;
                    //                    changeEngram.AppendChild(unit);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='ChangeEngram']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    changeEngram.AppendChild(time);
                }
            }

            // Value
            XPathNavigator navValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Value' and @category='ChangeEngram']");
            if (navValue != null)
            {
                XmlElement newValue = document.CreateElement("Value");
                newValue.IsEmpty = false;
                String value = navValue.GetAttribute("value", navValue.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    newValue.InnerXml = value;
                    changeEngram.AppendChild(newValue);
                }
            }
        }

        private void createRemoveEngram(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create RemoveEngram
            XmlElement removeEngram = document.CreateElement("RemoveEngram");
            removeEngram.IsEmpty = false;

            parentElement.AppendChild(removeEngram);

            // Name
            String removeEngramEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navEngramID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EngramID']/Component[@ID='{0}' and @Type='RemoveEngramEvent']", removeEngramEventId));
            if (navEngramID != null)
            {
                XPathNavigator navEngram = navEngramID.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    XmlElement name = document.CreateElement("Name");
                    name.IsEmpty = false;
                    String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        name.InnerXml = value;
                        removeEngram.AppendChild(name);
                    }
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='RemoveEngram']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    removeEngram.AppendChild(time);
                }
            }
        }

        private void createFlushEvents(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create FlushEvents
            XmlElement flushEvents = document.CreateElement("FlushEvents");
            flushEvents.IsEmpty = false;

            parentElement.AppendChild(flushEvents);

            // Unit
            String stateChangeEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement unit = document.CreateElement("Unit");
                    unit.IsEmpty = false;
                    String value = "UNIT";
                    if (!value.Equals(String.Empty))
                    {
                        unit.InnerXml = value;
                        flushEvents.AppendChild(unit);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='FlushEvent']", stateChangeEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            XmlElement unit = document.CreateElement("Unit");
                            unit.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                unit.InnerXml = value;
                                flushEvents.AppendChild(unit);
                            }
                        }
                    }
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='Flush']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    flushEvents.AppendChild(time);
                }
            }
        }

        private void createReiterate(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create FlushEvents
            XmlElement reiterate = document.CreateElement("Reiterate");
            reiterate.IsEmpty = false;

            parentElement.AppendChild(reiterate);

            // Start         
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='ReiterateEvent']");
            if (navTime != null)
            {
                XmlElement start = document.CreateElement("Start");
                start.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    start.InnerXml = value;
                    reiterate.AppendChild(start);
                }
            }

            // EngramRange
            String reiterateEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='ReiterateEvent']", reiterateEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, reiterate, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //reiterate.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // ReiterateThis
            XPathNodeIterator itMoveEvents = navLocal.Select("Component[@Type='MoveEvent']");
            if (itMoveEvents.Count > 0)
            {
                XmlElement reiterateThis = document.CreateElement("ReiterateThis");
                reiterateThis.IsEmpty = false;
                reiterate.AppendChild(reiterateThis);

                while (itMoveEvents.MoveNext())
                {
                    createMove_Event(itMoveEvents.Current, document, reiterateThis, navGlobal);
                }
            }
        }

        private void createOpenChatRoom(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create OpenChatRoom
            XmlElement openChatRoom = document.CreateElement("OpenChatRoom");
            openChatRoom.IsEmpty = false;

            parentElement.AppendChild(openChatRoom);

            // Name
            XPathNavigator navName = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Name' and @category='OpenChatRoom']");
            if (navName != null)
            {
                XmlElement room = document.CreateElement("Room");
                room.IsEmpty = false;
                String value = navName.GetAttribute("value", navName.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    room.InnerXml = value;
                    openChatRoom.AppendChild(room);
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='OpenChatRoom']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    openChatRoom.AppendChild(time);
                }
            }

            // Members
            String openChatRoomEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navOpenChatRoomEventMember = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='OpenChatRoomEventMember']/Component[@ID='{0}' and @Type='OpenChatRoomEvent']", openChatRoomEventId));
            if (navOpenChatRoomEventMember != null)
            {
                XmlElement members = document.CreateElement("Members");
                members.IsEmpty = false;
                XPathNodeIterator itMembers = navOpenChatRoomEventMember.Select("Component[@Type='DecisionMaker']");
                List<String> memberList = new List<String>();
                while (itMembers.MoveNext())
                {
                    String value = itMembers.Current.GetAttribute("Name", itMembers.Current.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        memberList.Add(value);
                    }
                }
                if (memberList.Count > 0)
                {
                    members.InnerXml = String.Join(",", memberList.ToArray());
                    openChatRoom.AppendChild(members);
                }
            }
        }

        private void createCloseChatRoom(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create CloseChatRoom
            XmlElement closeChatRoom = document.CreateElement("CloseChatRoom");
            closeChatRoom.IsEmpty = false;

            parentElement.AppendChild(closeChatRoom);

            // Roome
            String closeChatRoomEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navCloseChatRoom = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CloseChatRoom']/Component[@ID='{0}' and @Type='CloseChatRoomEvent']", closeChatRoomEventId));
            if (navCloseChatRoom != null)
            {
                XPathNavigator navOpenChatRoom = navCloseChatRoom.SelectSingleNode("Component[@Type='OpenChatRoomEvent']");
                if (navOpenChatRoom != null)
                {
                    XPathNavigator navOpenChatRoomName = navOpenChatRoom.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Name' and @category='OpenChatRoom']");
                    if (navOpenChatRoomName != null)
                    {
                        XmlElement room = document.CreateElement("Room");
                        room.IsEmpty = false;
                        String value = navOpenChatRoomName.GetAttribute("value", navOpenChatRoomName.NamespaceURI);
                        if (!value.Equals(String.Empty))
                        {
                            room.InnerXml = value;
                            closeChatRoom.AppendChild(room);
                        }
                   }
                }
            }

            // Time
            XPathNavigator navTime = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Time' and @category='CloseChatRoom']");
            if (navTime != null)
            {
                XmlElement time = document.CreateElement("Time");
                time.IsEmpty = false;
                String value = navTime.GetAttribute("value", navTime.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    time.InnerXml = value;
                    closeChatRoom.AppendChild(time);
                }
            }
        }

        private void createCompletion_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Completion_Event
            XmlElement completion_Event = document.CreateElement("Completion_Event");
            completion_Event.IsEmpty = false;

            parentElement.AppendChild(completion_Event);

            // ID
            String speciesId = String.Empty;
            String completionEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navIsUnit = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Unit' and @category='SpeciesCompletion']");
            if (navIsUnit != null)
            {
                if (Boolean.Parse(navIsUnit.GetAttribute("value", navIsUnit.NamespaceURI)))
                {
                    XmlElement id = document.CreateElement("ID");
                    id.IsEmpty = false;
                    String value = "UNIT";
                    speciesId = value;
                    if (!value.Equals(String.Empty))
                    {
                        id.InnerXml = value;
                        completion_Event.AppendChild(id);
                    }
                }
                else
                {
                    XPathNavigator navEventID = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventID']/Component[@ID='{0}' and @Type='CompletionEvent']", completionEventId));
                    if (navEventID != null)
                    {
                        XPathNavigator navUnit = navEventID.SelectSingleNode("Component[@Type='CreateEvent']");
                        if (navUnit != null)
                        {
                            String createEventId = navUnit.GetAttribute("ID", navUnit.NamespaceURI);
                            XPathNavigator navSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='CreateEventKind']/Component[@ID='{0}']/Component[@Type='Species']", createEventId));
                            if (navSpecies != null)
                            {
                                speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                            }
                            XmlElement id = document.CreateElement("ID");
                            id.IsEmpty = false;
                            String value = navUnit.GetAttribute("Name", navUnit.NamespaceURI);
                            if (!value.Equals(String.Empty))
                            {
                                id.InnerXml = value;
                                completion_Event.AppendChild(id);
                            }
                        }
                    }
                }
            }

            // EngramRange
            XPathNavigator navEventEngram = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='EventEngram']/Component[@ID='{0}' and @Type='CompletionEvent']", completionEventId));
            if (navEventEngram != null)
            {
                XPathNavigator navEngram = navEventEngram.SelectSingleNode("Component[@Type='Engram']");
                if (navEngram != null)
                {
                    createEngramRange(navEngram, document, completion_Event, navGlobal);
                    //XmlElement engramRange = document.CreateElement("EngramRange");
                    //engramRange.IsEmpty = false;
                    //completion_Event.AppendChild(engramRange);
                    //XmlElement name = document.CreateElement("Name");
                    //name.IsEmpty = false;

                    //String value = navEngram.GetAttribute("Name", navEngram.NamespaceURI);
                    //if (!value.Equals(String.Empty))
                    //{
                    //    name.InnerXml = value;
                    //    engramRange.AppendChild(name);

                    //    // Included
                    //    XPathNavigator navIncluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Include' and @category='EngramRange']");
                    //    if (navIncluded != null)
                    //    {
                    //        XmlElement included = document.CreateElement("Included");
                    //        included.IsEmpty = false;
                    //        String value1 = navIncluded.GetAttribute("value", navIncluded.NamespaceURI);
                    //        if (!value1.Equals(String.Empty))
                    //        {
                    //            String[] value1s = Regex.Split(value1, System.Environment.NewLine);
                    //            included.InnerXml = String.Join(",", value1s);
                    //            engramRange.AppendChild(included);
                    //        }
                    //    }

                    //    // Excluded
                    //    XPathNavigator navExcluded = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Engram Range Exclude' and @category='EngramRange']");
                    //    if (navExcluded != null)
                    //    {
                    //        XmlElement excluded = document.CreateElement("Excluded");
                    //        excluded.IsEmpty = false;
                    //        String value2 = navExcluded.GetAttribute("value", navExcluded.NamespaceURI);
                    //        if (!value2.Equals(String.Empty))
                    //        {
                    //            String[] value2s = Regex.Split(value2, System.Environment.NewLine);
                    //            excluded.InnerXml = String.Join(",", value2s);
                    //            engramRange.AppendChild(excluded);
                    //        }
                    //    }

                    //    // Comparison
                    //    XmlElement comparison = document.CreateElement("Comparison");
                    //    // Condition
                    //    XPathNavigator navComparison = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison' and @category='EngramRange']");
                    //    if (navComparison != null)
                    //    {
                    //        XmlElement condition = document.CreateElement("Condition");
                    //        condition.IsEmpty = false;
                    //        String value3 = navComparison.GetAttribute("value", navComparison.NamespaceURI);
                    //        if (!value3.Equals(String.Empty))
                    //        {
                    //            condition.InnerXml = value3;
                    //            comparison.AppendChild(condition);
                    //        }
                    //    }
                    //    // CompareTo
                    //    XPathNavigator navComparisonValue = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Comparison Value' and @category='EngramRange']");
                    //    if (navComparisonValue != null)
                    //    {
                    //        XmlElement compareTo = document.CreateElement("CompareTo");
                    //        compareTo.IsEmpty = false;
                    //        String value4 = navComparisonValue.GetAttribute("value", navComparisonValue.NamespaceURI);
                    //        if (!value4.Equals(String.Empty))
                    //        {
                    //            compareTo.InnerXml = value4;
                    //            comparison.AppendChild(compareTo);
                    //        }
                    //    }
                    //    if (comparison.HasChildNodes)
                    //        engramRange.AppendChild(comparison);
                    //}
                }
            }

            // Action
            XPathNavigator navAction = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Action' and @category='CompletionEvent']");
            if (navAction != null)
            {
                String value = navAction.GetAttribute("value", navAction.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    XmlElement action = document.CreateElement("Action"); // Empty means dont export it.
                    action.IsEmpty = false;

                    action.InnerXml = value;
                    completion_Event.AppendChild(action);
                }
            }

            // NewState
            XPathNavigator navState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='State' and @category='CompletionEvent']");
            if (navState != null)
            {
                XmlElement newState = document.CreateElement("NewState");
                newState.IsEmpty = false;
                String value = navState.GetAttribute("value", navState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        newState.InnerXml = value;
                        completion_Event.AppendChild(newState);
                    }
                }
            }

            // DoThis
            XPathNodeIterator itChildren = navLocal.SelectChildren("Component", navLocal.NamespaceURI);
            while (itChildren.MoveNext())
            {
                XmlElement doThis = document.CreateElement("DoThis");
                doThis.IsEmpty = false;
                createEvents(itChildren.Current, document, doThis, navGlobal);
                if (doThis.HasChildNodes)
                    completion_Event.AppendChild(doThis);
            }
        }

        private void createSpecies_Completion_Event(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Species_Completion_Event
            XmlElement species_Completion_Event = document.CreateElement("Species_Completion_Event");
            species_Completion_Event.IsEmpty = false;

            parentElement.AppendChild(species_Completion_Event);

            String speciesId = String.Empty;
            // Species
            String speciesCompletionEventId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navSpeciesCompletionEventSpecies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='SpeciesCompletionEventSpecies']/Component[@ID='{0}' and @Type='SpeciesCompletionEvent']", speciesCompletionEventId));
            if (navSpeciesCompletionEventSpecies != null)
            {
                XPathNavigator navSpecies = navSpeciesCompletionEventSpecies.SelectSingleNode("Component[@Type='Species']");
                if (navSpecies != null)
                {
                    XmlElement species = document.CreateElement("Species");
                    species.IsEmpty = false;
                    String value = navSpecies.GetAttribute("Name", navSpecies.NamespaceURI);
                    if (!value.Equals(String.Empty))
                    {
                        species.InnerXml = value;
                        species_Completion_Event.AppendChild(species);
                    }
                    speciesId = navSpecies.GetAttribute("ID", navSpecies.NamespaceURI);
                }
            }

            // Action
            XPathNavigator navAction = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Action' and @category='SpeciesCompletionEvent']");
            if (navAction != null)
            {
                String value = navAction.GetAttribute("value", navAction.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    XmlElement action = document.CreateElement("Action"); // Empty means dont export it.
                    action.IsEmpty = false;

                    action.InnerXml = value;
                    species_Completion_Event.AppendChild(action);
                }
            }

            // NewState
            XPathNavigator navState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='State' and @category='SpeciesCompletionEvent']");
            if (navState != null)
            {
                XmlElement newState = document.CreateElement("NewState");
                newState.IsEmpty = false;
                String value = navState.GetAttribute("value", navState.NamespaceURI);
                if (isStateValid(speciesId, value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        newState.InnerXml = value;
                        species_Completion_Event.AppendChild(newState);
                    }
                }
            }

            // DoThis
            XPathNodeIterator itChildren = navLocal.SelectChildren("Component", navLocal.NamespaceURI);
            while (itChildren.MoveNext())
            {
                XmlElement doThis = document.CreateElement("DoThis");
                doThis.IsEmpty = false;
                createEvents(itChildren.Current, document, doThis, navGlobal);
                if (doThis.HasChildNodes)
                    species_Completion_Event.AppendChild(doThis);
            }
        }

        private void createRule(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Rule
            XmlElement rule = document.CreateElement("Rule");
            rule.IsEmpty = false;

            parentElement.AppendChild(rule);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    rule.AppendChild(name);
                }
            }

            // Unit 
            String ruleId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navRuleUnit = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='RuleUnit']/Component[@ID='{0}' and @Type='Rule']", ruleId));
            if (navRuleUnit != null)
            {
                XPathNavigator navActor = navRuleUnit.SelectSingleNode("Component[@Type='Actor']");
                if (navActor != null)
                {
                    createUnit(navActor, document, rule, navGlobal);
                }
            }

            // Object 
            XPathNavigator navRuleObject = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='RuleObject']/Component[@ID='{0}' and @Type='Rule']", ruleId));
            if (navRuleObject != null)
            {
                XPathNavigator navActor = navRuleObject.SelectSingleNode("Component[@Type='Actor']");
                if (navActor != null)
                {
                    createObject(navActor, document, rule, navGlobal);
                }
            }

            // NewState
            XPathNavigator navNewState = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='NewState' and @category='Rule']");
            if (navNewState != null)
            {
                XmlElement newState = document.CreateElement("NewState");
                newState.IsEmpty = false;
                String value = navNewState.GetAttribute("value", navNewState.NamespaceURI);
                if (isStateValid("unit", value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        newState.InnerXml = value;
                        rule.AppendChild(newState);
                    }
                }
            }

            // From
            XPathNavigator navFrom = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='FromState' and @category='Rule']");
            if (navFrom != null)
            {
                XmlElement from = document.CreateElement("From");
                from.IsEmpty = false;
                String value = navFrom.GetAttribute("value", navFrom.NamespaceURI);
                if (isStateValid("unit", value, navGlobal))
                {
                    if (!value.Equals(String.Empty))
                    {
                        from.InnerXml = value;
                        rule.AppendChild(from);
                    }
                }
            }

            // Increment
            XPathNavigator navIncrement = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Increment' and @category='Rule']");
            if (navIncrement != null)
            {
                XmlElement increment = document.CreateElement("Increment");
                increment.IsEmpty = false;
                String value = navIncrement.GetAttribute("value", navIncrement.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    increment.InnerXml = value;
                    rule.AppendChild(increment);
                }
            }
        }

        private void createUnit(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Unit
            XmlElement unit = document.CreateElement("Unit");
            unit.IsEmpty = false;

            parentElement.AppendChild(unit);

            // Owner
            XPathNavigator navWho = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Who' and @category='Actor']");
            if (navWho != null)
            {
                XmlElement owner = document.CreateElement("Owner");
                owner.IsEmpty = false;
                String value = navWho.GetAttribute("value", navWho.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    if (value.Equals("Myself"))
                        value = "This";
                    owner.InnerXml = value;
                    unit.AppendChild(owner);
                }
            }

            // ID
            String actorId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navWhat = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='What' and @category='Actor']");
            if (navWhat != null)
            {
                XmlElement id = document.CreateElement("ID");
                id.IsEmpty = false;
                String value = navWhat.GetAttribute("value", navWhat.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    switch (value)
                    {
                        case "Any":
                            id.InnerXml = value;
                            unit.AppendChild(id);
                            break;
                        case "Of_Species":
                            XPathNavigator navActorKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorKind']/Component[@ID='{0}' and @Type='Actor']", actorId));
                            if (navActorKind != null)
                            {
                                XPathNavigator navActor = navActorKind.SelectSingleNode("Component[@Type='Species']");
                                if (navActor != null)
                                {
                                    String value1 = navActor.GetAttribute("Name", navActor.NamespaceURI);
                                    if (!value.Equals(String.Empty))
                                    {
                                        id.InnerXml = value1;
                                        unit.AppendChild(id);
                                    }
                                }
                            }
                            break;
                        case "Unit":
                            XPathNavigator navActorUnit = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorUnit']/Component[@ID='{0}' and @Type='Actor']", actorId));
                            if (navActorUnit != null)
                            {
                                XPathNavigator navActor = navActorUnit.SelectSingleNode("Component[@Type='CreateEvent']");
                                if (navActor != null)
                                {
                                    String value1 = navActor.GetAttribute("Name", navActor.NamespaceURI);
                                    if (!value.Equals(String.Empty))
                                    {
                                        id.InnerXml = value1;
                                        unit.AppendChild(id);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            // Region
            XPathNavigator navWhere = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Where' and @category='Actor']");
            if (navWhere != null)
            {
                XmlElement region = document.CreateElement("Region");
                region.IsEmpty = false;
                String value = navWhere.GetAttribute("value", navWhere.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    XPathNavigator navActorKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorRegion']/Component[@ID='{0}' and @Type='Actor']", actorId));

                    switch (value)
                    {
                        case "Anywhere":
                            break;
                        case "In_Region":
                            if (navActorKind != null)
                            {
                                unit.AppendChild(region);

                                XPathNodeIterator itActiveRegions = navActorKind.Select("Component[@Type='ActiveRegion']");
                                List<String> regions = new List<String>();
                                while (itActiveRegions.MoveNext())
                                {
                                    String value1 = itActiveRegions.Current.GetAttribute("Name", itActiveRegions.Current.NamespaceURI);
                                    regions.Add(value1);
                                }
                                if (regions.Count > 0)
                                {
                                    XmlElement zone = document.CreateElement("Zone");
                                    zone.IsEmpty = false;
                                    zone.InnerXml = String.Join(",", regions.ToArray());
                                    region.AppendChild(zone);
                                    XmlElement relationship = document.CreateElement("Relationship");
                                    relationship.IsEmpty = false;
                                    relationship.InnerXml = "InZone";
                                    region.AppendChild(relationship);
                                }
                            }
                            break;
                        case "Not_In_Region":
                            if (navActorKind != null)
                            {
                                unit.AppendChild(region);

                                XPathNodeIterator itActiveRegions = navActorKind.Select("Component[@Type='ActiveRegion']");
                                List<String> regions = new List<String>();
                                while (itActiveRegions.MoveNext())
                                {
                                    String value1 = itActiveRegions.Current.GetAttribute("Name", itActiveRegions.Current.NamespaceURI);
                                    regions.Add(value1);
                                }
                                if (regions.Count > 0)
                                {
                                    XmlElement zone = document.CreateElement("Zone");
                                    zone.IsEmpty = false;
                                    zone.InnerXml = String.Join(",", regions.ToArray());
                                    region.AppendChild(zone);
                                    XmlElement relationship = document.CreateElement("Relationship");
                                    relationship.IsEmpty = false;
                                    relationship.InnerXml = "NotInZone";
                                    region.AppendChild(relationship);                                    
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void createObject(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Object
            XmlElement obj = document.CreateElement("Object");
            obj.IsEmpty = false;

            parentElement.AppendChild(obj);

            // Owner
            XPathNavigator navWho = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Who' and @category='Actor']");
            if (navWho != null)
            {
                XmlElement owner = document.CreateElement("Owner");
                owner.IsEmpty = false;
                String value = navWho.GetAttribute("value", navWho.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    if (value.Equals("Myself"))
                        value = "This";
                    owner.InnerXml = value;
                    obj.AppendChild(owner);
                }
            }

            // ID
            String actorId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navWhat = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='What' and @category='Actor']");
            if (navWhat != null)
            {
                XmlElement id = document.CreateElement("ID");
                id.IsEmpty = false;
                String value = navWhat.GetAttribute("value", navWhat.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    switch (value)
                    {
                        case "Any":
                            id.InnerXml = value;
                            obj.AppendChild(id);
                            break;
                        case "Of_Species":
                            XPathNavigator navActorKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorKind']/Component[@ID='{0}' and @Type='Actor']", actorId));
                            if (navActorKind != null)
                            {
                                XPathNavigator navActor = navActorKind.SelectSingleNode("Component[@Type='Species']");
                                if (navActor != null)
                                {
                                    String value1 = navActor.GetAttribute("Name", navActor.NamespaceURI);
                                    if (!value.Equals(String.Empty))
                                    {
                                        id.InnerXml = value1;
                                        obj.AppendChild(id);
                                    }
                                }
                            }
                            break;
                        case "Unit":
                            XPathNavigator navActorUnit = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorUnit']/Component[@ID='{0}' and @Type='Actor']", actorId));
                            if (navActorUnit != null)
                            {
                                XPathNavigator navActor = navActorUnit.SelectSingleNode("Component[@Type='CreateEvent']");
                                if (navActor != null)
                                {
                                    String value1 = navActor.GetAttribute("Name", navActor.NamespaceURI);
                                    if (!value.Equals(String.Empty))
                                    {
                                        id.InnerXml = value1;
                                        obj.AppendChild(id);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            // Region
            XPathNavigator navWhere = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Where' and @category='Actor']");
            if (navWhere != null)
            {
                XmlElement region = document.CreateElement("Region");
                region.IsEmpty = false;
                String value = navWhere.GetAttribute("value", navWhere.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    XPathNavigator navActorKind = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@id='{0}' and @type='ActorRegion']/Component[@ID='{0}' and @Type='Actor']", actorId));

                    switch (value)
                    {
                        case "Anywhere":
                            break;
                        case "In_Region":
                            if (navActorKind != null)
                            {
                                obj.AppendChild(region);

                                XPathNodeIterator itActiveRegions = navActorKind.Select("Component[@Type='ActiveRegion']");
                                List<String> regions = new List<String>();
                                while (itActiveRegions.MoveNext())
                                {
                                    String value1 = itActiveRegions.Current.GetAttribute("Name", itActiveRegions.Current.NamespaceURI);
                                    regions.Add(value1);
                                }
                                if (regions.Count > 0)
                                {
                                    XmlElement zone = document.CreateElement("Zone");
                                    zone.IsEmpty = false;
                                    zone.InnerXml = String.Join(",", regions.ToArray());
                                    region.AppendChild(zone);
                                    XmlElement relationship = document.CreateElement("Relationship");
                                    relationship.IsEmpty = false;
                                    relationship.InnerXml = "InZone";
                                    region.AppendChild(relationship);
                                }
                            }
                            break;
                        case "Not_In_Region":
                            if (navActorKind != null)
                            {
                                obj.AppendChild(region);

                                XPathNodeIterator itActiveRegions = navActorKind.Select("Component[@Type='ActiveRegion']");
                                List<String> regions = new List<String>();
                                while (itActiveRegions.MoveNext())
                                {
                                    String value1 = itActiveRegions.Current.GetAttribute("Name", itActiveRegions.Current.NamespaceURI);
                                    regions.Add(value1);
                                }
                                if (regions.Count > 0)
                                {
                                    XmlElement zone = document.CreateElement("Zone");
                                    zone.IsEmpty = false;
                                    zone.InnerXml = String.Join(",", regions.ToArray());
                                    region.AppendChild(zone);
                                    XmlElement relationship = document.CreateElement("Relationship");
                                    relationship.IsEmpty = false;
                                    relationship.InnerXml = "NotInZone";
                                    region.AppendChild(relationship);
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void createScore(XPathNavigator navLocal, XmlDocument document, XmlElement parentElement, XPathNavigator navGlobal)
        {
            // Create Score
            XmlElement score = document.CreateElement("Score");
            score.IsEmpty = false;

            parentElement.AppendChild(score);

            // Name
            if (navLocal != null)
            {
                XmlElement name = document.CreateElement("Name");
                name.IsEmpty = false;
                String value = navLocal.GetAttribute("Name", navLocal.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    name.InnerXml = value;
                    score.AppendChild(name);
                }
            }

            // Rules
            String scoreId = navLocal.GetAttribute("ID", navLocal.NamespaceURI);
            XPathNavigator navScoreRules = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='ScoreRules']/Component[@ID='{0}' and @Type='Score']", scoreId));
            if (navScoreRules != null)
            {
                XPathNodeIterator itRules = navScoreRules.Select("Component[@Type='Rule']");
                List<String> ruleList = new List<String>();
                while (itRules.MoveNext())
                {
                    String value = itRules.Current.GetAttribute("Name", itRules.Current.NamespaceURI);
                    ruleList.Add(value);
                }
                if (ruleList.Count > 0)
                {
                    XmlElement rules = document.CreateElement("Rules");
                    rules.IsEmpty = false;
                    rules.InnerXml = String.Join(",", ruleList.ToArray());
                    score.AppendChild(rules);
                }
            }

            // Applies
            XPathNavigator navScoreApplies = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='ScoreApplies']/Component[@ID='{0}' and @Type='Score']", scoreId));
            if (navScoreApplies != null)
            {
                XPathNodeIterator itDecisionMakers = navScoreApplies.Select("Component[@Type='DecisionMaker']");
                List<String> decisionMakersList = new List<String>();
                while (itDecisionMakers.MoveNext())
                {
                    String value = itDecisionMakers.Current.GetAttribute("Name", itDecisionMakers.Current.NamespaceURI);
                    decisionMakersList.Add(value);
                }
                if (decisionMakersList.Count > 0)
                {
                    XmlElement applies = document.CreateElement("Applies");
                    applies.IsEmpty = false;
                    applies.InnerXml = String.Join(",", decisionMakersList.ToArray());
                    score.AppendChild(applies);
                }
            }

            // Viewers
            XPathNavigator navScoreViewers = navGlobal.SelectSingleNode(String.Format("/LinkTypes/Link[@type='ScoreViewers']/Component[@ID='{0}' and @Type='Score']", scoreId));
            if (navScoreViewers != null)
            {
                XPathNodeIterator itDecisionMakers = navScoreViewers.Select("Component[@Type='DecisionMaker']");
                List<String> decisionMakersList = new List<String>();
                while (itDecisionMakers.MoveNext())
                {
                    String value = itDecisionMakers.Current.GetAttribute("Name", itDecisionMakers.Current.NamespaceURI);
                    decisionMakersList.Add(value);
                }
                if (decisionMakersList.Count > 0)
                {
                    XmlElement viewers = document.CreateElement("Viewers");
                    viewers.IsEmpty = false;
                    viewers.InnerXml = String.Join(",", decisionMakersList.ToArray());
                    score.AppendChild(viewers);
                }
            }

            // Initial
            XPathNavigator navInitial = navLocal.SelectSingleNode("ComponentParameters/Parameter/Parameter[@displayedName='Initial' and @category='Score']");
            if (navInitial != null)
            {
                XmlElement initial = document.CreateElement("Initial");
                initial.IsEmpty = false;
                String value = navInitial.GetAttribute("value", navInitial.NamespaceURI);
                if (!value.Equals(String.Empty))
                {
                    initial.InnerXml = value;
                    score.AppendChild(initial);
                }
            }
        }

        private Boolean validate(IXPathNavigable iNav)
        {
            XPathNavigator navigator = iNav.CreateNavigator();
            StringReader stringReader = new StringReader(navigator.OuterXml);

            Boolean isValid = false;
            XmlReader schemaDatabase = settings.controller.GetXSD("DDDSchema_4_0_2.xsd");

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.Schemas.Add(null, schemaDatabase);
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            readerSettings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

            XmlReader reader = XmlReader.Create(stringReader, readerSettings);

            try
            {
                while (reader.Read()) ;
                isValid = true;
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Invalid Scenario");
                ////////////////////////////////////////////////////////////////////////////
                // REMOVE THIS -- TESTING ONLY -- DO NOT WANT TO WRITE INVALID FILE TO DISK
                //XmlTextWriter writer = new XmlTextWriter(@"C:\export.xml", Encoding.UTF8);
                //writer.Formatting = Formatting.Indented;
                //writer.WriteStartDocument();
                //navigator.WriteSubtree(writer);
                //writer.Close();
                ////////////////////////////////////////////////////////////////////////////
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
    }

    public class DDD_4_0_2_ExporterSettings : ControllerToXmlSettings
    {
        public DDD_4_0_2_ExporterSettings(Controller controller, 
                                          RootController rootController, 
                                          Boolean treeDataFormat) : base(controller, rootController, treeDataFormat)
        {
        }
    }
}
