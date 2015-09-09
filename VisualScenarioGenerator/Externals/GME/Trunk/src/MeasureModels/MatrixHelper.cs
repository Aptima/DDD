using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace AME.MeasureModels
{
    public class MatrixHelper
    {

        public static double Sum(Matrix m)
        {
            double dResult = 0;

            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    dResult += m[i, j];
                }
            }

            return dResult;
        }//Sum


    }
}
