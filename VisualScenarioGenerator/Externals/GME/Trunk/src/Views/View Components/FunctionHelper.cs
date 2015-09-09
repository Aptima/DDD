using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AME.Nodes;
using System.Xml.XPath;
using AME.Model;
using AME.Controllers;
using System.IO;
using AME;
using System.Drawing;
using AME.Views.View_Components.CoordinateTransform;

namespace AME.Views.View_Components
{
    public class ParsedFunction
    {
        private String functionName;
        private String actionIdentifier;
        private List<String> actionValues;

        public String FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }

        public String ActionIdentifier
        {
            get { return actionIdentifier; }
            set { actionIdentifier = value; }
        }

        public List<String> ActionValues
        {
            get { return actionValues; }
            set { actionValues = value; }
        }

        public ParsedFunction()
        {
            actionIdentifier = String.Empty;
            actionValues = new List<String>();
        }

        public ParsedFunction(String p_functionName, String p_actionIdentifier, List<String> p_actionValues) 
        {
            functionName = p_functionName;
            actionIdentifier = p_actionIdentifier;
            actionValues = p_actionValues;
        }
    }

    public class FunctionHelper
    {
        // node nav should have parameters enabled to lookup visual representation parameter value
        public static List<ParsedFunction> ParseFunctions(List<Function> functions)
        {
            List<ParsedFunction> returnList = new List<ParsedFunction>();

            String functionName = String.Empty;

            String atDelimiter = "@";

            // parse VisualRepresentation=Polygon@Location.Polygon Points
            foreach (Function f in functions)
            {
                List<String> functionValues = new List<String>();

                String functionAction = f.FunctionAction;

                int firstAt = functionAction.IndexOf(atDelimiter);
                int firstAtPlusPLus = firstAt + 1;

                String actionIdentifier = functionAction;

                if (firstAt != -1)
                {
                    actionIdentifier = functionAction.Substring(0, firstAt);
                }

                String tempValue;

                int nextAt = functionAction.IndexOf(atDelimiter, firstAtPlusPLus);

                while (nextAt != -1) // are there more values?  loop through and add them
                {
                    tempValue = functionAction.Substring(firstAtPlusPLus, nextAt - firstAtPlusPLus);
                    functionValues.Add(tempValue);

                    firstAtPlusPLus = nextAt+1;
                    nextAt = functionAction.IndexOf(atDelimiter, firstAtPlusPLus);
                }
                
                // last one
                tempValue = functionAction.Substring(firstAtPlusPLus, functionAction.Length - (firstAtPlusPLus));
                functionValues.Add(tempValue);

                returnList.Add(new ParsedFunction(f.FunctionName, actionIdentifier, functionValues));
            }
            
            return returnList;
        }

        public static String UpdateImageList(IController callingController, Dictionary<String, Bitmap> passedList, String imageFilePath)
        {
            String fileInfoString = imageFilePath;
            Stream imageStream;
            FileStream streamFromFileInfo = null;

            if (!imageFilePath.Contains("\\")) // not like "C:\task.png, e.g. "task.png"
            {
                imageStream = callingController.GetImage(imageFilePath);
            }
            else
            {
                FileInfo fileInfo = new FileInfo(fileInfoString);
                streamFromFileInfo = fileInfo.Open(FileMode.Open);
                imageStream = streamFromFileInfo;
            }

            if (!passedList.ContainsKey(fileInfoString))
            {
                Bitmap bitmap = new Bitmap(imageStream);
                passedList.Add(fileInfoString, new Bitmap(bitmap)); // key on node ID
                if (streamFromFileInfo != null)
                {
                    streamFromFileInfo.Close();
                }
                bitmap.Dispose();
                imageStream.Close();
            }
            return fileInfoString;
        }

        public static List<PointF> ParsePolygonString(String polygonString, String componentType, ICoordinateTransform coordinateTransformer) 
        {
            List<PointF> returnList = new List<PointF>();

            // left, center, right   referring to ( , ) which wraps a point (1 , 2) 
            int searchFrom = 0;

            while (searchFrom != -1 && polygonString.Length > 0)
            {
                int left = polygonString.IndexOf("(", searchFrom);
                int center = polygonString.IndexOf(",", searchFrom);
                int right = polygonString.IndexOf(")", searchFrom);

                int leftPlusPlus = left + 1;
                int centerPlusPlus = center + 1;

                String xString = polygonString.Substring(leftPlusPlus, center - leftPlusPlus);
                String yString = polygonString.Substring(centerPlusPlus, right - centerPlusPlus);

                xString = xString.Replace("\r\n","").Trim();
                yString = yString.Replace("\r\n","").Trim();

                if (xString.Length > 0 && yString.Length > 0)
                {
                    float xPoly = float.Parse(xString);
                    float yPoly = float.Parse(yString);

                    // use transform
                    xPoly = coordinateTransformer.RetrieveX(xPoly);
                    yPoly = coordinateTransformer.RetrieveY(yPoly);

                    returnList.Add(new PointF(xPoly, yPoly));
                }

                searchFrom = polygonString.IndexOf(",", right) + 1;

                if (searchFrom == 0)
                {
                    searchFrom = -1; // index returns 0 as failure, but we want 0 on the first iteration, so use -1
                }
            } // point load while

            coordinateTransformer.PostProcessPolygon(returnList, componentType);

            return returnList;
        }

        public static String UpdateImageList(IController callingController, ImageList passedList, String imageFilePath, Boolean useConfigPath)
        {
            String fileInfoString = imageFilePath;
            Stream imageStream = null;
            FileStream streamFromFileInfo = null;

            if (useConfigPath)
            {
                if (!imageFilePath.Contains("\\")) // not like "C:\task.png, e.g. "task.png"
                {
                    imageStream = callingController.GetImage(imageFilePath);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(fileInfoString);
                    streamFromFileInfo = fileInfo.Open(FileMode.Open);
                    imageStream = streamFromFileInfo;
                }
            }

            if (!passedList.Images.ContainsKey(fileInfoString))
            {
                if (imageStream != null)
                {
                    Bitmap bitmap = new Bitmap(imageStream);
                    passedList.Images.Add(fileInfoString, new Bitmap(bitmap)); // key on node ID
                    if (streamFromFileInfo != null)
                    {
                        streamFromFileInfo.Close();
                    }
                    bitmap.Dispose();
                }
            }
            return fileInfoString;
        }

        private static Dictionary<string, XPathExpression> seenXPaths = new Dictionary<string, XPathExpression>();

        public static String GetParameterValue(String fullParamName, XPathNavigator nodeNav)
        {
            String paramCategory, shortParamName;

            String paramValue = String.Empty;

            int delim = fullParamName.IndexOf(SchemaConstants.ParameterDelimiter);

            paramCategory = fullParamName.Substring(0, delim);
            shortParamName = fullParamName.Substring(delim + 1, fullParamName.Length - (delim + 1));

            XPathExpression xpath;
            if (!seenXPaths.ContainsKey(fullParamName))
            {
                String forCompile = String.Format("ComponentParameters/Parameter/Parameter[@category='{0}'][@displayedName='{1}']", paramCategory, shortParamName);
                seenXPaths.Add(fullParamName, XPathExpression.Compile(forCompile));
            }

            xpath = seenXPaths[fullParamName];

            XPathNodeIterator paramNav = nodeNav.Select(xpath);

            if (paramNav != null && paramNav.Count == 1)
            {
                paramNav.MoveNext();
                paramValue = paramNav.Current.GetAttribute(ConfigFileConstants.Value, String.Empty);
            }

            return paramValue;
        }

        public static string ProcessNavForImage(IController callingController, ImageList passedList, String componentType, XPathNavigator nav, List<Function> functions, Dictionary<String, String> parameterMap)
        {
            List<ParsedFunction> parsedFunctions = FunctionHelper.ParseFunctions(functions);
            return ProcessNavForImage(callingController, passedList, componentType, nav, parsedFunctions, parameterMap);
        }

        public static string ProcessNavForImage(IController callingController, ImageList passedList, String componentType, XPathNavigator nav, List<Function> functions)
        {
            List<ParsedFunction> parsedFunctions = FunctionHelper.ParseFunctions(functions);
            return ProcessNavForImage(callingController, passedList, componentType, nav, parsedFunctions, null);
        }

        private static string ProcessNavForImage(IController callingController, ImageList passedList, String componentType, XPathNavigator nav, List<ParsedFunction> parsedFunctions, Dictionary<String, String> parameterMap) 
        {
            String imagePath = componentType;

            //ParsedFunction vrImagePF = parsedFunctions.Find(delegate(ParsedFunction pf) { return pf.FunctionName == "VisualRepresentation" && pf.ActionIdentifier == "Image";  });
            ParsedFunction vrPF = parsedFunctions.Find(delegate(ParsedFunction pf) { return pf.FunctionName == "VisualRepresentation"; });
            if (vrPF != null)
            {
                String functionName = vrPF.FunctionName;
                String actionIdentifier = vrPF.ActionIdentifier;
                List<string> actionValues = vrPF.ActionValues;

                switch (actionIdentifier)
                {
                    case "Image":
                        if (actionValues.Count >= 1)
                        {
                            String paramValueForImage;
                            if (parameterMap != null)
                            {
                                paramValueForImage = parameterMap[actionValues[0]];
                            }
                            else
                            {

                                paramValueForImage = FunctionHelper.GetParameterValue(actionValues[0], nav);
                            }

                            try
                            {
                                if (paramValueForImage != null && paramValueForImage.Length > 0)
                                {
                                    imagePath = FunctionHelper.UpdateImageList(callingController, passedList, paramValueForImage, true);
                                }
                                else
                                {
                                    MessageBox.Show("Could not find image");
                                    imagePath = componentType; // use type
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Image Load");
                                imagePath = componentType; // use type
                            }
                        }
                        break;
                    case "DLL":
                        if (actionValues.Count >= 1)
                        {
                            String paramValueForImage;
                            if (parameterMap != null)
                            {
                                paramValueForImage = parameterMap[actionValues[0]];
                            }
                            else
                            {

                                paramValueForImage = FunctionHelper.GetParameterValue(actionValues[0], nav);
                            }

                            try
                            {
                                if (paramValueForImage != null && paramValueForImage.Length > 0)
                                {
                                    imagePath = FunctionHelper.UpdateImageList(callingController, passedList, paramValueForImage, false);
                                }
                                else
                                {
                                    //MessageBox.Show("Could not find image");
                                    imagePath = componentType; // use type
                                }
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show(ex.Message, "Image Load");
                                imagePath = componentType; // use type
                                throw;
                            }
                        }
                        break;
                }
            }
            return imagePath;
        }

        public static List<Function> GetFunctions(XPathNavigator nav)
        {
            List<Function> nodeFunctions = new List<Function>();

            XPathNodeIterator itFunctions = nav.Select("Functions/Function");

            while (itFunctions.MoveNext())
            {
                String name = itFunctions.Current.GetAttribute(XmlSchemaConstants.Display.Function.Name, itFunctions.Current.NamespaceURI);
                String action = itFunctions.Current.GetAttribute(XmlSchemaConstants.Display.Function.Action, itFunctions.Current.NamespaceURI);
                bool visible = Boolean.Parse(itFunctions.Current.GetAttribute(XmlSchemaConstants.Display.Function.Visible, itFunctions.Current.NamespaceURI));
                Function function = new Function(name, action, visible);
                XPathNodeIterator itArguments = itFunctions.Current.Select("Argument");
                List<String> arguments = new List<String>();
                while (itArguments.MoveNext())
                {
                    arguments.Add(itArguments.Current.Value);
                }
                if (arguments.Count > 0)
                    function.Arguments = arguments.ToArray();
                nodeFunctions.Add(function);
            }

            return nodeFunctions;
        }

        public static List<Function> GetFunctions(int RootID, int ComponentID, String linkType, IController passedController)
        {
            ComponentOptions compOptions = new ComponentOptions();
            compOptions.LevelDown = 0;
            IXPathNavigable comp = passedController.GetComponentAndChildren(RootID, ComponentID, linkType, compOptions);

            if (comp != null)
            {
                XPathNavigator nav = comp.CreateNavigator();

                //XPathNodeIterator iterator = nav.Select("//Component[@" + XmlSchemaConstants.Display.Component.Id + "='" + ComponentID + "']");

                XPathNodeIterator iterator = nav.Select("/Components/Component");

                if (iterator.Count > 0)
                {
                    iterator.MoveNext();

                    return GetFunctions(iterator.Current);
                }
            }

            return new List<Function>();
        }
    }
}
