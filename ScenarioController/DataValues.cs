using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
namespace DDD.CommonComponents.DataTypeTools
{
    interface DataValueInterface
    {
        string ToXML();
        void FromXML(string xml);
    }
    public class DataValue : DataValueInterface
    {
        public string xmlStartTag = null;
        public string xmlEndTag = null;
        public string dataType=null;
        public string ToXML()
        {
            return "";
        }
        public void FromXML(string xml)
        {

        }
        public string getType()
        {
            return this.dataType;
        }
    }
    
    public class StringValue : DataValue
    {
        static Regex regex = new Regex(@"^<StringType>(.*)</StringType>$");
        public string value;
        public StringValue()
        {
            dataType = "StringType";
            value = null;
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";
        }
        new public string ToXML()
        {
            string s = xmlStartTag + value.ToString() + xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                value = g.Value;
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
        }
    }
    public class DoubleValue : DataValue
    {
        
        static Regex regex = new Regex(@"^<DoubleType>(.*)</DoubleType>$");
        public double? value;
        public DoubleValue()
        {

            dataType = "DoubleType";
            value = null;
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";
        }
        new public string ToXML()
        {
            string s = xmlStartTag + value.ToString() + xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                double i;
                if (double.TryParse(g.Value, out i))
                {
                    value = i;
                }
                else
                {
                    value = null;
                }
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
        }
    }
    public class IntegerValue : DataValue
    {
        static Regex regex = new Regex(@"^<IntegerType>(.*)</IntegerType>$");
        public int? value;
        public IntegerValue()
        {
            dataType = "IntegerType";
            value = null;
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";
        }
        new public string ToXML()
        {
            string s = xmlStartTag + value.ToString() + xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                int i;
                //value = Convert.ToInt32(g.Value);
                if (Int32.TryParse(g.Value, out i))
                {
                    value = i;
                }
                else
                {
                    value = null;
                }
                
                
           
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
        }
    }


    public class LocationValue : DataValue
    {
        static Regex regex = new Regex(@"^<LocationType><X>(.*)</X><Y>(.*)</Y><Z>(.*)</Z></LocationType>$");
        public double? X, Y, Z;
        public LocationValue()
        {
            dataType = "LocationType";
            X = null;
            Y = null;
            Z = null;
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";
        }
        new public string ToXML()
        {
            string s = xmlStartTag;
            s += "<X>" + X.ToString();
            s += "</X><Y>" + Y.ToString(); 
            s += "</Y><Z>" + Z.ToString() + "</Z>";
            s += xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                double i;

                if (double.TryParse(m.Groups[1].Value, out i))
                {
                    X = i;
                }
                else
                {
                    X = null;
                }
                if (double.TryParse(m.Groups[2].Value, out i))
                {
                    Y = i;
                }
                else
                {
                    Y = null;
                }
                if (double.TryParse(m.Groups[3].Value, out i))
                {
                    Z = i;
                }
                else
                {
                    Z = null;
                }
                //X = Convert.ToDouble(m.Groups[1].Value);
                //Y = Convert.ToDouble(m.Groups[2].Value);
                //Z = Convert.ToDouble(m.Groups[3].Value);
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
        }
    }
    public class VelocityValue : DataValue
    {
        static Regex regex = new Regex(@"^<VelocityType><VX>(.*)</VX><VY>(.*)</VY><VZ>(.*)</VZ></VelocityType>$");
        public double? VX, VY, VZ;
        public VelocityValue()
        {
            dataType = "VelocityType";
            VX = null;
            VY = null;
            VZ = null;
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";
        }

