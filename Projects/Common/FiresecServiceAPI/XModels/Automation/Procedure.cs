﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using FiresecAPI.XModels.Automation;

namespace FiresecAPI.Models
{
	[DataContract]
	public class Procedure
	{
		public Procedure()
		{
			Name = "Новая процедура";
			InputObjects = new List<ProcedureInputObject>();
			Step = new List<ProcedureStep>();
		}

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public List<ProcedureInputObject> InputObjects { get; set; }

		[DataMember]
		public List<ProcedureStep> Step { get; set; }
	}
}