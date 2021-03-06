﻿using System;
using System.Linq;
using FiresecAPI.SKD.ReportFilters;
using Infrastructure.Common.Windows;
using FiresecAPI.Models;

namespace Infrastructure.Common.SKDReports
{
	public abstract class FilteredSKDReportProvider<T> : SKDReportProvider, IFilteredSKDReportProvider
		where T : SKDReportFilter
	{
		private bool _modelCreated;
		public FilteredSKDReportProvider(string title, int index, SKDReportGroup? group = null, PermissionType? permission = null)
			: base(title, index, group, permission)
		{
			_modelCreated = false;
			Filter = Activator.CreateInstance<T>();
		}

		protected T Filter { get; private set; }

		#region IFilteredSKDReportProvider Members

		public Type FilterType
		{
			get { return typeof(T); }
		}

		public virtual SKDReportFilter GetFilter()
		{
			Filter.User = ApplicationService.User.Name;
			Filter.Timestamp = DateTime.Now;
			return Filter;
		}

		public abstract FilterModel GetFilterModel();

		public void UpdateFilter(SKDReportFilter filter)
		{
			Filter = filter as T;
		}

		#endregion
	}
}
