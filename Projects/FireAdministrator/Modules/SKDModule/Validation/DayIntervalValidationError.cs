﻿using System;
using FiresecAPI.SKD;
using Infrastructure.Common.Validation;
using SKDModule.Events;
using Infrastructure.Common;

namespace SKDModule.Validation
{
	public class DayIntervalValidationError : ObjectValidationError<SKDDayInterval, ShowSKDDayIntervalsEvent, Guid>
	{
		public DayIntervalValidationError(SKDDayInterval interval, string error, ValidationErrorLevel level)
			: base(interval, error, level)
		{
		}

		public override ModuleType Module
		{
			get { return ModuleType.SKD; }
		}
		protected override Guid Key
		{
			get { return Object.UID; }
		}
		public override string Source
		{
			get { return Object.Name; }
		}
		public override string Address
		{
			get { return ""; }
		}
		public override string ImageSource
		{
			get { return "/Controls;component/Images/Shedule.png"; }
		}
	}
}