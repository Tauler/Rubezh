﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace XFiresecAPI
{
	[DataContract]
	public class MPTDevice
	{
		public MPTDevice()
		{
			MPTDeviceType = MPTDeviceType.Unknown;
			Delay = 0;
			Hold = 2;
		}

		public XDevice Device { get; set; }

		[DataMember]
		public Guid DeviceUID { get; set; }

		[DataMember]
		public MPTDeviceType MPTDeviceType { get; set; }

		[DataMember]
		public int Delay { get; set; }

		[DataMember]
		public int Hold { get; set; }

		public static List<MPTDeviceType> GetAvailableMPTDeviceTypes(XDriverType driverType)
		{
			var result = new List<MPTDeviceType>();
			switch (driverType)
			{
				case XDriverType.RSR2_MVK8:
					result.Add(MPTDeviceType.Bomb);
					break;

				case XDriverType.RSR2_OPS:
					result.Add(MPTDeviceType.DoNotEnterBoard);
					result.Add(MPTDeviceType.ExitBoard);
					result.Add(MPTDeviceType.AutomaticOffBoard);
					break;

				case XDriverType.RSR2_OPZ:
					result.Add(MPTDeviceType.Speaker);
					break;

				case XDriverType.RSR2_OPK:
					result.Add(MPTDeviceType.DoNotEnterBoard);
					result.Add(MPTDeviceType.ExitBoard);
					result.Add(MPTDeviceType.AutomaticOffBoard);
					result.Add(MPTDeviceType.Speaker);
					break;

				case XDriverType.RSR2_AM_1:
					result.Add(MPTDeviceType.Door);
					result.Add(MPTDeviceType.HandAutomatic);
					result.Add(MPTDeviceType.HandStart);
					result.Add(MPTDeviceType.HandStop);
					break;

				default:
					result.Add(MPTDeviceType.Unknown);
					break;
			}
			return result;
		}
	}
}