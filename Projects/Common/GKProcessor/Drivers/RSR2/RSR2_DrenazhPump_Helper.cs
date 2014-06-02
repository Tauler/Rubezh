﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public class RSR2_DrenazhPump_Helper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0xE0,
				DriverType = XDriverType.RSR2_Bush,
				UID = new Guid("1743FA7E-EF69-45B7-90CD-D9BF2B44644C"),
				Name = "Блок управления ДН",
				ShortName = "БУШ ДН RSR2",
				IsControlDevice = true,
				HasLogic = true,
				IgnoreHasLogic = true,
				IsPlaceable = true
			};
			GKDriversHelper.AddControlAvailableStates(driver);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.AutoOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.On);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);

			driver.AvailableCommandBits.Add(XStateBit.TurnOn_InManual);
			driver.AvailableCommandBits.Add(XStateBit.TurnOnNow_InManual);
			driver.AvailableCommandBits.Add(XStateBit.TurnOff_InManual);
			driver.AvailableCommandBits.Add(XStateBit.TurnOffNow_InManual);

			GKDriversHelper.AddIntProprety(driver, 0, "Задержка на включение, с", 2, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 1, "Задержка на выключение, с", 2, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 2, "Питание, 0.1 В", 80, 0, 100);
			GKDriversHelper.AddIntProprety(driver, 3, "Порог 1, Ом", 340, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 4, "Порог 2, Ом", 660, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 5, "Порог 3, Ом", 2350, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 6, "Порог 4, Ом", 3350, 0, 65535);
			GKDriversHelper.AddIntProprety(driver, 7, "Порог 5, Ом", 4500, 0, 65535);

			driver.MeasureParameters.Add(new XMeasureParameter() { No = 1, Name = "Отсчет задержки на включение, с", IsDelay = true });
			driver.MeasureParameters.Add(new XMeasureParameter() { No = 2, Name = "Отсчет задержки на выключение, с", IsDelay = true });

			return driver;
		}
	}
}