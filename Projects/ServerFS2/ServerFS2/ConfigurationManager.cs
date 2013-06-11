﻿using System.IO;
using System.Linq;
using System.Text;
using FiresecAPI.Models;
using Infrastructure.Common;
using Ionic.Zip;
using System.Collections.Generic;

namespace ServerFS2
{
	public static class ConfigurationManager
	{
		public static DeviceConfiguration DeviceConfiguration { get; set; }
		public static DriversConfiguration DriversConfiguration { get; set; }

		public static void Load()
		{
			MetadataHelper.Initialize();

			var serverConfigName = AppDataFolderHelper.GetServerAppDataPath("Config.fscp");
			var folderName = AppDataFolderHelper.GetFolder("Server2");
			var configFileName = Path.Combine(folderName, "Config.fscp");
			if (Directory.Exists(folderName))
				Directory.Delete(folderName, true);
			Directory.CreateDirectory(folderName);
			File.Copy(serverConfigName, configFileName);

			var zipFile = ZipFile.Read(configFileName, new ReadOptions { Encoding = Encoding.GetEncoding("cp866") });
			var fileInfo = new FileInfo(configFileName);
			var unzipFolderPath = Path.Combine(fileInfo.Directory.FullName, "Unzip");
			zipFile.ExtractAll(unzipFolderPath);

			var configurationFileName = Path.Combine(unzipFolderPath, "DeviceConfiguration.xml");
			DeviceConfiguration = ZipSerializeHelper.DeSerialize<DeviceConfiguration>(configurationFileName);

			configurationFileName = Path.Combine(unzipFolderPath, "DriversConfiguration.xml");
			DriversConfiguration = ZipSerializeHelper.DeSerialize<DriversConfiguration>(configurationFileName);
			DriversConfiguration.Drivers.ForEach(x => x.Properties.RemoveAll(z => z.IsAUParameter));
			DriverConfigurationParametersHelper.CreateKnownProperties(DriversConfiguration.Drivers);
			Update();
		}

		public static List<Device> Devices
		{
			get { return DeviceConfiguration.Devices; }
		}

		public static List<Zone> Zones
		{
			get { return DeviceConfiguration.Zones; }
		}

		public static void Update()
		{
			DeviceConfiguration.Update();
			DeviceConfiguration.Reorder();
			foreach (var device in Devices)
			{
				device.Driver = DriversConfiguration.Drivers.FirstOrDefault(x => x.UID == device.DriverUID);
				if (device.Driver == null)
				{
					continue;
				}
			}
			DeviceConfiguration.InvalidateConfiguration();
			DeviceConfiguration.UpdateCrossReferences();
			foreach (var device in Devices)
			{
				device.UpdateHasExternalDevices();
				device.DeviceState = new DeviceState()
				{
					DeviceUID = device.UID,
					Device = device
				};
			}

			foreach (var zone in DeviceConfiguration.Zones)
			{
				zone.ZoneState = new ZoneState()
				{
					Zone = zone,
					ZoneUID = zone.UID
				};
			}
		}
	}
}