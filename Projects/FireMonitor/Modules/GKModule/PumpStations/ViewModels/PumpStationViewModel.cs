﻿using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.GK;
using FiresecClient;
using GKModule.Events;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Events;

namespace GKModule.ViewModels
{
	public class PumpStationViewModel : BaseViewModel
	{
		public GKPumpStation PumpStation { get; private set; }
		public GKState State
		{
			get { return PumpStation.State; }
		}

		public PumpStationViewModel(GKPumpStation pumpStation)
		{
			ShowJournalCommand = new RelayCommand(OnShowJournal);
			ShowPropertiesCommand = new RelayCommand(OnShowProperties);
			PumpStation = pumpStation;
			State.StateChanged += new System.Action(OnStateChanged);
			OnStateChanged();

			Pumps = new ObservableCollection<DeviceViewModel>();
			foreach (var device in PumpStation.NSDevices)
			{
				var deviceViewModel = DevicesViewModel.Current.AllDevices.FirstOrDefault(x => x.Device == device);
				Pumps.Add(deviceViewModel);
			}
		}

		void OnStateChanged()
		{
			OnPropertyChanged(() => State);
			OnPropertyChanged(() => HasOnDelay);
			OnPropertyChanged(() => HasHoldDelay);
		}

		public ObservableCollection<DeviceViewModel> Pumps { get; private set; }

		public RelayCommand ShowJournalCommand { get; private set; }
		void OnShowJournal()
		{
			var showArchiveEventArgs = new ShowArchiveEventArgs()
			{
				GKPumpStation = PumpStation
			};
			ServiceFactory.Events.GetEvent<ShowArchiveEvent>().Publish(showArchiveEventArgs);
		}

		public RelayCommand ShowPropertiesCommand { get; private set; }
		void OnShowProperties()
		{
			DialogService.ShowWindow(new PumpStationDetailsViewModel(PumpStation));
		}

		public bool HasOnDelay
		{
			get { return State.StateClasses.Contains(XStateClass.TurningOn) && State.OnDelay > 0; }
		}
		public bool HasHoldDelay
		{
			get { return State.StateClasses.Contains(XStateClass.On) && State.HoldDelay > 0; }
		}

		public string StartPresentationName
		{
			get { return GKManager.GetPresentationLogic(PumpStation.StartLogic); }
		}
		public string StopPresentationName
		{
			get { return GKManager.GetPresentationLogic(PumpStation.StopLogic); }
		}
		public string AutomaticOffPresentationName
		{
			get { return GKManager.GetPresentationLogic(PumpStation.AutomaticOffLogic); }
		}
	}
}