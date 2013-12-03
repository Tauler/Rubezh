﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FiresecClient;
using GKProcessor;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using XFiresecAPI;

namespace GKModule.ViewModels
{
	public partial class DeviceViewModel
	{
		void InitializeParamsCommands()
		{
			ReadCommand = new RelayCommand(OnRead, CanReadWrite);
			WriteCommand = new RelayCommand(OnWrite, CanSync);
			ReadAllCommand = new RelayCommand(OnReadAll, CanReadWriteAll);
			WriteAllCommand = new RelayCommand(OnWriteAll, CanSyncAll);
			SyncFromDeviceToSystemCommand = new RelayCommand(OnSyncFromDeviceToSystem, CanSync);
			SyncFromAllDeviceToSystemCommand = new RelayCommand(OnSyncFromAllDeviceToSystem, CanSyncAll);

			CopyParamCommand = new RelayCommand(OnCopy, CanCopy);
			PasteParamCommand = new RelayCommand(OnPaste, CanPaste);
			PasteAllParamCommand = new RelayCommand(OnPasteAll, CanPasteAll);
			PasteTemplateCommand = new RelayCommand(OnPasteTemplate, CanPasteTemplate);
			PasteAllTemplateCommand = new RelayCommand(OnPasteAllTemplate, CanPasteAllTemplate);
			ResetAUPropertiesCommand = new RelayCommand(OnResetAUProperties);
		}

		public List<XDevice> GetRealChildren()
		{
			var devices = XManager.GetAllDeviceChildren(Device).Where(device => device.Driver.Properties.Any(x => x.IsAUParameter)).ToList();
			return devices;
		}

		#region Capy and Paste
		public static XDriver DriverCopy;
		public static List<XProperty> PropertiesCopy;

		public RelayCommand CopyParamCommand { get; private set; }
		void OnCopy()
		{
			DriverCopy = Device.Driver;
			PropertiesCopy = new List<XProperty>();
			foreach (var property in Device.Properties)
			{
				var driverProperty = Device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
				if (driverProperty != null && driverProperty.IsAUParameter)
				{
					var propertyCopy = new XProperty()
					{
						StringValue = property.StringValue,
						Name = property.Name,
						Value = property.Value,
						DriverProperty = driverProperty
					};
					PropertiesCopy.Add(propertyCopy);
				}
			}
		}
		bool CanCopy()
		{
			return HasAUProperties;
		}

		public RelayCommand PasteParamCommand { get; private set; }
		void OnPaste()
		{
			CopyParametersFromBuffer(Device);
			PropertiesViewModel.Update();
			UpdateDeviceParameterMissmatch();
		}
		bool CanPaste()
		{
			return DriverCopy != null && Device.DriverType == DriverCopy.DriverType;
		}

		public RelayCommand PasteAllParamCommand { get; private set; }
		void OnPasteAll()
		{
			foreach (var device in XManager.GetAllDeviceChildren(Device))
			{
				if (device.DriverType != DriverCopy.DriverType)
					continue;
				CopyParametersFromBuffer(device);
				var deviceViewModel = DevicesViewModel.Current.AllDevices.FirstOrDefault(x => x.Device == device);
				if (deviceViewModel != null)
					deviceViewModel.PropertiesViewModel.Update();
			}
			UpdateDeviceParameterMissmatch();
		}
		bool CanPasteAll()
		{
			return (Children.Count() > 0 && DriverCopy != null) && (XManager.GetAllDeviceChildren(Device).Any(x => x.DriverType == DriverCopy.DriverType));
		}

		static void CopyParametersFromBuffer(XDevice device)
		{
			foreach (var property in PropertiesCopy)
			{
				var deviceProperty = device.Properties.FirstOrDefault(x => x.Name == property.Name);
				if (deviceProperty != null)
				{
					deviceProperty.Value = property.Value;
				}
			}
			ServiceFactory.SaveService.GKChanged = true;
		}
		#endregion

		#region Template
		public RelayCommand PasteTemplateCommand { get; private set; }
		void OnPasteTemplate()
		{
			var parameterTemplateSelectationViewModel = new ParameterTemplateSelectationViewModel();
			if (DialogService.ShowModalWindow(parameterTemplateSelectationViewModel))
			{
				CopyParametersFromTemplate(parameterTemplateSelectationViewModel.SelectedParameterTemplate, Device);
				PropertiesViewModel.Update();
			}
			UpdateDeviceParameterMissmatch();
		}
		bool CanPasteTemplate()
		{
			return HasAUProperties;
		}

