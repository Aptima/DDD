<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">

			<a name="NewObject"><h2 style="color:white">New Object Events</h2>	</a>
			<xsl:choose>
				<xsl:when test="Scenario/NewObject">
					<xsl:apply-templates select="Scenario/NewObject">
					<!-- Sort by Time, Object Type -->
						<!--<xsl:sort data-type="number" select="Parameter[5]/Value"/>
						<xsl:sort data-type="text" select="Parameter[1]/Value"/>-->
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
	
	
	
	<xsl:template match="AttributeCollectionType">
	<xsl:choose>
		<xsl:when test="Attribute">	
			<xsl:for-each select="Attribute">
				<tr>
					<td width="10%" align="right"><i><xsl:value-of select="Name"/><xsl:text>:</xsl:text></i></td>
					<xsl:choose>
						<xsl:when test="Value/PolygonType">
							<td width="90%">
								<xsl:apply-templates select="Value/PolygonType"/>
							</td>
						</xsl:when>
						<xsl:when test="Value/LocationType">
							<td>
								<xsl:apply-templates select="Value/LocationType"/>
							</td>					
						</xsl:when>
						<xsl:otherwise>
							<td> 
								<xsl:value-of select="Value"/>
							</td>
						</xsl:otherwise>
					</xsl:choose>			
				</tr>
			</xsl:for-each>
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>none.</xsl:text>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	
	<xsl:template match="StateTableType">
		<xsl:choose>
			<xsl:when test="StateName">
				<tr>
					<td>	
						<xsl:for-each select="StateName">
								<xsl:value-of select="Name"/><xsl:text>, </xsl:text>
						</xsl:for-each>
					</td>
				</tr>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>none.</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="PolygonType">
		<xsl:choose>
			<xsl:when test="Point">
				<xsl:for-each select="Point">
						<xsl:text>(</xsl:text><xsl:value-of select="X"/><xsl:text>, </xsl:text>
						<xsl:value-of select="Y"/><xsl:text>), </xsl:text>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>none.</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="LocationType">
				<xsl:text>(</xsl:text>
				<xsl:value-of select="X"/><xsl:text>, </xsl:text>
				<xsl:value-of select="Y"/><xsl:text>, </xsl:text>
				<xsl:value-of select="Z"/>
				<xsl:text>)</xsl:text>
	</xsl:template>



	<xsl:template match="NewObject">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tr bgcolor="#C4CCD5">
				<td width="25%">
					<b><xsl:text>Time: </xsl:text></b>
					<xsl:value-of select="Parameter[5]/Value"/>
				</td>
				<td width="75%">
					<b><xsl:text>ID: </xsl:text></b>
					<xsl:value-of select="Parameter[1]/Value"/>
				</td>
			</tr>
			<tr>	
				<td colspan="100%">
					<b><xsl:text>Type: </xsl:text></b>
					<xsl:value-of select="Parameter[2]/Value"/>
				</td>
			</tr>
			<tr>
				<td colspan="100%">
					<b>Attributes</b>
					<table cellpadding="3" width="100%">
						<tbody align="left" >
							<xsl:apply-templates select="Parameter[4]/Value/AttributeCollectionType"/>
						</tbody>
					</table>
				</td>
			</tr>
			<tr>
				<td colspan="100%">
					<b>States</b>
					<table cellpadding="3">
						<tbody align="left">
							<xsl:apply-templates select="Parameter[3]/Value/StateTableType"/>
						</tbody>
					</table>
				</td>
			</tr>
		</table>
	</p>
	</xsl:template>	
	</xsl:stylesheet>




