﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKDDriver;
using FiresecAPI.SKD.ReportFilters;
using FiresecAPI.SKD;
using FiresecService.Report.Model;
using FiresecAPI;

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

        public Dictionary<Guid, DeletableObjectInfo> Organisations { get; private set; }
        public Dictionary<Guid, OrganisationBaseObjectInfo> Departments { get; private set; }
        public Dictionary<Guid, OrganisationBaseObjectInfo> Positions { get; private set; }

        public void LoadCache()
        {
            if (IsCacheLoaded)
                return;

            var organisationResult = DatabaseService.OrganisationTranslator.Get(new OrganisationFilter() { LogicalDeletationType = LogicalDeletationType.All });
            Organisations = CreateDictionary(organisationResult, orgnisation => new DeletableObjectInfo()
            {
                IsDeleted = orgnisation.IsDeleted,
                Name = orgnisation.Name,
                UID = orgnisation.UID,
            });

            var departmentResult = DatabaseService.DepartmentTranslator.Get(new DepartmentFilter() { LogicalDeletationType = LogicalDeletationType.All });
            Departments = CreateDictionary(departmentResult, department => new OrganisationBaseObjectInfo()
            {
                IsDeleted = department.IsDeleted,
                Name = department.Name,
                Organisation = Organisations[department.OrganisationUID].Name,
                OrganisationUID = department.OrganisationUID,
                UID = department.UID,
            });

            var positionResult = DatabaseService.PositionTranslator.Get(new PositionFilter() { LogicalDeletationType = LogicalDeletationType.All });
            Positions = CreateDictionary(positionResult, position => new OrganisationBaseObjectInfo()
            {
                IsDeleted = position.IsDeleted,
                Name = position.Name,
                Organisation = Organisations[position.OrganisationUID].Name,
                OrganisationUID = position.OrganisationUID,
                UID = position.UID,
            });
        }
        public List<EmployeeInfo> GetEmployees(SKDReportFilter filter)
        {
            LoadCache();
            var list = new List<EmployeeInfo>();
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
                employeeFilter.UIDs = ((IReportFilterEmployee)filter).Employees ?? new List<Guid>();
            var employeesResult = DatabaseService.EmployeeTranslator.Get(employeeFilter);
            if (employeesResult == null || employeesResult.Result == null)
                return new List<EmployeeInfo>();
            return employeesResult.Result.Select(employee => new EmployeeInfo()
            {
                Department = employee.Department == null ? null : employee.Department.Name,
                DepartmentUID = employee.Department == null ? (Guid?)null : employee.Department.UID,
                IsDeleted = employee.IsDeleted,
                Name = employee.Name,
                Organisation = Organisations[employee.OrganisationUID].Name,
                OrganisationUID = employee.OrganisationUID,
                Position = employee.Position == null ? null : employee.Position.Name,
                PositionUID = employee.Position == null ? (Guid?)null : employee.Position.UID,
                UID = employee.UID,
            }).ToList();
        }

        #region IDisposable Members

        public void Dispose()
        {
            DatabaseService.Dispose();
        }

        #endregion

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
