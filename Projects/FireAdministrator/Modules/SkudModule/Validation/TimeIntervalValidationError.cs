﻿using System;
using Infrastructure.Common.Validation;
using Infrastructure.Events;
using XFiresecAPI;
using FiresecAPI;

namespace SKDModule.Validation
{
	public class TimeIntervalValidationError : ObjectValidationError<SKDTimeInterval, ShowSKDTimeIntervalsEvent, Guid>
	{
		public TimeIntervalValidationError(SKDTimeInterval device, string error, ValidationErrorLevel level)
			: base(device, error, level)
		{
		}

		public override string Module
		{
			get { return "SKD"; }
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
			get { return "/Controls;component/Images/zone.png"; }
        }
	}
}