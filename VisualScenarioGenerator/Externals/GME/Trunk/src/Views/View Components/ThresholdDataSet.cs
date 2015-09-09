using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.XPath;
using AME.Controllers;

namespace AME.Views.View_Components
{
    public class ThresholdDataSet
    {

        private ThresholdData organizationData, roleData, billetData, informationProductData, activityData;
        public ThresholdData OrganizationData
        {
            get { return organizationData;  }
        }
        public ThresholdData RoleData
        {
            get { return roleData; }
        }
        public ThresholdData BilletData
        {
            get { return billetData; }
        }
        public ThresholdData InformationProductData
        {
            get { return informationProductData; }
        }
        public ThresholdData ActivityData
        {
            get { return activityData; }
        }

        private ThresholdData[] thresholdData;
        public ThresholdData[] AllSets
        {
            get { return thresholdData; }
        }

        public ThresholdDataSet(IController controller, Int32 configurationID)
        {
            organizationData = new ThresholdData("Organization");
            roleData = new ThresholdData("Role"); 
            billetData = new ThresholdData("Billet"); 
            informationProductData = new ThresholdData("Information Product"); 
            activityData = new ThresholdData("Activity");
            thresholdData = new ThresholdData[] { organizationData, roleData, billetData, informationProductData, activityData };
            
            XPathNodeIterator thresholds = controller.GetComponentAndChildren(configurationID, "Configuration", new ComponentOptions()).CreateNavigator().Select("/Components/Component/Component[@Type='Threshold']");
            if (thresholds.Count != 0)
            {
                ColorConverter cv = new ColorConverter();
                String baseString = "ComponentParameters/Parameter[@category='Threshold']/Parameter[@displayedName='";
                foreach (XPathNavigator threshold in thresholds)
                {
                    String name = threshold.GetAttribute("Name", "");
                    Double _lv = Double.Parse(threshold.SelectSingleNode(baseString + "LowerThresholdValue']/@value").ToString());
                    Double _mv1 = Double.Parse(threshold.SelectSingleNode(baseString + "MiddleThresholdValue1']/@value").ToString());
                    Double _mv2 = Double.Parse(threshold.SelectSingleNode(baseString + "MiddleThresholdValue2']/@value").ToString());
                    Double _uv = Double.Parse(threshold.SelectSingleNode(baseString + "UpperThresholdValue']/@value").ToString());
                    Boolean _lowerCheck = Boolean.Parse(threshold.SelectSingleNode(baseString + "LowerThresholdValueIsEqual']/@value").ToString());
                    Boolean _middleCheck1 = Boolean.Parse(threshold.SelectSingleNode(baseString + "MiddleThresholdValue1IsEqual']/@value").ToString());
                    Boolean _middleCheck2 = Boolean.Parse(threshold.SelectSingleNode(baseString + "MiddleThresholdValue2IsEqual']/@value").ToString());
                    Boolean _upperCheck = Boolean.Parse(threshold.SelectSingleNode(baseString + "UpperThresholdValueIsEqual']/@value").ToString());
                    Color _lowerColor = (Color)cv.ConvertFromString(threshold.SelectSingleNode(baseString + "LowerThresholdColor']/@value").ToString());
                    Color _middleColor = (Color)cv.ConvertFromString(threshold.SelectSingleNode(baseString + "MiddleThresholdColor']/@value").ToString());
                    Color _upperColor = (Color)cv.ConvertFromString(threshold.SelectSingleNode(baseString + "UpperThresholdColor']/@value").ToString());
                    bool found = false;
                    for(int i = 0; i < thresholdData.Length; i++)
                    {
                        ThresholdData data = thresholdData[i];
                        if (data.Name.Equals(name))
                        {
                            found = true;
                            data.LowerValue = _lv;
                            data.MiddleValue1 = _mv1;
                            data.MiddleValue2 = _mv2;
                            data.UpperValue = _uv;

                            data.LowerCheck = _lowerCheck;
                            data.MiddleCheck1 = _middleCheck1;
                            data.MiddleCheck2 = _middleCheck2;
                            data.UpperCheck = _upperCheck;

                            data.LowerColor = _lowerColor;
                            data.MiddleColor = _middleColor;
                            data.UpperColor = _upperColor;
                        }
                    }
                    if (!found)
                    {
                        throw new Exception("Could not find threshold data: " + name);
                    }
                }
            }
        }
    }
}
