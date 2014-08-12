﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecAPI.SKD;
using LinqKit;
using SKDDriver.Translators;

namespace SKDDriver
{
	public class EmployeeTranslator : WithShortTranslator<DataAccess.Employee, Employee, EmployeeFilter, ShortEmployee>
	{
		public EmployeeTranslator(DataAccess.SKDDataContext context,
			PositionTranslator positionTranslator,
			DepartmentTranslator departmentTranslator,
			AdditionalColumnTranslator additionalColumnTranslator,
			CardTranslator cardTranslator,
			PhotoTranslator photoTranslator,
			ScheduleTranslator scheduleTranslator)
			: base(context)
		{
			PositionTranslator = positionTranslator;
			DepartmentTranslator = departmentTranslator;
			AdditionalColumnTranslator = additionalColumnTranslator;
			CardTranslator = cardTranslator;
			PhotoTranslator = photoTranslator;
			ScheduleTranslator = scheduleTranslator;
		}

		PositionTranslator PositionTranslator;
		DepartmentTranslator DepartmentTranslator;
		AdditionalColumnTranslator AdditionalColumnTranslator;
		CardTranslator CardTranslator;
		PhotoTranslator PhotoTranslator;
		ScheduleTranslator ScheduleTranslator;

		protected override OperationResult CanSave(Employee employee)
		{
			bool hasSameName = Table.Any(x => x.FirstName == employee.FirstName &&
				x.SecondName == employee.SecondName &&
				x.LastName == employee.LastName &&
				x.OrganisationUID == employee.OrganisationUID &&
				x.UID != employee.UID &&
				x.IsDeleted == false);
			if (hasSameName)
				return new OperationResult("Сотрудник с таким же ФИО уже содержится в базе данных");
			return base.CanSave(employee);
		}

		protected override OperationResult CanDelete(Guid uid)
		{
			bool isAttendant = Context.Departments.Any(x => !x.IsDeleted && x.AttendantUID == uid);
			if (isAttendant)
				return new OperationResult("Невозможно удалить сотрудника, пока он указан как сопровождающий для одного из отделов");

			bool isContactEmployee = Context.Departments.Any(x => !x.IsDeleted && x.ContactEmployeeUID == uid);
			if (isContactEmployee)
				return new OperationResult("Невозможно удалить сотрудника, пока он указан как контактное лицо для одного из отделов");
			return base.CanDelete(uid);
		}

		protected override Employee Translate(DataAccess.Employee tableItem)
		{
			var result = base.Translate(tableItem);
			result.FirstName = tableItem.FirstName;
			result.SecondName = tableItem.SecondName;
			result.LastName = tableItem.LastName;
			result.Appointed = tableItem.Appointed;
			result.Dismissed = tableItem.Dismissed;
			result.Department = DepartmentTranslator.GetSingleShort(tableItem.DepartmentUID);
			result.Schedule = ScheduleTranslator.GetSingleShort(tableItem.ScheduleUID);
			result.ScheduleStartDate = tableItem.ScheduleStartDate;
			result.AdditionalColumns = AdditionalColumnTranslator.GetAllByEmployee<DataAccess.AdditionalColumn>(tableItem.UID);
			result.Type = (PersonType)tableItem.Type;
			result.Cards = CardTranslator.GetByEmployee<DataAccess.Card>(tableItem.UID);
			result.Position = PositionTranslator.GetSingleShort(tableItem.PositionUID);
			result.Photo = GetResult(PhotoTranslator.GetSingle(tableItem.PhotoUID));
			result.TabelNo = tableItem.TabelNo;
			result.CredentialsStartDate = tableItem.CredentialsStartDate;
			result.EscortUID = tableItem.EscortUID;
			result.DocumentNumber = tableItem.DocumentNumber;
			result.BirthDate = tableItem.BirthDate;
			result.BirthPlace = tableItem.BirthPlace;
			result.DocumentGivenBy = tableItem.DocumentGivenBy;
			result.DocumentGivenDate = tableItem.DocumentGivenDate;
			result.DocumentValidTo = tableItem.DocumentValidTo;
			result.Gender = (Gender)tableItem.Gender;
			result.DocumentDepartmentCode = tableItem.DocumentDepartmentCode;
			result.Citizenship = tableItem.Citizenship;
			result.DocumentType = (EmployeeDocumentType)tableItem.DocumentType;
			var guardZones = (from x in Context.GuardZones.Where(x => x.ParentUID == tableItem.UID) select x);
			foreach (var item in guardZones)
			{
				result.GuardZoneAccesses.Add(new XGuardZoneAccess
					{
						ZoneUID = item.ZoneUID,
						CanReset = item.CanReset,
						CanSet = item.CanSet
					});
			}
			return result;
		}

