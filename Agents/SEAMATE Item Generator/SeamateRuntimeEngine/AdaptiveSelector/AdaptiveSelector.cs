using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaptiveSelector
{
    public class AdaptiveSelector
    {
        private ISelectionMethod _currentGoal;
        private ISelectionMethod _selectCurrentCpe;
        private CpeMatrix _templateMatrix;

        public AdaptiveSelector(ISelectionMethod initialGoal, String formattedMatrixString)
        {
            _templateMatrix = new CpeMatrix();
            _templateMatrix.LoadCpeMatrix(formattedMatrixString);
            _currentGoal = initialGoal;
            _selectCurrentCpe = new MaintainMethod();
        }

        public CellRange GetNextItem(double cpe1, double cpe2, List<int> ignoreList, int failedAttempts)
        {
            return _currentGoal.SelectNextCell(_templateMatrix, cpe1, cpe2, ignoreList,failedAttempts);
        }
        public CellRange GetNextItem(double cpe1, double cpe2)
        {
            return _currentGoal.SelectNextCell(_templateMatrix, cpe1, cpe2, null,0);
        }

        public CellRange GetCurrentCpeItem(double cpe1, double cpe2)
        {
            return _selectCurrentCpe.SelectNextCell(_templateMatrix, cpe1, cpe2, null,0);
        }

        public CpeMatrix GetMatrix()
        {
            return _templateMatrix;
        }

        public void SetGoal(ISelectionMethod newGoal)
        {
            _currentGoal = newGoal;
        }
    }
}
