﻿using System.Windows;
using System.Windows.Controls;
using Infrastructure.Client.Plans.ViewModels;
using Infrastructure.Common;
using Infrastructure.Common.Services;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Events;
using System.Windows.Input;
using System.Collections.Generic;
using System;

namespace Infrastructure.Designer.DesignerItems
{
	public class DesignerItemBase : DesignerItem
	{
		private ContextMenu _contextMenu;
		private ContextMenu _elementContextMenu;
		public DesignerItemBase(ElementBase element)
			: base(element)
		{
			_contextMenu = null;
			ShowPropertiesCommand = new RelayCommand(OnShowProperties);
			DeleteCommand = new RelayCommand(OnDelete);
			DeleteCurrentCommand = new RelayCommand(OnDeleteCurrent);
		}

		protected override void OnShowProperties()
		{
			var property = CreatePropertiesViewModel();
			if (property != null)
			{
				DesignerCanvas.BeginChange();
				if (((DesignerCanvas)DesignerCanvas).Toolbox != null)
					((DesignerCanvas)DesignerCanvas).Toolbox.AcceptKeyboard = false;
				if (DialogService.ShowModalWindow(property))
				{
					Title = Element.PresentationName;
					OnDesignerItemPropertyChanged();
					Painter.Invalidate();
					if (ResizeChrome != null)
						ResizeChrome.InvalidateVisual();
					DesignerCanvas.Refresh();
					DesignerCanvas.DesignerChanged();
					DesignerCanvas.EndChange();
				}
				if (((DesignerCanvas)DesignerCanvas).Toolbox != null)
					((DesignerCanvas)DesignerCanvas).Toolbox.AcceptKeyboard = true;
			}
		}
		protected override void OnDelete()
		{
			((DesignerCanvas)DesignerCanvas).RemoveAllSelected();
		}
		protected virtual SaveCancelDialogViewModel CreatePropertiesViewModel()
		{
			var args = new ShowPropertiesEventArgs(Element);
			ServiceFactoryBase.Events.GetEvent<ShowPropertiesEvent>().Publish(args);
			return args.PropertyViewModel as SaveCancelDialogViewModel;
		}

		public override ContextMenu GetContextMenu()
		{
			if (_contextMenu == null)
			{
				_contextMenu = new ContextMenu();

				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Копировать (Ctrl+C)",
					"BCopy",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.CopyCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вырезать (Ctrl+X)",
					"BCut",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.CutCommand)
				);

				var menuItem = DesignerCanvasHelper.BuildMenuItem(
					"Вставить (Ctrl+V)",
					"BPaste",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.PasteCommand
				);
				menuItem.CommandParameter = DesignerCanvas;
				_contextMenu.Items.Add(menuItem);

				_contextMenu.Items.Add(new Separator());
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Удалить (Del)",
					"BDelete",
					DeleteCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Свойства",
					"BSettings",
					ShowPropertiesCommand
				));
				_contextMenu.Items.Add(new Separator());
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вверх",
					"BMoveForward",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveToFrontCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вниз",
					"BMoveBackward",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.SendToBackCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выше",
					"BMoveFront",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveForwardCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Ниже",
					"BMoveBack",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveBackwardCommand
				));
				_contextMenu.Items.Add(new Separator());
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по левому краю",
					"bshapes-align-hori-left",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignHorizontalLeftCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по вертикали",
					"bshapes-align-hori-center",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignHorizontalCenterCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по правому краю",
					"bshapes-align-hori-right",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignHorizontalRightCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по верхнему краю",
					"bshapes-align-verti-top",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignVerticalTopCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по горизонтали",
					"bshapes-align-verti-middle",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignVerticalCenterCommand
				));
				_contextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выровнять по нижнему краю",
					"bshapes-align-verti-bottom",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.AlignVerticalBottomCommand
				));
			};
			return _contextMenu;
		}
		public override ContextMenu GetElementContextMenu()
		{
			if (_elementContextMenu == null)
			{
				_elementContextMenu = new ContextMenu();

				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Копировать (Ctrl+C)",
					"BCopy",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.CopyCommand
				));
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вырезать (Ctrl+X)",
					"BCut",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.CutCommand)
				);

				_elementContextMenu.Items.Add(new Separator());
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Удалить",
					"BDelete",
					DeleteCurrentCommand
				));
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Свойства",
					"BSettings",
					ShowPropertiesCommand
				));
				_elementContextMenu.Items.Add(new Separator());
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вверх",
					"BMoveForward",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveToFrontCommand
				));
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Вниз",
					"BMoveBackward",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.SendToBackCommand
				));
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Выше",
					"BMoveFront",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveForwardCommand
				));
				_elementContextMenu.Items.Add(DesignerCanvasHelper.BuildMenuItem(
					"Ниже",
					"BMoveBack",
					((DesignerCanvas)DesignerCanvas).PlanDesignerViewModel.MoveBackwardCommand
				));
			};
			return _elementContextMenu;
		}

		public ICommand DeleteCurrentCommand { get; private set; }
		private void OnDeleteCurrent()
		{
			((DesignerCanvas)DesignerCanvas).RemoveDesignerItem(this);
			ServiceFactoryBase.Events.GetEvent<ElementRemovedEvent>().Publish(new List<ElementBase>() { Element });
			DesignerCanvas.DesignerChanged();
		}

		public override void DragDelta(Point point, Vector shift)
		{
			base.DragDelta(point, shift);
			if (IsSelected)
				DesignerCanvas.DesignerChanged();
		}

		//public override void DragStarted(Point point)
		//{
		//	if (IsSelected)
		//	{
		//		IsBusy = true;
		//		((DesignerCanvas)DesignerCanvas).BeginMove(point);
		//	}
		//}
		//public override void DragCompleted(Point point)
		//{
		//	IsBusy = false;
		//	((DesignerCanvas)DesignerCanvas).EndMove();
		//}

		protected override void OnChanged()
		{
			base.OnChanged();
			DesignerCanvas.DesignerChanged();
		}
		protected override object GetToolTip()
		{
			var tooltip = Painter == null ? null : Painter.GetToolTip(Title);
			return tooltip == null ? new ImageTextTooltipViewModel() { Title = string.IsNullOrEmpty(ClassName) ? Title : string.Format("{0}{1}{2}", Title, Environment.NewLine, ClassName), ImageSource = IconSource } : tooltip;
		}
	}
}