		protected override ShortEmployee TranslateToShort(DataAccess.Employee tableItem)
		{
			var shortEmployee = new ShortEmployee
			{
				UID = tableItem.UID,
				FirstName = tableItem.FirstName,
				SecondName = tableItem.SecondName,
				LastName = tableItem.LastName,
				Cards = CardTranslator.GetByEmployee<DataAccess.Card>(tableItem.UID),
				Type = (PersonType)tableItem.Type,
				Appointed = tableItem.Appointed.ToString("d MMM yyyy"),
				Dismissed = tableItem.Dismissed.ToString("d MMM yyyy"),
				OrganisationUID = tableItem.OrganisationUID,
				TextColumns = AdditionalColumnTranslator.GetTextColumns(tableItem.UID)
			};
			var position = Context.Positions.FirstOrDefault(x => x.UID == tableItem.PositionUID);
			if (position != null)
				shortEmployee.PositionName = position.Name;
			var department = Context.Departments.FirstOrDefault(x => x.UID == tableItem.DepartmentUID);
			if (department != null)
				shortEmployee.DepartmentName = department.Name;
			return shortEmployee;
		}

		protected override void TranslateBack(DataAccess.Employee tableItem, Employee apiItem)
		{
			base.TranslateBack(tableItem, apiItem);
			tableItem.FirstName = apiItem.FirstName;
			tableItem.SecondName = apiItem.SecondName;
			tableItem.LastName = apiItem.LastName;
			tableItem.Appointed = CheckDate(apiItem.Appointed);
			tableItem.Dismissed = CheckDate(apiItem.Dismissed);
			if (apiItem.Position != null)
				tableItem.PositionUID = apiItem.Position.UID;
			if (apiItem.Department != null)
				tableItem.DepartmentUID = apiItem.Department.UID;
			if (apiItem.Schedule != null)
				tableItem.ScheduleUID = apiItem.Schedule.UID;
			tableItem.ScheduleStartDate = CheckDate(apiItem.ScheduleStartDate);
			if (apiItem.Photo != null)
				tableItem.PhotoUID = apiItem.Photo.UID;
			tableItem.Type = (int)apiItem.Type;
			tableItem.TabelNo = apiItem.TabelNo;
			tableItem.CredentialsStartDate = CheckDate(apiItem.CredentialsStartDate);
			tableItem.EscortUID = apiItem.EscortUID;
			tableItem.DocumentNumber = apiItem.DocumentNumber;
			tableItem.BirthDate = CheckDate(apiItem.BirthDate);
			tableItem.BirthPlace = apiItem.BirthPlace;
			tableItem.DocumentGivenBy = apiItem.DocumentGivenBy;
			tableItem.DocumentGivenDate = CheckDate(apiItem.DocumentGivenDate);
			tableItem.DocumentValidTo = CheckDate(apiItem.DocumentValidTo);
			tableItem.Gender = (int)apiItem.Gender;
			tableItem.DocumentDepartmentCode = apiItem.DocumentDepartmentCode;
			tableItem.Citizenship = apiItem.Citizenship;
			tableItem.DocumentType = (int)apiItem.DocumentType;
		}

		public override OperationResult Save(Employee apiItem)
		{
			var columnSaveResult = AdditionalColumnTranslator.Save(apiItem.AdditionalColumns);
			if (columnSaveResult.HasError)
				return columnSaveResult;
			var zoneSaveResult = SaveGuardZones(apiItem);
			if (zoneSaveResult.HasError)
				return zoneSaveResult;
			if (apiItem.Photo != null && apiItem.Photo.Data != null && apiItem.Photo.Data.Count() > 0)
			{
				var photoSaveResult = PhotoTranslator.Save(apiItem.Photo);
				if (photoSaveResult.HasError)
					return photoSaveResult;
			}
			return base.Save(apiItem);
		}

