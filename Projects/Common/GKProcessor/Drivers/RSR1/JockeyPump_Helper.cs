﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public static class JockeyPump_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0x70,
				DriverType = GKDriverType.JockeyPump,
				UID = new Guid("1EB96235-8275-445F-9EB2-8F92157764F9"),
				Name = "Шкаф управления жокей насосом",
				ShortName = "Жокей насос",
				IsControlDevice = true,
				HasLogic = false,
				IsPlaceable = true,
				MaxAddressOnShleif = 15,
				IsIgnored = true,
			};

			GKDriversHelper.AddControlAvailableStates(driver);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.AutoOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.On);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOn);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);


			driver.AvailableCommandBits.Add(GKStateBit.TurnOn_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOnNow_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOff_InManual);

			GKDriversHelper.AddIntProprety(driver, 0x84, "Время ожидания ВД, мин", 2, 2, 65535);

			var property3 = new GKDriverProperty()
			{
				No = 0x8d,
				Name = "Тип контакта датчика НД",
				Caption = "Тип контакта датчика НД",
				DriverPropertyType = GKDriverPropertyTypeEnum.EnumType,
				IsLowByte = true,
				Mask = 1
			};
			property3.Parameters.Add(new GKDriverPropertyParameter() { Name = "Нормально разомкнутый", Value = 0 });
			property3.Parameters.Add(new GKDriverPropertyParameter() { Name = "Нормально замкнутый", Value = 1 });
			driver.Properties.Add(property3);

			var property4 = new GKDriverProperty()
			{
				No = 0x8d,
				Name = "Тип контакта датчика ВД",
				Caption = "Тип контакта датчика ВД",
				DriverPropertyType = GKDriverPropertyTypeEnum.EnumType,
				IsLowByte = true,
				Mask = 2
			};
			driver.Properties.Add(property4);
			property4.Parameters.Add(new GKDriverPropertyParameter() { Name = "Нормально разомкнутый", Value = 0 });
			property4.Parameters.Add(new GKDriverPropertyParameter() { Name = "Нормально замкнутый", Value = 2 });

			var manometerProperty = new GKDriverProperty()
			{
				No = 0x8d,
				Name = "Конфигурация",
				Caption = "Конфигурация",
				ToolTip = "Тип конфигурации",
				Default = 0,
				DriverPropertyType = GKDriverPropertyTypeEnum.EnumType,
				IsHieghByte = true,
				Mask = 1
			};
			manometerProperty.Parameters.Add(new GKDriverPropertyParameter() { Name = "Два одноконтактных", Value = 0 });
			manometerProperty.Parameters.Add(new GKDriverPropertyParameter() { Name = "Один двухконтактный", Value = 1 });
			driver.Properties.Add(manometerProperty);

			driver.MeasureParameters.Add(new GKMeasureParameter() { No = 0x80, Name = "Режим работы" });
			return driver;
		}
	}
}
