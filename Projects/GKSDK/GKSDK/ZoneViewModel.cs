﻿using System;
using System.Windows;
using FiresecAPI.Models;
using XFiresecAPI;
using Infrastructure.Common.Windows.ViewModels;

namespace GKSDK
{
	public class ZoneViewModel : BaseViewModel
	{
		public ZoneViewModel(XZoneState zoneState)
		{
			ZoneState = zoneState;
			_stateClass = zoneState.StateClass;
			zoneState.StateChanged += new Action(OnStateChanged);
			No = zoneState.Zone.No;
			Name = zoneState.Zone.Name;
		}

		public void SafeCall(Action action)
		{
			if (Application.Current != null && Application.Current.Dispatcher != null)
				Application.Current.Dispatcher.BeginInvoke(action);
		}

		void OnStateChanged()
		{
			StateClass = ZoneState.StateClass;
		}

		public XZoneState ZoneState { get; private set; }
		public int No { get; private set; }
		public string Name { get; private set; }

		XStateClass _stateClass;
		public XStateClass StateClass
		{
			get { return _stateClass; }
			set
			{
				_stateClass = value;
				OnPropertyChanged("StateClass");
			}
		}
	}
}