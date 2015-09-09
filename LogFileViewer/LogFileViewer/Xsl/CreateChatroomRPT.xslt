<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://www.w3.org/2005/xpath-functions">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">

			<a name="CreateChatRoom"><h2 style="color:white">Create Chatroom Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/CreateChatRoom">
					<!--<xsl:apply-templates select="Scenario/CreateChatRoom">
						<xsl:sort data-type="number" select="Parameter[3]/Value"/>
					</xsl:apply-templates>-->
						<xsl:call-template name="CreateChatRoom"/>
							
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
	
	
	
	<xsl:template name="CreateChatRoom">
		<xsl:for-each select="Scenario/CreateChatRoom[not ((Parameter[3]/Value = preceding-sibling::CreateChatRoom/Parameter[3]/Value) and (Parameter[1]/Name = preceding-sibling::CreateChatRoom/Parameter[1]/Name))]">
			<p>

				<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td with="25%"><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></td>
					<td width="75%"><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Name"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div>
							<b>
								<xsl:text>Membership: </xsl:text>
							</b>
							<xsl:for-each select="Parameter[2]/Value/StringListType/Value">
								<xsl:value-of select="current()"/>
								<xsl:text>, </xsl:text>
							</xsl:for-each>
						</div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
		</xsl:for-each>

</xsl:template>

	</xsl:stylesheet>




