﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public class AMP_1_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverTypeNo = 0x50,
				DriverType = GKDriverType.AMP_1,
				UID = new Guid("d8997f3b-64c4-4037-b176-de15546ce568"),
				Name = "Пожарная адресная метка АМП",
				ShortName = "АМП",
				HasZone = true,
				IsPlaceable = true,
				IsIgnored = true,
			};

			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Fire1);
			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Fire2);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire1);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire2);
			GKDriversHelper.AddAvailableStateBits(driver, GKStateBit.Test);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Test);

			var property1 = new GKDriverProperty()
			{
				No = 0x81,
				Name = "Тип шлейфа",
				Caption = "Тип шлейфа",
				Default = 0,
				Mask = 16 + 2*16 + 4*16 + 8*16
			};
			var property1Parameter1 = new GKDriverPropertyParameter()
			{
				Name = "Шлейф дымовых датчиков с определением двойной сработки",
				Value = 0*16
			};
			var property1Parameter2 = new GKDriverPropertyParameter()
			{
				Name = "Комбинированный шлейф дымовых и тепловых датчиков без определения двойной сработки тепловых датчиков и с определением двойной сработки дымовых",
				Value = 1 * 16
			};
			var property1Parameter3 = new GKDriverPropertyParameter()
			{
				Name = "Шлейф тепловых датчиков с определением двойной сработки",
				Value = 2 * 16
			};
			var property1Parameter4 = new GKDriverPropertyParameter()
			{
				Name = "Комбинированный шлейф дымовых и тепловых датчиков без определения двойной сработки и без контроля короткого замыкания ШС",
				Value = 3 * 16
			};
			property1.Parameters.Add(property1Parameter1);
			property1.Parameters.Add(property1Parameter2);
			property1.Parameters.Add(property1Parameter3);
			property1.Parameters.Add(property1Parameter4);
			driver.Properties.Add(property1);

			var property2 = new GKDriverProperty()
			{
				No = 0x81,
				Name = "Режим работы",
				Caption = "Режим работы",
				Default = 2,
				Mask = 15
			};
			var property2Parameter1 = new GKDriverPropertyParameter()
			{
				Name = "Не включать",
				Value = 0
			};
			var property2Parameter2 = new GKDriverPropertyParameter()
			{
				Name = "Переключается",
				Value = 1
			};
			var property2Parameter3 = new GKDriverPropertyParameter()
			{
				Name = "Включен постоянно",
				Value = 2
			};
			property2.Parameters.Add(property2Parameter1);
			property2.Parameters.Add(property2Parameter2);
			property2.Parameters.Add(property2Parameter3);
			driver.Properties.Add(property2);

			return driver;
		}
	}
}