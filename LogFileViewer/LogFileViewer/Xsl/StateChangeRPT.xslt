<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">
			<h2 style="color:white">State Change Events</h2>
			<xsl:choose>
				<xsl:when test="Scenario/StateChange">			
					<xsl:apply-templates select="Scenario/StateChange">
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
	
	


	<xsl:template match="StateChange">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%">
						<div><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>
					<td width="75%">
						<div><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></div>
					</td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>New State: </xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	
	</xsl:stylesheet>




