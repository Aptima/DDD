using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows;

namespace DashboardDataAccess
{
    public enum MeasureRowType
    {
        DataValue,
        CategoryType,
        SubCategoryType
    }

    public enum RadioButtonType
    {
        Off,
        Partial,
        On
    }

    public class MeasureSelection : INotifyPropertyChanged
    {
        private string measureName;
        private string category;
        private string subCategory;
        private bool visibilityMain;
        private bool visibilitySub;
        private MeasureRowType measureRowType;
        private bool expandedFlag;
        private RadioButtonType all;
        private RadioButtonType [] operatorArray;
        private int experimentEntityID;
        private bool measureDisabled;

        public MeasureSelection(string measureName, string category, string subCategory,
            MeasureRowType rowType, int numOperators, int measureID)
        {
            this.MeasureName = measureName;
            this.Category = category;
            this.SubCategory = subCategory;
            this.visibilityMain = true;
            this.visibilitySub = true;
            this.measureRowType = rowType;
            this.expandedFlag = true;
            this.operatorArray = new RadioButtonType[numOperators];
            this.MeasureID = measureID;
            this.experimentEntityID = 0;
            this.measureDisabled = false;
        }

        public string MeasureName
        {
            get { return measureName; }
            set { measureName = value; OnPropertyChanged("MeasureName"); }
        }

        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged("Category"); }
        }

        public string SubCategory
        {
            get { return subCategory; }
            set { subCategory = value; OnPropertyChanged("SubCategory"); }
        }

        public bool VisibilityMain
        {
            get { return visibilityMain; }
            set { visibilityMain = value; OnPropertyChanged("VisibilityMain"); }
        }

        public bool VisibilitySub
        {
            get { return visibilitySub; }
            set { visibilitySub = value; OnPropertyChanged("VisibilitySub"); }
        }

        public MeasureRowType MeasureRowType
        {
            get { return measureRowType; }
            set { measureRowType = value; OnPropertyChanged("MeasureRowType"); }
        }

        public bool ExpandedFlag
        {
            get { return expandedFlag; }
            set { expandedFlag = value; OnPropertyChanged("ExpandedFlag"); }
        }

        public RadioButtonType All
        {
            get { return all; }
            set { all = value; OnPropertyChanged("All"); }
        }

        public RadioButtonType this[int index]
        {
            get { return operatorArray[index]; }
            set { operatorArray[index] = value; OnPropertyChanged("Item[]"); }
        }

        private int measureID;

        public int MeasureID
        {
            get { return measureID; }
            set { measureID = value; OnPropertyChanged("MeasureID"); }
        }

        public int ExperimentEntityID
        {
            get { return experimentEntityID; }
            set { experimentEntityID = value; OnPropertyChanged("ExperimentEntityID"); }
        }

        public bool MeasureDisabled
        {
            get { return measureDisabled; }
            set { measureDisabled = value; OnPropertyChanged("MeasureDisabled"); }
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

    public class MeasuresDataModel: ObservableCollection<MeasureSelection>
    {
        private int numOperators = 3;

        public int NumOperators
        {
            get { return numOperators; }
            set { numOperators = value; }
        }

        private ObservableCollection<MeasureSelection> fullMeasureCollection = null;

        public MeasuresDataModel()
        {
            CreateMeasuresDataModel(0);
        }

        public MeasuresDataModel(int pNumOperators)
        {
            CreateMeasuresDataModel(pNumOperators);
        }

        public void CreateMeasuresDataModel(int pNumOperators)
        {
            string savedCategory = null;
            string savedSubCategory = null;
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            // Setup number of operators
            if (pNumOperators > 0)
            {
                numOperators = pNumOperators;
            }

            var query = from p in db.Measures select p;

            fullMeasureCollection = new ObservableCollection<MeasureSelection>();
            foreach (var item in query)
            {
                if (string.Compare(savedCategory, item.Category) != 0)
                {
                    fullMeasureCollection.Add(new MeasureSelection(item.Category, "", "", MeasureRowType.CategoryType, numOperators, -1));
                    savedCategory = item.Category;
                }
                if (string.Compare(savedSubCategory, item.SubCategory) != 0)
                {
                    fullMeasureCollection.Add(new MeasureSelection(item.SubCategory, item.Category, "", MeasureRowType.SubCategoryType, numOperators, -1));
                    savedSubCategory = item.SubCategory;
                }
                fullMeasureCollection.Add(new MeasureSelection(item.Name, item.Category, item.SubCategory,
                    MeasureRowType.DataValue, numOperators, item.MeasureID));
            }

            // Update Visible collection
            UpdateVisMeasureCollection();
        }

        public ObservableCollection<MeasureSelection> GetAllData()
        {
            return fullMeasureCollection;
        }

        public void UpdateVisMeasureCollection()
        {
            if (fullMeasureCollection == null)
            {
                return;
            }

            // Clear the existing list
            Clear();

            foreach (MeasureSelection item in fullMeasureCollection)
            {
                if (item.VisibilityMain && item.VisibilitySub && !item.MeasureDisabled)
                {
                    Add(item);
                }
            }                
        }

        public void RefreshAllCol()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].MeasureRowType == MeasureRowType.DataValue)
                {
                    bool hasOffValue = false;
                    bool hasOnValue = false;

                    for (int j = 0; j < this.numOperators; j++)
                    {
                        if (this[i][j] == RadioButtonType.Off)
                        {
                            hasOffValue = true;
                        }
                        else if (this[i][j] == RadioButtonType.On)
                        {
                            hasOnValue = true;
                        }
                    }

                    if ((hasOnValue) && (hasOffValue))
                    {
                        this[i].All = RadioButtonType.Partial;
                    }
                    else if (hasOnValue)
                    {
                        this[i].All = RadioButtonType.On;
                    }
                    else
                    {
                        this[i].All = RadioButtonType.Off;
                    }
                }
            }
        }

        public void RefreshSubCategory()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].MeasureRowType == MeasureRowType.SubCategoryType)
                {
                    bool hasOffValue = false;
                    bool hasOnValue = false;
                    bool hasPartialValue = false;

                    string subCategoryName = this[i].MeasureName;
                    string categoryName = this[i].Category;

                    // Handle other columns
                    for (int z = 0; z < numOperators; z++)
                    {
                        hasOffValue = false;
                        hasOnValue = false;
                        hasPartialValue = false;

                        for (int j = 0; j < this.Count; j++)
                        {
                            if ((this[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(this[j].SubCategory, subCategoryName) == 0) &&
                                (string.Compare(this[j].Category, categoryName) == 0))
                            {
                                if (this[j][z] == RadioButtonType.Off)
                                {
                                    hasOffValue = true;
                                }
                                else if (this[j][z] == RadioButtonType.On)
                                {
                                    hasOnValue = true;
                                }
                                else if (this[j][z] == RadioButtonType.Partial)
                                {
                                    hasPartialValue = true;
                                }
                            }
                        }

                        if (((hasOnValue) && (hasOffValue))  || (hasPartialValue))
                        {
                            this[i][z] = RadioButtonType.Partial;
                        }
                        else if (hasOnValue)
                        {
                            this[i][z] = RadioButtonType.On;
                        }
                        else
                        {
                            this[i][z] = RadioButtonType.Off;
                        }
                    }

                    // Handle All Column
                    hasOffValue = false;
                    hasOnValue = false;
                    hasPartialValue = false;
                    for (int j = 0; j < this.Count; j++)
                    {
                        if ((this[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(this[j].SubCategory, subCategoryName) == 0)
                            && (string.Compare(this[j].Category, categoryName) == 0))
                        {
                            if (this[j].All == RadioButtonType.Off)
                            {
                                hasOffValue = true;
                            }
                            else if (this[j].All == RadioButtonType.On)
                            {
                                hasOnValue = true;
                            }
                            else if (this[j].All == RadioButtonType.Partial)
                            {
                                hasPartialValue = true;
                            }
                        }
                    }

                    if (((hasOnValue) && (hasOffValue)) || (hasPartialValue))
                    {
                        this[i].All = RadioButtonType.Partial;
                    }
                    else if (hasOnValue)
                    {
                        this[i].All = RadioButtonType.On;
                    }
                    else
                    {
                        this[i].All = RadioButtonType.Off;
                    }

                }
            }
        }

        public void RefreshCategory()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].MeasureRowType == MeasureRowType.CategoryType)
                {
                    bool hasOffValue = false;
                    bool hasOnValue = false;
                    bool hasPartialValue = false;
                    string categoryName = this[i].MeasureName;

                    // Handle other columns
                    for (int z = 0; z < numOperators; z++)
                    {
                        hasOffValue = false;
                        hasOnValue = false;
                        hasPartialValue = false;

                        for (int j = 0; j < this.Count; j++)
                        {
                            if ((this[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(this[j].Category, categoryName) == 0))
                            {
                                if (this[j][z] == RadioButtonType.Off)
                                {
                                    hasOffValue = true;
                                }
                                else if (this[j][z] == RadioButtonType.On)
                                {
                                    hasOnValue = true;
                                }
                                else if (this[j][z] == RadioButtonType.Partial)
                                {
                                    hasPartialValue = true;
                                }
                            }
                        }

                        if (((hasOnValue) && (hasOffValue)) || (hasPartialValue))
                        {
                            this[i][z] = RadioButtonType.Partial;
                        }
                        else if (hasOnValue)
                        {
                            this[i][z] = RadioButtonType.On;
                        }
                        else
                        {
                            this[i][z] = RadioButtonType.Off;
                        }
                    }

                    // Handle All Column
                    hasOffValue = false;
                    hasOnValue = false;
                    hasPartialValue = false;
                    for (int j = 0; j < this.Count; j++)
                    {
                        if ((this[j].MeasureRowType == MeasureRowType.DataValue) && (string.Compare(this[j].Category, categoryName) == 0))
                        {
                            if (this[j].All == RadioButtonType.Off)
                            {
                                hasOffValue = true;
                            }
                            else if (this[j].All == RadioButtonType.On)
                            {
                                hasOnValue = true;
                            }
                            else if (this[j].All == RadioButtonType.Partial)
                            {
                                hasPartialValue = true;
                            }
                        }
                    }

                    if (((hasOnValue) && (hasOffValue)) || (hasPartialValue))
                    {
                        this[i].All = RadioButtonType.Partial;
                    }
                    else if (hasOnValue)
                    {
                        this[i].All = RadioButtonType.On;
                    }
                    else
                    {
                        this[i].All = RadioButtonType.Off;
                    }
                }
            }
        }

        public void LoadExperiementMeasures(Experiment currentExperiment,
            ObservableCollection<User> experimentUsers)
        {
            VisDashboardDataDataContext db = null;
            
            // Parameter checking
            if ((currentExperiment == null) || (experimentUsers == null) ||
                (experimentUsers.Count == 0))
            {
                return;
            }

            // Connect to database
            db = new VisDashboardDataDataContext();
            var query = from em in db.ExperimentMeasures
                        where em.ExperimentID == currentExperiment.ExperimentID
                        select em;
            ExperimentMeasure [] expMeasures = query.ToArray();

            // Clear Radio buttons
            ClearRadioButtons();

            // Delete Existing scenerio specific entity measures
            for (int j = 0; j < fullMeasureCollection.Count; j++)
            {
                if (fullMeasureCollection[j].ExperimentEntityID > 0)
                {
                    fullMeasureCollection.Remove(fullMeasureCollection[j]);
                    j--;
                }
            }

            // Add scenerio specific entity measures
            int numAdded = 0;
            List<string> addedEntityNames = new List<string>();
            for (int j = 0; j < fullMeasureCollection.Count; j++)
            {
                for (int i = 0; i < expMeasures.Length; i++)
                {
                    if ((fullMeasureCollection[j].MeasureID == expMeasures[i].MeasureID) && (expMeasures[i].ExperimentEntityID > 0)
                        && (addedEntityNames.Contains(expMeasures[i].ExperimentEntity.Name) != true))
                    {
                        MeasureSelection insertMeasure = new MeasureSelection(expMeasures[i].ExperimentEntity.Name, fullMeasureCollection[j].Category,
                            fullMeasureCollection[j].SubCategory,
                            MeasureRowType.DataValue, numOperators, fullMeasureCollection[j].MeasureID);
                        insertMeasure.ExperimentEntityID = (int) expMeasures[i].ExperimentEntityID;
                        fullMeasureCollection.Insert(j+numAdded+1, insertMeasure);
                        addedEntityNames.Add(expMeasures[i].ExperimentEntity.Name);
                        numAdded++;
                        fullMeasureCollection[j].MeasureDisabled = true;
                    }
                }
                j += numAdded;
                numAdded = 0;
                addedEntityNames.Clear();
            }

            // Reload the display
            UpdateVisMeasureCollection();

            // Fill out the radio button table
            for (int i = 0; i < expMeasures.Length; i++)
            {
                for (int j = 0; j < fullMeasureCollection.Count; j++)
                {
                    if ((fullMeasureCollection[j].MeasureID == expMeasures[i].MeasureID) &&
                        ((!expMeasures[i].ExperimentEntityID.HasValue) || (fullMeasureCollection[j].ExperimentEntityID == expMeasures[i].ExperimentEntityID)))
                    {
                        for (int z = 0; z < experimentUsers.Count; z++)
                        {
                            if (experimentUsers[z].UserID == expMeasures[i].UserID)
                            {
                                if ((expMeasures[i].Allowed.HasValue && (expMeasures[i].Allowed == true)) ||
                                    (expMeasures[i].Allowed.HasValue == false))
                                {
                                    // Set the radio button
                                    fullMeasureCollection[j][z] = RadioButtonType.On;
                                    break;
                                }
                            }                            
                        }
                        break;
                    }
                }
            }

            // Recalculate all rollups
            RefreshAllCol();

            // Recalculate Subcategory rollups
            RefreshSubCategory();

            // Recalculated Category rollups
            RefreshCategory();
        }

        private void ClearRadioButtons()
        {
            // Clear the radio buttion table
            for (int i = 0; i < fullMeasureCollection.Count; i++)
            {
                for (int j = 0; j < numOperators; j++)
                {
                    // Clear the radio button
                    fullMeasureCollection[i][j] = RadioButtonType.Off;
                }
            }

            // Recalculate all rollups
            RefreshAllCol();

            // Recalculate Subcategory rollups
            RefreshSubCategory();

            // Recalculated Category rollups
            RefreshCategory();
        }

        public void SaveExperiementMeasures(Experiment currentExperiment,
            ObservableCollection<User> experimentUsers)
        {
            VisDashboardDataDataContext db = null;

            // Parameter checking
            if ((currentExperiment == null) || (experimentUsers == null) ||
                (experimentUsers.Count == 0))
            {
                return;
            }

            // Connect to the database
            db = new VisDashboardDataDataContext();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    // Remove old experiment measures entries
                    var itemsToDelete = from em in db.ExperimentMeasures
                                where em.ExperimentID == currentExperiment.ExperimentID
                                select em;
                    db.ExperimentMeasures.DeleteAllOnSubmit(itemsToDelete);

                    // Loop through the data in radio button table
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (this[i].MeasureID > 0)
                        {
                            for (int j = 0; j < experimentUsers.Count; j++)
                            {
                                if (this[i][j] == RadioButtonType.On)
                                {
                                    if (this[i].ExperimentEntityID > 0)
                                    {
                                        db.ExperimentMeasures.InsertOnSubmit(new ExperimentMeasure
                                        {
                                            ExperimentID = currentExperiment.ExperimentID,
                                            MeasureID = this[i].MeasureID,
                                            UserID = experimentUsers[j].UserID,
                                            ExperimentEntityID = this[i].ExperimentEntityID,
                                            Allowed = true
                                        });
                                    }
                                    else
                                    {
                                        db.ExperimentMeasures.InsertOnSubmit(new ExperimentMeasure
                                        {
                                            ExperimentID = currentExperiment.ExperimentID,
                                            MeasureID = this[i].MeasureID,
                                            UserID = experimentUsers[j].UserID,
                                        });
                                    }
                                }
                                else if ((this[i][j] == RadioButtonType.Off) && (this[i].ExperimentEntityID > 0))
                                {
                                    db.ExperimentMeasures.InsertOnSubmit(new ExperimentMeasure
                                    {
                                        ExperimentID = currentExperiment.ExperimentID,
                                        MeasureID = this[i].MeasureID,
                                        UserID = experimentUsers[j].UserID,
                                        ExperimentEntityID = this[i].ExperimentEntityID,
                                        Allowed = false
                                    });
                                }

                            }
                        }
                    }

                    // Submit the database changes
                    db.SubmitChanges();
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Database Error: " + e.Message, "Failed to save database measures", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                finally
                {
                    // Cleanup
                    ts.Complete();
                    ts.Dispose();
                    db = null;
                }
            }
        }

        public void ResetExperiementMeasures(Experiment currentExperiment,
            ObservableCollection<User> experimentUsers)
        {
            // Clear the selections
            ClearRadioButtons();

            // Save the selections in the database
            SaveExperiementMeasures(currentExperiment, experimentUsers);
        }

        public void DeleteExperimentMeasures(Experiment experiment)
        {
            if (experiment == null)
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            var oldExperimentMeasures = from em in db.ExperimentMeasures
                              where em.ExperimentID == experiment.ExperimentID
                              select em;

            db.ExperimentMeasures.DeleteAllOnSubmit(oldExperimentMeasures);
            db.SubmitChanges();
        }
    }
}
