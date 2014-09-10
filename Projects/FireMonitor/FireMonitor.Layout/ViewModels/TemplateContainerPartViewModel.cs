﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FiresecAPI.Models.Layouts;
using Infrastructure.Common.Windows.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using LayoutModel = FiresecAPI.Models.Layouts.Layout;
using Infrastructure.Common.Services.Layout;
using Infrastructure.Common.Windows;
using FiresecClient;

namespace FireMonitor.Layout.ViewModels
{
	public class TemplateContainerPartViewModel : BaseViewModel
	{
		public TemplateContainerPartViewModel(LayoutPartReferenceProperties properties)
		{
			Layout = FiresecManager.LayoutsConfiguration.Layouts.FirstOrDefault(item => item.UID == properties.ReferenceUID);
			LoadLayout();
		}

		public LayoutModel Layout { get; private set; }
		private XmlLayoutSerializer _serializer;

		private void LoadLayout()
		{
			Initialize();
			if (Layout != null && Manager != null)
			{
				Manager.GridSplitterHeight = Layout.SplitterSize;
				Manager.GridSplitterWidth = Layout.SplitterSize;
				Manager.GridSplitterBackground = new SolidColorBrush(Layout.SplitterColor);
				Manager.BorderBrush = new SolidColorBrush(Layout.BorderColor);
				Manager.BorderThickness = new Thickness(Layout.BorderThickness);
				if (_serializer != null && !string.IsNullOrEmpty(Layout.Content))
					using (var tr = new StringReader(Layout.Content))
						_serializer.Deserialize(tr);
			}
		}
		private void Initialize()
		{
			var list = new List<LayoutPartViewModel>();
			var map = new Dictionary<Guid, ILayoutPartPresenter>();
			foreach (var module in ApplicationService.Modules)
			{
				var layoutProviderModule = module as ILayoutProviderModule;
				if (layoutProviderModule != null)
					foreach (var layoutPart in layoutProviderModule.GetLayoutParts())
						map.Add(layoutPart.UID, layoutPart);
			}
			if (Layout != null)
				foreach (var layoutPart in Layout.Parts)
					list.Add(new LayoutPartViewModel(layoutPart, map.ContainsKey(layoutPart.DescriptionUID) ? map[layoutPart.DescriptionUID] : new UnknownLayoutPartPresenter(layoutPart.DescriptionUID)));
			LayoutParts = new ObservableCollection<LayoutPartViewModel>(list);
		}

		private ObservableCollection<LayoutPartViewModel> _layoutParts;
		public ObservableCollection<LayoutPartViewModel> LayoutParts
		{
			get { return _layoutParts; }
			set
			{
				if (_layoutParts != null)
					_layoutParts.CollectionChanged -= LayoutPartsChanged;
				_layoutParts = value;
				_layoutParts.CollectionChanged += LayoutPartsChanged;
				OnPropertyChanged(() => LayoutParts);
			}
		}

		private LayoutPartViewModel _activeLayoutPart;
		public LayoutPartViewModel ActiveLayoutPart
		{
			get { return _activeLayoutPart; }
			set
			{
				_activeLayoutPart = value;
				OnPropertyChanged(() => ActiveLayoutPart);
			}
		}

		private DockingManager _manager;
		public DockingManager Manager
		{
			get { return _manager; }
			set
			{
				if (Manager != null)
				{
					Manager.DocumentClosing -= LayoutPartClosing;
					Manager.LayoutChanged -= LayoutChanged;
				}
				_manager = value;
				if (_serializer != null)
				{
					_serializer.LayoutSerializationCallback -= LayoutSerializationCallback;
					_serializer = null;
				}
				if (_manager != null)
				{
					Manager.LayoutChanged += LayoutChanged;
					Manager.DocumentClosing += LayoutPartClosing;
					Manager.LayoutUpdateStrategy = new LayoutUpdateStrategy();
					_serializer = new XmlLayoutSerializer(Manager);
					_serializer.LayoutSerializationCallback += LayoutSerializationCallback;
					LoadLayout();
				}
			}
		}

		private void LayoutSerializationCallback(object sender, LayoutSerializationCallbackEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(e.Model.ContentId))
				e.Content = LayoutParts.FirstOrDefault(item => item.UID == Guid.Parse(e.Model.ContentId));
		}
		private void LayoutChanged(object sender, EventArgs e)
		{
		}
		private void LayoutPartClosing(object sender, DocumentClosingEventArgs e)
		{
			var layoutPartViewModel = e.Document.Content as LayoutPartViewModel;
			if (layoutPartViewModel != null)
			{
				LayoutParts.Remove(layoutPartViewModel);
				e.Cancel = true;
			}
		}
		private void LayoutPartsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
		}
	}
}