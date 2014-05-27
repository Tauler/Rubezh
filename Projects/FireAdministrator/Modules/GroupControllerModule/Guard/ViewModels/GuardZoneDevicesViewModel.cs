﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.GK;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;

namespace GKModule.ViewModels
{
	public class GuardZoneDevicesViewModel : BaseViewModel
	{
		XGuardZone Zone;

		public GuardZoneDevicesViewModel()
		{
			AddCommand = new RelayCommand<object>(OnAdd, CanAdd);
			RemoveCommand = new RelayCommand<object>(OnRemove, CanRemove);
			Devices = new ObservableCollection<GuardZoneDeviceViewModel>();
			AvailableDevices = new ObservableCollection<GuardZoneDeviceViewModel>();
		}

		public void Initialize(XGuardZone zone)
		{
			Zone = zone;

			var devices = new HashSet<XDevice>();
			var availableDevices = new HashSet<XDevice>();

			foreach (var device in XManager.Devices)
			{
				if (device.IsInMPT)
					continue;

				if (device.Driver.HasLogic)
				{
					foreach (var clause in device.DeviceLogic.ClausesGroup.Clauses)
					{
						foreach (var clauseZone in clause.Zones)
						{
							if (clauseZone.BaseUID == zone.BaseUID)
							{
								devices.Add(device);
							}
						}
					}
				}

				if (device.Driver.HasZone)
				{
					if (device.ZoneUIDs.Contains(Zone.BaseUID))
					{
						devices.Add(device);
					}
					else
					{
						if (device.ZoneUIDs.Count == 0)
						{
							availableDevices.Add(device);
						}
					}
				}
			}

			Devices = new ObservableCollection<GuardZoneDeviceViewModel>();
			foreach (var device in devices)
			{
				var deviceViewModel = new GuardZoneDeviceViewModel(device)
				{
					IsBold = device.ZoneUIDs.Contains(Zone.BaseUID)
				};
				Devices.Add(deviceViewModel);
			}

			var selectedDevice = Devices.LastOrDefault();
			AvailableDevices = new ObservableCollection<GuardZoneDeviceViewModel>();
			foreach (var device in availableDevices)
			{
				if ((device.DriverType == XDriverType.GKIndicator) ||
					(device.DriverType == XDriverType.GKLine) ||
					(device.DriverType == XDriverType.GKRele) ||
					(device.DriverType == XDriverType.KAUIndicator))
					continue;

				var deviceViewModel = new GuardZoneDeviceViewModel(device)
				{
					IsBold = device.Driver.HasZone
				};

				AvailableDevices.Add(deviceViewModel);
			}

			var selectedAvailableDevice = AvailableDevices.LastOrDefault();
			//AvailableDevices = new ObservableCollection<ZoneDeviceViewModel>(AvailableDevices.Where(x => x.Device.Parent == null));
			OnPropertyChanged(() => Devices);
			OnPropertyChanged(() => AvailableDevices);

			SelectedDevice = selectedDevice;
			SelectedAvailableDevice = selectedAvailableDevice;
		}

		public void Clear()
		{
			Devices.Clear();
			AvailableDevices.Clear();
			SelectedDevice = null;
			SelectedAvailableDevice = null;
		}

		public void UpdateAvailableDevices()
		{
			OnPropertyChanged("AvailableDevices");
		}

		public ObservableCollection<GuardZoneDeviceViewModel> Devices { get; private set; }

		GuardZoneDeviceViewModel _selectedDevice;
		public GuardZoneDeviceViewModel SelectedDevice
		{
			get { return _selectedDevice; }
			set
			{
				_selectedDevice = value;
				OnPropertyChanged("SelectedDevice");
			}
		}

		public ObservableCollection<GuardZoneDeviceViewModel> AvailableDevices { get; private set; }

		GuardZoneDeviceViewModel _selectedAvailableDevice;
		public GuardZoneDeviceViewModel SelectedAvailableDevice
		{
			get { return _selectedAvailableDevice; }
			set
			{
				_selectedAvailableDevice = value;
				OnPropertyChanged("SelectedAvailableDevice");
			}
		}

		public RelayCommand<object> AddCommand { get; private set; }
		public IList SelectedAvailableDevices;
		void OnAdd(object parameter)
		{
			var availableDevicesIndex = AvailableDevices.IndexOf(SelectedAvailableDevice);
			var devicesIndex = Devices.IndexOf(SelectedDevice);

			SelectedAvailableDevices = (IList)parameter;
			var availabledeviceViewModels = new List<GuardZoneDeviceViewModel>();
			foreach (var availabledevice in SelectedAvailableDevices)
			{
				var availabledeviceViewModel = availabledevice as GuardZoneDeviceViewModel;
				if (availabledeviceViewModel != null)
					availabledeviceViewModels.Add(availabledeviceViewModel);
			}
			foreach (var availabledeviceViewModel in availabledeviceViewModels)
			{
				Devices.Add(availabledeviceViewModel);
				AvailableDevices.Remove(availabledeviceViewModel);
				//XManager.AddDeviceToZone(availabledeviceViewModel.Device, Zone);
			}

			Initialize(Zone);

			availableDevicesIndex = Math.Min(availableDevicesIndex, AvailableDevices.Count - 1);
			if (availableDevicesIndex > -1)
				SelectedAvailableDevice = AvailableDevices[availableDevicesIndex];

			devicesIndex = Math.Min(devicesIndex, Devices.Count - 1);
			if (devicesIndex > -1)
				SelectedDevice = Devices[devicesIndex];

			ServiceFactory.SaveService.GKChanged = true;
		}
		public bool CanAdd(object parameter)
		{
			return SelectedAvailableDevice != null;
		}

		public RelayCommand<object> RemoveCommand { get; private set; }
		public IList SelectedDevices;
		void OnRemove(object parameter)
		{
			var devicesIndex = Devices.IndexOf(SelectedDevice);
			var availableDevicesIndex = AvailableDevices.IndexOf(SelectedAvailableDevice);

			SelectedDevices = (IList)parameter;
			var deviceViewModels = new List<GuardZoneDeviceViewModel>();
			foreach (var device in SelectedDevices)
			{
				var deviceViewModel = device as GuardZoneDeviceViewModel;
				if (deviceViewModel != null)
					deviceViewModels.Add(deviceViewModel);
			}
			foreach (var deviceViewModel in deviceViewModels)
			{
				AvailableDevices.Add(deviceViewModel);
				Devices.Remove(deviceViewModel);
				//XManager.RemoveDeviceFromZone(deviceViewModel.Device, Zone);
			}

			Initialize(Zone);

			availableDevicesIndex = Math.Min(availableDevicesIndex, AvailableDevices.Count - 1);
			if (availableDevicesIndex > -1)
				SelectedAvailableDevice = AvailableDevices[availableDevicesIndex];

			devicesIndex = Math.Min(devicesIndex, Devices.Count - 1);
			if (devicesIndex > -1)
				SelectedDevice = Devices[devicesIndex];

			ServiceFactory.SaveService.GKChanged = true;
		}
		public bool CanRemove(object parameter)
		{
			return SelectedDevice != null && SelectedDevice.Device.Driver.HasZone;
		}
	}
}