﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI;
using FiresecAPI.SKD;
using FiresecAPI.SKD.ReportFilters;
using FiresecService.Report.Model;
using SKDDriver;
using Common;

namespace FiresecService.Report
{
	public class DataProvider : IDisposable
	{
		public DataProvider()
		{
			DatabaseService = new SKDDatabaseService();
			IsCacheLoaded = false;
		}

		public SKDDatabaseService DatabaseService { get; private set; }
		public bool IsCacheLoaded { get; private set; }

		public Dictionary<Guid, DeletableObjectInfo<Organisation>> Organisations { get; private set; }
		public Dictionary<Guid, OrganisationBaseObjectInfo<Department>> Departments { get; private set; }
		public Dictionary<Guid, OrganisationBaseObjectInfo<Position>> Positions { get; private set; }
		private Dictionary<Guid, EmployeeInfo> _employees;

		public void LoadCache()
		{
			if (IsCacheLoaded)
				return;

			_employees = new Dictionary<Guid, EmployeeInfo>();

			var organisationResult = DatabaseService.OrganisationTranslator.Get(new OrganisationFilter() { LogicalDeletationType = LogicalDeletationType.All });
			Organisations = CreateDictionary(organisationResult, organisation => new DeletableObjectInfo<Organisation>()
			{
				IsDeleted = organisation.IsDeleted,
				Name = organisation.Name,
				UID = organisation.UID,
				Item = organisation,
			});

			var departmentResult = DatabaseService.DepartmentTranslator.Get(new DepartmentFilter() { LogicalDeletationType = LogicalDeletationType.All });
			Departments = CreateDictionary(departmentResult, department => new OrganisationBaseObjectInfo<Department>()
			{
				IsDeleted = department.IsDeleted,
				Name = department.Name,
				Organisation = Organisations[department.OrganisationUID].Name,
				OrganisationUID = department.OrganisationUID,
				UID = department.UID,
				Item = department,
			});

			var positionResult = DatabaseService.PositionTranslator.Get(new PositionFilter() { LogicalDeletationType = LogicalDeletationType.All });
			Positions = CreateDictionary(positionResult, position => new OrganisationBaseObjectInfo<Position>()
			{
				IsDeleted = position.IsDeleted,
				Name = position.Name,
				Organisation = Organisations[position.OrganisationUID].Name,
				OrganisationUID = position.OrganisationUID,
				UID = position.UID,
				Item = position,
			});
		}
		public EmployeeInfo GetEmployee(Guid uid)
		{
			if (!_employees.ContainsKey(uid))
			{
				var result = DatabaseService.EmployeeTranslator.GetSingle(uid);
				_employees.Add(uid, result == null ? null : ConvertEmployee(result.Result));
			}
			return _employees[uid];
		}
		public List<EmployeeInfo> GetEmployees(IEnumerable<Guid> uids)
		{
			if (uids.IsEmpty())
				return new List<EmployeeInfo>();
			LoadCache();
			var employeeFilter = new EmployeeFilter()
			{
				UIDs = uids.ToList(),
				LogicalDeletationType = LogicalDeletationType.All,
			};
			var employeesResult = DatabaseService.EmployeeTranslator.Get(employeeFilter);
			if (employeesResult == null || employeesResult.Result == null)
				return new List<EmployeeInfo>();
			var employees = employeesResult.Result.Select(ConvertEmployee).ToList();
			employees.ForEach(employee =>
			{
				if (_employees.ContainsKey(employee.UID))
					_employees[employee.UID] = employee;
				else
					_employees.Add(employee.UID, employee);
			});
			return employees;
		}
		public List<EmployeeInfo> GetEmployees(SKDReportFilter filter)
		{
			var list = new List<EmployeeInfo>();
			var employeeFilter = GetEmployeeFilter(filter);
            return GetEmployees(employeeFilter);
        }
        public List<EmployeeInfo> GetEmployees(EmployeeFilter employeeFilter)
        {
            LoadCache();
            var employeesResult = DatabaseService.EmployeeTranslator.Get(employeeFilter);
			if (employeesResult == null || employeesResult.Result == null)
				return new List<EmployeeInfo>();
			var employees = employeesResult.Result.Select(ConvertEmployee).ToList();
			employees.ForEach(employee =>
			{
				if (_employees.ContainsKey(employee.UID))
					_employees[employee.UID] = employee;
				else
					_employees.Add(employee.UID, employee);
			});
			return employees;
		}
		public EmployeeFilter GetEmployeeFilter(SKDReportFilter filter)
		{
			var employeeFilter = new EmployeeFilter();
			var withDeleted = filter is IReportFilterArchive ? ((IReportFilterArchive)filter).UseArchive : false;
			employeeFilter.LogicalDeletationType = withDeleted ? LogicalDeletationType.All : LogicalDeletationType.Active;
			employeeFilter.WithDeletedDepartments = withDeleted;
			employeeFilter.WithDeletedPositions = withDeleted;
			if (filter is IReportFilterOrganisation)
				employeeFilter.OrganisationUIDs = ((IReportFilterOrganisation)filter).Organisations ?? new List<Guid>();
			if (filter is IReportFilterDepartment)
				employeeFilter.DepartmentUIDs = ((IReportFilterDepartment)filter).Departments ?? new List<Guid>();
			if (filter is IReportFilterPosition)
				employeeFilter.PositionUIDs = ((IReportFilterPosition)filter).Positions ?? new List<Guid>();
			if (filter is IReportFilterEmployee)
			{
				var reportFilterEmployee = (IReportFilterEmployee)filter;
				if (reportFilterEmployee.IsSearch)
				{
					employeeFilter.LastName = reportFilterEmployee.LastName;
					employeeFilter.FirstName = reportFilterEmployee.FirstName;
					employeeFilter.SecondName = reportFilterEmployee.SecondName;
				}
				else
					employeeFilter.UIDs = reportFilterEmployee.Employees ?? new List<Guid>();
				if (filter is IReportFilterEmployeeAndVisitor)
					employeeFilter.PersonType = ((IReportFilterEmployeeAndVisitor)filter).IsEmployee ? PersonType.Employee : PersonType.Guest;
			}
			return employeeFilter;
		}
		public bool IsEmployeeFilter(SKDReportFilter filter)
		{
			if ((filter is IReportFilterOrganisation && !((IReportFilterOrganisation)filter).Organisations.IsEmpty()) ||
				(filter is IReportFilterDepartment && !((IReportFilterDepartment)filter).Departments.IsEmpty()) ||
				(filter is IReportFilterPosition && !((IReportFilterPosition)filter).Positions.IsEmpty()))
				return true;
			var employeeFilter = filter as IReportFilterEmployee;
			if (employeeFilter == null)
				return false;
			return (!employeeFilter.IsSearch && !employeeFilter.Employees.IsEmpty()) || (employeeFilter.IsSearch && (!employeeFilter.LastName.IsEmpty() || !employeeFilter.FirstName.IsEmpty() || !employeeFilter.SecondName.IsEmpty()));
		}

		#region IDisposable Members

		public void Dispose()
		{
			DatabaseService.Dispose();
		}

		#endregion

		private EmployeeInfo ConvertEmployee(Employee employee)
		{
			if (employee == null)
				return null;
			return new EmployeeInfo()
				 {
					 Department = employee.Department == null ? null : employee.Department.Name,
					 DepartmentUID = employee.Department == null ? (Guid?)null : employee.Department.UID,
					 IsDeleted = employee.IsDeleted,
					 Name = employee.FIO,
					 Organisation = Organisations[employee.OrganisationUID].Name,
					 OrganisationUID = employee.OrganisationUID,
					 Position = employee.Position == null ? null : employee.Position.Name,
					 PositionUID = employee.Position == null ? (Guid?)null : employee.Position.UID,
					 UID = employee.UID,
					 Item = employee,
				 };
		}
		private Dictionary<Guid, T> CreateDictionary<ApiT, T>(OperationResult<IEnumerable<ApiT>> result, Converter<ApiT, T> converter)
			where T : ObjectInfo
			where ApiT : SKDModelBase
		{
			if (result == null || result.Result == null)
				return new Dictionary<Guid, T>();
			return result.Result.ToDictionary(item => item.UID, item => converter(item));
		}
	}
}
