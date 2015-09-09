using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;

namespace AME.Views.View_Components
{
    // 'custom' .net controls must implement this
    // to update themselves, using a controller reference
    public interface IViewComponent
    {
        IController Controller { get; set; }

        void UpdateViewComponent();

        IViewComponentHelper IViewHelper { get; }
    }
}
