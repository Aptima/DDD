using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VSG.Libraries;
using VSG.Dialogs;

using AME.Views.View_Components;
using AME.Controllers;
using VSG.Controllers;
namespace VSG.ViewComponents
{
    public partial class ScoringRulePanel : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        //private ScoringRuleDataStruct _datastore = ScoringRuleDataStruct.Empty;
        private Int32 scoreRuleID = -1;
        private IController controller;
        public ScoringRulePanel()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public void UpdateViewComponent()
        {
            VSGController myController = (VSGController)Controller;
            List<int> ids;
            //return;
            if (scoreRuleID >= 0)
            {
                string name = myController.GetComponentName(scoreRuleID);
                tabPage.Description = name;

                scoreIncrementTextBox1.UpdateViewComponent();

                ids = myController.GetChildIDs(scoreRuleID, "Actor", "RuleUnit");
                if (ids.Count >= 1)
                {
                    ob1WhoEnumBox.EnumName = "WhoEnum";
                    ob1WhoEnumBox.ComponentId = ids[0];
                    ob1WhoEnumBox.ParameterCategory = "Actor";
                    ob1WhoEnumBox.ParameterName = "Who";
                    ob1WhoEnumBox.UpdateViewComponent();

                    ob1WhatEnumBox.EnumName = "WhatEnum";
                    ob1WhatEnumBox.ComponentId = ids[0];
                    ob1WhatEnumBox.ParameterCategory = "Actor";
                    ob1WhatEnumBox.ParameterName = "What";
                    ob1WhatEnumBox.EnumName = "WhatEnum";
                    ob1WhatEnumBox.UpdateViewComponent();

                    if (ob1WhatEnumBox.SelectedItem != null)
                    {
                        switch ((Config.Parameters.Actor.WhatEnum)ob1WhatEnumBox.SelectedItem)
                        {
                            case Config.Parameters.Actor.WhatEnum.Any:
                                ob1WhatLinkBox.DisplayRootId = -1;
                                ob1WhatLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhatEnum.Of_Species:
                                ob1WhatLinkBox.DisplayRootId = myController.ScenarioId;
                                ob1WhatLinkBox.DisplayComponentType = "Species";
                                ob1WhatLinkBox.DisplayLinkType = "Scenario";
                                ob1WhatLinkBox.ConnectFromId = ids[0];
                                ob1WhatLinkBox.ConnectRootId = ids[0];
                                ob1WhatLinkBox.ConnectLinkType = "ActorKind";
                                ob1WhatLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhatEnum.Unit:
                                ob1WhatLinkBox.DisplayRootId = myController.ScenarioId;
                                ob1WhatLinkBox.DisplayComponentType = "CreateEvent";
                                ob1WhatLinkBox.DisplayLinkType = "Scenario";
                                ob1WhatLinkBox.ConnectFromId = ids[0];
                                ob1WhatLinkBox.ConnectRootId = ids[0];
                                ob1WhatLinkBox.ConnectLinkType = "ActorUnit";
                                ob1WhatLinkBox.UpdateViewComponent();
                                break;

                        }
                    }

                    ob1WhereEnumBox.EnumName = "WhereEnum";
                    ob1WhereEnumBox.ComponentId = ids[0];
                    ob1WhereEnumBox.ParameterCategory = "Actor";
                    ob1WhereEnumBox.ParameterName = "Where";
                    ob1WhereEnumBox.UpdateViewComponent();

                    if (ob1WhereEnumBox.SelectedItem != null)
                    {
                        switch ((Config.Parameters.Actor.WhereEnum)ob1WhereEnumBox.SelectedItem)
                        {
                            case Config.Parameters.Actor.WhereEnum.Anywhere:
                                ob1WhereLinkBox.DisplayRootId = -1;

                                ob1WhereLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhereEnum.In_Region:
                                ob1WhereLinkBox.DisplayRootId = myController.ScenarioId;
                                ob1WhereLinkBox.DisplayComponentType = "ActiveRegion";
                                ob1WhereLinkBox.DisplayLinkType = "Scenario";
                                ob1WhereLinkBox.ConnectFromId = ids[0];
                                ob1WhereLinkBox.ConnectRootId = ids[0];
                                ob1WhereLinkBox.ConnectLinkType = "ActorRegion";
                                ob1WhereLinkBox.OneToMany = true;
                                ob1WhereLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhereEnum.Not_In_Region:
                                ob1WhereLinkBox.DisplayRootId = myController.ScenarioId;
                                ob1WhereLinkBox.DisplayComponentType = "ActiveRegion";
                                ob1WhereLinkBox.DisplayLinkType = "Scenario";
                                ob1WhereLinkBox.ConnectFromId = ids[0];
                                ob1WhereLinkBox.ConnectRootId = ids[0];
                                ob1WhereLinkBox.ConnectLinkType = "ActorRegion";
                                ob1WhereLinkBox.OneToMany = true;
                                ob1WhereLinkBox.UpdateViewComponent();
                                break;

                        }
                    }

                }

                ruleTypeEnumBox.EnumName = "RuleTypes";
                ruleTypeEnumBox.ComponentId = ScoreRuleID;
                ruleTypeEnumBox.ParameterCategory = "Rule";
                ruleTypeEnumBox.ParameterName = "RuleType";
                ruleTypeEnumBox.UpdateViewComponent();

                if (ruleTypeEnumBox.SelectedItem == null ||
                    (Config.Parameters.Rule.RuleTypes)ruleTypeEnumBox.SelectedItem == Config.Parameters.Rule.RuleTypes.Object_1_Exists)
                {
                    groupBox2.Enabled = false;
                    ob2WhatEnumBox.Enabled = false;
                    ob2WhatLinkBox.Enabled = false;
                    ob2WhereEnumBox.Enabled = false;
                    ob2WhereLinkBox.Enabled = false;
                    ob2WhoEnumBox.Enabled = false;
                    fromStateComboBox.Enabled = false;
                    toStateComboBox1.Enabled = false;
                    
                }
                else
                {
                    groupBox2.Enabled = true;
                    ob2WhatEnumBox.Enabled = true;
                    ob2WhatLinkBox.Enabled = true;
                    ob2WhereEnumBox.Enabled = true;
                    ob2WhereLinkBox.Enabled = true;
                    ob2WhoEnumBox.Enabled = true;
                    //fromStateParameterTextBox1.Enabled = true;
                    //toStateParameterTextBox2.Enabled = true;
                    fromStateComboBox.Enabled = true;
                    toStateComboBox1.Enabled = true;

                    ids = myController.GetChildIDs(scoreRuleID, "Actor", "RuleObject");
                    int id;
                    if (ids.Count > 0)
                    {
                        id = ids[0];
                    }
                    else
                    {
                        //myController.TurnViewUpdateOff();
                        AME.Controllers.Base.Data_Structures.ComponentAndLinkID x;
                        x = myController.AddComponent(ScoreRuleID, ScoreRuleID, "Actor", "Object", "RuleObject", "");
                        id = x.ComponentID;
                        //myController.TurnViewUpdateOn(); only doing one update - no need?
                    }

                    ob2WhoEnumBox.EnumName = "WhoEnum";
                    ob2WhoEnumBox.ComponentId = id;
                    ob2WhoEnumBox.ParameterCategory = "Actor";
                    ob2WhoEnumBox.ParameterName = "Who";
                    ob2WhoEnumBox.UpdateViewComponent();

                    ob2WhatEnumBox.EnumName = "WhatEnum";
                    ob2WhatEnumBox.ComponentId = id;
                    ob2WhatEnumBox.ParameterCategory = "Actor";
                    ob2WhatEnumBox.ParameterName = "What";
                    ob2WhatEnumBox.EnumName = "WhatEnum";
                    ob2WhatEnumBox.UpdateViewComponent();

                    if (ob2WhatEnumBox.SelectedItem != null)
                    {
                        switch ((Config.Parameters.Actor.WhatEnum)ob2WhatEnumBox.SelectedItem)
                        {
                            case Config.Parameters.Actor.WhatEnum.Any:
                                ob2WhatLinkBox.DisplayRootId = -1;
                                ob2WhatLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhatEnum.Of_Species:
                                ob2WhatLinkBox.DisplayRootId = myController.ScenarioId;
                                ob2WhatLinkBox.DisplayComponentType = "Species";
                                ob2WhatLinkBox.DisplayLinkType = "Scenario";
                                ob2WhatLinkBox.ConnectFromId = id;
                                ob2WhatLinkBox.ConnectRootId = id;
                                ob2WhatLinkBox.ConnectLinkType = "ActorKind";
                                ob2WhatLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhatEnum.Unit:
                                ob2WhatLinkBox.DisplayRootId = myController.ScenarioId;
                                ob2WhatLinkBox.DisplayComponentType = "CreateEvent";
                                ob2WhatLinkBox.DisplayLinkType = "Scenario";
                                ob2WhatLinkBox.ConnectFromId = id;
                                ob2WhatLinkBox.ConnectRootId = id;
                                ob2WhatLinkBox.ConnectLinkType = "ActorUnit";
                                ob2WhatLinkBox.UpdateViewComponent();
                                break;

                        }
                    }

                    ob2WhereEnumBox.EnumName = "WhereEnum";
                    ob2WhereEnumBox.ComponentId = id;
                    ob2WhereEnumBox.ParameterCategory = "Actor";
                    ob2WhereEnumBox.ParameterName = "Where";
                    ob2WhereEnumBox.UpdateViewComponent();

                    if (ob2WhereEnumBox.SelectedItem != null)
                    {
                        switch ((Config.Parameters.Actor.WhereEnum)ob2WhereEnumBox.SelectedItem)
                        {
                            case Config.Parameters.Actor.WhereEnum.Anywhere:
                                ob2WhereLinkBox.DisplayRootId = -1;
                                ob2WhereLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhereEnum.In_Region:
                                ob2WhereLinkBox.DisplayRootId = myController.ScenarioId;
                                ob2WhereLinkBox.DisplayComponentType = "ActiveRegion";
                                ob2WhereLinkBox.DisplayLinkType = "Scenario";
                                ob2WhereLinkBox.ConnectFromId = ids[0];
                                ob2WhereLinkBox.ConnectRootId = ids[0];
                                ob2WhereLinkBox.ConnectLinkType = "ActorRegion";
                                ob2WhereLinkBox.OneToMany = true;
                                ob2WhereLinkBox.UpdateViewComponent();
                                break;
                            case Config.Parameters.Actor.WhereEnum.Not_In_Region:
                                ob2WhereLinkBox.DisplayRootId = myController.ScenarioId;
                                ob2WhereLinkBox.DisplayComponentType = "ActiveRegion";
                                ob2WhereLinkBox.DisplayLinkType = "Scenario";
                                ob2WhereLinkBox.ConnectFromId = ids[0];
                                ob2WhereLinkBox.ConnectRootId = ids[0];
                                ob2WhereLinkBox.ConnectLinkType = "ActorRegion";
                                ob2WhereLinkBox.OneToMany = true;
                                ob2WhereLinkBox.UpdateViewComponent();
                                break;

                        }
                    }

                    //fromStateParameterTextBox1.UpdateViewComponent();
                    //toStateParameterTextBox2.UpdateViewComponent();
                    fromStateComboBox.UpdateViewComponent();
                    toStateComboBox1.UpdateViewComponent();

                }

            }

            

        }

