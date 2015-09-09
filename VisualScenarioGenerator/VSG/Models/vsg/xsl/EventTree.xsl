<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:template match="/">
		<Component>
			<xsl:apply-templates select="//Component[@Type='CreateEvent']"/>
			<xsl:apply-templates select="//Component[@Type='Scenario']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='LaunchEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
  <xsl:template match="Component[@Type='WeaponLaunchEvent' and ../@Type='CompletionEvent']">
    <Component>
      <xsl:attribute name="ID">
        <xsl:value-of select="@ID"/>
      </xsl:attribute>
      <xsl:attribute name="Type">
        <xsl:value-of select="@Type"/>
      </xsl:attribute>
      <xsl:attribute name="Name">
        <xsl:value-of select="@Name"/>
      </xsl:attribute>
      <xsl:attribute name="Description">
        <xsl:value-of select="@Description"/>
      </xsl:attribute>
      <xsl:attribute name="eType">
        <xsl:value-of select="@eType"/>
      </xsl:attribute>
      <xsl:attribute name="LinkID">
        <xsl:value-of select="@LinkID"/>
      </xsl:attribute>
      <ComponentParameters>
        <Parameter category="Image" type="Complex">
          <Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
        </Parameter>
      </ComponentParameters>
      <Functions>
        <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
        <Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
        <Function Name="Move Up" Action="moveUp" Visible="true"/>
        <Function Name="Move Down" Action="moveDown" Visible="true"/>
        <Function Name="Delete" Action="deleteEvent" Visible="true"/>
        <Function Name="-----------" Action="" Visible="true"/>
        <Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
        <Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
        <Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
        <Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
        <Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
        <Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
        <Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
        <Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
        <Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
        <Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
        <Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
      </Functions>
    </Component>
  </xsl:template>
	<xsl:template match="Component[@Type='LaunchEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
  <xsl:template match="Component[@Type='WeaponLaunchEvent' and ../@Type='SpeciesCompletionEvent']">
    <Component>
      <xsl:attribute name="ID">
        <xsl:value-of select="@ID"/>
      </xsl:attribute>
      <xsl:attribute name="Type">
        <xsl:value-of select="@Type"/>
      </xsl:attribute>
      <xsl:attribute name="Name">
        <xsl:value-of select="@Name"/>
      </xsl:attribute>
      <xsl:attribute name="Description">
        <xsl:value-of select="@Description"/>
      </xsl:attribute>
      <xsl:attribute name="eType">
        <xsl:value-of select="@eType"/>
      </xsl:attribute>
      <xsl:attribute name="LinkID">
        <xsl:value-of select="@LinkID"/>
      </xsl:attribute>
      <ComponentParameters>
        <Parameter category="Image" type="Complex">
          <Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
        </Parameter>
      </ComponentParameters>
      <Functions>
        <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
        <Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
        <Function Name="Move Up" Action="moveUp" Visible="true"/>
        <Function Name="Move Down" Action="moveDown" Visible="true"/>
        <Function Name="Delete" Action="deleteEvent" Visible="true"/>
        <Function Name="-----------" Action="" Visible="true"/>
        <Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
        <Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
        <Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
        <Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
        <Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
        <Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
        <Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
        <Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
        <Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
        <Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
        <Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
      </Functions>
    </Component>
  </xsl:template>
	<xsl:template match="Component[@Type='LaunchEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
  <xsl:template match="Component[@Type='WeaponLaunchEvent' and ../@Type='CreateEvent']">
    <Component>
      <xsl:attribute name="ID">
        <xsl:value-of select="@ID"/>
      </xsl:attribute>
      <xsl:attribute name="Type">
        <xsl:value-of select="@Type"/>
      </xsl:attribute>
      <xsl:attribute name="Name">
        <xsl:value-of select="@Name"/>
      </xsl:attribute>
      <xsl:attribute name="Description">
        <xsl:value-of select="@Description"/>
      </xsl:attribute>
      <xsl:attribute name="eType">
        <xsl:value-of select="@eType"/>
      </xsl:attribute>
      <xsl:attribute name="LinkID">
        <xsl:value-of select="@LinkID"/>
      </xsl:attribute>
      <ComponentParameters>
        <Parameter category="Image" type="Complex">
          <Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
        </Parameter>
      </ComponentParameters>
      <Functions>
        <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
        <Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
        <Function Name="Move Up" Action="moveUp" Visible="true"/>
        <Function Name="Move Down" Action="moveDown" Visible="true"/>
        <Function Name="Delete" Action="deleteEvent" Visible="true"/>
        <Function Name="-----------" Action="" Visible="true"/>
        <Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
        <Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
        <Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
        <Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
        <Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
        <Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
      </Functions>
    </Component>
  </xsl:template>
	<xsl:template match="Component[@Type='CompletionEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='CompletionEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='CompletionEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SendChatMessageEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='TransferEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='TransferEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='TransferEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Create OpenChatRoomEvent Child" Action="createChild(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent Child" Action="createChild(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent Child" Action="createChild(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent Child" Action="createChild(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent Child" Action="createChild(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent Child" Action="createChild(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createChild(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='OpenChatRoomEvent' or @Type='CloseChatRoomEvent' or @Type='SendChatMessageEvent' or @Type='OpenVoiceChannelEvent' or @Type='CloseVoiceChannelEvent' or @Type='SendVoiceMessageEvent' or @Type='SendVoiceMessageToUserEvent' or @Type='RemoveEngramEvent' or @Type='ChangeEngramEvent' or @Type='ReiterateEvent' or @Type='SpeciesCompletionEvent' or @Type='FlushEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SpeciesCompletionEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SpeciesCompletionEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SpeciesCompletionEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent Child" Action="createChild(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent Child" Action="createChild(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent Child" Action="createChild(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent Child" Action="createChild(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent Child" Action="createChild(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='ReiterateEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent' or @Type='ChangeEngramEvent' or @Type='RemoveEngramEvent' or @Type='FlushEvent' or @Type='SpeciesCompletionEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='CloseChatRoomEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='StateChangeEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='StateChangeEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='StateChangeEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ChangeEngramEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ChangeEngramEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ChangeEngramEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='OpenVoiceChannelEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SendVoiceMessageEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SendVoiceMessageToUserEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='CloseVoiceChannelEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='MoveEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='MoveEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='MoveEvent' and ../@Type='ReiterateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='MoveEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ReiterateEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='MoveEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ReiterateEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='MoveEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ReiterateEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='MoveEvent']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='OpenChatRoomEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='FlushEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='FlushEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='FlushEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RemoveEngramEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RemoveEngramEvent' and ../@Type='Scenario']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create OpenChatRoomEvent After" Action="createSibling(OpenChatRoomEvent)" Visible="true"/>
				<Function Name="Create CloseChatRoomEvent After" Action="createSibling(CloseChatRoomEvent)" Visible="true"/>
				<Function Name="Create SendChatMessageEvent After" Action="createSibling(SendChatMessageEvent)" Visible="true"/>
				<Function Name="Create OpenVoiceChannelEvent After" Action="createSibling(OpenVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create CloseVoiceChannelEvent After" Action="createSibling(CloseVoiceChannelEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageEvent After" Action="createSibling(SendVoiceMessageEvent)" Visible="true"/>
				<Function Name="Create SendVoiceMessageToUserEvent After" Action="createSibling(SendVoiceMessageToUserEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RemoveEngramEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RevealEvent' and ../@Type='CompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RevealEvent' and ../@Type='SpeciesCompletionEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create ReiterateEvent After" Action="createSibling(ReiterateEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
				<Function Name="Create ChangeEngramEvent After" Action="createSibling(ChangeEngramEvent)" Visible="true"/>
				<Function Name="Create RemoveEngramEvent After" Action="createSibling(RemoveEngramEvent)" Visible="true"/>
				<Function Name="Create FlushEvent After" Action="createSibling(FlushEvent)" Visible="true"/>
				<Function Name="Create SpeciesCompletionEvent After" Action="createSibling(SpeciesCompletionEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='RevealEvent' and ../@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Move Up" Action="moveUp" Visible="true"/>
				<Function Name="Move Down" Action="moveDown" Visible="true"/>
				<Function Name="Delete" Action="deleteEvent" Visible="true"/>
				<Function Name="-----------" Action="" Visible="true"/>
				<Function Name="Create RevealEvent After" Action="createSibling(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent After" Action="createSibling(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent After" Action="createSibling(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent After" Action="createSibling(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent After" Action="createSibling(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent After" Action="createSibling(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent After" Action="createSibling(StateChangeEvent)" Visible="true"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>
				<Function Name="Create RevealEvent Child" Action="createChild(RevealEvent)" Visible="true"/>
				<Function Name="Create MoveEvent Child" Action="createChild(MoveEvent)" Visible="true"/>
				<Function Name="Create CompletionEvent Child" Action="createChild(CompletionEvent)" Visible="true"/>
				<Function Name="Create LaunchEvent Child" Action="createChild(LaunchEvent)" Visible="true"/>
        <Function Name="Create WeaponLaunchEvent Child" Action="createChild(WeaponLaunchEvent)" Visible="true"/>
				<Function Name="Create TransferEvent Child" Action="createChild(TransferEvent)" Visible="true"/>
				<Function Name="Create StateChangeEvent Child" Action="createChild(StateChangeEvent)" Visible="true"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='CompletionEvent' or @Type='LaunchEvent' or @Type='WeaponLaunchEvent' or @Type='TransferEvent' or @Type='StateChangeEvent']"/>
		</Component>
	</xsl:template>
</xsl:stylesheet>
