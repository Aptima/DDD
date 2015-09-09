<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:template match="/">
		<Component>
			<xsl:apply-templates select="//Component[@Type='Scenario']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Scenario']">
		<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
		<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
		<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
		<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
		<xsl:apply-templates select="Functions"/>
		<!--<xsl:apply-templates select="ComponentParameters"/>-->
		<ComponentParameters>
			<Parameter category="Image" type="Complex">
				<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
			</Parameter>
		</ComponentParameters>
		<Functions>
			<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
		</Functions>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Team</xsl:attribute>
			<xsl:attribute name="Name">Teams</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Team" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Team']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">DecisionMaker</xsl:attribute>
			<xsl:attribute name="Name">Decision Makers</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create DecisionMaker" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='DecisionMaker']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Network</xsl:attribute>
			<xsl:attribute name="Name">Networks (Sensors)</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Network" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Network']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Classification</xsl:attribute>
			<xsl:attribute name="Name">Classifications</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Classification" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Classification']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<!--<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">OpenVoiceChannelEvent</xsl:attribute>
			<xsl:attribute name="Name">Voice Channels</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create VoiceChannel" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='OpenVoiceChannelEvent']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>-->
	</xsl:template>
	<xsl:template match="Component[@Type='Team']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='DecisionMaker']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<Functions>
				<Function Name="Drag" Action="ApplicationNodes.PlayerObjectTypesTreeNode" Visible="false"/>
				<Function Name="Delete" Action="deleteDecisionMaker" Visible="true"/>
				<Function Name="Rename" Action="RenameComponent" Visible="true"/>
			</Functions>
			<!--<xsl:apply-templates select="Functions"/> -->
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Network']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='OpenVoiceChannelEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Classification']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="ComponentParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="Functions">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
