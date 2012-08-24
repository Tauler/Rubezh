﻿using System;
using System.Windows.Data;
using FiresecAPI;

namespace FiltersModule.Converters
{
	public class StateTypeToEventNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return FiresecAPI.Models.EnumsConverter.StateTypeToEventName((StateType)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}