        new public string ToXML()
        {

            string s = xmlStartTag;
            s += "<VX>" + VX.ToString();
            s += "</VX><VY>" + VY.ToString();
            s += "</VY><VZ>" + VZ.ToString() + "</VZ>";
            s += xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {

                double i;

                if (double.TryParse(m.Groups[1].Value, out i))
                {
                    VX = i;
                }
                else
                {
                    VX = null;
                }
                if (double.TryParse(m.Groups[2].Value, out i))
                {
                    VY = i;
                }
                else
                {
                    VY = null;
                }
                if (double.TryParse(m.Groups[3].Value, out i))
                {
                    VZ = i;
                }
                else
                {
                    VZ = null;
                }
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
        }
    }
    public class AttributeCollectionValue : DataValue
    {
        static Regex regex = new Regex(@"^<AttributeCollectionType>(<Attribute>.*</Attribute>)*</AttributeCollectionType>$");
        static Regex attregex = new Regex(@"<Attribute><Name>(.*?)</Name><Value>(.*?)</Value></Attribute>");
        public Dictionary<string, DataValue> attributes;
        public AttributeCollectionValue()
        {
            dataType = "AttributeCollectionType";
            attributes = new Dictionary<string, DataValue>();
            xmlStartTag = "<" + dataType + ">";
            xmlEndTag = "</" + dataType + ">";

        }
        public DataValue this[string attName]
        {
            get
            {
                return attributes[attName];
            }
            set
            {
                attributes[attName] = value;
            }
        }
        new public string ToXML()
        {
            string s = xmlStartTag;
            foreach (string key in attributes.Keys)
            {
                s += "<Attribute>";

                s += "<Name>" + key + "</Name>";
                s += "<Value>" + DataValueFactory.XMLSerialize(attributes[key]) + "</Value>";

                s += "</Attribute>";
            }
            s += xmlEndTag;
            return s;
        }
        new public void FromXML(string xml)
        {
            Match m = regex.Match(xml);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in attregex.Matches(s))
                {
                    string name = m2.Groups[1].Value;
                    string val = m2.Groups[2].Value;
                    attributes[name] = DataValueFactory.XMLDeserialize(val);
                }
            }
            else
            {
                throw new Exception("Invalid XML string");
            }
            
        }
    }

    public class DataValueFactory
    {
        //Dictionary<string, DataValue> dataTypes;
        static Regex typeregex = new Regex(@"^<(\w+)>");
        public DataValueFactory()
        {
            //typeregex = new Regex(@"^<(\w+)>.*$");
            //typeregex = new Regex(@"^<(\w+)>");
            //dataTypes = new Dictionary<string,DataValue>();
            //dataTypes["StringType"] = new StringValue();
            //dataTypes["IntegerType"] = new IntegerValue();
            //dataTypes["DoubleType"] = new DoubleValue();
            //dataTypes["LocationType"] = new LocationValue();
            //dataTypes["VelocityType"] = new VelocityValue();
            //dataTypes["AttributeCollectionType"] = new AttributeCollectionValue();

        }
        public static DataValue BuildValue(string dataType)
        {
            switch (dataType)
            {
                case "StringType":
                    return new StringValue();
                case "DoubleType":
                    return new DoubleValue();
                case "IntegerType":
                    return new IntegerValue();
                case "LocationType":
                    return new LocationValue();
                case "VelocityType":
                    return new VelocityValue();
                case "AttributeCollectionType":
                    return new AttributeCollectionValue();
                default:
                    return null;
            }
            //return dataTypes[dataType];

        }
        public static string XMLSerialize(DataValue v)
        {
            if (v == null)
            {
                return "";
            }
            switch (v.dataType)
            {
                case "StringType":
                    return ((StringValue)v).ToXML();
                case "DoubleType":
                    return ((DoubleValue)v).ToXML();
                case "IntegerType":
                    return ((IntegerValue)v).ToXML();
                case "LocationType":
                    return ((LocationValue)v).ToXML();
                case "VelocityType":
                    return ((VelocityValue)v).ToXML();
                case "AttributeCollectionType":
                    return ((AttributeCollectionValue)v).ToXML();
                default:
                    return "";
            }
        }
        public static DataValue XMLDeserialize(string xml)
        {
            Match m = typeregex.Match(xml);
            if (m.Success)
            {
                Group g = m.Groups[1];
                
                string dataType = g.ToString();
                switch (dataType)
                {
                    case "StringType":
                        StringValue sv = new StringValue();
                        sv.FromXML(xml);
                        return sv;
                    case "DoubleType":
                        DoubleValue dv = new DoubleValue();
                        dv.FromXML(xml);
                        return dv;
                    case "IntegerType":
                        IntegerValue iv = new IntegerValue();
                        iv.FromXML(xml);
                        return iv;
                    case "LocationType":
                        LocationValue lv = new LocationValue();
                        lv.FromXML(xml);
                        return lv;
                    case "VelocityType":
                        VelocityValue vv = new VelocityValue();
                        vv.FromXML(xml);
                        return vv;
                    case "AttributeCollectionType":
                        AttributeCollectionValue av = new AttributeCollectionValue();
                        av.FromXML(xml);
                        return av;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
    
}
