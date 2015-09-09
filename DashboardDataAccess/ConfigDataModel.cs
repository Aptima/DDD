using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows;
using System.Collections.ObjectModel;

namespace DashboardDataAccess
{
    public enum RTPMEType { Measure, Paramter, Trigger };

    public class ConfigDataModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> usersNotInConfig = null;

        public ObservableCollection<User> UsersNotInConfig
        {
            get { return usersNotInConfig; }
            set { usersNotInConfig = value; OnPropertyChanged("UsersNotInConfig"); }
        }

        private ObservableCollection<User> usersInConfig = null;

        public ObservableCollection<User> UsersInConfig
        {
            get { return usersInConfig; }
            set { usersInConfig = value; OnPropertyChanged("UsersInConfig"); }
        }

        private string[] curConfigUsers = null;

        public string[] CurConfigUsers
        {
            get { return curConfigUsers; }
            set { curConfigUsers = value; }
        }

        private int curConfigIndex = -1;

        public int CurConfigIndex
        {
            get { return curConfigIndex; }
            set {
                curConfigIndex = value;
                LoadConfigInfo();
            }
        }

        private int curExperimentID = -1;

        public int CurExperimentID
        {
            get { return curExperimentID; }
            set { curExperimentID = value; }
        }

        public Config CurConfig
        {
            get {
                if ((Configs != null) && (curConfigIndex >= 0) && (curConfigIndex < Configs.Length))
                {
                    return Configs[curConfigIndex];
                }
                return null;
            }
        }

        private Config[] configs = null;

        public Config[] Configs
        {
            get { return configs; }
            set { configs = value; OnPropertyChanged("Configs"); }
        }

        string[] configNames = { "Empty List" };

        public string[] ConfigNames
        {
            get { return configNames; }
            set { configNames = value; OnPropertyChanged("ConfigNames"); }
        }

        private ObservableCollection<ExperimentMeasure> configMeasuresTable = null;

        public ObservableCollection<ExperimentMeasure> ConfigMeasuresTable
        {
            get { return configMeasuresTable; }
            set { configMeasuresTable = value; OnPropertyChanged("ConfigMeasuresTable"); }
        }

        ObservableCollection<String> measureNames = null;

        public ObservableCollection<String> MeasureNames
        {
            get { return measureNames; }
            set { measureNames = value; OnPropertyChanged("MeasureNames"); }
        }

        
        private ObservableCollection<ExperimentDisplay> configDisplayTable = null;

        public ObservableCollection<ExperimentDisplay> ConfigDisplayTable
        {
            get { return configDisplayTable; }
            set { configDisplayTable = value; OnPropertyChanged("ConfigDisplayTable"); }
        }
        
        ObservableCollection<String> displayNames = null;

        public ObservableCollection<String> DisplayNames
        {
            get { return displayNames; }
            set { displayNames = value; OnPropertyChanged("DisplayNames"); }
        }

        private ConfigDisplay curConfigDisplay = null;

        public ConfigDisplay CurConfigDisplay
        {
            get { return curConfigDisplay; }
            set { curConfigDisplay = value; OnPropertyChanged("CurConfigDisplay"); }
        }

        ObservableCollection<String> metricNames = null;

        public ObservableCollection<String> MetricNames
        {
            get { return metricNames; }
            set { metricNames = value; OnPropertyChanged("MetricNames"); }
        }

        private ObservableCollection<BlockedFactorItem> configBlockFactorTable;

        public ObservableCollection<BlockedFactorItem> ConfigBlockFactorTable
        {
            get { return configBlockFactorTable; }
            set { configBlockFactorTable = value; OnPropertyChanged("ConfigBlockFactorTable"); }
        }

        private ObservableCollection<FactorItem> configFactorTable;