		protected override Expression<Func<DataAccess.Employee, bool>> IsInFilter(EmployeeFilter filter)
		{
			var result = base.IsInFilter(filter);
			result = result.And(e => e.Type == (int?)filter.PersonType);
			var departmentUIDs = filter.DepartmentUIDs;
			if (departmentUIDs.IsNotNullOrEmpty())
			{
				result = result.And(e => e != null);
			}

			var positionUIDs = filter.PositionUIDs;
			if (positionUIDs.IsNotNullOrEmpty())
				result = result.And(e => e != null && positionUIDs.Contains(e.PositionUID.Value));

			var appointedDates = filter.Appointed;
			if (appointedDates != null)
				result = result.And(e => e.Appointed >= appointedDates.StartDate && e.Appointed <= appointedDates.EndDate);

			var dismissedDates = filter.Dismissed;
			if (dismissedDates != null)
				result = result.And(e => e.Dismissed >= dismissedDates.StartDate && e.Dismissed <= dismissedDates.EndDate);

			if (!string.IsNullOrEmpty(filter.LastName))
				result = result.And(e => e.LastName.Contains(filter.LastName));

			if (!string.IsNullOrEmpty(filter.FirstName))
				result = result.And(e => e.FirstName.Contains(filter.FirstName));

			if (!string.IsNullOrEmpty(filter.SecondName))
				result = result.And(e => e.SecondName.Contains(filter.SecondName));

			if (filter.CardNo > 0)
			{
				result = result.And(e => Context.Cards.Any(x => x.Number == filter.CardNo && x.EmployeeUID == e.UID));
			}

			return result;
		}

		public OperationResult SaveGuardZones(Employee apiItem)
		{
			return SaveGuardZonesInternal(apiItem.UID, apiItem.GuardZoneAccesses);
		}

