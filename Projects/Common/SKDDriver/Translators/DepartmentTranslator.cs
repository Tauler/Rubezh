﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI;
using FiresecAPI.SKD;

namespace SKDDriver
{
	public class DepartmentTranslator : EmployeeTranslatorBase<DataAccess.Department, Department, DepartmentFilter, ShortDepartment>
	{
		public DepartmentTranslator(SKDDatabaseService databaseService)
			: base(databaseService)
		{
			Synchroniser = new DepartmentSynchroniser(Table, DatabaseService);
		}

		protected override OperationResult CanSave(Department item)
		{
			var result = base.CanSave(item);
			if (result.HasError)
				return result;
			bool hasSameName = Table.Any(x => x.Name == item.Name &&
				x.OrganisationUID == item.OrganisationUID &&
				x.UID != item.UID &&
				x.ParentDepartmentUID == item.ParentDepartmentUID &&
				x.IsDeleted == false);
			if (hasSameName)
				return new OperationResult("Отдел с таким же названием уже содержится в базе данных");
			else
				return new OperationResult();
		}

		protected override OperationResult BeforeDelete(Guid uid, DateTime removalDate)
		{
			return MarkDeletedByParentWithSubmit(uid, removalDate);
		}

		OperationResult MarkDeletedByParent(Guid parentDepartmentUID, DateTime removalDate)
		{
			try
			{
				var databaseItems = Table.Where(x => !x.IsDeleted && x.ParentDepartmentUID == parentDepartmentUID);
				if (databaseItems != null)
				{
					foreach (var item in databaseItems)
					{
						item.IsDeleted = true;
						item.RemovalDate = removalDate;
						var result = MarkDeletedByParent(item.UID, removalDate);
						if (result.HasError)
							return result;
					}
				}
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		OperationResult MarkDeletedByParentWithSubmit(Guid uid, DateTime removalDate)
		{
			try
			{
				var result = MarkDeletedByParent(uid, removalDate);
				if (result.HasError)
					return result;
				Context.SubmitChanges();
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		protected override OperationResult BeforeRestore(Guid uid, DateTime removalDate)
		{
			return RestoreByChildWithSubmit(uid, removalDate);
		}

		OperationResult RestoreByChild(Guid uid, DateTime removalDate)
		{
			try
			{
				var parent = Table.FirstOrDefault(x => x.Departments.Any(y => y.UID == uid));
				if (parent != null)
				{
					parent.IsDeleted = false;
					var result = RestoreByChild(parent.UID, removalDate);
					if (result.HasError)
						return result;
				}
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		OperationResult RestoreByChildWithSubmit(Guid uid, DateTime removalDate)
		{
			try
			{
				var result = RestoreByChild(uid, removalDate);
				if (result.HasError)
					return result;
				Context.SubmitChanges();
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}
 
		protected override Department Translate(DataAccess.Department tableItem)
		{
			var result = base.Translate(tableItem);

			var childDepartmentUIDs = new List<Guid>();
			foreach (var department in Context.Departments.Where(x => x.ParentDepartmentUID == tableItem.UID))
			{
				childDepartmentUIDs.Add(department.UID);
			}
			tableItem.Departments.ToList().ForEach(x => childDepartmentUIDs.Add(x.UID));
			result.Name = tableItem.Name;
			result.Description = tableItem.Description;
			result.ParentDepartmentUID = tableItem.ParentDepartmentUID;
			result.ChildDepartmentUIDs = childDepartmentUIDs;
			result.ContactEmployeeUID = tableItem.ContactEmployeeUID;
			result.AttendantEmployeeUID = tableItem.AttendantUID;
			result.Photo = GetResult(DatabaseService.PhotoTranslator.GetSingle(tableItem.PhotoUID));
			result.ChiefUID = tableItem.ChiefUID;
			result.Phone = tableItem.Phone;
			return result;
		}

		protected override void TranslateBack(DataAccess.Department tableItem, Department apiItem)
		{
			base.TranslateBack(tableItem, apiItem);
			tableItem.Name = apiItem.Name;
			tableItem.Description = apiItem.Description;
			tableItem.ParentDepartmentUID = apiItem.ParentDepartmentUID;
			tableItem.ContactEmployeeUID = apiItem.ContactEmployeeUID;
			tableItem.AttendantUID = apiItem.AttendantEmployeeUID;
			if(apiItem.Photo != null)
				tableItem.PhotoUID = apiItem.Photo.UID;
			tableItem.ChiefUID = apiItem.ChiefUID;
			tableItem.Phone = apiItem.Phone;
			if (tableItem.ExternalKey == null)
				tableItem.ExternalKey = "-1";
		}

		protected override ShortDepartment TranslateToShort(DataAccess.Department tableItem)
		{
			var result = base.TranslateToShort(tableItem);
			result.Name = tableItem.Name;
			result.Description = tableItem.Description;
			result.ChiefUID = tableItem.ChiefUID;
			result.Phone = tableItem.Phone;
			result.ParentDepartmentUID = tableItem.ParentDepartmentUID;
			
			result.ChildDepartmentUIDs = new List<Guid>();
			foreach (var department in Context.Departments.Where(x => x.ParentDepartmentUID == tableItem.UID))
				result.ChildDepartmentUIDs.Add(department.UID);
			
			return result;
		}

		public string GetName(Guid? uid)
		{
			if (uid == null)
				return "";
			var tableItem = Table.FirstOrDefault(x => x.UID == uid.Value);
			if(tableItem == null)
				return "";
			return tableItem.Name;
		}

		public OperationResult SaveChief(Guid uid, Guid chiefUID)
		{
			try
			{
				var tableItem = Table.FirstOrDefault(x => x.UID == uid);
				tableItem.ChiefUID = chiefUID;
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public override OperationResult Save(Department apiItem)
		{
			if (apiItem.Photo != null && apiItem.Photo.Data != null && apiItem.Photo.Data.Count() > 0)
			{
				var photoSaveResult = DatabaseService.PhotoTranslator.Save(apiItem.Photo);
				if (photoSaveResult.HasError)
					return photoSaveResult;
			}
			return base.Save(apiItem);
		}

		protected override bool IsSimilarNames(DataAccess.Department item1, DataAccess.Department item2)
		{
			return base.IsSimilarNames(item1, item2) && item1.ParentDepartmentUID == item2.ParentDepartmentUID;
		}

		protected override Guid? GetLinkUID(DataAccess.Employee employee)
		{
			return employee.DepartmentUID;
		}

		public DepartmentSynchroniser Synchroniser;
	}
}