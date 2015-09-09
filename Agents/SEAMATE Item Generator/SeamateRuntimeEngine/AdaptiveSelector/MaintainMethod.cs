using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class MaintainMethod : BaseMethod
    {
        public const String Name = "Maintain";
        #region ISelectionMethod Members

        public override CellRange SelectNextCell(CpeMatrix matrix, double cpe1, double cpe2, List<int> ignoreCells, int failedAttempts)
        {
            if (ignoreCells != null)
            {
                
                List<CellRange> possibleCells = matrix.GetCellsByCpe(cpe1, cpe2, failedAttempts);
                //go through each possible cell looking for those not already ignored.
                foreach (CellRange cr in possibleCells)
                {
                    if (!ignoreCells.Contains(cr.CellNumber))
                        return cr.Copy();
                }
                return null;
            }
            CellRange current = matrix.GetCellByCpe(cpe1, cpe2);
            return current; //because we're maintaining, keep in their cpe cell
        }

        public override String GetGoalName()
        {
            return Name;
        }
        #endregion
    }
}
