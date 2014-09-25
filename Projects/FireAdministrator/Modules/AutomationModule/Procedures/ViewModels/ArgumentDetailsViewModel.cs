﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using FiresecAPI.Automation;
using System.Collections.ObjectModel;
using Infrastructure;

namespace AutomationModule.ViewModels
{
	public class ArgumentDetailsViewModel : SaveCancelDialogViewModel
	{
		bool automationChanged;
		public VariableViewModel VariableViewModel { get; protected set; }
		public Argument Argument { get; private set; }

		public ArgumentDetailsViewModel(Argument argument, bool isList)
		{
			automationChanged = ServiceFactory.SaveService.AutomationChanged;
			Title = "Редактировать аргумент";
			Argument = new Argument();
			PropertyCopy.Copy<Argument, Argument>(argument, Argument);
			var newArgument = new Argument();
			PropertyCopy.Copy<Argument, Argument>(argument, newArgument);
			VariableViewModel = new VariableViewModel(argument, isList);
		}

		public override bool OnClosing(bool isCanceled)
		{
			ServiceFactory.SaveService.AutomationChanged = automationChanged;
			return base.OnClosing(isCanceled);
		}

		protected override bool Save()
		{
			PropertyCopy.Copy<ExplicitValue, ExplicitValue>(VariableViewModel.ExplicitValue.ExplicitValue, Argument.ExplicitValue);
			Argument.ExplicitValues = new List<ExplicitValue>();			
			foreach (var explicitValue in VariableViewModel.ExplicitValues)
			{
				var newExplicitValue = new ExplicitValue();
				PropertyCopy.Copy<ExplicitValue, ExplicitValue>(explicitValue.ExplicitValue, newExplicitValue);
				Argument.ExplicitValues.Add(newExplicitValue);
			}			
			return base.Save();
		}
	}
}
