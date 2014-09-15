﻿using System;

namespace SKDDriver.DataAccess
{
	public interface IDatabaseElement
	{
		Guid UID { get; set; }
	}

	public interface ILinkedToEmployee
	{
		Guid? EmployeeUID { get; set; }
	}

	public interface IIsDeletedDatabaseElement
	{
		bool IsDeleted { get; set; }
		DateTime RemovalDate { get; set; }
	}

	public interface IOrganisationDatabaseElement
	{
		Guid? OrganisationUID { get; set; }
	}

	public partial class Journal : IDatabaseElement { }
	public partial class AdditionalColumn : IDatabaseElement, ILinkedToEmployee { }
	public partial class Photo : IDatabaseElement { }
	public partial class NightSetting : IDatabaseElement { }

	public partial class ScheduleDay : IDatabaseElement, IIsDeletedDatabaseElement { }
	public partial class ScheduleZone : IDatabaseElement, IIsDeletedDatabaseElement { }
	public partial class Organisation : IDatabaseElement, IIsDeletedDatabaseElement { }
	public partial class DayIntervalPart : IDatabaseElement, IIsDeletedDatabaseElement { }
	public partial class CardDoor : IDatabaseElement, IIsDeletedDatabaseElement { }
	public partial class Card : IDatabaseElement, IIsDeletedDatabaseElement, ILinkedToEmployee { }
	
	public partial class AdditionalColumnType : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class Department : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class Employee : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class Holiday : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class DayInterval : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class Position : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class Schedule : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class ScheduleScheme : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class AccessTemplate : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
	public partial class PassCardTemplate : IDatabaseElement, IIsDeletedDatabaseElement, IOrganisationDatabaseElement { }
}