﻿using FiresecAPI.SKD.ReportFilters;
using Infrastructure.Common.SKDReports;
using System.Collections.Generic;
using SKDModule.Reports.ViewModels;

namespace SKDModule.Reports.Providers
{
	public class ReportProvider416 : FilteredSKDReportProvider<ReportFilter416>
	{
		public ReportProvider416()
			: base("Report416", "416. Отчет \"Список должностей организации\"", 416, SKDReportGroup.HR)
		{
		}

		public override FilterModel CreateFilterModel()
		{
			return new FilterModel()
			{
				Columns = new Dictionary<string, string> 
				{ 
					{ "c01", "Должность" },
					{ "c02", "Примечание" },
				},
				Pages = new List<FilterContainerViewModel>()
				{
					new OrganizationPageViewModel(false),
					new PositionPageViewModel(),
				},
			};
		}
	}
}