﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common;
using System.Xml.Serialization;
using System.Linq;
using FiresecClient;

namespace FiresecAPI.GK
{
	/// <summary>
	/// Базоый класс объектов ГК
	/// </summary>
	[DataContract]
	public abstract class GKBase : ModelBase, IStateProvider
	{
		public GKBase()
		{
			ClearDescriptor();
			ClearClauseDependencies();
			State = new GKState();
		}

		[XmlIgnore]
		public List<GKDevice> ClauseInputDevices { get; set; }
		[XmlIgnore]
		public List<GKZone> ClauseInputZones { get; set; }
		[XmlIgnore]
		public List<GKGuardZone> ClauseInputGuardZones { get; set; }
		[XmlIgnore]
		public List<GKDirection> ClauseInputDirections { get; set; }
		[XmlIgnore]
		public List<GKMPT> ClauseInputMPTs { get; set; }
		[XmlIgnore]
		public List<GKDelay> ClauseInputDelays { get; set; }

		public void ClearClauseDependencies()
		{
			ClauseInputDevices = new List<GKDevice>();
			ClauseInputZones = new List<GKZone>();
			ClauseInputGuardZones = new List<GKGuardZone>();
			ClauseInputDirections = new List<GKDirection>();
			ClauseInputMPTs = new List<GKMPT>();
			ClauseInputDelays = new List<GKDelay>();
		}

		[XmlIgnore]
		public List<GKBase> InputGKBases { get; set; }
		[XmlIgnore]
		public List<GKBase> OutputGKBases { get; set; }

		[XmlIgnore]
		public GKDevice KauDatabaseParent { get; set; }
		[XmlIgnore]
		public GKDevice GkDatabaseParent { get; set; }

		[XmlIgnore]
		public virtual bool IsLogicOnKau { get; set; }

		[XmlIgnore]
		public ushort GKDescriptorNo { get; set; }
		[XmlIgnore]
		public ushort KAUDescriptorNo { get; set; }

		public void PrepareInputOutputDependences()
		{
			var device = this as GKDevice;
			var zone = this as GKZone;
			var direction = this as GKDirection;
			var pumpStation = this as GKPumpStation;
			var mpt = this as GKMPT;
			var delay = this as GKDelay;
			var guardZone = this as GKGuardZone;
			var door = this as GKDoor;

			if (device != null)
			{
				LinkLogic(device, device.Logic.OnClausesGroup);
				LinkLogic(device, device.Logic.OffClausesGroup);
				LinkLogic(device, device.Logic.StopClausesGroup);
				if (device.IsInMPT)
				{
					var deviceMPTs = new List<GKMPT>(GKManager.MPTs.FindAll(x => x.MPTDevices.FindAll(y => y.MPTDeviceType == GKMPTDeviceType.Bomb).Any(z => z.Device == device)));
					foreach (var deviceMPT in deviceMPTs)
					{
						if (deviceMPT.SuspendLogic.OnClausesGroup.GetObjects().Count > 0)
						{
							LinkLogic(device, deviceMPT.SuspendLogic.OnClausesGroup);
						}
					}
				}
			}

			if (zone != null)
			{
				foreach (var zoneDevice in zone.Devices)
				{
					zone.LinkGKBases(zoneDevice);
				}
				zone.LinkGKBases(zone);
			}

			if (direction != null)
			{
				LinkLogic(direction, direction.Logic.OnClausesGroup);
				LinkLogic(direction, direction.Logic.OffClausesGroup);
			}

			if (pumpStation != null)
			{
				LinkLogic(pumpStation, pumpStation.StartLogic.OnClausesGroup);
				LinkLogic(pumpStation, pumpStation.StopLogic.OnClausesGroup);
				LinkLogic(pumpStation, pumpStation.AutomaticOffLogic.OnClausesGroup);
			}

			if (mpt != null)
			{
				LinkLogic(mpt, mpt.StartLogic.OnClausesGroup);
				LinkLogic(mpt, mpt.StopLogic.OnClausesGroup);
				LinkLogic(mpt, mpt.SuspendLogic.OnClausesGroup);
			}

			if (delay != null)
			{
				LinkLogic(delay, delay.Logic.OnClausesGroup);
				LinkLogic(delay, delay.Logic.OffClausesGroup);
			}

			if (guardZone != null)
			{
				foreach (var guardZoneDevice in guardZone.GuardZoneDevices)
				{
					if (guardZoneDevice.ActionType != GKGuardZoneDeviceActionType.ChangeGuard)
					{
						guardZone.LinkGKBases(guardZoneDevice.Device);
						if (guardZoneDevice.Device.DriverType == GKDriverType.RSR2_GuardDetector || guardZoneDevice.Device.DriverType == GKDriverType.RSR2_CodeReader)
						{
							guardZoneDevice.Device.LinkGKBases(guardZone);
						}
					}
				}
				guardZone.LinkGKBases(guardZone);
			}

			if (door != null)
			{
				if (door.EnterDevice != null)
					door.LinkGKBases(door.EnterDevice);
				if (door.ExitDevice != null)
					door.LinkGKBases(door.ExitDevice);
				if (door.LockDevice != null)
					door.LockDevice.LinkGKBases(door);
				if (door.LockControlDevice != null)
					door.LinkGKBases(door.LockControlDevice);
				door.LinkGKBases(door);
			}
		}

