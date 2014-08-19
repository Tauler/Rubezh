﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Automation;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.Generic;
using FiresecClient;
using ValueType = FiresecAPI.Automation.ValueType;

namespace AutomationModule.ViewModels
{
	public class ConditionStepViewModel : BaseViewModel, IStepViewModel
	{
		public ConditionArguments ConditionArguments { get; private set; }
		public ObservableCollection<ConditionViewModel> Conditions { get; private set; }
		Procedure Procedure { get; set; }
		public Action UpdateDescriptionHandler { get; set; }

		public ConditionStepViewModel(ConditionArguments conditionArguments, Procedure procedure, Action updateDescriptionHandler)
		{
			UpdateDescriptionHandler = updateDescriptionHandler;
			ConditionArguments = conditionArguments;
			Procedure = procedure;

			Conditions = new ObservableCollection<ConditionViewModel>();
			foreach (var condition in conditionArguments.Conditions)
			{
				var conditionViewModel = new ConditionViewModel(condition, procedure, updateDescriptionHandler);
				Conditions.Add(conditionViewModel);
			}
			if (Conditions.Count == 0)
			{
				var condition = new Condition();
				var conditionViewModel = new ConditionViewModel(condition, procedure, updateDescriptionHandler);
				conditionViewModel.UpdateDescriptionHandler = updateDescriptionHandler;
				ConditionArguments.Conditions.Add(condition);
				Conditions.Add(conditionViewModel);
			}
			JoinOperator = ConditionArguments.JoinOperator;

			AddCommand = new RelayCommand(OnAdd);
			RemoveCommand = new RelayCommand<ConditionViewModel>(OnRemove, CanRemove);
			ChangeJoinOperatorCommand = new RelayCommand(OnChangeJoinOperator);
		}

		public RelayCommand ChangeJoinOperatorCommand { get; private set; }
		void OnChangeJoinOperator()
		{
			JoinOperator = JoinOperator == JoinOperator.And ? JoinOperator.Or : JoinOperator.And;
		}

		public JoinOperator JoinOperator
		{
			get { return ConditionArguments.JoinOperator; }
			set
			{
				ConditionArguments.JoinOperator = value;
				OnPropertyChanged(()=>JoinOperator);
				if (UpdateDescriptionHandler != null)
					UpdateDescriptionHandler();
				ServiceFactory.SaveService.AutomationChanged = true;
			}
		}

		public RelayCommand AddCommand { get; private set; }
		public void OnAdd()
		{
			var condition = new Condition();
			var conditionViewModel = new ConditionViewModel(condition, Procedure ,UpdateDescriptionHandler);
			ConditionArguments.Conditions.Add(condition);
			Conditions.Add(conditionViewModel);
			if (UpdateDescriptionHandler != null)
				UpdateDescriptionHandler();
			ServiceFactory.SaveService.AutomationChanged = true;
		}

		public RelayCommand<ConditionViewModel> RemoveCommand { get; private set; }
		void OnRemove(ConditionViewModel conditionViewModel)
		{
			Conditions.Remove(conditionViewModel);
			ConditionArguments.Conditions.Remove(conditionViewModel.Condition);
			if (UpdateDescriptionHandler != null)
				UpdateDescriptionHandler();
			ServiceFactory.SaveService.AutomationChanged = true;
		}

		bool CanRemove(ConditionViewModel conditionViewModel)
		{
			return Conditions.Count > 1;
		}

		public void UpdateContent()
		{
			foreach (var conditionViewModel in Conditions)
			{
				conditionViewModel.UpdateContent();
			}
		}

		public string Description
		{
			get
			{
				var conditionViewModel = Conditions.FirstOrDefault();
				if (conditionViewModel == null)
					return "";

				string var1 = conditionViewModel.Variable1.DescriptionValue;
				if (String.IsNullOrEmpty(var1))
					var1 = "пусто";
				string var2 = conditionViewModel.Variable2.DescriptionValue;
				if (String.IsNullOrEmpty(var2))
					var2 = "пусто";
				var op = "";
				switch (conditionViewModel.SelectedConditionType)
				{
					case ConditionType.IsEqual:
						op = "==";
						break;
					case ConditionType.IsLess:
						op = "<";
						break;
					case ConditionType.IsMore:
						op = ">";
						break;
					case ConditionType.IsNotEqual:
						op = "!=";
						break;
					case ConditionType.IsNotLess:
						op = "≥";
						break;
					case ConditionType.IsNotMore:
						op = "≤";
						break;
				}
				var end = "";
				if (Conditions.Count > 1)
					end = JoinOperator == JoinOperator.And ? "и ..." : "или ...";

				return var1 + " " + op + " " + var2 + " " + end;
			}
		}
	}

