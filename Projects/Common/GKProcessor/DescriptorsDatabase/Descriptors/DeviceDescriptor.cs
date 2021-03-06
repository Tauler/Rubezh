﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecClient;

namespace GKProcessor
{
	public class DeviceDescriptor : BaseDescriptor
	{
		GKDevice Device { get; set; }

		public DeviceDescriptor(GKDevice device, DatabaseType databaseType)
			: base(device, databaseType)
		{
			DescriptorType = DescriptorType.Device;
			Device = device;
			CreateFormula();
		}

		public override void Build()
		{
			DeviceType = BytesHelper.ShortToBytes(Device.Driver.DriverTypeNo);

			var address = 0;
			if (Device.Driver.IsDeviceOnShleif)
				address = (Device.ShleifNo - 1) * 256 + Device.IntAddress;
			SetAddress((ushort)address);
			SetPropertiesBytes();

			if ((DatabaseType == DatabaseType.Gk && GKBase.IsLogicOnKau) || (DatabaseType == DatabaseType.Kau && !GKBase.IsLogicOnKau))
			{
				Formula = new FormulaBuilder();
				Formula.Add(FormulaOperationType.END);
			}
			Formula.Resolve(DatabaseType);
			FormulaBytes = Formula.GetBytes();
		}

		protected virtual void CreateFormula()
		{
			if (Device.Driver.HasLogic)
			{
				if (Device.Logic.OnClausesGroup.Clauses.Count + Device.Logic.OnClausesGroup.ClauseGroups.Count > 0)
				{
					Formula.AddClauseFormula(Device.Logic.OnClausesGroup, DatabaseType);
					if (Device.Logic.UseOffCounterLogic)
					{
						Formula.AddStandardTurning(Device, DatabaseType);
					}
					else
					{
						Formula.AddPutBit(GKStateBit.TurnOn_InAutomatic, Device, DatabaseType);
					}
				}
				if (!Device.Logic.UseOffCounterLogic && Device.Logic.OffClausesGroup.Clauses.Count + Device.Logic.OffClausesGroup.ClauseGroups.Count > 0)
				{
					Formula.AddClauseFormula(Device.Logic.OffClausesGroup, DatabaseType);
					Formula.AddPutBit(GKStateBit.TurnOff_InAutomatic, Device, DatabaseType);
				}
				if (Device.Logic.OnNowClausesGroup.Clauses.Count + Device.Logic.OnNowClausesGroup.ClauseGroups.Count > 0)
				{
					Formula.AddClauseFormula(Device.Logic.OnNowClausesGroup, DatabaseType);
					Formula.AddPutBit(GKStateBit.TurnOnNow_InAutomatic, Device, DatabaseType);
				}
				if (Device.Logic.OffNowClausesGroup.Clauses.Count + Device.Logic.OffNowClausesGroup.ClauseGroups.Count > 0)
				{
					Formula.AddClauseFormula(Device.Logic.OffNowClausesGroup, DatabaseType);
					Formula.AddPutBit(GKStateBit.TurnOffNow_InAutomatic, Device, DatabaseType);
				}
				if (Device.Logic.StopClausesGroup.Clauses.Count + Device.Logic.StopClausesGroup.ClauseGroups.Count > 0)
				{
					Formula.AddClauseFormula(Device.Logic.StopClausesGroup, DatabaseType);
					Formula.AddPutBit(GKStateBit.Stop_InManual, Device, DatabaseType);
				}
			}

			if (Device.DriverType == GKDriverType.RSR2_GuardDetector && Device.GuardZone != null)
			{
				Formula.AddGetBit(GKStateBit.On, Device.GuardZone, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOn_InAutomatic, Device, DatabaseType);
				Formula.AddGetBit(GKStateBit.Off, Device.GuardZone, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOff_InAutomatic, Device, DatabaseType);
			}
			if (Device.DriverType == GKDriverType.RSR2_CodeReader && Device.GuardZone != null)
			{
				Formula.AddGetBit(GKStateBit.On, Device.GuardZone, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOn_InAutomatic, Device, DatabaseType);
				Formula.AddGetBit(GKStateBit.Off, Device.GuardZone, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOff_InAutomatic, Device, DatabaseType);
			}
			if (Device.Door != null && Device.Door.LockDeviceUID == Device.UID)
			{
				Formula.AddGetBit(GKStateBit.On, Device.Door, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOn_InAutomatic, Device, DatabaseType);

				Formula.AddGetBit(GKStateBit.TurningOff, Device.Door, DatabaseType);
				Formula.AddGetBit(GKStateBit.Off, Device.Door, DatabaseType);
				Formula.Add(FormulaOperationType.OR);
				if (Device.Door.LockControlDevice != null)
				{
					Formula.AddGetBit(GKStateBit.On, Device.Door, DatabaseType);
					Formula.Add(FormulaOperationType.COM);
					Formula.AddGetBit(GKStateBit.Fire1, Device.Door.LockControlDevice, DatabaseType);
					Formula.Add(FormulaOperationType.AND);
					Formula.Add(FormulaOperationType.OR);
					//if (Device.Door.LockControlDevice.Properties.FirstOrDefault(x => x.Name == "Конфигурация").Value == 1)
					//{
					//	Formula.Add(FormulaOperationType.COM);
					//}
				}
				Formula.AddPutBit(GKStateBit.TurnOff_InAutomatic, Device, DatabaseType);
			}
			Formula.Add(FormulaOperationType.END);
			FormulaBytes = Formula.GetBytes();
		}

