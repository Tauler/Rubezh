﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.SKD;
using FiresecClient.SKDHelpers;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using SKDModule.Events;
using SKDModule.PassCard.ViewModels;
using FiresecClient;

namespace SKDModule.ViewModels
{
	public class EmployeeCardViewModel : BaseViewModel
	{
		public Organisation Organisation { get; private set; }
		public SKDCard Card { get; private set; }
		public EmployeeViewModel EmployeeViewModel { get; private set; }
		public CardDoorsViewModel CardDoorsViewModel { get; private set; }

		public EmployeeCardViewModel(Organisation organisation, EmployeeViewModel employeeViewModel, SKDCard card)
		{
			RemoveCommand = new RelayCommand(OnRemove, CanEditDelete);
			EditCommand = new RelayCommand(OnEdit, CanEditDelete);
			PrintCommand = new RelayCommand(OnPrint);
			SelectCardCommand = new RelayCommand(OnSelectCard);

			Organisation = organisation;
			EmployeeViewModel = employeeViewModel;
			Card = card;

			SetCardDoors();
		}

		public string Name
		{
			get { return "Пропуск " + Card.Number; }
		}

		List<CardDoor> GetCardDoors()
		{
			var cardDoors = new List<CardDoor>();
			cardDoors.AddRange(Card.CardDoors);
			if (Card.AccessTemplateUID != null)
			{
				var accessTemplates = AccessTemplateHelper.Get(new AccessTemplateFilter());
				if (accessTemplates != null)
				{
					var accessTemplate = accessTemplates.FirstOrDefault(x => x.UID == Card.AccessTemplateUID);
					if (accessTemplate != null)
					{
						foreach (var cardZone in accessTemplate.CardDoors)
						{
							if (!cardDoors.Any(x => x.DoorUID == cardZone.DoorUID))
								cardDoors.Add(cardZone);
						}
					}
				}
			}
			return cardDoors;
		}

		public void SetCardDoors()
		{
			var cardDoors = GetCardDoors();
			CardDoorsViewModel = new CardDoorsViewModel(cardDoors);
			OnPropertyChanged(() => CardDoorsViewModel);
		}

		public void UpdateCardDoors()
		{
			var cardDoors = GetCardDoors();
			CardDoorsViewModel.Update(cardDoors);
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			var cardRemovalReasonViewModel = new CardRemovalReasonViewModel();
			if (DialogService.ShowModalWindow(cardRemovalReasonViewModel))
			{
				var cardRemovalReason = cardRemovalReasonViewModel.RemovalReason;
				var toStopListResult = CardHelper.DeleteFromEmployee(Card, cardRemovalReason);
				if (!toStopListResult)
					return;
				EmployeeViewModel.Cards.Remove(this);
				ServiceFactory.Events.GetEvent<BlockCardEvent>().Publish(Card.UID);
				EmployeeViewModel.OnSelectEmployee();
			}
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var employeeCardDetailsViewModel = new EmployeeCardDetailsViewModel(Organisation, EmployeeViewModel.Model, Card);
			if (DialogService.ShowModalWindow(employeeCardDetailsViewModel))
			{
				var card = employeeCardDetailsViewModel.Card;
				Card = card;
				OnPropertyChanged(() => Card);
				OnPropertyChanged(() => Name);
				SetCardDoors();
			}
		}

		bool CanEditDelete()
		{
			return FiresecManager.CheckPermission(FiresecAPI.Models.PermissionType.Oper_SKD_Cards_Etit);
		}

		public RelayCommand PrintCommand { get; private set; }
		void OnPrint()
		{
			var passCardViewModel = new PassCardViewModel(EmployeeViewModel.Model.UID, Card);
			DialogService.ShowModalWindow(passCardViewModel);
		}

		public RelayCommand SelectCardCommand { get; private set; }
		void OnSelectCard()
		{
			EmployeeViewModel.SelectCard(this);
			IsCardSelected = true;
		}

		bool _isCardSelected;
		public bool IsCardSelected
		{
			get { return _isCardSelected; }
			set
			{
				_isCardSelected = value;
				OnPropertyChanged(() => IsCardSelected);
			}
		}
	}
}