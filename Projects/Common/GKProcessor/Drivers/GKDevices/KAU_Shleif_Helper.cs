﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public static class KAU_Shleif_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverType = GKDriverType.KAU_Shleif,
				UID = new Guid("2DF54B82-88E5-4168-9D9D-DDE1CFB71294"),
				Name = "АЛС",
				ShortName = "АЛС",
				CanEditAddress = false,
				HasAddress = true,
				IsAutoCreate = true,
				MinAddress = 1,
				MaxAddress = 8,
				IsDeviceOnShleif = false,
				IsPlaceable = false,
				IsReal = false,
				IsIgnored = true
			};

			return driver;
		}
	}
}