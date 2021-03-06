﻿using System;
using System.Linq;
using System.Collections.Generic;
using Common;
using FiresecAPI.GK;

namespace GKProcessor
{
	public class MPTDescriptor : BaseDescriptor
	{
		GKMPT MPT { get; set; }

		public MPTDescriptor(CommonDatabase database, GKMPT mpt, DatabaseType dataBaseType)
			: base(mpt, dataBaseType)
		{
			DescriptorType = DescriptorType.MPT;
			MPT = mpt;
			MPT.Hold = 10;
			MPT.DelayRegime = DelayRegime.On;
		}

		public override void Build()
		{
			DeviceType = BytesHelper.ShortToBytes((ushort)0x106);
			SetAddress((ushort)0);
			SetFormulaBytes();
			SetPropertiesBytes();
		}

		void SetFormulaBytes()
		{
			Formula = new FormulaBuilder();
			if ((DatabaseType == DatabaseType.Gk && GKBase.IsLogicOnKau) || (DatabaseType == DatabaseType.Kau && !GKBase.IsLogicOnKau))
			{
				Formula.Add(FormulaOperationType.END);
				FormulaBytes = Formula.GetBytes();
				return;
			}

			if (MPT.StartLogic.OnClausesGroup.GetObjects().Count > 0)
			{
				Formula.AddClauseFormula(MPT.StartLogic.OnClausesGroup, DatabaseType);
				//if (MPT.SuspendLogic.OnClausesGroup.GetObjects().Count > 0)
				//{
				//    Formula.AddClauseFormula(MPT.SuspendLogic.OnClausesGroup, DatabaseType);
				//    Formula.Add(FormulaOperationType.COM);
				//    Formula.Add(FormulaOperationType.AND);
				//}
				Formula.AddPutBit(GKStateBit.TurnOn_InAutomatic, MPT, DatabaseType);
			}

			if (MPT.StopLogic.OnClausesGroup.GetObjects().Count > 0)
			{
				Formula.AddClauseFormula(MPT.StopLogic.OnClausesGroup, DatabaseType);
				Formula.AddPutBit(GKStateBit.TurnOff_InAutomatic, MPT, DatabaseType);
			}

			SetRegime(GKMPTDeviceType.HandStart, GKStateBit.TurnOn_InManual);
			SetRegime(GKMPTDeviceType.HandStop, GKStateBit.TurnOff_InManual);
			SetRegime(GKMPTDeviceType.HandAutomaticOn, GKStateBit.SetRegime_Automatic);
			SetRegime(GKMPTDeviceType.HandAutomaticOff, GKStateBit.SetRegime_Manual);

			if (MPT.SuspendLogic.OnClausesGroup.GetObjects().Count > 0)
			{
				Formula.AddClauseFormula(MPT.SuspendLogic.OnClausesGroup, DatabaseType);
				Formula.AddPutBit(GKStateBit.Stop_InManual, MPT, DatabaseType);
			}

			Formula.Add(FormulaOperationType.END);
			FormulaBytes = Formula.GetBytes();
		}

		void SetRegime(GKMPTDeviceType deviceType, GKStateBit stateBit)
		{
			var hasOR = false;
			var handStartDevices = MPT.MPTDevices.Where(x => x.MPTDeviceType == deviceType).Select(x => x.Device).ToList();
			foreach (var device in handStartDevices)
			{
				Formula.AddGetBit(GKStateBit.Fire1, device, DatabaseType);
				if (hasOR)
					Formula.Add(FormulaOperationType.OR);
				hasOR = true;
			}
			if (hasOR)
				Formula.AddPutBit(stateBit, MPT, DatabaseType);
		}

		void SetPropertiesBytes()
		{
			var binProperties = new List<BinProperty>();
			binProperties.Add(new BinProperty()
			{
				No = 0,
				Value = (ushort)MPT.Delay
			});
			binProperties.Add(new BinProperty()
			{
				No = 1,
				Value = (ushort)MPT.Hold
			});
			binProperties.Add(new BinProperty()
			{
				No = 2,
				Value = (ushort)MPT.DelayRegime
			});

			foreach (var binProperty in binProperties)
			{
				Parameters.Add(binProperty.No);
				Parameters.AddRange(BitConverter.GetBytes(binProperty.Value));
				Parameters.Add(0);
			}
		}
	}
}