        public ObservableCollection<FactorItem> ConfigFactorTable
        {
            get { return configFactorTable; }
            set { configFactorTable = value; OnPropertyChanged("ConfigFactorTable"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ConfigDataModel()
        {
            configNames = null;

            // Setup current display
            curConfigDisplay = new ConfigDisplay();
            curConfigDisplay.Width = 275;
            curConfigDisplay.Height = 300;

            ConfigFactorTable = new ObservableCollection<FactorItem>();
            ConfigBlockFactorTable = new ObservableCollection<BlockedFactorItem>();

        }

        public void LoadExperimentConfigs(Experiment experiment)
        {
            if ((experiment == null) || (experiment.ExperimentID <= 0))
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            var config = from c in db.Configs
                              where c.ExperimentID == experiment.ExperimentID
                              select c;
            Configs = config.ToArray();

            ConfigNames = (from c in db.Configs
                           where c.ExperimentID == experiment.ExperimentID
                              select c.Name).ToArray();

            CurExperimentID = experiment.ExperimentID;
        }

        public bool AddNewConfig(CreateConfigInfo newConfigInfo)
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    Config newConfig = new Config
                    {
                        Name = newConfigInfo.Name,
                        ExperimentID = newConfigInfo.ExperimentID
                    };

                    db.Configs.InsertOnSubmit(newConfig);
                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Config Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public void LoadConfigInfo()
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            bool userFound = false;

            if (CurConfig == null)
            {
                return;
            }

            // Reset the CurConfigDisplay
            ClearConfigDisplay();

            // Clear the Display Builder
            ClearDisplayBuilder();

            // All Users in an experiment
            var allUsers = from uIe in db.UsersInExperiments
                            where uIe.ExperimentID == CurExperimentID
                            select uIe.User;

            // Load Users in Config
            var usersInExp = from u in db.Users
                             join uIe in db.UsersInConfigs on
                             new { u.UserID, ConfigID = CurConfig.ConfigID }
                             equals
                             new { uIe.UserID, uIe.ConfigID }
                             orderby u.Username
                             select u;
            UsersInConfig = new ObservableCollection<User>();
            usersInExp.ToList().ForEach(x => UsersInConfig.Add(x)); ;

            // Load Users not in Config
            var usersNotInExp = allUsers.Except(usersInExp);
            UsersNotInConfig = new ObservableCollection<User>();
            usersNotInExp.ToList().ForEach(x => UsersNotInConfig.Add(x));

            // Load list of users names
            if ((usersInExp.ToList() != null) && (usersInExp.ToList().Count > 0))
            {
                CurConfigUsers = new String[usersInExp.ToList().Count];
                for (int i = 0; i < usersInExp.ToList().Count; i++)
                {
                    CurConfigUsers[i] = usersInExp.ToList()[i].Username;
                }
            }
            else
            {
                CurConfigUsers = null;
            }

            // Load available measure names
            ConfigMeasuresTable = new ObservableCollection<ExperimentMeasure>();

            List<ExperimentMeasure> expMeasuresAll = (from eM in db.ExperimentMeasures
                                                      join uIc in db.UsersInConfigs on
                                                      new { eM.UserID, ConfigID = CurConfig.ConfigID }
                                                      equals
                                                      new { uIc.UserID, uIc.ConfigID }
                                                      where eM.ExperimentID == CurExperimentID
                                                      select eM).ToList();

            foreach (ExperimentMeasure expMeasure in expMeasuresAll)
            {
                if ((expMeasure.Allowed.HasValue) && (expMeasure.Allowed == false))
                {
                    continue;
                }

                // Make sure all users in configuration have access to this measure
                foreach (User user in UsersInConfig)
                {
                    userFound = false;
                    foreach (ExperimentMeasure expMeasure2 in expMeasuresAll)
                    {
                        if ((expMeasure2.UserID == user.UserID) &&
                            (expMeasure2.MeasureID == expMeasure.MeasureID))
                        {
                            userFound = true;
                            break;
                        }
                    }
                    if (!userFound)
                    {
                        break;
                    }
                }
                if (userFound)
                {
                    // Add this measure to the available measure list
                    ConfigMeasuresTable.Add(expMeasure);
                }
            }

            measureNames = new ObservableCollection<string>();
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if (!MeasureNames.Contains(expMeasure.Measure.Category))
                {
                    MeasureNames.Add(expMeasure.Measure.Category);
                }
            }
            OnPropertyChanged("MeasureNames");

            // Load Display Info
            LoadConfigDisplayInfo();

        }

        public void ClearDisplayBuilder()
        {
            ConfigMeasuresTable = null;
            MeasureNames = null;
            ConfigDisplayTable = null;
            DisplayNames = null;
            if (ConfigFactorTable != null)
            {
                ConfigFactorTable.Clear();
            }
            if (ConfigBlockFactorTable != null)
            {
                ConfigBlockFactorTable.Clear();
            }
            MetricNames = null;            
        }

        public void LoadConfigDisplayInfo()
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            bool userFound = false;

            if (CurConfig == null)
            {
                return;
            }

            // Load Available Display Names
            displayNames = new ObservableCollection<string>();
            ConfigDisplayTable = new ObservableCollection<ExperimentDisplay>();

            List<ExperimentDisplay> expDisplayAll = (from eD in db.ExperimentDisplays
                                                      join uIc in db.UsersInConfigs on
                                                      new { eD.UserID, ConfigID = CurConfig.ConfigID }
                                                      equals
                                                      new { uIc.UserID, uIc.ConfigID }
                                                      where eD.ExperimentID == CurExperimentID
                                                      select eD).ToList();

            foreach (ExperimentDisplay expDisplay in expDisplayAll)
            {
                // Make sure all users in configuration have access to this display
                foreach (User user in UsersInConfig)
                {
                    userFound = false;
                    foreach (ExperimentDisplay expDisplay2 in expDisplayAll)
                    {
                        if ((expDisplay2.UserID == user.UserID) &&
                            (expDisplay2.DisplayID == expDisplay.DisplayID))
                        {
                            userFound = true;
                            break;
                        }
                    }
                    if (!userFound)
                    {
                        break;
                    }
                }
                if ((userFound) && (!DisplayNames.Contains(expDisplay.Display.Name)))
                {
                    // Add this Display to the available Display list
                    ConfigDisplayTable.Add(expDisplay);
                    DisplayNames.Add(expDisplay.Display.Name);
                }
            }

            OnPropertyChanged("DisplayNames");

        }
        public void SelectDisplay(string displayName)
        {
            foreach (ExperimentDisplay expDisplay in ConfigDisplayTable)
            {
                if (expDisplay.Display.Name.CompareTo(displayName) == 0)
                {
                    CurConfigDisplay.Display = expDisplay.Display;
                    break;
                }
            }
        }

