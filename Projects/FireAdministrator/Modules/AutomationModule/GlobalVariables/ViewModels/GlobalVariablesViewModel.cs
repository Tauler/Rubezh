﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Automation;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.ViewModels;
using FiresecClient;
using System.Windows.Input;
using KeyboardKey = System.Windows.Input.Key;

namespace AutomationModule.ViewModels
{
	public class GlobalVariablesViewModel : MenuViewPartViewModel, IEditingViewModel, ISelectable<Guid>
	{
		public static GlobalVariablesViewModel Current { get; private set; }
		public GlobalVariablesViewModel()
		{
			Current = this;
			Menu = new GlobalVariablesMenuViewModel(this);
			AddCommand = new RelayCommand(OnAdd, CanAdd);
			DeleteCommand = new RelayCommand(OnDelete, CanEditDelete);
			EditCommand = new RelayCommand(OnEdit, CanEditDelete);
			RegisterShortcuts();
		}

		public void Initialize()
		{
			GlobalVariables = new ObservableCollection<VariableViewModel>();
			foreach (var globalVariable in FiresecManager.SystemConfiguration.AutomationConfiguration.GlobalVariables)
			{
				var globalVariableViewModel = new VariableViewModel(globalVariable);
				GlobalVariables.Add(globalVariableViewModel);
			}
			SelectedGlobalVariable = GlobalVariables.FirstOrDefault();
		}

		ObservableCollection<VariableViewModel> _globalVariables;
		public ObservableCollection<VariableViewModel> GlobalVariables
		{
			get { return _globalVariables; }
			set
			{
				_globalVariables = value;
				OnPropertyChanged(() => GlobalVariables);
			}
		}

		VariableViewModel _selectedGlobalVariable;
		public VariableViewModel SelectedGlobalVariable
		{
			get { return _selectedGlobalVariable; }
			set
			{
				_selectedGlobalVariable = value;
				OnPropertyChanged(() => SelectedGlobalVariable);
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var globalVariableDetailsViewModel = new VariableDetailsViewModel(null, "глобальная переменная", "Добавить глобальную переменную");
			if (DialogService.ShowModalWindow(globalVariableDetailsViewModel))
			{
				globalVariableDetailsViewModel.Variable.IsGlobal = true;
				FiresecManager.SystemConfiguration.AutomationConfiguration.GlobalVariables.Add(globalVariableDetailsViewModel.Variable);
				var globalVariableViewModel = new VariableViewModel(globalVariableDetailsViewModel.Variable);
				GlobalVariables.Add(globalVariableViewModel);
				SelectedGlobalVariable = globalVariableViewModel;
				SelectedGlobalVariable.Update();
				ServiceFactory.SaveService.AutomationChanged = true;
			}
		}

		bool CanAdd()
		{
			return true;
		}

		public RelayCommand DeleteCommand { get; private set; }
		void OnDelete()
		{
			FiresecManager.SystemConfiguration.AutomationConfiguration.GlobalVariables.Remove(SelectedGlobalVariable.Variable);
			GlobalVariables.Remove(SelectedGlobalVariable);
			SelectedGlobalVariable = GlobalVariables.FirstOrDefault();
			ServiceFactory.SaveService.AutomationChanged = true;
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var globalVariableDetailsViewModel = new VariableDetailsViewModel(SelectedGlobalVariable.Variable, "глобальная переменная", "Редактировать глобальную переменную");
			if (DialogService.ShowModalWindow(globalVariableDetailsViewModel))
			{
				globalVariableDetailsViewModel.Variable.IsGlobal = true;
				PropertyCopy.Copy(globalVariableDetailsViewModel.Variable, SelectedGlobalVariable.Variable);
				SelectedGlobalVariable.Update();
				ServiceFactory.SaveService.AutomationChanged = true;
			}
		}

		bool CanEditDelete()
		{
			return SelectedGlobalVariable != null;
		}

		public void Select(Guid globalVariableUid)
		{
			if (globalVariableUid != Guid.Empty)
			{
				SelectedGlobalVariable = GlobalVariables.FirstOrDefault(item => item.Variable.Uid == globalVariableUid);
			}
		}

		private void RegisterShortcuts()
		{
			RegisterShortcut(new KeyGesture(KeyboardKey.N, ModifierKeys.Control), AddCommand);
			RegisterShortcut(new KeyGesture(KeyboardKey.Delete, ModifierKeys.Control), DeleteCommand);
			RegisterShortcut(new KeyGesture(KeyboardKey.E, ModifierKeys.Control), EditCommand);
		}
	}
}