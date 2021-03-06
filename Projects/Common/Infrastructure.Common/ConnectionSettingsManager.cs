﻿using System;
using Common;

namespace Infrastructure.Common
{
	public static class ConnectionSettingsManager
	{
		static string _remoteAddress;
		public static string RemoteAddress
		{
			get { return _remoteAddress; }
			set
			{
				_remoteAddress = value;
				if (_remoteAddress == "localhost")
					_remoteAddress = "127.0.0.1";
			}
		}

		public static int RemotePort { get; set; }
		public static int ReportRemotePort { get; set; }

		static ConnectionSettingsManager()
		{
			try
			{
				RemoteAddress = GlobalSettingsHelper.GlobalSettings.RemoteAddress;
				RemotePort = GlobalSettingsHelper.GlobalSettings.RemotePort;
				ReportRemotePort = GlobalSettingsHelper.GlobalSettings.ReportRemotePort;
			}
			catch (Exception e)
			{
				Logger.Error(e, "AppSettingsManager.AppSettingsManager");
			}
		}

		public static string ServerAddress
		{
			get
			{
				var serviceAddress = "net.pipe://127.0.0.1/FiresecService/";
				if (IsRemote)
				{
					serviceAddress = "net.tcp://" + RemoteAddress + ":" + RemotePort.ToString() + "/FiresecService/";
				}
				return serviceAddress;
			}
		}

		public static string FSAgentServerAddress
		{
			get
			{
				var serviceAddress = "net.pipe://127.0.0.1/FSAgent/";
				if (IsRemote)
				{
					serviceAddress = "net.tcp://" + RemoteAddress + ":" + (RemotePort + 1).ToString() + "/FSAgent/";
				}
				return serviceAddress;
			}
		}

		public static string FS2ServerAddress
		{
			get
			{
				var serviceAddress = "net.pipe://127.0.0.1/FS2/";
				if (IsRemote)
				{
					serviceAddress = "net.tcp://" + RemoteAddress + ":" + (RemotePort + 2).ToString() + "/FS2/";
				}
				return serviceAddress;
			}
		}

		public static string ReportServerAddress
		{
			get
			{
				var serviceAddress = "net.pipe://127.0.0.1/ReportFiresecService/";
				if (IsRemote)
				{
					serviceAddress = "net.tcp://" + RemoteAddress + ":" + ReportRemotePort.ToString() + "/ReportFiresecService/";
				}
				return serviceAddress;
			}
		}

		public static bool IsRemote
		{
			get
			{
				if (string.IsNullOrEmpty(RemoteAddress))
					return false;
				return (RemoteAddress != "localhost" && RemoteAddress != "127.0.0.1");
			}
		}
	}
}