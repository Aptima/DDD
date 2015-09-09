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
		<xsl:apply-templates select="Functions"/>
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
			<xsl:attribute name="Type">LandRegion</xsl:attribute>
			<xsl:attribute name="Name">Land Regions</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<Functions>
				<Function Name="Create Land Region" Action="CreateComponent" Visible="true"/>
			</Functions>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='LandRegion']">
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">ActiveRegion</xsl:attribute>
			<xsl:attribute name="Name">Active Region</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<Functions>
				<Function Name="Create Active Region" Action="CreateComponent" Visible="true"/>
			</Functions>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='ActiveRegion']">
			</xsl:apply-templates>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='LandRegion']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="Component[@Type='LandRegion']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='ActiveRegion']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="Component[@Type='ActiveRegion']"/>
		</Component>
	</xsl:template>
	<xsl:template match="ComponentParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="Functions">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
