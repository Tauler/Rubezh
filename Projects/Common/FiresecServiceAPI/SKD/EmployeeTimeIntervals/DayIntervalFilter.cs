﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FiresecAPI.EmployeeTimeIntervals
{
	[DataContract]
	public class DayIntervalFilter : IsDeletedFilter
	{
		public DayIntervalFilter()
		{
		}

		[DataMember]
		public List<Guid> ScheduleSchemeUIDs { get; set; }
	}
}