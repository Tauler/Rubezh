﻿using System.Runtime.Serialization;

namespace FiresecAPI.SKD
{
	[DataContract]
	public class SKDReaderProperty
	{
		public SKDReaderProperty()
		{
			DUControl = new SKDReaderDUProperty();
			DUConversation = new SKDReaderDUProperty();
			VerificationTime = 5;
		}

		[DataMember]
		public SKDReaderDUProperty DUControl { get; set; }

		[DataMember]
		public SKDReaderDUProperty DUConversation { get; set; }

		[DataMember]
		public int VerificationTime { get; set; }
	}
}