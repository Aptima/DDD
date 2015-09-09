using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;

using DashboardDataAccess;
using ChartDirector;

namespace VisualizationDashboard
{
    public class PhaseTriggerData
    {
        private string phaseName = null;

        public string PhaseName
        {
            get { return phaseName; }
            set { phaseName = value; }
        }

        private string triggerName = null;

        public string TriggerName
        {
            get { return triggerName; }
            set { triggerName = value; }
        }
    }

    public abstract class DashboardVisualization
    {
        public uint[] barcodeColors = { 0xff0000, 0x00FF00, 0x0000FF, 0x800080, 0x008000, 0x808000, 0xFFFF00, 0x000080, 0x008080, 0x00FFFF,
                                      0xFFA500, 0xC0C0C0, 0x808080};

        public uint[] barcodeColorsTrans = { 0x8000cc00, 0x800000cc, 0x80cc0000, 0x80cc0000, 0x80cc0000, 0x80cc0000, 0x80cc0000, 0x80cc0000 };

        protected ConfigDataModel configDataModel = null;

        protected ConfigDisplay configDisplay = null;

        protected StackPanel configDisplayPanel = null;

        public List<string>[] chartLabels = null;

        protected List<int>[] chartMeasureIDs = null;

        public List<object> rtPMEData = null;

        static List<string> definedMeasureInstances = new List<string>();

        private Dictionary<string, XmlWriter> measureInstXmlMap;

        private List<PhaseTriggerData> phaseTriggerDataList = new List<PhaseTriggerData>();

        public abstract void GetDataLists();

        public abstract void InitVisualization(StackPanel configDisplayPanel);

        public abstract void UpdateVisualization();

        public DashboardVisualization(ConfigDisplay configDisplay, ConfigDataModel configDataModel, Dictionary<string, XmlWriter> measureInstXmlMap)
        {
            if ((configDataModel == null) || (configDisplay == null) || (measureInstXmlMap == null))
            {
                return;
            }

            this.configDataModel = configDataModel;
            this.configDisplay = configDisplay;
            this.measureInstXmlMap = measureInstXmlMap;

            // Initialize Phase to Trigger Map (temporary until read from database)
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "AttackObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "ClientMeasure_CapabilitySelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "ClientMeasure_ObjectSelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "ClientMeasure_ObjectTabSelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "ClientMeasure_ScreenViewTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "MoveObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestChatRoomCreateTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestCloseChatRoomTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestJoinVoiceChannelTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestLeaveVoiceChannelTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestMuteUserTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestStartedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Evaluating", TriggerName = "RequestStoppedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Evaluating", TriggerName = "RequestUnmuteUserTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "RequestWhiteboardRoomCreateTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "StartedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Evaluating", TriggerName = "StoppedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Evaluating", TriggerName = "SubplatformDockRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "SubplatformLaunchRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "TextChatRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "TransferObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "UpdateTagTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "WeaponLaunchRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "WhiteboardClearAllRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "WhiteboardClearRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Planning", TriggerName = "WhiteboardLineRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "WhiteboardSyncScreenViewRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "SA Building", TriggerName = "WhiteboardUndoRequestTrigger"});

            /* Old List 
            phaseTriggerDataList.Add(new PhaseTriggerData { PhaseName = "Executing", TriggerName = "AttackObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "ClientMeasure_CapabilitySelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "ClientMeasure_ObjectSelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "ClientMeasure_ObjectTabSelectedTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "ClientMeasure_ScreenViewTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Executing", TriggerName = "MoveObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestChatRoomCreateTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestCloseChatRoomTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestJoinVoiceChannelTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestLeaveVoiceChannelTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestMuteUserTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestStartedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestStoppedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestUnmuteUserTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "RequestWhiteboardRoomCreateTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "StartedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "StoppedTalkingTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Executing", TriggerName = "SubplatformDockRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Executing", TriggerName = "SubplatformLaunchRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "TextChatRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Executing", TriggerName = "TransferObjectRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "UpdateTagTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Executing", TriggerName = "WeaponLaunchRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "WhiteboardClearAllRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "WhiteboardClearRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "Planning", TriggerName = "WhiteboardLineRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "WhiteboardSyncScreenViewRequestTrigger"});
            phaseTriggerDataList.Add(new PhaseTriggerData{PhaseName = "SA Building", TriggerName = "WhiteboardUndoRequestTrigger"});
            */

