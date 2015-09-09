using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;

namespace DashboardDataAccess
{

    public class ExperimentDataModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> usersNotInExperiment = null;

        public ObservableCollection<User> UsersNotInExperiment
        {
            get { return usersNotInExperiment; }
            set { usersNotInExperiment = value; OnPropertyChanged("UsersNotInExperiment"); }
        }

        private ObservableCollection<User> usersInExperiment = null;

        public ObservableCollection<User> UsersInExperiment
        {
            get { return usersInExperiment; }
            set { usersInExperiment = value; OnPropertyChanged("UsersInExperiment"); }
        }

        private string[] curExperimentUsers = null;

        public string[] CurExperimentUsers
        {
            get { return curExperimentUsers; }
            set { curExperimentUsers = value; }
        }

        private int curExperimentIndex = -1;

        public int CurExperimentIndex
        {
            get { return curExperimentIndex; }
            set {
                curExperimentIndex = value;
                ReloadExperimentInfo();
                OnPropertyChanged("CurExperiment");
            }
        }

        public Experiment CurExperiment
        {
            get {
                if ((Experiments != null) && (curExperimentIndex >= 0) && (curExperimentIndex < Experiments.Length))
                {
                    return Experiments[curExperimentIndex];
                }
                return null;
            }
        }

        private Experiment[] experiments = null;

        public Experiment[] Experiments
        {
            get { return experiments; }
            set { experiments = value; OnPropertyChanged("Experiments"); }
        }

        string[] experimentNames = { "Empty List" };

        public string[] ExperimentNames
        {
            get { return experimentNames; }
            set { experimentNames = value; OnPropertyChanged("ExperimentNames"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ExperimentDataModel()
        {
            experimentNames = null;
        }

        public void LoadUserExperiments(User user)
        {
            if (user.UserID <= 0)
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            var exp = from e in db.Experiments
                              where e.CreatorID == user.UserID
                              select e;
            Experiments = exp.ToArray();

            ExperimentNames = (from e in db.Experiments
                               where e.CreatorID == user.UserID
                              select e.Name).ToArray();
        }

        public bool AddNewExperiment(CreateExperimentInfo newExperimentInfo)
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    Experiment newExperiment = new Experiment
                    {
                        Name = newExperimentInfo.Name,
                        CreatorID = newExperimentInfo.CreatorID
                    };

                    db.Experiments.InsertOnSubmit(newExperiment);
                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Experiment Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    ts.Complete();
                    ts.Dispose();
                    db = null;
                }
            }

            return true;
        }

        public void ReloadExperimentInfo()
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            if (CurExperiment == null)
            {
                return;
            }

            // All Users
            var allUsers = (from u in db.Users
                           join uIr in db.UserInRoles on
                             new { u.UserID, RoleName = "Experimenter" }
                             equals
                             new { uIr.UserID, uIr.Role.RoleName }
                             orderby u.Username
                           select u)
                           .Union
                            (from u in db.Users
                           join uIr in db.UserInRoles on
                             new { u.UserID, RoleName = "Operator" }
                             equals
                             new { uIr.UserID, uIr.Role.RoleName }
                             orderby u.Username
                           select u);

            // Load Users in Experiment
            var usersInExp = from u in db.Users
                             join uIe in db.UsersInExperiments on
                             new { u.UserID, ExperimentID = CurExperiment.ExperimentID }
                             equals
                             new { uIe.UserID, uIe.ExperimentID }
                             orderby u.Username
                             select u;
            UsersInExperiment = new ObservableCollection<User>();
            usersInExp.ToList().ForEach(x => UsersInExperiment.Add(x)); ;

            // Load Users not in Experiment
            var usersNotInExp = allUsers.Except(usersInExp);
            UsersNotInExperiment = new ObservableCollection<User>();
            usersNotInExp.ToList().ForEach(x => UsersNotInExperiment.Add(x));

