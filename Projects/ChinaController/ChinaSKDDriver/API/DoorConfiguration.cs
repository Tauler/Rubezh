﻿using FiresecAPI.SKD;

namespace ChinaSKDDriverAPI
{
	public class DoorConfiguration
	{
		public DoorConfiguration()
		{
			DoorDayIntervalsCollection = new DoorDayIntervalsCollection();
		}

		public string ChannelName { get; set; }
		public AccessState AccessState { get; set; }
		public AccessMode AccessMode { get; set; }
		public int EnableMode { get; set; }
		public bool IsSnapshotEnable { get; set; }

		public bool UseDoorOpenMethod { get; set; }
		public bool UseUnlockHoldInterval { get; set; }
		public bool UseCloseTimeout { get; set; }
		public bool UseOpenAlwaysTimeIndex { get; set; }
		public bool UseHolidayTimeIndex { get; set; }
		public bool UseBreakInAlarmEnable { get; set; }
		public bool UseRepeatEnterAlarmEnable { get; set; }
		public bool UseDoorNotClosedAlarmEnable { get; set; }
		public bool UseDuressAlarmEnable { get; set; }
		public bool UseDoorTimeSection { get; set; }
		public bool UseSensorEnable { get; set; }

		public DoorOpenMethod DoorOpenMethod { get; set; }
		public int UnlockHoldInterval { get; set; }
		public int CloseTimeout { get; set; }
		public int OpenAlwaysTimeIndex { get; set; }
		public int HolidayTimeRecoNo { get; set; }
		public bool IsBreakInAlarmEnable { get; set; }
		public bool IsRepeatEnterAlarmEnable { get; set; }
		public bool IsDoorNotClosedAlarmEnable { get; set; }
		public bool IsDuressAlarmEnable { get; set; }
		public DoorDayIntervalsCollection DoorDayIntervalsCollection { get; set; }
		public bool IsSensorEnable { get; set; }
	}

	public enum AccessState
	{
		ACCESS_STATE_NORMAL,
		ACCESS_STATE_CLOSEALWAYS,
		ACCESS_STATE_OPENALWAYS,
	}

	public enum AccessMode
	{
		ACCESS_MODE_HANDPROTECTED,
		ACCESS_MODE_SAFEROOM,
		ACCESS_MODE_OTHER,
	}

	public enum DoorOpenMethod
	{
		CFG_DOOR_OPEN_METHOD_UNKNOWN = 0,
		CFG_DOOR_OPEN_METHOD_PWD_ONLY,
		CFG_DOOR_OPEN_METHOD_CARD,
		CFG_DOOR_OPEN_METHOD_PWD_OR_CARD,
		CFG_DOOR_OPEN_METHOD_CARD_FIRST,
		CFG_DOOR_OPEN_METHOD_PWD_FIRST,
		CFG_DOOR_OPEN_METHOD_SECTION,
	}
}