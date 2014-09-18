﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FiresecAPI;
using FiresecAPI.SKD;
using LinqKit;

namespace SKDDriver
{
	public class OrganisationTranslator : IsDeletedTranslator<DataAccess.Organisation, Organisation, OrganisationFilter>
	{
		public OrganisationTranslator(SKDDatabaseService databaseService)
			: base(databaseService)
		{
		}

		protected OperationResult CanSave(OrganisationDetails item)
		{
			bool hasSameName = Table.Any(x => x.Name == item.Name && !x.IsDeleted && x.UID != item.UID);
			if (hasSameName)
				return new OperationResult("Организация таким же именем уже содержится в базе данных");
			return new OperationResult();
		}

		protected override OperationResult CanDelete(Guid uid)
		{
			if (Context.Employees.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней сотрудники");

			if (Context.Departments.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней отделы");

			if (Context.Positions.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней должности");

			if (Context.AccessTemplates.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней шаблоны доступа");

			if (Context.AdditionalColumnTypes.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней дополнительные колонки");

			if (Context.DayIntervals.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней дневные графики");

			if (Context.Holidays.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней праздники");

			if (Context.Schedules.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней графики работ");

			if (Context.ScheduleSchemes.Any(x => x.OrganisationUID == uid && !x.IsDeleted))
				return new OperationResult("Организация не может быть удалена, пока существуют привязанные к ней схемы работ");

			return base.CanDelete(uid);
		}

		protected override Organisation Translate(DataAccess.Organisation tableItem)
		{
			var result = base.Translate(tableItem);
			result.Name = tableItem.Name;
			result.Description = tableItem.Description;
			result.PhotoUID = tableItem.PhotoUID;
			result.DoorUIDs = (from x in Context.OrganisationDoors.Where(x => x.OrganisationUID == result.UID) select x.DoorUID).ToList();
			result.ZoneUIDs = (from x in Context.OrganisationZones.Where(x => x.OrganisationUID == result.UID) select x.ZoneUID).ToList();
			result.UserUIDs = (from x in Context.OrganisationUsers.Where(x => x.OrganisationUID == result.UID) select x.UserUID).ToList();
			result.GuardZoneUIDs = (from x in Context.GuardZones.Where(x => x.ParentUID == result.UID) select x.ZoneUID).ToList();
			result.ChiefUID = tableItem.ChiefUID;
			return result;
		}

		protected override void TranslateBack(DataAccess.Organisation tableItem, Organisation apiItem)
		{
			base.TranslateBack(tableItem, apiItem);
			tableItem.Name = apiItem.Name;
			tableItem.Description = apiItem.Description;
			tableItem.ChiefUID = apiItem.ChiefUID;
		}

		public OperationResult<OrganisationDetails> GetDetails(Guid uid)
		{
			try
			{
				var result = new OperationResult<OrganisationDetails>();
				var tableItem = Table.Where(x => x.UID.Equals(uid)).FirstOrDefault();
				if (tableItem == null)
					return result;
				var organisationDetails = new OrganisationDetails
					{
						Description = tableItem.Description,
						IsDeleted = tableItem.IsDeleted,
						Name = tableItem.Name,
						Photo = DatabaseService.PhotoTranslator.GetSingle(tableItem.PhotoUID).Result,
						RemovalDate = tableItem.RemovalDate,
						UID = tableItem.UID,
						DoorUIDs = (from x in Context.OrganisationDoors.Where(x => x.OrganisationUID == tableItem.UID) select x.DoorUID).ToList(),
						ZoneUIDs = (from x in Context.OrganisationZones.Where(x => x.OrganisationUID == tableItem.UID) select x.ZoneUID).ToList(),
						UserUIDs = (from x in Context.OrganisationUsers.Where(x => x.OrganisationUID == tableItem.UID) select x.UserUID).ToList(),
						GuardZoneUIDs = (from x in Context.GuardZones.Where(x => x.ParentUID == tableItem.UID) select x.ZoneUID).ToList(),
						ChiefUID = tableItem.ChiefUID
					};
				var photoResult = DatabaseService.PhotoTranslator.GetSingle(tableItem.PhotoUID);
				if (photoResult.HasError)
				{
					result.Error = photoResult.Error;
					return result;
				}
				organisationDetails.Photo = photoResult.Result;
				result.Result = organisationDetails;
				return result;
			}
			catch (Exception e)
			{
				return new OperationResult<OrganisationDetails>(e.Message);
			}
		}

		public OperationResult SaveDoors(Organisation apiItem)
		{
			return SaveDoorsInternal(apiItem.UID, apiItem.DoorUIDs);
		}

		public OperationResult SaveDoors(OrganisationDetails apiItem)
		{
			return SaveDoorsInternal(apiItem.UID, apiItem.DoorUIDs);
		}

		OperationResult SaveDoorsInternal(Guid organisationUID, List<Guid> doorUIDs)
		{
			try
			{
				var tableOrganisationZones = Context.OrganisationDoors.Where(x => x.OrganisationUID == organisationUID);
				Context.OrganisationDoors.DeleteAllOnSubmit(tableOrganisationZones);
				foreach (var zoneUID in doorUIDs)
				{
					var tableOrganisationZone = new DataAccess.OrganisationDoor();
					tableOrganisationZone.UID = Guid.NewGuid();
					tableOrganisationZone.OrganisationUID = organisationUID;
					tableOrganisationZone.DoorUID = zoneUID;
					Context.OrganisationDoors.InsertOnSubmit(tableOrganisationZone);
				}
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public OperationResult SaveZones(Organisation apiItem)
		{
			return SaveZonesInternal(apiItem.UID, apiItem.ZoneUIDs);
		}

		public OperationResult SaveZones(OrganisationDetails apiItem)
		{
			return SaveZonesInternal(apiItem.UID, apiItem.ZoneUIDs);
		}

		OperationResult SaveZonesInternal(Guid organisationUID, List<Guid> ZoneUIDs)
		{
			try
			{
				var tableOrganisationZones = Context.OrganisationZones.Where(x => x.OrganisationUID == organisationUID);
				Context.OrganisationZones.DeleteAllOnSubmit(tableOrganisationZones);
				foreach (var zoneUID in ZoneUIDs)
				{
					var tableOrganisationZone = new DataAccess.OrganisationZone();
					tableOrganisationZone.UID = Guid.NewGuid();
					tableOrganisationZone.OrganisationUID = organisationUID;
					tableOrganisationZone.ZoneUID = zoneUID;
					Context.OrganisationZones.InsertOnSubmit(tableOrganisationZone);
				}
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public OperationResult SaveGuardZones(Organisation apiItem)
		{
			return SaveGuardZonesInternal(apiItem.UID, apiItem.GuardZoneUIDs);
		}

		public OperationResult SaveGuardZones(OrganisationDetails apiItem)
		{
			return SaveGuardZonesInternal(apiItem.UID, apiItem.GuardZoneUIDs);
		}

		OperationResult SaveGuardZonesInternal(Guid organisationUID, List<Guid> GuardZoneUIDs)
		{
			try
			{
				var tableOrganisationGuardZones = Context.GuardZones.Where(x => x.ParentUID == organisationUID);
				Context.GuardZones.DeleteAllOnSubmit(tableOrganisationGuardZones);
				foreach (var GuardZoneUID in GuardZoneUIDs)
				{
					var tableOrganisationGuardZone = new DataAccess.GuardZone();
					tableOrganisationGuardZone.UID = Guid.NewGuid();
					tableOrganisationGuardZone.ParentUID = organisationUID;
					tableOrganisationGuardZone.ZoneUID = GuardZoneUID;
					Context.GuardZones.InsertOnSubmit(tableOrganisationGuardZone);
				}
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public OperationResult SaveUsers(Organisation apiItem)
		{
			return SaveUsersInternal(apiItem.UID, apiItem.UserUIDs);
		}

		public OperationResult SaveUsers(OrganisationDetails apiItem)
		{
			return SaveUsersInternal(apiItem.UID, apiItem.UserUIDs);
		}

		OperationResult SaveUsersInternal(Guid organisationUID, List<Guid> UserUIDs)
		{
			try
			{
				var tableOrganisationUsers = Context.OrganisationUsers.Where(x => x.OrganisationUID == organisationUID);
				Context.OrganisationUsers.DeleteAllOnSubmit(tableOrganisationUsers);
				foreach (var UserUID in UserUIDs)
				{
					var tableOrganisationUser = new DataAccess.OrganisationUser();
					tableOrganisationUser.UID = Guid.NewGuid();
					tableOrganisationUser.OrganisationUID = organisationUID;
					tableOrganisationUser.UserUID = UserUID;
					Context.OrganisationUsers.InsertOnSubmit(tableOrganisationUser);
				}
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public OperationResult SaveChief(Guid uid, Guid chiefUID)
		{
			try
			{
				var tableItem = Table.FirstOrDefault(x => x.UID == uid);
				var employee = Context.Employees.FirstOrDefault(x => x.UID == chiefUID);
				//tableItem.ChiefUID = chiefUID;
				tableItem.Employee = employee;
				Table.Context.SubmitChanges();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
			return new OperationResult();
		}

		public OperationResult Save(OrganisationDetails apiItem)
		{
			var saveDoorsResult = SaveDoors(apiItem);
			if (saveDoorsResult.HasError)
				return saveDoorsResult;
			var saveZonesResult = SaveZones(apiItem);
			if (saveZonesResult.HasError)
				return saveZonesResult;
			var saveGuardZonesResult = SaveGuardZones(apiItem);
			if (saveGuardZonesResult.HasError)
				return saveGuardZonesResult;
			var saveUsersResult = SaveUsers(apiItem);
			if (saveUsersResult.HasError)
				return saveUsersResult;
			if (apiItem.Photo != null && apiItem.Photo.Data != null && apiItem.Photo.Data.Count() > 0)
			{
				var savePhotoResult = DatabaseService.PhotoTranslator.Save(apiItem.Photo);
				if (savePhotoResult.HasError)
					return savePhotoResult;
			}
			try
			{
				if (apiItem == null)
					return new OperationResult("Попытка сохранить пустую запись");
				var verifyResult = CanSave(apiItem);
				if (verifyResult.HasError)
					return verifyResult;
				var tableItem = (from x in Table where x.UID.Equals(apiItem.UID) select x).FirstOrDefault();
				if (tableItem == null)
				{
					tableItem = new DataAccess.Organisation();
					tableItem.UID = apiItem.UID;
					tableItem.Name = apiItem.Name;
					tableItem.Description = apiItem.Description;
					if (apiItem.Photo != null)
						tableItem.PhotoUID = apiItem.Photo.UID;
					tableItem.IsDeleted = apiItem.IsDeleted;
					tableItem.RemovalDate = CheckDate(apiItem.RemovalDate);
					Table.InsertOnSubmit(tableItem);
				}
				else
				{
					tableItem.Name = apiItem.Name;
					tableItem.Description = apiItem.Description;
					if (apiItem.Photo != null)
						tableItem.PhotoUID = apiItem.Photo.UID;
					tableItem.IsDeleted = apiItem.IsDeleted;
					tableItem.RemovalDate = CheckDate(apiItem.RemovalDate);
				}
				Context.SubmitChanges();
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		protected override Expression<Func<DataAccess.Organisation, bool>> IsInFilter(OrganisationFilter filter)
		{
			var result = base.IsInFilter(filter);
			if (filter.UserUID != Guid.Empty)
				result = result.And(e => e.OrganisationUsers.Any(x => x.UserUID == filter.UserUID));
			return result;
		}
	}
}