﻿using System;
using System.Windows;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Common;
using FiresecClient;

namespace FireAdministrator
{
	public partial class App : Application
	{
		private const string SignalId = "Administrator";
		private const string WaitId = "AdministratorContinue";

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

#if DEBUG
			BindingErrorListener.Listen(m => MessageBox.Show(m));
#endif

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			bootstrapper = new Bootstrapper();
			//using (new DoubleLaunchLocker(SignalId, WaitId))
			bootstrapper.Initialize();
		}

		Bootstrapper bootstrapper;

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBoxService.ShowException(e.ExceptionObject as Exception);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			Logger.Info("App.OnExit");
			FiresecManager.Disconnect();
			VideoService.Close();
		}
	}
}