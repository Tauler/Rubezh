﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FiresecAPI.SKD.ReportFilters
{
	[DataContract]
	public class ReportFilter415 : SKDReportFilter, IReportFilterOrganisation, IReportFilterDepartment
	{
		#region IReportFilterOrganisation Members

		[DataMember]
		public List<Guid> Organisations { get; set; }

		#endregion

		#region IReportFilterDepartment Members

		[DataMember]
		public List<Guid> Departments { get; set; }

		#endregion
	}
}