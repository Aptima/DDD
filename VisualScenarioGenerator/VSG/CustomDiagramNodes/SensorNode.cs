using System;
using System.Collections.Generic;
using System.Text;
using AME.Views.View_Components;
using System.Xml.XPath;
using System.Drawing;
using System.Windows.Forms;
using AME.Nodes;
using AME.Controllers;
using AME.Model;
using Northwoods.Go;

namespace VSG.CustomDiagramNodes
{
    public class SensorNode
    {
        public SensorNode(Diagram dg, List<String> actionValues, XPathNavigator nodeNav, ImageList globalIl,
                int nodeID, int linkID, String nodeName, String nodeType, List<Function> nodeFunctions, 
                    bool xyFromDB, ref int startX, ref int startY, Dictionary<int, DiagramNode> docNodes)
        {
            // for now, copy DiagramNode creation.

            String imagePath = nodeType;

            //String imagePath = String.Empty;

            //String paramValueForImage = FunctionHelper.GetParameterValue(actionValues[0], nodeNav);

            //try
            //{
            //    imagePath = FunctionHelper.UpdateImageList(nodeImages, paramValueForImage);
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message, "Image Load");
            //    // use type

            //}

            dg.LoadNode(nodeID, linkID, nodeName, nodeType, nodeFunctions, imagePath, xyFromDB, ref startX, ref startY, docNodes);

            // also lookup and add sensor information
            ComponentOptions options = new ComponentOptions();
            options.LevelDown = 1;
            options.LinkParams = false;
            options.CompParams = false;

            String IDConstant = XmlSchemaConstants.Display.Component.Id;
            String ValueConstant = ConfigFileConstants.Value;

            XPathNavigator createEvent = null;
            IXPathNavigable navigable;
            XPathNavigator nav;


            // Event to CreateEvent
            navigable = dg.Controller.GetComponentAndChildren(nodeID, "EventID", options);
            nav = navigable.CreateNavigator();
            createEvent = nav.SelectSingleNode("Components/Component/Component");
      

