﻿using System.Windows.Controls;
using FiresecAPI.GK;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using Infrustructure.Plans.Presenter;

namespace Infrastructure.Client.Plans.Presenter
{
	public interface IBasePainter<T, TShowEvent>
			where T : IStateProvider
	{
		T CreateItem(PresenterItem presenterItem);
		StateTooltipViewModel<T> CreateToolTip();
		ContextMenu CreateContextMenu();
		WindowBaseViewModel CreatePropertiesViewModel();
		RelayCommand ShowInTreeCommand { get; set; }
		RelayCommand ShowPropertiesCommand { get; set; }
		bool IsPoint { get; }
	}
}