	public class ConditionViewModel : BaseViewModel
	{
		public Condition Condition { get; private set; }
		public ArithmeticParameterViewModel Variable1 { get; set; }
		public ArithmeticParameterViewModel Variable2 { get; set; }
		Procedure Procedure { get; set; }
		public Action UpdateDescriptionHandler { get; set; }

		public ConditionViewModel(Condition condition, Procedure procedure, Action updateDescriptionHandler)
		{
			Condition = condition;
			Procedure = procedure;
			var variablesAndArguments = new List<Variable>(Procedure.Variables);
			variablesAndArguments.AddRange(Procedure.Arguments);
			var variableTypes = new List<VariableType> { VariableType.IsGlobalVariable, VariableType.IsLocalVariable };
			ConditionValueTypes = new ObservableCollection<ValueType>(Enum.GetValues(typeof(ValueType)).Cast<ValueType>().ToList());
			Variable1 = new ArithmeticParameterViewModel(Condition.Variable1, variableTypes);
			Variable1.UpdateDescriptionHandler = updateDescriptionHandler;
			variableTypes.Add(VariableType.IsValue);
			Variable2 = new ArithmeticParameterViewModel(Condition.Variable2, variableTypes);
			Variable2.UpdateDescriptionHandler = updateDescriptionHandler;
			SelectedConditionValueType = Condition.ConditionValueType;
		}

		public ObservableCollection<ValueType> ConditionValueTypes { get; private set; }
		public ValueType SelectedConditionValueType
		{
			get { return Condition.ConditionValueType; }
			set
			{
				Condition.ConditionValueType = value;
				ConditionTypes = new ObservableCollection<ConditionType>();
				if ((value == ValueType.String)||(value == ValueType.Boolean))
					ConditionTypes = new ObservableCollection<ConditionType> { ConditionType.IsEqual, ConditionType.IsNotEqual };
				if ((value == ValueType.Integer)||(value == ValueType.DateTime))
					ConditionTypes = new ObservableCollection<ConditionType>(Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().ToList());
				if (UpdateDescriptionHandler != null)
					UpdateDescriptionHandler();
				OnPropertyChanged(() => ConditionTypes);
				UpdateContent();
				ServiceFactory.SaveService.AutomationChanged = true;
				OnPropertyChanged(() => SelectedConditionValueType);
			}
		}

		public void UpdateContent()
		{
			var allVariables = ProcedureHelper.GetAllVariables(Procedure);
			allVariables = allVariables.FindAll(x => (x.ValueType != ValueType.Object) && !x.IsList);

			if (SelectedConditionValueType == ValueType.Boolean)
			{
				allVariables = allVariables.FindAll(x => x.ValueType == ValueType.Boolean);
			}
			if (SelectedConditionValueType == ValueType.Integer)
			{
				allVariables = allVariables.FindAll(x => x.ValueType == ValueType.Integer);
			}
			if (SelectedConditionValueType == ValueType.DateTime)
			{
				allVariables = allVariables.FindAll(x => x.ValueType == ValueType.DateTime);
			}
			if (SelectedConditionValueType == ValueType.String)
			{
				allVariables = allVariables.FindAll(x => x.ValueType == ValueType.String);
			}
			Variable1.Update(allVariables);
			Variable2.Update(allVariables);
			SelectedConditionType = ConditionTypes.Contains(Condition.ConditionType) ? Condition.ConditionType : ConditionTypes.FirstOrDefault();
		}

		public ObservableCollection<ConditionType> ConditionTypes { get; private set; }
		public ConditionType SelectedConditionType
		{
			get { return Condition.ConditionType; }
			set
			{
				Condition.ConditionType = value;
				OnPropertyChanged(() => SelectedConditionType);
				if (UpdateDescriptionHandler != null)
					UpdateDescriptionHandler();
				ServiceFactory.SaveService.AutomationChanged = true;
			}
		}
	}
}
