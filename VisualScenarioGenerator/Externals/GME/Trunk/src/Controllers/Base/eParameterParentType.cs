/*
 * eNum             : eParameterParent
 * File             : eParameterParent.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * This eNum stores the possible values of the "parentType" in "parametertable".
 * The possible parents of a parameter can be Component or Link.
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
    public enum eParamParentType
    {
        Component,
        Link
    }//eParamParentType

    public enum eParameterRead
    {
        Display,
        Config
    }//eParameterRead

}//nsmespace Controllers