﻿using System;
using System.Linq;
using System.Collections.Generic;
using FiresecAPI.GK;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Common.Windows;
using FiresecClient;
using Common;
using System.Collections.ObjectModel;

namespace GKModule.ViewModels
{
	public class SchedulePartsViewModel : BaseViewModel
	{
		public GKSchedule Schedule { get; set; }

		public SchedulePartsViewModel(GKSchedule schedule)
		{
			AddCommand = new RelayCommand(OnAdd, CanAdd);
			DeleteCommand = new RelayCommand(OnDelete, CanDelete);
			Schedule = schedule;
			Update();
		}

		public void Update(GKSchedule schedule)
		{
			Schedule = schedule;
			OnPropertyChanged(() => Schedule);
			Update();
		}
		public void Update()
		{
			Parts = new SortableObservableCollection<SchedulePartViewModel>();
			for (int i = 0; i < Schedule.DayScheduleUIDs.Count; i++)
			{
				var dayScheduleUID = Schedule.DayScheduleUIDs[i];
				var daySchedule = GKManager.DeviceConfiguration.DaySchedules.FirstOrDefault(x => x.UID == dayScheduleUID);
				var schedulePartViewModel = new SchedulePartViewModel(Schedule, dayScheduleUID, i);
				Parts.Add(schedulePartViewModel);
			}
			SelectedPart = Parts.FirstOrDefault();
		}

		public RelayCommand WriteCommand { get; private set; }
		void OnWrite()
		{
			var result = FiresecManager.FiresecService.GKSetSchedule(Schedule);
			if (result.HasError)
			{
				MessageBoxService.ShowError(result.Error);
			}
		}

		ObservableCollection<SchedulePartViewModel> _parts;
		public ObservableCollection<SchedulePartViewModel> Parts
		{
			get { return _parts; }
			set
			{
				_parts = value;
				OnPropertyChanged(() => Parts);
			}
		}

		SchedulePartViewModel _selectedPart;
		public SchedulePartViewModel SelectedPart
		{
			get { return _selectedPart; }
			set
			{
				_selectedPart = value;
				OnPropertyChanged(() => SelectedPart);
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var daysCount = 1;
			if (Schedule.SchedulePeriodType == GKSchedulePeriodType.Weekly)
			{
				daysCount = 7;
			}
			for (int i = 0; i < daysCount; i++)
			{
				var daySchedule = GKManager.DeviceConfiguration.DaySchedules.FirstOrDefault();
				if (daySchedule != null)
				{
					Schedule.DayScheduleUIDs.Add(daySchedule.UID);
					var schedulePartViewModel = new SchedulePartViewModel(Schedule, Guid.Empty, Schedule.DayScheduleUIDs.Count - 1);
					Parts.Add(schedulePartViewModel);
				}
			}
			SelectedPart = Parts.LastOrDefault();
			ServiceFactory.SaveService.GKChanged = true;
		}
		bool CanAdd()
		{
			return Parts.Count < 50;
		}

		public RelayCommand DeleteCommand { get; private set; }
		void OnDelete()
		{
			if (Schedule.SchedulePeriodType == GKSchedulePeriodType.Weekly)
			{
				var weekNo = SelectedPart.Index / 7;
				for (int i = 6; i >= 0; i--)
				{
					var index = weekNo * 7 + i;
					Schedule.DayScheduleUIDs.RemoveAt(index);
					Parts.RemoveAt(index);
				}
			}
			else
			{
				Schedule.DayScheduleUIDs.Remove(SelectedPart.SelectedDaySchedule.UID);
				Parts.Remove(SelectedPart);
			}
			Update();
			ServiceFactory.SaveService.GKChanged = true;
		}
		bool CanDelete()
		{
			if (SelectedPart == null)
				return false;
			if (Schedule.SchedulePeriodType == GKSchedulePeriodType.Dayly)
				return Parts.Count > 1;
			else
				return Parts.Count > 7;
		}
	}
}