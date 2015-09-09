/*
 * Class            : ParamConstraint
 * File             : ParamConstraint.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Contains a parameter's all constraints.
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
    public class ParamConstraint
    {
        private RangeConstraint _range = null;
        private string _sDefault = null;

        #region Constructors

        #region Public constructors

        public ParamConstraint()
        {
            this._range = new RangeConstraint();
        }//constructor

        #endregion  //public constructors

        #endregion  //constructors

        #region Properties

        #region Public properties

        public RangeConstraint Range
        {
            get { return this._range; }
            set { this._range = value; }
        }//Range

        public string DefaultValue
        {
            get { return this._sDefault; }
            set { this._sDefault = value; }
        }//DefaultValue

        #endregion  //public properties

        #endregion  //properties

    }//ParamConstraint class
}//namespace AME.Controllers
