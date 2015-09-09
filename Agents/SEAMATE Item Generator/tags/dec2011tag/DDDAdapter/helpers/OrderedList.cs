using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeamateAdapter.DDD
{
    public class OrderedEventsList
    {
        private List<DDDEvent> _list;

        public OrderedEventsList()
        {
            _list = new List<DDDEvent>();
        }

        public void Add(DDDEvent i)
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
        public List<DDDEvent> GetEventsUpTo(int time)
        {
            List<DDDEvent> items = new List<DDDEvent>();

            for (int x = 0; x < _list.Count; )
            {
                if (_list[x].Time <= time)
                {
                    items.Add(_list[x]);
                    _list.RemoveAt(x);//no need to increment counter on this.
                }
                else
                {
                    break;
                }
            }

            return items;
        }
    }
}
