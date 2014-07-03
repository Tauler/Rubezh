﻿using FiresecAPI.SKD;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class TimeIntervalDetailsViewModel : SaveCancelDialogViewModel
	{
		public SKDTimeInterval TimeInterval { get; private set; }

		public TimeIntervalDetailsViewModel(SKDTimeInterval timeInterval = null)
		{
			Title = "Редактирование именованного интервала";
			TimeInterval = timeInterval;
			Name = TimeInterval.Name;
			Description = TimeInterval.Description;
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged("Description");
			}
		}

		protected override bool CanSave()
		{
			return !string.IsNullOrEmpty(Name) && Name != "Никогда";
		}

		protected override bool Save()
		{
			TimeInterval.Name = Name;
			TimeInterval.Description = Description;
			return true;
		}
	}
}