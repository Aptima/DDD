/*
 * Class            : Parameter
 * File             : Parameter.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * Used in base Controller class to manage parameters.
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

#endregion  //Namespaces

namespace AME.Controllers
{
    public class Parameter
    {
        private string m_sName;
        private Type m_tType;
        private string m_sDefaultValue;

        public Parameter(string name, Type type, string defaultValue)
        {
            this.m_sName = name;
            this.m_tType = type;
            this.m_sDefaultValue = defaultValue;
        }//constructor

        public Parameter(string name, Type type)
            : this(name, type, "")
        {
        }//constructor

        public string Name
        {
            get { return this.m_sName; }
        }//Name

        public Type Type
        {
            get { return this.m_tType; }
        }//Type

        public string DefaultValue
        {
            get { return this.m_sDefaultValue; }
        }//DefaultValue

    }//Parameter class

}//Controllers namespace