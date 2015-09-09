using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Collections;

namespace GME.Views.View_Components {

    public partial class GraphSetupTabPage : UserControl {

        public delegate void EventHandler(object sender, EventArgs ev);
        public event EventHandler ActivateEvent;

        private int index = -1;
        private Dictionary<int, DataTable> tData = new Dictionary<int, DataTable>();
        private String[] tableColumn = null;

        public GraphSetupTabPage(int index, Dictionary<int, DataTable> tData) {

            this.index = index;
            InitializeComponent();

            this.tData = tData;
            this.tableColumn = this.getDataColumnNames();
            this.typeToGraphCombo.Items.AddRange(this.tableColumn);
        }

        public int getIndex() {
            return this.index;
        }

        public bool isDisplayed() {
            return this.activateCheckBox.Checked;
        }

        public ChartViewData getGraphingData() {

            if (!this.isDisplayed()) {
                return null;
            }
            else if (this.simRunAxisCheckBox.Checked == true)
                return this.getChartDataSimRunAsX();
            else
                return this.getChartData();
        }

        private ChartViewData getChartData() {

            String title = "Graph " + (this.index + 1);
            String typeToGraph = "";
            String xLabel = "";
            String yLabel = "";
            try {
                typeToGraph = this.typeToGraphCombo.SelectedItem.ToString();
                xLabel = this.xAxisCombo.SelectedItem.ToString();
                yLabel = this.yAxisCombo.SelectedItem.ToString();
            }
            catch (NullReferenceException) {
                return null;
            }

            List<double> x = new List<double>();
            List<double> y = null;
            Dictionary<string, double[]> yData = new Dictionary<string, double[]>();
            DataTable table = null;
            foreach (String checkedItemName in this.itemsToGraphListBox.CheckedItems) {

                foreach (int key in this.tData.Keys) {

                    table = this.tData[key];
                    y = new List<double>();
                    String prefix = String.Empty;

                    foreach (DataRow row in table.Rows) {

                        if (row[typeToGraph].ToString().CompareTo(checkedItemName) == 0) {

                            try {
                                double xv = Double.Parse(row[xLabel].ToString());
                                if (!x.Contains(xv)) {
                                    x.Add(xv);
                                }

                                y.Add(Double.Parse(row[yLabel].ToString()));

                            }
                            catch (FormatException) {
                                throw new FormatException("Data you chose to graph is not numeric");
                            }
                        }
                    }

                    if (this.tData.Count > 1) {
                        prefix = "SimRun " + key + ": ";
                    }
                    yData.Add(prefix + checkedItemName, y.ToArray());
                }
            }

            double[] xData = x.ToArray();
            return new ChartViewData(title, xLabel, xData, yLabel, yData);
        }

        private ChartViewData getChartDataSimRunAsX() {

            String title = "Graph " + (this.index + 1);
            String typeToGraph = "";
            String xLabel = "";
            String yLabel = "";
            try {
                typeToGraph = this.typeToGraphCombo.SelectedItem.ToString();
                xLabel = this.xAxisCombo.SelectedItem.ToString();
                yLabel = this.yAxisCombo.SelectedItem.ToString();
            }
            catch (NullReferenceException) {
                return null;
            }

            DataTable table = null;
            SortedDictionary<int, String[][]> allData = new SortedDictionary<int,String[][]>();
            List<String[]> dataPoints = null;
            String[] dataPoint = null;

            foreach (int key in this.tData.Keys) {

                table = this.tData[key];
                dataPoints = new List<String[]>();
                foreach (String checkedItemName in this.itemsToGraphListBox.CheckedItems) {

                    foreach (DataRow row in table.Rows) {

                        if (row[typeToGraph].ToString().CompareTo(checkedItemName) == 0) {

                            dataPoint = new String[3];
                            dataPoint[0] = checkedItemName + ": " + xLabel + " " + row[xLabel]; //CTL: Cycle 1 
                            dataPoint[1] = row[xLabel].ToString(); //1 - for now unused field
                            dataPoint[2] = row[yLabel].ToString(); //82
                            dataPoints.Add(dataPoint);
                        }
                    }
                }
                allData.Add(key, dataPoints.ToArray());
            }

            double[] xValues = new double[allData.Count];
            Dictionary<String, List<double>> yTemp = new Dictionary<string, List<double>>();
            int i = 0;
            foreach (int key in allData.Keys) {
                xValues[i++] = key;

                foreach (String[] array in allData[key]) {
                    if (!yTemp.ContainsKey(array[0]))
                        yTemp.Add(array[0], new List<double>());

                    try {
                        yTemp[array[0]].Add(Double.Parse(array[2]));
                    }
                    catch (FormatException) {    
                        throw new FormatException("Data you chose to graph is not numeric");
                    }
                }
            }

            Dictionary<String, double[]> yValues = new Dictionary<string, double[]>();
            foreach (String key in yTemp.Keys) {
                yValues.Add(key, yTemp[key].ToArray());
            }

            return new ChartViewData(
                title, "SimRun", xValues, yLabel, yValues);
        }

