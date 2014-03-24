﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FiresecAPI
{
	[DataContract]
	public class EmployeeReplacementFilter : OrganizationFilterBase
	{
		[DataMember]
		public List<Guid> ScheduleUIDs { get; set; }

		[DataMember]
		public List<Guid> DepartmentUIDs { get; set; }

		[DataMember]
		public List<Guid> EmployeeUIDs { get; set; }
		
		[DataMember]
		public DateTimePeriod ReplacementStartDates { get; set; }

		[DataMember]
		public DateTimePeriod ReplacementEndDates { get; set; }

		public EmployeeReplacementFilter()
			: base()
		{
			ScheduleUIDs = new List<Guid>();
			DepartmentUIDs = new List<Guid>();
			EmployeeUIDs = new List<Guid>();
			ReplacementStartDates = new DateTimePeriod();
			ReplacementEndDates = new DateTimePeriod();
		}
	}
}