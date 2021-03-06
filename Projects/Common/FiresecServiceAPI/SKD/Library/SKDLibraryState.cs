﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using FiresecAPI.GK;
using Infrustructure.Plans.Devices;

namespace FiresecAPI.SKD
{
	[DataContract]
	public class SKDLibraryState : ILibraryState<SKDLibraryFrame, XStateClass>
	{
		public SKDLibraryState()
		{
			Frames = new List<SKDLibraryFrame>();
			Layer = 0;
		}

		[DataMember]
		public XStateClass StateClass { get; set; }

		[DataMember]
		public List<SKDLibraryFrame> Frames { get; set; }

		[DataMember]
		public int Layer { get; set; }

		#region ILibraryState<SKDLibraryFrame,XStateClass> Members

		XStateClass ILibraryState<SKDLibraryFrame, XStateClass>.StateType
		{
			get { return StateClass; }
		}

		#endregion
	}
}