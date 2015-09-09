<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="DDDSchema_4_0_2.xsd">

  <xsl:output method="xml" encoding="UTF-8" indent="yes" version="1.0"/>
  <xsl:key name="create_event_node_key" match="/Scenario/Create_Event/Subplatform" use="concat(./Kind/text(), ../Kind/text())"/>
  <xsl:key name="create_adopt_platform_key" match="/Scenario/Create_Event/Adopt_Platform" use="./Child"/>
  <xsl:key name="launch_event_kind_key" match="/Scenario/Launch_Event" use="./Kind"/>
  <xsl:key name="create_event_node_key_armament" match="/Scenario/Create_Event/Subplatform/Armament" use="./Weapon/text()"/>


  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="Scenario">
    <xsl:element name="Scenario">
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">
        <xsl:text>DDDSchema_4_1.xsd</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="ScenarioName">
    <xsl:element name="ScenarioName">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Description">
    <xsl:element name="Description">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="ClientSideAssetTransfer">
    <xsl:element name="ClientSideAssetTransfer">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Playfield">
    <xsl:element name="Playfield">
      <xsl:element name="MapFileName">
        <xsl:value-of select="MapFileName/text()"/>
      </xsl:element>
      <xsl:if test="./IconLibrary">
        <xsl:element name="IconLibrary">
          <xsl:value-of select="IconLibrary/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="UtmZone">
        <xsl:value-of select="UtmZone/text()"/>
      </xsl:element>
      <xsl:element name="VerticalScale">
        <xsl:value-of select="VerticalScale/text()"/>
      </xsl:element>
      <xsl:element name="HorizontalScale">
        <xsl:value-of select="HorizontalScale/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="LandRegion">
    <xsl:element name="LandRegion">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Vertex"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Vertex">
    <xsl:element name="Vertex">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="ActiveRegion">
    <xsl:element name="ActiveRegion">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Vertex"/>
      <xsl:if test="Start">
        <xsl:element name="Start">
          <xsl:value-of select="Start/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="End">
        <xsl:element name="End">
          <xsl:value-of select="End/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="SpeedMultiplier">
        <xsl:element name="SpeedMultiplier">
          <xsl:value-of select="SpeedMultiplier/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="BlocksMovement">
        <xsl:element name="BlocksMovement">
          <xsl:value-of select="BlocksMovement/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="SensorsBlocked">
        <xsl:element name="SensorsBlocked">
          <xsl:value-of select="SensorsBlocked/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="IsVisible">
        <xsl:element name="IsVisible">
          <xsl:value-of select="IsVisible/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Color">
        <xsl:element name="Color">
          <xsl:value-of select="Color/text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Team">
    <xsl:element name="Team">
      <xsl:element name="Name">
        <xsl:value-of select="./Name/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Against"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="/Scenario/Team/Against">
    <xsl:element name="Against">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="DecisionMaker">
    <xsl:element name="DecisionMaker">
      <xsl:element name="Role">
        <xsl:value-of select="./Role/text()"/>
      </xsl:element>
      <xsl:element name="Identifier">
        <xsl:value-of select="./Identifier/text()"/>
      </xsl:element>
      <xsl:element name="Color">
        <xsl:value-of select="./Color/text()"/>
      </xsl:element>
      <xsl:if test="Briefing">
        <xsl:element name="Briefing">
          <xsl:value-of select="Briefing/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Team">
        <xsl:value-of select="./Team/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Network">
    <xsl:element name="Network">
      <xsl:element name="Name">
        <xsl:value-of select="./Name/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Member"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/Scenario/Network/Member">
    <xsl:element name="Member">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="DefineEngram">
    <xsl:element name="DefineEngram">
      <xsl:element name="Name">
        <xsl:value-of select="./Name/text()"/>
      </xsl:element>
      <xsl:element name="Value">
        <xsl:value-of select="./Value/text()"/>
      </xsl:element>
      <xsl:if test="Type">
        <xsl:element name="Type">
          <xsl:value-of select="Type/text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Sensor">
    <xsl:element name="Sensor">
      <xsl:element name="Name">
        <xsl:value-of select="./Name/text()"/>
      </xsl:element>
      <xsl:if test="Attribute">
        <xsl:element name="Attribute">
          <xsl:value-of select="Attribute/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Engram">
        <xsl:element name="Engram">
          <xsl:value-of select="Engram/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Cone">
        <xsl:apply-templates select="Cone"/>
      </xsl:if>
      <xsl:if test="Extent">
        <xsl:element name="Extent">
          <xsl:value-of select="Extent/text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Cone">
    <xsl:element name="Cone">
      <xsl:element name="Spread">
        <xsl:value-of select="./Spread/text()"/>
      </xsl:element>
      <xsl:element name="Extent">
        <xsl:value-of select="./Extent/text()"/>
      </xsl:element>
      <xsl:element name="Direction">
        <xsl:value-of select="./Direction/text()"/>
      </xsl:element>
      <xsl:element name="Level">
        <xsl:value-of select="./Level/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/Scenario/TimeToAttack">
    <xsl:element name="TimeToAttack">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="/Scenario/Genus">
    <xsl:element name="Genus">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="/Scenario/Species">
    <xsl:element name="Species">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
      <xsl:element name="Base">
        <xsl:value-of select="Base/text()"/>
      </xsl:element>
      <xsl:if test="Size">
        <xsl:element name="Size">
          <xsl:value-of select="Size/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="IsWeapon">
        <xsl:element name="IsWeapon">
          <xsl:value-of select="IsWeapon/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="RemoveOnDestruction">
        <xsl:element name="RemoveOnDestruction">
          <xsl:value-of select="RemoveOnDestruction/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:call-template name="display_subplatform_capacity">
        <xsl:with-param name="speciesNode" select="."/>
      </xsl:call-template>
      <xsl:call-template name="display_subplatform_capacity_armament">
        <xsl:with-param name="speciesNode" select="."/>
      </xsl:call-template>
      <xsl:element name="FullyFunctional">
        <xsl:apply-templates select="FullyFunctional" mode="Species"/>
      </xsl:element>
      <xsl:apply-templates select="DefineState" mode="Species"/>
    </xsl:element>
  </xsl:template>


  <xsl:template name="display_subplatform_capacity">
    <xsl:param name="speciesNode"/>

    <xsl:for-each select="/Scenario/Create_Event[Kind/text()=$speciesNode/Name/text()]/Subplatform[generate-id()=generate-id(key('create_event_node_key',concat(./Kind/text(), ../Kind/text()))[1])]">
      <xsl:variable name="kind_string" select="./Kind"/>
      <xsl:variable name="subplatforms" select="/Scenario/Create_Event/Subplatform[Kind=$kind_string]"/>
      <xsl:variable name="test_current" select="."/>
      <xsl:if test="$subplatforms">
        <xsl:call-template name="render_subplatform_capacity">
          <xsl:with-param name="platform_node" select="$subplatforms"/>
          <xsl:with-param name="count" select="1"/>
          <xsl:with-param name="sub_platform_count" select="0"/>
        </xsl:call-template>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>


  <xsl:template name="render_subplatform_capacity">
    <xsl:param name="platform_node"/>
    <xsl:param name="count"/>
    <xsl:param name="sub_platform_count"/>

    <xsl:choose>
      <xsl:when test="$count &gt; (count($platform_node))">
        <xsl:element name="SubplatformCapacity">
          <xsl:element name="SpeciesName">
            <xsl:value-of select="$platform_node/Kind"/>
          </xsl:element>
          <xsl:element name="Count">
            <xsl:value-of select="$sub_platform_count"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="$platform_node/Docked/Count">
          <xsl:choose>
            <xsl:when test="number($platform_node[position()=$count]/Docked/Count/text()) &gt; $sub_platform_count ">
              <xsl:call-template name="render_subplatform_capacity">
                <xsl:with-param name="platform_node" select="$platform_node"/>
                <xsl:with-param name="sub_platform_count" select="number($platform_node[position()=$count]/Docked/Count/text())"/>
                <xsl:with-param name="count" select="$count + 1"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="render_subplatform_capacity">
                <xsl:with-param name="platform_node" select="$platform_node"/>
                <xsl:with-param name="sub_platform_count" select="$sub_platform_count"/>
                <xsl:with-param name="count" select="$count + 1"/>
              </xsl:call-template>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="display_subplatform_capacity_armament">
    <xsl:param name="speciesNode"/>

    <xsl:if test="/Scenario/Create_Event/Subplatform[Kind/text()=$speciesNode/Name/text()]/Armament">
      <xsl:for-each select="/Scenario/Create_Event/Subplatform[Kind/text()=$speciesNode/Name/text()]/Armament[generate-id()=generate-id(key('create_event_node_key_armament', ./Weapon/text() )[1])]">
        <xsl:variable name="kind_string" select="$speciesNode/Name"/>
        <xsl:variable name="subplatforms" select="/Scenario/Create_Event/Subplatform[Kind=$kind_string]/Armament"/>
        <xsl:if test="$subplatforms">
          <xsl:call-template name="render_subplatform_capacity_armament">
            <xsl:with-param name="platform_node" select="$subplatforms"/>
            <xsl:with-param name="count" select="1"/>
            <xsl:with-param name="sub_platform_count" select="0"/>
          </xsl:call-template>
        </xsl:if>
      </xsl:for-each>
    </xsl:if>
  </xsl:template>


  <xsl:template name="render_subplatform_capacity_armament">
    <xsl:param name="platform_node"/>
    <xsl:param name="count"/>
    <xsl:param name="sub_platform_count"/>

    <xsl:choose>
      <xsl:when test="$count &gt; (count($platform_node))">
        <xsl:element name="SubplatformCapacity">
          <xsl:element name="SpeciesName">
            <xsl:value-of select="$platform_node/Weapon"/>
          </xsl:element>
          <xsl:element name="Count">
            <xsl:value-of select="$sub_platform_count"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="$platform_node/Count">
          <xsl:choose>
            <xsl:when test="number($platform_node[position()=$count]/Count/text()) &gt; $sub_platform_count ">
              <xsl:call-template name="render_subplatform_capacity_armament">
                <xsl:with-param name="platform_node" select="$platform_node"/>
                <xsl:with-param name="sub_platform_count" select="number($platform_node[position()=$count]/Count/text())"/>
                <xsl:with-param name="count" select="$count + 1"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="render_subplatform_capacity_armament">
                <xsl:with-param name="platform_node" select="$platform_node"/>
                <xsl:with-param name="sub_platform_count" select="$sub_platform_count"/>
                <xsl:with-param name="count" select="$count + 1"/>
              </xsl:call-template>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="DefineState" mode="Species">
    <xsl:element name="DefineState">
      <xsl:element name="State">
        <xsl:value-of select="State/text()"/>
      </xsl:element>
      <xsl:if test="Icon">
        <xsl:element name="Icon">
          <xsl:value-of select="Icon/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="StateParameters">
        <xsl:element name="StateParameters">
          <xsl:apply-templates select="StateParameters" mode="FullyFunctional"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="FullyFunctional" mode="Species" >
    <xsl:if test="Icon">
      <xsl:element name="Icon">
        <xsl:value-of select="Icon/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="StateParameters">
      <xsl:element name="StateParameters">
        <xsl:apply-templates select="StateParameters" mode="FullyFunctional"/>
      </xsl:element>
    </xsl:if>
  </xsl:template>


  <xsl:template match="StateParameters" mode="FullyFunctional" >
    <xsl:if test="LaunchDuration">
      <xsl:element name="LaunchDuration">
        <xsl:value-of select="LaunchDuration/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="DockingDuration">
      <xsl:element name="DockingDuration">
        <xsl:value-of select="DockingDuration/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="TimeToAttack">
      <xsl:element name="TimeToAttack">
        <xsl:value-of select="TimeToAttack/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="MaximumSpeed">
      <xsl:element name="MaximumSpeed">
        <xsl:value-of select="MaximumSpeed/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="FuelCapacity">
      <xsl:element name="FuelCapacity">
        <xsl:value-of select="FuelCapacity/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="InitialFuelLoad">
      <xsl:element name="InitialFuelLoad">
        <xsl:value-of select="InitialFuelLoad/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="FuelConsumptionRate">
      <xsl:element name="FuelConsumptionRate">
        <xsl:value-of select="FuelConsumptionRate/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="FuelDepletionState">
      <xsl:element name="FuelDepletionState">
        <xsl:value-of select="FuelDepletionState/text()"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="Stealable">
      <xsl:element name="Stealable">
        <xsl:value-of select="Stealable/text()"/>
      </xsl:element>
    </xsl:if>

    <xsl:apply-templates select="Sense" mode="StateParameters"/>
    <xsl:apply-templates select="Capability" mode="StateParameters"/>
    <xsl:apply-templates select="SingletonVulnerability" mode="StateParameters"/>
    <xsl:apply-templates select="ComboVulnerability" mode="StateParameters"/>
    <xsl:apply-templates select="Emitter" mode="StateParameters"/>

  </xsl:template>


  <xsl:template match="Sense" mode="StateParameters">
    <xsl:element name="Sense">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Capability" mode="StateParameters">
    <xsl:element name="Capability">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Proximity" mode="Capability"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Proximity" mode="Capability">
    <xsl:element name="Proximity">
      <xsl:element name="Range">
        <xsl:value-of select="Range/text()"/>
      </xsl:element>
      <xsl:element name="Effect">
        <xsl:apply-templates select="Effect" mode="Proximity"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Effect" mode="Proximity">
    <xsl:element name="Intensity">
      <xsl:value-of select="Intensity/text()"/>
    </xsl:element>
    <xsl:if test="Probability">
      <xsl:element name="Probability">
        <xsl:value-of select="Probability/text()"/>
      </xsl:element>
    </xsl:if>
  </xsl:template>


  <xsl:template match="SingletonVulnerability" mode="StateParameters">
    <xsl:element name="SingletonVulnerability">
      <xsl:element name="Capability">
        <xsl:value-of select="Capability/text()"/>
      </xsl:element>
      <xsl:element name="Transitions">
        <xsl:apply-templates select="Transitions" mode="SingletonVulnerability"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Transitions" mode="SingletonVulnerability">
    <xsl:for-each select="./*">
      <xsl:if test="name()='Effect'">
        <xsl:element name="Effect">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Range'">
        <xsl:element name="Range">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Probability'">
        <xsl:element name="Probability">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='State'">
        <xsl:element name="State">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="ComboVulnerability" mode="StateParameters">
    <xsl:element name="ComboVulnerability">
      <xsl:apply-templates select="Contribution" mode="ComboVulnerability"/>
      <xsl:element name="NewState">
        <xsl:value-of select="NewState/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Contribution" mode="ComboVulnerability">
    <xsl:element name="Contribution">
      <xsl:element name="Capability">
        <xsl:value-of select="Capability/text()"/>
      </xsl:element>
      <xsl:element name="Effect">
        <xsl:value-of select="Effect/text()"/>
      </xsl:element>
      <xsl:if test="Range">
        <xsl:element name="Range">
          <xsl:value-of select="Range/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Probability">
        <xsl:element name="Probability">
          <xsl:value-of select="Probability/text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Emitter" mode="StateParameters">
    <xsl:element name="Emitter">
      <xsl:if test="Attribute">
        <xsl:element name="Attribute">
          <xsl:value-of select="Attribute/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Engram">
        <xsl:element name="Engram">
          <xsl:value-of select="Engram/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:apply-templates select="NormalEmitter" mode="Emitter"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="NormalEmitter" mode="Emitter">
    <xsl:element name="NormalEmitter">
      <xsl:for-each select="./*">
        <xsl:if test="name()='Level'">
          <xsl:element name="Level">
            <xsl:value-of select="text()"/>
          </xsl:element>
        </xsl:if>
        <xsl:if test="name()='Variance'">
          <xsl:element name="Variance">
            <xsl:value-of select="text()"/>
          </xsl:element>
        </xsl:if>
        <xsl:if test="name()='Percent'">
          <xsl:element name="Percent">
            <xsl:value-of select="text()"/>
          </xsl:element>
        </xsl:if>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>


  <xsl:template match="ChangeEngram" >
    <xsl:element name="ChangeEngram">
      <xsl:if test="Name">
        <xsl:element name="Name">
          <xsl:value-of select="Name/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Unit">
        <xsl:element name="Unit">
          <xsl:value-of select="Unit/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Time">
        <xsl:element name="Time">
          <xsl:value-of select="Time/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Value">
        <xsl:value-of select="Value/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="CloseChatRoom">
    <xsl:element name="CloseChatRoom">
      <xsl:element name="Room">
        <xsl:value-of select="Room/text()"/>
      </xsl:element>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Completion_Event">
    <xsl:element name="Completion_Event">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Action">
        <xsl:element name="Action">
          <xsl:value-of select="Action/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="NewState">
        <xsl:element name="NewState">
          <xsl:value-of select="NewState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:apply-templates select="DoThis" mode="Completion_Event"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="EngramRange" mode="Event">
    <xsl:for-each select="./*">
      <xsl:if test="name()='Name'">
        <xsl:element name="Name">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Unit'">
        <xsl:element name="Unit">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Included'">
        <xsl:element name="Included">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Excluded'">
        <xsl:element name="Excluded">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Comparison'">
        <xsl:element name="Comparison">
          <xsl:element name="Condition">
            <xsl:value-of select="Condition/text()"/>
          </xsl:element>
          <xsl:element name="CompareTo">
            <xsl:value-of select="CompareTo/text()"/>
          </xsl:element>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>


  <xsl:template match="DoThis" mode="Completion_Event">
    <xsl:element name="DoThis">
      <xsl:apply-templates select="Reveal_Event"/>
      <xsl:apply-templates select="Move_Event" />
      <xsl:apply-templates select="Completion_Event" />
      <xsl:apply-templates select="Transfer_Event"/>
      <xsl:apply-templates select="StateChange_Event"/>
      <xsl:apply-templates select="Launch_Event"/>
      <xsl:apply-templates select="DefineEngram"/>
      <xsl:apply-templates select="ChangeEngram"/>
      <xsl:apply-templates select="RemoveEngram"/>
      <xsl:apply-templates select="FlushEvents"/>
      <xsl:apply-templates select="Species_Completion_Event"/>
      <xsl:apply-templates select="Reiterate"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Create_Event" >
    <xsl:call-template name="renderCreateEvent">
      <xsl:with-param name="event_node" select="."/>
    </xsl:call-template>
  </xsl:template>


  <xsl:template name="renderCreateEvent">
    <xsl:param name="event_node"/>

    <xsl:variable name="sub_platform_node" select="$event_node/Subplatform"/>
    <xsl:variable name="adopt_platform_node" select="$event_node/Adopt_Platform"/>
    <xsl:variable name="sub_platform_value_list" select="''"/>

    <xsl:for-each select="$sub_platform_node">
      <xsl:call-template name="recurse_create_event">
        <xsl:with-param name="sub_platform_node" select="."/>
        <xsl:with-param name="platform_count" select="'0'"/>
        <xsl:with-param name="parent" select="$event_node"/>
        <xsl:with-param name="mode" select="'event'"/>
      </xsl:call-template>
    </xsl:for-each>

    <xsl:element name="Create_Event">
      <xsl:element name="ID">
        <xsl:value-of select="$event_node/ID/text()"/>
      </xsl:element>
      <xsl:element name="Kind">
        <xsl:value-of select="$event_node/Kind/text()"/>
      </xsl:element>
      <xsl:element name="Owner">
        <xsl:value-of select="$event_node/Owner/text()"/>
      </xsl:element>
      <xsl:if test="Subplatform">
        <xsl:element name="Subplatform">
          <xsl:for-each select="$sub_platform_node">
            <xsl:choose>
              <xsl:when test="position()= count($sub_platform_node)">
                <xsl:call-template name="recurse_create_event">
                  <xsl:with-param name="sub_platform_node" select="."/>
                  <xsl:with-param name="platform_count" select="'0'"/>
                  <xsl:with-param name="parent" select="$event_node"/>
                  <xsl:with-param name="mode" select="'sub_platform'"/>
                  <xsl:with-param name="last_platform" select="'true'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="recurse_create_event">
                  <xsl:with-param name="sub_platform_node" select="."/>
                  <xsl:with-param name="platform_count" select="'0'"/>
                  <xsl:with-param name="parent" select="$event_node"/>
                  <xsl:with-param name="mode" select="'sub_platform'"/>
                  <xsl:with-param name="last_platform" select="'false'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
        </xsl:element>
      </xsl:if>
      <xsl:if test="count($sub_platform_node) = 0">
        <xsl:if test="count($adopt_platform_node) &gt; 0">
          <xsl:element name="Subplatform">
            <xsl:for-each select="$adopt_platform_node">
              <xsl:value-of select="Child/text( )"/>
              <xsl:if test="position() &lt; count($adopt_platform_node)">
                <xsl:text>,</xsl:text>
              </xsl:if>
            </xsl:for-each>
          </xsl:element>
        </xsl:if>
      </xsl:if>
    </xsl:element>
  </xsl:template>


  <xsl:template name="recurse_create_event">
    <xsl:param name="sub_platform_node"/>
    <xsl:param name="platform_count"/>
    <xsl:param name="parent"/>
    <xsl:param name="mode"/>
    <xsl:param name="last_platform"/>

    <xsl:if test="(number($platform_count)) &lt; (number($sub_platform_node/Docked/Count))">
      <xsl:choose>
        <xsl:when test="$mode = 'sub_platform'">
          <xsl:if test="$last_platform = 'true'">
            <xsl:choose>
              <xsl:when test="$platform_count &lt; (number($sub_platform_node/Docked/Count/text())-1)">
                <!--xsl:choose>
              		<xsl:when test="$sub_platform_node/Armament">
              			<xsl:value-of select="concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count)"/>
              			<xsl:call-template name="append_armaments">                
						    <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
						    <xsl:with-param name="armament_count" select="'0'"/>
						    <xsl:with-param name="platform_count" select="$platform_count"/>
						    <xsl:with-param name="parent" select="$parent"/>               	
		                </xsl:call-template>    
		                <xsl:text>,</xsl:text>          			
              		</xsl:when>
              		<xsl:otherwise-->
                <xsl:value-of select="concat(concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count),',')"/>
                <!--/xsl:otherwise>
              	</xsl:choose-->
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count)"/>
                <!--xsl:call-template name="append_armaments"> 
				    <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
				    <xsl:with-param name="armament_count" select="'0'"/>
				    <xsl:with-param name="platform_count" select="$platform_count"/>
				    <xsl:with-param name="parent" select="$parent"/>                    	
                </xsl:call-template-->
                <xsl:call-template name="append_adopt_platform"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
          <xsl:if test="$last_platform = 'false'">
            <!--xsl:choose>
              		<xsl:when test="$sub_platform_node/Armament">
              			<xsl:value-of select="concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count)"/>
              			<xsl:call-template name="append_armaments">                
						    <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
						    <xsl:with-param name="armament_count" select="'0'"/>
						    <xsl:with-param name="platform_count" select="$platform_count"/>
						    <xsl:with-param name="parent" select="$parent"/>               	
		                </xsl:call-template>
		                <xsl:text>,</xsl:text>              			
              		</xsl:when>
              		<xsl:otherwise-->
            <xsl:value-of select="concat(concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count),',')"/>
            <!--/xsl:otherwise>
              	</xsl:choose-->
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <!-- Constructs Create Events for all Armaments if any	-->
          <xsl:if test="$sub_platform_node/Armament">
            <xsl:call-template name="recurse_create_event_armament">
              <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
              <xsl:with-param name="platform_count" select="$platform_count"/>
              <xsl:with-param name="armament_count" select="'0'"/>
              <xsl:with-param name="parent" select="$parent"/>
              <xsl:with-param name="mode" select="$mode"/>
              <xsl:with-param name="last_platform" select="$last_platform"/>
            </xsl:call-template>
          </xsl:if>
          <!-- Constructs create event for parent of Armament/Weapon if any-->
          <xsl:element name="Create_Event">
            <xsl:element name="ID">
              <xsl:value-of select="concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), $platform_count)"/>
            </xsl:element>
            <xsl:element name="Kind">
              <xsl:value-of select="Kind/text()"/>
            </xsl:element>
            <xsl:element name="Owner">
              <xsl:value-of select="$parent/Owner/text()"/>
            </xsl:element>
            <xsl:if test="$sub_platform_node/Armament">
              <xsl:element name="Subplatform">
                <xsl:call-template name="append_armaments">
                  <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
                  <xsl:with-param name="armament_count" select="'0'"/>
                  <xsl:with-param name="platform_count" select="$platform_count"/>
                  <xsl:with-param name="parent" select="$parent"/>
                </xsl:call-template>
              </xsl:element>
            </xsl:if>
          </xsl:element>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:call-template name="recurse_create_event">
        <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
        <xsl:with-param name="platform_count" select="number($platform_count) + 1"/>
        <xsl:with-param name="parent" select="$parent"/>
        <xsl:with-param name="mode" select="$mode"/>
        <xsl:with-param name="last_platform" select="$last_platform"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>


  <xsl:template name="recurse_create_event_armament">
    <xsl:param name="sub_platform_node"/>
    <xsl:param name="platform_count"/>
    <xsl:param name="armament_count"/>
    <xsl:param name="parent"/>
    <xsl:param name="mode"/>
    <xsl:param name="last_platform"/>

    <xsl:if test="(number($armament_count)) &lt; (number($sub_platform_node/Armament/Count))">
      <xsl:element name="Create_Event">
        <xsl:element name="ID">
          <xsl:value-of select="concat(concat(concat(concat(concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), 
			  		  $platform_count), '_'), $sub_platform_node/Armament/Weapon/text()), '_'), $armament_count)"/>
        </xsl:element>
        <xsl:element name="Kind">
          <xsl:value-of select="Armament/Weapon/text()"/>
        </xsl:element>
        <xsl:element name="Owner">
          <xsl:value-of select="$parent/Owner/text()"/>
        </xsl:element>
      </xsl:element>
      <xsl:call-template name="recurse_create_event_armament">
        <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
        <xsl:with-param name="platform_count" select="$platform_count"/>
        <xsl:with-param name="armament_count" select="number($armament_count) + 1"/>
        <xsl:with-param name="parent" select="$parent"/>
        <xsl:with-param name="mode" select="$mode"/>
        <xsl:with-param name="last_platform" select="$last_platform"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>


  <xsl:template name="append_armaments">
    <xsl:param name="sub_platform_node"/>
    <xsl:param name="armament_count"/>
    <xsl:param name="platform_count"/>
    <xsl:param name="parent"/>

    <xsl:if test="(number($armament_count)) &lt; (number($sub_platform_node/Armament/Count))">
      <xsl:value-of select="concat(concat(concat(concat(concat(concat(concat(concat($parent/ID/text(), '_'),$sub_platform_node/Kind/text()),'_'), 
			  		  $platform_count), '_'), $sub_platform_node/Armament/Weapon/text()), '_'), $armament_count)"/>

      <xsl:if test="number($armament_count) &lt; ((number($sub_platform_node/Armament/Count)) - 1)">
        <xsl:text>,</xsl:text>
      </xsl:if>

      <xsl:call-template name="append_armaments">
        <xsl:with-param name="sub_platform_node" select="$sub_platform_node"/>
        <xsl:with-param name="armament_count" select="number($armament_count) + 1"/>
        <xsl:with-param name="platform_count" select="$platform_count"/>
        <xsl:with-param name="parent" select="$parent"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>


  <xsl:template name="append_adopt_platform">
    <xsl:if test="count(parent::node()/Adopt_Platform) &gt; 0">
      <xsl:for-each select="parent::node()/Adopt_Platform">
        <xsl:text>,</xsl:text>
        <xsl:value-of select="Child/text( )"/>
      </xsl:for-each>
    </xsl:if>

  </xsl:template>

  <xsl:template match="Armament" mode="Subplatform">
    <xsl:element name="Armament">
      <xsl:element name="Weapon">
        <xsl:value-of select="Weapon/text()"/>
      </xsl:element>
      <xsl:element name="Count">
        <xsl:value-of select="Count/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Docked" mode="Subplatform">
    <xsl:element name="Docked">
      <xsl:element name="Count">
        <xsl:value-of select="Count/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Launched" mode="Subplatform">
    <xsl:element name="Launched">
      <xsl:if test="ID">
        <xsl:element name="ID">
          <xsl:value-of select="ID/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:apply-templates select="Location" mode="Platform"/>
      <xsl:if test="InitialState">
        <xsl:element name="InitialState">
          <xsl:value-of select="InitialState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="InitialParameters">
        <xsl:element name="InitialParameters">
          <xsl:apply-templates select="InitialParameters" mode="Platform"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Adopt_Platform" mode="Create_Event">
    <xsl:element name="Adopt_Platform">
      <xsl:element name="Child">
        <xsl:value-of select="Child/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Location" mode="Platform"/>
      <xsl:if test="InitialState">
        <xsl:element name="InitialState">
          <xsl:value-of select="InitialState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="InitialParameters">
        <xsl:element name="InitialParameters">
          <xsl:apply-templates select="InitialParameters" mode="Platform"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Location" mode="Platform">
    <xsl:element name="Location">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="InitialParameters" mode="Platform">
    <xsl:for-each select="./*">
      <xsl:if test="name()='Parameter'">
        <xsl:element name="Parameter">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Setting'">
        <xsl:element name="Setting">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="FlushEvents">
    <xsl:element name="FlushEvents">
      <xsl:element name="Unit">
        <xsl:value-of select="text()"/>
      </xsl:element>
      <xsl:element name="Time">
        <xsl:value-of select="text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Launch_Event">
    <xsl:element name="Launch_Event">
      <xsl:element name="Parent">
        <xsl:value-of select="Parent/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:if test="Kind">
        <xsl:call-template name="display_kind_id">
          <xsl:with-param name="current_launch_event" select="."/>
        </xsl:call-template>
      </xsl:if>
      <xsl:if test="Child">
        <xsl:element name="Child">
          <xsl:value-of select="Child/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="RelativeLocation">
        <xsl:value-of select="RelativeLocation/text()"/>
      </xsl:element>
      <xsl:if test="InitialState">
        <xsl:element name="InitialState">
          <xsl:value-of select="InitialState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="StartupParameters">
        <xsl:element name="StartupParameters">
          <xsl:apply-templates select="StartupParameters" mode="Platform"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>


  <xsl:template name="display_kind_id">
    <xsl:param name="current_launch_event"/>


    <xsl:variable name="launch_event_kind" select="key('launch_event_kind_key', $current_launch_event/Kind)"/>
    <xsl:variable name="create_event_node" select="/Scenario/Create_Event[Subplatform/Kind=$current_launch_event/Kind]"/>



    <xsl:for-each select="$launch_event_kind">
      <xsl:if test="./RelativeLocation = $current_launch_event/RelativeLocation">
        <xsl:element name="Child">
          <xsl:value-of select="concat( concat(concat(concat($create_event_node/ID/text(), '_'),$current_launch_event/Kind), '_'), (position()-1) )"/>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>


    <!--xsl:variable name="launch_event_node" select="/Scenario/Launch_Event[Parent != $current_launch_event/Parent]"/>	
			<xsl:variable name="launch_event_node" select="/Scenario/Launch_Event[Parent != $current_launch_event/Parent]"/>	
		<xsl:variable name="create_event_node" select="/Scenario/Create_Event[Subplatform/Kind=$kind][ID != $current_launch_event/Parent]"/>
	

		<xsl:variable name="create_event_node" select="/Scenario/Create_Event[Subplatform/Kind=$kind][ID != $current_launch_event/Parent]"/>
			
		<xsl:call-template name="render_kind_id">
			<xsl:with-param name="launch_event_node" select="$launch_event_node"/>
			<xsl:with-param name="create_event_node" select="$create_event_node"/>
			<xsl:with-param name="count" select="'1'"/>
		</xsl:call-template-->
  </xsl:template>


  <xsl:template name="render_kind_id">
    <xsl:param name="launch_event_node"/>
    <xsl:param name="create_event_node"/>
    <xsl:param name="count"/>

    <xsl:variable name="current_launch_event_node" select="$launch_event_node[position()=number($count)]"/>
    <xsl:choose>
      <xsl:when test="count($create_event_node[ID != $current_launch_event_node/Parent and position()= 1] )&gt; 0">
        <xsl:for-each select="$create_event_node[ID != $current_launch_event_node/Parent and position()= 1]">
          <xsl:element name="Child">
            <xsl:value-of select="ID/text()"/>
          </xsl:element>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="render_kind_id">
          <xsl:with-param name="launch_event_node" select="$launch_event_node"/>
          <xsl:with-param name="create_event_node" select="$create_event_node"/>
          <xsl:with-param name="count" select="$count + 1"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="StartupParameters" mode="Platform">
    <xsl:for-each select="./*">
      <xsl:if test="name()='Parameter'">
        <xsl:element name="Parameter">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="name()='Setting'">
        <xsl:element name="Setting">
          <xsl:value-of select="text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>


  <xsl:template match="Move_Event">
    <xsl:element name="Move_Event">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:element name="Throttle">
        <xsl:value-of select="Throttle/text()"/>
      </xsl:element>
      <xsl:element name="Destination">
        <xsl:value-of select="Destination/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="OpenChatRoom">
    <xsl:element name="OpenChatRoom">
      <xsl:element name="Room">
        <xsl:value-of select="Room/text()"/>
      </xsl:element>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:apply-templates select="Members"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Members">
    <xsl:element name="Members">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Reiterate">
    <xsl:element name="Reiterate">
      <xsl:element name="Start">
        <xsl:value-of select="Start/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:apply-templates select="ReiterateThis" mode="Reiterate"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="ReiterateThis" mode="Reiterate">
    <xsl:element name="ReiterateThis">
      <xsl:apply-templates select="Move_Event"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="RemoveEngram" >
    <xsl:element name="RemoveEngram">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Reveal_Event">
    <xsl:element name="Reveal_Event">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:element name="InitialLocation">
        <xsl:value-of select="InitialLocation/text()"/>
      </xsl:element>
      <xsl:if test="InitialState">
        <xsl:element name="InitialState">
          <xsl:value-of select="InitialState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="StartupParameters">
        <xsl:element name="StartupParameters">
          <xsl:apply-templates select="StartupParameters" mode="Platform"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Species_Completion_Event">
    <xsl:element name="Species_Completion_Event">
      <xsl:element name="Species">
        <xsl:value-of select="Species/text()"/>
      </xsl:element>
      <xsl:if test="Action">
        <xsl:element name="Action">
          <xsl:value-of select="Action/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="NewState">
        <xsl:element name="NewState">
          <xsl:value-of select="NewState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:apply-templates select="DoThis" mode="Completion_Event"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="StateChange_Event" >
    <xsl:element name="StateChange_Event">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:element name="NewState">
        <xsl:value-of select="NewState/text()"/>
      </xsl:element>
      <xsl:apply-templates select="From" mode="Event"/>
      <xsl:apply-templates select="Except" mode="Event"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="From" mode="Event">
    <xsl:element name="From">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Except" mode="Event">
    <xsl:element name="Except">
      <xsl:value-of select="text()"/>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Transfer_Event">
    <xsl:element name="Transfer_Event">
      <xsl:element name="ID">
        <xsl:value-of select="ID/text()"/>
      </xsl:element>
      <xsl:if test="EngramRange">
        <xsl:element name="EngramRange">
          <xsl:apply-templates select="EngramRange" mode="Event"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Time">
        <xsl:value-of select="Time/text()"/>
      </xsl:element>
      <xsl:element name="From">
        <xsl:value-of select="From/text()"/>
      </xsl:element>
      <xsl:element name="To">
        <xsl:value-of select="To/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template match="Rule" >
    <xsl:element name="Rule">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
      <xsl:element name="Unit">
        <xsl:element name="Owner">
          <xsl:value-of select="Unit/Owner/text()"/>
        </xsl:element>
        <xsl:element name="ID">
          <xsl:value-of select="Unit/ID/text()"/>
        </xsl:element>
        <xsl:if test="Unit/Region">
          <xsl:element name="Region">
            <xsl:element name="Zone">
              <xsl:value-of select="Unit/Region/Zone/text()"/>
            </xsl:element>
            <xsl:if test="Unit/Region/Relationship">
              <xsl:element name="Relationship">
                <xsl:value-of select="Unit/Region/Relationship/text()"/>
              </xsl:element>
            </xsl:if>
          </xsl:element>
        </xsl:if>
      </xsl:element>
      <xsl:if test="Object">
        <xsl:element name="Object">
          <xsl:element name="Owner">
            <xsl:value-of select="Object/Owner/text()"/>
          </xsl:element>
          <xsl:element name="ID">
            <xsl:value-of select="Object/ID/text()"/>
          </xsl:element>
          <xsl:if test="Object/Region">
            <xsl:element name="Region">
              <xsl:element name="Zone">
                <xsl:value-of select="Object/Region/Zone/text()"/>
              </xsl:element>
              <xsl:if test="Object/Region/Relationship">
                <xsl:element name="Relationship">
                  <xsl:value-of select="Object/Region/Relationship/text()"/>
                </xsl:element>
              </xsl:if>
            </xsl:element>
          </xsl:if>
        </xsl:element>
      </xsl:if>
      <xsl:if test="NewState">
        <xsl:element name="NewState">
          <xsl:value-of select="NewState/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="From">
        <xsl:element name="From">
          <xsl:value-of select="From/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:element name="Increment">
        <xsl:value-of select="Increment/text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Score" >
    <xsl:element name="Score">
      <xsl:element name="Name">
        <xsl:value-of select="Name/text()"/>
      </xsl:element>
      <xsl:element name="Rules">
        <xsl:value-of select="Rules/text()"/>
      </xsl:element>
      <xsl:element name="Applies">
        <xsl:value-of select="Applies/text()"/>
      </xsl:element>
      <xsl:if test="Viewers">
        <xsl:element name="Viewers">
          <xsl:value-of select="Viewers/text()"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="Initial">
        <xsl:element name="Initial">
          <xsl:value-of select="Initial/text()"/>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="text()"/>

</xsl:stylesheet>
