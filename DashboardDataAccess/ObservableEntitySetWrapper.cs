using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Collections;
namespace DashboardDataAccess
{
    public class ObservableEntitySetWrapper<T> : IEnumerable<T>, IList<T>, INotifyCollectionChanged
        where T : class
    {
        EntitySet<T> _dataSource;
        List<T> unbound;

        public ObservableEntitySetWrapper(EntitySet<T> dataSource)
        {
            _dataSource = dataSource;
            _dataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(_dataSource_ListChanged);
            unbound = new List<T>();
        }

        void _dataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, _dataSource[e.NewIndex]));
            else if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted)
            {
                // Item already deleted from collection, and it is not appeart how to correctly
                // use NotifyCollectionChangedAction.Remove in this case.
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemMoved)
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, _dataSource[e.NewIndex], e.NewIndex, e.OldIndex));
            else
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, e);
        }

        public EntitySet<T> Source
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        #region ExtraData
        public void AddUnbound(T extraItem)
        {
            unbound.Add(extraItem);
            OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, extraItem));
        }

        public void RemoveUnbound(T extraItem)
        {
            if (unbound.Remove(extraItem))
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, extraItem));

        }
        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ObservableWrapperEnumerator<T>(this);
        }

        #endregion

        #region IList<T> Members

        public int IndexOf(T item)
        {
            if (unbound.Contains(item))
                return unbound.IndexOf(item);
            return GetIndex(item) + unbound.Count;
        }

        private int GetIndex(T item)
        {
            IList il = _dataSource as IList;
            if (il != null)
                return il.IndexOf(item);
            //does not have indexof loop through collection
            int i = -1;
            foreach (T t in _dataSource)
            {
                i++;
                if (item == t)
                    break;
            }
            return i;
        }

        public void Insert(int index, T item)
        {
            if (index < unbound.Count)
                throw new InvalidOperationException();
            _dataSource.Insert(index - unbound.Count, item);
            OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public T this[int index]
        {
            get
            {
                T val;
                if (index < unbound.Count)
                    val = unbound[index];
                else
                    val = _dataSource[index - unbound.Count];
                return val;
            }
            set
            {
                if (index < unbound.Count)
                    unbound[index] = value;
                else
                    _dataSource[index - unbound.Count] = value;
            }
        }

        public void RemoveAt(int index)
        {
            if (index < unbound.Count)
                throw new InvalidOperationException();
            NotifyCollectionChangedEventArgs ea = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, _dataSource[index - unbound.Count]);
            _dataSource.RemoveAt(index - unbound.Count);
            OnCollectionChanged(this, ea);

        }


        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            _dataSource.Add(item);
        }

        public bool Contains(T item)
        {
            return unbound.Contains(item) || _dataSource.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _dataSource.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool ans = true;
            NotifyCollectionChangedEventArgs ea = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0);
            if (unbound.Contains(item))
                ans = unbound.Remove(item);
            else
                ans = _dataSource.Remove(item);

            if (ans)
            {
                OnCollectionChanged(this, ea);
            }
            return ans;

        }

        #endregion

        #region ICollection<T> Members


        /// <summary>
        /// Does not clear the underlying datasource
        /// sets the datasource to null;
        /// </summary>
        public void Clear()
        {
            _dataSource = null;
        }

        public int Count
        {
            get
            {
                return unbound.Count + _dataSource.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ObservableWrapperEnumerator<T>(this);
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }

    class ObservableWrapperEnumerator<T> : IEnumerator<T>
        where T : class
    {
        public ObservableWrapperEnumerator(ObservableEntitySetWrapper<T> wrapper)
        {
            _wrapper = wrapper;
        }

        private ObservableEntitySetWrapper<T> _wrapper;
        int _index = -1;

        public void Reset()
        {
            _index = -1;
        }

        public bool MoveNext()
        {
            if (_index >= _wrapper.Count - 1)
                return false;

            _index++;
            return true;
        }
        #region IEnumerator<T> Members

        public T Current
        {
            get { return _wrapper[_index]; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return _wrapper[_index]; }
        }

        #endregion
    }
}
