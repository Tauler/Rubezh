﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Automation;
using FiresecClient;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;

namespace AutomationModule.ViewModels
{
	public class ProcedureArgumentsViewModel : SaveCancelDialogViewModel
	{
		public ProcedureArgumentsViewModel(Procedure procedure)
		{
			Title = procedure.Name;
			SaveCaption = "Выполнить";
			ArgumentViewModels = new ArgumentsViewModel(procedure);
		}

		public ArgumentsViewModel ArgumentViewModels { get; private set; }

		public static void Run(Procedure procedure)
		{
			List<Argument> args = null;
			if (procedure.Arguments.Count > 0)
			{
				var viewModel = new ProcedureArgumentsViewModel(procedure);
				if (DialogService.ShowModalWindow(viewModel))
					args = viewModel.ArgumentViewModels.Arguments.Select(x => x.Argument).ToList();
			}
			ProcedureHelper.Run(procedure, args);
		}
	}
}
