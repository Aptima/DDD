<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
		<body bgcolor="#566578">
			<h1 style="color:white"><xsl:value-of select="Scenario/@name"/></h1>

			<br><a style="color:white" href="#NewObject">New Object Events</a></br>
			<br><a style="color:white" href="#RevealObject">Reveal Object Events</a></br>
			<br><a style="color:white" href="#SubplatformLaunch">Subplatform Launch Events</a></br>
			<br><a style="color:white" href="#WeaponLaunch">Weapon Launch Events</a></br>
			<br><a style="color:white" href="#SubplatformDock">Subplatform Dock Events</a></br>
			<br><a style="color:white" href="#AttackObject">Attack Object Events</a></br>
			<br><a style="color:white" href="#SelfDefenseAttackStarted">Self Defense Attack Started Events</a></br>
			<br><a style="color:white" href="#MoveObject">Move Object Events</a></br>
			<br><a style="color:white" href="#MoveDone">Move Done Events</a></br>
			<br><a style="color:white" href="#StateChange">State Change Events</a></br>
			<br><a style="color:white" href="#CreateChatroom">Create Chatroom Events</a></br>
			<br><a style="color:white" href="#TextChatMessage">Text Chat Message Events</a></br>			
			<br />
			<br />
			<br />
			<br />


			<a name="NewObject"><h2 style="color:white">New Object Events</h2>	</a>
			<xsl:choose>
				<xsl:when test="Scenario/NewObject">
					<xsl:apply-templates select="Scenario/NewObject">
					<!-- Sort by Time, Object Type -->
						<xsl:sort data-type="number" select="Parameter[5]/Value"/>
						<xsl:sort data-type="text" select="Parameter[2]/Value"/>
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
			
			<a name="RevealObject"><h2 style="color:white">Reveal Object Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/RevealObject">
					<xsl:apply-templates select="Scenario/RevealObject">
							<!-- Sort by Time, ObjectName -->
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
		
			<a name="SubplatformDock"><h2 style="color:white">Subplatform Dock Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/SubplatformDock">
					<xsl:apply-templates select="Scenario/SubplatformDock">
					<!-- Sort by Time -->
						<xsl:sort data-type="number" select="Parameter[3]/Value"/>
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
			
			<a name="AttackObject"><h2 style="color:white">Attack Object Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/AttackObject">
					<xsl:apply-templates select="Scenario/AttackObject">
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
		
			<a name="SelfDefenseAttackStarted"><h2 style="color:white">Self Defense Attack Started Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/SelfDefenseAttackStarted">
					<xsl:apply-templates select="Scenario/SelfDefenseAttackStarted">
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
			
			<a name="MoveObject"><h2 style="color:white">Move Object Events</h2></a>
			<xsl:choose>
				<xsl:when test="Scenario/MoveObject">
					<xsl:apply-templates select="Scenario/MoveObject">
					<!-- Sort by Time, ID -->
						<xsl:sort data-type="number" select="Parameter[4]/Value"/>
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

			<a name="StateChange"><h2 style="color:white">State Change Events</h2></a>
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

			<a name="CreateChatroom">
				<h2 style="color:white">Create Chatroom Events</h2>
			</a>
			<xsl:choose>
				<xsl:when test="Scenario/CreateChatRoom">
					<xsl:apply-templates select="Scenario/CreateChatRoom">
						<!-- Sort by Time, ID -->
						<xsl:sort data-type="number" select="Parameter[3]/Value"/>
						<xsl:sort data-type="text" select="Parameter[1]/Value"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<table bgcolor="#FFFFFF" width="100%">
						<tr>
							<td>
								<b>No Records Found.</b>
							</td>
						</tr>
					</table>
				</xsl:otherwise>
			</xsl:choose>
			<br />
			<br />

			<a name="TextChatMessage">
				<h2 style="color:white">Text Chat Message Events</h2>
			</a>
			<xsl:choose>
				<xsl:when test="Scenario/TextChat">
					<xsl:apply-templates select="Scenario/TextChat">
						<!-- Sort by Time, ID -->
						<xsl:sort data-type="number" select="Parameter[3]/Value"/>
						<xsl:sort data-type="text" select="Parameter[1]/Value"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<table bgcolor="#FFFFFF" width="100%">
						<tr>
							<td>
								<b>No Records Found.</b>
							</td>
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


	
	<xsl:template match="RevealObject">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tr  bgcolor="#C4CCD5">
				<td width="25%">
					<b><xsl:text>Time : </xsl:text></b>
					<xsl:value-of select="Parameter[3]/Value"/>
				</td>
				<td width="75%">
					<b><xsl:text>ID : </xsl:text></b>
					<xsl:value-of select="Parameter[1]/Value"/>
				</td>
			</tr>
			<tr>
				<td colspan="100%">
					<table cellpadding="3">
						<tbody align="left">
							<tr>
								<th>Attributes</th>
							</tr>
							<xsl:apply-templates select="Parameter[2]/Value/AttributeCollectionType"/>
						</tbody>
					</table>
				</td>
			</tr>
		</table>
	</p>
	</xsl:template>




	<xsl:template match="NewObject">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tr bgcolor="#C4CCD5">
				<td width="25%">
					<b><xsl:text>Time : </xsl:text></b>
					<xsl:value-of select="Parameter[5]/Value"/>
				</td>
				<td width="75%">
					<b><xsl:text>ID : </xsl:text></b>
					<xsl:value-of select="Parameter[1]/Value"/>
				</td>
			</tr>
			<tr>	
				<td colspan="100%">
					<b><xsl:text>Type : </xsl:text></b>
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



	<xsl:template match="SubplatformLaunch">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%">
						<b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[4]/Value"/>
					</td>
					<td width="75%">
						<b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/>
					</td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Parent ID:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
						<div><b><xsl:text>Destination Location(x,y,z):</xsl:text></b><xsl:apply-templates select="Parameter[3]/Value/LocationType"/></div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>
	
	
	<xsl:template match="WeaponLaunch">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td with="25%"><b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[4]/Value"/></td>
					<td width="75%"><b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Parent ID:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
						<div><b><xsl:text>Target ID:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>


	<xsl:template match="SubplatformDock">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%"><b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></td>
					<td width="75%"><b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<b><xsl:text>Parent ID:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	<xsl:template match="AttackObject">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%"><b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[4]/Value"/></td>
					<td width="75%"><b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Target ID:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
						<div><b><xsl:text>Capability:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>
	
	
	<xsl:template match="SelfDefenseAttackStarted">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%">
						<b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/>
					</td>
					<td width="75%">
						<b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/>
					</td>
				</tr>
				<tr>
					<td>
						<b><xsl:text>Target ID:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/>
					</td>					
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>
	

	<xsl:template match="MoveObject">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%"><b><xsl:text>Time: </xsl:text></b><xsl:value-of select="Parameter[4]/Value"/></td>
					<td width="75%"><b><xsl:text>ID: </xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>Location (x,y,z): </xsl:text></b><xsl:apply-templates select="Parameter[2]/Value/LocationType"/></div>
						<div><b><xsl:text>Throttle: </xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>


	<xsl:template match="MoveDone">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%"><b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></td>
					<td width="75%"><b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></td>
				</tr>
				<tr>
					<td colspan="100%">
						<b><xsl:text>Reason:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	<xsl:template match="StateChange">
	<p>
		<table bgcolor="#FFFFFF" width="100%">
			<tbody>
				<tr bgcolor="#C4CCD5">
					<td width="25%">
						<div><b><xsl:text>Time:</xsl:text></b><xsl:value-of select="Parameter[3]/Value"/></div>
					</td>
					<td width="75%">
						<div><b><xsl:text>ID:</xsl:text></b><xsl:value-of select="Parameter[1]/Value"/></div>
					</td>
				</tr>
				<tr>
					<td colspan="100%">
						<div><b><xsl:text>New State:</xsl:text></b><xsl:value-of select="Parameter[2]/Value"/></div>
					</td>
				</tr>
			</tbody>
		</table>
	</p>
	</xsl:template>

	
	</xsl:stylesheet>




