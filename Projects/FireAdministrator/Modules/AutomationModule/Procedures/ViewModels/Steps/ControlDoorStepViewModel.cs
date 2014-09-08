﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Automation;
using FiresecAPI.SKD;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using ValueType = FiresecAPI.Automation.ValueType;

namespace AutomationModule.ViewModels
{
	public class ControlDoorStepViewModel : BaseStepViewModel
	{
		ControlDoorArguments ControlDoorArguments { get; set; }
		Procedure Procedure { get; set; }
		public ArithmeticParameterViewModel Variable1 { get; private set; }

		public ControlDoorStepViewModel(ControlDoorArguments controlDoorArguments, Procedure procedure, Action updateDescriptionHandler)
			: base(updateDescriptionHandler)
		{
			ControlDoorArguments = controlDoorArguments;
			Procedure = procedure;
			Commands = ProcedureHelper.GetEnumObs<DoorCommandType>();
			Variable1 = new ArithmeticParameterViewModel(ControlDoorArguments.Variable1);
			OnPropertyChanged(() => Commands);
			SelectDoorCommand = new RelayCommand(OnSelectDoor);
			UpdateContent();
		}

		public ObservableCollection<DoorCommandType> Commands { get; private set; }

		DoorCommandType _selectedCommand;
		public DoorCommandType SelectedCommand
		{
			get { return _selectedCommand; }
			set
			{
				_selectedCommand = value;
				ControlDoorArguments.DoorCommandType = value;
				OnPropertyChanged(()=>SelectedCommand);
			}
		}

		DoorViewModel _selectedDoor;
		public DoorViewModel SelectedDoor
		{
			get { return _selectedDoor; }
			set
			{
				_selectedDoor = value;
				Variable1.UidValue = Guid.Empty;
				if (_selectedDoor != null)
				{
					Variable1.UidValue = _selectedDoor.Door.UID;
				}
				OnPropertyChanged(() => SelectedDoor);
			}
		}
		
		public RelayCommand SelectDoorCommand { get; private set; }
		private void OnSelectDoor()
		{
			var doorSelectationViewModel = new DoorSelectionViewModel(SelectedDoor != null ? SelectedDoor.Door : null);
			if (DialogService.ShowModalWindow(doorSelectationViewModel))
			{
				SelectedDoor = doorSelectationViewModel.SelectedDoor;
			}
		}

		public void UpdateContent()
		{
			Variable1.Update(ProcedureHelper.GetAllVariables(Procedure, ValueType.Object, ObjectType.ControlDoor, false));
			if (Variable1.UidValue != Guid.Empty)
			{
				var door = SKDManager.Doors.FirstOrDefault(x => x.UID == Variable1.UidValue);
				SelectedDoor = door != null ? new DoorViewModel(door) : null;
				SelectedCommand = ControlDoorArguments.DoorCommandType;
			}
		}

		public override string Description
		{
			get { return ""; }
		}
	}
}
