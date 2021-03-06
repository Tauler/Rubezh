﻿using System.Runtime.Serialization;
using FiresecAPI.Models;
using System.Collections.Generic;

namespace FiresecAPI.GK
{
	/// <summary>
	/// Логика включения объектов ГК
	/// </summary>
	[DataContract]
	public class GKLogic
	{
		public GKLogic()
		{
			OnClausesGroup = new GKClauseGroup();
			OffClausesGroup = new GKClauseGroup();
			OnNowClausesGroup = new GKClauseGroup();
			OffNowClausesGroup = new GKClauseGroup();
			StopClausesGroup = new GKClauseGroup();
			UseOffCounterLogic = true;
		}

		/// <summary>
		/// Группа условий для включения
		/// </summary>
		[DataMember]
		public GKClauseGroup OnClausesGroup { get; set; }

		/// <summary>
		/// Группа условий для выключения
		/// </summary>
		[DataMember]
		public GKClauseGroup OffClausesGroup { get; set; }

		/// <summary>
		/// Группа условий для включения немедленно
		/// </summary>
		[DataMember]
		public GKClauseGroup OnNowClausesGroup { get; set; }

		/// <summary>
		/// Группа условий для выключения немедленно
		/// </summary>
		[DataMember]
		public GKClauseGroup OffNowClausesGroup { get; set; }

		/// <summary>
		/// Группа условий для остановки
		/// </summary>
		[DataMember]
		public GKClauseGroup StopClausesGroup { get; set; }

		/// <summary>
		/// Использовать условие включения в качестве противоположного для условия выключения
		/// </summary>
		[DataMember]
		public bool UseOffCounterLogic { get; set; }

		public List<GKBase> GetObjects()
		{
			var result = new List<GKBase>();
			result.AddRange(OnClausesGroup.GetObjects());
			result.AddRange(OffClausesGroup.GetObjects());
			result.AddRange(OnNowClausesGroup.GetObjects());
			result.AddRange(OffNowClausesGroup.GetObjects());
			result.AddRange(StopClausesGroup.GetObjects());
			return result;
		}
	}
}