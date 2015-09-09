using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeamateAdapter
{
    public class ScheduledItemsList
    {
        private List<ScheduledItem> _list;

        public ScheduledItemsList()
        {
            _list = new List<ScheduledItem>();
        }

        public void Add(ScheduledItem i)
        {
            if (_list.Count == 0)
            {
                _list.Add(i);
                return;
            }
            for (int x = 0; x < _list.Count; x++)
            {
                if (i.Time < _list[x].Time)
                {
                    _list.Insert(x, i);
                    return;
                }
            }
            _list.Add(i);
        }
        public int Count()
        {
            return _list.Count;
        }
        public void Clear()
        {
            _list.Clear();
        }
        /// <summary>
        /// Retrieves all ScheduledItems with a time less than or equal to the time parameter.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<ScheduledItem> GetItemsUpTo(int time)
        {
            List<ScheduledItem> items = new List<ScheduledItem>();

            for (int x = 0; x < _list.Count; )
            {
                if (_list[x].Time <= time)
                {
                    items.Add(_list[x]);
                    _list.RemoveAt(x);
                }
                else
                {
                    break;
                }
            }

            return items;
        }
        public List<ScheduledItem> GetFirstItemsPerDmUpTo(int time)
        {
            List<String> dmsAdded = new List<String>();
            List<ScheduledItem> items = new List<ScheduledItem>();

            for (int x = 0; x < _list.Count; )
            {
                if (_list[x].Time <= time && !dmsAdded.Contains(_list[x].DM_ID))
                {
                    items.Add(_list[x]);
                    _list.RemoveAt(x);
                    dmsAdded.Add(_list[x].DM_ID);
                }
                else
                {
                   // break;
                }
            }

            return items;
        }
    }
}
