using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class CpeMatrix
    {
        /*
         *  |------------------------------------------------------------
         *  |       | Dim2 lowest   | ....................| Dim2 Highest|
         *  |------------------------------------------------------------
         *  | Dim1  |      1        |                           7
         *  | Lowest|               |
         *  |------------------------------------------------------------
         *  |   .
         *  |   .
         *  |   .
         *  |------------------------------------------------------------
         *  | Dim1   |      43      |                           49
         *  | Highest|              |
         *  |------------------------------------------------------------
         */
        private List<List<CellRange>> _matrix;
        public CpeMatrix()
        { }

        private void DemoLoad()
        {
            _matrix = new List<List<CellRange>>();
            for (int x = 0; x < 7; x++)
            {
                _matrix.Add(new List<CellRange>());
                for (int y = 0; y < 7; y++)
                {
                   // _matrix[x].Add(new CellRange());
                }
            }
            _matrix[0].Add(new CellRange(-5, -1.5, -5, -1.5,   1, 0, 0));
            _matrix[1].Add(new CellRange(-5, -1.5, -1.5, -.75, 2, 1, 0));
            _matrix[2].Add(new CellRange(-5, -1.5, -.75, -.25, 3, 2, 0));
            _matrix[3].Add(new CellRange(-5, -1.5, -.25, .25,  4, 3, 0));
            _matrix[4].Add(new CellRange(-5, -1.5, .25, .75,   5, 4, 0));
            _matrix[5].Add(new CellRange(-5, -1.5, .75, 1.5,   6, 5, 0));
            _matrix[6].Add(new CellRange(-5, -1.5, 1.5, 5,     7, 6, 0));

            _matrix[0].Add(new CellRange(-1.5, -.75, -5, -1.5, 8, 0, 1));
            _matrix[1].Add(new CellRange(-1.5, -.75, -1.5, -.75, 9, 1, 1));
            _matrix[2].Add(new CellRange(-1.5, -.75, -.75, -.25, 10, 2, 1));
            _matrix[3].Add(new CellRange(-1.5, -.75, -.25, .25, 11, 3, 1));
            _matrix[4].Add(new CellRange(-1.5, -.75, .25, .75, 12, 4, 1));
            _matrix[5].Add(new CellRange(-1.5, -.75, .75, 1.5, 13, 5, 1));
            _matrix[6].Add(new CellRange(-1.5, -.75, 1.5, 5, 14, 6, 1));

            _matrix[0].Add(new CellRange(-.75, -.25, -5, -1.5, 15, 0, 2));
            _matrix[1].Add(new CellRange(-.75, -.25, -1.5, -.75, 16, 1, 2));
            _matrix[2].Add(new CellRange(-.75, -.25, -.75, -.25, 17, 2, 2));
            _matrix[3].Add(new CellRange(-.75, -.25, -.25, .25, 18, 3, 2));
            _matrix[4].Add(new CellRange(-.75, -.25, .25, .75, 19, 4, 2));
            _matrix[5].Add(new CellRange(-.75, -.25, .75, 1.5, 20, 5, 2));
            _matrix[6].Add(new CellRange(-.75, -.25, 1.5, 5, 21, 6, 2));

            _matrix[0].Add(new CellRange(-.25, .25, -5, -1.5, 22, 0, 3));
            _matrix[1].Add(new CellRange(-.25, .25, -1.5, -.75, 23, 1, 3));
            _matrix[2].Add(new CellRange(-.25, .25, -.75, -.25, 24, 2, 3));
            _matrix[3].Add(new CellRange(-.25, .25, -.25, .25, 25, 3, 3));
            _matrix[4].Add(new CellRange(-.25, .25, .25, .75, 26, 4, 3));
            _matrix[5].Add(new CellRange(-.25, .25, .75, 1.5, 27, 5, 3));
            _matrix[6].Add(new CellRange(-.25, .25, 1.5, 5, 28, 6, 3));

            _matrix[0].Add(new CellRange(.25,.75,  -5, -1.5, 29, 0, 4));
            _matrix[1].Add(new CellRange(.25,.75,  -1.5, -.75, 30, 1, 4));
            _matrix[2].Add(new CellRange(.25,.75,  -.75, -.25, 31, 2, 4));
            _matrix[3].Add(new CellRange(.25,.75,  -.25, .25, 32, 3, 4));
            _matrix[4].Add(new CellRange(.25,.75,  .25, .75, 33, 4, 4));
            _matrix[5].Add(new CellRange(.25,.75,  .75, 1.5, 34, 5, 4));
            _matrix[6].Add(new CellRange(.25,.75,  1.5, 5, 35, 6, 4));

            _matrix[0].Add(new CellRange(.75, 1.5, -5, -1.5, 36, 0, 5));
            _matrix[1].Add(new CellRange( .75, 1.5, -1.5, -.75, 37, 1, 5));
            _matrix[2].Add(new CellRange( .75, 1.5, -.75, -.25, 38, 2, 5));
            _matrix[3].Add(new CellRange( .75, 1.5, -.25, .25, 39, 3, 5));
            _matrix[4].Add(new CellRange( .75, 1.5, .25, .75, 40, 4, 5));
            _matrix[5].Add(new CellRange(.75, 1.5,  .75, 1.5, 41, 5, 5));
            _matrix[6].Add(new CellRange(.75, 1.5,  1.5, 5, 42, 6, 5));

            _matrix[0].Add(new CellRange(1.5, 5, -5, -1.5, 43, 0, 6));
            _matrix[1].Add(new CellRange(1.5, 5, -1.5, -.75, 44, 1, 6));
            _matrix[2].Add(new CellRange(1.5, 5, -.75, -.25, 45, 2, 6));
            _matrix[3].Add(new CellRange(1.5, 5, -.25, .25, 46, 3, 6));
            _matrix[4].Add(new CellRange(1.5, 5, .25, .75, 47, 4, 6));
            _matrix[5].Add(new CellRange(1.5, 5, .75, 1.5, 48, 5, 6));
            _matrix[6].Add(new CellRange(1.5, 5, 1.5, 5, 49, 6, 6));
        }
        public void LoadCpeMatrix(String formattedString)
        {
            DemoLoad();
            return;
            
            _matrix = new List<List<CellRange>>();
            //parse string, looks like: [[{-5,1.5},{-1.5,-.75},{-.75.-.25},{-.25,.25},{.25,.75},{.75,1.5},{1.5,5}],[{-5,1.5},{-1.5,-.75},{-.75.-.25},{-.25,.25},{.25,.75},{.75,1.5},{1.5,5}]] => Defines a 7x7 
            // [ DIM 1 Array, DIM 2 Array]

            //as insert, keep x,y up to date


            //after inserted traverse whole matrix and number the cells, from left-to-right, top-to-bottom
        }

        public CellRange GetCellById(int id)
        {
            for (int x = 0; x < _matrix.Count; x++)
            {
                for (int y = 0; y < _matrix[x].Count; y++)
                {
                    if (_matrix[x][y].CellNumber == id)
                        return _matrix[x][y].Copy();
                }
            }

            return null;
        }

        public CellRange GetCellByIndex(int dim1index, int dim2index)
        {
            if (_matrix.Count > dim2index)
            {
                if (_matrix[dim2index].Count > dim1index)
                {
                    return _matrix[dim2index][dim1index].Copy();
                }
            }
            return null;
        }

        public CellRange GetCellByCpe(double cpe1, double cpe2) //wiggle room is how many cells you can be off
        {
            for (int x = 0; x < _matrix.Count; x++)
            {
                for (int y = 0; y < _matrix[x].Count; y++)
                {
                    if (_matrix[x][y].Dimension1End >= cpe1 && _matrix[x][y].Dimension1Start <= cpe1 &&
                        _matrix[x][y].Dimension2End >= cpe2 && _matrix[x][y].Dimension2Start <= cpe2)
                        return GetCellByIndex(y,x);// _matrix[x][y].Copy();
                }
            }
            return null;
        }
        public List<CellRange> GetCellsByIndex(int dim1index, int dim2index, int wiggleRoom) //wiggle room is how many cells you can be off
        {
            List<CellRange> cells = new List<CellRange>();
            CellRange cr = null;
            for (int x = Math.Max(dim1index - wiggleRoom, 0); x <= Math.Min(_matrix.Count, dim1index + wiggleRoom); x++)
            {
                for (int y = Math.Max(dim2index - wiggleRoom, 0); y <= Math.Min(_matrix.Count, dim2index + wiggleRoom); y++)//int y = 0; y < _matrix[x].Count; y++)
                {
                    cr = GetCellByIndex(y, x);
                    if(cr != null)
                        cells.Add(cr);// _matrix[x][y].Copy();
                }
            }
            if (cells.Count == 0)
                return null;
            return cells;
        }

        public List<CellRange> GetCellsByCpe(double cpe1, double cpe2, int wiggleRoom) //wiggle room is how many cells you can be off
        {
            List<CellRange> cells = new List<CellRange>();
            CellRange center = GetCellByCpe(cpe1, cpe2);
            for (int x = Math.Max(center.Xindex-wiggleRoom,0); x <= Math.Min(_matrix.Count,center.Xindex+wiggleRoom); x++)
            {
                for (int y = Math.Max(center.Yindex-wiggleRoom,0); y <= Math.Min(_matrix.Count,center.Yindex+wiggleRoom); y++)//int y = 0; y < _matrix[x].Count; y++)
                {
                   // if (_matrix[x][y].Dimension1End >= cpe1 && _matrix[x][y].Dimension1Start <= cpe1 &&
                    //    _matrix[x][y].Dimension2End >= cpe2 && _matrix[x][y].Dimension2Start <= cpe2)
                        cells.Add(GetCellByIndex(y, x));// _matrix[x][y].Copy();
                }
            }
            if(cells.Count==0)
                return null;

            return cells;
        }

        public int GetColumnCount()
        {
            return _matrix.Count;
        }
        public int GetRowCount()
        {
            return _matrix[0].Count;
        }
    }
}
