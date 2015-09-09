<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" />
<xsl:template match="/">
	<xsl:choose>
		<xsl:when test="/Scenario/NewObject">
			<xsl:for-each select="/Scenario/NewObject">
				<xsl:value-of select="Parameter[5]/Value"/>
				<xsl:text>,</xsl:text>
				<xsl:value-of select="Parameter[1]/Value"/>
				<xsl:text>,</xsl:text>
				<xsl:value-of select="Parameter[2]/Value"/>
				<xsl:text>,</xsl:text>
				<xsl:for-each select="Parameter[4]/Value/AttributeCollectionType">
					<xsl:choose>
						<xsl:when test="Attribute/Name = 'OwnerID'">
							<xsl:value-of select="Attribute/Value"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>nil</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
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





