﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common;

namespace FiresecAPI.GK
{
	[DataContract]
	public abstract class XBase : ModelBase, IStateProvider
	{
		public XBase()
		{
			ClearDescriptor();
			ClearClauseDependencies();
		}

		public List<XDevice> ClauseInputDevices { get; set; }
		public List<XZone> ClauseInputZones { get; set; }
		public List<XGuardZone> ClauseInputGuardZones { get; set; }
		public List<XDirection> ClauseInputDirections { get; set; }
		public List<XMPT> ClauseInputMPTs { get; set; }
		public List<XDelay> ClauseInputDelays { get; set; }

		public void ClearClauseDependencies()
		{
			ClauseInputDevices = new List<XDevice>();
			ClauseInputZones = new List<XZone>();
			ClauseInputGuardZones = new List<XGuardZone>();
			ClauseInputDirections = new List<XDirection>();
			ClauseInputMPTs = new List<XMPT>();
			ClauseInputDelays = new List<XDelay>();
		}

		public List<XBase> InputXBases { get; set; }
		public List<XBase> OutputXBases { get; set; }

		public XDevice KauDatabaseParent { get; set; }
		public XDevice GkDatabaseParent { get; set; }

		public ushort GKDescriptorNo { get; set; }
		public ushort KAUDescriptorNo { get; set; }

		public void ClearDescriptor()
		{
			InputXBases = new List<XBase>();
			OutputXBases = new List<XBase>();
		}

		public abstract XBaseObjectType ObjectType { get; }
		public string DescriptorPresentationName
		{
			get { return ObjectType.ToDescription() + " " + PresentationName; }
		}

		public XBaseInternalState InternalState { get; set; }
		public XState State { get; set; }

		#region IStateProvider Members

		IDeviceState<XStateClass> IStateProvider.StateClass
		{
			get { return State; }
		}

		Guid IIdentity.UID
		{
			get { return UID; }
		}

		#endregion
	}
}