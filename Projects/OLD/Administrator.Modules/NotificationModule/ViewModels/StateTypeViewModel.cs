﻿using FiresecAPI;
using Infrastructure.Common.Windows.ViewModels;

namespace NotificationModule.ViewModels
{
	public class StateTypeViewModel : BaseViewModel
	{
		public StateTypeViewModel(StateType stateType)
		{
			StateType = stateType;
		}

		public StateType StateType { get; private set; }

		public bool IsChecked { get; set; }
	}
}