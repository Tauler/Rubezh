﻿using System;
using System.Linq;
using System.Windows.Data;
using FiresecAPI.GK;
using FiresecClient;

namespace GKModule.Converters
{
	public class JournalItemToIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var journalItem = (JournalItem)value;
			switch (journalItem.JournalItemType)
			{
				case JournalItemType.Device:
					var device = XManager.Devices.FirstOrDefault(x => x.BaseUID == journalItem.ObjectUID);
					if (device == null)
						return "/Controls;component/Images/blank.png";
					return device.Driver.ImageSource;

				case JournalItemType.Zone:
					return "/Controls;component/Images/zone.png";

				case JournalItemType.Direction:
					return "/Controls;component/Images/Blue_Direction.png";

				case JournalItemType.Delay:
					return "/Controls;component/Images/Delay.png";

				case JournalItemType.Pim:
					return "/Controls;component/Images/Pim.png";

				case JournalItemType.GkUser:
					return "/Controls;component/Images/Chip.png";

				case JournalItemType.PumpStation:
					return "/Controls;component/Images/BPumpStation.png";

				case JournalItemType.MPT:
					return "/Controls;component/Images/BMPT.png";

				default:
					return "/Controls;component/Images/blank.png";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}
}