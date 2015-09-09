using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

using DashboardDataAccess;

namespace DashboardPermissionTool
{


public class CategoryCheckConverter : IValueConverter
{
   #region IValueConverter Members

   public object Convert(object value,
                         Type targetType,
                         object parameter,
                         System.Globalization.CultureInfo culture)
   {
        string trueValue;
        string subTrueValue;
        string falseValue;
        string param = parameter.ToString();
        
        switch (param)
	    {
            case "Visibility":
                trueValue = "Visible";
                subTrueValue = "Visible";
                falseValue = "Hidden";
                break;
            case "FontWeight":
                trueValue = "Bold";
                subTrueValue = "Bold";
                falseValue = "None";
                break;
            case "Margin":
                trueValue = "0";
                subTrueValue = "20,0,0,0";
                falseValue = "40,0,0,0";
                break;
		    default:
                trueValue = "True";
                subTrueValue = "True";
                falseValue = "False";
                break;
	    }        

        if ((value != null) && (value is MeasureSelection))
        {
           MeasureRowType rowType = ((MeasureSelection)value).MeasureRowType;
           bool result = ((rowType == MeasureRowType.CategoryType) || (rowType == MeasureRowType.SubCategoryType));


           if (result)
           {
               if (rowType == MeasureRowType.CategoryType)
               {
                   return trueValue;
               }
               else
               {
                   return subTrueValue;
               }
           }
           else
           {
               return falseValue;
           }
        }
        else
        {
            return falseValue;
        }
   }
 
   public object ConvertBack(object value,
                             Type targetType,
                             object parameter,
                             System.Globalization.CultureInfo culture)
   {
      return null;
   }
 
   #endregion

#region Singleton Implementation

   private static CategoryCheckConverter categoryCheckInstance = new CategoryCheckConverter(); 

   private CategoryCheckConverter()
   {
   }

   public static CategoryCheckConverter CategoryCheckInstance
   {
       get
       {
           return categoryCheckInstance;
       }
   }

#endregion

}

public class RadioButtonColorConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value,
                          Type targetType,
                          object parameter,
                          System.Globalization.CultureInfo culture)
    {
        if ((value != null) && (value is MeasureSelection))
        {
            RadioButtonType buttonType;
            string param = parameter.ToString();

            switch (param)
            {
                case "All":
                    buttonType = ((MeasureSelection)value).All;
                    break;
                case "Operator1":
                    buttonType = ((MeasureSelection)value)[0];
                    break;
                case "Operator2":
                    buttonType = ((MeasureSelection)value)[1];
                    break;
                case "Operator3":
                    buttonType = ((MeasureSelection)value)[2];
                    break;
                default:
                    buttonType = RadioButtonType.Off;
                    break;
            }

            if (buttonType == RadioButtonType.On)
            {
                return "Green";
            }
            else if (buttonType == RadioButtonType.Partial)
            {
                return "Gray";
            }
            else
            {
                return "Red";
            }
        }
        else
        {
            return "Red";
        }
    }

    public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
    {
        return null;
    }

    #endregion

    #region Singleton Implementation

    private static RadioButtonColorConverter radioButtonColorConverterInstance = new RadioButtonColorConverter();

    private RadioButtonColorConverter()
    {
    }

    public static RadioButtonColorConverter RadioButtonColorConverterInstance
    {
        get
        {
            return radioButtonColorConverterInstance;
        }
    }

    #endregion

}

}
