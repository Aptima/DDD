using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Aptima.Asim.DDD.Client
{
    class MessageFilter: IMessageFilter
    {
        private static MessageFilter _instance = null;

        public const int WM_MOUSEWHEEL = 0x020A;

        public static MessageFilter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageFilter();
                }
                return _instance;
            }
        }

        private  MessageFilter()
        {
        }
        #region IMessageFilter Members
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
