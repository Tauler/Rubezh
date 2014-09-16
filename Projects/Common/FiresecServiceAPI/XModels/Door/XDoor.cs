﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Runtime.Serialization;

namespace FiresecAPI.GK
{
	public class XDoor : INamedBase, IIdentity
	{
		public XDevice EnterDevice { get; set; }
		public XDevice ExitDevice { get; set; }
		public XDevice LockDevice { get; set; }
		public XDevice LockControlDevice { get; set; }

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public ushort No { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public int EnterLevel { get; set; }

		[DataMember]
		public Guid EnterDeviceUID { get; set; }

		[DataMember]
		public Guid ExitDeviceUID { get; set; }

		[DataMember]
		public Guid LockDeviceUID { get; set; }

		[DataMember]
		public Guid LockControlDeviceUID { get; set; }

		public string PresentationName
		{
			get { return No + "." + Name; }
		}

		public void OnChanged()
		{
			if (Changed != null)
				Changed();
		}
		public event Action Changed;
	}
}