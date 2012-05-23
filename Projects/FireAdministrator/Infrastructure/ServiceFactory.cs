﻿using System.Windows;
using Infrastructure.Common;
using Microsoft.Practices.Prism.Events;
using Infrastructure.Client.Login;

namespace Infrastructure
{
	public class ServiceFactory
	{
		public static void Initialize(ILayoutService ILayoutService, IUserDialogService IUserDialogService,
			IProgressService IProgressService, IValidationService IValidationService)
		{
			SaveService = new SaveService();
			Events = new EventAggregator();
			ResourceService = new ResourceService();
			Layout = ILayoutService;
			UserDialogs = IUserDialogService;
			ProgressService = IProgressService;
			ValidationService = IValidationService;
			LoginService = new LoginService(UserDialogs, "Администратор", "Администратор. Авторизация");
		}

		public static AppSettings AppSettings { get; set; }
		public static SaveService SaveService { get; private set; }
		public static IEventAggregator Events { get; private set; }
		public static ResourceService ResourceService { get; private set; }
		public static ILayoutService Layout { get; private set; }
		public static IUserDialogService UserDialogs { get; private set; }
		public static IProgressService ProgressService { get; private set; }
		public static IValidationService ValidationService { get; private set; }
		public static LoginService LoginService { get; private set; }

		public static Window ShellView { get; set; }
	}
}