﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Common;
using FiresecAPI;
using FiresecAPI.Automation;
using FiresecAPI.Models;
using FiresecAPI.SKD;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Events;
using Ionic.Zip;
using FiresecAPI.Journal;

namespace FireAdministrator
{
	public static class ConfigManager
	{
		public static bool SetNewConfig()
		{
			try
			{
				ServiceFactory.Events.GetEvent<BeforeConfigurationSerializeEvent>().Publish(null);
				ServiceFactory.Events.GetEvent<ConfigurationSavingEvent>().Publish(null);

				var validationResult = ServiceFactory.ValidationService.Validate();
				if (validationResult.HasErrors())
				{
					if (validationResult.CannotSave())
					{
						MessageBoxService.ShowWarning("Обнаружены ошибки. Операция прервана");
						return false;
					}

					if (!MessageBoxService.ShowQuestion("Конфигурация содержит ошибки. Продолжить?"))
						return false;
				}

				WaitHelper.Execute(() =>
				{
					FiresecManager.FiresecService.GKAddMessage(JournalEventNameType.Применение_конфигурации, "");
					LoadingService.Show("Применение конфигурации", "Применение конфигурации", 10);

					var tempFileName = SaveAllConfigToFile();
					using (var fileStream = new FileStream(tempFileName, FileMode.Open))
					{
						FiresecManager.FiresecService.SetConfig(fileStream);
					}
					File.Delete(tempFileName);

					if (ServiceFactory.SaveService.HasChanges)
						FiresecManager.FiresecService.NotifyClientsOnConfigurationChanged();
				});
				LoadingService.Close();
				ServiceFactory.SaveService.Reset();
				return true;
			}
			catch (Exception e)
			{
				Logger.Error(e, "MenuView.SetNewConfig");
				MessageBoxService.ShowError(e.Message, "Ошибка при выполнении операции");
				return false;
			}
		}

		public static string SaveAllConfigToFile(bool saveAnyway = false)
		{
			try
			{
				var tempFolderName = AppDataFolderHelper.GetTempFolder();
				if (!Directory.Exists(tempFolderName))
					Directory.CreateDirectory(tempFolderName);

				var tempFileName = AppDataFolderHelper.GetTempFileName();
				if (File.Exists(tempFileName))
					File.Delete(tempFileName);

				TempZipConfigurationItemsCollection = new ZipConfigurationItemsCollection();

				if (ServiceFactory.SaveService.PlansChanged || saveAnyway)
					AddConfiguration(tempFolderName, "PlansConfiguration.xml", FiresecManager.PlansConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.SoundsChanged || ServiceFactory.SaveService.FilterChanged || ServiceFactory.SaveService.CamerasChanged || ServiceFactory.SaveService.EmailsChanged || ServiceFactory.SaveService.AutomationChanged || saveAnyway)
					AddConfiguration(tempFolderName, "SystemConfiguration.xml", FiresecManager.SystemConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.GKChanged || ServiceFactory.SaveService.GKInstructionsChanged || saveAnyway)
					AddConfiguration(tempFolderName, "GKDeviceConfiguration.xml", GKManager.DeviceConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.SecurityChanged || saveAnyway)
					AddConfiguration(tempFolderName, "SecurityConfiguration.xml", FiresecManager.SecurityConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.GKLibraryChanged || saveAnyway)
					AddConfiguration(tempFolderName, "GKDeviceLibraryConfiguration.xml", GKManager.DeviceLibraryConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.SKDChanged || saveAnyway)
					AddConfiguration(tempFolderName, "SKDConfiguration.xml", SKDManager.SKDConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.SKDLibraryChanged || saveAnyway)
					AddConfiguration(tempFolderName, "SKDLibraryConfiguration.xml", SKDManager.SKDLibraryConfiguration, 1, 1, true);
				if (ServiceFactory.SaveService.LayoutsChanged || saveAnyway)
					AddConfiguration(tempFolderName, "LayoutsConfiguration.xml", FiresecManager.LayoutsConfiguration, 1, 1, false);

				AddConfiguration(tempFolderName, "ZipConfigurationItemsCollection.xml", TempZipConfigurationItemsCollection, 1, 1, true);

				var destinationImagesDirectory = AppDataFolderHelper.GetFolder(Path.Combine(tempFolderName, "Content"));
				if (Directory.Exists(ServiceFactory.ContentService.ContentFolder))
				{
					if (Directory.Exists(destinationImagesDirectory))
						Directory.Delete(destinationImagesDirectory);
					if (!Directory.Exists(destinationImagesDirectory))
						Directory.CreateDirectory(destinationImagesDirectory);
					var sourceImagesDirectoryInfo = new DirectoryInfo(ServiceFactory.ContentService.ContentFolder);
					foreach (var fileInfo in sourceImagesDirectoryInfo.GetFiles())
					{
						fileInfo.CopyTo(Path.Combine(destinationImagesDirectory, fileInfo.Name));
					}
				}

				var zipFile = new ZipFile(tempFileName);
				zipFile.AddDirectory(tempFolderName);
				zipFile.Save(tempFileName);
				zipFile.Dispose();
				if (Directory.Exists(tempFolderName))
					Directory.Delete(tempFolderName, true);

				return tempFileName;
			}
			catch (Exception e)
			{
				Logger.Error(e, "ConfigManager.SaveAllConfigToFile");
			}
			return null;
		}

