﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecService.Configuration;
using FiresecService.Processor;

namespace FiresecService.Service
{
	public partial class FiresecService
	{
		void NotifyConfigurationChanged()
		{
			ClientsCash.OnConfigurationChanged();
		}

		public OperationResult<bool> SetDeviceConfiguration(DeviceConfiguration deviceConfiguration)
		{
			ConfigurationFileManager.SetDeviceConfiguration(deviceConfiguration);
			ConfigurationCash.DeviceConfiguration = deviceConfiguration;

			FiresecManager.ConvertBack(deviceConfiguration, true);

			OperationResult<bool> result = null;
			if (AppSettings.OverrideFiresec1Config)
			{
				result = FiresecSerializedClient.SetNewConfig(FiresecManager.ConfigurationConverter.FiresecConfiguration).ToOperationResult();
			}

			var thread = new Thread(new ThreadStart(NotifyConfigurationChanged));
			thread.Start();

			return result;
		}

		public OperationResult<bool> DeviceWriteConfiguration(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceWriteConfig(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		[Obsolete]
		public OperationResult<bool> DeviceWriteAllConfiguration(DeviceConfiguration deviceConfiguration)
		{
			OperationResult<bool> result = null;
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			foreach (var device in deviceConfiguration.Devices)
			{
				if (device.Driver.CanWriteDatabase)
				{
					result = FiresecSerializedClient.DeviceWriteConfig(firesecConfiguration, device.PlaceInTree).ToOperationResult();
					if (result.HasError)
					{
						return result;
					}
				}
			}

			return result;
		}

		public OperationResult<bool> DeviceSetPassword(DeviceConfiguration deviceConfiguration, Guid deviceUID, DevicePasswordType devicePasswordType, string password)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceSetPassword(firesecConfiguration, device.PlaceInTree, password, (int)devicePasswordType).ToOperationResult();
		}

		public OperationResult<bool> DeviceDatetimeSync(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceDatetimeSync(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		public OperationResult<string> DeviceGetInformation(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceGetInformation(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		public OperationResult<List<string>> DeviceGetSerialList(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			var result = FiresecSerializedClient.DeviceGetSerialList(firesecConfiguration, device.PlaceInTree).ToOperationResult();

			var operationResult = new OperationResult<List<string>>()
			{
				HasError = result.HasError,
				Error = result.Error
			};
			if (result.Result != null)
				operationResult.Result = result.Result.Split(';').ToList();
			return operationResult;
		}

		public OperationResult<string> DeviceUpdateFirmware(DeviceConfiguration deviceConfiguration, Guid deviceUID, byte[] bytes, string fileName)
		{
			fileName = Guid.NewGuid().ToString();
			Directory.CreateDirectory("Temp");
			fileName = Directory.GetCurrentDirectory() + "\\Temp\\" + fileName;
			using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				stream.Write(bytes, 0, bytes.Length);
			}

			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			//fileName = "D:/Projects/3rdParty/Firesec/XHC/Рубеж-2АМ/2_25/sborka2_25(161211)_1.HXC";
			return FiresecSerializedClient.DeviceUpdateFirmware(firesecConfiguration, device.PlaceInTree, fileName).ToOperationResult();
		}

		public OperationResult<string> DeviceVerifyFirmwareVersion(DeviceConfiguration deviceConfiguration, Guid deviceUID, byte[] bytes, string fileName)
		{
			fileName = Guid.NewGuid().ToString();
			Directory.CreateDirectory("Temp");
			fileName = Directory.GetCurrentDirectory() + "\\Temp\\" + fileName;
			using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				stream.Write(bytes, 0, bytes.Length);
			}

			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			//`fileName = "D:/Projects/3rdParty/Firesec/XHC/Рубеж-2АМ/2_25/sborka2_25(161211)_2.HXC";
			return FiresecSerializedClient.DeviceVerifyFirmwareVersion(firesecConfiguration, device.PlaceInTree, fileName).ToOperationResult();
		}

		public OperationResult<string> DeviceReadEventLog(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceReadEventLog(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		public OperationResult<DeviceConfiguration> DeviceAutoDetectChildren(DeviceConfiguration deviceConfiguration, Guid deviceUID, bool fastSearch)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			var result = FiresecSerializedClient.DeviceAutoDetectChildren(firesecConfiguration, device.PlaceInTree, fastSearch);

			var operationResult = new OperationResult<DeviceConfiguration>()
			{
				HasError = result.HasError,
				Error = result.ErrorString
			};

			var configurationManager = new ConfigurationConverter();
			operationResult.Result = configurationManager.ConvertOnlyDevices(result.Result);
			return operationResult;
		}

		public OperationResult<DeviceConfiguration> DeviceReadConfiguration(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			var result = FiresecSerializedClient.DeviceReadConfig(firesecConfiguration, device.PlaceInTree);

			var operationResult = new OperationResult<DeviceConfiguration>()
			{
				HasError = result.HasError,
				Error = result.ErrorString
			};

			var configurationManager = new ConfigurationConverter();
			operationResult.Result = configurationManager.ConvertOnlyDevices(result.Result);
			return operationResult;
		}

		public OperationResult<List<DeviceCustomFunction>> DeviceCustomFunctionList(Guid driverUID)
		{
			var result = FiresecSerializedClient.DeviceCustomFunctionList(driverUID.ToString().ToUpper());

			var operationResult = new OperationResult<List<DeviceCustomFunction>>()
			{
				HasError = result.HasError,
				Error = result.ErrorString
			};

			operationResult.Result = DeviceCustomFunctionConverter.Convert(result.Result);

			return operationResult;
		}

		public OperationResult<string> DeviceCustomFunctionExecute(DeviceConfiguration deviceConfiguration, Guid deviceUID, string functionName)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceCustomFunctionExecute(firesecConfiguration, device.PlaceInTree, functionName).ToOperationResult();
		}

		public OperationResult<string> DeviceGetGuardUsersList(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceGetGuardUsersList(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		public OperationResult<bool> DeviceSetGuardUsersList(DeviceConfiguration deviceConfiguration, Guid deviceUID, string users)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceSetGuardUsersList(firesecConfiguration, device.PlaceInTree, users).ToOperationResult();
		}

		public OperationResult<string> DeviceGetMDS5Data(DeviceConfiguration deviceConfiguration, Guid deviceUID)
		{
			var firesecConfiguration = FiresecManager.ConvertBack(deviceConfiguration, false);
			var device = deviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.DeviceGetMDS5Data(firesecConfiguration, device.PlaceInTree).ToOperationResult();
		}

		public void AddToIgnoreList(List<Guid> deviceGuids)
		{
			var devicePaths = new List<string>();
			foreach (var guid in deviceGuids)
			{
				var device = ConfigurationCash.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == guid);
				devicePaths.Add(device.PlaceInTree);
			}

			FiresecSerializedClient.AddToIgnoreList(devicePaths);
		}

		public void RemoveFromIgnoreList(List<Guid> deviceGuids)
		{
			var devicePaths = new List<string>();
			foreach (var guid in deviceGuids)
			{
				var device = ConfigurationCash.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == guid);
				devicePaths.Add(device.PlaceInTree);
			}

			FiresecSerializedClient.RemoveFromIgnoreList(devicePaths);
		}

		public void ResetStates(List<ResetItem> resetItems)
		{
			var firesecResetHelper = new FiresecResetHelper(FiresecManager);
			firesecResetHelper.ResetStates(resetItems);
		}

		public void AddUserMessage(string message)
		{
			FiresecSerializedClient.AddUserMessage(message);
		}

		public OperationResult<bool> ExecuteCommand(Guid deviceUID, string methodName)
		{
			var device = FiresecManager.DeviceConfigurationStates.DeviceStates.FirstOrDefault(x => x.UID == deviceUID);
			if (device != null)
			{
				FiresecSerializedClient.ExecuteCommand(device.PlaceInTree, methodName).ToOperationResult();
			}
			var operationResult = new OperationResult<bool>()
			{
				Result = false,
				HasError = true,
				Error = "Не найдено устройство по идентификатору"
			};
			return operationResult;
		}

		public OperationResult<bool> CheckHaspPresence()
		{
			return FiresecSerializedClient.CheckHaspPresence().ToOperationResult();
		}

		public OperationResult<string> GetConfigurationParameters(Guid deviceUID, int paramNo)
		{
			var device = ConfigurationCash.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
			return FiresecSerializedClient.GetConfigurationParameters(device.PlaceInTree, paramNo).ToOperationResult();
		}

		public void SetConfigurationParameters(Guid deviceUID)
		{
		}
	}
}