﻿using System;
using FiresecAPI.GK;

namespace FiresecAPI.SKD
{
	public static class ChinaController_1_Helper
	{
		public static SKDDriver Create()
		{
			var driver = new SKDDriver()
			{
				UID = new Guid("3D8FEF42-BAF6-422D-9A4A-E6EF0072896D"),
				Name = "Контроллер на одну дверь",
				ShortName = "Контроллер",
				DriverType = SKDDriverType.ChinaController_1,
				DefaultDoorType = DoorType.TwoWay,
				CanChangeDoorType = false,
				IsPlaceable = true
			};
			driver.Children.Add(SKDDriverType.Reader);
			driver.AvailableStateClasses.Add(XStateClass.Norm);
			driver.AvailableStateClasses.Add(XStateClass.Failure);
			driver.AvailableStateClasses.Add(XStateClass.Unknown);

			driver.AutocreationItems.Add(new SKDDriverAutocreationItem(SKDDriverType.Reader, 2));
			driver.AutocreationItems.Add(new SKDDriverAutocreationItem(SKDDriverType.Lock, 1));
			driver.AutocreationItems.Add(new SKDDriverAutocreationItem(SKDDriverType.LockControl, 1));
			driver.AutocreationItems.Add(new SKDDriverAutocreationItem(SKDDriverType.Button, 1));

			driver.Properties = CommonChinaControllerHelper.CreateProperties();
			return driver;
		}
	}
}