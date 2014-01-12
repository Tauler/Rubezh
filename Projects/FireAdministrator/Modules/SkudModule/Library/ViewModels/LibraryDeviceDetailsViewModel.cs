﻿using System.Collections.ObjectModel;
using System.Linq;
using FiresecClient;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;
using FiresecAPI;

namespace SkudModule.ViewModels
{
	public class LibraryDeviceDetailsViewModel : SaveCancelDialogViewModel
	{
		public LibraryDeviceDetailsViewModel()
		{
			Title = "Добавить устройство";

			Devices = new ObservableCollection<LibraryDeviceViewModel>();
			var drivers = (from SKDDriver driver in SKDManager.Drivers select driver).ToList();
			foreach (var driver in drivers)
			{
				if (!SKDManager.SKDLibraryConfiguration.Devices.Any(x => x.DriverId == driver.UID) && driver.IsPlaceable)
				{
					var skdLibraryDevice = new SKDLibraryDevice()
					{
						Driver = driver,
						DriverId = driver.UID
					};
					var deviceViewModel = new LibraryDeviceViewModel(skdLibraryDevice);
					Devices.Add(deviceViewModel);
				}
			}
			SelectedDevice = Devices.FirstOrDefault();
		}

		public ObservableCollection<LibraryDeviceViewModel> Devices { get; private set; }
		public LibraryDeviceViewModel SelectedDevice { get; set; }

		protected override bool CanSave()
		{
			return SelectedDevice != null;
		}
	}
}