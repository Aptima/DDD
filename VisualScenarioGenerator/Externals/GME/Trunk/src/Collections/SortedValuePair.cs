/*
 * Class            : SortedValuePair
 * File             : SortedValuePair.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * The generic Key-Value pair to be stored in SortedValueList collection. Pair is 
 * compared based on the value.
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
    public class SortedValuePair<TKey, TValue>
        : IComparable<SortedValuePair<TKey, TValue>>,
        IComparable
        where TValue : IComparable
    {
        private TKey m_key;
        private TValue m_value;

        public SortedValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }//constructor

        public TKey Key
        {
            get { return this.m_key; }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException("Key");
                }
                this.m_key = value;
            }//set
        }//Key

        public TValue Value
        {
            get { return this.m_value; }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException("Value");
                }
                this.m_value = value;
            }//set
        }//Value

        public KeyValuePair<TKey, TValue> KeyValuePair
        {
            get
            {
                return new KeyValuePair<TKey, TValue>(this.Key, this.Value);
            }//get
        }//KeyValuePair

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(SortedValuePair<TKey, TValue>))
            {
                throw new System.ArgumentException("Wrong type");
            }

            SortedValuePair<TKey, TValue> tObj = (SortedValuePair<TKey, TValue>)obj;

            return this.Value.CompareTo(tObj.Value);
        }//CompareTo

        #endregion


        #region IComparable<SortedValuePair<TKey,TValue>> Members

        int IComparable<SortedValuePair<TKey, TValue>>.CompareTo(SortedValuePair<TKey, TValue> other)
        {
            return this.Value.CompareTo(other.Value);
        }

        #endregion

    }//SortedValuePair class
}//namespace Collections