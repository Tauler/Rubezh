﻿using System.Collections.ObjectModel;
using FiresecAPI;
using FiresecAPI.Automation;

namespace AutomationModule.ViewModels
{
	public class ControlSKDDeviceStepViewModel: BaseStepViewModel
	{
		ControlSKDDeviceArguments ControlSKDDeviceArguments { get; set; }
		public ArgumentViewModel SKDDeviceArgument { get; private set; }

		public ControlSKDDeviceStepViewModel(StepViewModel stepViewModel) : base(stepViewModel)
		{
			ControlSKDDeviceArguments = stepViewModel.Step.ControlSKDDeviceArguments;
			Commands = ProcedureHelper.GetEnumObs<SKDDeviceCommandType>();
			SKDDeviceArgument = new ArgumentViewModel(ControlSKDDeviceArguments.SKDDeviceArgument, stepViewModel.Update, null);
			SelectedCommand = ControlSKDDeviceArguments.Command;
		}

		public ObservableCollection<SKDDeviceCommandType> Commands { get; private set; }
		SKDDeviceCommandType _selectedCommand;
		public SKDDeviceCommandType SelectedCommand
		{
			get { return _selectedCommand; }
			set
			{
				_selectedCommand = value;
				ControlSKDDeviceArguments.Command = value;
				OnPropertyChanged(()=>SelectedCommand);
			}
		}

		public override void UpdateContent()
		{
			SKDDeviceArgument.Update(Procedure, ExplicitType.Object, objectType:ObjectType.SKDDevice, isList:false);
		}

		public override string Description
		{
			get
			{
				return "Устройство: " + SKDDeviceArgument.Description + " Команда: " + SelectedCommand.ToDescription();
			}
		}
	}
}