            // Load list of users names
            if ((usersInExp.ToList() != null) && (usersInExp.ToList().Count > 0))
            {
                CurExperimentUsers = new String[usersInExp.ToList().Count];
                for (int i = 0; i < usersInExp.ToList().Count; i++)
                {
                    CurExperimentUsers[i] = usersInExp.ToList()[i].Username;
                }
            }
            else
            {
                CurExperimentUsers = null;
            }
        }

        public void SaveScenarioFileInfo()
        {
            if (CurExperiment == null)
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            Experiment experiment = null;

            var query = from e in db.Experiments
                      where e.ExperimentID == CurExperiment.ExperimentID
                      select e;

            try
            {
                experiment = query.Single();
            }
            catch (Exception)
            {
                return;
            }

            experiment.ScenarioFilePath = CurExperiment.ScenarioFilePath;
            experiment.ScenarioFileType = CurExperiment.ScenarioFileType;

            db.SubmitChanges();
        }

        public void ScanDDDScenario(MeasuresDataModel measureDataModel)
        {
            List<String> dmIDs = new List<String>();
            List<AssetEntity> entities = new List<AssetEntity>();
            Dictionary<String, ExperimentEntity> entityMap = new Dictionary<string, ExperimentEntity>();

            int operatorTypeID = -1;
            int assetTypeID = -1;

            try
            {
                if ((CurExperiment.ScenarioFilePath == null) || (CurExperiment.ScenarioFilePath.Length <= 0))
                {
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Scanning a scenario file will clear all selections on the Measures tab.  Do you wish to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Open scenario file
                XPathDocument Doc = new XPathDocument(CurExperiment.ScenarioFilePath);
                XPathNavigator navigator = Doc.CreateNavigator();
                navigator.MoveToRoot();

                // Make sure this is a DDD Scenario file
                XPathNavigator node = navigator.SelectSingleNode("/Scenario/ScenarioName");
                String scenarioName = node.Value;

                if ((scenarioName == null) || (scenarioName.Length <= 0))
                {
                    return;
                }

                // Get all of the DecisionMaker ids
                XPathNodeIterator iterator = navigator.Select("/Scenario/DecisionMaker");
                String dmID = null;

                while (iterator.MoveNext())
                {
                    node = iterator.Current.SelectSingleNode("Identifier");
                    dmID = node.Value;

                    if ((dmID == null) || (dmID.Length <= 0))
                    {
                        continue;
                    }

                    dmIDs.Add(dmID);
                }

                // Get all of the entity names
                iterator = navigator.Select("/Scenario/Create_Event");
                String entityID = null;

                while (iterator.MoveNext())
                {
                    string ownerName = null;
                    node = iterator.Current.SelectSingleNode("ID");
                    entityID = node.Value;

                    node = iterator.Current.SelectSingleNode("Owner");
                    if (node != null)
                    {
                        ownerName = node.Value;
                    }

                    if ((dmID == null) || (dmID.Length <= 0))
                    {
                        continue;
                    }

                    AssetEntity newAssetEntity = new AssetEntity();
                    newAssetEntity.AssetName = entityID;
                    newAssetEntity.OwnerName = ownerName;
                    entities.Add(newAssetEntity);
                }

                // Clear the experiment measures
                measureDataModel.DeleteExperimentMeasures(CurExperiment);

                // Clear the experiment entities
                ClearExperimentEntities();

                // Connect to database
                VisDashboardDataDataContext db = new VisDashboardDataDataContext();

                // Get Entity type for operators
                var query = from et in db.EntityTypes where et.Name == "Operator" select et;

                try
                {
                    operatorTypeID = query.Single().EntityTypeID;
                }
                catch (Exception)
                {
                    return;
                }

                // Get Entity type for assets
                query = from et in db.EntityTypes where et.Name == "Asset" select et;

                try
                {
                    assetTypeID = query.Single().EntityTypeID;
                }
                catch (Exception)
                {
                    return;
                }

                // Add the new experiment DMs
                ExperimentEntity newExpEntity = null;

                foreach (String id in dmIDs)
                {
                    newExpEntity = new ExperimentEntity();
                    newExpEntity.Name = id;
                    newExpEntity.ExperimentID = CurExperiment.ExperimentID;
                    newExpEntity.EntityTypeID = operatorTypeID;
                    newExpEntity.OwnerExperimentEntityID = null;

                    db.ExperimentEntities.InsertOnSubmit(newExpEntity);
                    entityMap.Add(id, newExpEntity);
                }

                foreach (AssetEntity entity in entities)
                {
                    newExpEntity = new ExperimentEntity();
                    newExpEntity.Name = entity.AssetName;
                    newExpEntity.ExperimentID = CurExperiment.ExperimentID;
                    newExpEntity.EntityTypeID = assetTypeID;
                    if ((entity.OwnerName != null) && (entityMap.Keys.Contains(entity.OwnerName)))
                    {
                        newExpEntity.ExperimentEntity1 = entityMap[entity.OwnerName];
                    }
                    else
                    {
                        newExpEntity.OwnerExperimentEntityID = null;
                    }

                    db.ExperimentEntities.InsertOnSubmit(newExpEntity);
                    entityMap.Add(entity.AssetName, newExpEntity);
                }
                db.SubmitChanges();

                // Add new Experiment Measures
                ExperimentMeasure newExperimentMeasure = null;
                var queryMeasures = from p in db.Measures select p;

                foreach(UsersInExperiment user in CurExperiment.UsersInExperiments)
                {

                    foreach (var measure in queryMeasures)
                    {
                        if (measure.Name.CompareTo("Operator") == 0)
                        {
                            foreach (String id in dmIDs)
                            {
                                newExperimentMeasure = new ExperimentMeasure
                                {
                                    ExperimentID = CurExperiment.ExperimentID,
                                    MeasureID = measure.MeasureID,
                                    UserID = user.UserID,
                                    ExperimentEntity = entityMap[id],
                                    Allowed = false
                                };
                                db.ExperimentMeasures.InsertOnSubmit(newExperimentMeasure);
                            }
                        }
                        else if (measure.Name.CompareTo("Asset") == 0)
                        {
                            foreach (AssetEntity entity in entities)
                            {
                                newExperimentMeasure = new ExperimentMeasure
                                {
                                    ExperimentID = CurExperiment.ExperimentID,
                                    MeasureID = measure.MeasureID,
                                    UserID = user.UserID,
                                    ExperimentEntity = entityMap[entity.AssetName],
                                    Allowed = false
                                };
                                db.ExperimentMeasures.InsertOnSubmit(newExperimentMeasure);
                            }
                        }
                    }
                }

                db.SubmitChanges();

                // Reload experiment measures
                measureDataModel.LoadExperiementMeasures(CurExperiment,
                    UsersInExperiment);

            }
            catch (Exception)
            {
                return;
            }
        }

        private void ClearExperimentEntities()
        {
            if (CurExperiment == null)
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            var oldEntities = from e in db.ExperimentEntities
                        where e.ExperimentID == CurExperiment.ExperimentID
                        select e;

            db.ExperimentEntities.DeleteAllOnSubmit(oldEntities);
            db.SubmitChanges();
        }
    }

    public class CreateExperimentInfo
    {
        private string _name = null;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        private int _creatorID = 0;

        public int CreatorID
        {
            get { return _creatorID; }
            set
            {
                _creatorID = value;
            }
        }
    }

    public class AssetEntity
    {
        private string _AssetName = null;

        public string AssetName
        {
            get { return _AssetName; }
            set { _AssetName = value; }
        }
        private string _OwnerName = null;

        public string OwnerName
        {
            get { return _OwnerName; }
            set { _OwnerName = value; }
        }
    }
}
