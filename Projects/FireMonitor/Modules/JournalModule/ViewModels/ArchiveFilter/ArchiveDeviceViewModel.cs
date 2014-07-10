﻿using FiresecAPI.GK;
using Infrastructure.Common.CheckBoxList;
using Infrastructure.Common.TreeList;

namespace JournalModule.ViewModels
{
	public class ArchiveDeviceViewModel : TreeNodeViewModel<ArchiveDeviceViewModel>, ICheckBoxItem
	{
		public ArchiveDeviceViewModel(XDevice device)
		{
			Device = device;
			Name = device.PresentationName;
		}

		public XDevice Device { get; private set; }
		public string Name { get; private set; }

		bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				OnPropertyChanged(() => IsChecked);
				if (ItemsList != null)
					ItemsList.Update();
			}
		}

		public ICheckBoxItemList ItemsList { get; set; }

		public bool CanCheck
		{
			get { return Device.IsRealDevice; }
		}
	}
}