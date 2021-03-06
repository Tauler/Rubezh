﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.ViewModels;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Events;
using KeyboardKey = System.Windows.Input.Key;

namespace VideoModule.ViewModels
{
	public class CamerasViewModel : MenuViewPartViewModel, IEditingViewModel, ISelectable<Guid>
	{
		bool _lockSelection = false;
		public static CamerasViewModel Current { get; private set; }
		public CamerasViewModel()
		{
			Menu = new CamerasMenuViewModel(this);
			AddCommand = new RelayCommand(OnAdd);
			DeleteCommand = new RelayCommand(OnDelete, CanEditDelete);
			EditCommand = new RelayCommand(OnEdit, CanEditDelete);
			SettingsCommand = new RelayCommand(OnSettings);
			RegisterShortcuts();
			SubscribeEvents();
			IsRightPanelEnabled = true;
			Current = this;
		}

		public void Initialize()
		{
			Cameras = new ObservableCollection<CameraViewModel>();
			foreach (var camera in FiresecManager.SystemConfiguration.Cameras)
			{
				var cameraViewModel = new CameraViewModel(this, camera);
				Cameras.Add(cameraViewModel);
			}
			SelectedCamera = Cameras.FirstOrDefault();
		}

		public ObservableCollection<CameraViewModel> Cameras { get; private set; }

		DeviceViewModel _selectedDevice;
		public DeviceViewModel SelectedDevice
		{
			get { return _selectedDevice; }
			set
			{
				_selectedDevice = value;
				OnPropertyChanged(() => SelectedDevice);
			}
		}

		CameraViewModel _selectedCamera;
		public CameraViewModel SelectedCamera
		{
			get { return _selectedCamera; }
			set
			{
				_selectedCamera = value;
				OnPropertyChanged(() => SelectedCamera);
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var devicesViewModel = new DeviceSelectionViewModel();
			if (DialogService.ShowModalWindow(devicesViewModel))
			{
				Cameras = new ObservableCollection<CameraViewModel>();
				foreach (var camera in FiresecManager.SystemConfiguration.Cameras)
				{
					Cameras.Add(new CameraViewModel(this, camera));
					OnPropertyChanged(() => Cameras);
				}
			}
		}

		public RelayCommand DeleteCommand { get; private set; }
		void OnDelete()
		{
			var camera = SelectedCamera.Camera;
			Cameras.Remove(SelectedCamera);
			FiresecManager.SystemConfiguration.Cameras.Remove(camera);
			camera.OnChanged();
			ServiceFactory.SaveService.CamerasChanged = true;
			SelectedCamera = Cameras.FirstOrDefault();
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var dvrDetailsViewModel = new CameraDetailsViewModel(SelectedCamera.Camera);
			if (DialogService.ShowModalWindow(dvrDetailsViewModel))
			{
				SelectedCamera.Camera = dvrDetailsViewModel.Camera;
				SelectedCamera.Update();
				SelectedCamera.Camera.OnChanged();
				ServiceFactory.SaveService.CamerasChanged = true;
			}
		}

		bool CanEditDelete()
		{
			return SelectedCamera != null;
		}

		public RelayCommand SettingsCommand { get; private set; }
		void OnSettings()
		{
			var settingsSelectionViewModel = new SettingsSelectionViewModel(FiresecManager.SystemConfiguration.RviSettings);
			if (DialogService.ShowModalWindow(settingsSelectionViewModel))
			{
				FiresecManager.SystemConfiguration.RviSettings = settingsSelectionViewModel.RviSettings;
				ServiceFactory.SaveService.CamerasChanged = true;
			}
		}

		private void SubscribeEvents()
		{
			ServiceFactory.Events.GetEvent<ElementAddedEvent>().Unsubscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementRemovedEvent>().Unsubscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementChangedEvent>().Subscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementSelectedEvent>().Unsubscribe(OnElementSelected);

			ServiceFactory.Events.GetEvent<ElementAddedEvent>().Subscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementRemovedEvent>().Subscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementChangedEvent>().Subscribe(OnElementChanged);
			ServiceFactory.Events.GetEvent<ElementSelectedEvent>().Subscribe(OnElementSelected);
		}

		private void OnCameraChanged(Guid cameraUID)
		{
			var camera = Cameras.FirstOrDefault(x => x.Camera.UID == cameraUID);
			if (camera != null)
			{
				camera.Update();
				// TODO: FIX IT
				if (!_lockSelection)
					SelectedCamera = camera;
			}
		}

		private void OnElementChanged(List<ElementBase> elements)
		{
			_lockSelection = true;
			elements.ForEach(element =>
			{
				var elementCamera = element as ElementCamera;
				if (elementCamera != null)
					OnCameraChanged(elementCamera.CameraUID);
			});
			_lockSelection = false;
		}
		private void OnElementSelected(ElementBase element)
		{
			var elementCamera = element as ElementCamera;
			if (elementCamera != null)
			{
				_lockSelection = true;
				Select(elementCamera.CameraUID);
				_lockSelection = false;
			}
		}

		public void Select(Guid cameraUID)
		{
			if (cameraUID != Guid.Empty)
			{
				SelectedCamera = Cameras.FirstOrDefault(item => item.Camera.UID == cameraUID);
			}
		}

		private void RegisterShortcuts()
		{
			RegisterShortcut(new KeyGesture(KeyboardKey.N, ModifierKeys.Control), AddCommand);
			RegisterShortcut(new KeyGesture(KeyboardKey.Delete, ModifierKeys.Control), DeleteCommand);
			RegisterShortcut(new KeyGesture(KeyboardKey.E, ModifierKeys.Control), EditCommand);
		}
	}
}