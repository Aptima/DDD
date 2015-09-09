using System;
using System.Collections.Generic;
using System.Text;
using AME.Views.View_Components;

namespace AME.Views.View_Component_Packages
{
    public interface IViewComponentPanel
    {
        IViewComponentHelper IViewHelper { get; }

        void UpdateViewComponent();
    }
}
