using System;
using System.Collections.Generic;
using System.Text;
using VisualScenarioGenerator.VSGPanes;

using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    public class View_Scenario : View 
    {
        public View_Scenario()
            : base(new NavP_Scenario(), new CntP_Scenario())
        {
        }


        // There is no content panel for the Scenario View.
    
        public override void  Notify(object object_data)
        {
            VSG_Panel.Views[ViewType.Playfield].UpdateView(object_data);
        }
    }
}
