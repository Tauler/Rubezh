﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Infrustructure.Plans.Devices;

namespace DeviceControls
{
	public interface ILibraryDevice<TLibraryFrame>
		where TLibraryFrame : ILibraryFrame
	{
		Guid DriverId { get; set; }
		IEnumerable<ILibraryState<TLibraryFrame>> GetStates();
	}
	public abstract class BaseDevicePicture<TLibraryFrame>
		where TLibraryFrame : ILibraryFrame
	{
		private Dictionary<Guid, Brush> _brushes = new Dictionary<Guid, Brush>();

		public void LoadCache()
		{
			_brushes.Clear();
			RegisterBrush(null);
			foreach (var libraryDevice in EnumerateLibrary())
				RegisterBrush(libraryDevice);
		}
		public abstract void LoadDynamicCache();

		protected Brush GetBrush(Guid driverUID)
		{
			if (!_brushes.ContainsKey(driverUID))
			{
				var libraryDevice = EnumerateLibrary().FirstOrDefault(x => x.DriverId == driverUID);
				if (libraryDevice == null)
				{
					if (!_brushes.ContainsKey(Guid.Empty))
						RegisterBrush(null);
					return _brushes[Guid.Empty];
				}
				else
					RegisterBrush(libraryDevice);
			}
			return _brushes[driverUID];
		}
		protected Brush CreateDynamicBrush(List<TLibraryFrame> frames)
		{
			var visualBrush = new VisualBrush();
			visualBrush.Visual = new FramesControl(frames.Cast<ILibraryFrame>());
			return visualBrush;
		}

		private void RegisterBrush(ILibraryDevice<TLibraryFrame> libraryDevice)
		{
			var frameworkElement = libraryDevice == null ? PictureCacheSource.EmptyPicture : GetDefaultPicture(libraryDevice);
			var brush = new VisualBrush(frameworkElement);
			if (_brushes.ContainsKey(libraryDevice == null ? Guid.Empty : libraryDevice.DriverId))
				_brushes[libraryDevice == null ? Guid.Empty : libraryDevice.DriverId] = brush;
			else
				_brushes.Add(libraryDevice == null ? Guid.Empty : libraryDevice.DriverId, brush);
		}
		private FrameworkElement GetDefaultPicture(ILibraryDevice<TLibraryFrame> device)
		{
			var state = GetDefaultState(device);
			return state != null && state.Frames.Count > 0 ? Helper.GetVisual(state.Frames[0].Image) : PictureCacheSource.EmptyPicture;
		}

		protected abstract ILibraryState<TLibraryFrame> GetDefaultState(ILibraryDevice<TLibraryFrame> libraryrDevice);
		protected abstract IEnumerable<ILibraryDevice<TLibraryFrame>> EnumerateLibrary();

		public Brush CreatePreviewBrush(List<TLibraryFrame> frames)
		{
			return CreateDynamicBrush(frames);
		}
	}
}