/*
 * Class            : SortedValueList
 * File             : SortedValueList.cs
 * Author           : Bhavna Mangal
 * Description      : 
 * A generic DictionaryList class which stores values as Key-Value pair based on 
 * Key's uniqueness and sorts values based on the Value. Allows to sort values in 
 * Incrementing or Decrementing order.
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

    /// <summary>
    /// A SortedDictionaryList which sorts items based on value.
    /// </summary>
    public class SortedValueList<TKey, TValue>
        : IDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IDictionary,
        ICollection,
        IEnumerable
        where TValue : IComparable
    {

        private List<SortedValuePair<TKey, TValue>> m_itemList;
        private ReverseComparer<SortedValuePair<TKey, TValue>> m_revComparer;

        public SortedValueList()
        {
            m_itemList = new List<SortedValuePair<TKey, TValue>>();
            m_revComparer = new ReverseComparer<SortedValuePair<TKey, TValue>>(true);
        }//constructor

        #region Private Members

        private List<KeyValuePair<TKey, TValue>> PairList
        {
            get
            {
                List<KeyValuePair<TKey, TValue>> pairList =
                    new List<KeyValuePair<TKey, TValue>>(this.Count);

                this.Sort();
                foreach (SortedValuePair<TKey, TValue> pair in m_itemList)
                {
                    pairList.Add(pair.KeyValuePair);
                }
                return pairList;
            }//get
        }//PairList

        private Dictionary<TKey, TValue> PairDictionary
        {
            get
            {
                Dictionary<TKey, TValue> dictionary =
                    new Dictionary<TKey, TValue>(this.Count);

                this.Sort();
                foreach (SortedValuePair<TKey, TValue> pair in m_itemList)
                {
                    dictionary.Add(pair.Key, pair.Value);
                }

                return dictionary;
            }//get
        }//PairDictionary

        private SortedValuePair<TKey, TValue> GetPair(TKey key)
        {
            int keyIndex = ((List<TKey>)this.Keys).IndexOf(key);

            if ((keyIndex >= 0) && (this.m_itemList.Count > 0))
            {
                return this.m_itemList[keyIndex];
            }

            return null;
        }//GetPair

        private void Sort()
        {
            this.m_itemList.Sort(m_revComparer);
        }//Sort

        #endregion

        #region Public Members

        public int IndexOfKey(TKey key)
        {
            return ((List<TKey>)this.Keys).IndexOf(key);
        }//IndexOfKey

        public KeyValuePair<TKey, TValue> GetPairByIndex(int index)
        {
            if (index >= this.Count)
            {
                throw new System.ArgumentOutOfRangeException();
            }
            return this.m_itemList[index].KeyValuePair;
        }//GetPairByIndex

        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new System.ArgumentNullException("Key");
            }

            if (value == null)
            {
                throw new System.ArgumentNullException("Value");
            }

            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }

            SortedValuePair<TKey, TValue> keyValuePair =
                new SortedValuePair<TKey, TValue>(key, value);
            this.m_itemList.Add(keyValuePair);
            this.Sort();
        }//Add

        public bool ContainsKey(TKey key)
        {
            bool bContains = (this.GetPair(key) != null);
            return bContains;
        }//ContainsKey

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keyList = new List<TKey>();

                this.Sort();
                foreach (SortedValuePair<TKey, TValue> item in this.m_itemList)
                {
                    keyList.Add(item.Key);
                }

                return keyList;
            }//get
        }//Keys

        public bool Remove(TKey key)
        {
            bool bRemoved = false;
            SortedValuePair<TKey, TValue> pair = this.GetPair(key);

            if (pair != null)
            {
                bRemoved = this.m_itemList.Remove(pair);
                this.Sort();
            }
            return bRemoved;
        }//Remove

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = this.IndexOfKey(key);
            if (index >= 0)
            {
                value = GetPairByIndex(index).Value;
                return true;
            }
            value = default(TValue);
            return false;
        }//TryGetValue

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> valList = new List<TValue>();

                this.Sort();
                foreach (SortedValuePair<TKey, TValue> item in this.m_itemList)
                {
                    valList.Add(item.Value);
                }

                return valList;
            }//get
        }//Values

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new System.ArgumentNullException("Key");
                }

                SortedValuePair<TKey, TValue> pair = this.GetPair(key);

                if (pair != null)
                {
                    return pair.Value;
                }
                throw new System.Exception("Does not contain this key.");
                //return default(TValue);
            }//get
            set
            {
                this.Add(key, value);
            }//set
        }//Item

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }//Add

        public void Clear()
        {
            this.m_itemList.Clear();
        }//Clear

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool bContains = false;
            SortedValuePair<TKey, TValue> pair = this.GetPair(item.Key);

            if (pair != null)
            {
                bContains = pair.Value.Equals(item.Value);
            }
            return bContains;
        }//Contains

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException("Array is null.");
            }
            if ((arrayIndex < 0) || (arrayIndex > array.Length))
            {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((array.Length - arrayIndex) < this.Count)
            {
                throw new System.ArithmeticException
                    ("The number of items to be copied is greater than the available space from arrayIndex to the end of the destination array.");
            }

            this.Sort();
            for (int i = 0; i < this.Count; i++)
            {
                KeyValuePair<TKey, TValue> item =
                    new KeyValuePair<TKey, TValue>(m_itemList[i].Key, m_itemList[i].Value);
                array[arrayIndex + i] = item;
            }
        }//CopyTo

        public int Count
        {
            get
            {
                return this.m_itemList.Count;
            }//get
        }//Count

        public bool IsReadOnly
        {
            get { return false; }
        }//IsReadOnly

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool bRemoved = false;

            if (this.Contains(item))
            {
                this.Remove(item.Key);
            }

            return bRemoved;
        }//Remove

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.PairList.GetEnumerator();
        }//GetEnumerator

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.PairList.GetEnumerator();
        }//GetEnumerator

        #endregion

        #region IDictionary Members

        public void Add(object key, object value)
        {
            if (key.GetType() != typeof(TKey))
            {
                throw new System.ArgumentException("Invalid key type.");
            }

            if (value.GetType() != typeof(TValue))
            {
                throw new System.ArgumentException("Invalid value type.");
            }

            this.Add((TKey)key, (TValue)value);
        }//Add

        public bool Contains(object key)
        {
            if (key.GetType() != typeof(TKey))
            {
                throw new System.ArgumentException("Invalid key type.");
            }

            return this.ContainsKey((TKey)key);
        }//Contains

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.PairDictionary.GetEnumerator();
        }//GetEnumerator

        public bool IsFixedSize
        {
            get { return false; }
        }//IsFixedSize

        ICollection IDictionary.Keys
        {
            get
            {
                return this.PairDictionary.Keys;
            }//get
        }//Keys

        public void Remove(object key)
        {
            if (key.GetType() != typeof(TKey))
            {
                throw new System.ArgumentException("Invalid key type.");
            }

            this.Remove((TKey)key);
        }//Remove

        ICollection IDictionary.Values
        {
            get
            {
                return this.PairDictionary.Values;
            }//get
        }//Values

        public object this[object key]
        {
            get
            {
                if (key.GetType() != typeof(TKey))
                {
                    throw new System.ArgumentException("Invalid key type.");
                }
                return this[(TKey)key];
            }//get
            set
            {
                if (key.GetType() != typeof(TKey))
                {
                    throw new System.ArgumentException("Invalid key type.");
                }
                this[(TKey)key] = (TValue)value;
            }//set
        }//Item

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }//CopyTo

        public bool IsSynchronized
        {
            get { return false; }
        }//IsSynchronized

        public object SyncRoot
        {
            get { return this; }
        }//SyncRoot

        #endregion

    }//SortedValueList class
}//namespace Collections