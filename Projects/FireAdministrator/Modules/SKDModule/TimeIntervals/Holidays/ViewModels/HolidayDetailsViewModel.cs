﻿using System;
using System.Collections.ObjectModel;
using FiresecAPI.SKD;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class HolidayDetailsViewModel : SaveCancelDialogViewModel
	{
		public SKDHoliday Holiday { get; private set; }

		public HolidayDetailsViewModel(SKDHoliday holiday = null)
		{
			if (holiday == null)
			{
				Title = "Новый праздничный день";
				holiday = new SKDHoliday()
				{
					Name = "Праздничный день",
				};
			}
			else
			{
				Title = "Редактирование праздничного дня";
			}

			AvailableTypeNos = new ObservableCollection<int>();
			for (int i = 1; i <= 8; i++)
			{
				AvailableTypeNos.Add(i);
			}

			Holiday = holiday;
			DateTime = holiday.DateTime;
			TypeNo = holiday.TypeNo;
			Name = holiday.Name;
		}

		DateTime _dateTime;
		public DateTime DateTime
		{
			get { return _dateTime; }
			set
			{
				_dateTime = value;
				OnPropertyChanged(() => DateTime);
			}
		}

		int _typeNo;
		public int TypeNo
		{
			get { return _typeNo; }
			set
			{
				_typeNo = value;
				OnPropertyChanged(() => TypeNo);
			}
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

		public ObservableCollection<int> AvailableTypeNos { get; private set; }

		protected override bool Save()
		{
			Holiday.DateTime = DateTime;
			Holiday.TypeNo = TypeNo;
			Holiday.Name = Name;
			return true;
		}
	}
}