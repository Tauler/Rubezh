﻿using System;
using XFiresecAPI;

namespace GKModule.Converter
{
	public static class GKIndicatorHelper
	{
		public static XDriver Create()
		{
			var xDriver = new XDriver()
			{
				DriverTypeNo = 0x103,
				DriverType = XDriverType.GKIndicator,
				UID = DriversHelper.GKIndicator_UID,
				OldDriverUID = Guid.Empty,
				CanEditAddress = false,
				HasAddress = true,
				IsAutoCreate = true,
				MinAutoCreateAddress = 2,
				MaxAutoCreateAddress = 11,
				ImageSource = FiresecClient.FileHelper.GetIconFilePath("Device_Device.ico"),
				HasImage = true,
				IsChildAddressReservedRange = false,
				Name = "Индикатор ГК",
				ShortName = "Индикатор ГК"
			};

			var modeProperty = new XDriverProperty()
			{
				No = 0,
				Name = "Mode",
				Caption = "Режим работы",
				ToolTip = "Режим работы индикатора",
				Default = 0,
				DriverPropertyType = XDriverPropertyTypeEnum.EnumType,
				IsInternalDeviceParameter = true
			};
			modeProperty.Parameters.Add(new XDriverPropertyParameter() { Name = "Не гореть", Value = 0 });
			modeProperty.Parameters.Add(new XDriverPropertyParameter() { Name = "Гореть 0.25 сек", Value = 1 });
			modeProperty.Parameters.Add(new XDriverPropertyParameter() { Name = "Гореть 0.5 сек", Value = 3 });
			modeProperty.Parameters.Add(new XDriverPropertyParameter() { Name = "Гореть 0.75 сек", Value = 7 });
			modeProperty.Parameters.Add(new XDriverPropertyParameter() { Name = "Мигать", Value = 15 });
			xDriver.Properties.Add(modeProperty);

			return xDriver;
		}
	}
}