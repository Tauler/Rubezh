﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FiresecAPI.Models;
using FiresecAPI;

namespace XFiresecAPI
{
	[DataContract]
	public class XZoneState
	{
		public XZoneState()
		{
			StateType = StateType.Norm;
		}

		public XZone Zone { get; set; }

		[DataMember]
		public ushort No { get; set; }

		[DataMember]
		public List<XStateType> States { get; set; }

		[DataMember]
		public StateType StateType { get; set; }

		public event Action StateChanged;
		public void OnStateChanged()
		{
			if (StateChanged != null)
				StateChanged();
		}
	}
}