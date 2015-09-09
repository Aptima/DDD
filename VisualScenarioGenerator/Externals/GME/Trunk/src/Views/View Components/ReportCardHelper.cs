/*****************************************************
Project		: MOST
Author		: Art Rogers
Copyright	: 2008 Aptima, Inc.
******************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

namespace AME.Views.View_Components {

    public class ReportCardHelper {

        // These "things" are specific to the MOST algorithm - we need to make real constants
        private const string decisionMakerID = "K";
        private const string assettID = "M";
        private const string taskID = "T";
        private const string taskComplete = "tasksCompletedPerDM";
        private const string taskIncomplete = "tasksNotCompletedPerDM";

        private int numberOfDM = 0;
        private int numberOfAsset = 0;
        private int numberOfTask = 0;

        private int numberOfTaskIncomplete = 0;

        //*************************************************
        // Construction/Destruction
        //*************************************************
        public ReportCardHelper() {
        }

        //*************************************************
        // Public Methods
        //*************************************************
        public ReportCardModel GetReportCardData(string outputFileName, string inputFileName) {
            ReportCardModel model = new ReportCardModel();
            try {
                FileStream stream = new FileStream(outputFileName, FileMode.Open);
                XmlTextReader reader = new XmlTextReader(stream);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                stream.Close();
                XmlNode node;
                DataRow row;
                foreach (XmlElement element in doc.DocumentElement) {
                    if (element.Name == "Measures") {
                        for (int i=element.ChildNodes.Count-1; i>-1; i--) {
                            node = element.ChildNodes[i];
                            if (node.Name == "Measure") {
                                row = model.Tables[ReportCardModel.TABLE_REPORT_CARD].NewRow();
                                foreach (XmlAttribute att in node.Attributes) {
                                    if (att.Name == "displayName") {
                                        row[ReportCardModel.MEASURE_NAME] = att.InnerText;
                                        break;
                                    }
                                }
                                model.Tables[ReportCardModel.TABLE_REPORT_CARD].Rows.Add(row);
                            }
                            //foreach (XmlNode node1 in node.ChildNodes) {
                            //    System.Diagnostics.Debug.WriteLine("Output Node1: " + node1.Name + " = " + node1.InnerText);
                            //    foreach (XmlNode node2 in node1.ChildNodes) {
                            //        System.Diagnostics.Debug.WriteLine("Output Node2: " + node2.Name + " = " + node2.InnerText);
                            //    }
                            //}
                        }
                    }
                    else if (element.Name == "ComponentParameters") {
                        for (int i=element.ChildNodes.Count-1; i>-1; i--) {
                            node = element.ChildNodes[i];
                            if (node.Name == "Parameter") {
                                row = model.Tables[ReportCardModel.TABLE_REPORT_CARD].NewRow();
                                foreach (XmlAttribute att in node.Attributes) {
                                    if (att.Name == "displayName") {
                                        row[ReportCardModel.MEASURE_NAME] = att.InnerText;
                                        break;
                                    }
                                }
                                model.Tables[ReportCardModel.TABLE_REPORT_CARD].Rows.Add(row);
                            }
                        }
                    }
                }
                reader.Close();
                stream = new FileStream(inputFileName, FileMode.Open);
                reader = new XmlTextReader(stream);
                doc = new XmlDocument();
                doc.Load(reader);
                stream.Close();
                foreach (XmlElement element in doc.DocumentElement) {
                    System.Diagnostics.Debug.WriteLine("Input Element: " + element.Name + " = " + element.InnerText);
                    if (element.Name == "Parameter") {
                        foreach (XmlAttribute att in element.Attributes) {
                            if (att.Name == "name") {
                                if (att.Value == decisionMakerID || att.Value == assettID || att.Value == taskID) {
                                    // There is only one piece of data to find
                                    for (int i=element.ChildNodes.Count-1; i>-1; i--) {
                                        node = element.ChildNodes[i];
                                        //System.Diagnostics.Debug.WriteLine("Input Node: " + node.Name + " = " + node.InnerText);
                                        foreach (XmlNode node1 in node.ChildNodes) {
                                            //System.Diagnostics.Debug.WriteLine("Input Node1: " + node1.Name + " = " + node1.InnerText);
                                            foreach (XmlNode node2 in node1.ChildNodes) {
                                                if (node2.Name == "Value") {
                                                    switch (att.Value) {
                                                    case decisionMakerID:
                                                        numberOfDM = Convert.ToInt16(node2.InnerText);
                                                        break;
                                                    case assettID:
                                                        numberOfAsset = Convert.ToInt16(node2.InnerText);
                                                        break;
                                                    case taskID:
                                                        numberOfTask = Convert.ToInt16(node2.InnerText);
                                                        break;
                                                    }
                                                }
                                                //System.Diagnostics.Debug.WriteLine("Input Node2: " + node2.Name + " = " + node2.InnerText);
                                            }
                                        }
                                    }
                                }
                                else if (att.Value == taskIncomplete) {
                                    for (int i=element.ChildNodes.Count-1; i>-1; i--) {
                                        node = element.ChildNodes[i];
                                        //System.Diagnostics.Debug.WriteLine("Input Node: " + node.Name + " = " + node.InnerText);
                                        foreach (XmlNode node1 in node.ChildNodes) {
                                            //System.Diagnostics.Debug.WriteLine("Input Node1: " + node1.Name + " = " + node1.InnerText);
                                            foreach (XmlNode node2 in node1.ChildNodes) {
                                                if (node2.Name == "Value") {
                                                    numberOfTaskIncomplete += Convert.ToInt16(node2.InnerText);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine("DM count = " + numberOfDM);
                System.Diagnostics.Debug.WriteLine("Asset count = " + numberOfAsset);
                System.Diagnostics.Debug.WriteLine("Task count = " + numberOfTask);
                System.Diagnostics.Debug.WriteLine("Incomplete Task count = " + numberOfTaskIncomplete);
                reader.Close();
                return model;
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