        public int ScoreRuleID
        {
            get
            {
                return scoreRuleID;
            }
            set
            {
                scoreRuleID = value;
                scoreIncrementTextBox1.ComponentId = value;
                fromStateComboBox.ComponentId = value;
                toStateComboBox1.ComponentId = value;
                toStateComboBox1.SpeciesId = -1;
                //toStateParameterTextBox2.ComponentId = value;
                //fromStateParameterTextBox1.ComponentId = value;
            }
        }
        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
                scoreIncrementTextBox1.Controller = value;
                ob1WhoEnumBox.Controller = value;
                ob1WhatEnumBox.Controller = value;
                ob1WhereEnumBox.Controller = value;
                ob1WhatLinkBox.Controller = value;
                ob1WhereLinkBox.Controller = value;
                ob2WhoEnumBox.Controller = value;
                ob2WhatEnumBox.Controller = value;
                ob2WhereEnumBox.Controller = value;
                ob2WhatLinkBox.Controller = value;
                ob2WhereLinkBox.Controller = value;
                ruleTypeEnumBox.Controller = value;
                fromStateComboBox.Controller = value;
                toStateComboBox1.Controller = value;
                //fromStateParameterTextBox1.Controller = value;
                //toStateParameterTextBox2.Controller = value;

                //scenario_name.Controller = controller;
                //scenario_description.Controller = controller;
                //scenario_time_to_attack.Controller = controller;
            }
        }
















    }
}
