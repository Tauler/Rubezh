﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FiresecAPI.Models;
using FS2Api;
using ServerFS2.Journal;
using ServerFS2.Service;
using System.Diagnostics;
using ServerFS2.Operations;
using ServerFS2.Helpers;

namespace ServerFS2.Monitoring
{
	public partial class MonitoringPanel
	{
		const int MaxSequentUnAnswered = 10;
		const int MaxFireMessages = 1024;
		const int MaxSecurityMessages = 500;
		public const int RequestExpiredTime = 5;
		public static readonly object Locker = new object();

		public Device PanelDevice { get; private set; }
		List<Device> RealChildren;
		DeviceStatesManager DeviceStatesManager;
		bool IsConnectionLost;
		public bool IsInitialized { get; private set; }

		public List<Request> Requests { get; private set; }
		bool IsFireReadingNeeded = false;
		bool IsSecurityReadingNeeded = false;
		int LastDeviceFireIndex { get; set; }
		int LastDeviceSecurityIndex { get; set; }

		int _lastSystemFireIndex;
		public int LastSystemFireIndex
		{
			get { return _lastSystemFireIndex; }
			set
			{
				_lastSystemFireIndex = value;
				LastJournalIndexHelper.SetLastFireJournalIndex(PanelDevice, value);
			}
		}

		int _lastSystemSecurityIndex;
		public int LastSystemSecurityIndex
		{
			get { return _lastSystemSecurityIndex; }
			set
			{
				_lastSystemSecurityIndex = value;
				LastJournalIndexHelper.SetLastSecurityJournalIndex(PanelDevice, value);
			}
		}

		public MonitoringPanel(Device device)
		{
			PanelDevice = device;
			Requests = new List<Request>();
			ResetStateIds = new List<string>();
			DevicesToIgnore = new List<Device>();
			ZonesToSetGuard = new List<Zone>();
			ZonesToResetGuard = new List<Zone>();
			CommandItems = new List<CommandItem>();
			LastSystemFireIndex = LastJournalIndexHelper.GetLastFireJournalIndex(device);
			LastSystemSecurityIndex = LastJournalIndexHelper.GetLastSecurityJournalIndex(device);
			RealChildren = PanelDevice.GetRealChildren();
			DeviceStatesManager = new DeviceStatesManager();
		}

		public bool Initialize()
		{
			DeviceStatesManager.CanNotifyClients = false;
			IsInitialized = DeviceStatesManager.ReadConfigurationAndUpdateStates(PanelDevice);
			if (!IsInitialized)
			{
				PanelDevice.DeviceState.IsPanelConnectionLost = true;
				DeviceStatesManager.ForseUpdateDeviceStates(PanelDevice);
				CheckDBMissmatch();
				return false;
			}
			else
			{
				PanelDevice.DeviceState.IsPanelConnectionLost = false;
				DeviceStatesManager.ForseUpdateDeviceStates(PanelDevice);
			}
			DeviceStatesManager.UpdatePanelState(PanelDevice);
			GetInformationOperationHelper.GetDeviceInformation(PanelDevice);
			DeviceStatesManager.CanNotifyClients = true;
			SerialNo = GetSerialNo();
			return true;
		}

		public void ProcessMonitoring()
		{
			if (IsInitialized && !PanelDevice.DeviceState.IsWrongPanel && !PanelDevice.DeviceState.IsDBMissmatch)
			{
				if (IsFireReadingNeeded || IsSecurityReadingNeeded)
				{
					var journalItems = GetNewItems();
					DeviceStatesManager.UpdateDeviceStateOnJournal(journalItems);
					DeviceStatesManager.UpdatePanelState(PanelDevice);
					DeviceStatesManager.UpdatePanelParameters(PanelDevice);
				}
				DoTasks();
			}

			CheckConnectionLost();
			RequestLastIndex(0x00);
			if (PanelDevice.Driver.DriverType == DriverType.Rubezh_2OP || PanelDevice.Driver.DriverType == DriverType.USB_Rubezh_2OP)
			{
				RequestLastIndex(0x02);
			}
		}

