'''
Created on May 20, 2009

@author: mtherrien
'''
from lxml import etree
import os
import pprint
import string

ppp = pprint.pprint
def CamelCase(s):
    newS = s[0];
    for i in range(1, len(s) -1):
        if s[i].islower() and s[i + 1].isupper():
            newS +=  s[i] + "_" 
        else:
            newS += s[i]
    newS += s[-1]
    return string.upper(newS)
        
        
if __name__ == '__main__':
    doc = etree.parse("../events.xml", etree.XMLParser())
    events = etree.XPath('//EventType')(doc)
    values = set();
    s = ""
    for event in events:
        
        eventType = etree.XPath('@Name')(event)[0]
        
        params = etree.XPath('./Parameter')(event)
        s = str.format('public static SimulationEvent create{1}( ', CamelCase(eventType), eventType)
        for param in params:
            value = param.get("DataType");
            file = os.path.abspath(str.format("/Documents and Settings/mtherrien/Dev/pheonix/DDDJavaNetworkTools/src/com/aptima/ddd/networktools/jxab/{0}.java", value))
            #print file
            if not os.path.exists(file) :
                value = value.replace("Type", "") 
            # exception Integer -> BigInteger
            if value in "Integer":
                value = "BigInteger"
            s+= value +" "+param.get("Name") + ", "
        if len(params) > 0:
            s=s[0:-2]
            
        s+=')\n{\n'
        s+=str.format("  SimulationEvent e = new SimulationEvent({0});",CamelCase(eventType) )
        if len(params) > 0:
            s+="   Value v = new Value();\n"
        for param in params:
             
            value = param.get("DataType")
            values.add(value)
            name = param.get("Name")
            s+=str.format('  v = new Value();\n   v.set{0}({1});\n', value , name )
            s+=str.format('  e.addParameter("{0}", v);\n', name )
        s += '   return e;\n}\n'
        print s        
        