		OperationResult SaveGuardZonesInternal(Guid parentUID, List<XGuardZoneAccess> GuardZones)
		{
			try
			{
				var tableOrganisationGuardZones = Context.GuardZones.Where(x => x.ParentUID == parentUID);
				Context.GuardZones.DeleteAllOnSubmit(tableOrganisationGuardZones);
				foreach (var guardZone in GuardZones)
				{
					var tableOrganisationGuardZone = new DataAccess.GuardZone();
					tableOrganisationGuardZone.UID = Guid.NewGuid();
					tableOrganisationGuardZone.ParentUID = parentUID;
					tableOrganisationGuardZone.ZoneUID = guardZone.ZoneUID;
					tableOrganisationGuardZone.CanSet = guardZone.CanSet;
					tableOrganisationGuardZone.CanReset = guardZone.CanReset;
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

		public OperationResult AddPassJournal(Guid employeeUID, Guid zoneUID)
		{
			InvalidatePassJournal();

			try
			{
				var exitPassJournal = Context.PassJournals.FirstOrDefault(x => x.EmployeeUID == employeeUID && x.ExitTime == null);
				if (exitPassJournal != null)
				{
					exitPassJournal.ExitTime = DateTime.Now;
				}
				var enterPassJournal = new DataAccess.PassJournal();
				enterPassJournal.UID = Guid.NewGuid();
				enterPassJournal.EmployeeUID = employeeUID;
				enterPassJournal.ZoneUID = zoneUID;
				enterPassJournal.EnterTime = DateTime.Now;
				enterPassJournal.ExitTime = null;
				Context.PassJournals.InsertOnSubmit(enterPassJournal);
				Context.SubmitChanges();
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		void InvalidatePassJournal()
		{
			try
			{
				var hasChanges = false;
				var emptyExitPassJournals = Context.PassJournals.Where(x => x.ExitTime == null);
				foreach (var emptyExitPassJournal in emptyExitPassJournals)
				{
					var enterTime = emptyExitPassJournal.EnterTime.Value;
					var nowTime = DateTime.Now;
					if (nowTime.Date > enterTime.Date)
					{
						emptyExitPassJournal.EnterTime = new DateTime(enterTime.Year, enterTime.Month, enterTime.Day, 23, 59, 59);
						hasChanges = true;
					}
				}
				if (hasChanges)
				{
					Context.SubmitChanges();
				}
			}
			catch {}
		}

		public void InsertPassJournalTestData()
		{
			var employeeUID = GetList(new EmployeeFilter()).Result.FirstOrDefault().UID;
			var zoneUID = SKDManager.Zones.FirstOrDefault().UID;

			var random = new Random();
			for (int day = 0; day < 100; day++)
			{
				var dateTime = DateTime.Now.AddDays(-day);

				var seconds = new List<int>();
				var count = random.Next(0, 5);
				for (int i = 0; i < count * 2; i++)
				{
					var totalSeconds = random.Next(0, 24 * 60 * 60);
					seconds.Add(totalSeconds);
				}
				seconds.Sort();

				for (int i = 0; i < count * 2; i += 2)
				{
					var startTimeSpan = TimeSpan.FromSeconds(seconds[i]);
					var endTimeSpan = TimeSpan.FromSeconds(seconds[i + 1]);

					var passJournal = new DataAccess.PassJournal();
					passJournal.UID = Guid.NewGuid();
					passJournal.EmployeeUID = employeeUID;
					passJournal.ZoneUID = zoneUID;
					passJournal.EnterTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, startTimeSpan.Hours, startTimeSpan.Minutes, startTimeSpan.Seconds);
					passJournal.ExitTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, endTimeSpan.Hours, endTimeSpan.Minutes, endTimeSpan.Seconds);
					Context.PassJournals.InsertOnSubmit(passJournal);
				}
			}

			Context.SubmitChanges();
		}

		public OperationResult<List<DayTimeTrack>> GetTimeTracks(Guid employeeUID, DateTime startDate, DateTime endDate)
		{
			//InsertPassJournalTestData();

			InvalidatePassJournal();

			try
			{
				var timeTracks = new List<DayTimeTrack>();
				for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
				{
					timeTracks.Add(GetTimeTrack(employeeUID, date));
				}
				return new OperationResult<List<DayTimeTrack>> { Result = timeTracks };
			}
			catch (Exception e)
			{
				return new OperationResult<List<DayTimeTrack>>(e.Message);
			}
		}

		DayTimeTrack GetTimeTrack(Guid employeeUID, DateTime date)
		{
			var passJournals = Context.PassJournals.Where(x => x.EmployeeUID == employeeUID && x.EnterTime != null && x.EnterTime.Value.Date == date.Date).ToList();
			if (passJournals == null)
				passJournals = new List<DataAccess.PassJournal>();

			var employee = Table.FirstOrDefault(x => x.UID == employeeUID);
			if (employee == null)
				return new DayTimeTrack("Не найден сотрудник");
			var schedule = Context.Schedules.FirstOrDefault(x => x.UID == employee.ScheduleUID);
			if (schedule == null)
				return new DayTimeTrack("Не найден график");
			var scheduleScheme = Context.ScheduleSchemes.FirstOrDefault(x => x.UID == schedule.ScheduleSchemeUID.Value);
			if (scheduleScheme == null)
				return new DayTimeTrack("Не найдена схема работы");
			var scheduleSchemeType = (FiresecAPI.EmployeeTimeIntervals.ScheduleSchemeType)scheduleScheme.Type;

			var days = Context.Days.Where(x => x.ScheduleSchemeUID == scheduleScheme.UID);
			if (days == null || days.Count() == 0)
				return new DayTimeTrack();
			int dayNo = -1;

			switch (scheduleSchemeType)
			{
				case FiresecAPI.EmployeeTimeIntervals.ScheduleSchemeType.Week:
					dayNo = (int)date.DayOfWeek - 1;
					if (dayNo == -1)
						dayNo = 6;
					break;
				case FiresecAPI.EmployeeTimeIntervals.ScheduleSchemeType.SlideDay:
					var daysCount = days.Count();
					var period = new TimeSpan(date.Ticks - employee.ScheduleStartDate.Ticks);
					dayNo = (int)Math.IEEERemainder((int)period.TotalDays, daysCount);
					break;
				case FiresecAPI.EmployeeTimeIntervals.ScheduleSchemeType.Month:
					dayNo = (int)date.Day;
					break;
			}
			var day = days.FirstOrDefault(x => x.Number == dayNo);
			if (day == null)
				return new DayTimeTrack("Не найден день");

			List<DataAccess.Interval> intervals = new List<DataAccess.Interval>();
			if (day.NamedIntervalUID != null)
			{
				var namedInterval = Context.NamedIntervals.FirstOrDefault(x => x.UID == day.NamedIntervalUID);
				if (namedInterval == null)
					return new DayTimeTrack("Не найден именованный интервал");
				intervals = Context.Intervals.Where(x => x.NamedIntervalUID == namedInterval.UID).ToList();
			}
			var scheduleZones = Context.ScheduleZones.Where(x => x.ScheduleUID == schedule.UID).ToList();

			var dayTimeTrack = new DayTimeTrack();
			dayTimeTrack.EmployeeUID = employeeUID;
			dayTimeTrack.Date = date;
			dayTimeTrack.IsIgnoreHoliday = schedule.IsIgnoreHoliday;
			dayTimeTrack.IsOnlyFirstEnter = schedule.IsOnlyFirstEnter;
			dayTimeTrack.AllowedLate = TimeSpan.FromSeconds(schedule.AllowedLate);
			dayTimeTrack.AllowedEarlyLeave = TimeSpan.FromSeconds(schedule.AllowedEarlyLeave);

			if (!schedule.IsIgnoreHoliday)
			{
				var holiday = Context.Holidays.FirstOrDefault(x => x.Date == date && x.Type == (int)FiresecAPI.EmployeeTimeIntervals.HolidayType.Holiday && x.OrganisationUID == employee.OrganisationUID);
				if (holiday != null)
				{
					dayTimeTrack.IsHoliday = true;
				}
				holiday = Context.Holidays.FirstOrDefault(x => x.Date == date && x.Type == (int)FiresecAPI.EmployeeTimeIntervals.HolidayType.BeforeHoliday && x.OrganisationUID == employee.OrganisationUID);
				if (holiday != null)
				{
					dayTimeTrack.HolidayReduction = holiday.Reduction;
				}
				holiday = Context.Holidays.FirstOrDefault(x => x.TransferDate == date && x.Type == (int)FiresecAPI.EmployeeTimeIntervals.HolidayType.WorkingHoliday && x.OrganisationUID == employee.OrganisationUID);
				if (holiday != null)
				{
					dayTimeTrack.IsHoliday = true;
				}
			}

			if (!dayTimeTrack.IsHoliday)
			{
				foreach (var tableInterval in intervals)
				{
					var timeTrackPart = new TimeTrackPart();
					timeTrackPart.StartTime = new TimeSpan(Math.BigMul(tableInterval.BeginTime, 10000000));
					timeTrackPart.EndTime = new TimeSpan(Math.BigMul(tableInterval.EndTime, 10000000));
					dayTimeTrack.PlannedTimeTrackParts.Add(timeTrackPart);
				}
				dayTimeTrack.PlannedTimeTrackParts = dayTimeTrack.PlannedTimeTrackParts.OrderBy(x => x.StartTime.Ticks).ToList();
			}
			if (dayTimeTrack.HolidayReduction > 0)
			{
				var reductionTimeSpan = new TimeSpan(Math.BigMul(dayTimeTrack.HolidayReduction, 10000000));
				var lastTimeTrack = dayTimeTrack.PlannedTimeTrackParts.LastOrDefault();
				if (lastTimeTrack != null)
				{
					if (lastTimeTrack.Delta.TotalHours > reductionTimeSpan.TotalHours)
					{
						lastTimeTrack.EndTime = lastTimeTrack.EndTime.Subtract(reductionTimeSpan);
					}
					else
					{
						dayTimeTrack.PlannedTimeTrackParts.Remove(lastTimeTrack);
					}
				}
			}

			foreach (var passJournal in passJournals)
			{
				var scheduleZone = scheduleZones.FirstOrDefault(x => x.ZoneUID == passJournal.ZoneUID);
				if (scheduleZone != null)
				{
					if (passJournal.EnterTime.HasValue && passJournal.ExitTime.HasValue)
					{
						var timeTrackPart = new TimeTrackPart();
						timeTrackPart.StartTime = passJournal.EnterTime.Value.TimeOfDay;
						timeTrackPart.EndTime = passJournal.ExitTime.Value.TimeOfDay;
						timeTrackPart.ZoneUID = passJournal.ZoneUID;
						dayTimeTrack.RealTimeTrackParts.Add(timeTrackPart);
					}
				}
			}
			dayTimeTrack.RealTimeTrackParts = dayTimeTrack.RealTimeTrackParts.OrderBy(x => x.StartTime.Ticks).ToList();

			var operationResult = SKDDatabaseService.TimeTrackExceptionTranslator.Get(date, employeeUID);
			if (!operationResult.HasError)
			{
				dayTimeTrack.TimeTrackException = operationResult.Result;
			}

			return dayTimeTrack;
		}
	}
}