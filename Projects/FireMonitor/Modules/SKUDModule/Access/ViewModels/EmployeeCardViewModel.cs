﻿using System;
using System.Collections.Generic;
using FiresecAPI;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class EmployeeCardViewModel : BaseViewModel
	{
		public SKDCard Card { get; private set; }
		UserAccessViewModel UserAccessViewModel;
		List<Guid> ZoneUIDs;
		public CardZonesViewModel CardZonesViewModel { get; private set; }

		public EmployeeCardViewModel(UserAccessViewModel userAccessViewModel, SKDCard card)
		{
			RemoveCommand = new RelayCommand(OnRemove);
			ShowPropertiesCommand = new RelayCommand(OnShowProperties, CanShowProperties);

			UserAccessViewModel = userAccessViewModel;
			Card = card;
			ZoneUIDs = new List<Guid>();
			CardZonesViewModel = new CardZonesViewModel(Card.CardZones);
		}

		bool _isBlocked;
		public bool IsBlocked
		{
			get { return _isBlocked; }
			set
			{
				_isBlocked = value;
				OnPropertyChanged("IsBlocked");
			}
		}

		public string ID
		{
			get { return Card.Series + "/" + Card.Number; }
		}
		public DateTime StartDate
		{
			get { return Card.ValidFrom.GetValueOrDefault(DateTime.MinValue); }
		}
		public DateTime EndDate
		{
			get { return Card.ValidTo.GetValueOrDefault(DateTime.MaxValue); }
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			UserAccessViewModel.RemoveCard(this);
		}

		public RelayCommand ShowPropertiesCommand { get; private set; }
		void OnShowProperties()
		{
			var employeeCardDetailsViewModel = new EmployeeCardDetailsViewModel(Card);
			if (DialogService.ShowModalWindow(employeeCardDetailsViewModel))
			{
				Card = employeeCardDetailsViewModel.Card;
				OnPropertyChanged("ID");
				OnPropertyChanged("StartDate");
				OnPropertyChanged("EndDate");
				CardZonesViewModel.Update();
			}
		}
		public bool CanShowProperties()
		{
			return true;
		}
	}
}