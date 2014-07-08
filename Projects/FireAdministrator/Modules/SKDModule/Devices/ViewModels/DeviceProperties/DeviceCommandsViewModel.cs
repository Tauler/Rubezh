﻿using System.ComponentModel;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecAPI.SKD;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Events;
using Microsoft.Win32;
using System.Threading;
using System;
using Infrastructure.Common.Services;

namespace SKDModule.ViewModels
{
	public class DeviceCommandsViewModel : BaseViewModel
	{
		DevicesViewModel DevicesViewModel;

		public DeviceCommandsViewModel(DevicesViewModel devicesViewModel)
		{
			DevicesViewModel = devicesViewModel;
			ShowControllerConfigurationCommand = new RelayCommand(OnShowControllerConfiguration, CanShowControllerConfiguration);
			ShowLockConfigurationCommand = new RelayCommand(OnShowLockConfiguration, CanShowLockConfiguration);
			WriteTimeSheduleConfigurationCommand = new RelayCommand(OnWriteTimeSheduleConfiguration, CanWriteTimeSheduleConfiguration);
			WriteAllTimeSheduleConfigurationCommand = new RelayCommand(OnWriteAllTimeSheduleConfiguration);
		}

		public DeviceViewModel SelectedDevice
		{
			get { return DevicesViewModel.SelectedDevice; }
		}

		public RelayCommand ShowControllerConfigurationCommand { get; private set; }
		void OnShowControllerConfiguration()
		{
			var controllerPropertiesViewModel = new ControllerPropertiesViewModel(SelectedDevice.Device);
			DialogService.ShowModalWindow(controllerPropertiesViewModel);
		}
		bool CanShowControllerConfiguration()
		{
			return SelectedDevice != null && SelectedDevice.Device.Driver.IsController;
		}

		public RelayCommand ShowLockConfigurationCommand { get; private set; }
		void OnShowLockConfiguration()
		{
			var lockPropertiesViewModel = new LockPropertiesViewModel(SelectedDevice.Device);
			if (DialogService.ShowModalWindow(lockPropertiesViewModel))
			{
				ServiceFactory.SaveService.SKDChanged = true;
			}
		}
		bool CanShowLockConfiguration()
		{
			return SelectedDevice != null && SelectedDevice.Device.Driver.DriverType == SKDDriverType.Lock;
		}

		public RelayCommand WriteTimeSheduleConfigurationCommand { get; private set; }
		void OnWriteTimeSheduleConfiguration()
		{
			if (CheckNeedSave(true))
			{
				//if (ValidateConfiguration())
				{
					WriteTimeSheduleWithProgress();
					//WriteTimeSheduleWithProgress();

					//var result = FiresecManager.FiresecService.SKDWriteTimeSheduleConfiguration(SelectedDevice.Device);
					//if (result.HasError)
					//{
					//    MessageBoxService.ShowError(result.Error);
					//}
				}
			}
		}
		bool CanWriteTimeSheduleConfiguration()
		{
			return SelectedDevice != null && SelectedDevice.Device.Driver.IsController;
		}


		void WriteTimeSheduleWithProgress()
		{
			var thread = new Thread(() =>
			{
				var result = FiresecManager.FiresecService.SKDWriteTimeSheduleConfiguration(SelectedDevice.Device);

				ApplicationService.Invoke(new Action(() =>
				{
					if (result.HasError)
					{
						LoadingService.Close();
						MessageBoxService.ShowError(result.Error);
					}
				}));
			});
			thread.Name = "DeviceCommandsViewModel OnWriteTimeSheduleConfiguration";
			thread.Start();
		}



		public RelayCommand WriteAllTimeSheduleConfigurationCommand { get; private set; }
		void OnWriteAllTimeSheduleConfiguration()
		{
			if (CheckNeedSave(true))
			{
				if (ValidateConfiguration())
				{
					var result = FiresecManager.FiresecService.SKDWriteAllTimeSheduleConfiguration();
					if (result.HasError)
					{
						MessageBoxService.ShowError(result.Error);
					}
				}
			}
		}

		bool ValidateConfiguration()
		{
			var validationResult = ServiceFactory.ValidationService.Validate();
			if (validationResult.HasErrors("SKD"))
			{
				if (validationResult.CannotSave("SKD") || validationResult.CannotWrite("SKD"))
				{
					MessageBoxService.ShowWarning("Обнаружены ошибки. Операция прервана");
					return false;
				}
			}
			return true;
		}

		bool CheckNeedSave(bool syncParameters = false)
		{
			if (ServiceFactory.SaveService.SKDChanged)
			{
				if (MessageBoxService.ShowQuestion("Для выполнения этой операции необходимо применить конфигурацию. Применить сейчас?") == System.Windows.MessageBoxResult.Yes)
				{
					var cancelEventArgs = new CancelEventArgs();
					ServiceFactory.Events.GetEvent<SetNewConfigurationEvent>().Publish(cancelEventArgs);
					return !cancelEventArgs.Cancel;
				}
				return false;
			}
			return true;
		}
	}
}