﻿using System.Collections.Generic;
using System.Linq;
using DevicesModule.ViewModels;
using FiresecAPI.Models;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using DevicesModule.DeviceProperties;

namespace DevicesModule.ViewModels
{
	public class OneDeviceParameterViewModel : BaseViewModel
	{
		public Device Device { get; private set; }
		public List<StringPropertyViewModel> StringProperties { get; set; }
		public List<BoolPropertyViewModel> BoolProperties { get; set; }
		public List<EnumPropertyViewModel> EnumProperties { get; set; }

		public OneDeviceParameterViewModel(Device device)
		{
			Device = device;
			StringProperties = new List<StringPropertyViewModel>();
			BoolProperties = new List<BoolPropertyViewModel>();
			EnumProperties = new List<EnumPropertyViewModel>();
			if (device != null)
			{
				foreach (var driverProperty in device.Driver.Properties)
				{
					if (driverProperty.IsAUParameter)
					{
						if (device.Driver.DriverType == DriverType.Exit)
							continue;

						if (device.Driver.DriverType == DriverType.PumpStation && driverProperty.Name == "AllowControl")
							continue;

						if (driverProperty.IsHidden || driverProperty.IsControl)
							continue;

						if (IsFakeTimeoutProperty(driverProperty))
							continue;

						switch (driverProperty.DriverPropertyType)
						{
							case DriverPropertyTypeEnum.EnumType:
								EnumProperties.Add(new EnumPropertyViewModel(driverProperty, device));
								break;

							case DriverPropertyTypeEnum.StringType:
							case DriverPropertyTypeEnum.IntType:
							case DriverPropertyTypeEnum.ByteType:
								StringProperties.Add(new StringPropertyViewModel(driverProperty, device));
								break;

							case DriverPropertyTypeEnum.BoolType:
								BoolProperties.Add(new BoolPropertyViewModel(driverProperty, device));
								break;
						}
					}
				}
			}
		}

		bool IsFakeTimeoutProperty(DriverProperty driverProperty)
		{
			switch (Device.Driver.DriverType)
			{
				case DriverType.RM_1:
				case DriverType.MRO:
				case DriverType.MRO_2:
				case DriverType.MDU:
				case DriverType.Valve:
				case DriverType.MPT:
					if (driverProperty.Name == "Timeout")
						return true;
					return false;
			}
			return false;
		}

		bool _isAuParametersReady = true;
		public bool IsAuParametersReady
		{
			get { return _isAuParametersReady; }
			set
			{
				_isAuParametersReady = value;
				OnPropertyChanged("IsAuParametersReady");
			}
		}
	}
}