		void LinkLogic(GKBase gkBase, GKClauseGroup clauseGroup)
		{
			if (clauseGroup.Clauses != null)
			{
				foreach (var clause in clauseGroup.Clauses)
				{
					foreach (var clauseDevice in clause.Devices)
						gkBase.LinkGKBases(clauseDevice);
					foreach (var zone in clause.Zones)
						gkBase.LinkGKBases(zone);
					foreach (var guardZone in clause.GuardZones)
						gkBase.LinkGKBases(guardZone);
					foreach (var direction in clause.Directions)
						gkBase.LinkGKBases(direction);
					foreach (var mpt in clause.MPTs)
						gkBase.LinkGKBases(mpt);
					foreach (var delay in clause.Delays)
						gkBase.LinkGKBases(delay);
				}
			}
			if (clauseGroup.ClauseGroups != null)
			{
				foreach (var group in clauseGroup.ClauseGroups)
				{
					LinkLogic(gkBase, group);
				}
			}
		}

		public void LinkGKBases(GKBase dependsOn)
		{
			AddInputOutputObject(this.InputGKBases, dependsOn);
			AddInputOutputObject(dependsOn.OutputGKBases, this);
		}

		void AddInputOutputObject(List<GKBase> objects, GKBase newObject)
		{
			if(objects == null)
				objects = new List<GKBase>();
			if (objects.All(x => x.UID != newObject.UID))
				objects.Add(newObject);
		}


		public void ClearDescriptor()
		{
			InputGKBases = new List<GKBase>();
			OutputGKBases = new List<GKBase>();
		}

		[XmlIgnore]
		public abstract GKBaseObjectType ObjectType { get; }
		[XmlIgnore]
		public string DescriptorPresentationName
		{
			get { return ObjectType.ToDescription() + " " + PresentationName; }
		}

		[XmlIgnore]
		public GKBaseInternalState InternalState { get; set; }
		[XmlIgnore]
		public GKState State { get; set; }

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

		public GKDevice GetDataBaseParent()
		{
			PrepareInputOutputDependences();
			var allDependentDevices = GetFullTree(this).Cast<GKDevice>().ToList();
			var allDependentGuardZones = GetFullTree(this, true).Cast<GKGuardZone>().ToList();
			allDependentGuardZones.ForEach(x => allDependentDevices.AddRange(GetGuardZoneDependetnDevicesByCodes(x)));
			var kauParents = allDependentDevices.Select(x => x.KAUParent).ToList();
			kauParents = kauParents.Distinct().ToList();
			if (kauParents.Count == 1 && kauParents.FirstOrDefault() != null)
				return kauParents.FirstOrDefault();
			if (this is GKDevice)
				return (this as GKDevice).GKParent;
			if (allDependentDevices != null && allDependentDevices.Count > 0)
				return allDependentDevices.FirstOrDefault().GKParent;
			return null;
		}

		List<GKDevice> GetGuardZoneDependetnDevicesByCodes(GKGuardZone currentZone)
		{
			var dependentZones = GKManager.GuardZones.FindAll(x => x.GetCodeUids().Intersect(currentZone.GetCodeUids()).Any());
			var allDependentDevices = new List<GKDevice>();
			dependentZones.ForEach(x => x.PrepareInputOutputDependences());
			dependentZones.ForEach(x => allDependentDevices.AddRange(GetFullTree(x).Cast<GKDevice>().ToList()));
			return allDependentDevices.Select(x => x.KAUParent).Distinct().ToList();
		}

		List<GKBase> GetFullTree(GKBase gkBase, bool isGuardZone = false)
		{
			if (isGuardZone)
				return GetAllDependentObjects(gkBase, new List<GKBase>()).Where(x => x is GKGuardZone).ToList();
			return GetAllDependentObjects(gkBase, new List<GKBase>()).Where(x => x is GKDevice).ToList();
		}

		List<GKBase> GetAllDependentObjects(GKBase gkBase, List<GKBase> result)
		{
			var inputObjects = new List<GKBase>(gkBase.InputGKBases);
			inputObjects.RemoveAll(x => x.UID == gkBase.UID);
			foreach (var inputObject in new List<GKBase>(inputObjects))
			{
				inputObject.PrepareInputOutputDependences();
				if (result.All(x => x.UID != inputObject.UID))
					result.Add(inputObject);
				else
					continue;
				result.AddRange(GetAllDependentObjects(inputObject, result).FindAll(x => !result.Contains(x)));
			}
			if (gkBase is GKGuardZone)
			{
				var guardZoneDevice = (gkBase as GKGuardZone).GuardZoneDevices.FirstOrDefault(x => x.ActionType == GKGuardZoneDeviceActionType.ChangeGuard);
				if (guardZoneDevice != null)
				{
					if (result.All(x => x.UID != guardZoneDevice.Device.UID))
						result.Add(guardZoneDevice.Device);
				}
			}
			return result;
		}
	}
}