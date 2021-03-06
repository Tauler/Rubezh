﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace VideoModule.ViewModels
{
	class CameraDetailsViewModel : SaveCancelDialogViewModel
	{
		public Camera Camera { get; private set; }
		public bool IsEditMode { get; private set; }

		public CameraDetailsViewModel(Camera camera = null)
		{
			if (camera != null)
			{
				Camera = camera;
				Title = "Свойства камеры";
				IsEditMode = true;
				CopyProperties();
			}
			else
			{
				Title = "Создание нового видеоустройства";
				Camera = new Camera();
				Name = "Видеоустройство";
				Address = "172.16.5.201";
				ChannelsCount = 1;
				ChannelNumber = 1;
				IsEditMode = false;
			}
			ShowCommand = new RelayCommand(OnShow);
		}

		int _left;
		public int Left
		{
			get { return _left; }
			set
			{
				_left = value;
				OnPropertyChanged(() => Left);
			}
		}

		int _top;
		public int Top
		{
			get { return _top; }
			set
			{
				_top = value;
				OnPropertyChanged(() => Top);
			}
		}

		int _width;
		public int Width
		{
			get { return _width; }
			set
			{
				_width = value;
				OnPropertyChanged(() => Width);
			}
		}

		int _height;
		public int Height
		{
			get { return _height; }
			set
			{
				_height = value;
				OnPropertyChanged(() => Height);
			}
		}

		void CopyProperties()
		{
			Name = Camera.Name;
			Address = Camera.Ip;
			ChannelNumber = Camera.ChannelNumber + 1;
			Left = Camera.Left;
			Top = Camera.Top;
			Width = Camera.Width;
			Height = Camera.Height;
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(() => Name);
			}
		}

		string _address;
		public string Address
		{
			get { return _address; }
			set
			{
				_address = value;
				OnPropertyChanged(() => Address);
			}
		}

		int _channelsCount;
		public int ChannelsCount
		{
			get { return _channelsCount; }
			set
			{
				_channelsCount = value;
				OnPropertyChanged(() => ChannelsCount);
			}
		}

		int _channelNumber;
		public int ChannelNumber
		{
			get { return _channelNumber; }
			set
			{
				_channelNumber = value;
				OnPropertyChanged(() => ChannelNumber);
			}
		}

		VlcControl _vlcControl;
		public ImageSource Image
		{
			get
			{
				if (_vlcControl == null)
					return new BitmapImage();
				return _vlcControl.VideoSource;
			}
		}

		public RelayCommand ShowCommand { get; private set; }
		void OnShow()
		{
			try
			{
				if (!VlcContext.IsInitialized)
				{
					VlcContext.LibVlcDllsPath = FiresecManager.SystemConfiguration.RviSettings.DllsPath;
					VlcContext.LibVlcPluginsPath = FiresecManager.SystemConfiguration.RviSettings.PluginsPath;
					VlcContext.StartupOptions.IgnoreConfig = true;
					VlcContext.StartupOptions.LogOptions.LogInFile = false;
					VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;
					VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
					VlcContext.Initialize();
				}
				_vlcControl = new VlcControl { Media = new LocationMedia(Camera.RviRTSP) };
				_vlcControl.PositionChanged -= VlcControlOnPositionChanged;
				_vlcControl.PositionChanged += VlcControlOnPositionChanged;
				if (_vlcControl.IsPlaying)
					_vlcControl.Stop();
				_vlcControl.Play();
			}
			catch (Exception e)
			{
				MessageBoxService.ShowWarning(e.Message);
			}
		}

		private void VlcControlOnPositionChanged(VlcControl sender, VlcEventArgs<float> vlcEventArgs)
		{
			OnPropertyChanged(() => Image);
		}

		public override bool OnClosing(bool isCanceled)
		{
			if (_vlcControl != null && _vlcControl.IsPlaying)
				_vlcControl.Stop();
			return base.OnClosing(isCanceled);
		}

		protected override bool Save()
		{
			Camera.Name = Name;
			Camera.Ip = Address;
			Camera.Left = Left;
			Camera.Top = Top;
			Camera.Width = Width;
			Camera.Height = Height;
			return base.Save();
		}
	}
}