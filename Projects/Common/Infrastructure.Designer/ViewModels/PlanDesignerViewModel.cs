﻿using System;
using Infrastructure.Client.Plans;
using Infrastructure.Common.Services;
using Infrastructure.Common.Windows.ViewModels;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.Events;

namespace Infrastructure.Designer.ViewModels
{
	public partial class PlanDesignerViewModel : BaseViewModel, IPlanDesignerViewModel
	{
		public event EventHandler Updated;
		public event EventHandler IsCollapsedChanged;
		public DesignerCanvas DesignerCanvas { get; set; }

		private bool _isNotEmpty;
		public bool IsNotEmpty
		{
			get { return _isNotEmpty; }
			protected set
			{
				_isNotEmpty = value;
				OnPropertyChanged(() => IsNotEmpty);
			}
		}
		public bool AllowScalePoint { get; protected set; }
		public bool FullScreenSize { get; protected set; }

		public PlanDesignerViewModel()
		{
			IsNotEmpty = false;
			CanCollapse = true;
			InitializeHistory();
			InitializeZIndexCommands();
			InitializeAlignCommands();
			InitializeCopyPasteCommands();

			ServiceFactoryBase.Events.GetEvent<ShowElementEvent>().Subscribe(OnShowElement);
		}

		public void Update()
		{
			OnUpdated();
		}
		private void OnUpdated()
		{
			if (Updated != null)
				Updated(this, EventArgs.Empty);
		}

		public virtual void RegisterDesignerItem(DesignerItem designerItem)
		{
		}

		#region IPlanDesignerViewModel Members

		public object Toolbox
		{
			get { return DesignerCanvas.Toolbox; }
		}

		public CommonDesignerCanvas Canvas
		{
			get { return DesignerCanvas; }
		}

		public bool HasPermissionsToScale
		{
			get { return true; }
		}

		public bool AlwaysShowScroll
		{
			get { return true; }
		}

		public bool CanCollapse { get; protected set; }

		private bool _isCollapsed;
		public bool IsCollapsed
		{
			get { return _isCollapsed; }
			set
			{
				if (IsCollapsed != value)
				{
					_isCollapsed = value;
					OnPropertyChanged(() => IsCollapsed);
					if (IsCollapsedChanged != null)
						IsCollapsedChanged(this, EventArgs.Empty);
				}
			}
		}

		#endregion

		public void Save()
		{
			if (IsNotEmpty)
				NormalizeZIndex();
		}

		private void OnShowElement(Guid elementUID)
		{
			DesignerCanvas.Toolbox.SetDefault();
			DesignerCanvas.DeselectAll();
			foreach (var designerItem in DesignerCanvas.Items)
				if (designerItem.Element.UID == elementUID && designerItem.IsEnabled)
				{
					designerItem.IsSelected = true;
					break;
				}
		}
	}
}