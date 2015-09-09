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
			<xsl:attribute name="Type">DecisionMaker</xsl:attribute>
			<xsl:attribute name="Name">Decision Makers</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<xsl:apply-templates select="Component[@Type='DecisionMaker']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">GlobalEvents</xsl:attribute>
			<xsl:attribute name="Name">Global Events</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<!--<xsl:apply-templates select="Component[@Type='DecisionMaker']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>-->
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='DecisionMaker']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<Functions>
			<Function Name="Drag" Action="ApplicationNodes.CreateInstanceTreeNode" Visible="false"/>
            <Function Name="Create Unit" Action="createObjectInstance" Visible="true"/>

            </Functions>
			<xsl:apply-templates select="ComponentParameters"/>
            <xsl:apply-templates select="Component[@Type='CreateEvent']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
	</xsl:template>
    <xsl:template match="Component[@Type='CreateEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<Functions>
				<Function Name="Rename" Action="RenameComponent" Visible="true"/>
				<Function Name="Delete" Action="DeleteComponent" Visible="true"/>
            </Functions>
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
