using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AME.Views.View_Components
{
    // use these calls to do flicker free updates on most .NET controls
    // careful, uses Windows API!

    // The suggested use is to call SuspendDrawing(control), update 
    // the control or its data, then call ResumeDrawing(control)

    public static class DrawingUtility
    {
        private static bool suspend = false;

        public static bool Suspend
        {
            get { return DrawingUtility.suspend; }
            set { DrawingUtility.suspend = value; }
        }

        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32", CharSet = CharSet.Auto)]

        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        public static void SuspendDrawing(Control c)  
        {
            try
            {
                // disable drawing
                if (c is TabPage)
                {
                    SendMessage(c.Parent.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                }
                else
                    SendMessage(c.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                suspend = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
        }

        public static void ResumeDrawing(Control c)
        {
            try
            {
                //re-enable drawing
                if (c is TabPage)
                {
                    SendMessage(c.Parent.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                    //c.Refresh(); // <-- actually redraw
                    c.Parent.Refresh();
                }
                else
                {
                    SendMessage(c.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

                    c.Refresh(); // <-- actually redraw
                }
                suspend = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
        }
    }
}