		public RelayCommand PasteAllTemplateCommand { get; private set; }
		void OnPasteAllTemplate()
		{
			var parameterTemplateSelectationViewModel = new ParameterTemplateSelectationViewModel();
			if (DialogService.ShowModalWindow(parameterTemplateSelectationViewModel))
			{
				var devices = XManager.GetAllDeviceChildren(Device);
				devices.Add(Device);
				foreach (var device in devices)
				{
					CopyParametersFromTemplate(parameterTemplateSelectationViewModel.SelectedParameterTemplate, device);
					var deviceViewModel = DevicesViewModel.Current.AllDevices.FirstOrDefault(x => x.Device == device);
					if (deviceViewModel != null)
						deviceViewModel.PropertiesViewModel.Update();
				}
			}
			UpdateDeviceParameterMissmatch();
		}
		bool CanPasteAllTemplate()
		{
			return Children.Count() > 0;
		}

		static void CopyParametersFromTemplate(XParameterTemplate parameterTemplate, XDevice device)
		{
			var deviceParameterTemplate = parameterTemplate.DeviceParameterTemplates.FirstOrDefault(x => x.XDevice.DriverUID == device.Driver.UID);
			if (deviceParameterTemplate != null)
			{
				foreach (var property in deviceParameterTemplate.XDevice.Properties)
				{
					var deviceProperty = device.Properties.FirstOrDefault(x => x.Name == property.Name);
					if (deviceProperty != null)
					{
						deviceProperty.Value = property.Value;
					}
				}
			}
		}
		#endregion

		public RelayCommand WriteCommand { get; private set; }
		void OnWrite()
		{
			var device = new List<XDevice> { Device };
			if (Validate(device))
			{
				LoadingService.Show("Запись параметров в устройствo " + Device.PresentationName, null, 1, true);
				WriteDevices(device);
				SyncFromSystemToDeviceProperties(device);
			}
		}

		public RelayCommand WriteAllCommand { get; private set; }
		void OnWriteAll()
		{
			var devices = GetRealChildren();
			if (Validate(devices))
			{
				LoadingService.Show("Запись параметров в дочерние устройства " + Device.PresentationName, null, 1, true);
				WriteDevices(devices);
				SyncFromSystemToDeviceProperties(devices);
			}
		}

		public void SyncFromSystemToDeviceProperties(List<XDevice> devices)
		{
			foreach (var device in devices)
			{
				foreach (var property in device.Properties)
				{
					var deviceProperty = device.DeviceProperties.FirstOrDefault(x => x.Name == property.Name);
					if (deviceProperty == null)
					{
						device.DeviceProperties.Add(new XProperty
							{
								DriverProperty = property.DriverProperty,
								Name = property.Name,
								Value = property.Value
							});
						ServiceFactory.SaveService.GKChanged = true;
					}
					if (deviceProperty != null && deviceProperty.Value != property.Value)
					{
						deviceProperty.Value = property.Value;
						ServiceFactory.SaveService.GKChanged = true;
					}
				}
				var deviceViewModel = DevicesViewModel.Current.AllDevices.FirstOrDefault(x => x.Device == device);
				if (deviceViewModel != null)
					deviceViewModel.PropertiesViewModel.Update();
			}
			UpdateDeviceParameterMissmatch();
		}

		public RelayCommand SyncFromDeviceToSystemCommand { get; private set; }
		void OnSyncFromDeviceToSystem()
		{
			CopyFromDeviceToSystem(Device);
			PropertiesViewModel.Update();
			UpdateDeviceParameterMissmatch();
		}

		public RelayCommand SyncFromAllDeviceToSystemCommand { get; private set; }
		void OnSyncFromAllDeviceToSystem()
		{
			var devices = GetRealChildren();
			devices.Add(Device);
			foreach (var device in devices)
			{
				CopyFromDeviceToSystem(device);
				var deviceViewModel = DevicesViewModel.Current.AllDevices.FirstOrDefault(x => x.Device == device);
				if (deviceViewModel != null)
					deviceViewModel.PropertiesViewModel.Update();
			}
			UpdateDeviceParameterMissmatch();
		}

		bool CanSync()
		{
			return HasAUProperties;
		}

		bool CanSyncAll()
		{
			return Children.Any();
		}

		static void CopyFromDeviceToSystem(XDevice device)
		{
			foreach (var deviceProperty in device.DeviceProperties)
			{
				var property = device.Properties.FirstOrDefault(x => x.Name == deviceProperty.Name);
				property.Value = deviceProperty.Value;
			}
			ServiceFactory.SaveService.GKChanged = true;
		}

