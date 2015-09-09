<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">

			<a name="History_AttackerObjectReport"><h2 style="color:white">Attacker Object Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/History_AttackerObjectReport">
					<xsl:apply-templates select="Scenario/History_AttackerObjectReport">
					<!-- Sort by Time -->
						<xsl:sort data-type="number" select="Parameter[6]/Value"/>
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


	<xsl:template match="LocationType">
		<xsl:text>(</xsl:text>
		<xsl:value-of select="X"/>
		<xsl:text>, </xsl:text>
		<xsl:value-of select="Y"/>
		<xsl:text>, </xsl:text>
		<xsl:value-of select="Z"/>
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template match="History_AttackerObjectReport">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td with="25%"><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[6]/Value"/></td>
					<td width="25%"><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
          <td width="25%"><b><xsl:text>TargetID: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></td>
					<td width="25%"><b><xsl:text>Capability Name: </xsl:text></b><xsl:value-of select="Parameter[5]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Attacker Location: </xsl:text></b><xsl:apply-templates select="Parameter[2]/Value/LocationType"/></div>
            <div><b><xsl:text>Target Location: </xsl:text></b><xsl:apply-templates select="Parameter[4]/Value/LocationType"/></div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	</xsl:stylesheet>