		void SetPropertiesBytes()
		{
			var binProperties = new List<BinProperty>();

			if (DatabaseType == DatabaseType.Gk && Device.Driver.IsDeviceOnShleif)
			{
				return;
			}
			foreach (var property in Device.Properties)
			{
				var driverProperty = Device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
				if (driverProperty != null && driverProperty.IsAUParameter)
				{
					if (driverProperty.CanNotEdit)
					{
						if (Device.DriverType == GKDriverType.MPT)
							property.Value = Device.IsChildMPTOrMRO() ? (ushort)(2 << 6) : (ushort)(1 << 6);
						if (Device.DriverType == GKDriverType.MRO_2)
							property.Value = Device.IsChildMPTOrMRO() ? (ushort)1 : (ushort)2;

						if (Device.DriverType == GKDriverType.RSR2_MVP)
						{
							if (driverProperty.Name == "Число АУ на АЛС3 МВП")
								property.Value = (ushort)Device.Children[0].AllChildren.Count(x => x.Driver.IsReal);
							if (driverProperty.Name == "Число АУ на АЛС4 МВП")
								property.Value = (ushort)Device.Children[1].AllChildren.Count(x => x.Driver.IsReal);
						}
					}

					byte no = driverProperty.No;
					ushort value = property.Value;
					if (driverProperty.Mask > 0)
					{
						if (driverProperty.DriverPropertyType == GKDriverPropertyTypeEnum.BoolType)
						{
							if (value > 0)
								value = (ushort)driverProperty.Mask;
						}
						else
						{
							value = (ushort)(value & driverProperty.Mask);
						}
					}
					if (driverProperty.IsHieghByte)
						value = (ushort)(value * 256);
					if (Device.DriverType == GKDriverType.RSR2_KAU && driverProperty.No == 1)
					{
						value = (ushort)(256 + value % 256);
					}

					var binProperty = binProperties.FirstOrDefault(x => x.No == no);
					if (binProperty == null)
					{
						binProperty = new BinProperty()
						{
							No = no
						};
						binProperties.Add(binProperty);
					}
					binProperty.Value += value;
				}
			}

			foreach (var binProperty in binProperties)
			{
				Parameters.Add(binProperty.No);
				Parameters.AddRange(BitConverter.GetBytes(binProperty.Value));
				Parameters.Add(0);
			}
		}
	}
}