		public void RequestLastIndex(byte journalType)
		{
			var requestType = RequestType.ReadFireIndex;
			if (journalType == 0x02)
				requestType = RequestType.ReadSecurityIndex;
			var request = new Request(requestType, new List<byte> { 0x01, 0x21, journalType });
			lock (Locker)
			{
				Requests.Add(request);
			}
			if (PanelDevice.ParentUSB.UID == PanelDevice.UID)
			{
				var response = USBManager.SendShortAttempt(PanelDevice, request.Bytes);
				if (!response.HasError)
				{
					OnResponceRecieved(request, response);
				}
			}
			else
			{
				USBManager.SendAsync(PanelDevice, request);
			}
		}

		public void OnResponceRecieved(Request request, Response response)
		{
			AnsweredCount++;
			if (request.RequestType == RequestType.ReadFireIndex || request.RequestType == RequestType.ReadSecurityIndex)
			{
				LastIndexReceived(response, request.RequestType);
			}
			lock (Locker)
			{
				Requests.RemoveAll(x => x != null && x.Id == request.Id);
			}
		}

		public void LastIndexReceived(Response response, RequestType requestType)
		{
			if (response.HasError)
			{
				return;
			}
			SequentUnAnswered = 0;
			OnConnectionAppeared();
			var lastDeviceIndex = -1;
			if (response.Id > 0)
			{
				if (response.Bytes.Count < 10)
					return;
				lastDeviceIndex = BytesHelper.ExtractInt(response.Bytes, 7);
			}
			else
			{
				if (response.Bytes.Count < 4)
					return;
				lastDeviceIndex = BytesHelper.ExtractInt(response.Bytes, 0);
			}
			if (requestType == RequestType.ReadFireIndex)
			{
				LastDeviceFireIndex = lastDeviceIndex;
				if (LastSystemFireIndex == -1)
				{
					LastSystemFireIndex = LastDeviceFireIndex;
				}
				if (LastDeviceFireIndex - LastSystemFireIndex > MaxFireMessages)
				{
					LastSystemFireIndex = LastDeviceFireIndex - MaxFireMessages;
				}
				if (LastDeviceFireIndex > LastSystemFireIndex)
				{
					IsFireReadingNeeded = true;
				}
			}
			else
			{
				LastDeviceSecurityIndex = lastDeviceIndex;
				if (LastSystemSecurityIndex == -1)
				{
					LastSystemSecurityIndex = LastDeviceSecurityIndex;
				}
				if (LastDeviceSecurityIndex - LastSystemSecurityIndex > MaxSecurityMessages)
				{
					LastSystemSecurityIndex = LastDeviceSecurityIndex - MaxSecurityMessages;
				}
				if (LastDeviceSecurityIndex > LastSystemSecurityIndex)
				{
					IsSecurityReadingNeeded = true;
				}
			}
		}

		public List<FS2JournalItem> GetNewItems()
		{
			Requests.RemoveAll(x => x != null && x.RequestType == RequestType.ReadFireIndex || x.RequestType == RequestType.ReadSecurityIndex);
			var journalItems = new List<FS2JournalItem>();
			for (int i = LastSystemFireIndex + 1; i <= LastDeviceFireIndex; i++)
			{
				var journalItem = JournalHelper.ReadItem(PanelDevice, i, 0x00);
				if (journalItem != null)
				{
					OnNewJournalItem(journalItem);
					journalItems.Add(journalItem);
				}
			}
			for (int i = LastSystemSecurityIndex + 1; i <= LastDeviceSecurityIndex; i++)
			{
				var journalItem = JournalHelper.ReadItem(PanelDevice, i, 0x02);
				if (journalItem != null)
				{
					OnNewJournalItem(journalItem);
					journalItems.Add(journalItem);
				}
			}
			LastSystemFireIndex = LastDeviceFireIndex;
			LastSystemSecurityIndex = LastDeviceSecurityIndex;
			IsFireReadingNeeded = false;
			IsSecurityReadingNeeded = false;
			return journalItems;
		}

		void OnNewJournalItem(FS2JournalItem fsJournalItem)
		{
			CallbackManager.NewJournalItems(new List<FS2JournalItem>() { fsJournalItem });
			DatabaseHelper.AddJournalItem(fsJournalItem);
		}
	}
}