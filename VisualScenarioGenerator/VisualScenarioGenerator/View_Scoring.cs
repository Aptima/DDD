using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.VSGPanes;
using VisualScenarioGenerator.Dialogs;

namespace VisualScenarioGenerator
{
    class View_Scoring: View
    {
        //private List<object> _ActorList = new List<object>();
        //private List<object> _LocationList = new List<object>();
        private List<object> _ScoringRuleList = new List<object>();
        //private List<object> _ExistenceRuleList = new List<object>();
        private List<object> _ScoreList = new List<object>();

        public View_Scoring(): base (new NavP_Scoring(), new CntP_Scoring())
        {
        }

        public CntP_Scoring GetContentPanel()
        {
            return ((CntP_Scoring)ContentPanel);
        }

        public NavP_Scoring GetNavigationPanel()
        {
            return ((NavP_Scoring)NavigatorPanel);
        }

        public bool NameInUse(Scoring_NodeTypes type, string name)
        {
            switch (type)
            {
                //case Scoring_NodeTypes.Actors:
                //    foreach (ActorDataStruct s1 in _ActorList)
                //    {
                //        if (s1.ID == name)
                //        {
                //            MessageBox.Show(string.Format("An object with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                //            return true;
                //        }
                //    }
                //    break;
                //case Scoring_NodeTypes.Location:
                //    foreach (LocationDataStruct s1 in _LocationList)
                //    {
                //        if (s1.ID == name)
                //        {
                //            MessageBox.Show(string.Format("An object with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                //            return true;
                //        }
                //    }
                //    break;
                case Scoring_NodeTypes.Scoring_Rule:
                    foreach (ScoringRuleDataStruct s1 in _ScoringRuleList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("An object with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            return true;
                        }
                    }
                    break;
                //case Scoring_NodeTypes.Existence_Rule:
                //    foreach (ExistenceRuleDataStruct s1 in _ExistenceRuleList)
                //    {
                //        if (s1.ID == name)
                //        {
                //            MessageBox.Show(string.Format("An object with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                //            return true;
                //        }
                //    }
                //    break;
                case Scoring_NodeTypes.Score:
                    foreach (ScoreDataStruct s1 in _ScoreList)
                    {
                        if (s1.ID == name)
                        {
                            MessageBox.Show(string.Format("An object with ID ({0}) already exists within this subtree.  Please choose another ID.", name), "Cannot complete update.");
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public bool NameInUse(object object_data)
        {
            //if (object_data is ActorDataStruct)
            //{
            //    return NameInUse(Scoring_NodeTypes.Actors, ((ActorDataStruct)object_data).ID);
            //}
            //if (object_data is LocationDataStruct)
            //{
            //    return NameInUse(Scoring_NodeTypes.Location, ((LocationDataStruct)object_data).ID);
            //}
            if (object_data is ScoringRuleDataStruct)
            {
                return NameInUse(Scoring_NodeTypes.Scoring_Rule, ((ScoringRuleDataStruct)object_data).ID);
            }
            //if (object_data is ExistenceRuleDataStruct)
            //{
            //    return NameInUse(Scoring_NodeTypes.Existence_Rule, ((ExistenceRuleDataStruct)object_data).ID);
            //}
            if (object_data is ScoreDataStruct)
            {
                return NameInUse(Scoring_NodeTypes.Score, ((ScoreDataStruct)object_data).ID);
            }
            return false;
        }

        //public ActorDataStruct CreateActor(string name)
        //{
        //    ActorDataStruct ac = ActorDataStruct.Empty;
        //    ac.ID = name;
        //    _ActorList.Add(ac);
        //
        //    return ac;
        //}
        //public LocationDataStruct CreateLocation(string name)
        //{
        //    LocationDataStruct l = LocationDataStruct.Empty;
        //    l.ID = name;
        //    _LocationList.Add(l);
        //
        //    return l;
        //}
        public ScoringRuleDataStruct CreateScoringRule(string name)
        {
            ScoringRuleDataStruct en = ScoringRuleDataStruct.Empty;
            en.ID = name;
            _ScoringRuleList.Add(en);

            return en;
        }
        //public ExistenceRuleDataStruct CreateExistenceRule(string name)
        //{
        //    ExistenceRuleDataStruct ex = ExistenceRuleDataStruct.Empty;
        //    ex.ID = name;
        //    _ExistenceRuleList.Add(ex);
        //
        //    return ex;
        //}
        public ScoreDataStruct CreateScore(string name)
        {
            ScoreDataStruct sc = ScoreDataStruct.Empty;
            sc.ID = name;
            _ScoreList.Add(sc);

            return sc;
        }

        //public object GetActor(int index)
        //{
        //    try
        //    {
        //        return _ActorList[index];
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //    return null;
        //}
        //public object GetLocation(int index)
        //{
        //    try
        //    {
        //        return _LocationList[index];
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //    return null;
        //}
        public object GetScoringRule(int index)
        {
            try
            {
                return _ScoringRuleList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }
        //public object GetExistenceRule(int index)
        //{
        //    try
        //    {
        //        return _ExistenceRuleList[index];
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //    return null;
        //}
        public object GetScore(int index)
        {
            try
            {
                return _ScoreList[index];
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }

        //public void RemoveActor(int index)
        //{
        //    try
        //    {
        //        _ActorList.RemoveAt(index);
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //}
        //public void RemoveLocation(int index)
        //{
        //    try
        //    {
        //        _LocationList.RemoveAt(index);
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //}
        public void RemoveScoringRule(int index)
        {
            try
            {
                _ScoringRuleList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        //public void RemoveExistenceRule(int index)
        //{
        //    try
        //    {
        //        _ExistenceRuleList.RemoveAt(index);
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //    }
        //}
        public void RemoveScore(int index)
        {
            try
            {
                _ScoreList.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        public override void UpdateContentPanel(object object_data)
        {
            GetContentPanel().Update(object_data);
        }
        public override void UpdateNavigatorPanel(Control control, object object_data)
        {
            if (NameInUse(object_data))
            {
                /* Dont continue if object data contains an id already in use for the subtree
                 * */
                return;
            }

            GetNavigationPanel().Update(object_data);
            int node_index = GetNavigationPanel().SelectedNode.Index;

            List<object> list = null;
            //if (object_data is ActorDataStruct)
            //{
            //    list = _ActorList;
            //    if (list.Count > node_index)
            //    {
            //        list[node_index] = object_data;
            //    }
            //    else
            //    {
            //        list.Add(object_data);
            //    }
            //}
            //if (object_data is LocationDataStruct)
            //{
            //    list = _LocationList;
            //    if (list.Count > node_index)
            //    {
            //        list[node_index] = object_data;
            //    }
            //    else
            //    {
            //        list.Add(object_data);
            //    }
            //}
            if (object_data is ScoringRuleDataStruct)
            {
                list = _ScoringRuleList;
                if (list.Count > node_index)
                {
                    list[node_index] = object_data;
                }
                else
                {
                    list.Add(object_data);
                }
            }
            //if (object_data is ExistenceRuleDataStruct)
            //{
            //    list = _ExistenceRuleList;
            //    if (list.Count > node_index)
            //    {
            //        list[node_index] = object_data;
            //    }
            //    else
            //    {
            //        list.Add(object_data);
            //    }
            //}
            if (object_data is ScoreDataStruct)
            {
                list = _ScoreList;
                if (list.Count > node_index)
                {
                    list[node_index] = object_data;
                }
                else
                {
                    list.Add(object_data);
                }
            }
        }


        public override void UpdateView(object object_data)
        {
            base.UpdateView(object_data);
        }

    }
    //public struct ActorDataStruct : ICloneable
    //{
    //    public string ID;
    //    public static ActorDataStruct Empty = new ActorDataStruct();

    //    #region ICloneable Members

    //    public object Clone()
    //    {
    //        ActorDataStruct obj = new ActorDataStruct();
    //        obj.ID = ID;
    //        return obj;
    //    }

    //    #endregion
    //}


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

    public struct ScoringRuleDataStruct : ICloneable
    {
        public string ID;
        public static ScoringRuleDataStruct Empty = new ScoringRuleDataStruct();

        #region ICloneable Members

        public object Clone()
        {
            ScoringRuleDataStruct obj = new ScoringRuleDataStruct();
            obj.ID = ID;
            return obj;
        }

        #endregion
    }

    //public struct ExistenceRuleDataStruct : ICloneable
    //{
    //    public string ID;
    //    public static ExistenceRuleDataStruct Empty = new ExistenceRuleDataStruct();

    //    #region ICloneable Members

    //    public object Clone()
    //    {
    //        ExistenceRuleDataStruct obj = new ExistenceRuleDataStruct();
    //        obj.ID = ID;
    //        return obj;
    //    }

    //    #endregion
    //}

    public struct ScoreDataStruct : ICloneable
    {
        public string ID;
        public static ScoreDataStruct Empty = new ScoreDataStruct();

        #region ICloneable Members

        public object Clone()
        {
            ScoreDataStruct obj = new ScoreDataStruct();
            obj.ID = ID;
            return obj;
        }

        #endregion
    }




    

}
