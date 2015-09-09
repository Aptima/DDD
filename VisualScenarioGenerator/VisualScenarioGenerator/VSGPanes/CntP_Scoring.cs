using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator.VSGPanes
{
    public partial class CntP_Scoring : Ctl_ContentPane
    {
        private Control _selected_control = null;

        public CntP_Scoring()
        {
            InitializeComponent();

            //this.ctl_Actor1.Hide();
            //this.ctl_Actor1.ContentPane = this;
            //ctl_Actor1.Dock = DockStyle.Fill;

            //this.ctl_Location1.Hide();
            //this.ctl_Location1.ContentPane = this;
            //ctl_Location1.Dock = DockStyle.Fill;

            this.ctl_ScoringRule1.Hide();
            this.ctl_ScoringRule1.ContentPane = this;
            ctl_ScoringRule1.Dock = DockStyle.Fill;

            //this.ctl_ExistenceRule1.Hide();
            //this.ctl_ExistenceRule1.ContentPane = this;
            //ctl_ExistenceRule1.Dock = DockStyle.Fill;

            this.ctl_Score1.Hide();
            this.ctl_Score1.ContentPane = this;
            ctl_Score1.Dock = DockStyle.Fill;

        }

        public void ShowPanel(Scoring_NodeTypes type)
        {
            switch (type)
            {
                //case Scoring_NodeTypes.Actors:
                //    if (_selected_control != ctl_Actor1)
                //    {
                //        if (_selected_control != null)
                //        {
                //            _selected_control.Hide();
                //        }
                //        _selected_control = ctl_Actor1;
                //        _selected_control.Show();
                //    }
                //    break;
                //case Scoring_NodeTypes.Location:
                //    if (_selected_control != ctl_Location1)
                //    {
                //        if (_selected_control != null)
                //        {
                //            _selected_control.Hide();
                //        }
                //        _selected_control = ctl_Location1;
                //        _selected_control.Show();
                //    }
                //    break;
                case Scoring_NodeTypes.Scoring_Rule:
                    if (_selected_control != ctl_ScoringRule1)
                    {
                        if (_selected_control != null)
                        {
                            _selected_control.Hide();
                        }
                        _selected_control = ctl_ScoringRule1;
                        _selected_control.Show();
                    }
                    break;
                //case Scoring_NodeTypes.Existence_Rule:
                //    if (_selected_control != ctl_ExistenceRule1)
                //    {
                //        if (_selected_control != null)
                //        {
                //            _selected_control.Hide();
                //        }
                //        _selected_control = ctl_ExistenceRule1;
                //        _selected_control.Show();
                //    }
                //    break;
                case Scoring_NodeTypes.Score:
                    if (_selected_control != ctl_Score1)
                    {
                        if (_selected_control != null)
                        {
                            _selected_control.Hide();
                        }
                        _selected_control = ctl_Score1;
                        _selected_control.Show();
                    }
                    break;

            }
        }

        public override void Update(object object_data)
        {
            //if (object_data is ActorDataStruct)
            //{
            //    ctl_Actor1.Update(object_data);
            //}
            //if (object_data is LocationDataStruct)
            //{
            //    ctl_Location1.Update(object_data);
            //}
            if (object_data is ScoringRuleDataStruct)
            {
                ctl_ScoringRule1.Update(object_data);
            }
            //if (object_data is ExistenceRuleDataStruct)
            //{
            //    ctl_ExistenceRule1.Update(object_data);
            //}
            if (object_data is ScoreDataStruct)
            {
                ctl_Score1.Update(object_data);
            }
        }
    }
}
