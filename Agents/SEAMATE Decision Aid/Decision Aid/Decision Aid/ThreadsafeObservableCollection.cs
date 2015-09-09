using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace Decision_Aid
{
    public class ThreadsafeObservableCollection<T> : ObservableCollection<T>
    {
        private Dispatcher _dispatcher;
        private ReaderWriterLockSlim _lock;

        public ThreadsafeObservableCollection()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _lock = new ReaderWriterLockSlim();
        }

        protected override void ClearItems()
        {
            _dispatcher.InvokeIfRequired(() =>
            {
                _lock.EnterWriteLock();
                try
                {
                    base.ClearItems();
                }
                catch (Exception ex)
                { }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }
        protected override void InsertItem(int index, T item)
        {
            _dispatcher.InvokeIfRequired(() =>
            {
                if (index > this.Count)
                    return;
                _lock.EnterWriteLock();
                try
                {
                    base.InsertItem(index, item);
                }
                catch (Exception ex)
                { }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            _dispatcher.InvokeIfRequired(() =>
            {
                _lock.EnterReadLock();
                Int32 itemCount = this.Count;
                _lock.ExitReadLock();

                if (oldIndex >= itemCount || newIndex >= itemCount || oldIndex == newIndex)
                    return;

                _lock.EnterWriteLock();
                try
                {
                    base.MoveItem(oldIndex, newIndex);
                }
                catch (Exception ex)
                { }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }
        protected override void RemoveItem(int index)
        {
            _dispatcher.InvokeIfRequired(() =>
            {
                if (index >= this.Count)
                    return;

                _lock.EnterWriteLock();
                try
                {
                    base.RemoveItem(index);
                }
                catch (Exception ex)
                { }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }
        protected override void SetItem(int index, T item)
        {
            _dispatcher.InvokeIfRequired(() =>
            {
                _lock.EnterWriteLock();
                try
                {
                    base.SetItem(index, item);
                }
                catch (Exception ex)
                { }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }, DispatcherPriority.DataBind);
        }
        public T[] ToSyncArray()
        {
            _lock.EnterReadLock();
            try
            {
                T[] _sync = new T[this.Count];
                this.CopyTo(_sync, 0);
                return _sync;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
