using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public interface ISelectionMethod
    {
        
         CellRange SelectNextCell(CpeMatrix matrix, double cpe1, double cpe2, List<int> ignoreList, int failedAttempts);
        String GetGoalName();
    }
}
