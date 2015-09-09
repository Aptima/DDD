/*
 * Class            : Controller
 * File             : Controller_Others.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      : 
 * Contains the classes being used in Controller class for storage, etc..
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
    public partial class Controller
    {
        public class ChildComponents : 
            IComparable,
            IComparable<ChildComponents>
        {
            private int m_iID;
            private string m_sName;
            private List<int> m_childComponentIds;

            public ChildComponents(int id, string name)
            {
                this.m_iID = id;
                this.m_sName = name;
                this.m_childComponentIds = new List<int>();
            }//constructor

            public int ID
            {
                get { return this.m_iID; }
            }//ID

            public string Name
            {
                get { return this.m_sName; }
            }//Name

            public List<int> ChildIDs
            {
                get { return this.m_childComponentIds; }
                set { this.m_childComponentIds = value; }
            }//ChildIDs


            #region IComparable Members

            public int CompareTo(object obj)
            {
                if (obj.GetType() != typeof(ChildComponents))
                {
                    throw new System.ArgumentException("Invalid type");
                }

                return this.CompareTo((ChildComponents)obj);
            }//CompareTo

            #endregion

            #region IComparable<ChildComponents> Members

            public int CompareTo(ChildComponents other)
            {
                return this.m_childComponentIds.Count.CompareTo(other.m_childComponentIds.Count);
            }//CompareTo

            #endregion
        }//ChildComponents class
    
    }//Controller class
}//Controllers namespace
