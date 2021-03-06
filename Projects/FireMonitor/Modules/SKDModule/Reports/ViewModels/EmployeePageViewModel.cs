﻿using System;
using System.Collections.Generic;
using FiresecAPI.SKD;
using FiresecAPI.SKD.ReportFilters;
using Infrastructure.Common.SKDReports;
using SKDModule.ViewModels;

namespace SKDModule.Reports.ViewModels
{
	public class EmployeePageViewModel : FilterContainerViewModel
	{
		public EmployeePageViewModel()
		{
			Title = "Сотрудник";
			Filter = new EmployeesFilterViewModel();
		}

		public EmployeesFilterViewModel Filter { get; private set; }
		private bool _allowVisitor;
		public bool AllowVisitor
		{
			get { return _allowVisitor; }
			set
			{
				_allowVisitor = value;
				OnPropertyChanged(() => AllowVisitor);
			}
		}
		private bool _isEmployee;
		public bool IsEmployee
		{
			get { return _isEmployee; }
			set
			{
				_isEmployee = value;
				OnPropertyChanged(() => IsEmployee);
				if (AllowVisitor)
					Filter.Initialize(new List<Guid>(), FiresecAPI.SKD.LogicalDeletationType.Active, IsEmployee ? PersonType.Employee : PersonType.Guest);
			}
		}

		public override void LoadFilter(SKDReportFilter filter)
		{
			var employeeFilter = filter as IReportFilterEmployee;
			if (employeeFilter == null)
				return;
			AllowVisitor = employeeFilter is IReportFilterEmployeeAndVisitor;
			_isEmployee = AllowVisitor ? ((IReportFilterEmployeeAndVisitor)employeeFilter).IsEmployee : true;
			OnPropertyChanged(() => IsEmployee);
			var ef = new EmployeeFilter()
			{
				PersonType = IsEmployee ? PersonType.Employee : PersonType.Guest,
				LogicalDeletationType = LogicalDeletationType.Active,
			};
			if (employeeFilter.IsSearch)
			{
				ef.LastName = employeeFilter.LastName;
				ef.FirstName = employeeFilter.FirstName;
				ef.SecondName = employeeFilter.SecondName;
			}
			else
				ef.UIDs = employeeFilter.Employees;
			Filter.Initialize(ef);
		}
		public override void UpdateFilter(SKDReportFilter filter)
		{
			var employeeFilter = filter as IReportFilterEmployee;
			if (employeeFilter == null)
				return;
			employeeFilter.Employees = Filter.UIDs;
			employeeFilter.IsSearch = Filter.IsSearch;
			if (Filter.IsSearch)
			{
				employeeFilter.LastName = Filter.LastName;
				employeeFilter.FirstName = Filter.FirstName;
				employeeFilter.SecondName = Filter.SecondName;
			}
			if (AllowVisitor)
				((IReportFilterEmployeeAndVisitor)employeeFilter).IsEmployee = IsEmployee;
		}
	}
}