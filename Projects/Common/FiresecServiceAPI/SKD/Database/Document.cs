﻿using System;
using System.Runtime.Serialization;

namespace FiresecAPI
{
	[DataContract]
	public class Document
	{
		public Document()
		{
			UID = Guid.NewGuid();
		}

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public int No { get; set; }

		[DataMember]
		public DateTime? IssueDate { get; set; }

		[DataMember]
		public DateTime? LaunchDate { get; set; }
	}
}