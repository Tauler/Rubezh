﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using Infrastructure;
using Infrastructure.Common.TreeList;

namespace LibraryModule.ViewModels
{
	public class DeviceViewModel : TreeNodeViewModel<DeviceViewModel>
	{
		public LibraryDevice LibraryDevice { get; private set; }
		public LibraryDevicePresenter Presenter { get; private set; }
		public Driver Driver
		{
			get { return LibraryDevice.Driver; }
		}
		public string Title
		{
			get
			{
				if (Presenter == null)
					return LibraryDevice.PresentationName;
				else
				{
					if (LibraryDevice.Driver.PresenterKeyProperty != null && LibraryDevice.Driver.PresenterKeyProperty.DriverPropertyType == DriverPropertyTypeEnum.EnumType)
					{
						var parameter = LibraryDevice.Driver.PresenterKeyProperty.Parameters.FirstOrDefault(item => item.Value == Presenter.Key);
						if (parameter != null)
							return parameter.Name;
					}
					return Presenter.Key;
				}
			}
		}
		public List<LibraryState> States
		{
			get { return Presenter == null ? LibraryDevice.States : Presenter.States; }
		}

		public DeviceViewModel(LibraryDevice libraryDevice, LibraryDevicePresenter presenter = null)
		{
			Presenter = presenter;
			LibraryDevice = libraryDevice;
			AddChildren();
		}

		private void AddChildren()
		{
			if (Presenter == null && LibraryDevice.Presenters != null)
				foreach (var presenter in LibraryDevice.Presenters)
					AddChild(new DeviceViewModel(LibraryDevice, presenter));
		}

		public string AlternativeName
		{
			get { return LibraryDevice.AlternativeName; }
			set
			{
				LibraryDevice.AlternativeName = value;
				//OnPropertyChanged(() => AlternativeName);
				OnPropertyChanged(() => LibraryDevice);
				ServiceFactory.SaveService.LibraryChanged = true;
			}
		}
	}
}