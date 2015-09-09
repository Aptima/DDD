using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.Controls
{
    public class PanelViewportRegion : PanelControl
    {
        private Microsoft.DirectX.Direct3D.Viewport view;
        private Microsoft.DirectX.Direct3D.Viewport backup;

        public Microsoft.DirectX.Direct3D.Viewport PanelViewport
        {
            get
            {
                return view;
            }
        }
        public PanelViewportRegion()
        {
            view = new Viewport();
            
        }
        public void SetViewportDimensions(Rectangle viewport_dimensions)
        {
            view.X = viewport_dimensions.X;
            view.Y = viewport_dimensions.Y;
            view.Height = viewport_dimensions.Height;
            view.Width = viewport_dimensions.Width;
        }
        /// <summary>
        /// This might not make sense, revisit.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        protected void StartViewport(Canvas c, Rectangle r)
        {
            backup = c.Viewport;
            SetViewportDimensions(r);
            c.Viewport = PanelViewport;
        }
        protected void StartViewport(Canvas c)
        {
            backup = c.Viewport;
            c.Viewport = PanelViewport;
        }
        protected void EndViewport(Canvas c)
        {
            c.Viewport = backup;
        }
    }
}
