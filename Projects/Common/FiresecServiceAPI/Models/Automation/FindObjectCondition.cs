﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FiresecAPI.Automation
{
	[DataContract]
	public class FindObjectCondition
	{
		public FindObjectCondition()
		{
			SourceArgument = new Argument();
		}

		[DataMember]
		public ConditionType ConditionType { get; set; }

		[DataMember]
		public Argument SourceArgument { get; set; }

		[DataMember]
		public Property Property { get; set; }
	}

	public enum Property
	{
		[Description("Примечание")]
		Description,

		[Description("Имя")]
		Name,

		[Description("Адрес")]
		IntAddress,

		[Description("Шлейф")]
		ShleifNo,

		[Description("Состояние")]
		State,

		[Description("Номер")]
		No,

		[Description("Тип")]
		Type,

		[Description("Идентификатор")]
		Uid,

		[Description("Задержка")]
		Delay,

		[Description("Текущая Задержка")]
		CurrentDelay,

		[Description("Удержание")]
		Hold,

		[Description("Текущее Удержание")]
		CurrentHold,

		[Description("Режим")]
		DelayRegime
	}
}