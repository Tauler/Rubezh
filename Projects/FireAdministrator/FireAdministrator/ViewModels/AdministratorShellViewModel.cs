using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Ribbon;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using FireAdministrator.Properties;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Windows.Shapes;
using System;
using System.Linq;
using System.Collections.Generic;
using FiresecAPI.Models;

namespace FireAdministrator.ViewModels
{
	public class AdministratorShellViewModel : ShellViewModel
	{
		private MenuViewModel _menu;
		private RibbonMenuItemViewModel _showToolbar;
		private RibbonMenuItemViewModel _showMenu;

		public AdministratorShellViewModel()
			: base(ClientType.Administrator)
		{
			Title = GlobalSettingsHelper.GlobalSettings.UseStrazhBrand ? "Администратор" : "Администратор ОПС FireSec";
			Height = 700;
			Width = 1000;
			MinWidth = 1000;
			MinHeight = 550;
			ShowToolbarCommand = new RelayCommand(OnShowToolbar, CanShowMenu);
			ShowMenuCommand = new RelayCommand(OnShowMenu, CanShowMenu);
			_menu = new MenuViewModel();
			_menu.LogoSource = "Logo";
			Toolbar = _menu;
			RibbonContent = new RibbonMenuViewModel();
			AddRibbonItem();
			AllowLogoIcon = false;
			RibbonVisible = true;
		}

		public override void Run()
		{
			base.Run();
			ServiceFactory.Layout.ShortcutService.Shortcuts.Add(new KeyGesture(Key.S, ModifierKeys.Control), _menu.SaveCommand);			
		}
		public override bool OnClosing(bool isCanceled)
		{
			try
			{
				AlarmPlayerHelper.Dispose();
				if (ServiceFactory.SaveService.HasChanges)
				{
					var result = MessageBoxService.ShowQuestionExtended("Применить изменения в конфигурации?");
					switch (result)
					{
						case MessageBoxResult.Yes:
							return !((MenuViewModel)Toolbar).SetNewConfig();
						case MessageBoxResult.No:
							return false;
						case MessageBoxResult.Cancel:
							return true;
					}
				}
				FiresecManager.Disconnect();
				return base.OnClosing(isCanceled);
			}
			finally
			{

			}
		}

		public override void OnClosed()
		{
			//FiresecManager.Disconnect();
			base.OnClosed();
		}

		private void AddRibbonItem()
		{
			_showToolbar = new RibbonMenuItemViewModel("", ShowToolbarCommand, "BToolbar");
			_showMenu = new RibbonMenuItemViewModel("", ShowMenuCommand, "BMenu");
			UpdateToolbarTitle();
			RibbonContent.Items.Add(new RibbonMenuItemViewModel("Проект", new ObservableCollection<RibbonMenuItemViewModel>()
			{
				new RibbonMenuItemViewModel("Новый", _menu.CreateNewCommand, "BNew", "Создать новую конфигурацию"),
				new RibbonMenuItemViewModel("Открыть", _menu.LoadFromFileCommand, "BLoad", "Открыть конфигурацию из файла"),
				new RibbonMenuItemViewModel("Проверить", _menu.ValidateCommand, "BCheck", "Проверить конфигурацию"),
				new RibbonMenuItemViewModel("Применить", _menu.SetNewConfigCommand, "BDownload", "Применить конфигурацию"),
				new RibbonMenuItemViewModel("Сохранить", _menu.SaveCommand, "BSave", "Сохранить конфигурацию в файл"),
				new RibbonMenuItemViewModel("Сохранить как", _menu.SaveAsCommand, "BSaveAs", "Сохранить как"),
				new RibbonMenuItemViewModel("Слияние конфигураций", _menu.MergeConfigurationCommand, "BAllParameters", "Слияние конфигураций"),
				new RibbonMenuItemViewModel("Вид", new ObservableCollection<RibbonMenuItemViewModel>()
				{
					_showMenu,
					_showToolbar,
				}, "BView") { Order = 1000 }, 
			}, "BConfig", "Операции с конфигурацией") { Order = int.MinValue });
			RibbonContent.Items.Add(new RibbonMenuItemViewModel("Выход", ApplicationCloseCommand, "BExit") { Order = int.MaxValue });
		}

		public RelayCommand ShowToolbarCommand { get; private set; }
		private void OnShowToolbar()
		{
			_menu.IsMenuVisible = !_menu.IsMenuVisible;
			UpdateToolbarTitle();
		}
		public RelayCommand ShowMenuCommand { get; private set; }
		private void OnShowMenu()
		{
			_menu.IsMainMenuVisible = !_menu.IsMainMenuVisible;
			UpdateToolbarTitle();
		}
		private bool CanShowMenu()
		{
			return ToolbarVisible;
		}

		private void UpdateToolbarTitle()
		{
			_showToolbar.Text = _menu.IsMenuVisible ? "Скрыть панель инструментов" : "Показать панель инструментов";
			_showMenu.Text = _menu.IsMainMenuVisible ? "Скрыть панель меню" : "Показать панель меню";
		}
	}
}