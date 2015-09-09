using System;
using System.Collections.Generic;
using System.Text;
using AME.Adapters;
using AME.Controllers;
using System.Windows.Forms;

namespace AME.Tools
{
    public interface IImportTool
    {
        Int32 RootId { get; }
        Boolean PutTypeInName { get; set; }
        Boolean PutNameInDescription { get; set; }
        //Controller Controller { set; }
        //RootController RootController { set; }
        Boolean Import(IController controller, IImportAdapter adapter, String filename, Form topLevelForm, Boolean clearDB);
    }
}
