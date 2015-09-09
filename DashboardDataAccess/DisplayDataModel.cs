using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Transactions;
using System.Windows;
using System.Data.SqlClient;

namespace DashboardDataAccess
{
    public class DisplaySelection : INotifyPropertyChanged
    {
        #region DisplayName

        private string _displayName = null;

        /// <summary>
        /// Gets or sets the DisplayName property. This observable property 
        /// indicates this name of the display
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    string old = _displayName;
                    _displayName = value;
                    RaisePropertyChanged("DisplayName");
                }
            }
        }
        #endregion

        #region All

        private RadioButtonType _all;

        /// <summary>
        /// Gets or sets the All property. This observable property 
        /// indicates ....
        /// </summary>
        public RadioButtonType All
        {
            get { return _all; }
            set
            {
                if (_all != value)
                {
                    _all = value;
                    RaisePropertyChanged("All");
                }
            }
        }

        #endregion

        #region DisplayID

        private int _displayID = 0;

        /// <summary>
        /// Gets or sets the DisplayID property. This observable property 
        /// indicates the id of this display.
        /// </summary>
        public int DisplayID
        {
            get { return _displayID; }
            set
            {
                if (_displayID != value)
                {
                    _displayID = value;
                    RaisePropertyChanged("DisplayID");
                }
            }
        }

        #endregion

        #region ImageResourcePath

        private String _imageResourcePath = null;

        /// <summary>
        /// Gets or sets the ImageResourcePath property. This observable property 
        /// indicates the locate of the image resource for this display.
        /// </summary>
        public String ImageResourcePath
        {
            get { return _imageResourcePath; }
            set
            {
                if (_imageResourcePath != value)
                {
                    _imageResourcePath = value;
                    RaisePropertyChanged("ImageResourcePath");
                }
            }
        }

        #endregion

        
        public RadioButtonType this[int index]
        {
            get { return operatorArray[index]; }
            set { operatorArray[index] = value; RaisePropertyChanged("Item[]"); }
        }

        private RadioButtonType[] operatorArray;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public DisplaySelection(string displayName, int numOperators, int displayID, String imageResourcePath)
        {
            this.DisplayName = displayName;
            this.operatorArray = new RadioButtonType[numOperators];
            this.DisplayID = displayID;
            this.ImageResourcePath = "/DashboardpermissionTool;Component/" + imageResourcePath;
        }

    }

    public class DisplayDataModel : ObservableCollection<DisplaySelection>
    {
        private int numOperators = 3;

        public int NumOperators
        {
            get { return numOperators; }
            set { numOperators = value; }
        }

        public DisplayDataModel()
        {
            CreateDisplayDataModel(0);
        }

        public DisplayDataModel(int pNumOperators)
        {
            CreateDisplayDataModel(pNumOperators);
        }

        public void CreateDisplayDataModel(int pNumOperators)
        {
            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            // Clear the existing list
            Clear();

            // Setup number of operators
            if (pNumOperators > 0)
            {
                numOperators = pNumOperators;
            }

            var query = from d in db.Displays select d;

            foreach (var item in query)
            {
                Add(new DisplaySelection(item.Name, numOperators, item.DisplayID, item.ImageResourcePath));
            }

        }

        public void LoadExperiementDisplays(Experiment currentExperiment,
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
            var query = from ed in db.ExperimentDisplays
                        where ed.ExperimentID == currentExperiment.ExperimentID
                        select ed;
            ExperimentDisplay[] expDisplays = query.ToArray();

            // Clear Radio buttons
            ClearRadioButtons();

            // Fill out the radio button table
            for (int i = 0; i < expDisplays.Length; i++)
            {
                for (int j = 0; j < this.Count; j++)
                {
                    if (this[j].DisplayID == expDisplays[i].DisplayID)
                    {
                        for (int z = 0; z < experimentUsers.Count; z++)
                        {
                            if (experimentUsers[z].UserID == expDisplays[i].UserID)
                            {
                                // Set the radio button
                                this[j][z] = RadioButtonType.On;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            // Recalculate all rollups
            RefreshAllCol();

        }

        private void ClearRadioButtons()
        {
            // Clear the radio buttion table
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < numOperators; j++)
                {
                    // Clear the radio button
                    this[i][j] = RadioButtonType.Off;
                }
            }

            // Recalculate all rollups
            RefreshAllCol();

        }

        public void RefreshAllCol()
        {
            for (int i = 0; i < this.Count; i++)
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

        public void SaveExperiementDisplays(Experiment currentExperiment,
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
                    var itemsToDelete = from em in db.ExperimentDisplays
                                        where em.ExperimentID == currentExperiment.ExperimentID
                                        select em;
                    db.ExperimentDisplays.DeleteAllOnSubmit(itemsToDelete);

                    // Loop through the data in radio button table
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (this[i].DisplayID > 0)
                        {
                            for (int j = 0; j < experimentUsers.Count; j++)
                            {
                                if (this[i][j] == RadioButtonType.On)
                                {
                                    db.ExperimentDisplays.InsertOnSubmit(new ExperimentDisplay
                                    {
                                        ExperimentID = currentExperiment.ExperimentID,
                                        DisplayID = this[i].DisplayID,
                                        UserID = experimentUsers[j].UserID
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
                    MessageBox.Show("Database Error: " + e.Message, "Failed to save database displays", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public void ResetExperiementDisplays(Experiment currentExperiment,
            ObservableCollection<User> experimentUsers)
        {
            // Clear the selections
            ClearRadioButtons();

            // Save the selections in the database
            SaveExperiementDisplays(currentExperiment, experimentUsers);
        }

        public void DeleteExperimentDisplays(Experiment experiment)
        {
            if (experiment == null)
            {
                return;
            }

            VisDashboardDataDataContext db = new VisDashboardDataDataContext();

            var oldExperimentMeasures = from ed in db.ExperimentDisplays
                                        where ed.ExperimentID == experiment.ExperimentID
                                        select ed;

            db.ExperimentDisplays.DeleteAllOnSubmit(oldExperimentMeasures);
            db.SubmitChanges();
        }

    }
}
