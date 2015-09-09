<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">

			<a name="WeaponLaunch"><h2 style="color:white">Weapon Launch Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/WeaponLaunch">
					<xsl:apply-templates select="Scenario/WeaponLaunch">
					<!-- Sort by Time -->
						<xsl:sort data-type="number" select="Parameter[4]/Value"/>
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
	
	
	
	<xsl:template match="WeaponLaunch">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td with="25%"><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[4]/Value"/></td>
					<td width="75%"><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Parent ID: </xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
						<div><b><xsl:text>Target ID: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	</xsl:stylesheet>




