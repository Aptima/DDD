'''
Created on May 20, 2009

@author: mtherrien
'''
from lxml import etree
import string
import pprint

ppp = pprint.pprint
def CamelCase(s):
    newS = s[0];
    for i in range(1, len(s) - 1):
        if s[i].islower() and s[i + 1].isupper():
            newS += s[i] + "_" 
        else:
            newS += s[i]
    newS += s[-1]
    return string.upper(newS)
        
        
if __name__ == '__main__':
    doc = etree.parse("../events.xml", etree.XMLParser())
    events = etree.XPath('//EventType')(doc)
    values = set();
    s = ""#'<xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://aptima.com/afar" elementFormDefault="qualified">    <xsd:import namespace="http://aptima.com/afar" schemaLocation="values.xsd"/>'
    for event in events:
        
        eventType = etree.XPath('@Name')(event)[0]
        
        params = etree.XPath('./Parameter')(event)
        s += str.format('{0}("{1}"), \n', CamelCase(eventType), eventType)
        
        for param in params:
            value = param.get("DataType")
            values.add(value)
            name = param.get("Name")
            #s +=  str.format('<xsd:element ref="Parameter"/>')
            #s +=  str.format('<xsd:element name="{0}"><xsd:complexType><xsd:sequence>', "Parameter")
            #s +=  str.format('<xsd:element name="{0}" default="{1}"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:pattern value="{1}"/></xsd:restriction>', "Name", name)                       
            #s +=  str.format('</xsd:simpleType></xsd:element>')
            #s +=  str.format('<xsd:element name="{0}" type="{1}">', "Value", value)  
            #s +=  str.format('<xsd:element name="{0}"/>', value)  
            #s +=  str.format('</xsd:element>')
            #s +=  str.format('</xsd:sequence></xsd:complexType></xsd:element >')
        #s +=  str.format('</xsd:sequence></xsd:complexType >\n')
    #s += '</xsd:schema>'
    #print s


    #    f = open(eventType+".xsd", "w" )
    #    f.write(s)
    #    f.close()
#    for value in values:
#        s += str.format('    <xs:element ref="{0}"/> \n', value)
    print s        
#    
#   for value in values:
#        print str.format('    <xs:element name="{0}"/> ', value)
         
# ppp(events)
        
