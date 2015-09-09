using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace AME.Controllers.Base.TypeConversion
{

    public class MilitaryTimeConverter : TypeConverter
    {
        public static String MilitaryTimeFormat = "MM/dd/yyyy HHmm";
        public static String StandardTimeFormat = "G";

        public MilitaryTimeConverter() { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                DateTime ds;
                try
                {
                    ds = DateTime.ParseExact((string)value, new string[] { StandardTimeFormat }, culture, DateTimeStyles.AllowWhiteSpaces);
                }
                catch (Exception)
                {
                    try
                    {
                        ds = DateTime.ParseExact((string)value, new string[] { MilitaryTimeFormat }, culture, DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (Exception)
                    {
                        ds = DateTime.Parse((string)value);
                    }
                }
                return ds;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(DateTime))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((DateTime)value).ToString(MilitaryTimeFormat);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (value is string)
            {
                try
                {
                    DateTime ds;
                    try
                    {
                        ds = DateTime.ParseExact((string)value, new string[] { StandardTimeFormat }, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (Exception)
                    {
                        ds = DateTime.ParseExact((string)value, new string[] { MilitaryTimeFormat }, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}


