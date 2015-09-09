using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Elsehemy.Controls
{
    public class ColorWheelEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iwefs = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            Color c;
            using (ColorWheelContainer cwc = new ColorWheelContainer(iwefs))
            {
                cwc.Color = (Color)value;
                iwefs.DropDownControl(cwc);
                if (cwc.Result == DialogResult.OK)
                {
                    c = cwc.Color;
                }
                else
                {
                    c = (Color)value;
                }
            }
            return c;
        }
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true; // we will use the picked color and fill the rectangle.
        }
        public override bool IsDropDownResizable
        {
            get
            {
                return false;//we don't want it to be resizable
            }
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            Color c = (Color)e.Value;
            e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);
        }
    }
}
