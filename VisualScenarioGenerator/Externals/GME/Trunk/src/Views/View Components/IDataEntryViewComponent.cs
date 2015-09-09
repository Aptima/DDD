using System;
using System.Collections.Generic;
using System.Text;
using GME.Controllers;

namespace GME.Views.View_Components
{
    // 'custom' .net controls must implement this
    // to update themselves, using a controller reference
    public interface IDataEntryViewComponent 
    {
        IDataEntryController Controller { get; set; }

        void UpdateViewComponent();
    }
}