		bool Validate(IEnumerable<XDevice> devices)
		{
			foreach (var device in devices)
			{
				foreach (var property in device.Properties)
				{
					var driverProperty = device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
					if (IsPropertyValid(property, driverProperty))
					{
						MessageBoxService.Show("Устройство " + device.PresentationDriverAndAddress + "\nЗначение параметра\n" + driverProperty.Caption + "\nдолжно быть целым числом " + "в диапазоне от " + driverProperty.Min.ToString() + " до " + driverProperty.Max.ToString(), "Firesec");
						return false;
					}
				}
			}
			return true;
		}

		static bool IsPropertyValid(XProperty property, XDriverProperty driverProperty)
		{
			int value;
			return
					driverProperty != null &&
					driverProperty.IsAUParameter &&
					driverProperty.DriverPropertyType == XDriverPropertyTypeEnum.IntType &&
					(!int.TryParse(Convert.ToString(property.Value), out value) ||
					(value < driverProperty.Min || value > driverProperty.Max));
		}

		public RelayCommand ReadCommand { get; private set; }
		void OnRead()
		{
			LoadingService.Show("Чтение параметров из устройства " + Device.PresentationName, null, 1, true);
			ReadDevices(new List<XDevice> { Device });
			PropertiesViewModel.Update();
		}

		bool CanReadWrite()
		{
			return HasAUProperties;
		}

		public RelayCommand ReadAllCommand { get; private set; }
		void OnReadAll()
		{
			var devices = GetRealChildren();
			devices.Add(Device);
			LoadingService.Show("Чтение параметров из дочерних устройств " + Device.PresentationName, null, 1, true);
			ReadDevices(devices);
		}

		bool CanReadWriteAll()
		{
			return Children.Count() > 0;
		}

		public bool HasAUProperties
		{
			get { return Device.Driver.Properties.Count(x => x.IsAUParameter) > 0; }
		}

		static void ReadDevices(IEnumerable<XDevice> devices)
		{
			var errorLog = "";
			DescriptorsManager.Create();
			var i = 0;
			LoadingService.AddCount(devices.Count());
			foreach (var device in devices)
			{
				if (LoadingService.IsCanceled)
					break;
				i++;
				var result = FiresecManager.FiresecService.GKGetSingleParameter(device);
				if(result.HasError)
					errorLog += "\n" + device.PresentationName;
			}
			LoadingService.Close();
			if (errorLog != "")
				MessageBoxService.ShowError("Ошибка при получении параметров следующих устройств:" + errorLog);
		}

		static void WriteDevices(IEnumerable<XDevice> devices)
		{
			var errorLog = "";
			DescriptorsManager.Create();
			var i = 0;
			LoadingService.AddCount(devices.Count());
			foreach (var device in devices)
			{
				if (LoadingService.IsCanceled)
					break;
				i++;
				var result = FiresecManager.FiresecService.GKSetSingleParameter(device);
				if (result.HasError)
					errorLog += "\n" + device.PresentationName;
				if (devices.Count() > 1 && i < devices.Count())
					Thread.Sleep(100);
			}
			LoadingService.Close();
			if (errorLog != "")
				MessageBoxService.ShowError("Ошибка при записи параметров в следующие устройства:" + errorLog);
		}

		public RelayCommand ResetAUPropertiesCommand { get; private set; }
		void OnResetAUProperties()
		{
			foreach (var property in Device.Properties)
			{
				var driverProperty = Device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
				if (driverProperty != null)
				{
					property.Value = driverProperty.Default;
				}
			}
			PropertiesViewModel = new PropertiesViewModel(Device);
			OnPropertyChanged("PropertiesViewModel");
		}

		void UpdateDeviceParameterMissmatch()
		{
			var allDevices = DevicesViewModel.Current.AllDevices;
			allDevices.ForEach(x => x.PropertiesViewModel.DeviceParameterMissmatchType = DeviceParameterMissmatchType.Equal);
			foreach (var deviceViewModel in allDevices)
			{
				deviceViewModel.PropertiesViewModel.UpdateDeviceParameterMissmatchType();
				if (deviceViewModel.PropertiesViewModel.DeviceParameterMissmatchType == DeviceParameterMissmatchType.Unknown)
				{
					deviceViewModel.GetAllParents().ForEach(x => x.PropertiesViewModel.DeviceParameterMissmatchType = DeviceParameterMissmatchType.Unknown);
				}
			}
			foreach (var deviceViewModel in allDevices)
			{
				deviceViewModel.PropertiesViewModel.UpdateDeviceParameterMissmatchType();
				if (deviceViewModel.PropertiesViewModel.DeviceParameterMissmatchType == DeviceParameterMissmatchType.Unequal)
				{
					deviceViewModel.GetAllParents().ForEach(x => x.PropertiesViewModel.DeviceParameterMissmatchType = DeviceParameterMissmatchType.Unequal);
				}
			}
		}
	}
}