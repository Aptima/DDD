using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace AME.MeasureModels
{

    public abstract class MeasureModel : IMeasureModel
    {

        protected IXPathNavigable _measureInputXml;
        protected IXPathNavigable _measureOutputXml;
        protected string _sDisplayName;
        protected string _sGraphType;

        #region Properties

        #region Public properties

        public IXPathNavigable MeasureInputXml
        {
            get { return this._measureInputXml; }
            set { this._measureInputXml = value; }
        }

        public IXPathNavigable MeasureOutputXml
        {
            get { return this._measureOutputXml; }
        }

        public string DisplayName
        {
            get { return this._sDisplayName; }
            set { this._sDisplayName = value; }
        }

        public string GraphType
        {
            get { return this._sGraphType; }
            set { this._sGraphType = value; }
        }


        #endregion  //public properties

        #endregion  //properties

        #region Methods

        #region Private methods

        private void _GetDataCount(XPathNavigator nav, out int dataSetCount, out int dataCount)
        {
            dataSetCount = 0;
            dataCount = 0;

            XPathNodeIterator dataSetIterator = nav.Select("DataSet");
            dataSetCount = dataSetIterator.Count;

            foreach (XPathNavigator dataSet in dataSetIterator)
            {
                XPathNodeIterator dataIterator = dataSet.Select("Data");
                dataCount = dataIterator.Count;

                break;
            }//foreach dataset
        }//_GetDataCount


        #endregion  //private methods

        #region Inherited methods

        //currently supports upto 2 Dimensions, and types int & double
        protected object GetVariable(string varName)
        {
            object oVar = null;
            string sVarType;
            Type tVarType = null;
            int iDataSetCount = 0;
            int iDataCount = 0;
            int iDimensions = -1;
            string[,] sVariableValues = null;

            XPathNavigator navMeasureInput = this._measureInputXml.CreateNavigator();

            XPathNodeIterator variableIterator =
                navMeasureInput.Select(
                string.Format("Parameter[@name='{0}']", varName)
                );
            foreach (XPathNavigator variable in variableIterator)
            {
                XPathNavigator variableClone = variable.Clone();
                this._GetDataCount(variableClone, out iDataSetCount, out iDataCount);
                if (iDataSetCount == 0) //no data
                    break;

                sVarType = variable.GetAttribute("type", navMeasureInput.NamespaceURI);
                tVarType = Type.GetType(sVarType, true);

                iDimensions = int.Parse(variable.GetAttribute("dimensions", navMeasureInput.NamespaceURI));

                int i = 0;
                int j = 0;
                XPathNodeIterator dataSetIterator;
                dataSetIterator = variable.Select("DataSet");

                if (iDimensions == 1)
                {
                    sVariableValues = new string[iDataCount, iDataSetCount];
                    foreach (XPathNavigator dataSet in dataSetIterator)
                    {
                        XPathNodeIterator valueIterator = dataSet.Select("Data/Value");
                        foreach (XPathNavigator val in valueIterator)
                        {
                            sVariableValues[i, j] = val.Value;
                        }//foreach data
                        //i++;
                        //j = 0;
                        j++;
                    }//foreach dataSet
                }//1D
                else
                {
                    sVariableValues = new string[iDataSetCount, iDataCount];
                    foreach (XPathNavigator dataSet in dataSetIterator)
                    {
                        XPathNodeIterator valueIterator = dataSet.Select("Data/Value");
                        foreach (XPathNavigator val in valueIterator)
                        {
                            sVariableValues[i, j] = val.Value;
                            j++;
                        }//foreach data
                        i++;
                        j = 0;
                    }//foreach dataSet
                }//not 1D

                break;  //as there should be only 1 parameter by a name.
            }//foreach variable

            if (iDimensions == 0)
            {
                oVar = Convert.ChangeType(sVariableValues[0, 0], tVarType);
            }//0D
            else
            {
                Type tVarTypeToCreate = tVarType;
                if (
                    tVarType == typeof(int)
                    || (tVarType == typeof(double))
                    )
                {
                    tVarTypeToCreate = typeof(double);
                }

                if (iDimensions == 1)
                {
                    Array dDatas = Array.CreateInstance(tVarTypeToCreate, iDataSetCount);
                    for (int i = 0; i < iDataSetCount; i++)
                    {
                        dDatas.SetValue(Convert.ChangeType(sVariableValues[0, i], tVarTypeToCreate), i);
                    }
                    oVar = dDatas;
                }//1D
                else if (iDimensions == 2)
                {
                    Array dDatas = Array.CreateInstance(tVarTypeToCreate, iDataSetCount, iDataCount);
                    for (int i = 0; i < iDataSetCount; i++)
                    {
                        for (int j = 0; j < iDataCount; j++)
                        {
                            dDatas.SetValue(Convert.ChangeType(sVariableValues[i, j], tVarTypeToCreate), i, j);
                        }
                    }
                    oVar = dDatas;
                }//2D
            }//not 0D

            return oVar;
        }//GetVariable

        protected XmlDocument CreateMeasureOutputXml(out XmlElement measure)
        {
            string measureName = this.GetType().Name;

            XmlDocument outXmlDoc = new XmlDocument();
            XmlDeclaration declaration = outXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            outXmlDoc.AppendChild(declaration);

            XmlElement xeMeasure = outXmlDoc.CreateElement("Measure");
            outXmlDoc.AppendChild(xeMeasure);

            XmlAttribute xaMeasure_name = outXmlDoc.CreateAttribute("name");
            xaMeasure_name.Value = measureName;
            xeMeasure.Attributes.Append(xaMeasure_name);
            
            XmlAttribute xaMeasure_displayName = outXmlDoc.CreateAttribute("displayName");
            xaMeasure_displayName.Value = this.DisplayName;
            xeMeasure.Attributes.Append(xaMeasure_displayName);

            if ((this.GraphType != null) && (this.GraphType != string.Empty))
            {
                XmlAttribute xaMeasure_graphType = outXmlDoc.CreateAttribute("graphType");
                xaMeasure_graphType.Value = this.GraphType;
                xeMeasure.Attributes.Append(xaMeasure_graphType);
            }
            
            measure = xeMeasure;
            return outXmlDoc;
        }//CreateMeasureOutputXml

        protected XmlElement CreateOutputParameter(XmlDocument doc, string name, Type type, int dimensions, string axis, bool isOutput, Array paramArray)
        {
            string output = string.Empty;
            output = ConvertToString(paramArray);

            return this.CreateOutputParameter(doc, name, type, dimensions, axis, isOutput, output);
        }//CreateOutputParameter

        private string ConvertToString(Array paramArray)
        {
            string output = string.Empty;

            if (paramArray.Rank == 1)
            {
                output = this.ConvertToString_1D(paramArray);
            }//1D
            else if (paramArray.Rank == 2)
            {
                output = this.ConvertToString_2D(paramArray);
            }//2D

            return output;
        }

        private string ConvertToString_2D(Array paramArray)
        {
            string output = string.Empty;
            int iRowCount = paramArray.GetLength(0);
            int iColCount = paramArray.GetLength(1);

            output += "[";
            for (int i = 0; i < iRowCount; i++)
            {
                output += "[";
                for (int j = 0; j < iColCount; j++)
                {
                    output += paramArray.GetValue(i, j).ToString();
                    if (j != iColCount - 1)
                    {
                        output += ",";
                    }
                }
                output += "]";
                if (i != iRowCount - 1)
                {
                    output += "\n";
                }
            }
            output += "]";

            return output;
        }

        private string ConvertToString_1D(Array paramArray)
        {
            string output = string.Empty;
            output += "[";
            for (int i = 0; i < paramArray.Length; i++)
            {
                output += paramArray.GetValue(i).ToString();
                if (i != paramArray.Length - 1)
                {
                    output += ",";
                }
            }
            output += "]";

            return output;
        }

        protected XmlElement CreateOutputParameter(XmlDocument doc, string name, Type type, int dimensions, string axis, bool isOutput, string output)
        {
            XmlElement xeParam = this.CreateOutputParameter(doc, name, type, dimensions, axis, isOutput);

            List<XmlElement> xeOutputDataSets = this.CreateOutputDataSets(doc, output, dimensions);
            foreach (XmlElement dataSet in xeOutputDataSets)
            {
                xeParam.AppendChild(dataSet);
            }

            return xeParam;
        }//CreateOutputParameter

        private XmlElement CreateOutputParameter(XmlDocument doc, string name, Type type, int dimensions, string axis, bool isOutput)
        {
            XmlElement xeParam = doc.CreateElement("Parameter");

            XmlAttribute xaParam_Name = doc.CreateAttribute("name");
            xaParam_Name.Value = name;
            xeParam.Attributes.Append(xaParam_Name);

            XmlAttribute xaParam_Type = doc.CreateAttribute("type");
            xaParam_Type.Value = type.FullName;
            xeParam.Attributes.Append(xaParam_Type);

            XmlAttribute xaParam_Dimension = doc.CreateAttribute("dimensions");
            xaParam_Dimension.Value = dimensions.ToString();
            xeParam.Attributes.Append(xaParam_Dimension);

            if (axis != null)
            {
                XmlAttribute xaParam_Axis = doc.CreateAttribute("axis");
                xaParam_Axis.Value = axis;
                xeParam.Attributes.Append(xaParam_Axis);
            }

            if (isOutput)
            {
                XmlAttribute xaParam_Output = doc.CreateAttribute("output");
                xaParam_Output.Value = isOutput.ToString().ToLower();
                xeParam.Attributes.Append(xaParam_Output);
            }

            return xeParam;
        }//CreateOutputParameter
         
        //we get string in this format
        //2
        //[[NaN,NaN,NaN,NaN,5,0,NaN]]
        //[[0,0,0,0,1,0,0]\n [0,0,0,0,1,0,0]\n [0,0,0,0,1,0,0]\n [0,0,0,0,1,0,0]\n [0,0,0,0,1,0,0]]
        protected List<XmlElement> CreateOutputDataSets(XmlDocument doc, string output, int dimensions)
        {
            List<XmlElement> dataSets = new List<XmlElement>();

            //Replacing infinite output by 0
            output = output.Replace("NaN", "0");
            output = output.Replace("Infinity", "0");

            string[] sDataSets;
            if (dimensions == 1)
            {
                sDataSets = output.Split(',');
            }//1D
            else
            {
                sDataSets = output.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }//not 1D

            for (int i = 0; i < sDataSets.Length; i++)
            {
                XmlElement dataSet = doc.CreateElement("DataSet");
                dataSets.Add(dataSet);

                string sDataSet = sDataSets[i];
                sDataSet = sDataSet.Trim();
                sDataSet = sDataSet.Trim(new char[] { '[', ']' });

                string[] sDatas = sDataSet.Split(',');

                for (int j = 0; j < sDatas.Length; j++)
                {
                    XmlElement data = doc.CreateElement("Data");
                    dataSet.AppendChild(data);

                    string sData = sDatas[j];

                    XmlElement value = doc.CreateElement("Value");
                    data.AppendChild(value);
                    value.InnerText = sData;
                }

            }//foreach dataset


            return dataSets;
        }//CreateOutputDataSets


        #endregion

        #region Public methods

        public abstract bool Start();

        #endregion  //public methods

        #endregion  //methods

    }//MeasureModel class
}//namespace GME.MeasureModels
