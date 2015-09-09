/*
 * Class            : Controller
 * File             : Controller.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      : 
 * A generic comparer which allows to compare values in any direction.
 */

#region Imported Namespaces

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;

#endregion  //Namespaces

namespace Collections
{
    public class ReverseComparer<T>
        : IComparer<T>
        where T : IComparable<T>
    {
        private bool m_bIncrementOrder = true;

        public ReverseComparer()
            : this(true)
        {
        }//constructor

        public ReverseComparer(bool incrementOrder)
        {
            this.IncrementOrder = incrementOrder;
        }//constructor

        public bool IncrementOrder
        {
            get { return this.m_bIncrementOrder; }
            set { this.m_bIncrementOrder = value; }
        }//IncrementOrder

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            int iCompare = x.CompareTo(y);

            if (this.IncrementOrder)
            {
                iCompare = -iCompare;
            }
            //iCompare = this.IncrementOrder ? -iCompare : iCompare;

            return iCompare;
        }

        #endregion

    }//ReverseComparer class
}//namespace Collections