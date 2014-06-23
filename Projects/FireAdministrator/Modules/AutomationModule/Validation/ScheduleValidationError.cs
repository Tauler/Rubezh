﻿using System;
using AutomationModule.Events;
using FiresecAPI.Automation;
using Infrastructure.Common.Validation;

namespace AutomationModule.Validation
{
	class ScheduleValidationError : ObjectValidationError<AutomationSchedule, ShowAutomationSchedulesEvents, Guid>
	{
		public ScheduleValidationError(AutomationSchedule schedule, string error, ValidationErrorLevel level)
			: base(schedule, error, level)
		{
		}

		public override string Module
		{
			get { return "Schedule"; }
		}
		protected override Guid Key
		{
			get { return Object.Uid; }
		}
		public override string Address
		{
			get { return ""; }
		}
		public override string Source
		{
			get { return Object.Name; }
		}
		public override string ImageSource
		{
			get { return "/Controls;component/Images/SelectNone.png"; }
		}
	}
}