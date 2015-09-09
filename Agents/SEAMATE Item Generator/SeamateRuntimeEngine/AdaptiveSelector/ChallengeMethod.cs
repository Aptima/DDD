using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class ChallengeMethod : BaseMethod
    {
        public const String Name = "Challenge";
        #region ISelectionMethod Members

        public override CellRange SelectNextCell(CpeMatrix matrix, double cpe1, double cpe2, List<int> ignoreCells, int failedAttempts)
        {
            CellRange current = matrix.GetCellByCpe(cpe1, cpe2);
            int x = current.Xindex;
            int y = current.Yindex;
            List<CellRange> possibleCells = new List<CellRange>();
            if (Math.Abs(cpe1 - cpe2) >= Threshold)
            {
                if (cpe1 > cpe2)
                {
                    //focus on CPE2
                    x = current.Xindex;
                    y = current.Yindex;
                    x = Math.Min(x + 1, matrix.GetColumnCount() - 1);
                    
                }
                else
                {
                    //focus on CPE1
                    x = current.Xindex;
                    y = current.Yindex;
                    y = Math.Min(y + 1, matrix.GetRowCount() - 1);
                    
                }
            }
            else
            {
                //focus on both
                x = current.Xindex;
                y = current.Yindex;
                x = Math.Min(x + 1, matrix.GetColumnCount() - 1);
                y = Math.Min(y + 1, matrix.GetRowCount() - 1);
                
            }


            if (ignoreCells != null)
            {
                possibleCells = matrix.GetCellsByIndex(y, x, failedAttempts);
                foreach (CellRange cr in possibleCells)
                {
                    if (!ignoreCells.Contains(cr.CellNumber))
                        return cr.Copy();
                }
                return null;
            }
            return matrix.GetCellByIndex(y, x);
            /*
            //then move appropriately
            if (Math.Abs(cpe1 - cpe2) >= Threshold)
            {
                if (cpe1 > cpe2)
                {
                    //focus on CPE2
                    int x = current.Xindex;
                    int y = current.Yindex;
                    x = Math.Min(x + 1, matrix.GetColumnCount() - 1);
                    return matrix.GetCellByIndex(y, x);
                }
                else
                {
                    //focus on CPE1
                    int x = current.Xindex;
                    int y = current.Yindex;
                    y = Math.Min(y + 1, matrix.GetRowCount() - 1);
                    return matrix.GetCellByIndex(y, x);
                }
            }
            else
            { 
                //focus on both
                int x = current.Xindex;
                int y = current.Yindex;
                x = Math.Min(x + 1, matrix.GetColumnCount()-1);
                y = Math.Min(y + 1, matrix.GetRowCount()-1);
                return matrix.GetCellByIndex(y, x);
            }
            return null;*/
        }

        public override String GetGoalName()
        {
            return Name;
        }
        #endregion
    }
}
