﻿using FiresecAPI.Models;
using Infrastructure.Common.Windows.ViewModels;

namespace VideoModule.ViewModels
{
	public class SettingsSelectionViewModel : SaveCancelDialogViewModel
	{
		public RviSettings RviSettings { get; private set; }

		public SettingsSelectionViewModel(RviSettings rviSettings)
		{
			Title = "Настройки";
			RviSettings = rviSettings;
			Ip = rviSettings.Ip;
			Port = rviSettings.Port;
			Login = rviSettings.Login;
			Password = rviSettings.Password;
			DllsPath = rviSettings.DllsPath;
			PluginsPath = rviSettings.PluginsPath;
		}

		string _ip;
		public string Ip
		{
			get { return _ip; }
			set
			{
				_ip = value;
				OnPropertyChanged(() => Ip);
			}
		}

		int _port;
		public int Port
		{
			get { return _port; }
			set
			{
				_port = value;
				OnPropertyChanged(() => Port);
			}
		}

		string _login;
		public string Login
		{
			get { return _login; }
			set
			{
				_login = value;
				OnPropertyChanged(() => Login);
			}
		}

		string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged(() => Password);
			}
		}

		string _dllsPath;
		public string DllsPath
		{
			get { return _dllsPath; }
			set
			{
				_dllsPath = value;
				OnPropertyChanged(() => DllsPath);
			}
		}

		string _pluginsPath;
		public string PluginsPath
		{
			get { return _pluginsPath; }
			set
			{
				_pluginsPath = value;
				OnPropertyChanged(() => PluginsPath);
			}
		}

		protected override bool Save()
		{
			RviSettings.Ip = Ip;
			RviSettings.Port = Port;
			RviSettings.Login = Login;
			RviSettings.Password = Password;
			RviSettings.DllsPath = DllsPath;
			RviSettings.PluginsPath = PluginsPath;
			return base.Save();
		}
	}
}
