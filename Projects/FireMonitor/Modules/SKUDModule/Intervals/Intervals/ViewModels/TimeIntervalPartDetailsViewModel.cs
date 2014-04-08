﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.EmployeeTimeIntervals;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class TimeIntervalPartDetailsViewModel : SaveCancelDialogViewModel
	{
		bool IsNew;
		NamedInterval EmployeeTimeInterval;
		public TimeInterval TimeIntervalPart { get; private set; }

		public TimeIntervalPartDetailsViewModel(NamedInterval employeeTimeInterval, TimeInterval timeIntervalPart = null)
		{
			EmployeeTimeInterval = employeeTimeInterval;
			if (timeIntervalPart == null)
			{
				Title = "Новый интервал";
				IsNew = true;
				timeIntervalPart = new TimeInterval();
			}
			else
			{
				Title = "Редактирование интервала";
				IsNew = false;
			}
			TimeIntervalPart = timeIntervalPart;

			StartTime = timeIntervalPart.StartTime;
			EndTime = timeIntervalPart.EndTime;
			AvailableTransitions = new ObservableCollection<IntervalTransitionType>(Enum.GetValues(typeof(IntervalTransitionType)).OfType<IntervalTransitionType>());
			SelectedTransition = timeIntervalPart.IntervalTransitionType;
		}

		DateTime _startTime;
		public DateTime StartTime
		{
			get { return _startTime; }
			set
			{
				_startTime = value;
				OnPropertyChanged("StartTime");
			}
		}

		DateTime _endTime;
		public DateTime EndTime
		{
			get { return _endTime; }
			set
			{
				_endTime = value;
				OnPropertyChanged("EndTime");
			}
		}

		ObservableCollection<IntervalTransitionType> _availableTransitions;
		public ObservableCollection<IntervalTransitionType> AvailableTransitions
		{
			get { return _availableTransitions; }
			set
			{
				_availableTransitions = value;
				OnPropertyChanged("AvailableTransitions");
			}
		}

		IntervalTransitionType _selectedTransition;
		public IntervalTransitionType SelectedTransition
		{
			get { return _selectedTransition; }
			set
			{
				_selectedTransition = value;
				OnPropertyChanged("SelectedTransition");
			}
		}

		protected override bool Save()
		{
			if (!Validate())
				return false;
			TimeIntervalPart.StartTime = StartTime;
			TimeIntervalPart.EndTime = EndTime;
			TimeIntervalPart.IntervalTransitionType = SelectedTransition;
			return true;
		}

		bool Validate()
		{
			var timeIntervalParts = CloneEmployeeTimeIntervalPart();

			if (timeIntervalParts[0].IntervalTransitionType == IntervalTransitionType.NextDay)
			{
				MessageBoxService.ShowWarning("Последовательность интервалов не может начинаться со следующего дня");
				return false;
			}

			var currentDateTime = DateTime.MinValue;
			foreach (var timeIntervalPart in timeIntervalParts)
			{
				var startTime = timeIntervalPart.StartTime;
				if (timeIntervalPart.IntervalTransitionType == IntervalTransitionType.NextDay)
					startTime = startTime.AddDays(1);
				var endTime = timeIntervalPart.EndTime;
				if (timeIntervalPart.IntervalTransitionType != IntervalTransitionType.Day)
					endTime = endTime.AddDays(1);

				if (startTime < currentDateTime)
				{
					MessageBoxService.ShowWarning("Последовательность интервалов не должна быть пересекающейся");
					return false;
				}
				if (startTime == currentDateTime)
				{
					MessageBoxService.ShowWarning("Пауза между интервалами не должна быть нулевой");
					return false;
				}
				currentDateTime = startTime;

				if (endTime < currentDateTime)
				{
					MessageBoxService.ShowWarning("Последовательность интервалов не должна быть пересекающейся");
					return false;
				}
				if (endTime == currentDateTime)
				{
					MessageBoxService.ShowWarning("Интервал не может иметь нулевую длительность");
					return false;
				}
				currentDateTime = endTime;
			}
			return true;
		}

		List<TimeInterval> CloneEmployeeTimeIntervalPart()
		{
			var timeIntervalParts = new List<TimeInterval>();
			foreach (var timeIntervalPart in EmployeeTimeInterval.TimeIntervals)
			{
				var clonedEmployeeTimeIntervalPart = new TimeInterval()
				{
					UID = timeIntervalPart.UID,
					StartTime = timeIntervalPart.StartTime,
					EndTime = timeIntervalPart.EndTime,
					IntervalTransitionType = timeIntervalPart.IntervalTransitionType
				};
				timeIntervalParts.Add(clonedEmployeeTimeIntervalPart);
			}
			if (IsNew)
			{
				var newEmployeeTimeIntervalPart = new TimeInterval()
				{
					StartTime = StartTime,
					EndTime = EndTime,
					IntervalTransitionType = SelectedTransition
				};
				timeIntervalParts.Add(newEmployeeTimeIntervalPart);
			}
			else
			{
				var deitingTimeIntervalPart = timeIntervalParts.FirstOrDefault(x => x.UID == TimeIntervalPart.UID);
				if (deitingTimeIntervalPart != null)
				{
					deitingTimeIntervalPart.StartTime = StartTime;
					deitingTimeIntervalPart.EndTime = EndTime;
					deitingTimeIntervalPart.IntervalTransitionType = SelectedTransition;
				}
			}
			return timeIntervalParts;
		}
	}
}