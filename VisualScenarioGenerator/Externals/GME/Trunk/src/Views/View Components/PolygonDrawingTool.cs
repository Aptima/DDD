using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Windows.Forms;
using System.Drawing;
using AME.Views.View_Components;

namespace AME.Views.View_Components
{

    public class PolygonDrawingTool : GoTool
    {
        private Cursor previous;
        private DiagramPolygon myPolygon = null;

        public delegate void OnPolygonCreatedWithPoints(String PointString);

        // Define an event based on the above delegate
        public event OnPolygonCreatedWithPoints PolygonCreatedWithPoints;

        public PolygonDrawingTool(GoView view) : base(view) { }

        public override void Start()
        {
            previous = this.View.Cursor;
            this.View.Cursor = Cursors.Cross;
        }

        public override void Stop()
        {
            if (myPolygon != null)
            {
                myPolygon.Remove();
                if (PolygonCreatedWithPoints != null)
                {
                    PolygonCreatedWithPoints(myPolygon.GetPointString());
                }
                myPolygon = null;
            }

            this.View.Cursor = previous;
        }

        public override void DoMouseDown()
        {
            if (myPolygon == null)
            {
                myPolygon = new DiagramPolygon(this.View);
                myPolygon.Style = GoPolygonStyle.Line;
                myPolygon.Brush = Brushes.LightBlue;

                myPolygon.AddPoint(this.LastInput.DocPoint);
                myPolygon.AddPoint(this.LastInput.DocPoint); // pivot point
                this.View.Layers.Default.Add(myPolygon);
            }
            else
            {
                myPolygon.AddPoint(this.LastInput.DocPoint);
            }
        }

        public override void DoMouseMove()
        {
            if (myPolygon != null)
            {
                int numpts = myPolygon.PointsCount;

                PointF current = this.LastInput.DocPoint;

                this.View.DoAutoScroll(this.LastInput.ViewPoint); // autoscroll as they move around

                myPolygon.SetPoint(numpts - 1, current);
            }
        }


        public override void DoMouseUp() 
        {
            // override or tool will stop
        }

        public override void DoKeyDown()
        {
            if (this.LastInput.Key == Keys.Enter)
            {
                if (myPolygon != null && myPolygon.PointsCount > 2)
                {
                    // remove pivot
                    myPolygon.RemovePoint(myPolygon.PointsCount - 1);

                    this.View.StopAutoScroll();
                    StopTool();
                }
            }
            else if (this.LastInput.Control && this.LastInput.Key == Keys.Z)
            {   // undo
                if (myPolygon != null)
                {
                    myPolygon.RemovePoint(myPolygon.PointsCount - 1);
                }
            }
            else
            {
                base.DoKeyDown();
            }
        }
    }
}

