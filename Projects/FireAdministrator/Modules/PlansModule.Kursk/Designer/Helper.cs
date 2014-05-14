﻿using System;
using System.Linq;
using System.Windows.Media;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecClient;

namespace PlansModule.Kursk.Designer
{
	internal static class Helper
	{
		public static void SetXDevice(ElementRectangleTank element, XDevice device)
		{
			element.XDeviceUID = device == null ? Guid.Empty : device.BaseUID;
			element.BackgroundColor = GetTankColor(device);
		}
		public static void SetXDevice(ElementRectangleTank element)
		{
			XDevice xdevice = GetXDevice(element);
			SetXDevice(element, xdevice);
		}
		public static XDevice GetXDevice(ElementRectangleTank element)
		{
			return element.XDeviceUID == Guid.Empty ? null : XManager.DeviceConfiguration.Devices.Where(item => item.DriverType == XDriverType.RSR2_Bush && item.BaseUID == element.XDeviceUID).FirstOrDefault();
		}
		public static Color GetTankColor(XDevice xdevice)
		{
			Color color = Colors.Black;
			if (xdevice != null)
				color = Colors.LightCyan;
			return color;
		}
		public static string GetTankTitle(ElementRectangleTank element)
		{
			var device = GetXDevice(element);
			return device == null ? "Бак" : "Бак " + device.DottedAddress;
		}
	}
}