﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.Reports.ViewModels
{
	public class CheckedItemViewModel<TItem> : BaseViewModel
	{
		public CheckedItemViewModel(TItem item)
		{
			Item = item;
		}

		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				OnPropertyChanged(() => IsChecked);
			}
		}
		public TItem Item { get; private set; }
	}
}