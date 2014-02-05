﻿using System.Collections.Generic;
using System.Linq;
using FiresecClient;
using Infrastructure.Common.Validation;
using XFiresecAPI;

namespace SKDModule.Validation
{
    public static partial class Validator
	{
		static List<IValidationError> Errors { get; set; }

		public static IEnumerable<IValidationError> Validate()
		{
			Errors = new List<IValidationError>();
			ValidateTimeIntervals();
			return Errors;
		}
	}
}