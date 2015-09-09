using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Views.View_Components
{
    public interface IViewComponentHelper
    {
        void IViewHelperUpdateViewComponent(UpdateType typeOfUpdate);

        UpdateType IViewHelperUpdateType { get; set; }

        Boolean IViewHelperVisible { get; }

        Boolean NeedsUpdate { get; set; }

        List<String> ParameterCategories { get; set; }

        UpdateType LatestEventFromController { get; set; }

        Boolean InvokeRequired { get; }

        void InvokeUpdate(UpdateType typeOfUpdate);
    }
}
