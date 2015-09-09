using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Views.Assessment
{
    public interface IAssessmentView
    {
        IAssessmentViewHelper Helper { get; }
        void Populate();
    }
}
