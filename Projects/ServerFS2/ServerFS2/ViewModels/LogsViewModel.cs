﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.ObjectModel;

namespace ServerFS2.ViewModels
{
	public class LogsViewModel : DialogViewModel
	{
		public static LogsViewModel Current { get; private set; }

		public LogsViewModel()
		{
			Current = this;
			Title = "Лог сервера FS2";
			HidLogs = new ObservableCollection<string>();
		}

		public void AddLog(string value)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				HidLogs.Insert(0, value);
				if (HidLogs.Count > 1000)
					HidLogs.Remove(HidLogs.Last());
			}));
		}

		public ObservableCollection<string> HidLogs { get; private set; }
	}
}