﻿using System;
using System.Linq;
using ChinaSKDDriverNativeApi;
using System.Runtime.InteropServices;
using ChinaSKDDriverAPI;
using System.Threading;
using System.Collections.Generic;

namespace ChinaSKDDriver
{
	public partial class Wrapper
	{
		#region Helpers
		public static string CharArrayToString(char[] charArray)
		{
			var result = new string(charArray);
			int i = result.IndexOf('\0');
			if (i >= 0)
				result = result.Substring(0, i);
			return result;
		}

		public static string CharArrayToStringNoTrim(char[] charArray)
		{
			var result = new string(charArray);
			//int i = result.IndexOf('\0');
			//if (i >= 0)
			//    result = result.Substring(0, i);
			return result;
		}

		public static char[] StringToCharArray(string str, int size)
		{
			var result = new char[size];
			var charArray = str.ToCharArray();
			for (int i = 0; i < Math.Min(charArray.Count(), size); i++)
			{
				result[i] = charArray[i];
			}
			return result;
		}

		public static DateTime NET_TIMEToDateTime(NativeWrapper.NET_TIME netTime)
		{
			DateTime dateTime = DateTime.MinValue;
			try
			{
				if (netTime.dwYear == 0 || netTime.dwMonth == 0 || netTime.dwDay == 0)
					return new DateTime();
				dateTime = new DateTime(netTime.dwYear, netTime.dwMonth, netTime.dwDay, netTime.dwHour, netTime.dwMinute, netTime.dwSecond);
			}
			catch { }
			return dateTime;
		}
		#endregion

		#region CommonDevice
		public DeviceSoftwareInfo GetDeviceSoftwareInfo()
		{
			NativeWrapper.WRAP_DevConfig_TypeAndSoftInfo_Result outResult;
			var result = NativeWrapper.WRAP_DevConfig_TypeAndSoftInfo(LoginID, out outResult);

			DeviceSoftwareInfo deviceSoftwareInfo = null;
			if (result)
			{
				deviceSoftwareInfo = new DeviceSoftwareInfo();
				deviceSoftwareInfo.SoftwareBuildDate = new DateTime(outResult.dwSoftwareBuildDate_1, outResult.dwSoftwareBuildDate_2, outResult.dwSoftwareBuildDate_3);
				deviceSoftwareInfo.DeviceType = Wrapper.CharArrayToString(outResult.szDevType);
				deviceSoftwareInfo.SoftwareVersion = Wrapper.CharArrayToString(outResult.szSoftWareVersion);
			}
			return deviceSoftwareInfo;
		}

		public DeviceNetInfo GetDeviceNetInfo()
		{
			NativeWrapper.WRAP_CFG_NETWORK_INFO_Result outResult;
			var result = NativeWrapper.WRAP_Get_DevConfig_IPMaskGate(LoginID, out outResult);

			DeviceNetInfo deviceNetInfo = null;
			if (result)
			{
				deviceNetInfo = new DeviceNetInfo();
				deviceNetInfo.IP = Wrapper.CharArrayToString(outResult.szIP);
				deviceNetInfo.SubnetMask = Wrapper.CharArrayToString(outResult.szSubnetMask);
				deviceNetInfo.DefaultGateway = Wrapper.CharArrayToString(outResult.szDefGateway);
				deviceNetInfo.MTU = outResult.nMTU;
			}
			return deviceNetInfo;
		}

		public bool SetDeviceNetInfo(DeviceNetInfo deviceNetInfo)
		{
			var result = NativeWrapper.WRAP_Set_DevConfig_IPMaskGate(LoginID, deviceNetInfo.IP, deviceNetInfo.SubnetMask, deviceNetInfo.DefaultGateway, deviceNetInfo.MTU);
			return result;
		}

		public string GetDeviceMacAddress()
		{
			NativeWrapper.WRAP_DevConfig_MAC_Result outResult;
			var result = NativeWrapper.WRAP_DevConfig_MAC(LoginID, out outResult);
			if (result)
			{
				var macAddress = Wrapper.CharArrayToString(outResult.szMAC);
				return macAddress;
			}
			return null;
		}

		public int GetMaxPageSize()
		{
			NativeWrapper.WRAP_DevConfig_RecordFinderCaps_Result outResult;
			var result = NativeWrapper.WRAP_DevConfig_RecordFinderCaps(LoginID, out outResult);
			if (result)
			{
				var maxPageSize = outResult.nMaxPageSize;
				return maxPageSize;
			}
			return -1;
		}

