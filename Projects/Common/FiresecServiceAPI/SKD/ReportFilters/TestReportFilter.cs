﻿using System;
using System.Runtime.Serialization;

namespace FiresecAPI.SKD.ReportFilters
{
	[DataContract]
	public class TestReportFilter : SKDReportFilter
	{
		[DataMember]
		public DateTime Timestamp { get; set; }
	}
}
