using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public abstract class BaseMethod : ISelectionMethod
    {
        public static double Threshold = 0.50;

        public virtual CellRange SelectNextCell(CpeMatrix matrix, double cpe1, double cpe2, List<int> ignoreCells, int failedAttempts)
        {
            
            throw new NotImplementedException();
        }

        public virtual String GetGoalName()
        {
            return "";
        }
    }
}
