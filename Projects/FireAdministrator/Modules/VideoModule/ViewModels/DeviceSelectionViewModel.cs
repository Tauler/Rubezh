﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common.Windows.ViewModels;
using RviClient.RVIServiceReference;

namespace VideoModule.ViewModels
{
	public class DeviceSelectionViewModel : SaveCancelDialogViewModel
	{
		public DeviceSelectionViewModel()
		{
			Title = "Устройства";
			Devices = new ObservableCollection<DeviceViewModel>();

			var devices = GetDevices();
			foreach (var device in devices)
			{
				foreach (var channel in device.Channels)
				{
					var deviceViewModel = new DeviceViewModel(device, channel);
					var camera = FiresecManager.SystemConfiguration.Cameras.FirstOrDefault(x => x.RviDeviceUID == device.Guid && x.RviChannelNo == channel.Number);
					if (camera != null)
					{
						deviceViewModel.IsChecked = true;
					}
					Devices.Add(deviceViewModel);
				}
			}
		}

		public ObservableCollection<DeviceViewModel> Devices { get; private set; }

		protected override bool Save()
		{
			var cameras = new List<FiresecAPI.Models.Camera>();
			foreach (var device in Devices)
			{
				if (device.IsChecked)
				{
					var camera = FiresecManager.SystemConfiguration.Cameras.FirstOrDefault(x => x.RviDeviceUID == device.Device.Guid && x.RviChannelNo == device.Channel.Number);
					if (camera == null)
						camera = new FiresecAPI.Models.Camera();
					var stream = device.Channel.Streams.FirstOrDefault();
					if (stream != null)
					{
						camera.Ip = device.Device.Ip;

						camera.RviDeviceUID = device.Device.Guid;
						camera.RviChannelNo = device.Channel.Number;
						camera.RviRTSP = stream.Rtsp;
					}

					if (!cameras.Contains(camera))
						cameras.Add(camera);
				}
			}
			FiresecManager.SystemConfiguration.Cameras = cameras;
			ServiceFactory.SaveService.CamerasChanged = true;
			return true;
		}

		List<Device> GetDevices()
		{
			var devices = new List<Device>();
			var binding = new NetTcpBinding(SecurityMode.None);
			binding.OpenTimeout = TimeSpan.FromMinutes(10);
			binding.SendTimeout = TimeSpan.FromMinutes(10);
			binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
			binding.MaxReceivedMessageSize = Int32.MaxValue;
			binding.ReliableSession.InactivityTimeout = TimeSpan.MaxValue;
			binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
			binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
			binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
			binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
			binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
			binding.Security.Mode = SecurityMode.None;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
			binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			var ip = FiresecManager.SystemConfiguration.RviSettings.Ip;
			var port = FiresecManager.SystemConfiguration.RviSettings.Port;
			var login = FiresecManager.SystemConfiguration.RviSettings.Login;
			var password = FiresecManager.SystemConfiguration.RviSettings.Password;
			var endpointAddress = new EndpointAddress(new Uri("net.tcp://" + ip + ":" + port + "/Integration"));

			using (IntegrationClient client = new IntegrationClient(binding, endpointAddress))
			{
				var sessionUID = Guid.NewGuid();

				var sessionInitialiazationIn = new SessionInitialiazationIn();
				sessionInitialiazationIn.Header = new HeaderRequest()
				{
					Request = Guid.NewGuid(),
					Session = sessionUID
				};
				sessionInitialiazationIn.Login = login;
				sessionInitialiazationIn.Password = password;
				var sessionInitialiazationOut = client.SessionInitialiazation(sessionInitialiazationIn);
				//var errorMessage = sessionInitialiazationOut.Header.HeaderResponseMessage.Information;

				var perimeterIn = new PerimeterIn();
				perimeterIn.Header = new HeaderRequest()
				{
					Request = Guid.NewGuid(),
					Session = sessionUID
				};
				var perimeterOut = client.GetPerimeter(perimeterIn);
				devices = perimeterOut.Devices.ToList();

				var sessionCloseIn = new SessionCloseIn();
				sessionCloseIn.Header = new HeaderRequest()
				{
					Request = Guid.NewGuid(),
					Session = sessionUID
				};
				var sessionCloseOut = client.SessionClose(sessionCloseIn);
			}

			return devices;
		}
	}
}
