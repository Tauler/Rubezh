﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.GK;

namespace FiresecClient
{
	public partial class GKManager
	{
		public static void ChangeDeviceZones(GKDevice device, List<GKZone> zones)
		{
			foreach (var zone in device.Zones)
			{
				zone.Devices.Remove(device);
				zone.OnChanged();
			}
			device.Zones.Clear();
			device.ZoneUIDs.Clear();
			foreach (var zone in zones)
			{
				device.Zones.Add(zone);
				device.ZoneUIDs.Add(zone.UID);
				zone.Devices.Add(device);
				zone.OnChanged();
			}
			device.OnChanged();
		}

		public static void AddDeviceToZone(GKDevice device, GKZone zone)
		{
			if (!device.Zones.Contains(zone))
				device.Zones.Add(zone);
			if (!device.ZoneUIDs.Contains(zone.UID))
				device.ZoneUIDs.Add(zone.UID);
			zone.Devices.Add(device);
			zone.OnChanged();
			device.OnChanged();
		}

		public static void RemoveDeviceFromZone(GKDevice device, GKZone zone)
		{
			if (zone != null)
			{
				device.Zones.Remove(zone);
				device.ZoneUIDs.Remove(zone.UID);
				zone.Devices.Remove(device);
				zone.OnChanged();
				device.OnChanged();
			}
		}

		public static void AddDevice(GKDevice device)
		{
			device.InitializeDefaultProperties();
		}

		public static void RemoveDevice(GKDevice device)
		{
			var parentDevice = device.Parent;
			foreach (var zone in device.Zones)
			{
				zone.Devices.Remove(device);
				zone.OnChanged();
			}
			foreach (var direction in device.Directions)
			{
				direction.InputDevices.Remove(device);
				direction.OutputDevices.Remove(device);
				direction.OnChanged();
			}
			parentDevice.Children.Remove(device);
			Devices.Remove(device);
		}

		#region RebuildRSR2Addresses
		public static void RebuildRSR2Addresses(GKDevice parentDevice)
		{
			foreach (var shliefDevice in parentDevice.Children)
			{
				RebuildRSR2Addresses_Children = new List<GKDevice>();
				RebuildRSR2Addresses_AddChild(shliefDevice);

				byte currentAddress = 1;
				foreach (var device in RebuildRSR2Addresses_Children)
				{
					device.IntAddress = currentAddress;
					if (!device.Driver.IsGroupDevice)
					{
						currentAddress++;
					}
					device.OnChanged();
				}
			}
		}

		static List<GKDevice> RebuildRSR2Addresses_Children;
		static void RebuildRSR2Addresses_AddChild(GKDevice device)
		{
			if (device.DriverType != GKDriverType.RSR2_MVP_Part && device.DriverType != GKDriverType.RSR2_KAU_Shleif)
				RebuildRSR2Addresses_Children.Add(device);

			foreach (var child in device.Children)
			{
				RebuildRSR2Addresses_AddChild(child);
			}
		}
		#endregion

		public static void AddZone(GKZone zone)
		{
			Zones.Add(zone);
		}

		public static void RemoveZone(GKZone zone)
		{
			foreach (var device in zone.Devices)
			{
				device.Zones.Remove(zone);
				device.ZoneUIDs.Remove(zone.UID);
				device.OnChanged();
			}
			foreach (var direction in zone.Directions)
			{
				direction.InputZones.Remove(zone);
				direction.OnChanged();
			}
			Zones.Remove(zone);
			zone.OnChanged();
		}

		public static void EditZone(GKZone zone)
		{
			foreach (var device in zone.Devices)
			{
				device.OnChanged();
			}
			foreach (var device in zone.DevicesInLogic)
			{
				device.OnChanged();
			}
			foreach (var direction in zone.Directions)
			{
				direction.OnChanged();
			}
			zone.OnChanged();
		}

		public static void AddDirection(GKDirection direction)
		{
			Directions.Add(direction);
		}

		public static void RemoveDirection(GKDirection direction)
		{
			foreach (var zone in direction.InputZones)
			{
				zone.Directions.Remove(direction);
				zone.OnChanged();
			}
			Directions.Remove(direction);
			direction.OnChanged();
		}

		public static void ChangeDirectionZones(GKDirection direction, List<GKZone> zones)
		{
			foreach (var zone in direction.InputZones)
			{
				zone.Directions.Remove(direction);
				zone.OnChanged();
			}
			direction.InputZones.Clear();
			foreach (var zone in zones)
			{
				direction.InputZones.Add(zone);
				var directionZone = new GKDirectionZone()
				{
					ZoneUID = zone.UID,
					Zone = zone
				};
				zone.Directions.Add(direction);
				zone.OnChanged();
			}
			direction.OnChanged();
		}

		public static void ChangeDirectionDevices(GKDirection direction, List<GKDevice> devices)
		{
			foreach (var device in direction.InputDevices)
			{
				device.Directions.Remove(direction);
				device.OnChanged();
			}
			direction.InputDevices.Clear();
			foreach (var device in devices)
			{
				var directionDevice = new GKDirectionDevice()
				{
					DeviceUID = device.UID,
					Device = device
				};
				if(device.Driver.AvailableStateBits.Contains(GKStateBit.Fire1))
				{
					directionDevice.StateBit = GKStateBit.Fire1;
				}
				else if (device.Driver.AvailableStateBits.Contains(GKStateBit.Fire2))
				{
					directionDevice.StateBit = GKStateBit.Fire2;
				}
				else if (device.Driver.AvailableStateBits.Contains(GKStateBit.On))
				{
					directionDevice.StateBit = GKStateBit.On;
				}
				direction.InputDevices.Add(device);
				device.Directions.Add(direction);
				device.OnChanged();
			}
			direction.OnChanged();
		}

		public static void ChangeLogic(GKDevice device, GKLogic logic)
		{
			foreach (var clause in device.Logic.OnClausesGroup.Clauses)
			{
				foreach (var direction in clause.Directions)
				{
					direction.OutputDevices.Remove(device);
					direction.OnChanged();
					device.Directions.Remove(direction);
				}
			}
			device.Logic = logic;
			DeviceConfiguration.InvalidateOneLogic(device, device.Logic);
			device.OnChanged();
		}

		public static void ChangeDriver(GKDevice device, GKDriver driver)
		{
			var changeZone = !(device.Driver.HasZone && driver.HasLogic);
			device.Driver = driver;
			device.DriverUID = driver.UID;
			if (driver.IsRangeEnabled)
				device.IntAddress = driver.MinAddress;

			device.Children.Clear();
			if (driver.IsGroupDevice)
			{
				var groupDriver = GKManager.Drivers.FirstOrDefault(x => x.DriverType == device.Driver.GroupDeviceChildType);

				for (byte i = 0; i < device.Driver.GroupDeviceChildrenCount; i++)
				{
					var autoDevice = GKManager.AddChild(device, null, groupDriver, (byte)(device.IntAddress + i));
				}
			}

			if (changeZone)
			{
				RemoveDeviceFromZone(device, null);
				ChangeLogic(device, new GKLogic());
			}
			device.Properties = new List<GKProperty>();
		}
	}
}