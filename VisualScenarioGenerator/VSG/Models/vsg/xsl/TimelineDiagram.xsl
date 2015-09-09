<?xml version="1.0" encoding="UTF-8"?>
<?altova_samplexml Untitled7.xml?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:template match="/">
	<Components>
		<Component>
			<xsl:apply-templates select="Components/Component[@Type='CreateEvent']"/>
		</Component>
	</Components>
	</xsl:template>
	<xsl:template match="Component[@Type='CreateEvent']">
		<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
		<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
		<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
		<ComponentParameters>
			<Parameter category="Image" type="Complex">
				<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
			</Parameter>
		</ComponentParameters>
		<Functions>
			<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
		</Functions>
		<xsl:apply-templates select="//Component[@Type='RevealEvent' or @Type='MoveEvent' or @Type='AdoptPlatform' or @Type='LaunchEvent']"/>
	</xsl:template>
	<xsl:template match="Component[@Type='RevealEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<Functions>
				<xsl:apply-templates select="Functions/Function"/>
				<Function Name="VisualRepresentation" Action="VSG.CustomDiagramNodes.SensorNode@Image.Image File" Visible="false"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='MoveEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<Functions>
				<xsl:apply-templates select="Functions/Function"/>
				<Function Name="VisualRepresentation" Action="VSG.CustomDiagramNodes.SensorNode@Image.Image File" Visible="false"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='AdoptPlatform']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<Functions>
				<xsl:apply-templates select="Functions/Function"/>
				<Function Name="VisualRepresentation" Action="VSG.CustomDiagramNodes.SensorNode@Image.Image File" Visible="false"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='LaunchEvent']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:apply-templates select="ComponentParameters"/>
			<Functions>
				<xsl:apply-templates select="Functions/Function"/>
				<Function Name="VisualRepresentation" Action="VSG.CustomDiagramNodes.SensorNode@Image.Image File" Visible="false"/>
			</Functions>
		</Component>
	</xsl:template>
	<xsl:template match="ComponentParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="Functions">
		<xsl:copy-of select="."/>
	</xsl:template>
		<xsl:template match="Function">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
