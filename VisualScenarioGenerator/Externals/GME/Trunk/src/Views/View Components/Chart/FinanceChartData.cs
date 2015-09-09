using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace AME.Views.View_Components
{
	/// <summary>
	/// Summary description for FinanceChartData.
	/// </summary>
	public class FinanceChartData
	{
        private Hashtable chartDataObsElementHashTable = null;
        private Hashtable chartDataObsElementSummaryHashTable = null;

        private Hashtable chartDetailDataObsElementHashTable = null;

        private Hashtable chartDataHashTable = null;
        private Hashtable chartDataNVCHashTable = null;

        public class ChartDetailDataClass
        {
            private List<string> name = null;
            private List<double> value = null;
            private List<string> nameupdated = null;
            private List<double> valueupdated = null;
            public ChartDetailDataClass()
            {
                this.name = new List<string>();
                this.value = new List<double>();
                this.nameupdated = new List<string>();
                this.valueupdated = new List<double>();
            }
            public virtual List<double> Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    this.value = value;
                }
            }
            public virtual List<string> Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }

            }
            public virtual List<double> Valueupdated
            {
                get
                {
                    return this.valueupdated;
                }
                set
                {
                    this.valueupdated = value;
                }
            }
            public virtual List<string> Nameupdated
            {
                get
                {
                    return this.nameupdated;
                }
                set
                {
                    this.nameupdated = value;
                }

            }
        }

        public class ChartDataClass 
        {
            private NameValueCollection timeData = null;
            private List<double> timeStamps = null;
		    private List<double> highData = null;
            private List<bool> realDataAtThisPoint = null;
            private List<double> iconType = null;
            private int duration = 24;
            public ChartDataClass()
            {
                this.timeStamps = new List<double>();
                this.highData = new List<double>();
                this.timeData = new NameValueCollection();
                this.iconType = new List<double>();
                this.realDataAtThisPoint = new List<bool>();
            }

            public virtual List<bool> RealDataAtThisPoint
            {
                get
                {
                    return this.realDataAtThisPoint;
                }
                set
                {
                    this.realDataAtThisPoint = value;
                }
            }
            public virtual NameValueCollection TimeData
            {
                get
                {
                    return this.timeData;
                }
                set
                {
                    this.timeData = value;
                }
            }
            public virtual List<double> TimeStamps
            {
                get
                {
                    return this.timeStamps;
                }
                set
                {
                    this.timeStamps = value;
                }
            }
            public virtual List<double> IconType
            {
                get
                {
                    return this.iconType;
                }
                set
                {
                    this.iconType = value;
                }
            }
            public virtual int Duration
            {
                get
                {
                    return this.duration;
                }
            }

            public virtual List<double> HighData
            {
                get
                {
                    return this.highData;
                }
                set
                {
                    this.highData = value;
                }
            }
        }
        
		public FinanceChartData()
		{
            chartDataObsElementHashTable = new Hashtable();
            chartDataObsElementSummaryHashTable = new Hashtable();
            chartDataHashTable = new Hashtable();
            chartDataNVCHashTable = new Hashtable();
            chartDetailDataObsElementHashTable = new Hashtable();
		}

        public ChartDetailDataClass GetDataForDetailPopup(OutputType element, String chartType, String key)
        {
            Hashtable h = null;
            if (chartDetailDataObsElementHashTable.ContainsKey(chartType))
            {
                h = (Hashtable)chartDetailDataObsElementHashTable[chartType];
                if (h.Contains(key))
                {
                    return (ChartDetailDataClass)h[key];
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public Hashtable GetHashtableForObsElement(String orgElement)
        {
            if (chartDataObsElementHashTable.ContainsKey(orgElement))
            {
                return (Hashtable)chartDataObsElementHashTable[orgElement];
            }
            return null;
        }
        public ICollection GetElementKeysForObsElementHashtable(String orgElement)
        {
            Hashtable temp;
            if (chartDataObsElementHashTable.ContainsKey(orgElement))
            {
                temp = (Hashtable)chartDataObsElementHashTable[orgElement];
                return temp.Keys;
            }
            return null;
        }
        
        public ChartDataClass GetDataForChart(string key)
        {
            if (chartDataHashTable != null)
            {
                if (chartDataHashTable.ContainsKey(key))
                    return (ChartDataClass)chartDataHashTable[key];
            }
            return null;
        }
        public ChartDataClass GetDataForNVCChart(string orgElement, string key)
        {
            Hashtable temp;
            if (chartDataObsElementHashTable.ContainsKey(orgElement))
            {
                temp = (Hashtable)chartDataObsElementHashTable[orgElement];
                if (temp.ContainsKey(key))
                {
                    return (ChartDataClass)temp[key];
                }
            }
            return null;
        }
        public double GetSummaryDataForTreeView(string orgElement, string key)
        {
             Hashtable temp;
             ChartDataClass tempData;
             if (chartDataObsElementSummaryHashTable.ContainsKey(orgElement))
             {
                 temp = (Hashtable)chartDataObsElementSummaryHashTable[orgElement];
                 if (temp.ContainsKey(key))
                 {
                     tempData = (ChartDataClass)temp[key];
                     return (double)tempData.HighData[0];
                 }
             }
            return 0.0;
        }

        public void Normalize(int durationOfModule)
        {
            Hashtable finalElementHT = new Hashtable();
            Hashtable finalSubElementHT = new Hashtable();
            Hashtable elementHashT = null;
            ChartDataClass replacementDataClass = new ChartDataClass();
            ArrayList timepoints = new ArrayList();
            for (double i = 0; i < durationOfModule; i++)
            {
                timepoints.Add(i);
            }
            ArrayList dataAtTimepoints = new ArrayList();
            ArrayList dataAtIconType = new ArrayList();
            double lastval = 0;
            double iconlastval = 1.0;
            foreach (string elementLevel in this.chartDataObsElementHashTable.Keys)
            {
                if (elementLevel != "InfoElement") // hack - skip info element data
                {
                    elementHashT = (Hashtable)this.chartDataObsElementHashTable[elementLevel];
                    finalSubElementHT = new Hashtable();
                    lastval = 0;
                    foreach (string ElementItems in elementHashT.Keys)
                    {
                        ChartDataClass tempDataClass = (ChartDataClass)elementHashT[ElementItems];
                        dataAtTimepoints = new ArrayList();
                        dataAtIconType = new ArrayList();

                        lastval = 0;
                        iconlastval = 1.0;
                        for (double i = 0; i < durationOfModule; i++)
                        {
                            if (tempDataClass.TimeStamps.Contains(i))
                            {
                                lastval = tempDataClass.HighData[tempDataClass.TimeStamps.IndexOf(i)];
                                dataAtTimepoints.Add(lastval);
                                iconlastval = tempDataClass.IconType[tempDataClass.TimeStamps.IndexOf(i)];
                                dataAtIconType.Add(iconlastval);
                            }
                            else
                            {
                                dataAtTimepoints.Add(lastval);
                                dataAtIconType.Add(0.0);
                            }
                        }

                        replacementDataClass = new ChartDataClass();
                        for (int k = 0; k < durationOfModule; k++)
                        {
                            replacementDataClass.TimeStamps.Add((double)timepoints[k]);
                            replacementDataClass.HighData.Add((double)dataAtTimepoints[k]);
                            replacementDataClass.IconType.Add((double)dataAtIconType[k]);
                            replacementDataClass.TimeData.Add(timepoints[k].ToString(), dataAtTimepoints[k].ToString());
                        }
                        finalSubElementHT[ElementItems] = replacementDataClass;
                    }
                    finalElementHT[elementLevel] = finalSubElementHT;
                }
                else
                {
                    // info element
                    finalElementHT[elementLevel] = (Hashtable)this.chartDataObsElementHashTable[elementLevel]; // use existing table
                }
            }
            this.chartDataObsElementHashTable = finalElementHT;
        }

        public void AddUpdatedChartData(double timePeriod, string key, string keyType, string name, string id, double seconds)
        {
            Hashtable tempElementLevelHash;
            String combinedKey = key + "-" + timePeriod;
            ChartDetailDataClass tempElementDataClass = null;

            if (this.chartDetailDataObsElementHashTable.ContainsKey(keyType))
            {
                tempElementLevelHash = (Hashtable)this.chartDetailDataObsElementHashTable[keyType];
                if (tempElementLevelHash.ContainsKey(combinedKey))
                {
                    tempElementDataClass = (ChartDetailDataClass)tempElementLevelHash[combinedKey];
                    tempElementDataClass.Nameupdated.Add(name);
                    tempElementDataClass.Valueupdated.Add(seconds);
                }
                else
                {
                    tempElementDataClass = new ChartDetailDataClass();
                    tempElementDataClass.Nameupdated.Add(name);
                    tempElementDataClass.Valueupdated.Add(seconds);
                    tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                }
            }
            else
            {
                tempElementDataClass = new ChartDetailDataClass();
                tempElementDataClass.Nameupdated.Add(name);
                tempElementDataClass.Valueupdated.Add(seconds);
                tempElementLevelHash = new Hashtable();
                tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                this.chartDetailDataObsElementHashTable.Add(keyType, tempElementLevelHash);
            }
        }

        public void AddDemandedChartData(double timePeriod, string key, string keyType, string name, string id, double completeness)
        {
            Hashtable tempElementLevelHash;
            String combinedKey = key + "-" + timePeriod;
            ChartDetailDataClass tempElementDataClass = null;

            if (this.chartDetailDataObsElementHashTable.ContainsKey(keyType))
            {
                tempElementLevelHash = (Hashtable)this.chartDetailDataObsElementHashTable[keyType];
                if (tempElementLevelHash.ContainsKey(combinedKey))
                {
                    tempElementDataClass = (ChartDetailDataClass)tempElementLevelHash[combinedKey];
                    tempElementDataClass.Name.Add(name);
                    tempElementDataClass.Value.Add(completeness);
                }
                else
                {
                    tempElementDataClass = new ChartDetailDataClass();
                    tempElementDataClass.Name.Add(name);
                    tempElementDataClass.Value.Add(completeness);
                    tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                }
            }
            else
            {
                tempElementDataClass = new ChartDetailDataClass();
                tempElementDataClass.Name.Add(name);
                tempElementDataClass.Value.Add(completeness);
                tempElementLevelHash = new Hashtable();
                tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                this.chartDetailDataObsElementHashTable.Add(keyType, tempElementLevelHash);
            }
        }
        public void AddWorkingOnChartData(double timePeriod, string key, string keyType, string name, string id)
        {
            Hashtable tempElementLevelHash;
            String combinedKey = key + "-" + timePeriod;
            ChartDetailDataClass tempElementDataClass = null;

            if (this.chartDetailDataObsElementHashTable.ContainsKey(keyType))
            {
                tempElementLevelHash = (Hashtable)this.chartDetailDataObsElementHashTable[keyType];
                if (tempElementLevelHash.ContainsKey(combinedKey))
                {
                    tempElementDataClass = (ChartDetailDataClass)tempElementLevelHash[combinedKey];
                    tempElementDataClass.Name.Add(name);
                    tempElementDataClass.Value.Add(0);
                }
                else
                {
                    tempElementDataClass = new ChartDetailDataClass();
                    tempElementDataClass.Name.Add(name);
                    tempElementDataClass.Value.Add(0);
                    tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                }
            }
            else
            {
                tempElementDataClass = new ChartDetailDataClass();
                tempElementDataClass.Name.Add(name);
                tempElementDataClass.Value.Add(0);
                tempElementLevelHash = new Hashtable();
                tempElementLevelHash.Add(combinedKey, tempElementDataClass);
                this.chartDetailDataObsElementHashTable.Add(keyType, tempElementLevelHash);
            }
        }
        public void BuildChartData(double timePeriod, string obsElement, string obsElementType, double obsValue, string obsValueType, double isThereDetailData)
        {
            ChartDataClass tempElementDataClass;
            Hashtable tempElementLevelHash;
            // does the element type exist in the hash table?
            if (obsValueType.Equals("Summary"))
            {
                BuildSummaryChartData(timePeriod, obsElement, obsElementType, obsValue, obsValueType);
            }
            else
            {
                if (this.chartDataObsElementHashTable.ContainsKey(obsElementType))
                {
                    tempElementLevelHash = (Hashtable)this.chartDataObsElementHashTable[obsElementType];
                    //This is the hashtable that contains all the instances of the ElementType
                    //Does the element exist in the hashtable

                    if (tempElementLevelHash.ContainsKey(obsElement))
                    {
                        tempElementDataClass = (ChartDataClass)tempElementLevelHash[obsElement];
                        tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                        tempElementDataClass.TimeStamps.Add(timePeriod);
                        tempElementDataClass.IconType.Add(isThereDetailData);
                        tempElementDataClass.HighData.Add(obsValue);
                    }
                    //if not, create a new element hash to contain it
                    else
                    {
                        tempElementDataClass = new ChartDataClass();
                        tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                        tempElementDataClass.TimeStamps.Add(timePeriod);
                        tempElementDataClass.HighData.Add(obsValue);
                        tempElementDataClass.IconType.Add(isThereDetailData);
                        tempElementLevelHash.Add(obsElement, tempElementDataClass);
                    }
                }
                else
                {
                    tempElementDataClass = new ChartDataClass();
                    tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                    tempElementDataClass.TimeStamps.Add(timePeriod);
                    tempElementDataClass.HighData.Add(obsValue);
                    tempElementDataClass.IconType.Add(isThereDetailData);
                    tempElementLevelHash = new Hashtable();
                    tempElementLevelHash.Add(obsElement, tempElementDataClass);
                    this.chartDataObsElementHashTable.Add(obsElementType, tempElementLevelHash);
                }
            }
        }

        public void BuildSummaryChartData(double timePeriod, string obsElement, string obsElementType, double obsValue, string obsValueType)
        {
            ChartDataClass tempElementDataClass;
            Hashtable tempElementLevelHash;
            // does the element type exist in the hash table?
            if (this.chartDataObsElementSummaryHashTable.ContainsKey(obsElementType))
            {
                tempElementLevelHash = (Hashtable)this.chartDataObsElementSummaryHashTable[obsElementType];
                //This is the hashtable that contains all the instances of the ElementType
                //Does the element exist in the hashtable

                if (tempElementLevelHash.ContainsKey(obsElement))
                {
                    tempElementDataClass = (ChartDataClass)tempElementLevelHash[obsElement];
                    tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                    tempElementDataClass.TimeStamps.Add(timePeriod);
                    tempElementDataClass.HighData.Add(obsValue);
                }
                //if not, create a new element hash to contain it
                else
                {
                    tempElementDataClass = new ChartDataClass();
                    tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                    tempElementDataClass.TimeStamps.Add(timePeriod);
                    tempElementDataClass.HighData.Add(obsValue);
                    tempElementLevelHash.Add(obsElement, tempElementDataClass);
                }
            }
            else
            {
                tempElementDataClass = new ChartDataClass();
                tempElementDataClass.TimeData.Add(timePeriod.ToString(), obsValue.ToString());
                tempElementDataClass.TimeStamps.Add(timePeriod);
                tempElementDataClass.HighData.Add(obsValue);
                tempElementLevelHash = new Hashtable();
                tempElementLevelHash.Add(obsElement, tempElementDataClass);
                this.chartDataObsElementSummaryHashTable.Add(obsElementType, tempElementLevelHash);
            }
        }
	}
}
