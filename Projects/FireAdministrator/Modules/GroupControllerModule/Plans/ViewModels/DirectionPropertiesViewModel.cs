﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.GK;
using FiresecClient;
using GKModule.Events;
using GKModule.ViewModels;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using Infrustructure.Plans.Elements;

namespace GKModule.Plans.ViewModels
{
	public class DirectionPropertiesViewModel : SaveCancelDialogViewModel
	{
		private IElementDirection _element;
		private DirectionsViewModel _directionsViewModel;

		public DirectionPropertiesViewModel(IElementDirection element, DirectionsViewModel directionsViewModel)
		{
			_directionsViewModel = directionsViewModel;
			_element = element;
			CreateCommand = new RelayCommand(OnCreate);
			EditCommand = new RelayCommand(OnEdit, CanEdit);
			Title = "Свойства фигуры: ГК Направление";
			var directions = GKManager.Directions;
			Directions = new ObservableCollection<DirectionViewModel>();
			foreach (var direction in directions)
			{
				var directionViewModel = new DirectionViewModel(direction);
				Directions.Add(directionViewModel);
			}
			if (_element.DirectionUID != Guid.Empty)
				SelectedDirection = Directions.FirstOrDefault(x => x.Direction.UID == _element.DirectionUID);
		}

		public ObservableCollection<DirectionViewModel> Directions { get; private set; }

		private DirectionViewModel _selectedDirection;
		public DirectionViewModel SelectedDirection
		{
			get { return _selectedDirection; }
			set
			{
				_selectedDirection = value;
				OnPropertyChanged(() => SelectedDirection);
			}
		}

		public RelayCommand CreateCommand { get; private set; }
		private void OnCreate()
		{
			Guid xdirectionUID = _element.DirectionUID;
			var createDirectionEventArg = new CreateGKDirectionEventArg();
			ServiceFactory.Events.GetEvent<CreateGKDirectionEvent>().Publish(createDirectionEventArg);
			if (createDirectionEventArg.Direction != null)
				_element.DirectionUID = createDirectionEventArg.Direction.UID;
			GKPlanExtension.Instance.Cache.BuildSafe<GKDirection>();
			GKPlanExtension.Instance.SetItem<GKDirection>(_element);
			UpdateDirections(xdirectionUID);
			if (!createDirectionEventArg.Cancel)
				Close(true);
		}

		public RelayCommand EditCommand { get; private set; }
		private void OnEdit()
		{
			ServiceFactory.Events.GetEvent<EditGKDirectionEvent>().Publish(SelectedDirection.Direction.UID);
			SelectedDirection.Update(SelectedDirection.Direction);
		}
		private bool CanEdit()
		{
			return SelectedDirection != null;
		}

		protected override bool Save()
		{
			Guid directionUID = _element.DirectionUID;
			GKPlanExtension.Instance.SetItem<GKDirection>(_element, SelectedDirection == null ? null : SelectedDirection.Direction);
			UpdateDirections(directionUID);
			return base.Save();
		}
		private void UpdateDirections(Guid directionUID)
		{
			if (_directionsViewModel != null)
			{
				if (directionUID != _element.DirectionUID)
					Update(directionUID);
				Update(_element.DirectionUID);
				_directionsViewModel.LockedSelect(_element.DirectionUID);
			}
		}
		private void Update(Guid directionUID)
		{
			var direction = _directionsViewModel.Directions.FirstOrDefault(x => x.Direction.UID == directionUID);
			if (direction != null)
				direction.Update();
		}
	}
}