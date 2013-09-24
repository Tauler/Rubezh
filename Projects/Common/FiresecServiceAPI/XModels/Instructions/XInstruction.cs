﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;

namespace XFiresecAPI
{
	[DataContract]
	public class XInstruction
	{
		public XInstruction()
		{
			UID = Guid.NewGuid();
			ZoneUIDs = new List<Guid>();
			Devices = new List<Guid>();
			Name = "";
			Text = "";
		}

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public XAlarmType AlarmType { get; set; }

		[DataMember]
		public XInstructionType InstructionType { get; set; }

		[DataMember]
		public string Text { get; set; }

		[DataMember]
		public List<Guid> ZoneUIDs { get; set; }

		[DataMember]
		public List<Guid> Devices { get; set; }

		[DataMember]
		public string AudioSource { get; set; }

		public bool HasAudio
		{
			get
			{
				return File.Exists(AudioSource);
			}
		}

		[DataMember]
		public string VideoSource { get; set; }

		public bool HasVideo
		{
			get
			{
				return File.Exists(VideoSource);
			}
		}
	}
}