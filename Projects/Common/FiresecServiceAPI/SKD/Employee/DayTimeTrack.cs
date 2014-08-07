﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FiresecAPI.SKD
{
	[DataContract]
	public class DayTimeTrack
	{
		public DayTimeTrack()
		{
			TimeTrackParts = new List<DayTimeTrackPart>();
			Intervals = new List<Interval>();
		}

		public DayTimeTrack(string error)
		{
			Error = error;
		}

		[DataMember]
		public Guid EmployeeUID { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public List<DayTimeTrackPart> TimeTrackParts { get; set; }

		[DataMember]
		public List<Interval> Intervals { get; set; }

		[DataMember]
		public TimeSpan Total { get; set; }

		[DataMember]
		public TimeSpan TotalMiss { get; set; }

		[DataMember]
		public TimeSpan TotalInSchedule { get; set; }

		[DataMember]
		public TimeSpan TotalOutSchedule { get; set; }

		[DataMember]
		public string Error { get; set; }
	}

	[DataContract]
	public class DayTimeTrackPart
	{
		[DataMember]
		public DateTime StartTime { get; set; }

		[DataMember]
		public DateTime EndTime { get; set; }

		[DataMember]
		public Guid ZoneUID { get; set; }
	}
}