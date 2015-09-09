using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class CellRange
    {
        public int CellNumber = 0;
        public int Xindex = 0;
        public int Yindex = 0;

        public double Dimension1Start = 0.0;
        public double Dimension1End = 0.0;
        public double Dimension2Start = 0.0;
        public double Dimension2End = 0.0;
        public CellRange()
        { }
        public CellRange(double start, double end, double start2, double end2)
        {
            Dimension1Start = start;
            Dimension1End = end; 
            Dimension2Start = start2;
            Dimension2End = end2;
        }
        public CellRange(double start, double end, double start2, double end2, int cellNumber, int xIndex, int yIndex)
        {
            Dimension1Start = start;
            Dimension1End = end;
            Dimension2Start = start2;
            Dimension2End = end2;
            CellNumber = cellNumber;
            Xindex = xIndex;
            Yindex = yIndex;
        }
        public CellRange Copy()
        {
            CellRange r = new CellRange();

            r.CellNumber = this.CellNumber;
            r.Xindex = this.Xindex;
            r.Yindex = this.Yindex;
            r.Dimension1Start = this.Dimension1Start;
            r.Dimension1End = this.Dimension1End;
            r.Dimension2Start = this.Dimension2Start;
            r.Dimension2End = this.Dimension2End;
            return r;
        }
    }
}
