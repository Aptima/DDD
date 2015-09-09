using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;
using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    public class View_Timeline: View
    {
        public string CurrentAssetID = string.Empty;
        

        public View_Timeline()
            : base(new NavP_Timeline(), new CntP_Playfield())

        {
        }
        public View_Timeline(CntP_Playfield playfield)
            : base(new NavP_Timeline(), playfield)
        {
        }


        public NavP_Timeline GetNavigationPanel()
        {
            return ((NavP_Timeline)NavigatorPanel);
        }

        public new void Hide()
        {
            _navigation_panel.Hide();
            _content_panel.Hide();
            ((CntP_Playfield)_content_panel).ShowPlayfield(false);
        }

        public void UpdateScene(string icon_library, string map_file, System.Drawing.Bitmap map)
        {
            CntP_Playfield playfield = (CntP_Playfield)_content_panel;
            playfield.SetScene(icon_library, map_file, map);

        }

        public new void Show()
        {
            _navigation_panel.Show();
            _content_panel.Show();
                CntP_Playfield playfield = (CntP_Playfield)_content_panel;
                playfield.ChangeMode(ContentPanelMode.Timeline);
                playfield.ShowPlayfield(true);

        }

        public override void UpdateContentPanel(object object_data)
        {
            Console.WriteLine("Update Content Panel");
            _content_panel.Update(object_data);
        }
        public override void UpdateNavigatorPanel(Control control, object object_data)
        {
            Console.WriteLine("Update Navigation Panel");
            _navigation_panel.Update(object_data);
        }

        public override void UpdateView(object object_data)
        {
            // Receiving ObjectTypes Updates.
            if (object_data is StructObjectTypes)
            {
                _navigation_panel.Update(object_data);
            }
        }

    }





    //public struct LocationDataStruct : ICloneable
    //{
    //    public string ID;
    //    public static LocationDataStruct Empty = new LocationDataStruct();

    //    #region ICloneable Members

    //    public object Clone()
    //    {
    //        LocationDataStruct obj = new LocationDataStruct();
    //        obj.ID = ID;
    //        return obj;
    //    }

    //    #endregion
    //}

}
