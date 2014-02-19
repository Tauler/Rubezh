﻿using System;
using System.Linq;
using FiresecAPI;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.ObjectModel;

namespace SKDModule.ViewModels
{
	public class EmployeeCardDetailsViewModel : SaveCancelDialogViewModel
	{
		public SKDCard Card { get; private set; }
		public AccessZonesSelectationViewModel AccessZones { get; private set; }
		public AccessZonesSelectationViewModel AdditionalGUDZones { get; private set; }
		public AccessZonesSelectationViewModel ExceptedGUDZones { get; private set; }

		public EmployeeCardDetailsViewModel(SKDCard card = null)
		{
			Card = card;
			if (card == null)
			{
				Title = "Создание карты";
				card = new SKDCard()
				{
					Series = 0,
					Number = 0,
					ValidFrom = DateTime.Now,
					ValidTo = DateTime.Now.AddYears(1)
				};
			}
			else
			{
				Title = string.Format("Свойства карты: {0}", card.Series + "/" + card.Number);
			}
			Card = card;
			if (Card.Series.HasValue)
				IDFamily = Card.Series.Value;
			if (Card.Number.HasValue)
				IDNo = Card.Number.Value;
			if (Card.ValidFrom.HasValue)
				StartDate = Card.ValidFrom.Value;
			if (Card.ValidTo.HasValue)
				EndDate = Card.ValidTo.Value;

			AccessZones = new AccessZonesSelectationViewModel(Card.CardZones);
			AdditionalGUDZones = new AccessZonesSelectationViewModel(Card.AdditionalGUDZones);
			ExceptedGUDZones = new AccessZonesSelectationViewModel(Card.ExceptedGUDZones);

			AvailableGUDs = new ObservableCollection<GUD>();
			SelectedGUD = AvailableGUDs.FirstOrDefault(x => x.UID == Card.GUDUid);
		}

		int _idFamily;
		public int IDFamily
		{
			get { return _idFamily; }
			set
			{
				_idFamily = value;
				OnPropertyChanged("IDFamily");
			}
		}

		int _idNo;
		public int IDNo
		{
			get { return _idNo; }
			set
			{
				_idNo = value;
				OnPropertyChanged("IDNo");
			}
		}

		DateTime _startDate;
		public DateTime StartDate
		{
			get { return _startDate; }
			set
			{
				_startDate = value;
				OnPropertyChanged("StartDate");
			}
		}

		DateTime _endDate;
		public DateTime EndDate
		{
			get { return _endDate; }
			set
			{
				_endDate = value;
				OnPropertyChanged("EndDate");
			}
		}

		ObservableCollection<GUD> _availableGUDs;
		public ObservableCollection<GUD> AvailableGUDs
		{
			get { return _availableGUDs; }
			set
			{
				_availableGUDs = value;
				OnPropertyChanged("AvailableGUDs");
			}
		}

		GUD _selectedGUD;
		public GUD SelectedGUD
		{
			get { return _selectedGUD; }
			set
			{
				_selectedGUD = value;
				OnPropertyChanged("SelectedGUD");
			}
		}

		protected override bool Save()
		{
			Card.Series = IDFamily;
			Card.Number = IDNo;
			Card.ValidFrom = StartDate;
			Card.ValidTo = EndDate;
			Card.CardZones = AccessZones.GetCardZones();
			Card.AdditionalGUDZones = AdditionalGUDZones.GetCardZones();
			Card.ExceptedGUDZones = ExceptedGUDZones.GetCardZones();

			if (SelectedGUD != null)
				Card.GUDUid = SelectedGUD.UID;
			return true;
		}
	}
}