﻿using System.Linq;
using FiresecAPI;
using XFiresecAPI;

namespace SKDModule.ViewModels
{
	public class BoolPropertyViewModel : BasePropertyViewModel
	{
		public BoolPropertyViewModel(XDriverProperty driverProperty, SKDDevice device)
			: base(driverProperty, device)
		{
			var property = device.Properties.FirstOrDefault(x => x.Name == driverProperty.Name);
			if (property != null)
				_isChecked = property.Value > 0;
			else
				_isChecked = (driverProperty.Default == (ushort)1) ? true : false;
		}

		bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				OnPropertyChanged("IsChecked");
				Save(value ? (ushort)1 : (ushort)0);
			}
		}
	}
}