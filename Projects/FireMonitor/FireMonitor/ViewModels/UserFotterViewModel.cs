﻿using FiresecClient;
using Infrastructure;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Events;

namespace FireMonitor.ViewModels
{
	public class UserFotterViewModel : BaseViewModel
	{
		public UserFotterViewModel()
		{
			ServiceFactory.Events.GetEvent<UserChangedEvent>().Unsubscribe(OnUserChanged);
			ServiceFactory.Events.GetEvent<UserChangedEvent>().Subscribe(OnUserChanged);
		}

		public string UserName
		{
			get { return FiresecManager.CurrentUser.Name; }
		}
		void OnUserChanged(UserChangedEventArgs userChangedEventArgs)
		{
			OnPropertyChanged("UserName");
		}
	}
}