using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;

namespace VisualScenarioGenerator
{
    public enum ViewType:int  {Scenario, Playfield, Types, Scoring, Timeline, Preview};

    public partial class VSG_Panel : UserControl
    {
        private ViewType _view = ViewType.Scenario;
        public ViewType View
        {
            get
            {
                return _view;
            }
        }
        private static SortedDictionary<ViewType, View> _Views = new SortedDictionary<ViewType, View>();
        public static SortedDictionary<ViewType, View> Views
        {
            get
            {
                return _Views;
            }
        }

        public VSG_Panel()
        {
            InitializeComponent();
            InitializeViews();
        }

        private void InitializeViews()
        {
            _Views.Add(ViewType.Scenario, new View_Scenario());
            _Views[ViewType.Scenario].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Scenario].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            CntP_Playfield playfield = new CntP_Playfield();

            _Views.Add(ViewType.Playfield, new View_Playfield(playfield));
            _Views[ViewType.Playfield].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Playfield].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            _Views.Add(ViewType.Preview, new View_Preview(playfield));
            _Views[ViewType.Preview].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Preview].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            _Views.Add(ViewType.Scoring, new View_Scoring());
            _Views[ViewType.Scoring].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Scoring].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            _Views.Add(ViewType.Timeline, new View_Timeline(playfield));
            _Views[ViewType.Timeline].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Timeline].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            _Views.Add(ViewType.Types, new View_ObjectTypes());
            _Views[ViewType.Types].BindNavigatorControl(Navigator_SplitContainer.Panel2, DockStyle.Fill, false);
            _Views[ViewType.Types].BindViewControl(VSG_SplitContainer.Panel2, DockStyle.Fill, false);

            TopLevel_Navigator.SelectedIndices.Add(0);
        }

        public void Show_SceneView()
        {
            // Don't show a navigation control when scenario view is up.
            Navigator_SplitContainer.Panel2Collapsed = true;

            _Views[ViewType.Types].Hide();
            ((View_Timeline)_Views[ViewType.Timeline]).Hide();
            _Views[ViewType.Scoring].Hide();
            _Views[ViewType.Preview].Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Hide();
            _Views[ViewType.Scenario].Show();
        }
        public void Show_PlayfieldView()
        {
            Navigator_SplitContainer.Panel2Collapsed = false;

           _Views[ViewType.Types].Hide();
           ((View_Timeline)_Views[ViewType.Timeline]).Hide();
           _Views[ViewType.Scoring].Hide();
           ((View_Preview)_Views[ViewType.Preview]).Hide();
           _Views[ViewType.Scenario].Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Show();
        }
        public void Show_PreviewView()
        {
            Navigator_SplitContainer.Panel2Collapsed = false;

            _Views[ViewType.Types].Hide();
            ((View_Timeline)_Views[ViewType.Timeline]).Hide();
            _Views[ViewType.Scoring].Hide();
            _Views[ViewType.Scenario].Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Hide();
            ((View_Preview)_Views[ViewType.Preview]).Show();
        }
        public void Show_ScoreView()
        {
            Navigator_SplitContainer.Panel2Collapsed = false;

            _Views[ViewType.Types].Hide();
            ((View_Timeline)_Views[ViewType.Timeline]).Hide();
            ((View_Preview)_Views[ViewType.Preview]).Hide();
            _Views[ViewType.Scenario].Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Hide();
            _Views[ViewType.Scoring].Show();
        }
        public void Show_TimelineView()
        {
            Navigator_SplitContainer.Panel2Collapsed = false;

            _Views[ViewType.Types].Hide();
            _Views[ViewType.Scoring].Hide();
            ((View_Preview)_Views[ViewType.Preview]).Hide();
            _Views[ViewType.Scenario].Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Hide();
            ((View_Timeline)_Views[ViewType.Timeline]).Show();

        }
        public void Show_TypesView()
        {
            Navigator_SplitContainer.Panel2Collapsed = false;

            ((View_Timeline)_Views[ViewType.Timeline]).Hide();
            ((View_Playfield)_Views[ViewType.Playfield]).Hide();
            _Views[ViewType.Scoring].Hide();
            ((View_Preview)_Views[ViewType.Preview]).Hide();
            _Views[ViewType.Scenario].Hide();
            _Views[ViewType.Types].Show();
        }

        private void TopLevel_Navigator_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
                return;

            //System.Console.WriteLine("idx = {0}, {1}", e.ItemIndex, sender.ToString());

            switch (e.ItemIndex)
            {
                case 0:
                    // Do ScenarioLevel
                    Show_SceneView();
                    _view = ViewType.Scenario;
                    break;
                case 1:
                    // Do Playfield Level
                    Show_PlayfieldView();
                    _view = ViewType.Playfield;
                    break;
                case 2:
                    // Do Types Level
                    Show_TypesView();
                    _view = ViewType.Types;
                    break;
                
                case 3:
                    // Do Timeline Level
                    Show_TimelineView();
                    _view = ViewType.Timeline;
                    break;
                case 4:
                    // Do Scoring Level
                    Show_ScoreView();
                    _view = ViewType.Scoring;
                    break;
                case 5:
                    // Do Preview Level
                    Show_PreviewView();
                    _view = ViewType.Preview;
                    break;
            }
            if (Parent is IVSGForm)
            {
                ((IVSGForm)Parent).VSG_ViewChange(_view);
            }
        }

        private void TopLevel_Navigator_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void VSG_Panel_Load(object sender, EventArgs e)
        {
            VSG_SplitContainer.Panel1MinSize = 220;
            //VSG_SplitContainer.Panel2MinSize = 518;
        }

    }
}
