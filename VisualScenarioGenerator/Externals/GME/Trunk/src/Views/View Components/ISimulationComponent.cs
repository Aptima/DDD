using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;

namespace AME.Views.View_Components
{
    // 'custom' .net controls must implement this
    // to update themselves, using a controller reference
    public interface ISimulationComponent
    {
        ModelingController Controller { get; set; }

        void UpdateViewComponent();
    }    
}
