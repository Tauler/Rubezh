﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FiresecAPI.SKD.ReportFilters
{
	[DataContract]
	public class PositionsReportFilter : SKDReportFilter, IReportFilterOrganisation, IReportFilterPosition, IReportFilterArchive
	{
		#region IReportFilterOrganisation Members

		[DataMember]
		public List<Guid> Organisations { get; set; }

		#endregion

		#region IReportFilterPosition Members

		[DataMember]
		public List<Guid> Positions { get; set; }

		#endregion

		#region IReportFilterArchive Members

		[DataMember]
		public bool UseArchive { get; set; }

		#endregion
	}
}
