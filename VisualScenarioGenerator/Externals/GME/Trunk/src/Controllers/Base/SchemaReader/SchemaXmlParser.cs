/*
 * Class            : SchemaXmlParser
 * File             : SchemaXmlParser.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Parses the values to be read from schema through xml file.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml.XPath;
using AME.Views.View_Component_Packages;
using AME.Model;
using System.Xml;

#endregion  //Namespaces


namespace AME.Controllers
{
    public class SchemaXmlParser
    {

        #region Constructors

        #region Public constructors

        #endregion  //public constructors

        public SchemaXmlParser()
        {
        }//constructor

        #endregion  //constructors

        #region Methods

        #region Public methods

        public List<ComponentFunction> ParseComponentFunction(string name, string value)
        {
            List<ComponentFunction> lCompFuncs = new List<ComponentFunction>();

            switch (name)
            {
                case XmlSchemaConstants.Config.VisibleFunctions:
                case XmlSchemaConstants.Config.InvisibleFunctions:

                    bool bVisible = (name == XmlSchemaConstants.Config.VisibleFunctions) ? true : false;

                    string[] aFuncs = value.Split(new char[] { XmlSchemaConstants.Config.FunctionDelimiter });
                    for (int i = 0; i < aFuncs.Length; i++)
                    {
                        string sFunc = aFuncs[i];
                        sFunc = sFunc.Trim();

                        string[] sFuncNameAction = sFunc.Split(new char[] { XmlSchemaConstants.Config.FunctionNameValueDelimiter });
                        if (sFuncNameAction.Length == 2)
                        {
                            string sFuncName = sFuncNameAction[0].Trim();
                            string sFuncAction = sFuncNameAction[1].Trim();

                            ComponentFunction compFunc = new ComponentFunction(sFuncName, sFuncAction, bVisible);
                            lCompFuncs.Add(compFunc);
                        }//if
                    }//for each function

                    break;

            }//switch

            return lCompFuncs;
        }//ParseComponentFunction

        #endregion  //public methods

        #endregion  //methods

    }//SchemaXmlParser class
}//namespace AME.Controllers
