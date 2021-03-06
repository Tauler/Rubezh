﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FiresecAPI.GK
{
	[DataContract]
	public class GKUser
	{
		[DataMember]
		public int GKNo { get; set; }

		[DataMember]
		public int Number { get; set; }

		[DataMember]
		public string FIO { get; set; }

		[DataMember]
		public bool IsActive { get; set; }

		[DataMember]
		public GKUserType UserType { get; set; }
	}
}