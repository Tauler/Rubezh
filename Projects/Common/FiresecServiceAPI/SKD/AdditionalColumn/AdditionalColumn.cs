﻿using System;
using System.Runtime.Serialization;

namespace FiresecAPI
{
	[DataContract]
	public class AdditionalColumn : SKDIsDeletedModel
	{
		[DataMember]
		public Guid? EmployeeUID { get; set; }

		[DataMember]
		public Photo Photo { get; set; }

		[DataMember]
		public AdditionalColumnType AdditionalColumnType { get; set; }

		[DataMember]
		public string TextData { get; set; }
	}
}