            if (createEvent != null) 
            {
                int createEventID = Int32.Parse(createEvent.GetAttribute(IDConstant, nav.NamespaceURI));

                // CreateEvent to Species
                navigable = dg.Controller.GetComponentAndChildren(createEventID, "CreateEventKind", options);
                nav = navigable.CreateNavigator();
                XPathNavigator species = nav.SelectSingleNode("Components/Component/Component");
                if (species != null)
                {
                    int speciesID = Int32.Parse(species.GetAttribute(IDConstant, nav.NamespaceURI));

                    // Species to FullyFunctional
                    navigable = dg.Controller.GetComponentAndChildren(speciesID, "Scenario", options);
                    nav = navigable.CreateNavigator();
                    XPathNavigator fullyFunctional = nav.SelectSingleNode("Components/Component/Component[@Name='FullyFunctional']");
                    if (fullyFunctional != null)
                    {
                        int ffID = Int32.Parse(fullyFunctional.GetAttribute(IDConstant, nav.NamespaceURI));

                        // Species to Sensor
                        options.CompParams = true;
                        navigable = dg.Controller.GetComponentAndChildren(ffID, "StateSensor", options);
                        nav = navigable.CreateNavigator();
                        XPathNodeIterator allSensors = nav.Select("Components/Component/Component");

                        // Sensor.Attribute_Sensor
                        // Sensor.Global_Sensor
                        // Sensor.Attribute
                        // Sensor.Range

                        String globalSensor = String.Empty;
                        String attributeSensor = String.Empty;
                        String attribute = String.Empty;
                        String range = String.Empty;

                        int largestRange = 0;

                        foreach (XPathNavigator sensor in allSensors)
                        {
                            int sensorID = Int32.Parse(sensor.GetAttribute(IDConstant, nav.NamespaceURI));

                            // is it global or attribute = location
                            XPathNavigator globalNav = sensor.SelectSingleNode
                            ("ComponentParameters/Parameter/Parameter[@category='Sensor'][@displayedName='Global_Sensor']");
                            if (globalNav != null) 
                            {
                                globalSensor = globalNav.GetAttribute(ValueConstant, globalNav.NamespaceURI);
                            }
                            
                            XPathNavigator attributeSensorNav = sensor.SelectSingleNode
                            ("ComponentParameters/Parameter/Parameter[@category='Sensor'][@displayedName='Attribute_Sensor']");
                            if (attributeSensorNav != null) 
                            {
                                attributeSensor = attributeSensorNav.GetAttribute(ValueConstant, attributeSensorNav.NamespaceURI);
                            }
                            XPathNavigator attributeNav = sensor.SelectSingleNode
                            ("ComponentParameters/Parameter/Parameter[@category='Sensor'][@displayedName='Attribute']");
                            if (attributeNav != null) 
                            {
                                attribute = attributeNav.GetAttribute(ValueConstant, attributeNav.NamespaceURI);
                            }
                            XPathNavigator rangeNav = sensor.SelectSingleNode
                            ("ComponentParameters/Parameter/Parameter[@category='Sensor'][@displayedName='Range']");
                            if (rangeNav != null)
                            {
                                range = rangeNav.GetAttribute(ValueConstant, rangeNav.NamespaceURI);
                            }

                            if (globalSensor.Equals("true", StringComparison.CurrentCultureIgnoreCase) ||
                                (attributeSensor.Equals("true", StringComparison.CurrentCultureIgnoreCase) 
                                    && attribute.Equals("Location")) )
                            {
                                // use range
                                int rangeInt = Int32.Parse(range);

                                if (rangeInt > largestRange)
                                {
                                    largestRange = rangeInt;
                                }

                                String srSpread = String.Empty;
                                String srRange = String.Empty;

                                // check sensor ranges
                                navigable = dg.Controller.GetComponentAndChildren(sensorID, "Scenario", options);
                                nav = navigable.CreateNavigator();
                                XPathNodeIterator srsNav = nav.Select("Components/Component/Component[@Type='SensorRange']");
                                foreach (XPathNavigator srNav in srsNav)
                                {
                                    // check spread and range
                                    XPathNavigator spreadNav = srNav.SelectSingleNode
                                    ("ComponentParameters/Parameter/Parameter[@category='SensorRange'][@displayedName='Spread']");
                                    if (spreadNav != null)
                                    {
                                        srSpread = spreadNav.GetAttribute(ValueConstant, spreadNav.NamespaceURI);
                                    }

                                    XPathNavigator srRangeNav = srNav.SelectSingleNode
                                    ("ComponentParameters/Parameter/Parameter[@category='SensorRange'][@displayedName='Range']");
                                    if (srRangeNav != null)
                                    {
                                        srRange = srRangeNav.GetAttribute(ValueConstant, srRangeNav.NamespaceURI);
                                    }

                                    int spread = Int32.Parse(srSpread);

                                    if (spread == 360)
                                    {
                                        rangeInt = Int32.Parse(srRange);

                                        if (rangeInt > largestRange)
                                        {
                                            largestRange = rangeInt;
                                        }
                                    }
                                }
                            }
                        }

                        if (largestRange > 0) 
                        {
                            // meters to pixels
                            if (dg.CoordinateTransformer != null)
                            {
                                largestRange = (int)dg.CoordinateTransformer.RetrieveX(largestRange);
                            }

                            GoEllipse sensorEllipse = new GoEllipse();
                            sensorEllipse.Pen = new Pen(Color.Green, 2.0f);
                            sensorEllipse.Position = new PointF(startX-(largestRange/2), startY-(largestRange/2));
                            sensorEllipse.Size = new SizeF(largestRange, largestRange);
                            sensorEllipse.Movable = false;
                            sensorEllipse.Selectable = false;

                            dg.Document.Add(sensorEllipse);
                        }
                    }
                }
            }
        }
    }
}