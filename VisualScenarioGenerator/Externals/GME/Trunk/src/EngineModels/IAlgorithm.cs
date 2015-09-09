using System;
using System.Collections.Generic;
using System.Text;

namespace AME.EngineModels
{
    public interface IAlgorithm<I, O>
    {
        O Compute(I input);
    }
}
