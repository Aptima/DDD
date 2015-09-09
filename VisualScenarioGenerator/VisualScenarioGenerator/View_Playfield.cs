using System;
using System.Collections.Generic;
using System.Text;

using VisualScenarioGenerator.VSGPanes;
using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    public class View_Playfield: View
    {

        public View_Playfield(): base(new NavP_Playfield(), new CntP_Playfield() )
        {
        }
        public View_Playfield(CntP_Playfield playfield)
            : base(new NavP_Playfield(), playfield)
        {
        }

        public new void Hide()
        {
            _navigation_panel.Hide();
            _content_panel.Hide();
            ((CntP_Playfield)_content_panel).ShowPlayfield(false);
        }


        public new void Show()
        {
            Console.WriteLine("Show Playfield Scene");
            _navigation_panel.Show();
            _content_panel.Show();
                CntP_Playfield playfield = (CntP_Playfield)_content_panel;
                playfield.ChangeMode(ContentPanelMode.Playfield);
                playfield.ShowPlayfield(true);

        }

        public override void UpdateView(object object_data)
        {
            ScenarioResourcesDataStruct s = (ScenarioResourcesDataStruct)object_data;
            CntP_Playfield playfield = (CntP_Playfield)_content_panel;
            playfield.SetScene(s.IconLibrary, s.MapFile, s.Map);
        }

    }

    /* *****************************************************************************************
     * View Data Structures
     * *****************************************************************************************/
    public struct NewAssetInstanceDataStruct : ICloneable
    {
        public string ID;
        public static NewAssetInstanceDataStruct Empty = new NewAssetInstanceDataStruct();

        #region ICloneable Members

        public object Clone()
        {
            NewAssetInstanceDataStruct obj = new NewAssetInstanceDataStruct();
            obj.ID = ID;
            return obj;
        }

        #endregion
    }
    public struct DeleteAssetInstanceDataStruct : ICloneable
    {
        public string ID;
        public static DeleteAssetInstanceDataStruct Empty = new DeleteAssetInstanceDataStruct();

        #region ICloneable Members

        public object Clone()
        {
            DeleteAssetInstanceDataStruct obj = new DeleteAssetInstanceDataStruct();
            obj.ID = ID;
            return obj;
        }

        #endregion
    }
    public struct SelectAssetInstanceDataStruct : ICloneable
    {
        public string ID;
        public static SelectAssetInstanceDataStruct Empty = new SelectAssetInstanceDataStruct();

        #region ICloneable Members

        public object Clone()
        {
            SelectAssetInstanceDataStruct obj = new SelectAssetInstanceDataStruct();
            obj.ID = ID;
            return obj;
        }

        #endregion
    }

}
