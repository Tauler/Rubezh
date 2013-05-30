﻿using System.Windows.Controls;
using Infrastructure.Common;
using Infrastructure.Common.TreeList;

namespace PlansModule.ViewModels
{
	public class ElementBaseViewModel : TreeItemViewModel<ElementBaseViewModel>
	{
		public RelayCommand ShowOnPlanCommand { get; protected set; }

		public virtual ContextMenu ContextMenu
		{
			get { return null; }
		}

		private bool _isBold;
		public bool IsBold
		{
			get { return _isBold; }
			set
			{
				_isBold = value;
				OnPropertyChanged(() => IsBold);
			}
		}
		private bool _isGroupHaveChild;
		public bool IsGroupHasChild	
		{
			get { return _isGroupHaveChild; }
			set
			{
				_isGroupHaveChild = value;
				OnPropertyChanged(() => IsGroupHasChild);
			}
		}
		
	}
}
