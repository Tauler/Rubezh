﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public static class RM_1_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0x75,
				DriverType = GKDriverType.RM_1,
				UID = new Guid("4a60242a-572e-41a8-8b87-2fe6b6dc4ace"),
				Name = "Релейный исполнительный модуль РМ-1",
				ShortName = "РМ-1",
				IsControlDevice = true,
				HasLogic = true,
				IsPlaceable = true,
				IsIgnored = true,
			};

			GKDriversHelper.AddControlAvailableStates(driver);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.AutoOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.On);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOn);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOff);

			driver.AvailableCommandBits.Add(GKStateBit.TurnOn_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOnNow_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOff_InManual);

			var property1 = new GKDriverProperty()
			{
				No = 0x82,
				Name = "Конфигурация релейного модуля",
				Caption = "Конфигурация релейного модуля",
				Default = 1,
				IsLowByte = true
			};
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Выключено, Пуск - Включено", 1);
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Выключено, Пуск - Мерцает", 2);
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Включено,  Пуск - Выключено", 3);
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Включено,  Пуск - Мерцает", 4);
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Мерцает,   Пуск - Выключено", 5);
			GKDriversHelper.AddPropertyParameter(property1, "Стоп - Мерцает,   Пуск - Включено", 6);
			driver.Properties.Add(property1);

			GKDriversHelper.AddIntProprety(driver, 0x83, "Задержка на пуск, с", 0, 0, 255).IsLowByte=true;
			GKDriversHelper.AddIntProprety(driver, 0x83, "Время удержания, с", 0, 0, 255).IsHieghByte = true;

			var property2 = new GKDriverProperty()
			{
				No = 0x85,
				Name = "Тип контроля выхода",
				Caption = "Тип контроля выхода",
				Default = 1,
				IsLowByte=true
			};
			GKDriversHelper.AddPropertyParameter(property2, "Состояние цепи не контролируется", 1);
			GKDriversHelper.AddPropertyParameter(property2, "Цепь контролируется только на обрыв", 2);
			GKDriversHelper.AddPropertyParameter(property2, "Цепь контролируется только на короткое замыкание", 3);
			GKDriversHelper.AddPropertyParameter(property2, "Цепь контролируется на короткое замыкание и на обрыв", 4);
			driver.Properties.Add(property2);

			return driver;
		}
	}
}