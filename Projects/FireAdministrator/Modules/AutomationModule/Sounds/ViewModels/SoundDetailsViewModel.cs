﻿using FiresecAPI;
using FiresecAPI.Models;
using Infrastructure.Common.Windows.ViewModels;


namespace AutomationModule.ViewModels
{
	public class SoundDetailsViewModel: SaveCancelDialogViewModel
	{
		public AutomationSound Sound { get; private set; }

		public SoundDetailsViewModel(AutomationSound sound)
		{
			Title = "Свойства звукового элемента";
			Sound = sound;
			Name = Sound.Name;
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(() => Name);
			}
		}

		protected override bool Save()
		{
			Sound.Name = Name;
			return base.Save();
		}
	}
}