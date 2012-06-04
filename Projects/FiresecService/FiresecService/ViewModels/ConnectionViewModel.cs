﻿using System;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;

namespace FiresecService.ViewModels
{
	public class ConnectionViewModel : BaseViewModel
	{
		public FiresecService.Service.FiresecService FiresecService { get; set; }
		public Guid UID { get; set; }
		public string IpAddress { get; set; }
		public string ClientType { get; set; }
		public DateTime ConnectionDate { get; set; }

		string _userName;
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged("UserName");
			}
		}

		string _currentOperationName;
		public string CurrentOperationName
		{
			get { return _currentOperationName; }
			set
			{
				_currentOperationName = value;
				OnPropertyChanged("CurrentOperationName");
			}
		}
	}
}