            GetDataLists();
        }

        public List<string>[] GetConfigDisplayLabels()
        {
            return chartLabels;
        }

        protected void GetFactorLabels(string measureName, string factorName, List<string> chartLabels, List<int> measureIDs)
        {
            GetFactorLabels(measureName, factorName, chartLabels, measureIDs, false, false);
        }

        protected void GetFactorLabels(string measureName, string factorName, List<string> chartLabels, List<int> measureIDs, bool skipTotals)
        {
            GetFactorLabels(measureName, factorName, chartLabels, measureIDs, skipTotals, false);
        }

        protected void GetFactorLabels(string measureName, string factorName, List<string> chartLabels, List<int> measureIDs,
            bool skipTotals, bool reverseOrder)
        {
            string totalMeasureName = null;
            int totalMeasureID = 0;

            // Find all levels for this factor in the config measure table
            foreach (ExperimentMeasure expMeasure in configDataModel.ConfigMeasuresTable)
            {
                if ((expMeasure.Measure.Category.CompareTo(measureName) == 0) &&
                    (expMeasure.Measure.SubCategory.CompareTo(factorName) == 0))
                {
                    // Make total last in returned list
                    if ((expMeasure.Measure.Name.CompareTo("All") == 0) ||
                        (expMeasure.Measure.Name.CompareTo("Total") == 0) ||
                        (expMeasure.Measure.Name.CompareTo("Team") == 0))
                    {
                        totalMeasureName = expMeasure.Measure.Name;
                        totalMeasureID = expMeasure.MeasureID;
                    }
                    else
                    {
                        if (expMeasure.ExperimentEntityID > 0)
                        {
                            chartLabels.Add(expMeasure.ExperimentEntity.Name);
                            measureIDs.Add(expMeasure.MeasureID);
                        }
                        else
                        {
                            chartLabels.Add(expMeasure.Measure.Name);
                            measureIDs.Add(expMeasure.MeasureID);
                        }
                    }
                }
            }

            if ((totalMeasureName != null) && (!skipTotals))
            {
                chartLabels.Add(totalMeasureName);
                measureIDs.Add(totalMeasureID);
            }

            if (reverseOrder)
            {
                chartLabels.Reverse();
                measureIDs.Reverse();
            }
        }

        protected bool GetConfigDisplayData(int dataPos, ref double[] dataArray)
        {
            if (rtPMEData == null)
            {
                return false;
            }

            if (rtPMEData.Count < dataPos)
            {
                return false;
            }

            if (rtPMEData[dataPos] is InstCurValueList)
            {
                InstCurValueList instCurValueList = (InstCurValueList)rtPMEData[dataPos];

                dataArray = instCurValueList.dataValues.ToArray();

                return true;
            }
            else
            {
                return false;
            }
        }

        protected string CreateRTPMEInstanceDef(ConfigDisplay configDisplay, List<string> factorLevels, string altMetricName)
        {
            string instanceID;
            string measureDefID;
            int totalFactors;
            int numProcessedFactors = 0;
            Dictionary<string, FactorInfo> nonMeasureFactorMap = new Dictionary<string, FactorInfo>();
            Dictionary<string, FactorInfo> measureFactorMap = new Dictionary<string, FactorInfo>();
            int factorPos = 0;
            string metricName = null;
            int i = 0;

            // Handle passed in alternative metric name
            if (altMetricName != null)
            {
                metricName = altMetricName;
            }
            else
            {
                metricName = configDisplay.MetricName;
            }

            // Special measure cases
            if (metricName.CompareTo("Weapons used") == 0)
            {
                return CreateRTPMEDiffInstanceDef(configDisplay, factorLevels, altMetricName);
            }
            else if (metricName.CompareTo("Fuel used") == 0)
            {
                return CreateRTPMEDiffInstanceDef(configDisplay, factorLevels, altMetricName);
            }

            // Determine total number of factors
            totalFactors = configDisplay.NumFactors + configDisplay.NumBlockedFactors;

            // Removed Parameters and triggers from search list
            factorPos = 0;
            foreach (DisplayFactor displayFactor in configDisplay.DisplayFactors)
            {
                RTPMEType rtPMEType = configDataModel.GetMeasureRTPMEType(configDisplay.MeasureName, displayFactor.FactorName,
                    factorLevels[factorPos]);

                if (displayFactor.FactorName.CompareTo("Phase") == 0)
                {
                    rtPMEType = RTPMEType.Trigger;
                }

                if ((rtPMEType == RTPMEType.Paramter) || (rtPMEType == RTPMEType.Trigger))
                {
                    // Remove from search list
                    FactorInfo nonMeasureFactor = new FactorInfo();

                    nonMeasureFactor.RtPMEType = rtPMEType;
                    nonMeasureFactor.FactorName = displayFactor.FactorName;
                    nonMeasureFactor.LevelName = factorLevels[factorPos];
                    nonMeasureFactor.MeasureID = configDataModel.LocateMeasureID(configDisplay.MeasureName, nonMeasureFactor.FactorName,
                        nonMeasureFactor.LevelName);
                    nonMeasureFactorMap.Add(displayFactor.FactorName, nonMeasureFactor);
                }

                factorPos++;
            }

            foreach (DisplayBlockedFactor displayBlockedFactor in configDisplay.DisplayBlockedFactors)
            {
                RTPMEType rtPMEType = configDataModel.GetMeasureRTPMEType(configDisplay.MeasureName, displayBlockedFactor.Measure.SubCategory,
                    displayBlockedFactor.LevelName);

                if ((rtPMEType == RTPMEType.Paramter) || (rtPMEType == RTPMEType.Trigger))
                {
                    // Remove from search list
                    FactorInfo nonMeasureFactor = new FactorInfo();

                    nonMeasureFactor.RtPMEType = rtPMEType;
                    nonMeasureFactor.FactorName = displayBlockedFactor.Measure.SubCategory;
                    nonMeasureFactor.LevelName = displayBlockedFactor.LevelName;
                    nonMeasureFactor.MeasureID = configDataModel.LocateMeasureID(configDisplay.MeasureName, nonMeasureFactor.FactorName,
                        nonMeasureFactor.LevelName);

                    nonMeasureFactorMap.Add(displayBlockedFactor.Measure.SubCategory, nonMeasureFactor);
                }
            }

            // Sort the nonMeasureFactorMap
            var sortedDict = (from entry in nonMeasureFactorMap orderby entry.Value.MeasureID ascending select entry);
            nonMeasureFactorMap = sortedDict.ToDictionary(x => x.Key, x => x.Value);

            // Sort the factors and blocked factors according to their order in the measures table
            while (numProcessedFactors < totalFactors - nonMeasureFactorMap.Count)
            {
                int curMeasureID = int.MaxValue;
                string factorName = "";
                string levelName = "";

                // Loop throug all of the factors and blocked factors looking for the lowest
                // one in the measure table
                factorPos = 0;
                foreach (DisplayFactor displayFactor in configDisplay.DisplayFactors)
                {
                    if (nonMeasureFactorMap.ContainsKey(displayFactor.FactorName) ||
                        measureFactorMap.ContainsKey(displayFactor.FactorName))
                    {
                        factorPos++;
                        continue;
                    }

                    int measureID = configDataModel.LocateMeasureID(configDisplay.MeasureName, displayFactor.FactorName,
                        factorLevels[factorPos]);

                    if ((measureID > 0) && (measureID < curMeasureID))
                    {
                        curMeasureID = measureID;
                        factorName = displayFactor.FactorName;
                        levelName = factorLevels[factorPos];
                    }

                    factorPos++;
                }

                foreach (DisplayBlockedFactor displayBlockedFactor in configDisplay.DisplayBlockedFactors)
                {
                    if (nonMeasureFactorMap.ContainsKey(displayBlockedFactor.Measure.SubCategory) ||
                        measureFactorMap.ContainsKey(displayBlockedFactor.Measure.SubCategory))
                    {
                        continue;
                    }

                    int measureID = configDataModel.LocateMeasureID(configDisplay.MeasureName, displayBlockedFactor.Measure.SubCategory,
                        displayBlockedFactor.LevelName);

                    if ((measureID > 0) && (measureID < curMeasureID))
                    {
                        curMeasureID = measureID;
                        factorName = displayBlockedFactor.Measure.SubCategory;
                        levelName = displayBlockedFactor.LevelName;
                    }
                }

                if (curMeasureID != int.MaxValue)
                {
                    FactorInfo measureFactor = new FactorInfo();

                    measureFactor.RtPMEType = RTPMEType.Measure;
                    measureFactor.FactorName = factorName;
                    measureFactor.LevelName = levelName;

                    measureFactorMap.Add(factorName, measureFactor);
                    numProcessedFactors++;
                }
            }

            // Create the name of the measure definition
            measureDefID = configDisplay.MeasureName + "_" + metricName;

            foreach (FactorInfo factorInfo in measureFactorMap.Values)
            {
                string modeLevel = null;

                if (measureFactorMap.Keys.Contains("Mode"))
                {
                    modeLevel = measureFactorMap["Mode"].LevelName;
                }
                if ((configDisplay.MeasureName.CompareTo("Communication") == 0) &&
                    (factorInfo.FactorName.CompareTo("Type") == 0) &&
                    (modeLevel.CompareTo("Chat") != 0))
                {
                    continue;
                }
                measureDefID += "_" + factorInfo.LevelName;
            }

            // Create the name for this measurement instance
            instanceID = configDisplay.MeasureName + "_" + metricName;

            measureFactorMap.Values.ToList().ForEach(x => instanceID += "_" + x.LevelName);
            nonMeasureFactorMap.Values.ToList().ForEach(x => instanceID += "_" + x.LevelName);

            // Determine if this measurement instance is already defined
            if (IsMeasurementInstanceDefined(instanceID))
            {
                return instanceID;
            }

            // Check for all or team in this measure, if found generate a sum
            string factorToSum = null;
            factorToSum = LocateFactorToSum(measureFactorMap, nonMeasureFactorMap);
            if (factorToSum != null)
            {
                return CreateRTPMESumInstanceDef(factorToSum, configDisplay, factorLevels, altMetricName, instanceID);
            }

            // Check for asset/operator combination that can be skipped
            string operatorName = null;
            string assetName = null;

            i = 0;
            foreach (DisplayFactor factor in configDisplay.DisplayFactors)
            {
                if ((factor.FactorName.CompareTo("Operator") == 0) || (factor.FactorName.CompareTo("To Operator") == 0) ||
                    (factor.FactorName.CompareTo("From Operator") == 0))
                {
                    operatorName = factorLevels[i];
                    if ((operatorName.CompareTo("Team") == 0) || (operatorName.CompareTo("All") == 0))
                    {
                        operatorName = null;
                    }
                    else
                    {
                        break;
                    }
                }
                i++;
            }

            if (operatorName == null)
            {
                foreach (DisplayBlockedFactor factor in configDisplay.DisplayBlockedFactors)
                {
                    if ((factor.Measure.SubCategory.CompareTo("Operator") == 0) || (factor.Measure.SubCategory.CompareTo("To Operator") == 0) ||
                        (factor.Measure.SubCategory.CompareTo("From Operator") == 0))
                    {
                        operatorName = factor.LevelName;
                        if ((operatorName.CompareTo("Team") == 0) || (operatorName.CompareTo("All") == 0))
                        {
                            operatorName = null;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            i = 0;
            foreach (DisplayFactor factor in configDisplay.DisplayFactors)
            {
                if (factor.FactorName.CompareTo("Asset") == 0)
                {
                    assetName = factorLevels[i];
                    break;
                }
                i++;
            }
            if ((operatorName != null) && (assetName != null))
            {
                if ((operatorName.CompareTo("Team") != 0) && ((operatorName.CompareTo("All") != 0)) && (assetName.CompareTo("All") != 0))
                {
                    ExperimentEntity entity = configDataModel.GetEntityInfo(configDisplay.MeasureName, "Asset", assetName);
                    if (entity != null)
                    {
                        if ((entity.ExperimentEntity1 != null) && (entity.ExperimentEntity1.Name.CompareTo(operatorName) != 0))
                        {
                            // No Value when operator does not own this asset
                            return null;
                        }
                    }
                }
            }

            i = 0;
            string phaseName = null;
            foreach (DisplayFactor factor in configDisplay.DisplayFactors)
            {
                if (factor.FactorName.CompareTo("Phase") == 0)
                {
                    phaseName = factorLevels[i];
                    if (phaseName.CompareTo("Total") == 0)
                    {
                        phaseName = null;
                    }
                    break;
                }
                i++;
            }

            // Check for Workload_Count and generate sum of Communication and Execution measures if neccessary
            if (measureDefID.CompareTo("Workload_Count") == 0)
            {
                return CreateWorkloadSumInstanceDef(operatorName, phaseName, configDisplay, factorLevels, altMetricName, instanceID);
            }

            // Create trigger instances
            if ((nonMeasureFactorMap.Keys.Contains("Phase")) && (operatorName != null) && (operatorName.Length > 0))
            {
                // Make sure all of the required phase triggers exist for this user
                CreateRTPMETriggerInstances(operatorName, instanceID);
            }                

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(instanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return null;
            }

            measureInstXml.WriteStartElement("MeasurementInstance");
            measureInstXml.WriteAttributeString("InstanceOf", measureDefID);
            measureInstXml.WriteAttributeString("ID", instanceID);
            measureInstXml.WriteAttributeString("Visibility", "Visible");

            // Determine directionality
            string directionality = null;

            // Determine Directionality
            foreach (FactorInfo factor in measureFactorMap.Values)
            {
                if (factor.FactorName.CompareTo("Directionality") == 0)
                {
                    directionality = factor.LevelName;
                    break;
                }
            }

            // Add operator parameter
            if ((operatorName != null) && (operatorName.Length > 0))
            {
                measureInstXml.WriteStartElement("ParameterValue");
                measureInstXml.WriteAttributeString("Name", "Operator");
                measureInstXml.WriteAttributeString("Value", operatorName);
                measureInstXml.WriteEndElement();
            }

            // Add Parameters
            foreach (FactorInfo factorInfo in nonMeasureFactorMap.Values)
            {
                if (factorInfo.RtPMEType == RTPMEType.Paramter)
                {
                    if ((factorInfo.FactorName.CompareTo("To Operator") == 0) ||
                        (factorInfo.FactorName.CompareTo("From Operator") == 0) ||
                        (factorInfo.FactorName.CompareTo("Operator") == 0))
                    {
                        continue;
                    }
                    else
                    {
                        measureInstXml.WriteStartElement("ParameterValue");
                        measureInstXml.WriteAttributeString("Name", factorInfo.FactorName);
                        measureInstXml.WriteAttributeString("Value", factorInfo.LevelName);
                        measureInstXml.WriteEndElement();
                    }
                }
            }

            // Output measurement trigger section
            if ((phaseName != null) && (phaseName.Length > 0))
            {
                measureInstXml.WriteStartElement("MeasurementTrigger");

                foreach (PhaseTriggerData phaseTriggerData in phaseTriggerDataList)
                {
                    string triggerID = phaseTriggerData.TriggerName + "_" + operatorName;

                    if (phaseTriggerData.PhaseName.CompareTo(phaseName) == 0)
                    {
                        measureInstXml.WriteStartElement("TriggerStart");
                        measureInstXml.WriteAttributeString("Type", "Measure");
                        measureInstXml.WriteAttributeString("Ref", triggerID);
                        measureInstXml.WriteEndElement();
                    }
                    else
                    {
                        measureInstXml.WriteStartElement("TriggerEnd");
                        measureInstXml.WriteAttributeString("Type", "Measure");
                        measureInstXml.WriteAttributeString("Ref", triggerID);
                        measureInstXml.WriteEndElement();
                    }
                }

                measureInstXml.WriteEndElement();
            }

            // Close XML Element
            measureInstXml.WriteEndElement();

            AddMeasurementInstance(instanceID);

            return instanceID;
        }

        private string LocateFactorToSum(Dictionary<string, FactorInfo> measureFactorMap, Dictionary<string, FactorInfo>  nonMeasureFactorMap)
        {
            string directionality = null;

            // Determine Directionality
            foreach (FactorInfo factor in measureFactorMap.Values)
            {
                if (factor.FactorName.CompareTo("Directionality") == 0)
                {
                    directionality = factor.LevelName;
                    break;
                }
            }

            // Loop through measure factor map looking for an All or Total
            foreach (FactorInfo factor in measureFactorMap.Values)
            {
                if (((factor.LevelName.CompareTo("All") == 0) && (factor.FactorName.CompareTo("Type") != 0))
                    || (factor.LevelName.CompareTo("Team") == 0))
                {
                    return factor.FactorName;
                }
            }

            // Loop through non-measure factor map looking for an All or Total
            foreach (FactorInfo factor in nonMeasureFactorMap.Values)
            {
                // Ignore factor based on directionallity
                if ((directionality != null) && (factor.FactorName.CompareTo("From Operator") == 0) &&
                    (directionality.CompareTo("Incoming") == 0))
                {
                    continue;
                }
                else if ((directionality != null) && (factor.FactorName.CompareTo("To Operator") == 0) &&
                    (directionality.CompareTo("Outgoing") == 0))
                {
                    continue;
                }

                if (((factor.LevelName.CompareTo("All") == 0) && (factor.FactorName.CompareTo("Type") != 0))
                    || (factor.LevelName.CompareTo("Team") == 0))
                {
                    return factor.FactorName;
                }
            }

            return null;
        }

        static int maxID = 1;

        protected string CreateRTPMEMaxInstanceDef(ConfigDisplay configDisplay, List<string> instanceList)
        {
            string instanceID;
            string measureDefID = "Max" + instanceList.Count.ToString();

            instanceID = configDisplay.Name + "_Max_" + configDisplay.MeasureName + "_" + configDisplay.MetricName + "_" + maxID.ToString();
            maxID++;

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(instanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return null;
            }

            // Create the measurement instance definition
            measureInstXml.WriteStartElement("MeasurementInstance");
            measureInstXml.WriteAttributeString("InstanceOf", measureDefID);
            measureInstXml.WriteAttributeString("ID", instanceID);
            measureInstXml.WriteAttributeString("Visibility", "Visible");

            // Add Parameters
            for (int i = 0; i < instanceList.Count; i++)
            {
                measureInstXml.WriteStartElement("ParameterValue");
                measureInstXml.WriteAttributeString("Name", "Value" + (i + 1).ToString());
                measureInstXml.WriteAttributeString("Type", "MeasureRef");
                measureInstXml.WriteAttributeString("Value", instanceList[i]);
                measureInstXml.WriteEndElement();
            }

            // Close XML Element
            measureInstXml.WriteEndElement();

            AddMeasurementInstance(instanceID);

            return instanceID;
        }

        private static int ratioID = 1;

        protected string CreateRTPMERatioInstanceDef(ConfigDisplay configDisplay, string dividendID, string divisorID)
        {
            string instanceID;
            string measureDefID = "Ratio";

            instanceID = configDisplay.Name + "_Ratio_" + configDisplay.MeasureName + "_" + ratioID.ToString();
            ratioID++;

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(instanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return null;
            }

            // Create the measurement instance definition
            measureInstXml.WriteStartElement("MeasurementInstance");
            measureInstXml.WriteAttributeString("InstanceOf", measureDefID);
            measureInstXml.WriteAttributeString("ID", instanceID);
            measureInstXml.WriteAttributeString("Visibility", "Visible");

            // Add Dividend Parameter
            measureInstXml.WriteStartElement("ParameterValue");
            measureInstXml.WriteAttributeString("Name", "Dividend");
            measureInstXml.WriteAttributeString("Type", "MeasureRef");
            measureInstXml.WriteAttributeString("Value", dividendID);
            measureInstXml.WriteEndElement();

            // Add Divisor Parameter
            measureInstXml.WriteStartElement("ParameterValue");
            measureInstXml.WriteAttributeString("Name", "Divisor");
            measureInstXml.WriteAttributeString("Type", "MeasureRef");
            measureInstXml.WriteAttributeString("Value", divisorID);
            measureInstXml.WriteEndElement();

            // Close XML Element
            measureInstXml.WriteEndElement();

            AddMeasurementInstance(instanceID);

            return instanceID;
        }

        protected string CreateRTPMESumInstanceDef(string factorToSum, ConfigDisplay configDisplay, List<string> factorLevels, string altMetricName,
            string sumInstID)
        {
            List<string> sumInstIDs = new List<string>();

            // Get a list of factor levels to sum
            List<string> levelsToSum = configDataModel.GetFactorLevels(configDisplay.MeasureName, factorToSum);
            if ((levelsToSum == null) || (levelsToSum.Count == 0))
            {
                return null;
            }

            foreach (string levelName in levelsToSum)
            {
                if (levelName.CompareTo("Weapons Used") == 0)
                {
                    continue;   // Not currently implemented
                }

                // Generate a copy of factorLevels
                List<string> newFactorLevels = new List<string>();
                factorLevels.ForEach(x => newFactorLevels.Add(x));

                // Make a copy of config display so that changes are only temporary
                ConfigDisplay newConfigDisplay = DeepCloneConfigDisplay(configDisplay);

                // Modify configDisplay
                bool factorFound = false;
                int i = 0;
                foreach (DisplayFactor factor in newConfigDisplay.DisplayFactors)
                {
                    if (factor.FactorName.CompareTo(factorToSum) == 0)
                    {
                        newFactorLevels[i] = levelName;
                        factorFound = true;
                        break;
                    }
                    i++;
                }

                if (!factorFound)
                {
                    foreach (DisplayBlockedFactor blockedFactor in newConfigDisplay.DisplayBlockedFactors)
                    {
                        if (blockedFactor.Measure.SubCategory.CompareTo(factorToSum) == 0)
                        {
                            blockedFactor.LevelName = levelName;
                            factorFound = true;
                            break;
                        }
                    }
                }

                if (!factorFound)
                {
                    return null;
                }

                // Generate measurement instances
                string newInstID = CreateRTPMEInstanceDef(newConfigDisplay, newFactorLevels, altMetricName);
                if (newInstID != null)
                {
                    sumInstIDs.Add(newInstID);
                }

            }

            if (sumInstIDs.Count == 0)
            {
                return sumInstID;       // This sum will always have a value of 0
            }

            // Sum measurement instances
            string newSumInstID = CreateRTPMESumInstanceDef(configDisplay, factorLevels, sumInstIDs, sumInstID);

            return newSumInstID;
        }

        protected string CreateWorkloadSumInstanceDef(string operatorName, string phaseName, ConfigDisplay configDisplay, List<string> factorLevels, string altMetricName,
            string sumInstID)
        {
            List<string> sumInstIDs = new List<string>();
            List<string> newFactorLevels = null;
            ConfigDisplay newConfigDisplay = null;
            string newInstID = null;

            // Commuication Count component
            newConfigDisplay = CreateTotalCommConfigDisplay(operatorName, phaseName, out newFactorLevels, configDisplay);
            newInstID = CreateRTPMEInstanceDef(newConfigDisplay, newFactorLevels, "Count");
            if (newInstID != null)
            {
                sumInstIDs.Add(newInstID);
            }

            // Execution Count Component
            newConfigDisplay = CreateTotalExecutionConfigDisplay(operatorName, phaseName, out newFactorLevels, configDisplay);
            newInstID = CreateRTPMEInstanceDef(newConfigDisplay, newFactorLevels, "Count");
            if (newInstID != null)
            {
                sumInstIDs.Add(newInstID);
            }

            if (sumInstIDs.Count != 2)
            {
                return null;
            }

            // Sum measurement instances
            string newSumInstID = CreateRTPMESumInstanceDef(configDisplay, factorLevels, sumInstIDs, sumInstID);

            return newSumInstID;
        }

        private ConfigDisplay CreateTotalCommConfigDisplay(string operatorName, string phaseName, out List<string> newFactorLevels, ConfigDisplay configDisplay)
        {
            DisplayFactor newDisplayFactor = null;
            DisplayBlockedFactor newDisplayBlockedFactor = null;
            Measure newMeasure = null;

            newFactorLevels = new List<string>();

            ConfigDisplay newConfigDisplay = new ConfigDisplay();
            newConfigDisplay.Name = "WorkloadSumCommunication";
            newConfigDisplay.MeasureName = "Communication";
            newConfigDisplay.MetricName = "Count";
            newConfigDisplay.Display = configDisplay.Display;
            newConfigDisplay.Config = configDisplay.Config;
            newConfigDisplay.ConfigID = configDisplay.ConfigID;
            newConfigDisplay.NumBlockedFactors = 4;
            newConfigDisplay.NumFactors = 2;

            newConfigDisplay.DisplayFactors = new System.Data.Linq.EntitySet<DisplayFactor>();
            newConfigDisplay.DisplayBlockedFactors = new System.Data.Linq.EntitySet<DisplayBlockedFactor>();

            // Factors
            // From Operator
            newDisplayFactor = new DisplayFactor();
            newDisplayFactor.ConfigDisplay = newConfigDisplay;
            newDisplayFactor.FactorName = "From Operator";
            newDisplayFactor.FactorLabel = "X Axis";
            newDisplayFactor.FactorPos = 1;
            newFactorLevels.Add(operatorName);
            // Phase
            newDisplayFactor = new DisplayFactor();
            newDisplayFactor.ConfigDisplay = newConfigDisplay;
            newDisplayFactor.FactorName = "Phase";
            newDisplayFactor.FactorLabel = "Y Axis";
            newDisplayFactor.FactorPos = 2;
            newFactorLevels.Add(phaseName);

            // Blocked Factors
            // Directionality = Outgoing
            newDisplayBlockedFactor = new DisplayBlockedFactor();
            newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
            newDisplayBlockedFactor.LevelName = "Outgoing";
            newDisplayBlockedFactor.MeasureID = configDataModel.LocateMeasureID("Communication", "Directionality", "Outgoing", out newMeasure);
            newDisplayBlockedFactor.Measure = newMeasure;
            // Mode = All
            newDisplayBlockedFactor = new DisplayBlockedFactor();
            newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
            newDisplayBlockedFactor.LevelName = "All";
            newDisplayBlockedFactor.MeasureID = configDataModel.LocateMeasureID("Communication", "Mode", "All", out newMeasure);
            newDisplayBlockedFactor.Measure = newMeasure;
            // Type = All
            newDisplayBlockedFactor = new DisplayBlockedFactor();
            newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
            newDisplayBlockedFactor.LevelName = "All";
            newDisplayBlockedFactor.MeasureID = configDataModel.LocateMeasureID("Communication", "Type", "All", out newMeasure);
            newDisplayBlockedFactor.Measure = newMeasure;
            // To Operator = Team (ignored)
            newDisplayBlockedFactor = new DisplayBlockedFactor();
            newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
            newDisplayBlockedFactor.LevelName = "Team";
            newDisplayBlockedFactor.MeasureID = configDataModel.LocateMeasureID("Communication", "To Operator", "Team", out newMeasure);
            newDisplayBlockedFactor.Measure = newMeasure;

            return newConfigDisplay;
        }

        private ConfigDisplay CreateTotalExecutionConfigDisplay(string operatorName, string phaseName, out List<string> newFactorLevels, ConfigDisplay configDisplay)
        {
            DisplayFactor newDisplayFactor = null;
            DisplayBlockedFactor newDisplayBlockedFactor = null;
            Measure newMeasure = null;

            newFactorLevels = new List<string>();

            ConfigDisplay newConfigDisplay = new ConfigDisplay();
            newConfigDisplay.Name = "WorkloadSumExecution";
            newConfigDisplay.MeasureName = "Execution";
            newConfigDisplay.MetricName = "Count";
            newConfigDisplay.Display = configDisplay.Display;
            newConfigDisplay.Config = configDisplay.Config;
            newConfigDisplay.ConfigID = configDisplay.ConfigID;
            newConfigDisplay.NumBlockedFactors = 1;
            newConfigDisplay.NumFactors = 2;

            newConfigDisplay.DisplayFactors = new System.Data.Linq.EntitySet<DisplayFactor>();
            newConfigDisplay.DisplayBlockedFactors = new System.Data.Linq.EntitySet<DisplayBlockedFactor>();

            // Factors
            // From Operator
            newDisplayFactor = new DisplayFactor();
            newDisplayFactor.ConfigDisplay = newConfigDisplay;
            newDisplayFactor.FactorName = "Operator";
            newDisplayFactor.FactorLabel = "X Axis";
            newDisplayFactor.FactorPos = 1;
            newFactorLevels.Add(operatorName);
            // Phase
            newDisplayFactor = new DisplayFactor();
            newDisplayFactor.ConfigDisplay = newConfigDisplay;
            newDisplayFactor.FactorName = "Phase";
            newDisplayFactor.FactorLabel = "Y Axis";
            newDisplayFactor.FactorPos = 2;
            newFactorLevels.Add(phaseName);

            // Blocked Factors
            // Object = All
            newDisplayBlockedFactor = new DisplayBlockedFactor();
            newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
            newDisplayBlockedFactor.LevelName = "All";
            newDisplayBlockedFactor.MeasureID = configDataModel.LocateMeasureID("Execution", "Object", "All", out newMeasure);
            newDisplayBlockedFactor.Measure = newMeasure;

            return newConfigDisplay;
        }

        private static ConfigDisplay DeepCloneConfigDisplay(ConfigDisplay configDisplay)
        {
            if (configDisplay == null)
            {
                return null;
            }

            // Generate a copy of the configDisplay and factor level list to modify
            ConfigDisplay newConfigDisplay = new ConfigDisplay();
            newConfigDisplay.Name = configDisplay.Name;
            newConfigDisplay.MeasureName = configDisplay.MeasureName;
            newConfigDisplay.MetricName = configDisplay.MetricName;
            //newConfigDisplay.DisplayID = configDisplay.DisplayID;
            newConfigDisplay.Display = configDisplay.Display;
            newConfigDisplay.Config = configDisplay.Config;
            newConfigDisplay.ConfigID = configDisplay.ConfigID;
            newConfigDisplay.NumBlockedFactors = configDisplay.NumBlockedFactors;
            newConfigDisplay.NumFactors = configDisplay.NumFactors;

            newConfigDisplay.DisplayFactors = new System.Data.Linq.EntitySet<DisplayFactor>();
            foreach (DisplayFactor displayFactor in configDisplay.DisplayFactors)
            {
                DisplayFactor newDisplayFactor = new DisplayFactor();

                newDisplayFactor.DisplayFactorID = displayFactor.DisplayFactorID;
                newDisplayFactor.ConfigDisplay = newConfigDisplay;
                //newDisplayFactor.ConfigDisplayID = displayFactor.ConfigDisplayID;
                newDisplayFactor.FactorName = displayFactor.FactorName;
                newDisplayFactor.FactorLabel = displayFactor.FactorLabel;
                newDisplayFactor.FactorPos = displayFactor.FactorPos;
            }

            newConfigDisplay.DisplayBlockedFactors = new System.Data.Linq.EntitySet<DisplayBlockedFactor>();
            foreach (DisplayBlockedFactor displayBlockedFactor in configDisplay.DisplayBlockedFactors)
            {
                DisplayBlockedFactor newDisplayBlockedFactor = new DisplayBlockedFactor();

                newDisplayBlockedFactor.DisplayBlockedFactorID = displayBlockedFactor.DisplayBlockedFactorID;
                newDisplayBlockedFactor.ConfigDisplay = newConfigDisplay;
                //newDisplayBlockedFactor.ConfigDisplayID = displayBlockedFactor.ConfigDisplayID;
                newDisplayBlockedFactor.LevelName = displayBlockedFactor.LevelName;
                newDisplayBlockedFactor.Measure = displayBlockedFactor.Measure;
                //newDisplayBlockedFactor.MeasureID = displayBlockedFactor.MeasureID;
            }

            return newConfigDisplay;
        }

        private static int sumID = 1;

        protected string CreateRTPMESumInstanceDef(ConfigDisplay configDisplay, List<string> factorLevels, List<string> listToTotal, string sumInstID)
        {
            string instanceID;
            int j = 1;
            string measureDefID = "SumInt" + listToTotal.Count.ToString();

            // Create the name for this measurement instance
            if ((sumInstID == null) || (sumInstID.Length == 0))
            {
                instanceID = configDisplay.MeasureName + "_" + configDisplay.MetricName;

                for (int i = 0; i < factorLevels.Count; i++)
                {
                    instanceID += "_" + factorLevels[i];
                }
                instanceID += "_" + sumID.ToString();
                sumID++;
            }
            else
            {
                instanceID = sumInstID;
            }

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(instanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return null;
            }

            // Create the measurement instance definition
            measureInstXml.WriteStartElement("MeasurementInstance");
            measureInstXml.WriteAttributeString("InstanceOf", measureDefID);
            measureInstXml.WriteAttributeString("ID", instanceID);
            measureInstXml.WriteAttributeString("Visibility", "Visible");

            // Add Parameters
            foreach (String measureInst in listToTotal)
            {
                measureInstXml.WriteStartElement("ParameterValue");
                measureInstXml.WriteAttributeString("Name", "Value" + (j++).ToString());
                measureInstXml.WriteAttributeString("Type", "MeasureRef");
                measureInstXml.WriteAttributeString("Value", measureInst);
                measureInstXml.WriteEndElement();
            }

            // Close XML Element
            measureInstXml.WriteEndElement();

            AddMeasurementInstance(instanceID);

            return instanceID;
        }

        protected void SingleChartInit(StackPanel configDisplayPanel)
        {
            SingleChartInit(configDisplayPanel, false, false);
        }

        protected void SingleChartInit(StackPanel configDisplayPanel, bool skipLegend)
        {
            SingleChartInit(configDisplayPanel, skipLegend, false);
        }

        protected void SingleChartInit(StackPanel configDisplayPanel, bool skipLegend, bool useTransColors)
        {
            List<string>[] chartLabels = null;

            // Initialize chart

            // Get the Factor Labels
            chartLabels = GetConfigDisplayLabels();
            if (chartLabels == null)
            {
                return;
            }

            int numOpenFactors = chartLabels.Length;

            // Tan border
            SolidColorBrush titleBorderBrush = new SolidColorBrush();
            titleBorderBrush.Color = Color.FromRgb(248, 245, 232);

            // Create a gird with a single entry for the chart
            Grid grid = new Grid();
            grid.Width = configDisplay.Width;
            grid.Height = configDisplay.Height;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;

            ColumnDefinition colDef = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef);

            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowDef);
            rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowDef);
            rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowDef);

            // Add Title
            Border border = new Border();
            border.BorderBrush = titleBorderBrush;
            border.BorderThickness = new Thickness(4);
            TextBlock txt = new TextBlock();
            txt.Text = configDisplay.Name;
            txt.FontSize = 14;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            border.Child = txt;
            Grid.SetColumn(border, 0);
            Grid.SetRow(border, 0);
            grid.Children.Add(border);

            // Create the legend for chart
            int legendHeight = 0;

            if ((chartLabels[numOpenFactors - 1] != null) &&
                (chartLabels[numOpenFactors - 1].Count > 6))
            {
                legendHeight = 75;
            }
            else
            {
                legendHeight = 40;
            }

            if (!skipLegend)
            {
                XYChart legend = new XYChart(configDisplay.Width, legendHeight);
                Image img = new Image();
                LegendBox legendBox = legend.addLegend(0, 0, false);
                DockPanel dockPanel = new DockPanel();

                legendBox.setBackground(0xffffff, 0xffffff);
                legendBox.setCols(-5);

                uint[] currentColors = null;
                if (useTransColors)
                {
                    currentColors = barcodeColorsTrans;
                }
                else {
                    currentColors = barcodeColors;
                }

                if (chartLabels[numOpenFactors-1] == null)
                {
                    legendBox.addKey(configDisplay.MetricName, (int)currentColors[0]);
                }
                else
                {
                    for (int z = 0; z < chartLabels[numOpenFactors-1].Count; z++)
                    {
                        if (z < barcodeColors.Length)
                        {
                            legendBox.addKey(chartLabels[numOpenFactors - 1][z], (int) currentColors[z]);
                        }
                        else
                        {
                            legendBox.addKey(chartLabels[numOpenFactors - 1][z], (int) currentColors[0]);
                        }
                    }
                }

                System.Drawing.Image imgWinForms = legend.makeImage();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                imgWinForms.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                CroppedBitmap croppedBitmap = new CroppedBitmap(bi, new Int32Rect(0, 0, configDisplay.Width, legendHeight));
                img.Source = croppedBitmap;
                img.Height = legendHeight;
                img.Width = configDisplay.Width;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetColumn(dockPanel, 0);
                Grid.SetRow(dockPanel, 1);
                dockPanel.Children.Add(img);
                grid.Children.Add(dockPanel);
            }

            // Add Grid to config display panel
            configDisplayPanel.Children.Add(grid);
        }

        static int diffID = 1;

        protected string CreateRTPMEDiffInstanceDef(ConfigDisplay configDisplay, List<string> factorLevels, string altMetricName)
        {
            string metricName = null;
            string firstMetricName = null;
            string secondMetricName = null;
            string measureIns1 = null;
            string measureIns2 = null;
            string instanceID = null;
            string measureDefID = "Difference";

            // Handle passed in alternative metric name
            if (altMetricName != null)
            {
                metricName = altMetricName;
            }
            else
            {
                metricName = configDisplay.MetricName;
            }

            // Determine metric names
            if (metricName.CompareTo("Weapons used") == 0)
            {
                firstMetricName = "Weapons initial";
                secondMetricName = "Weapons left";
            }
            else if (metricName.CompareTo("Fuel used") == 0)
            {
                firstMetricName = "Fuel initial";
                secondMetricName = "Fuel left";
            }

            if ((firstMetricName == null) || (secondMetricName == null))
            {
                return null;
            }

            measureIns1 = CreateRTPMEInstanceDef(configDisplay, factorLevels, firstMetricName);
            measureIns2 = CreateRTPMEInstanceDef(configDisplay, factorLevels, secondMetricName);

            if ((measureIns1 == null) || (measureIns2 == null))
            {
                return null;
            }

            // Create the name for this measurement instance
            instanceID = configDisplay.MeasureName + "_" + metricName;

            for (int i = 0; i < factorLevels.Count; i++)
            {
                instanceID += "_" + factorLevels[i];
            }
            instanceID += "_" + diffID.ToString();
            diffID++;

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(instanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return null;
            }

            // Create the measurement instance definition
            measureInstXml.WriteStartElement("MeasurementInstance");
            measureInstXml.WriteAttributeString("InstanceOf", measureDefID);
            measureInstXml.WriteAttributeString("ID", instanceID);
            measureInstXml.WriteAttributeString("Visibility", "Visible");

            // Operand 1
            measureInstXml.WriteStartElement("ParameterValue");
            measureInstXml.WriteAttributeString("Name", "Operand1");
            measureInstXml.WriteAttributeString("Type", "MeasureRef");
            measureInstXml.WriteAttributeString("Value", measureIns1);
            measureInstXml.WriteEndElement();

            // Operand 2
            measureInstXml.WriteStartElement("ParameterValue");
            measureInstXml.WriteAttributeString("Name", "Operand2");
            measureInstXml.WriteAttributeString("Type", "MeasureRef");
            measureInstXml.WriteAttributeString("Value", measureIns2);
            measureInstXml.WriteEndElement();

            // Close XML Element
            measureInstXml.WriteEndElement();

            AddMeasurementInstance(instanceID);

            return instanceID;
        }

        protected void CreateRTPMETriggerInstances(string userID, string measureInstanceID)
        {
            string instanceID = null;

            // Create the measurement instance definition
            XmlWriter measureInstXml = GetMeasureFileID(measureInstanceID, measureInstXmlMap);
            if (measureInstXml == null)
            {
                return;
            }

            foreach (PhaseTriggerData phaseTriggerData in phaseTriggerDataList)
            {
                instanceID = phaseTriggerData.TriggerName + "_" + userID;

                if (IsMeasurementInstanceDefined(instanceID))
                {
                    return;
                }

                measureInstXml.WriteStartElement("MeasurementInstance");
                measureInstXml.WriteAttributeString("InstanceOf", phaseTriggerData.TriggerName);
                measureInstXml.WriteAttributeString("ID", instanceID);
                measureInstXml.WriteAttributeString("Visibility", "Visible");

                measureInstXml.WriteStartElement("ParameterValue");
                measureInstXml.WriteAttributeString("Name", "UserID");
                measureInstXml.WriteAttributeString("Value", userID);
                measureInstXml.WriteEndElement();

                // Close XML Element
                measureInstXml.WriteEndElement();

                AddMeasurementInstance(instanceID);
            }
        }

        private bool IsMeasurementInstanceDefined(string instanceID)
        {
            if ((instanceID == null) || (instanceID.Length == 0))
            {
                return false;
            }

            return definedMeasureInstances.Contains(instanceID);
        }

        private bool AddMeasurementInstance(string instanceID)
        {
            if ((instanceID == null) || (instanceID.Length == 0))
            {
                return false;
            }

            if (definedMeasureInstances.Contains(instanceID))
            {
                return false;
            }

            definedMeasureInstances.Add(instanceID);

            System.Diagnostics.Trace.WriteLine("Instance Added: " + instanceID);

            return true;
        }

        static XmlWriter GetMeasureFileID(string measureDefID, Dictionary<string, XmlWriter> measureInstXmlMap)
        {
            string measureFileID = null;

            if (measureDefID.Contains("Communication"))
            {
                measureFileID = "CoVE_Communication";
            }
            else if (measureDefID.Contains("Outcome"))
            {
                measureFileID = "CoVE_Outcome";
            }
            else if (measureDefID.Contains("Workload"))
            {
                measureFileID = "CoVE_Workload_Triggers";
            }
            else if (measureDefID.Contains("Execution"))
            {
                measureFileID = "CoVE_Execution";
            }
            else
            {
                measureFileID = "CoVE_Communication";
            }

            XmlWriter measureInstXml = measureInstXmlMap[measureFileID];
            if (measureInstXml == null)
            {
                return null;
            }

            return measureInstXml;
        }

    }
}