		static ZipConfigurationItemsCollection TempZipConfigurationItemsCollection = new ZipConfigurationItemsCollection();

		static void AddConfiguration(string folderName, string name, VersionedConfiguration configuration, int minorVersion, int majorVersion, bool useXml)
		{
			configuration.BeforeSave();
			configuration.Version = new ConfigurationVersion() { MinorVersion = minorVersion, MajorVersion = majorVersion };
			ZipSerializeHelper.Serialize(configuration, Path.Combine(folderName, name), useXml);

			TempZipConfigurationItemsCollection.ZipConfigurationItems.Add(new ZipConfigurationItem(name, minorVersion, majorVersion));
		}

		public static void CreateNew()
		{
			try
			{
				if (MessageBoxService.ShowQuestion("Вы уверены, что хотите создать новую конфигурацию"))
				{
					ServiceFactory.Events.GetEvent<ConfigurationClosedEvent>().Publish(null);
					GKManager.SetEmptyConfiguration();
					FiresecManager.PlansConfiguration = new PlansConfiguration();
					FiresecManager.SystemConfiguration.Cameras = new List<Camera>();
					FiresecManager.SystemConfiguration.AutomationConfiguration = new AutomationConfiguration();
					FiresecManager.PlansConfiguration.Update();
					SKDManager.SetEmptyConfiguration();
					FiresecManager.LayoutsConfiguration = new LayoutsConfiguration();

					ServiceFactory.Events.GetEvent<ConfigurationChangedEvent>().Publish(null);

					ServiceFactory.SaveService.PlansChanged = true;
					ServiceFactory.SaveService.CamerasChanged = true;
					ServiceFactory.SaveService.GKChanged = true;
					ServiceFactory.SaveService.GKInstructionsChanged = true;
					ServiceFactory.SaveService.SKDChanged = true;
					ServiceFactory.SaveService.LayoutsChanged = true;

					ShowFirstDevice();
				}
			}
			catch (Exception e)
			{
				Logger.Error(e, "MenuView.CreateNew");
				MessageBox.Show(e.Message, "Ошибка при выполнении операции");
			}
		}

		public static void ShowFirstDevice()
		{
			ServiceFactory.Layout.Close();
			ServiceFactory.Layout.ShowFooter(null);
			if (ApplicationService.Modules.Any(x => x.Name == "Групповой контроллер"))
			{
				var deviceUID = Guid.Empty;
				var firstDevice = GKManager.Devices.FirstOrDefault();
				if (firstDevice != null)
					deviceUID = firstDevice.UID;
				ServiceFactory.Events.GetEvent<ShowGKDeviceEvent>().Publish(deviceUID);
			}
		}
	}
}