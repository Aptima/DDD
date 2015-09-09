<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" />
<xsl:template match="/">
	<xsl:choose>
		<xsl:when test="/Scenario/CreateChatRoom">
			<xsl:for-each select="/Scenario/CreateChatRoom[not ((Parameter[3]/Value = preceding-sibling::CreateChatRoom/Parameter[3]/Value) and (Parameter[1]/Name = preceding-sibling::CreateChatRoom/Parameter[1]/Name))]">
				<xsl:value-of select="Parameter[3]/Value"/>
				<xsl:text>,</xsl:text>
				<xsl:value-of select="Parameter[1]/Value"/>
				<xsl:text>,</xsl:text>
				<xsl:for-each select="Parameter[2]/Value/StringListType/Value">
					<xsl:value-of select="current()"/>
					<xsl:text> </xsl:text>
				</xsl:for-each>
				<xsl:text>&#10;</xsl:text>
			</xsl:for-each>
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>0 records.</xsl:text>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>
</xsl:stylesheet>





