<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<body bgcolor="#566578">
				<h2 style="color:white">Scenario Logfile Summary</h2>
				<div style="background-color:white">
					<b>Filename: </b><xsl:value-of select="Scenario/@name"/><br/>
					<b>Map filename: </b><xsl:value-of select="Scenario/Playfield/Parameter[1]/Value"/><br/>
					<b>Icon library: </b><xsl:value-of select="Scenario/Playfield/Parameter[2]/Value"/><br/>
					<b>Scenario name:</b> <xsl:value-of select="Scenario/Playfield/Parameter[8]/Value"/><br/>
				</div>
				<br/>
				<div style="background-color:white">
					<b>Scenario Description:</b>
					<br/>
					<br/>
					<xsl:value-of select="Scenario/Playfield/Parameter[9]/Value"/>
					<br/>
				</div>
				<br/>
				<h2 style="color:white"> Event Counts</h2>
				<div style="background-color:white">
					<table>
						<tr>
							<td style="font-weight:bold">New Object Events: </td>
							<td><xsl:value-of select="count(Scenario/NewObject)"/></td>
						</tr>
						<tr>
							<td style="font-weight:bold">Reveal Object Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/RevealObject)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Subplatform Launch Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/SubplatformLaunch)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Subplatform Dock Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/SubplatformDock)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Weapon Launch Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/WeaponLaunch)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Self Defense Attack Started Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/SelfDefenseAttackStarted)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Move Object Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/MoveObject)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Move Done Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/MoveDone)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">State Change Events: </td>
							<td><xsl:value-of select="count(Scenario/StateChange)"/></td>
						</tr>
						<tr>
							<td style="font-weight:bold">Create Chatroom Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/CreateChatRoom)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Text Chat Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/TextChat)"/>
							</td>
						</tr>
						<tr>
							<td style="font-weight:bold">Attacked Object Events: </td>
							<td>
								<xsl:value-of select="count(Scenario/History_AttackedObjectReport)"/>
							</td>
						</tr>
            <tr>
              <td style="font-weight:bold">Attacker Object Events: </td>
              <td>
                <xsl:value-of select="count(Scenario/History_AttackerObjectReport)"/>
              </td>
            </tr>
					</table>
				</div>
				<br />
				<br />


			</body>
		</html>
	</xsl:template>
	
	</xsl:stylesheet>




