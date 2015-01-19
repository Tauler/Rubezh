﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FiresecAPI.SKD.ReportFilters
{
	[DataContract]
	public class ReportFilter424 : SKDReportFilter, IReportFilterPeriod, IReportFilterOrganisation, IReportFilterDepartment, IReportFilterPosition, IReportFilterEmployee
	{
		#region IReportFilterOrganisation Members

		[DataMember]
		public List<Guid> Organisations { get; set; }

		#endregion

		#region IReportFilterDepartment Members

		[DataMember]
		public List<Guid> Departments { get; set; }

		#endregion

		#region IReportFilterPosition Members

		[DataMember]
		public List<Guid> Positions { get; set; }

		#endregion

		#region IReportFilterEmployee Members

		[DataMember]
		public List<Guid> Employees { get; set; }

		#endregion

		#region IReportFilterPeriod Members

		public ReportPeriodType PeriodType { get; set; }
		public DateTime DateTimeFrom { get; set; }
		public DateTime DateTimeTo { get; set; }

		#endregion
	}
}