        private String[] getDataColumnNames() {

            List<String[]> columns = new List<String[]>();
            String[] names = null;
            foreach (DataTable table in this.tData.Values) {

                int i = 0;
                names = new String[table.Columns.Count];
                foreach (DataColumn c in table.Columns) {
                    names[i++] = c.Caption;
                }

                columns.Add(names);
            }

            return this.getAllCommonItems(columns);
        }

        /// <summary>
        /// Removes the duplicate items and empty Strings in each individual 
        /// String[] and then returns only the items that are present in EACH 
        /// of the Sting[] of the List.  Returned items are sorted.
        /// </summary>
        private String[] getAllCommonItems(List<String[]> list) {

            SortedList<String, int> bigList = new SortedList<string, int>();
            List<String> smallList = null;

            foreach (String[] item in list) {

                smallList = new List<string>();

                //build list of all unique elements of each String array by discarding duplicates
                foreach (String element in item) {
                    if (element.Trim().CompareTo(String.Empty) != 0 && !smallList.Contains(element))
                        smallList.Add(element);
                }

                //add them to the bigList and keep count of the number
                //of String arrays they are present at
                foreach (String str in smallList) {

                    if (!bigList.ContainsKey(str))
                        bigList.Add(str, 1); //add new items with count = 1
                    else
                        ++bigList[str]; //item exists, increment the count by 1 
                }
            }

            //if this value is not present in ALL String arrays - discard it
            List<String> keys = new List<string>(bigList.Keys);
            foreach (String key in keys) {
                if (bigList[key] != this.tData.Count)
                    bigList.Remove(key);
            }

            String[] finalArray = new String[bigList.Keys.Count];
            bigList.Keys.CopyTo(finalArray, 0);
            Array.Sort(finalArray);
            return finalArray;
        }

        private void this_OnEnabledChange(object sender, EventArgs e) {
            if (this.Enabled == false) {
                this.activateCheckBox.Checked = false;
                this.typeToGraphCombo.SelectedIndex = -1;
                this.xAxisCombo.SelectedIndex = -1;
                this.simRunAxisCheckBox.Checked = false;
                this.yAxisCombo.SelectedIndex = -1;
                this.itemsToGraphListBox.Items.Clear();
            }
        }

        private void activateCheckBox_CheckedChanged(object sender, EventArgs e) {
            ActivateEvent(this, new ActivatePanelEvent(this.activateCheckBox.Checked));
        }

        private void typeToGraphCombo_SelectedIndexChanged(object sender, EventArgs e) {

            this.itemsToGraphListBox.Items.Clear();
            this.xAxisCombo.Items.Clear();
            this.yAxisCombo.Items.Clear();
            this.simRunAxisCheckBox.Checked = false;

            try {

                String selectedType = this.typeToGraphCombo.SelectedItem.ToString();
                List<String[]> bigList = new List<string[]>(this.tData.Count);
                List<String> tempList = null;
                foreach (DataTable table in this.tData.Values) {
                    tempList = new List<string>();
                    foreach (DataRow row in table.Rows) {
                        tempList.Add(row[selectedType].ToString());
                    }
                    bigList.Add(tempList.ToArray());
                }

                String[] typeArray = this.getAllCommonItems(bigList);
                this.itemsToGraphListBox.Items.AddRange(typeArray);

                this.xAxisCombo.Items.AddRange(this.tableColumn);
                this.xAxisCombo.Items.Remove(selectedType);
                this.yAxisCombo.Items.AddRange(this.tableColumn);
                this.yAxisCombo.Items.Remove(selectedType);
            }
            catch (NullReferenceException) {
                //this.typeToGraphCombo.SelectedItem == null
            }
            finally {
                this.xAxisCombo.Enabled = true;
                this.yAxisCombo.Enabled = true;
                this.itemsToGraphListBox.Enabled = true;

                if (this.tData.Count > 1) {
                    this.simRunAxisCheckBox.Enabled = true;
                }
            }
        }
    }

    public class ActivatePanelEvent : EventArgs {
        private bool activate = false;

        public ActivatePanelEvent(bool activate) {
            this.activate = activate;
        }

        public bool isActive() {
            return this.activate;
        }
    }

    public class ChartViewData {

        private string title = "";
        private string xAxisLabel = "";
        private string yAxisLabel = "";
        private double[] xValues = { };
        private Dictionary<string, double[]> yValues = new Dictionary<string, double[]>();

        public ChartViewData(String title, String xAxisLabel, double[] xValues,
            String yAxisLabel, Dictionary<string, double[]> yValues) {

            this.title = title;
            this.xAxisLabel = xAxisLabel;
            this.xValues = xValues;
            this.yAxisLabel = yAxisLabel;
            this.yValues = yValues;
        }

        public String Title {
            get {
                return this.title;
            }
        }

        public String XAxisLabel {
            get {
                return this.xAxisLabel;
            }
        }

        public String YAxisLabel {
            get {
                return this.yAxisLabel;
            }
        }

        public double[] XValues {
            get {
                return this.xValues;
            }
        }

        public Dictionary<string, double[]> YValues {
            get {
                return this.yValues;
            }
        }

    }
}
