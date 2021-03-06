﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public class KAUIndicator_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0x103,
				DriverType = GKDriverType.KAUIndicator,
				UID = new Guid("17A2B7D1-CB62-4AF7-940E-BC30B004B0D0"),
				Name = "Индикатор Неисправность КАУ",
				ShortName = "Индикатор Неисправность КАУ",
				CanEditAddress = false,
				HasAddress = false,
				IsAutoCreate = true,
				MinAddress = 1,
				MaxAddress = 1,
				IsDeviceOnShleif = false,
				IsPlaceable = true
			};

			driver.AvailableStateClasses.Add(XStateClass.Norm);
			driver.AvailableStateClasses.Add(XStateClass.Unknown);
			driver.AvailableStateClasses.Add(XStateClass.On);

			var modeProperty = new GKDriverProperty()
			{
				No = 0,
				Name = "Mode",
				Caption = "Режим работы",
				ToolTip = "Режим работы индикатора",
				Default = 1,
				DriverPropertyType = GKDriverPropertyTypeEnum.EnumType,
				IsAUParameter = true
			};
			modeProperty.Parameters.Add(new GKDriverPropertyParameter() { Name = "Выключено", Value = 0 });
			modeProperty.Parameters.Add(new GKDriverPropertyParameter() { Name = "Включено", Value = 1 });
			modeProperty.Parameters.Add(new GKDriverPropertyParameter() { Name = "Мерцает", Value = 2 });
			driver.Properties.Add(modeProperty);

			driver.Properties.Add(
				new GKDriverProperty()
				{
					No = 1,
					Name = "OnDuration",
					Caption = "Продолжительность горения для режима 2, мс",
					ToolTip = "Продолжительность горения для режима 2, мс",
					Default = 0,
					DriverPropertyType = GKDriverPropertyTypeEnum.IntType,
					IsAUParameter = true
				}
				);

			driver.Properties.Add(
				new GKDriverProperty()
				{
					No = 2,
					Name = "OffDuration",
					Caption = "Продолжительность гашения для режима 2, мс",
					ToolTip = "Продолжительность гашения для режима 2, мс",
					DriverPropertyType = GKDriverPropertyTypeEnum.IntType,
					Default = 0,
					IsAUParameter = true
				}
				);

			return driver;
		}
	}
}