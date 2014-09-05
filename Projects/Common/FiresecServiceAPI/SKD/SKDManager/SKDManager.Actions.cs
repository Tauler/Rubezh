﻿using System;
using System.Linq;

namespace FiresecAPI.SKD
{
	public partial class SKDManager
	{
		public static void EditDevice(SKDDevice device)
		{
			if (device.Door != null)
				device.Door.OnChanged();
			device.OnChanged();
		}

		public static void DeleteDevice(SKDDevice device)
		{
			foreach (var subDevice in device.Children)
				DeleteDeviceInternal(subDevice);
			DeleteDeviceInternal(device);
			if (device.Parent != null)
				device.Parent.Children.Remove(device);
		}
		private static void DeleteDeviceInternal(SKDDevice device)
		{
			if (device.Zone != null)
			{
				device.Zone.Devices.Remove(device);
				device.Zone.OnChanged();
			}
			if (device.Door != null)
			{
				device.Door.InDeviceUID = Guid.Empty;
				device.Door.OutDeviceUID = Guid.Empty;
				device.Door.OnChanged();
			}
			Devices.Remove(device);
			device.OnChanged();
		}

		public static void ChangeDeviceZone(SKDDevice device, SKDZone zone)
		{
			if (device.Zone != null && device.Zone != zone)
			{
				device.Zone.Devices.Remove(device);
				device.Zone.OnChanged();
			}
			else
			{
				zone.Devices.Add(device);
			}
			device.ZoneUID = zone.UID;
			device.Zone = zone;
		}

		public static void EditZone(SKDZone zone)
		{
			foreach (var device in zone.Devices)
				device.OnChanged();
			zone.OnChanged();
		}

		public static void RemoceZone(SKDZone zone)
		{
			foreach (var device in zone.Devices)
			{
				device.Zone = null;
				device.ZoneUID = Guid.Empty;
				device.OnChanged();
			}
			Zones.Remove(zone);
			zone.OnChanged();
		}

		public static void ChangeDoorDevice(SKDDoor door, SKDDevice device)
		{
			if (door.InDevice != null)
			{
				door.InDevice.Door = null;
			}
			door.InDeviceUID = device.UID;
			device.Door = door;
			UpdateDoor(door);
		}

		public static void UpdateDoor(SKDDoor door)
		{
			door.InDevice = Devices.FirstOrDefault(x => x.UID == door.InDeviceUID);
			door.OutDevice = Devices.FirstOrDefault(x => x.UID == door.OutDeviceUID);

			if (door.InDevice != null)
			{
				switch (door.DoorType)
				{
					case DoorType.OneWay:
						door.OutDevice = door.InDevice.Parent.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Button && x.IntAddress == door.InDevice.IntAddress);
						break;

					case DoorType.TwoWay:
						door.OutDevice = door.InDevice.Parent.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Reader && x.IntAddress == door.InDevice.IntAddress + 1);
						break;
				}
				if (door.OutDevice != null)
				{
					door.OutDeviceUID = door.OutDevice.UID;
				}
			}

			if (door.InDevice != null)
			{
				door.InDevice.Door = door;
			}
			if (door.OutDevice != null)
			{
				door.OutDevice.Door = door;
			}
		}

		public static void RemoveDoor(SKDDoor door)
		{
			if (door.InDevice != null)
			{
				door.InDevice.Door = null;
				door.InDevice.OnChanged();
			}

			if (door.OutDevice != null)
			{
				door.OutDevice.Door = null;
				door.OutDevice.OnChanged();
			}

			SKDManager.SKDConfiguration.Doors.Remove(door);
			door.OnChanged();
		}
	}
}