﻿using System.Runtime.Serialization;

namespace FiresecAPI.Automation
{
	[DataContract]
	public class SendEmailArguments
	{
		public SendEmailArguments()
		{
			EMailAddressFromArgument = new Argument();
			EMailAddressToArgument = new Argument();
			EMailTitleArgument = new Argument();
			EMailContentArgument = new Argument();
			SmtpArgument = new Argument();
			PortArgument = new Argument();
			LoginArgument = new Argument();
			PasswordArgument = new Argument();
		}

		[DataMember]
		public Argument EMailAddressFromArgument { get; set; }

		[DataMember]
		public Argument EMailAddressToArgument { get; set; }

		[DataMember]
		public Argument EMailTitleArgument { get; set; }

		[DataMember]
		public Argument EMailContentArgument { get; set; }

		[DataMember]
		public Argument SmtpArgument { get; set; }

		[DataMember]
		public Argument PortArgument { get; set; }

		[DataMember]
		public Argument LoginArgument { get; set; }

		[DataMember]
		public Argument PasswordArgument { get; set; }
	}
}