﻿using FiresecAPI.Models;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;
using System.Collections.ObjectModel;
using FiresecClient;
using System.Collections.Generic;
using System;
using System.Linq;
using Infrastructure;
using FiresecAPI;

namespace SKDModule.ViewModels
{
	public class DeviceCommandsViewModel : BaseViewModel
	{
		public SKDDevice Device { get; private set; }
		public SKDDeviceState DeviceState
		{
			get { return Device.State; }
		}

		public DeviceCommandsViewModel(SKDDevice device)
		{
			Device = device;
			DeviceState.StateChanged -= new System.Action(OnStateChanged);
			DeviceState.StateChanged += new System.Action(OnStateChanged);

			SetRegimeOpenCommand = new RelayCommand(OnSetRegimeOpen, CanSetRegimeOpen);
			SetRegimeCloseCommand = new RelayCommand(OnSetRegimeClose, CanSetRegimeClose);
			SetRegimeControlCommand = new RelayCommand(OnSetRegimeControl, CanSetRegimeControl);
			SetRegimeConversationCommand = new RelayCommand(OnSetRegimeConversation, CanSetRegimeConversation);
			OpenCommand = new RelayCommand(OnOpen, CanOpen);
			CloseCommand = new RelayCommand(OnClose, CanClose);
		}

		public bool CanControl
		{
			get { return Device.DriverType == SKDDriverType.Controller && FiresecManager.CheckPermission(PermissionType.Oper_ControlDevices); }
		}

		public RelayCommand SetRegimeOpenCommand { get; private set; }
		void OnSetRegimeOpen()
		{
			if (ServiceFactory.SecurityService.Validate())
			{
				FiresecManager.FiresecService.SKDSetRegimeOpen(Device);
			}
		}
		bool CanSetRegimeOpen()
		{
			return true;
		}

		public RelayCommand SetRegimeCloseCommand { get; private set; }
		void OnSetRegimeClose()
		{
			if (ServiceFactory.SecurityService.Validate())
			{
				FiresecManager.FiresecService.SKDSetRegimeClose(Device);
			}
		}
		bool CanSetRegimeClose()
		{
			return true;
		}

		public RelayCommand SetRegimeControlCommand { get; private set; }
		void OnSetRegimeControl()
		{
			if (ServiceFactory.SecurityService.Validate())
			{
				FiresecManager.FiresecService.SKDSetRegimeControl(Device);
			}
		}
		bool CanSetRegimeControl()
		{
			return true;
		}

		public RelayCommand SetRegimeConversationCommand { get; private set; }
		void OnSetRegimeConversation()
		{
			if (ServiceFactory.SecurityService.Validate())
			{
				FiresecManager.FiresecService.SKDSetRegimeConversation(Device);
			}
		}
		bool CanSetRegimeConversation()
		{
			return true;
		}

		public RelayCommand OpenCommand { get; private set; }
		void OnOpen()
		{
            if (ServiceFactory.SecurityService.Validate())
            {
				FiresecManager.FiresecService.SKDOpenDevice(Device);
            }
		}
		bool CanOpen()
		{
			return true;
		}

		public RelayCommand CloseCommand { get; private set; }
        void OnClose()
        {
            if (ServiceFactory.SecurityService.Validate())
            {
				FiresecManager.FiresecService.SKDCloseDevice(Device);
            }
        }
		bool CanClose()
		{
			return true;
		}

		void OnStateChanged()
		{
		}
	}
}