<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">
	
			<a name="MoveDone"><h2 style="color:white">Move Done Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/MoveDone">
					<xsl:apply-templates select="Scenario/MoveDone">
					<!-- Sort by Time, ID -->
						<xsl:sort data-type="number" select="Parameter[3]/Value"/>
						<xsl:sort data-type="text" select="Parameter[1]/Value"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<table bgcolor="#FFFFFF" width="100%">
						<tr>
							<td><b>No Records Found.</b></td>
						</tr>
					</table>
				</xsl:otherwise>
			</xsl:choose>
			<br />
			<br />

		</body>
	</html>
	</xsl:template>
		

	
	<xsl:template match="MoveDone">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%"><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></td>
					<td width="75%"><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<b><xsl:text>Reason: </xsl:text></b><xsl:value-of select="Parameter[2]/Value"/>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>
	
	</xsl:stylesheet>




