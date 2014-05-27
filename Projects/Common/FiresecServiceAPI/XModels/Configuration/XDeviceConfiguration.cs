﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FiresecAPI.GK
{
	[DataContract]
	public partial class XDeviceConfiguration : VersionedConfiguration
	{
		public XDeviceConfiguration()
		{
			Devices = new List<XDevice>();
			Zones = new List<XZone>();
			Directions = new List<XDirection>();
			PumpStations = new List<XPumpStation>();
			MPTs = new List<XMPT>();
			JournalFilters = new List<XJournalFilter>();
			Instructions = new List<XInstruction>();
			GuardUsers = new List<XGuardUser>();
			GuardZones = new List<XGuardZone>();
			ParameterTemplates = new List<XParameterTemplate>();
		}

		public List<XDevice> Devices { get; set; }

		[DataMember]
		public XDevice RootDevice { get; set; }

		[DataMember]
		public List<XZone> Zones { get; set; }

		[DataMember]
		public List<XDirection> Directions { get; set; }

		[DataMember]
		public List<XPumpStation> PumpStations { get; set; }

		[DataMember]
		public List<XMPT> MPTs { get; set; }

		[DataMember]
		public List<XDelay> Delays { get; set; }

		[DataMember]
		public List<XJournalFilter> JournalFilters { get; set; }

		[DataMember]
		public List<XInstruction> Instructions { get; set; }

		[DataMember]
		public List<XGuardUser> GuardUsers { get; set; }

		[DataMember]
		public List<XGuardZone> GuardZones { get; set; }

		[DataMember]
		public List<XParameterTemplate> ParameterTemplates { get; set; }

		public void Update()
		{
			Devices = new List<XDevice>();
			if (RootDevice != null)
			{
				RootDevice.Parent = null;
				Devices.Add(RootDevice);
				AddChild(RootDevice);
			}
		}

		void AddChild(XDevice parentDevice)
		{
			foreach (var device in parentDevice.Children)
			{
				device.Parent = parentDevice;
				Devices.Add(device);
				AddChild(device);
			}
		}

		public List<XZone> SortedZones
		{
			get
			{
				return (
				from XZone zone in Zones
				orderby zone.No
				select zone).ToList();
			}
		}

		public void Reorder()
		{
			foreach (var device in Devices)
			{
				device.SynchronizeChildern();
			}
		}

		public override bool ValidateVersion()
		{
			bool result = true;

			if (RootDevice == null)
			{
				var device = new XDevice();
				device.DriverUID = new Guid("938947C5-4624-4A1A-939C-60AEEBF7B65C");
				RootDevice = device;
				result = false;
			}

			Update();

			if (Delays == null)
			{
				Delays = new List<XDelay>();
				result = false;
			}

			if (PumpStations == null)
			{
				PumpStations = new List<XPumpStation>();
				result = false;
			}

			if (MPTs == null)
			{
				MPTs = new List<XMPT>();
				result = false;
			}

			foreach (var delay in Delays)
			{
				result &= ValidateDeviceLogic(delay.DeviceLogic);
			}
			foreach (var mpt in MPTs)
			{
				result &= ValidateDeviceLogic(mpt.StartLogic);
				foreach (var mptDevice in mpt.MPTDevices)
				{
				}
			}
			foreach (var device in Devices)
			{
				device.BaseUID = device.UID;
				result &= ValidateDeviceLogic(device.DeviceLogic);
				result &= ValidateDeviceLogic(device.NSLogic);
			}
			foreach (var zone in Zones)
			{
				zone.BaseUID = zone.UID;
			}
			foreach (var direction in Directions)
			{
				direction.BaseUID = direction.UID;
			}
			foreach (var pumpStation in PumpStations)
			{
				pumpStation.BaseUID = pumpStation.UID;
				result &= ValidateDeviceLogic(pumpStation.StartLogic);
				result &= ValidateDeviceLogic(pumpStation.StopLogic);
				result &= ValidateDeviceLogic(pumpStation.AutomaticOffLogic);
			}
			foreach (var parameterTemplate in ParameterTemplates)
			{
				foreach (var deviceParameterTemplate in parameterTemplate.DeviceParameterTemplates)
				{
					deviceParameterTemplate.XDevice.BaseUID = deviceParameterTemplate.XDevice.UID;
					result &= ValidateDeviceLogic(deviceParameterTemplate.XDevice.DeviceLogic);
					result &= ValidateDeviceLogic(deviceParameterTemplate.XDevice.NSLogic);
				}
			}

			foreach (var journalFilter in JournalFilters)
			{
			}

			if (GuardZones == null)
			{
				GuardZones = new List<XGuardZone>();
				result = false;
			}

			return result;
		}

		bool ValidateDeviceLogic(XDeviceLogic deviceLogic)
		{
			var result = true;

			if (deviceLogic.ClausesGroup == null)
			{
				deviceLogic.ClausesGroup = new XClauseGroup();
				result = false;
			}
			if (deviceLogic.OffClausesGroup == null)
			{
				deviceLogic.OffClausesGroup = new XClauseGroup();
				result = false;
			}
			return result;
		}
	}
}