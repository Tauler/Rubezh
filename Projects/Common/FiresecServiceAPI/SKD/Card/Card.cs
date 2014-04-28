﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FiresecAPI
{
	[DataContract]
	public class SKDCard : SKDIsDeletedModel
	{
		public SKDCard()
		{
			CardZones = new List<CardZone>();
			CardType = CardType.Constant;
		}

		[DataMember]
		public int Series { get; set; }

		[DataMember]
		public int Number { get; set; }

		[DataMember]
		public Guid? HolderUID { get; set; }

		[DataMember]
		public DateTime StartDate { get; set; }

		[DataMember]
		public DateTime EndDate { get; set; }

		[DataMember]
		public List<CardZone> CardZones { get; set; }

		[DataMember]
		public Guid? CardTemplateUID { get; set; }

		[DataMember]
		public Guid? AccessTemplateUID { get; set; }

		[DataMember]
		public bool IsInStopList { get; set; }

		[DataMember]
		public CardType CardType { get; set; }

		[DataMember]
		public string StopReason { get; set; }

		[DataMember]
		public string EmployeeName { get; set; }

		public string PresentationName
		{
			get { return Series.ToString() + "/" + Number; }
		}
	}
}