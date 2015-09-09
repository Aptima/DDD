using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class ConsolidateMethod : BaseMethod
    {
        public const String Name = "Consolidate";
        #region ISelectionMethod Members

        public override CellRange SelectNextCell(CpeMatrix matrix, double cpe1, double cpe2, List<int> ignoreCells, int failedAttempts)
        {
            CellRange current;
            if (ignoreCells != null)
            {
                current = matrix.GetCellByCpe(cpe1, cpe2);
                List<CellRange> possibleCells;// = matrix.GetCellsByCpe(cpe1, cpe2, failedAttempts);
                if (Math.Abs(cpe1 - cpe2) >= Threshold)
                {
                    if (cpe1 > cpe2)
                    {
                        //focus on CPE2
                        int x = current.Xindex;
                        int y = current.Yindex;
                        x = Math.Max(x - 1, 0);
                        possibleCells = matrix.GetCellsByIndex(y, x, failedAttempts);
                    }
                    else
                    {
                        //focus on CPE1
                        int x = current.Xindex;
                        int y = current.Yindex;
                        y = Math.Max(y - 1, 0);
                        possibleCells = matrix.GetCellsByIndex(y, x, failedAttempts);
                    }
                }
                else
                {
                    //focus on both
                    int x = current.Xindex;
                    int y = current.Yindex;
                    x = Math.Max(x - 1, 0);
                    y = Math.Max(y - 1, 0);
                    possibleCells = matrix.GetCellsByIndex(y, x, failedAttempts);
                }
                //go through each possible cell looking for those not already ignored.
                foreach (CellRange cr in possibleCells)
                {
                    if (!ignoreCells.Contains(cr.CellNumber))
                        return cr.Copy();
                }
                return null;
            }
            current = matrix.GetCellByCpe(cpe1, cpe2);
            //then move appropriately
            if (Math.Abs(cpe1 - cpe2) >= Threshold)
            {
                if (cpe1 > cpe2)
                {
                    //focus on CPE2
                    int x = current.Xindex;
                    int y = current.Yindex;
                    x = Math.Max(x - 1, 0);
                    return matrix.GetCellByIndex(y, x);
                }
                else
                {
                    //focus on CPE1
                    int x = current.Xindex;
                    int y = current.Yindex;
                    y = Math.Max(y - 1, 0);
                    return matrix.GetCellByIndex(y, x);
                }
            }
            else
            { 
                //focus on both
                int x = current.Xindex;
                int y = current.Yindex;
                x = Math.Max(x - 1, 0);
                y = Math.Max(y - 1, 0);
                return matrix.GetCellByIndex(y, x);
            }
            return null;
        }

        public override String GetGoalName()
        {
            return Name;
        }
        #endregion
    }
}
