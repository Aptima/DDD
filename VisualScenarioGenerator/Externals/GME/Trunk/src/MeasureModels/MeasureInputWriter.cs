using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;

namespace AME.MeasureModels
{
    public class MeasureInputWriter
    {
        private static MeasureInputWriter _measureInputWriter = null;

        #region Constructors

        #region Static constructors

        static MeasureInputWriter()
        {
            MeasureInputWriter.Writer = new MeasureInputWriter();
        }//constructor

        #endregion  //static constructors

        #endregion  //constructors


        #region Properties

        #region Static properties

        #region Private static properties

        private static MeasureInputWriter Writer
        {
            get { return MeasureInputWriter._measureInputWriter; }
            set { MeasureInputWriter._measureInputWriter = value; }
        }//Writer

        #endregion  //private static properties

        #endregion  //static properties

        #endregion  //properties


        #region Methods

        #region Static methods

        #region Public static methods

        public static Array ConvertTo2DArray<T>(T a)
        {
            return Writer._ConvertTo2DArray<T>(a);
        }//ConvertTo2DArray

        public static Array ConvertTo2DArray<T>(T[] a)
        {
            return Writer._ConvertTo2DArray<T>(a);
        }//ConvertTo2DArray

        public static Array ConvertTo2DArray(Matrix a)
        {
            return Writer._ConvertTo2DArray(a);
        }//ConvertTo2DArray

        public static XmlElement CreateParameter(
            XmlDocument doc,
            string paramName,
            Type paramType,
            int dimensions,
            Array allData)
        {
            return Writer._CreateParameter(doc, paramName, paramType, dimensions, allData);
        }//CreateParameter

        public static void CreateMeasureInputXml(string measureName, out XmlDocument doc, out XmlElement component)
        {
            Writer._CreateMeasureInputXml(measureName, out doc, out component);
        }//CreateMeasureInputXml

        #endregion  //public static methods

        #endregion  //static methods


        #region Instance methods

        #region Private instance methods

        private Array _ConvertTo2DArray<T>(T a)
        {
            T[,] output = new T[1, 1];
            output[0, 0] = a;
            return output;
        }//_ConvertTo2DArray

        private Array _ConvertTo2DArray<T>(T[] a)
        {
            T[,] output = new T[a.Length, 1];
            for (int i = 0; i < a.Length; i++)
            {
                output[i, 0] = a[i];
            }

            return output;
        }//_ConvertTo2DArray

        private Array _ConvertTo2DArray(Matrix a)
        {
            double[,] output = new double[a.RowCount, a.ColumnCount];

            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    double dVal = a[i, j];
                    if (double.IsNaN(dVal))
                    {
                        dVal = 0;
                    }
                    output[i, j] = dVal;
                }//foreach column
            }//foreach row

            return output;
        }//_ConvertTo2DArray

        private XmlElement _CreateParameter(XmlDocument doc, string paramName, Type paramType, int dimensions, Array allData)
        {
            XmlElement xeParam = doc.CreateElement("Parameter");
            XmlAttribute xaParam_Name = doc.CreateAttribute("name");
            xaParam_Name.Value = paramName;
            xeParam.Attributes.Append(xaParam_Name);
            XmlAttribute xaParam_Type = doc.CreateAttribute("type");
            xaParam_Type.Value = paramType.FullName;
            xeParam.Attributes.Append(xaParam_Type);
            XmlAttribute xaParam_Dimentions = doc.CreateAttribute("dimensions");
            xaParam_Dimentions.Value = dimensions.ToString();
            xeParam.Attributes.Append(xaParam_Dimentions);

            List<XmlElement> dataSets = this._CreateParameterDataSets(doc, allData, dimensions);
            foreach (XmlElement dataSet in dataSets)
            {
                xeParam.AppendChild(dataSet);
            }

            return xeParam;
        }//_CreateParameter

        private List<XmlElement> _CreateParameterDataSets(XmlDocument doc, Array allData, int dimensions)
        {
            List<XmlElement> dataSets = new List<XmlElement>();

            //for 0, 1, & 2D evertything comes up as double array
            for (int i = 0; i < allData.GetLength(0); i++)
            {
                XmlElement dataSet = doc.CreateElement("DataSet");
                dataSets.Add(dataSet);
                for (int j = 0; j < allData.GetLength(1); j++)
                {
                    XmlElement data = doc.CreateElement("Data");
                    dataSet.AppendChild(data);
                    XmlElement value = doc.CreateElement("Value");
                    value.InnerText = allData.GetValue(i, j).ToString();
                    data.AppendChild(value);
                }
            }

            return dataSets;
        }//_CreateParameterDataSets

        private void _CreateMeasureInputXml(string measureName, out XmlDocument doc, out XmlElement component)
        {
            XmlDocument inXmlDoc = new XmlDocument();
            XmlDeclaration declaration = inXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            inXmlDoc.AppendChild(declaration);

            XmlElement xeComponent = inXmlDoc.CreateElement("Component");
            inXmlDoc.AppendChild(xeComponent);

            XmlAttribute xaComp_type = inXmlDoc.CreateAttribute("type");
            xaComp_type.Value = "Measure";
            xeComponent.Attributes.Append(xaComp_type);

            XmlAttribute xaComp_name = inXmlDoc.CreateAttribute("name");
            xaComp_name.Value = measureName;
            xeComponent.Attributes.Append(xaComp_name);

            doc = inXmlDoc;
            component = xeComponent;
        }//_CreateMeasureInputXml

        #endregion  //private instance methods

        #endregion  //instance methods


        #endregion  //methods

    }//MeasureInputWriter class
}//namespace GME.MeasureModels