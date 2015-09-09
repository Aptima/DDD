#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

#endregion  //Namespaces

namespace AME.Controllers
{

    // small grouped data class
    // provides a more convenient way to return key data about a component
    // used in checking UseClassName parameter 
    internal class ComponentIDName
    {
        private int m_ID;

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public ComponentIDName(int id, string name)
        {
            m_ID = id;
            m_Name = name;
        }
    }
}