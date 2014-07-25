﻿using ChinaSKDDriver;
using Infrastructure.Common.Windows.ViewModels;

namespace ControllerSDK.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public static Wrapper Wrapper { get; set; }

		public CardsViewModel CardsViewModel { get; private set; }
		public CardRecsViewModel CardRecsViewModel { get; private set; }
		public PasswordsViewModel PasswordsViewModel { get; private set; }
		public HolidaysViewModel HolidaysViewModel { get; private set; }
		public TimeShedulesViewModel TimeShedulesViewModel { get; private set; }
		public ControlViewModel ControlViewModel { get; private set; }
		public JournalViewModel JournalViewModel { get; private set; }
		public NativeJournalViewModel NativeJournalViewModel { get; private set; }
		public CommonViewModel CommonViewModel { get; private set; }
		public DoorConfigurationViewModel DoorConfigurationViewModel { get; private set; }
		public LogItemsViewModel LogItemsViewModel { get; private set; }

		public MainViewModel()
		{
			Wrapper = new Wrapper();

			CardsViewModel = new CardsViewModel();
			CardRecsViewModel = new CardRecsViewModel();
			PasswordsViewModel = new PasswordsViewModel();
			HolidaysViewModel = new HolidaysViewModel();
			TimeShedulesViewModel = new TimeShedulesViewModel();
			ControlViewModel = new ControlViewModel();
			JournalViewModel = new JournalViewModel();
			NativeJournalViewModel = new NativeJournalViewModel();
			CommonViewModel = new CommonViewModel();
			DoorConfigurationViewModel = new DoorConfigurationViewModel();
			LogItemsViewModel = new LogItemsViewModel();
		}
	}
}