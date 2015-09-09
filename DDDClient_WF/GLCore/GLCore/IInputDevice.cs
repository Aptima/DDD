using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Common.GLCore
{
    interface IDeviceInput
    {
        void OnMouseClick(object sender, MouseEventArgs e);
        void OnMouseDoubleClick(object sender, MouseEventArgs e);
        void OnMouseDown(object sender, MouseEventArgs e);
        void OnMouseMove(object sender, MouseEventArgs e);
        void OnMouseUp(object sender, MouseEventArgs e);
        void OnMouseWheel(object sender, MouseEventArgs e);
        void OnKeyDown(object sender, KeyEventArgs e);
        void OnKeyPress(object sender, KeyPressEventArgs e);
        void OnKeyUp(object sender, KeyEventArgs e);
    }
}
