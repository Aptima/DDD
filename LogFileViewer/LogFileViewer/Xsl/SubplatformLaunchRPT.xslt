<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">


			<a name="SubplatformLaunch"><h2 style="color:white">Subplatform Launch Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/SubplatformLaunch">
					<xsl:apply-templates select="Scenario/SubplatformLaunch">
					<!-- Sort by Time, Object Type -->
						<xsl:sort data-type="number" select="Parameter[4]/Value"/>
						<!--<xsl:sort data-type="text" select="Parameter[2]/Value"/>-->
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
				<xsl:value-of select="X"/><xsl:text>, </xsl:text>
				<xsl:value-of select="Y"/><xsl:text>, </xsl:text>
				<xsl:value-of select="Z"/>
				<xsl:text>)</xsl:text>
	</xsl:template>

	
	<xsl:template match="SubplatformLaunch">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%">
						<b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[4]/Value"/>
					</td>
					<td width="75%">
						<b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/>
					</td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Parent ID: </xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
						<div><b><xsl:text>Destination Location(x,y,z): </xsl:text></b><xsl:apply-templates select="Parameter[3]/Value/LocationType"/></div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>	
	
	</xsl:stylesheet>




