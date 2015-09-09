using System;
using System.Collections.Generic;
using System.Text;
using VisualScenarioGenerator.VSGPanes;

namespace VisualScenarioGenerator
{
    public class View_Preview:View
    {
        public View_Preview()
            : base(new NavP_Preview(), new CntP_Playfield())
        {
        }
        public View_Preview(CntP_Playfield playfield)
            : base(new NavP_Preview(), playfield)
        {
        }

        //public new void Hide()
        //{
        //    _navigation_panel.Hide();
        //    _content_panel.Hide();
        //    ((CntP_Preview)_content_panel).ShowPlayfield(false);
        //}
        //public new void Show()
        //{
        //    _navigation_panel.Show();
        //    _content_panel.Show();
        //    ((CntP_Preview)_content_panel).ShowPlayfield(true);
        //}
        public new void Hide()
        {
            _navigation_panel.Hide();
            _content_panel.Hide();
            ((CntP_Playfield)_content_panel).ShowPlayfield(false);
        }

        public void UpdateScene(string icon_library, string map_file, System.Drawing.Bitmap map)
        {
            Console.WriteLine("Update Scene");
            CntP_Playfield playfield = (CntP_Playfield)_content_panel;
            playfield.SetScene(icon_library, map_file, map);

        }

        public new void Show()
        {
            Console.WriteLine("Show Preview Scene");
            _navigation_panel.Show();
            _content_panel.Show();
            CntP_Playfield playfield = (CntP_Playfield)_content_panel;
            playfield.ChangeMode(ContentPanelMode.Preview);
            playfield.ShowPlayfield(true);

        }

    }
}
