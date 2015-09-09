using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Data.Linq;
using System.Windows.Shapes;
using System.Windows;

using DashboardDataAccess;

namespace VisualizationDashboard
{

    public class VisibilityConverter : IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object [] value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
        {
            //string param = parameter.ToString();
            string param = "DisplayComplete";
            Visibility retVal = Visibility.Hidden;

            switch (param)
            {
                case "DisplayComplete":
                    if (value.Length != 4)
                    {
                        break;
                    }
                    
                    if (!(value[0] is int))
                    {
                        break;
                    }

                    if (!(value[1] is ObservableEntitySetWrapper<DisplayFactor>))
                    {
                        break;
                    }

                    if (!(value[2] is int))
                    {
                        break;
                    }

                    if (!(value[3] is ObservableEntitySetWrapper<DisplayBlockedFactor>))
                    {
                        break;
                    }

                    int numFactors = (int) value[0];
                    ObservableEntitySetWrapper<DisplayFactor> displayFactors = value[1] as ObservableEntitySetWrapper<DisplayFactor>;
                    int numBlockedFactors = (int)value[2];
                    ObservableEntitySetWrapper<DisplayBlockedFactor> displayBlockedFactors = value[3] as ObservableEntitySetWrapper<DisplayBlockedFactor>;

                    if ((numFactors > 0) && (displayFactors.Count == numFactors) &&
                        (displayBlockedFactors.Count == numBlockedFactors))
                    {
                        retVal = Visibility.Visible;
                    }
                    else if ((numFactors > 0) && (displayFactors.Count == numFactors - 1) &&
                        (displayBlockedFactors.Count == numBlockedFactors + 1))
                    {
                        retVal = Visibility.Visible;
                    }
                    else
                    {
                        retVal = Visibility.Hidden;
                    }

                    break;
                default:
                    break;
            }

            return retVal;
        }

        public object [] ConvertBack(object value,
                                  Type [] targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion

        #region Singleton Implementation

        private static VisibilityConverter visibilityInstance = new VisibilityConverter();

        private VisibilityConverter()
        {
        }

        public static VisibilityConverter VisibilityInstance
        {
            get
            {
                return visibilityInstance;
            }
        }

        #endregion

    }

    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if ((targetType != typeof(bool)) && (targetType != typeof(Visibility)))
                throw new InvalidOperationException("The target must be a boolean or Visibility");

            if (targetType == typeof(bool))
                return !(bool)value;

            if (value == null)
            {
                return Visibility.Visible;
            }

            if (((bool)value) == true)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class StringToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            String inputStr = null;

            if ((targetType != typeof(bool)) && (targetType != typeof(Visibility)))
                throw new InvalidOperationException("The target must be a boolean or Visibility");

            if (targetType == typeof(bool))
            {
                if ((value == null) || (!(value is String)))
                {
                    return false;
                }

                inputStr = (String)value;

                return (inputStr.Length > 0);
            }

            if ((value == null) || (!(value is String)))
            {
                return Visibility.Collapsed;
            }

            inputStr = (String)value;

            if (inputStr.Length > 0)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

}