		public DateTime GetDateTime()
		{
			NativeWrapper.NET_TIME outResult;
			var result = NativeWrapper.WRAP_DevConfig_GetCurrentTime(LoginID, out outResult);
			if (result)
			{
				try
				{
					var dateTime = new DateTime(outResult.dwYear, outResult.dwMonth, outResult.dwDay, outResult.dwHour, outResult.dwMinute, outResult.dwSecond);
					return dateTime;
				}
				catch { }
			}
			return DateTime.MinValue;
		}

		public bool SetDateTime(DateTime dateTime)
		{
			var result = NativeWrapper.WRAP_DevConfig_SetCurrentTime(LoginID, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
			return result;
		}

		public bool DeleteAll()
		{
			var result = NativeWrapper.WRAP_DevCtrl_DeleteCfgFile(LoginID);
			return result;
		}

		public bool Reboot()
		{
			var result = NativeWrapper.WRAP_DevCtrl_ReBoot(LoginID);
			return result;
		}

		public string GetProjectPassword()
		{
			NativeWrapper.WRAP_GeneralConfig_Password outResult;
			var result = NativeWrapper.WRAP_GetProjectPassword(LoginID, out outResult);
			if (result)
			{
				var projectPassword = Wrapper.CharArrayToString(outResult.szProjectPassword);
				return projectPassword;
			}
			else
			{
				return null;
			}
		}

		public bool SetProjectPassword(string projectPassword)
		{
			var result = NativeWrapper.WRAP_SetProjectPassword(LoginID, projectPassword);
			return result;
		}

		public int GetLogsCount()
		{
			NativeWrapper.QUERY_DEVICE_LOG_PARAM logParam = new NativeWrapper.QUERY_DEVICE_LOG_PARAM();
			var result = NativeWrapper.WRAP_DevCtrl_GetLogCount(LoginID, ref logParam);
			return result;
		}

		public DoorConfiguration GetDoorConfiguration()
		{
			NativeWrapper.CFG_ACCESS_EVENT_INFO outResult;
			var result = NativeWrapper.WRAP_GetDoorConfiguration(LoginID, 0, out outResult);
			if (result)
			{
				var doorConfiguration = new DoorConfiguration();
				doorConfiguration.IsBreakInAlarmEnable = outResult.abBreakInAlarmEnable;
				doorConfiguration.ChannelName = Wrapper.CharArrayToString(outResult.szChannelName);
				doorConfiguration.AceessState = (AceessState)outResult.emState;
				doorConfiguration.AceessMode = (AceessMode)outResult.emMode;
				doorConfiguration.EnableMode = outResult.nEnableMode;
				doorConfiguration.IsSnapshotEnable = outResult.bSnapshotEnable;
				doorConfiguration.IsDoorOpenMethod = outResult.abDoorOpenMethod;
				doorConfiguration.IsUnlockHoldInterval = outResult.abUnlockHoldInterval;
				doorConfiguration.IsCloseTimeout = outResult.abCloseTimeout;
				doorConfiguration.IsOpenAlwaysTimeIndex = outResult.abOpenAlwaysTimeIndex;
				doorConfiguration.IsHolidayTimeIndex = outResult.abHolidayTimeIndex;
				doorConfiguration.IsBreakInAlarmEnable = outResult.abBreakInAlarmEnable;
				doorConfiguration.IsRepeatEnterAlarmEnable = outResult.abRepeatEnterAlarmEnable;
				doorConfiguration.IsDoorNotClosedAlarmEnable = outResult.abDoorNotClosedAlarmEnable;
				doorConfiguration.IsDuressAlarmEnable = outResult.abDuressAlarmEnable;
				doorConfiguration.IsDoorTimeSection = outResult.abDoorTimeSection;
				doorConfiguration.IsSensorEnable = outResult.abSensorEnable;
				doorConfiguration.DoorOpenMethod = (DoorOpenMethod)outResult.emDoorOpenMethod;
				doorConfiguration.UnlockHoldInterval = outResult.nUnlockHoldInterval;
				doorConfiguration.CloseTimeout = outResult.nCloseTimeout;
				doorConfiguration.OpenAlwaysTimeIndex = outResult.nOpenAlwaysTimeIndex;
				doorConfiguration.HolidayTimeRecoNo = outResult.nHolidayTimeRecoNo;
				doorConfiguration.IsBreakInAlarmEnable2 = outResult.bBreakInAlarmEnable;
				doorConfiguration.IsRepeatEnterAlarmEnable2 = outResult.bRepeatEnterAlarm;
				doorConfiguration.IsDoorNotClosedAlarmEnable2 = outResult.bDoorNotClosedAlarmEnable;
				doorConfiguration.IsDuressAlarmEnable2 = outResult.bDuressAlarmEnable;
				doorConfiguration.IsSensorEnable2 = outResult.bSensorEnable;


				var timeSheduleIntervals = new List<TimeSheduleInterval>();
				for (int i = 0; i < outResult.stuDoorTimeSection.Count(); i++)
				{
					var cfg_DOOROPEN_TIMESECTION_INFO = outResult.stuDoorTimeSection[i];
					var timeSheduleInterval = new TimeSheduleInterval();
					timeSheduleInterval.BeginHours = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuStartTime.dwHour;
					timeSheduleInterval.BeginMinutes = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuStartTime.dwMinute;
					timeSheduleInterval.BeginSeconds = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuStartTime.dwSecond;
					timeSheduleInterval.EndHours = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuEndTime.dwHour;
					timeSheduleInterval.EndMinutes = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuEndTime.dwMinute;
					timeSheduleInterval.EndSeconds = cfg_DOOROPEN_TIMESECTION_INFO.stuTime.stuEndTime.dwSecond;
					timeSheduleIntervals.Add(timeSheduleInterval);
				}

				var timeShedules = new List<TimeShedule>();
				for (int i = 0; i < 7; i++)
				{
					var timeShedule = new TimeShedule();
					for (int j = 0; j < 4; j++)
					{
						var timeSheduleInterval = timeSheduleIntervals[i * 4 + j];
						timeShedule.TimeSheduleIntervals.Add(timeSheduleInterval);
					}
					timeShedules.Add(timeShedule);
				}
				doorConfiguration.TimeShedules = timeShedules;
				return doorConfiguration;
			}
			return null;
		}

		public bool SetDoorConfiguration(DoorConfiguration doorConfiguration)
		{
			NativeWrapper.CFG_ACCESS_EVENT_INFO info = new NativeWrapper.CFG_ACCESS_EVENT_INFO();

			info.abBreakInAlarmEnable = doorConfiguration.IsBreakInAlarmEnable;
			info.szChannelName = StringToCharArray(doorConfiguration.ChannelName, 128);
			info.emState = (NativeWrapper.CFG_ACCESS_STATE)doorConfiguration.AceessState;
			info.emMode = (NativeWrapper.CFG_ACCESS_MODE)doorConfiguration.AceessMode;
			info.nEnableMode = doorConfiguration.EnableMode;
			info.bSnapshotEnable = doorConfiguration.IsSnapshotEnable;
			info.abDoorOpenMethod = doorConfiguration.IsDoorOpenMethod;
			info.abUnlockHoldInterval = doorConfiguration.IsUnlockHoldInterval;
			info.abCloseTimeout = doorConfiguration.IsCloseTimeout;
			info.abOpenAlwaysTimeIndex = doorConfiguration.IsOpenAlwaysTimeIndex;
			info.abHolidayTimeIndex = doorConfiguration.IsHolidayTimeIndex;
			info.abBreakInAlarmEnable = doorConfiguration.IsBreakInAlarmEnable;
			info.abRepeatEnterAlarmEnable = doorConfiguration.IsRepeatEnterAlarmEnable;
			info.abDoorNotClosedAlarmEnable = doorConfiguration.IsDoorNotClosedAlarmEnable;
			info.abDuressAlarmEnable = doorConfiguration.IsDuressAlarmEnable;
			info.abDoorTimeSection = doorConfiguration.IsDoorTimeSection;
			info.abSensorEnable = doorConfiguration.IsSensorEnable;
			info.emDoorOpenMethod = (NativeWrapper.CFG_DOOR_OPEN_METHOD)doorConfiguration.DoorOpenMethod;
			info.nUnlockHoldInterval = doorConfiguration.UnlockHoldInterval;
			info.nCloseTimeout = doorConfiguration.CloseTimeout;
			info.nOpenAlwaysTimeIndex = doorConfiguration.OpenAlwaysTimeIndex;
			info.nHolidayTimeRecoNo = doorConfiguration.HolidayTimeRecoNo;
			info.bBreakInAlarmEnable = doorConfiguration.IsBreakInAlarmEnable2;
			info.bRepeatEnterAlarm = doorConfiguration.IsRepeatEnterAlarmEnable2;
			info.bDoorNotClosedAlarmEnable = doorConfiguration.IsDoorNotClosedAlarmEnable2;
			info.bDuressAlarmEnable = doorConfiguration.IsDuressAlarmEnable2;
			info.bSensorEnable = doorConfiguration.IsSensorEnable2;

			info.stuDoorTimeSection = new NativeWrapper.CFG_DOOROPEN_TIMESECTION_INFO[7 * 4];
			for (int i = 0; i < info.stuDoorTimeSection.Count(); i++)
			{
				info.stuDoorTimeSection[i].stuTime.stuStartTime.dwHour = 1;
				info.stuDoorTimeSection[i].stuTime.stuStartTime.dwMinute = i;
				info.stuDoorTimeSection[i].stuTime.stuStartTime.dwSecond = i;

				info.stuDoorTimeSection[i].stuTime.stuEndTime.dwHour = 2;
				info.stuDoorTimeSection[i].stuTime.stuEndTime.dwMinute = i * 2;
				info.stuDoorTimeSection[i].stuTime.stuEndTime.dwSecond = i * 2;
			}
			var result = NativeWrapper.WRAP_SetDoorConfiguration(LoginID, 0, ref info);
			return result;
		}

		#endregion

		#region Common

		public int LoginID { get; private set; }

		void OnfDisConnect(Int32 lLoginID, string pchDVRIP, Int32 nDVRPort, UInt32 dwUser)
		{
			if (dwUser == 0)
			{
				return;
			}
		}

		void OnfHaveReConnect(Int32 lLoginID, string pchDVRIP, Int32 nDVRPort, UInt32 dwUser)
		{
			return;
		}

		public int Connect(string ipAddress, int port, string login, string password)
		{
			NativeWrapper.fDisConnectDelegate dCbFunc = new NativeWrapper.fDisConnectDelegate(OnfDisConnect);
			var result = NativeWrapper.CLIENT_Init(dCbFunc, (UInt32)0);

			NativeWrapper.NET_DEVICEINFO deviceInfo;
			int error = 0;
			LoginID = NativeWrapper.CLIENT_Login(ipAddress, (UInt16)37777, login, password, out deviceInfo, out error);

			NativeWrapper.fHaveReConnect onHaveReConnect = new NativeWrapper.fHaveReConnect(OnfHaveReConnect);
			NativeWrapper.CLIENT_SetAutoReconnect(onHaveReConnect, 0);

			return LoginID;
		}

		public void StartListen()
		{
			NativeWrapper.fMessCallBack onfMessCallBack = new NativeWrapper.fMessCallBack(OnfMessCallBack);
			NativeWrapper.CLIENT_SetDVRMessCallBack(onfMessCallBack, 0);

			NativeWrapper.CLIENT_StartListenEx(LoginID);
		}

		bool OnfMessCallBack(Int32 lCommand, Int32 lLoginID, IntPtr pBuf, Int32 dwBufLen, string pchDVRIP, Int32 nDVRPort, UInt32 dwUser)
		{
			if (lCommand == 0x3181)
			{
				var nativeJournalItem = (NativeWrapper.ALARM_ACCESS_CTL_EVENT_INFO)Marshal.PtrToStructure(pBuf, typeof(NativeWrapper.ALARM_ACCESS_CTL_EVENT_INFO));

				var doorNo = nativeJournalItem.nDoor;
				var status = nativeJournalItem.bStatus;
				var eventType = nativeJournalItem.emEventType;
				var cardType = nativeJournalItem.emCardType;
				var openMethod = nativeJournalItem.emOpenMethod;
				var doorName = Wrapper.CharArrayToString(nativeJournalItem.szDoorName);
				var cardNo = Wrapper.CharArrayToString(nativeJournalItem.szCardNo);
				var password = Wrapper.CharArrayToString(nativeJournalItem.szPwd);
				var dateTime = Wrapper.NET_TIMEToDateTime(nativeJournalItem.stuTime);

				var journalItem = new SKDJournalItem();
				journalItem.SystemDateTime = DateTime.Now;
				journalItem.DeviceDateTime = dateTime;
				journalItem.Name = "Проход через считыватель";

				var description = "";
				switch (eventType)
				{
					case NativeWrapper.NET_ACCESS_CTL_EVENT_TYPE.NET_ACCESS_CTL_EVENT_ENTRY:
						description = "Вход через дверь";
						break;

					case NativeWrapper.NET_ACCESS_CTL_EVENT_TYPE.NET_ACCESS_CTL_EVENT_EXIT:
						description = "Выход через дверь";
						break;

					case NativeWrapper.NET_ACCESS_CTL_EVENT_TYPE.NET_ACCESS_CTL_EVENT_UNKNOWN:
						description = "Неизвестно";
						break;
				}
				journalItem.Description = description;

				if (NewJournalItem != null)
					NewJournalItem(journalItem);
			}

			return true;
		}


		public event Action<SKDJournalItem> NewJournalItem;

		public bool Disconnect()
		{
			var result = NativeWrapper.CLIENT_Cleanup();
			return result;
		}
		#endregion
	}
}