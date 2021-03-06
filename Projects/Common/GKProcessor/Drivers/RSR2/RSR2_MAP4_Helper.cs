﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public static class RSR2_MAP4_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0xE1,
				DriverType = GKDriverType.RSR2_MAP4,
				UID = new Guid("42B3C448-2FDD-43D4-AEE0-F173CB8D6CF8"),
				Name = "Метка адресная пожарная АМП-R2",
				ShortName = "АМП-R2",
				HasZone = true,
				IsPlaceable = true
			};

			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Test);
			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Failure);
			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Fire1);
			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Fire2);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire1);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire2);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);

			var property1 = new GKDriverProperty()
			{
				No = 0,
				Name = "Конфигурация",
				Caption = "Конфигурация",
				Default = 0
			};
			var property1Parameter1 = new GKDriverPropertyParameter()
			{
				Name = "Шлейф дымовых датчиков",
				Value = 0
			};
			var property1Parameter2 = new GKDriverPropertyParameter()
			{
				Name = "Комбинированный шлейф",
				Value = 1
			};
			var property1Parameter3 = new GKDriverPropertyParameter()
			{
				Name = "Шлейф тепловых датчиков",
				Value = 2
			};
			property1.Parameters.Add(property1Parameter1);
			property1.Parameters.Add(property1Parameter2);
			property1.Parameters.Add(property1Parameter3);
			driver.Properties.Add(property1);

			GKDriversHelper.AddIntProprety(driver, 1, "Порог питания, 0.1 В", 80, 1, 1000);
			GKDriversHelper.AddIntProprety(driver, 2, "Порог 1, 0.1 Ом", 250, 1, 10000);
			GKDriversHelper.AddIntProprety(driver, 3, "Порог 2, 0.1 Ом", 750, 1, 10000);
			GKDriversHelper.AddIntProprety(driver, 4, "Порог 3, 0.1 Ом", 1500, 1, 10000);
			GKDriversHelper.AddIntProprety(driver, 5, "Порог 4, 0.1 Ом", 4500, 1, 10000);
			GKDriversHelper.AddIntProprety(driver, 6, "Порог 5, 0.1 Ом", 6000, 1, 10000);

			driver.MeasureParameters.Add(new GKMeasureParameter() { No = 1, Name = "Сопротивление, Ом", InternalName = "Resistance" });
			driver.MeasureParameters.Add(new GKMeasureParameter() { No = 2, Name = "Питание, В" });

			return driver;
		}
	}
}