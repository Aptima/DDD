<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:template match="/">
		<xsl:apply-templates select="/Components"/>
	</xsl:template>
	<xsl:template match="Components">
		<!--		<Components xsi:noNamespaceSchemaLocation="C:\workspace\MOST\Trunk\src\Config\ConfigXSD\Components.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">-->
		<Components xsi:noNamespaceSchemaLocation="Components.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
			<xsl:apply-templates select="Component">
				<xsl:sort select="@LinkID" data-type="number"/>
			</xsl:apply-templates>
		</Components>
	</xsl:template>
	<xsl:template match="Component">
		<!--<Component Type="Task" ID="913" Name="30101" Description="" eType="Instance" LinkID="3795" ClassID="901" SubclassID="908">-->
		<Component>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:if test="@BaseType">
				<xsl:attribute name="BaseType"><xsl:value-of select="@BaseType"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@LinkID">
				<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@ClassID">
				<xsl:attribute name="ClassID"><xsl:value-of select="@ClassID"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@SubclassID">
				<xsl:attribute name="SubclassID"><xsl:value-of select="@SubclassID"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="ComponentParameters"/>
			<xsl:apply-templates select="LinkParameters"/>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="Component">
				<xsl:sort select="@LinkID" data-type="number"/>
			</xsl:apply-templates>
		</Component>
	</xsl:template>
	<xsl:template match="ComponentParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="LinkParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="Functions">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
