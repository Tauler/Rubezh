﻿using FiresecAPI.Automation;

namespace AutomationModule.ViewModels
{
	public class RunProgramStepViewModel : BaseStepViewModel
	{
		RunProgramArguments RunProgramArguments { get; set; }
		public ArgumentViewModel PathArgument { get; private set; }
		public ArgumentViewModel ParametersArgument { get; private set; }

		public RunProgramStepViewModel(StepViewModel stepViewModel) : base(stepViewModel)
		{
			RunProgramArguments = stepViewModel.Step.RunProgramArguments;
			PathArgument = new ArgumentViewModel(RunProgramArguments.PathArgument, stepViewModel.Update, UpdateContent);
			ParametersArgument = new ArgumentViewModel(RunProgramArguments.ParametersArgument, stepViewModel.Update, UpdateContent);
		}

		public override void UpdateContent()
		{
			PathArgument.Update(Procedure, ExplicitType.String, isList:false);
			ParametersArgument.Update(Procedure, ExplicitType.String, isList: false);
		}

		public override string Description
		{
			get
			{
				return "Путь к программе: " + PathArgument.Description + " Параметры запуска: " + ParametersArgument.Description;
			}
		}
	}
}
