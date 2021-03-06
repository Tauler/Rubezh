﻿using System.Windows.Controls;
using Controls.TreeList;
using FiresecAPI.SKD;
using SKDModule.ViewModels;

namespace SKDModule.Views
{
	public partial class SchedulesView : UserControl, IWithDeletedView
	{
		public SchedulesView()
		{
			InitializeComponent();
			_changeIsDeletedViewSubscriber = new ChangeIsDeletedViewSubscriber(this);
		}

		ChangeIsDeletedViewSubscriber _changeIsDeletedViewSubscriber;

		public TreeList TreeList
		{
			get { return _treeList; }
			set { _treeList = value; }
		}

		private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			var deletationType = (DataContext as SchedulesViewModel).IsWithDeleted ? LogicalDeletationType.All : LogicalDeletationType.Active;
			_changeIsDeletedViewSubscriber = new ChangeIsDeletedViewSubscriber(this, deletationType);
		}
	}
}