﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecAPI.SKD;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Painters;
using Infrustructure.Plans.Presenter;
using SKDModule.Events;
using SKDModule.ViewModels;
using Infrastructure.Client.Plans;
using Infrastructure.Client.Plans.Presenter;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.Plans.Designer
{
	class SKDZonePainter : BaseZonePainter<SKDZone, ShowSKDZoneEvent>
	{
		private ZoneViewModel _zoneViewModel;

		public SKDZonePainter(PresenterItem presenterItem)
			: base(presenterItem)
		{
			if (Item != null)
				_zoneViewModel = new ViewModels.ZoneViewModel(Item);
		}

		protected override SKDZone CreateItem(PresenterItem presenterItem)
		{
			var element = presenterItem.Element as IElementZone;
			return element == null ? null : PlanPresenter.Cache.Get<SKDZone>(element.ZoneUID);
		}
		protected override StateTooltipViewModel<SKDZone> CreateToolTip()
		{
			return new SKDZoneTooltipViewModel(Item);
		}
		protected override ContextMenu CreateContextMenu()
		{
			var contextMenu = new ContextMenu();
			if (Item != null)
			{
				contextMenu.Items.Add(Helper.CreateShowInTreeItem());
				contextMenu.Items.Add(UIHelper.BuildMenuItem(
					"Команда",
					"pack://application:,,,/Controls;component/Images/BTurnOff.png",
					_zoneViewModel.ZoneCommand
				));
				contextMenu.Items.Add(Helper.CreateShowPropertiesItem());
			}
			return contextMenu;
		}
		protected override WindowBaseViewModel CreatePropertiesViewModel()
		{
			return new ZoneDetailsViewModel(Item);
		}
	}
}