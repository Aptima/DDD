<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:template match="/">
		<Component>
			<xsl:apply-templates select="//Component[@Type='Scenario']"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Scenario']">
		<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
		<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
		<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
		<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
		<xsl:apply-templates select="Functions"/>
		<!--<xsl:apply-templates select="ComponentParameters"/>-->
		<ComponentParameters>
			<Parameter category="Image" type="Complex">
				<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
			</Parameter>
		</ComponentParameters>
		<Functions>
			<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
		</Functions>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Engram</xsl:attribute>
			<xsl:attribute name="Name">Engrams</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Engram" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Engram']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<!--
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Emitter</xsl:attribute>
			<xsl:attribute name="Name">Emitters</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
            <ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Emitter" Action="CreateComponent" Visible="true"/>
                <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Emitter']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
-->
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Sensor</xsl:attribute>
			<xsl:attribute name="Name">Sensors</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Sensor" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Sensor']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
		<Component>
			<xsl:attribute name="ID">-1</xsl:attribute>
			<xsl:attribute name="Type">Species</xsl:attribute>
			<xsl:attribute name="Name">Species Definition</xsl:attribute>
			<xsl:attribute name="eType">None</xsl:attribute>
			<ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
			</ComponentParameters>
			<Functions>
				<Function Name="Create Species" Action="CreateComponent" Visible="true"/>
				<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
			</Functions>
			<xsl:apply-templates select="Component[@Type='Species']">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Engram']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Emitter']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Level</xsl:attribute>
				<xsl:attribute name="Name">Levels</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Level" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Level']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Sensor']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">SensorRange</xsl:attribute>
				<xsl:attribute name="Name">Sensor Ranges</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Sensor Range" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='SensorRange']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Proximity']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Effect</xsl:attribute>
				<xsl:attribute name="Name">Effects</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Effect" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Effect']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Singleton']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Transition</xsl:attribute>
				<xsl:attribute name="Name">Transitions</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Transition" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Transition']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Transition']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Effect']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Capability']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Proximity</xsl:attribute>
				<xsl:attribute name="Name">Proximities</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Proximity" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Proximity']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Combo']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Contribution</xsl:attribute>
				<xsl:attribute name="Name">Contributions</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Contribution" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Contribution']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Contribution']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Level']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='SensorRange']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='State']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Emitter</xsl:attribute>
				<xsl:attribute name="Name">Emitters</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Emitter" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Emitter']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Capability</xsl:attribute>
				<xsl:attribute name="Name">Capabilities</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Capability" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Capability']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Singleton</xsl:attribute>
				<xsl:attribute name="Name">Singleton Vulnerability</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Singleton Vulnerability" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Singleton']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Combo</xsl:attribute>
				<xsl:attribute name="Name">Combo Vulnerability</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create Combo Vulnerability" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Combo']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
		</Component>
	</xsl:template>
	<xsl:template match="Component[@Type='Species']">
		<Component>
			<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
			<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
			<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
			<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
			<xsl:apply-templates select="Functions"/>
			<xsl:apply-templates select="ComponentParameters"/>
			<!-- create dummy nodes to hang from species-->
			<!--
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Capability</xsl:attribute>
				<xsl:attribute name="Name">Capabilities</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
                <ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
                </ComponentParameters>
				<Functions>
					<Function Name="Create Capability" Action="CreateComponent" Visible="true"/>
                    <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Capability']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>			
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Combo</xsl:attribute>
				<xsl:attribute name="Name">Combo Vulnerability</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
                <ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
                </ComponentParameters>
				<Functions>
					<Function Name="Create Combo Vulnerability" Action="CreateComponent" Visible="true"/>
                    <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Combo']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">Singleton</xsl:attribute>
				<xsl:attribute name="Name">Singleton Vulnerability</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
                <ComponentParameters>
				<Parameter category="Image" type="Complex">
					<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
				</Parameter>
                </ComponentParameters>
				<Functions>
					<Function Name="Create Singleton Vulnerability" Action="CreateComponent" Visible="true"/>
                    <Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='Singleton']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
            -->
			<Component>
				<xsl:attribute name="ID">-1</xsl:attribute>
				<xsl:attribute name="Type">State</xsl:attribute>
				<xsl:attribute name="Name">States</xsl:attribute>
				<xsl:attribute name="eType">None</xsl:attribute>
				<ComponentParameters>
					<Parameter category="Image" type="Complex">
						<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
					</Parameter>
				</ComponentParameters>
				<Functions>
					<Function Name="Create State" Action="CreateComponent" Visible="true"/>
					<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>
				</Functions>
				<xsl:apply-templates select="Component[@Type='State']">
					<xsl:sort select="@Name"/>
				</xsl:apply-templates>
			</Component>
		</Component>
	</xsl:template>
	<xsl:template match="ComponentParameters">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="Functions">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
