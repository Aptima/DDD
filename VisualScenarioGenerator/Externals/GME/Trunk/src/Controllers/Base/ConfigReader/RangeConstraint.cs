/*
 * Class            : RangeConstraint
 * File             : RangeConstraint.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Contains Min and Max constraint values of a parameter.
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
    public class RangeConstraint
    {
        private string _sMin = null;
        private string _sMax = null;

        #region Constructors

        #region Public constructors

        public RangeConstraint()
        {
        }//constructor

        public RangeConstraint(string min, string max)
        {
            this._sMin = min;
            this._sMax = max;
        }//constructor

        #endregion  //public constructors

        #endregion  //constructors

        #region Properties

        #region Public properties

        public string Min
        {
            get { return this._sMin; }
            set { this._sMin = value; }
        }//Min

        public string Max
        {
            get { return this._sMax; }
            set { this._sMax = value; }
        }//Max

        #endregion  //public properties

        #endregion  //properties

    }//RangeConstraint class
}//namespace AME.Controllers
