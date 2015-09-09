import string
events = {
        "Scenario":{},
        "CreateEvent":{},
        "RevealEvent":{},
        "MoveEvent":{},
        "LaunchEvent":{},
        "WeaponLaunchEvent":{},
        "TransferEvent":{},
        "StateChangeEvent":{},
        "CompletionEvent":{},
        "ReiterateEvent":{},
        "OpenChatRoomEvent":{},
        "CloseChatRoomEvent":{},
		"SendChatMessageEvent":{},
		"SendVoiceMessageEvent":{},
		"SendVoiceMessageToUserEvent":{},
		"OpenVoiceChannelEvent":{},
		"CloseVoiceChannelEvent":{},
        "ChangeEngramEvent":{},
        "RemoveEngramEvent":{},
        "SpeciesCompletionEvent":{},
        "FlushEvent":{},       
    }

events["Scenario"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>']
events["Scenario"]["children"] = ['OpenChatRoomEvent',
                                  'CloseChatRoomEvent',
								  'SendChatMessageEvent',
								  'OpenVoiceChannelEvent',
								  'CloseVoiceChannelEvent',
								  'SendVoiceMessageEvent',
								  'SendVoiceMessageToUserEvent',
                                  'RemoveEngramEvent',
                                  'ChangeEngramEvent',
                                  'ReiterateEvent',
                                  'SpeciesCompletionEvent',
                                  'FlushEvent']


events["CreateEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>']
events["CreateEvent"]["children"] = ['RevealEvent',
                                  'MoveEvent',
                                  'CompletionEvent',
                                  'LaunchEvent',
                                  'WeaponLaunchEvent',
                                  'TransferEvent',
                                  'StateChangeEvent']


events["RevealEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["RevealEvent"]["children"] = []


events["MoveEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["MoveEvent"]["children"] = []


events["LaunchEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["LaunchEvent"]["children"] = []

events["WeaponLaunchEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["WeaponLaunchEvent"]["children"] = []


events["TransferEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["TransferEvent"]["children"] = []


events["StateChangeEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["StateChangeEvent"]["children"] = []


events["CompletionEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["CompletionEvent"]["children"] = ['RevealEvent',
                                         'MoveEvent',
                                         'ReiterateEvent',
                                         'CompletionEvent',
                                         'LaunchEvent',
                                         'WeaponLaunchEvent',
                                         'TransferEvent',
                                         'StateChangeEvent',
                                         'ChangeEngramEvent',
                                         'RemoveEngramEvent',
                                         'FlushEvent',
                                         'SpeciesCompletionEvent']


events["ReiterateEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["ReiterateEvent"]["children"] = ['MoveEvent']


events["OpenChatRoomEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["OpenChatRoomEvent"]["children"] = []


events["CloseChatRoomEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["CloseChatRoomEvent"]["children"] = []

events["SendChatMessageEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["SendChatMessageEvent"]["children"] = []

events["OpenVoiceChannelEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["OpenVoiceChannelEvent"]["children"] = []


events["CloseVoiceChannelEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["CloseVoiceChannelEvent"]["children"] = []

events["SendVoiceMessageEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["SendVoiceMessageEvent"]["children"] = []

events["SendVoiceMessageToUserEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["SendVoiceMessageToUserEvent"]["children"] = []

events["ChangeEngramEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["ChangeEngramEvent"]["children"] = []


events["RemoveEngramEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["RemoveEngramEvent"]["children"] = []


events["SpeciesCompletionEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["SpeciesCompletionEvent"]["children"] = ['RevealEvent',
                                         'MoveEvent',
                                         'ReiterateEvent',
                                         'CompletionEvent',
                                         'LaunchEvent',
                                         'WeaponLaunchEvent',
                                         'TransferEvent',
                                         'StateChangeEvent',
                                         'ChangeEngramEvent',
                                         'RemoveEngramEvent',
                                         'FlushEvent',
                                         'SpeciesCompletionEvent']


events["FlushEvent"]["functions"] = [
    '<Function Name="VisualRepresentation" Action="Image@Image.Image File" Visible="false"/>',
    '<Function Name="Drag" Action="ApplicationNodes.EventTreeNode" Visible="false"/>'
    '<Function Name="Move Up" Action="moveUp" Visible="true"/>',
	'<Function Name="Move Down" Action="moveDown" Visible="true"/>',
	'<Function Name="Delete" Action="deleteEvent" Visible="true"/>']
events["FlushEvent"]["children"] = []

for eventName in events:
    events[eventName]['parents'] = []
    events[eventName]['siblings'] = {}
    
for eventName in events:
    for childName in events[eventName]['children']:
        if not eventName in events[childName]['parents']:
            events[childName]['parents'].append(eventName)
for eventName in events:
    for parentName in events[eventName]['parents']:
        events[eventName]['siblings'][parentName] = events[parentName]['children']
        

def printEventXSL(name,parentName):
    if parentName == None:
        print '<xsl:template match="Component[@Type=\'%s\']">'%name
    else:
        print '<xsl:template match="Component[@Type=\'%s\' and ../@Type=\'%s\']">'%(name,parentName)
    print '''<Component>
<xsl:attribute name="ID"><xsl:value-of select="@ID"/></xsl:attribute>
<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute>
<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
<xsl:attribute name="Description"><xsl:value-of select="@Description"/></xsl:attribute>
<xsl:attribute name="eType"><xsl:value-of select="@eType"/></xsl:attribute>
<xsl:attribute name="LinkID"><xsl:value-of select="@LinkID"/></xsl:attribute>
<ComponentParameters>
<Parameter category="Image" type="Complex">
<Parameter displayedName="Image File" propertyName="IMF" type="System.String" value="Folder.ico" category="Image" description="The image for this component" browsable="true" editor="FileNameEditor"/>
</Parameter>
</ComponentParameters>
<Functions>'''
            

    
    for func in events[name]['functions']:
        print func
    if len(events[name]['functions']) > 2:
        print '<Function Name="-----------" Action="" Visible="true"/>'
    
    if parentName != None:
        for childType in events[name]['siblings'][parentName]:
            print '<Function Name="Create %s After" Action="createSibling(%s)" Visible="true"/>'%(childType,childType)
            
    if len(events[name]['siblings']) > 0 and len(events[name]['children']) > 0:
        print '<Function Name="-----------" Action="" Visible="true"/>'
    for childType in events[name]['children']:
        print '<Function Name="Create %s Child" Action="createChild(%s)" Visible="true"/>'%(childType,childType)
    print '</Functions>'   

    if events[name]['children']:
        print '<xsl:apply-templates select="Component[%s]"></xsl:apply-templates>' % string.join(['@Type=\'%s\''%child for child in events[name]['children']],' or ')
    #if events[name]['children']:
    #    print [].
    
    print '''</Component>
</xsl:template>'''
def printXSL():
    print '<?xml version="1.0" encoding="UTF-8"?>'
    print '<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">'
    print '''<xsl:template match="/">
<Component>
<xsl:apply-templates select="//Component[@Type='CreateEvent']"/>
<xsl:apply-templates select="//Component[@Type='Scenario']"/>
</Component>
</xsl:template>'''
    for eventName in events:
        if events[eventName]['parents']:
            for parentName in events[eventName]['parents']:
                printEventXSL(eventName,parentName)
        else:
            printEventXSL(eventName,None)
            
    print '</xsl:stylesheet>'


printXSL()