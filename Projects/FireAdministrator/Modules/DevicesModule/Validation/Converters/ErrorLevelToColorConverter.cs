﻿using System;
using System.Windows.Data;
using System.Windows.Media;
using FiresecClient.Validation;

namespace DevicesModule.Validation.Converters
{
    public class ErrorLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((ErrorLevel) value)
            {
                case ErrorLevel.CannotSave:
                    return Brushes.DarkRed;

                case ErrorLevel.CannotWrite:
                    return Brushes.OrangeRed;

                case ErrorLevel.Warning:
                    return Brushes.Orange;

                default:
                    return Brushes.Orange;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}