﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public class Shuv_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0x86,
				DriverType = GKDriverType.Shuv,
				UID = new Guid("70C76BEF-E5FE-4DAC-B183-3C6F10FFDF1C"),
				Name = "Шкаф управления вентилятором",
				ShortName = "ШУВ",
				IsControlDevice = true,
				HasLogic = true,
				IsPlaceable = true,
				IsIgnored = true,
			};

			GKDriversHelper.AddControlAvailableStates(driver);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.AutoOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.On);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOn);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);

			driver.AvailableCommandBits.Add(GKStateBit.TurnOn_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOnNow_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.TurnOff_InManual);
			driver.AvailableCommandBits.Add(GKStateBit.Stop_InManual);

			GKDriversHelper.AddPlainEnumProprety2(driver, 0x82, "Внешний сигнал шкафа управления", 0, "Сигнал с кнопок «Пуск» и «Стоп»", "Сигнал с датчика", 0);
			GKDriversHelper.AddIntProprety(driver, 0x83, "Время удержания запуска, мин. 0 - неограничено", 0, 0, 255);
			GKDriversHelper.AddIntProprety(driver, 0x84, "Время отложенного запуска, с", 0, 0, 255);
			GKDriversHelper.AddIntProprety(driver, 0x85, "Время ожидания выхода на режим, с. 0 - не ждать сигнала", 0, 0, 255);

			return driver;
		}
	}
}