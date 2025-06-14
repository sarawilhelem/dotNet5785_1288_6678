﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    class ConvertUpdateToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string act = (string)value;
            return act switch
            {
                "UPDATE" => true,
                "ADD" => (object)false,
                _ => throw new NotImplementedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConvertUpdateToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string act = (string)value;
            return act switch
            {
                "UPDATE" => Visibility.Visible,
                "ADD" => (object)Visibility.Collapsed,
                _ => throw new NotImplementedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConvertAddToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string act = (string)value;
            return act switch
            {
                "ADD" => Visibility.Visible,
                "UPDATE" => (object)Visibility.Collapsed,
                _ => throw new NotImplementedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConvertDateTime : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("MM/dd/yyyy HH:mm:ss"); // Adjust format as needed
            }
            return value;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParse(value.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }
    }

    public class BooleanToNullableConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? true : (bool?) null;
            }
            return null; // In case of other types, return null
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue; // Return the boolean as-is
            }
            if (value == null)
            {
                return false; // Null means false
            }
            return false; // Default to false if not boolean or null
        }
    }

}
