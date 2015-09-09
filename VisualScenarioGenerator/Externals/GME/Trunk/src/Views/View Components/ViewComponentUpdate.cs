using System;
using System.Collections.Generic;
using System.Text;
using Controllers;

namespace View_Components
{
    // 'custom' .net controls must implement this
    // to update themselves, using a controller reference
    public interface ViewComponentUpdate
    {
        IDataEntryController Controller { get; set; }

        void UpdateViewComponent();
    }
}
