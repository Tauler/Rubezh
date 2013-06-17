﻿using System;
using XFiresecAPI;

namespace Common.GK
{
	public class RSR2_CombinedDetector_Helper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0xDF,
				DriverType = XDriverType.RSR2_CombinedDetector,
				UID = new Guid("1CCE48EB-B60B-4E06-8290-A39591CD3DA2"),
				Name = "Пожарный комбинированный извещатель ИПК RSR2",
				ShortName = "ИПК RSR2",
				HasZone = true,
                IsPlaceable = true
			};
			GKDriversHelper.AddAvailableStates(driver, XStateType.Test);
			GKDriversHelper.AddAvailableStates(driver, XStateType.Fire1);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Info);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire1);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Service);

			GKDriversHelper.AddIntProprety(driver, 0, "Порог срабатывания по температуре, C", 0, 70, 0, 100);
			GKDriversHelper.AddIntProprety(driver, 1, "Порог срабатывания по градиенту температуры, C/мин", 0, 5, 1, 50);
			GKDriversHelper.AddIntProprety(driver, 2, "Порог срабатывания по дыму, 0.001*дБ/м", 0, 180, 50, 200);
			GKDriversHelper.AddIntProprety(driver, 3, "Порог запыленности, 0.001*дБ/м", 0, 200, 0, 500);

			driver.AUParameters.Add(new XAUParameter() { No = 1, Name = "Температура, C", InternalName = "Temperature" });
			driver.AUParameters.Add(new XAUParameter() { No = 2, Name = "Градиент температуры", InternalName = "TemperatureDelta" });
			driver.AUParameters.Add(new XAUParameter() { No = 3, Name = "Дым, 0.001*дБ/м", InternalName = "Dustinness" });
			driver.AUParameters.Add(new XAUParameter() { No = 4, Name = "Пыль, 0.001*дБ/м", InternalName = "Smokeness" });
			driver.AUParameters.Add(new XAUParameter() { No = 5, Name = "Дата последнего обслуживания", InternalName = "LastServiceTime" });

			return driver;
		}
	}
}