using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using AME.Controllers;
using AME.Model;

namespace AME.Views.View_Components
{
    public class ScrollingListView : System.Windows.Forms.ListView
    {
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x020A;

        public event EventHandler HScrollMoved;
        public event EventHandler VScrollMoved;
        public event EventHandler MouseWheelRotated;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    this.OnHScrollMove();
                    break;

                case WM_VSCROLL:
                    this.OnVScrollMove();
                    break;

                case WM_MOUSEWHEEL:
                    this.OnMouseWheelRotate();
                    break;

            }//switch

            base.WndProc(ref m);
        }//WndProc

        protected void OnHScrollMove()
        {
            if (this.HScrollMoved != null)
            {
                this.HScrollMoved(this, new EventArgs());
            }
        }//OnHScrollMove

        protected void OnVScrollMove()
        {
            if (this.VScrollMoved != null)
            {
                this.VScrollMoved(this, new EventArgs());
            }
        }//OnVScrollMove

        protected void OnMouseWheelRotate()
        {
            if (this.MouseWheelRotated != null)
            {
                this.MouseWheelRotated(this, new EventArgs());
            }
        }

    }//ScrollingListView class
}//AME.Views.View_Components namespace
