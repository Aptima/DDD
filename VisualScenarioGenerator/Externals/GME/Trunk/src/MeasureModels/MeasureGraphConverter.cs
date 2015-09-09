using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AME.MeasureModels
{
    public class MeasureGraphConverter
    {
        private IXPathNavigable measureOutput;

        public IXPathNavigable MeasureOutput
        {
            get { return measureOutput; }
        }

        public MeasureGraphConverter(IXPathNavigable measureOutput)
        {
            this.measureOutput = measureOutput;
        }

        public IXPathNavigable Transform()
        {
            XmlDocument document = new XmlDocument();

            if (measureOutput == null)
                return null;

            XPathNavigator navigator = measureOutput.CreateNavigator();

            XmlElement measures = createMeasures(document);
            document.AppendChild(measures);

            XPathNodeIterator itMeasures = navigator.Select("/Component/Measures/Measure");
            while (itMeasures.MoveNext())
            {
                String graphType = itMeasures.Current.GetAttribute("graphType", itMeasures.Current.NamespaceURI);
                if (!graphType.Equals(String.Empty))
                {
                    XmlElement measure = createMeasure(document, measures, itMeasures.Current);
                    measures.AppendChild(measure);
                }
            }

            return document;
        }

        private XmlElement createMeasures(XmlDocument document)
        {
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            document.AppendChild(declaration);
            XmlElement component = document.CreateElement("Measures");

            return component;
        }

        private XmlElement createMeasure(XmlDocument document, XmlElement parent, XPathNavigator navigator)
        {
            XmlElement measure = document.CreateElement("Measure");
            XmlAttribute measureName = document.CreateAttribute("name");
            String displayName = navigator.GetAttribute("displayName", navigator.NamespaceURI);
            measureName.Value = displayName;
            measure.Attributes.Append(measureName);

            List<String[]> dataSets = new List<String[]>();
            List<String> dataNames = new List<String>();
            List<List<String>> valueLists = new List<List<String>>();

            XPathNodeIterator itParameters = navigator.Select("Parameter");
            while (itParameters.MoveNext())
            {
                String axis = itParameters.Current.GetAttribute("axis", itParameters.Current.NamespaceURI);
                String output = itParameters.Current.GetAttribute("output", itParameters.Current.NamespaceURI);
                
                switch (axis + output)
                {
                    case "x":
                        dataSets = createDataSets(itParameters.Current);
                        break;
                    
                    case "y":
                        dataNames = createData(itParameters.Current, false);
                        break;

                    case "xy":
                        dataSets = createDataSets(itParameters.Current);
                        dataNames = createData(itParameters.Current, false);
                        break;

                    case "ytrue":
                        valueLists = createValues(itParameters.Current);
                        dataNames = createData(itParameters.Current, true);
                        break;

                    case "true":
                        valueLists = createValues(itParameters.Current);
                        break;
                }
            }

            Int32 index = 0;
            foreach (String[] ds in dataSets)
            {
                XmlElement dataSet = document.CreateElement("DataSet");
                XmlElement data1 = document.CreateElement("Data");
                XmlAttribute data1Name = document.CreateAttribute("name");
                data1Name.Value = ds[0];
                data1.Attributes.Append(data1Name);
                XmlAttribute data1Value = document.CreateAttribute("value");
                data1Value.Value = ds[1];
                data1.Attributes.Append(data1Value);
                dataSet.AppendChild(data1);

                for (Int32 i = 0; i < valueLists[index].Count; i++ )
                {
                    String dn = dataNames[i];
                    XmlElement data = document.CreateElement("Data");
                    XmlAttribute dataName = document.CreateAttribute("name");
                    dataName.Value = dn;
                    data.Attributes.Append(dataName);
                    XmlAttribute dataValue = document.CreateAttribute("value");
                    dataValue.Value = valueLists[index][i];
                    data.Attributes.Append(dataValue);
                    dataSet.AppendChild(data);
                }
            
                measure.AppendChild(dataSet);
                index++;
            }

            return measure;
        }

        private List<String[]> createDataSets(XPathNavigator navigator)
        {
            List<String[]> dataSets = new List<String[]>();
            String parameterName = navigator.GetAttribute("name", navigator.NamespaceURI);
            XPathNodeIterator itDataSets = navigator.Select("DataSet");
            while (itDataSets.MoveNext())
            {
                XPathNavigator dataValue = itDataSets.Current.SelectSingleNode("Data/Value");
                if (dataValue != null)
                {
                    String[] pair = new String[2];
                    pair[0] = parameterName;
                    pair[1] = dataValue.Value;
                    dataSets.Add(pair);
                }
            }
            return dataSets;
        }

        private List<String> createData(XPathNavigator navigator, Boolean output)
        {            
            List<String> data = new List<String>();
            String parameterName = navigator.GetAttribute("name", navigator.NamespaceURI);
            XPathNodeIterator itDataSets = navigator.Select("DataSet");
            while (itDataSets.MoveNext())
            {
                XPathNavigator dataValue = itDataSets.Current.SelectSingleNode("Data/Value");
                if (dataValue != null)
                {
                    if (output)
                        data.Add(parameterName);
                    else
                        data.Add(dataValue.Value);
                }
            }
            return data;
        }

        private List<List<String>> createValues(XPathNavigator navigator)
        {
            List<List<String>> valueLists = new List<List<String>>();
            XPathNodeIterator itDataSets = navigator.Select("DataSet");
            while (itDataSets.MoveNext())
            {
                List<String> values = new List<String>();
                XPathNodeIterator itData = itDataSets.Current.Select("Data");
                while (itData.MoveNext())
                {
                    XPathNavigator dataValue = itData.Current.SelectSingleNode("Value");
                    if (dataValue != null)
                    {
                        values.Add(dataValue.Value);
                    }
                }
                valueLists.Add(values);
            }
            return valueLists;
        }
    }
}