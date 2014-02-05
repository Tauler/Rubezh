﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using FiresecAPI;
using System.Collections.ObjectModel;

namespace SKDModule.ViewModels
{
	public class SlideDayIntervalPartViewModel : BaseViewModel
	{
		SlideDayIntervalViewModel SlideDayIntervalViewModel;
		public SKDTimeInterval TimeInterval { get; private set; }

		public SlideDayIntervalPartViewModel(SlideDayIntervalViewModel slideDayIntervalViewModel, SKDTimeInterval timeInterval)
		{
			SlideDayIntervalViewModel = slideDayIntervalViewModel;
			TimeInterval = timeInterval;

			AvailableTimeIntervals = new ObservableCollection<SKDTimeInterval>();
			foreach (var interval in SKDManager.SKDConfiguration.TimeIntervals)
			{
				AvailableTimeIntervals.Add(interval);
			}
			SelectedTimeInterval = TimeInterval;
		}

		public ObservableCollection<SKDTimeInterval> AvailableTimeIntervals { get; private set; }

		SKDTimeInterval _selectedTimeInterval;
		public SKDTimeInterval SelectedTimeInterval
		{
			get { return _selectedTimeInterval; }
			set
			{
				_selectedTimeInterval = value;
				OnPropertyChanged("SelectedTimeInterval");
				SlideDayIntervalViewModel.Update();
			}
		}
	}
}