        public void LoadMetricDisplayInfo(string measureName)
        {
            MetricNames = new ObservableCollection<string>();

            if (measureName == null)
            {
                return;
            }

            MetricNames.Add("Click here to select");

            // Add the Metrics for this measure
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory.CompareTo("Metric") == 0) &&
                    (!MetricNames.Contains(expMeasure.Measure.Name)))
                {
                    MetricNames.Add(expMeasure.Measure.Name);
                }
            }

            if (MetricNames.Count == 1)
            {
                MetricNames.Clear();
            }

        }

        public int ResetBlockedFactors(String measureName, String metricName, Boolean updateExisting)
        {
            int blockedFactorCount = 0;
            if (updateExisting == false)
            {
                // Show all factors as blocked factors with appropriate drop downs
                ConfigBlockFactorTable.Clear();
            }
            else if (ConfigBlockFactorTable == null)
            {
                return - 1;
            }

            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory.CompareTo("Metric") != 0))
                {
                    Boolean blockedFactorFound = false;

                    foreach (BlockedFactorItem item in ConfigBlockFactorTable)
                    {
                        if (item.FactorName.CompareTo(expMeasure.Measure.SubCategory) == 0)
                        {
                            blockedFactorFound = true;
                            break;
                        }
                    }

                    if (!blockedFactorFound)
                    {
                        BlockedFactorItem factorItem = new BlockedFactorItem();
                        factorItem.FactorName = expMeasure.Measure.SubCategory;
                        factorItem.MeasureID = expMeasure.Measure.MeasureID;
                        factorItem.FactorLevelNames = new ObservableCollection<string>();
                        factorItem.FactorLevelNames.Add("Click here to select");

                        foreach (ExperimentMeasure expMeasure2 in ConfigMeasuresTable)
                        {
                            if ((expMeasure2.Measure.Category.CompareTo(measureName) == 0) &&
                                (expMeasure2.Measure.SubCategory.CompareTo(factorItem.FactorName) == 0) &&
                                (expMeasure2.ExperimentEntityID > 0) &&
                                (!factorItem.FactorLevelNames.Contains(expMeasure2.ExperimentEntity.Name)))
                            {
                                factorItem.FactorLevelNames.Add(expMeasure2.ExperimentEntity.Name);
                            }
                            else if ((expMeasure2.Measure.Category.CompareTo(measureName) == 0) &&
                                (expMeasure2.Measure.SubCategory.CompareTo(factorItem.FactorName) == 0) &&
                                ((!expMeasure2.ExperimentEntityID.HasValue) || (expMeasure2.ExperimentEntityID == 0)) &&
                                (!factorItem.FactorLevelNames.Contains(expMeasure2.Measure.Name)))
                            {
                                factorItem.FactorLevelNames.Add(expMeasure2.Measure.Name);
                            }
                        }

                        if (factorItem.FactorLevelNames.Count == 1)
                        {
                            factorItem.FactorLevelNames.Clear();
                        }

                        ConfigBlockFactorTable.Add(factorItem);
                        blockedFactorCount++;
                    }
                }
            }

            // Remove set Factors from Blocked Factor list
            if (updateExisting)
            {
                foreach (DisplayFactor df in CurConfigDisplay.DisplayFactors)
                {
                    foreach (BlockedFactorItem bf in ConfigBlockFactorTable)
                    {
                        if (bf.FactorName.CompareTo(df.FactorName) == 0)
                        {
                            ConfigBlockFactorTable.Remove(bf);
                            blockedFactorCount--;
                            break;
                        }
                    }
                }
            }
                        
            OnPropertyChanged("ConfigBlockFactorTable");

            return blockedFactorCount;
        }

        public int ResetFactors(String measureName, String metricName)
        {
            int factorCount = 0;

            // Clear the Factor Table
            ConfigFactorTable.Clear();

            // Find the factor labels needed for the selected display
            if (CurConfigDisplay.Display == null)
            {
                return - 1;
            }

            foreach (DisplayFactorLabel displayFactorLabels in CurConfigDisplay.Display.DisplayFactorLabels)
            {
                FactorItem factorItem = new FactorItem();
                factorItem.DisplayFactorLabel = "Factor " + displayFactorLabels.FactorPos.ToString() + " (" + displayFactorLabels.Label + ")";
                factorItem.FactorPos = displayFactorLabels.FactorPos;

                factorItem.FactorNames = new ObservableCollection<string>();
                factorItem.FactorNames.Add("Click here to select");

                foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
                {
                    if ((expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                        (expMeasure.Measure.SubCategory.CompareTo("Metric") != 0))
                    {
                        if (!factorItem.FactorNames.Contains(expMeasure.Measure.SubCategory))
                        {
                            factorItem.FactorNames.Add(expMeasure.Measure.SubCategory);
                        }
                    }
                }

                if (factorItem.FactorNames.Count == 1)
                {
                    factorItem.FactorNames.Clear();
                }

                ConfigFactorTable.Add(factorItem);
                factorCount++;
            }

            OnPropertyChanged("ConfigFactorTable");

            return factorCount;
        }

        public void AddConfigFactor(int pos, string factorName)
        {
            Boolean factorInDisplay = false;

            foreach (DisplayFactor df in CurConfigDisplay.DisplayFactors)
            {
                if (df.FactorName.CompareTo(factorName) == 0)
                {
                    // This factor is already in the config
                    factorInDisplay = true;
                    break;
                }
            }
            if (!factorInDisplay)
            {
                DisplayFactor newDisplayFactor = new DisplayFactor();
                newDisplayFactor.FactorName = factorName;
                newDisplayFactor.FactorPos = pos;
                newDisplayFactor.FactorLabel = CurConfigDisplay.Display.DisplayFactorLabels[pos-1].Label;

                CurConfigDisplay.DisplayFactors.Add(newDisplayFactor);

                // Remove this factor from the blocked factor list if neccessary
                RemoveConfigBlockedFactor(factorName);

            }

        }

        public void RemoveConfigFactor(int pos, string factorName)
        {
            foreach (DisplayFactor df in CurConfigDisplay.DisplayFactors)
            {
                if (df.FactorName.CompareTo(factorName) == 0)
                {
                    // Remove this factor
                    CurConfigDisplay.DisplayFactors.Remove(df);
                    break;
                }
            }
        }

        public int LocateMeasureID(string measureName, string factorName, string levelName)
        {
            Measure measure = null;

            return LocateMeasureID(measureName, factorName, levelName, out measure);
        }

        public int LocateMeasureID(string measureName, string factorName, string levelName, out Measure measure)
        {
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.Measure.Name.CompareTo(levelName) == 0) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0) &&
                    ((!expMeasure.ExperimentEntityID.HasValue) || (expMeasure.ExperimentEntityID == 0))
                    )
                {
                    measure = expMeasure.Measure;
                    return expMeasure.Measure.MeasureID;
                }
                else if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.ExperimentEntityID > 0) &&
                    (expMeasure.ExperimentEntity.Name.CompareTo(levelName) == 0) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0)
                    )
                {
                    measure = expMeasure.Measure;
                    return expMeasure.Measure.MeasureID;
                }
            }

            // Nothing found in experiment - Check full measure list
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            var query = from p in db.Measures select p;
            foreach (var item in query)
            {
                if ((string.Compare(item.Category, measureName) == 0) &&
                    (string.Compare(item.SubCategory, factorName) == 0) &&
                    (string.Compare(item.Name, levelName) == 0))
                {
                    measure = item;
                    return item.MeasureID;
                }
            }

            measure = null;
            return -1;
        }

        public ExperimentEntity GetEntityInfo(string measureName, string factorName, string levelName)
        {
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.ExperimentEntityID > 0) &&
                    (expMeasure.ExperimentEntity.Name.CompareTo(levelName) == 0) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0)
                    )
                {
                    return expMeasure.ExperimentEntity;
                }
            }

            return null;
        }

        public RTPMEType GetMeasureRTPMEType(string measureName, string factorName, string levelName)
        {
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.Measure.Name.CompareTo(levelName) == 0) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0) &&
                    ((!expMeasure.ExperimentEntityID.HasValue) || (expMeasure.ExperimentEntityID == 0))
                    )
                {
                    if ((expMeasure.Measure.RTPMEngineType != null) && (expMeasure.Measure.RTPMEngineType.Length > 0))
                    {
                        if (expMeasure.Measure.RTPMEngineType.CompareTo("Parameter") == 0)
                        {
                            return RTPMEType.Paramter;
                        }
                        else if (expMeasure.Measure.RTPMEngineType.CompareTo("Trigger") == 0)
                        {
                            return RTPMEType.Trigger;
                        }
                    }
                }
                else if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.ExperimentEntityID > 0) &&
                    (expMeasure.ExperimentEntity.Name.CompareTo(levelName) == 0) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0)
                    )
                {
                    if ((expMeasure.Measure.RTPMEngineType != null) && (expMeasure.Measure.RTPMEngineType.Length > 0))
                    {
                        if (expMeasure.Measure.RTPMEngineType.CompareTo("Parameter") == 0)
                        {
                            return RTPMEType.Paramter;
                        }
                        else if (expMeasure.Measure.RTPMEngineType.CompareTo("Trigger") == 0)
                        {
                            return RTPMEType.Trigger;
                        }
                    }
                }
            }

            return RTPMEType.Measure;
        }

        public bool GetMeasureInfo(int measureID, out string measureName, out string measureFactor,
                    out string measureLevel)
        {
            measureName = null;
            measureFactor = null;
            measureLevel = null;

            return false;
        }

        public void AddConfigBlockedFactor(string blockedFactorName, string blockedFactorLevel, int measureID)
        {
            Boolean blockedFactorInDisplay = false;

            foreach (DisplayBlockedFactor bf in CurConfigDisplay.DisplayBlockedFactors)
            {
                if (bf.MeasureID == measureID)
                {
                    // This factor is already in the config
                    blockedFactorInDisplay = true;

                    // Upade the level name
                    bf.LevelName = blockedFactorLevel;
                    break;
                }
            }
            if (!blockedFactorInDisplay)
            {
                DisplayBlockedFactor newDisplayBlockedFactor = new DisplayBlockedFactor();
                newDisplayBlockedFactor.MeasureID = measureID;
                newDisplayBlockedFactor.LevelName = blockedFactorLevel;

                CurConfigDisplay.DisplayBlockedFactors.Add(newDisplayBlockedFactor);
            }

        }

        public void RemoveConfigBlockedFactor(string factorName)        
        {

            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(CurConfigDisplay.MeasureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0))
                {
                    foreach (DisplayBlockedFactor bf in CurConfigDisplay.DisplayBlockedFactors)
                    {
                        if (bf.MeasureID == expMeasure.Measure.MeasureID)
                        {
                            // Remove this factor
                            CurConfigDisplay.DisplayBlockedFactors.Remove(bf);
                            break;
                        }
                    }
                }
            }
        }

        public void RemoveConfigBlockedFactor(string blockedFactorName, string blockedFactorLevel)
        {
            int measureID = LocateMeasureID(CurConfigDisplay.MeasureName, blockedFactorName, blockedFactorLevel);

            if (measureID <= 0)
            {
                return;
            }

            foreach (DisplayBlockedFactor bf in CurConfigDisplay.DisplayBlockedFactors)
            {
                if (bf.MeasureID == measureID)
                {
                    // Remove this factor
                    CurConfigDisplay.DisplayBlockedFactors.Remove(bf);
                    break;
                }
            }
        }

        public void ClearConfigDisplay()
        {
            CurConfigDisplay.MeasureName = null;
            CurConfigDisplay.MetricName = null;
            CurConfigDisplay.Name = null;
            CurConfigDisplay.NumFactors = 0;
            CurConfigDisplay.NumBlockedFactors = 0;
            CurConfigDisplay.Display = null;
            CurConfigDisplay.DisplayFactors.Clear();
            CurConfigDisplay.DisplayBlockedFactors.Clear();
            CurConfigDisplay.Name = null;
        }

        public Boolean AddCurrentDisplay()
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    // Copy data into new object
                    ConfigDisplay newConfigDisplay = new ConfigDisplay
                    {
                        Name = CurConfigDisplay.Name,
                        ConfigID = CurConfig.ConfigID,
                        DisplayID = CurConfigDisplay.Display.DisplayID,
                        MeasureName = CurConfigDisplay.MeasureName,
                        MetricName = CurConfigDisplay.MetricName,
                        NumFactors = CurConfigDisplay.NumFactors,
                        NumBlockedFactors = CurConfigDisplay.NumBlockedFactors,
                        Width = CurConfigDisplay.Width,
                        Height = CurConfigDisplay.Height
                    };

                    // Copy DisplayFactors to new object
                    foreach (DisplayFactor factor in CurConfigDisplay.DisplayFactors)
                    {
                        DisplayFactor newDisplayFactor = new DisplayFactor
                        {
                            FactorName = factor.FactorName,
                            FactorLabel = factor.FactorLabel,
                            FactorPos = factor.FactorPos
                        };
                        newConfigDisplay.DisplayFactors.Add(newDisplayFactor);
                    }

                    // Copy DisplayBlockedFactors to new object
                    foreach (DisplayBlockedFactor blockedFactor in CurConfigDisplay.DisplayBlockedFactors)
                    {
                        DisplayBlockedFactor newDisplayBlockedFactor = new DisplayBlockedFactor
                        {
                            MeasureID = blockedFactor.MeasureID,
                            LevelName = blockedFactor.LevelName
                        };
                        newConfigDisplay.DisplayBlockedFactors.Add(newDisplayBlockedFactor);
                    }

                    db.ConfigDisplays.InsertOnSubmit(newConfigDisplay);
                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Display Creation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public ObservableCollection<ConfigDisplay> LoadConfigDisplays()
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            ObservableCollection<ConfigDisplay> displayConfigsCol = null;

            if (CurConfig == null)
            {
                return null;
            }

            // All Users in an experiment
            var diplayConfigs = from cd in db.ConfigDisplays
                            where cd.ConfigID == CurConfig.ConfigID
                            select cd;

            displayConfigsCol = new ObservableCollection<ConfigDisplay>();
            diplayConfigs.ToList().ForEach(x => displayConfigsCol.Add(x));
            return displayConfigsCol;

        }

        public Boolean RemoveCurrentDisplay()
        {
            // Remove the current display if it already exists
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            ConfigDisplay existingConfigDisplay = null;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from cd in db.ConfigDisplays
                                where cd.ConfigID == CurConfig.ConfigID &&
                                cd.Name == CurConfigDisplay.Name
                                select cd;

                    try
                    {
                        existingConfigDisplay = query.Single();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    // Delete any contained DisplayFactors
                    foreach (DisplayFactor factor in existingConfigDisplay.DisplayFactors)
                    {
                        db.DisplayFactors.DeleteOnSubmit(factor);
                    }

                    // Delete any contained DisplayBlockedFactors
                    foreach (DisplayBlockedFactor blockedFactor in existingConfigDisplay.DisplayBlockedFactors)
                    {
                        db.DisplayBlockedFactors.DeleteOnSubmit(blockedFactor);
                    }

                    // Delete this ConfigDisplay
                    db.ConfigDisplays.DeleteOnSubmit(existingConfigDisplay);

                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Display Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public Boolean DeleteConfigDisplay(ConfigDisplay configDisplay)
        {
            // Remove the current display if it already exists
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            ConfigDisplay existingConfigDisplay = null;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from cd in db.ConfigDisplays
                                where cd.ConfigID == CurConfig.ConfigID &&
                                cd.Name == configDisplay.Name
                                select cd;

                    try
                    {
                        existingConfigDisplay = query.Single();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    // Delete any contained DisplayFactors
                    foreach (DisplayFactor factor in existingConfigDisplay.DisplayFactors)
                    {
                        db.DisplayFactors.DeleteOnSubmit(factor);
                    }

                    // Delete any contained DisplayBlockedFactors
                    foreach (DisplayBlockedFactor blockedFactor in existingConfigDisplay.DisplayBlockedFactors)
                    {
                        db.DisplayBlockedFactors.DeleteOnSubmit(blockedFactor);
                    }

                    // Delete this ConfigDisplay
                    db.ConfigDisplays.DeleteOnSubmit(existingConfigDisplay);

                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Display Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public bool UpdateConfigDisplay(ConfigDisplay configDisplay, int width, int height)
        {
            if (configDisplay == null)
            {
                return false;
            }

            // Obtain the edited display display
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();
            ConfigDisplay existingConfigDisplay = null;

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var query = from cd in db.ConfigDisplays
                                where cd.ConfigID == configDisplay.ConfigID &&
                                cd.Name == configDisplay.Name
                                select cd;

                    try
                    {
                        existingConfigDisplay = query.Single();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    // Update the size if needed
                    if ((existingConfigDisplay.Width != width) || (existingConfigDisplay.Height != height))
                    {
                        existingConfigDisplay.Width = width;
                        existingConfigDisplay.Height = height;
                        db.SubmitChanges();
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Display Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public List<string> GetFactorLevels(string measureName, string factorToSum)
        {
            List<string> factorLevels = new List<string>();

            // Loop through the experiments measures looking for experiment entities that match
            foreach (ExperimentMeasure expMeasure in ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Name != null) &&
                    (expMeasure.Measure.Category != null) &&
                    (expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory != null) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorToSum) == 0) &&
                    ((expMeasure.ExperimentEntityID.HasValue) && (expMeasure.ExperimentEntityID != 0))
                    )
                {
                    factorLevels.Add(expMeasure.ExperimentEntity.Name);
                }
            }

            // Loop through measure table looking for measures that match
            if (factorLevels.Count == 0)
            {
                VisDashboardDataDataContext db = new VisDashboardDataDataContext();

                var query = from p in db.Measures select p;

                foreach (var measure in query)
                {
                    if ((measure.Name != null) &&
                        (measure.Category != null) &&
                        (measure.Category.CompareTo(measureName) == 0) &&
                        (measure.SubCategory != null) &&
                        (measure.SubCategory.CompareTo(factorToSum) == 0))
                    {
                        factorLevels.Add(measure.Name);
                    }
                }
            }

            if (factorLevels.Count == 0)
            {
                return null;
            }

            // Remove any all or team
            List<string> factorsToRemove = new List<string>();

            foreach (string factorLevel in factorLevels)
            {
                if ((factorLevel.CompareTo("All") == 0) || (factorLevel.CompareTo("Team") == 0))
                {
                    factorsToRemove.Add(factorLevel);
                }
            }

            factorsToRemove.ForEach(x => factorLevels.Remove(x));

            return factorLevels;
        }

    }

    public class CreateConfigInfo
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

        private int _experimentID = 0;

        public int ExperimentID
        {
            get { return _experimentID; }
            set
            {
                _experimentID = value;
            }
        }
    }

    public class BlockedFactorItem : INotifyPropertyChanged
    {
        private string factorName = null;

        private int measureID;

        public int MeasureID
        {
            get { return measureID; }
            set { measureID = value; OnPropertyChanged("MeasureID"); }
        }
       
        public string FactorName
        {
          get { return factorName; }
          set { factorName = value; OnPropertyChanged("FactorName"); }
        }

        private ObservableCollection<String> factorLevelNames;

        public ObservableCollection<String> FactorLevelNames
        {
            get { return factorLevelNames; }
            set { factorLevelNames = value; OnPropertyChanged("FactorLevelNames"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class FactorItem : INotifyPropertyChanged
    {
        private string displayFactorLabel = null;

        public string DisplayFactorLabel
        {
            get { return displayFactorLabel; }
            set { displayFactorLabel = value; OnPropertyChanged("DisplayFactorLabel"); }
        }

        private int factorPos = 0;

        public int FactorPos
        {
            get { return factorPos; }
            set { factorPos = value; OnPropertyChanged("FactorPos"); }
        }

        private ObservableCollection<String> factorNames;

        public ObservableCollection<String> FactorNames
        {
            get { return factorNames; }
            set { factorNames = value; OnPropertyChanged("FactorNames"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
